using System.Net;
using AIWAB.Common.Configuration.General;
using ChatBotAPI.MessagingPlatforms.Twilio;
using ChatBotAPI.Services;
using AIProviderAPI.Protos;
using Microsoft.Extensions.DependencyInjection;
using Twilio;
using AIWAB.Common.Configuration.ExternalMsgPlatform;
using ChatBotAPI.MessagingPlatforms;
using AIWAB.Common.Core.AIProviderAPI.GrpcClients;
using QuestionAnswerAPI.Protos;
using AIWAB.Common.Core.QuestionAnswerAPI.GrpcClients;
using AIWAB.Common.General.MessageQueue;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<ExternalMsgPlatformSettings>(builder.Configuration.GetSection("ExternalMessagingSettings"));

var appSettings= builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
var aiProviderEndpoint = appSettings?.Endpoints.FirstOrDefault(kvp => kvp.Key == "AIProviderAPI").Value;
var qnaEndpoint = appSettings?.Endpoints.FirstOrDefault(kvp => kvp.Key == "QnAAPI").Value;

var externalMsgSettings = builder.Configuration.GetSection("ExternalMessagingSettings").Get<ExternalMsgPlatformSettings>();
var twilioSettings = externalMsgSettings?.MessagingPlatforms["Twilio"];
if (twilioSettings != null)
{
    TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);
}

builder.Services.AddTransient<TwilioService>();
builder.Services.AddSingleton<MessagingPlatformFactory>();
builder.Services.AddScoped<IChatBotService, ChatBotService>();
builder.Services.AddScoped<IMessageResponseHandler, MessageResponseHandler>();
builder.Services.AddSingleton<IMessageQueue, InMemoryMessageQueue>();
builder.Services.AddHostedService<MessageProcessorService>();
builder.Services.AddControllers();


builder.Services.AddGrpcClient<AIProviderService.AIProviderServiceClient>(options =>
{
    options.Address = new Uri($"{aiProviderEndpoint?.Url}");
});
builder.Services.AddTransient<IAIProviderClientService, AIProviderClientService>();

builder.Services.AddGrpcClient<QnAService.QnAServiceClient>(options =>
{
    options.Address = new Uri($"{qnaEndpoint?.Url}");
});
builder.Services.AddTransient<IQnAClientService, QnAClientService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

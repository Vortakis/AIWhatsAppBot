using AIWAB.Common.Configuration.General;
using AIWAB.Common.Configuration.MessageService;
using ChatBotAPI.Clients;
using ChatBotAPI.MessagingPlatforms.Twilio;
using ChatBotAPI.MessagingServices;
using ChatBotAPI.Services;
using ExtAIProviderAPI.Protos;
using Microsoft.Extensions.DependencyInjection;
using Twilio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<ExternalMsgPlatformSettings>(builder.Configuration.GetSection("ExternalMessagingSettings"));

var appSettings= builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
var aiProviderEndpoint = appSettings?.Endpoints.FirstOrDefault(kvp => kvp.Key == "AIProviderAPI").Value;

var externalMsgSettings = builder.Configuration.GetSection("ExternalMessagingSettings").Get<ExternalMsgPlatformSettings>();
var twilioSettings = externalMsgSettings?.MessagingPlatforms["Twilio"];
if (twilioSettings != null)
{
    TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);
}

builder.Services.AddTransient<TwilioService>();
builder.Services.AddTransient<MessagingPlatformFactory>();
builder.Services.AddScoped<IChatBotService, ChatBotService>();
builder.Services.AddScoped<IMessageResponseHandler, MessageResponseHandler>();

builder.Services.AddControllers();

if (aiProviderEndpoint == null)
{
    throw new InvalidOperationException("The AIProviderAPI endpoint configuration is missing.");
}

builder.Services.AddGrpcClient<AIProviderService.AIProviderServiceClient>(options =>
{
    options.Address = new Uri(aiProviderEndpoint.Url);
});

builder.Services.AddTransient<IAIProviderClientService, AIProviderClientService>();

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

using AIWAB.Common.Configuration.MessageService;
using ChatBotAPI.Clients;
using ChatBotAPI.MessageServices;
using ChatBotAPI.MessagingServices;
using ChatBotAPI.Services;
using ExtAIProviderAPI.Protos;
using Twilio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ExternalMsgPlatformSettings>(builder.Configuration.GetSection("ExternalMessagingSettings"));


var externalMsgSettings = builder.Configuration.GetSection("ExternalMessagingSettings").Get<ExternalMsgPlatformSettings>();
var twilioSettings = externalMsgSettings?.MessagingPlatforms["Twilio"];
if (twilioSettings != null)
{
    TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);
}

builder.Services.AddTransient<WhatsAppService>();
builder.Services.AddTransient<MessagingPlatformFactory>();
builder.Services.AddScoped<IChatBotService, ChatBotService>();

builder.Services.AddControllers();

builder.Services.AddGrpcClient<AIProviderService.AIProviderServiceClient>(options =>
{
    options.Address = new Uri("https://your-aiapi-service-url");
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

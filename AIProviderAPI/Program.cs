using AIWAB.Common.Configuration.ExternalAI;
using AIProviderAPI.AIProviders;
using AIProviderAPI.Services;
using OpenAI;
using AIWAB.Common.Configuration.ExternalMsgPlatform;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

// Add services to the container.
builder.Services.Configure<ExternalAISettings>(builder.Configuration.GetSection("ExternalAISettings"));
builder.Services.Configure<ExternalMsgPlatformSettings>(builder.Configuration.GetSection("ExternalMessagingSettings"));

builder.Services.AddSingleton(sp =>
{
    var externalAISettings = builder.Configuration.GetSection("ExternalAISettings").Get<ExternalAISettings>();
    var openAISettings = externalAISettings?.AIProviders["OpenAI"]; 

    return new OpenAIClient(openAISettings?.ApiKey);
});
builder.Services.AddTransient<OpenAIProvider>();
builder.Services.AddSingleton<AIProviderFactory>();
builder.Services.AddScoped<IAIProviderService, AIProviderService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
  
    if (env.IsDevelopment())
    {
        options.ListenAnyIP(7103, listenOptions =>
        {
            listenOptions.UseHttps();
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        });
    }
    else
    {
        options.ListenAnyIP(8080, listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        });
    }
   
});


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
app.MapGrpcService<AIProviderServiceGrpc>();
app.MapPost("/", () => "This is the AIAPI gRPC server.");
app.Run();

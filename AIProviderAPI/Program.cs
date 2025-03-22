using AIWAB.Common.Configuration.ExternalAI;
using AIProviderAPI.AIProviders;
using AIProviderAPI.Services;
using OpenAI;
using System.Reflection.PortableExecutable;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ExternalAISettings>(builder.Configuration.GetSection("ExternalAISettings"));
builder.Services.AddSingleton(sp =>
{
    var externalAISettings = builder.Configuration.GetSection("ExternalAISettings").Get<ExternalAISettings>();
    var openAISettings = externalAISettings?.AIProviders["OpenAI"]; 

    return new OpenAIClient(openAISettings?.ApiKey);
});
builder.Services.AddTransient<OpenAIProvider>();
builder.Services.AddSingleton<AIProviderFactory>();
builder.Services.AddScoped<IAIProviderService, AIProviderService>();

builder.Services.AddControllers();

builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.UseHttps();  
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;  
    });
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
app.MapGrpcService<AIPoviderServiceGrpc>();
app.MapPost("/", () => "This is the AIAPI gRPC server.");
app.Run();

using AIProviderAPI.Protos;
using AIWAB.Common.Configuration.ExternalAI;
using AIWAB.Common.Configuration.General;
using AIWAB.Common.Core.AIProviderAPI.GrpcClients;
using QuestionAnswerAPI.Repository;
using QuestionAnswerAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

// Add services to the container.

#region GRPC AIProvider Client
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
builder.Services.Configure<ExternalAISettings>(builder.Configuration.GetSection("ExternalAISettings"));
var aiProviderEndpoint = appSettings?.Endpoints.FirstOrDefault(kvp => kvp.Key == "AIProviderAPI").Value;
if (aiProviderEndpoint == null)
{
    throw new InvalidOperationException("The AIProviderAPI endpoint configuration is missing.");
}

builder.Services.AddGrpcClient<AIProviderService.AIProviderServiceClient>(options =>
{
    options.Address = new Uri($"{aiProviderEndpoint.Url}");
});

builder.Services.AddTransient<IAIProviderClientService, AIProviderClientService>();
#endregion

#region GRPC Server
builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel(options =>
{
    if (env.IsDevelopment())
    {
        options.ListenAnyIP(7138, listenOptions =>
        {
            listenOptions.UseHttps();
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        });
    } else
    {
        options.ListenAnyIP(8080, listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        });
    }
});
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register services.
builder.Services.AddSingleton<IQnARepository, QnARepository>();
builder.Services.AddScoped<IQnAService, QnAService>();
builder.Services.AddHostedService<QnAService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<QnAServiceGrpc>();
app.MapPost("/", () => "This is the AIAPI gRPC server.");
app.Run();

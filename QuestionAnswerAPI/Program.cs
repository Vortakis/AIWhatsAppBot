using AIProviderAPI.Protos;
using AIWAB.Common.Configuration.General;
using AIWAB.Common.Core.AIProviderAPI.GrpcClients;
using QuestionAnswerAPI.Repository;
using QuestionAnswerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
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



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register services.
builder.Services.AddScoped<IQnARepository, QnARepository>();
builder.Services.AddScoped<IQnAService, QnAService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

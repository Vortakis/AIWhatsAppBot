using AIWAB.Common.Configuration.ExternalAI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ExternalAISettings>(builder.Configuration.GetSection("ExternalAISettings"));
builder.Services.AddSingleton(sp =>
{
    var externalAISettings = builder.Configuration.GetSection("ExternalAISettings").Get<ExternalAISettings>();
    var openAISettings = externalAISettings?.AIProviders["OpenAI"]; 

    return new OpenAIClient(openAISettings?.ApiKey);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

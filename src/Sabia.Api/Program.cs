using Microsoft.Extensions.AI;
using OllamaSharp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddSingleton<IChatClient>(
    new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2:3b"));

var app = builder.Build();
app.UseCors();

app.MapPost("/chat", async (ChatRequest req, IChatClient chat) =>
{
    var response = await chat.GetResponseAsync(req.Message);
    return Results.Ok(new { reply = response.Text });
});

app.Run();

record ChatRequest(string Message);

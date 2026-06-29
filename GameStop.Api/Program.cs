using GameStop.Api.Data;
using GameStop.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddGameStopDb();
var app = builder.Build();


app.MapGameEndpoints();
app.MapGenresEndpoints(); 

app.MigrateDb();

app.Run();

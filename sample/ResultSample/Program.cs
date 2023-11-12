using ResultSample.Endpoints;
using ResultSample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<WeatherForcastService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<BookService>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.MapControllers();

app.MapBookEndpoints();

app.Run();

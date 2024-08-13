using System.Net;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Pokedex_Api;
using Pokedex_Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.AddResultConvention(c => c.AddDefaultMap().For(
        ResultStatus.Invalid,
        HttpStatusCode.BadRequest,
        resultStatusOptions => resultStatusOptions.With((_, result) => result.ValidationErrors)));
});
builder.Services.AddCorsPolicy();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCorsPolicy();
app.UseAuthorization();

using var scope = app.Services.CreateScope();
var dataContext = scope.ServiceProvider.GetRequiredService<PokedexDbContext>();
dataContext.Database.EnsureCreated();
await DbInitializer.Initialize(dataContext);

app.MapControllers();

app.Run();

using Scalar.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Parcil1_P4Luis.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<NumberService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var numberService = scope.ServiceProvider.GetRequiredService<NumberService>();
    await numberService.InitializeAsync();
}

    app.MapOpenApi();
    app.MapScalarApiReference();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

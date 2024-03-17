using Microsoft.AspNetCore.Mvc;
using Refit;
using RefitLearn;
using RefitLearn.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddRefitClient<IUsersClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));

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

var group = app.MapGroup("refit");

group.MapGet("getUsers", async (IUsersClient client) =>
{
    var users = await client.GetUsers();

    StringBuilder str = new();

    foreach (User user in users)
    {
        str.Append($"{user}, ");
    }

    return str.ToString();
});

group.MapGet("getUserById", async (IUsersClient client, [FromQuery] int id) =>
{
    var user = await client.GetUser(1);

    return user;
});

app.MapPost("createUser", async (IUsersClient client) =>
{
    var newUser = new User
    {
        Email = "alo123@gmail124.com",
        Name = "Alok Dab"
    };

    var result = await client.CreateUser(newUser);

    return result;
});

app.Run();

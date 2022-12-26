using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using KnowYourPostUsers.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserContext>(ops => 
    ops.UseNpgsql(builder.Configuration.GetSection("DbConnection").Value, b => b.MigrationsAssembly("KnowYourPostUsers")));

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<UserContext>();
    context.Database.Migrate();
}

app.MapPost("/user", (UserVM user, UserContext context) =>
{
    context.Users.Add(new(user.Email, user.Password));
    context.SaveChanges();
    return Results.Ok();
});

app.MapPost("/user/exists", (UserVM user, UserContext context) =>
{
    var exists = context.Users.Any(u => u.Email == user.Email && u.Password == user.Password);
    return Results.Ok(exists);
});

app.Run();


class UserVM
{
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("password")] public string Password { get; set; }
}
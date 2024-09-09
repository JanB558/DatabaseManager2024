using Microsoft.EntityFrameworkCore;
using ASPWebAPI.Context;
using ASPWebAPI.Services;
using ASPWebAPI.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CourseDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISQLServerService, SQLServerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// people
// GET all people
app.MapGet("/person", async (ISQLServerService sqlService) =>
    Results.Ok(await sqlService.GetPeopleAsync()));
// GET person by id
app.MapGet("/person/{id}", async (int id, ISQLServerService sqlService) =>
    Results.Ok(await sqlService.GetPersonAsync(id) is Person person
    ? Results.Ok(person) : Results.NotFound()));
// ADD person
app.MapPost("/persons", async (Person person, ISQLServerService sqlService) =>
{
    var createdPerson = await sqlService.AddPersonAsync(person);
    return Results.Created($"/persons/{createdPerson.ID}", createdPerson);
});

// courses

app.Run();
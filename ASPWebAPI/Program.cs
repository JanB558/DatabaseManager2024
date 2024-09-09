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
// CONNECTION STRING SHOULD NOT BE ON GITHUB ITS JUST AN EXAMPLE

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
app.MapPost("/person", async (Person person, ISQLServerService sqlService) =>
{
    var createdPerson = await sqlService.AddPersonAsync(person);
    return Results.Created($"/persons/{createdPerson.ID}", createdPerson);
});
// UPDATE person
app.MapPut("/person", async (Person person, ISQLServerService sqlService) =>
    await sqlService.UpdatePersonAsync(person)
    ? Results.Ok(person) : Results.NotFound());
// DELETE person
app.MapDelete("/person/{id}", async (int id, ISQLServerService sqlService) =>
    await sqlService.DeletePersonAsync(id)
        ? Results.Ok() : Results.NotFound());

// courses

app.Run();
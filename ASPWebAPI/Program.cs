using Microsoft.EntityFrameworkCore;
using ASPWebAPI.Context;
using ASPWebAPI.Services;
using ASPWebAPI.Model;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CourseDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// WARNING - REAL CONNECTION STRING SHOULD NOT BE PUSHED TO GITHUB

builder.Services.AddScoped<ISQLServerService, SQLServerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region person
// GET all people
app.MapGet("/person", async (ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.GetPeopleAsync();
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});

// GET person by id
app.MapGet("/person/{id}", async (int id, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.GetPersonAsync(id);
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});

// ADD person
app.MapPost("/person", async (Person person, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.AddPersonAsync(person);
        return Results.Created();
    }
    catch (Exception ex) 
    { 
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem(); 
    } 
});

// UPDATE person
app.MapPut("/person", async (Person person, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.UpdatePersonAsync(person);
        if (result) return Results.Ok();
        else return Results.BadRequest();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});

// DELETE person
app.MapDelete("/person/{id}", async (int id, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.DeletePersonAsync(id);
        if (result) return Results.Ok();
        else return Results.NotFound();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});
#endregion

#region course
//GET all courses
app.MapGet("/course", async (ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.GetCoursesAsync();
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});

//GET course by id
app.MapGet("/course/{id}", async (int id, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.GetCourseAsync(id);
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});

//ADD course
app.MapPost("/course", async (Course course, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.AddCourseAsync(course);
        return Results.Created();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});

//UPDATE course
app.MapPut("/course", async (Course course, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.UpdateCourseAsync(course);
        if (result) return Results.Ok();
        else return Results.BadRequest();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});

//DELETE course
app.MapDelete("/course/{id}", async (int id, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.DeleteCourseAsync(id);
        if (result) return Results.Ok();
        else return Results.NotFound();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});
#endregion

#region enrollment
//GET all enrolments
app.MapGet("/enrollment", async (ISQLServerService sqlService) =>
    Results.Ok(await sqlService.GetEnrollmentsAsync()));

//GET all full enrolments
app.MapGet("/enrollmentcompl", async (ISQLServerService sqlService) =>
    Results.Ok(await sqlService.GetEnrollmentsWithDetailsAsync()));

//GET all enrollments for person
app.MapGet("/enrollmentperson/{id}", async (int id, ISQLServerService sqlService) =>
{
    var enrollments = await sqlService.GetEnrollmentsPersonAsync(id);
    return enrollments.Any() ? Results.Ok(enrollments) : Results.NotFound();
});

//ADD enrollment
app.MapPost("/enrollment", async (Enrollment enrollment, ISQLServerService sqlService) =>
{
    var createdEnrollment = await sqlService.AddEnrollmentAsync(enrollment);
    return Results.Created($"/course/{createdEnrollment.ID}", createdEnrollment);
});

//UPDATE enrollment
app.MapPut("/enrollment", async (Enrollment enrollment, ISQLServerService sqlService) =>
    await sqlService.UpdateEnrollmentAsync(enrollment)
    ? Results.Ok(enrollment) : Results.NotFound());

//DELETE enrollment
app.MapDelete("/enrollment/{id}", async (int id, ISQLServerService sqlService) =>
    await sqlService.DeleteEnrollmentAsync(id)
    ? Results.Ok() : Results.NotFound());
#endregion

#region mix
//GET course with person count
app.MapGet("/courseenrollmentcount", async (ISQLServerService sqlService) =>
    Results.Ok(await sqlService.GetCoursesWithPersonCountAsync()));

#endregion
app.Run();
using Microsoft.EntityFrameworkCore;
using ASPWebAPI.Context;
using ASPWebAPI.Services;
using ASPWebAPI.Model;
using System.Diagnostics;
using ASPWebAPI.Validation;

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
}).AddEndpointFilter<ValidationFilter<Person>>();

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
}).AddEndpointFilter<ValidationFilter<Person>>();

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
}).AddEndpointFilter<ValidationFilter<Course>>();

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
}).AddEndpointFilter<ValidationFilter<Course>>();

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
{
    try
    {
        var result = await sqlService.GetEnrollmentsAsync();
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }
    catch (Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});

//GET all full enrolments
app.MapGet("/enrollmentcomplete", async (ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.GetEnrollmentsWithDetailsAsync();
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});
// GET enrollment by id
app.MapGet("/enrollment/{id}", async (int id, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.GetEnrollmentAsync(id);
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});
//GET all enrollments for person
app.MapGet("/enrollmentcompleteperson/{id}", async (int id, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.GetEnrollmentsWithDetailsByPersonAsync(id);
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }catch(Exception ex)
    {
        Console.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});
//GET all enrollments for course
app.MapGet("/enrollmentcompletecourse/{id}", async (int id, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.GetEnrollmentsWithDetailsByCourseAsync(id);
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});
//ADD enrollment
app.MapPost("/enrollment", async (Enrollment enrollment, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.AddEnrollmentAsync(enrollment);
        return Results.Created();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
}).AddEndpointFilter<ValidationFilter<Enrollment>>();

//UPDATE enrollment
app.MapPut("/enrollment", async (Enrollment enrollment, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.UpdateEnrollmentAsync(enrollment);
        if (result) return Results.Ok();
        else return Results.BadRequest();
    }catch(Exception ex)
    {
        Console.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
}).AddEndpointFilter<ValidationFilter<Enrollment>>();

//DELETE enrollment
app.MapDelete("/enrollment/{id}", async (int id, ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.DeleteEnrollmentAsync(id);
        if (result) return Results.Ok();
        else return Results.NotFound();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});
#endregion

#region mix
//GET course with person count
app.MapGet("/courseenrollmentcount", async (ISQLServerService sqlService) =>
{
    try
    {
        var result = await sqlService.GetCoursesWithPersonCountAsync();
        if (result != null) return Results.Ok(result);
        else return Results.NoContent();
    }catch(Exception ex)
    {
        Debug.WriteLine(ex.Message); //TODO add logger
        return Results.Problem();
    }
});

#endregion
app.Run();
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
// WARNING - CONNECTION STRING SHOULD NOT BE PUSHED TO GITHUB ITS JUST AN EXAMPLE

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
    Results.Ok(await sqlService.GetPeopleAsync()));
// GET person by id
app.MapGet("/person/{id}", async (int id, ISQLServerService sqlService) =>
    Results.Ok(await sqlService.GetPersonAsync(id) is Person person
    ? Results.Ok(person) : Results.NotFound()));
// ADD person
app.MapPost("/person", async (Person person, ISQLServerService sqlService) =>
{
    var createdPerson = await sqlService.AddPersonAsync(person);
    return Results.Created($"/person/{createdPerson.ID}", createdPerson);
});
// UPDATE person
app.MapPut("/person", async (Person person, ISQLServerService sqlService) =>
    await sqlService.UpdatePersonAsync(person)
    ? Results.Ok(person) : Results.NotFound());
// DELETE person
app.MapDelete("/person/{id}", async (int id, ISQLServerService sqlService) =>
    await sqlService.DeletePersonAsync(id)
    ? Results.Ok() : Results.NotFound());
#endregion
#region course
//GET all courses
app.MapGet("/course", async (ISQLServerService sqlService) =>
    Results.Ok(await sqlService.GetCoursesAsync()));
//GET course by id
app.MapGet("/course/{id}", async (int id, ISQLServerService sqlService) =>
    Results.Ok(await sqlService.GetCourseAsync(id) is Course course
    ? Results.Ok(course) : Results.NotFound()));
//ADD course
app.MapPost("/course", async (Course course, ISQLServerService sqlService) =>
{
    var createdCourse = await sqlService.AddCourseAsync(course);
    return Results.Created($"/course/{createdCourse.ID}", createdCourse);
});
//UPDATE course
app.MapPut("/course", async (Course course, ISQLServerService sqlService) =>
    await sqlService.UpdateCourseAsync(course)
    ? Results.Ok(course) : Results.NotFound());
//DELETE course
app.MapDelete("/course/{id}", async (int id, ISQLServerService sqlService) =>
    await sqlService.DeleteCourseAsync(id)
    ? Results.Ok() : Results.NotFound());
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
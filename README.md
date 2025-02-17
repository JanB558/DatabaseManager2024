# Database Manager 2024
Example Database Manager project.

# API
Minimal API. Full error handling and logging included.

## Used technology
- .NET 8
- Microsoft SQL Server
  
## API Endpoints
### GET
**person** - get all people\
**person/id** - get person by id\
**course** - get all courses\
**course/id** - get course by id\
**enrollment** - get all enrollments (just enrollments)\
**enrollmentcomplete** - get all full enrollments (contains data about people and courses)\
**enrollmentcompletecourse/id** - get all full enrollments for course by course id\
**enrollmentcompleteperson/id** - get all full enrollments for person by person id\
**courseenrollmentcount** - get courses with enrollment count\
**personenrollmentcount** - get people with enrollment count

### POST
**person**\
**course**\
**enrollment**

### PUT
**person**\
**course**\
**enrollment**

### DELETE
**person/id**\
**course/id**\
**enrollment/id**

# Web App
ASP.NET user interface for API.

## Key features
- create, edit, delete courses
- create, edit, delete people
- add people to courses
- delete people from courses
- mark courses as complete

## Used technology
- .NET 8
- ASP.NET MVC
- Bootstrap

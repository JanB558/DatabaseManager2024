USE master
GO

ALTER DATABASE CourseDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

DROP DATABASE CourseDB;
GO

CREATE DATABASE CourseDB;
GO

USE CourseDB;
GO

CREATE TABLE Course (
	ID INT PRIMARY KEY IDENTITY(1,1),
	CourseName NVARCHAR(50)
);
GO

CREATE TABLE Person (
    ID INT PRIMARY KEY IDENTITY(1,1), 
    FirstName NVARCHAR(50) NOT NULL,   
    LastName NVARCHAR(50) NOT NULL,  
	CourseID INT, 
	FOREIGN KEY (CourseID) REFERENCES Course(ID)
);
GO

INSERT INTO Course (CourseName) VALUES ('Physics')
INSERT INTO Course (CourseName) VALUES ('Mathematics')
INSERT INTO Course (CourseName) VALUES ('Science')

SELECT * FROM Course

INSERT INTO Person (FirstName, LastName, CourseID) VALUES ('John', 'Doe', 2)
INSERT INTO Person (FirstName, LastName, CourseID) VALUES ('Jane', 'Doe', 3)
INSERT INTO Person (FirstName, LastName, CourseID) VALUES ('Peter', 'Jackson', 1)
INSERT INTO Person (FirstName, LastName, CourseID) VALUES ('Matt', 'Damon', 2)

SELECT p.ID, p.FirstName, p.LastName, c.CourseName FROM Person p INNER JOIN Course c ON c.ID = p.CourseID
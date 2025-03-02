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
	CourseName NVARCHAR(50) NOT NULL,
    VersionStamp rowversion
);
GO

CREATE TABLE Person (
    ID INT PRIMARY KEY IDENTITY(1,1), 
    FirstName NVARCHAR(50) NOT NULL,   
    LastName NVARCHAR(50) NOT NULL,
    VersionStamp rowversion
);
GO

INSERT INTO Course (CourseName) VALUES ('Physics')
INSERT INTO Course (CourseName) VALUES ('Mathematics')
INSERT INTO Course (CourseName) VALUES ('Science')

SELECT * FROM Course

INSERT INTO Person (FirstName, LastName) VALUES ('John', 'Doe')
INSERT INTO Person (FirstName, LastName) VALUES ('Jane', 'Doe')
INSERT INTO Person (FirstName, LastName) VALUES ('Peter', 'Jackson')
INSERT INTO Person (FirstName, LastName) VALUES ('Matt', 'Damon')

SELECT p.ID, p.FirstName, p.LastName FROM Person p

CREATE TABLE Enrollment (
    ID INT PRIMARY KEY IDENTITY(1,1),
    PersonID INT NOT NULL,
    CourseID INT NOT NULL,
    EnrollmentDate DATE NOT NULL,
    CompletionDate DATE NULL,
    VersionStamp rowversion,
    CONSTRAINT UQ_Enrollment_PersonID_CourseID UNIQUE (PersonID, CourseID),
    CONSTRAINT FK_Enrollment_PersonID FOREIGN KEY (PersonID) REFERENCES Person(ID),
    CONSTRAINT FK_Enrollment_CourseID FOREIGN KEY (CourseID) REFERENCES Course(ID)
);

INSERT INTO Enrollment (PersonID, CourseID, EnrollmentDate)
VALUES 
(1, 1, CAST(GETDATE() AS DATE)),
(2, 2, CAST(GETDATE() AS DATE)),
(3, 1, CAST(GETDATE() AS DATE))

SELECT e.ID, p.FirstName, p.LastName, c.CourseName FROM Enrollment e
INNER JOIN Person p ON e.PersonID = p.ID
INNER JOIN Course c ON e.CourseID = c.ID;
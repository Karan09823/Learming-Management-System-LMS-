use LMSIntern;

create table Admin_table(
Admin_id int not null  primary key identity(1,1),
Admin_Name varchar(max) not null,
Admin_Email varchar(max) not null,
Admin_password varchar(max) not null
);



create table Instructor_table(
Instructor_id int  primary key identity(1,1),
Instructor_Name varchar(max) not null,
Instructor_Email varchar(max) not null,
Instructor_password varchar(max) not null
);

create table Course_table(
course_id int  primary key identity(1,1),
course_name varchar(max) not null,
module_name varchar(max) not null,
module_link varchar(max) not null,
course_flag bit  default 0,
course_code varchar(max) not null,
course_instructorId int  foreign key references Instructor_table(Instructor_id)
);

create table Learner_table(
Learner_id int not null primary key identity(1,1),
Learner_name varchar(max)  not null,
Learner_email varchar(max) not null,
Learner_password varchar(max) not null
);

CREATE TABLE Learner_Courses (
Learner_courseId int primary key identity(1,1),
Ins_Id int not null,
Ins_Name varchar(max) not null,
CourseCode varchar(max) not null,
CourseName varchar(max) not null,
LearnerId int not null,
AccessFlag bit default 0,
LearnerName varchar(max) not null,
LearnerEmail varchar(max) not null 
);

insert into Admin_table(Admin_Name,Admin_Email,Admin_password) values('admin1','admin1@gmail.com','123456');



drop table Course_table;
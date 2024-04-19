use master;

CREATE TABLE Animal (
    IdAnimal int primary key,
    Name nvarchar(200) not null,
    Description nvarchar(200),
    Category nvarchar(200) not null,
    Area nvarchar(200) not null
);
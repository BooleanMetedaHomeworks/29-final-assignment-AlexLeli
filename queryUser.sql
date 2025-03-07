    CREATE TABLE Users
    (
        ID_User int primary key identity (1,1) not null,
        Email nvarchar(50) not null unique,
        PasswordHash nvarchar(255) not null
    );


    CREATE TABLE Roles
    (
        ID_Role INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
        [Name] NVARCHAR(50) NOT NULL UNIQUE
    );

    CREATE TABLE UserRole
    (
        ID_User INT NOT NULL,
        ID_Role INT NOT NULL,
        PRIMARY KEY (ID_User, ID_Role),
        FOREIGN KEY (ID_User) REFERENCES Users(ID_User),
        FOREIGN KEY (ID_Role) REFERENCES Roles(ID_Role)
    );
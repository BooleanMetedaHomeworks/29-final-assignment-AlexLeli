-- Tabella Categories
CREATE TABLE Categories (
    ID_Category INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL UNIQUE
);

-- Tabella Dishes
CREATE TABLE Dishes (
    ID_Dish INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL UNIQUE,
    [Description] NVARCHAR(255),
    Price DECIMAL(8,2) NOT NULL CHECK (Price >= 0),
    ID_Category INT NULL,
    CONSTRAINT FK_Dishes_Categories FOREIGN KEY (ID_Category) REFERENCES Categories(ID_Category) ON DELETE SET NULL
);

-- Tabella Menues
CREATE TABLE Menues (
    ID_Menu INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL UNIQUE
);

-- Tabella ponte Dish_Menu per la relazione N a N
CREATE TABLE Dish_Menu (
    ID_Dish INT NOT NULL,
    ID_Menu INT NOT NULL,
    PRIMARY KEY (ID_Dish, ID_Menu),
    CONSTRAINT FK_Dish_Menu_Dish FOREIGN KEY (ID_Dish) REFERENCES Dishes(ID_Dish) ON DELETE CASCADE,
    CONSTRAINT FK_Dish_Menu_Menu FOREIGN KEY (ID_Menu) REFERENCES Menues(ID_Menu) ON DELETE CASCADE
);
CREATE DATABASE AgriConnectDB;
GO

USE AgriConnectDB;
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL, -- 'Farmer' or 'Employee'
    Email NVARCHAR(255) NOT NULL
);

CREATE TABLE Farmers (
    FarmerId INT PRIMARY KEY IDENTITY,
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Name NVARCHAR(100),
    Email NVARCHAR(100)
);
ALTER TABLE Farmers
DROP CONSTRAINT DF__Farmers__Created__01142BA1;

SELECT
    dc.name AS DefaultConstraintName
FROM
    sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
WHERE
    c.object_id = OBJECT_ID('Farmers')
    AND c.name = 'CreatedDate';


CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY,
    FarmerId INT FOREIGN KEY REFERENCES Farmers(FarmerId),
     UserId INT FOREIGN KEY REFERENCES Users(UserId), 
    ProductName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    ProductionDate DATE
);



ALTER TABLE Farmers
DROP COLUMN CreatedDate;


DROP TABLE Users;

INSERT INTO Products (FarmerId, ProductName, Category, ProductionDate)
VALUES 
(1, 'Tomatoes', 'Vegetable', '2025-05-01'),
(2, 'Apples', 'Fruit', '2025-04-15'),
(3, 'Potatoes', 'Vegetable', '2025-06-10'),
(1, 'Lettuce', 'Vegetable', '2025-05-20'),
(4, 'Oranges', 'Fruit', '2025-03-25'),
(2, 'Strawberries', 'Fruit', '2025-05-05'),
(3, 'Carrots', 'Vegetable', '2025-06-01'),
(4, 'Spinach', 'Vegetable', '2025-04-18'),
(1, 'Peaches', 'Fruit', '2025-03-30'),
(2, 'Cucumbers', 'Vegetable', '2025-05-22');


INSERT INTO Users (Username, PasswordHash, Role, Email)
VALUES 
('brucewayne', 'hashedpassword1', 'Farmer', 'bruce@wayneenterprises.com'),
('dickgrayson', 'hashedpassword2', 'Employee', 'dick@bludhaven.com'),
('barbaragordon', 'hashedpassword3', 'Farmer', 'barbara@oracle.net'),
('jasontodd', 'hashedpassword4', 'Employee', 'jason@redhood.com'),
('timdrake', 'hashedpassword5', 'Farmer', 'tim@robin.net'),
('damianwayne', 'hashedpassword6', 'Employee', 'damian@batfamily.com');

INSERT INTO Farmers (UserId, Name, Email)
VALUES 
(1,'Joker', 'joker@arkham.com'),
(2, 'Harley Quinn', 'harley@arkham.com'),
(3, 'Riddler', 'riddler@arkham.com'),
(4, 'Penguin', 'penguin@gotham.com'),
(5, 'Two-Face', 'twoface@gotham.com'),
(6, 'Poison Ivy', 'ivy@arkham.com');


SELECT * FROM Users
SELECT * FROM Farmers
SELECT * FROM Products

UPDATE Farmers SET Name = 'Unknown' WHERE Name IS NULL;
UPDATE Farmers SET Email = 'noemail@example.com' WHERE Email IS NULL;

DELETE FROM Products;
DELETE FROM Farmers;
DELETE FROM Users;




CREATE DATABASE ProductManagementDb;
GO

USE ProductManagementDb;
GO

CREATE SCHEMA AppSchema;
GO


CREATE TABLE AppSchema.ParentGroup (
    ParentGroupId INT IDENTITY(1,1) PRIMARY KEY,
    ParentGroupName VARCHAR(255) NOT NULL UNIQUE,
);

CREATE TABLE AppSchema.SubGroup(
    SubGroupId INT IDENTITY(1,1) PRIMARY KEY,
    SubGroupName VARCHAR(255) NOT NULL UNIQUE,
    ParentGroupId INT FOREIGN KEY REFERENCES AppSchema.ParentGroup(ParentGroupId)
);

CREATE TABLE AppSchema.Product (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName VARCHAR(255) NOT NULL UNIQUE,
    ProductAdded DATETIME NOT NULL,
    ProductEdited DATETIME DEFAULT GETDATE(),
    Price DECIMAL(5,2) NOT NULL,
    PriceVat DECIMAL(5,2) NOT NULL,
    Vat INT DEFAULT 20,
    SubGroupId INT FOREIGN KEY REFERENCES AppSchema.SubGroup(SubGroupId),
    Active BIT
);

CREATE TABLE AppSchema.Store (
    StoreId INT IDENTITY(1,1) PRIMARY KEY,
    StoreName VARCHAR(255) NOT NULL UNIQUE,
);

CREATE TABLE AppSchema.StoreProduct (
    StoreId INT FOREIGN KEY REFERENCES AppSchema.Store(StoreId),
    ProductId INT FOREIGN KEY REFERENCES AppSchema.Product(ProductId),
    ProductAmount INT DEFAULT 0,
    CONSTRAINT PK_Store_Product PRIMARY KEY (StoreId, ProductId)
);
GO
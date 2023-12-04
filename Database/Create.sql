
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
    Vat INT NOT NULL,
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
    ProductAmount INT,
    CONSTRAINT PK_Store_Product PRIMARY KEY (StoreId, ProductId)
);
GO

CREATE PROCEDURE AppSchema.spProduct_Get
	@ProductId INT = NULL
AS
BEGIN
	SELECT product.ProductId,
		product.ProductName,
		product.ProductAdded,
		product.ProductEdited,
		product.Price,
		product.PriceVat,
		product.Vat,
		subGroup.SubGroupName,
		product.Active
		FROM AppSchema.Product AS product
		JOIN AppSchema.SubGroup AS subGroup ON
		product.SubGroupId = subGroup.SubGroupId
		WHERE product.ProductId = ISNULL(@ProductId, product.ProductId) AND product.Active = 1
END
GO

CREATE PROCEDURE AppSchema.spStore_GetByProduct
@ProductId INT
AS
BEGIN
	SELECT store.StoreId, store.StoreName , storeProduct.ProductAmount
		FROM AppSchema.StoreProduct AS storeProduct
		JOIN AppSchema.Store AS store
		ON storeProduct.StoreId = store.StoreId
		WHERE storeProduct.ProductId = @ProductId
END
GO

CREATE PROCEDURE AppSchema.spSubGroup_Get
@ParentGroupId INT
AS
BEGIN
	SELECT * FROM AppSchema.SubGroup AS subGroup
		WHERE subGroup.ParentGroupId = @ParentGroupId
END
GO

CREATE PROCEDURE AppSchema.spProduct_Add
@ProductName NVARCHAR(255),
@Price DECIMAL(5,2),
@PriceVat DECIMAL(5,2),
@Vat INT,
@SubGroupId INT,
@OutputProductId INT OUTPUT
AS
BEGIN
	INSERT INTO AppSchema.Product 
		VALUES(@ProductName, GETDATE(), GETDATE(), @Price, @PriceVat, @Vat, @SubGroupId, 'true');
	SET @OutputProductId = @@IDENTITY
END
GO

CREATE PROCEDURE AppSchema.spProduct_Edit
@ProductId INT,
@ProductName NVARCHAR(255),
@Price DECIMAL(5,2),
@PriceVat DECIMAL(5,2),
@Vat INT,
@SubGroupId INT
AS
BEGIN
	UPDATE AppSchema.Product
		SET ProductName = @ProductName, 
		ProductEdited = GETDATE(),
		Price = @Price,
		PriceVat = @PriceVat,
		Vat = @Vat,
		SubGroupId = @SubGroupId,
		Active = 1
	WHERE ProductId = @ProductId
END
GO

CREATE PROCEDURE AppSchema.spStoreProduct_Delete
@StoreProductId INT
AS
BEGIN
	DELETE FROM AppSchema.StoreProduct
	WHERE ProductId = @StoreProductId;
END
GO

CREATE PROCEDURE AppSchema.spProduct_Delete
@ProductId INT
AS
BEGIN
	UPDATE AppSchema.Product
		SET Active = 0, ProductEdited = GETDATE()
		WHERE ProductId = @ProductId;
	EXEC AppSchema.spStoreProduct_Delete @StoreProductId = @ProductId
END
GO

CREATE PROCEDURE AppSchema.spStoreProduct_Add
@StoreId INT,
@ProductId INT
AS
BEGIN
	INSERT INTO AppSchema.StoreProduct
		VALUES (@StoreId, @ProductId, 0)
END
GO

CREATE PROCEDURE AppSchema.spStoreProduct_Patch
@StoreId INT,
@ProductId INT,
@ProductAmount INT
AS
BEGIN
	UPDATE AppSchema.StoreProduct
		SET ProductAmount = @ProductAmount + (
			SELECT ProductAmount FROM AppSchema.StoreProduct
			WHERE StoreId = @StoreId AND ProductId = @ProductId)
		WHERE StoreId = @StoreId AND ProductId = @ProductId
END
GO

CREATE PROCEDURE AppSchema.spStoreProduct_Get
@StoreId INT,
@ProductId INT
AS
BEGIN
	SELECT * FROM AppSchema.StoreProduct
	WHERE StoreId = @StoreId AND ProductId = @ProductId
END
GO
SET XACT_ABORT ON;

IF (SELECT COUNT_BIG(*) FROM dbo.Customers) <> 40000
    THROW 50002, 'Load customer.sql first; product.sql expects exactly 40000 customers.', 1;

IF EXISTS (SELECT 1 FROM dbo.Customers WHERE Id < 1 OR Id > 40000)
    THROW 50003, 'Customers must have IDs from 1 through 40000.', 1;

IF EXISTS (SELECT 1 FROM dbo.Product)
    THROW 50004, 'Product must be empty before loading product.sql.', 1;

BEGIN TRY
    BEGIN TRANSACTION;

    SET IDENTITY_INSERT dbo.Product ON;

    ;WITH Digits AS
    (
        SELECT n
        FROM (VALUES (0), (1), (2), (3), (4), (5), (6), (7), (8), (9)) AS d(n)
    ),
    ProductNumbers AS
    (
        SELECT TOP (80000)
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Id
        FROM Digits AS a
        CROSS JOIN Digits AS b
        CROSS JOIN Digits AS c
        CROSS JOIN Digits AS d
        CROSS JOIN Digits AS e
    )
    INSERT INTO dbo.Product (Id, Name, Description, Price, CustomerId)
    SELECT
        Id,
        CONCAT('Product ', RIGHT('00000' + CONVERT(varchar(5), Id), 5)),
        CONCAT('Dummy product ', Id),
        CONVERT(decimal(18, 2), 9.99 + (Id % 10000) / 100.0),
        ((Id - 1) / 2) + 1
    FROM ProductNumbers;

    SET IDENTITY_INSERT dbo.Product OFF;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    SET IDENTITY_INSERT dbo.Product OFF;
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

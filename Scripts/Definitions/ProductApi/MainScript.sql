IF NOT EXISTS (SELECT NAME FROM SYS.DATABASES WHERE NAME = 'MangoProduct')
	BEGIN
		CREATE DATABASE MangoProduct;
	END;
GO

USE MangoProduct;

--> Tables

IF NOT EXISTS (SELECT  1 FROM SYS.TABLES WHERE NAME = 'Category')
	BEGIN

		CREATE TABLE Category (
			Id INT NOT NULL IDENTITY (1, 1)
			CONSTRAINT PK_Category_Id PRIMARY KEY (Id),
			Name VARCHAR(45) NOT NULL
			CONSTRAINT DF_Category_Name DEFAULT(''),
			Created DATETIME NOT NULL
			CONSTRAINT DF_Category_Created DEFAULT('1900-01-01 00:00:00.0'),
			CreatedBy INT NOT NULL
			CONSTRAINT DF_Category_CreatedBy DEFAULT(-1),
			Updated DATETIME NOT NULL
			CONSTRAINT DF_Category_Updated DEFAULT('1900-01-01 00:00:00.0'),
			UpdatedBy INT NOT NULL
			CONSTRAINT DF_Category_UpdatedBy DEFAULT(-1)
		);

	END;
GO

IF NOT EXISTS (SELECT  1 FROM SYS.TABLES WHERE NAME = 'Product')
	BEGIN

		CREATE TABLE Product (
			Id INT NOT NULL IDENTITY (1, 1)
			CONSTRAINT PK_Product_Id PRIMARY KEY (Id),
			Name VARCHAR(45) NOT NULL
			CONSTRAINT DF_Product_Name DEFAULT(''),
			Price DECIMAL(18, 2) NOT NULL
			CONSTRAINT DF_Product_Price DEFAULT(0),
			CategoryId INT NOT NULL
			CONSTRAINT DF_Product_CategoryId DEFAULT(-1)
			CONSTRAINT FK_Product_CategoryId FOREIGN KEY (CategoryId) REFERENCES Category (Id),
			ImageUrl VARCHAR(1024) NOT NULL
			CONSTRAINT DF_Product_ImageUrl DEFAULT(''),
			Created DATETIME NOT NULL
			CONSTRAINT DF_Product_Created DEFAULT('1900-01-01 00:00:00.0'),
			CreatedBy INT NOT NULL
			CONSTRAINT DF_Product_CreatedBy DEFAULT(-1),
			Updated DATETIME NOT NULL
			CONSTRAINT DF_Product_Updated DEFAULT('1900-01-01 00:00:00.0'),
			UpdatedBy INT NOT NULL
			CONSTRAINT DF_Product_UpdatedBy DEFAULT(-1)
		);

	END;
GO

--> Store Procedures

IF OBJECT_ID( 'spDeleteCategory' ) IS NULL
	EXEC ('CREATE PROCEDURE spDeleteCategory AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spDeleteCategory (
	@Id INT,
	@ReturnsObject BIT = 1
)
AS
/*								
** Name:						spDeleteCategory
** Purpose:						
** Returns:						Si @ReturnsObject
**									1 - Returns an object response (Result, Message, Error)
**									0 - Returns the error
**								
** Date of Creation:			12/February/2025
** Author of Creation:			MSoto
** Date of Modification:		
** Author of Modification:		
** Revision:					0
*/								
BEGIN

DECLARE @Trancount INT = -1,
		@Error INT = -1;

DECLARE @Result AS TABLE (
	Result BIT,
	Message VARCHAR(500),
	Error INT DEFAULT(-1)
);

SET NOCOUNT ON
	BEGIN TRY

		SET @Trancount = @@Trancount;

		IF @Trancount > 0
			SAVE TRANSACTION DeleteCategory;
		ELSE
			BEGIN TRANSACTION

		DELETE FROM dbo.Category
		WHERE Id = @Id;

		GOTO _FinTran;
		_RollBack:

		IF @Trancount = 0
			ROLLBACK TRANSACTION;
		ELSE IF @Trancount <> -1 AND XACT_STATE() <> -1
			ROLLBACK TRANSACTION DeleteCategory;

		IF EXISTS (SELECT 1 FROM @Result)
			BEGIN
				DELETE @Result;
			END;

		GOTO _Fin;
		_FinTran:

		IF @Trancount = 0
			COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		SET @Error = @@ERROR;
		PRINT CONCAT('[', ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' : ', @Error, ']');

		INSERT INTO @Result (Result, Message, Error)
		VALUES (0, ERROR_MESSAGE(), @Error);

		IF @Trancount = 0
			ROLLBACK TRANSACTION
		ELSE IF @Trancount <> -1
			IF XACT_STATE() <> -1
				ROLLBACK TRANSACTION DeleteCategory;

	END CATCH

	_Fin:
	SET NOCOUNT OFF;

	IF (@ReturnsObject = 1)
		BEGIN
			IF NOT EXISTS (SELECT 1 FROM @Result)
				BEGIN
					INSERT INTO @Result (
						Result, Message
					)
					SELECT 1, '';
				END;
			
			SELECT * FROM @Result;
		END;
	ELSE RETURN @Error;
END;

GO

IF OBJECT_ID( 'spQueryCategories' ) IS NULL
	EXEC ('CREATE PROCEDURE spQueryCategories AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spQueryCategories (
	@Id INT = -1,
	@Name VARCHAR(45) = ''
)
AS
/*								
** Name:						spQueryCategories
** Purpose:						
** Fields:						
** Dependencies:				
** Creation Date:				06/Marzo/2025
** Creation Author:				MSoto
** Modification Date:			
** Modification Author:			
** Revision:					0
*/								
BEGIN

IF EXISTS (
		SELECT 1
			FROM SYS.SYSOBJECTS
		WHERE ID = OBJECT_ID('dbo.#Response')
			AND TYPE = 'U'
	)
	BEGIN
		DROP TABLE #Response;
	END;

CREATE TABLE #Response (
	IsSuccess BIT,
	Message VARCHAR(500),
	Id INT,
	Name VARCHAR(45)
);

SET NOCOUNT ON
	BEGIN TRY

		IF @Id > 0 AND NOT EXISTS (SELECT 1 FROM dbo.Category WHERE Id = @Id)
			BEGIN
				INSERT INTO #Response (IsSuccess, Message)
				SELECT 0, 'Doesn''t exists an product with this id';

				GOTO _END;
			END;

		INSERT INTO #Response (
			IsSuccess, Message, Id,
			Name
		) 
		SELECT 1, '', C.Id,
			C.Name
		FROM dbo.Category C
		WHERE @Id IN (-1, 0, C.Id)
			AND (Name LIKE CONCAT('%', @Name, '%') OR @Name = '');

		IF NOT EXISTS (SELECT 1 FROM #Response)
			BEGIN
				INSERT INTO #Response (IsSuccess, Message)
				SELECT 0, 'No se encontraron registros.';
			END;

	END TRY
	BEGIN CATCH

		INSERT INTO #Response (IsSuccess, Message)
		SELECT 0, CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());
		PRINT CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());

	END CATCH

	_END:

	SELECT * FROM #Response;

	IF EXISTS (
			SELECT 1
				FROM SYS.SYSOBJECTS
			WHERE ID = OBJECT_ID('Tempdb.dbo.#Response')
				AND TYPE = 'U'
		)
		BEGIN
			DROP TABLE #Response;
		END;

END;

GO

IF OBJECT_ID( 'spSaveCategory' ) IS NULL
	EXEC ('CREATE PROCEDURE spSaveCategory AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spSaveCategory (
	@Id INT,
	@Name VARCHAR(45),
	@UserId INT,
	@ReturnsObject BIT = 1
)
AS
/*								
** Name:						spSaveCategory
** Purpose:						
** Returns:						Si @ReturnsObject
**									1 - Returns an object response (Result, Message, Error)
**									0 - Returns the error
**								
** Date of Creation:			06/Marzo/2025
** Author of Creation:			MSoto
** Date of Modification:		
** Author of Modification:		
** Revision:					0
*/								
BEGIN

DECLARE @Trancount INT = -1,
		@Error INT = -1;

DECLARE @Result AS TABLE (
	Result BIT,
	Message VARCHAR(500),
	Id INT,
	Name VARCHAR(45),
	Error INT DEFAULT(-1)
);

SET NOCOUNT ON
	BEGIN TRY

		SET @Trancount = @@Trancount;

		IF @Trancount > 0
			SAVE TRANSACTION SaveCategory;
		ELSE
			BEGIN TRANSACTION

		IF @Id > 0
			BEGIN
				UPDATE dbo.Category SET
					Name = @Name,
					Updated = GETDATE(),
					UpdatedBy = @UserId
				WHERE Id = @Id;
			END;
		ELSE
			BEGIN
				INSERT INTO dbo.Category(
					Name, Created, CreatedBy,
					Updated, UpdatedBy
				) VALUES (
					@Name, GETDATE(), @UserId,
					GETDATE(), @UserId
				);
			END;

		GOTO _FinTran;
		_RollBack:

		IF @Trancount = 0
			ROLLBACK TRANSACTION;
		ELSE IF @Trancount <> -1 AND XACT_STATE() <> -1
			ROLLBACK TRANSACTION SaveCategory;

		IF EXISTS (SELECT 1 FROM @Result)
			BEGIN
				DELETE @Result;
			END;

		GOTO _Fin;
		_FinTran:

		IF @Trancount = 0
			COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		SET @Error = @@ERROR;
		PRINT CONCAT('[', ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' : ', @Error, ']');

		INSERT INTO @Result (Result, Message, Error)
		VALUES (0, ERROR_MESSAGE(), @Error);

		IF @Trancount = 0
			ROLLBACK TRANSACTION
		ELSE IF @Trancount <> -1
			IF XACT_STATE() <> -1
				ROLLBACK TRANSACTION SaveCategory;

	END CATCH

	_Fin:
	SET NOCOUNT OFF;

	IF (@ReturnsObject = 1)
		BEGIN
			IF NOT EXISTS (SELECT 1 FROM @Result)
				BEGIN
					INSERT INTO @Result (
						Result, Message, Id, Name
					)
					SELECT 1, '', Id, Name
						FROM dbo.Category
					WHERE Id IN (@Id, SCOPE_IDENTITY());
				END;
			
			SELECT * FROM @Result;
		END;
	ELSE RETURN @Error;
END;

GO

IF OBJECT_ID( 'spDeleteProduct' ) IS NULL
	EXEC ('CREATE PROCEDURE spDeleteProduct AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spDeleteProduct (
	@Id INT,
	@ReturnsObject BIT = 1
)
AS
/*								
** Name:						spDeleteProduct
** Purpose:						
** Returns:						Si @ReturnsObject
**									1 - Returns an object response (Result, Message, Error)
**									0 - Returns the error
**								
** Date of Creation:			12/February/2025
** Author of Creation:			MSoto
** Date of Modification:		
** Author of Modification:		
** Revision:					0
*/								
BEGIN

DECLARE @Trancount INT = -1,
		@Error INT = -1;

DECLARE @Result AS TABLE (
	Result BIT,
	Message VARCHAR(500),
	Error INT DEFAULT(-1)
);

SET NOCOUNT ON
	BEGIN TRY

		SET @Trancount = @@Trancount;

		IF @Trancount > 0
			SAVE TRANSACTION NewCoupon;
		ELSE
			BEGIN TRANSACTION

		DELETE FROM dbo.Product
		WHERE Id = @Id;

		GOTO _FinTran;
		_RollBack:

		IF @Trancount = 0
			ROLLBACK TRANSACTION;
		ELSE IF @Trancount <> -1 AND XACT_STATE() <> -1
			ROLLBACK TRANSACTION NewCoupon;

		IF EXISTS (SELECT 1 FROM @Result)
			BEGIN
				DELETE @Result;
			END;

		GOTO _Fin;
		_FinTran:

		IF @Trancount = 0
			COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		SET @Error = @@ERROR;
		PRINT CONCAT('[', ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' : ', @Error, ']');

		INSERT INTO @Result (Result, Message, Error)
		VALUES (0, ERROR_MESSAGE(), @Error);

		IF @Trancount = 0
			ROLLBACK TRANSACTION
		ELSE IF @Trancount <> -1
			IF XACT_STATE() <> -1
				ROLLBACK TRANSACTION NewCoupon;

	END CATCH

	_Fin:
	SET NOCOUNT OFF;

	IF (@ReturnsObject = 1)
		BEGIN
			IF NOT EXISTS (SELECT 1 FROM @Result)
				BEGIN
					INSERT INTO @Result (
						Result, Message
					)
					SELECT 1, '';
				END;
			
			SELECT * FROM @Result;
		END;
	ELSE RETURN @Error;
END;

GO

IF OBJECT_ID( 'spQueryProducts' ) IS NULL
	EXEC ('CREATE PROCEDURE spQueryProducts AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spQueryProducts (
	@Id INT = -1
)
AS
/*								
** Name:						spQueryProducts
** Purpose:						
** Fields:						
** Dependencies:				
** Creation Date:				06/Marzo/2025
** Creation Author:				MSoto
** Modification Date:			
** Modification Author:			
** Revision:					0
*/								
BEGIN

IF EXISTS (
		SELECT 1
			FROM SYS.SYSOBJECTS
		WHERE ID = OBJECT_ID('dbo.#Response')
			AND TYPE = 'U'
	)
	BEGIN
		DROP TABLE #Response;
	END;

CREATE TABLE #Response (
	IsSuccess BIT,
	Message VARCHAR(500),
	Id INT,
	Name VARCHAR(85),
	Price DECIMAL(18, 2),
	CategoryName VARCHAR(45),
	ImageUrl VARCHAR(1024)
);

SET NOCOUNT ON
	BEGIN TRY

		IF @Id > 0 AND NOT EXISTS (SELECT 1 FROM Product WHERE Id = @Id)
			BEGIN
				INSERT INTO #Response (IsSuccess, Message)
				SELECT 0, 'Doesn''t exists an product with this id';

				GOTO _END;
			END;

		INSERT INTO #Response (
			IsSuccess, Message, Id,
			Name, Price, CategoryName,
			ImageUrl
		) 
		SELECT 1, '', P.Id,
			P.Name, P.Price, C.Name,
			P.ImageUrl
		FROM dbo.Product P
			INNER JOIN dbo.Category C ON C.Id = P.CategoryId
		WHERE @Id IN (-1, P.Id);

		IF NOT EXISTS (SELECT 1 FROM #Response)
			BEGIN
				INSERT INTO #Response (IsSuccess, Message)
				SELECT 0, 'No se encontraron registros.';
			END;

	END TRY
	BEGIN CATCH

		INSERT INTO #Response (IsSuccess, Message)
		SELECT 0, CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());
		PRINT CONCAT(ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' - ', ERROR_LINE());

	END CATCH

	_END:

	SELECT * FROM #Response;

	IF EXISTS (
			SELECT 1
				FROM SYS.SYSOBJECTS
			WHERE ID = OBJECT_ID('Tempdb.dbo.#Response')
				AND TYPE = 'U'
		)
		BEGIN
			DROP TABLE #Response;
		END;

END;

GO

IF OBJECT_ID( 'spSaveProduct' ) IS NULL
	EXEC ('CREATE PROCEDURE spSaveProduct AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spSaveProduct (
	@Id INT,
	@Name VARCHAR(45),
	@Price DECIMAL(18, 2),
	@CategoryId INT,
	@ImageUrl VARCHAR(1024),
	@UserId INT,
	@ReturnsObject BIT = 1
)
AS
/*								
** Name:						spSaveProduct
** Purpose:						
** Returns:						Si @ReturnsObject
**									1 - Returns an object response (Result, Message, Error)
**									0 - Returns the error
**								
** Date of Creation:			12/February/2025
** Author of Creation:			MSoto
** Date of Modification:		
** Author of Modification:		
** Revision:					0
*/								
BEGIN

DECLARE @Trancount INT = -1,
		@Error INT = -1;

DECLARE @Result AS TABLE (
	Result BIT,
	Message VARCHAR(500),
	Id INT,
	Name VARCHAR(85),
	Price DECIMAL(18, 2),
	CategoryName VARCHAR(45),
	ImageUrl VARCHAR(1024),
	Error INT DEFAULT(-1)
);

SET NOCOUNT ON
	BEGIN TRY

		SET @Trancount = @@Trancount;

		IF @Trancount > 0
			SAVE TRANSACTION SaveProduct;
		ELSE
			BEGIN TRANSACTION

		IF @Id > 0
			BEGIN
				UPDATE dbo.Product SET
					Name = @Name,
					Price = @Price,
					CategoryId = @CategoryId,
					ImageUrl = @ImageUrl,
					Updated = GETDATE(),
					UpdatedBy = @UserId
				WHERE Id = @Id;
			END;
		ELSE
			BEGIN
				INSERT INTO dbo.Product(
					Name, Price, CategoryId,
					ImageUrl, Updated, UpdatedBy,
					Created, CreatedBy
				) VALUES (
					@Name, @Price, @CategoryId,
					@ImageUrl, GETDATE(), @UserId,
					GETDATE(), @UserId
				);
			END;

		GOTO _FinTran;
		_RollBack:

		IF @Trancount = 0
			ROLLBACK TRANSACTION;
		ELSE IF @Trancount <> -1 AND XACT_STATE() <> -1
			ROLLBACK TRANSACTION SaveProduct;

		IF EXISTS (SELECT 1 FROM @Result)
			BEGIN
				DELETE @Result;
			END;

		GOTO _Fin;
		_FinTran:

		IF @Trancount = 0
			COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		SET @Error = @@ERROR;
		PRINT CONCAT('[', ERROR_PROCEDURE(), ' : ', ERROR_MESSAGE(), ' : ', @Error, ']');

		INSERT INTO @Result (Result, Message, Error)
		VALUES (0, ERROR_MESSAGE(), @Error);

		IF @Trancount = 0
			ROLLBACK TRANSACTION
		ELSE IF @Trancount <> -1
			IF XACT_STATE() <> -1
				ROLLBACK TRANSACTION SaveProduct;

	END CATCH

	_Fin:
	SET NOCOUNT OFF;

	IF (@ReturnsObject = 1)
		BEGIN
			IF NOT EXISTS (SELECT 1 FROM @Result)
				BEGIN
					INSERT INTO @Result (
						Result, Message, Id,
						Name, Price, CategoryName,
						ImageUrl
					)
					SELECT 1, '', P.Id,
						P.Name, P.Price, C.Name,
						P.ImageUrl
					FROM dbo.Product P
						INNER JOIN dbo.Category C ON C.Id = P.CategoryId
					WHERE P.Id IN (@Id, SCOPE_IDENTITY());
				END;
			
			SELECT * FROM @Result;
		END;
	ELSE RETURN @Error;
END;

GO
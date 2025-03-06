IF OBJECT_ID( 'spSaveProduct' ) IS NULL
	EXEC ('CREATE PROCEDURE spSaveProduct AS SET NOCOUNT ON;');
GO

ALTER PROCEDURE spSaveProduct (
	@Id INT,
	@Name VARCHAR(45),
	@Price DECIMAL(18, 2),
	@CategoryId INT,
	@ImageUrl VARCHAR(1024),
	@UserId VARCHAR(45),
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
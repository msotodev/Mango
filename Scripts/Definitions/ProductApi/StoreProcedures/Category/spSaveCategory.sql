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
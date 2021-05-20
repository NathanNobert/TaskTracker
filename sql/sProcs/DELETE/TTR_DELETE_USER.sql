IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_DELETE_USER' AND type = 'P')
DROP PROCEDURE TTR_DELETE_USER
GO

/*==============================================================================
TTR_DELETE_USER

DESCRIPTION: 
Stored proc to delete a record from the TTR_USERS table

DATE CREATED:2021.05.10
AUTHOR:nnobert

INPUT:
@p_usr_id                           INT 

RETURNS:
@@error
--------------------------------------------------------------------------------
Change History
--------------------------------------------------------------------------------

==============================================================================*/
CREATE PROCEDURE TTR_DELETE_USER
	@p_usr_id INT = Null
AS

DECLARE @l_error INT

BEGIN TRANSACTION

DELETE FROM TTR_USER
	WHERE USR_ID = @p_usr_id

SET @l_error = @@ERROR
IF @l_error != 0 GOTO errorhandler

COMMIT TRANSACTION
RETURN @l_error

errorhandler:
ROLLBACK TRANSACTION
RETURN @l_error

GO


IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_DELETE_TASKS' AND type = 'P')
DROP PROCEDURE TTR_DELETE_TASKS
GO

/*==============================================================================
TTR_DELETE_TASKS

DESCRIPTION: 
Stored proc to delete a record from the TTR_TASKS table

DATE CREATED:2021.05.10
AUTHOR:rforbes

INPUT:
@p_tts_id                           INT 

RETURNS:
@@error
--------------------------------------------------------------------------------
Change History
--------------------------------------------------------------------------------

==============================================================================*/
CREATE PROCEDURE TTR_DELETE_TASKS
	@p_tts_id INT = Null
AS

DECLARE @l_error INT

BEGIN TRANSACTION

DELETE FROM TTR_TASKS
	WHERE TTS_ID = @p_tts_id

SET @l_error = @@ERROR
IF @l_error != 0 GOTO errorhandler

COMMIT TRANSACTION
RETURN @l_error

errorhandler:
ROLLBACK TRANSACTION
RETURN @l_error

GO


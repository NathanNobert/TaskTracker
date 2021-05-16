IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_UPDATE_TASKS' AND type = 'P')
DROP PROCEDURE TTR_UPDATE_TASKS
GO

/*==============================================================================
TTR_UPDATE_TASKS

DESCRIPTION: 
Stored Proc to update a row in the TTR_TASKS table.

DATE CREATED:2021.05.16
AUTHOR:rforbes

INPUT:
@p_tts_code     				VARCHAR(20)
@p_tts_desc		            	VARCHAR(max)
@p_tts_active  					bit

OUTPUT:
@p_retval                           INT

RETURNS:
@l_error
--------------------------------------------------------------------------------
Change History
--------------------------------------------------------------------------------
==============================================================================*/
CREATE PROCEDURE TTR_UPDATE_TASKS
	@p_tts_code			VARCHAR(20)		    = NULL,
	@p_tts_desc			VARCHAR(max)		= NULL,
	@p_tts_active       bit                 = Null,
	@p_retval           INT = 0 OUT
AS

DECLARE @l_error INT

BEGIN TRANSACTION

UPDATE TTR_TASKS SET
	TTS_CODE   = @p_tts_code,
	TTS_DESC   = @p_tts_desc,
	TTS_ACTIVE = @p_tts_active

WHERE TTS_ID = @p_tts_id

SET @l_error = @@ERROR
IF @l_error != 0 GOTO errorhandler

SET @p_retval = @p_tts_id

COMMIT TRANSACTION
RETURN @l_error

errorhandler:
IF @@TRANCOUNT > 0
	ROLLBACK TRANSACTION
RETURN @l_error
GO


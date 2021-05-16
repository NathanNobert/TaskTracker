IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_TASKS' AND type = 'P')
DROP PROCEDURE TTR_GET_TASKS 
GO

/*==============================================================================
TTR_GET_USERS

DESCRIPTION: 
Stored Proc to select all rows in the TTR_TASKS table.

DATE CREATED:2021.05.10
AUTHOR:rforbes

INPUT:
@p_tts_id                         INT 



OUTPUT:
TTR_TASKS Data Set
--------------------------------------------------------------------------------
==============================================================================*/
CREATE PROCEDURE TTR_GET_TASKS
	@p_tts_id INT = Null,
    @p_tts_code varchar(8) = Null	 
AS

SELECT 	TTS_ID,
        TTS_CODE,
        TTS_DESC,
        TTS_ACTIVE

  FROM  TTR_TASKS
 WHERE ( TTS_ID = @p_tts_id OR @p_tts_id IS NULL)
 AND  (TTS_CODE = @p_tts_code OR @p_tts_code is NULL)

GO

--select * from TTR_TASKS
IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_TASK' AND type = 'P')
DROP PROCEDURE TTR_GET_TASK
GO

/*==============================================================================
TTR_GET_TASK

DESCRIPTION: 
Stored Proc to select all rows in the TTR_TASK table.

DATE CREATED:2021.05.10
AUTHOR:rforbes

INPUT:
@p_tsk_id                         INT 



OUTPUT:
TTR_TASK Data Set
--------------------------------------------------------------------------------
==============================================================================*/
CREATE PROCEDURE TTR_GET_TASK
	@p_tsk_id INT = Null,
    @p_tsk_code varchar(8) = Null	 
AS

SELECT 	TSK_ID,
        TSK_CODE,
        TSK_DESC,
        TSK_ACTIVE

  FROM  TTR_TASK
 WHERE ( TSK_ID = @p_tsk_id OR @p_tsk_id IS NULL)
 AND  (TSK_CODE = @p_tsk_code OR @p_tsk_code is NULL)

GO
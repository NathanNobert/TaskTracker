IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_GET_USER' AND type = 'P')
DROP PROCEDURE TTR_GET_USERS
GO

/*==============================================================================
TTR_GET_USER

DESCRIPTION: 
Stored Proc to select all rows in the TTR_USER table.

DATE CREATED:2021.05.10
AUTHOR:nnobert

INPUT:
@p_usr_id                         INT 



OUTPUT:
TTR_USER Data Set
--------------------------------------------------------------------------------
==============================================================================*/
CREATE PROCEDURE TTR_GET_USER
	@p_usr_id INT = Null,
    @p_usr_lan_name varchar(8) = Null	 
AS

SELECT 	USR_ID,
        USR_EMAIL,
		USR_LAN_NAME,
		USR_FNAME,
        USR_LNAME,
        USR_PASSWORD,
        USR_ADMIN

  FROM  TTR_USER
 WHERE ( USR_ID = @p_usr_id OR @p_usr_id IS NULL)
 AND  (USR_LAN_NAME = @p_usr_lan_name or @p_usr_lan_name is NULL)

GO


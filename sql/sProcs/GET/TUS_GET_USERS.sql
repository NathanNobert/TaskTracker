IF EXISTS (SELECT name FROM sysobjects 
		WHERE name = 'TTR_GET_USERS' AND type = 'P')
DROP PROCEDURE TTR_GET_USERS 
GO

/*==============================================================================
TTR_GET_USERS

DESCRIPTION: 
Stored Proc to select all rows in the TTR_USERS table.

DATE CREATED:2021.05.10
AUTHOR:nnobert

INPUT:
@p_tus_id                         INT 



OUTPUT:
TTR_USERS Data Set
--------------------------------------------------------------------------------
==============================================================================*/
CREATE PROCEDURE TTR_GET_USERS
	@p_tus_id INT = Null,
    @p_tus_lan_name varchar(8) = Null	 
AS

SELECT 	TUS_ID,
        TUS_EMAIL,
		TUS_LAN_NAME,
		TUS_FNAME,
        TUS_LNAME,
        TUS_PASSWORD,
        TUS_ADMIN

  FROM  TTR_USERS 
 WHERE ( TUS_ID = @p_tus_id OR @p_tus_id IS NULL)
 AND  (TUS_LAN_NAME = @p_tus_lan_name or @p_tus_lan_name is NULL)

GO


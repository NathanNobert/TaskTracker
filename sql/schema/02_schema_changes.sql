use TaskTrackerDev

BEGIN TRY
    BEGIN TRANSACTION
     
        CREATE TABLE TTR_USER(
            USR_ID int IDENTITY(1,1) PRIMARY KEY,
            USR_EMAIL varchar(50),
            USR_LAN_NAME varchar(8),
            USR_FNAME varchar(50),
            USR_LNAME varchar(50),
            USR_PASSWORD varchar(20),
            USR_ADMIN bit
        )

        insert into TTR_USER VALUES('nobert@ualberta.ca','nnobert', 'Nathan', 'Nobert', 'password', 1)
        insert into TTR_USER VALUES('rdforbes@ualberta.ca','rforbes', 'Riley', 'Forbes', 'password', 1)


        CREATE TABLE TTR_TASK_JOIN_USER(
            TJU_ID int IDENTITY(1,1) PRIMARY KEY,
            USR_ID_USER int,
            TSK_ID_TASK int
        )

        ALTER TABLE TTR_TASK_JOIN_USER
        ADD CONSTRAINT TUS_ID_USER_FK_USR_ID
        foreign key(USR_ID_USER) references TTR_USER(USR_ID)

        ALTER TABLE TTR_TASK_JOIN_USER
        ADD CONSTRAINT TTS_ID_TASK_FK_TSK_ID
        foreign key(TSK_ID_TASK) references TTR_TASK(TSK_ID)

    COMMIT TRANSACTION

PRINT 'Database update completed successfully.'
END TRY

BEGIN CATCH
	PRINT '
	!! Database update failed: ' + ERROR_MESSAGE()
	ROLLBACK TRANSACTION
	PRINT '
	Rolled Back'
END CATCH

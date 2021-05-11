use TaskTrackerDev

BEGIN TRY
    BEGIN TRANSACTION
        CREATE TABLE TTR_USERS(
            TUS_ID int IDENTITY(1,1) PRIMARY KEY,
            TUS_EMAIL varchar(50),
            TUS_LAN_NAME varchar(8),
            TUS_FNAME varchar(50),
            TUS_LNAME varchar(50),
            TUS_PASSWORD varchar(20),
            TUS_ADMIN bit
        )

        insert into TTR_USERS VALUES('nobert@ualberta.ca','nnobert', 'Nathan', 'Nobert', 'password', 1)
        insert into TTR_USERS VALUES('rdforbes@ualberta.ca','rforbes', 'Riley', 'Forbes', 'password', 1)

        CREATE TABLE TTR_TASKS_JOIN_USERS(
            TJU_ID int IDENTITY(1,1) PRIMARY KEY,
            TUS_ID_USERS int,
            TTS_ID_TASKS int
        )

        ALTER TABLE TTR_TASKS_JOIN_USERS
        ADD CONSTRAINT TUS_ID_USERS_FK_TUS_ID
        foreign key(TUS_ID_USERS) references TTR_USERS(TUS_ID)

        ALTER TABLE TTR_TASKS_JOIN_USERS
        ADD CONSTRAINT TTS_ID_TASKS_FK_TUS_ID
        foreign key(TTS_ID_TASKS) references TTR_TASKS(TTS_ID)

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

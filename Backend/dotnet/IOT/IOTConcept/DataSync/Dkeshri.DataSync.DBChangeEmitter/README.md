# Data-Sync-Emmiter

This application help to track the change in `MsSql` Database changes and send that changes to `RabbitMq` Message broker. On Start of this application it create a Table called **ChangeTrackers**, Which contain list of tables that will be tracked. There is hosted service which check the Database change in every **10 secs** and Send that changes to RabbiMq queue (default_queue: DataSyncQueue).

# Installation Steps

## Pre-requisite

> Note: Before running image need some steps to perform

### Step 1: Enable Change tracking on Database

> If not enabled please run below command.

```sql
ALTER DATABASE YourDatabaseName
SET CHANGE_TRACKING = ON 
(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);
```

# Database Synchronizer With Rabbit MQ

### Docker Build from IOTConcept Directory 

```bash
docker build -f DataSync/DataSync.DBChangeEmitter/Dockerfile -t dkeshri/data-sync-emitter:latest .
```
![alt text](image.png)

## Project Summary

This Project Get the changes of Tables and the push it to RabbitMQ Queue.
Then Pick that Message and Update the database on other side.

For Eg. in IOT Application We sync data from Edge to cloud.

## Enable Change Tracker in Database

### Step 1: Enable Change tracking in DATABASE

```bash 
ALTER DATABASE STORE
SET CHANGE_TRACKING = ON 
(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);
```


### Step 2: Enable Change tracking on Table 

```bash
ALTER TABLE Customers
ENABLE CHANGE_TRACKING;
```


#### Command to check changes on table 

> Note: lastChangeVersion is an Interger value form where you want changes.

```bash
SELECT * FROM CHANGETABLE(CHANGES Customers, lastChangeVersion) AS CT;
```


### Summary : How Change tracker works

The `CHANGE_TRACKING_CURRENT_VERSION()` function in SQL Server is used with the Change Tracking feature. It returns the current version of the database, which is a monotonically increasing number that is incremented whenever a tracked table is modified.

**Key Details** :

Purpose: 

Helps identify the latest change version in a database. This can be used to synchronize data or track changes efficiently.
Return Type: The function returns a bigint value representing the current version.
Usage Context:
Data synchronization: When syncing data, you can store the last retrieved version and use it later to fetch only changes since that version.
Optimized querying: Useful for querying changes incrementally without scanning the entire data.

>Syntax:

```sql
CHANGE_TRACKING_CURRENT_VERSION()
```
Example:
Enable Change Tracking
Before using `CHANGE_TRACKING_CURRENT_VERSION`(), ensure that change tracking is enabled for the database and the required tables:

```sql
ALTER DATABASE [YourDatabaseName]
SET CHANGE_TRACKING = ON 
(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);

ALTER TABLE [YourTableName]
ENABLE CHANGE_TRACKING;
```

Retrieve the Current Version

```sql
SELECT CHANGE_TRACKING_CURRENT_VERSION() AS CurrentVersion;
```

Use Case
If you're implementing data synchronization:

Retrieve and store the version when you fetch changes:
```sql
DECLARE @currentVersion BIGINT = CHANGE_TRACKING_CURRENT_VERSION();
SELECT @currentVersion AS SyncVersion;
Query changes since the last sync version:
sql
Copy code
SELECT * 
FROM CHANGETABLE(CHANGES YourTableName, @lastSyncVersion) AS CT;
```

> Notes: The version number does not correspond to a timestamp or datetime; it's an internal counter.
Ensure you manage version retention effectively, as changes beyond the retention period won't be accessible.

# Ef Core Migration
```bash
Scaffold-DbContext "Server=localhost;Database=XYZ_DB_NAME;User Id=sa;Password=XYZ;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir 'D:\Deepak_Keshri' -Tables [SCHEMA].[TABLE_NAME] -f
```

```bash
Scaffold-DbContext "Server=localhost;Database=Store;User Id=sa;Password=MsSqlServer;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir 'D:\dkeshri\Model' -Tables [dbo].[USERS] -f
```
## You need to install below package then you can run migration 

```bash
Microsoft.EntityFrameworkCore.Tools
```
if require then Add this package too in the startup project
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design

```
# Code First Approach


## Migration In Visual Studio.
# Step 1
```bash
Add-Migration IntialSQLDB -Context StoreDataContext -OutputDir ".\Migrations\SQL"
```
Note SET default project DATA\Store.Data.Logic in package manage console. and then run the above command.
# Step 2
```bash
Update-Database
```
Note SET default project DATA\Store.Data.Logic in package manage console. and then run the above command.

## OR

### FOR SQL Sctipt generation
Note: do not run Update-Database, run below command

```bash
script-migration
```

## Migration In VS Code.

#### Note: Before running steps command, install dotnet ef tool by below command.

```bash
dotnet tool install --global dotnet-ef --version 6.0.9
```
Verify Installation
```bash
dotnet ef
```

#### Note: Run command in the soultion directory ex: \KiranaStore\Backend\Store.WebServer.
# Step 1
```bash
dotnet ef migrations add IntialSQLDB --project store.data.logic --startup-project store.webapi -o ".\Migrations\SQL"
```
Note SET default project DATA\Store.Data.Login in package manage console. and then run the above command.
# Step 2
```bash
dotnet ef database update --project store.data.logic --startup-project store.webapi
```


## Database drop in Azure data studio.
```bash
USE master;
GO

ALTER DATABASE Store SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE Store;
GO
```

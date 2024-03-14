# Hangfire Demo

Quick demo of using hangfire with Mongo!

📝 There are three kinds of jobs present, an on demand job that can be triggered via `JobsController`, an on demand scheduled job that can be triggered via `JobsController`, and a hourly recurring job registered during the application startup.

## Spinning up a docker instance of Mongo

- x86: `docker run -d -p 27017:27017 --name hangfire-demo-mongo -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=password mongo`
- arm: `docker run -d -p 27017:27017 --name hangfire-demo-mongo -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=password arm64v8/mongo`

## Spinning up the service

- Set your connection string, `SchedulerDb`, and preferences in your secrets according to how the mongo instance was setup
  ```json
  {
    "ConnectionStrings": {
      "SchedulerDb": "mongodb://localhost:27017"
    },
    "HangfireConfig": {
      "DatabaseConnectionKey": "SchedulerDb",
      "DatabaseName": "SchedulerDb",
      "ServerName": "HangfireDemo.Scheduler",
      "AutomaticRetryAttempts": 3,
      // TailNotificationsCollection: single instance
      // Watch: replica set
      "CheckQueuedJobsStrategy": "TailNotificationsCollection",
      "CheckConnection": true,
      "DashboardPath": "/hangfire",
      "DashboardTitle": "Hangfire Dashboard"
    },
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "AllowedHosts": "*"
  }
  ```
- VS Code: use the included profile
- cli: `dotnet run --project ./HangfireDemo/HangfireDemo.csproj --urls https://localhost:7282`

## Try it out!

- Swagger: https://localhost:7282/swagger
- Hangfire Dashboard: https://localhost:7282/hangfire

## References

- Mongo: https://www.mongodb.com/docs/drivers/csharp/current/
- Hangfire: https://docs.hangfire.io/en/latest/

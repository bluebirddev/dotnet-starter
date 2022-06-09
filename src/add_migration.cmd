set /p migration=""
dotnet ef migrations add %migration% --project Bluebird.Core.Starter.Repository.PostgresSql --startup-project Bluebird.Core.Starter.Api
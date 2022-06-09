# Publish dotnet project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

RUN --mount=type=secret,id=MANDATES_PACKAGES_GITHUB_PAT MANDATES_PACKAGES_GITHUB_PAT=`cat /run/secrets/MANDATES_PACKAGES_GITHUB_PAT` && dotnet nuget add source --username DeltaDeveloperAccount --password $MANDATES_PACKAGES_GITHUB_PAT --store-password-in-clear-text --name github "https://nuget.pkg.github.com/The-Delta-Studio/index.json"

WORKDIR /app

COPY src/*.sln .
COPY src/Payce.LoadTesting.Api/*.csproj ./Payce.LoadTesting.Api/
COPY src/Payce.LoadTesting.Common/*.csproj ./Payce.LoadTesting.Common/
COPY src/Payce.LoadTesting.Domain/*.csproj ./Payce.LoadTesting.Domain/
COPY src/Payce.LoadTesting.Gateway.AWS.Cognito/*.csproj ./Payce.LoadTesting.Gateway.AWS.Cognito/
COPY src/Payce.LoadTesting.Gateway.Payce.Core.Mandates/*.csproj ./Payce.LoadTesting.Gateway.Payce.Core.Mandates/
COPY src/Payce.LoadTesting.Gateway.Payce.InputFileBatcher.Absa.Debicheck/*.csproj ./Payce.LoadTesting.Gateway.Payce.InputFileBatcher.Absa.Debicheck/
COPY src/Payce.LoadTesting.Gateway.Payce.QueuePopulator.Absa.Debicheck/*.csproj ./Payce.LoadTesting.Gateway.Payce.QueuePopulator.Absa.Debicheck/
COPY src/Payce.LoadTesting.Gateway.Payce.Recurring.Collector/*.csproj ./Payce.LoadTesting.Gateway.Payce.Recurring.Collector/
COPY src/Payce.LoadTesting.Repository.PostgresSql/*.csproj ./Payce.LoadTesting.Repository.PostgresSql/
COPY src/Payce.LoadTesting.Test/*.csproj ./Payce.LoadTesting.Test/

COPY src/Payce.LoadTesting.Api/. ./Payce.LoadTesting.Api/
COPY src/Payce.LoadTesting.Common/. ./Payce.LoadTesting.Common/
COPY src/Payce.LoadTesting.Domain/. ./Payce.LoadTesting.Domain/
COPY src/Payce.LoadTesting.Gateway.AWS.Cognito/. ./Payce.LoadTesting.Gateway.AWS.Cognito/
COPY src/Payce.LoadTesting.Gateway.Payce.Core.Mandates/. ./Payce.LoadTesting.Gateway.Payce.Core.Mandates/
COPY src/Payce.LoadTesting.Gateway.Payce.InputFileBatcher.Absa.Debicheck/. ./Payce.LoadTesting.Gateway.Payce.InputFileBatcher.Absa.Debicheck/
COPY src/Payce.LoadTesting.Gateway.Payce.QueuePopulator.Absa.Debicheck/. ./Payce.LoadTesting.Gateway.Payce.QueuePopulator.Absa.Debicheck/
COPY src/Payce.LoadTesting.Gateway.Payce.Recurring.Collector/. ./Payce.LoadTesting.Gateway.Payce.Recurring.Collector/
COPY src/Payce.LoadTesting.Repository.PostgresSql/. ./Payce.LoadTesting.Repository.PostgresSql/
COPY src/Payce.LoadTesting.Test/. ./Payce.LoadTesting.Test/

WORKDIR /app
RUN dotnet publish Payce.LoadTesting.Api/Payce.LoadTesting.Api.csproj -c Release -o out

# Build runtime env
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime-env

RUN adduser \
  --disabled-password \
  --home /app \
  --gecos '' app \
  && chown -R app /app
USER app

WORKDIR /app

COPY --from=build-env /app/out ./

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 \
  DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Payce.LoadTesting.Api.dll"]
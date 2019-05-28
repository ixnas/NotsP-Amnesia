FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY *.sln ./
COPY . ./
RUN dotnet restore

COPY . ./
WORKDIR /app/Amnesia.WebApi
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/Amnesia.WebApi/out ./
ENTRYPOINT [ "dotnet", "Amnesia.WebApi.dll" ]
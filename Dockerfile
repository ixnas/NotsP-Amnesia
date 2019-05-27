FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publisch -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /App
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "Amnesia.dll" ]

//in dockerignore
bin\
obj\
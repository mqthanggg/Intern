FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish YarpGateway.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /src
RUN apt-get update && apt-get install -y curl
COPY --from=build /app/publish/ .
ENTRYPOINT [ "dotnet", "YarpGateway.dll" ]
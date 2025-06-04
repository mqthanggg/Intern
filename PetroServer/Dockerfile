FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
RUN apt-get update && apt-get install -y curl
WORKDIR /source

COPY ../*.sln .
COPY PetroServer/*.csproj ./PetroServer/
RUN dotnet restore

COPY PetroServer/. ./PetroServer/
WORKDIR /source/PetroServer
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5170
ENTRYPOINT ["dotnet", "PetroServer.dll"]
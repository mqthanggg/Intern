FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish PetroServer.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /src
RUN apt-get update && apt-get install -y curl
COPY --from=build /app/publish/ .
COPY --from=build /src/certs ./certs
COPY --from=build /src/rsa_key ./rsa_key
ENTRYPOINT [ "dotnet", "PetroServer.dll" ]
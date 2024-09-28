ARG runtime="mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim"
ARG sdk="mcr.microsoft.com/dotnet/sdk:8.0"

FROM ${sdk} AS build

WORKDIR /src

COPY ["Directory.Build.props", "./"]
COPY ["Directory.Build.targets", "./"]

COPY ["Common/Applications/AppServices/.", "Common/Applications/AppServices/"]
COPY ["Common/Applications/Handlers/.", "Common/Applications/Handlers/"]
COPY ["Common/Cqrs/.", "Common/Cqrs/"]
COPY ["Common/Contracts/.", "Common/Contracts/"]
COPY ["Common/Clients/.", "Common/Clients/"]
COPY ["Common/Hosts/Api/.", "Common/Hosts/Api/"]
COPY ["Common/Hosts/Common/.", "Common/Hosts/Common/"]
COPY ["Common/Domain/.", "Common/Domain/"]
COPY ["Common/Infrastructure/.", "Common/Infrastructure/"]

COPY ["Notification/Contracts/.", "Notification/Contracts/"]
COPY ["Notification/Clients/.", "Notification/Clients/"]

COPY ["Security/Applications/Handlers/.", "Security/Applications/Handlers/"]
COPY ["Security/Applications/AppServices/.", "Security/Applications/AppServices/"]
COPY ["Security/Contracts/.", "Security/Contracts/"]
COPY ["Security/Clients/.", "Security/Clients/"]
COPY ["Security/Hosts/Public/.", "Security/Hosts/Public/"]

RUN dotnet restore "./Security/Hosts/Public/InsuranceGoSmoke.Security.Public.Hosts.Api.csproj"

WORKDIR "/src/Security/Hosts/Public"
RUN dotnet build "./InsuranceGoSmoke.Security.Public.Hosts.Api.csproj" -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "./InsuranceGoSmoke.Security.Public.Hosts.Api.csproj" -o /app/publish /p:UseAppHost=false

FROM ${runtime} AS final


RUN apt-get -y update; apt-get -y install curl
RUN useradd -ms /bin/bash nonroot
USER nonroot

EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsuranceGoSmoke.Security.Public.Hosts.Api.dll"]
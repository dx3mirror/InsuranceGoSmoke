ARG runtime="mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim"
ARG sdk="mcr.microsoft.com/dotnet/sdk:8.0"

FROM ${sdk} AS build

WORKDIR /src

COPY ["Directory.Build.props", "./"]
COPY ["Directory.Build.targets", "./"]

COPY ["Common/Hosts/Common/.", "Common/Hosts/Common/"]
COPY ["Common/Hosts/Consumers/.", "Common/Hosts/Consumers/"]
COPY ["Common/Contracts/.", "Common/Contracts/"]
COPY ["Common/Clients/.", "Common/Clients/"]

COPY ["Notification/Contracts/.", "Notification/Contracts/"]
COPY ["Notification/Clients/.", "Notification/Clients/"]
COPY ["Notification/Hosts/Consumers/.", "Notification/Hosts/Consumers/"]

RUN dotnet restore "./Notification/Hosts/Consumers/Kortros.Notification.Consumers.csproj"

WORKDIR "/src/Notification/Hosts/Consumers"
RUN dotnet build "./InsuranceGoSmoke.Notification.Consumers.csproj" -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "./InsuranceGoSmoke.Notification.Consumers.csproj" -o /app/publish /p:UseAppHost=false

FROM ${runtime} AS final


RUN apt-get -y update; apt-get -y install curl
RUN useradd -ms /bin/bash nonroot
USER nonroot

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsuranceGoSmoke.Notification.Consumers.dll"]
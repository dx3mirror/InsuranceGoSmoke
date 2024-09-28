ARG runtime="mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim"
ARG sdk="mcr.microsoft.com/dotnet/sdk:8.0"

FROM ${sdk} AS build

WORKDIR /src

COPY ["Directory.Build.props", "./"]
COPY ["Directory.Build.targets", "./"]

COPY ["Common/Contracts/.", "Common/Contracts/"]

COPY ["Static/.", "Static/"]

RUN dotnet restore "./Static/Hosts/InsuranceGoSmoke.Static.csproj"

WORKDIR "/src/Static/Hosts"
RUN dotnet build "./InsuranceGoSmoke.Static.csproj" -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "./InsuranceGoSmoke.Static.csproj" -o /app/publish /p:UseAppHost=false

FROM ${runtime} AS final

RUN apt-get -y update; apt-get -y install curl
RUN useradd -ms /bin/bash nonroot
USER nonroot

EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsuranceGoSmoke.Static.dll"]
ARG runtime="mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim"
ARG sdk="mcr.microsoft.com/dotnet/sdk:8.0"

FROM ${sdk} AS build

WORKDIR /src

COPY ["Directory.Build.props", "./"]
COPY ["Directory.Build.targets", "./"]

COPY ["Common/Contracts/.", "Common/Contracts/"]

COPY ["Static/Hosts/.", "Static/Hosts/"]

RUN dotnet restore "./Static/Hosts/Kortros.Static.csproj"

WORKDIR "/src/Static"
RUN dotnet build "./Hosts/Kortros.Static.csproj" -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "./Hosts/Kortros.Static.csproj" -o /app/publish /p:UseAppHost=false

FROM ${runtime} AS final

# вынести в отдельный образ, чтобы не дублировать в каждом Dockerfile
RUN apt-get -y update; apt-get -y install curl
RUN useradd -ms /bin/bash nonroot
USER nonroot

EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kortros.Static.dll"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base


ENV HOME /home/jenkins
# Create a non-root user
RUN useradd -m -d /home/jenkins jenkins

# Set the user for subsequent commands
USER jenkins

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["nzWorks.API/nzWorks.API.csproj", "nzWorks.API/"]
RUN dotnet restore "nzWorks.API/nzWorks.API.csproj"
COPY . .
WORKDIR "/src/nzWorks.API"
RUN dotnet build "nzWorks.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "nzWorks.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "nzWorks.API.dll"]

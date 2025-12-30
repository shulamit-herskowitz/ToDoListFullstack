# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["TodoApi.csproj", "./"]
RUN dotnet restore "TodoApi.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "TodoApi.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Expose ports
EXPOSE 80
EXPOSE 443

# Copy published app
COPY --from=publish /app/publish .

# Set environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "TodoApi.dll"]

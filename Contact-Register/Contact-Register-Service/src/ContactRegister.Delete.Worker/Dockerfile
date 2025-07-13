# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY *.sln .
COPY src/ContactRegister.Delete.Worker/ src/ContactRegister.Delete.Worker/
COPY src/ContactRegister.Application/ src/ContactRegister.Application/
COPY src/ContactRegister.Domain/ src/ContactRegister.Domain/
COPY src/ContactRegister.Infrastructure/ src/ContactRegister.Infrastructure/
COPY shared/ContactRegister.Shared/ shared/ContactRegister.Shared/

# Restore dependencies
RUN dotnet restore src/ContactRegister.Delete.Worker/ContactRegister.Delete.Worker.csproj

# Build the application
WORKDIR /app/src/ContactRegister.Delete.Worker
RUN dotnet publish ContactRegister.Delete.Worker.csproj -c Release -o out --no-restore

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/src/ContactRegister.Delete.Worker/out ./

# Environment variables
ENV ASPNETCORE_HTTP_PORTS=80
ENV ASPNETCORE_ENVIRONMENT=Release

# Expose port 80
EXPOSE 80 443

# Start the application
ENTRYPOINT ["dotnet", "ContactRegister.Delete.Worker.dll"]

# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY *.sln .
COPY src/ContactRegister.Update.Worker/ ./src/ContactRegister.Update.Worker/
COPY src/ContactRegister.Application/ ./src/ContactRegister.Application/
COPY src/ContactRegister.Domain/ ./src/ContactRegister.Domain/
COPY src/ContactRegister.Infrastructure/ ./src/ContactRegister.Infrastructure/
COPY shared/ContactRegister.Shared/ ./shared/ContactRegister.Shared/

# Restore dependencies
RUN dotnet restore ./src/ContactRegister.Update.Worker/ContactRegister.Update.Worker.csproj

# Build the application
WORKDIR /app/src/ContactRegister.Update.Worker
RUN dotnet publish ContactRegister.Update.Worker.csproj -c Release -o out --no-restore

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/src/ContactRegister.Update.Worker/out ./

# Environment variables
ENV ASPNETCORE_HTTP_PORTS=80
ENV ASPNETCORE_ENVIRONMENT=Release

# Expose port 80
EXPOSE 80 443

# Start the application
ENTRYPOINT ["dotnet", "ContactRegister.Update.Worker.dll"]

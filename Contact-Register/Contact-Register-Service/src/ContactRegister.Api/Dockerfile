﻿# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY *.sln .
COPY Contact-Register-Service/src/ContactRegister.Domain/ ./Contact-Register-Service/src/ContactRegister.Domain/
COPY Contact-Register-Service/src/ContactRegister.Application/ ./Contact-Register-Service/src/ContactRegister.Application/
COPY Contact-Register-Service/src/ContactRegister.Infrastructure/ ./Contact-Register-Service/src/ContactRegister.Infrastructure/
COPY Contact-Register-Service/src/ContactRegister.Api/ ./Contact-Register-Service/src/ContactRegister.Api/
COPY Contact-Register-Service/shared/ContactRegister.Shared/ ./Contact-Register-Service/shared/ContactRegister.Shared/

# Restore dependencies
RUN dotnet restore ./Contact-Register-Service/src/ContactRegister.Api/ContactRegister.Api.csproj

# Build the application
WORKDIR /app/Contact-Register-Service/src/ContactRegister.Api
RUN dotnet publish ContactRegister.Api.csproj -c Release -o out --no-restore

## Run migrations
# RUN dotnet tool install --version 8 --global dotnet-ef
# ENV PATH="$PATH:/root/.dotnet/tools"
# ENTRYPOINT dotnet-ef database update

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/src/ContactRegister.Api/out ./

# Environment variables
ENV ASPNETCORE_HTTP_PORTS=80

# Expose port 80
EXPOSE 80 443

# Start the application
ENTRYPOINT ["dotnet", "ContactRegister.Api.dll"]

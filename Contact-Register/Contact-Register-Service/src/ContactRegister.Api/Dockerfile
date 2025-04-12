# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY *.sln .
COPY src/ContactRegister.Domain/*.csproj ./src/ContactRegister.Domain/
COPY src/ContactRegister.Application/*.csproj ./src/ContactRegister.Application/
COPY src/ContactRegister.Infrastructure/*.csproj ./src/ContactRegister.Infrastructure/
COPY src/ContactRegister.Api/*.csproj ./src/ContactRegister.Api/
COPY shared/ContactRegister.Shared/*.csproj ./src/ContactRegister.Shared/

# Restore dependencies
RUN dotnet restore ./src/ContactRegister.Api/ContactRegister.Api.csproj

# Copy the entire source code
COPY . .

# Build the application
WORKDIR /app/src/ContactRegister.Api
RUN dotnet publish ContactRegister.Api.csproj -c Release -o out

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

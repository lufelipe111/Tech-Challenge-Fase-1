# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY *.sln .
COPY src/Contact.Register.Domain/*.csproj ./src/Contact.Register.Domain/
COPY src/Contact.Register.Application/*.csproj ./src/Contact.Register.Application/
COPY src/Contact.Register.Infrastructure/*.csproj ./src/Contact.Register.Infrastructure/
COPY src/Contact.Register.Api/*.csproj ./src/Contact.Register.Api/
COPY shared/Contact.Register.Shared/*.csproj ./src/Contact.Register.Shared/

# Restore dependencies
RUN dotnet restore ./src/Contact.Register.Api/Contact.Register.Api.csproj

# Copy the entire source code
COPY . .

# Build the application
WORKDIR /app/src/Contact.Register.Api
RUN dotnet publish Contact.Register.Api.csproj -c Release -o out

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/src/Contact.Register.Api/out ./

# Environment variables
ENV ASPNETCORE_HTTP_PORTS=80

# Expose port 80
EXPOSE 80 443

# Start the application
ENTRYPOINT ["dotnet", "Contact.Register.Api.dll"]

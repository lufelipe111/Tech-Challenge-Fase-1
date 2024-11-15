# How to Run the Application

This section provides instructions on how to build and run the application using Docker and Docker Compose. By following these steps, you'll have the application and an SQL Server database running in containers on your local machine.

## Prerequisites

- **Docker**: Ensure that Docker is installed and running on your system.
    - [Install Docker Desktop](https://www.docker.com/products/docker-desktop) for your operating system.
- **Docker Compose**: Comes bundled with Docker Desktop. No additional installation is required.

## Steps to Run the Application

### 1. Clone the Repository

First, clone the repository to your local machine:

```bash
git clone https://github.com/lufelipe111/Tech-Challenge-Fase-1.git
```

### 2. Navigate to the Project Directory

Change your working directory to the root of the cloned repository:

```bash
cd Contact-Register
```

### 3. Build and Run with Docker Compose

Use Docker Compose to build the Docker images and start the containers:

```bash
docker-compose up --build
```

This command does the following:

- **Builds** the Docker image for the application using the provided `Dockerfile`.
- **Starts** the containers defined in the `docker-compose.yml` file:
    - **Application Container**: Runs the .NET 8 API.
    - **SQL Server Container**: Hosts the SQL Server database.

### 4. Verify the Containers are Running

Once Docker Compose finishes building and starting the containers, you should see logs indicating that both the application and SQL Server are running.

To list the running containers, open a new terminal window and run:

```bash
docker ps
```

You should see entries for both the application and the SQL Server containers.

### 5. Access the Application

The application documentation should now be accessible at `http://localhost:8080/swagger`.

- **API Endpoints**: You can interact with the API endpoints using tools like `curl`, Postman, or your web browser.

  For example, to get all contacts:

  ```bash
  curl -X 'GET' 'http://localhost:8080/Contact/Get' -H 'accept: */*'
  ```

### 6. Stopping the Application

To stop the containers, press `Ctrl+C` in the terminal where Docker Compose is running. Then, to ensure all containers are stopped and resources are cleaned up, run:

```bash
docker-compose down
```

This command stops and removes the containers, networks, and volumes created by `docker-compose up`.

## Additional Commands and Information

### Running in Detached Mode

To run the containers in the background (detached mode), use:

```bash
docker-compose up --build -d
```

### Rebuilding the Images

If you make changes to the code and need to rebuild the images, run:

```bash
docker-compose up --build
```

### Viewing Logs

To view the logs from the running containers:

```bash
docker-compose logs -f
```

To view logs for a specific service (e.g., the application):

```bash
docker-compose logs -f Contact.Register.Api
```

### Accessing the SQL Server Database

You can connect to the SQL Server instance using SQL Server Management Studio (SSMS) or any other SQL client.

- **Server**: `localhost,1433`
- **Username**: `sa`
- **Password**: `Password_123`

Ensure that the SQL client is configured to allow connections to localhost on port 1433.

### Applying Database Migrations Manually

If you need to apply migrations manually, you can execute the following commands inside the application container.

First, get the container ID or name:

```bash
docker ps
```

Then, access the container's shell:

```bash
docker exec -it <container_id_or_name> /bin/bash
```

Once inside the container, navigate to the application's directory and run:

```bash
dotnet ef database update
```

### Cleaning Up Docker Resources

To remove all stopped containers, unused networks, and dangling images, you can run:

```bash
docker system prune
```

## Troubleshooting

- **Port Conflicts**: If port `5000` is already in use, you can change the port mapping in the `docker-compose.yml` file:

  ```yaml
  services:
    Contact.Register.Api:
      ports:
        - "<your_desired_port>:80"
  ```

  Replace `your_desired_port` with an available port number.

- **Database Connection Issues**: Ensure that the SQL Server container is running and healthy. Check the logs for any errors:

  ```bash
  docker-compose logs sqlserver
  ```

- **Docker Not Running**: Ensure that Docker Desktop is running. The Docker daemon must be active to build images and run containers.

- **Permission Issues on Linux/macOS**: You might need to adjust file permissions or run commands with `sudo` if you encounter permission errors.

## Optional: Running Without Docker

If you prefer to run the application without Docker, follow these steps:

### Prerequisites

- **.NET 8 SDK**: Install from the [.NET Download page](https://dotnet.microsoft.com/download/dotnet/8.0).
- **SQL Server**: Install SQL Server locally or use a remote instance.
- **EF Core Tools**: Install the Entity Framework Core tools:

  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Steps

1. **Set Up the Database**:

    - Update the `ConnectionStrings` in `appsettings.json` or `appsettings.Development.json` with your SQL Server instance details.
    - Apply migrations to create the database schema:

      ```bash
      dotnet ef database update --project src/Contact.Register.Infrastructure --startup-project src/Contact.Register.Api
      ```

2. **Restore Dependencies**:

   ```bash
   dotnet restore
   ```

3. **Build the Solution**:

   ```bash
   dotnet build
   ```

4. **Run the Application**:

   ```bash
   dotnet run --project src/Contact.Register.Api
   ```

5. **Access the Application**:

   The application will be running at `https://localhost:5001` or `http://localhost:5000` by default.

## Notes

- **Environment Variables**: When running without Docker, ensure that any required environment variables are set or configured in the `appsettings` files.
- **HTTPS Configuration**: By default, the application runs with HTTPS enabled when not using Docker. You might need to trust the local development certificate. For more information, see [Enforce HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl).

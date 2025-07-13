@echo off
echo --- Starting Full Stack Deployment to Kubernetes ---

echo.
echo --- Building Docker Images ---
docker build -t contact-register-api -f "../Contact-Register-Service/src/ContactRegister.Api/Dockerfile" "../Contact-Register-Service" || exit /b
docker build -t worker-storage -f "../Contact-Register-Service/src/ContactRegister.Storage.Worker/Dockerfile" "../Contact-Register-Service" || exit /b
docker build -t worker-update -f "../Contact-Register-Service/src/ContactRegister.Update.Worker/Dockerfile" "../Contact-Register-Service" || exit /b
docker build -t worker-delete -f "../Contact-Register-Service/src/ContactRegister.Delete.Worker/Dockerfile" "../Contact-Register-Service" || exit /b

echo.
echo --- Loading Docker Images into Kind Cluster ---
kind load docker-image contact-register-api --name fiap-tech-challenge-2025 || exit /b
kind load docker-image worker-storage --name fiap-tech-challenge-2025 || exit /b
kind load docker-image worker-update --name fiap-tech-challenge-2025 || exit /b
kind load docker-image worker-delete --name fiap-tech-challenge-2025 || exit /b

echo.
echo --- Applying Kubernetes Manifests ---
echo --- Applying Persistent Volume Claims ---
kubectl apply -f mssql-pvc.yaml || exit /b
kubectl apply -f rabbitmq-pvc.yaml || exit /b
kubectl apply -f prometheus-pvc.yaml || exit /b
kubectl apply -f grafana-pvc.yaml || exit /b

echo.
echo --- Applying Configurations and Deployments ---
kubectl apply -f secrets.yaml || exit /b
kubectl apply -f mssql-deployment.yaml || exit /b
kubectl apply -f rabbitmq-deployment.yaml || exit /b
kubectl apply -f prometheus-deployment.yaml || exit /b
kubectl apply -f grafana-dashboard-config.yaml || exit /b
kubectl apply -f grafana-deployment.yaml || exit /b
kubectl apply -f api-deployment.yaml || exit /b
kubectl apply -f worker-storage-deployment.yaml || exit /b
kubectl apply -f worker-update-deployment.yaml || exit /b
kubectl apply -f worker-delete-deployment.yaml || exit /b

echo.
echo --- Deployment Complete ---
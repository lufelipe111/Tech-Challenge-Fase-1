#!/bin/bash
set -e

echo "--- Starting Full Stack Deployment to Kubernetes ---"

echo
echo "--- Building Docker Images ---"
docker build -t contact-register-api -f "../Contact-Register-Service/src/ContactRegister.Api/Dockerfile" "../Contact-Register-Service"
docker build -t worker-storage -f "../Contact-Register-Service/src/ContactRegister.Storage.Worker/Dockerfile" "../Contact-Register-Service"
docker build -t worker-update -f "../Contact-Register-Service/src/ContactRegister.Update.Worker/Dockerfile" "../Contact-Register-Service"
docker build -t worker-delete -f "../Contact-Register-Service/src/ContactRegister.Delete.Worker/Dockerfile" "../Contact-Register-Service"

echo
echo "--- Loading Docker Images into Kind Cluster ---"
kind load docker-image contact-register-api --name fiap-tech-challenge-2025
kind load docker-image worker-storage --name fiap-tech-challenge-2025
kind load docker-image worker-update --name fiap-tech-challenge-2025
kind load docker-image worker-delete --name fiap-tech-challenge-2025

echo
echo "--- Applying Kubernetes Manifests ---"
echo "--- Applying Persistent Volume Claims ---"
kubectl apply -f mssql-pvc.yaml
kubectl apply -f rabbitmq-pvc.yaml
kubectl apply -f prometheus-pvc.yaml
kubectl apply -f grafana-pvc.yaml

echo
echo "--- Applying Configurations and Deployments ---"
kubectl apply -f secrets.yaml
kubectl apply -f mssql-deployment.yaml
kubectl apply -f rabbitmq-deployment.yaml
kubectl apply -f prometheus-deployment.yaml
kubectl apply -f grafana-dashboard-config.yaml
kubectl apply -f grafana-deployment.yaml
kubectl apply -f api-deployment.yaml
kubectl apply -f worker-storage-deployment.yaml
kubectl apply -f worker-update-deployment.yaml
kubectl apply -f worker-delete-deployment.yaml

echo
echo "--- Deployment Complete ---"
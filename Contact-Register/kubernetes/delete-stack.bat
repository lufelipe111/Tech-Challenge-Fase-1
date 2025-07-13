@echo off
echo --- Deleting Full Stack from Kubernetes ---

echo.
echo --- Deleting Kubernetes Deployments, Services, and Configs ---
rem Note: This script does NOT delete PersistentVolumeClaims (PVCs) to prevent data loss.
rem If you want to delete the data and start fresh, you must manually delete the PVCs:
rem kubectl delete pvc mssql-pvc rabbitmq-pvc prometheus-pvc grafana-pvc

kubectl delete -f worker-delete-deployment.yaml
kubectl delete -f worker-update-deployment.yaml
kubectl delete -f worker-storage-deployment.yaml
kubectl delete -f api-deployment.yaml
kubectl delete -f grafana-deployment.yaml
kubectl delete -f grafana-dashboard-config.yaml
kubectl delete -f prometheus-deployment.yaml
kubectl delete -f rabbitmq-deployment.yaml
kubectl delete -f mssql-deployment.yaml
kubectl delete -f secrets.yaml

echo.
echo --- Deletion Complete ---
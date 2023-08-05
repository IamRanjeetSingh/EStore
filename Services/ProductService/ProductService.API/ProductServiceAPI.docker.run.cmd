echo Running ProductService.API docker image
docker run ^
--detach ^
--publish-all ^
--net EStoreNetwork ^
--name product-service-api ^
product-service-api
pause

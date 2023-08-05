echo Running UserServiceAPI docker image
docker run ^
--detach ^
--publish-all ^
--net EStoreNetwork ^
--name user-service-api ^
user-service-api
pause

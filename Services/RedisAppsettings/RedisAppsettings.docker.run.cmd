echo Running RedisAppsettings docker image
docker run ^
--detach ^
--publish-all ^
--volume "C:/Users/harsisin/source/myRepos/EStore/Services/RedisAppsettings/data":"/data" ^
--volume "C:/Users/harsisin/source/myRepos/EStore/Services/RedisAppsettings/shared":"/shared" ^
--net EStoreNetwork ^
--name redis-appsettings ^
redis-appsettings ^
bash -c "redis-server --appendonly yes --dir /data"
pause

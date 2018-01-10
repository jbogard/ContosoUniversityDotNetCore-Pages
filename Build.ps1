
docker-compose -f "docker-compose.ci.yml" up -d --remove-orphans

docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" build


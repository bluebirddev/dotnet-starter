#!/bin/bash

echo 'Starting Docker DB script'

if [ "$(docker ps -q -f name=dev-postgres)" ]
then
    echo 'Docker postgres container already running'
else
    docker run -d --rm \
            --name dev-postgres \
            -e POSTGRES_USER=postgres \
            -e POSTGRES_PASSWORD=password \
            -e POSTGRES_DB=starter \
            -p 5432:5432 \
            postgres:14.1
fi

while ! docker exec -u postgres dev-postgres sh -c 'psql; exit $?' >/dev/null 2>&1; do
  echo 'Waiting for DB to start accepting connections...' 
  sleep 1
done

echo 'DB port opened'
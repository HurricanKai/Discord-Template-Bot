#!/bin/bash
# !! Run this script from the build directory !!

cd ../*.Bot || echo 'ERROR: Could not find .Bot project.'
NAME=$(jq '.Build.DockerName' botSettings.json | tr -d '"')

cd .. 

if [ "$(docker ps -f name="$NAME")" ];
  then 
    echo 'Stopping Docker container.'
    docker container stop "$NAME"
    
    echo 'Removing Docker container.'
    docker container rm "$NAME"
fi

echo 'Building Docker image.'
docker build -t "$NAME" -f ./Dockerfile .

echo 'Running Docker process in a container.'
docker run --network host --restart on-failure:5 --name "$NAME" -d "$NAME"
#!/bin/bash
# !! Run this script from the build directory !!

cd ../*.Bot || echo 'ERROR: Could not find .Bot project.'
NAME=$(jq '.Build.DockerName' botSettings.json)

cd .. || echo 'ERROR: Dockerfile not found.'
FILE=../Dockerfile

echo 'Trying to find Dockerfile'
if [ -f "$FILE" ]; 
  then
    echo 'Found Dockerfile, navigating to directory.'
    cd ..
  else
    echo 'Could not find Dockerfile. Please navigate to the build directory and run this script.' 
    exit
fi

if [ "$(docker ps -f name="$(NAME)")" ];
  then 
    echo 'Stopping Docker container.'
    docker container stop "$(NAME)"
    
    echo 'Removing Docker container.'
    docker container rm "$(NAME)"
fi

echo 'Building Docker image.'
docker build -t "$NAME" -f ./Dockerfile .

echo 'Running Docker process in a container.'
docker run --network host --restart on-failure:5 --name "$(NAME)" -d "$(NAME)"
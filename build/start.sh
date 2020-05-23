#!/bin/bash
# !! Run this script from the build directory !!

FILE=../Dockerfile
NAME="template"

echo 'Trying to find Dockerfile'
if [ -f "$FILE" ]; 
  then
    echo 'Found Dockerfile, navigating to directory.'
    cd ..
  else
    echo 'Could not find Dockerfile. Please navigate to the build directory and run this script.' 
    exit
fi

if [ "$(docker inspect -f '{{.State.Running}}' "$NAME")" == "true" ];
  then
    echo 'Stopping existing Docker containers.'
    docker container stop $NAME
    
    echo 'Removing existing Docker containers.'
    docker container rm $NAME
fi

if [ "$(docker ps -q -f name=$NAME)" ]; 
  then
    docker container rm $NAME
fi

echo 'Building Docker image.'
docker build -t $NAME -f ./Dockerfile .

echo 'Running Docker process in a container.'
docker run --network host --restart on-failure:5 --name $NAME -d $NAME
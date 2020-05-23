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

echo 'Stopping Docker container.'
docker container stop $NAME
    
echo 'Removing Docker container.'
docker container rm $NAME

echo 'Building Docker image.'
docker build -t $NAME -f ./Dockerfile .

echo 'Running Docker process in a container.'
docker run --network host --restart on-failure:5 --name $NAME -d $NAME
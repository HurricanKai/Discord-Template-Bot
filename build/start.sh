#!/bin/bash

FILE=../Dockerfile

if [ -f "$FILE" ]; then
  cd ..
fi

# Stop any existing containers.
docker container stop template

# Remove any existing containers.
docker container rm template

# Build a new container called 
docker build -t template -f ./Dockerfile .

docker run --network host --restart on-failure:5 --name "template" -d template
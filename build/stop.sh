NAME="template"

if [ "$(docker inspect -f '{{.State.Running}}' "$NAME")" == "true" ];
  then
    echo 'Existing running Docker container found.'
    
    echo 'Stopping Docker container.'
    docker container stop $NAME
    
    echo 'Removing Docker container.'
    docker container rm $NAME
fi

if [ "$(docker ps -f name=$NAME)" ]; 
  then
    echo 'Non-running Docker container found.'
    echo 'Removing Docker container.'
    docker container rm $NAME
fi
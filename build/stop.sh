NAME="template"

if [ "$(docker ps -f name=$NAME)" ];
  then 
    echo 'Stopping Docker container.'
    docker container stop $NAME
    
    echo 'Removing Docker container.'
    docker container rm $NAME
fi
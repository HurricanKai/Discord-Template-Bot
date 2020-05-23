NAME="template"

echo 'Stopping Docker container.'
docker container stop $NAME
    
echo 'Removing Docker container.'
docker container rm $NAME
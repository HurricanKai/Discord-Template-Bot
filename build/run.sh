cd ..

docker container stop template

docker container rm template

docker build -t template -f ./Dockerfile .

docker run --network host --restart on-failure:5 --name "template" -d template
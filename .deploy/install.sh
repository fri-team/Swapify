#!/bin/bash

ENV_FILE=.env
REPO_NAME=Swapify

if [ ! -f "$ENV_FILE" ]; then
  echo "Environment file '$ENV_FILE' for docker container is missing."
  exit 1
fi

echo "Copying file '$ENV_FILE' to $REPO_NAME"
cp $ENV_FILE $REPO_NAME

if [ -d "$REPO_NAME" ]; then
  echo "Repository $REPO_NAME already exists. Pulling changes..."
  cd $REPO_NAME
  git pull
else
  echo "Repository $REPO_NAME does not exists. Cloning..."
  git clone "https://github.com/fri-team/$REPO_NAME.git"
  cd $REPO_NAME
fi

echo
echo "Building new version of docker containers..."
docker-compose up -d --build

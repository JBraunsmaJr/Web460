#!/bin/bash

BASE_CONTAINER_NAME="web460-jbraunsma"
ORIGINAL_IFS=$(IFS)

build_container() {
  project="$1"
  echo "Checking for Dockerfile in $project..."
    
    # Ensure a dockerfile exists inside directory
    if test -f "$project/Dockerfile"; then
        echo "Building Docker container for $project..."
        name=${project[-1]}
        current_tag="$BASE_CONTAINER_NAME-$name"
        docker build -f "$project/Dockerfile" -t "$current_tag" "$project"      
    fi
}

find $(pwd) -type d | while read -r F;
do
  build_container "$F"
done

IFS=$ORIGINAL_IFS
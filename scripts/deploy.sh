#!/bin/bash
set -e

AWS_ACCOUNT_ID="751505269853"
AWS_REGION="us-east-1"
REPO_NAME="myapp"

aws ecr get-login-password --region $AWS_REGION | docker login --username AWS --password-stdin $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com

docker pull $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$REPO_NAME:latest

docker stop myapp || true
docker rm myapp || true

docker run -d \
  --name myapp \
  -p 5000:5000 \
  --restart unless-stopped \
  $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$REPO_NAME:latest

#!/usr/bin/env sh

set -eu

sudo curl -L "https://github.com/docker/compose/releases/download/v2.1.1/docker-compose-linux-x86_64" -o /usr/local/bin/docker-compose
chmod +x /usr/local/bin/docker-compose

docker-compose -v
#!/usr/bin/env sh

set -eu

apk add --no-cache build-base
apk add --no-cache python3

# Add python pip and bash
apk add --no-cache py-pip bash

# Install docker-compose via pip
pip install --no-cache-dir docker-compose
docker-compose -v
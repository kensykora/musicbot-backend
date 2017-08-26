#!/bin/bash
if [[ $TRAVIS_PULL_REQUEST == "false" ]]; then
  scripts/az-group-deploy.sh -g $AZ_RG_BUILD-$TRAVIS_BUILD_NUMBER -l $AZ_LOCATION -a src/arm
fi
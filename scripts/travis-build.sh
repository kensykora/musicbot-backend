#!/bin/bash
if [[ $TRAVIS_PULL_REQUEST == 'false' ]]; then
    sudo apt-get -y install python3-pip python-dev
    echo 'deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ wheezy main' | sudo tee /etc/apt/sources.list.d/azure-cli.list
    sudo apt-key adv --keyserver packages.microsoft.com --recv-keys 417A0893
    sudo apt-get install apt-transport-https
    sudo apt-get update -qq
    sudo apt-get install azure-cli
    az login --service-principal -u $AZ_AD_SP_ID -p $AZ_AD_SP_PASS -t $AZ_AD_TENANT
fi
#!/bin/bash
if [ -z "$1" ]
then
  echo "Error: Missing required input parameter 1"
  exit 1
fi

if [ -z "$2" ]
then
  echo "Error: Missing required input parameter 2"
  exit 1
fi

if [ -z "$3" ]
then
  echo "Error: Missing required input parameter 3"
  exit 1
fi


host=$1
runtimes=$2
client=$3

echo "Host: "$host
echo "Runtimes: "$runtimes
echo "Client: "$client

dotnet run -c Release --framework $host --runtimes $runtimes --filter $client

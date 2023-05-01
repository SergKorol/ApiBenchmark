#!/bin/sh
host=$1
runtimes=$2
client=$3
if [ -z "$runtimes" ]; then
    runtimes="net7 net6 net5"
fi
dotnet run -project ApiBenchmark.BenchmarkTests.csproj -c Release -f $host --runtimes $runtimes --filter $client
#if [ -z "$client" ]; then
#    dotnet run -project ApiBenchmark.BenchmarkTests.csproj -c Release -f $host --runtimes $runtimes
#    else 
#    dotnet run -project ApiBenchmark.BenchmarkTests.csproj -c Release -f $host --runtimes $runtimes --filter $client
#fi

# This script is used to run the benchmarks in the ApiBenchmark.BenchmarkTests project.


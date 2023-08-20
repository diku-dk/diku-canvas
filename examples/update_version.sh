#!/usr/bin/env bash

if [ $# -lt 2  ]
then
    echo "$0 VERSION FILE
          You entered only $# parameters"
    exit 1
fi

awk -v version="$1" \
    '/nuget:DIKU.Canvas/{printf "#r \"nuget:DIKU.Canvas, %s\"\n", version} !/nuget:DIKU.Canvas/' $2 \
    > tmpfile && mv -f tmpfile $2

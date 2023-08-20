#!/usr/bin/env bash

if [ $# -lt 2  ]
then
    echo "$0 {jon|ken|clean} FILE
          You entered only $# parameters"
    exit 1
fi

case "${1}" in
    ken)
        awk 'NR == 1 {printf "#i \"nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release\"\n"}
             NR == 2 {printf "//#i \"nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/\"\n"}
             NR > 2' $2 > tmpfile && mv -f tmpfile $2;;
    jon)
        awk 'NR == 1 {printf "//#i \"nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release\"\n"}
             NR == 2 {printf "#i \"nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/\"\n"}
             NR > 2' $2 > tmpfile && mv -f tmpfile $2 ;;
    clean)
        awk 'NR > 2' $2 > tmpfile && mv -f tmpfile $2 ;;
esac

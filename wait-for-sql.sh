#!/bin/bash
# wait-for-sql.sh

set -e

host="$1"
shift
cmd="$@"

echo "Waiting for SQL Server at $host..."

until /opt/mssql-tools/bin/sqlcmd -S $host -U sa -P "YourStrong!Passw0rd" -Q "SELECT 1" &> /dev/null
do
  >&2 echo "SQL Server is unavailable - sleeping"
  sleep 2
done

>&2 echo "SQL Server is up - executing command"
exec $cmd

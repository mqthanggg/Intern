
echo "host replication all 0.0.0.0/0 md5" >> /var/lib/postgresql/data/pg_hba.conf

set -e
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    ALTER SYSTEM SET max_connections = 200;
    ALTER SYSTEM SET shared_buffers = '512MB';
EOSQL
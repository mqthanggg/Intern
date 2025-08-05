psql -At postgres://postgres:admin@database-primary:5432/Intern -c "
SELECT concat('\"', usename, '\" \"', passwd, '\"') FROM pg_shadow
" > /etc/pgbouncer/userlist.txt 
pgbouncer /etc/pgbouncer/pgbouncer.ini
#FROM ubuntu as pullsql
#WORKDIR /sql

FROM debian AS curl
RUN apt-get update && apt-get install -y curl
WORKDIR /curldata
RUN curl https://gist.githubusercontent.com/niklasstich/6f6502d72e30044ed7d3d3e12fae5c2b/raw/8974bb0d491c755fd143245093ce6d0618bf2dff/MediaContentUpdated.sql > populate.sql

FROM bitnami/mariadb
#COPY --from=pullsql populate.sql ./populate.sql
ENV MYSQL_DATABASE=mediacontent
ENV MARIADB_ROOT_PASSWORD=rootpasswd
COPY --from=curl /curldata/populate.sql /docker-entrypoint-initdb.d/populate.sql
EXPOSE 3306

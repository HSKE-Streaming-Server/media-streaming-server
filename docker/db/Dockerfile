#FROM ubuntu as pullsql
#WORKDIR /sql

FROM debian AS curl
RUN apt-get update && apt-get install -y curl
WORKDIR /curldata
RUN curl https://gist.githubusercontent.com/DominikWiesner/eae0105f8d881785f6539beba9b6eba0/raw/e6a83821563a2bb7b666f327ca02c50a6b60e738/mediacontentdb.sql > populate.sql

FROM bitnami/mariadb
#COPY --from=pullsql populate.sql ./populate.sql
ENV MYSQL_DATABASE=mediacontent
ENV MARIADB_ROOT_PASSWORD=rootpasswd
COPY --from=curl /curldata/populate.sql /docker-entrypoint-initdb.d/populate.sql
EXPOSE 3306

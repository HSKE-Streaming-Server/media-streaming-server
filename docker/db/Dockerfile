#FROM ubuntu as pullsql
#WORKDIR /sql

FROM debian AS curl
RUN apt-get update && apt-get install -y curl
WORKDIR /curldata
RUN curl https://gist.githubusercontent.com/DominikWiesner/571a858c927cc1a6f3275c939a503f05/raw/97f43550598778a4736fc2885d97f1bdd901c47f/mediacontent.sql > populate.sql
RUN curl https://gist.githubusercontent.com/niklasstich/cedf71bb4294808249da5f486cf12fdf/raw/ce7a72f141cb22b1ecf237f51805000cb7eafa7a/login.sql > login.sql

FROM bitnami/mariadb
ENV MYSQL_DATABASE=mediacontent
ENV MARIADB_ROOT_PASSWORD=rootpasswd
COPY --from=curl /curldata/*.sql /docker-entrypoint-initdb.d/
EXPOSE 3306

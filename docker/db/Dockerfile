FROM bitnami/mariadb
ENV MYSQL_DATABASE=mediacontent
ENV MARIADB_ROOT_PASSWORD=rootpasswd
USER root
RUN apt-get update && apt-get install -y curl
WORKDIR /docker-entrypoint-initdb.d
RUN curl https://gist.githubusercontent.com/DominikWiesner/acf6d8f723a0d2c359a133e03b52bc69/raw/e7ef274eeb7deff9d61be5c176b408e3318e8266/mediacontent.sql > populate.sql
RUN curl https://gist.githubusercontent.com/niklasstich/cedf71bb4294808249da5f486cf12fdf/raw/ce7a72f141cb22b1ecf237f51805000cb7eafa7a/login.sql > login.sql
RUN curl https://gist.githubusercontent.com/michaelpezke/bb0158dc9a95bb10d59b740f9bb0c9cf/raw/bb0ef5048e38d6245a2934aec7dd812dc0d76d8c/alreadytranscodedmpd.sql > alreadytranscodedmpd.sql
EXPOSE 3306

#FROM ubuntu as pullsql
#WORKDIR /sql


FROM mariadb:10.5
RUN apt-get update && apt-get install -y curl
#COPY --from=pullsql populate.sql ./populate.sql
ENV MYSQL_DATABASE=mediacontent
RUN curl https://gist.githubusercontent.com/niklasstich/6f6502d72e30044ed7d3d3e12fae5c2b/raw/8974bb0d491c755fd143245093ce6d0618bf2dff/MediaContentUpdated.sql --output /docker-entrypoint-initdb.d/populate.sql
EXPOSE 3306

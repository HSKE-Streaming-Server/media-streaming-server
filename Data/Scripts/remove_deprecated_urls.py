import pandas
import pymysql
import requests


def check_404(link):
    response = requests.head(url=link)
    print(link)
    print(response.status_code)
    if response.status_code != 404 and response.status_code != 403:
        print("False")
        return False
    else:
        print("True")
        return True

def remove_deprecated(id, connection):
    print("Removing id = "+ id)
    sql = "DELETE FROM `mediacontent` WHERE ID = %s;"
    connection.execute(sql, (str(id),))




connection = pymysql.connect(host='db',
                             user='root',
                             password='rootpasswd',
                             db='mediacontent',
                             charset='utf8mb4',
                             cursorclass=pymysql.cursors.DictCursor)

iddd = 'ec6eb85f1d17186490a272ace69f09c798c11d6f'
remove_deprecated(iddd, connection.cursor())

sql_query = "SELECT ID, Link FROM `mediacontent` WHERE Tunersource = 0"

df = pandas.read_sql(sql_query, connection)

df['is_deprecated'] = df.apply(lambda x: check_404(x['Link']), axis=1)

df_deprecated = df.loc[df['is_deprecated'] == True]


try:
    with connection.cursor() as cursor:

        print(str(len(df_deprecated.index)) + " Links are deprecated")
        df_deprecated = df_deprecated.apply(lambda x: remove_deprecated(x['ID'], cursor), axis=1)


finally:
    connection.commit()
    connection.close()
    print("finished")



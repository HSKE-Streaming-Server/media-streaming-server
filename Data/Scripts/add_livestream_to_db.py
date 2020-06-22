import requests
import pymysql
import hashlib

db_description_length = 350

url = 'https://mediathekviewweb.de/api/query'
headers = {"Content-Type": "text/plain"}
livestream = "livestream"
channels = ['ARD', 'ZDF','sat','arte.DE','phoenix','KiKA','ZDFtivi','BR','hr','mdr','NDR','ORF','rbb','SR','SRF','SWR','WDR']

connection = pymysql.connect(host='db',
                             user='root',
                             password='rootpasswd',
                             db='mediacontent',
                             charset='utf8mb4',
                             cursorclass=pymysql.cursors.DictCursor)

try:
    with connection.cursor() as cursor:
        for channel in channels:
            for i in range(1):
                data = {
                    "queries": [
                        {
                            "fields": [
                                "topic"
                            ],
                            "query": livestream
                        },
                        {
                            "fields": [
                                "channel"
                            ],
                            "query": '"' + channel + '"'
                        }
                    ],
                    "sortBy": "timestamp",
                    "sortOrder": "desc",
                    "future": False,
                    "offset": 0,
                    "size": 10 # was 4 before
                }

                response = requests.post(url=url, json=data, headers=headers)
                response_dict = response.json()

                print(channel + ": " + livestream)
                if response_dict['result']['queryInfo']['resultCount'] == 0:
                    continue


                for x in range(len(response_dict['result']['results'])):

                    # check if it really is a livestream
                    if int(response_dict['result']['results'][x]['timestamp']) != 0:
                        continue

                    mediacontent_id = str(hashlib.sha1((response_dict['result']['results'][x]['url_video']).encode('utf-8')).hexdigest())
                    sql = "SELECT count(*) FROM `mediacontent` WHERE ID = %s"

                    cursor.execute(sql, (mediacontent_id,))
                    id_check = cursor.fetchall()

                    # checks if the entry is a duplicate
                    if id_check[0]['count(*)'] == 1:
                        continue

                    if 'description' in response_dict['result']['results'][x]:
                        if not response_dict['result']['results'][x]['description']:
                            media_description = None
                        else:
                            media_description = str(response_dict['result']['results'][x]['description'])
                            if len(media_description) >= db_description_length:
                                media_description = str( media_description[:(db_description_length-3)] + '..')
                    else:
                        media_description = None

                    sql = "INSERT INTO `mediacontent` (`ID`, `Name`, `Category`, `Tunersource`, `LiveStream`, `Link`, `Description`) VALUES " \
                    "(%s, %s, %s, 0, 1, %s, %s)"

                    data_tuple = (mediacontent_id,
                                    str(response_dict['result']['results'][x]['title']),
                                    str(response_dict['result']['results'][x]['channel']),
                                    str(response_dict['result']['results'][x]['url_video']),
                                    media_description
                                  )

                    print(sql, data_tuple)
                    cursor.execute(sql, data_tuple)


finally:
    connection.commit()
    connection.close()







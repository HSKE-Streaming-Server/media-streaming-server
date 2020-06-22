import requests
import pymysql
import hashlib

db_description_length = 350

url = 'https://mediathekviewweb.de/api/query'
headers = {"Content-Type": "text/plain"}
abc = ['a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z']
channels = ['ARD','ZDF','sat','arte.DE','phoenix','KiKA','ZDFtivi','BR','hr','mdr','NDR','ORF','rbb','SR','SRF','SWR','WDR']

connection = pymysql.connect(host='db',
                             user='root',
                             password='rootpasswd',
                             db='mediacontent',
                             charset='utf8mb4',
                             cursorclass=pymysql.cursors.DictCursor)

try:
    with connection.cursor() as cursor:
        for channel in channels:
            for letter in abc:
                data = {
                    "queries": [
                        {
                            "fields": [
                                "title",
                                "topic"
                            ],
                            "query": '"' + letter + '"'
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
                    "size": 2
                }

                response = requests.post(url=url, json=data, headers=headers)
                response_dict = response.json()

                print(channel + ": " + letter)
                if response_dict['result']['queryInfo']['resultCount'] == 0:
                    continue


                for x in range(len(response_dict['result']['results'])):

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
                    "(%s, %s, %s, 0, 0, %s, %s)"

                    data_tuple = (mediacontent_id,
                                    str(response_dict['result']['results'][x]['title']),
                                    str(response_dict['result']['results'][x]['channel']),
                                    str(response_dict['result']['results'][x]['url_video']),
                                    media_description
                                  )

                    cursor.execute(sql, data_tuple)


finally:
    connection.commit()
    connection.close()







from configparser import ConfigParser
import psycopg2

def config(filename='database.ini', section='postgresql'):
    parser = ConfigParser()
    parser.read(filename)

    db = {}
    if parser.has_section(section):
        params = parser.items(section)
        for param in params:
            db[param[0]] = param[1]
    else:
        print('{0} szekció nem található a {1} fájlban.'.format(section, filename))

    return db

def connect():
    conn = None
    try:
        schemaFile = '_CreateSchema.sql'
        schema = open(schemaFile, 'r')
        sql = ''
        if schema.mode == 'r':
            sql = schema.read()

        if len(sql) == 0:
            print('{0} fájl üres.'.format(schemaFile))
            return

        params = config()

        print('Kapcsolódás az adatbázishoz...')
        conn = psycopg2.connect(**params)

        cur = conn.cursor()

        print('Séma generálása...')
        cur.execute(sql)

        conn.commit()
        cur.close()
    except (Exception, psycopg2.DatabaseError) as error:
        print(error)
    finally:
        if conn is not None:
            conn.close()
            print('Adatbázis kapcsolat lezárva.')

if __name__ == '__main__':
    connect()
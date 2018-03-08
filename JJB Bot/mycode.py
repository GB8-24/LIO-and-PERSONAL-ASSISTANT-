import pyodbc
import RPi.GPIO as GPIO
from time import strftime
GPIO.setmode(GPIO.BCM)
GPIO.setup(18, GPIO.OUT)
GPIO.setup(23, GPIO.OUT)
GPIO.setup(24, GPIO.OUT)
GPIO.setup(25, GPIO.OUT)
threads = []
def relay():
    GPIO.output(18, GPIO.HIGH)
    GPIO.output(23, GPIO.HIGH)
    GPIO.output(24, GPIO.HIGH)
    GPIO.output(25, GPIO.HIGH)
    sleep(60)
    GPIO.output(18, GPIO.LOW)
    GPIO.output(23, GPIO.LOW)
    GPIO.output(24, GPIO.LOW)
    GPIO.output(25, GPIO.LOW)

dsn = 'rpitestsqlserverdatasource'
user = 'pavanintern@pavanintern'
password = '9787082757Msd'
database = 'pavanintern'
connString = 'DSN={0};UID={1};PWD={2};DATABASE={3};'.format(dsn,user,password,database)
conn = pyodbc.connect(connString)
cursor = conn.cursor()
cursor.execute('select AGE from [Table]')
row = cursor.fetchone()

while row:
    dbTime = str(row[0])
    print('Look =',dbTime)
    currentTime = 21
    row=cursor.fetchone()
    if(dbTime == currentTime):
        t = threading.Thread(target=relay)
        threads.append(t)
        t.start()

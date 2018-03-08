import pyodbc
import RPi.GPIO as GPIO
from time import strftime
GPIO.setmode(GPIO.BCM)
GPIO.setup(18, GPIO.OUT)
GPIO.setup(23, GPIO.OUT)
GPIO.setup(24, GPIO.OUT)
GPIO.setup(25, GPIO.OUT)

dsn = 'rpitestsqlserverdatasource'
user = 'pavanintern@pavanintern'
password = '9787082757Msd'
database = 'pavanintern'
connString = 'DSN={0};UID={1};PWD={2};DATABASE={3};'.format(dsn,user,password,database)
connString2 = 'Driver={ODBC Driver 13 for SQL Server};Server=tcp:pavanintern.database.windows.net,1433;Database=pavanintern;Uid=pavanintern@pavanintern;Pwd=9787082757Msd;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;'
conn = pyodbc.connect(connString2)

while True:
     cursor = conn.cursor()
     cursor.execute('select top 1 device from [Msjarvis] order by ID desc')
     row = cursor.fetchone()
     if(row):
        dev = str(row[0])
	print(dev)
        if(dev == 'LightsOn'):
	 print('Lights On')
	 GPIO.output(18, GPIO.HIGH)
        elif(dev == 'FansOn'):
	 print('Fans On')
	 GPIO.output(23, GPIO.HIGH)
        elif(dev == 'TvOn'):
	 print('TVs On')
	 GPIO.output(24, GPIO.HIGH)
        elif(dev == 'HeaterOn'):
	 print('Heater On')
	 GPIO.output(25, GPIO.HIGH)
	else:
	 print('Devices Off')
	 GPIO.output(18, GPIO.LOW)
	 GPIO.output(23, GPIO.LOW)
	 GPIO.output(24, GPIO.LOW)
	 GPIO.output(25, GPIO.LOW)

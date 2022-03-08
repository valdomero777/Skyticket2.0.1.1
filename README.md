# Skyticket
Skyticket virtual ticket

Develop a special instance with the Virtual Printer,


Every job for the virtual printer will be  ask

"How do you need your ticket?"
 
1) Physically (You done this Job)
2) WhatsApp
3) Telegram
4) SMS
5) Email

Then will be write Save local in a database or something, can be a access file:


ID (local database)
ID Terminal (This need be fix constant )
ID Client  (This need be fix constant )

Image (image ticket)
PrintMethod (Can be Email/Printed/WhatsApp/Telegram/SMS)
Email
MobilePhone
Sent (This can be a boolean Flag)
DateSent


Then every minute will be a run a process to in the local machine:

If there some ticket that are  with sent in the field "SENT" in zero we need to send to Application Web Sever ,

the local app will be insert in a master database (POSTGRESQL) we will sent you a IP wit the database and credentials

And then will be updated the database local put the field Sent
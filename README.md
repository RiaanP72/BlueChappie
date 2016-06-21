# BlueChappie
This one-page application uses SQL server or an elasticsearch server to store images with their meta data as retrieved from the Flickr.com API.
The elsaticsearch connections uses both lowlevel elastic client and nest.
I tried to keep all the code in one file clsMainProgram.cs so that definition reference is easy.
In the __Installation folder in a SQL script to create the data base on an SQL server. There is also BlueChappie.7z which contains a backup made of the data base and already contains some data, so you can restore it to a SQL server.
The images are stored Base64 encoded to enable easy transport.
My elasticsearch server lives on my Debian Linux box.
#Quick Setup:
Download the file BlueChappie.zip ( https://github.com/RiaanP72/BlueChappie/blob/master/BlueChappie.zip ) and extract it to your IIS server www folder. if it cannot have it's own DNS entry ,ake sure it's "Converted to Application". It is build with .NET 4.5.2 so the recommended application pool is .NET v4.5 classic or integrated.

# Buggs:
No know buggs.

#ElasticSearch
It is my 1st time playing around with elasticsearch and I must say a learnt allot in these few days. It is a great product!
To see what BlueChappie runs like the video is on https://youtu.be/vFHNb5jcGWg
#The important bit:
In the root directory of the BlueChappie application is the web.config (from line 14) (https://github.com/RiaanP72/BlueChappie/blob/master/BlueChappie/Web.config)
#PLEASE
Ensure that you configure the appSettings section correctly (I did not cater for authentication on elasticsearch, but it can be added if required)
The StorageType by default is for SQL wich is sql server (script is in the __installation folder)
The SQLConnectionString and elasticsearch keys are self elplanitory.

Let me know if I left anything out or you have any questions.

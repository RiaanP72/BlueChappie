# BlueChappie

This API uses ASP.NET 4.5.2 and aggregates images and metadata about those images from an external source Flickr.com. 
The API focus is on the Controllers that is uses throughout the application. The data structure connects to either a MSSQL data base or ElasticSearch Index.
The _ViewStart.cshtml pointing to Index.cshtml run a one-page application that utilises these REST API’s GET and POST methods.
This API uses SQL server or an elasticsearch server to store images with their meta data as retrieved from the Flickr.com API.
The elsaticsearch connections uses both lowlevel elastic client and nest.
I tried to keep all the code in one file clsMainProgram.cs so that definition reference is easy.

In the __Installation folder in a SQL script to create the data base on an SQL server. A backup copy of the database is located in BlueChappie Install from the master tree (BlueChappie.exe (7zip) with the 10mb files).
https://github.com/RiaanP72/BlueChappie-Install

The images are stored Base64 encoded to enable easy transport.

My elasticsearch server lives on my Debian Linux box.
To see what BlueChappie runs like the video is on https://youtu.be/vFHNb5jcGWg


#Quick Setup:
Download the file BlueChappie.zip ( https://github.com/RiaanP72/BlueChappie/blob/master/BlueChappie.zip ) and extract it to your IIS server www folder. if it cannot have it's own DNS entry ,make sure it's "Converted to Application". It is build with .NET 4.5.2 so the recommended application pool is .NET v4.5 classic or integrated.


# Buggs:
No know bugs.

#ElasticSearch
It is my 1st time playing around with elasticsearch and I must say a learnt allot in these few days. It is a great product!

#The important bit:
In the root directory of the BlueChappie application is the web.config (from line 14) (https://github.com/RiaanP72/BlueChappie/blob/master/BlueChappie/Web.config)
#PLEASE
Ensure that you configure the appSettings section correctly (I did not cater for authentication on elasticsearch, but it can be added if required)
The StorageType by default is for SQL which is sql server (script is in the __installation folder)
The SQLConnectionString and elasticsearch keys are self elplanitory.

Let me know if I left anything out or you have any questions.

# SS-Contacts-API
A simple API for storing contacts.

## Setup
* Clone the repository to your local machine.
* This repository utilizes [LiteDB](https://www.litedb.org/) for embedded document storage. 
  * By default, the database will be stored as `C:\Temp\Contacts.db`. 
  * You will need to verify that this directory exists before running the API. 
  * If you would like to change the default location of the database, you will need to change the value of connectionString inside of `Web.config`.

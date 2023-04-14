# Walking Tracker Bot

This is a Viber bot created using ASP.NET Core WebApi. The bot provides walking tour suggestions to users based on their location using data stored in a MSSQL database. The procedure for calculating the walking tours is also included in the project.

## Prerequisites
To run this bot locally, you need to have the following software installed on your system:

- .NET 6 SDK
- Ngrok

## Usage
1. Clone the repository and open it in Visual Studio or another code editor.
2. Open the MSSQL script file provided and run it to create the necessary database tables.
3. Modify the connection string in the appsettings.json file to point to the newly created database.
4. Build and run the project in your code editor.
5. Start ngrok and expose the local port the project is running on.
6. Register a Viber public account and set up a bot.
7. Configure the Viber bot webhook to point to the ngrok URL.
8. Use the bot to get walking tour suggestions based on the user's location or to get information about walking tours by entering an IMEI number.

## Calculations
The procedure for calculating the walking tours includes the following steps:

1. Divide the data into separate walks. A new walk is considered if the time between the last signal is 30 minutes or more.
2. Calculate the distance covered for each walk.
3. Calculate the duration of each walk.
4. Calculate the total distance and total duration for the day.
5. Create a Viber bot that displays the total information and a top 10 list of walking tours by distance covered.

## Built With
- ASP.NET Core WebApi
- Entity Framework Core
- Ngrok

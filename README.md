SteamPlayedGames
===================

A simple web application to know which games you've completed and which you don't on steam.

## What you can do

- Connect with your steam account to retrieve all your games
- Mark games as completed or not
- Retrieve the metacritics scores of your games
- Order them by playtime
- Show your total playtime for every games (I'm sure this data could be scary for some people ;) )


## Technologies Used

 - Asp.Net (Web Forms)
 - Fluent Metacritic to parse their website (since no api exists back then)
 - XML file to store data (ligthweight and easy to export)
 - Steam API to retrieve the player games
 - Ajax (Single Page Application)

## Installation

You can download the source code and [setup a IIS Server](http://helpdeskgeek.com/windows-8/install-and-setup-a-website-in-iis-8-on-windows-8/) to use it. There's no setup for the database since it only uses xml files.
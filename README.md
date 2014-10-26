Renegade-X-Stats-Tracker
========================

A stat tracking system with website for Renegade-X.

# Features
- Player kill/death tracking (along with headshots)
- Building kills
- Vehicle kills
- Configurable automatic announcements
- Simple administration powers (ability to mute/kick/ban players)
- Bans based on SteamID and/or IP address

# Installation instructions
- Either download a pre-built binary from [releases](https://github.com/JaTochNietDan/Renegade-X-Stats-Tracker/releases) or else build it yourself.
- Setup your configuration file (see below for example configuration)
- Set up a new database in your MySQL database
- Execute the [SQL dump](https://raw.githubusercontent.com/JaTochNietDan/Renegade-X-Stats-Tracker/master/Renegade%20X%20Stat%20Tracker/Renegade%20X%20Stat%20Tracker/renegade.sql) on your database
- Run the program!

# Configuration

## Example

```ini
[LOG]
path=log.txt (This should be the path to your Renegade X server's log file)
[DATABASE]
host=127.0.0.1
database=renegade
user=root
password=
[RCON] (This should contain the details of your server's RCON)
host=127.0.0.1
port=7777
password=
[ANNOUNCEMENTS] (Here you should have a total which is the number of announcements and count them up from 1)
total=1
1=Thanks for reading, visit me on http://www.jatochnietdan.com
2=Another announcement, woo.
```

# Administration commands
*Hint: You can make yourself an administrator by going into the database and setting the `level` column to 4 in your row.*

You can run commands in-game by prefixing them with an exclamation mark, for example:
`!kick Sneaky`

| Command | Parameters | Level Required | Description |
| ------- | ---------- | -------------- | ----------- |
| kick    | Playername | 2              | Kicks a player from the server |
| ban    | Playername | 3              | Bans a player from the server. If the player is from Steam, they're SteamID will be banned, if not, their IP will be banned |
| unban    | Player's ID | 3              | Unban a player from the server via their ID |
| setlevel    | Playername | 4              | Set a player's level (makes them an administrator) |
| addbots | Number of bots | 3 | Add a specified number of bots to the server |
| restartmap | none | 3 | Will restart the map |
| changemap | mapname | 3 | Will change to a specified map |
| mute | Playername | 1 | Will prevent a player from talking |
| unmute | Playername | 1 | Will allow a muted player to talk again |

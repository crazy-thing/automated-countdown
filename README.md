# Countdown 
[![Download here](https://img.shields.io/badge/Download-Here-blue?style=for-the-badge)](https://github.com/crazy-thing/countdown/releases) [![License](https://img.shields.io/badge/License-GPL--3-0FC269?style=for-the-badge)](https://github.com/crazy-thing/countdown/blob/main/LICENSE) [![Release](https://img.shields.io/badge/Release-v.1.0.0-20A6A5?style=for-the-badge)](https://github.com/crazy-thing/countdown/releases/tag/v1.0.0)

This program was created for a church as a simple way to display a countdown timer and bible verses on their live stream. It's been tested using OBS studio displaying the bible verses as a browser source and the countdown timer as a text file.

This program utilizes Rob Keplin's <a href="https://www.rkeplin.com/the-holy-bible-open-source-rest-api/">"The Holy Bible – Open Source REST API" </a> for getting bible verses.

# Features

- Create and manage countdown timers and live bible verses
- Automation and scheduling
- Lots of configurable settings

# Future Improvements
- Command parser
- Refactor code for efficiency and readability
- Add a GUI
- Better error handling

# Documentation 


| Command | Description                                       | Required parameters                                         | Example                                  |                       
|---------|---------------------------------------------------|-------------------------------------------------------------|------------------------------------------|
| `start` | Starts a specific or multiple tasks               | [See below for usage of start command](#start)              |                                          |
| `stop`  | Stops a specific or all tasks                     |  "enter name here", "all"                                   | stop "countdown1"                        |
| `show`  | Shows all running tasks                           | none                                                        | show                                     |
| `set`   | Sets a configuration option for a setting         | [See below for usage of set command](#set)                  |                                          |
| `enable-auto-start` | Enables the automatic startup of a specified task  | countdown,  bible-verses                       | enable-auto-start countdown              |
| `disable-auto-start`| Disables the automatic startup of a specified task | countdown, bible-verses                        | disable-auto-start bible-verses          |
| `edit-bible-verses` | Opens the variables file for quick configuration   | none                                           | edit-bible-verses                        |
| `exit`  | Exits the program                                 |  none                                                       | exit                                     |                                


## start

| Argument | Description                                            | Required parameters | Optional Parameters                                                 | Example                                                                                                  |                       
|----------|--------------------------------------------------------|---------------------|---------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------|
| countdown| Specifies a countdown task                             | yyyy-mm-dd hh:mm:ss | name "enter name here", file-path "enter file path here"            | start countdown 2023-10-10 16:30:00 name "my countdown" file-path "C:\Users\User\Documents\countdown.txt"|
| bible-verses| Specifies a bible-verses task                       | none                | name "enter name here" file-path, "enter file path here"            | start bible-verses name "new bible verses" file-path "C:\Users\User\Documents\bible-verses.html"         |
| countdown-verses| Specifies both countdown and bible-verses tasks | yyyy-mm-dd hh:mm:ss | none (Uses default settings)                                        | start countdown-verses 2023-10-10 16:30:00                                                               |

##  set 

| Argument                 | Description                                                    | Required parameters                                                       | Example                                                         |                       
|--------------------------|----------------------------------------------------------------|---------------------------------------------------------------------------|-----------------------------------------------------------------|
| countdown-text           | Sets the text displayed during countdown                       | "enter text here"                                                         | set countdown-text "Livestream begins in"                       |
| countdown-over-text      | Sets the text displayed when the countdown has ended           | "enter text here"                                                         | set countdown-over-text "Countdown over"                        |
| countdown-format         | Sets the format for the countdown time to be displayed as      | dd:hh:mm:ss, dd:hh:mm dd:hh, dd, hh:mm:ss, hh:mm, hh, mm:ss, mm, ss       | set countdown-format hh:mm:ss                                   |
| file-path                | Sets the default file path for countdowns                      | "enter file path here"                                                    | set file-path "C:\Users\user\Documents\countdown.txt"           |
| auto-start-time          | Sets the day and time for the countdown automatically count to | *full day of week or daily (daily, monday, tuesday, wednesday, thursday, friday, saturday, sunday), hh:mm:ss | set auto-start-time monday 16:00:00 | 
| bible-verses-interval    | Sets the time to wait before getting a new bible verse         | *time in seconds*                                                         | set bible-verses-interval 10                                    |
| bible-verses-file-path   | Sets the file path for the bible verses                        | "enter file path here"                                                    | set bible-verses-file-path "C:\Users\user\Documents\verses.html |
| bible-verses-translation | Sets the translation to display the bible verses in            | ASV, BBE, DARBY, KJV, WEB, YLT, ESV, NIV, NLT                             | set bible-verses-translation KJV                                |
| bible-verses-genre       | Sets the genre of books to pull bible verses from              | All, Law, History, Wisdom, Prophets, Gospels, Acts, Epistles, Apocalyptic | set bible-verses-genre Wisdom                                   |


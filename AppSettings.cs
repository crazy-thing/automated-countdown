class AppSettings
{
    public string CountdownText { get; set; } = "Live in";
    public string CountdownOverText { get; set; } = "Starting soon!";
    public string CountdownFormat {get; set;} = "hh:mm:ss";
    public string FilePath { get; set; }
    public Boolean AutoStartCountdown {get; set;} = false;
    public DateTime AutoCountdownDateTime {get; set;}
    public DayOfWeek AutoCountdownDay {get; set;}
    public string AutoCountdownTime {get; set;}
    public Boolean AutoStartBibleVersesLoop {get; set;} = false;
    public int BibleVersesLoopInterval {get; set;} = 30;
    public string BibleVersesFilePath {get; set;}
    public string BibleVersesTranslation {get; set;} = "KJV";
    public string BibleVersesGenre {get; set;} = "All";
    
}
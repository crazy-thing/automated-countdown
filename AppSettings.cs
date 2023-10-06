class AppSettings
{
    public string CountdownText { get; set; }
    public string CountdownOverText { get; set; }
    public string CountdownFormat {get; set;}
    public string FilePath { get; set; }
    public Boolean AutoStartCountdown {get; set;}
    public DateTime AutoCountdownDateTime {get; set;}
    public DayOfWeek AutoCountdownDay {get; set;}
    public string AutoCountdownTime {get; set;}
    public Boolean AutoStartBibleVersesLoop {get; set;}
    public int BibleVersesLoopInterval {get; set;}
    public string BibleVersesFilePath {get; set;}
    public string BibleVersesTranslation {get; set;}
    public string BibleVersesGenre {get; set;}
    

    //     public string BibleVersesFilePath {get; set;}
    //     public string BibleVersesInfoFilePath {get; set;}
}
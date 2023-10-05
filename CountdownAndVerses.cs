using System;
using System.Threading;

class BibleVersesWriter
{
    public static Dictionary<string, string> nameToIds = new Dictionary<string, string>();


    public static void StartBibleVerses()
    {
        string bibleVersesName = $"verses{Program.nameToIds.Count + 1}";
        string bibleVersesId = Guid.NewGuid().ToString();

        Program.nameToIds.Add(bibleVersesName, bibleVersesId);
        CancellationTokenSource cts = new CancellationTokenSource();
        Task.Run(() => StartBibleVersesInternal(cts, bibleVersesName));

        lock (Program.lockObject)
        {
            Program.tasks.Add(bibleVersesName, cts);
        }

    }

    public static async Task StartBibleVersesInternal(CancellationTokenSource cts, string bibleVersesName)
    {
        string filePath = SettingsManager.GetBibleVersesFilePath();

        CreateDefaultTemplate(filePath);

        string template = File.ReadAllText(filePath);
        string prevBibleVerse = "{bibleVerse}";
        string prevBibleVerseInfo = "{bibleVerseInfo}";

        while (!cts.IsCancellationRequested)
        {
        int interval = SettingsManager.GetBibleVersesLoopInterval();

        BibleVerseModel bibleVerseModel = BibleVerses.GetBibleVerse().Result;
        string bibleVerseInfo = $"{bibleVerseModel.book.name} {bibleVerseModel.chapterId}:{bibleVerseModel.verseId}";

        template = template.Replace(prevBibleVerseInfo, bibleVerseInfo).Replace(prevBibleVerse, bibleVerseModel.verse);

        prevBibleVerseInfo = bibleVerseInfo;
        prevBibleVerse = bibleVerseModel.verse;
        File.WriteAllText(SettingsManager.GetBibleVersesFilePath(), template);
        Console.WriteLine("DONE");

        Thread.Sleep(interval);
        }

        if (cts.IsCancellationRequested)
        {
            Console.WriteLine($"Bible Verses Loop {bibleVersesName} finished!");
        }
        else
        {
            Console.WriteLine($"Bible Verses Loop {bibleVersesName} canceled!");
        }



    }

    public static void CreateDefaultTemplate(string filePath)
    {
        string defaultTemplate = @"
<!DOCTYPE html>
<html>
<head>
    <script>
        function refreshPage() {
            setTimeout(function() {
                location.reload();
            }, 500);
        }

        window.onload = refreshPage;
    </script>
</head>
<body>
    <div class=""template"">
        <div class=""template-verse-info"">
            <p class=""template-verse-info-text"">{bibleVerseInfo}</p>
        </div>
        <div class=""template-verse"">
            <p class=""template-verse-text"">{bibleVerse}</p>
        </div>
    </div>

</body>
<style>
    .body {
        background: transparent;
    }
    .template {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }
    .template-verse-info {
        display: flex;
        text-align: center;
        justify-content: center;
        align-items: center;
        font-size: 38px;
        font-family: ""Arial"";
    }
    .template-verse {
        max-width: 1200px;
        font-size: 54px;
        display: flex;
        justify-content: center;
        align-items: center;
        text-align: center;
        text-overflow: ellipsis ellipsis;
        word-break: normal;
        white-space: normal;
        font-family: ""Arial"";
    }

    .template-verse-text {
        width: 100%;
    }

</style>
</html>
";

    File.WriteAllText(filePath, defaultTemplate);
    }

}
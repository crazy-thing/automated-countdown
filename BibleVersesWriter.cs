using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

class BibleVersesWriter
{
    public static Dictionary<string, string> nameToIds = new Dictionary<string, string>();
    public static string variablesPath = "verses-variables.txt";
    public static string templatePath ="verses-template.html";

    public static void StartBibleVerses(string bibleVersesName = null, string versesFilePath = null)
    {
        if (string.IsNullOrWhiteSpace(bibleVersesName))
            {
                bibleVersesName = $"verses{Program.nameToIds.Count + 1}";
            }

        string bibleVersesId = Guid.NewGuid().ToString();

        if (string.IsNullOrWhiteSpace(versesFilePath))
            {
                versesFilePath = SettingsManager.GetBibleVersesFilePath();
            }

        Program.nameToIds.Add(bibleVersesName, bibleVersesId);
        CancellationTokenSource cts = new CancellationTokenSource();
        Task.Run(() => StartBibleVersesInternal(cts, bibleVersesName, versesFilePath));
        TaskInfo taskInfo = new TaskInfo
        {
            TaskType = "Bible-Verses",
            CancellationTokenSource = cts,
        };

        lock (Program.lockObject)
        {
            Program.tasks.Add(bibleVersesName, taskInfo);
        }
    }

    public static async Task StartBibleVersesInternal(CancellationTokenSource cts, string bibleVersesName, string filePath)
    {
        Console.WriteLine("Started bible verses with name of: " + bibleVersesName);
        CreateFromTemplate(filePath);


        string template = File.ReadAllText(filePath);
        string prevBibleVerse = "{bibleVerse}";
        string prevBibleVerseInfo = "{bibleVerseInfo}";

        while (!cts.IsCancellationRequested)
        {
        int interval = SettingsManager.GetBibleVersesLoopInterval();

        BibleVerseModel bibleVerseModel = await BibleVerses.GetBibleVerse();
        string bibleVerseInfo = $"{bibleVerseModel.book.name} {bibleVerseModel.chapterId}:{bibleVerseModel.verseId}";
        template = template.Replace(prevBibleVerseInfo, bibleVerseInfo).Replace(prevBibleVerse, bibleVerseModel.verse);
        prevBibleVerseInfo = bibleVerseInfo;
        prevBibleVerse = bibleVerseModel.verse;
        File.WriteAllText(filePath, template);

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

    public static void CreateFromTemplate(string filePath)
    {

        if (!File.Exists(templatePath))
        {
            CreateDefaultTemplate();
        }

        var template = File.ReadAllText(templatePath);

        File.WriteAllText(filePath, template);
    }

    public static void EditBibleVersesVariables()
    {

        if (File.Exists(variablesPath))
        {
            Process.Start("notepad.exe", variablesPath);
        }
        else
        {
            CreateVariablesFile();
        }
    }


    public static void CreateDefaultTemplate()
    {
        if (!File.Exists(variablesPath))
        {
            CreateVariablesFile();
        }

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
        font-size: {verseInfoFontSize}px;
        font-family: ""{verseInfoFont}"";
        color: {verseInfoTextColor};
    }
    .template-verse {
        max-width: 1200px;
        font-size: {verseFontSize}px;
        display: flex;
        justify-content: center;
        align-items: center;
        text-align: center;
        text-overflow: ellipsis ellipsis;
        word-break: normal;
        white-space: normal;
        font-family: ""{verseFont}"";
        color: {verseTextColor};
    }

    .template-verse-text {
        width: 100%;
    }

</style>
</html>
";

    var variableDict = new Dictionary<string, string>();
    foreach (string line in File.ReadLines(variablesPath))
    {
        string[] parts = line.Split('=');
        if (parts.Length == 2)
        {
            variableDict[parts[0].Trim()] = parts[1].Trim();
        }
    }

    foreach(var variable in variableDict)
    {
        string variablePlaceholder = "{" + variable.Key + "}";
        defaultTemplate = defaultTemplate.Replace(variablePlaceholder, variable.Value);
    }

    File.WriteAllText(templatePath, defaultTemplate);
    }

    public static void CreateVariablesFile()
    {        
        string defaultVariables = 
@"
verseInfoFont=Arial
verseInfoFontSize=48
verseInfoTextColor=#fff

verseFont=Arial
verseFontSize=60
verseTextColor=#fff

";
        File.WriteAllText(variablesPath, defaultVariables);
    }
}
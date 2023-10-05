using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;

class BibleVerses
{

    private static string fullUrl;
    private static string baseApiUrl = "https://bible-go-api.rkeplin.com/v1/books";
    private static string translation = "NIV";


    public static string GetRandomNum(int minValue, int maxValue)
    {
        Random random = new Random();

        int randomNumber = random.Next(minValue, maxValue);

        string randomNum = randomNumber.ToString();

        return randomNum;
    }

    public static async Task GetFullUrl()
    {
        HttpClient httpClient = new HttpClient();
        try
            {
            List<int> allVerses = new List<int>();
            string bookNum = GetRandomNum(1, 66);
            string bookUrl = $"{baseApiUrl}/{bookNum}/chapters";
            int bookLen = 0;
            HttpResponseMessage bookResponse = await httpClient.GetAsync(bookUrl);
            if (bookResponse.IsSuccessStatusCode)
            {
                string content = await bookResponse.Content.ReadAsStringAsync();
                JsonDocument bookDoc = JsonDocument.Parse(content);
                bookLen = bookDoc.RootElement.GetArrayLength();
            }
            else
            {
                Console.WriteLine("Error: " + bookResponse.StatusCode);
            }

            string chapNum = GetRandomNum(1, bookLen);
            string chapUrl = $"{bookUrl}/{chapNum}";
            int chapLen = 0;
            HttpResponseMessage chapResponse = await httpClient.GetAsync(chapUrl);
            if (chapResponse.IsSuccessStatusCode)
            {
                string chapContent = await chapResponse.Content.ReadAsStringAsync();
                JsonDocument chapDoc = JsonDocument.Parse(chapContent);
    
                foreach (JsonElement element in chapDoc.RootElement.EnumerateArray())
                {
                    int verse = element.GetProperty("id").GetInt32();
                    allVerses.Add(verse);
                }
                }
            else
            {
                Console.WriteLine("Error: " + chapResponse.StatusCode);
            }

            int last = allVerses[allVerses.Count - 1];
            int first = allVerses[0];
            string verseId = GetRandomNum(first, last);
            fullUrl = $"{chapUrl}/{verseId}?translation={translation}";

            }
        
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            httpClient.Dispose();
        }

    }

    public static async Task<BibleVerseModel> GetBibleVerse()
    {
        HttpClient httpClient = new HttpClient();
        try
        {
            await GetFullUrl();

            HttpResponseMessage bibleVerses = await httpClient.GetAsync(fullUrl);
            if (bibleVerses.IsSuccessStatusCode)
            {
                string bibleVersesContent = await bibleVerses.Content.ReadAsStringAsync();
                BibleVerseModel bibleVerseModel = JsonSerializer.Deserialize<BibleVerseModel>(bibleVersesContent);
                return bibleVerseModel;
                // set setting or variable for the current bible verse. then render that in a while loop similar to the countdown timer. add padding to get rid of old text

            }
            else
            {
                Console.WriteLine("Error: " + bibleVerses.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("FINSIEHD");
            httpClient.Dispose();
        }

        return null;
    }

}
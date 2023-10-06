using System.Collections;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.VisualBasic;

class BibleVerses
{

    private static string fullUrl;
    private static string baseApiUrl = "https://bible-go-api.rkeplin.com/v1/books";

    public class Book
    {
        public int id { get; set; }
        public Genre genre { get; set; }
    }

    public class Genre
    {
        public string name { get; set; }
    }

    public static string GetRandomNum(int minValue, int maxValue)
    {
        Random random = new Random();

        int randomNumber = random.Next(minValue, maxValue);

        string randomNum = randomNumber.ToString();

        return randomNum;
    }

    public static async Task<string> GetBookIdByGenre(string genre)
    {
        HttpClient httpClient = new HttpClient();
        try
        {
            HttpResponseMessage booksRes = await httpClient.GetAsync(baseApiUrl);
            if (booksRes.IsSuccessStatusCode)
            {
                string bookResContent = await booksRes.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
                var books = JsonSerializer.Deserialize<Book[]>(bookResContent, options);

                var filteredBooks = books.Where(book => book.genre.name == genre).ToList();

                List<int> bookIds = new List<int>();

                foreach (var book in filteredBooks)
                {
                    Console.WriteLine($"id {book.id} and genre {book.genre.name}");
                    bookIds.Add(book.id);
                }
                string randomBookNum = GetRandomNum(bookIds.First(), bookIds.Last() + 1);
                Console.WriteLine(randomBookNum);
                return randomBookNum;
            }
            else
            {
                Console.WriteLine("Failed to fetch");
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        return null;
    }

    public static async Task GetFullUrl()
    {   
        string searchByGenre = SettingsManager.GetBibleVersesGenre();

        HttpClient httpClient = new HttpClient();
        try
            {
            
            string bookNum = string.Empty;
            if (searchByGenre == "All")
            {
                bookNum = GetRandomNum(1, 66);
            }
            else
            {
                if (Util.allowedGenres.Contains(searchByGenre))
                {
                    bookNum = await GetBookIdByGenre(searchByGenre);
                }
                else
                {
                    Console.WriteLine($"Invalid genre. Please choose from {Util.allowedGenres}");
                }
            }


            List<int> allVerses = new List<int>();
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
            fullUrl = $"{chapUrl}/{verseId}?translation={SettingsManager.GetBibleVersesTranslation()}";

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
            httpClient.Dispose();
        }

        return null;
    }

}
using CatFactsGiphyAPI.Data;
using CatFactsGiphyAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CatFactsGiphyAPI.Services
{
    public interface ICatFactsService
    {
        Task<CatFactResponse> GetRandomFactAsync();
        Task<string> GetGifForQueryAsync(string query);
        Task<FactGifResponse> GetFactWithGifAsync();
        Task<List<SearchHistory>> GetSearchHistoryAsync();
    }

    public class CatFactsService : ICatFactsService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _giphyApiKey;

        public CatFactsService(HttpClient httpClient, AppDbContext context, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _context = context;
            _configuration = configuration;
            _giphyApiKey = _configuration["GiphyApiKey"] ?? throw new InvalidOperationException("Giphy API Key not found");
        }

        public async Task<CatFactResponse> GetRandomFactAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://catfact.ninja/fact");
                var catFact = JsonConvert.DeserializeObject<CatFactResponse>(response);
                return catFact ?? new CatFactResponse();
            }
            catch (Exception ex)
            {
                // Log error here
                throw new Exception($"Error fetching cat fact: {ex.Message}");
            }
        }

        public async Task<string> GetGifForQueryAsync(string query)
        {
            try
            {
                var encodedQuery = Uri.EscapeDataString(query);
                var giphyUrl = $"https://api.giphy.com/v1/gifs/search?api_key={_giphyApiKey}&q={encodedQuery}&limit=1";

                var response = await _httpClient.GetStringAsync(giphyUrl);
                var giphyResponse = JsonConvert.DeserializeObject<GiphyResponse>(response);

                if (giphyResponse?.Data?.Count > 0)
                {
                    return giphyResponse.Data[0].Images.Original.Url;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                // Log de error
                throw new Exception($"Error fetching GIF: {ex.Message}");
            }
        }

        public async Task<FactGifResponse> GetFactWithGifAsync()
        {
            var fact = await GetRandomFactAsync();
            var queryWords = ExtractFirstThreeWords(fact.Fact);
            var gifUrl = await GetGifForQueryAsync(queryWords);


            return new FactGifResponse
            {
                Fact = fact.Fact,
                GifUrl = gifUrl,
                QueryWords = queryWords
            };
        }

        public async Task<List<SearchHistory>> GetSearchHistoryAsync()
        {
            return await _context.SearchHistories
                .OrderByDescending(x => x.SearchDate)
                .ToListAsync();
        }

        private string ExtractFirstThreeWords(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Quitar puntuación y dividir en palabras
            var cleanText = Regex.Replace(text, @"[^\w\s]", "");
            var words = cleanText.Split(new char[] { ' ', '\t', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", words.Take(3));
        }

    }
}
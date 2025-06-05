using CatFactsGiphyAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatFactsGiphyAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class CatFactsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CatFactsController> _logger;
        private readonly string _giphyApiKey;
        private readonly ICatFactsService _catFactsService;

        private static readonly List<SearchHistory> _searchHistory = new List<SearchHistory>();

        public CatFactsController(HttpClient httpClient, ILogger<CatFactsController> logger, IConfiguration configuration, ICatFactsService catFactsService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _giphyApiKey = configuration["GiphyApiKey"] ?? "voaNIOg1u7ONPbckzWK71C48YqCOkhVP";
            _catFactsService = catFactsService;
        }

        [HttpGet("fact")]
        public async Task<ActionResult<FactGifResponse>> GetFact()
        {
            try
            {
                var result = await _catFactsService.GetFactWithGifAsync();

                // AGREGAR: Guardar en historial REAL después de obtener el fact
                var historyItem = new SearchHistory
                {
                    SearchDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    FactText = result.Fact,
                    QueryWords = result.QueryWords,
                    GifUrl = result.GifUrl
                };
                
                // Insertar al inicio (más reciente primero)
                _searchHistory.Insert(0, historyItem);

                _logger.LogInformation($"Fact guardado en historial. Total items: {_searchHistory.Count}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cat fact and GIF");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("gif")]
        public async Task<ActionResult<FactGifResponse>> GetGif([FromQuery] string query, [FromQuery] int offset = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    return BadRequest("Query parameter is required");
                }

                // Asegurar que offset esté en rango válido (Giphy permite hasta 4999)
                offset = Math.Max(0, Math.Min(offset, 4999));

                // CLAVE: Agregar offset para obtener diferentes resultados
                var giphyUrl = $"https://api.giphy.com/v1/gifs/search?api_key={_giphyApiKey}&q={Uri.EscapeDataString(query)}&limit=1&offset={offset}&rating=g&lang=en";

                var response = await _httpClient.GetAsync(giphyUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var giphyResponse = JsonSerializer.Deserialize<GiphyResponse>(content);

                string gifUrl = "https://via.placeholder.com/400x300?text=No+GIF+Found";

                if (giphyResponse?.data?.Length > 0)
                {
                    gifUrl = giphyResponse.data[0].images.original.url;
                }

                var result = new FactGifResponse
                {
                    Fact = "", 
                    QueryWords = query,
                    GifUrl = gifUrl
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting GIF for query: {Query} with offset: {Offset}", query, offset);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        [HttpGet("history")]
        public ActionResult<List<SearchHistory>> GetHistory()
        {
            try
            {
                _logger.LogInformation($"Obteniendo historial. Items disponibles: {_searchHistory.Count}");

                // Si no hay historial real, devolver lista vacía
                if (_searchHistory.Count == 0)
                {
                    _logger.LogInformation("No hay historial disponible. Genera algunos facts primero.");
                    return Ok(new List<SearchHistory>());
                }

                // Devolver historial real (ya está ordenado por más reciente primero)
                return Ok(_searchHistory.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting search history");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

    }

    // Clases para deserializar las respuestas de las APIs
    public class CatFactResponse
    {
        public string fact { get; set; }
        public int length { get; set; }
    }

    public class GiphyResponse
    {
        public GiphyData[] data { get; set; }
    }

    public class GiphyData
    {
        public GiphyImages images { get; set; }
    }

    public class GiphyImages
    {
        public GiphyImage original { get; set; }
    }

    public class GiphyImage
    {
        public string url { get; set; }
    }

    // Clases para las respuestas de nuestra API
    public class FactGifResponse
    {
        public string Fact { get; set; }
        public string QueryWords { get; set; }
        public string GifUrl { get; set; }
    }

    public class SearchHistory
    {
        public required string SearchDate { get; set; }
        public required string FactText { get; set; }
        public required string QueryWords { get; set; }
        public required string GifUrl { get; set; }
    }
}
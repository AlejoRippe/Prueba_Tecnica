namespace CatFactsGiphyAPI.Models
{
    public class CatFactResponse
    {
        public string Fact { get; set; } = string.Empty;
        public int Length { get; set; }
    }

    public class GiphyResponse
    {
        public List<GiphyData> Data { get; set; } = new List<GiphyData>();
    }

    public class GiphyData
    {
        public string Id { get; set; } = string.Empty;
        public GiphyImages Images { get; set; } = new GiphyImages();
    }

    public class GiphyImages
    {
        public GiphyImageInfo Original { get; set; } = new GiphyImageInfo();
    }

    public class GiphyImageInfo
    {
        public string Url { get; set; } = string.Empty;
    }

    public class FactGifResponse
    {
        public string Fact { get; set; } = string.Empty;
        public string GifUrl { get; set; } = string.Empty;
        public string QueryWords { get; set; } = string.Empty;
    }
}
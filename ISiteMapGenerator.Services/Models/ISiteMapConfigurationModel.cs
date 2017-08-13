namespace ISiteMapGenerator.Services.Models
{
    public interface ISiteMapConfigurationModel
    {
        int? CurrentPage { get; }

        int Size { get; }

        string CreateSitemapUrl(int currentPage);
    }
}

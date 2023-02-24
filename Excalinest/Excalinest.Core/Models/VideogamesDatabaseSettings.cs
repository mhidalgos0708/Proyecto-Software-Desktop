namespace Excalinest.Core.Models;

public class VideogamesDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string VideogamesCollectionName { get; set; } = null!;
}

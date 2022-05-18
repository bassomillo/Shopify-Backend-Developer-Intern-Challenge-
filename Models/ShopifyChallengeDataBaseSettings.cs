namespace ShopifyChallengeBackEndApi.Models;

public class ShopifyChallengeDataBaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string InventoryCollectionName { get; set; } = null!;

    public string RecycleCollectionName { get; set; } = null!;

}
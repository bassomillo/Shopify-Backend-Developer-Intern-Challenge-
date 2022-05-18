using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ShopifyChallengeBackEndApi.Models;

namespace ShopifyChallengeBackEndApi.Services;
public class InventoryService
{
    private readonly IMongoCollection<Inventory> _inventoryCollection;

    public InventoryService(
        IOptions<CSI5112BackendDataBaseSettings> csi5112BackendDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            csi5112BackendDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackendDataBaseSettings.Value.DatabaseName);

        _inventoryCollection = mongoDatabase.GetCollection<Inventory>(
            csi5112BackendDataBaseSettings.Value.InventoryCollectionName);
    }

    public async Task<List<Inventory>> GetAsync() =>
        await _inventoryCollection.Find(_ => true).ToListAsync();

    public async Task<Inventory?> GetAsync(string id) =>
        await _inventoryCollection.Find(x => x.product_id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Inventory newBook) =>
        await _inventoryCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Inventory updatedInventory) =>
        await _inventoryCollection.ReplaceOneAsync(x => x.product_id == id, updatedInventory);

    public async Task RemoveAsync(string id) =>
        await _inventoryCollection.DeleteOneAsync(x => x.product_id == id);
}
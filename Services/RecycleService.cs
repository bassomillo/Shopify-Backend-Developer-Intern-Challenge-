using System;
using System.Runtime.Serialization.Formatters;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ShopifyChallengeBackEndApi.Models;

namespace ShopifyChallengeBackEndApi.Services;
public class RecycleService
{
    private readonly IMongoCollection<Recycle> _recycleCollection;
    private readonly InventoryService _inventoryService;

    public RecycleService(InventoryService inventoryService,
        IOptions<ShopifyChallengeDataBaseSettings> shopifyChallengeDataBaseSettings)
    {
        _inventoryService = inventoryService;
        var mongoClient = new MongoClient(
            shopifyChallengeDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            shopifyChallengeDataBaseSettings.Value.DatabaseName);

        _recycleCollection = mongoDatabase.GetCollection<Recycle>(
            shopifyChallengeDataBaseSettings.Value.RecycleCollectionName);
    }

    public async Task<List<Recycle>> GetAsync() =>
        await _recycleCollection.Find(_ => true).ToListAsync();

    public async Task<Recycle?> GetAsync(string id) =>
        await _recycleCollection.Find(x => x.recycle_id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(string id, string comment)
    {

        Inventory inventory = await _inventoryService.GetAsync(id);
        Recycle recycle = new Recycle();
        if (inventory != null)
        {
            recycle.category = inventory.category;
            recycle.description = inventory.description;
            recycle.name = inventory.name;
            recycle.price = inventory.price;
            recycle.quantity = inventory.quantity;
            recycle.product_id = inventory.product_id;
            recycle.comment = comment;
            recycle.date = DateTime.Now.ToString();
            await _recycleCollection.InsertOneAsync(recycle);
            await _inventoryService.RemoveAsync(id);
        }
    }


    public async Task UpdateAsync(string id, Recycle updatedRecycle) =>
        await _recycleCollection.ReplaceOneAsync(x => x.recycle_id == id, updatedRecycle);

    public async Task RemoveAsync(string id)
    {
        Recycle recycle = await _recycleCollection.Find(x => x.product_id == id).FirstOrDefaultAsync();
        if (recycle != null)
        {
            Inventory inventory = new Inventory();
            inventory.category = recycle.category;
            inventory.description = recycle.description;
            inventory.name = recycle.name;
            inventory.price = recycle.price;
            inventory.quantity = recycle.quantity;
            await _inventoryService.CreateAsync(inventory);
            await _recycleCollection.DeleteOneAsync(x => x.product_id == id);
        }

    }

}
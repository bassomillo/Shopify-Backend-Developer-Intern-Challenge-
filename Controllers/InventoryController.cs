using ShopifyChallengeBackEndApi.Models;
using ShopifyChallengeBackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ShopifyChallengeBackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public InventoryController(InventoryService inventoryService) =>
        _inventoryService = inventoryService;

    [HttpGet]
    public async Task<List<Inventory>> Get() =>
        await _inventoryService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Inventory>> Get(string id)
    {
        var inventory = await _inventoryService.GetAsync(id);

        if (inventory is null)
        {
            return NotFound();
        }

        return inventory;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Inventory newInventory)
    {
        await _inventoryService.CreateAsync(newInventory);

        return CreatedAtAction(nameof(Get), new { id = newInventory.product_id }, newInventory);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Inventory updatedInventory)
    {
        var inventory = await _inventoryService.GetAsync(id);

        if (inventory is null)
        {
            return NotFound();
        }

        updatedInventory.product_id = inventory.product_id;

        await _inventoryService.UpdateAsync(id, updatedInventory);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var inventory = await _inventoryService.GetAsync(id);

        if (inventory is null)
        {
            return NotFound();
        }

        await _inventoryService.RemoveAsync(id);

        return NoContent();
    }
}
using ShopifyChallengeBackEndApi.Models;
using ShopifyChallengeBackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ShopifyChallengeBackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _inventoryService;
    private readonly RecycleService _recycleService;

    public InventoryController(RecycleService recycleService, InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
        _recycleService = recycleService;
    }

    /// <summary>
    /// View all the inventories
    /// </summary>
    [HttpGet("all")]
    public async Task<List<Inventory>> Get() =>
        await _inventoryService.GetAsync();

    /// <summary>
    /// View a specific inventory by ID 
    /// </summary>
    [HttpGet("getByID")]
    public async Task<ActionResult<Inventory>> Get([FromQuery] string id)
    {
        var inventory = await _inventoryService.GetAsync(id);

        if (inventory is null)
        {
            return NotFound();
        }

        return inventory;
    }

    /// <summary>
    /// Create a new inventory, donot input the product_id value, just delete the first line when you try this API
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Post(Inventory newInventory)
    {
        await _inventoryService.CreateAsync(newInventory);

        return CreatedAtAction(nameof(Get), new { id = newInventory.product_id }, newInventory);
    }

    /// <summary>
    /// Update a specific inventory by ID 
    /// </summary>
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] string id, [FromQuery] Inventory updatedInventory)
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

    /// <summary>
    /// Deletes a specific inventory with comment and put it into the recycle bin.
    /// </summary>
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] string product_id, [FromQuery] string comment)
    {
        if(comment is null || comment.Equals("")){
            comment = "*";
        }
        await _recycleService.CreateAsync(product_id, comment);
        // var inventory = await _inventoryService.GetAsync(product_id);

        // if (inventory is null)
        // {
        //     return NotFound();
        // }

        // await _inventoryService.RemoveAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Undeletion of a specific inventory by product id 
    /// </summary>
    [HttpPost("recover")]
    public async Task<List<Inventory>> Recover([FromQuery] string product_id)
    {
        await _recycleService.RemoveAsync(product_id);
        return await _inventoryService.GetAsync();

    }
}

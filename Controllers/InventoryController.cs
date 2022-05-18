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


    [HttpGet("all")]
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

    [HttpPost("recover")]
    public async Task<List<Inventory>> Recover([FromQuery] string product_id)
    {
        await _recycleService.RemoveAsync(product_id);
        return await _inventoryService.GetAsync();

    }
}
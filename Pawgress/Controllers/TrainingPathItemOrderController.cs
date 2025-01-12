using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pawgress.Models;
using Pawgress.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pawgress.Data;

namespace Pawgress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingPathItemOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainingPathItemOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("TrainingPath/{trainingPathId}")]
        public async Task<IActionResult> GetItemsByTrainingPath(Guid trainingPathId)
        {
            var orders = await _context.TrainingPathItemOrders
                .Include(o => o.TrainingPathItem)
                .Where(o => o.TrainingPathId == trainingPathId)
                .OrderBy(o => o.Order)
                .ToListAsync();

            // Return an empty array if no items are found
            if (!orders.Any())
                return Ok(new List<object>());

            var result = orders.Select(o => new
            {
                o.Id,
                o.Order,
                TrainingPathItem = new
                {
                    o.TrainingPathItem.Id,
                    o.TrainingPathItem.CreationDate,
                    o.TrainingPathItem.UpdateDate,
                    Type = o.TrainingPathItem is Lesson ? "Lesson" : "Quiz"
                }
            });

            return Ok(result);
        }


        // POST: api/TrainingPathItemOrder
        [HttpPost]
        public async Task<IActionResult> AddItemToTrainingPath([FromBody] TrainingPathItemOrderDto dto)
        {
            
            var trainingPath = await _context.TrainingPaths.FindAsync(dto.TrainingPathId);
            if (trainingPath == null)
                return NotFound("Training path not found.");

            var item = await _context.TrainingPathItems.FindAsync(dto.TrainingPathItemId);
            if (item == null)
                return NotFound("Training path item not found.");

            var order = new TrainingPathItemOrder
            {
                Id = Guid.NewGuid(),
                TrainingPathId = dto.TrainingPathId,
                TrainingPathItemId = dto.TrainingPathItemId,
                Order = dto.Order
            };

            _context.TrainingPathItemOrders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemsByTrainingPath), new { trainingPathId = dto.TrainingPathId }, order);
        }

        // PUT: api/TrainingPathItemOrder/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] int newOrder)
        {
            var order = await _context.TrainingPathItemOrders.FindAsync(id);
            if (order == null)
                return NotFound("Training path item order not found.");

            order.Order = newOrder;

            _context.TrainingPathItemOrders.Update(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/TrainingPathItemOrder/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveItemFromTrainingPath(Guid id)
        {
            var order = await _context.TrainingPathItemOrders.FindAsync(id);
            if (order == null)
                return NotFound("Training path item order not found.");

            _context.TrainingPathItemOrders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/TrainingPathItemOrder/TrainingPath/{trainingPathId}
        [HttpDelete("TrainingPath/{trainingPathId}")]
        public async Task<IActionResult> ClearTrainingPathItems(Guid trainingPathId)
        {
            var orders = await _context.TrainingPathItemOrders
                .Where(o => o.TrainingPathId == trainingPathId)
                .ToListAsync();

            if (!orders.Any())
                return NotFound("No training path items found for this training path.");

            _context.TrainingPathItemOrders.RemoveRange(orders);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("TrainingPath/{trainingPathId}/Reorder")]
        public async Task<IActionResult> ReorderItems(Guid trainingPathId, [FromBody] List<TrainingPathItemOrderDto> items)
        {
            var existingOrders = await _context.TrainingPathItemOrders
                .Where(o => o.TrainingPathId == trainingPathId)
                .ToListAsync();

            foreach (var item in items)
            {
                var order = existingOrders.FirstOrDefault(o => o.TrainingPathItemId == item.TrainingPathItemId);
                if (order != null)
                {
                    order.Order = item.Order;
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
}

    }

    // DTO for creating/updating TrainingPathItemOrder
    public class TrainingPathItemOrderDto
    {
        public Guid TrainingPathId { get; set; }
        public Guid TrainingPathItemId { get; set; }
        public int Order { get; set; }
    }
}

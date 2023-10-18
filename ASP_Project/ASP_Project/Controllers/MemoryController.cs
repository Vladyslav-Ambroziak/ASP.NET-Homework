using ASP_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemoryController : ControllerBase
    {
        private readonly AspPhonesContext _dbContext;
        public MemoryController(AspPhonesContext dbContext) { _dbContext = dbContext; }

        [HttpGet]
        [Route("Get mamories")]
        public async Task<ActionResult<IEnumerable<Memory>>> GetAllMemories()
        {
            if (_dbContext.Memories == null)
                return BadRequest("List memories empty");

            return await _dbContext.Memories.ToListAsync();
        }

        [HttpPut]
        [Route("Add memory")]
        public async Task<ActionResult> AddMemory(int amount)
        {
            if (_dbContext.Memories.Any(x => x.Amount != amount))
            {
                _dbContext.Memories.Add(new Memory { Amount = amount });
                _dbContext.SaveChanges();
                return Ok(_dbContext.Memories.FirstOrDefault(x => x.Amount == amount));
            }
            return BadRequest("Memory already exist");
        }

        [HttpDelete]
        [Route("Delete memory")]
        public async Task<ActionResult> DeleteMemory(int amount)
        {
            
            if (_dbContext.Memories.Any(x => x.Amount == amount))
            {
                int memoryId = _dbContext.Memories.FirstOrDefault(x => x.Amount == amount).Id;
                foreach (var phone in _dbContext.Phones.Where(x => x.IdMemory == memoryId))
                    phone.IdMemory = null;
                _dbContext.Memories.Remove(_dbContext.Memories.FirstOrDefault(x => x.Amount == amount));
                _dbContext.SaveChanges();
                return Ok("Phone remove");
            }
            return BadRequest("Memory not found");
        }
        [HttpPut]
        [Route("Update memory")]
        public async Task<ActionResult> UpdatePhone(int Id, int amount)
        {
            if (_dbContext.Memories.Any(x => x.Id == Id))
            {
                _dbContext.Memories.FirstOrDefault(x => x.Id == Id).Amount = amount;
                _dbContext.SaveChanges();
                return Ok($"Memory [{Id}] update");
            }
            return BadRequest("Memory not exist");
        }
    }
}

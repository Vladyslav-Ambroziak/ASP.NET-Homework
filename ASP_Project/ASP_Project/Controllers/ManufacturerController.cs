using ASP_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManufacturerController : ControllerBase
    {
        private readonly AspPhonesContext _dbContext;
        public ManufacturerController(AspPhonesContext dbContext) { _dbContext = dbContext; }

        [HttpGet]
        [Route("Get manufacturers")]
        public async Task<ActionResult<IEnumerable<Manufacturer>>> GetAllManufacturers()
        {
            if (_dbContext.Memories == null)
                return BadRequest("List manufacturers empty");

            return await _dbContext.Manufacturers.ToListAsync();
        }

        [HttpPut]
        [Route("Add manufacturer")]
        public async Task<ActionResult> AddMemory(string companyName)
        {
            if (_dbContext.Manufacturers.Any(x => x.CompanyName != companyName))
            {
                _dbContext.Manufacturers.Add(new Manufacturer { CompanyName = companyName });
                _dbContext.SaveChanges();
                return Ok(_dbContext.Manufacturers.FirstOrDefault(x => x.CompanyName != companyName));
            }
            return BadRequest("Manufacturer already exist");
        }

        [HttpDelete]
        [Route("Delete manufacturer")]
        public async Task<ActionResult> DeleteMemory(string companyName)
        {
            if (_dbContext.Manufacturers.Any(x => x.CompanyName == companyName))
            {
                int manufacturerId = _dbContext.Manufacturers.FirstOrDefault(x => x.CompanyName == companyName).Id;
                AspPhonesContext forRemove = new AspPhonesContext();
                foreach (var phone in _dbContext.Phones.Where(x => x.IdManufacturer == manufacturerId))
                {
                    forRemove.Phones.Remove(phone);
                    forRemove.SaveChanges();
                }
                _dbContext.Manufacturers.Remove(_dbContext.Manufacturers.FirstOrDefault(x => x.CompanyName == companyName));
                _dbContext.SaveChanges();
                return Ok("Manufacturer remove");
            }
            return BadRequest("Manufacturer not found");
        }

        [HttpPut]
        [Route("Update manufacturer")]
        public async Task<ActionResult> UpdatePhone(string oldCompanyName, string newCompanyName)
        {
            if (_dbContext.Manufacturers.Any(x => x.CompanyName == oldCompanyName))
            {
                _dbContext.Manufacturers.FirstOrDefault(x => x.CompanyName == oldCompanyName).CompanyName = newCompanyName;
                _dbContext.SaveChanges();
                return Ok($"Manufacturer [{oldCompanyName}] update to [{newCompanyName}]");
            }
            return BadRequest("Manufacturer not exist");
        }
    }
}

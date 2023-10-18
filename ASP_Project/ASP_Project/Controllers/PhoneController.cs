using ASP_Project.Models;
using ASP_Project.ModelsConfguration;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneController : ControllerBase
    {
        private readonly AspPhonesContext _dbContext;
        public PhoneController(AspPhonesContext dbContext) { _dbContext = dbContext; }

        [HttpGet]
        [Route("Get phones")]
        public async Task<ActionResult<IEnumerable<PhoneCnfg>>> GetAll()
        {
            if (_dbContext.Phones == null)
                return BadRequest("List phones empty");

            List<PhoneCnfg> listPhones = new List<PhoneCnfg>();
            foreach (var phone in _dbContext.Phones)
            {
                AspPhonesContext aspPhonesContext = new AspPhonesContext();
                listPhones.Add(new PhoneCnfg(
                    phone.Id,
                    aspPhonesContext.Manufacturers.FirstOrDefault(x => x.Id == phone.IdManufacturer).CompanyName,
                    phone.Series,
                    aspPhonesContext.Memories.FirstOrDefault(x => x.Id == phone.IdMemory),
                    phone.Color,
                    phone.BatteryCapacity,
                    phone.ScreenDiagonal,
                    phone.Camera,
                    phone.Price
                    ));
            }

            return listPhones.ToList();
        }

        [HttpPost]
        [Route("Add phone")]
        public async Task<ActionResult> AddPhone(PhoneCnfg phone)
        {
            if (phone.MemoryPhone == 0)
                phone.MemoryPhone = null;
            if (!_dbContext.Manufacturers.Any(x => x.CompanyName == phone.Manufacturer))
                return BadRequest("Manufacturer not registered");
            if (phone.MemoryPhone != null)
                if (!_dbContext.Memories.Any(x => x.Amount == phone.MemoryPhone))
                    return BadRequest("Memory not exist in database");
            if (phone.Series == "")
                return BadRequest("Indicate the phone series");
            if (phone.BatteryCapacity == null || phone.BatteryCapacity == 0)
                return BadRequest("Indicate the battery capacity");
            if (phone.ScreenDiagonal == null || phone.ScreenDiagonal == 0)
                return BadRequest("Indicate the screen diagonal");
            if (phone.Price == null)
                return BadRequest("Indicate the price");

            AspPhonesContext searchId = new AspPhonesContext();
            int? memoryId;
            if (phone.MemoryPhone == null)
                memoryId = null;
            else
                memoryId = searchId.Memories.FirstOrDefault(x => x.Amount == phone.MemoryPhone).Id;

            _dbContext.Phones.Add(
                new Phone
                {
                    IdManufacturer = searchId.Manufacturers.FirstOrDefault(x => x.CompanyName == phone.Manufacturer).Id,
                    Series = phone.Series,
                    IdMemory = memoryId,
                    Color = phone.Color,
                    BatteryCapacity = phone.BatteryCapacity,
                    ScreenDiagonal = phone.ScreenDiagonal,
                    Camera = phone.Camera,
                    Price = phone.Price
                }
                );
            _dbContext.SaveChanges();
            return Ok(phone);
        }

        [HttpDelete]
        [Route("Delete phone")]
        public async Task<ActionResult> DeletePhone(string nameManufacturer, string nameSeries)
        {
            if (_dbContext.Manufacturers.Any(x => x.CompanyName == nameManufacturer))
            {
                int manufacturerId = _dbContext.Manufacturers.FirstOrDefault(x => x.CompanyName == nameManufacturer).Id;
                if (_dbContext.Phones.Any(x => x.IdManufacturer == manufacturerId && x.Series == nameSeries))
                {
                    AspPhonesContext searchPhoneForRemove = new AspPhonesContext();
                    foreach (var phone in searchPhoneForRemove.Phones.Where(x => x.IdManufacturer == manufacturerId && x.Series == nameSeries))
                    {
                        _dbContext.Phones.Remove(phone);
                        _dbContext.SaveChanges();
                    }
                    return Ok("Phone remove");
                }
                return NotFound("Phone's series not found");
            }
            return NotFound("Manufacturer not exist in database");
        }

        [HttpPost]
        [Route("Update phone")]
        public async Task<ActionResult> UpdatePhone(PhoneCnfg phone)
        {
            if (_dbContext.Phones.Any(x => x.Id == phone.Id))
            {
                if (phone.MemoryPhone == 0)
                    phone.MemoryPhone = null;
                if (!_dbContext.Manufacturers.Any(x => x.CompanyName == phone.Manufacturer))
                    return BadRequest("Manufacturer not registered");
                if (phone.MemoryPhone != null)
                    if (!_dbContext.Memories.Any(x => x.Amount == phone.MemoryPhone))
                        return BadRequest("Memory not exist in database");
                if (phone.Series == "")
                    return BadRequest("Indicate the phone series");
                if (phone.BatteryCapacity == null || phone.BatteryCapacity == 0)
                    return BadRequest("Indicate the battery capacity");
                if (phone.ScreenDiagonal == null || phone.ScreenDiagonal == 0)
                    return BadRequest("Indicate the screen diagonal");
                if (phone.Price == null)
                    return BadRequest("Indicate the price");

                AspPhonesContext searchId = new AspPhonesContext();
                int? memoryId;
                if (phone.MemoryPhone == null)
                    memoryId = null;
                else
                    memoryId = searchId.Memories.FirstOrDefault(x => x.Amount == phone.MemoryPhone).Id;

                Phone newPhone = new Phone
                {
                    Id = phone.Id,
                    IdManufacturer = searchId.Manufacturers.FirstOrDefault(x => x.CompanyName == phone.Manufacturer).Id,
                    Series = phone.Series,
                    IdMemory = memoryId,
                    Color = phone.Color,
                    BatteryCapacity = phone.BatteryCapacity,
                    ScreenDiagonal = phone.ScreenDiagonal,
                    Camera = phone.Camera,
                    Price = phone.Price
                };


                _dbContext.Phones.Update(newPhone);
                _dbContext.SaveChanges();
                return Ok("Phone update");
            }
            return BadRequest("not found phone");
        }

        [HttpPut]
        [Route("Get phones by manufacturer")]
        public async Task<ActionResult<IEnumerable<PhoneCnfg>>> GetManufacturer(string nameManufacturer)
        {
            if (!_dbContext.Manufacturers.Any(x => x.CompanyName == nameManufacturer))
                return BadRequest("Manufacturer not registered");

            int manufacturerId = _dbContext.Manufacturers.FirstOrDefault(x => x.CompanyName == nameManufacturer).Id;
            List<PhoneCnfg> listPhones = new List<PhoneCnfg>();
            foreach (var phone in _dbContext.Phones.Where(x => x.IdManufacturer == manufacturerId))
            {
                AspPhonesContext aspPhonesContext = new AspPhonesContext();
                listPhones.Add(new PhoneCnfg(
                    phone.Id,
                    aspPhonesContext.Manufacturers.FirstOrDefault(x => x.Id == phone.IdManufacturer).CompanyName,
                    phone.Series,
                    aspPhonesContext.Memories.FirstOrDefault(x => x.Id == phone.IdMemory),
                    phone.Color,
                    phone.BatteryCapacity,
                    phone.ScreenDiagonal,
                    phone.Camera,
                    phone.Price
                    ));
            }

            return listPhones.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using currentweather.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

class IgnoreJsonAttributesResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
        foreach (var prop in props)
        {
            prop.Ignored = false;   // Ignore [JsonIgnore]
            prop.Converter = null;  // Ignore [JsonConverter]
            prop.PropertyName = prop.UnderlyingName;  // restore original property name
        }
        return props;
    }
}

namespace currentweather.Controllers
{
    [AllowAnonymous]
    //    [Authorize(Policy = "PCMUsersOnly")]
    [Authorize]
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class pcm_customerController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public pcm_customerController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/pcm_customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pcm_customer>>> Getpcm_customer()
        {

            return await _context.pcm_customer.ToListAsync();
        }

        // GET: api/pcm_customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pcm_customer>> Getpcm_customer(long id)
        {
            var pcm_customer = await _context.pcm_customer.FindAsync(id);

            if (pcm_customer == null)
            {
                return NotFound();
            }

            return pcm_customer;
        }

        // GET: api/pcm_customer/5/photo
        [HttpGet("{id}/photo")]
        public async Task<string> Getpcm_customerphoto(long id)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new IgnoreJsonAttributesResolver();
            settings.Formatting = Formatting.Indented;

//            string json = JsonConvert.SerializeObject(foo, settings);

            var pcm_customer = await _context.pcm_customer.FindAsync(id);

            if (pcm_customer == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(pcm_customer, settings);
        }

        // PUT: api/pcm_customer/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpcm_customer(long id, pcm_customer pcm_customer)
        {
            if (id != pcm_customer.id)
            {
                return BadRequest();
            }

            _context.Entry(pcm_customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pcm_customerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/pcm_customer
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<pcm_customer>> Postpcm_customer(pcm_customer pcm_customer)
        {
            _context.pcm_customer.Add(pcm_customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpcm_customer", new { id = pcm_customer.id }, pcm_customer);
        }

        // DELETE: api/pcm_customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<pcm_customer>> Deletepcm_customer(long id)
        {
            var pcm_customer = await _context.pcm_customer.FindAsync(id);
            if (pcm_customer == null)
            {
                return NotFound();
            }

            _context.pcm_customer.Remove(pcm_customer);
            await _context.SaveChangesAsync();

            return pcm_customer;
        }

        private bool pcm_customerExists(long id)
        {
            return _context.pcm_customer.Any(e => e.id == id);
        }
    }
}

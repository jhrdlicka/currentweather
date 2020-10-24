using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using currentweather.Models;

namespace currentweather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarDaysController : ControllerBase
    {
        private readonly CurrentWeatherContext _context;

        public CalendarDaysController(CurrentWeatherContext context)
        {
            _context = context;
        }

        // GET: api/CalendarDays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarDay>>> GetCalendarDay()
        {
            return await _context.CalendarDay.ToListAsync();
        }

        // GET: api/CalendarDays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarDay>> GetCalendarDay(long id)
        {
            var calendarDay = await _context.CalendarDay.FindAsync(id);

            if (calendarDay == null)
            {
                return NotFound();
            }

            return calendarDay;
        }

        // PUT: api/CalendarDays/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarDay(long id, CalendarDay calendarDay)
        {
            if (id != calendarDay.Id)
            {
                return BadRequest();
            }

            _context.Entry(calendarDay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarDayExists(id))
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

        // POST: api/CalendarDays
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CalendarDay>> PostCalendarDay(CalendarDay calendarDay)
        {
            _context.CalendarDay.Add(calendarDay);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCalendarDay), new { id = calendarDay.Id }, calendarDay);
            //return CreatedAtAction("GetCalendarDay", new { id = calendarDay.Id }, calendarDay);
        }

        // DELETE: api/CalendarDays/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CalendarDay>> DeleteCalendarDay(long id)
        {
            var calendarDay = await _context.CalendarDay.FindAsync(id);
            if (calendarDay == null)
            {
                return NotFound();
            }

            _context.CalendarDay.Remove(calendarDay);
            await _context.SaveChangesAsync();

            return calendarDay;
        }

        private bool CalendarDayExists(long id)
        {
            return _context.CalendarDay.Any(e => e.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viber_bot.Models;

[Route("api/[controller]")]
[ApiController]
public class WalksController : ControllerBase
{
    private readonly WalkDbContext _context;

    public WalksController(WalkDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Walk>>> GetWalks()
    {
        return await _context.Walks.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Walk>> GetWalk(int id)
    {
        var walk = await _context.Walks.FindAsync(id);

        if (walk == null)
        {
            return NotFound();
        }

        return walk;
    }

    [HttpPost]
    public async Task<ActionResult<Walk>> PostWalk(Walk walk)
    {
        _context.Walks.Add(walk);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWalk), new { id = walk.Id }, walk);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutWalk(int id, Walk walk)
    {
        if (id != walk.Id)
        {
            return BadRequest();
        }

        _context.Entry(walk).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WalkExists(id))
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWalk(int id)
    {
        var walk = await _context.Walks.FindAsync(id);
        if (walk == null)
        {
            return NotFound();
        }

        _context.Walks.Remove(walk);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool WalkExists(int id)
    {
        return _context.Walks.Any(e => e.Id == id);
    }

    private double CalculateDistance(Walk walk)
    {
        const double EarthRadius = 6371; // Радіус Землі в км
        var lat1 = walk.Latitude;
        var lon1 = walk.Longitude;
        var lat2 = walk.Latitude;
        var lon2 = walk.Longitude;
        var dLat = Deg2Rad(lat2 - lat1);
        var dLon = Deg2Rad(lon2 - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = EarthRadius * c; // Дистанція в км
        return distance;
    }

    // Перетворюємо градуси в радіани
    private double Deg2Rad(double deg)
    {
        return deg * (Math.PI / 180);
    }

    // Обчислюємо час (у хвилинах) для певної прогулянки
    private double CalculateTime(Walk walk)
    {
        var startTime =
            _context.Walks.Where(w => w.Date_track.Date == walk.Date_track.Date && w.Date_track < walk.Date_track)
                .OrderByDescending(w => w.Date_track).FirstOrDefault()?.Date_track ?? walk.Date_track;
        var endTime = walk.Date_track;
        var time = (endTime - startTime).TotalMinutes;
        return time;
    }

    // Топ 10 прогулянок
    [HttpGet("walks/{imei}")]
    public async Task<ActionResult> GetWalks(string imei)
    {
        var walks = await _context.Walks.Where(w => w.IMEI == imei).ToListAsync();

        var totalWalks = walks.Count();
        var totalKilometers = walks.Sum(w => w.Kilometers);
        var totalMinutes = walks.Sum(w => w.Minutes);

        var topWalks = walks.OrderByDescending(w => w.Kilometers).Take(10).ToList();

        var response = new
        {
            TotalWalks = totalWalks,
            TotalKilometers = totalKilometers,
            TotalMinutes = totalMinutes,
            TopWalks = topWalks
        };

        return Ok(response);
    }
}
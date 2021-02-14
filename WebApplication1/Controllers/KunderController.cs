using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KunderController : Controller
    {
        private readonly BankContext _context;

        public KunderController(BankContext context)
        {
            _context = context;
        }

        // GET: api/Kunder/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKunde(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kunde = await _context.Kunder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kunde == null)
            {
                return NotFound();
            }

            return Ok(kunde);
        }

        // POST: api/Kunder
        [HttpPost]
        public async Task<IActionResult> Register(Kunde kunde)
        {
            if (KundeExists(kunde))
            {
                Console.WriteLine("Kunden eksisterer " + kunde.Id);
                return BadRequest();
            }
            _context.Add(kunde);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Kunder/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kunde = await _context.Kunder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kunde == null)
            {
                return NotFound();
            }
            //Fjerner kunden
            _context.Kunder.Remove(kunde);

            //Fjerner alle assosierte lån med denne kunden
            //For utprøving
            var laan = await _context.Laan.Where(l => l.KundeId == kunde.Id).ToListAsync();
            _context.Laan.RemoveRange(laan);

            //lagrer endringene
            await _context.SaveChangesAsync();
            

            return Ok();
        }


        private bool KundeExists(Kunde kunde)
        {
            return _context.Kunder.Any(e => e.Id == kunde.Id || 
                (e.Fornavn.ToUpper().Equals(kunde.Fornavn.ToUpper()) && 
                 e.Etternavn.ToUpper().Equals(kunde.Etternavn.ToUpper())));
        }
    }
}

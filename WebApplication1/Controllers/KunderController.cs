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
     ///<summary>
     /// API for å hente info om kunder.
     /// For nå er det bare for å hente for- og etternavn på kunder gitt kundeNr.
     /// I tillegg til kan en slette kunder og deres tilhørende lån(for testing), og opprette nye kunder gitt at kundeID ikke er i bruk og kunden ikke er registrert fra fø
    ///</summary>
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
        /// <summary>
        /// Henter kunden med gitt kundeId
        /// </summary>
        /// <param name="id">id til kunden</param>
        /// <returns>
        /// statuskode 200 med kunden, 404 hvis det ikke er gitt noen id, eller id-en ikke samsvarer med en kunde
        /// </returns>
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
        /// <summary>
        /// Registrerer en kunde
        /// </summary>
        /// <param name="kunde">kunden som skal registrererss</param>
        /// <returns>
        /// statuskode 200 om kunden ble registrert. 400 om kundeId-en er i bruk eller om kunden har en bruker fra før
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Register(Kunde kunde)
        {
            //Sjekker om kundeId-en er i bruk eller om kunden har en bruker fra før
            if (KundeExists(kunde) || _context.Kunder.Any(k => k.Fornavn.Equals(kunde.Fornavn) && k.Etternavn.Equals(kunde.Etternavn)) )
            {
                //Console.WriteLine("Kunden eksisterer " + kunde.Id);
                return BadRequest();
            }
            _context.Add(kunde);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Kunder/{id}
        /// <summary>
        /// Fjerner kunden og alle lån som er koblet til denne kunden
        /// </summary>
        /// <param name="id">ID til kunden</param>
        /// <returns>
        /// statuskode 200 om kunden ble fjernet. 404 om Kunden ikke finnes
        /// </returns>
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

        /// <summary>
        /// Sjekker at Id-en ikke er i bruk og at kunden ikke er blitt registert fra før
        /// </summary>
        /// <param name="kunde">kunden som skal sjekkes</param>
        /// <returns>true om kunden eksisterer</returns>

        private bool KundeExists(Kunde kunde)
        {
            return _context.Kunder.Any(e => e.Id == kunde.Id);
        }
     
    }
}

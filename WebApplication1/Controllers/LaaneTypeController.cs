using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{


    /// <summary>
    /// API for å håndtere lånetyper
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LaaneTypeController : Controller
    {
        private readonly BankContext _context;

        public LaaneTypeController(BankContext context)
        {
            _context = context;
        }


        //GET api/LaaneType
        /// <summary>
        /// Returnerer alle lånetypene som fins
        /// </summary>
        /// <returns>
        /// statuskode 200 og en liste med alle lånetypene
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaaneType>>> GetLåneTyper()
        {
            return Ok(await _context.LaaneTyper.ToListAsync());
        }


        //GET api/LaaneType/{id}
        /// <summary>
        /// Henter lånetypen med den gitte id-en
        /// </summary>
        /// <param name="id">id til lånetypen</param>
        /// <returns>statuskode 200 og lånetypen, 404 om id mangler eller om id ikke samsvarer med en lånetype</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLåneType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var låneType = await _context.LaaneTyper
                .FirstOrDefaultAsync(m => m.Id == id);
            if (låneType == null)
            {
                return NotFound();
            }

            return Ok(låneType);
        }


        // POST: api/LaaneType
        /// <summary>
        /// Legger til en ny lånetype
        /// </summary>
        /// <param name="laaneType">lånetypen som skal legges til</param>
        /// <returns>
        /// statuskode 200 om den ble lagt til, 400 om iden allerede er i bruk
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<LaaneType>> PostLåneType(LaaneType laaneType)
        {
            if (LåneTypeExists(laaneType.Id))
            {
                return BadRequest();
            }
            _context.LaaneTyper.Add(laaneType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLåneType), new { Id = laaneType.Id }, laaneType);
        }


        //DELETE api/LaaneType/{id}
        /// <summary>
        /// fjerner lånetypen med den gitte id-en
        /// </summary>
        /// <param name="id">id til lånetypen</param>
        /// <returns>
        /// statuskode 200 om den ble fjernet, 400 om den ikke eksisterer fra før eller om det ikke er gitt noen id
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLåneType(int? id)
        {
            if (!id.HasValue || !LåneTypeExists(id.Value))
            {
                return BadRequest();
            }
            var type = await _context.LaaneTyper.FirstOrDefaultAsync(p => p.Id == id);
            if (type == null)
            {
                return BadRequest();
            }
            _context.LaaneTyper.Remove(type);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// sjekker om en gitt id samsvarer til en lånetype
        /// </summary>
        /// <param name="id">id-en som vi skal sjekke</param>
        /// <returns>true om den samsvarer med en lånetype, feil ellers</returns>
        private bool LåneTypeExists(int id)
        {
            return _context.LaaneTyper.Any(e => e.Id == id);
        }
    }
}

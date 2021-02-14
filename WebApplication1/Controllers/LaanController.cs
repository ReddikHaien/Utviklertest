using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.KlientRequest;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaanController : Controller
    {

        private readonly BankContext _context;

        public LaanController(BankContext context)
        {
            _context = context;
        }

        [HttpGet("Laan")]
        public async Task<ActionResult<IEnumerable<Laan>>> getLån()
        {
            var laan = await _context.Laan.ToListAsync();
            laan.ForEach(l => FinnKundeOgType(l));
            return laan;
        }

        [HttpGet("Laan/{lånID}")]
        public async Task<ActionResult> getLån(int? lånID)
        {
            if (!lånID.HasValue) return NotFound();

            var laan = await _context.Laan.FirstOrDefaultAsync(l => l.Id == lånID);

            if (laan == null) return NotFound();

            FinnKundeOgType(laan);

            return Ok(laan);
        }

        [HttpGet("Kunde/{kundeID}")]
        public async Task<ActionResult> GetLånForKunde(int? kundeID)
        {
            if (!kundeID.HasValue) Console.WriteLine("ingen verdi gitt!!");
            Console.WriteLine("henter lån hos kunde " + kundeID.ToString());
            if (!kundeID.HasValue || !KundeExists(kundeID.Value)) return NotFound();

            var laan = _context.Laan.Where(k => k.Kunde.Id == kundeID);

            await laan.ForEachAsync(l => FinnKundeOgType(l));

            return Ok(laan.ToList());

        }


        [HttpGet("Type/{typeID}")]
        public async Task<ActionResult> GetLånOfType(int? typeID)
        {
            Console.WriteLine("fikk forespørsel for " + typeID.ToString());
            if (!typeID.HasValue || !LaaneTypeExists(typeID.Value)) return NotFound();

            var laan = _context.Laan.Where(l => l.LaaneType.Id == typeID);
            await laan.ForEachAsync(l => FinnKundeOgType(l) );

            return Ok(laan.ToList());
        }


        [HttpPost("TaOpp")]
        public async Task<ActionResult> TaOppLån(LaanRequest forespørsel)
        {
            //sjekker at forespørselen er godkjent
            if (!forespørsel.Valider(_context)) return BadRequest();

            Kunde kunde = await _context.Kunder.FirstAsync(k => k.Id == forespørsel.KundeId);
            LaaneType type = await _context.LaaneTyper.FirstAsync(t => t.Id == forespørsel.LaaneTypeId);
                       
            DateTime dato = forespørsel.Dato.HasValue ? forespørsel.Dato.Value : DateTime.Now;
            decimal LaaneSum = forespørsel.LaaneSum;
            int år = forespørsel.Aar;



            _context.Laan.Add(new Laan
            {
                KundeId = kunde.Id,
                LaaneTypeId = type.Id,
                LaaneDato = dato,
                ForrigeBetaling = dato,
                Aar = forespørsel.Aar,
                LaaneSum = LaaneSum,
                Innbetalt = 0m,
            }); ;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private async void FinnKundeOgType(Laan laan)
        {
            laan.Kunde = await _context.Kunder.FirstOrDefaultAsync(k => k.Id == laan.KundeId);
            laan.LaaneType = await _context.LaaneTyper.FirstOrDefaultAsync(t => t.Id == laan.LaaneTypeId);
        }
        
        private bool KundeExists(int kundeID)
        {
            return _context.Kunder.Any( k => k.Id == kundeID );
        }

        private bool LaaneTypeExists(int typeID)
        {
            return _context.LaaneTyper.Any( t => t.Id == typeID );
        }
    }
}

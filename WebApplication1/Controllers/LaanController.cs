using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.KlientRequest;
using WebApplication1.Controllers.SkjemaTyper;
namespace WebApplication1.Controllers
{

    /// <summary>
    /// API for å ta opp lån og å hente inn lån.
    /// Litt merkelig api siden den er api/Laan/{action}/{ id}. Sikkert en MYE bedre måte å strukturere dette
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LaanController : Controller
    {

        private readonly BankContext _context;

        public LaanController(BankContext context)
        {
            _context = context;
        }

        // GET api/Laan/Laan
        /// <summary>
        /// Henter alle lånene som finnes i databasen
        /// </summary>
        /// <returns>
        /// statuskode 200 og alle lånene
        /// </returns>
        [HttpGet("Laan")]
        public async Task<ActionResult<IEnumerable<Laan>>> getLån()
        {
            var laan = await _context.Laan.ToListAsync();

            //fyller inn feltene Kunde og LaaneType
            laan.ForEach(l => FinnKundeOgType(l));
            return Ok(laan);
        }

        // GET api/Laan/Laan/{id}
        /// <summary>
        /// Henter lånet med den gitte id-en
        /// </summary>
        /// <param name="lånID">id til lånet</param>
        /// <returns>
        /// statuskode 200 om lånet finnes. 404 om LåneId ikke ble gitt eller låneId ikke samsvarer med et lån
        /// </returns>
        [HttpGet("Laan/{lånID}")]
        public async Task<ActionResult> getLån(int? lånID)
        {
            if (!lånID.HasValue) return NotFound();

            var laan = await _context.Laan.FirstOrDefaultAsync(l => l.Id == lånID);

            if (laan == null) return NotFound();

            FinnKundeOgType(laan);

            return Ok(laan);
        }

        // GET api/Laan/Kunde/{id}
        /// <summary>
        /// Henter alle lån som tilhører den gitte kunden
        /// </summary>
        /// <param name="kundeID">Id til kunde</param>
        /// <returns>
        /// statuskode 200 og lånene til kunden. Om kundeID mangler eller kunden ikke eksisterer så returnerer den statuskoden 404
        /// </returns>
        [HttpGet("Kunde/{kundeID}")]
        public async Task<ActionResult> GetLånForKunde(int? kundeID)
        {
            //if (!kundeID.HasValue) Console.WriteLine("ingen verdi gitt!!");
            //Console.WriteLine("henter lån hos kunde " + kundeID.ToString());
            
            //sjekker om kunden finnes
            if (!kundeID.HasValue || !KundeExists(kundeID.Value)) return NotFound();


            var laan = _context.Laan.Where(k => k.Kunde.Id == kundeID);


            await laan.ForEachAsync(l => FinnKundeOgType(l));

            return Ok(laan.ToList());

        }

        // GET api/Laan/Type/{id}
        /// <summary>
        /// Henter alle lån som har den gitte lånetypen
        /// </summary>
        /// <param name="typeID">id til Lånetypen</param>
        /// <returns>
        /// statuskode 200 med låntypen om den eksisterer eller statuskode 404 om id mangler eller den ikke samsvarer med noen lånetype
        /// </returns>
        [HttpGet("Type/{typeID}")]
        public async Task<ActionResult> GetLånOfType(int? typeID)
        {
            //Console.WriteLine("fikk forespørsel for " + typeID.ToString());
            if (!typeID.HasValue || !LaaneTypeExists(typeID.Value)) return NotFound();

            var laan = _context.Laan.Where(l => l.LaaneType.Id == typeID);
            await laan.ForEachAsync(l => FinnKundeOgType(l) );

            return Ok(laan.ToList());
        }

        // POST api/Laan/TaOpp
        /// <summary>
        /// Oppretter et nytt lån for den gitte kunden.
        /// Forespørselen blir validert ved å sjekke at kunden og typen eksisterer, 
        /// og at nedbetalingsår og sum er over 0
        /// </summary>
        /// <param name="forespørsel">forespørselen kunden har sendt</param>
        /// <returns>
        /// statuskode 200 om lånet ble opprettet eller statuskode 400 om info mangler eller ikke er godkjent
        /// </returns>

        [HttpPost("TaOpp")]
        public async Task<ActionResult> TaOppLån(LaanRequest forespørsel)
        {
            //sjekker at forespørselen er godkjent
            if (forespørsel == null ||  !forespørsel.Valider(_context)) return BadRequest();

            //Henter kundeinfo og lånetypen
            Kunde kunde = await _context.Kunder.FirstAsync(k => k.Id == forespørsel.KundeId);
            LaaneType type = await _context.LaaneTyper.FirstAsync(t => t.Id == forespørsel.LaaneTypeId);
            
            //oppretter et nytt datoobject om det ikker er spesifisert noen(testing slik at eg kan opprette lån på tidligere tidspunkt)
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


        
        /// <summary>
        /// Fyller <paramref name="laan"/>.Kunde og <paramref name="laan"/>.LaaneType med sine respektive verdier
        /// </summary>
        /// <param name="laan">Lånet som skal fylles</param>
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

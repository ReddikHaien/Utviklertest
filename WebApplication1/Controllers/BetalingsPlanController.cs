    using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApplication1.Models;
using WebApplication1.Models.KlientRequest;
using WebApplication1.Models.ServerResultat;
using WebApplication1.Controllers.SkjemaTyper;
namespace WebApplication1.Controllers
{
    ///<summary>
    /// API for å lage betalingsplaner.
    /// Brukeren sender inn en POST til api/BetalingsPlan og får en 
    /// WebApplication1.Models.ServerResultat.BetalingsPlan tilbake
    ///</summary>

    [Route("api/[controller]")]
    [ApiController]
    public class BetalingsPlanController : ControllerBase
    {
        private readonly BankContext _context;

        public BetalingsPlanController(BankContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Tar inn en BetalingsInfo og sender tilbake en BetalingsPlan. 
        /// Lånetypen blir mottatt i form av en id
        /// </summary>
        /// <param name="info">info om lånet som skal beregnes</param>
        /// <returns>
        /// statuskode 200 og en betalingsplan, om infoen mangler returneres statuskoden 400, og om lånetypen ikke finnes returneres statuskode 404 
        /// </returns>
        [HttpPost]
        public ActionResult<BetalingsPlan> PostBetalingsPlan(BetalingsPlanInfo info)
        {
            if (info == null)
            {
                return BadRequest();
            }

            var type = _context.LaaneTyper.FirstOrDefault(p => p.Id == info.LaaneTypeId);

            if (type == null)
            {
                return NotFound("fant ikke laanetypen med id " + info.LaaneTypeId);
            }

            AbstractSkjema skjema = SkjemaController.getSkjemaObj(info.SkjemaId);
            
            if (skjema == null)
            {
                return NotFound("Fant ikke skjematype med id " + info.SkjemaId);
            }

            //innputvariabler
            decimal rente = type.Rente/1200m;
            decimal totalSum = info.Sum;
            int mnd = info.Aar * 12;

            MaanedsPris[] pris = skjema.GetBetalingsPlan(totalSum, mnd, rente);

            decimal fullBetaling = 0;

            for (int i = 0; i < pris.Length; i++)
            {
                fullBetaling += pris[i].Rente + pris[i].Avdrag;
            }

            return Ok(new BetalingsPlan {
                LaaneTypen = type,
                Betalinger = pris,
                TotalSum = fullBetaling
            }) ;
        }
    }
}

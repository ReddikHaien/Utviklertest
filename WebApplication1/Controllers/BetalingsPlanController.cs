using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApplication1.Models;
using WebApplication1.Models.KlientRequest;
using WebApplication1.Models.ServerResultat;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BetalingsPlanController : ControllerBase
    {
        private readonly BankContext _context;

        public BetalingsPlanController(BankContext context)
        {
            _context = context;
        }

        // Post api/DataPlan
        [HttpPost]
        public ActionResult<BetalingsPlan> PostBetalingsPlan(BetalingsPlanInfo info)
        {
            if (info == null)
            {
                return BadRequest();
            }

            var type = _context.LaaneTyper.FirstOrDefault(p => p.Id == info.LaaneTypeId);

            decimal rente = type.Rente/36500m;
            decimal totalSum = info.Sum;
            int år = info.Aar;
            int mnd = år * 12;

            decimal fastSum = totalSum / mnd;

            decimal[] månedsPris = new decimal[mnd];

            decimal totalSumMedRente = totalSum;
            for (int i = 0; i < mnd; i++)
            {
                decimal renteForMnd = (totalSum - fastSum * i) * rente;
                månedsPris[i] = renteForMnd + fastSum;
                totalSumMedRente += renteForMnd;
            }


            return Ok(new BetalingsPlan { FastMndPris = fastSum, LaaneTypen = type, TotalMndPris = månedsPris, TotalSum = totalSumMedRente });
        }
    }
}

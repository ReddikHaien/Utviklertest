using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Controllers.SkjemaTyper;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkjemaController : Controller
    {
        private static readonly AbstractSkjema[] skjemaer = new AbstractSkjema[]
        {
            new SerielånSkjema(),
            new AnnuitetslånSkjema(),
        };

        //Setter opp id til skjematypene slik at skjemaer[skjema.id] == skjema
        static SkjemaController() {
            for (int i = 0; i < skjemaer.Length; i++)
            {
                skjemaer[i].Id = i;
            }
        }

        [HttpGet]
        public ActionResult GetSkjemaer()
        {
            return Ok(skjemaer);
        }

        [HttpGet("{id}")]
        public ActionResult GetSkjema(int? id)
        {
            if (!id.HasValue || id.Value < 0 || id.Value >= skjemaer.Length)
            {
                return NotFound();
            }

            return Ok(skjemaer[id.Value]);
        }

        public static AbstractSkjema getSkjemaObj(int id)
        {
            if (id < 0 || id >= skjemaer.Length)
            {
                return null;
            }

            return skjemaer[id];
        }
    }
}

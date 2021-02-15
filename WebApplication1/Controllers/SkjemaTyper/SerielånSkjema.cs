using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers.SkjemaTyper
{
    public class SerielånSkjema : AbstractSkjema
    {
        public SerielånSkjema() : base("Serielån") {}

        public override MaanedsPris[] GetBetalingsPlan(decimal sum, int måneder, decimal rente)
        {
            MaanedsPris[] plan = new MaanedsPris[måneder];

            decimal gjenstående = sum;
            decimal prisPerMåned = sum / måneder;

            for (int i = 0; i < måneder; i++)
            {
                plan[i] = new MaanedsPris
                {
                    Avdrag = prisPerMåned,
                    Rente = gjenstående * rente,
                };
            gjenstående -= prisPerMåned;
            }


            return plan;
        }
    }
}

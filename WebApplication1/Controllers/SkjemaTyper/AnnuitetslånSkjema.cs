using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers.SkjemaTyper
{
    public class AnnuitetslånSkjema : AbstractSkjema
    {
        public AnnuitetslånSkjema(): base("Annuitetslån"){}

        /// <summary>
        /// Lager en betalingsplan som har en fast sum i måneden
        /// </summary>
        /// <param name="sum"> total lånesum</param>
        /// <param name="måneder"> antall måneder</param>
        /// <param name="rente">renten på lånet i % per måned</param>
        /// <returns> en tabell med betalinger per måned </returns>
        public override MaanedsPris[] GetBetalingsPlan(decimal sum, int måneder, decimal rente)
        {
            MaanedsPris[] plan = new MaanedsPris[måneder];

                decimal EMI = sum * rente * ((decimal)Math.Pow(1 + (double)rente, måneder) / (decimal)(Math.Pow(1 + (double)rente, måneder) - 1));

            decimal gjenstående = sum;
            for (int i = 0; i < måneder; i++)
            {
                decimal renteB = gjenstående * rente;
                plan[i] = new MaanedsPris
                {
                    Rente = renteB,
                    Avdrag = EMI - renteB
                };
                gjenstående -= (EMI - renteB);
            }

            return plan;

        }
    }
}

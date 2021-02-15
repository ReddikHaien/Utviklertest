using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers.SkjemaTyper
{

    public class MaanedsPris
    {
        public decimal Avdrag { get; set; }
        public decimal Rente { get; set; }
    }
    public abstract class AbstractSkjema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AbstractSkjema(string name)
        {
            this.Name = name;
        }
        /// <summary>
        /// Lager en betalingsplan i form av en tabell med betalinger for hver måned
        /// </summary>
        /// <param name="sum">den totale lånesummen</param>
        /// <param name="måneder">antall måneder for å betale ned</param>
        /// <param name="rente">renten per måned i %</param>
        /// <returns></returns>
        public abstract MaanedsPris[] GetBetalingsPlan(decimal sum, int måneder, decimal rente);
    }
}

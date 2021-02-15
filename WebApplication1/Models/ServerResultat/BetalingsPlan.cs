using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Controllers.SkjemaTyper;

namespace WebApplication1.Models.ServerResultat
{
    public class BetalingsPlan
    {   
        //Den totale lånesummen
        public decimal TotalSum { get; set; }

        //Den valgte Lånetypen
        public LaaneType LaaneTypen { get; set; }

        //Det valgte skjemaet
        public AbstractSkjema Skjema { get; set; }

        //En Liste med den totale prisen per måned(fast + rente)
        public MaanedsPris[] Betalinger { get; set; }
    }
}

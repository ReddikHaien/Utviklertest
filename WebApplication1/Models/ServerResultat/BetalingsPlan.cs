using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.ServerResultat
{
    public class BetalingsPlan
    {
        //Den faste månedsprisen per måned
        public decimal FastMndPris { get; set; }
        
        //Den totale lånesummen
        public decimal TotalSum { get; set; }

        //Den valgte Lånetypen
        public LaaneType LaaneTypen { get; set; }

        //En Liste med den totale prisen per måned(fast + rente)
        public decimal[] TotalMndPris { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Models.KlientRequest
{
    public class LaanRequest
    {
        //Kunden som skal ta opp lånet
        public int KundeId { get; set; }
        
        //lånetypen
        public int LaaneTypeId { get; set; }
        
        //summen som skal lånes
        public decimal LaaneSum { get; set; }

        //dato for lånet (kan være null, blir da satt til systemets tid)
        public DateTime? Dato { get; set; }

        //år med nedbetaling
        public int Aar { get; set; }

        public bool Valider(BankContext context)
        {
            //Sjekker om kundeId og TypeID eksisterer
            if (context.Kunder.Any(k => k.Id == this.KundeId))
            {
                if (context.LaaneTyper.Any( t => t.Id == this.LaaneTypeId))
                {
                    //Sjekke om lånesum og nedbetalingsår er lovlig
                    return LaaneSum > 0 && Aar > 0;
                }
            }

            return false;
        }
    }
}

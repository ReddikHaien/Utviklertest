using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.KlientRequest
{
    public class BetalingsPlanInfo
    {
        public decimal Sum { get; set; }
        public int Aar { get; set; }
        public int LaaneTypeId {get; set; }
    }
}

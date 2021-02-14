using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
    public class Laan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime LaaneDato { get; set; }
        public DateTime ForrigeBetaling { get; set; }
        public decimal LaaneSum { get; set; }
        public decimal Innbetalt { get; set; }
        public int Aar { get; set; }

        public int KundeId { get; set; }
        public Kunde Kunde{ get; set; }
        
        public int LaaneTypeId { get; set; }
        public LaaneType LaaneType { get; set; }
        
    }
}

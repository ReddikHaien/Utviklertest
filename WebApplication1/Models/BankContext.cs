using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
    public class BankContext : DbContext
    {
        public DbSet<LaaneType> LaaneTyper { get; set; }
        public DbSet<Kunde> Kunder { get; set; }
        public DbSet<Laan> Laan { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bankDB.db");
        }
    }
}

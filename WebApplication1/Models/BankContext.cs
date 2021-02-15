using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
    public class BankContext : DbContext
    {
        /// <summary>
        /// L'
        /// </summary>
        private readonly bool _recievedOptions;
        public BankContext(): base() {
            _recievedOptions = false;
        }
        public BankContext(bool testing, DbContextOptions<BankContext> options) : base(options) {
            _recievedOptions = testing;
        }

        public DbSet<LaaneType> LaaneTyper { get; set; }
        public DbSet<Kunde> Kunder { get; set; }
        public DbSet<Laan> Laan { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!_recievedOptions)
            {
                optionsBuilder.UseSqlite("Data Source=../WebApplication1/bankDB.db");
            }
            else
            {
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}

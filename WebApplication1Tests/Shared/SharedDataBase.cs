using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WebApplication1Tests.Shared
{

    /// <summary>
    /// Klasse for å ha en felles bankcontext for alle testene
    /// Hentet fra https://docs.microsoft.com/en-us/ef/core/testing/sharing-databases
    /// </summary>

    public class SharedDataBase : IDisposable
    {
        private static SharedDataBase _instance = null;
        public static SharedDataBase GetSharedDataBase()
        {
            if (_instance == null)
            {
                _instance = new SharedDataBase();
            }
            return _instance;
        }
        public DbConnection Connection { get; }

        protected DbContextOptions<BankContext> ContextOptions;
        public SharedDataBase()
        {
            ContextOptions = new DbContextOptionsBuilder<BankContext>().UseSqlite(CreateInMemoryDataBase()).Options;
            Seed();

            Connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
        }

        private static DbConnection CreateInMemoryDataBase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        private void Seed()
        {

            using var bd = new BankContext(true,ContextOptions);

            bd.Database.EnsureCreated();
            bd.Database.EnsureDeleted();
                
                    //noen Lånetyper
                    bd.Add(new LaaneType { Id = 1, Navn = "Lånetype A", Rente = 1 });
                    bd.Add(new LaaneType { Id = 2, Navn = "Lånetype B", Rente = 2 });
                    bd.Add(new LaaneType { Id = 3, Navn = "Lånetype C", Rente = 3 });

                    //noen kunder
                    bd.Add(new Kunde { Id = 1, Fornavn = "Knut", Etternavn = "Pettersen" });
                    bd.Add(new Kunde { Id = 2, Fornavn = "Arne", Etternavn = "Gregorsen" });
                    bd.Add(new Kunde { Id = 3, Fornavn = "John", Etternavn = "Øder" });

                    //noen lån
                    bd.Add(new Laan
                    {
                        Aar = 20,
                        KundeId = 1,
                        LaaneDato = new DateTime(2003, 05, 13),
                        ForrigeBetaling = new DateTime(2021, 02, 13),
                        LaaneSum = 3000000,
                        Innbetalt = 2700000,
                        LaaneTypeId = 1
                    });
                    bd.Add(new Laan
                    {
                        Aar = 25,
                        KundeId = 2,
                        LaaneDato = new DateTime(),
                        ForrigeBetaling = new DateTime(),
                        LaaneSum = 1000000,
                        Innbetalt = 0,
                        LaaneTypeId = 3
                    });

                    bd.SaveChanges();
             
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        /// <summary>
        /// Sjekker om navnene på type a og b er like. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true om navnet på type a er det samme som navnet på type b</returns>
        protected bool CheckEqTypeNames(Type a, Type b)
        {
            return a.Name.Equals(b.Name);
        }
    }
}

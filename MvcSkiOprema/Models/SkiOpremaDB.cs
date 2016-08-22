using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcSkiOprema.Models
{
    public class SkiOpremaDB : DbContext
    {
        public DbSet<Kategorija> Kategorije { get; set; }

        public DbSet<Oprema> Oprema { get; set; }

        public DbSet<Proizvodac> Proizvodaci { get; set; }

        public DbSet<Rezervacija> Rezervacije { get; set; }

        public DbSet<StavkaRezervacije> StavkeRezervacije { get; set; }

        public DbSet<Cart> Carts { get; set; }
        
        //ako se promijenio model, tada pobrisi pa kreiraj bazu podataka (to ne raditi u produkciji)
        static SkiOpremaDB()
        {
            SqlConnection.ClearAllPools();
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SkiOpremaDB>());
        }
    }
}
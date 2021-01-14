using Microsoft.EntityFrameworkCore;
using PanzerGeneral2_0.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanzerGeneral2_0.Databases
{
    class SqliteDbContext : DbContext
    {

        public static string DATABASE_NAME { get; } = "PanzerDB.sqlite";


        public DbSet<UnitModel> Unit { get; set; }
        public DbSet<GameStateModel> GameState { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data source={DATABASE_NAME}");
        }
    }
}

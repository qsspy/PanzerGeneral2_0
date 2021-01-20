using Microsoft.EntityFrameworkCore;
using PanzerGeneral2_0.DataModels;

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

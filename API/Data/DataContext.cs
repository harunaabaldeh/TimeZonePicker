using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {
            
        }

        public DbSet<DateTimeModel> DateTimeModels { get; set; }
        public DbSet<DateTimeValue> DateTimeValues { get; set; }
    }
}
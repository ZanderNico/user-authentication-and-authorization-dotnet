using Microsoft.EntityFrameworkCore;
using UserAuth.Models;

namespace UserAuth.Data
{
	public class DataContext: DbContext
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
            
        }
        public DbSet<Users> Users { get; set; }
    }
}

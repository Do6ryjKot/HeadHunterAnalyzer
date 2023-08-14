using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities {
	
	public class RepositoryContext : DbContext {

		public RepositoryContext(DbContextOptions options) : base(options) { }

		public DbSet<Word> Words { get; set; }
		public DbSet<Vacancy> Vacancies { get; set; }
		public DbSet<Company> Companies { get; set; }
	}
}

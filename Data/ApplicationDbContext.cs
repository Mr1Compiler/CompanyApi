using CompanyApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Employee>()
				.HasOne(e => e.Department)
				.WithMany(d => d.Employees)
				.HasForeignKey(e => e.DepartmentId)
				.OnDelete(DeleteBehavior.Restrict);
		}

		public DbSet<Employee> Employees { get; set; }
		public DbSet<Department> Departments { get; set; }
	}
}

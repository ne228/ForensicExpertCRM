using ForensicExpertCRM_Web.Data.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForensicExpertCRM_Web.Data;

public class ApplicationDbContext : IdentityDbContext<MyUser>
{

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Expert> Experts { get; set; }

    public DbSet<ExpertManagment> ExpertManagments { get; set; }
    public DbSet<EmployeeManagment> EmployeeManagments { get; set; }

    public DbSet<TypeExpertise> TypesExpertise { get; set; }

    public DbSet<Expertise> Expertises { get; set; }

    public DbSet<MyFile> Files { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    //modelBuilder.Entity<Expert>()
    //    //        .HasMany(c => c.TypesExpertise)
    //    //        .WithMany(s => s.Id)


    //            //.UsingEntity(j => j.ToTable("Enrollments"));
    //}
}
namespace PrescriptionManagementSystem.Models
{
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Prescription & Pharmacy: One-to-Many
            modelBuilder.Entity<Prescription>()
                .HasOne<Pharmacy>(p => p.Pharmacy)
                .WithMany(ph => ph.Prescriptions)
                .HasForeignKey(p => p.PharmacyId);

            // Prescription & Patient: One-to-Many
            modelBuilder.Entity<Prescription>()
                .HasOne<Patient>(p => p.Patient)
                .WithMany(pa => pa.Prescriptions)
                .HasForeignKey(p => p.PatientId);

            // Prescription & Physician: One-to-Many
            modelBuilder.Entity<Prescription>()
                .HasOne<Physician>(p => p.Physician)
                .WithMany(ph => ph.Prescriptions)
                .HasForeignKey(p => p.PhysicianId);

            // Configure the precision and scale for the Price in Medicine
            modelBuilder.Entity<Medicine>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)"); // This means a total of 18 digits, 2 of which are after the decimal point.

            // Configure the precision and scale for the TotalCost in Prescription
            modelBuilder.Entity<Prescription>()
                .Property(p => p.TotalCost)
                .HasColumnType("decimal(18,2)"); // Adjust the precision and scale as needed for your domain.
        }

    }

}

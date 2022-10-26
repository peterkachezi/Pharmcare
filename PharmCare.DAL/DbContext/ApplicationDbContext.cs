

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmCare.DAL.Models;


namespace PharmCare.DAL.DbContext
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<SubCounty> SubCounties { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ContactPerson> ContactPersons { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<GoodsReceivedHistory> GoodsReceivedHistories { get; set; }
        public virtual DbSet<GoodsReceivedNote> GoodsReceivedNotes { get; set; }
        public virtual DbSet<LeafSetting> LeafSettings { get; set; }
        public virtual DbSet<ManufacturerPayment> ManufacturerPayments { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<OpeningBalance> OpeningBalances { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SalesDetail> SalesDetails { get; set; }
        public virtual DbSet<Shelf> Shelves { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<MedicalCondition> MedicalConditions { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedCounties(modelBuilder);
        }
        public static void SeedCounties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<County>().HasData(
            new County { Id = Guid.NewGuid(), Name = "Bomet" },
            new County { Id = Guid.NewGuid(), Name = "Bungoma" },
            new County { Id = Guid.NewGuid(), Name = "Busia" },
            new County { Id = Guid.NewGuid(), Name = "Elgeyo-Marakwet" },
            new County { Id = Guid.NewGuid(), Name = "Embu" },
            new County { Id = Guid.NewGuid(), Name = "Garissa" },
            new County { Id = Guid.NewGuid(), Name = "Homa Bay" },
            new County { Id = Guid.NewGuid(), Name = "Isiolo" },
            new County { Id = Guid.NewGuid(), Name = "Kajiado" },
            new County { Id = Guid.NewGuid(), Name = "Kakamega" },
            new County { Id = Guid.NewGuid(), Name = "Kericho" },
            new County { Id = Guid.NewGuid(), Name = "Kiambu" },
            new County { Id = Guid.NewGuid(), Name = "Kilifi" },
            new County { Id = Guid.NewGuid(), Name = "Kirinyaga" },
            new County { Id = Guid.NewGuid(), Name = "Kisii" },
            new County { Id = Guid.NewGuid(), Name = "Kisumu" },
            new County { Id = Guid.NewGuid(), Name = "Kitui" },
            new County { Id = Guid.NewGuid(), Name = "Kwale" },
            new County { Id = Guid.NewGuid(), Name = "Laikipia" },
            new County { Id = Guid.NewGuid(), Name = "Lamu" },
            new County { Id = Guid.NewGuid(), Name = "Machakos" },
            new County { Id = Guid.NewGuid(), Name = "Makueni" },
            new County { Id = Guid.NewGuid(), Name = "Mandera" },
            new County { Id = Guid.NewGuid(), Name = "Marsabit" },
            new County { Id = Guid.NewGuid(), Name = "Meru" },
            new County { Id = Guid.NewGuid(), Name = "Migori" },
            new County { Id = Guid.NewGuid(), Name = "Mambasa" },
            new County { Id = Guid.NewGuid(), Name = "Muranga" },
            new County { Id = Guid.NewGuid(), Name = "Nairobi" },
            new County { Id = Guid.NewGuid(), Name = "Nakuru" },
            new County { Id = Guid.NewGuid(), Name = "Nandi" },
            new County { Id = Guid.NewGuid(), Name = "Narok" },
            new County { Id = Guid.NewGuid(), Name = "Nyamira" },
            new County { Id = Guid.NewGuid(), Name = "Nyandarua" },
            new County { Id = Guid.NewGuid(), Name = "Nnyeri" },
            new County { Id = Guid.NewGuid(), Name = "Samburu" },
            new County { Id = Guid.NewGuid(), Name = "Siaya" },
            new County { Id = Guid.NewGuid(), Name = "Taita Taveta" },
            new County { Id = Guid.NewGuid(), Name = "Tana River" },
            new County { Id = Guid.NewGuid(), Name = "Tharaka-Nithi" },
            new County { Id = Guid.NewGuid(), Name = "Trans-Nzoia" },
            new County { Id = Guid.NewGuid(), Name = "Turkana" },
            new County { Id = Guid.NewGuid(), Name = "Uasin Gishu" },
            new County { Id = Guid.NewGuid(), Name = "Vihiga" },
            new County { Id = Guid.NewGuid(), Name = "Wajir" },
            new County { Id = Guid.NewGuid(), Name = "West Pokot" },
            new County { Id = Guid.NewGuid(), Name = "Baringo" });
        }


    }


}

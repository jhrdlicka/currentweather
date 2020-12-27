using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Identity.Models;
using Microsoft.Extensions.Configuration;


namespace currentweather.Models
{
    public partial class CurrentWeatherContext: IdentityDbContext <AppUser>
    {

        public IConfiguration Configuration { get; }


        public CurrentWeatherContext(DbContextOptions<CurrentWeatherContext> Options, IConfiguration configuration) : base(Options)
        {
            Configuration = configuration;
        }

        /*

        public CurrentWeatherContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        */

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // For Guid Primary Key
            modelBuilder.Entity<AppUser>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<doc_document>(entity =>
            {
                entity.Property(e => e.url).IsUnicode(false);
            });

            modelBuilder.Entity<ker_reference>(entity =>
            {
                entity.HasIndex(e => new { e.id, e.reftabnm, e.namenm })
                    .HasName("UQ__ker_refe__9A5F5B8BFCAC0E20")
                    .IsUnique();

                entity.Property(e => e.namenm)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.reftabnm)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ker_reftab>(entity =>
            {
                entity.HasIndex(e => new { e.id, e.reftabnm })
                    .HasName("UQ__ker_reft__7AFB89079E5D66BB")
                    .IsUnique();

                entity.Property(e => e.reftabnm)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<pcm_calevent>(entity =>
            {
                entity.HasIndex(e => e.customerid)
                    .HasName("ix_relationship2");

                entity.Property(e => e.currencynm).IsUnicode(false);

                entity.Property(e => e.gcalid).IsUnicode(false);

                entity.Property(e => e.price).HasColumnType("money");

                entity.Property(e => e.xordered)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("There exists an OrderSession for this CalEvent (the event/session is covered by any order)");

                entity.HasOne(d => d.customer)
                    .WithMany(p => p.pcm_calevent)
                    .HasForeignKey(d => d.customerid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("pcm_customerevent_fk");
            });

            modelBuilder.Entity<pcm_customer>(entity =>
            {
                entity.Property(e => e.active).HasDefaultValueSql("((1))");

                entity.Property(e => e.currencynm)
                    .IsUnicode(false)
                    .HasComment("currency - link to a CURRENCY reference table");

                entity.Property(e => e.emailcalendar)
                    .IsUnicode(false)
                    .HasComment("Customer email for communication and invitations");

                entity.Property(e => e.emailinvoice).IsUnicode(false);

                entity.Property(e => e.phone).IsUnicode(false);

                entity.Property(e => e.price10sessions)
                    .HasColumnType("money")
                    .HasComment("agreed discounted price for 10 sessions");

                entity.Property(e => e.pricesession)
                    .HasColumnType("money")
                    .HasComment("Agreed price per session");

                entity.Property(e => e.name).IsRequired();

                entity.HasOne(d => d.photodocument)
                    .WithMany(p => p.pcm_customer)
                    .HasForeignKey(d => d.photodocumentid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("doc_photodocumentcustomer_fk");
            });

            modelBuilder.Entity<pcm_invoice>(entity =>
            {
                entity.HasIndex(e => e.orderid)
                    .HasName("ix_relationship11");

                entity.Property(e => e.currencynm).IsUnicode(false);

                entity.Property(e => e.email)
                    .IsUnicode(false)
                    .HasComment("e-mail address where the invoice was sent");

                entity.Property(e => e.eventdate).HasComment("Chargeable event date (datum zdanitelneho plneni)");

                entity.Property(e => e.invoicenr).IsUnicode(false);

                entity.Property(e => e.postaddr)
                    .HasComment("Post address where the invoice was sent");

                entity.Property(e => e.price).HasColumnType("money");

                entity.Property(e => e.scandocumentid).HasComment("Scanned version of the invoice - link to doc_document");

                entity.Property(e => e.sent).HasComment("The invoice was sent to the customer");

                entity.Property(e => e.sourcedocumentid).HasComment("Source file with the invoice - link to doc_documents");

                entity.HasOne(d => d.order)
                    .WithMany(p => p.pcm_invoice)
                    .HasForeignKey(d => d.orderid)
                    .HasConstraintName("pcm_orderinvoice_tb");

                entity.HasOne(d => d.scandocument)
                    .WithMany(p => p.pcm_invoicescandocument)
                    .HasForeignKey(d => d.scandocumentid)
                    .HasConstraintName("doc_scandocumentidinvoice_fk");

                entity.HasOne(d => d.sourcedocument)
                    .WithMany(p => p.pcm_invoicesourcedocument)
                    .HasForeignKey(d => d.sourcedocumentid)
                    .HasConstraintName("doc_sourcedocumentidinvoice_fk");
            });

            modelBuilder.Entity<pcm_order>(entity =>
            {
                entity.HasIndex(e => e.customerid)
                    .HasName("ix_relationship8");

                entity.Property(e => e.currencynm).IsUnicode(false);

                entity.Property(e => e.price)
                    .HasColumnType("money")
                    .HasComment("Agreed price of the order");

                entity.Property(e => e.xfullyscheduled).HasComment("All OrderSessions are linked to non-deleted CalEvents");

                entity.Property(e => e.xinvoiced).HasComment("The invoice for for the order was sent");

                entity.HasOne(d => d.customer)
                    .WithMany(p => p.pcm_order)
                    .HasForeignKey(d => d.customerid)
                    .HasConstraintName("pcm_customerorder_fk");
            });

            modelBuilder.Entity<pcm_ordersession>(entity =>
            {
                entity.HasIndex(e => e.caleventid)
                    .HasName("ix_relationship10");

                entity.HasIndex(e => e.orderid)
                    .HasName("ix_relationship9");

                entity.Property(e => e.currencynm).IsUnicode(false);

                entity.Property(e => e.price).HasColumnType("money");

                entity.HasOne(d => d.calevent)
                    .WithMany(p => p.pcm_ordersession)
                    .HasForeignKey(d => d.caleventid)
                    .HasConstraintName("pcm_caleventsession_fk");

                entity.HasOne(d => d.order)
                    .WithMany(p => p.pcm_ordersession)
                    .HasForeignKey(d => d.orderid)
                    .HasConstraintName("pcm_odersession_fk");
            });

            modelBuilder.Entity<pcm_payment>(entity =>
            {
                entity.HasIndex(e => e.invoiceid)
                    .HasName("ix_relationship12");

                entity.Property(e => e.amount).HasColumnType("money");

                entity.Property(e => e.currencynm).IsUnicode(false);

                entity.Property(e => e.referencenr)
                    .IsUnicode(false)
                    .HasComment("Reference number - usually invoicenr (variabilni symbol)");

                entity.Property(e => e.typenm)
                    .IsUnicode(false)
                    .HasComment("Type of the payment (link to PAYMENTTYPE reference table)");

                entity.HasOne(d => d.invoice)
                    .WithMany(p => p.pcm_payment)
                    .HasForeignKey(d => d.invoiceid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("pcm_invoicepayment_tb");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
       
        public DbSet<CalendarDay> CalendarDay { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<WeatherSample> WeatherSample { get; set; }
        public DbSet<WeatherForecast> WeatherForecast { get; set; }
        public virtual DbSet<pcm_calevent> pcm_calevent { get; set; }
        public virtual DbSet<pcm_customer> pcm_customer { get; set; }
        public virtual DbSet<pcm_invoice> pcm_invoice { get; set; }
        public virtual DbSet<pcm_order> pcm_order { get; set; }
        public virtual DbSet<pcm_ordersession> pcm_ordersession { get; set; }
        public virtual DbSet<pcm_payment> pcm_payment { get; set; }
        public virtual DbSet<doc_document> doc_document { get; set; }
        public virtual DbSet<ker_reference> ker_reference { get; set; }
        public virtual DbSet<ker_reftab> ker_reftab { get; set; }

    }
}

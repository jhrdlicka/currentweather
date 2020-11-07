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

            modelBuilder.Entity<Pcm_CalEvent>(entity =>
            {
                entity.ToTable("PCM_CalEvent_TB");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("IX_Relationship2");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CanceledReason).HasColumnName("Canceled_reason");

                entity.Property(e => e.CurrencyNm)
                    .HasColumnName("Currency_NM")
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.GcalCreated).HasColumnName("GCal_Created");

                entity.Property(e => e.GcalDescription).HasColumnName("GCal_Description");

                entity.Property(e => e.GcalEnd).HasColumnName("GCal_End");

                entity.Property(e => e.GcalHtmllink)
                    .HasColumnName("GCal_HTMLlink")
                    .IsUnicode(false);

                entity.Property(e => e.GcalId)
                    .HasColumnName("GCal_Id")
                    .IsUnicode(false);

                entity.Property(e => e.GcalJson).HasColumnName("GCal_JSON");

                entity.Property(e => e.GcalLocation).HasColumnName("GCal_Location");

                entity.Property(e => e.GcalStart).HasColumnName("GCal_Start");

                entity.Property(e => e.GcalStatus)
                    .HasColumnName("GCal_Status")
                    .IsUnicode(false);

                entity.Property(e => e.GcalSummary).HasColumnName("GCal_Summary");

                entity.Property(e => e.GcalUpdated).HasColumnName("GCal_Updated");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.XOrdered)
                    .IsRequired()
                    .HasColumnName("x_Ordered")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("There exists an OrderSession for this CalEvent (the event/session is covered by any order)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CalEvents)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("PCM_CustomerEvent_FK");
            });

            modelBuilder.Entity<Pcm_Customer>(entity =>
            {
                entity.ToTable("PCM_Customer_TB");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CurrencyNm)
                    .HasColumnName("Currency_nm")
                    .IsUnicode(false);

                entity.Property(e => e.EMailCalendar)
                    .HasColumnName("eMail_calendar")
                    .IsUnicode(false);

                entity.Property(e => e.EMailInvoice)
                    .HasColumnName("eMail_invoice")
                    .IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.Photo).HasColumnType("image");

                entity.Property(e => e.Price10sessions)
                    .HasColumnName("Price_10sessions")
                    .HasColumnType("money");

                entity.Property(e => e.PriceSession)
                    .HasColumnName("Price_session")
                    .HasColumnType("money");

                entity.Property(e => e.Surname).IsRequired();
            });

            modelBuilder.Entity<Pcm_Invoice>(entity =>
            {
                entity.ToTable("PCM_Invoice_TB");

                entity.HasIndex(e => e.OrderId)
                    .HasName("IX_Relationship11");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CanceledReason).HasColumnName("Canceled_reason");

                entity.Property(e => e.CurrencyNm)
                    .HasColumnName("Currency_nm")
                    .IsUnicode(false);

                entity.Property(e => e.EMail)
                    .HasColumnName("eMail")
                    .IsUnicode(false)
                    .HasComment("e-mail address where the invoice was sent");

                entity.Property(e => e.EventDate).HasComment("Chargeable event date (datum zdanitelneho plneni)");

                entity.Property(e => e.InvoiceNr)
                    .HasColumnName("InvoiceNR")
                    .IsUnicode(false);

                entity.Property(e => e.OrderId).HasColumnName("Order_id");

                entity.Property(e => e.PostAddr)
                    .IsUnicode(false)
                    .HasComment("Post address where the invoice was sent");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.Scan)
                    .HasColumnType("image")
                    .HasComment("Scanned version of the invoice");

                entity.Property(e => e.Sent).HasComment("The invoice was sent to the customer");

                entity.Property(e => e.Sourcefile).HasComment("Source file with the invoice");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .IsUnicode(false)
                    .HasComment("Link to a document with the invoice");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PCM_OrderInvoice_TB");
            });

            modelBuilder.Entity<Pcm_OrderSession>(entity =>
            {
                entity.ToTable("PCM_OrderSession_TB");

                entity.HasIndex(e => e.CaleventId)
                    .HasName("IX_Relationship10");

                entity.HasIndex(e => e.OrderId)
                    .HasName("IX_Relationship9");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CaleventId).HasColumnName("Calevent_Id");

                entity.Property(e => e.CurrencyNm)
                    .HasColumnName("Currency_NM")
                    .IsUnicode(false);

                entity.Property(e => e.OrderId).HasColumnName("Order_id");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Calevent)
                                    .WithMany(p => p.OrderSessions)
                                    .HasForeignKey(d => d.CaleventId)
                                    .HasConstraintName("PCM_CaleventSession_FK");                

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderSessions)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PCM_OderSession_FK");
            });

            modelBuilder.Entity<Pcm_Order>(entity =>
            {
                entity.ToTable("PCM_Order_TB");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("IX_Relationship8");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CurrencyNm)
                    .HasColumnName("Currency_NM")
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasComment("Agreed price of the order");

                entity.Property(e => e.XFullyScheduled)
                    .HasColumnName("x_FullyScheduled")
                    .HasComment("All OrderSessions are linked to non-deleted CalEvents");

                entity.Property(e => e.XInvoiced)
                    .HasColumnName("x_Invoiced")
                    .HasComment("The invoice for for the order was sent");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("PCM_CustomerOrder_FK");
            });

            modelBuilder.Entity<Pcm_Payment>(entity =>
            {
                entity.ToTable("PCM_Payment_TB");

                entity.HasIndex(e => e.InvoiceId)
                    .HasName("IX_Relationship12");

                entity.Property(e => e.Id)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CurrencyNm)
                    .HasColumnName("Currency_nm")
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceId).HasColumnName("Invoice_Id");

                entity.Property(e => e.ReferenceNr)
                    .IsUnicode(false)
                    .HasComment("Reference number - usually invoicenr (variabilni symbol)");

                entity.Property(e => e.TypeNm)
                    .HasColumnName("Type_nm")
                    .IsUnicode(false)
                    .HasComment("Type of the payment (link to PAYMENTTYPE reference table)");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("PCM_InvoicePayment_TB");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
       
        public DbSet<CalendarDay> CalendarDay { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<WeatherSample> WeatherSample { get; set; }
        public DbSet<WeatherForecast> WeatherForecast { get; set; }
        public virtual DbSet<Pcm_CalEvent> PcmCalEventTb { get; set; }
        public virtual DbSet<Pcm_Customer> PcmCustomerTb { get; set; }
        public virtual DbSet<Pcm_Invoice> PcmInvoiceTb { get; set; }
        public virtual DbSet<Pcm_OrderSession> PcmOrderSessionTb { get; set; }
        public virtual DbSet<Pcm_Order> PcmOrderTb { get; set; }
        public virtual DbSet<Pcm_Payment> PcmPaymentTb { get; set; }

    }
}

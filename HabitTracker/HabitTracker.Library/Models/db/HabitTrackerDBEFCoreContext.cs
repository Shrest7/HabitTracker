using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HabitTracker.Library.Models.db
{
    public partial class HabitTrackerDBEFCoreContext : DbContext
    {
        public HabitTrackerDBEFCoreContext()
        {
        }

        public HabitTrackerDBEFCoreContext(DbContextOptions<HabitTrackerDBEFCoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Date> Date { get; set; }
        public virtual DbSet<DateHabit> DateHabit { get; set; }
        public virtual DbSet<Habit> Habit { get; set; }
        public virtual DbSet<ProgressView> ProgressView { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Date>(entity =>
            {
                entity.Property(e => e.Date1)
                    .HasColumnName("Date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<DateHabit>(entity =>
            {
                entity.HasOne(d => d.Date)
                    .WithMany(p => p.DateHabit)
                    .HasForeignKey(d => d.DateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DateFK");

                entity.HasOne(d => d.Habit)
                    .WithMany(p => p.DateHabit)
                    .HasForeignKey(d => d.HabitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HabitFK");
            });

            modelBuilder.Entity<Habit>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Habit__737584F6A2F54806")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Reason).HasDefaultValueSql("((999))");
            });

            modelBuilder.Entity<ProgressView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ProgressView");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using ChallengeAtmApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Infrastructure.Context;

public partial class PostgresContext : DbContext
{

    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auth> Auths { get; set; }

    public virtual DbSet<CardInformation> CardInformations { get; set; }

    public virtual DbSet<CustomerInformation> CustomerInformations { get; set; }

    public virtual DbSet<FailedLoginAttempt> FailedLoginAttempts { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("DB_NAME");
        var username = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auth>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Auth_pkey");

            entity.ToTable("Auth");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.HashedPin).HasMaxLength(255);

            entity.HasOne(d => d.CardNumberNavigation).WithMany(p => p.Auths)
                .HasForeignKey(d => d.CardNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Auth_CardNumber_fkey");
        });

        modelBuilder.Entity<CardInformation>(entity =>
        {
            entity.HasKey(e => e.CardNumber).HasName("CardInformation_pkey");

            entity.ToTable("CardInformation");

            entity.HasIndex(e => e.CardNumber, "CardInformation_CardNumber_key").IsUnique();

            entity.Property(e => e.CardNumber).ValueGeneratedNever();
            entity.Property(e => e.IsBlocked).HasDefaultValue(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.CardInformations)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CardInformation_CustomerId_fkey");
        });

        modelBuilder.Entity<CustomerInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CustomerInformation_pkey");

            entity.ToTable("CustomerInformation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<FailedLoginAttempt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FailedLoginAttempts_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttemptCount).HasDefaultValue(0);

            entity.HasOne(d => d.CardNumberNavigation).WithMany(p => p.FailedLoginAttempts)
                .HasForeignKey(d => d.CardNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FailedLoginAttempts_CardNumber_fkey");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Token_pkey");

            entity.ToTable("Token");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Token1)
                .HasColumnType("character varying")
                .HasColumnName("Token");

            entity.HasOne(d => d.Auth).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.AuthId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Token_AuthId_fkey");
        });

        modelBuilder.Entity<TransactionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TransactionHistory_pkey");

            entity.ToTable("TransactionHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CardNumberNavigation).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.CardNumber)
                .HasConstraintName("TransactionHistory_CardNumber_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TransactionHistory_CustomerId_fkey");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.TransactionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TransactionHistory_TransactionTypeId_fkey");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TransactionType_pkey");

            entity.ToTable("TransactionType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

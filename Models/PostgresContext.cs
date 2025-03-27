﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Models;

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

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
            .HasPostgresEnum("pgsodium", "key_type", new[] { "aead-ietf", "aead-det", "hmacsha512", "hmacsha256", "auth", "shorthash", "generichash", "kdf", "secretbox", "secretstream", "stream_xchacha20" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "pgjwt")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("pgsodium", "pgsodium")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Auth>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Auth_pkey");

            entity.ToTable("Auth");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.HashedPin)
                .HasColumnType("character varying")
                .HasColumnName("hashedPin");

            entity.HasOne(d => d.Customer).WithMany(p => p.Auths)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("Auth_customerId_fkey");
        });

        modelBuilder.Entity<CardInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CardInformation_pkey");

            entity.ToTable("CardInformation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CardNumber).HasColumnName("cardNumber");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");

            entity.HasOne(d => d.Customer).WithMany(p => p.CardInformations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("CardInformation_customerId_fkey");
        });

        modelBuilder.Entity<CustomerInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CustomerInformation_pkey");

            entity.ToTable("CustomerInformation");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.AccountBalance).HasColumnName("accountBalance");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("userName");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Token_pkey");

            entity.ToTable("Token");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthId).HasColumnName("authId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Token1)
                .HasColumnType("character varying")
                .HasColumnName("token");

            entity.HasOne(d => d.Auth).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.AuthId)
                .HasConstraintName("Token_authId_fkey");
        });

        modelBuilder.Entity<TransactionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TransactionHistory_pkey");

            entity.ToTable("TransactionHistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.RemainingBalance).HasColumnName("remainingBalance");
            entity.Property(e => e.TransactionAmount).HasColumnName("transactionAmount");
            entity.Property(e => e.TransactionTypeId).HasColumnName("transactionTypeId");

            entity.HasOne(d => d.Customer).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("TransactionHistory_customerId_fkey");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.TransactionTypeId)
                .HasConstraintName("TransactionHistory_transactionTypeId_fkey");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TransactionType_pkey");

            entity.ToTable("TransactionType");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

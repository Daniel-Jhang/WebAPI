using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NorthwindWebAPI.Models;

namespace NorthwindWebAPI.Data;

public partial class LabContext : DbContext
{
    public LabContext()
    {
    }

    public LabContext(DbContextOptions<LabContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TodoList> TodoLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-RR1UH4B6\\SQLEXPRESS;Database=LAB;Trusted_Connection=True; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoList>(entity =>
        {
            entity.HasKey(e => e.TodoId).HasName("PK__TodoList__95862552544A5B5C");

            entity.ToTable("TodoList");

            entity.Property(e => e.TodoId).ValueGeneratedNever();
            entity.Property(e => e.Context)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SqlId)
                .ValueGeneratedOnAdd()
                .HasColumnName("sqlId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Data;

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

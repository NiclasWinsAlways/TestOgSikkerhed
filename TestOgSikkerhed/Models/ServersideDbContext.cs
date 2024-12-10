using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestOgSikkerhed.Models;

public partial class ServersideDbContext : DbContext
{
    public ServersideDbContext()
    {
    }

    public ServersideDbContext(DbContextOptions<ServersideDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cpr> Cprs { get; set; }

    public virtual DbSet<Todolist> Todolists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ServersideDB;Trusted_Connection=True;MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cpr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cpr__3214EC076FC7DAD9");

            entity.ToTable("Cpr");

            entity.Property(e => e.CprNr).HasMaxLength(50);
        });

        modelBuilder.Entity<Todolist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Todolist__3214EC07C3DEF4E0");

            entity.ToTable("Todolist");

            entity.Property(e => e.Item).HasMaxLength(300);

            entity.HasOne(d => d.Cpr).WithMany(p => p.Todolists)
                .HasForeignKey(d => d.CprId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Todolist__CprId__38996AB5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

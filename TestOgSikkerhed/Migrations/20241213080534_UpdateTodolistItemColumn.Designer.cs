﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestOgSikkerhed.Models;

#nullable disable

namespace TestOgSikkerhed.Migrations
{
    [DbContext(typeof(ServersideDbContext))]
    [Migration("20241213080534_UpdateTodolistItemColumn")]
    partial class UpdateTodolistItemColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestOgSikkerhed.Models.Cpr", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CprNr")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__Cpr__3214EC076FC7DAD9");

                    b.ToTable("Cpr", (string)null);
                });

            modelBuilder.Entity("TestOgSikkerhed.Models.Todolist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CprId")
                        .HasColumnType("int");

                    b.Property<string>("Item")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id")
                        .HasName("PK__Todolist__3214EC07C3DEF4E0");

                    b.HasIndex(new[] { "CprId" }, "IX_Todolist_CprId");

                    b.ToTable("Todolist", (string)null);
                });

            modelBuilder.Entity("TestOgSikkerhed.Models.Todolist", b =>
                {
                    b.HasOne("TestOgSikkerhed.Models.Cpr", "Cpr")
                        .WithMany("Todolists")
                        .HasForeignKey("CprId")
                        .IsRequired()
                        .HasConstraintName("FK__Todolist__CprId__38996AB5");

                    b.Navigation("Cpr");
                });

            modelBuilder.Entity("TestOgSikkerhed.Models.Cpr", b =>
                {
                    b.Navigation("Todolists");
                });
#pragma warning restore 612, 618
        }
    }
}

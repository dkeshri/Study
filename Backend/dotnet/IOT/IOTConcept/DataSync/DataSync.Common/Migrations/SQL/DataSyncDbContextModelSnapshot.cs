﻿// <auto-generated />
using DataSync.Common.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataSync.Common.Migrations.SQL
{
    [DbContext(typeof(DataSyncDbContext))]
    partial class DataSyncDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataSync.Common.Data.Entities.ChangeTracker", b =>
                {
                    b.Property<int>("TableName")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TableName"));

                    b.Property<long>("ChangeVersion")
                        .HasColumnType("bigint");

                    b.HasKey("TableName");

                    b.ToTable("ChangeTrackers");
                });
#pragma warning restore 612, 618
        }
    }
}

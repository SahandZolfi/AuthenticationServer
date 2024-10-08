﻿// <auto-generated />
using System;
using AuthenticationServer.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthenticationServer.Persistence.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    partial class DataBaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Title = "Normal User"
                        },
                        new
                        {
                            Id = 2,
                            Title = "Admin"
                        });
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("BannedUpTo")
                        .HasColumnType("datetime2");

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasMaxLength(145)
                        .HasColumnType("nvarchar(145)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("nvarchar(320)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsBanned")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("LastPassword")
                        .HasColumnType("datetime2");

                    b.Property<string>("LoweredUsername")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProfileImageName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("DefaultProfile.png");

                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.UserAccountActivity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("AccountActivityType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserAccountActivities", (string)null);
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.UserReport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("ReporterByUserId")
                        .HasColumnType("bigint");

                    b.Property<long>("ReportingId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ReporterByUserId");

                    b.HasIndex("ReportingId");

                    b.ToTable("UserReports", (string)null);
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.UserTMPUniqueCode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpireTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("UserTMPCodeType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserTMPUniqueCodes", (string)null);
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.User", b =>
                {
                    b.HasOne("AuthenticationServer.Domain.Entities.UserEntities.Role", "Role")
                        .WithMany("UsersWithThisRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.UserAccountActivity", b =>
                {
                    b.HasOne("AuthenticationServer.Domain.Entities.UserEntities.User", "User")
                        .WithMany("UserAccountActivities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.UserReport", b =>
                {
                    b.HasOne("AuthenticationServer.Domain.Entities.UserEntities.User", "ReportedByUser")
                        .WithMany("Reportings")
                        .HasForeignKey("ReporterByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuthenticationServer.Domain.Entities.UserEntities.User", "Reporting")
                        .WithMany("Reports")
                        .HasForeignKey("ReportingId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("ReportedByUser");

                    b.Navigation("Reporting");
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.UserTMPUniqueCode", b =>
                {
                    b.HasOne("AuthenticationServer.Domain.Entities.UserEntities.User", "User")
                        .WithMany("UserTMPUniqueCodes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.Role", b =>
                {
                    b.Navigation("UsersWithThisRole");
                });

            modelBuilder.Entity("AuthenticationServer.Domain.Entities.UserEntities.User", b =>
                {
                    b.Navigation("Reportings");

                    b.Navigation("Reports");

                    b.Navigation("UserAccountActivities");

                    b.Navigation("UserTMPUniqueCodes");
                });
#pragma warning restore 612, 618
        }
    }
}

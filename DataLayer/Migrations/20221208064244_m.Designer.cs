﻿// <auto-generated />
using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221208064244_m")]
    partial class m
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataLayer.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DeptName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("DataLayer.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("DeptId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SalaryId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeptId");

                    b.HasIndex("SalaryId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DataLayer.Leave", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LegalLeaves")
                        .HasColumnType("int");

                    b.Property<int>("NoOfLeaves")
                        .HasColumnType("int");

                    b.Property<Guid?>("SalaryDeductionId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TotalLeaves")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SalaryDeductionId");

                    b.ToTable("Leaves");
                });

            modelBuilder.Entity("DataLayer.Salary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BasicSalary")
                        .HasColumnType("int");

                    b.Property<int>("DA")
                        .HasColumnType("int");

                    b.Property<Guid>("EmpId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("HRA")
                        .HasColumnType("int");

                    b.Property<int>("TA")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Salaries");
                });

            modelBuilder.Entity("DataLayer.SalaryDeduction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LeaveDeductedSal")
                        .HasColumnType("int");

                    b.Property<int>("PF")
                        .HasColumnType("int");

                    b.Property<int>("TotalSalary")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SalaryDeductions");
                });

            modelBuilder.Entity("DataLayer.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContactNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResetCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataLayer.Employee", b =>
                {
                    b.HasOne("DataLayer.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DeptId");

                    b.HasOne("DataLayer.Salary", "Salary")
                        .WithMany("Employees")
                        .HasForeignKey("SalaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Salary");
                });

            modelBuilder.Entity("DataLayer.Leave", b =>
                {
                    b.HasOne("DataLayer.SalaryDeduction", "SalaryDeduction")
                        .WithMany("Leaves")
                        .HasForeignKey("SalaryDeductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SalaryDeduction");
                });

            modelBuilder.Entity("DataLayer.SalaryDeduction", b =>
                {
                    b.HasOne("DataLayer.Salary", "Salary")
                        .WithOne("SalaryDeduction")
                        .HasForeignKey("DataLayer.SalaryDeduction", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Salary");
                });

            modelBuilder.Entity("DataLayer.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DataLayer.Salary", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("SalaryDeduction")
                        .IsRequired();
                });

            modelBuilder.Entity("DataLayer.SalaryDeduction", b =>
                {
                    b.Navigation("Leaves");
                });
#pragma warning restore 612, 618
        }
    }
}
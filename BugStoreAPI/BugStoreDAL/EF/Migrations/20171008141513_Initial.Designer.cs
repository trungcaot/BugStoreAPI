﻿// <auto-generated />
using BugStoreDAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BugStoreDAL.EF.Migrations
{
    [DbContext(typeof(StoreContext))]
    [Migration("20171008141513_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BugStoreModels.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<byte[]>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.ToTable("Categories","Store");
                });

            modelBuilder.Entity("BugStoreModels.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<decimal>("LineItemTotal");

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<byte[]>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Orders","Store");
                });

            modelBuilder.Entity("BugStoreModels.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<decimal>("CurrentPrice")
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("Description")
                        .HasMaxLength(3800);

                    b.Property<bool>("IsFeatured");

                    b.Property<string>("ModelName")
                        .HasMaxLength(50);

                    b.Property<string>("ModelNumber")
                        .HasMaxLength(50);

                    b.Property<string>("ProductImage")
                        .HasMaxLength(150);

                    b.Property<string>("ProductImageLarge")
                        .HasMaxLength(150);

                    b.Property<string>("ProductImageThumb")
                        .HasMaxLength(150);

                    b.Property<byte[]>("TimeStamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasMaxLength(8);

                    b.Property<decimal>("UnitCost")
                        .HasColumnType("decimal(19,4)");

                    b.Property<int>("UnitsInStock");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products","Store");
                });

            modelBuilder.Entity("BugStoreModels.ViewModels.OrderProductInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<string>("CategoryName");

                    b.Property<decimal>("CurrentPrice");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime?>("DateCreated");

                    b.Property<string>("Description");

                    b.Property<bool>("IsFeatured");

                    b.Property<decimal>("LineItemTotal");

                    b.Property<string>("ModelName");

                    b.Property<string>("ModelNumber");

                    b.Property<int>("ProductId");

                    b.Property<string>("ProductImage");

                    b.Property<int>("Quantity");

                    b.Property<byte[]>("TimeStamp");

                    b.Property<decimal>("UnitCost");

                    b.Property<int>("UnitsInStock");

                    b.HasKey("Id");

                    b.ToTable("ViewModels");
                });

            modelBuilder.Entity("BugStoreModels.Order", b =>
                {
                    b.HasOne("BugStoreModels.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BugStoreModels.Product", b =>
                {
                    b.HasOne("BugStoreModels.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Models;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(BankContext))]
    [Migration("20210211170935_LagtTilLaan")]
    partial class LagtTilLaan
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("WebApplication1.Models.Kunde", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Etternavn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fornavn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Kunder");
                });

            modelBuilder.Entity("WebApplication1.Models.Laan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("KundeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LaaneDato")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("LaaneSum")
                        .HasColumnType("TEXT");

                    b.Property<int?>("LaaneTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("KundeId");

                    b.HasIndex("LaaneTypeId");

                    b.ToTable("Laan");
                });

            modelBuilder.Entity("WebApplication1.Models.LaaneType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Navn")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Rente")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LaaneTyper");
                });

            modelBuilder.Entity("WebApplication1.Models.Laan", b =>
                {
                    b.HasOne("WebApplication1.Models.Kunde", "Kunde")
                        .WithMany()
                        .HasForeignKey("KundeId");

                    b.HasOne("WebApplication1.Models.LaaneType", "LaaneType")
                        .WithMany()
                        .HasForeignKey("LaaneTypeId");

                    b.Navigation("Kunde");

                    b.Navigation("LaaneType");
                });
#pragma warning restore 612, 618
        }
    }
}

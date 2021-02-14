﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Models;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(BankContext))]
    [Migration("20210211160236_LagtTilKunde")]
    partial class LagtTilKunde
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

            modelBuilder.Entity("WebApplication1.Models.LaaneType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Navn")
                        .HasColumnType("TEXT");

                    b.Property<double>("Rente")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("LaaneTyper");
                });
#pragma warning restore 612, 618
        }
    }
}
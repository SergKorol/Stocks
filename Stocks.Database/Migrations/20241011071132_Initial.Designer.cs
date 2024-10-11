﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Stocks.Database;

#nullable disable

namespace Stocks.Database.Migrations
{
    [DbContext(typeof(MarketContext))]
    [Migration("20241011071132_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Stocks.Database.Entities.MarketAssetEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Currency")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Exchange")
                        .HasColumnType("text");

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal?>("TickSize")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("MarketAssets");
                });

            modelBuilder.Entity("Stocks.Database.Entities.MarketPriceChangeEntity", b =>
                {
                    b.Property<Guid?>("Id")
                        .HasColumnType("uuid");

                    b.Property<double>("AskPrice")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("AskUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("BidPrice")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("BidUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("LastPrice")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("MarketPrices");
                });

            modelBuilder.Entity("Stocks.Database.Entities.MarketPriceChangeEntity", b =>
                {
                    b.HasOne("Stocks.Database.Entities.MarketAssetEntity", "MarketAsset")
                        .WithOne("MarketPrice")
                        .HasForeignKey("Stocks.Database.Entities.MarketPriceChangeEntity", "Id");

                    b.Navigation("MarketAsset");
                });

            modelBuilder.Entity("Stocks.Database.Entities.MarketAssetEntity", b =>
                {
                    b.Navigation("MarketPrice");
                });
#pragma warning restore 612, 618
        }
    }
}

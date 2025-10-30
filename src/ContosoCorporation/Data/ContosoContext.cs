using System;
using System.Collections.Generic;
using ContosoCorporation.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoCorporation.Data;

public partial class ContosoContext : DbContext
{
    public ContosoContext(DbContextOptions<ContosoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CurrencyExchange> CurrencyExchanges { get; set; }

    public virtual DbSet<CurrencyExchange1> CurrencyExchanges1 { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customer1> Customers1 { get; set; }

    public virtual DbSet<Date> Dates { get; set; }

    public virtual DbSet<Date1> Dates1 { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderRow> OrderRows { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Product1> Products1 { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Sale1> Sales1 { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Store1> Stores1 { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<CurrencyExchange>(entity =>
        {
            entity.HasKey(e => new { e.Date, e.FromCurrency, e.ToCurrency });

            entity.ToTable("CurrencyExchange", "Data");

            entity.Property(e => e.FromCurrency)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.ToCurrency)
                .HasMaxLength(3)
                .IsFixedLength();
        });

        modelBuilder.Entity<CurrencyExchange1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Currency Exchange");

            entity.Property(e => e.FromCurrency)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.ToCurrency)
                .HasMaxLength(3)
                .IsFixedLength();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerKey);

            entity.ToTable("Customer", "Data");

            entity.Property(e => e.CustomerKey).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Company).HasMaxLength(50);
            entity.Property(e => e.Continent).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CountryFull).HasMaxLength(50);
            entity.Property(e => e.EndDt).HasColumnName("EndDT");
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.GivenName).HasMaxLength(150);
            entity.Property(e => e.MiddleInitial).HasMaxLength(150);
            entity.Property(e => e.Occupation).HasMaxLength(100);
            entity.Property(e => e.StartDt).HasColumnName("StartDT");
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.StateFull).HasMaxLength(50);
            entity.Property(e => e.StreetAddress).HasMaxLength(150);
            entity.Property(e => e.Surname).HasMaxLength(150);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Vehicle).HasMaxLength(50);
            entity.Property(e => e.ZipCode).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Customer");

            entity.Property(e => e.Address).HasMaxLength(150);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Continent).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .HasColumnName("Country Code");
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(301);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.StateCode)
                .HasMaxLength(50)
                .HasColumnName("State Code");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(50)
                .HasColumnName("Zip Code");
        });

        modelBuilder.Entity<Date>(entity =>
        {
            entity.HasKey(e => e.DateKey);

            entity.ToTable("Date", "Data");

            entity.Property(e => e.DateKey).HasMaxLength(50);
            entity.Property(e => e.Date1).HasColumnName("Date");
            entity.Property(e => e.DayofWeek).HasMaxLength(30);
            entity.Property(e => e.DayofWeekShort).HasMaxLength(30);
            entity.Property(e => e.Month).HasMaxLength(30);
            entity.Property(e => e.MonthShort).HasMaxLength(30);
            entity.Property(e => e.Quarter).HasMaxLength(2);
            entity.Property(e => e.YearMonth).HasMaxLength(30);
            entity.Property(e => e.YearMonthShort).HasMaxLength(30);
            entity.Property(e => e.YearQuarter).HasMaxLength(30);
        });

        modelBuilder.Entity<Date1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Date");

            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(30)
                .HasColumnName("Day of Week");
            entity.Property(e => e.DayOfWeekNumber).HasColumnName("Day of Week Number");
            entity.Property(e => e.DayOfWeekShort)
                .HasMaxLength(30)
                .HasColumnName("Day of Week Short");
            entity.Property(e => e.Month).HasMaxLength(30);
            entity.Property(e => e.MonthNumber).HasColumnName("Month Number");
            entity.Property(e => e.MonthShort)
                .HasMaxLength(30)
                .HasColumnName("Month Short");
            entity.Property(e => e.Quarter).HasMaxLength(2);
            entity.Property(e => e.WorkingDay).HasColumnName("Working Day");
            entity.Property(e => e.WorkingDayNumber).HasColumnName("Working Day Number");
            entity.Property(e => e.YearMonth)
                .HasMaxLength(30)
                .HasColumnName("Year Month");
            entity.Property(e => e.YearMonthNumber).HasColumnName("Year Month Number");
            entity.Property(e => e.YearMonthShort)
                .HasMaxLength(30)
                .HasColumnName("Year Month Short");
            entity.Property(e => e.YearQuarter)
                .HasMaxLength(30)
                .HasColumnName("Year Quarter");
            entity.Property(e => e.YearQuarterNumber).HasColumnName("Year Quarter Number");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderKey);

            entity.ToTable("Orders", "Data");

            entity.HasIndex(e => e.CustomerKey, "IX_Orders_CustomerKey");

            entity.HasIndex(e => e.StoreKey, "IX_Orders_StoreKey");

            entity.Property(e => e.OrderKey).ValueGeneratedNever();
            entity.Property(e => e.CurrencyCode).HasMaxLength(5);

            entity.HasOne(d => d.CustomerKeyNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.StoreKeyNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Stores");
        });

        modelBuilder.Entity<OrderRow>(entity =>
        {
            entity.HasKey(e => new { e.OrderKey, e.LineNumber });

            entity.ToTable("OrderRows", "Data");

            entity.HasIndex(e => e.OrderKey, "IX_OrderRows_OrderKey");

            entity.HasIndex(e => e.ProductKey, "IX_OrderRows_ProductKey");

            entity.Property(e => e.NetPrice).HasColumnType("money");
            entity.Property(e => e.UnitCost).HasColumnType("money");
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(d => d.OrderKeyNavigation).WithMany(p => p.OrderRows)
                .HasForeignKey(d => d.OrderKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderRows_Orders");

            entity.HasOne(d => d.ProductKeyNavigation).WithMany(p => p.OrderRows)
                .HasForeignKey(d => d.ProductKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderRows_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductKey).HasName("PK_TestProduct");

            entity.ToTable("Product", "Data");

            entity.Property(e => e.ProductKey).ValueGeneratedNever();
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.CategoryName).HasMaxLength(30);
            entity.Property(e => e.Color).HasMaxLength(20);
            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.Manufacturer).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductCode).HasMaxLength(255);
            entity.Property(e => e.ProductName).HasMaxLength(500);
            entity.Property(e => e.SubCategoryName).HasMaxLength(50);
            entity.Property(e => e.WeightUnit).HasMaxLength(20);
        });

        modelBuilder.Entity<Product1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Product");

            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(30);
            entity.Property(e => e.CategoryCode).HasColumnName("Category Code");
            entity.Property(e => e.Color).HasMaxLength(20);
            entity.Property(e => e.Manufacturer).HasMaxLength(50);
            entity.Property(e => e.ProductCode)
                .HasMaxLength(255)
                .HasColumnName("Product Code");
            entity.Property(e => e.ProductName)
                .HasMaxLength(500)
                .HasColumnName("Product Name");
            entity.Property(e => e.Subcategory).HasMaxLength(50);
            entity.Property(e => e.SubcategoryCode).HasColumnName("Subcategory Code");
            entity.Property(e => e.UnitCost)
                .HasColumnType("money")
                .HasColumnName("Unit Cost");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("money")
                .HasColumnName("Unit Price");
            entity.Property(e => e.WeightUnitMeasure)
                .HasMaxLength(20)
                .HasColumnName("Weight Unit Measure");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => new { e.OrderKey, e.LineNumber });

            entity.ToTable("Sales", "Data");

            entity.HasIndex(e => e.CustomerKey, "IX_Sales_CustomerKey");

            entity.HasIndex(e => e.ProductKey, "IX_Sales_ProductKey");

            entity.HasIndex(e => e.StoreKey, "IX_Sales_StoreKey");

            entity.Property(e => e.CurrencyCode).HasMaxLength(5);
            entity.Property(e => e.NetPrice).HasColumnType("money");
            entity.Property(e => e.UnitCost).HasColumnType("money");
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(d => d.CustomerKeyNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Customers");

            entity.HasOne(d => d.ProductKeyNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Products");

            entity.HasOne(d => d.StoreKeyNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.StoreKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Stores");
        });

        modelBuilder.Entity<Sale1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Sales");

            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(5)
                .HasColumnName("Currency Code");
            entity.Property(e => e.DeliveryDate).HasColumnName("Delivery Date");
            entity.Property(e => e.ExchangeRate).HasColumnName("Exchange Rate");
            entity.Property(e => e.LineNumber).HasColumnName("Line Number");
            entity.Property(e => e.NetPrice)
                .HasColumnType("money")
                .HasColumnName("Net Price");
            entity.Property(e => e.OrderDate).HasColumnName("Order Date");
            entity.Property(e => e.OrderNumber).HasColumnName("Order Number");
            entity.Property(e => e.UnitCost)
                .HasColumnType("money")
                .HasColumnName("Unit Cost");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("money")
                .HasColumnName("Unit Price");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreKey);

            entity.ToTable("Store", "Data");

            entity.Property(e => e.StoreKey).ValueGeneratedNever();
            entity.Property(e => e.CountryCode).HasMaxLength(50);
            entity.Property(e => e.CountryName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Store1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Store");

            entity.Property(e => e.CloseDate).HasColumnName("Close Date");
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OpenDate).HasColumnName("Open Date");
            entity.Property(e => e.SquareMeters).HasColumnName("Square Meters");
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StoreCode).HasColumnName("Store Code");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

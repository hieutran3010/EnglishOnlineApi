﻿// <auto-generated />
using System;
using HelenExpress.Data;
using HelenExpress.Data.JSONModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HelenExpress.Data.Migrations
{
    [DbContext(typeof(HeLenExpressDbContext))]
    [Migration("20200802183523_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:citext", ",,")
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("HelenExpress.Data.Entities.Bill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v1mc()");

                    b.Property<string>("AccountantUserId")
                        .HasColumnName("accountantuserid")
                        .HasColumnType("citext");

                    b.Property<string>("AirlineBillId")
                        .HasColumnName("airlinebillid")
                        .HasColumnType("citext");

                    b.Property<string>("ChildBillId")
                        .HasColumnName("childbillid")
                        .HasColumnType("citext");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("createdby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<double?>("CustomerPaymentAmount")
                        .HasColumnName("customerpaymentamount")
                        .HasColumnType("double precision");

                    b.Property<double?>("CustomerPaymentDebt")
                        .HasColumnName("customerpaymentdebt")
                        .HasColumnType("double precision");

                    b.Property<string>("CustomerPaymentType")
                        .HasColumnName("customerpaymenttype")
                        .HasColumnType("citext");

                    b.Property<DateTime>("Date")
                        .HasColumnName("date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("citext");

                    b.Property<string>("DestinationCountry")
                        .HasColumnName("destinationcountry")
                        .HasColumnType("citext");

                    b.Property<string>("InternationalParcelVendor")
                        .HasColumnName("internationalparcelvendor")
                        .HasColumnType("citext");

                    b.Property<bool>("IsArchived")
                        .HasColumnName("isarchived")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPrintedVatBill")
                        .HasColumnName("isprintedvatbill")
                        .HasColumnType("boolean");

                    b.Property<string>("LicenseUserId")
                        .HasColumnName("licenseuserid")
                        .HasColumnType("citext");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnName("modifiedby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("modifiedon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Period")
                        .HasColumnName("period")
                        .HasColumnType("citext");

                    b.Property<double?>("Profit")
                        .HasColumnName("profit")
                        .HasColumnType("double precision");

                    b.Property<double?>("ProfitBeforeTax")
                        .HasColumnName("profitbeforetax")
                        .HasColumnType("double precision");

                    b.Property<double?>("PurchasePriceAfterVatInUsd")
                        .HasColumnName("purchasepriceaftervatinusd")
                        .HasColumnType("double precision");

                    b.Property<int?>("PurchasePriceAfterVatInVnd")
                        .HasColumnName("purchasepriceaftervatinvnd")
                        .HasColumnType("integer");

                    b.Property<double?>("PurchasePriceInUsd")
                        .HasColumnName("purchasepriceinusd")
                        .HasColumnType("double precision");

                    b.Property<int?>("PurchasePriceInVnd")
                        .HasColumnName("purchasepriceinvnd")
                        .HasColumnType("integer");

                    b.Property<double?>("QuotationPriceInUsd")
                        .HasColumnName("quotationpriceinusd")
                        .HasColumnType("double precision");

                    b.Property<string>("ReceiverAddress")
                        .HasColumnName("receiveraddress")
                        .HasColumnType("citext");

                    b.Property<Guid?>("ReceiverId")
                        .HasColumnName("receiverid")
                        .HasColumnType("uuid");

                    b.Property<string>("ReceiverName")
                        .HasColumnName("receivername")
                        .HasColumnType("citext");

                    b.Property<string>("ReceiverPhone")
                        .HasColumnName("receiverphone")
                        .HasColumnType("citext");

                    b.Property<double?>("SalePrice")
                        .HasColumnName("saleprice")
                        .HasColumnType("double precision");

                    b.Property<string>("SaleUserId")
                        .HasColumnName("saleuserid")
                        .HasColumnType("citext");

                    b.Property<string>("SenderAddress")
                        .HasColumnName("senderaddress")
                        .HasColumnType("citext");

                    b.Property<Guid?>("SenderId")
                        .HasColumnName("senderid")
                        .HasColumnType("uuid");

                    b.Property<string>("SenderName")
                        .HasColumnName("sendername")
                        .HasColumnType("citext");

                    b.Property<string>("SenderPhone")
                        .HasColumnName("senderphone")
                        .HasColumnType("citext");

                    b.Property<string>("Status")
                        .HasColumnName("status")
                        .HasColumnType("citext");

                    b.Property<int?>("UsdExchangeRate")
                        .HasColumnName("usdexchangerate")
                        .HasColumnType("integer");

                    b.Property<long?>("Vat")
                        .HasColumnName("vat")
                        .HasColumnType("bigint");

                    b.Property<double?>("VendorFuelChargeFeeInUsd")
                        .HasColumnName("vendorfuelchargefeeinusd")
                        .HasColumnType("double precision");

                    b.Property<double?>("VendorFuelChargeFeeInVnd")
                        .HasColumnName("vendorfuelchargefeeinvnd")
                        .HasColumnType("double precision");

                    b.Property<double>("VendorFuelChargePercent")
                        .HasColumnName("vendorfuelchargepercent")
                        .HasColumnType("double precision");

                    b.Property<Guid>("VendorId")
                        .HasColumnName("vendorid")
                        .HasColumnType("uuid");

                    b.Property<string>("VendorName")
                        .HasColumnName("vendorname")
                        .HasColumnType("citext");

                    b.Property<double?>("VendorNetPriceInUsd")
                        .HasColumnName("vendornetpriceinusd")
                        .HasColumnType("double precision");

                    b.Property<double>("VendorOtherFee")
                        .HasColumnName("vendorotherfee")
                        .HasColumnType("double precision");

                    b.Property<double?>("VendorPaymentAmount")
                        .HasColumnName("vendorpaymentamount")
                        .HasColumnType("double precision");

                    b.Property<double?>("VendorPaymentDebt")
                        .HasColumnName("vendorpaymentdebt")
                        .HasColumnType("double precision");

                    b.Property<string>("VendorPaymentType")
                        .HasColumnName("vendorpaymenttype")
                        .HasColumnType("citext");

                    b.Property<double>("WeightInKg")
                        .HasColumnName("weightinkg")
                        .HasColumnType("double precision");

                    b.Property<string>("ZoneName")
                        .HasColumnName("zonename")
                        .HasColumnType("citext");

                    b.HasKey("Id")
                        .HasName("pk_bill");

                    b.HasIndex("AirlineBillId")
                        .IsUnique()
                        .HasName("ix_bill_airlinebillid");

                    b.HasIndex("Date")
                        .HasName("ix_bill_date");

                    b.HasIndex("Period")
                        .HasName("ix_bill_period");

                    b.HasIndex("ReceiverId")
                        .HasName("ix_bill_receiverid");

                    b.HasIndex("SenderId")
                        .HasName("ix_bill_senderid");

                    b.HasIndex("VendorId")
                        .HasName("ix_bill_vendorid");

                    b.ToTable("bill");
                });

            modelBuilder.Entity("HelenExpress.Data.Entities.BillDescription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v1mc()");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("createdby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnName("modifiedby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("modifiedon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("citext");

                    b.HasKey("Id")
                        .HasName("pk_billdescription");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_billdescription_name");

                    b.ToTable("billdescription");
                });

            modelBuilder.Entity("HelenExpress.Data.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v1mc()");

                    b.Property<string>("Address")
                        .HasColumnName("address")
                        .HasColumnType("citext");

                    b.Property<string>("Code")
                        .HasColumnName("code")
                        .HasColumnType("citext");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("createdby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Hint")
                        .HasColumnName("hint")
                        .HasColumnType("citext");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnName("modifiedby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("modifiedon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("citext");

                    b.Property<string>("NickName")
                        .HasColumnName("nickname")
                        .HasColumnType("citext");

                    b.Property<string>("Phone")
                        .HasColumnName("phone")
                        .HasColumnType("citext");

                    b.Property<string>("SaleUserId")
                        .HasColumnName("saleuserid")
                        .HasColumnType("citext");

                    b.HasKey("Id")
                        .HasName("pk_customer");

                    b.HasIndex("Address")
                        .HasName("ix_customer_address");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasName("ix_customer_code");

                    b.HasIndex("Name")
                        .HasName("ix_customer_name");

                    b.HasIndex("Phone")
                        .IsUnique()
                        .HasName("ix_customer_phone");

                    b.ToTable("customer");
                });

            modelBuilder.Entity("HelenExpress.Data.Entities.ExportSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v1mc()");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("createdby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("ExportType")
                        .HasColumnName("exporttype")
                        .HasColumnType("citext");

                    b.Property<string>("FilePath")
                        .HasColumnName("filepath")
                        .HasColumnType("citext");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnName("modifiedby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("modifiedon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Note")
                        .HasColumnName("note")
                        .HasColumnType("citext");

                    b.Property<string>("Status")
                        .HasColumnName("status")
                        .HasColumnType("citext");

                    b.Property<string>("UserId")
                        .HasColumnName("userid")
                        .HasColumnType("citext");

                    b.HasKey("Id")
                        .HasName("pk_exportsession");

                    b.HasIndex("UserId", "ExportType")
                        .IsUnique()
                        .HasName("ix_exportsession_userid_exporttype");

                    b.ToTable("exportsession");
                });

            modelBuilder.Entity("HelenExpress.Data.Entities.Params", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v1mc()");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("createdby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Key")
                        .HasColumnName("key")
                        .HasColumnType("citext");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnName("modifiedby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("modifiedon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Value")
                        .HasColumnName("value")
                        .HasColumnType("citext");

                    b.HasKey("Id")
                        .HasName("pk_params");

                    b.ToTable("params");
                });

            modelBuilder.Entity("HelenExpress.Data.Entities.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v1mc()");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("createdby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnName("modifiedby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("modifiedon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Role")
                        .HasColumnName("role")
                        .HasColumnType("citext");

                    b.Property<string>("UserId")
                        .HasColumnName("userid")
                        .HasColumnType("citext");

                    b.HasKey("Id")
                        .HasName("pk_userrole");

                    b.HasIndex("Role")
                        .HasName("ix_userrole_role");

                    b.HasIndex("UserId", "Role")
                        .IsUnique()
                        .HasName("ix_userrole_userid_role");

                    b.ToTable("userrole");
                });

            modelBuilder.Entity("HelenExpress.Data.Entities.Vendor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v1mc()");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("createdby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<double?>("FuelChargePercent")
                        .HasColumnName("fuelchargepercent")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsStopped")
                        .HasColumnName("isstopped")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnName("modifiedby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("modifiedon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("citext");

                    b.Property<string>("OfficeAddress")
                        .HasColumnName("officeaddress")
                        .HasColumnType("citext");

                    b.Property<double?>("OtherFeeInUsd")
                        .HasColumnName("otherfeeinusd")
                        .HasColumnType("double precision");

                    b.Property<string>("Phone")
                        .HasColumnName("phone")
                        .HasColumnType("citext");

                    b.Property<VendorQuotation[]>("VendorQuotations")
                        .HasColumnName("vendorquotations")
                        .HasColumnType("jsonb");

                    b.HasKey("Id")
                        .HasName("pk_vendor");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("ix_vendor_name");

                    b.ToTable("vendor");
                });

            modelBuilder.Entity("HelenExpress.Data.Entities.Zone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v1mc()");

                    b.Property<string[]>("Countries")
                        .HasColumnName("countries")
                        .HasColumnType("jsonb");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnName("createdby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnName("modifiedby")
                        .HasColumnType("citext");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("modifiedon")
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc'::text, now())");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("citext");

                    b.Property<Guid>("VendorId")
                        .HasColumnName("vendorid")
                        .HasColumnType("uuid");

                    b.HasKey("Id")
                        .HasName("pk_zone");

                    b.HasIndex("VendorId")
                        .HasName("ix_zone_vendorid");

                    b.HasIndex("Name", "VendorId")
                        .IsUnique()
                        .HasName("ix_zone_name_vendorid");

                    b.ToTable("zone");
                });

            modelBuilder.Entity("HelenExpress.Data.Entities.Bill", b =>
                {
                    b.HasOne("HelenExpress.Data.Entities.Customer", "Receiver")
                        .WithMany("ReceivedBills")
                        .HasForeignKey("ReceiverId")
                        .HasConstraintName("fk_bill_customers_receiverid");

                    b.HasOne("HelenExpress.Data.Entities.Customer", "Sender")
                        .WithMany("SendBills")
                        .HasForeignKey("SenderId")
                        .HasConstraintName("fk_bill_customers_senderid");

                    b.HasOne("HelenExpress.Data.Entities.Vendor", "Vendor")
                        .WithMany("Bills")
                        .HasForeignKey("VendorId")
                        .HasConstraintName("fk_bill_vendors_vendorid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HelenExpress.Data.Entities.Zone", b =>
                {
                    b.HasOne("HelenExpress.Data.Entities.Vendor", "Vendor")
                        .WithMany("Zones")
                        .HasForeignKey("VendorId")
                        .HasConstraintName("fk_zone_vendor_vendorid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

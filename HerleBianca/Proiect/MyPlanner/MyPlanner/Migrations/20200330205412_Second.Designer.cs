﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyPlanner.Data;

namespace MyPlanner.Migrations
{
    [DbContext(typeof(MyPlannerContext))]
    [Migration("20200330205412_Second")]
    partial class Second
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyPlanner.Models.MyTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Asignee")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Due_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Movement")
                        .HasColumnType("int");

                    b.Property<string>("Owner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Physical_Effort")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Tag")
                        .HasColumnType("int");

                    b.Property<int>("Urgency")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MyTask");
                });

            modelBuilder.Entity("MyPlanner.Models.User", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("age")
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("encrypted_password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("first_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("last_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("other_ocupation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone_number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("picture_path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("rating")
                        .HasColumnType("int");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}

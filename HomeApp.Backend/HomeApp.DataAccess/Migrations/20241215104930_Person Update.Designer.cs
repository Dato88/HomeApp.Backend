﻿// <auto-generated />
using System;
using HomeApp.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeApp.DataAccess.Migrations
{
    [DbContext(typeof(HomeAppContext))]
    [Migration("20241215104930_Person Update")]
    partial class PersonCommandsUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetCell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BudgetColumnId")
                        .HasColumnType("integer");

                    b.Property<int>("BudgetGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("BudgetRowId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BudgetColumnId");

                    b.HasIndex("BudgetGroupId");

                    b.HasIndex("BudgetRowId");

                    b.HasIndex("PersonId");

                    b.ToTable("BudgetCells");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetColumn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("BudgetColumns");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("BudgetGroups");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetRow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("BudgetRows");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Username")
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("TodoDone")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("TodoExecutionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TodoName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<int>("TodoPriority")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.TodoGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TodoGroupName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("TodoGroups");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.TodoGroupMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TodoGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("TodoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TodoGroupId");

                    b.HasIndex("TodoId");

                    b.ToTable("TodoGroupMapping");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.TodoUserMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("PersonId")
                        .HasColumnType("integer");

                    b.Property<int>("TodoId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("TodoId");

                    b.ToTable("TodoUserMapping");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetCell", b =>
                {
                    b.HasOne("HomeApp.DataAccess.Models.BudgetColumn", "BudgetColumn")
                        .WithMany("BudgetCells")
                        .HasForeignKey("BudgetColumnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeApp.DataAccess.Models.BudgetGroup", "BudgetGroup")
                        .WithMany("BudgetCells")
                        .HasForeignKey("BudgetGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeApp.DataAccess.Models.BudgetRow", "BudgetRow")
                        .WithMany("BudgetCells")
                        .HasForeignKey("BudgetRowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeApp.DataAccess.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BudgetColumn");

                    b.Navigation("BudgetGroup");

                    b.Navigation("BudgetRow");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetGroup", b =>
                {
                    b.HasOne("HomeApp.DataAccess.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetRow", b =>
                {
                    b.HasOne("HomeApp.DataAccess.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.TodoGroupMapping", b =>
                {
                    b.HasOne("HomeApp.DataAccess.Models.TodoGroup", "TodoGroup")
                        .WithMany("TodoGroupMappings")
                        .HasForeignKey("TodoGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeApp.DataAccess.Models.Todo", "Todo")
                        .WithMany("TodoGroupMappings")
                        .HasForeignKey("TodoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Todo");

                    b.Navigation("TodoGroup");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.TodoUserMapping", b =>
                {
                    b.HasOne("HomeApp.DataAccess.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");

                    b.HasOne("HomeApp.DataAccess.Models.Todo", "Todo")
                        .WithMany("TodoUserMappings")
                        .HasForeignKey("TodoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Todo");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetColumn", b =>
                {
                    b.Navigation("BudgetCells");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetGroup", b =>
                {
                    b.Navigation("BudgetCells");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.BudgetRow", b =>
                {
                    b.Navigation("BudgetCells");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.Todo", b =>
                {
                    b.Navigation("TodoGroupMappings");

                    b.Navigation("TodoUserMappings");
                });

            modelBuilder.Entity("HomeApp.DataAccess.Models.TodoGroup", b =>
                {
                    b.Navigation("TodoGroupMappings");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TestSystem.Models;

#nullable disable

namespace TestSystem.Migrations
{
    [DbContext(typeof(TestingSystemDbContext))]
    [Migration("20231108064505_addTest")]
    partial class addTest
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TestSystem.Models.AskAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AnswerValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("TestAskId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TestAskId");

                    b.ToTable("AskAnswer");
                });

            modelBuilder.Entity("TestSystem.Models.AskType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AskType");
                });

            modelBuilder.Entity("TestSystem.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("TestSystem.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("TestSystem.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatorUserId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatorUserId");

                    b.ToTable("Test");
                });

            modelBuilder.Entity("TestSystem.Models.TestAsk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Ask")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RightAnswerId")
                        .HasColumnType("integer");

                    b.Property<int?>("TestAskId")
                        .HasColumnType("integer");

                    b.Property<int?>("TestId")
                        .HasColumnType("integer");

                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RightAnswerId");

                    b.HasIndex("TestAskId");

                    b.HasIndex("TestId");

                    b.HasIndex("TypeId");

                    b.ToTable("TestAsk");
                });

            modelBuilder.Entity("TestSystem.Models.TestUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("RightAnswersCount")
                        .HasColumnType("integer");

                    b.Property<int>("TestId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.HasIndex("UserId");

                    b.ToTable("TestUser");
                });

            modelBuilder.Entity("TestSystem.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserRoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserRoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TestSystem.Models.AskAnswer", b =>
                {
                    b.HasOne("TestSystem.Models.TestAsk", null)
                        .WithMany("Answers")
                        .HasForeignKey("TestAskId");
                });

            modelBuilder.Entity("TestSystem.Models.Test", b =>
                {
                    b.HasOne("TestSystem.Models.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreatorUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatorUser");
                });

            modelBuilder.Entity("TestSystem.Models.TestAsk", b =>
                {
                    b.HasOne("TestSystem.Models.AskAnswer", "RightAnswer")
                        .WithMany()
                        .HasForeignKey("RightAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestSystem.Models.TestAsk", null)
                        .WithMany("SubAsk")
                        .HasForeignKey("TestAskId");

                    b.HasOne("TestSystem.Models.Test", null)
                        .WithMany("Asks")
                        .HasForeignKey("TestId");

                    b.HasOne("TestSystem.Models.AskType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RightAnswer");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("TestSystem.Models.TestUser", b =>
                {
                    b.HasOne("TestSystem.Models.Test", "Test")
                        .WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestSystem.Models.User", "User")
                        .WithMany("CompletedTests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TestSystem.Models.User", b =>
                {
                    b.HasOne("TestSystem.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("TestSystem.Models.Role", "UserRole")
                        .WithMany()
                        .HasForeignKey("UserRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("TestSystem.Models.Test", b =>
                {
                    b.Navigation("Asks");
                });

            modelBuilder.Entity("TestSystem.Models.TestAsk", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("SubAsk");
                });

            modelBuilder.Entity("TestSystem.Models.User", b =>
                {
                    b.Navigation("CompletedTests");
                });
#pragma warning restore 612, 618
        }
    }
}

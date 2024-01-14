﻿// <auto-generated />
using System;
using FileToEmailLinker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileToEmailLinker.Migrations
{
    [DbContext(typeof(FileToEmailLinkerContext))]
    [Migration("20240114112630_ReceiverCanHaveMultipleMailingPlans")]
    partial class ReceiverCanHaveMultipleMailingPlans
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("FileToEmailLinker.Models.Entities.FileRef", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FileRef");
                });

            modelBuilder.Entity("FileToEmailLinker.Models.Entities.MailingPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ActiveState")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileStringList")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SchedulationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SchedulationId");

                    b.ToTable("MailingPlan");
                });

            modelBuilder.Entity("FileToEmailLinker.Models.Entities.Receiver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Receiver");
                });

            modelBuilder.Entity("FileToEmailLinker.Models.Entities.Schedulation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Recurrence")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Schedulation");
                });

            modelBuilder.Entity("MailingPlanReceiver", b =>
                {
                    b.Property<int>("MailingPlanListId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReceiverListId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MailingPlanListId", "ReceiverListId");

                    b.HasIndex("ReceiverListId");

                    b.ToTable("MailingPlanReceiver");
                });

            modelBuilder.Entity("FileToEmailLinker.Models.Entities.MailingPlan", b =>
                {
                    b.HasOne("FileToEmailLinker.Models.Entities.Schedulation", "Schedulation")
                        .WithMany()
                        .HasForeignKey("SchedulationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedulation");
                });

            modelBuilder.Entity("MailingPlanReceiver", b =>
                {
                    b.HasOne("FileToEmailLinker.Models.Entities.MailingPlan", null)
                        .WithMany()
                        .HasForeignKey("MailingPlanListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FileToEmailLinker.Models.Entities.Receiver", null)
                        .WithMany()
                        .HasForeignKey("ReceiverListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

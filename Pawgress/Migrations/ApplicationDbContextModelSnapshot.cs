﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pawgress.Data;

#nullable disable

namespace Pawgress.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Pawgress.Models.DogProfile", b =>
                {
                    b.Property<Guid>("DogProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DogProfileId");

                    b.ToTable("DogProfiles");
                });

            modelBuilder.Entity("Pawgress.Models.Folder", b =>
                {
                    b.Property<Guid>("FolderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentFolderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FolderId");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("Pawgress.Models.Lesson", b =>
                {
                    b.Property<Guid>("LessonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TrainingPathId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Video")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LessonId");

                    b.HasIndex("FolderId");

                    b.HasIndex("TrainingPathId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("Pawgress.Models.Note", b =>
                {
                    b.Property<Guid>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DogProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("NoteId");

                    b.HasIndex("DogProfileId");

                    b.HasIndex("UserId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Pawgress.Models.Quiz", b =>
                {
                    b.Property<Guid>("QuizId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AchievedScore")
                        .HasColumnType("int");

                    b.Property<Guid?>("FolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaxScore")
                        .HasColumnType("int");

                    b.Property<string>("QuizName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TrainingPathId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuizId");

                    b.HasIndex("FolderId");

                    b.HasIndex("TrainingPathId");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("Pawgress.Models.TrainingPath", b =>
                {
                    b.Property<Guid>("TrainingPathId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TrainingPathId");

                    b.ToTable("TrainingPaths");
                });

            modelBuilder.Entity("Pawgress.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProgressData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Pawgress.Models.User_DogProfile", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DogProfileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "DogProfileId");

                    b.HasIndex("DogProfileId");

                    b.ToTable("UserDogProfiles");
                });

            modelBuilder.Entity("Pawgress.Models.User_TrainingPath", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TrainingPathId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CompletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Progress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "TrainingPathId");

                    b.HasIndex("TrainingPathId");

                    b.ToTable("UserTrainingPaths");
                });

            modelBuilder.Entity("Pawgress.Models.Folder", b =>
                {
                    b.HasOne("Pawgress.Models.Folder", "ParentFolder")
                        .WithMany("SubFolders")
                        .HasForeignKey("ParentFolderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("Pawgress.Models.Lesson", b =>
                {
                    b.HasOne("Pawgress.Models.Folder", null)
                        .WithMany("Lessons")
                        .HasForeignKey("FolderId");

                    b.HasOne("Pawgress.Models.TrainingPath", "TrainingPath")
                        .WithMany("Lessons")
                        .HasForeignKey("TrainingPathId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingPath");
                });

            modelBuilder.Entity("Pawgress.Models.Note", b =>
                {
                    b.HasOne("Pawgress.Models.DogProfile", "DogProfile")
                        .WithMany("Notes")
                        .HasForeignKey("DogProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pawgress.Models.User", "User")
                        .WithMany("Notes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DogProfile");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pawgress.Models.Quiz", b =>
                {
                    b.HasOne("Pawgress.Models.Folder", null)
                        .WithMany("Quizzes")
                        .HasForeignKey("FolderId");

                    b.HasOne("Pawgress.Models.TrainingPath", "TrainingPath")
                        .WithMany("Quizzes")
                        .HasForeignKey("TrainingPathId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingPath");
                });

            modelBuilder.Entity("Pawgress.Models.User_DogProfile", b =>
                {
                    b.HasOne("Pawgress.Models.DogProfile", "DogProfile")
                        .WithMany("UserDogProfiles")
                        .HasForeignKey("DogProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pawgress.Models.User", "User")
                        .WithMany("DogProfiles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DogProfile");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pawgress.Models.User_TrainingPath", b =>
                {
                    b.HasOne("Pawgress.Models.TrainingPath", "TrainingPath")
                        .WithMany("Users")
                        .HasForeignKey("TrainingPathId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pawgress.Models.User", "User")
                        .WithMany("TrainingPaths")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingPath");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pawgress.Models.DogProfile", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("UserDogProfiles");
                });

            modelBuilder.Entity("Pawgress.Models.Folder", b =>
                {
                    b.Navigation("Lessons");

                    b.Navigation("Quizzes");

                    b.Navigation("SubFolders");
                });

            modelBuilder.Entity("Pawgress.Models.TrainingPath", b =>
                {
                    b.Navigation("Lessons");

                    b.Navigation("Quizzes");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Pawgress.Models.User", b =>
                {
                    b.Navigation("DogProfiles");

                    b.Navigation("Notes");

                    b.Navigation("TrainingPaths");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using notepad_react.Models;

namespace notepad_react.Migrations
{
    [DbContext(typeof(NotesContext))]
    [Migration("20200114165429_markdown")]
    partial class markdown
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("notepad_react.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("NVarChar(64)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("notepad_react.Models.Note", b =>
                {
                    b.Property<int>("NoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("NVarChar(MAX)");

                    b.Property<bool>("IsMarkdown")
                        .HasColumnType("bit");

                    b.Property<DateTime>("NoteDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .HasColumnType("NVarChar(64)");

                    b.HasKey("NoteID");

                    b.HasIndex("Title")
                        .IsUnique()
                        .HasFilter("[Title] IS NOT NULL");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("notepad_react.Models.NoteCategory", b =>
                {
                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<int>("NoteID")
                        .HasColumnType("int");

                    b.HasKey("CategoryID", "NoteID");

                    b.HasIndex("NoteID");

                    b.ToTable("NoteCategories");
                });

            modelBuilder.Entity("notepad_react.Models.NoteCategory", b =>
                {
                    b.HasOne("notepad_react.Models.Category", "Category")
                        .WithMany("NoteCategories")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("notepad_react.Models.Note", "Note")
                        .WithMany("NoteCategories")
                        .HasForeignKey("NoteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

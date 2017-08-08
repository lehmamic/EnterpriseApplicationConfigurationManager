using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Zuehlke.Eacm.Web.Backend.DataAccess;

namespace Zuehlke.Eacm.Web.Backend.Migrations
{
    [DbContext(typeof(EacmDbContext))]
    [Migration("20170808122621_RemoveAggregateName")]
    partial class RemoveAggregateName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(4000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<Guid>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId", "Name")
                        .IsUnique();

                    b.ToTable("Entities");
                });

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("EntityId");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationProject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(4000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<DateTimeOffset>("TimeStamp");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationProperty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(4000);

                    b.Property<Guid>("EntityId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("PropertyType")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("EntityId", "Name")
                        .IsUnique();

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("EntryId");

                    b.Property<Guid>("PropertyId");

                    b.Property<string>("Value")
                        .HasMaxLength(4000);

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.HasIndex("EntryId", "PropertyId")
                        .IsUnique();

                    b.ToTable("Values");
                });

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AggregateId");

                    b.Property<string>("CorrelationId");

                    b.Property<string>("Payload")
                        .IsRequired();

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationEntity", b =>
                {
                    b.HasOne("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationProject", "Project")
                        .WithMany("Entities")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationEntry", b =>
                {
                    b.HasOne("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationEntity", "Entity")
                        .WithMany("Entries")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationProperty", b =>
                {
                    b.HasOne("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationEntity", "Entity")
                        .WithMany("Properties")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationValue", b =>
                {
                    b.HasOne("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationEntry", "Entry")
                        .WithMany("Values")
                        .HasForeignKey("EntryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Zuehlke.Eacm.Web.Backend.DataAccess.ConfigurationProperty", "Property")
                        .WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

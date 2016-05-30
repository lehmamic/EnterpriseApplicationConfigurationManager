using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Zuehlke.Eacm.Web.Backend.DataAccess;

namespace Zuehlke.Eacm.Web.Backend.Migrations
{
    [DbContext(typeof(EacmDbContext))]
    [Migration("20160530135812_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Zuehlke.Eacm.Web.Backend.DataAccess.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AggregateId");

                    b.Property<string>("AggregateType")
                        .IsRequired();

                    b.Property<string>("CorrelationId");

                    b.Property<string>("Payload")
                        .IsRequired();

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });
        }
    }
}

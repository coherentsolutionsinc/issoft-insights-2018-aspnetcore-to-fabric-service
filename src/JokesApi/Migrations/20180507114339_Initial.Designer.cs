﻿// <auto-generated />
using JokesApi.Impl.Services.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace JokesApi.Migrations
{
    [DbContext(typeof(DbJokesContext))]
    [Migration("20180507114339_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JokesApi.Impl.Services.EntityFramework.DbJokeEntity", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Author");

                    b.Property<string>("Category");

                    b.Property<string>("Content");

                    b.Property<string>("Language");

                    b.Property<string>("Name");

                    b.Property<DateTime>("PublishDate");

                    b.HasKey("Id");

                    b.ToTable("Jokes");
                });
#pragma warning restore 612, 618
        }
    }
}

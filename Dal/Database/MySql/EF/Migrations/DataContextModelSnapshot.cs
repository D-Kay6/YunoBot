﻿// <auto-generated />

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Dal.Database.MySql.EF.Migrations
{
    [DbContext(typeof(DataContext))]
    internal class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Entity.AutoChannel", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<bool>("Enabled")
                    .HasColumnType("tinyint(1)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                    .HasMaxLength(100);

                b.Property<string>("Prefix")
                    .IsRequired()
                    .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                    .HasMaxLength(100);

                b.HasKey("ServerId");

                b.ToTable("AutoChannel");
            });

            modelBuilder.Entity("Entity.AutoRole", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<bool>("Enabled")
                    .HasColumnType("tinyint(1)");

                b.Property<string>("Prefix")
                    .IsRequired()
                    .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                    .HasMaxLength(100);

                b.HasKey("ServerId");

                b.ToTable("AutoRole");
            });

            modelBuilder.Entity("Entity.Ban", b =>
            {
                b.Property<ulong>("UserId")
                    .HasColumnType("bigint unsigned");

                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<DateTime?>("EndDate")
                    .HasColumnType("datetime(6)");

                b.Property<string>("Reason")
                    .HasColumnType("longtext CHARACTER SET utf8mb4");

                b.HasKey("UserId", "ServerId");

                b.HasIndex("ServerId");

                b.ToTable("Ban");
            });

            modelBuilder.Entity("Entity.CommandSetting", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<string>("Prefix")
                    .IsRequired()
                    .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                    .HasMaxLength(20);

                b.HasKey("ServerId");

                b.ToTable("CommandSetting");
            });

            modelBuilder.Entity("Entity.CustomCommand", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<string>("Command")
                    .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                    .HasMaxLength(50);

                b.Property<string>("Response")
                    .IsRequired()
                    .HasColumnType("longtext CHARACTER SET utf8mb4");

                b.HasKey("ServerId", "Command");

                b.ToTable("CustomCommand");
            });

            modelBuilder.Entity("Entity.GeneratedChannel", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<ulong>("ChannelId")
                    .HasColumnType("bigint unsigned");

                b.HasKey("ServerId", "ChannelId");

                b.ToTable("GeneratedChannel");
            });

            modelBuilder.Entity("Entity.LanguageSetting", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<int>("Language")
                    .HasColumnType("int");

                b.HasKey("ServerId");

                b.ToTable("LanguageSetting");
            });

            modelBuilder.Entity("Entity.PermaChannel", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<bool>("Enabled")
                    .HasColumnType("tinyint(1)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                    .HasMaxLength(100);

                b.Property<string>("Prefix")
                    .IsRequired()
                    .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                    .HasMaxLength(100);

                b.HasKey("ServerId");

                b.ToTable("PermaChannel");
            });

            modelBuilder.Entity("Entity.PermaRole", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<bool>("Enabled")
                    .HasColumnType("tinyint(1)");

                b.Property<string>("Prefix")
                    .IsRequired()
                    .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                    .HasMaxLength(100);

                b.HasKey("ServerId");

                b.ToTable("PermaRole");
            });

            modelBuilder.Entity("Entity.RoleIgnore", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<ulong>("UserId")
                    .HasColumnType("bigint unsigned");

                b.HasKey("ServerId", "UserId");

                b.HasIndex("UserId");

                b.ToTable("RoleIgnore");
            });

            modelBuilder.Entity("Entity.Server", b =>
            {
                b.Property<ulong>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint unsigned");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                    .HasMaxLength(100);

                b.HasKey("Id");

                b.ToTable("Server");
            });

            modelBuilder.Entity("Entity.User", b =>
            {
                b.Property<ulong>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint unsigned");

                b.Property<string>("Name")
                    .HasColumnType("varchar(32) CHARACTER SET utf8mb4")
                    .HasMaxLength(32);

                b.HasKey("Id");

                b.ToTable("User");
            });

            modelBuilder.Entity("Entity.WelcomeMessage", b =>
            {
                b.Property<ulong>("ServerId")
                    .HasColumnType("bigint unsigned");

                b.Property<ulong?>("ChannelId")
                    .HasColumnType("bigint unsigned");

                b.Property<string>("Message")
                    .IsRequired()
                    .HasColumnType("varchar(2000) CHARACTER SET utf8mb4")
                    .HasMaxLength(2000);

                b.Property<bool>("UseImage")
                    .HasColumnType("tinyint(1)");

                b.HasKey("ServerId");

                b.ToTable("WelcomeMessage");
            });

            modelBuilder.Entity("Entity.AutoChannel", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithOne("AutoChannel")
                    .HasForeignKey("Entity.AutoChannel", "ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.AutoRole", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithOne("AutoRole")
                    .HasForeignKey("Entity.AutoRole", "ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.Ban", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithMany()
                    .HasForeignKey("ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Entity.User", "User")
                    .WithMany("Bans")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.CommandSetting", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithOne("CommandSetting")
                    .HasForeignKey("Entity.CommandSetting", "ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.CustomCommand", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithMany("CustomCommands")
                    .HasForeignKey("ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.GeneratedChannel", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithMany("GeneratedChannels")
                    .HasForeignKey("ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Entity.AutoChannel", "AutoChannel")
                    .WithMany("Channels")
                    .HasForeignKey("ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.LanguageSetting", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithOne("LanguageSetting")
                    .HasForeignKey("Entity.LanguageSetting", "ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.PermaChannel", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithOne("PermaChannel")
                    .HasForeignKey("Entity.PermaChannel", "ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.PermaRole", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithOne("PermaRole")
                    .HasForeignKey("Entity.PermaRole", "ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.RoleIgnore", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithMany("IgnoredUsers")
                    .HasForeignKey("ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Entity.User", "User")
                    .WithMany("IgnoredRoles")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Entity.WelcomeMessage", b =>
            {
                b.HasOne("Entity.Server", "Server")
                    .WithOne("WelcomeMessage")
                    .HasForeignKey("Entity.WelcomeMessage", "ServerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
#pragma warning restore 612, 618
        }
    }
}
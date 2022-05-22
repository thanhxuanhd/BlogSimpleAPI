﻿// <auto-generated />
using System;
using Blog.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blog.WebApi.Migrations;

[DbContext(typeof(BlogDbContext))]
partial class BlogDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "7.0.0-preview.3.22175.1")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

        modelBuilder.Entity("Blog.Core.Model.Comment", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid?>("ChangeBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("ChangeOn")
                    .HasColumnType("datetime2");

                b.Property<string>("Content")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("CreateBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("CreateOn")
                    .HasColumnType("datetime2");

                b.Property<Guid?>("DeleteBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("DeleteOn")
                    .HasColumnType("datetime2");

                b.Property<Guid?>("PostId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Title")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("PostId");

                b.ToTable("Comments");
            });

        modelBuilder.Entity("Blog.Core.Model.Page", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Alias")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<Guid?>("ChangeBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("ChangeOn")
                    .HasColumnType("datetime2");

                b.Property<string>("Content")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("CreateBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("CreateOn")
                    .HasColumnType("datetime2");

                b.Property<Guid?>("DeleteBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("DeleteOn")
                    .HasColumnType("datetime2");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.HasKey("Id");

                b.ToTable("Pages");
            });

        modelBuilder.Entity("Blog.Core.Model.Post", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid?>("ChangeBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("ChangeOn")
                    .HasColumnType("datetime2");

                b.Property<string>("Content")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("CreateBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("CreateOn")
                    .HasColumnType("datetime2");

                b.Property<Guid?>("DeleteBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("DeleteOn")
                    .HasColumnType("datetime2");

                b.Property<bool>("IsPublic")
                    .HasColumnType("bit");

                b.Property<string>("MetaData")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("MetaDescription")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("PostCategoryId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Title")
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.Property<string>("Url")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("PostCategoryId");

                b.ToTable("Posts");
            });

        modelBuilder.Entity("Blog.Core.Model.PostCategory", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("CategoryDescription")
                    .HasMaxLength(5000)
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("CategoryName")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<Guid?>("ChangeBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("ChangeOn")
                    .HasColumnType("datetime2");

                b.Property<Guid>("CreateBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("CreateOn")
                    .HasColumnType("datetime2");

                b.Property<Guid?>("DeleteBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("DeleteOn")
                    .HasColumnType("datetime2");

                b.Property<bool>("IsPublic")
                    .HasColumnType("bit");

                b.Property<string>("MetaData")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("MetaDescription")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid?>("ParentId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Url")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("ParentId");

                b.ToTable("PostCategorys");
            });

        modelBuilder.Entity("Blog.Core.Model.PostTag", b =>
            {
                b.Property<Guid>("PostID")
                    .HasColumnType("uniqueidentifier")
                    .HasColumnOrder(1);

                b.Property<Guid>("TagID")
                    .HasMaxLength(50)
                    .HasColumnType("uniqueidentifier")
                    .HasColumnOrder(2);

                b.HasKey("PostID", "TagID");

                b.HasIndex("TagID");

                b.ToTable("PostTags", (string)null);
            });

        modelBuilder.Entity("Blog.Core.Model.Tag", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid?>("ChangeBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("ChangeOn")
                    .HasColumnType("datetime2");

                b.Property<Guid>("CreateBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("CreateOn")
                    .HasColumnType("datetime2");

                b.Property<Guid?>("DeleteBy")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("DeleteOn")
                    .HasColumnType("datetime2");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Type")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("Id");

                b.ToTable("Tags");
            });

        modelBuilder.Entity("Blog.Core.Model.User", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("int");

                b.Property<DateTime>("BirthDay")
                    .HasColumnType("datetime2");

                b.Property<string>("ConcurrencyStamp")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Email")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("bit");

                b.Property<string>("FullName")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("IsActive")
                    .HasColumnType("bit");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("bit");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("datetimeoffset");

                b.Property<string>("NormalizedEmail")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("NormalizedUserName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PasswordHash")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("bit");

                b.Property<string>("RefreshTokenHash")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Sex")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("bit");

                b.Property<string>("UserName")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("AppUsers", (string)null);
            });

        modelBuilder.Entity("Blog.Core.Model.UserRole", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("ConcurrencyStamp")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Description")
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("NormalizedName")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("AppRoles", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("RoleId")
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid?>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AppRoleClaims", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AppUserClaims", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
            {
                b.Property<Guid>("UserId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ProviderKey")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("UserId1")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("UserId");

                b.HasIndex("UserId1");

                b.ToTable("AppUserLogins", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
            {
                b.Property<Guid>("RoleId")
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid?>("UserRoleId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("RoleId", "UserId");

                b.HasIndex("UserId");

                b.HasIndex("UserRoleId");

                b.ToTable("AppUserRoles", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
            {
                b.Property<Guid>("UserId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("UserId1")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Value")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("UserId");

                b.HasIndex("UserId1");

                b.ToTable("AppUserTokens", (string)null);
            });

        modelBuilder.Entity("Microsoft.EntityFrameworkCore.AutoHistory", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("Changed")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("Created")
                    .HasColumnType("datetime2");

                b.Property<int>("Kind")
                    .HasColumnType("int");

                b.Property<string>("RowId")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("TableName")
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnType("nvarchar(128)");

                b.HasKey("Id");

                b.ToTable("AutoHistory");
            });

        modelBuilder.Entity("Blog.Core.Model.Comment", b =>
            {
                b.HasOne("Blog.Core.Model.Post", null)
                    .WithMany("Comments")
                    .HasForeignKey("PostId");
            });

        modelBuilder.Entity("Blog.Core.Model.Post", b =>
            {
                b.HasOne("Blog.Core.Model.PostCategory", null)
                    .WithMany("Posts")
                    .HasForeignKey("PostCategoryId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Blog.Core.Model.PostCategory", b =>
            {
                b.HasOne("Blog.Core.Model.PostCategory", null)
                    .WithMany("PostCategories")
                    .HasForeignKey("ParentId");
            });

        modelBuilder.Entity("Blog.Core.Model.PostTag", b =>
            {
                b.HasOne("Blog.Core.Model.Post", "Post")
                    .WithMany()
                    .HasForeignKey("PostID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Blog.Core.Model.Tag", "Tag")
                    .WithMany()
                    .HasForeignKey("TagID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Post");

                b.Navigation("Tag");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
            {
                b.HasOne("Blog.Core.Model.User", null)
                    .WithMany("AppRoleClaims")
                    .HasForeignKey("UserId");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
            {
                b.HasOne("Blog.Core.Model.User", null)
                    .WithMany("AppUserClaims")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
            {
                b.HasOne("Blog.Core.Model.User", null)
                    .WithMany("AppUserLogins")
                    .HasForeignKey("UserId1")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
            {
                b.HasOne("Blog.Core.Model.User", null)
                    .WithMany("AppUserRoles")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Blog.Core.Model.UserRole", null)
                    .WithMany("AppUserRoles")
                    .HasForeignKey("UserRoleId");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
            {
                b.HasOne("Blog.Core.Model.User", null)
                    .WithMany("AppUserTokens")
                    .HasForeignKey("UserId1")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Blog.Core.Model.Post", b =>
            {
                b.Navigation("Comments");
            });

        modelBuilder.Entity("Blog.Core.Model.PostCategory", b =>
            {
                b.Navigation("PostCategories");

                b.Navigation("Posts");
            });

        modelBuilder.Entity("Blog.Core.Model.User", b =>
            {
                b.Navigation("AppRoleClaims");

                b.Navigation("AppUserClaims");

                b.Navigation("AppUserLogins");

                b.Navigation("AppUserRoles");

                b.Navigation("AppUserTokens");
            });

        modelBuilder.Entity("Blog.Core.Model.UserRole", b =>
            {
                b.Navigation("AppUserRoles");
            });
#pragma warning restore 612, 618
    }
}

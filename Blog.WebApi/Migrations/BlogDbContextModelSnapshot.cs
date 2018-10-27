﻿// <auto-generated />
using System;
using Blog.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blog.WebApi.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    partial class BlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview2-35157")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Blog.Core.Model.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ChangeBy");

                    b.Property<DateTime?>("ChangeOn");

                    b.Property<string>("Content");

                    b.Property<Guid>("CreateBy");

                    b.Property<DateTime>("CreateOn");

                    b.Property<Guid?>("DeleteBy");

                    b.Property<DateTime?>("DeleteOn");

                    b.Property<Guid?>("PostId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Blog.Core.Model.Page", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<Guid?>("ChangeBy");

                    b.Property<DateTime?>("ChangeOn");

                    b.Property<string>("Content");

                    b.Property<Guid>("CreateBy");

                    b.Property<DateTime>("CreateOn");

                    b.Property<Guid?>("DeleteBy");

                    b.Property<DateTime?>("DeleteOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("Blog.Core.Model.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ChangeBy");

                    b.Property<DateTime?>("ChangeOn");

                    b.Property<string>("Content");

                    b.Property<Guid>("CreateBy");

                    b.Property<DateTime>("CreateOn");

                    b.Property<Guid?>("DeleteBy");

                    b.Property<DateTime?>("DeleteOn");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("MetaData");

                    b.Property<string>("MetaDescription");

                    b.Property<Guid>("PostCategoryId");

                    b.Property<string>("Title")
                        .HasMaxLength(200);

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("PostCategoryId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Blog.Core.Model.PostCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryDescription")
                        .HasMaxLength(5000);

                    b.Property<string>("CategoryName")
                        .HasMaxLength(50);

                    b.Property<Guid?>("ChangeBy");

                    b.Property<DateTime?>("ChangeOn");

                    b.Property<Guid>("CreateBy");

                    b.Property<DateTime>("CreateOn");

                    b.Property<Guid?>("DeleteBy");

                    b.Property<DateTime?>("DeleteOn");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("MetaData");

                    b.Property<string>("MetaDescription");

                    b.Property<Guid?>("ParentId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("PostCategorys");
                });

            modelBuilder.Entity("Blog.Core.Model.PostTag", b =>
                {
                    b.Property<Guid>("PostID");

                    b.Property<Guid>("TagID")
                        .HasMaxLength(50);

                    b.HasKey("PostID", "TagID");

                    b.HasIndex("TagID");

                    b.ToTable("PostTags");
                });

            modelBuilder.Entity("Blog.Core.Model.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ChangeBy");

                    b.Property<DateTime?>("ChangeOn");

                    b.Property<Guid>("CreateBy");

                    b.Property<DateTime>("CreateOn");

                    b.Property<Guid?>("DeleteBy");

                    b.Property<DateTime?>("DeleteOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Blog.Core.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime>("BirthDay");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("RefreshTokenHash");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Sex");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("Blog.Core.Model.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Description")
                        .HasMaxLength(256);

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.HasKey("Id");

                    b.ToTable("AppRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AppRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AppUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("ProviderKey");

                    b.Property<Guid>("UserId1");

                    b.HasKey("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("AppUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("RoleId");

                    b.Property<Guid>("UserId");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("AppUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<Guid>("UserId1");

                    b.Property<string>("Value");

                    b.HasKey("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("AppUserTokens");
                });

            modelBuilder.Entity("Microsoft.EntityFrameworkCore.AutoHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Changed");

                    b.Property<DateTime>("Created");

                    b.Property<int>("Kind");

                    b.Property<string>("RowId")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("AutoHistory");
                });

            modelBuilder.Entity("Blog.Core.Model.Comment", b =>
                {
                    b.HasOne("Blog.Core.Model.Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("Blog.Core.Model.Post", b =>
                {
                    b.HasOne("Blog.Core.Model.PostCategory")
                        .WithMany("Posts")
                        .HasForeignKey("PostCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Blog.Core.Model.PostCategory", b =>
                {
                    b.HasOne("Blog.Core.Model.PostCategory")
                        .WithMany("PostCategories")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Blog.Core.Model.PostTag", b =>
                {
                    b.HasOne("Blog.Core.Model.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Blog.Core.Model.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Blog.Core.Model.User")
                        .WithMany("AppRoleClaims")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Blog.Core.Model.User")
                        .WithMany("AppUserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Blog.Core.Model.User")
                        .WithMany("AppUserLogins")
                        .HasForeignKey("UserId1")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Blog.Core.Model.User")
                        .WithMany("AppUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Blog.Core.Model.User")
                        .WithMany("AppUserTokens")
                        .HasForeignKey("UserId1")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

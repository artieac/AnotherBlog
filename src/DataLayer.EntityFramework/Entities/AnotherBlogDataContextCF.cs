/**
 * Copyright (c) 2009 Arthur Correa.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Common Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/cpl1.0.php
 *
 * Contributors:
 *    Arthur Correa – initial contribution
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.DataLayer.MappingDomainObjects;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Entities
{
    public class AnotherBlogDataContextCF : DbContext
    {
        private readonly string _connectionString;

        public AnotherBlogDataContextCF(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogExtension> BlogExtensions { get; set; }
        public DbSet<BlogList> BlogLists { get; set; }
        public DbSet<BlogListItem> BlogListItems { get; set; }
        public DbSet<BlogUser> BlogUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<DbInfo> DbInfos { get; set; }
        public DbSet<ExtensionConfiguration> ExtensionConfigurations { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SiteInfo> SiteInfos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AnotherBlogUser> Users { get; set; }
        public DbSet<TagCount> TagCounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Blog
            modelBuilder.Entity<Blog>().ToTable("Blogs");
            modelBuilder.Entity<Blog>().HasKey(b => b.Id);

            // BlogPost
            modelBuilder.Entity<BlogPost>().ToTable("BlogEntries");
            modelBuilder.Entity<BlogPost>().HasKey(bp => bp.Id);
            modelBuilder.Entity<BlogPost>().Ignore(bp => bp.Tags);
            modelBuilder.Entity<BlogPost>().Ignore(bp => bp.CommentCount);
            modelBuilder.Entity<BlogPost>()
                .HasOne(bp => bp.Blog)
                .WithMany()
                .HasForeignKey("BlogId");
            modelBuilder.Entity<BlogPost>()
                .Navigation(bp => bp.Blog)
                .AutoInclude();
            modelBuilder.Entity<BlogPost>()
                .HasOne<AnotherBlogUser>(bp => bp.Author)
                .WithMany()
                .HasForeignKey("UserId");
            modelBuilder.Entity<BlogPost>()
                .Navigation(bp => bp.Author)
                .AutoInclude();

            // BlogExtension
            modelBuilder.Entity<BlogExtension>().ToTable("BlogExtensions");
            modelBuilder.Entity<BlogExtension>().HasKey(be => be.Id);

            // BlogList
            modelBuilder.Entity<BlogList>().ToTable("BlogLists");
            modelBuilder.Entity<BlogList>().HasKey(bl => bl.Id);
            modelBuilder.Entity<BlogList>()
                .HasOne(bl => bl.Blog)
                .WithMany()
                .HasForeignKey(bl => bl.BlogId);
            modelBuilder.Entity<BlogList>()
                .HasMany(bl => bl.Items)
                .WithOne(bli => bli.BlogList)
                .HasForeignKey(bli => bli.BlogListId);
            modelBuilder.Entity<BlogList>()
                .Navigation(bp => bp.Items)
                .AutoInclude();

            // BlogListItem
            modelBuilder.Entity<BlogListItem>().ToTable("BlogListItems");
            modelBuilder.Entity<BlogListItem>().HasKey(bli => bli.Id);

            // BlogUser
            modelBuilder.Entity<BlogUser>().ToTable("BlogUsers");
            modelBuilder.Entity<BlogUser>().HasKey(bu => bu.Id);
            modelBuilder.Entity<BlogUser>()
                .HasOne(bu => bu.Blog)
                .WithMany()
                .HasForeignKey("BlogId");
            modelBuilder.Entity<BlogUser>()
                .HasOne<AnotherBlogUser>(bu => bu.User)
                .WithMany()
                .HasForeignKey("UserId");
            modelBuilder.Entity<BlogUser>()
                .HasOne(bu => bu.Role)
                .WithOne()
                .HasForeignKey<BlogUser>("RoleId");

            // Comment
            modelBuilder.Entity<Comment>().ToTable("EntryComments");
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().Property(c => c.Text).HasColumnName("Comment");
            modelBuilder.Entity<Comment>().Property(c => c.Status).HasConversion<int>();
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(bp => bp.Comments)
                .HasForeignKey("EntryId");

            // DbInfo
            modelBuilder.Entity<DbInfo>().ToTable("DbInfo");
            modelBuilder.Entity<DbInfo>().HasKey(d => d.Version);

            // ExtensionConfiguration
            modelBuilder.Entity<ExtensionConfiguration>().ToTable("ExtensionConfiguration");
            modelBuilder.Entity<ExtensionConfiguration>().HasKey(ec => ec.Id);
            modelBuilder.Entity<ExtensionConfiguration>().Property(ec => ec.Id).HasColumnName("ConfigurationId");

            // PostTag
            modelBuilder.Entity<PostTag>().ToTable("BlogEntryTags");
            modelBuilder.Entity<PostTag>().HasKey(pt => pt.Id);
            modelBuilder.Entity<PostTag>().Property(pt => pt.Id).HasColumnName("BlogEntryTagId");
            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany()
                .HasForeignKey("TagId");
            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany()
                .HasForeignKey("BlogEntryId");

            // Role
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().Property(r => r.Id).HasColumnName("RoleId");

            // SiteInfo
            modelBuilder.Entity<SiteInfo>().ToTable("SiteInfo");
            modelBuilder.Entity<SiteInfo>().HasKey(s => s.SiteId);

            // Tag
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<Tag>().HasKey(t => t.Id);
            modelBuilder.Entity<Tag>()
                .HasOne(t => t.Blog)
                .WithMany()
                .HasForeignKey(t => t.BlogId);
            modelBuilder.Entity<Tag>().Ignore(t => t.BlogEntries);

            // AnotherBlogUser
            modelBuilder.Entity<AnotherBlogUser>().ToTable("Users");
            modelBuilder.Entity<AnotherBlogUser>().HasKey(u => u.Id);
            modelBuilder.Entity<AnotherBlogUser>().Ignore(u => u.Roles);

            // TagCount - Keyless entity for raw SQL queries
            modelBuilder.Entity<TagCount>().HasNoKey().ToView(null);
        }

        public DbSet<T> GetTable<T>() where T : class
        {
            return this.Set<T>();
        }

        public IEnumerable<T> CreateQuery<T>(string queryString) where T : class
        {
            return this.Set<T>().FromSqlRaw(queryString);
        }

        public IEnumerable<T> CreateQuery<T>(string queryString, IDictionary<string, object> queryParams) where T : class
        {
            IList<SqlParameter> sqlParameters = new List<SqlParameter>();

            foreach (string paramKey in queryParams.Keys)
            {
                sqlParameters.Add(new SqlParameter(paramKey, queryParams[paramKey]));
            }

            return this.Set<T>().FromSqlRaw(queryString, sqlParameters.ToArray());
        }

        public IEnumerable<T> ExecuteSQL<T>(string queryString) where T : class
        {
            return this.Set<T>().FromSqlRaw(queryString);
        }

        public IEnumerable<T> ExecuteSQL<T>(string queryString, IDictionary<string, object> queryParams) where T : class
        {
            IList<SqlParameter> sqlParameters = new List<SqlParameter>();

            foreach (string paramKey in queryParams.Keys)
            {
                sqlParameters.Add(new SqlParameter(paramKey, queryParams[paramKey]));
            }

            return this.Set<T>().FromSqlRaw(queryString, sqlParameters.ToArray());
        }
    }
}

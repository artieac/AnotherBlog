using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Entity.ModelConfiguration;
using System.Data.SqlClient;

using AlwaysMoveForward.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Entities
{
    public class AnotherBlogDataContextCF : DbContext
    {
        public AnotherBlogDataContextCF(String connectionString) : base(connectionString) { }
        public AnotherBlogDataContextCF(System.Data.Common.DbConnection dbConnection) : base(dbConnection, false){}

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogExtension> BlogExtensions { get; set; }
        public DbSet<BlogList> BlogLists { get; set; }
        public DbSet<BlogListItem> BlogListItems { get; set; }
        public DbSet<BlogUser> BlogUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<DbInfo> DbInfos { get; set; }
        public DbSet<ExtensionConfiguration> ExtensionConfiguration { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SiteInfo> SiteInfos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().ToTable("Blogs");
            modelBuilder.Entity<Blog>().HasKey(b => b.BlogId);

            modelBuilder.Entity<BlogPost>().ToTable("BlogEntries");
            modelBuilder.Entity<BlogPost>().HasKey(blogPost => blogPost.EntryId);
            modelBuilder.Entity<BlogPost>().Property(blogPost => blogPost.EntryText).HasColumnName("EntryText");
            modelBuilder.Entity<BlogPost>()
                .HasRequired<Blog>(blogPost => blogPost.Blog)
                .WithMany(blog => blog.Posts)
                .Map(blog => blog.MapKey("BlogId"));
            modelBuilder.Entity<BlogPost>()
                .HasRequired<User>(blogPost => blogPost.Author)
                .WithMany()
                .Map(user => user.MapKey("UserId"));
            modelBuilder.Entity<BlogPost>().Property(blogPost => blogPost.Title);
            modelBuilder.Entity<BlogPost>().Property(blogPost => blogPost.IsPublished);
            modelBuilder.Entity<BlogPost>().Property(blogPost => blogPost.DatePosted);
            modelBuilder.Entity<BlogPost>().Property(blogPost => blogPost.DateCreated);
            modelBuilder.Entity<BlogPost>().Property(blogPost => blogPost.TimesViewed);

            modelBuilder.Entity<BlogExtension>().ToTable("BlogExtensions");
            modelBuilder.Entity<BlogExtension>().HasKey(b => b.ExtensionId);

            modelBuilder.Entity<BlogList>().ToTable("BlogLists");
            modelBuilder.Entity<BlogList>().HasKey(blogList => blogList.Id);
            modelBuilder.Entity<BlogList>().Property(blogList => blogList.Name);
            modelBuilder.Entity<BlogList>().HasMany<BlogListItem>(blogList => blogList.Items);
            modelBuilder.Entity<BlogList>()
                .HasRequired<Blog>(blogList => blogList.Blog)
                .WithMany()
                .Map(blog => blog.MapKey("BlogId"));


            modelBuilder.Entity<BlogListItem>().ToTable("BlogListItems");
            modelBuilder.Entity<BlogListItem>().HasKey(blogListItem => blogListItem.Id);
            modelBuilder.Entity<BlogListItem>().Property(blogListItem => blogListItem.Name);
            modelBuilder.Entity<BlogListItem>().HasRequired<BlogList>(blogListItem => blogListItem.BlogList);
      
            modelBuilder.Entity<BlogUser>().ToTable("BlogUsers");
            modelBuilder.Entity<BlogUser>().HasKey(blogUser => blogUser.BlogUserId);
            modelBuilder.Entity<BlogUser>()
                .HasRequired<Blog>(blogUser => blogUser.Blog)
                .WithMany(blog => blog.Users)
                .Map(blog => blog.MapKey("BlogId"));
            modelBuilder.Entity<BlogUser>()
                .HasRequired<User>(blogUser => blogUser.User)
                .WithMany()
                .Map(user => user.MapKey("UserId"));
            modelBuilder.Entity<BlogUser>()
                .HasRequired<Role>(blogUser => blogUser.Role)
                .WithMany()
                .Map(role => role.MapKey("RoleId"));

            modelBuilder.Entity<Comment>().ToTable("EntryComments");
            modelBuilder.Entity<Comment>().HasKey(comment => comment.CommentId);
            modelBuilder.Entity<Comment>()
                .HasRequired<BlogPost>(comment => comment.Post)
                .WithMany(post => post.Comments)
                .Map(post => post.MapKey("EntryId"));
            modelBuilder.Entity<Comment>().Property(comment => comment.AuthorEmail);
            modelBuilder.Entity<Comment>().Property(comment => comment.AuthorName);
            modelBuilder.Entity<Comment>().Property(comment => comment.Text).HasColumnName("Comment");
            modelBuilder.Entity<Comment>().Property(comment => comment.Status);
            modelBuilder.Entity<Comment>().Property(comment => comment.Link);
            modelBuilder.Entity<Comment>().Property(comment => comment.DatePosted);
                       
            modelBuilder.Entity<DbInfo>().ToTable("DbInfo");
            modelBuilder.Entity<DbInfo>().HasKey(b => b.Version);
            
            modelBuilder.Entity<ExtensionConfiguration>().ToTable("ExtensionConfiguration");
            modelBuilder.Entity<ExtensionConfiguration>().HasKey(b => b.ConfigurationId);

            modelBuilder.Entity<PostTag>().ToTable("BlogEntryTags");
            modelBuilder.Entity<PostTag>().HasKey(bet => bet.PostTagId).Property(bet => bet.PostTagId).HasColumnName("BlogEntryTagId");
            modelBuilder.Entity<PostTag>()
                .HasRequired<Tag>(bet => bet.Tag)
                .WithMany()
                .Map(tag => tag.MapKey("TagId"));
            modelBuilder.Entity<PostTag>()
                .HasRequired<BlogPost>(bet => bet.Post)
                .WithMany()
                .Map(post => post.MapKey("BlogEntryId"));
            
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Role>().HasKey(b => b.RoleId);
            
            modelBuilder.Entity<SiteInfo>().ToTable("SiteInfo");
            modelBuilder.Entity<SiteInfo>().HasKey(b => b.SiteId);

            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<Tag>().HasKey(t => t.Id);
            modelBuilder.Entity<Tag>().Property(t => t.Name);
            modelBuilder.Entity<Tag>()
                .HasRequired<Blog>(tag => tag.Blog)
                .WithMany()
                .Map(blog => blog.MapKey("BlogId"));

            
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(b => b.UserId);
        }

        public DbSet<TableDTO> GetTable<TableDTO>() where TableDTO : class
        {
            DbSet<TableDTO> retVal = null;

            Type targetType = typeof(TableDTO);

            if (targetType == typeof(Blog))
            {
                retVal = this.Blogs as DbSet<TableDTO>;
            }
            else if (targetType == typeof(BlogPost))
            {
                retVal = this.BlogPosts as DbSet<TableDTO>;
            }
            else if (targetType == typeof(BlogExtension))
            {
                retVal = this.BlogExtensions as DbSet<TableDTO>;
            }
            else if (targetType == typeof(BlogList))
            {
                retVal = this.BlogLists as DbSet<TableDTO>;
            }
            else if (targetType == typeof(BlogListItem))
            {
                retVal = this.BlogListItems as DbSet<TableDTO>;
            }
            else if (targetType == typeof(BlogUser))
            {
                retVal = this.BlogUsers as DbSet<TableDTO>;
            }
            else if (targetType == typeof(Comment))
            {
                retVal = this.Comments as DbSet<TableDTO>;
            }
            else if (targetType == typeof(DbInfo))
            {
                retVal = this.DbInfos as DbSet<TableDTO>;
            }
            else if (targetType == typeof(ExtensionConfiguration))
            {
                retVal = this.ExtensionConfiguration as DbSet<TableDTO>;
            }
            else if (targetType == typeof(PostTag))
            {
                retVal = this.PostTags as DbSet<TableDTO>;
            }
            else if (targetType == typeof(Role))
            {
                retVal = this.Roles as DbSet<TableDTO>;
            }
            else if (targetType == typeof(SiteInfo))
            {
                retVal = this.SiteInfos as DbSet<TableDTO>;
            }
            else if (targetType == typeof(Tag))
            {
                retVal = this.Tags as DbSet<TableDTO>;
            }
            else if (targetType == typeof(User))
            {
                retVal = this.Users as DbSet<TableDTO>;
            }
            else if (targetType == typeof(BlogList))
            {
                retVal = this.BlogLists as DbSet<TableDTO>;
            }
            else if (targetType == typeof(BlogListItem))
            {
                retVal = this.BlogListItems as DbSet<TableDTO>;
            }

            return retVal;
        }

        public IEnumerable<DTOType> CreateQuery<DTOType>(String queryString) where DTOType : class 
        {
            return this.GetTable<DTOType>().SqlQuery(queryString);
        }

        public IEnumerable<DTOType> CreateQuery<DTOType>(String queryString, IDictionary<String, object> queryParams) where DTOType : class 
        {
            IList<SqlParameter> sqlParameters = new List<SqlParameter>();

            foreach (String paramKey in queryParams.Keys)
            {
                sqlParameters.Add(new SqlParameter(paramKey, queryParams[paramKey]));
            }

            return this.GetTable<DTOType>().SqlQuery(queryString, queryParams);
        }

        public IEnumerable<DestinationType> ExecuteSQL<DestinationType>(String queryString) where DestinationType : class
        {
            return this.Database.SqlQuery<DestinationType>(queryString);
        }

        public IEnumerable<DestinationType> ExecuteSQL<DestinationType>(String queryString, IDictionary<String, object> queryParams) where DestinationType : class
        {
            IList<SqlParameter> sqlParameters = new List<SqlParameter>();

            foreach(String paramKey in queryParams.Keys)
            {
                sqlParameters.Add(new SqlParameter(paramKey, queryParams[paramKey]));
            }

            return this.Database.SqlQuery<DestinationType>(queryString, sqlParameters.ToArray());
        }

        private String GenerateEFConnectionString()
        {
            string connectionString = new System.Data.EntityClient.EntityConnectionStringBuilder
            {
                Metadata = "res://*",
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(this.Database.Connection.ConnectionString).ConnectionString
            }.ConnectionString;

            return connectionString;
        }
    }
}

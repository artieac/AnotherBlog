﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Models.API;

namespace AlwaysMoveForward.AnotherBlog.Web.Controllers.API
{
    public class CommentController : BaseApiController
    {
        // GET api/<controller>
        [Route("api/Blog/{blogSubFolder}/Comments/{status}"), HttpGet()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public IList<GetCommentModel> Get(string blogSubFolder, string status)
        {
            IList<GetCommentModel> model = new List<GetCommentModel>();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog != null)
            {
                IList<BlogPost> posts = this.Services.BlogEntryService.GetAllByBlog(targetBlog, true);

                if (status == null || string.Compare(status, "All", true) == 0)
                {
                    foreach (BlogPost post in posts)
                    {
                        foreach (Comment comment in post.Comments)
                        {
                            GetCommentModel newItem = new GetCommentModel(comment);
                            newItem.BlogPostId = post.Id;
                            model.Add(newItem);
                        }
                    }
                }
                else
                {
                    CommentStatus targetStatus = (CommentStatus)Enum.Parse(typeof(CommentStatus), status);

                    foreach (BlogPost post in posts)
                    {
                        foreach (Comment comment in post.FilteredComments(targetStatus))
                        {
                            GetCommentModel newItem = new GetCommentModel(comment);
                            newItem.BlogPostId = post.Id;
                            model.Add(newItem);
                        }
                    }
                }
            }

            return model;
        }

        // GET api/<controller>
        [Route("api/Blog/{blogSubFolder}/BlogPost/{postId:int}/Comments"), HttpGet()]
        public IList<Comment> Get(string blogSubFolder, int postId)
        {
            IList<Comment> model = new List<Comment>();

            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);

            if (targetBlog != null)
            {
                BlogPost targetEntry = Services.BlogEntryService.GetById(targetBlog, postId);

                if (targetEntry != null)
                {
                    model = targetEntry.Comments.Where(comment => comment.Status == CommentStatus.Approved).ToList();
                }
            }

            return model;
        }

        // GET api/<controller>/5
        [Route("api/Blog/{blogSubFolder}/BlogPost/{postId:int}/Comment/{commentId:int}"), HttpGet()]
        public string Get(int commentId)
        {
            return "value";
        }

        // POST api/<controller>
        [Route("api/Blog/{blogSubFolder}/BlogPost/{postId:int}/Comment"), HttpPost()]
        public IList<Comment> Post(string blogSubFolder, int postId, [FromBody]CommentModel input)
        {
            IList<Comment> model = new List<Comment>();
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            BlogPost targetEntry = Services.BlogEntryService.GetById(targetBlog, postId);

            if (!string.IsNullOrEmpty(input.AuthorName) &&
                !string.IsNullOrEmpty(input.AuthorEmail) &&
                !string.IsNullOrEmpty(input.CommentText) &&
                targetEntry != null)
            {
                model = targetEntry.Comments.Where(comment => comment.Status == CommentStatus.Approved).ToList();

                using (this.Services.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        Comment newComment = targetEntry.AddComment(input.AuthorName, input.AuthorEmail, input.CommentText, input.CommentLink, this.CurrentPrincipal.CurrentUser);
                        this.Services.BlogEntryService.Save(targetEntry);
                        model.Add(newComment);
                        this.Services.UnitOfWork.EndTransaction(true);
                    }
                    catch (Exception e)
                    {
                        LogManager.GetLogger().Error(e);
                        this.Services.UnitOfWork.EndTransaction(false);
                    }
                }
            }

            return model;
        }

        // PUT api/<controller>/5
        [Route("api/Blog/{blogSubFolder}/BlogPost/{postId:int}/Comment/{commentId:int}"), HttpPut()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public IList<Comment> Put(string blogSubFolder, int postId, int commentId, [FromBody]CommentModel input)
        {
            IList<Comment> model = new List<Comment>();
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            BlogPost targetEntry = Services.BlogEntryService.GetById(targetBlog, postId);

            if (!string.IsNullOrEmpty(input.AuthorName) &&
                !string.IsNullOrEmpty(input.AuthorEmail) &&
                !string.IsNullOrEmpty(input.CommentText) &&
                targetEntry != null)
            {
            }

            return model;
        }

        [Route("api/Blog/{blogSubFolder}/BlogPost/{postId:int}/Comment/{commentId:int}/{newState}"), HttpPut()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public Comment Put(string blogSubFolder, int postId, int commentId, string newState)
        {
            Comment model = null;
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            BlogPost blogPost = Services.BlogEntryService.GetById(targetBlog, postId);

            if (blogPost != null)
            {
                if (blogPost != null)
                {
                    CommentStatus parsedState = (CommentStatus)Enum.Parse(typeof(CommentStatus), newState);
                    model = blogPost.UpdateCommentStatus(commentId, parsedState);
                    this.Services.BlogEntryService.Save(blogPost);
                }
            }

            return model;
        }

        // PUT api/<controller>/5
        [Route("api/Blog/{blogSubFolder}/Comments/{newState}"), HttpPut()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public void Put(string blogSubFolder, string newState, [FromBody] IDictionary<int, int> input)
        {
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            CommentStatus parsedState = (CommentStatus)Enum.Parse(typeof(CommentStatus), newState);

            IDictionary<int, BlogPost> blogPosts = new Dictionary<int, BlogPost>();

            foreach (int key in input.Keys)
            {
                if(!blogPosts.ContainsKey(input[key]))
                {
                    blogPosts[input[key]] = Services.BlogEntryService.GetById(targetBlog, input[key]);
                }

                if (blogPosts[input[key]] != null)
                {
                    blogPosts[input[key]].UpdateCommentStatus(key, parsedState);
                }
            }

            foreach(int blogPostId in blogPosts.Keys)
            {
                this.Services.BlogEntryService.Save(blogPosts[blogPostId]);
            }
        }

        // DELETE api/<controller>/5
        [Route("api/Blog/{blogSubFolder}/BlogPost/{postId:int}/Comment/{commentId:int}"), HttpDelete()]
        [WebAPIAuthorization(RequiredRoles = RoleType.Names.SiteAdministrator + "," + RoleType.Names.Administrator + "," + RoleType.Names.Blogger, IsBlogSpecific = true)]
        public void Delete(string blogSubFolder, int postId, int commentId)
        {
            IList<Comment> model = new List<Comment>();
            Blog targetBlog = this.Services.BlogService.GetBySubFolder(blogSubFolder);
            BlogPost blogPost = Services.BlogEntryService.GetById(targetBlog, postId);

            if (blogPost != null)
            {
                if (blogPost != null)
                {
                    blogPost.UpdateCommentStatus(commentId, CommentStatus.Deleted);
                    this.Services.BlogEntryService.Save(blogPost);
                }
            }
        }
    }
}
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

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class UserRepository : EntityFrameworkRepository<AnotherBlogUser, long>, IUserRepository
    {
        internal UserRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override string IdPropertyName
        {
            get { return "Id"; }
        }

        private void PopulateRoles(AnotherBlogUser user)
        {
            if (user == null)
            {
                return;
            }

            user.Roles = new Dictionary<long, RoleType.Id>();

            var blogUserRoles = from bu in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUsers
                                where bu.User.Id == user.Id
                                select new { BlogId = bu.Blog.Id, RoleId = bu.Role.Id };

            foreach (var blogUserRole in blogUserRoles)
            {
                if (Enum.IsDefined(typeof(RoleType.Id), blogUserRole.RoleId))
                {
                    user.Roles[blogUserRole.BlogId] = (RoleType.Id)blogUserRole.RoleId;
                }
            }
        }

        private void PopulateRoles(IEnumerable<AnotherBlogUser> users)
        {
            if (users == null || !users.Any())
            {
                return;
            }

            var userIds = users.Select(u => u.Id).ToList();

            var allBlogUserRoles = (from bu in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUsers
                                    where userIds.Contains(bu.User.Id)
                                    select new { UserId = bu.User.Id, BlogId = bu.Blog.Id, RoleId = bu.Role.Id })
                                   .ToList();

            foreach (var user in users)
            {
                user.Roles = new Dictionary<long, RoleType.Id>();

                var userRoles = allBlogUserRoles.Where(r => r.UserId == user.Id);
                foreach (var blogUserRole in userRoles)
                {
                    if (Enum.IsDefined(typeof(RoleType.Id), blogUserRole.RoleId))
                    {
                        user.Roles[blogUserRole.BlogId] = (RoleType.Id)blogUserRole.RoleId;
                    }
                }
            }
        }

        public override AnotherBlogUser GetById(long id)
        {
            var user = base.GetById(id);
            PopulateRoles(user);
            return user;
        }

        public override IList<AnotherBlogUser> GetAll()
        {
            var users = base.GetAll();
            PopulateRoles(users);
            return users;
        }

        public IList<AnotherBlogUser> GetBlogWriters(long blogId)
        {
            var users = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Users
                         join userBlog in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUsers on foundItem.Id equals userBlog.User.Id
                         join userRoles in ((UnitOfWork)this.UnitOfWork).DataContext.Roles on userBlog.Role.Id equals userRoles.Id
                         where (userRoles.Name == "Administrator" || userRoles.Name == "Blogger") &&
                             userBlog.Blog.Id == blogId &&
                             userBlog.Role.Id == userRoles.Id
                         select foundItem).ToList();

            PopulateRoles(users);
            return users;
        }

        public AnotherBlogUser GetByOAuthServiceUserId(long userId)
        {
            var user = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Users
                        where foundItem.Id == userId
                        select foundItem).SingleOrDefault();

            PopulateRoles(user);
            return user;
        }

        public AnotherBlogUser GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var user = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Users
                        where foundItem.Email == email
                        select foundItem).SingleOrDefault();

            PopulateRoles(user);
            return user;
        }

        public AnotherBlogUser GetByExternalId(string externalId)
        {
            if (string.IsNullOrEmpty(externalId))
            {
                return null;
            }

            var user = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Users
                        where foundItem.OAuthServiceUserId == externalId
                        select foundItem).SingleOrDefault();

            PopulateRoles(user);
            return user;
        }
    }
}

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

        public IList<AnotherBlogUser> GetBlogWriters(int blogId)
        {
            // Query Users table and map to AnotherBlogUser
            var users = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Users
                        join userBlog in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUsers on foundItem.Id equals userBlog.User.Id
                        join userRoles in ((UnitOfWork)this.UnitOfWork).DataContext.Roles on userBlog.Role.Id equals userRoles.Id
                        where (userRoles.Name == "Administrator" || userRoles.Name == "Blogger") &&
                            userBlog.Blog.Id == blogId &&
                            userBlog.Role.Id == userRoles.Id
                        select foundItem;

            // Map User to AnotherBlogUser
            return users.ToList().Select(u => MapUserToAnotherBlogUser(u)).ToList();
        }

        public AnotherBlogUser GetByOAuthServiceUserId(long userId)
        {
            var user = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.Users
                        where foundItem.Id == userId
                        select foundItem).SingleOrDefault();

            return user != null ? MapUserToAnotherBlogUser(user) : null;
        }

        private AnotherBlogUser MapUserToAnotherBlogUser(User user)
        {
            if (user == null) return null;

            return new AnotherBlogUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}

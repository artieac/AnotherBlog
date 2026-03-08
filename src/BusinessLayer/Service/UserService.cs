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
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AlwaysMoveForward.Common.DomainModel;
using AlwaysMoveForward.Common.Configuration;
using AlwaysMoveForward.Common.Business;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.Common.Utilities;
using AlwaysMoveForward.OAuth.Client;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.Factories;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Service
{
    public class UserService : AnotherBlogService
    {

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
            : base(unitOfWork)
        {
            this.UserRepository = userRepository;
        }

        protected IUserRepository UserRepository { get; private set; }

        public AnotherBlogUser Save(long userId, bool isSiteAdmin, bool isApprovedCommenter, string userAbout)
        {
            AnotherBlogUser userToSave = null;

            if (userId != 0)
            {
                userToSave = this.UserRepository.GetById(userId);
            }

            if (userToSave == null)
            {
                userToSave = new AnotherBlogUser();
            }

            userToSave.IsSiteAdministrator = isSiteAdmin;
            userToSave.ApprovedCommenter = isApprovedCommenter;

            if (userAbout != null)
            {
                userToSave.About = Utils.StripJavascript(userAbout);
            }
            else
            {
                userToSave.About = string.Empty;
            }

            return this.UserRepository.Save(userToSave);
        }

        public AnotherBlogUser Save(AnotherBlogUser user)
        {
            if(user != null)
            {
                user = this.UserRepository.Save(user);
            }

            return user;
        }
        public void Delete(long userId)
        {
            AnotherBlogUser targetUser = this.UserRepository.GetById(userId);

            using (this.UnitOfWork.BeginTransaction())
            {
                if (targetUser != null)
                {
                    this.UserRepository.Delete(targetUser);
                    this.UnitOfWork.EndTransaction(true);
                }
            }
        }

        public IList<AnotherBlogUser> GetAll()
        {
            return this.UserRepository.GetAll();
        }

        public AnotherBlogUser GetById(long userId)
        {
            return this.UserRepository.GetById(userId);
        }      

        public IList<AnotherBlogUser> GetBlogWriters(Blog targetBlog)
        {
            return this.UserRepository.GetBlogWriters(targetBlog.Id);
        }

        public AnotherBlogUser AddBlogRole(long userId, long blogId, RoleType.Id roleId)
        {
            AnotherBlogUser retVal = null;

            using (this.UnitOfWork.BeginTransaction())
            {
                try
                {
                    retVal = this.UserRepository.GetById(userId);

                    if (retVal != null)
                    {
                        retVal.AddRole(blogId, roleId);
                    }

                    retVal = this.UserRepository.Save(retVal);
                    this.UnitOfWork.EndTransaction(true);
                }
                catch (Exception e)
                {
                    LogManager.GetLogger().Error(e);
                    this.UnitOfWork.EndTransaction(false);
                }
            }

            return retVal;
        }

        public AnotherBlogUser RemoveBlogRole(long userId, long blogId)
        {
            AnotherBlogUser retVal = null;

            using (this.UnitOfWork.BeginTransaction())
            {
                try
                {
                    retVal = this.UserRepository.GetById(userId);

                    if (retVal != null)
                    {
                        retVal.RemoveRole(blogId);
                    }

                    retVal = this.UserRepository.Save(retVal);
                    this.UnitOfWork.EndTransaction(true);
                }
                catch (Exception e)
                {
                    LogManager.GetLogger().Error(e);
                    this.UnitOfWork.EndTransaction(false);
                }
            }

            return retVal;
        }

        public AnotherBlogUser GetFromAMFUser(IOAuthToken accessToken)
        {
            AnotherBlogUser retVal = null;

            AlwaysMoveForward.Common.DomainModel.User amfUser = null;

            if (amfUser != null)
            {
                retVal = this.UserRepository.GetByOAuthServiceUserId(amfUser.Id);

                if (retVal == null)
                {
                    retVal = UserFactory.Create(amfUser);
                }

                retVal.AccessToken = accessToken.Token;
                retVal.AccessTokenSecret = accessToken.Secret;
                retVal = this.UserRepository.Save(retVal);
            }

            return retVal;
        }

        public AnotherBlogUser GetByEmail(string email)
        {
            return this.UserRepository.GetByEmail(email);
        }

        public AnotherBlogUser GetByExternalId(string externalId)
        {
            return this.UserRepository.GetByExternalId(externalId);
        }

        public AnotherBlogUser CreateFromAuth0(string email, string name, string auth0UserId)
        {
            AnotherBlogUser newUser = new AnotherBlogUser();
            newUser.Email = email ?? string.Empty;
            newUser.OAuthServiceUserId = auth0UserId ?? string.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                var nameParts = name.Split(' ', 2);
                newUser.FirstName = nameParts[0];
                newUser.LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
            }
            else
            {
                newUser.FirstName = string.Empty;
                newUser.LastName = string.Empty;
            }

            newUser.IsSiteAdministrator = false;
            newUser.ApprovedCommenter = false;
            newUser.About = string.Empty;

            return this.UserRepository.Save(newUser);
        }

        public AnotherBlogUser UpdateExternalId(long userId, string externalId)
        {
            AnotherBlogUser user = this.UserRepository.GetById(userId);

            if (user != null)
            {
                user.OAuthServiceUserId = externalId;
                user = this.UserRepository.Save(user);
            }

            return user;
        }
    }
}

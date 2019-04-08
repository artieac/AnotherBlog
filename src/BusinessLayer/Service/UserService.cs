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
using PucksAndProgramming.Common.DomainModel;
using PucksAndProgramming.Common.Configuration;
using PucksAndProgramming.Common.Business;
using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.Common.DataLayer.Repositories;
using PucksAndProgramming.Common.Utilities;
using PucksAndProgramming.OAuth.Client;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.AnotherBlog.Common.Factories;

namespace PucksAndProgramming.AnotherBlog.BusinessLayer.Service
{
    public class UserService
    {
 
        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IOAuthRepository oauthRepository) : base()
        {
            this.UnitOfWork = unitOfWork;
            this.UserRepository = userRepository;
            this.OAuthRepository = oauthRepository;
        }

        protected IUnitOfWork UnitOfWork { get; private set; }

        protected IUserRepository UserRepository { get; private set; }

        protected IOAuthRepository OAuthRepository { get; private set; }

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

        public AnotherBlogUser GetByOAuthServiceUserId(string userId)
        {
            return this.UserRepository.GetByOAuthServiceUserId(userId);
        }

        public AnotherBlogUser GetByOAuthToken(IOAuthToken accessToken)
        {
            AnotherBlogUser retVal = null;

            PucksAndProgramming.Common.DomainModel.User amfUser = this.GetFromOAuthService(accessToken);

            if (amfUser != null)
            {
                retVal = this.UserRepository.GetByOAuthServiceUserId(amfUser.Id.ToString());

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

        public User GetFromOAuthService(IOAuthToken oauthToken)
        {
            return this.OAuthRepository.GetUserInfo(oauthToken);
        }

        public AnotherBlogUser GetByEmail(string email)
        {
            return this.UserRepository.GetByEmail(email);
        }
    }
}

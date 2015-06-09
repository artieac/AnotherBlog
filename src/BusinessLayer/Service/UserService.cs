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

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Service
{
    public class UserService
    {
        private const string GuestUserName = "guest";
        private static AnotherBlogUser guestUser = null;

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
        public void Delete(int userId)
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

        public AnotherBlogUser GetDefaultUser()
        {
            if (UserService.guestUser == null)
            {
                UserService.guestUser = new AnotherBlogUser();
                guestUser.ApprovedCommenter = false;
                guestUser.IsSiteAdministrator = false;
                guestUser.Roles = new Dictionary<int, RoleType.Id>();
            }

            return UserService.guestUser;
        }

        public IList<AnotherBlogUser> GetAll()
        {
            return this.UserRepository.GetAll();
        }

        public AnotherBlogUser GetById(int userId)
        {
            return this.UserRepository.GetById(userId);
        }      

        public IList<AnotherBlogUser> GetBlogWriters(Blog targetBlog)
        {
            return this.UserRepository.GetBlogWriters(targetBlog.Id);
        }

        public AnotherBlogUser AddBlogRole(int userId, int blogId, RoleType.Id roleId)
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

        public AnotherBlogUser RemoveBlogRole(int userId, int blogId)
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

            AlwaysMoveForward.Common.DomainModel.User amfUser = this.GetAMFUserInfo(accessToken);

            if (amfUser != null)
            {
                retVal = this.UserRepository.GetByOAuthServiceUserId(amfUser.Id);

                if (retVal == null)
                {
                    retVal = new AnotherBlogUser();
                    retVal.OAuthServiceUserId = amfUser.Id;
                    retVal.FirstName = amfUser.FirstName;
                    retVal.LastName = amfUser.LastName;
                    retVal.IsSiteAdministrator = false;
                    retVal.ApprovedCommenter = false;
                }

                retVal.AccessToken = accessToken.Token;
                retVal.AccessTokenSecret = accessToken.Secret;
                retVal = this.UserRepository.Save(retVal);
            }

            return retVal;
        }

        public User GetAMFUserInfo(IOAuthToken oauthToken)
        {
            return this.OAuthRepository.GetUserInfo(oauthToken);
        }
    }
}

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
using System.Text;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.Map;
using AlwaysMoveForward.Common.DataLayer.Entities;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Entities;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;
using AlwaysMoveForward.Common.Utilities;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    /// <summary>
    /// This class contains all the code to extract User data from the repository using LINQ
    /// </summary>
    /// <param name="dataContext"></param>
    public class UserRepository : LINQRepository<User, UserDTO>, IUserRepository
    {
        internal UserRepository(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager)
        {

        }

        public override string IdPropertyName
        {
            get { return "UserId"; }
        }

        protected override UserDTO GetDTOByDomain(User targetItem)
        {
            return this.GetDtoById(targetItem.UserId);
        }
        /// <summary>
        /// Get a specific by their user name.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetByUserName(string userName)
        {
            return this.GetByProperty("UserName", userName);
        }
        /// <summary>
        /// This method is used by the login.  If no match is found then something doesn't jibe in the login attempt.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetByUserNameAndPassword(string userName, string password)
        {
            UserDTO retVal = null;

            try
            {
                retVal = (from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.UserDTOs where foundItem.UserName == userName && foundItem.Password == password select foundItem).Single();
            }
            catch (Exception e)
            {
                LogManager.GetLogger().Warn(e.Message);
            }

            return this.Map(retVal);

        }
        /// <summary>
        /// Get a specific user by email
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public User GetByEmail(string userEmail)
        {
            return this.GetByProperty("Email", userEmail);
        }
        /// <summary>
        /// Get all users that have the Administrator or Blogger role for the specific blog.
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public IList<User> GetBlogWriters(int blogId)
        {
            IQueryable<UserDTO> dtoList = from foundItem in ((UnitOfWork)this.UnitOfWork).DataContext.UserDTOs
                                        join userBlog in ((UnitOfWork)this.UnitOfWork).DataContext.BlogUserDTOs on foundItem.UserId equals userBlog.UserDTO.UserId
                                        join userRoles in ((UnitOfWork)this.UnitOfWork).DataContext.RoleDTOs on userBlog.RoleDTO.RoleId equals userRoles.RoleId
                                        where (userRoles.Name == "Administrator" || userRoles.Name == "Blogger") &&
                                          userBlog.BlogId == blogId && 
                                          userBlog.RoleDTO.RoleId == userRoles.RoleId
                                        select foundItem;
            return this.Map(dtoList.ToList());
        }

        public override User Save(User itemToSave)
        {
            UserDTO targetItem = this.GetDTOByDomain(itemToSave);

            if (targetItem == null)
            {
                targetItem = this.Map(itemToSave);
                ((UnitOfWork)this.UnitOfWork).DataContext.GetTable<UserDTO>().InsertOnSubmit(targetItem);
            }
            else
            {
                targetItem.About = itemToSave.About;
                targetItem.ApprovedCommenter = itemToSave.ApprovedCommenter;
                targetItem.DisplayName = itemToSave.DisplayName;
                targetItem.Email = itemToSave.Email;
                targetItem.IsActive = itemToSave.IsActive;
                targetItem.IsSiteAdministrator = itemToSave.IsActive;
                targetItem.Password = itemToSave.Password;
            }

            this.UnitOfWork.Flush();

            return this.Map(targetItem);
        }
    }
}

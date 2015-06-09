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
using System.IO;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.Common.Business;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Service
{
    public class UploadedFileManager : AnotherBlogService
    {
        public UploadedFileManager(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public string UploadedFileRoot(Blog targetBlog)
        {
            return "Content/UploadedFiles/" + targetBlog.SubFolder.ToString(); 
        }

        public string GeneratePath(Blog targetBlog)
        {
            string retVal = AppDomain.CurrentDomain.BaseDirectory;

            DateTime pathGenerator = DateTime.Now;
            retVal += this.UploadedFileRoot(targetBlog) + "/" + pathGenerator.Year + "/" + pathGenerator.Month;

            return retVal;
        }

        public List<string> GetRecentUploadedFiles_LocalPaths(Blog targetBlog)
        {
            List<string> retVal = new List<string>();

            DateTime currentDate = DateTime.Now;
            string currentMonthPath = AppDomain.CurrentDomain.BaseDirectory + this.UploadedFileRoot(targetBlog) + "/" + currentDate.Year + "/" + currentDate.Month;

            if (Directory.Exists(currentMonthPath))
            {
                string[] currentMonthFiles = Directory.GetFiles(currentMonthPath);
                retVal.AddRange(new List<string>(currentMonthFiles));
            }

            DateTime lastMonth = currentDate.AddMonths(-1);
            string lastMonthPath = AppDomain.CurrentDomain.BaseDirectory + this.UploadedFileRoot(targetBlog) + "/" + lastMonth.Year + "/" + lastMonth.Month;
            
            if (Directory.Exists(lastMonthPath))
            {
                string[] lastMonthFiles = Directory.GetFiles(lastMonthPath);
                retVal.AddRange(new List<string>(lastMonthFiles));
            }

            return retVal;
        }

        public List<string> GetRecentUploadedFiles_RelativePaths(Blog targetBlog)
        {
            List<string> retVal = new List<string>();

            DateTime currentDate = DateTime.Now;
            string currentMonthPath = "/" + this.UploadedFileRoot(targetBlog) + "/" + currentDate.Year + "/" + currentDate.Month;

            if (Directory.Exists(currentMonthPath))
            {
                string[] currentMonthFiles = Directory.GetFiles(currentMonthPath);
                retVal.AddRange(new List<string>(currentMonthFiles));
            }

            DateTime lastMonth = currentDate.AddMonths(-1);
            string lastMonthPath = "/" + this.UploadedFileRoot(targetBlog) + "/" + lastMonth.Year + "/" + lastMonth.Month;

            if (Directory.Exists(lastMonthPath))
            {
                string[] lastMonthFiles = Directory.GetFiles(lastMonthPath);
                retVal.AddRange(new List<string>(lastMonthFiles));
            }

            return retVal;
        }

    }
}

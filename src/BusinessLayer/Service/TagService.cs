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
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.Business;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;

namespace AlwaysMoveForward.AnotherBlog.BusinessLayer.Service
{
    public class TagService : AnotherBlogService
    {
        public TagService(IUnitOfWork unitOfWork, ITagRepository tagRepository) : base(unitOfWork) 
        {
            this.TagRepository = tagRepository;
        }

        protected ITagRepository TagRepository { get; private set; }

        public Tag Create()
        {
            Tag retVal = new Tag();
            retVal.Id = -1;
            return retVal;
        }

        public IList<Tag> GetAll(Blog targetBlog)
        {
            return this.TagRepository.GetAll(targetBlog.Id);
        }

        public IList GetAllWithCount(Blog targetBlog)
        {
            return this.TagRepository.GetAllWithCount(targetBlog.Id);
        }
    }
}

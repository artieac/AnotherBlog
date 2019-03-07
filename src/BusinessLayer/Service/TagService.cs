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

using PucksAndProgramming.Common.DataLayer;
using PucksAndProgramming.Common.Business;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;
using PucksAndProgramming.Common.DataLayer.Repositories;
using PucksAndProgramming.AnotherBlog.Common.DataLayer.Repositories;

namespace PucksAndProgramming.AnotherBlog.BusinessLayer.Service
{
    public class TagService : AnotherBlogService
    {
        public TagService(IUnitOfWork unitOfWork, ITagRepository tagRepository) : base(unitOfWork) 
        {
            this.TagRepository = tagRepository;
        }

        protected ITagRepository TagRepository { get; private set; }

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

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
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Criterion;
using AlwaysMoveForward.Common.DomainModel.Poll;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.Common.DataLayer.NHibernate;
using AlwaysMoveForward.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.DataLayer.DTO;
using AlwaysMoveForward.AnotherBlog.DataLayer.DataMapper;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.Repositories
{
    public class PollRepository : NHibernateRepository<PollQuestion, PollQuestionDTO, int>, IPollRepository
    {
        public PollRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override PollQuestionDTO GetDTOById(PollQuestion domainInstance)
        {
            return this.GetDTOById(domainInstance.Id);
        }

        protected override PollQuestionDTO GetDTOById(int idSource)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<PollQuestionDTO>();
            criteria.Add(Expression.Eq("Id", idSource));
            return criteria.UniqueResult<PollQuestionDTO>();
        }

        protected override DataMapBase<PollQuestion, PollQuestionDTO> GetDataMapper()
        {
            return new PollQuestionDataMap(); 
        }

        public IList<PollQuestion> GetAllByActiveFlag(bool isActive)
        {
            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<PollQuestionDTO>();
            criteria.Add(Expression.Eq("IsActive", isActive));
            return this.GetDataMapper().Map(criteria.List<PollQuestionDTO>());
        }

        public PollQuestion GetByPollOptionId(int pollOptionId)
        {
            PollQuestion retVal = null;

            ICriteria criteria = this.UnitOfWork.CurrentSession.CreateCriteria<PollQuestionDTO>();
            criteria.Add(Expression.Eq("Id", pollOptionId));
            PollOptionDTO foundOption = criteria.UniqueResult<PollOptionDTO>();

            if (foundOption != null)
            {
                retVal = this.GetDataMapper().Map(foundOption.Question);
            }

            return retVal;
        }
    }
}

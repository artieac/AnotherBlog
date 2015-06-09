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
using NUnit.Framework;
using AlwaysMoveForward.Common.DomainModel.Poll;
using AlwaysMoveForward.AnotherBlog.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.IntegrationTests.Repository
{
    [TestFixture]
    public class PollRepositoryTest : RepositoryTestBase
    {
        [Test]
        public void PollRepository_GetAllTest()
        {
            IList<PollQuestion> foundQuestions = this.RepositoryManager.PollRepository.GetAll();

            Assert.IsNotNull(foundQuestions);
            Assert.IsTrue(foundQuestions.Count > 0);
        }

        [Test]
        public void PollRepository_SaveTest()
        {
            PollQuestion newQuestion = new PollQuestion();
            newQuestion.Title = "TestTitle";
            newQuestion.QuestionText = "TestQuestion:" + Guid.NewGuid().ToString();
            newQuestion.Options = new List<PollOption>();

            PollOption newPoll = new PollOption();
            newPoll.OptionText = "TestPoll:" + Guid.NewGuid().ToString();
            newPoll.VoterAddresses = new List<VoterAddress>();

            VoterAddress newVoterAddress = new VoterAddress(System.Net.IPAddress.Loopback);
            newPoll.VoterAddresses.Add(newVoterAddress);
            newQuestion.Options.Add(newPoll);

            PollQuestion savedQuestion = this.RepositoryManager.PollRepository.Save(newQuestion);

            Assert.IsNotNull(savedQuestion);
        }

        [Test]
        public void PollRepository_UpdateTest()
        {
            PollQuestion targetQuestion = this.RepositoryManager.PollRepository.GetById(1);            
            PollOption newPoll = new PollOption();
            newPoll.OptionText = "TestPoll:" + Guid.NewGuid().ToString();
            newPoll.VoterAddresses = new List<VoterAddress>();

            VoterAddress newVoterAddress = new VoterAddress(System.Net.IPAddress.Loopback);
            newPoll.VoterAddresses.Add(newVoterAddress);
            targetQuestion.Options.Add(newPoll);

            PollQuestion savedQuestion = this.RepositoryManager.PollRepository.Save(targetQuestion);

            Assert.IsNotNull(savedQuestion);
        }
    }
}

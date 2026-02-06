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
using NUnit.Framework;
using Moq;
using AlwaysMoveForward.Common.DataLayer;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Repositories;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Services
{
    [TestFixture]
    public class SiteInfoServiceUnitTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<ISiteInfoRepository> mockSiteInfoRepository;
        private SiteInfoService service;
        private SiteInfo testSiteInfo;

        [SetUp]
        public void Setup()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockSiteInfoRepository = new Mock<ISiteInfoRepository>();

            service = new SiteInfoService(mockUnitOfWork.Object, mockSiteInfoRepository.Object);

            testSiteInfo = new SiteInfo
            {
                SiteId = 1,
                Name = "Test Site",
                About = "About the site",
                ContactEmail = "contact@test.com",
                DefaultTheme = "default",
                SiteAnalyticsId = "UA-12345",
                DefaultAuthor = "Test Author",
                DefaultKeywords = "test, keywords"
            };
        }

        [Test]
        public void GetSiteInfo_ReturnsSiteInfo()
        {
            mockSiteInfoRepository.Setup(x => x.GetSiteInfo()).Returns(testSiteInfo);

            var result = service.GetSiteInfo();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test Site"));
            mockSiteInfoRepository.Verify(x => x.GetSiteInfo(), Times.Once);
        }

        [Test]
        public void GetSiteInfo_WithNoSiteInfo_ReturnsNull()
        {
            mockSiteInfoRepository.Setup(x => x.GetSiteInfo()).Returns((SiteInfo)null);

            var result = service.GetSiteInfo();

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Save_WithExistingSiteInfo_UpdatesSiteInfo()
        {
            mockSiteInfoRepository.Setup(x => x.GetSiteInfo()).Returns(testSiteInfo);
            mockSiteInfoRepository.Setup(x => x.Save(It.IsAny<SiteInfo>())).Returns((SiteInfo s) => s);

            var result = service.Save("Updated Site", "Updated About", "new@test.com", "newtheme", "UA-99999", "New Author", "new, keywords");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Updated Site"));
            Assert.That(result.About, Is.EqualTo("Updated About"));
            Assert.That(result.ContactEmail, Is.EqualTo("new@test.com"));
            Assert.That(result.DefaultTheme, Is.EqualTo("newtheme"));
            Assert.That(result.SiteAnalyticsId, Is.EqualTo("UA-99999"));
            Assert.That(result.DefaultAuthor, Is.EqualTo("New Author"));
            Assert.That(result.DefaultKeywords, Is.EqualTo("new, keywords"));
            mockSiteInfoRepository.Verify(x => x.Save(It.IsAny<SiteInfo>()), Times.Once);
        }

        [Test]
        public void Save_WithNoExistingSiteInfo_CreatesSiteInfo()
        {
            mockSiteInfoRepository.Setup(x => x.GetSiteInfo()).Returns((SiteInfo)null);
            mockSiteInfoRepository.Setup(x => x.Save(It.IsAny<SiteInfo>())).Returns((SiteInfo s) => s);

            var result = service.Save("New Site", "New About", "new@test.com", "default", "UA-12345", "Author", "keywords");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("New Site"));
            mockSiteInfoRepository.Verify(x => x.Save(It.IsAny<SiteInfo>()), Times.Once);
        }

        [Test]
        public void Save_PreservesAllFields()
        {
            mockSiteInfoRepository.Setup(x => x.GetSiteInfo()).Returns((SiteInfo)null);
            SiteInfo savedSiteInfo = null;
            mockSiteInfoRepository.Setup(x => x.Save(It.IsAny<SiteInfo>()))
                .Callback<SiteInfo>(s => savedSiteInfo = s)
                .Returns((SiteInfo s) => s);

            service.Save("Site", "About", "email@test.com", "theme", "analytics", "author", "keywords");

            Assert.That(savedSiteInfo.Name, Is.EqualTo("Site"));
            Assert.That(savedSiteInfo.About, Is.EqualTo("About"));
            Assert.That(savedSiteInfo.ContactEmail, Is.EqualTo("email@test.com"));
            Assert.That(savedSiteInfo.DefaultTheme, Is.EqualTo("theme"));
            Assert.That(savedSiteInfo.SiteAnalyticsId, Is.EqualTo("analytics"));
            Assert.That(savedSiteInfo.DefaultAuthor, Is.EqualTo("author"));
            Assert.That(savedSiteInfo.DefaultKeywords, Is.EqualTo("keywords"));
        }
    }
}

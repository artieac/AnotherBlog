using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.Web.Models.API
{
    public class TagCountModel
    {
        public TagCountModel(string blogSubFolder, TagCount tagCount)
        {
            this.TagName = tagCount.TagName;
            this.TagCount = tagCount.Count;
            this.TagUrl = AlwaysMoveForward.AnotherBlog.Web.Code.Utilities.Utils.GenerateTagLink(blogSubFolder, this.TagName);
        }

        public string TagName { get; private set; }
        public int TagCount {get; private set;}
        public string TagUrl { get; private set; }
    }
}
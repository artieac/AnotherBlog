using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PucksAndProgramming.AnotherBlog.Common.DomainModel;

namespace PucksAndProgramming.AnotherBlog.Web.Models.API
{
    public class TagCountModel
    {
        public TagCountModel(string blogSubFolder, TagCount tagCount)
        {
            this.TagName = tagCount.TagName;
            this.TagCount = tagCount.Count;
            this.TagUrl = PucksAndProgramming.AnotherBlog.Web.Code.Utilities.Utils.GenerateTagLink(blogSubFolder, this.TagName);
        }

        public string TagName { get; private set; }
        public int TagCount {get; private set;}
        public string TagUrl { get; private set; }
    }
}
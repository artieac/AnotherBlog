using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Extensions
{
    public class TabElements
    {
        public TabElements()
        {
            this.TabItems = new List<TabItem>();
        }

        public class TabItem
        {
            public string Title { get; set; }
            public string TargetUrl { get; set; }
            public string Image { get; set; }
        }

        public int SelectedTab { get; set; }
        public List<TabItem> TabItems { get; set; }

        public void SetSelectedTab(Uri currentUrl)
        {
            for (int i = 0; i < this.TabItems.Count; i++)
            {
                if (currentUrl.ToString().Contains(this.TabItems[i].TargetUrl))
                {
                    this.SelectedTab = i;
                    break;
                }
            }
        }

        public void Add(string title, string targetUrl, string image)
        {
            TabItem newTab = new TabItem();
            newTab.Title = title;
            newTab.TargetUrl = targetUrl;
            newTab.Image = image;
            this.TabItems.Add(newTab);
        }
    }
}
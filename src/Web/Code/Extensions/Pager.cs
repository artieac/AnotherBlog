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
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Extensions
{
    public class Pager
    {
        private readonly int pageSize;
        private readonly int currentPage;
        private readonly int totalItemCount;
        private readonly RouteValueDictionary linkWithoutPageValuesDictionary;
        private ViewContext viewContext;

        public Pager(ViewContext viewContext, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary)
        {
            this.viewContext = viewContext;
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.totalItemCount = totalItemCount;
            this.linkWithoutPageValuesDictionary = valuesDictionary;
        }

        public string RenderHtml()
        {
            int pageCount = (int)Math.Ceiling(this.totalItemCount / (double)this.pageSize);
            int numberOfPagesToDisplay = 10;

            var sb = new StringBuilder();

            // Previous
            if (this.currentPage > 1)
            {
                sb.Append(this.GeneratePageLink("&lt;", this.currentPage - 1));
            }
            else
            {
                sb.Append("<span class=\"disabled\">&lt;</span>");
            }

            int start = 1;
            int end = pageCount;

            if (pageCount > numberOfPagesToDisplay)
            {
                int middle = (int)Math.Ceiling(numberOfPagesToDisplay / 2d) - 1;
                int below = this.currentPage - middle;
                int above = this.currentPage + middle;

                if (below < 4)
                {
                    above = numberOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (pageCount - 4))
                {
                    above = pageCount;
                    below = pageCount - numberOfPagesToDisplay;
                }

                start = below;
                end = above;
            }

            if (start > 3)
            {
                sb.Append(this.GeneratePageLink("1", 1));
                sb.Append(this.GeneratePageLink("2", 2));
                sb.Append("...");
            }
            for (int i = start; i <= end; i++)
            {
                if (i == this.currentPage)
                {
                    sb.AppendFormat("<span class=\"current\">{0}</span>", i);
                }
                else
                {
                    sb.Append(this.GeneratePageLink(i.ToString(), i));
                }
            }
            if (end < (pageCount - 3))
            {
                sb.Append("...");
                sb.Append(this.GeneratePageLink((pageCount - 1).ToString(), pageCount - 1));
                sb.Append(this.GeneratePageLink(pageCount.ToString(), pageCount));
            }

            // Next
            if (this.currentPage < pageCount)
            {
                sb.Append(this.GeneratePageLink("&gt;", this.currentPage + 1));
            }
            else
            {
                sb.Append("<span class=\"disabled\">&gt;</span>");
            }
            return sb.ToString();
        }

        private string GeneratePageLink(string linkText, int pageNumber)
        {
            var pageLinkValueDictionary = new RouteValueDictionary(this.linkWithoutPageValuesDictionary);
            pageLinkValueDictionary.Add("page", pageNumber);
            
            VirtualPathData virtualPathData = this.viewContext.RouteData.Route.GetVirtualPath(this.viewContext.RequestContext, pageLinkValueDictionary);

            if (virtualPathData != null)
            {
                string linkFormat = "<a href=\"/{0}\">{1}</a>";
                return string.Format(linkFormat, virtualPathData.VirtualPath, linkText);
            }
            else
            {
                return null;
            }
        }
    }
}
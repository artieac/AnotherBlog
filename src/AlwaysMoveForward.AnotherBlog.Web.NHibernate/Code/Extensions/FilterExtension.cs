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
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Extensions
{
    public static class FilterExtension
    {
        #region HtmlHelper extensions

        public static IHtmlString FilterOption(this HtmlHelper htmlHelper, string optionName, string selectedOption)
        {
            return htmlHelper.Raw(GenerateFilterOption(optionName, selectedOption));
        }

        #endregion
        public static string GenerateFilterOption(string optionName, string selectedOption)
        {
            string retVal = "<option";
            retVal += " id='" + optionName + "'";
            retVal += " name='" + optionName + "'";
            retVal += " value='" + optionName + "'";

            if (optionName == selectedOption)
            {
                retVal += " selected";
            }

            retVal += ">";
            retVal += optionName;
            retVal += "</option>";
            return retVal;
        }
    }
}
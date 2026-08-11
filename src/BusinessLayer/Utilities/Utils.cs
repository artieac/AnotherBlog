using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AnotherBlog.Core.Utilities
{
    public class Utils
    {
        const string HTML_TAG_PATTERN = "<.*?>";
        /// <summary>
        /// Clean all HTML tags out of the given string
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string StripHtml(string inputString)
        {
            string retVal = "";

            if (inputString != null)
            {
                // Replace with a space rather than deleting outright - tags are what
                // separate table cells/rows (and other block elements) from each
                // other, so deleting them with nothing in their place glues adjacent
                // cell/element text into one unbroken word.
                retVal = Regex.Replace(inputString, @"<(.|\n)*?>", " ");
                retVal = Regex.Replace(retVal, @"\s+", " ").Trim();
            }

            return retVal;
        }
        /// <summary>
        /// Clean all javascript tags out of the given string 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string StripJavascript(string inputString)
        {
            string retVal = inputString;

            int scriptStart = retVal.IndexOf("<script>");

            if (scriptStart > -1)
            {
                int scriptEnd = retVal.IndexOf("</script>");
                retVal.Remove(scriptStart, ((scriptEnd + 9) - scriptStart));
            }

            return retVal;
        }
    }
}

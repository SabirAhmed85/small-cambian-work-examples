using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActiveCareForms
{
    public static class Extensions
    {
        public static string ContentWithTimestamp(this UrlHelper Url, string contentPath)
        {
            // Method used within views to ensure the browser always requests up-to-date version of the file.
            // Appends a querystring variable representing the date and time last modified, so that when
            // the file changes the browsers and any intermediate proxies will not use a cached copy.
            string filePath = Url.RequestContext.HttpContext.Request.MapPath(contentPath);
            var fileDate = File.GetLastWriteTime(filePath);
            return string.Format("{0}?v={1:yyyyMMddHHmmss}", Url.Content(contentPath), fileDate);
        }
    }
}
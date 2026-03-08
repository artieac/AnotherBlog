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
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization; 

namespace AlwaysMoveForward.AnotherBlog.Web.Code.Utilities
{
    public class XmlResult : ActionResult
    {
        public XmlResult(object objectToSerialize)
        {
            this.ObjectToSerialize = objectToSerialize;
        }

        public object ObjectToSerialize { get; set; }

        /// <summary> 
        /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream. 
        /// </summary> 
        /// <param name="context">The controller context for the current request.</param> 
        public override void ExecuteResult(ActionContext context)
        {
            if (this.ObjectToSerialize != null)
            {
                var xs = new XmlSerializer(this.ObjectToSerialize.GetType());
                HttpResponse response = context.HttpContext.Response;
                response.ContentType = "text/xml";
                response.StatusCode = StatusCodes.Status200OK;

                using var writer = new System.IO.StreamWriter(response.Body);
                writer.Write(this.ObjectToSerialize);
                writer.Flush();
            }
        }
    }
}

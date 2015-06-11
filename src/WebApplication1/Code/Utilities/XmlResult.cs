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
 using System.Web.Mvc; 
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
        public override void ExecuteResult(ControllerContext context)
        {
            if (this.ObjectToSerialize != null)
            {
                var xs = new XmlSerializer(this.ObjectToSerialize.GetType());
                context.HttpContext.Response.ContentType = "text/xml";
                xs.Serialize(context.HttpContext.Response.Output, this.ObjectToSerialize);
            }
        }
    }
}

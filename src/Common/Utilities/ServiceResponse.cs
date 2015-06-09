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

namespace AlwaysMoveForward.AnotherBlog.Common.Utilities
{
    public class ServiceResponse<ReturnClass> where ReturnClass : class
    {
        public ServiceResponse(int errorCode, string errorMessage)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.HasError = true;
        }

        public ServiceResponse(ReturnClass returnValue)
        {
            this.ReturnValue = returnValue;
            this.HasError = false;
        }

        public bool HasError { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public ReturnClass ReturnValue { get; set; }
    }
}

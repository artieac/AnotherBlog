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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlwaysMoveForward.AnotherBlog.Common.DomainModel;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;

namespace AlwaysMoveForward.AnotherBlog.Web.Models
{
    public class CalendarModel
    {
        public static string GenerateDateFilter(DateTime targetDate)
        {
            return targetDate.ToString("MM") + "-" + targetDate.ToString("dd") + "-" + targetDate.ToString("yyyy");
        }

        public CalendarModel()
        {
            this.TargetMonth = DateTime.Now;
        }

        // Not sure I like this......Not sure how else to do it though
        public string RouteInformation { get; set; }
        public Blog TargetBlog { get; set; }
        
        private DateTime targetMonth;
        public DateTime TargetMonth 
        {
            get { return this.targetMonth; }
            set
            {
                this.targetMonth = value;

                DateTime trackingDate = value;
                trackingDate = trackingDate.AddDays(-(value.Day - 1));

                switch (trackingDate.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        break;
                    case DayOfWeek.Monday:
                        trackingDate = trackingDate.AddDays(-1);
                        break;
                    case DayOfWeek.Tuesday:
                        trackingDate = trackingDate.AddDays(-2);
                        break;
                    case DayOfWeek.Wednesday:
                        trackingDate = trackingDate.AddDays(-3);
                        break;
                    case DayOfWeek.Thursday:
                        trackingDate = trackingDate.AddDays(-4);
                        break;
                    case DayOfWeek.Friday:
                        trackingDate = trackingDate.AddDays(-5);
                        break;
                    case DayOfWeek.Saturday:
                        trackingDate = trackingDate.AddDays(-6);
                        break;
                }

                this.TrackingDate = trackingDate;
            }
        }
        public DateTime TrackingDate { get; set; }
        public IList<DateTime> CurrentMonthBlogDates { get; set; }

        public string GenerateUrlForMonth(int monthOffset)
        {
            string retVal = this.RouteInformation + "/Month";
            DateTime targetMonth = this.TargetMonth.AddMonths(monthOffset);
            retVal += "/" + targetMonth.Year + "/" + targetMonth.Month;

            return retVal;
        }

        public string GenerateUrlForDay(DateTime trackingDate)
        {
            string retVal = string.Empty;

            retVal += this.RouteInformation + "/Day/" + trackingDate.Year + "/" + trackingDate.Month + "/" + trackingDate.Day;

            return retVal;
        }
    }
}



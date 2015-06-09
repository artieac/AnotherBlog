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
using System.Net;
using AlwaysMoveForward.AnotherBlog.Common.DataLayer.Map;
using AlwaysMoveForward.AnotherBlog.Common.DomainModel;

namespace AlwaysMoveForward.AnotherBlog.DataLayer.DTO
{
    [NHibernate.Mapping.Attributes.Class(Table = "VoterAddresses")]
    public class VoterAddressDTO
    {
        public VoterAddressDTO()
        {
            this.Id = -1;
        }

        [NHibernate.Mapping.Attributes.Id(0, Name = "Id", Column = "Id", UnsavedValue = "-1")]
        [NHibernate.Mapping.Attributes.Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        public virtual IPAddress IPAddress { get; set; }

        [NHibernate.Mapping.Attributes.Property]
        public virtual string Address
        {
            get { return this.IPAddress.ToString(); }
            set { this.IPAddress = IPAddress.Parse(value); }
        }

        [NHibernate.Mapping.Attributes.ManyToOne(Name="Option", Class="PollOptionDTO", ClassType=typeof(PollOptionDTO), Column="PollOptionId")]
        public virtual PollOptionDTO Option { get; set; }
    }
}

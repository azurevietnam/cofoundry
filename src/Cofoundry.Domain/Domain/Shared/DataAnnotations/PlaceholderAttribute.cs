﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cofoundry.Domain
{
    /// <summary>
    /// Use this to provide a UI hint to the admin interface 
    /// to add a placeholder attribute in an html input field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PlaceholderAttribute : Attribute, IMetadataAttribute
    {
        public PlaceholderAttribute(string placeholder)
        {
            Placeholder = placeholder;
        }

        /// <summary>
        /// The text to display as a hint in the input field when no value
        /// is entered.
        /// </summary>
        public string Placeholder { get; set; }

        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData
                .AddAdditionalValueIfNotEmpty("Placeholder", Placeholder)
                ;
        }
    }
}

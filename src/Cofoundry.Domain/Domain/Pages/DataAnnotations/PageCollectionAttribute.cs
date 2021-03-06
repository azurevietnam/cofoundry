﻿using Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cofoundry.Domain
{
    /// <summary>
    /// Use this to decorate an integer array and indicate that it should be a
    /// collection of database ids for Pages. Optional parameters indicate 
    /// whether the collection is sortable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PageCollectionAttribute : Attribute, IMetadataAttribute, IEntityRelationAttribute
    {
        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData
                .AddAdditionalValueIfNotEmpty("Orderable", IsOrderable);

            modelMetaData.TemplateHint = "PageCollection";
        }

        /// <summary>
        /// Can the collection be manually ordered by the user?
        /// </summary>
        public bool IsOrderable { get; set; }
        
        public IEnumerable<EntityDependency> GetRelations(object model, PropertyInfo propertyInfo)
        {
            Condition.Requires(model).IsNotNull();
            Condition.Requires(propertyInfo).IsNotNull();

            var ids = propertyInfo.GetValue(model) as int[];

            if (ids != null)
            {
                foreach (var id in ids)
                {
                    yield return new EntityDependency(PageEntityDefinition.DefinitionCode, id, false);
                }
            }
        }
    }
}

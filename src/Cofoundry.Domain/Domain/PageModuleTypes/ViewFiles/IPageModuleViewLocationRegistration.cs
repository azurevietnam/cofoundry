﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Domain
{
    /// <summary>
    /// Register a location that contains PageModule views, so that they can be put in a non-standard
    /// view location. Registers the location as an embedded resource path and a view location.
    /// </summary>
    public interface IPageModuleViewLocationRegistration
    {
        /// <summary>
        /// Returns prefixes of paths to be registered as module locations. Only include the start e.g. 'PageModules' 
        /// rather than 'PageModules/{0}.cshtml' because the format will be automatically added. You can include any 
        /// number of directory nestings. The module views themselves should be in the folder format 
        /// '{Path}/{ModuleTypeName}/{ModuleTypeName}.cshtml' or '{Path}/{ModuleTypeName}.cshtml'. Templates should 
        /// be located in the folder format '{Path}/{ModuleTypeName}/Templates/{ModuleTypeName}.cshtml' or
        /// '{Path}/Templates/{ModuleTypeName}.cshtml'
        /// </summary>
        string[] GetPathPrefixes();
    }
}

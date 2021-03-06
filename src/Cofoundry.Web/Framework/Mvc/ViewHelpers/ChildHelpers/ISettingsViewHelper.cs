﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cofoundry.Domain;

namespace Cofoundry.Web
{
    /// <summary>
    /// Helper for accessing configuration settings from a view
    /// </summary>
    public interface ISettingsViewHelper
    {
        #region public methods

        T Get<T>() where T : ICofoundrySettings;

        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cofoundry.Web.Identity
{
    public class AuthenticationResult
    {
        public bool IsAuthenticated { get; set; }
        public bool RequiresPasswordChange { get; set; }
        public string ReturnUrl { get; set; }
    }
}
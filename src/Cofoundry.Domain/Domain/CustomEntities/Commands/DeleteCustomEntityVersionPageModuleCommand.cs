﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cofoundry.Domain.CQS;
using Cofoundry.Core.Validation;

namespace Cofoundry.Domain
{
    public class DeleteCustomEntityVersionPageModuleCommand : ICommand, ILoggableCommand
    {
        [PositiveInteger]
        [Required]
        public int CustomEntityVersionPageModuleId { get; set; }
    }
}

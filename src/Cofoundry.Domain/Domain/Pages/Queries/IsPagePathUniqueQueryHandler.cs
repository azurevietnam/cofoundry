﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cofoundry.Domain.Data;
using Cofoundry.Domain.CQS;

namespace Cofoundry.Domain
{
    public class IsPagePathUniqueQueryHandler 
        : IQueryHandler<IsPagePathUniqueQuery, bool>
        , IAsyncQueryHandler<IsPagePathUniqueQuery, bool>
        , IPermissionRestrictedQueryHandler<IsPagePathUniqueQuery, bool>
    {
        #region constructor

        private readonly CofoundryDbContext _dbContext;

        public IsPagePathUniqueQueryHandler(
            CofoundryDbContext dbContext
            )
        {
            _dbContext = dbContext;
        }
        
        #endregion

        #region execution

        public bool Execute(IsPagePathUniqueQuery query, IExecutionContext executionContext)
        {
            var exists = Exists(query).Any();
            return !exists;
        }

        public async Task<bool> ExecuteAsync(IsPagePathUniqueQuery query, IExecutionContext executionContext)
        {
            var exists = await Exists(query).AnyAsync();
            return !exists;
        }

        #endregion

        #region helpers

        private IQueryable<Page> Exists(IsPagePathUniqueQuery query)
        {
            return _dbContext
                .Pages
                .AsNoTracking()
                .Where(d => d.PageId != query.PageId
                    && !d.IsDeleted
                    && d.UrlPath == query.UrlPath
                    && d.LocaleId == query.LocaleId
                    && d.WebDirectoryId == query.WebDirectoryId
                    );
        }

        #endregion
        
        #region Permission

        public IEnumerable<IPermissionApplication> GetPermissions(IsPagePathUniqueQuery query)
        {
            yield return new PageReadPermission();
        }

        #endregion
    }

}

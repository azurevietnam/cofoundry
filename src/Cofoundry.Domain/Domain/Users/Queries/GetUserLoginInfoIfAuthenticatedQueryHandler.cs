﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cofoundry.Domain.Data;
using Cofoundry.Domain.CQS;
using System.Data.Entity;

namespace Cofoundry.Domain
{
    /// <summary>
    /// A query handler that gets information about a user if the specified credentials
    /// pass an authentication check
    /// </summary>
    public class GetUserLoginInfoIfAuthenticatedQueryHandler 
        : IQueryHandler<GetUserLoginInfoIfAuthenticatedQuery, UserLoginInfo>
        , IAsyncQueryHandler<GetUserLoginInfoIfAuthenticatedQuery, UserLoginInfo>
        , IIgnorePermissionCheckHandler
    {
        private readonly UserAuthenticationHelper _userAuthenticationHelper;
        private readonly CofoundryDbContext _dbContext;

        public GetUserLoginInfoIfAuthenticatedQueryHandler(
            CofoundryDbContext dbContext,
            UserAuthenticationHelper userAuthenticationHelper
            )
        {
            _userAuthenticationHelper = userAuthenticationHelper;
            _dbContext = dbContext;
        }

        public UserLoginInfo Execute(GetUserLoginInfoIfAuthenticatedQuery query, IExecutionContext executionContext)
        {
            if (string.IsNullOrWhiteSpace(query.Username) || string.IsNullOrWhiteSpace(query.Password)) return null;

            var user = Query(query).FirstOrDefault();

            return MapResult(query, user);
        }

        public async Task<UserLoginInfo> ExecuteAsync(GetUserLoginInfoIfAuthenticatedQuery query, IExecutionContext executionContext)
        {
            if (string.IsNullOrWhiteSpace(query.Username) || string.IsNullOrWhiteSpace(query.Password)) return null;

            var user = await Query(query).FirstOrDefaultAsync();

            return MapResult(query, user);
        }

        private UserLoginInfo MapResult(GetUserLoginInfoIfAuthenticatedQuery query, User user)
        {
            if (_userAuthenticationHelper.IsPasswordCorrect(user, query.Password))
            {
                var result = Mapper.Map<UserLoginInfo>(user);
                return result;
            }

            return null;
        }

        private IQueryable<User> Query(GetUserLoginInfoIfAuthenticatedQuery query)
        {
            return _dbContext
                .Users
                .AsNoTracking()
                .FilterByUserArea(query.UserAreaCode)
                .FilterCanLogIn()
                .Where(u => u.Username == query.Username);
        }
    }
}

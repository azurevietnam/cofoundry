﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cofoundry.Domain.CQS;
using Cofoundry.Domain;
using Cofoundry.Core.Mail;
using Cofoundry.Domain.MailTemplates;
using Cofoundry.Core;

namespace Cofoundry.Web.Identity
{
    /// <summary>
    /// A helper class with shared functionality between controllers
    /// that manage the currently logged in users account.
    /// </summary>
    public class AccountManagementControllerHelper
    {
        #region constructor

        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IUserContextService _userContextService;
        private readonly IMailService _mailService;
        private readonly IControllerResponseHelper _controllerResponseHelper;

        public AccountManagementControllerHelper(
            IQueryExecutor queryExecutor,
            IMailService mailService,
            ICommandExecutor commandExecutor,
            IUserContextService userContextService,
            IControllerResponseHelper controllerResponseHelper
            )
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
            _userContextService = userContextService;
            _mailService = mailService;
            _controllerResponseHelper = controllerResponseHelper;
        }

        #endregion
        
        #region change password

        public void InitViewModel(IChangePasswordViewModel vm)
        {
            var cx = _userContextService.GetCurrentContext();
            vm.IsPasswordChangeRequired = cx.IsPasswordChangeRequired;
        }

        /// <summary>
        /// Changes a users password, sending them an email notification if the operation 
        /// was successful.
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <param name="vm">The IChangePasswordTemplate containing the data entered by the user.</param>
        /// <param name="notificationTemplate">An IPasswordChangedNotificationTemplate to use when sending the notification</param>
        public void ChangePassword<TNotificationTemplate>(Controller controller, IChangePasswordViewModel vm, TNotificationTemplate notificationTemplate) where TNotificationTemplate : IPasswordChangedTemplate
        {
            var user = _queryExecutor.Execute(new GetCurrentUserMicroSummaryQuery());
            ChangePassword(controller, vm, user);

            if (controller.ModelState.IsValid)
            {
                // Send notification
                if (notificationTemplate != null)
                {
                    notificationTemplate.FirstName = user.FirstName;
                    notificationTemplate.LastName = user.LastName;

                    _mailService.Send(user.Email, user.GetFullName(), notificationTemplate);
                }
            }
        }

        /// <summary>
        /// Changes a users password, without sending them an email notification
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <param name="vm">The IChangePasswordTemplate containing the data entered by the user.</param>
        public void ChangePassword(Controller controller, IChangePasswordViewModel vm)
        {
            var user = _queryExecutor.Execute(new GetCurrentUserMicroSummaryQuery());
            ChangePassword(controller, vm, user);
        }

        /// <summary>
        /// Changes a users password, without sending them an email notification
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <param name="vm">The IChangePasswordTemplate containing the data entered by the user.</param>
        private void ChangePassword(Controller controller, IChangePasswordViewModel vm, UserMicroSummary user)
        {
            if (controller.ModelState.IsValid)
            {
                if (user == null)
                {
                    throw new NotPermittedException("User not logged in");
                }

                var command = new UpdateCurrentUserUserPasswordCommand();
                command.NewPassword = vm.NewPassword;
                command.OldPassword = vm.OldPassword;
                _controllerResponseHelper.ExecuteIfValid(controller, command);
            }
        }

        #endregion
    }
}
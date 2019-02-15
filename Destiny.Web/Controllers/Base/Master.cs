using Destiny.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Destiny.Web.Controllers.Base
{
    public abstract class Master : Controller
    {
        // GET: Master
        protected override void OnException(ExceptionContext filterContext)
        {
            //ErrorLogger.Error(filterContext.Exception);
            base.OnException(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                      || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }
            UserViewModel userInfo = (UserViewModel)Session["userInfo"];

            if (userInfo == null && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Manage") && filterContext.ActionDescriptor.ActionName.Equals("StrokeManagement"))
            {
                filterContext.Result = RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }
            else if (userInfo != null)
            {
                ViewBag.UserName = userInfo.UserName;
                ViewBag.RoleID = userInfo.RoleID;
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName != "DownloadServiceReport" && filterContext.ActionDescriptor.ActionName != "GenerateServiceReport")
                base.OnActionExecuted(filterContext);
        }
    }
}
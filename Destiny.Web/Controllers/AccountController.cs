using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Destiny.Web.Models;
using Destiny.Web.DB;
using System.Web.Script.Serialization;
using Destiny.Web.Controllers.Base;

namespace Destiny.Web.Controllers
{
    public class AccountController : Master
    {
        //private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }
                
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }        

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(UserViewModel userInfor)
        {
            //UserViewModel userInfor = new UserViewModel();
            DBHelper db = new DBHelper();
            try
            {
                //var userName = System.Web.HttpContext.Current.Request["UserName"];
                //var Mobile = System.Web.HttpContext.Current.Request["Mobile"];
                //var Email = System.Web.HttpContext.Current.Request["Email"];
                //var Password = System.Web.HttpContext.Current.Request["Password"];
                //userInfor = new UserViewModel() {
                //    UserName = userName,
                //    Mobile = Mobile,
                //    Email = Email,
                //    Password = Password
                //};
                UserViewModel user = db.Register(userInfor);
                Session["userInfo"] = user;
                ViewBag.UserName = user.UserName;
                ViewBag.RoleID = user.RoleID;
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Success" }));
            }
            catch(Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = ex.Message }));
            }
        }

        [HttpPost]
        public JsonResult UserLogOff()
        {
            try
            {
                Session["userInfo"] = null;
                ViewBag.UserName = null;
                ViewBag.RoleID = null;
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Success" }));
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = ex.Message }));
            }
        }
     
        [HttpPost]
        public ActionResult UserLogin(UserViewModel userInfo)
        {
            try
            {
                DBHelper db = new DBHelper();
                var user = db.Login(userInfo);
                if (user != null)
                {
                    Session["userInfo"] = user;
                    ViewBag.UserName = user.UserName;
                    ViewBag.RoleID = user.RoleID;
                    return Json(new JavaScriptSerializer().Serialize(new { Status = "Success" }));
                }
                else
                    return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = "登录失败" }));
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = ex.Message }));
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        
       
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookStore.Site.Models.DTOs;
using BookStore.Site.Models.Infrastructures.Repositories;
using BookStore.Site.Models.Services;
using BookStore.Site.Models.Services.Interfaces;
using BookStore.Site.Models.ViewModels;

namespace BookStore.Site.Controllers
{
    public class MembersController : Controller
    {
	    private IMemberRepository repository;
	    private MemberService service;

		public MembersController()
	    {
		    repository = new MemberRepository();
		    service = new MemberService(repository);
	    }

		// GET: Members/Index
	    public ActionResult Index()
	    {
		    return View();
	    }

		// GET: Members/Register
		public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVM model)
        {
	        if (!ModelState.IsValid)
	        {
		        return View(model);
	        }

	        var service = new MemberService(repository);

	        (bool IsSuccess, string ErrorMessage) response = 
		        service.CreateNewMember(model.ToRequestDto());

	        if (response.IsSuccess)
	        {
		        // 建檔成功 redirect to confirm page
		        return View("RegisterConfirm");
	        }
	        else
	        {
		        ModelState.AddModelError(string.Empty, response.ErrorMessage);
		        return View(model);
	        }
        }

        public ActionResult ActiveRegister(int memberId, string confirmCode)
        {
	        // var service = new MemberService(repository);
	        service.ActiveRegister(memberId, confirmCode);

	        return View();
        }

        public ActionResult Login()
        {
	        return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
	        // var service = new MemberService(repository);
	        (bool IsSuccess, string ErrorMessage) response = 
		        service.Login(model.Account, model.Password);

	        if (response.IsSuccess)
	        {
		        // 記住登入成功的會員
		        var rememberMe = false;

		        string returnUrl = ProcessLogin(model.Account, rememberMe, out HttpCookie cookie);

		        Response.Cookies.Add(cookie);

		        return Redirect(returnUrl);
	        }

	        ModelState.AddModelError(string.Empty, response.ErrorMessage);

	        return this.View(model);
        }
        public ActionResult Logout()
        {
	        Session.Abandon();
	        FormsAuthentication.SignOut();
	        return Redirect("/Members/Login");
        }

        public ActionResult EditProfile()
        {
	        string currentUserAccount = User.Identity.Name;

	        MemberDto entity = repository.GetByAccount(currentUserAccount);
	        EditProfileVM model = entity.ToEditProfileVM();

	        return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfileVM model)
        {
	        string currentUserAccount = User.Identity.Name;

	        if (ModelState.IsValid == false)
	        {
		        return View(model);
	        }

	        UpdateProfileDto request = model.ToDto(currentUserAccount);
	        try
	        {
		        service.UpdateProfile(request);
	        }
	        catch (Exception ex)
	        {
		        ModelState.AddModelError(string.Empty, ex.Message);
	        }

	        if (ModelState.IsValid == true)
	        {
		        return RedirectToAction("Index");
	        }
	        else
	        {
		        return View(model);
	        }
        }

        [Authorize]
        public ActionResult EditPassword()
        {
	        return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditPassword(EditPasswordVM model)
        {
	        string currentUserAccount = User.Identity.Name;

	        if (ModelState.IsValid == false)
	        {
		        return View(model);
	        }

	        ChangePasswordRequest request = model.ToRequest(currentUserAccount);
	        try
	        {
		        service.ChangePassword(request);
	        }
	        catch (Exception ex)
	        {
		        ModelState.AddModelError(string.Empty, ex.Message);
	        }

	        if (ModelState.IsValid == true)
	        {
		        return RedirectToAction("Index");
	        }
	        else
	        {
		        return View(model);
	        }
        }

        public ActionResult ForgetPassword()
        {
	        return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgetPasswordVM model)
        {
	        if (ModelState.IsValid == false)
	        {
		        return View(model);
	        }

	        try
	        {
		        string urlTemplate = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content("~/") + "Members/ResetPassword?memberid={0}&confirmCode={1}";

		        service.RequestResetPassword(model.Account, model.Email, urlTemplate);

		        return View("ConfirmForgetPassword");
	        }
	        catch (Exception ex)
	        {
		        ModelState.AddModelError(string.Empty, ex.Message);
	        }

	        return View(model);
        }

        public ActionResult ResetPassword(int memberId, string confirmCode)
        {
			// 在 httpPost的action裡,會判斷memberid, confirmCode 是否正確

	        return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(int memberId, string confirmCode, ResetPasswordVM model)
        {
	        if (ModelState.IsValid == false)
	        {
		        return View(model);
	        }

	        try
	        {
		        service.ResetPassword(memberId, confirmCode, model.Password);
	        }
	        catch (Exception ex)
	        {
		        ModelState.AddModelError(string.Empty, ex.Message);
	        }

	        if (ModelState.IsValid == false)
	        {
		        return View(model);
	        }
	        else
	        {
		        return View("ResetPasswordConfirm");
	        }

        }

		private string ProcessLogin(string account, bool rememberMe, out HttpCookie cookie)
        {
	        var member = repository.GetByAccount(account);
	        string roles = String.Empty; // 在本範例, 沒有用到角色權限,所以存入空白

	        // 建立一張認證票
	        FormsAuthenticationTicket ticket =
		        new FormsAuthenticationTicket(
			        1,          // 版本別, 沒特別用處
			        account,
			        DateTime.Now,   // 發行日
			        DateTime.Now.AddDays(2), // 到期日
			        rememberMe,     // 是否續存
			        roles,          // userdata
			        "/" // cookie位置
		        );

	        // 將它加密
	        string value = FormsAuthentication.Encrypt(ticket);

	        // 存入cookie
	        cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

	        // 取得return url
	        string url = FormsAuthentication.GetRedirectUrl(account, true); //第二個引數沒有用處

	        return url;
        }
	}
}
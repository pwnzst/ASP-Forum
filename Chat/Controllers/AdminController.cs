using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.Controllers
{
    public class AdminController : Controller
    {
        private Models.ChatContext chat;
        // GET: Admin /Админ портал
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin /Админ/Users
        public ViewResult Users()
        {
            chat = Models.ChatContext.Context ?? new Models.ChatContext();
            List<Models.UserAdminData> userAdminData = new List<Models.UserAdminData>();
            foreach (var user in Models.ChatContext.Context.Users.ToList())
            {
                userAdminData.Add(new Models.UserAdminData()
                {
                    user = user,
                    isBan = (user.LastLogin.GetValueOrDefault().Equals(DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["banDate"]))),
                    shown = user.ShownData == 0 ? "Логин" :
                            user.ShownData == 1 ? "Имя" : "Почта",
                    postCnt = Models.UserUtil.GetPostCnt(user.Id),
                    secCnt = Models.UserUtil.GetSectionsCnt(user.Id),
                    themeCnt = Models.UserUtil.GetThemeCnt(user.Id),
                    lastPost = Models.UserUtil.GetLastPost(user.Id),
                    cites = Models.UserUtil.GetCommentsCnt(user.Id)
                });
            }
            ViewBag.userAdminData = userAdminData;
            return View();
        }

        public JsonResult ActivateUser(int status, int uid)
        {
            Models.User user = Models.UserUtil.GetUser(uid);
            int result = 0;
            if (user == null)
            {
                result = -1;
            }
            else
            {
                if (status == 1)
                    user.LastLogin = DateTime.Now;
                else if (status == -1)
                    user.LastLogin = DateTime.Parse(System.
                        Configuration.
                        ConfigurationManager.
                        AppSettings["banDate"]
                        );
                else
                    result = -2;

                if (result == 0)
                {
                    try
                    {
                        Models.ChatContext.Context.SaveChanges();
                    }
                    catch
                    {
                        result = -3;
                    }

                }
                
            }
                return Json(new
                {
                    status,
                    uid,
                    result
                },
                JsonRequestBehavior.AllowGet);
            }
        }
    }

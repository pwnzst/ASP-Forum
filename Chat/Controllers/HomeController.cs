using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;


namespace Chat.Controllers
{
    /// <summary>
    /// The main <c>HomeController</c> class. 
    /// Contains all methods for back-end
    /// </summary>
    /// <remarks>
    /// This class can auth, logout, addPost, addTheme, editUser etc.
    /// </remarks>
    public class HomeController : Controller
    {
        Models.ChatContext chat;

        /// <summary>
        /// This method is loading Index page with needed DATA
        /// It checks if there are any sections/themes. If not - it adds "default" sections and themes and saves changes
        /// </summary>
        /// <returns>
        /// nothing
        /// </returns>
        public ActionResult Index()
        {
            if (Models.ChatContext.Context == null)
                chat = new Models.ChatContext();
            else
                chat = Models.ChatContext.Context;

            int n = chat.Sections.Count();
            if (n == 0)  // No sections - create
            {
                chat.Sections.Add(
                    new Models.Section()
                    {
                        Id = 1,
                        Id_Author=1,
                        Title = "Продам / куплю",
                        Description="Раздел о барахолке",
                        CreateDT=DateTime.Now
                    }
                );
                chat.Sections.Add(
                    new Models.Section()
                    {
                        Id = 2,
                        Id_Author = 2,
                        Title = "Болталка",
                        Description = "Разговоры на произвольные темы",
                        CreateDT = DateTime.Now
                    }
                );
                chat.SaveChanges();
            } 

            n = chat.Themes.Count();
            if (n == 0)
            {
                chat.Themes.Add(
                    new Models.Theme()
                    {
                        Id=1,
                        Id_Author=1,
                        Id_Section=1,
                        Title="Продукты питания",
                        Description="Покупка / продажа продуктов питания (Б/У)",
                        CreateDT=DateTime.Now
                    }
                );
                chat.Themes.Add(
                    new Models.Theme()
                    {
                        Id = 2,
                        Id_Author = 2,
                        Id_Section = 1,
                        Title = "Автозапчасти",
                        Description = "Покупка / продажа поломанных деталей от мотоциклов и автомобилей",
                        CreateDT = DateTime.Now
                    }
                );
                chat.Themes.Add(
                    new Models.Theme()
                    {
                        Id = 3,
                        Id_Author = 1,
                        Id_Section = 2,
                        Title = "Знакомства",
                        Description = "Раздел для знакомств и общения",
                        CreateDT = DateTime.Now
                    }
                );

                chat.Themes.Add(
                    new Models.Theme()
                    {
                        Id = 4,
                        Id_Author = 2,
                        Id_Section = 2,
                        Title = "Предсказания",
                        Description = "Прогнозы погоды, политики, экономики",
                        CreateDT = DateTime.Now
                    }
                );
                chat.SaveChanges();
            }
            //ViewBag.UsersCount = chat.Users.Count();
            //ViewBag.SectionsCount = chat.Sections.Count();
            //ViewBag.ThemesCount = chat.Themes.Count();
            //ViewBag.PostsCount = chat.Posts.Count();

            ViewBag.Sections = chat.Sections.ToList();
            var thList = chat.Themes.ToList();
            ViewBag.Themes = thList;

            // Список разделов и информации по ним
            List<Models.Section_Data> sd = new List<Models.Section_Data>();

            // Цикл по разделам
            foreach (var s in ViewBag.Sections)
            {
                // Готовим блок данных по разделу
                Models.Section_Data d = new Models.Section_Data();
                d.section = s;
                // Выбираем темы данного раздела

                var thems = from t in thList
                            where t.Id_Section == s.Id
                            select t;
                // добавляем данные в список
                foreach (Models.Theme th in thems)
                {
                    var dat = new Models.IndexData() 
                    {
                        theme = th,
                        lastPost = Chat.Models.Statistics.GetLastPostInTheme(th.Id),
                        postsCnt = Chat.Models.Statistics.GetThemePostCnt(th.Id),
                        authorShown = Chat.Models.UserUtil.GetUserShown(th.Id_Author),
                       
                   

                    };

                    if (dat.lastPost != null)
                    {
                        dat.lastPostAuthorShown = Models.UserUtil.GetUserShown(dat.lastPost.Id_Author);
                    }

                    d.data.Add(dat);
                }
                sd.Add(d);

                // Передаем список на отображение
                ViewBag.SectionData = sd;  
            }

            var themes = from t in chat.Themes
                         join s in chat.Sections on t.Id_Section equals s.Id
                         select new Models.TwoStrings()
                         {
                             s1 = t.Title,
                             s2 = s.Title
                         };

            List<Models.Section_Themes> st = new List<Models.Section_Themes>();

            foreach (var s in chat.Sections)
            {
                st.Add(new Models.Section_Themes()
                {
                    section = s
                });
            }

            foreach(var s in st)
            {
                var q = from t in chat.Themes
                        where t.Id_Section == s.section.Id
                        select t;

                foreach(Models.Theme t in q)
                {
                    s.themes.Add(t);
                }
            }

            ViewBag.Section_Themes = st;

            return View();
        }

        /// <summary>
        /// This method is an auth method that provides auth and if success - redirectToRoute
        /// it checks if user is not banned and set his lastLogin to current time
        /// and set sessionUserId to current user.Id
        /// </summary>
        /// <param name="authData">Authdata (login/pass) to auth</param>
        /// <returns>RedirectToRoute to HomePage</returns>
        /// <example>
        /// <code>
        /// user.LastLogin = DateTime.Now;
        /// Models.ChatContext.Context.SaveChanges();
        /// </code>
        /// </example>
        public ActionResult Auth(Models.AuthData authData)
        {
            Models.User user = Models.UserUtil.GetUser(authData);
            
            if (user != null)
            {
                if (! Models.UserUtil.IsBanned(user)) { 
                    user.LastLogin = DateTime.Now;
                    Models.ChatContext.Context.SaveChanges();
                }
                Session["authUserId"] = user.Id;
                Session["Error"] = null;
            }
            else
            {
                Session["authUserId"] = null;
                Session["Error"] = "Авторизация отклонена";
            }
            return RedirectToRoute(
                new
                {
                    controller = "Home",
                    action = "Index"
                });
        }

        /// <summary>
        /// This method is a logout method that redirects u to the homePage
        /// it sets sessionUserId to null
        /// then it redirects user to the homePage
        /// </summary>
        /// <returns>nothing</returns>
        /// <example>
        /// <code>
        ///  Session["authUserId"] = null;
        /// </code>
        /// </example>
        public ActionResult Logout()
        {
            Session["authUserId"] = null;
            return RedirectToRoute(new { controller="Home", action="Index" });
        }

        public ActionResult About(int id)
        {
            if (chat == null) chat = new Models.ChatContext();

            var posts = from   p in chat.Posts
                        where  p.Id_Theme == id
                        select p;

            ViewBag.Posts = posts.ToList();
            ViewBag.Id_Theme = id;
            if (Session["AddPostMessage"] != null)
            {
                ViewBag.ErrMessage = Session["AddPostMessage"];
                Session["AddPostMessage"] = null;
            }
            return View();
        }

        /// <summary>
        /// This method is an AddPost method that provides sending a post (message) if user is logged in.
        /// it checkes if the user is not banned, if so - it checks for an empty post, post without theme, author
        /// if everything is fine it adds Post to the database with the all data
        /// else it sends the email if u are cited
        /// </summary>
        /// <param name="post">Post object</param>
        /// <returns>redirectToRoute to the theme</returns>
        /// <example>
        /// <code>
        ///  post.CreateDT = DateTime.Now;
        ///   chat.Posts.Add(post);
        ///   chat.SaveChanges();
        /// </code>
        /// </example>
        public RedirectToRouteResult AddPost(Models.Post post)
        {
            
            if (chat == null) chat = new Models.ChatContext();

            // Разделяем функционал по признаку наличия ненулевого Id
            if (post.Id == 0)
            {   // Добавление нового поста
                // 1. Проверить на пустоту и на валидность

                var bannedUser = Models.UserUtil.IsBanned(Convert.ToInt32(Session["authUserId"]));

                
                

                if (String.IsNullOrEmpty(post.Message))
                {
                    Session["AddPostMessage"] = "Сообщение не может быть пустым";
                }
                else if (post.Id_Theme == 0)
                {
                    Session["AddPostMessage"] = "Укажите тему сообщения";
                }
                else if (post.Id_Author == 0)
                {
                    Session["AddPostMessage"] = "Необходимо авторизоваться";
                }
                else if (post.Id_Author != Convert.ToInt32(Session["authUserId"]))
                {
                    Session["AddPostMessage"] = "Ошибка подлинности. Повторите вход в систему";
                }
                else if (bannedUser) Session["AddPostMessage"] = "Вы забанены! Обратитесь к администратору!";
                else
                {
                   

                    

                    // 2. Добавить в БД
                    post.CreateDT = DateTime.Now;
                    chat.Posts.Add(post);
                    chat.SaveChanges();

                    var authorCiteId = 1;
                    var pMessage = "";
                    var pTheme = "";
                    
                    var mailedUser = Models.UserUtil.GetUser(1);
                    try { 
                    authorCiteId = (from p in chat.Posts
                                        where post.Id_Cite == p.Id
                                        select p.Id_Author).First();

                    pMessage = (from p in chat.Posts
                                    where post.Id_Cite == p.Id
                                    select p.Message).First();

                    pTheme = (from p in chat.Posts
                                  join t in chat.Themes on p.Id_Theme equals t.Id
                                  where post.Id_Cite == p.Id
                                  select t.Title).First();
                    
                    
                        mailedUser = Models.UserUtil.GetUser(Convert.ToInt32(authorCiteId));
                    }
                    catch(Exception e)
                    {

                    }
                    if (authorCiteId != null)
                    {
                        if (IsValidEmail(mailedUser.Email))
                        {
                            Models.UserUtil.SendMail(mailedUser.Email, "Ваше сообщение : <a href=\"http://localhost:63787/Home/About/" + post.Id_Theme+ "\" target=\"_blank\" style='font-weight:bold; font-size:25px; text-decoration:none;'>" + pMessage + "</a> в теме <i><b>" + pTheme + "</b></i> было цитировано на нашем форуме.<br/> <span style='color:red; font-weight:bold;'>Мы с нетерпением Вас ждем.<span>");
                        }
                        else
                        {
                            Debug.Write("Почта не указана либо указана не верно, письмо не отправлено!");
                        }
                    }
                    Session["AddPostMessage"] = "Сообщение добавлено";
                }
            }
            else   // Id != 0   -  редактирование поста
            {
                // 1. Проверить на пустоту и на валидность
                if (String.IsNullOrEmpty(post.Message))
                {
                    Session["AddPostMessage"] = "Сообщение не может быть пустым";
                }
                else
                {
                    var p = chat.Posts.Find(post.Id);
                    if (p == null)
                    {
                        Session["AddPostMessage"] = "Сообщение не найдено в БД";
                    }
                    else
                    {
                        // 2. Вносим правки
                        p.Message = post.Message;
                        chat.SaveChanges();
                        Session["AddPostMessage"] = "Сообщение сохранено";
                    }
                }
                
            }

            // 3. Возврат на страницу темы
            return RedirectToRoute(new {
                controller="Home",
                action="About",
                id=post.Id_Theme
            });
            
        }

        /// <summary>
        /// Contact page method
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return PartialView();
        }

        /// <summary>
        /// Registration Page
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Registration()
        {
            ViewBag.Message = "Registration page.";

            return View();
        }

        /// <summary>
        /// This method is for new user registration
        /// it generates passHash for a password+salt and if all fields are filled it adds data to the database (creates new user)
        /// </summary>
        /// <param name="user">user that wants to register</param>
        /// <returns>View</returns>
        /// <example>
        /// <code>
        /// Models.ChatContext chat = new Models.ChatContext();
        /// chat.Users.Add(user);
        /// chat.SaveChanges();
        /// </code>
        /// </example>
        public ActionResult RegUser(Models.User user)
        {
            if (!String.IsNullOrEmpty(user.Login))
            {
                user.PassSalt = Utils.GetPassSalt();
                user.PassHash = Utils.GetHashByPassSalt(user.PassHash, user.PassSalt);
                user.Registered = DateTime.Now;

                if(Request.Files.Count > 0)
                {
                    var file = (HttpPostedFileWrapper)Request.Files[0];

                    string path = Server.MapPath("/");

                    Utils.MoveUploadAvatar(file, path);

                    user.AvatarFile = file.FileName;
                }

                Models.ChatContext chat = new Models.ChatContext();
                chat.Users.Add(user);
                chat.SaveChanges();
            }
            ViewBag.User = user;

            return View();
        }

        /// <summary>
        /// Cabinet page method
        /// </summary>
        /// <returns>View</returns>
        public ViewResult Cabinet()
        {
            if( Session["authUserId"] == null )
            {
                Session["Error"] = "Необходимо авторизоваться";
                Response.Redirect("/Home");
                return null;
            }
            if (Session["CabinetMessage"] != null)
            {
                ViewBag.CabinetMessage = Session["CabinetMessage"];
                Session["CabinetMessage"] = null;
            }
            ViewBag.User = Models.UserUtil.GetUser(Convert.ToInt32(Session["authUserId"]));
            return View();
        }

        /// <summary>
        /// This method provides user data editing.
        /// it checkes if session[id] is not null.
        /// and if new fields are not empty it allows to edit the user data.
        /// </summary>
        /// <param name="user">user itself</param>
        /// <returns>RedirectToRoute to cabinet</returns>
        public ActionResult EditUser(Models.User user)
        {
            // Проверка авторизации
            if (Session["authUserId"] == null)
            {
                Session["Error"] = "Необходимо авторизоваться";
                Response.Redirect("/Home");
                return null;
            }
            // Находим пользователя
            var updated = Models.ChatContext.Context.Users.Find(
                Convert.ToInt32(Session["authUserId"])
            );
            if(updated != null)
            {
                updated.RealName = user.RealName;
                updated.Email = user.Email;
                updated.ShownData = user.ShownData;
                updated.Status = user.Status;

                if (!String.IsNullOrEmpty(user.PassHash))
                {
                    // Требуется обновление пароля

                    // Проверка на шаблоны
                    if (Utils.ValidatePassword(user.PassHash))
                    {
                        updated.PassSalt = Utils.GetPassSalt();
                        updated.PassHash = Utils.GetHashByPassSalt(user.PassHash, updated.PassSalt);
                    }
                    else
                    {
                        // Не соответствует шаблонам
                        Session["CabinetMessage"] = "Пароль должен содержать цифру, букву (м и Б), спецсимвол";
                        return RedirectToRoute(new { action = "Cabinet" });
                    }
                }

                if (Request.Files.Count > 0)
                {                    
                    var file = (HttpPostedFileWrapper)Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        if (file.ContentType.StartsWith("image"))
                        {
                            // Требуется обновление аватара
                            string path = Server.MapPath("/");
                            Utils.MoveUploadAvatar(file, path);
                            updated.AvatarFile = file.FileName;
                        }
                        else
                        {
                            Session["CabinetMessage"] = "Файл аватар не является допустимым изображением";
                            return RedirectToRoute(new { action = "Cabinet" });
                        }
                    }
                }

                Models.ChatContext.Context.SaveChanges();

                // Д.З. 
                //      * проверить загруженные файлы на допустимые типы изображений


            }
            return RedirectToRoute(new { action = "Cabinet" });
        }

        /// <summary>
        /// This method provides adding themes to the new or existing section.
        /// It returns errCode if there are any errors.
        /// Then it adds needed section, theme and post itself
        /// </summary>
        /// <param name="section">section name</param>
        /// <param name="theme">theme name</param>
        /// <param name="description">description</param>
        /// <param name="secDescription">second description</param>
        /// <returns>errorCode if something is wrong or addTheme()</returns>
        /// <example>
        /// <code>
        /// if (String.IsNullOrEmpty(section))     return "-1";
        /// if (String.IsNullOrEmpty(theme))       return "-2";
        /// if (Session["authUserId"] == null)     return "-3";
        /// if (String.IsNullOrEmpty(description)) return "-4";
        /// </code>
        /// </example>
        public String AddTheme(String section, String theme, String description, String secDescription)
        {
            if (String.IsNullOrEmpty(section))     return "-1";
            if (String.IsNullOrEmpty(theme))       return "-2";
            if (Session["authUserId"] == null)     return "-3";
            if (String.IsNullOrEmpty(description)) return "-4";

            var sec = from s in Models.ChatContext.Context.Sections
                      where s.Title.ToLower().Equals(section.ToLower())
                      select s;
            int sec_id = 0;
            foreach (var s in sec)
                sec_id = s.Id;


            if (sec_id > 0)
            {   // Добавление темы к существ. разделу             

                // Проверить, что тема уже есть (в данном разделе)

                var th = from t in Models.ChatContext.Context.Themes
                         where t.Title.ToLower().Equals(theme.ToLower().Trim())
                            && t.Id_Section == sec_id
                         select t;
                int th_id = 0;
                foreach (var t in th)
                    th_id = t.Id;
                if(th_id != 0)
                {  // тема уже есть
                    return "-5";
                }

                Models.ChatContext.Context.Themes.Add(
                    new Models.Theme()
                    {
                        Title = theme,
                        Id_Author = Convert.ToInt32(Session["authUserId"]),
                        Id_Section = sec_id,
                        CreateDT = DateTime.Now,
                        Description = description
                        
                    }
                );
                Models.ChatContext.Context.SaveChanges();

                th_id = 0;
                foreach (var t in th)
                    th_id = t.Id;
                return th_id.ToString();                
            }
            else
            {
                // Создание нового раздела и добавление к нему темы
                Models.ChatContext.Context.Sections.Add(
                     new Models.Section()
                     {
                         Id_Author = Convert.ToInt32(Session["authUserId"]),
                         Title = section,
                         CreateDT = DateTime.Now,
                         Description = secDescription
                     }
                );
                Models.ChatContext.Context.SaveChanges();
                return AddTheme(section, theme, description, secDescription);
            }
            // Д.З. Стилизация блока добавления раздела/темы

            //return sec_id.ToString();
        }

        /// <summary>
        /// This method is checking if an email is Valid
        /// It checks if the email is notnullorWhiteSpace and checks for Regex.IsMatch in a try..catch section
        /// </summary>
        /// <param name="email">email itself</param>
        /// <returns>the result (true/false)</returns>
        /// <example>
        /// <code>
        /// if (string.IsNullOrWhiteSpace(email))
        ///        return false;
        /// </code>
        /// </example>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }






















        // Задачи: 
        // 1 редактирование постов(только собственных) ✔
        // 2 Отправка почты при цитировании
        // 3 Кабинет пользователя ✔
        // 4 Админка по пользователям ✔
        // 5 Смайлики (замена текстовых блоков) ✔
        // 6 Создание тем ✔

        // 
    }
}
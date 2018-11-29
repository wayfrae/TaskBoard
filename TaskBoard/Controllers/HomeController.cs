using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskBoard.Models;
using TaskBoard.Controllers;

namespace TaskBoard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BoardController bc = new BoardController();
            Singleton singleton = Singleton.Instance;
            singleton.Groups = bc.GetAllGroups();
            singleton.Boards = bc.GetAllBoards();
            return View("Index", singleton);
        }        

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
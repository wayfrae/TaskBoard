using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskBoard.Models;

namespace TaskBoard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BoardController bc = new BoardController();
            BoardViewModel data = new BoardViewModel();
            data.Groups = bc.GetAllGroups();
            data.Boards = bc.GetAllBoards();
            return View("Index", data);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ex3_web.Models;


namespace Ex3_web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Display(string ip, int port)
        {
            SingeltonInfo.Instance.openServer(ip, port);
            SingeltonInfo.Instance.ReadOnlyOnce();
            ViewBag.lon = SingeltonInfo.Instance.Lon +180;
            ViewBag.lat = SingeltonInfo.Instance.Lat +90;
            return View();
        }

        public ActionResult Save(string ip, int port,int second,int lenth,int name)
        {

            return View();
        }


        [HttpGet]
        public ActionResult Display3Param(string ip, int port, int time)
        {
         
            return View();
        }

    }
}
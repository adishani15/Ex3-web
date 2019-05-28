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
            SingeltonCommand.Instance.connectServer(ip, port);


            ViewBag.lon = SingeltonCommand.Instance.getInfo("lon")+180;
            ViewBag.lat = SingeltonCommand.Instance.getInfo("lat")+180;
            SingeltonCommand.Instance.close();
            return View();
        }

        public ActionResult Save(string ip, int port,int second,int time,string name)
        {
            SingeltonCommand.Instance.connectServer(ip, port);


            return View();
        }


        [HttpGet]
        public ActionResult Display3Param(string ip, int port, int time)
        {
         
            return View();
        }

        [HttpPost]
        public ActionResult Read(string ip, int port, int time)
        {
           
            return View();
        }

    }
}
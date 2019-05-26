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
            //SingeltonInfo.Instance.openServer(ip, port);

            //ViewBag.lon = SingeltonInfo.Instance.Lon +180;
            //ViewBag.lat = SingeltonInfo.Instance.Lat +90;
            String a = "s";


            SingeltonInfo.Instance.WriteToFile(a);

            return View();
        }



    }
}
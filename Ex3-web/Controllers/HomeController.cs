using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
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


            ViewBag.lon = SingeltonCommand.Instance.getInfo("lon") + 180;
            ViewBag.lat = SingeltonCommand.Instance.getInfo("lat") + 180;
            SingeltonCommand.Instance.close();
            return View();
        }

        public ActionResult Save(string ip, int port, int second, int time, string name)
        {
            SingeltonCommand.Instance.connectServer(ip, port);
            SingeltonCommand.Instance.OpenFile(name);
            Session["time"] = time;
            Session["second"] = second;
            return View();
        }


        [HttpGet]
        public ActionResult Display3Param(string ip, int port, int time)
        {
            Session["time"] = time;
            SingeltonCommand.Instance.connectServer(ip, port);
            return View();
        }


        [HttpPost]
        public string Read()
        {
            List<float> point = new List<float>();
            Random r = new Random();
            var lon = SingeltonCommand.Instance.getInfo("lon") + r.Next(50);
            var lat = SingeltonCommand.Instance.getInfo("lat") + r.Next(50);

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Location");
            writer.WriteElementString("Lon", lon.ToString());
            writer.WriteElementString("Lat", lat.ToString());
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

    [HttpPost]

        public string GetAllParm()
        {
            List<float> list = SingeltonCommand.Instance.OnTimedEvent();
            return ToXml(list);
            
        }

        private string ToXml(List<float> list)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("list");


            writer.WriteElementString("lon", list[0].ToString());
            writer.WriteElementString("lat", list[1].ToString());
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();

        }


    }
}
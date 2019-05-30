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
        // The defult screen
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // Function 1- Show where is the airplane.
        [HttpGet]
        public ActionResult Display(string ip, int port)
        {
            string[] list = ip.Split('.');
            // if here, need to go to function 3- save the airplane route
            if (list.Length != 4)
            {
                return RedirectToAction("DisplayFile", new { name = ip, time = port });
            }
            else
            {
                SingeltonCommand.Instance.connectServer(ip, port);
                // get the information (lon,lat) from the simulator
                ViewBag.lon = SingeltonCommand.Instance.getInfo("lon");
                ViewBag.lat = SingeltonCommand.Instance.getInfo("lat");
                // close the connection
                SingeltonCommand.Instance.close();
                return View();
            }
        }

        // Function 3- save the airplane route in a file.
        public ActionResult Save(string ip, int port, int second, int time, string name)
        {
            SingeltonCommand.Instance.connectServer(ip, port);
            SingeltonCommand.Instance.OpenFile(name);
            // save time and second in Session
            Session["time"] = time;
            Session["second"] = second;
            return View();
        }


        public ActionResult DisplayFile(string name, int time)
        {
            Session["time"] = time;
            // read file
            SingeltonCommand.Instance.ReadAll(name);
            return View();

        }

        // function 2- track the airplane
        [HttpGet]
        public ActionResult Display3Param(string ip, int port, int time)
        {
            Session["time"] = time;
            SingeltonCommand.Instance.connectServer(ip, port);
            return View();
        }

        // Save lon and lat in xml file.
        [HttpPost]
        public string Read()
        {
            List<float> point = new List<float>();
            // get the information (lon,lat) from the simulator
            var lon = SingeltonCommand.Instance.getInfo("lon");
            var lat = SingeltonCommand.Instance.getInfo("lat");
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            // write lon and lat to the xml file
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

        public string DataFromFile()
        {
            List<float> list = SingeltonCommand.Instance.Line;
            return ToXml(list);
        }

        private string ToXml(List<float> list)
        {

            //Init XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("list");
            // if there is nothing to write
            if (list == null)
            {
                writer.WriteElementString("check", "no");
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                return sb.ToString();
            }

            writer.WriteElementString("check", "list");

            writer.WriteElementString("lon", (list[0]).ToString());
            writer.WriteElementString("lat", (list[1] ).ToString());
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();

        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace Ex3_web.Models
{
    public class Command
    {
        public Dictionary<string, double> pathRead = new Dictionary<string, double>();
        public Dictionary<string, string> SimulatorPath = new Dictionary<string, string>();
        NetworkStream ns;
        TcpClient client;
        TcpListener server;
        private bool didConnect;

        /*the constractor will set the map of all the path we need to know to connect with the flight simolator*/
        public Command()
        {
            this.SetTheMap();
            didConnect = false;
        }
        /*here we really do the connection to the server*/
        public void connectServer(string ip,int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip),port);
            this.server = new TcpListener(ep);
            this.client = new TcpClient();
            client.Connect(ep);
            Console.WriteLine("You are connected");
            this.ns = client.GetStream();
            this.didConnect = true;

        }



        private void SetTheMap()
        {
            this.SimulatorPath.Add("lon","get /position/longitude-deg");
            this.SimulatorPath.Add("lat","get /position/latitude-deg");
            this.SimulatorPath.Add("alt","get /instrumentation/altimeter/indicated-altitude-ft");
            this.SimulatorPath.Add("direction","get /instrumentation/heading-indicator/indicated-heading-deg");
            this.SimulatorPath.Add("speed","get /instrumentation/airspeed-indicator/indicated-speed-kt");
        }


        /*this function is working from the view modle and gets her param ftom there and do a work on the 
         * data and pass it to the simolator*/
        public void setInfo(List<string> path)
        {
            string goTo = "set ";
            goTo += this.SimulatorPath[path[0]];
            goTo += " ";
            goTo += path[1];
            goTo += "\r\n";
            Console.WriteLine(goTo);
            byte[] byteTime = System.Text.Encoding.ASCII.GetBytes(goTo.ToString());
            this.ns.Write(byteTime, 0, byteTime.Length);

        }

        /*this function works from the auto view model this is an anather function because we need todo it on anather thread*/
        public void setFromAuto(List<List<string>> s)
        {
            //the thread
            Thread thread = new Thread(() =>
            {
                if (s.Count != 0)
                {
                    using (NetworkStream stream = new NetworkStream(this.client.Client, false))
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {

                        while (s.Count != 0)
                        {
                            List<string> temp = s[0];
                            temp.Add("\r\n");
                            s.RemoveAt(0);
                            string path = this.Concat(temp);

                            byte[] data = System.Text.Encoding.ASCII.GetBytes(path);
                            Console.WriteLine(path);
                            writer.Write(data);
                            writer.Flush();
                            Thread.Sleep(2000);
                        }
                    }
                }


            }); thread.Start();
        }


        private string Concat(List<String> thePath)
        {
            string r = "";
            for (int i = 0; i < thePath.Count; i++)
            {
                r += thePath[i];
            }
            return r;
        }

        /*check if was a conection if does not dont call to the write functions*/
        public bool GetDidConnect()
        {
            return this.didConnect;
        }

        /*close the connection to the server*/
        public void close()
        {
            this.client.Close();
            this.server.Stop();
        }

        public float getInfo(string name)
        {
            string command = this.SimulatorPath[name];

        }


    }
}

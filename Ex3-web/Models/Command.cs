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
        StreamWriter createText;
        string name;
        List<List<float>> myList;
        int myLine;

        /*the constractor will set the map of all the path we need to know to connect with the flight simolator*/
        public Command()
        {
            this.SetTheMap();
            didConnect = false;
            this.myLine = 0;
            
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

        public List<float> Line
        {
            get {
                if (myLine < myList.Count)
                {
                    int i = this.myLine;
                    this.myLine++;
                    return this.myList[i];
                }
                else
                {
                    return null;
                }

            }
        }



        private void SetTheMap()
        {
            this.SimulatorPath.Add("lon","get /position/longitude-deg\r\n");
            this.SimulatorPath.Add("lat", "get /position/latitude-deg\r\n");
            this.SimulatorPath.Add("rudder", "get /controls/flight/rudder\r\n");
            this.SimulatorPath.Add("throttle", "get /controls/engines/current-engine/throttle\r\n");

        }


        
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
            this.ns.Close();
        }

        public float getInfo(string name)
        {
            string command = this.SimulatorPath[name];
            byte[] byteTime = System.Text.Encoding.ASCII.GetBytes(command);
            
            this.ns.Write(byteTime, 0, byteTime.Length);
            byte[] data = new byte[this.client.ReceiveBufferSize];
            int byteRead = this.ns.Read(data, 0, Convert.ToInt32(this.client.ReceiveBufferSize));
            string request = Encoding.ASCII.GetString(data, 0, byteRead);
            request = request.Split('=')[1].Split(' ')[1].Split('\'')[1];
            float ans = float.Parse(request);
            return ans;


        }

        public void OpenFile(string name1)
        {
            this.name = name1 + ".txt";
            this.createText = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + @"\" + this.name);
            //StreamWriter KeepWrite = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + @"\" + name);
            this.createText.Close();
            this.didConnect = false;
        }

        public List<float> OnTimedEvent()
        {
            List<float> ret = new List<float>();
            //Lon, lat, throttle, rudder.
            float lon = getInfo("lon");
            ret.Add(lon);
            float lat = getInfo("lat");
            ret.Add(lat);
            float throttle = getInfo("throttle");
            ret.Add(throttle);
            float rudder = getInfo("rudder");
            ret.Add(rudder);
            string forWrite = lon.ToString() + " " + lat.ToString() + " " + throttle.ToString() + " " + rudder.ToString();

            this.WriteFile(this.name, forWrite);
            return ret;
        }

        public void WriteFile(string name,string data)
        {
           
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + name;
            using (StreamWriter KeepWrite = File.AppendText(path))
            {
                KeepWrite.WriteLine(data);
               
            }

        }

        public void ReadAll(string name)
        
        {
            this.myLine = 0;
            name = name + ".txt";
            string[] lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\" + name);
            List<List<float>> temp= new List<List<float>>();
            for (int i = 0; i < lines.Length; i++)
            {
                

                temp.Add(SpiltTheText(lines[i]));
            }
            this.myList = temp;
        }

        private List<float> SpiltTheText(string line)
        {
            List<float> list = new List<float>();
            String[] data = line.Split(' ');
            list.Add(float.Parse(data[0]));
            list.Add(float.Parse(data[1]));

            return list;
        }

         

    }
}

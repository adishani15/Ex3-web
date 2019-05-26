using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace Ex3_web.Models
{
    public class Info
    {
        private bool shouldStop;
        private float lon;
        private float lat;
        TcpListener server;
        TcpClient clientSocket;
        Thread thread;
        List<String> DataFromSimolator;
        Boolean FileIsopen;

        public Info()
        {
            shouldStop = false;
            lon = 0.0f;
            lat = 0.0f;
            DataFromSimolator = new List<string>();
            FileIsopen = false;

        }

        // Property of Lon
        public float Lon
        {
            get { return lon; }
            set
            {
                lon = value;

            }
        }

        // Property of Lat
        public float Lat
        {
            get { return lat; }
            set
            {
                lat = value;

            }
        }

        public List<string> DataFromClient
        {
            get { return DataFromSimolator; }

        }

        private static string readLine(BinaryReader reader)
        {
            // storage the line
            char[] buffer = new char[1024];
            int i = 0;
            // the end of line
            char end = '\0';
            // read untill the end of line
            while (i < 1024 && end != '\n')
            {
                char input = reader.ReadChar();
                buffer[i] = input;
                end = buffer[i];
                i++;
            }

            return new string(buffer);
        }
        public void openServer(string ip, int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            this.server = new TcpListener(ep);
            // open the server
            server.Start();
            // wait for connect
            this.clientSocket = server.AcceptTcpClient();
            // after connection- start listen to the flight.
            this.thread = new Thread(() => listenFlight(server, clientSocket));
            thread.Start();
        }

        private void listenFlight(TcpListener server, TcpClient clientSocket)
        {
            NetworkStream stream = clientSocket.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            DateTime start = DateTime.UtcNow;
            string inputLine;

            while (!shouldStop)
            {
                inputLine = readLine(reader);
                if (Convert.ToInt32((DateTime.UtcNow - start).TotalSeconds) < 90)
                {
                    continue;
                }

                DataFromSimolator.Add(inputLine);
                Thread.Sleep(250);
            }


        }
        /**
         * close the socket.
         * */
        public void close()
        {
            this.shouldStop = true;
            this.thread.Abort();
            this.clientSocket.Close();
            this.server.Stop();

        }

        void ReadOnlyOnce()
        {
            NetworkStream stream = clientSocket.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            DateTime start = DateTime.UtcNow;
            string inputLine;
            string[] splitStr;
            inputLine = readLine(reader);
            splitStr = inputLine.Split(',');
            Lon = float.Parse(splitStr[0]);
            Lat = float.Parse(splitStr[1]);

        }

        public void WriteToFile(string line)

        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"\Names.txt");
            path = Environment.CurrentDirectory;
            path = path + "\\tex.txt";
            using (FileStream fs = File.Create(path))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            // Open the stream and read it back.
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }

    }    
}


        
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ex3_web.Models;

namespace Ex3_web
{
    public class SingeltonCommand
    {
        private static Command m_Instance = null;
        public static Command Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Command();
                }
                return m_Instance;
            }
        }
    }
}

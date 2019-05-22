using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ex3_web.Models;

namespace Ex3_web
{
    public class SingeltonInfo
    {
        private static Info m_Instance = null;
        public static Info Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Info();
                }
                return m_Instance;
            }
        }
    }
}
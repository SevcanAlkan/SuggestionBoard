﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SuggestionBoard.Web.Helper
{
    public static class GetHostMachineIP
    {
        public static string Get()
        {
            string ipAddress = "127.0.0.1";

            try
            {
                ipAddress = Dns.GetHostAddresses(new Uri("http://docker.for.win.localhost").Host)[0].ToString();
            }
            catch (SocketException)
            {
                //throw es;
            }
            catch (Exception)
            {
                //throw e;                
            }

            return ipAddress;
        }
    }

    public interface IGetHostMachineIP
    {
        string Get();
    }
}

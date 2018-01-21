using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole
{
    public class NotificationHub : Hub
    {
        public NotificationHub()
        {
            _instance = this;
        }

        private static NotificationHub _instance { get; set; }

        
        public static void Reload()
        {
            if (_instance != null)
            {
                _instance.Clients.All.reload();
            }
        }
    }
}
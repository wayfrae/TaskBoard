using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TaskBoard.Hubs
{
    public class BoardHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using TaskBoard.Models;

namespace TaskBoard.Hubs
{
    public class BoardHub : Hub
    {
        Singleton instance = Singleton.Instance;

        public void Send(string json)
        {
            JObject jobject = JObject.Parse(json);
            Clients.All.broadcastC(instance.Boards);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaskBoard.Models;

namespace TaskBoard.Hubs
{
    public class BoardHub : Hub
    {
        Singleton instance = Singleton.Instance;

        public void Send(string json)
        {
            //Console.WriteLine(json);
            //JObject jobject = JObject.Parse(json);
            Clients.All.broadcast(json);
            instance.Boards = Parse(json);
        }

        private ObservableCollection<Board> Parse(string json)
        {
            ObservableCollection<Board> boards = new ObservableCollection<Board>();

            dynamic obj = JsonConvert.DeserializeObject(json);
            foreach (var item in obj)
            {
                if (item != null)
                {
                    boards.Add(new Board
                    {
                        ID = item.id,
                        Title = item.title,
                        Body = item.body,
                        Owner = item.owner,
                        IsLocked = (item.isLocked == 1) ? true : false
                    });
                }
                
            }
            
            return boards;
        }

    }
}
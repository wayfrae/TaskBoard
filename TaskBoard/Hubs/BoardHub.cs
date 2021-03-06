﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaskBoard.Models;

namespace TaskBoard.Hubs
{
    public class BoardHub : Hub
    {
        Singleton instance = Singleton.Instance;
        private Controllers.BoardController bc = new Controllers.BoardController();

        public void AddBoard(int groupId)
        {
            bc.CreateBoard("", "", groupId);
            instance.Boards = bc.GetAllBoards();
        }

        public void AddGroup(string name)
        {
            bc.CreateGroup(name);
            instance.Groups = bc.GetAllGroups();
            instance.Boards = bc.GetAllBoards();
        }

        public void DeleteBoard(int boardId)
        {
            bc.RemoveBoard(boardId);
            instance.Boards = bc.GetAllBoards();
        }

        public void Send(string json)
        {
            //Console.WriteLine(json);
            //JObject jobject = JObject.Parse(json);
            Parse(json);
            Clients.All.broadcast(CreateJSON());
        }

        public string CreateJSON()
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(instance.Boards);
        }

        private void Parse(string json)
        {
            ObservableCollection<Board> boards = new ObservableCollection<Board>();

            dynamic obj = JsonConvert.DeserializeObject(json);
            Boolean cont = true;
            int index = 0;
            foreach (var item in obj)
            {
                if (index < 1)
                {
                    if (item.type == "addBoard")
                    {
                        AddBoard(int.Parse(item.groupId.ToString()));
                        cont = false;
                    }
                    else if (item.type == "addGroup")
                    {
                        AddGroup(item.name.ToString());
                        var serializer = new JavaScriptSerializer();
                        Clients.All.broadcast(serializer.Serialize(instance.Groups));
                        cont = false;
                    }
                    else if (item.type == "removeBoard")
                    {
                        DeleteBoard(int.Parse(item.boardId.ToString()));
                        Clients.All.broadcast("{\"type\": \"removeBoard\", \"id\": " + item.boardId.ToString() + "}");
                        cont = false;
                    }
                }
                else
                {
                    if (item != null)
                    {
                        int tempID, tempOwner;
                        int.TryParse(item.id.ToString(), out tempID);
                        int.TryParse(item.owner.ToString(), out tempOwner);

                        Board b = new Board();

                        b.ID = tempID;
                        b.Title = item.title.ToString();
                        b.Body = item.body.ToString();
                        b.Owner = tempOwner;
                        b.IsLocked = (item.locked.ToString().Equals("1")) ? true : false;
                        
                        boards.Add(b);
                    }
                }

                index++;
                
            }

            if (cont)
            {
                instance.Boards = boards;
            }        
            
        }

    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TaskBoard.Controllers;

namespace TaskBoard.Models
{
    public sealed class Singleton
    {
        private static readonly Singleton _instance = new Singleton();
        private BoardController bc;

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

        public ObservableCollection<Board> Boards { get; set; }        
        public ObservableCollection<Group> Groups { get; set; }

        private Singleton()
        {
            BoardController bc = new BoardController();
            this.Groups = bc.GetAllGroups();
            this.Boards = bc.GetAllBoards();
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if the changed object is Boards
            if(sender is ObservableCollection<Board>)
            {
                ObservableCollection<Board> boards = (ObservableCollection<Board>)sender;
                foreach(Board board in boards)
                {
                    bc.CreateOrEditBoard(board);
                }
            }

            //if the changed object is Groups
            if(sender is ObservableCollection<Group>)
            {
                ObservableCollection<Group> boards = (ObservableCollection<Group>)sender;

            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using TaskBoard.Controllers;

namespace TaskBoard.Models
{
    public sealed class Singleton : INotifyPropertyChanged
    {
        private static readonly Singleton _instance = new Singleton();
        private BoardController bc = new BoardController();
        private ObservableCollection<Board> _boards;
        private ObservableCollection<Group> _groups;
        private int count = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

        public ObservableCollection<Board> Boards
        {
            get
            {
                return _boards;
            }
            set
            {
                _boards = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Group> Groups
        {
            get
            {
                return _groups;
            }
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        private Singleton()
        {
            this.Groups = bc.GetAllGroups();
            this.Boards = bc.GetAllBoards();
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (propertyName.Equals("Boards") && count > 3)
            {
                foreach (Board board in this.Boards)
                {
                    bc.CreateOrEditBoard(board);
                }
            }
            
            count++;
        }    

    }
}
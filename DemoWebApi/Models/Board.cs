using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebApi.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Descripiton { get; set; }
        public string Title { get; set; }
        public IList<Card> Cards { get; set; }
        public BoardState State { get; set; }
    }

    public enum BoardState
    {
        Active,
        Closed,
        Deleted
    }
}
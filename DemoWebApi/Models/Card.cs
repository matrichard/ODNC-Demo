using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DemoWebApi.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public CardState State { get; set; }

        #region Required

        //[Required]
        public int Priority { get; set; }

        #endregion

        public int BoardId { get; set; }
    }


    public enum CardState
    {
        Active,
        Closed,
        InProgress,
        Deleted
    }
}
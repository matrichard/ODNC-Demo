using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoWebApi.Models;

namespace DemoWebApi.Data
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        IEnumerable<Card> GetCardsForBoard(int parentId);
    }

    public class CardRepository : ICardRepository
    {
        public IEnumerable<Card> GetAllCards()
        {
            using (var ctx = new BoardContext())
            {
                return ctx.Cards.ToList();
            }
        }

        public IEnumerable<Card> GetCardsForBoard(int parentId)
        {
            using (var ctx = new BoardContext())
            {
                return ctx.Cards.Where(x => x.BoardId == parentId).ToList();
            }
        }
    }
}
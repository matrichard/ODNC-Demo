using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DemoWebApi.Data;
using DemoWebApi.Models;

namespace DemoWebApi.Controllers
{
    public class CardsController : ApiController
    {
        private readonly ICardRepository repository;

        public CardsController() :
            this(new CardRepository())
        {
        }

        public CardsController(ICardRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Card> Get()
        {
            return this.repository.GetAllCards();
        }

        public HttpResponseMessage Post(Card card)
        {
            //var x = ModelState.IsValid;
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        #region SubController

        public IEnumerable<Card> Get(string parentController, int parentId)
        {
            return this.repository.GetCardsForBoard(parentId);
        }

        #endregion
    }
}

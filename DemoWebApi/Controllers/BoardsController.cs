using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using DemoWebApi.Data;
using DemoWebApi.Models;

namespace DemoWebApi.Controllers
{
    public class BoardsController : ApiController
    {
        private readonly IBoardRepository repository;

        public BoardsController() :
            this(new BoardRepository())
        {
        }

        public BoardsController(IBoardRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Board> Get()
        {
            return this.repository.GetAllBoards();
        }

        public Board Get(int id)
        {
            var board = this.repository.GetBoard(id);
            return board;

            #region Message 1

            //return Request.CreateResponse(HttpStatusCode.OK, board);

            #endregion

            #region Message 2

            //var msg = new HttpResponseMessage(HttpStatusCode.OK)
            //    {
            //        Content = new ObjectContent(typeof(Board), board, new JsonMediaTypeFormatter())
            //    };
            //return msg;

            #endregion

            #region Message 3

            //var negotiator = Configuration.Services.GetService(typeof(IContentNegotiator)) as IContentNegotiator;
            //var result = negotiator.Negotiate(typeof(Board), Request, Configuration.Formatters);
            //var content = new ObjectContent(typeof(Board), board, result.Formatter);
            //content.Headers.ContentType = result.MediaType;

            //return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            #endregion

        }

        public void Post(Board board)
        {
            if (board == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            this.repository.Add(board);

            #region Message

            //var msg = new HttpResponseMessage(HttpStatusCode.Created);
            //var location = Url.Link(WebApiConfig.DefaultApiRoute, new { id = board.Id });
            //msg.Headers.Location = new Uri(location);
            //return msg;

            #endregion
        }
        
        #region Action

        [HttpPost]
        [ActionName("Close")]
        public HttpResponseMessage Close(int id)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var board = this.repository.GetBoard(id);
            if (board == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new StringContent("board not found");
                response.ReasonPhrase = "INVALID_ID";
                throw new HttpResponseException(response);
            }

            if (board.State == BoardState.Deleted)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("Cannot close a deleted board");
                response.ReasonPhrase = "INVALID_STATE";
                throw new HttpResponseException(response);
            }

            board.State = BoardState.Active;
            this.repository.Save(board);

            return response;
        }

        #endregion

        public void Put(Board b)
        {
            
        }
    }
}

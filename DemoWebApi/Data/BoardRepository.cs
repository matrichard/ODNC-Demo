using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DemoWebApi.Models;
using System.Data.Entity;

namespace DemoWebApi.Data
{
    public interface IBoardRepository
    {
        IEnumerable<Board> GetAllBoards();
        Board GetBoard(int id);
        void Save(Board board);
        Board Add(Board board);
    }

    public class BoardRepository : IBoardRepository
    {
        public IEnumerable<Board> GetAllBoards()
        {
            using (var ctx = new BoardContext())
            {
                return ctx.Boards.ToList();
            }
        }

        public Board GetBoard(int id)
        {
            using (var ctx = new BoardContext())
            {
                return ctx.Boards.SingleOrDefault(x => x.Id == id);
            }
        }

        public void Save(Board board)
        {
            using (var ctx = new BoardContext())
            {
                ctx.Boards.Attach(board);
                ctx.Entry(board).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public Board Add(Board board)
        {
            using (var ctx = new BoardContext())
            {
                ctx.Boards.Add(board);
                ctx.SaveChanges();
                return board;
            }
        }
    }
}
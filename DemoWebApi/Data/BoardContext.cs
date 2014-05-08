using System;
using System.Collections.Generic;
using System.Data.Entity;
using DemoWebApi.Models;

namespace DemoWebApi.Data
{
    public class BoardContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<Card> Cards { get; set; }
    }


    public class DbInitializer : DropCreateDatabaseIfModelChanges<BoardContext>
    {
        protected override void Seed(BoardContext context)
        {
            var activeBoard = new Board
                {
                    Descripiton = "This is an ACTIVE board",
                    Title = "ACTIVE Board",
                    State = BoardState.Active,
                    Cards = new List<Card>
                        {
                            new Card
                                {
                                    Title = "First Card",
                                    Description = "The first card in the board",
                                    State = CardState.Active
                                },
                            new Card
                                {
                                    Title = "Deleted Card",
                                    Description = "A deleted card on the board",
                                    State = CardState.Deleted
                                },
                            new Card
                                {
                                    Title = "InProgress Card",
                                    Description = "A InProgress card on the board",
                                    State = CardState.InProgress
                                }
                        }
                };
            var deletedBoard = new Board
                {
                    Descripiton = "This is a DELETED board",
                    Title = "DELETED Board",
                    State = BoardState.Deleted,
                    Cards = new List<Card>
                        {
                            new Card
                                {
                                    Title = "Closed Card",
                                    Description = "A closed card in the board",
                                    State = CardState.Closed,
                                },
                            new Card
                                {
                                    Title = "Deleted Card",
                                    Description = "A deleted card on the board",
                                    State = CardState.Deleted
                                },
                            new Card
                                {
                                    Title = "InProgress Card",
                                    Description = "A InProgress card on the board",
                                    State = CardState.InProgress
                                }
                        }
                };
            var closedBoard = new Board
            {
                Descripiton = "This is a CLOSED board",
                Title = "CLOSED Board",
                State = BoardState.Closed,
                Cards = new List<Card>
                        {
                            new Card
                                {
                                    Title = "First Card",
                                    Description = "The first card in the board",
                                    State = CardState.Active
                                },
                            new Card
                                {
                                    Title = "Deleted Card",
                                    Description = "A deleted card on the board",
                                    State = CardState.Deleted
                                },
                            new Card
                                {
                                    Title = "InProgress Card",
                                    Description = "A InProgress card on the board",
                                    State = CardState.InProgress
                                }
                        }
            };

            context.Boards.Add(activeBoard);
            context.Boards.Add(deletedBoard);
            context.Boards.Add(closedBoard);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
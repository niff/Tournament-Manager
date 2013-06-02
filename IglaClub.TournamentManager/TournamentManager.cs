using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IglaClub.ObjectModel;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.TournamentManager
{
    public class TournamentManager
    {
        private readonly IIglaClubDbContext db;
        
        public TournamentManager(IIglaClubDbContext dbContext)
        {
            this.db = dbContext;
        }

        public bool AddPair(long tournamentId, long player1Id, long player2Id)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            User player1 = db.Users.Find(player1Id);
            User player2 = db.Users.Find(player2Id);
            tournament.Pairs.Add(new Pair() { Tournament = tournament, PairNumber = 1, Player1 = player1, Player2 = player2} );
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Repositories
{
    public class TournamentRepository : BaseRepository
    {
        public TournamentRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }

        public Tournament GetTournamentWithPairsAndOwner(long id)
        {
            return db.Tournaments
                .Include(t => t.Owner)
                .Include(t => t.Pairs)
                .FirstOrDefault(t => t.Id == id);
        }

        public Tournament GetTournamentWithResultsBoardsPairs(long id)
        {
            return db.Tournaments
                .Include(t => t.Results)
                .Include(t => t.Results.Select(r => r.NS))
                .Include(t => t.Results.Select(r => r.EW))
                .Include(t => t.Boards)
                .Include(t => t.Pairs)
                .FirstOrDefault(t => t.Id == id);
        }

        public Tournament GetTournament(long id)
        {
            return db.Tournaments
                .FirstOrDefault(t => t.Id == id);
        }

        public IList<Tournament> GetOncoming()
        {
            DateTime dateTime = DateTime.Today.AddDays(-1);
            return
                db.Tournaments.Where(
                    t => t.PlannedStartDate >= dateTime && t.TournamentStatus != TournamentStatus.Started)
                    //.Include(t => t.Owner)
                    //.Include(t => t.Pairs)
                    .OrderBy(x => x.PlannedStartDate)
                    .ToList();
        }

        public IList<Tournament> GetPast()
        {
            return
                db.Tournaments.Where(
                    t => t.PlannedStartDate < DateTime.Today && t.TournamentStatus == TournamentStatus.Finished)
                    //.Include(t=>t.Owner)
                    //.Include(t => t.Pairs)
                    .OrderByDescending(x => x.PlannedStartDate)
                    .ToList();
        }

        public IList<Tournament> GetOngoing()
        {
            return db.Tournaments.Where(t => t.TournamentStatus == TournamentStatus.Started)
                // .Include(t=>t.Owner)
                //.Include(t => t.Pairs)
                .OrderByDescending(x => x.TournamentStatus).ThenBy(x => x.PlannedStartDate)
                .ToList();
        }

        public bool UserIsTournamentOwner(string userName, long tournamentId)
        {
            var tournament = db.Tournaments.Find(tournamentId);
            if (tournament == null || tournament.Owner == null ||
                String.Compare(tournament.Owner.Login, userName, StringComparison.OrdinalIgnoreCase) != 0)
                return false;
            return true;
        }

        public bool UserIsSubscribedForTournament(string userName, long tournamentId)
        {
            var tournament = db.Tournaments.Find(tournamentId);
            if (tournament == null || tournament.Pairs == null)
                return false;
            return
                tournament.Pairs.Any(
                    p =>
                        (p.Player1 != null && p.Player1.Login == userName) ||
                        (p.Player2 != null && p.Player2.Login == userName));
        }

        public IList<Tournament> GetTournamentsToPlayForUser(string name)
        {
            var res = from t in db.Tournaments
                join p in db.Pairs on t.Id equals p.Tournament.Id
                where p.Player1.Login == name || p.Player2.Login == name
                select t;
            return res.ToList();
        }
    }
}

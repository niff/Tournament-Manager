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

        public IList<Tournament> GetPlanned()
        {
            DateTime dateFrom = DateTime.Today.AddDays(-1);
            return
                db.Tournaments.Where(
                    t => t.TournamentStatus == TournamentStatus.Planned &&
                        ( !t.PlannedStartDate.HasValue || t.PlannedStartDate >= dateFrom))
                    .OrderBy(x=>x.CreationDate)
                    .ThenBy(x => x.PlannedStartDate)
                    .ToList();
        }

        public IList<Tournament> GetFinished()
        {
            return
                db.Tournaments.Where(
                    t => t.TournamentStatus == TournamentStatus.Finished)
                    .OrderByDescending(x => x.PlannedStartDate)
                    .ToList();
        }

        public IList<Tournament> GetOngoing()
        {
            return db.Tournaments.Where(t => t.TournamentStatus == TournamentStatus.Started)
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
            var tournaments = GetTournamentsBySubscribedUser(name);
            return  tournaments.Where(t=>t.TournamentStatus == TournamentStatus.Planned).ToList();
        }

        public IList<Tournament> GetCurrentlyPlayedByUser(string userLogin)
        {
            var tournaments = GetTournamentsBySubscribedUser(userLogin);
            return tournaments.Where(t=>t.TournamentStatus == TournamentStatus.Started).ToList();
        }

        private IQueryable<Tournament> GetTournamentsBySubscribedUser(string userLogin)
        {
            var res = from t in db.Tournaments
                join p in db.Pairs on t.Id equals p.Tournament.Id
                where p.Player1.Login == userLogin || p.Player2.Login == userLogin
                select t;
            return res;
        }
    }
}

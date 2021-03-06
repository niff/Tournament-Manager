﻿using System;
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

        public IEnumerable<Tournament> GetPlanned()
        {
            DateTime dateFrom = DateTime.Today.AddDays(-1);
            return
                db.Tournaments.Where(
                    t => t.TournamentStatus == TournamentStatus.Planned 
                        && ( !t.PlannedStartDate.HasValue || t.PlannedStartDate >= dateFrom))
                    .OrderBy(t => t.PlannedStartDate)
                    .ThenBy(x => x.PlannedStartDate);
        }

        public IEnumerable<Tournament> GetFinished()
        {
            return
                db.Tournaments.Where(
                    t => t.TournamentStatus == TournamentStatus.Finished)
                    .OrderByDescending(x => x.PlannedStartDate);
        }

        public IEnumerable<Tournament> GetOngoing()
        {
            return db.Tournaments.Where(t => t.TournamentStatus == TournamentStatus.Started)
                .OrderByDescending(x => x.TournamentStatus).ThenBy(x => x.PlannedStartDate);
        }

        public bool UserIsTournamentOwner(string userName, long tournamentId)
        {
            var user = db.Users.FirstOrDefault(u => u.Login.Equals(userName)) ??
                       db.Users.FirstOrDefault(u => u.Email.Equals(userName));
            var tournament = db.Tournaments.Find(tournamentId);
            if (tournament == null || user == null || tournament.OwnerId != user.Id) return false;
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

        public IList<Tournament> GetAvailableTournamentsByUser(string userLogin)
        {
            var tournaments =
                db.Tournaments.Where(
                    t =>
                        t.Pairs.All(p => p.Player1.Login != userLogin && p.Player2.Login != userLogin) &&
                        t.TournamentStatus == TournamentStatus.Planned
                        );

            return tournaments.OrderBy(t => t.PlannedStartDate).ToList();
        }

        public IList<Tournament> GetTournamentsToPlayByUser(string userLogin)
        {
            var tournaments = GetTournamentsBySubscribedUser(userLogin);
            return tournaments.Where(t => t.TournamentStatus == TournamentStatus.Planned).
                OrderBy(t => t.PlannedStartDate).ToList();
        }

        public IList<Tournament> GetCurrentlyPlayingByUser(string userLogin)
        {
            var tournaments = GetTournamentsBySubscribedUser(userLogin);
            return tournaments.Where(t => t.TournamentStatus == TournamentStatus.Started).
                OrderBy(t => t.PlannedStartDate).ToList();
        }

        public IEnumerable<Tournament> GetTournamentsByOwnerUser(string userLogin)
        {
            var tournaments = from t in db.Tournaments
                where t.Owner.Login == userLogin 
                orderby t.PlannedStartDate descending 
                select t;
            return tournaments;
        }

        private IQueryable<Tournament> GetTournamentsBySubscribedUser(string userLogin)
        {
            var res = from t in db.Tournaments
                join p in db.Pairs on t.Id equals p.Tournament.Id
                      where p.Player1.Login == userLogin || p.Player2.Login == userLogin
                      orderby t.PlannedStartDate descending 
                select t;
            return res;
        }

        public IList<Tournament> GetTournamentsAlreadyPlayedByUser(string userLogin)
        {
            var tournaments = GetTournamentsBySubscribedUser(userLogin);
            return tournaments.Where(t => (t.TournamentStatus == TournamentStatus.Finished)).
                OrderByDescending(t => t.PlannedStartDate).ToList();
        }

        private IQueryable<Tournament> GetTournamentsByClub(long clubId)
        {
            var res = from t in db.Tournaments
                      where t.Club.Id == clubId
                      orderby t.PlannedStartDate descending 
                select t;
            return res;
        }

        public IList<Tournament> GetTournamentsPlannedByClub(long clubId)
        {
            var tournaments = GetTournamentsByClub(clubId);
            return tournaments.Where(t => t.TournamentStatus == TournamentStatus.Planned).
                OrderBy(t => t.PlannedStartDate).ToList();
        }
        public IList<Tournament> GetTournamentsFinishedByClub(long clubId)
        {
            var tournaments = GetTournamentsByClub(clubId);
            return tournaments.Where(t => t.TournamentStatus == TournamentStatus.Finished).
                OrderBy(t => t.PlannedStartDate).ToList();
        }
        public IList<Tournament> GetTournamentsStartedByClub(long clubId)
        {
            var tournaments = GetTournamentsByClub(clubId);
            return tournaments.Where(t => t.TournamentStatus == TournamentStatus.Started).
                OrderBy(t => t.PlannedStartDate).ToList();
        }

        public bool UserIsManagingAtLeastOneTournament(long userId)
        {
            return db.Tournaments.Any(t => t.OwnerId == userId);
        }
    }
}

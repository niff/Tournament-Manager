﻿using System;
using System.Collections.Generic;
using System.Linq;
using IglaClub.ObjectModel;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;
using IglaClub.ObjectModel.Exceptions;


namespace IglaClub.TournamentManager
{
    public class TournamentManager
    {
        private readonly IIglaClubDbContext db;

        public TournamentManager(IIglaClubDbContext dbContext)
        {
            this.db = dbContext;
        }

        public OperationStatus AddPair(long tournamentId, long player1Id, long player2Id)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            User player1 = db.Users.Find(player1Id);
            User player2 = db.Users.Find(player2Id);
            var userSubscribedToPair = tournament.Pairs.FirstOrDefault(p => p.ContainsUser(player1Id) || p.ContainsUser(player2Id));
            if (userSubscribedToPair != null)
            {
                return new OperationStatus(false,string.Format("User is already subscribed to another pair ({0})", userSubscribedToPair.ToString()));
            }
            int pairNumber = tournament.Pairs.Any() ? tournament.Pairs.Max(p => p.PairNumber) + 1 : 1;
            tournament.Pairs.Add(new Pair() { Tournament = tournament, PairNumber = pairNumber, Player1 = player1, Player2 = player2} );
            db.SaveChanges();
            return OperationStatus.SucceedOperation;
        }

        public OperationStatus StartTournament(long tournamentId)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            if (tournament.Pairs.Count == 0)
            {
                return new OperationStatus(false,"No pairs subscribed");
            }
            IEnumerable<BoardInstance> boards = GenerateEmptyBoards(tournament);
            TournamentHelper.AddBoardsToTournament(tournament, boards);

            IEnumerable<Result> results = TournamentHelper.GenerateInitialSittingPosition(tournament);
            TournamentHelper.AddResultsToTournament(tournament, results);
            
            tournament.CurrentRound = 1;
            tournament.TournamentStatus = TournamentStatus.Started;
            tournament.StartDate = DateTime.Now;
            db.SaveChanges();
            return new OperationStatus(true);
        }

        public OperationStatus GenerateNextRound(long tournamentId, bool withPairRepeats)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);

            if (tournament == null)
            {
                return new OperationStatus(false,"Tournament with id " + tournamentId + " not found");
            }
            
            if (tournament.TournamentMovingType != TournamentMovingType.Cavendish)
                return new OperationStatus(false,"Tournament with id " + tournamentId + " moving type is different than cavendish (cannot generate extra round)");;

            var notFinishedBoardsInRoundCount = tournament.ResultsNotFinishedInCurrentRound;
            if (notFinishedBoardsInRoundCount > 0)
            {
                return new OperationStatus(false, "Round is still not finished. " + notFinishedBoardsInRoundCount + " results are not entered.");
            }

            IEnumerable<BoardInstance> boards = GenerateEmptyBoards(tournament);
            TournamentHelper.AddBoardsToTournament(tournament, boards);

            IEnumerable<Result> results = TournamentHelper.GenerateNextCavendishRound(tournament,withPairRepeats);
            TournamentHelper.AddResultsToTournament(tournament, results);

            tournament.CurrentRound ++;
            
            db.SaveChanges();
            return new OperationStatus(true);
        }

        //todo add new empty result, add board on fly
        public OperationStatus AddNewResult(long tournamentId)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            if (tournament == null)
                return new OperationStatus(false, "Tournament with id " + tournamentId + " not found");
            if (tournament.Results.Count == 0)
                return new OperationStatus(false, "No results, cannot determine board");

            var result = new Result();
            result.Tournament = tournament;
            result.RoundNumber = tournament.CurrentRound;
            result.Board = GetBoardForNewResult(tournament);

            result.NS = tournament.Pairs.FirstOrDefault();
            result.EW = tournament.Pairs.FirstOrDefault(p=>p.Id != result.NS.Id);
            
            tournament.Results.Add(result);
            db.SaveChanges();
            return new OperationStatus(true);
        }

        private BoardInstance GetBoardForNewResult(Tournament tournament)
        {
            var firstResult = tournament.Results.FirstOrDefault(r => r.RoundNumber == tournament.CurrentRound) ??
                              tournament.Results.FirstOrDefault();

            if (firstResult == null)
            {
                var maxBoardNumber = tournament.Boards.Max(b => b.BoardNumber);
                return CreateEmptyBoardInstance(tournament, maxBoardNumber + 1);
            }
            
            return firstResult.Board;
        }

        private IEnumerable<BoardInstance> GenerateEmptyBoards(Tournament tournament)
        {
            var boardInstances = new List<BoardInstance>();
            if (tournament.TournamentMovingType != TournamentMovingType.Cavendish)
                throw new NotSupportedException("By now only Cavendish tournament type is supported");
            var currentMaxBoardNumber = tournament.Boards.Count == 0 ? 0 : tournament.Boards.Max(b => b.BoardNumber);
            for (int i = currentMaxBoardNumber + 1; i < tournament.BoardsInRound + currentMaxBoardNumber + 1; i++)
            {
                boardInstances.Add(CreateEmptyBoardInstance(tournament, i));
            }

            return boardInstances;
        }

        private BoardInstance CreateEmptyBoardInstance(Tournament tournament, int boardNumber)
        {
            var boardDefinition = new BoardDefinition()
                {
                    CreationDate = DateTime.Now,
                    Dealer = TournamentHelper.GetDealerByBoardNumber(boardNumber),
                    Vulnerability = TournamentHelper.GetVulneralbityByBoardNumber(boardNumber)
                };
            db.BoardDefinitions.Add(boardDefinition);
            db.SaveChanges();

            var boardInstance = new BoardInstance()
                {
                    BoardDefinition = boardDefinition,
                    BoardNumber = boardNumber,
                    Tournament = tournament
                };
            return boardInstance;
        }

        public OperationStatus CalculateScore(long tournamentId)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            if (tournament == null)
            {
                return new OperationStatus(false, "Tournament not found");
            }
            TournamentHelper.UpdateEachResultScore(tournament.Results);
            db.SaveChanges();
            return new OperationStatus(true);
        }
        public OperationStatus CalculateResults(long tournamentId)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            if (tournament == null)
            {
                return new OperationStatus(false, "Tournament not found");
            }
            TournamentHelper.UpdatePointsPerBoard(tournament);
            TournamentHelper.UpdatePairsResultsInTournament(tournament);
            db.SaveChanges();
            return new OperationStatus(true);
        }

        public OperationStatus CalculateResultsComplete(long tournamentId)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            if (tournament == null)
            {
                return new OperationStatus(false, "Tournament not found");
            }
            TournamentHelper.UpdateEachResultScore(tournament.Results);
            TournamentHelper.UpdatePointsPerBoard(tournament);
            TournamentHelper.UpdatePairsResultsInTournament(tournament);
            db.SaveChanges();
            return new OperationStatus(true);
        }

        public OperationStatus RemoveLastRound(long tournamentId)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            if(tournament.CurrentRound < 1)
                return new OperationStatus(false, "NoRoundsToRemove");
            var resultsToRemove= tournament.Results.Where(r => r.RoundNumber == tournament.CurrentRound).ToList();
            foreach (var result in resultsToRemove)
            {
                tournament.Results.Remove(result);
                db.Results.Remove(result);
            }
            tournament.CurrentRound--;
            db.SaveChanges();
            return new OperationStatus(true);
        }

        public bool Create(Tournament tournament, string userName)
        {
            if (tournament.Id != 0)
                return false;
            tournament.TournamentStatus = TournamentStatus.Planned;
            tournament.CreationDate = DateTime.Now;
            tournament.Owner = db.Users.FirstOrDefault(u => String.Compare(u.Login, userName, StringComparison.OrdinalIgnoreCase) == 0);
            db.Tournaments.Add(tournament);
            db.SaveChanges();
            return true;
        }

        public long DeleteResult(long resultId)
        {
            Result result = db.Results.Find(resultId);
            db.Results.Remove(result);
            db.SaveChanges();
            return result.TournamentId;
        }

        public void DeleteTournament(long tournamentId)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            foreach (var result in tournament.Results.ToList())
            {
                db.Results.Remove(result);
            }
            foreach (var pair in tournament.Pairs.ToList())
            {
                db.Pairs.Remove(pair);
            }
            foreach (var boardInstance in tournament.Boards.ToList())
            {
                db.BoardInstances.Remove(boardInstance);
            }
            db.Tournaments.Remove(tournament);
            db.SaveChanges();
        }


        public void UndoTournamentStart(long tournamentId)
        {
            while (RemoveLastRound(tournamentId).Ok)
            {
            }
            var tournament = db.Tournaments.Find(tournamentId);
            tournament.TournamentStatus = TournamentStatus.Planned;
            db.SaveChanges();
        }

        public OperationStatus Finish(long tournamentid)
        {
            var tournament = db.Tournaments.Find(tournamentid);
            tournament.TournamentStatus = TournamentStatus.Finished;
            tournament.FinishDate = DateTime.UtcNow;
            db.SaveChanges();
            return new OperationStatus(true);
        }
    }
}

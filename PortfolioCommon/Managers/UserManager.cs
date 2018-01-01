using PortfolioCommon.Access;
using PortfolioCommon.Entities;
using PortfolioCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.Managers
{
    public class UserManager
    {
        private static object _lock = new object();

        private IUserDataAccess _userDataAccess;
        private CloudQueueAccess _cloudQueueAccess;
        private CloudBlobAccess _cloudBlobAccess;

        public UserManager(IUserDataAccess UserdataAccess, CloudQueueAccess cloudQueueAccess, CloudBlobAccess cloudBlobAccess)
        {
            _userDataAccess = UserdataAccess;
            _cloudQueueAccess = cloudQueueAccess;
            _cloudBlobAccess = cloudBlobAccess;
        }


        public UserEntity GetUser(Guid userId)
        {
            UserEntity user = _userDataAccess.GetUser(userId);
            return user;
        }

        //public List<RaffleEntity> ReadAllRaffles()
        //{
        //    List<RaffleEntity> raffles = _dataAccess.ReadAllRaffles();
        //    return raffles;
        //}

        //public RaffleEntity CreateNewRaffle()
        //{
        //    lock (_lock)
        //    {
        //        var newRaffle = new RaffleEntity();
        //        newRaffle.Id = Guid.NewGuid();
        //        newRaffle.SetStatus(RaffleStatus.Open);
        //        newRaffle.CreateDate = DateTime.Now;
        //        newRaffle.UpdateDate = newRaffle.CreateDate;

        //        _dataAccess.InsertRaffle(newRaffle);

        //        return newRaffle;
        //    }
        //}

        //public void CloseRaffleAndDraw(Guid raffleId)
        //{
        //    RaffleEntity raffle = ReadRaffle(raffleId);

        //    if (raffle.GetStatus() != RaffleStatus.Open)
        //    {
        //        throw new InvalidOperationException("Raffle is not open. Raffle ID: " + raffleId);
        //    }

        //    raffle.SetStatus(RaffleStatus.Closed);
        //    raffle.UpdateDate = DateTime.Now;

        //    _dataAccess.UpdateRaffle(raffle);
        //    _cloudQueueAccess.PostDrawRaffleMessage(raffleId);
        //}

        //public List<BetEntity> ReadBets(Guid raffleId)
        //{
        //    List<BetEntity> betsForRaffle = _dataAccess.ReadBetsForRaffle(raffleId);
        //    return betsForRaffle;
        //}

        //public BetEntity PlaceBet(Guid raffleId, int betNumber, string submitedBy)
        //{
        //    if (betNumber < 1 || betNumber > 6)
        //    {
        //        throw new ArgumentOutOfRangeException("betNumber must be within 1 to 6 inclusive.");
        //    }

        //    lock (_lock)
        //    {
        //        checkIfRaffleIsOpen(raffleId);
        //        int nextTicketNumber = getLastTicketNumber(raffleId) + 1;

        //        var bet = new BetEntity();

        //        bet.RaffleId = raffleId;
        //        bet.TicketNumber = nextTicketNumber;
        //        bet.BetNumber = betNumber;
        //        bet.SubmittedBy = submitedBy;
        //        bet.CreateDate = DateTime.Now;
        //        bet.UpdateDate = bet.CreateDate;

        //        _dataAccess.InsertBet(bet);

        //        uploadBetTicketFile(bet);

        //        return bet;
        //    }
        //}

        //public byte[] GetTicketFileContents(Guid raffleId, int ticketNumber)
        //{
        //    string ticketBlobName = createTicketBlobName(raffleId, ticketNumber);
        //    byte[] ticketFileContents = _cloudBlobAccess.DownloadBlob(ticketBlobName);

        //    return ticketFileContents;
        //}

        //private void checkIfRaffleIsOpen(Guid raffleId)
        //{
        //    RaffleEntity raffle = ReadRaffle(raffleId);

        //    if (raffle == null)
        //    {
        //        throw new InvalidOperationException("Raffle not found. Raffle ID: " + raffleId);
        //    }

        //    if (raffle.GetStatus() != RaffleStatus.Open)
        //    {
        //        throw new InvalidOperationException("Raffle is not open. Raffle ID: " + raffleId);
        //    }
        //}

        //private int getLastTicketNumber(Guid raffleId)
        //{
        //    List<BetEntity> betsForRaffle = ReadBets(raffleId);

        //    if (betsForRaffle.Count == 0)
        //    {
        //        return 0;
        //    }

        //    int maxTicketNumber = betsForRaffle.Max(bet => bet.TicketNumber);
        //    return maxTicketNumber;
        //}

        //private void uploadBetTicketFile(BetEntity bet)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine(string.Format("Raffle ID: {0}", bet.RaffleId));
        //    sb.AppendLine(string.Format("Ticket number: {0}", bet.TicketNumber));
        //    sb.AppendLine(string.Format("Bet number: {0}", bet.BetNumber));
        //    sb.AppendLine(string.Format("Submitted by: {0}", bet.SubmittedBy));
        //    sb.AppendLine(string.Format("Bet Date: {0}", bet.CreateDate));

        //    byte[] fileContents = Encoding.Unicode.GetBytes(sb.ToString());
        //    string ticketBlobName = createTicketBlobName(bet.RaffleId, bet.TicketNumber);
        //    _cloudBlobAccess.UploadBlob(fileContents, ticketBlobName);
        //}

        //private string createTicketBlobName(Guid raffleId, int ticketNumber)
        //{
        //    string ticketBlobName = string.Format("{0}_{1}", raffleId, ticketNumber);
        //    return ticketBlobName;
        //}
    }
}

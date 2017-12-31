using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.Interfaces
{
    public interface IUserDataAccess
    {
        RaffleEntity ReadRaffle(Guid raffleId);
        List<RaffleEntity> ReadAllRaffles();
        void InsertRaffle(RaffleEntity raffle);
        void UpdateRaffle(RaffleEntity raffle);
        List<BetEntity> ReadBetsForRaffle(Guid raffleId);
        void InsertBet(BetEntity bet);
        void ClearData();
    }
}

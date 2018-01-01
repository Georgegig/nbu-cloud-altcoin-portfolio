using PortfolioCommon.Entities;
using PortfolioCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.Access
{
    public class SqlServerDataAccess : IUserDataAccess
    {
        public SqlServerDataAccess()
        {
        }

        private const string _connectionString = @"Server=DESKTOP-SJ5O489;Database=NbuAltcoin;Integrated Security=True;";

        private const string GetUserQuery = @"Select * from User Where [Id] = @Id";

        public UserEntity GetUser(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(GetUserQuery, connection);

                adapter.SelectCommand.Parameters.AddWithValue("@Id", userId);

                DataTable tableRaffles = new DataTable();
                adapter.Fill(tableRaffles);

                if (tableRaffles.Rows.Count == 0)
                {
                    return null;
                }

                DataRow row = tableRaffles.Rows[0];

                UserEntity raffle = createUserFromDataRow(row);
                return raffle;
            }
        }
        private UserEntity createUserFromDataRow(DataRow row)
        {
            UserEntity user = new UserEntity();

            user.Id = row.Field<Guid>(Constants.UserColumnNames.Id);
            user.Email = row.Field<string>(Constants.UserColumnNames.Email);
            user.Name = row.Field<string>(Constants.UserColumnNames.Name);
            user.Password = row.Field<string>(Constants.UserColumnNames.Password);

            return user;
        }


        //private const string SelectAllRafflesQuery = @"Select * from Raffle";
        //private const string InsertRaffleQuery = @"Insert into Raffle (Id, Status, WinningNumber, WinningTicketsResult, CreateDate, UpdateDate) values (@Id, @Status, @WinningNumber, @WinningTicketsResult, @CreateDate, @UpdateDate)";
        //private const string UpdateRaffleQuery = @"Update Raffle Set Status = @Status, WinningNumber = @WinningNumber, WinningTicketsResult = @WinningTicketsResult, CreateDate = @CreateDate, UpdateDate = @UpdateDate where Id = @Id";
        //private const string SelectBetsForRaffleQuery = @"Select * from Bet Where [RaffleId] = @RaffleId";
        //private const string InsertBetQuery = @"Insert into Bet (RaffleId, TicketNumber, BetNumber, SubmittedBy, CreateDate, UpdateDate) values (@RaffleId, @TicketNumber, @BetNumber, @SubmittedBy, @CreateDate, @UpdateDate)";

        //private const string ClearBetsQuery = @"Delete from Bet";
        //private const string ClearRafflesQuery = @"Delete from Raffle";
        //public List<RaffleEntity> ReadAllRaffles()
        //{
        //    List<RaffleEntity> raffles = new List<RaffleEntity>();

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        SqlDataAdapter adapter = new SqlDataAdapter();
        //        adapter.SelectCommand = new SqlCommand(SelectAllRafflesQuery, connection);

        //        DataTable tableRaffles = new DataTable();
        //        adapter.Fill(tableRaffles);

        //        foreach (DataRow row in tableRaffles.Rows)
        //        {
        //            RaffleEntity raffle = createRaffleFromDataRow(row);
        //            raffles.Add(raffle);
        //        }
        //    }

        //    return raffles;
        //}

        //public void InsertRaffle(RaffleEntity raffle)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        SqlCommand insertCommand = new SqlCommand(InsertRaffleQuery, connection);

        //        insertCommand.Parameters.AddWithValue("@Id", raffle.Id);
        //        insertCommand.Parameters.AddWithValue("@Status", raffle.Status);

        //        if (raffle.WinningNumber == null)
        //        {
        //            insertCommand.Parameters.AddWithValue("@WinningNumber", DBNull.Value);
        //        }
        //        else
        //        {
        //            insertCommand.Parameters.AddWithValue("@WinningNumber", raffle.WinningNumber);
        //        }

        //        if (raffle.WinningTicketsResult == null)
        //        {
        //            insertCommand.Parameters.AddWithValue("@WinningTicketsResult", DBNull.Value);
        //        }
        //        else
        //        {
        //            insertCommand.Parameters.AddWithValue("@WinningTicketsResult", raffle.WinningTicketsResult);
        //        }
        //        insertCommand.Parameters.AddWithValue("@CreateDate", raffle.CreateDate);
        //        insertCommand.Parameters.AddWithValue("@UpdateDate", raffle.UpdateDate);

        //        insertCommand.ExecuteNonQuery();
        //    }
        //}

        //public void UpdateRaffle(RaffleEntity raffle)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        SqlCommand updateCommand = new SqlCommand(UpdateRaffleQuery, connection);

        //        updateCommand.Parameters.AddWithValue("@Id", raffle.Id);
        //        updateCommand.Parameters.AddWithValue("@Status", raffle.Status);

        //        if (raffle.WinningNumber == null)
        //        {
        //            updateCommand.Parameters.AddWithValue("@WinningNumber", DBNull.Value);
        //        }
        //        else
        //        {
        //            updateCommand.Parameters.AddWithValue("@WinningNumber", raffle.WinningNumber);
        //        }

        //        if (raffle.WinningTicketsResult == null)
        //        {
        //            updateCommand.Parameters.AddWithValue("@WinningTicketsResult", DBNull.Value);
        //        }
        //        else
        //        {
        //            updateCommand.Parameters.AddWithValue("@WinningTicketsResult", raffle.WinningTicketsResult);
        //        }

        //        updateCommand.Parameters.AddWithValue("@CreateDate", raffle.CreateDate);
        //        updateCommand.Parameters.AddWithValue("@UpdateDate", raffle.UpdateDate);

        //        updateCommand.ExecuteNonQuery();
        //    }
        //}

        //public List<BetEntity> ReadBetsForRaffle(Guid raffleId)
        //{
        //    List<BetEntity> bets = new List<BetEntity>();

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        SqlDataAdapter adapter = new SqlDataAdapter();
        //        adapter.SelectCommand = new SqlCommand(SelectBetsForRaffleQuery, connection);

        //        adapter.SelectCommand.Parameters.AddWithValue("@RaffleId", raffleId);

        //        DataTable tableBets = new DataTable();
        //        adapter.Fill(tableBets);

        //        foreach (DataRow row in tableBets.Rows)
        //        {
        //            BetEntity bet = createBetFromDataRow(row);
        //            bets.Add(bet);
        //        }
        //    }

        //    return bets;
        //}

        //public void InsertBet(BetEntity bet)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        SqlCommand insertCommand = new SqlCommand(InsertBetQuery, connection);

        //        insertCommand.Parameters.AddWithValue("@RaffleId", bet.RaffleId);
        //        insertCommand.Parameters.AddWithValue("@TicketNumber", bet.TicketNumber);
        //        insertCommand.Parameters.AddWithValue("@BetNumber", bet.BetNumber);
        //        insertCommand.Parameters.AddWithValue("@SubmittedBy", bet.SubmittedBy);
        //        insertCommand.Parameters.AddWithValue("@CreateDate", bet.CreateDate);
        //        insertCommand.Parameters.AddWithValue("@UpdateDate", bet.UpdateDate);

        //        insertCommand.ExecuteNonQuery();
        //    }
        //}

        //public void ClearData()
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        SqlCommand commandClearBets = new SqlCommand(ClearBetsQuery, connection);
        //        commandClearBets.ExecuteNonQuery();

        //        SqlCommand commandClearRaffles = new SqlCommand(ClearRafflesQuery, connection);
        //        commandClearRaffles.ExecuteNonQuery();
        //    }
        //}


        //private BetEntity createBetFromDataRow(DataRow row)
        //{
        //    BetEntity bet = new BetEntity();

        //    bet.RaffleId = row.Field<Guid>(Constants.ColumnNames.RaffleId);
        //    bet.TicketNumber = row.Field<int>(Constants.ColumnNames.TicketNumber);
        //    bet.BetNumber = row.Field<int>(Constants.ColumnNames.BetNumber);
        //    bet.SubmittedBy = row.Field<string>(Constants.ColumnNames.SubmittedBy);
        //    bet.CreateDate = row.Field<DateTime>(Constants.ColumnNames.CreateDate);
        //    bet.UpdateDate = row.Field<DateTime>(Constants.ColumnNames.UpdateDate);

        //    return bet;
        //}
    }
}
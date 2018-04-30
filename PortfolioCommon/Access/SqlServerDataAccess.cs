using PortfolioCommon.Entities;
using PortfolioCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.Access
{
    public class SqlServerDataAccess : IPortfolioDataAccess
    {
        public SqlServerDataAccess()
        {
        }

        private const string _connectionString = @"Server=DESKTOP-SJ5O489;Database=NbuAltcoin;Integrated Security=True;";

        #region User queries
        private const string GetUserQuery = @"Select * from [dbo].[User] Where [Id] = @Id";
        private const string InsertUserQuery = @" INSERT INTO [dbo].[User] (Id, Name, Email, Password)
        VALUES(@Id, @Name, @Email, @Password)";
        private const string CheckExistingEmailQuery = @"
  SELECT COUNT(*)
  FROM [dbo].[User]
  WHERE Email = @Email";
        private const string GetUserPasswordByEmailQuery = @" SELECT Password
  FROM [dbo].[User]
  WHERE Email = @Email";
        private const string ClearUsersQuery = @"Delete from [dbo].[User]";
        #endregion

        #region Portfolio queries
        private const string SelectAllCoinsForUserPortfolioQuery = @"SELECT c.Id as Id,
PortfolioId,
c.Name as Name,
Symbol,
Rank,
Price_USD,
Amount
FROM [dbo].Coin c
INNER JOIN [dbo].[Portfolio] p ON p.Id = c.PortfolioId
INNER JOIN [dbo].[User] u ON p.UserId = u.Id
WHERE u.Email = @Email";
        private const string InsertPortfolioQuery = @"INSERT INTO [dbo].[Portfolio] (Id, UserId) VALUES (@Id, @UserId)";
        private const string GetUserPortfolioIdQuery = @"SELECT p.Id as Id FROM [dbo].[Portfolio] p
INNER JOIN [dbo].[User] u ON p.UserId = u.Id
WHERE u.Email = @Email";
        private const string InsertCoinQuery = @"INSERT INTO [dbo].[Coin] (SysId, Id, PortfolioId, Name, Symbol, Rank, Price_USD, Amount)
VALUES (@SysId, @Id, @PortfolioId, @Name, @Symbol, @Rank, @Price_USD, @Amount)";
        private const string ClearPortfolioQuery = @"Delete from [dbo].[Portfolio]";
        private const string PortfolioHasCoinQuery = @"SELECT COUNT(c.Id) FROM [dbo].Coin c 
INNER JOIN [dbo].[Portfolio] p ON p.Id = c.PortfolioId
WHERE p.Id = @PortfolioId 
AND c.id = @CoinId";
        private const string UpdateCoinAmountQuery = @"
UPDATE [dbo].[Coin]   
SET Amount = @Amount 
WHERE PortfolioId = @PortfolioId
AND Id = @CoinId";
        private const string UpdateCoinPriceQuery = @"
UPDATE [dbo].[Coin]   
SET Price_USD = @Price_USD 
WHERE PortfolioId = @PortfolioId
AND Id = @CoinId";
        private const string DeletePortfolioQuery = @"DELETE FROM COIN WHERE PortfolioId = @PortfolioId";
        #endregion

        #region User methods

        public string GetUserPassword(string email)
        {
            string pass = string.Empty;
            object result = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(GetUserPasswordByEmailQuery, connection);

                cmd.Parameters.AddWithValue("@Email", email);

                result = cmd.ExecuteScalar();
                if (result != null)
                {
                    pass = (string)result;
                }
                else
                {
                    return string.Empty;
                }
            }

            return pass;
        }

        private bool CheckEmailExists(string email)
        {
            var count = 0;
            object result = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(CheckExistingEmailQuery, connection);

                cmd.Parameters.AddWithValue("@Email", email);

                result = cmd.ExecuteScalar();
                if (result != null)
                {
                    count = (int)result;
                }
                else
                {
                    return false;
                }
            }
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RegisterUser(UserEntity user)
        {
            bool emailExists = this.CheckEmailExists(user.Email);
            if (!emailExists)
            {
                var userId = Guid.NewGuid();
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                string savedPasswordHash = Convert.ToBase64String(hashBytes);
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand insertCommand = new SqlCommand(InsertUserQuery, connection);

                    insertCommand.Parameters.AddWithValue("@Id", userId);
                    insertCommand.Parameters.AddWithValue("@Name", user.Name);
                    insertCommand.Parameters.AddWithValue("@Email", user.Email);
                    insertCommand.Parameters.AddWithValue("@Password", savedPasswordHash);

                    insertCommand.ExecuteNonQuery();
                }
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand insertCommand = new SqlCommand(InsertPortfolioQuery, connection);

                    insertCommand.Parameters.AddWithValue("@Id", Guid.NewGuid());
                    insertCommand.Parameters.AddWithValue("@UserId", userId);

                    insertCommand.ExecuteNonQuery();
                }
            }
            else
            {
                throw new Exception("Email alredy exists");
            }
        }

        public UserEntity GetUser(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(GetUserQuery, connection);

                adapter.SelectCommand.Parameters.AddWithValue("@Id", userId);

                DataTable tableUsers = new DataTable();
                adapter.Fill(tableUsers);

                if (tableUsers.Rows.Count == 0)
                {
                    return null;
                }

                DataRow row = tableUsers.Rows[0];

                UserEntity raffle = createUserFromDataRow(row);
                return raffle;
            }
        }
        #endregion

        #region Portfolio methods
        public List<CoinEntity> GetUserPortfolio(string email)
        {
            List<CoinEntity> coins = new List<CoinEntity>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(SelectAllCoinsForUserPortfolioQuery, connection);

                adapter.SelectCommand.Parameters.AddWithValue("@Email", email);
                DataTable tableCoins = new DataTable();
                adapter.Fill(tableCoins);

                foreach (DataRow row in tableCoins.Rows)
                {
                    CoinEntity coin = createCoinFromDataRow(row);
                    coins.Add(coin);
                }
            }

            return coins;
        }

        private Guid GetUserPortfolioId(string email)
        {
            Guid portfolioId = Guid.Empty;
            object result = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(GetUserPortfolioIdQuery, connection);

                cmd.Parameters.AddWithValue("@Email", email);

                result = cmd.ExecuteScalar();
                if (result != null)
                {
                    portfolioId = (Guid)result;
                }
            }

            return portfolioId;
        }
        
        private bool UserPortfolioAlreadyContainsCoin(Guid portfolioId, string coinId)
        {
            var count = 0;
            object result = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(PortfolioHasCoinQuery, connection);

                cmd.Parameters.AddWithValue("@PortfolioId", portfolioId);
                cmd.Parameters.AddWithValue("@CoinId", coinId);

                result = cmd.ExecuteScalar();
                if (result != null)
                {
                    count = (int)result;
                }
                else
                {
                    return false;
                }
            }
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateCoinPrice(string email, CoinEntity coin)
        {
            Guid portfolioId = this.GetUserPortfolioId(email);

            if (this.UserPortfolioAlreadyContainsCoin(portfolioId, coin.Id))
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand insertCommand = new SqlCommand(UpdateCoinPriceQuery, connection);

                    insertCommand.Parameters.AddWithValue("@CoinId", coin.Id);
                    insertCommand.Parameters.AddWithValue("@PortfolioId", portfolioId);
                    insertCommand.Parameters.AddWithValue("@Price_USD", coin.Price_USD);

                    insertCommand.ExecuteNonQuery();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void AddCoinToUserPortfolio(string email, CoinEntity coin)
        {
            Guid portfolioId = this.GetUserPortfolioId(email);

            if (this.UserPortfolioAlreadyContainsCoin(portfolioId, coin.Id))
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand insertCommand = new SqlCommand(UpdateCoinAmountQuery, connection);

                    insertCommand.Parameters.AddWithValue("@CoinId", coin.Id);
                    insertCommand.Parameters.AddWithValue("@PortfolioId", portfolioId);
                    insertCommand.Parameters.AddWithValue("@Amount", coin.Amount);

                    insertCommand.ExecuteNonQuery();
                }
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand insertCommand = new SqlCommand(InsertCoinQuery, connection);

                    insertCommand.Parameters.AddWithValue("@SysId", Guid.NewGuid());
                    insertCommand.Parameters.AddWithValue("@Id", coin.Id);
                    insertCommand.Parameters.AddWithValue("@PortfolioId", portfolioId);
                    insertCommand.Parameters.AddWithValue("@Name", coin.Name);
                    insertCommand.Parameters.AddWithValue("@Symbol", coin.Symbol);
                    insertCommand.Parameters.AddWithValue("@Rank", coin.Rank);
                    insertCommand.Parameters.AddWithValue("@Price_USD", coin.Price_USD);
                    insertCommand.Parameters.AddWithValue("@Amount", coin.Amount);

                    insertCommand.ExecuteNonQuery();
                }
            }            
        }

        public void DeleteUserPortfolio(string email)
        {
            Guid portfolioId = this.GetUserPortfolioId(email);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand insertCommand = new SqlCommand(DeletePortfolioQuery, connection);
                
                insertCommand.Parameters.AddWithValue("@PortfolioId", portfolioId);

                insertCommand.ExecuteNonQuery();
            }
        }
        #endregion

        #region General methods
        public void ClearData()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand commandClearBets = new SqlCommand(ClearUsersQuery, connection);
                commandClearBets.ExecuteNonQuery();

                SqlCommand commandClearRaffles = new SqlCommand(ClearPortfolioQuery, connection);
                commandClearRaffles.ExecuteNonQuery();
            }
        }
        #endregion

        #region Helpers
        private UserEntity createUserFromDataRow(DataRow row)
        {
            UserEntity user = new UserEntity();

            user.Id = row.Field<Guid>(Constants.UserColumnNames.Id);
            user.Email = row.Field<string>(Constants.UserColumnNames.Email);
            user.Name = row.Field<string>(Constants.UserColumnNames.Name);
            user.Password = row.Field<string>(Constants.UserColumnNames.Password);

            return user;
        }
        private CoinEntity createCoinFromDataRow(DataRow row)
        {
            CoinEntity coin = new CoinEntity();

            coin.Id = row.Field<string>(Constants.CoinColumnNames.Id);
            coin.PortfolioId = row.Field<Guid>(Constants.CoinColumnNames.PortfolioId);
            coin.Name = row.Field<string>(Constants.CoinColumnNames.Name);
            coin.Price_USD = row.Field<decimal>(Constants.CoinColumnNames.Price_USD);
            coin.Rank = row.Field<int>(Constants.CoinColumnNames.Rank);
            coin.Symbol = row.Field<string>(Constants.CoinColumnNames.Symbol);
            coin.Amount = row.Field<double>(Constants.CoinColumnNames.Amount);

            return coin;
        }
        #endregion       
    }
}


INSERT INTO [dbo].[User] (Id, Name, Email, Password)
VALUES('96ac53d0-018d-45b9-a990-a9514b99d3df', 'Georgi Georgiev', 'george_gig@abv.bg', 'Aabv123abv@')

SELECT COUNT(*)
FROM [dbo].[User]
WHERE Email = 'a@a.bg'

SELECT Password
FROM [dbo].[User]
WHERE Email = 'a@aaa.bq'

SELECT TOP (1000) *
FROM [dbo].[Portfolio]

INSERT INTO [dbo].[Portfolio] (Id, UserId) VALUES('b598f2bd-2684-4341-bffa-576384f5040c', '8C6D15C-ED90-4BCF-AF2A-3203342BA329')

--"id":"bitcoin",
--            "name":"Bitcoin",
--            "symbol":"BTC",
--            "rank":"1",
--            "price_usd":"14610.7",
--            "amount":"1"

SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 
CREATE TABLE [dbo].[Coin]( 
[Id] [nvarchar](50) NOT NULL, 
[PortfolioId] [uniqueidentifier] NOT NULL,
[Name] [nvarchar](50) NOT NULL, 
[Symbol] [nvarchar](5) NOT NULL, 
[Rank] [int],
[Price_USD] [decimal](30,10),
[Amount] [float],
CONSTRAINT [PK_Coin] PRIMARY KEY CLUSTERED 
( 
[Id] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ,
CONSTRAINT FK_Coin_Portfolio FOREIGN KEY (PortfolioId)     
    REFERENCES [dbo].[Portfolio] (Id)     
    ON DELETE CASCADE    
    ON UPDATE CASCADE    
) ON [PRIMARY] 
GO 

SELECT c.Id as Id,
PortfolioId,
c.Name as Name,
Symbol,
Rank,
Price_USD,
Amount
FROM [dbo].Coin c
INNER JOIN [dbo].[Portfolio] p ON p.Id = c.PortfolioId
INNER JOIN [dbo].[User] u ON p.UserId = u.Id
WHERE u.Email = 'george_gig@abv.bg'

INSERT INTO [dbo].[Portfolio] (Id, UserId) VALUES ()

SELECT * FROM COIN

DELETE FROM dbo.Coin

SELECT COUNT(c.Id) FROM [dbo].Coin c 
INNER JOIN [dbo].[Portfolio] p ON p.Id = c.PortfolioId
WHERE p.Id = @PortfolioId 

UPDATE [dbo].[Coin]   
SET Amount = @Amount 
WHERE PortfolioId = @PortfolioId
AND Id = @CoinId

SELECT TOP (1000) *
FROM [dbo].[User]

SELECT TOP (1000) *
FROM [dbo].[Portfolio]

SELECT TOP (1000) *
FROM [dbo].[Coin]
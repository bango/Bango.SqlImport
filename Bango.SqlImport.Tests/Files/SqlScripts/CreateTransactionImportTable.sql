/****** Object:  Table [dbo].[TransactionImport]    Script Date: 22/10/2018 15:53:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TransactionImport](
	[TranId] [VARCHAR](200) NOT NULL,
	[TranAmount] [DECIMAL](18, 0) NOT NULL,
	[TranDate] [DATETIME2](7) NOT NULL,
	[CurrencyIso3] [CHAR](3) NOT NULL,
	[Success] [BIT] NOT NULL,
	[ImportId] [INT] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
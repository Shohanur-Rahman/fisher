USE [DB_A443D3_newfiser]
GO
/****** Object:  UserDefinedFunction [dbo].[MyHTMLDecode]    Script Date: 1/30/2019 11:58:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[MyHTMLDecode] (@vcWhat nVARCHAR(MAX))
RETURNS nVARCHAR(MAX)
AS
BEGIN
    DECLARE @vcResult nVARCHAR(MAX)
    DECLARE @siPos INT
        ,@vcEncoded nVARCHAR(7)
        ,@siChar INT

    SET @vcResult = RTRIM(LTRIM(CAST(REPLACE(@vcWhat COLLATE Latin1_General_BIN, CHAR(0), '') AS nVARCHAR(MAX))))

    SELECT @vcResult = REPLACE(REPLACE(@vcResult, '&#160;', ' '), '&nbsp;', ' ')

    IF @vcResult = ''
        RETURN @vcResult

    SELECT @siPos = PATINDEX('%&#[0-9][0-9][0-9];%', @vcResult)

    WHILE @siPos > 0
    BEGIN
        SELECT @vcEncoded = SUBSTRING(@vcResult, @siPos, 6)
            ,@siChar = CAST(SUBSTRING(@vcEncoded, 3, 3) AS INT)
            ,@vcResult = REPLACE(@vcResult, @vcEncoded, NCHAR(@siChar))
            ,@siPos = PATINDEX('%&#[0-9][0-9][0-9];%', @vcResult)
    END

    SELECT @siPos = PATINDEX('%&#[0-9][0-9][0-9][0-9];%', @vcResult)

    WHILE @siPos > 0
    BEGIN
        SELECT @vcEncoded = SUBSTRING(@vcResult, @siPos, 7)
            ,@siChar = CAST(SUBSTRING(@vcEncoded, 3, 4) AS INT)
            ,@vcResult = REPLACE(@vcResult, @vcEncoded, NCHAR(@siChar))
            ,@siPos = PATINDEX('%&#[0-9][0-9][0-9][0-9];%', @vcResult)
    END

    SELECT @siPos = PATINDEX('%#[0-9][0-9][0-9][0-9]%', @vcResult)

    WHILE @siPos > 0
    BEGIN
        SELECT @vcEncoded = SUBSTRING(@vcResult, @siPos, 5)
            ,@vcResult = REPLACE(@vcResult, @vcEncoded, '')
            ,@siPos = PATINDEX('%#[0-9][0-9][0-9][0-9]%', @vcResult)
    END

    SELECT @vcResult = REPLACE(REPLACE(@vcResult, NCHAR(160), ' '), CHAR(160), ' ')

    SELECT @vcResult = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@vcResult, '&amp;', '&'), '&quot;', '"'), '&lt;', '<'), '&gt;', '>'), '&amp;amp;', '&')

    RETURN @vcResult
END



GO
/****** Object:  UserDefinedFunction [dbo].[udf_StripHTML]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[udf_StripHTML] (@HTMLText nVARCHAR(MAX))
RETURNS nVARCHAR(MAX) AS
BEGIN
    DECLARE @Start INT
    DECLARE @End INT
    DECLARE @Length INT
    SET @Start = CHARINDEX('<',@HTMLText)
    SET @End = CHARINDEX('>',@HTMLText,CHARINDEX('<',@HTMLText))
    SET @Length = (@End - @Start) + 1
    WHILE @Start > 0 AND @End > 0 AND @Length > 0
    BEGIN
        SET @HTMLText = STUFF(@HTMLText,@Start,@Length,'')
        SET @Start = CHARINDEX('<',@HTMLText)
        SET @End = CHARINDEX('>',@HTMLText,CHARINDEX('<',@HTMLText))
        SET @Length = (@End - @Start) + 1
    END
    RETURN LTRIM(RTRIM(@HTMLText))
END



GO
/****** Object:  UserDefinedFunction [dbo].[uf_AddThousandSeparators]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_AddThousandSeparators](@NumStr nvarchar(50))
RETURNS nVarchar(50)
AS
BEGIN
declare @OutStr nVarchar(50)
declare @i int
declare @run int

Select @i=CHARINDEX('.',@NumStr)
if @i=0 
    begin
    set @i=LEN(@NumStr)
    Set @Outstr=''
    end
else
    begin   
     Set @Outstr=SUBSTRING(@NUmStr,@i,50)
     Set @i=@i -1
    end 


Set @run=0

While @i>0
    begin
      if @Run=3
        begin
          Set @Outstr=','+@Outstr
          Set @run=0
        end
      Set @Outstr=SUBSTRING(@NumStr,@i,1) +@Outstr  
      Set @i=@i-1
      Set @run=@run + 1     
    end

    RETURN @OutStr

END



GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetFeedCategorynameByFeedCategoryId]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_GetFeedCategorynameByFeedCategoryId] 
(
	@id int
	
)
RETURNS nvarchar(max)
AS
BEGIN  
   DECLARE @Result AS nvarchar(max)

   SELECT @Result = [FeedCategoryName] from [dbo].[FeedCategories] where [FeedCategoryId] = @id;
   
   IF @Result IS NULL SET @Result = 'Not Found'
      
   RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId] 
(
	@feedId int,
	@catId int
	
)
RETURNS nvarchar(max)
AS
BEGIN  
   DECLARE @Result AS nvarchar(max)
   DECLARE @feedName AS nvarchar(max)
   DECLARE @categoryName AS nvarchar(max)

   SELECT @feedName = [Name] from [dbo].[Feed] where [ID] = @feedId;
   IF @feedName IS NULL or @feedName = '' SET @feedName = 'Not Found'

   SELECT @categoryName = [FeedCategoryName] from [dbo].[FeedCategories] where [FeedCategoryId] = @catId;
   IF @categoryName IS NULL or @categoryName = '' SET @categoryName = 'Not Found'

   SET @Result = @feedName + ' ('+ @categoryName + ')'

   IF @Result IS NULL SET @Result = 'Not Found'
      
   RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetFishNameByFishId]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_GetFishNameByFishId] 
(
	@fishId int
	
)
RETURNS nvarchar(max)
AS
BEGIN  
   DECLARE @Result AS nvarchar(max)

   SELECT @Result = [Name] from [dbo].[Fish] where [ID] = @fishId;
   
   IF @Result IS NULL SET @Result = 'Not Found'
      
   RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetFishSellerDueAmount]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_GetFishSellerDueAmount] 
(
	@projectid int,
	@SellDate as datetime,
	@FishSellerId as int
	
)
RETURNS Decimal(18,2)
AS
BEGIN  
   DECLARE @Result AS Decimal(18,2);
   SELECT @Result = sum (c.FishAmountDue)  from FishSellingReport c where projectid=@projectid and SellDate=@SellDate and FishSellerId=@FishSellerId;
   
   IF @Result IS NULL SET @Result = 0
      
   RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetFishSellerNameByFishId]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[uf_GetFishSellerNameByFishId] 
(
	@fishSellerId int
	
)
RETURNS nvarchar(max)
AS
BEGIN  
   DECLARE @Result AS nvarchar(max)

   SELECT @Result = [Name] from [dbo].[FishSeller] where [ID] = @fishSellerId;
   
   IF @Result IS NULL SET @Result = 'Not Found'
      
   RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetFishSellerNameByFishShellerId]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[uf_GetFishSellerNameByFishShellerId] 
(
	@fishSellerId int
	
)
RETURNS nvarchar(max)
AS
BEGIN  
   DECLARE @Result AS nvarchar(max)

   SELECT @Result = [Name] from [dbo].[FishSeller] where [ID] = @fishSellerId;
   
   IF @Result IS NULL SET @Result = 'Not Found'
      
   RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetFishSellerPaidAmount]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_GetFishSellerPaidAmount] 
(
	@projectid int,
	@SellDate as datetime,
	@FishSellerId as int
	
)
RETURNS Decimal(18,2)
AS
BEGIN  
   DECLARE @Result AS Decimal(18,2);
   SELECT @Result = sum (c.FishAmountPaid)  from FishSellingReport c where projectid=@projectid and SellDate=@SellDate and FishSellerId=@FishSellerId;
   
   IF @Result IS NULL SET @Result = 0
      
   RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetFishSellerTotalAmount]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_GetFishSellerTotalAmount] 
(
	@projectid int,
	@SellDate as datetime,
	@FishSellerId as int
	
)
RETURNS Decimal(18,2)
AS
BEGIN  
   DECLARE @Result AS Decimal(18,2);
   SELECT @Result = sum (c.TotalSellPrice)  from FishSellingReport c where projectid=@projectid and SellDate=@SellDate and FishSellerId=@FishSellerId;
   
   IF @Result IS NULL SET @Result = 0
      
   RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetProjectNameByProjectId]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_GetProjectNameByProjectId] 
(
	@id int
	
)
RETURNS nvarchar(max)
AS
BEGIN  
   DECLARE @Result AS nvarchar(max)

   SELECT @Result = [Name] from [dbo].[Project] where [ID] = @id;
   
   IF @Result IS NULL SET @Result = 'Not Found'
      
   RETURN @Result
END


GO
/****** Object:  UserDefinedFunction [dbo].[uf_GetUserProjectNameByAreaIdAndProjectId]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_GetUserProjectNameByAreaIdAndProjectId] 
(
	@areaId int,
	@projectId int
	
)
RETURNS nvarchar(max)
AS
BEGIN  
   DECLARE @Result AS nvarchar(max)
   DECLARE @areaName AS nvarchar(max)  
   DECLARE @projectName AS nvarchar(max)
   
   SELECT @areaName = [Name] from [dbo].[Area] where [ID] = @areaId;
   SELECT @projectName = [Name] from [dbo].[Area] where [ID] = @projectId;
      
	  IF @areaName IS not null and @projectName is not null
		SET @Result = @areaName + ' (' + @projectName + ')'

	IF @areaName IS null and @projectName is not null
		SET @Result =  @projectName

	IF @areaName IS not null and @projectName is null
		SET @Result = @areaName 

   IF @Result IS NULL SET @Result = 'Not Found'
      
   RETURN @Result
END


GO
/****** Object:  Table [dbo].[Area]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Union_Name] [nvarchar](500) NULL,
	[WardNumber] [nvarchar](500) NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Area] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FarmRentalReports]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FarmRentalReports](
	[FarmRentalReportID] [int] IDENTITY(1,1) NOT NULL,
	[FarmRentalReportName] [nvarchar](max) NOT NULL,
	[FarmRentalProjectID] [int] NOT NULL,
	[FarmRentalDetails] [nvarchar](max) NOT NULL,
	[FarmRentalLandAmount] [decimal](18, 2) NOT NULL,
	[FarmRentalServieFee] [decimal](18, 2) NOT NULL,
	[FarmRentalMainFee] [decimal](18, 2) NOT NULL,
	[FarmRentalTotalFee] [decimal](18, 2) NOT NULL,
	[FarmRentalCostPerAmount] [decimal](18, 2) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [date] NULL,
	[CreatedID] [int] NULL,
	[EditedDate] [date] NULL,
	[EditedID] [int] NULL,
	[FarmRentalDate] [date] NOT NULL,
 CONSTRAINT [PK_FarmRentalReports] PRIMARY KEY CLUSTERED 
(
	[FarmRentalReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Feed]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feed](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Feed] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FeedCategories]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedCategories](
	[FeedCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[FeedCategoryName] [nvarchar](max) NOT NULL,
	[FeedCategoryDetails] [nvarchar](max) NOT NULL,
	[FeedCategoryImageUrl] [nvarchar](max) NOT NULL,
	[CreatedDate] [date] NULL,
	[CreatedId] [int] NULL,
	[FeedCategoryFeedId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_FeedCategories] PRIMARY KEY CLUSTERED 
(
	[FeedCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FeedDistributionReports]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedDistributionReports](
	[FeedDistributionReportId] [int] IDENTITY(1,1) NOT NULL,
	[FeedDistributionName] [nvarchar](max) NOT NULL,
	[FeedDistributionDate] [date] NOT NULL,
	[FeedDistributionFeedId] [int] NOT NULL,
	[FeedDistributionFeedategoryId] [int] NOT NULL,
	[FeedDistributionQuantityId] [int] NOT NULL,
	[FeedDistributionSakNumber] [decimal](18, 2) NOT NULL,
	[FeedDistributionTotalWeight] [decimal](18, 2) NOT NULL,
	[CreatedId] [int] NULL,
	[CreatedDate] [date] NULL,
	[EditedId] [int] NULL,
	[EditedDate] [date] NULL,
	[FeedDistributionDescription] [nvarchar](max) NULL,
	[AreaId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_FeedDistributionReports] PRIMARY KEY CLUSTERED 
(
	[FeedDistributionReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FeedSellingReport]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedSellingReport](
	[FeedSellingReportId] [int] IDENTITY(1,1) NOT NULL,
	[SellingFeedReportNumber] [nvarchar](max) NOT NULL,
	[SellingReportName] [nvarchar](max) NOT NULL,
	[SellingReportFeedCategoryId] [int] NOT NULL,
	[SellingFeedAreaId] [int] NOT NULL,
	[SellingFeedProjectId] [int] NOT NULL,
	[SellingFeedWeightPerSack] [int] NOT NULL,
	[SellingFeedSackNumber] [decimal](18, 0) NOT NULL,
	[SellingFeedTotalWeight] [decimal](18, 0) NOT NULL,
	[SellingFeedPricePerKg] [decimal](18, 0) NOT NULL,
	[SellingFeedTotalPrice] [decimal](18, 0) NOT NULL,
	[SellingFeedCalculationDate] [date] NOT NULL,
	[SellingFeedCreateDate] [date] NULL,
	[SellingFeedCreatedId] [int] NULL,
	[SellingFeedEditedDate] [date] NULL,
	[SellignFeedEditedId] [int] NULL,
	[SellingFeedSellNote] [nvarchar](max) NULL,
	[FishSellingIsActive] [bit] NULL,
	[FeedId] [int] NOT NULL,
 CONSTRAINT [PK_FeedSellingReport] PRIMARY KEY CLUSTERED 
(
	[FeedSellingReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Fish]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fish](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedId] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Fish] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FishSeller]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FishSeller](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Age] [int] NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedId] [int] NULL,
	[Description] [nvarchar](max) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_FishSeller] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FishSellingReport]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FishSellingReport](
	[FishSellId] [int] IDENTITY(1,1) NOT NULL,
	[SellingFishName] [nvarchar](max) NOT NULL,
	[TotalSellInKG] [decimal](18, 2) NOT NULL,
	[SellDate] [date] NOT NULL,
	[SellFishId] [int] NOT NULL,
	[PiecesPerKG] [int] NOT NULL,
	[TotalPiecesFish] [int] NULL,
	[PricePerKG] [decimal](18, 2) NOT NULL,
	[TotalSellPrice] [decimal](18, 2) NOT NULL,
	[FishImageUrl] [nvarchar](max) NOT NULL,
	[CalculatedDate] [date] NULL,
	[CalculatedById] [int] NULL,
	[CalculationEditedDate] [date] NULL,
	[CalCulationEditedId] [int] NULL,
	[SellNote] [nvarchar](max) NULL,
	[AreaId] [int] NULL,
	[ProjectId] [int] NULL,
	[FishSellerId] [int] NOT NULL,
	[FishAmountPaid] [decimal](18, 2) NOT NULL,
	[FishAmountDue] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_FishSellingReport] PRIMARY KEY CLUSTERED 
(
	[FishSellId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FishSellingReportFishMaperTable]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FishSellingReportFishMaperTable](
	[FishSellingReportFishMaperTableID] [int] IDENTITY(1,1) NOT NULL,
	[FishSellingReportFishMaperTableFishId] [int] NOT NULL,
	[FishSellingReportFishMaperTableCalculationReportId] [int] NOT NULL,
 CONSTRAINT [PK_FishSellingReportFishMaperTable] PRIMARY KEY CLUSTERED 
(
	[FishSellingReportFishMaperTableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Project](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[AreaId] [int] NOT NULL,
	[Land] [nvarchar](max) NULL,
	[Cost] [nvarchar](max) NULL,
	[Time] [nvarchar](max) NULL,
	[ImageUrl] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedId] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/30/2019 11:58:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [bigint] IDENTITY(1,1) NOT NULL,
	[AddressOne] [nvarchar](200) NULL,
	[AddressTwo] [nvarchar](200) NULL,
	[City] [nvarchar](100) NULL,
	[ConfirmPassword] [nvarchar](100) NULL,
	[Country] [nvarchar](100) NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[CurrentPassword] [nvarchar](max) NULL,
	[AreaID] [int] NULL,
	[EditedBy] [bigint] NULL,
	[EditedDate] [datetime] NULL,
	[EmailAddress] [nvarchar](200) NULL,
	[FirstName] [nvarchar](100) NULL,
	[IsActivated] [bit] NOT NULL,
	[IsTemporaryPassword] [bit] NULL,
	[LastName] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Position] [nvarchar](100) NULL,
	[PostCode] [nvarchar](50) NULL,
	[PublicationStatus] [bit] NULL,
	[Religion] [nvarchar](100) NULL,
	[RegistrationConfirmed] [nvarchar](50) NULL,
	[Role] [nvarchar](200) NULL,
	[UserImageCaption] [nvarchar](max) NULL,
	[UserImagePath] [nvarchar](max) NULL,
	[UserImageSize] [bigint] NULL,
	[UserName] [nvarchar](50) NULL,
	[UserRegistartionGuid] [nvarchar](max) NULL,
	[IsTrialUser] [bit] NULL,
	[TrialStartDate] [datetime] NULL,
	[TrialEndDate] [datetime] NULL,
	[IsImageUploadedByUser] [bit] NOT NULL,
	[UserCreatedBy] [nvarchar](200) NOT NULL,
	[CreatedIP] [nvarchar](500) NULL,
	[EditedIP] [nvarchar](500) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Area] ON 

INSERT [dbo].[Area] ([ID], [Name], [Union_Name], [WardNumber], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (4, N'চা বাগান 1', N'চা বাগান', N'9', N'Uploads/CreateSegment/Area/d8fa3110-b0b7-4876-95b5-5ac3c5b3fd32-62j2yq8kGt.jpg', CAST(N'2018-10-31 12:49:06.157' AS DateTime), 1, 0)
INSERT [dbo].[Area] ([ID], [Name], [Union_Name], [WardNumber], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (5, N'চা বাগান 2', N'চা বাগান', N'চা বাগান', N'Uploads/CreateSegment/Area/5e1deef7-d77f-4b7a-8dc0-579568d851b2-17021774_1769087536743100_659452363095289705_n.jpg', CAST(N'2018-10-31 12:49:42.540' AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[Area] OFF
SET IDENTITY_INSERT [dbo].[FarmRentalReports] ON 

INSERT [dbo].[FarmRentalReports] ([FarmRentalReportID], [FarmRentalReportName], [FarmRentalProjectID], [FarmRentalDetails], [FarmRentalLandAmount], [FarmRentalServieFee], [FarmRentalMainFee], [FarmRentalTotalFee], [FarmRentalCostPerAmount], [IsDeleted], [CreatedDate], [CreatedID], [EditedDate], [EditedID], [FarmRentalDate]) VALUES (1, N'চান্দারক্ষেত', 3, N'বিশ্ব সুন্দরী প্রতিযোগিতায় অংশ নিয়ে আলোচনায় আসা জান্নাতুল ফেরদৌসি ঐশী চার সপ্তাহ হয় দেশে ফিরেছেন। দেশে ফেরার পর তাঁকে নিয়ে কেউ কেউ নাটক, চলচ্চিত্র ও বিজ্ঞাপনচিত্র বানাতে চেয়েছিলেন। পরিচালক আর পণ্য প্রতিষ্ঠানগুলোও ঐশীর জনপ্রিয়তাকে কাজ লাগাতে চাইছিল। এদিকে ঐশী-ভক্তরাও অধীর আগ্রহে অপেক্ষা করছিলেন, কোন মাধ্যমে অভিষেক হচ্ছে সাম্প্রতিক সময়ে আলোচনায় আসা ঐশীর। শেষ পর্যন্ত জানা গেল, একটি পোশাক প্রস্তুতকারী প্রতিষ্ঠানে স্থিরচিত্রের মডেল হওয়ার মধ্য দিয়ে পেশাদারভাবে যাত্রা শুরু হয়েছে তাঁর।&amp;nbsp;', CAST(1282.00 AS Decimal(18, 2)), CAST(84500.00 AS Decimal(18, 2)), CAST(1416403.00 AS Decimal(18, 2)), CAST(1500903.00 AS Decimal(18, 2)), CAST(1170.75 AS Decimal(18, 2)), 1, CAST(N'2019-01-07' AS Date), 1, CAST(N'2019-01-07' AS Date), 1, CAST(N'2019-01-01' AS Date))
INSERT [dbo].[FarmRentalReports] ([FarmRentalReportID], [FarmRentalReportName], [FarmRentalProjectID], [FarmRentalDetails], [FarmRentalLandAmount], [FarmRentalServieFee], [FarmRentalMainFee], [FarmRentalTotalFee], [FarmRentalCostPerAmount], [IsDeleted], [CreatedDate], [CreatedID], [EditedDate], [EditedID], [FarmRentalDate]) VALUES (2, N'মধ্যমণি', 5, N'বিশ্ব সুন্দরী প্রতিযোগিতায় অংশ নিয়ে আলোচনায় আসা জান্নাতুল ফেরদৌসি ঐশী চার সপ্তাহ হয় দেশে ফিরেছেন। দেশে ফেরার পর তাঁকে নিয়ে কেউ কেউ নাটক, চলচ্চিত্র ও বিজ্ঞাপনচিত্র বানাতে চেয়েছিলেন। পরিচালক আর পণ্য প্রতিষ্ঠানগুলোও ঐশীর জনপ্রিয়তাকে কাজ লাগাতে চাইছিল। এদিকে ঐশী-ভক্তরাও অধীর আগ্রহে অপেক্ষা করছিলেন, কোন মাধ্যমে অভিষেক হচ্ছে সাম্প্রতিক সময়ে আলোচনায় আসা ঐশীর। শেষ পর্যন্ত জানা গেল, একটি পোশাক প্রস্তুতকারী প্রতিষ্ঠানে স্থিরচিত্রের মডেল হওয়ার মধ্য দিয়ে পেশাদারভাবে যাত্রা শুরু হয়েছে তাঁর।&amp;nbsp;', CAST(10032.00 AS Decimal(18, 2)), CAST(443000.00 AS Decimal(18, 2)), CAST(1179428.00 AS Decimal(18, 2)), CAST(1622428.00 AS Decimal(18, 2)), CAST(161.73 AS Decimal(18, 2)), 0, CAST(N'2019-01-07' AS Date), 1, NULL, NULL, CAST(N'2019-01-05' AS Date))
INSERT [dbo].[FarmRentalReports] ([FarmRentalReportID], [FarmRentalReportName], [FarmRentalProjectID], [FarmRentalDetails], [FarmRentalLandAmount], [FarmRentalServieFee], [FarmRentalMainFee], [FarmRentalTotalFee], [FarmRentalCostPerAmount], [IsDeleted], [CreatedDate], [CreatedID], [EditedDate], [EditedID], [FarmRentalDate]) VALUES (3, N'জাবিরাগাড়া', 5, N'বিশ্ব সুন্দরী প্রতিযোগিতায় অংশ নিয়ে আলোচনায় আসা জান্নাতুল ফেরদৌসি ঐশী চার সপ্তাহ হয় দেশে ফিরেছেন। দেশে ফেরার পর তাঁকে নিয়ে কেউ কেউ নাটক, চলচ্চিত্র ও বিজ্ঞাপনচিত্র বানাতে চেয়েছিলেন। পরিচালক আর পণ্য প্রতিষ্ঠানগুলোও ঐশীর জনপ্রিয়তাকে কাজ লাগাতে চাইছিল। এদিকে ঐশী-ভক্তরাও অধীর আগ্রহে অপেক্ষা করছিলেন, কোন মাধ্যমে অভিষেক হচ্ছে সাম্প্রতিক সময়ে আলোচনায় আসা ঐশীর। শেষ পর্যন্ত জানা গেল, একটি পোশাক প্রস্তুতকারী প্রতিষ্ঠানে স্থিরচিত্রের মডেল হওয়ার মধ্য দিয়ে পেশাদারভাবে যাত্রা শুরু হয়েছে তাঁর।&amp;nbsp;', CAST(4387.00 AS Decimal(18, 2)), CAST(50000.00 AS Decimal(18, 2)), CAST(4512342.00 AS Decimal(18, 2)), CAST(4562342.00 AS Decimal(18, 2)), CAST(1039.97 AS Decimal(18, 2)), 0, CAST(N'2019-01-07' AS Date), 1, NULL, NULL, CAST(N'2019-01-03' AS Date))
INSERT [dbo].[FarmRentalReports] ([FarmRentalReportID], [FarmRentalReportName], [FarmRentalProjectID], [FarmRentalDetails], [FarmRentalLandAmount], [FarmRentalServieFee], [FarmRentalMainFee], [FarmRentalTotalFee], [FarmRentalCostPerAmount], [IsDeleted], [CreatedDate], [CreatedID], [EditedDate], [EditedID], [FarmRentalDate]) VALUES (4, N'সূর্যোদয়', 5, N'বিশ্ব সুন্দরী প্রতিযোগিতায় অংশ নিয়ে আলোচনায় আসা জান্নাতুল ফেরদৌসি ঐশী চার সপ্তাহ হয় দেশে ফিরেছেন। দেশে ফেরার পর তাঁকে নিয়ে কেউ কেউ নাটক, চলচ্চিত্র ও বিজ্ঞাপনচিত্র বানাতে চেয়েছিলেন। পরিচালক আর পণ্য প্রতিষ্ঠানগুলোও ঐশীর জনপ্রিয়তাকে কাজ লাগাতে চাইছিল। এদিকে ঐশী-ভক্তরাও অধীর আগ্রহে অপেক্ষা করছিলেন, কোন মাধ্যমে অভিষেক হচ্ছে সাম্প্রতিক সময়ে আলোচনায় আসা ঐশীর। শেষ পর্যন্ত জানা গেল, একটি পোশাক প্রস্তুতকারী প্রতিষ্ঠানে স্থিরচিত্রের মডেল হওয়ার মধ্য দিয়ে পেশাদারভাবে যাত্রা শুরু হয়েছে তাঁর।&amp;nbsp;', CAST(8745.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), CAST(15478945.00 AS Decimal(18, 2)), CAST(15566445.00 AS Decimal(18, 2)), CAST(1780.04 AS Decimal(18, 2)), 0, CAST(N'2019-01-07' AS Date), 1, NULL, NULL, CAST(N'2019-01-04' AS Date))
INSERT [dbo].[FarmRentalReports] ([FarmRentalReportID], [FarmRentalReportName], [FarmRentalProjectID], [FarmRentalDetails], [FarmRentalLandAmount], [FarmRentalServieFee], [FarmRentalMainFee], [FarmRentalTotalFee], [FarmRentalCostPerAmount], [IsDeleted], [CreatedDate], [CreatedID], [EditedDate], [EditedID], [FarmRentalDate]) VALUES (5, N'জোরা পুকুর', 6, N'বিশ্ব সুন্দরী প্রতিযোগিতায় অংশ নিয়ে আলোচনায় আসা জান্নাতুল ফেরদৌসি ঐশী চার সপ্তাহ হয় দেশে ফিরেছেন। দেশে ফেরার পর তাঁকে নিয়ে কেউ কেউ নাটক, চলচ্চিত্র ও বিজ্ঞাপনচিত্র বানাতে চেয়েছিলেন। পরিচালক আর পণ্য প্রতিষ্ঠানগুলোও ঐশীর জনপ্রিয়তাকে কাজ লাগাতে চাইছিল। এদিকে ঐশী-ভক্তরাও অধীর আগ্রহে অপেক্ষা করছিলেন, কোন মাধ্যমে অভিষেক হচ্ছে সাম্প্রতিক সময়ে আলোচনায় আসা ঐশীর। শেষ পর্যন্ত জানা গেল, একটি পোশাক প্রস্তুতকারী প্রতিষ্ঠানে স্থিরচিত্রের মডেল হওয়ার মধ্য দিয়ে পেশাদারভাবে যাত্রা শুরু হয়েছে তাঁর।&amp;nbsp;', CAST(9874.00 AS Decimal(18, 2)), CAST(65784.00 AS Decimal(18, 2)), CAST(5987452.00 AS Decimal(18, 2)), CAST(6053236.00 AS Decimal(18, 2)), CAST(613.05 AS Decimal(18, 2)), 0, CAST(N'2019-01-07' AS Date), 1, NULL, NULL, CAST(N'2019-01-06' AS Date))
INSERT [dbo].[FarmRentalReports] ([FarmRentalReportID], [FarmRentalReportName], [FarmRentalProjectID], [FarmRentalDetails], [FarmRentalLandAmount], [FarmRentalServieFee], [FarmRentalMainFee], [FarmRentalTotalFee], [FarmRentalCostPerAmount], [IsDeleted], [CreatedDate], [CreatedID], [EditedDate], [EditedID], [FarmRentalDate]) VALUES (6, N'ঘুলা পুকুর', 5, N'বিশ্ব সুন্দরী প্রতিযোগিতায় অংশ নিয়ে আলোচনায় আসা জান্নাতুল ফেরদৌসি ঐশী চার সপ্তাহ হয় দেশে ফিরেছেন। দেশে ফেরার পর তাঁকে নিয়ে কেউ কেউ নাটক, চলচ্চিত্র ও বিজ্ঞাপনচিত্র বানাতে চেয়েছিলেন। পরিচালক আর পণ্য প্রতিষ্ঠানগুলোও ঐশীর জনপ্রিয়তাকে কাজ লাগাতে চাইছিল। এদিকে ঐশী-ভক্তরাও অধীর আগ্রহে অপেক্ষা করছিলেন, কোন মাধ্যমে অভিষেক হচ্ছে সাম্প্রতিক সময়ে আলোচনায় আসা ঐশীর। শেষ পর্যন্ত জানা গেল, একটি পোশাক প্রস্তুতকারী প্রতিষ্ঠানে স্থিরচিত্রের মডেল হওয়ার মধ্য দিয়ে পেশাদারভাবে যাত্রা শুরু হয়েছে তাঁর।&amp;nbsp;', CAST(85432.00 AS Decimal(18, 2)), CAST(6587.00 AS Decimal(18, 2)), CAST(2587454.00 AS Decimal(18, 2)), CAST(2594041.00 AS Decimal(18, 2)), CAST(30.36 AS Decimal(18, 2)), 0, CAST(N'2019-01-07' AS Date), 1, NULL, NULL, CAST(N'2019-01-02' AS Date))
SET IDENTITY_INSERT [dbo].[FarmRentalReports] OFF
SET IDENTITY_INSERT [dbo].[Feed] ON 

INSERT [dbo].[Feed] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (2, N'রূপসী', N'রুপসী বাংলা টিভি ইউটিউব চ্যানেলে বাংলা ভাষায় বিভিন্ন বিষয় সম্পর্কে আলোচনা ও টিপস দেওয়া হয়ে থাকে। যেখানে সারাবিশ্ব তাদের ভাষাকে এগিয়ে নিয়ে যাচ্ছে সেখানে আমরা', N'Uploads/CreateSegment/Feed/c48bfe8a-ab12-4866-b263-2112d9a19eaf-49896089_1998696470224170_3530142991924592640_n.jpg', CAST(N'2018-10-29 22:16:08.093' AS DateTime), 1, 0)
INSERT [dbo].[Feed] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (4, N'মেগা', N'জাতীয় অর্থনৈতিক পরিষদের নির্বাহী কমিটির (একনেক) সভায় কুমিল্লা বিশ্ববিদ্যালয়ের ১৬৫৫ কোটি ৫০ লাখ টাকার মেগা প্রকল্পের অনুমোদন দেওয়া হয়েছে। মঙ্গলবার (২৩ অক্টোবর) রাজধানীর শেরে বাংলা নগরে এনইসি সম্মেলন কক্ষে একনেক চেয়ারপারসন ও প্রধানমন্ত্রী শেখ হাসিনার সভাপতিত্বে একনেক সভায় এ অনুমোদন দেওয়া', N'Uploads/CreateSegment/Feed/c4adbea1-3533-46cd-b424-386d9ae516c3-6.jpg', CAST(N'2018-10-31 11:30:59.337' AS DateTime), 1, 0)
INSERT [dbo].[Feed] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (6, N'রূপসী 2', N'জাতীয় অর্থনৈতিক পরিষদের নির্বাহী কমিটির (একনেক) সভায় কুমিল্লা বিশ্ববিদ্যালয়ের ১৬৫৫ কোটি ৫০ লাখ টাকার মেগা প্রকল্পের অনুমোদন দেওয়া হয়েছে। মঙ্গলবার (২৩ অক্টোবর) রাজধানীর শেরে বাংলা নগরে এনইসি সম্মেলন কক্ষে একনেক চেয়ারপারসন ও প্রধানমন্ত্রী শেখ হাসিনার সভাপতিত্বে একনেক সভায় এ অনুমোদন দেওয়া', N'Uploads/CreateSegment/Feed/51a4b708-045a-4d03-9f90-da916e001ba5-100px-Rupasi_bangla.png', CAST(N'2018-12-21 00:05:54.647' AS DateTime), 1, 0)
INSERT [dbo].[Feed] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (7, N'sfgs', N'fgsfgsfg', N'Uploads/CreateSegment/Feed/36c498db-ca09-4f67-b7b7-7fc80f1295df-pp6.png', CAST(N'2018-12-21 00:07:12.090' AS DateTime), 1, 0)
INSERT [dbo].[Feed] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (8, N'Test TEst', N'asdasdasd adsdasdasd', N'Uploads/CreateSegment/Feed/6b425fae-f1de-4d4d-a15a-ff4c4bd07178-image.png', CAST(N'2019-01-06 21:27:09.693' AS DateTime), 1, 0)
INSERT [dbo].[Feed] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (10, N'asda asdasdas', N'dasd asdasdasd', N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-06 21:27:32.243' AS DateTime), 1, 0)
INSERT [dbo].[Feed] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (11, N'কাজি ফিনিশার', N'fghd dg dg hdgh', N'Uploads/CreateSegment/Feed/1977fd6a-9d6e-400d-a5b4-c61637687240-49896089_1998696470224170_3530142991924592640_n.jpg', CAST(N'2019-01-07 19:30:10.000' AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[Feed] OFF
SET IDENTITY_INSERT [dbo].[FeedCategories] ON 

INSERT [dbo].[FeedCategories] ([FeedCategoryId], [FeedCategoryName], [FeedCategoryDetails], [FeedCategoryImageUrl], [CreatedDate], [CreatedId], [FeedCategoryFeedId], [IsDeleted]) VALUES (10, N'স্টারটার', N'sdfasdasd', N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-06' AS Date), 1, 2, 0)
INSERT [dbo].[FeedCategories] ([FeedCategoryId], [FeedCategoryName], [FeedCategoryDetails], [FeedCategoryImageUrl], [CreatedDate], [CreatedId], [FeedCategoryFeedId], [IsDeleted]) VALUES (11, N'গ্রোয়ার', N'Test', N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-06' AS Date), 1, 6, 0)
INSERT [dbo].[FeedCategories] ([FeedCategoryId], [FeedCategoryName], [FeedCategoryDetails], [FeedCategoryImageUrl], [CreatedDate], [CreatedId], [FeedCategoryFeedId], [IsDeleted]) VALUES (12, N'গ্রোয়ার', N'adf asdf asd fasdf', N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-29' AS Date), 1, 11, 0)
INSERT [dbo].[FeedCategories] ([FeedCategoryId], [FeedCategoryName], [FeedCategoryDetails], [FeedCategoryImageUrl], [CreatedDate], [CreatedId], [FeedCategoryFeedId], [IsDeleted]) VALUES (13, N'ফিনিশার', N'adsf as asd asdf', N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-29' AS Date), 1, 11, 0)
INSERT [dbo].[FeedCategories] ([FeedCategoryId], [FeedCategoryName], [FeedCategoryDetails], [FeedCategoryImageUrl], [CreatedDate], [CreatedId], [FeedCategoryFeedId], [IsDeleted]) VALUES (14, N'স্টারটার', N'adsfasdf', N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-29' AS Date), 1, 11, 0)
INSERT [dbo].[FeedCategories] ([FeedCategoryId], [FeedCategoryName], [FeedCategoryDetails], [FeedCategoryImageUrl], [CreatedDate], [CreatedId], [FeedCategoryFeedId], [IsDeleted]) VALUES (15, N'গ্রোয়ার', N'dghdgh', N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-30' AS Date), 1, 2, 0)
SET IDENTITY_INSERT [dbo].[FeedCategories] OFF
SET IDENTITY_INSERT [dbo].[FeedDistributionReports] ON 

INSERT [dbo].[FeedDistributionReports] ([FeedDistributionReportId], [FeedDistributionName], [FeedDistributionDate], [FeedDistributionFeedId], [FeedDistributionFeedategoryId], [FeedDistributionQuantityId], [FeedDistributionSakNumber], [FeedDistributionTotalWeight], [CreatedId], [CreatedDate], [EditedId], [EditedDate], [FeedDistributionDescription], [AreaId], [ProjectId], [IsActive]) VALUES (3, N'test', CAST(N'2019-01-27' AS Date), 2, 15, 40, CAST(2.00 AS Decimal(18, 2)), CAST(80.00 AS Decimal(18, 2)), 3, CAST(N'2019-01-27' AS Date), 3, CAST(N'2019-01-30' AS Date), N'test', 5, 4, 0)
INSERT [dbo].[FeedDistributionReports] ([FeedDistributionReportId], [FeedDistributionName], [FeedDistributionDate], [FeedDistributionFeedId], [FeedDistributionFeedategoryId], [FeedDistributionQuantityId], [FeedDistributionSakNumber], [FeedDistributionTotalWeight], [CreatedId], [CreatedDate], [EditedId], [EditedDate], [FeedDistributionDescription], [AreaId], [ProjectId], [IsActive]) VALUES (4, N'test', CAST(N'2019-01-25' AS Date), 2, 10, 1, CAST(52.00 AS Decimal(18, 2)), CAST(1040.00 AS Decimal(18, 2)), 3, CAST(N'2019-01-27' AS Date), NULL, NULL, NULL, 5, 4, 1)
INSERT [dbo].[FeedDistributionReports] ([FeedDistributionReportId], [FeedDistributionName], [FeedDistributionDate], [FeedDistributionFeedId], [FeedDistributionFeedategoryId], [FeedDistributionQuantityId], [FeedDistributionSakNumber], [FeedDistributionTotalWeight], [CreatedId], [CreatedDate], [EditedId], [EditedDate], [FeedDistributionDescription], [AreaId], [ProjectId], [IsActive]) VALUES (5, N'হিসাবের নাম লিখুন', CAST(N'2019-01-27' AS Date), 2, 10, 25, CAST(2.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), 3, CAST(N'2019-01-27' AS Date), 3, CAST(N'2019-01-29' AS Date), NULL, 5, 4, 0)
SET IDENTITY_INSERT [dbo].[FeedDistributionReports] OFF
SET IDENTITY_INSERT [dbo].[FeedSellingReport] ON 

INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (9, N'A5874', N'কাজী ফিডের হিসাব', 4, 5, 5, 25, CAST(150 AS Decimal(18, 0)), CAST(3750 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(318750 AS Decimal(18, 0)), CAST(N'2018-12-01' AS Date), CAST(N'2018-12-14' AS Date), 2, NULL, NULL, N'গত ৯ ফেব্রুয়ারি ইউটিউবে দেওয়া হয় গানটি। এরপর সেই চোখের ইশারা ভাইরাল হয় অনলাইনে। প্রিয়া রাতারাতি তারকা বনে যান। এরপর প্রিয়াকে নিয়ে হইচই পড়ে যায়। তাঁকে নিয়ে চলচ্চিত্র তৈরির ভাবনা শুরু হয় নির্মাতাদের মধ্যে। গানটি নিয়ে ধর্মীয় অনুভূতিতে আঘাত হানার অভিযোগে মামলাও হয় প্রিয়ার বিরুদ্ধে। এমনকি রাহুল গান্ধী পর্যন্ত মাত হয়ে যান প্রিয়ার ভ্রুর নাচে। রাহুল গান্ধীর ভ্রু নাচানো একটি ছবি অনলাইনে প্রকাশ হলে এ নিয়ে শুরু হয় বিতর্ক।', NULL, 1)
INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (10, N'#8547', N'কে এন বি ফিডের হিসাব', 5, 5, 5, 50, CAST(225 AS Decimal(18, 0)), CAST(11250 AS Decimal(18, 0)), CAST(65 AS Decimal(18, 0)), CAST(731250 AS Decimal(18, 0)), CAST(N'2018-12-05' AS Date), CAST(N'2018-12-14' AS Date), 2, NULL, NULL, N'গুগল প্রকাশ করেছে জনপ্রিয় এই সার্চ ইঞ্জিনে সবচেয়ে বেশি খোঁজ করা মানুষের তালিকা। ভারতীয় ব্যবহারকারীদের চোখে বিনোদনজগতের তালিকায় শীর্ষ তারকা হিসেবে উঠে এসেছে প্রিয়া প্রকাশের নাম। গত ফেব্রুয়ারি মাসে ভারতের দক্ষিণের মালয়ালাম ভাষার রোমান্টিক-কমেডি ছবি ওরু আদার লাভ-এর &amp;lsquo;মানিক্য মালারায়া পুভি&amp;rsquo; গানের দৃশ্যে প্রিয়ার চোখের ইশারায় কাত হয়ে যায় নেট দুনিয়া।', NULL, 1)
INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (11, N'#98745', N'চান্দার ক্ষেত মৎস্য খামার', 4, 5, 5, 25, CAST(355 AS Decimal(18, 0)), CAST(8875 AS Decimal(18, 0)), CAST(75 AS Decimal(18, 0)), CAST(665625 AS Decimal(18, 0)), CAST(N'2018-12-19' AS Date), CAST(N'2018-12-14' AS Date), 2, NULL, NULL, N'প্রিয়ার পরেই আছেন সদ্য বিবাহিত মার্কিন গায়ক নিক জোনাস। ভারতীয় অভিনেত্রী প্রিয়াঙ্কা চোপড়ার সঙ্গে প্রেম ও বিয়ে নিয়ে এ বছর তিনি বেশ আলোচনায় ছিলেন বলা চলে। এর পরপরই আছেন যথাক্রমে স্বপ্না চৌধুরী, প্রিয়াঙ্কা চোপড়া, আনন্দ আহুজা, সারা আলী খান, সালমান খান, মেগান মার্কেল, অনুপ জালোটা ও বনি কাপুর। ডিএনএ।', NULL, 1)
INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (12, N'#A3254 update', N'sgsfdgsfg', 10, 5, 5, 25, CAST(50 AS Decimal(18, 0)), CAST(1250 AS Decimal(18, 0)), CAST(108 AS Decimal(18, 0)), CAST(135000 AS Decimal(18, 0)), CAST(N'2019-01-08' AS Date), CAST(N'2019-01-09' AS Date), 3, NULL, NULL, N'dghdgh', NULL, 1)
INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (13, N'#A3254 update', N'dfghdghdgh', 10, 0, 0, 50, CAST(120 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), CAST(780000 AS Decimal(18, 0)), CAST(N'2019-01-08' AS Date), CAST(N'2019-01-09' AS Date), 3, CAST(N'2019-01-09' AS Date), 3, N'sfgsfgsfg', NULL, 1)
INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (14, N'#A3254 update', N'sfgsfg', 15, 5, 4, 25, CAST(454 AS Decimal(18, 0)), CAST(11350 AS Decimal(18, 0)), CAST(8 AS Decimal(18, 0)), CAST(18160 AS Decimal(18, 0)), CAST(N'2019-01-08' AS Date), CAST(N'2019-01-09' AS Date), 3, CAST(N'2019-01-30' AS Date), 3, N'dghdghdgh', NULL, 2)
INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (15, N'sfdgsfg', N'sdfgsfg', 10, 5, 4, 25, CAST(20 AS Decimal(18, 0)), CAST(500 AS Decimal(18, 0)), CAST(5 AS Decimal(18, 0)), CAST(2500 AS Decimal(18, 0)), CAST(N'2019-01-11' AS Date), CAST(N'2019-01-12' AS Date), 3, CAST(N'2019-01-29' AS Date), 3, N'sfgdsh', NULL, 2)
INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (16, N'gjkgjk', N'fgjkgjk', 14, 5, 4, 25, CAST(12 AS Decimal(18, 0)), CAST(300 AS Decimal(18, 0)), CAST(60 AS Decimal(18, 0)), CAST(18000 AS Decimal(18, 0)), CAST(N'2019-01-09' AS Date), CAST(N'2019-01-12' AS Date), 3, CAST(N'2019-01-29' AS Date), 3, N'fhj', NULL, 11)
INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (17, N'fghfgh', N'fghfgh', 11, 5, 4, 25, CAST(4 AS Decimal(18, 0)), CAST(100 AS Decimal(18, 0)), CAST(2 AS Decimal(18, 0)), CAST(24 AS Decimal(18, 0)), CAST(N'2019-01-27' AS Date), CAST(N'2019-01-27' AS Date), 3, CAST(N'2019-01-29' AS Date), 3, NULL, NULL, 6)
INSERT [dbo].[FeedSellingReport] ([FeedSellingReportId], [SellingFeedReportNumber], [SellingReportName], [SellingReportFeedCategoryId], [SellingFeedAreaId], [SellingFeedProjectId], [SellingFeedWeightPerSack], [SellingFeedSackNumber], [SellingFeedTotalWeight], [SellingFeedPricePerKg], [SellingFeedTotalPrice], [SellingFeedCalculationDate], [SellingFeedCreateDate], [SellingFeedCreatedId], [SellingFeedEditedDate], [SellignFeedEditedId], [SellingFeedSellNote], [FishSellingIsActive], [FeedId]) VALUES (18, N'#sfg sfg sfgs fdg', N'sgfsfgsfg sf gsfg sfg sfdg sfdg sdfg sfg sfg', 10, 5, 4, 50, CAST(95 AS Decimal(18, 0)), CAST(4750 AS Decimal(18, 0)), CAST(95 AS Decimal(18, 0)), CAST(451250 AS Decimal(18, 0)), CAST(N'2019-01-29' AS Date), CAST(N'2019-01-30' AS Date), 3, CAST(N'2019-01-30' AS Date), 3, N'sfg sfg sfg', NULL, 2)
SET IDENTITY_INSERT [dbo].[FeedSellingReport] OFF
SET IDENTITY_INSERT [dbo].[Fish] ON 

INSERT [dbo].[Fish] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (1, N'রূপ চাঁদা', N'আস্ত মাছটি ধুয়ে মুখের পাশে সামান্য কেটে পেট পরিষ্কার করে নিতে হবে ।মাছের এপিঠ ওপিঠ ছুরি দিয়ে চিরে নিতে হবে ।সামান্য লবণ, হলু্&amp;zwnj;দ ও মরিচ মাখিয়ে ৩০মিনিট ম্যারিনেট করতে হবে(এই মাছে বেশী মসলা দিলে সামুদ্রিক মাছের সুঘ্রানটাই বিলীন হয়েযাবে)। ৩০মিনিট পর চুলায় ফ্রাই প্যান বসিয়ে সরিষার তেল দিতে হবে,তপ্ত তাওয়ায়', N'Uploads/CreateSegment/Fish/310a31ca-958d-4379-894f-3b526e7e55d1-3.jpg', CAST(N'2018-12-20 23:36:16.633' AS DateTime), 1, 0)
INSERT [dbo].[Fish] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (2, N'দেশি পাঙ্গাস', N'এর প্রাকৃতিক প্রজনন ক্ষেত্রসমূহ ক্ষতিগ্রস্থ হওয়ার ফলে পাঙ্গাস মাছের উৎপাদনও ক্রমশঃ কমে যাচ্ছে। তবে পুকুরে পাঙ্গাস চাষের ব্যাপক সম্ভাবনা থাকায় আশির দশক থেকেই এর ওপর কার্যক্রম অব্যহত রয়েছে। পাঙ্গাস মাছের বিভিন্ন জাত: পাঙ্গাস মাঝের জাতগুলোর মধ্যে দেশী পাঙ্গাস ও থাই পাঙ্গাস সবচেয়ে বেশি জনপ্রিয়', N'Uploads/CreateSegment/Fish/a470c08c-2586-4f1a-ba7f-f1f634156ba6-9.jpg', CAST(N'2018-12-20 23:44:01.683' AS DateTime), 1, 0)
INSERT [dbo].[Fish] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (3, N'দেশি ইলিশ', N'দেশে ইলিশের উৎপাদন বাড়ার পাশাপাশি প্রতিবছর আমদানির পরিমাণ লাফিয়ে বাড়ছে। গত অর্থবছরের তুলনায় এ অর্থবছরের প্রথম নয় মাসে আমদানি বেড়েছে আড়াই গুণের বেশি। এতে দেশি মৎস্যজীবীদের ব্যবসায়িক ক্ষতির মুখে পড়ার আশঙ্কা রয়েছে বলে মনে করছেন সংশ্লিষ্ট ব্যক্তিরা।', N'Uploads/CreateSegment/Fish/b3fcb2e7-63a8-4881-a8e6-498b35273268-5.jpg', CAST(N'2018-12-20 23:45:09.960' AS DateTime), 1, 0)
INSERT [dbo].[Fish] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (4, N'দেশি রুই', N'আন্তর্জাতিকভাবে রুই মাছের নাম বলা হয়ে থাকে রোহু। ... রুই মাছ শাকাশী।এরা সাধারণত জলের মধ্যস্তরে চলাচল করে৷ এদের মুখ কিছুটা নিচের দিকে নামানো এবং পুরু ঠোঁট থাকার কারণে জলজ উদ্ভিদ, আগাছা এবং মাঝে মাঝে জলের তলদেশ ... প্রতি ১০০ গ্রাম রুই মাছে ১৬.৪ গ্রাম আমিষ, ১.৪ গ্রাম চর্বি, ৬৮০ মিলিগ্রাম ক্যালসিয়াম, ২২৩ মিলিগ্রাম ফসফরাস থাকে', N'Uploads/CreateSegment/Fish/8f3a1787-43a2-4f13-9cd1-fc34b2b24c3f-1.png', CAST(N'2018-12-20 23:45:56.280' AS DateTime), 1, 0)
INSERT [dbo].[Fish] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (5, N'চিংড়ি', N'চিংড়ি একটি সন্ধিপদী (আর্থ্রোপোডা) প্রাণী। স্বাদু জলের চিংড়ির গণ (Genus) প্যালিমন (Palaemon) এর বিভিন্ন প্রজাতিকে একত্রে চিংড়ী বলে।  চিংড়ি বিষয়ক এই নিবন্ধটি অসম্পূর্ণ। আপনি চাইলে এটিকে সম্প্রসারিত করে উইকিপিডিয়াকে সাহায্য করতে পারেন', N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-20 23:46:51.417' AS DateTime), 1, 0)
INSERT [dbo].[Fish] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (7, N'test test test', N'test', N'Uploads/CreateSegment/Fish/4501de68-4826-4631-9b93-b7046651cf32-image.png', CAST(N'2019-01-06 21:23:50.713' AS DateTime), 1, 0)
INSERT [dbo].[Fish] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (8, N'asdasdasd', N'asdasdasd', N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-06 21:25:09.923' AS DateTime), 1, 0)
INSERT [dbo].[Fish] ([ID], [Name], [Description], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (9, N'sdsdsd', N'hgfhgfhg', N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-25 03:42:04.733' AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[Fish] OFF
SET IDENTITY_INSERT [dbo].[FishSeller] ON 

INSERT [dbo].[FishSeller] ([ID], [Name], [Age], [ImageUrl], [CreatedDate], [CreatedId], [Description], [IsDeleted]) VALUES (3, N'Mr Abul ', 15, N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-09 17:58:29.723' AS DateTime), 1, N'adsf adsf ads asd fas a asdf', 0)
INSERT [dbo].[FishSeller] ([ID], [Name], [Age], [ImageUrl], [CreatedDate], [CreatedId], [Description], [IsDeleted]) VALUES (4, N'Mr Babul', 25, N'Uploads/CreateSegment/Fish/98bf24d4-b22a-4fef-8034-029d7dab9fbd-pT5eMoKAc.png', CAST(N'2019-01-09 18:04:39.993' AS DateTime), 1, N'sgsfg', 0)
SET IDENTITY_INSERT [dbo].[FishSeller] OFF
SET IDENTITY_INSERT [dbo].[FishSellingReport] ON 

INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (18, N'পাঙ্গাস মাছ বিক্রি', CAST(350.00 AS Decimal(18, 2)), CAST(N'2018-12-12' AS Date), 30, 2, 700, CAST(75.00 AS Decimal(18, 2)), CAST(26250.00 AS Decimal(18, 2)), N'Uploads/CreateSegment/Fish/6479afb2-333a-4ce3-83ae-89cd0c34ee4f-pangas-cover-20171010190837.jpg', CAST(N'2018-12-12' AS Date), 2, CAST(N'2018-12-12' AS Date), 2, NULL, 4, 3, 4, CAST(26250.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (19, N'পুটি মাছ বিক্রি', CAST(650.00 AS Decimal(18, 2)), CAST(N'2019-01-10' AS Date), 4, 1, 650, CAST(135.00 AS Decimal(18, 2)), CAST(87750.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-12' AS Date), 2, CAST(N'2019-01-12' AS Date), 3, NULL, 5, 4, 3, CAST(6875.00 AS Decimal(18, 2)), CAST(80875.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (20, N'পাঙ্গাস মাছ বিক্রি', CAST(780.00 AS Decimal(18, 2)), CAST(N'2018-12-14' AS Date), 30, 1, 780, CAST(75.00 AS Decimal(18, 2)), CAST(58500.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-12' AS Date), 2, CAST(N'2018-12-14' AS Date), 2, N'গণফোরামের সভাপতি ও জাতীয় ঐক্যফ্রন্টের শীর্ষ নেতা ড. কামাল হোসেনের উদ্দেশে আওয়ামী লীগের সভানেত্রী ও প্রধানমন্ত্রী শেখ হাসিনা বলেছেন, খামোশ বললেই কি মানুষের মুখ খামোশ হয়ে যাবে? খামোশ বললে জনগণ খামোশ হয়ে যাবে না, মানুষকে খামোশ রাখা যাবে না।', 5, 5, 4, CAST(58500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (22, N'dfghdg ', CAST(78.00 AS Decimal(18, 2)), CAST(N'2018-12-12' AS Date), 31, 5, 390, CAST(7801.00 AS Decimal(18, 2)), CAST(608478.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-12' AS Date), 2, CAST(N'2018-12-12' AS Date), 2, N'sdf s sdfg sfdg sdfg', 4, 3, 4, CAST(608478.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (23, N'পুটি মাছ বিক্রি', CAST(800.00 AS Decimal(18, 2)), CAST(N'2018-12-01' AS Date), 33, 2, 1600, CAST(150.00 AS Decimal(18, 2)), CAST(120000.00 AS Decimal(18, 2)), N'Uploads/CreateSegment/Fish/6fbb704a-3ba9-4d69-87d9-339cb9adfb51-download.jpg', CAST(N'2018-12-12' AS Date), 2, NULL, NULL, N'dsfg sfd gsf sf tey etyety etyey etyeutyu wrt wrtfgsd fsg&amp;nbsp;', 4, 3, 4, CAST(120000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (24, N'test selling', CAST(875.00 AS Decimal(18, 2)), CAST(N'2019-01-09' AS Date), 3, 1, 875, CAST(78.00 AS Decimal(18, 2)), CAST(68250.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-13' AS Date), 2, CAST(N'2019-01-12' AS Date), 3, N'as dads', 5, 4, 3, CAST(68250.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (25, N'সূর্যোদয় হিসাব', CAST(985.00 AS Decimal(18, 2)), CAST(N'2018-12-14' AS Date), 31, 3, 2955, CAST(135.00 AS Decimal(18, 2)), CAST(132975.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-13' AS Date), 2, CAST(N'2018-12-14' AS Date), 2, N'&lt;span style="color:#242729;font-family:Arial, ''Helvetica Neue'', Helvetica, sans-serif;font-size:15px;background-color:#ffffff;"&gt;r, if you like the ?? operator (which evaluates to the first non-null value&amp;nbsp;&lt;/span&gt;&lt;span style="color:#242729;font-family:Arial, ''Helvetica Neue'', Helvetica, sans-serif;font-size:15px;background-color:#ffffff;"&gt;r, if you like the ?? operator (which evaluates to the first non-null value&amp;nbsp;&lt;/span&gt;', 1, 0, 4, CAST(132975.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (26, N'পুটি মাছ বিক্রি', CAST(7878.00 AS Decimal(18, 2)), CAST(N'2018-12-11' AS Date), 31, 20, 157560, CAST(0.10 AS Decimal(18, 2)), CAST(787.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-13' AS Date), 2, CAST(N'2018-12-13' AS Date), 2, N'sdfgsfg sfd sdf', 1, 0, 4, CAST(787.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (28, N'hg gh g', CAST(850.00 AS Decimal(18, 2)), CAST(N'2018-12-05' AS Date), 31, 3, 2550, CAST(135.00 AS Decimal(18, 2)), CAST(114750.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-14' AS Date), 2, NULL, NULL, N'jhjjh gj&amp;nbsp;', 5, 5, 4, CAST(114750.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (29, N'পুটি মাছ', CAST(3.00 AS Decimal(18, 2)), CAST(N'2018-12-17' AS Date), 31, 5, 15, CAST(2.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-16' AS Date), 2, CAST(N'2018-12-16' AS Date), 2, NULL, 5, 5, 4, CAST(6.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (30, N'hg gh g', CAST(52.00 AS Decimal(18, 2)), CAST(N'2019-01-08' AS Date), 2, 13, 676, CAST(320.00 AS Decimal(18, 2)), CAST(16640.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-09' AS Date), 3, CAST(N'2019-01-27' AS Date), 3, NULL, 5, 4, 3, CAST(16640.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (31, N'পুটি মাছ বিক্রি', CAST(654.00 AS Decimal(18, 2)), CAST(N'2019-01-12' AS Date), 4, 1, 654, CAST(98.00 AS Decimal(18, 2)), CAST(64092.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-12' AS Date), 3, CAST(N'2019-01-13' AS Date), 3, N'sdfgsdgsfg', 5, 4, 4, CAST(53950.00 AS Decimal(18, 2)), CAST(10142.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (32, N'dhd gf hdg hdh', CAST(985.00 AS Decimal(18, 2)), CAST(N'2019-01-12' AS Date), 5, 9, 8865, CAST(120.00 AS Decimal(18, 2)), CAST(118200.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-12' AS Date), 3, CAST(N'2019-01-12' AS Date), 3, N'xf gfg', 5, 4, 4, CAST(118200.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (34, N'152', CAST(1.00 AS Decimal(18, 2)), CAST(N'2019-01-21' AS Date), 2, 3, 3, CAST(6.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-21' AS Date), 3, NULL, NULL, N'adasdasd', 5, 4, 4, CAST(6.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (35, N'asdasdas', CAST(15.00 AS Decimal(18, 2)), CAST(N'2019-01-27' AS Date), 2, 1, 15, CAST(0.80 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-27' AS Date), 3, NULL, NULL, NULL, 5, 4, 3, CAST(2.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (36, N'asdas', CAST(7.00 AS Decimal(18, 2)), CAST(N'2019-01-27' AS Date), 2, 1, 7, CAST(1.00 AS Decimal(18, 2)), CAST(7.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-27' AS Date), 3, NULL, NULL, NULL, 5, 4, 3, CAST(3.00 AS Decimal(18, 2)), CAST(4.00 AS Decimal(18, 2)))
INSERT [dbo].[FishSellingReport] ([FishSellId], [SellingFishName], [TotalSellInKG], [SellDate], [SellFishId], [PiecesPerKG], [TotalPiecesFish], [PricePerKG], [TotalSellPrice], [FishImageUrl], [CalculatedDate], [CalculatedById], [CalculationEditedDate], [CalCulationEditedId], [SellNote], [AreaId], [ProjectId], [FishSellerId], [FishAmountPaid], [FishAmountDue]) VALUES (37, N'455', CAST(25.00 AS Decimal(18, 2)), CAST(N'2019-01-27' AS Date), 2, 5, 125, CAST(50.24 AS Decimal(18, 2)), CAST(1256.00 AS Decimal(18, 2)), N'assets/global/img/rams-logo.jpeg', CAST(N'2019-01-27' AS Date), 3, NULL, NULL, NULL, 5, 4, 4, CAST(251.00 AS Decimal(18, 2)), CAST(1005.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[FishSellingReport] OFF
SET IDENTITY_INSERT [dbo].[Project] ON 

INSERT [dbo].[Project] ([ID], [Name], [AreaId], [Land], [Cost], [Time], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (3, N'চা বাগান,চা বাগান', 4, N'1201', N'100000000', N'10', N'assets/global/img/rams-logo.jpeg', CAST(N'2018-11-14 23:19:30.797' AS DateTime), 1, 0)
INSERT [dbo].[Project] ([ID], [Name], [AreaId], [Land], [Cost], [Time], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (4, N'test for area ', 5, N'Dont know', N'4545454', N'2.50', N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-10 20:46:34.580' AS DateTime), 1, 0)
INSERT [dbo].[Project] ([ID], [Name], [AreaId], [Land], [Cost], [Time], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (5, N'sfgsdfg', 5, N'sfgsf', N'sfg', N'sfgsfg', N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-10 20:47:06.837' AS DateTime), 1, 0)
INSERT [dbo].[Project] ([ID], [Name], [AreaId], [Land], [Cost], [Time], [ImageUrl], [CreatedDate], [CreatedId], [IsDeleted]) VALUES (6, N'sfgsf', 4, N'gsfg', N'sfg', N'sfg', N'assets/global/img/rams-logo.jpeg', CAST(N'2018-12-10 20:47:22.117' AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[Project] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [AddressOne], [AddressTwo], [City], [ConfirmPassword], [Country], [CreatedBy], [CreatedDate], [CurrentPassword], [AreaID], [EditedBy], [EditedDate], [EmailAddress], [FirstName], [IsActivated], [IsTemporaryPassword], [LastName], [Password], [ProjectID], [PhoneNumber], [Position], [PostCode], [PublicationStatus], [Religion], [RegistrationConfirmed], [Role], [UserImageCaption], [UserImagePath], [UserImageSize], [UserName], [UserRegistartionGuid], [IsTrialUser], [TrialStartDate], [TrialEndDate], [IsImageUploadedByUser], [UserCreatedBy], [CreatedIP], [EditedIP]) VALUES (1, N'Dhaka', N'Dhaka', N'Dhaka', N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', N'Bangladesh', 1, CAST(N'2017-04-04 00:00:00.000' AS DateTime), NULL, 4, 1, CAST(N'2019-01-12 21:19:03.890' AS DateTime), N'admin@gmail.com', N'Admin', 1, NULL, N'update', N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', 3, N'07737327171', N'Administrator', N'1207', 1, N'Dhaka', N'1', NULL, N'Rams-logo.png', N'Uploads/User/eccfc5fb-1404-4d4f-9065-c305fff7c7c9-49896089_1998696470224170_3530142991924592640_n.jpg', 127154, N'admin@gmail.com', N'f4d61299-5076-4bc7-8aa0-a5b644424937', 0, NULL, NULL, 0, N'SuperAdmin', NULL, N'103.232.100.78')
INSERT [dbo].[Users] ([UserID], [AddressOne], [AddressTwo], [City], [ConfirmPassword], [Country], [CreatedBy], [CreatedDate], [CurrentPassword], [AreaID], [EditedBy], [EditedDate], [EmailAddress], [FirstName], [IsActivated], [IsTemporaryPassword], [LastName], [Password], [ProjectID], [PhoneNumber], [Position], [PostCode], [PublicationStatus], [Religion], [RegistrationConfirmed], [Role], [UserImageCaption], [UserImagePath], [UserImageSize], [UserName], [UserRegistartionGuid], [IsTrialUser], [TrialStartDate], [TrialEndDate], [IsImageUploadedByUser], [UserCreatedBy], [CreatedIP], [EditedIP]) VALUES (2, NULL, NULL, NULL, N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', NULL, 34, CAST(N'2018-01-14 21:04:50.257' AS DateTime), NULL, 4, 1, CAST(N'2018-04-17 05:56:23.820' AS DateTime), N'pirujali@test.com', N'Michael', 1, NULL, N'Devitt', N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', 3, NULL, N'Director', NULL, NULL, NULL, N'1', N'User', N'rams-logo', N'users-logo/137e5650-aa9f-47c6-a69a-b4c115690514.png', 89045, N'Pirujali', N'b1adce4e-a9d9-4165-9efc-1be6ea30aa30', 0, NULL, NULL, 0, N'User', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [AddressOne], [AddressTwo], [City], [ConfirmPassword], [Country], [CreatedBy], [CreatedDate], [CurrentPassword], [AreaID], [EditedBy], [EditedDate], [EmailAddress], [FirstName], [IsActivated], [IsTemporaryPassword], [LastName], [Password], [ProjectID], [PhoneNumber], [Position], [PostCode], [PublicationStatus], [Religion], [RegistrationConfirmed], [Role], [UserImageCaption], [UserImagePath], [UserImageSize], [UserName], [UserRegistartionGuid], [IsTrialUser], [TrialStartDate], [TrialEndDate], [IsImageUploadedByUser], [UserCreatedBy], [CreatedIP], [EditedIP]) VALUES (3, NULL, NULL, NULL, N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', NULL, NULL, CAST(N'2018-12-15 11:07:44.617' AS DateTime), NULL, 5, NULL, CAST(N'2018-12-15 11:07:45.697' AS DateTime), N'chabagan@test.com', N'SDFSDF', 1, NULL, N'asdasd', N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', 4, NULL, N'company User', NULL, NULL, NULL, N'1', N'User', NULL, NULL, NULL, N'Chabagan', N'855dd834-77fa-4fc4-a77e-fdc90a774b2f', 0, NULL, NULL, 0, N'CompanyAdmin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [AddressOne], [AddressTwo], [City], [ConfirmPassword], [Country], [CreatedBy], [CreatedDate], [CurrentPassword], [AreaID], [EditedBy], [EditedDate], [EmailAddress], [FirstName], [IsActivated], [IsTemporaryPassword], [LastName], [Password], [ProjectID], [PhoneNumber], [Position], [PostCode], [PublicationStatus], [Religion], [RegistrationConfirmed], [Role], [UserImageCaption], [UserImagePath], [UserImageSize], [UserName], [UserRegistartionGuid], [IsTrialUser], [TrialStartDate], [TrialEndDate], [IsImageUploadedByUser], [UserCreatedBy], [CreatedIP], [EditedIP]) VALUES (4, NULL, NULL, NULL, N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', NULL, NULL, CAST(N'2018-12-16 05:56:41.417' AS DateTime), NULL, 4, NULL, CAST(N'2018-12-16 05:56:41.417' AS DateTime), N'gasbari@test.com', N'As', 1, NULL, N'Khan', N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', 3, NULL, N'User', NULL, NULL, NULL, N'1', NULL, NULL, NULL, NULL, N'gasbari@test.com', N'd865eff7-b759-434d-a690-c30150fdf2bb', 0, NULL, NULL, 0, N'CompanyAdmin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [AddressOne], [AddressTwo], [City], [ConfirmPassword], [Country], [CreatedBy], [CreatedDate], [CurrentPassword], [AreaID], [EditedBy], [EditedDate], [EmailAddress], [FirstName], [IsActivated], [IsTemporaryPassword], [LastName], [Password], [ProjectID], [PhoneNumber], [Position], [PostCode], [PublicationStatus], [Religion], [RegistrationConfirmed], [Role], [UserImageCaption], [UserImagePath], [UserImageSize], [UserName], [UserRegistartionGuid], [IsTrialUser], [TrialStartDate], [TrialEndDate], [IsImageUploadedByUser], [UserCreatedBy], [CreatedIP], [EditedIP]) VALUES (5, NULL, NULL, NULL, N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', NULL, NULL, CAST(N'2018-12-16 21:25:44.110' AS DateTime), NULL, 4, NULL, CAST(N'2019-01-08 16:36:50.363' AS DateTime), N'sfsfgsfg@afads.com', N'sfgsfg', 1, NULL, N'sfgs', N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', 3, NULL, N'adfsadsf', NULL, NULL, NULL, N'1', NULL, NULL, N'Uploads/User/7f6c9771-3af3-49dc-ac80-6b94eff4db27-49582277_1969600933148722_4066460243985432576_n.jpg', NULL, N'sfsfgsfg@afads.com', N'8640d6e9-af90-48ce-91cd-185f613917dd', 0, NULL, NULL, 0, N'CompanyAdmin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [AddressOne], [AddressTwo], [City], [ConfirmPassword], [Country], [CreatedBy], [CreatedDate], [CurrentPassword], [AreaID], [EditedBy], [EditedDate], [EmailAddress], [FirstName], [IsActivated], [IsTemporaryPassword], [LastName], [Password], [ProjectID], [PhoneNumber], [Position], [PostCode], [PublicationStatus], [Religion], [RegistrationConfirmed], [Role], [UserImageCaption], [UserImagePath], [UserImageSize], [UserName], [UserRegistartionGuid], [IsTrialUser], [TrialStartDate], [TrialEndDate], [IsImageUploadedByUser], [UserCreatedBy], [CreatedIP], [EditedIP]) VALUES (6, NULL, NULL, NULL, N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', NULL, NULL, CAST(N'2019-01-06 21:36:09.673' AS DateTime), NULL, 4, NULL, CAST(N'2019-01-08 16:49:57.563' AS DateTime), N'testuser@gmail.com', N'User', 1, NULL, N'User', N'+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=', 3, NULL, N'chabagan@test.com', NULL, NULL, NULL, N'1', NULL, NULL, N'Uploads/User/9def95b8-c23d-4723-b5aa-435d2ea4fae0-02-black-254x203.jpg', NULL, N'testuser@gmail.com', N'd12d8e19-ea24-4e67-9a55-3295bf2f203f', 0, NULL, NULL, 0, N'CompanyAdmin', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  StoredProcedure [dbo].[up_GetAdminDashBoardOveriew]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREAte PROCEDURE [dbo].[up_GetAdminDashBoardOveriew]
AS
BEGIN

	
	DECLARE @TotalOveriew TABLE
	(
		TotalFishSell decimal(18,2),
		TotalFeedBuy decimal(18,2),
		TotalFeedDistribute decimal(18,2),
		TotalRecord decimal(18,2)
	)

	DECLARE @TotalFishSell AS decimal(18,2)
	DECLARE @TotalFeedBuy AS decimal(18,2)
	DECLARE @TotalFeedDistribute AS decimal(18,2)
	DECLARE @TotalRecord AS decimal(18,2)

	SELECT @TotalFishSell = [TotalSellPrice] from [dbo].[FishSellingReport];
	SELECT @TotalFeedBuy = [SellingFeedTotalPrice] from [dbo].[FeedSellingReport];
	SELECT @TotalFeedDistribute = [FeedDistributionTotalWeight] from [dbo].[FeedDistributionReports];
	set @TotalRecord = '250.55'

	INSERT INTO @TotalOveriew (TotalFishSell,TotalFeedBuy,TotalFeedDistribute,TotalRecord) Values(@TotalFishSell, @TotalFeedBuy,@TotalFeedDistribute,@TotalRecord)
	select * from @TotalOveriew
END


GO
/****** Object:  StoredProcedure [dbo].[up_GetFarmRentalReportsBySearchParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFarmRentalReportsBySearchParam] 
( 
	@startDate date,
	@endDate Date,
	@projectId  varchar (10),
	@calCulationName nvarchar(max),
	@isPartial int=1
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @projectId !='' AND @calCulationName !='')
		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE (
					d.[IsDeleted] = 0 and
					d.[FarmRentalDate] >=@startDate
					AND 
					d.[FarmRentalDate] <=@endDate 
					AND 
					d.[FarmRentalProjectID] = @projectId 
					AND  
					d.[FarmRentalReportName] like +'%'+ @calCulationName +'%'
				)
				
		END

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @projectId !='' AND @calCulationName ='')
		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE (
					d.[IsDeleted] = 0 and
					d.[FarmRentalDate] >=@startDate
					AND 
					d.[FarmRentalDate] <=@endDate 
					AND 
					d.[FarmRentalProjectID] = @projectId 
				)
				
		END

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @projectId ='' AND @calCulationName ='')
		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE (
					d.[IsDeleted] = 0 and
					d.[FarmRentalDate] >=@startDate
					AND 
					d.[FarmRentalDate] <=@endDate 
				)
				
		END


	if @isPartial = 1 AND (@startDate ='' AND @endDate ='' AND @projectId !='' AND @calCulationName !='')
		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE (
					d.[IsDeleted] = 0
					AND 
					d.[FarmRentalProjectID] = @projectId 
					AND  
					d.[FarmRentalReportName] like +'%'+ @calCulationName +'%'
				)
				
		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @projectId ='' AND @calCulationName !='')
		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE (
					d.[IsDeleted] = 0 and
					d.[FarmRentalDate] >=@startDate
					AND 
					d.[FarmRentalDate] <=@endDate 
					AND  
					d.[FarmRentalReportName] like +'%'+ @calCulationName +'%'
				)
				
		END


	if @isPartial = 1 AND (@startDate ='' AND @endDate !='' AND @projectId !='' AND @calCulationName !='')
		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE (
					d.[IsDeleted] = 0 
					AND 
					d.[FarmRentalDate] <=@endDate 
					AND 
					d.[FarmRentalProjectID] = @projectId 
					AND  
					d.[FarmRentalReportName] like +'%'+ @calCulationName +'%'
				)
		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate ='' AND @projectId !='' AND @calCulationName !='')
		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE (
					d.[IsDeleted] = 0 and
					d.[FarmRentalDate] >=@startDate
					AND 
					d.[FarmRentalProjectID] = @projectId 
					AND  
					d.[FarmRentalReportName] like +'%'+ @calCulationName +'%'
				)
				
		END

		if @isPartial = 1 AND (@startDate !='' AND @endDate ='' AND @projectId !='' AND @calCulationName ='')
		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE (
					d.[IsDeleted] = 0 and
					d.[FarmRentalDate] >=@startDate
					AND 
					d.[FarmRentalProjectID] = @projectId 
				)
				
		END


		if @isPartial = 1 AND (@startDate ='' AND @endDate !='' AND @projectId !='' AND @calCulationName ='')
		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE (
					d.[IsDeleted] = 0
					AND 
					d.[FarmRentalDate] <=@endDate 
					AND 
					d.[FarmRentalProjectID] = @projectId 
				)
				
		END


	if @isPartial = 0

		BEGIN
			SELECT
			d.[FarmRentalReportID],
			d.[FarmRentalReportName],
			d.[FarmRentalDetails],
			convert(nvarchar(15),d.[FarmRentalDate],106) AS FarmRentalDate,
			(select t.[Name] from  [dbo].[Project] t where t.[ID]=d.[FarmRentalProjectID]) as ProjectName,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalLandAmount])) As FarmRentalLandAmount,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalMainFee])) As FarmRentalMainFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalTotalFee])) As FarmRentalTotalFee,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FarmRentalServieFee])) As FarmRentalServieFee,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate

			FROM
			[dbo].[FarmRentalReports] d

			WHERE d.[IsDeleted] = 0 

		END


END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFeedBuyingChartReportForAdmin]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFeedBuyingChartReportForAdmin] 
AS 
BEGIN 

	select
	convert(nvarchar(500),sum(c.[SellingFeedTotalPrice])) As SellingFeedPricePerKg,
	convert(varchar(15), c.[SellingFeedCalculationDate],106) CalculationDate
	FROM [dbo].[FeedSellingReport] c
	group by c.[SellingFeedCalculationDate]
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFeedBuyingChartReportForUser]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFeedBuyingChartReportForUser] 
( 
	@areaId int,
	@projectId int
) 

AS 
BEGIN 

	select
	convert(nvarchar(500),sum(c.[SellingFeedTotalPrice])) As SellingFeedPricePerKg,
	convert(varchar(15), c.[SellingFeedCalculationDate],106) CalculationDate
	FROM [dbo].[FeedSellingReport] c
	 where c.[SellingFeedAreaId]=@areaId and c.[SellingFeedProjectId]=@projectId
	group by c.[SellingFeedCalculationDate]
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFeedBuyingReportByFeedCategory]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFeedBuyingReportByFeedCategory] 
( 
	@areaId int,
	@projectId int
) 

AS 
BEGIN 
	select
	convert(nvarchar(500),sum(c.[SellingFeedTotalPrice])) As SellingFeedTotalPrice,
	[dbo].[uf_GetFeedCategorynameByFeedCategoryId] (c.SellingReportFeedCategoryId)FeedCategory
	FROM [dbo].[FeedSellingReport] c
	where c.[SellingFeedAreaId]=@areaId and c.[SellingFeedProjectId]=@projectId
	group by c.SellingReportFeedCategoryId
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFeedBuyingReportByFeedCategoryForAdmin]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFeedBuyingReportByFeedCategoryForAdmin] 
AS 
BEGIN 
	select
	convert(nvarchar(500),sum(c.[SellingFeedTotalPrice])) As SellingFeedTotalPrice,
	[dbo].[uf_GetFeedCategorynameByFeedCategoryId] (c.SellingReportFeedCategoryId)FeedCategory
	FROM [dbo].[FeedSellingReport] c
	group by c.SellingReportFeedCategoryId
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFeedCategorySearchResult]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFeedCategorySearchResult] 
( 
	@searchText nvarchar(500), 
	@pageNumber int, 
	@pageSize int 
) 

AS 
BEGIN 

	SELECT 
	f.[FeedCategoryId], 
	f.[FeedCategoryName], 
	f.[FeedCategoryDetails], 
	f.[FeedCategoryImageUrl],
	convert(varchar(10),f.[CreatedDate],103) CreaetdDate, 
	f.[CreatedId] 

	from [dbo].[FeedCategories] f 

	where 
	( ( Convert(varchar(100),f.[FeedCategoryId]) = @searchText ) 

	OR ( (f.[FeedCategoryName] LIKE +'%'+ @searchText +'%' )
	OR (convert(varchar(10),f.[CreatedDate],103) LIKE @searchText +'%') ) )
	AND f.IsDeleted = 0


END



GO
/****** Object:  StoredProcedure [dbo].[up_GetFeedDistributionReportsBySearchParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFeedDistributionReportsBySearchParam] 
( 
	@startDate date,
	@endDate Date,
	@feedId  varchar (10),
	@categoryId varchar (10),
	@isPartial int=1,
	@areaId nvarchar (10),
	@projectId nvarchar (10)
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @feedId !='' AND @categoryId !='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(
					d.IsActive = 0 and
					d.[FeedDistributionDate] >=@startDate
					AND 
					d.[FeedDistributionDate] <=@endDate 
					AND 
					d.[FeedDistributionFeedId] = @feedId 
					AND  
					d.[FeedDistributionFeedategoryId] = @categoryId
				)
				
		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @feedId !='' AND @categoryId ='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(
					d.IsActive = 0 and
					d.[FeedDistributionDate] >=@startDate
					AND 
					d.[FeedDistributionDate] <=@endDate 
					AND 
					d.[FeedDistributionFeedId] = @feedId 
				)
				
		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @feedId ='' AND @categoryId ='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(
					d.IsActive = 0 and
					d.[FeedDistributionDate] >=@startDate
					AND 
					d.[FeedDistributionDate] <=@endDate 
				)
				
		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @feedId ='' AND @categoryId !='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(
					d.IsActive = 0 and
					d.[FeedDistributionDate] >=@startDate
					AND 
					d.[FeedDistributionDate] <=@endDate 
					AND  
					d.[FeedDistributionFeedategoryId] =@categoryId
				)
				
		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate ='' AND @feedId !='' AND @categoryId !='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(
					d.IsActive = 0 and
					d.[FeedDistributionDate] >=@startDate
					AND 
					d.[FeedDistributionFeedId] = @feedId 
					AND  
					d.[FeedDistributionFeedategoryId] = @categoryId
				)
				
		END

	if @isPartial = 1 AND (@startDate !='' AND @endDate ='' AND @feedId ='' AND @categoryId ='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(
					d.IsActive = 0 and
					d.[FeedDistributionDate] >=@startDate
				)
				
		END


		if @isPartial = 1 AND (@startDate ='' AND @endDate !='' AND @feedId ='' AND @categoryId ='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(	d.IsActive = 0 and
					d.[FeedDistributionDate] <=@endDate 
				)
				
		END


	if @isPartial = 1 AND (@startDate ='' AND @endDate ='' AND @feedId !='' AND @categoryId !='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(
					d.IsActive = 0 and
					d.[FeedDistributionFeedId] = @feedId 
					AND  
					d.[FeedDistributionFeedategoryId] = @categoryId
				)
				
		END

	if @isPartial = 1 AND (@startDate ='' AND @endDate ='' AND @feedId !='' AND @categoryId ='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(
					d.IsActive = 0 and
					d.[FeedDistributionFeedId] = @feedId
				)
				
		END


		if @isPartial = 1 AND (@startDate ='' AND @endDate !='' AND @feedId !='' AND @categoryId !='')
		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d

			WHERE 
				(
					d.IsActive = 0 and
					d.[FeedDistributionDate] <=@endDate 
					AND 
					d.[FeedDistributionFeedId] = @feedId 
					AND  
					d.[FeedDistributionFeedategoryId] = @categoryId
				)
				
		END

	if @isPartial = 0

		BEGIN
			SELECT
			d.[FeedDistributionReportId],
			d.[FeedDistributionName],
			d.[FeedDistributionQuantityId],
			convert(nvarchar(15),d.[FeedDistributionDate],106) AS FeedDistributionDate,
			(select t.[FeedCategoryName] from [dbo].[FeedCategories] t where t.[FeedCategoryId]=d.[FeedDistributionFeedategoryId]) as FeedCategoryName,
			convert(nvarchar(100),d.[FeedDistributionSakNumber]) As FeedDistributionSakNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),d.[FeedDistributionTotalWeight])) As FeedDistributionTotalWeight,
			convert(nvarchar(15),d.[CreatedDate],106) AS CreatedDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](d.[FeedDistributionFeedId],d.[FeedDistributionFeedategoryId]) FeedNameWithCat

			FROM
			[dbo].[FeedDistributionReports] d where d.IsActive = 0
		END


END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFeedSellingReportsBySearchParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFeedSellingReportsBySearchParam] 
( 
	@startDate date,
	@endDate Date,
	@categoryId  varchar (10),
	@feedId varchar (10),
	@isPartial int=1,
	@areaId int,
	@projectId int
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @categoryId !='' AND @feedId !='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingFeedCalculationDate <=@endDate AND c.SellingReportFeedCategoryId =@categoryId) AND (c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			order by c.SellingFeedCalculationDate ASC

		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @categoryId !='' AND @feedId ='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingFeedCalculationDate <=@endDate AND c.SellingReportFeedCategoryId =@categoryId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			order by c.SellingFeedCalculationDate ASC

		END



	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @categoryId ='' AND @feedId ='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingFeedCalculationDate <=@endDate)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			order by c.SellingFeedCalculationDate ASC

		END

	if @isPartial = 1 AND (@startDate ='' AND @endDate ='' AND @categoryId !='' AND @feedId !='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				c.SellingReportFeedCategoryId =@categoryId
				AND
				(c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			order by c.SellingFeedCalculationDate ASC

		END

	if @isPartial = 1 AND (@startDate ='' AND @endDate ='' AND @categoryId ='' AND @feedId !='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			order by c.SellingFeedCalculationDate ASC

		END

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @categoryId ='' AND @feedId !='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingFeedCalculationDate <=@endDate) 
				AND (c.FeedId =@feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			order by c.SellingFeedCalculationDate ASC

		END


		if @isPartial = 1 AND (@startDate !='' AND @endDate ='' AND @categoryId !='' AND @feedId ='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingReportFeedCategoryId =@categoryId)  
				AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			)
			order by c.SellingFeedCalculationDate ASC

		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate ='' AND @categoryId ='' AND @feedId !='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate ) AND 
				(c.FeedId =@feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			order by c.SellingFeedCalculationDate ASC

		END

	if @isPartial = 1 AND (@startDate ='' AND @endDate !='' AND @categoryId !='' AND @feedId ='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate <=@endDate AND c.SellingReportFeedCategoryId =@categoryId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			order by c.SellingFeedCalculationDate ASC

		END


	if @isPartial = 1 AND (@startDate ='' AND @endDate !='' AND @categoryId !='' AND @feedId !='')

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate <=@endDate AND c.SellingReportFeedCategoryId =@categoryId) AND 
				(c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId
			order by c.SellingFeedCalculationDate ASC

		END

	if @isPartial = 0

		BEGIN

			SELECT
			c.[FeedSellingReportId],
			c.[SellingFeedReportNumber],
			c.[SellingReportName],
			c.[SellingReportName] + ' ('+c.[SellingFeedReportNumber]+')' AS FullName,
			convert(nvarchar(15),c.SellingFeedCalculationDate,106) AS SellingFeedCalculationDate,
			convert(nvarchar(100),c.SellingFeedWeightPerSack) As SellingFeedWeightPerSack,
			(select t.FeedCategoryName from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) FeedCategoryName,
			(select t.FeedCategoryImageUrl from [dbo].FeedCategories t where t.FeedCategoryId = c.SellingReportFeedCategoryId) MainFishImage,
			convert(nvarchar(100),c.SellingFeedSackNumber) As SellingFeedSackNumber,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalWeight)) As SellingFeedTotalWeight,
			convert(nvarchar(100),c.SellingFeedPricePerKg) As SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.SellingFeedTotalPrice)) As SellingFeedTotalPrice,
			convert(nvarchar(15),c.SellingFeedCreateDate,106) AS SellingFeedCreateDate,
			[dbo].[uf_GetFeedNameWithCategoryByFeedIdAndCatId](c.FeedId,c.SellingReportFeedCategoryId) FeedNameWithCat

			FROM [dbo].[FeedSellingReport] c where  c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

			order by c.SellingFeedCalculationDate ASC

		END


END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellerPDFInfo]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFishSellerPDFInfo] 
( 
	@SellDate date,
	@projectid int,
	@FishSellerId int
) 

AS 
BEGIN 

select [dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,
SellingFishName,dbo.uf_AddThousandSeparators(convert(nvarchar(500), TotalSellInKG)) TotalSellInKG, projectid, 
SellDate,FishSellerId, dbo.uf_AddThousandSeparators(TotalSellPrice) as totalAmount
, dbo.uf_AddThousandSeparators(FishAmountPaid) as PaidAmount,
PiecesPerKG,convert(nvarchar(500),TotalPiecesFish) TotalPiecesFish
, dbo.uf_AddThousandSeparators(FishAmountDue) as DueAmount,SellFishId,FishSellId
from FishSellingReport
	where SellDate= @SellDate and projectid=@projectid and FishSellerId=@FishSellerId
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellerPDFInfoTotalCalculation]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFishSellerPDFInfoTotalCalculation] 
( 
	@SellDate date,
	@projectid int,
	@FishSellerId int
) 

AS 
BEGIN 

	select 
	dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) TotalAmount,
	dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount,
	dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount
	from FishSellingReport
	where SellDate= @SellDate and projectid=@projectid and FishSellerId=@FishSellerId
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellerSearchResults]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFishSellerSearchResults] 
( 
	@searchText nvarchar(500)
) 

AS 
BEGIN 

	SELECT 
	f.[ID], 
	f.[Name], 
	f.[Age], 
	[dbo].MyHTMLDecode([dbo].[udf_StripHTML](f.Description)) as Description, 
	f.[ImageUrl],
	convert(varchar(10),f.[CreatedDate],103) CreaetdDate, 
	f.[CreatedId] 

	from [dbo].[FishSeller] f 

	where 
	( ( Convert(varchar(100),f.[ID]) = @searchText ) 
	OR ( (f.[Name] LIKE +'%'+ @searchText +'%' )
	OR (convert(varchar(10),f.[CreatedDate],103) LIKE @searchText +'%') ) )
	AND 
	f.IsDeleted =0



END



GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellingChartReport]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[up_GetFishSellingChartReport]
	
AS
BEGIN
	SELECT 
	convert(nvarchar(500),sum([TotalSellPrice])) As TotalSell,
	[dbo].[uf_GetUserProjectNameByAreaIdAndProjectId]([AreaId],[ProjectId]) AS ProjectName
	FROM [dbo].[FishSellingReport]
	GROUP BY [AreaId],[ProjectId]
	ORDER BY TotalSell
END



GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellingChartReportForUser]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFishSellingChartReportForUser] 
( 
	@areaId int,
	@projectId int
) 

AS 
BEGIN 

	select
	convert(nvarchar(500),sum(c.[TotalSellPrice])) As TotalSellPrice,
	convert(varchar(15), c.[SellDate],106) CalculatedDate
	FROM [dbo].[FishSellingReport] c
	 where c.[AreaId]=@areaId and c.[ProjectId]=@projectId
	group by c.[SellDate]
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellingColumnChartReportForAdmin]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFishSellingColumnChartReportForAdmin]
AS 
BEGIN 

	select
	convert(nvarchar(500),sum(c.[TotalSellPrice])) As TotalSellPrice,
	convert(varchar(15), c.[SellDate],106) CalculatedDate
	FROM [dbo].[FishSellingReport] c
	group by c.[SellDate]
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellingReportByFishId]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFishSellingReportByFishId] 
( 
	@areaId int,
	@projectId int
) 

AS 
BEGIN 
	select
	convert(nvarchar(500),sum(c.[TotalSellPrice])) As TotalSellPrice,
	[dbo].[uf_GetFishNameByFishId](c.SellFishId) FishName
	FROM [dbo].[FishSellingReport] c
	where c.[AreaId]=@areaId and c.[ProjectId]=@projectId
	group by c.SellFishId
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellingReportByParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFishSellingReportByParam] 
( 
	@startDate date,
	@endDate Date,
	@fishId int,
	@calCulationName nvarchar(max),
	@isPartial int=1,
	@areaId int,
	@projectId int
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @fishId !='' AND @calCulationName !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >=@startDate AND c.[SellDate] <=@endDate AND c.[SellFishId] =@fishId AND c.[SellingFishName] like @calCulationName
			)
			AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END

	if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @fishId ='' AND @calCulationName ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @fishId ='' and @calCulationName ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				
			)AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @fishId ='' and @calCulationName ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				
			)AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @fishId !='' and @calCulationName ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellFishId] = @fishId
				
			)AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @fishId ='' and @calCulationName !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellingFishName] like @calCulationName
			)AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @fishId ='' and @calCulationName !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[SellingFishName] like @calCulationName
			)AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @fishId ='' and @calCulationName !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellingFishName] like @calCulationName
			)AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @fishId ='' and @calCulationName !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[SellingFishName] like @calCulationName
			)AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END
		
		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @fishId !='' and @calCulationName ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[SellFishId] = @fishId
			)AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END




	  if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @fishId !='' and @calCulationName ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[SellFishId] = @fishId
				
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @fishId ='' and @calCulationName !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[SellingFishName] like @calCulationName
				
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END


		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @fishId !='' and @calCulationName !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[SellFishId] = IIF( @fishId != '' , @fishId ,@fishId ) )
				AND
				(c.[SellingFishName] like IIF( @calCulationName !='', @calCulationName ,@calCulationName ) )
				
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @fishId !='' and @calCulationName ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE c.[SellFishId] = @fishId AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @fishId ='' and @calCulationName !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[SellingFishName] like @calCulationName)
				
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
			order by c.[SellDate] ASC

		END


	if @isPartial = 0

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c where c.[AreaId]=@areaId and c.[ProjectId]=@projectId

			order by c.[FishSellId] desc

		END

END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellingReportForAdminByParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFishSellingReportForAdminByParam] 
( 
	@startDate date,
	@endDate Date,
	@isPartial int=1,
	@areaId int,
	@projectId int
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @projectId !='' AND @areaId !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,			
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >=@startDate AND c.[SellDate] <=@endDate AND c.[ProjectId] =@projectId AND c.[AreaId]=@areaId
			)
			order by c.[SellDate] ASC

		END

	if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' AND @areaId ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
			)
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @areaId ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				
			)
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId ='' and @areaId ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				
			)
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId !='' and @areaId ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[ProjectId] = @projectId
				
			)
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[AreaId]=@areaId
			)
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[AreaId]=@areaId
			)
			order by c.[SellDate] ASC

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[AreaId]=@areaId
			)
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[AreaId]=@areaId
			)
			order by c.[SellDate] ASC

		END
		
		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId !='' and @areaId ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[ProjectId] = @projectId
			)
			order by c.[SellDate] ASC

		END




	  if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId !='' and @areaId ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[ProjectId] = @projectId
				
			)
			order by c.[SellDate] ASC

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[AreaId]=@areaId
				
			)
			order by c.[SellDate] ASC

		END


		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId !='' and @areaId !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[ProjectId] = @projectId )
				AND
				(c.[AreaId]=@areaId)
				
			)
			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId !='' and @areaId ='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE c.[ProjectId] = @projectId

			order by c.[SellDate] ASC

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[AreaId]=@areaId)
				
			)
			order by c.[SellDate] ASC

		END


	if @isPartial = 0

		BEGIN

			SELECT
			c.[FishSellId],
			c.[SellingFishName],
			convert(nvarchar(15),c.[SellDate],106) AS FishSellingDate,
			convert(nvarchar(100),c.[TotalSellInKG]) As TotalSellInKG,
			(select t.[Name] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishName,
			(select t.[ImageUrl] from [dbo].[Fish] t where t.[ID] = c.[SellFishId]) MainFishImage,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[PricePerKG])) As PricePerKG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[TotalSellPrice])) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountPaid])) As FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(100),c.[FishAmountDue])) As FishAmountDue,
			(select t.Name + ' (' + convert(nvarchar(100),t.Age) from FishSeller t where t.ID = c.FishSellerId) SellerName,
			c.[FishImageUrl] As SellFishImageUrl,
			convert(nvarchar(15),c.[CalculatedDate],106) AS CalculatedDate,
			'All' As AllFishName,
			c.[AreaId],
			c.[ProjectId]

			FROM [dbo].[FishSellingReport] c

			order by c.[FishSellId] desc

		END

END




GO
/****** Object:  StoredProcedure [dbo].[up_GetFishSellingSellerReportForAdminByParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetFishSellingSellerReportForAdminByParam] 
( 
	@startDate date,
	@endDate Date,
	@isPartial int=1,
	@projectId int,
	@sellerID int
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @projectId !='' AND @sellerID !='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate, [dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c 
			WHERE (
				c.[SellDate] >=@startDate AND c.[SellDate] <=@endDate AND c.[ProjectId] =@projectId AND c.[FishSellerId] = @sellerID
			)
			group by SellDate,projectid,FishSellerId

		END

	if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' AND @sellerID ='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
			)
			group by SellDate,projectid,FishSellerId

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @sellerID ='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] >= @startDate
				
			)
			group by SellDate,projectid,FishSellerId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId ='' and @sellerID ='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] <= @endDate
				
			)
			group by SellDate,projectid,FishSellerId

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId !='' and @sellerID ='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[ProjectId] = @projectId
				
			)
			group by SellDate,projectid,FishSellerId

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @sellerID !='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[FishSellerId] = @sellerID
			)
			group by SellDate,projectid,FishSellerId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId ='' and @sellerID !='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[FishSellerId] = @sellerID
			)
			group by SellDate,projectid,FishSellerId

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @sellerID !='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[FishSellerId] = @sellerID
			)
			group by SellDate,projectid,FishSellerId

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' and @sellerID !='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[FishSellerId] = @sellerID
			)
			group by SellDate,projectid,FishSellerId

		END
		
		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId !='' and @sellerID ='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[ProjectId] = @projectId
			)
			group by SellDate,projectid,FishSellerId

		END




	  if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId !='' and @sellerID ='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[ProjectId] = @projectId
				
			) 

			group by SellDate,projectid,FishSellerId

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' and @sellerID !='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[FishSellerId] = @sellerID
				
			)
			group by SellDate,projectid,FishSellerId

		END


		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId !='' and @sellerID !='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				(c.[ProjectId] = @projectId )
				AND
				(c.[FishSellerId] = @sellerID)
				
			)
			group by SellDate,projectid,FishSellerId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId !='' and @sellerID ='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE c.[ProjectId] = @projectId
			group by SellDate,projectid,FishSellerId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId ='' and @sellerID !='')

		BEGIN

			select CONVERT(VARCHAR(20),  c.SellDate) as SDate,[dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c

			WHERE (
				(c.[FishSellerId] = @sellerID)
				
			)
			group by SellDate,projectid,FishSellerId

		END


	if @isPartial = 0

		BEGIN

			select  CONVERT(VARCHAR(20),  c.SellDate) as SDate, [dbo].[uf_GetFishSellerNameByFishShellerId] (FishSellerId) as Name,projectid, convert(nvarchar(15),c.[SellDate],106) SellDate,FishSellerId, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerTotalAmount] (projectid,SellDate,FishSellerId)) as totalAmount, 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerPaidAmount] (projectid,SellDate,FishSellerId)) as PaidAmount , 
			dbo.uf_AddThousandSeparators([dbo].[uf_GetFishSellerDueAmount] (projectid,SellDate,FishSellerId)) as DueAmount,
			[dbo].[uf_GetProjectNameByProjectId]([ProjectId]) ProjectName
			from FishSellingReport c
			group by SellDate,projectid,FishSellerId

		END

END




GO
/****** Object:  StoredProcedure [dbo].[up_GetProjectListByAreaId]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetProjectListByAreaId] 
( 
	
	@areaId int 
) 

AS 
BEGIN 

SELECT 
	f.ID, 
	f.Name
	from Project f 

	where 
	
	f.[AreaId] = @areaId 

	Order By f.ID ASC

END



GO
/****** Object:  StoredProcedure [dbo].[up_GetSegmentAreaListBySearch]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetSegmentAreaListBySearch] 
( 
	@searchText nvarchar(500), 
	@pageNumber int, 
	@pageSize int 
) 

AS 
BEGIN 

	SELECT 
	f.ID, 
	f.Name, 
	f.[Union_Name], 
	f.[WardNumber],
	f.ImageUrl, 
	convert(varchar(10),f.CreatedDate,103) CreaetdDate, 
	f.CreatedId 

	from Area f 

	where 
	( ( Convert(varchar(100),f.ID) = @searchText ) 

	OR ( (f.Name LIKE +'%'+ @searchText +'%' ) 
	OR (f.[Union_Name] LIKE +'%'+ @searchText ) 
	OR (f.[WardNumber] LIKE +'%'+ @searchText )
	OR (convert(varchar(10),f.CreatedDate,103) LIKE @searchText +'%') ) )
	AND f.IsDeleted = 0


END



GO
/****** Object:  StoredProcedure [dbo].[up_GetSegmentFeedListBySearch]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetSegmentFeedListBySearch] 
( 
	@searchText nvarchar(500), 
	@pageNumber int, 
	@pageSize int 
) 

AS 
BEGIN 

	SELECT 
	f.ID, 
	f.Name, 
	[dbo].MyHTMLDecode([dbo].[udf_StripHTML](f.Description)) as Description, 
	f.ImageUrl, 
	convert(varchar(10),f.CreatedDate,103) CreaetdDate, 
	f.CreatedId 

	from Feed f 

	where 
	( ( Convert(varchar(100),f.ID) = @searchText ) 

	OR ( (f.Name LIKE +'%'+ @searchText +'%' ) 
	OR (f.Description LIKE +'%'+ @searchText +'%' ) 
	OR (convert(varchar(10),f.CreatedDate,103) LIKE @searchText +'%') ) )
	AND f.IsDeleted = 0


END



GO
/****** Object:  StoredProcedure [dbo].[up_GetSegmentFishListBySearch]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetSegmentFishListBySearch] 
( 
	@searchText nvarchar(500), 
	@pageNumber int, 
	@pageSize int 
) 

AS 
BEGIN 

	SELECT 
	f.ID, 
	f.Name, 
	[dbo].MyHTMLDecode([dbo].[udf_StripHTML](f.Description)) as Description, 
	f.ImageUrl, 
	convert(varchar(10),f.CreatedDate,103) CreaetdDate, 
	f.CreatedId 

	from Fish f 

	where 
	( ( Convert(nvarchar(100),f.ID) = @searchText ) 

	OR ( (f.Name LIKE +'%'+ @searchText +'%' ) 
	OR (f.Description LIKE +'%'+ @searchText ) 
	OR (convert(nvarchar(10),f.CreatedDate,103) LIKE @searchText +'%') ) )
	AND f.IsDeleted = 0



END



GO
/****** Object:  StoredProcedure [dbo].[up_GetSegmentProjectListBySearch]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetSegmentProjectListBySearch] 
( 
	@searchText nvarchar(500), 
	@pageNumber int, 
	@pageSize int 
) 

AS 
BEGIN 

	SELECT 
	f.ID, 
	f.Name, 
	f.[AreaId],
	f.ImageUrl, 
	f.[Time],
	f.[Land],
	dbo.uf_AddThousandSeparators(convert(nvarchar, f.[Cost])) AS Cost,
	convert(varchar(10),f.CreatedDate,103) CreaetdDate, 
	f.CreatedId 

	from Project f 

	where 
	( ( Convert(varchar(100),f.ID) = @searchText ) 

	OR ( (f.Name LIKE +'%'+ @searchText +'%' )  
	OR (convert(varchar(10),f.CreatedDate,103) LIKE @searchText +'%') ) )
	AND f.IsDeleted = 0
END



GO
/****** Object:  StoredProcedure [dbo].[up_GetTotalFeedSellingReportsBySearchParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetTotalFeedSellingReportsBySearchParam] 
( 
	@startDate date,
	@endDate Date,
	@categoryId  varchar (10),
	@feedId varchar (10),
	@isPartial int=1,
	@areaId int,
	@projectId int
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @categoryId !='' AND @feedId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber

			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingFeedCalculationDate <=@endDate AND c.SellingReportFeedCategoryId =@categoryId) AND 
				(c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @categoryId !='' AND @feedId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingFeedCalculationDate <=@endDate AND c.SellingReportFeedCategoryId =@categoryId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END



	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @categoryId ='' AND @feedId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingFeedCalculationDate <=@endDate)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END

	if @isPartial = 1 AND (@startDate ='' AND @endDate ='' AND @categoryId !='' AND @feedId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c

			WHERE (
				c.SellingReportFeedCategoryId =@categoryId
				AND
				(c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END

	if @isPartial = 1 AND (@startDate ='' AND @endDate ='' AND @categoryId ='' AND @feedId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @categoryId ='' AND @feedId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingFeedCalculationDate <=@endDate) AND 
				(c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END


		if @isPartial = 1 AND (@startDate !='' AND @endDate ='' AND @categoryId !='' AND @feedId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate AND c.SellingReportFeedCategoryId =@categoryId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END


	if @isPartial = 1 AND (@startDate !='' AND @endDate ='' AND @categoryId ='' AND @feedId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate >=@startDate ) AND 
				(c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END

	if @isPartial = 1 AND (@startDate ='' AND @endDate !='' AND @categoryId !='' AND @feedId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate <=@endDate AND c.SellingReportFeedCategoryId =@categoryId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END


	if @isPartial = 1 AND (@startDate ='' AND @endDate !='' AND @categoryId !='' AND @feedId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c

			WHERE (
				(c.SellingFeedCalculationDate <=@endDate AND c.SellingReportFeedCategoryId =@categoryId) AND 
				(c.FeedId = @feedId)
				
			) AND c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END

	if @isPartial = 0

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[SellingFeedTotalWeight]))) As SellingFeedTotalWeight,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedTotalPrice]))) SellingFeedTotalPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedPricePerKg]))) SellingFeedPricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[SellingFeedSackNumber]))) SellingFeedSackNumber
			FROM [dbo].[FeedSellingReport] c  where c.[SellingFeedAreaId]= @areaId and [SellingFeedProjectId] = @projectId

		END


END




GO
/****** Object:  StoredProcedure [dbo].[up_GetTotalFishSellingReportByParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetTotalFishSellingReportByParam] 
( 
	@startDate date,
	@endDate Date,
	@fishId int,
	@calCulationName nvarchar(max),
	@isPartial int=1,
	@areaId int,
	@projectId int
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @fishId !='' AND @calCulationName !='')

		BEGIN
			SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >=@startDate AND c.[SellDate] <=@endDate AND c.[SellFishId] =@fishId AND c.[SellingFishName] like @calCulationName
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END
		
		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @fishId ='' AND @calCulationName ='')

		BEGIN
			SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @fishId ='' and @calCulationName ='')

		BEGIN
			SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @fishId ='' and @calCulationName ='')
			BEGIN
			SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE c.[SellDate] <= @endDate  AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @fishId !='' and @calCulationName ='')

		BEGIN
			SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellFishId] = @fishId
				
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @fishId ='' and @calCulationName !='')

		BEGIN
			SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellingFishName] like @calCulationName
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @fishId ='' and @calCulationName !='')

		BEGIN
			SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[SellingFishName] like @calCulationName
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @fishId ='' and @calCulationName !='')

		BEGIN
				SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellingFishName] like @calCulationName
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @fishId ='' and @calCulationName !='')

		BEGIN
					SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[SellingFishName] like @calCulationName
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END
		
		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @fishId !='' and @calCulationName ='')

		BEGIN
					SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[SellFishId] = @fishId
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END




	  if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @fishId !='' and @calCulationName ='')

		BEGIN
					SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[SellFishId] = @fishId
				
			)
			 AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId
		END


		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @fishId ='' and @calCulationName !='')

		BEGIN
					SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[SellingFishName] like @calCulationName
				
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END


		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @fishId !='' and @calCulationName !='')

		BEGIN
					SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[SellFishId] = IIF( @fishId != '' , @fishId ,@fishId ) )
				AND
				(c.[SellingFishName] like IIF( @calCulationName !='', @calCulationName ,@calCulationName ) )
				
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @fishId !='' and @calCulationName ='')

		BEGIN
					SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE c.[SellFishId] = @fishId AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @fishId ='' and @calCulationName !='')

		BEGIN
					SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[SellingFishName] like @calCulationName)
				
			) AND c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END



	if @isPartial = 0

		BEGIN
			SELECT 
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TOTAL_SELL_IN_KG,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c where  c.[AreaId]=@areaId and c.[ProjectId]=@projectId

		END
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetTotalFishSellingReportForAdminByParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetTotalFishSellingReportForAdminByParam] 
( 
	@startDate date,
	@endDate Date,
	@isPartial int=1,
	@areaId int,
	@projectId int
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @projectId !='' AND @areaId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue
			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >=@startDate AND c.[SellDate] <=@endDate AND c.[ProjectId] =@projectId AND c.[AreaId]=@areaId
			)

		END

	if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' AND @areaId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
			)

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @areaId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				
			)

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId ='' and @areaId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				
			)

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId !='' and @areaId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[ProjectId] = @projectId
				
			)

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[AreaId]=@areaId
			)
		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[AreaId]=@areaId
			)

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[AreaId]=@areaId
			)

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[AreaId]=@areaId
			)

		END
		
		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId !='' and @areaId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[ProjectId] = @projectId
			)

		END




	  if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId !='' and @areaId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[ProjectId] = @projectId
				
			)

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[AreaId]=@areaId
				
			)

		END


		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId !='' and @areaId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[ProjectId] = @projectId )
				AND
				(c.[AreaId]=@areaId)
				
			)

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId !='' and @areaId ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE c.[ProjectId] = @projectId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId ='' and @areaId !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[AreaId]=@areaId)
				
			)

		END


	if @isPartial = 0

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountPaid]))) FishAmountPaid,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[FishAmountDue]))) FishAmountDue

			FROM [dbo].[FishSellingReport] c


		END

END




GO
/****** Object:  StoredProcedure [dbo].[up_GetTotalFishSellingSellerReportForAdminByParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetTotalFishSellingSellerReportForAdminByParam] 
( 
	@startDate date,
	@endDate Date,
	@isPartial int=1,
	@projectId int,
	@sellerID int
) 

AS 
BEGIN 

	if @isPartial = 1 AND (@startDate !='' AND @endDate !='' AND @projectId !='' AND @sellerID !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c
			WHERE (
				c.[SellDate] >=@startDate AND c.[SellDate] <=@endDate AND c.[ProjectId] =@projectId AND c.[FishSellerId] = @sellerID
			)

		END

	if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' AND @sellerID ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
			)

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @sellerID ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				
			)

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId ='' and @sellerID ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				
			)

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId !='' and @sellerID ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[ProjectId] = @projectId
				
			)

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @sellerID !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[FishSellerId] = @sellerID
			)

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId ='' and @sellerID !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[FishSellerId] = @sellerID
			)

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate ='' and @projectId ='' and @sellerID !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[FishSellerId] = @sellerID
			)

		END

		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' and @sellerID !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[FishSellerId] = @sellerID
			)

		END
		
		if (@isPartial = 1 and  @startDate ='' and @endDate !='' and @projectId !='' and @sellerID ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] <= @endDate
				AND
				c.[ProjectId] = @projectId
			)

		END




	  if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId !='' and @sellerID ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[ProjectId] = @projectId
				
			) 

		END


		if (@isPartial = 1 and  @startDate !='' and @endDate !='' and @projectId ='' and @sellerID !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				c.[SellDate] >= @startDate
				AND
				c.[SellDate] <= @endDate
				AND
				c.[FishSellerId] = @sellerID
				
			)

		END


		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId !='' and @sellerID !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[ProjectId] = @projectId )
				AND
				(c.[FishSellerId] = @sellerID)
				
			)

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId !='' and @sellerID ='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE c.[ProjectId] = @projectId

		END

		if (@isPartial = 1 and  @startDate ='' and @endDate ='' and @projectId ='' and @sellerID !='')

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

			WHERE (
				(c.[FishSellerId] = @sellerID)
				
			)

		END


	if @isPartial = 0

		BEGIN

			SELECT
			dbo.uf_AddThousandSeparators(convert(nvarchar(500), SUM(c.[TotalSellInKG]))) As TotalSellInKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalSellPrice]))) As TotalSellPrice,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[PricePerKG]))) PricePerKg,
			dbo.uf_AddThousandSeparators(convert(nvarchar(500),SUM(c.[TotalPiecesFish]))) TotalPiecesFish

			FROM [dbo].[FishSellingReport] c

		END

END




GO
/****** Object:  StoredProcedure [dbo].[up_GetTrashDataByTableID]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[up_GetTrashDataByTableID](
	@tableID int,
	@isAllData int
)
AS
BEGIN

	DECLARE @TrashDataTable TABLE
	(
		RowId int NOT NULL identity(1,1),
		ID int, 
		Name nvarchar(max),
		ImageUrl nvarchar(max),
		TableName nvarchar(max),
		TableID int,
		CreatedDate nvarchar(13)
	)

	if @isAllData = 0

		BEGIN

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'Fish', 1, convert(varchar(10),CreatedDate,103)
			FROM [Fish] where isDeleted=1;


			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'Feed', 2, convert(varchar(10),CreatedDate,103)
			FROM [Feed] where isDeleted=1;

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT FeedCategoryId, FeedCategoryName, FeedCategoryImageUrl, 'FeedCategories', 3, convert(varchar(10),CreatedDate,103)
			FROM [FeedCategories] where isDeleted=1;

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'Area', 4, convert(varchar(10),CreatedDate,103)
			FROM [Area] where isDeleted=1;

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'Project', 5, convert(varchar(10),CreatedDate,103)
			FROM [Project] where isDeleted=1;

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'FishSeller', 6, convert(varchar(10),CreatedDate,103)
			FROM [FishSeller] where isDeleted=1;

			select * from @TrashDataTable t

			order by RowId

		END

	if @isAllData = 1 and @tableID = 1

		BEGIN

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'Fish', 1, convert(varchar(10),CreatedDate,103)
			FROM [Fish] where isDeleted=1;

			select * from @TrashDataTable t
			order by RowId

		END

	if @isAllData = 1 and @tableID = 2

		BEGIN

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'Feed', 2, convert(varchar(10),CreatedDate,103)
			FROM [Feed] where isDeleted=1;

			select * from @TrashDataTable t
			order by RowId
		END

	if @isAllData = 1 and @tableID = 3

		BEGIN

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT FeedCategoryId, FeedCategoryName, FeedCategoryImageUrl, 'FeedCategories', 3, convert(varchar(10),CreatedDate,103)
			FROM [FeedCategories] where isDeleted=1;
			
			select * from @TrashDataTable t
			order by RowId
		END

	if @isAllData = 1 and @tableID = 4

		BEGIN

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'Area', 4, convert(varchar(10),CreatedDate,103)
			FROM [Area] where isDeleted=1;
			
			select * from @TrashDataTable t
			order by RowId
		END

	if @isAllData = 1 and @tableID = 5

		BEGIN

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'Project', 5, convert(varchar(10),CreatedDate,103)
			FROM [Project] where isDeleted=1;
			
			select * from @TrashDataTable t
			order by RowId
		END

	if @isAllData = 1 and @tableID = 6

		BEGIN

			INSERT INTO @TrashDataTable (ID, Name,ImageUrl,TableName, TableID, CreatedDate)
			SELECT ID, Name, ImageUrl, 'FishSeller', 6, convert(varchar(10),CreatedDate,103)
			FROM [FishSeller] where isDeleted=1;
			
			select * from @TrashDataTable t
			order by RowId
		END

END


GO
/****** Object:  StoredProcedure [dbo].[up_GetUserListBySearchParam]    Script Date: 1/30/2019 11:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[up_GetUserListBySearchParam] 
( 
	@searchText nvarchar(500)
) 

AS 
BEGIN 

	SELECT 
	u.[UserID], 
	u.[FirstName],
	u.[FirstName] + ' ' + u.[LastName] as UserFullName, 
	u.[PhoneNumber],
	u.[UserImagePath],
	[dbo].[uf_GetUserProjectNameByAreaIdAndProjectId] (u.[AreaID],u.[ProjectID]) AS ProjectDetails

	from [dbo].[Users] u

	where 
		( ( Convert(nvarchar(100),u.[UserID]) = @searchText )
		OR (u.[FirstName] LIKE +'%'+ @searchText +'%' )
		OR (u.[LastName] LIKE +'%'+ @searchText +'%' ))

END



GO

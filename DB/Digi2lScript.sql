USE [master]
GO
/****** Object:  Database [Digi2l_DB]    Script Date: 1/27/2022 5:10:30 PM ******/
CREATE DATABASE [Digi2l_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Digi2l_DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Digi2l_DB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Digi2l_DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Digi2l_DB_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Digi2l_DB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Digi2l_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Digi2l_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Digi2l_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Digi2l_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Digi2l_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Digi2l_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [Digi2l_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Digi2l_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Digi2l_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Digi2l_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Digi2l_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Digi2l_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Digi2l_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Digi2l_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Digi2l_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Digi2l_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Digi2l_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Digi2l_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Digi2l_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Digi2l_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Digi2l_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Digi2l_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Digi2l_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Digi2l_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [Digi2l_DB] SET  MULTI_USER 
GO
ALTER DATABASE [Digi2l_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Digi2l_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Digi2l_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Digi2l_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Digi2l_DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Digi2l_DB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Digi2l_DB', N'ON'
GO
ALTER DATABASE [Digi2l_DB] SET QUERY_STORE = OFF
GO
USE [Digi2l_DB]
GO
/****** Object:  User [UTC]    Script Date: 1/27/2022 5:10:30 PM ******/
CREATE USER [UTC] FOR LOGIN [UTC] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [NT AUTHORITY\SYSTEM]    Script Date: 1/27/2022 5:10:30 PM ******/
CREATE USER [NT AUTHORITY\SYSTEM] FOR LOGIN [NT AUTHORITY\SYSTEM] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [NT AUTHORITY\SYSTEM]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblABBPriceMaster]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblABBPriceMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ZohoPriceMasterID] [nvarchar](255) NULL,
	[ABBPriceCode] [nvarchar](255) NULL,
	[ABBPlanName] [nvarchar](255) NULL,
	[PlanPeriod] [nvarchar](255) NULL,
	[TotalPlan(Months)] [nvarchar](255) NULL,
	[PriceStartDate] [nvarchar](255) NULL,
	[PriceEndDate] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblABBRegistration]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblABBRegistration](
	[ABBRegistrationId] [int] IDENTITY(1,1) NOT NULL,
	[ZohoABBRegistrationId] [nvarchar](50) NULL,
	[BusinessUnitId] [int] NULL,
	[RegdNo] [nvarchar](10) NULL,
	[SponsorOrderNo] [nvarchar](150) NULL,
	[CustFirstName] [varchar](150) NULL,
	[CustLastName] [varchar](150) NULL,
	[CustMobile] [nvarchar](50) NULL,
	[CustEmail] [nvarchar](250) NULL,
	[CustAddress1] [varchar](250) NULL,
	[CustAddress2] [varchar](250) NULL,
	[Landmark] [nvarchar](250) NULL,
	[CustPinCode] [nvarchar](250) NULL,
	[CustCity] [varchar](150) NULL,
	[CustState] [varchar](150) NULL,
	[NewProductCategoryId] [int] NULL,
	[NewProductCategoryTypeId] [int] NULL,
	[NewBrandId] [int] NULL,
	[NewSize] [nvarchar](50) NULL,
	[ProductSrNo] [nvarchar](50) NULL,
	[ModelNumber] [nvarchar](50) NULL,
	[ABBPlanName] [nvarchar](150) NULL,
	[HSNCode] [nvarchar](50) NULL,
	[InvoiceDate] [date] NULL,
	[InvoiceNo] [nvarchar](150) NULL,
	[NewPrice] [decimal](10, 2) NULL,
	[ABBFees] [decimal](10, 2) NULL,
	[OrderType] [nvarchar](150) NULL,
	[SponsorProdCode] [nvarchar](50) NULL,
	[ABBPriceId] [int] NULL,
	[UploadDateTime] [datetime] NULL,
	[BusinessPartnerId] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[YourRegistrationNo] [nvarchar](50) NULL,
	[InvoiceImage] [nvarchar](max) NULL,
	[ABBPlanPeriod] [nvarchar](10) NULL,
	[NoOfClaimPeriod] [nvarchar](10) NULL,
	[ProductNetPrice] [decimal](10, 2) NULL,
 CONSTRAINT [PK__tblABBRegistration__D65247C2BDC3EBF0] PRIMARY KEY CLUSTERED 
(
	[ABBRegistrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBizlogTicket]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBizlogTicket](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BizlogTicketNo] [nvarchar](100) NOT NULL,
	[SponsrorOrderNo] [nvarchar](100) NULL,
	[ConsumerName] [nvarchar](100) NULL,
	[ConsumerComplaintNumber] [nvarchar](100) NULL,
	[AddressLine1] [nvarchar](100) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[City] [nvarchar](100) NULL,
	[Pincode] [nvarchar](100) NULL,
	[TelephoneNumber] [nvarchar](100) NULL,
	[RetailerPhoneNo] [nvarchar](100) NULL,
	[AlternateTelephoneNumber] [nvarchar](100) NULL,
	[EmailId] [nvarchar](100) NULL,
	[DateOfPurchase] [nvarchar](100) NULL,
	[DateOfComplaint] [nvarchar](100) NULL,
	[NatureOfComplaint] [nvarchar](100) NULL,
	[IsUnderWarranty] [nvarchar](100) NULL,
	[Brand] [nvarchar](100) NULL,
	[ProductCategory] [nvarchar](100) NULL,
	[ProductName] [nvarchar](100) NULL,
	[ProductCode] [nvarchar](100) NULL,
	[Model] [nvarchar](100) NULL,
	[IdentificationNo] [nvarchar](100) NULL,
	[DropLocation] [nvarchar](100) NULL,
	[DropLocAddress1] [nvarchar](100) NULL,
	[DropLocAddress2] [nvarchar](100) NULL,
	[DropLocCity] [nvarchar](100) NULL,
	[DropLocState] [nvarchar](100) NULL,
	[DropLocPincode] [nvarchar](100) NULL,
	[DropLocContactPerson] [nvarchar](100) NULL,
	[DropLocContactNo] [nvarchar](100) NULL,
	[DropLocAlternateNo] [nvarchar](100) NULL,
	[PhysicalEvaluation] [nvarchar](100) NULL,
	[TechEvalRequired] [nvarchar](100) NULL,
	[Value] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK__tblBizlo__3214EC07524CF2E9] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBizlogTicketStatus]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBizlogTicketStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BizlogTicketNo] [nvarchar](100) NOT NULL,
	[Status] [nvarchar](200) NULL,
	[Remarks] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBrand]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBrand](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBusinessPartner]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessPartner](
	[BusinessPartnerId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[StoreCode] [nvarchar](150) NULL,
	[QRCodeURL] [text] NULL,
	[QRImage] [text] NULL,
	[ContactPersonFirstName] [varchar](150) NULL,
	[ContactPersonLastName] [varchar](150) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](150) NULL,
	[AddressLine1] [nvarchar](250) NULL,
	[AddressLine2] [nvarchar](250) NULL,
	[Pincode] [nvarchar](150) NULL,
	[City] [nvarchar](150) NULL,
	[State] [nvarchar](150) NULL,
	[BusinessUnitId] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK__tblBusinessPartner__D65247C2BDC3EBF0] PRIMARY KEY CLUSTERED 
(
	[BusinessPartnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBusinessUnit]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessUnit](
	[BusinessUnitId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[RegistrationNumber] [nvarchar](150) NULL,
	[QRCodeURL] [text] NULL,
	[LogoName] [varchar](200) NULL,
	[ContactPersonFirstName] [varchar](150) NULL,
	[ContactPersonLastName] [varchar](150) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](150) NULL,
	[AddressLine1] [nvarchar](250) NULL,
	[AddressLine2] [nvarchar](250) NULL,
	[Pincode] [nvarchar](150) NULL,
	[City] [nvarchar](150) NULL,
	[State] [nvarchar](150) NULL,
	[LoginId] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK__tblBusinessUnit__D65247C2BDC3EBF0] PRIMARY KEY CLUSTERED 
(
	[BusinessUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCurrentAuthtoken]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCurrentAuthtoken](
	[CurrentAuthtokenId] [int] IDENTITY(1,1) NOT NULL,
	[CurrentAuthTokenName] [nvarchar](400) NULL,
	[CurrentAuthToken] [nvarchar](400) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CurrentAuthtokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCustomerDetails]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCustomerDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[ZipCode] [nvarchar](255) NULL,
	[Address1] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblErrorLog]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblErrorLog](
	[ErrorLogId] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [nvarchar](200) NULL,
	[MethodName] [nvarchar](200) NULL,
	[SponsorOrderNo] [nvarchar](100) NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK__tblError__D65247C2BDC3EBF0] PRIMARY KEY CLUSTERED 
(
	[ErrorLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblEVCApproved]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEVCApproved](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ZohoEVCApprovedId] [nvarchar](255) NULL,
	[BussinessName] [nvarchar](255) NULL,
	[EVCRegdNo] [nvarchar](50) NULL,
	[ContactPerson] [nvarchar](255) NULL,
	[EVCMobileNumber] [nvarchar](50) NULL,
	[EmailID] [nvarchar](255) NULL,
	[RegdAddressLine1] [nvarchar](255) NULL,
	[RegdAddressLine2] [nvarchar](255) NULL,
	[PinCode] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[EVCWalletAmount] [nvarchar](50) NULL,
	[ContactPersonAddress] [nvarchar](max) NULL,
	[UploadGSTRegistration] [nvarchar](max) NULL,
	[CopyofCancelledCheque] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblExchangeOrder]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblExchangeOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](255) NULL,
	[ZohoSponsorOrderId] [nvarchar](255) NULL,
	[OrderStatus] [nvarchar](255) NULL,
	[CustomerDetailsId] [int] NULL,
	[ProductTypeId] [int] NULL,
	[BrandId] [int] NULL,
	[Bonus] [nvarchar](50) NULL,
	[SponsorOrderNumber] [nvarchar](255) NULL,
	[EstimatedDeliveryDate] [nvarchar](255) NULL,
	[ProductCondition] [nvarchar](50) NULL,
	[LoginID] [int] NULL,
	[ExchPriceCode] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblImages]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblImages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BizlogTicketId] [int] NULL,
	[SponsorId] [int] NULL,
	[ImageURL] [text] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblMessageDetail]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMessageDetail](
	[MessageDetailId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NULL,
	[PhoneNumber] [nvarchar](15) NULL,
	[Message] [nvarchar](150) NULL,
	[ResponseJSON] [nvarchar](600) NULL,
	[SendDate] [datetime] NULL,
	[MessageType] [tinyint] NULL,
	[Email] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblModOfPayment]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblModOfPayment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModeofPayment] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPickupStatus]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPickupStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPinCode]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPinCode](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ZipCode] [int] NULL,
	[Location] [nvarchar](255) NULL,
	[HubControl] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPriceMaster]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPriceMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ZohoPriceMasterId] [nvarchar](255) NULL,
	[ExchPriceCode] [nvarchar](255) NULL,
	[ProductCategoryId] [int] NULL,
	[ProductCat] [nvarchar](255) NULL,
	[ProductTypeId] [int] NULL,
	[ProductType] [nvarchar](255) NULL,
	[ProductTypeCode] [nvarchar](255) NULL,
	[BrandName-1] [nvarchar](255) NULL,
	[BrandName-2] [nvarchar](255) NULL,
	[BrandName-3] [nvarchar](255) NULL,
	[BrandName-4] [nvarchar](255) NULL,
	[Quote-P-High] [nvarchar](255) NULL,
	[Quote-Q-High] [nvarchar](255) NULL,
	[Quote-R-High] [nvarchar](255) NULL,
	[Quote-S-High] [nvarchar](255) NULL,
	[Quote-P] [nvarchar](255) NULL,
	[Quote-Q] [nvarchar](255) NULL,
	[Quote-R] [nvarchar](255) NULL,
	[Quote-S] [nvarchar](255) NULL,
	[PriceStartDate] [nvarchar](255) NULL,
	[PriceEndDate] [nvarchar](255) NULL,
	[OtherBrand] [nvarchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK__tblProdu__3214EC074D67AF83] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductCategory]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Code] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductType]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Code] [nvarchar](255) NULL,
	[Size] [nvarchar](255) NULL,
	[ProductCatId] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK__tblProdu__3214EC07BF1A99F2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProgramMaster]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProgramMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ZohoProgramMasterID] [nvarchar](255) NULL,
	[LoginCredentials] [nvarchar](255) NULL,
	[SponsorBussinessName] [nvarchar](255) NULL,
	[ProgramCode] [nvarchar](255) NULL,
	[Exch] [nvarchar](255) NULL,
	[ExchPriceCode] [nvarchar](255) NULL,
	[PaymentTo] [nvarchar](255) NULL,
	[ABB] [nvarchar](255) NULL,
	[ABBPriceCode] [nvarchar](255) NULL,
	[ProgStartDate] [nvarchar](255) NULL,
	[ProgEndDate] [nvarchar](255) NULL,
	[PreeQC] [nvarchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK__tblProgr__3214EC0713C61DD6] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Login] ON 
GO
INSERT [dbo].[Login] ([id], [username], [password]) VALUES (1, N'infobyd@digimart.co.in', N'Info1234!')
GO
INSERT [dbo].[Login] ([id], [username], [password]) VALUES (3, N'samsung@digi2l.co.in', N'Samsung!2345')
GO
SET IDENTITY_INSERT [dbo].[Login] OFF
GO
SET IDENTITY_INSERT [dbo].[tblABBPriceMaster] ON 
GO
INSERT [dbo].[tblABBPriceMaster] ([Id], [ZohoPriceMasterID], [ABBPriceCode], [ABBPlanName], [PlanPeriod], [TotalPlan(Months)], [PriceStartDate], [PriceEndDate], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'4186686000000214007', N'TCQ-ABB-PR-001', N'TATA CLiQ 2021', N'60', N'60', N'12-Jul-2021', N'31-Dec-2021', 1, NULL, CAST(N'2021-07-29T15:23:44.000' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblABBPriceMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[tblABBRegistration] ON 
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1, N'4186686000001624003', 1, NULL, N'566785456788', N'priya', N'patel', N'8879067856', N'priyanka@infobyd.in', N'test-2 line 1', N'palasia', N'near krishna electronics', N'452010', N'Indore', N'Madhya Pradesh', 4, 9, 2, N'23', N'56ghy6', N'6565hgh', N'6hg7h', N'67hh', CAST(N'2022-01-20' AS Date), N'657ghy', CAST(560.00 AS Decimal(10, 2)), CAST(40.00 AS Decimal(10, 2)), N'ABB', N'76hju6', 567, CAST(N'2022-01-20T21:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-20T21:01:39.000' AS DateTime), NULL, CAST(N'2022-01-20T21:01:39.000' AS DateTime), NULL, NULL, N'65', N'56', CAST(600.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (2, N'4186686000001626003', 1, NULL, N'576576576765', N'priya', N'patel', N'8879067856', N'priyanka@infobyd.in', N'test-2 line 1', NULL, NULL, N'500010', N'Geraldton', N'Western Australia', 1, 2, 2, N'23', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'ABB', NULL, NULL, NULL, 3, 1, NULL, CAST(N'2022-01-20T21:09:26.000' AS DateTime), NULL, CAST(N'2022-01-20T21:09:26.000' AS DateTime), NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (3, N'4186686000001627003', 1, N'TTPSDU', N'5676556565', N'meenu', N'verma', N'6876888778', N'priya@gmail.com', NULL, NULL, NULL, N'452011', N'Indore', N'Madhya Pradesh', 3, 11, 1, N'23', N'6565gffb', N'5665ggf', N'55tgt', N'65767hyy', CAST(N'2022-01-20' AS Date), N'56678hy', CAST(1200.00 AS Decimal(10, 2)), CAST(100.00 AS Decimal(10, 2)), N'ABB', N'56gh', 5667, CAST(N'2022-01-20T21:29:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-20T21:30:25.000' AS DateTime), NULL, CAST(N'2022-01-20T21:30:25.000' AS DateTime), NULL, NULL, N'54', N'56', CAST(1300.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (4, N'4186686000001622015', 1, N'MPN8DU', N'67778897', N'Pooja', N'Sharma', N'4658879898', N'pooja@gmail.com', N'56, AB road', N'vijay nagar', NULL, N'452010', N'Indore', N'Madhya Pradesh', 4, 10, 2, N'23', N'56hg', N'rty65', N'545b', N'54fb', CAST(N'2022-01-21' AS Date), N'54655656gb', CAST(560.00 AS Decimal(10, 2)), CAST(40.00 AS Decimal(10, 2)), N'ABB', N'6776gjh', 5666, CAST(N'2022-01-21T21:34:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-20T21:34:29.000' AS DateTime), NULL, CAST(N'2022-01-20T21:34:29.000' AS DateTime), NULL, NULL, N'55', N'54', CAST(600.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (5, N'4186686000001655009', NULL, N'884QMU', NULL, N'priyanka', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', N'near krishna traders', N'500010', N'Indore', N'Madhya Pradesh', 1, 2, 2002, NULL, N'454654', N'bgt567', N'BSH-BH-84-2022-2', NULL, CAST(N'2022-01-24' AS Date), N'54fgh56', NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-24T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-24T19:52:42.000' AS DateTime), NULL, CAST(N'2022-01-24T19:52:42.000' AS DateTime), NULL, NULL, N'84', N'6', CAST(450.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1005, N'4186686000001662003', 1, N'823UKU', NULL, N'priya', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Geraldton', N'Western Australia', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-25T11:44:30.000' AS DateTime), NULL, CAST(N'2022-01-25T11:44:30.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1006, N'4186686000001664003', 1, N'500NKU', NULL, NULL, NULL, N'8319010116', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-25T13:19:50.000' AS DateTime), NULL, CAST(N'2022-01-25T13:19:50.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1007, NULL, 1, N'688BUU', NULL, N'priyanka', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-25T13:27:37.000' AS DateTime), NULL, NULL, NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1008, N'4186686000001665003', 1, N'874UXU', NULL, N'priyanka', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-25T13:53:49.000' AS DateTime), NULL, CAST(N'2022-01-25T13:53:49.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1009, N'4186686000001664007', 1, N'650CRU', NULL, N'test1', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 4, 1, NULL, CAST(N'2022-01-25T14:00:33.000' AS DateTime), NULL, CAST(N'2022-01-25T14:00:33.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1010, N'4186686000001655017', 1, N'354GIU', NULL, N'test2', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 4, 1, NULL, CAST(N'2022-01-25T14:02:49.000' AS DateTime), NULL, CAST(N'2022-01-25T14:02:49.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1011, N'4186686000001666003', 1, N'464QHU', NULL, N'priyanka', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, CAST(N'2022-01-25' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-25T14:16:14.000' AS DateTime), NULL, CAST(N'2022-01-25T14:16:14.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1012, N'4186686000001663019', 1, N'464QHU', NULL, N'priyanka', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, CAST(N'2022-01-25' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-25T14:18:44.000' AS DateTime), NULL, CAST(N'2022-01-25T14:18:44.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1013, N'4186686000001665007', 1, N'690VAU', NULL, N'test5', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, CAST(N'2022-01-25' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 7, 1, NULL, CAST(N'2022-01-25T14:19:34.000' AS DateTime), NULL, CAST(N'2022-01-25T14:19:34.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1014, N'4186686000001655021', 1, N'690VAU', NULL, N'test5', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, CAST(N'2022-01-25' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 7, 1, NULL, CAST(N'2022-01-25T14:22:22.000' AS DateTime), NULL, CAST(N'2022-01-25T14:22:22.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1015, N'4186686000001665011', 1, N'835YEU', NULL, N'TEST7', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 2, 7, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, CAST(N'2022-01-25' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 9, 1, NULL, CAST(N'2022-01-25T14:26:35.000' AS DateTime), NULL, CAST(N'2022-01-25T14:26:35.000' AS DateTime), NULL, NULL, N'84', N'6', CAST(1800.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1016, N'4186686000001658009', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, CAST(N'2022-01-25T14:27:57.000' AS DateTime), NULL, CAST(N'2022-01-25T14:27:57.000' AS DateTime), NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1017, N'4186686000001655025', 1, N'811ZAU', NULL, N'priyanka', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-25T15:10:56.000' AS DateTime), NULL, CAST(N'2022-01-25T15:10:56.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1018, N'4186686000001662007', 1, N'657FQU', NULL, N'priyanka', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, NULL, N'BSH-BH-84-2022-2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-25T16:00:04.000' AS DateTime), NULL, CAST(N'2022-01-25T16:00:04.000' AS DateTime), NULL, NULL, N'84', N'6', NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1019, N'4186686000001662011', NULL, NULL, NULL, N'priyanka', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 4, 9, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 1, 1, NULL, CAST(N'2022-01-25T18:01:36.000' AS DateTime), NULL, CAST(N'2022-01-25T18:01:36.000' AS DateTime), NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1020, N'4186686000001665031', 1, N'210AOU', NULL, N'testn', N'verma', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', N'near krishna traders', N'500010', N'Indore', N'Madhya Pradesh', 2, 7, 2002, NULL, N'4545rt', N'45drt', N'BSH-BH-84-2022-2', NULL, CAST(N'2022-01-25' AS Date), N'3465465', NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-25T18:13:12.000' AS DateTime), NULL, CAST(N'2022-01-25T18:13:12.000' AS DateTime), NULL, NULL, N'84', N'6', CAST(2300.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1021, N'4186686000001665035', 1, N'475VJU', NULL, N'mysha', N'patel', N'7415247205', N'priyanka28iet@gmail.com', N'Ring road', NULL, NULL, N'452011', N'Bhopal', N'Madhya Pradesh', 2, 7, 2002, NULL, N'56778TYU', N'567TYUI', N'BSH-BH-84-2022-2', NULL, CAST(N'2022-01-25' AS Date), N'856987965', NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 10, 1, NULL, CAST(N'2022-01-25T21:20:01.000' AS DateTime), NULL, CAST(N'2022-01-25T21:20:01.000' AS DateTime), NULL, NULL, N'84', N'6', CAST(1500.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1022, N'4186686000001655035', 1, N'423YCU', NULL, N'priyanka', N'patel', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 4, 10, 2002, NULL, N'56656gty', N'5676ghgh', N'BSH-BH-84-2022-2', NULL, CAST(N'2022-01-26' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-25T00:00:00.000' AS DateTime), 15, 1, NULL, CAST(N'2022-01-25T21:37:30.000' AS DateTime), NULL, CAST(N'2022-01-25T21:37:30.000' AS DateTime), NULL, NULL, N'84', N'6', CAST(3400.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1023, N'4186686000001675017', 1, N'300EEZ', NULL, N'trs', N'fdf', N'7415247205', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 4, 10, 2002, NULL, NULL, N'657657', NULL, NULL, CAST(N'2022-01-27' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-27T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-27T16:45:51.000' AS DateTime), NULL, CAST(N'2022-01-27T16:45:51.000' AS DateTime), NULL, N'20220127164551454.png', N'84', N'6', CAST(4500.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[tblABBRegistration] ([ABBRegistrationId], [ZohoABBRegistrationId], [BusinessUnitId], [RegdNo], [SponsorOrderNo], [CustFirstName], [CustLastName], [CustMobile], [CustEmail], [CustAddress1], [CustAddress2], [Landmark], [CustPinCode], [CustCity], [CustState], [NewProductCategoryId], [NewProductCategoryTypeId], [NewBrandId], [NewSize], [ProductSrNo], [ModelNumber], [ABBPlanName], [HSNCode], [InvoiceDate], [InvoiceNo], [NewPrice], [ABBFees], [OrderType], [SponsorProdCode], [ABBPriceId], [UploadDateTime], [BusinessPartnerId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [YourRegistrationNo], [InvoiceImage], [ABBPlanPeriod], [NoOfClaimPeriod], [ProductNetPrice]) VALUES (1024, N'4186686000001676011', 1, N'875GXZ', NULL, N'fghgf', N'gfhg', N'8319010116', N'priyanka@infobyd.in', N'test-2 line 1', N'ab road', NULL, N'500010', N'Indore', N'Madhya Pradesh', 1, 1, 2002, NULL, NULL, N'67gh', NULL, NULL, CAST(N'2022-01-27' AS Date), NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2022-01-27T00:00:00.000' AS DateTime), 3, 1, NULL, CAST(N'2022-01-27T16:58:27.000' AS DateTime), NULL, CAST(N'2022-01-27T16:58:27.000' AS DateTime), NULL, N'20220127165827258.png', N'84', N'6', CAST(6765.00 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [dbo].[tblABBRegistration] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBizlogTicket] ON 
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3004, N'UTC-MOB-111-7OHPZ', N'SAM03356', N'AnuTest Sharma', N'123456', N'12, MG road', N'Saket Nagar', N'Kolkata', N'700088', N'8767586997', NULL, NULL, N'anu@infobyd.in', NULL, N'23-09-2021', N'Pick And Drop (One Way)', N'No', N'Samsung', N'mobile', N'Window AC', NULL, N'Window AC', N'AHH923', N'EVC-Test-03', N'Biz Log Local Warehouse', N'Biz Log Local Warehouse', N'CALCUTTA', N'WEST BENGAL', N'700075', N'HJ', N'+918657434260', NULL, N'No', N'No', N'0.00', 1, NULL, CAST(N'2021-09-23T18:01:04.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3005, N'UTC-MOB-101-SG1TO', N'11364785640', N'Indu N', N'123456', N'Line 1', N'Line 2', N'Bangalore', N'560030', N'7291919293', NULL, NULL, N'ecomsiel@gmail.com', NULL, N'22-09-2021', N'Pick And Drop (One Way)', N'No', N'Samsung', N'mobile', N'LCD Television', NULL, N'LCD Television', N'QSH109', N'Test-1', N'Shop address 1', N'Shop address 2', N'Mumbai', N'MAHARASHTRA', N'400070', N'JH', N'+918652241429', NULL, N'No', N'No', N'', 1, NULL, CAST(N'2021-09-28T15:02:38.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3006, N'UTC-AC-101-UH8O2', N'SAM3', N'priya ', N'123456', N'test1', N'test2', N'Banglore', N'560027', N'+917434475590', NULL, NULL, N'priya@infobyd.in', NULL, N'29-09-2021', N'Pick And Drop (ONE WAY)', N'No', N'Samsung', N'Air Conditioner', N'Window AC', NULL, N'Window AC', N'FQQ738', N'Test-2', N'test-2 line 1', N'test-2 line 2', N'HYD', N'TELANGANA', N'500010', N'SPD', N'+917777777777', NULL, N'No', N'No', N'0.00', 1, NULL, CAST(N'2021-09-29T14:06:57.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3007, N'UTC-RFG-101-2IA7E', N'SAM3', N'priya ', N'123456', N'test1', N'test2', N'Banglore', N'560027', N'+917434475590', NULL, NULL, N'priya@infobyd.in', NULL, N'29-09-2021', N'Pick And Drop (ONE WAY)', N'No', N'Samsung', N'Refrigerator', N'Double Door', NULL, N'Double Door', N'FQQ738', N'Test-2', N'test-2 line 1', N'test-2 line 2', N'HYD', N'TELANGANA', N'500010', N'SPD', N'+917777777777', NULL, N'No', N'No', N'0.00', 1, NULL, CAST(N'2021-09-29T14:11:09.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3008, N'UTC-AC-101-EZOH5', N'SAM4', N'Test 1 ', N'123456', N'add1', N'add2', N'banglore', N'560027', N'+919978945678', NULL, NULL, N'priya@infobyd.in', NULL, N'29-09-2021', N'Pick And Drop (ONE WAY)', N'No', N'LG', N'Air Conditioner', N'Window AC', NULL, N'Window AC', N'JHL351', N'Test-2', N'test-2 line 1', N'test-2 line 2', N'HYD', N'TELANGANA', N'500010', N'SPD', N'+917777777777', NULL, N'No', N'No', N'0.00', 1, NULL, CAST(N'2021-09-29T20:53:05.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3009, N'UTC-AC-111-1RUGU', N'SAM2', N'AnuTest Sharma', N'123456', N'12, MG road', N'Saket Nagar', N'Kolkata', N'700088', N'8767586997', NULL, NULL, N'anu@infobyd.in', NULL, N'29-09-2021', N'Pick And Drop (One Way)', N'No', N'Samsung', N'Air Conditioner', N'Window AC', NULL, N'Window AC', N'YHT473', N'Test-1', N'Correct address-1', N'Correct address-2', N'Mumbai', N'MAHARASHTRA', N'400070', N'JH', N'+918652241429', NULL, N'No', N'No', N'', 1, NULL, CAST(N'2021-09-30T10:47:22.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3010, N'UTC-AC-111-HCGOA', N'SAM2', N'AnuTest Sharma', N'123456', N'12, MG road', N'Saket Nagar', N'Kolkata', N'700088', N'8767586997', NULL, NULL, N'anu@infobyd.in', NULL, N'29-09-2021', N'Pick And Drop (One Way)', N'No', N'Samsung', N'Air Conditioner', N'Window AC', NULL, N'Window AC', N'YHT473', N'Test-3', N'Correct address-1', N'Correct address-2', N'Mumbai', N'MAHARASHTRA', N'700075', N'HJ', N'+918657434260', NULL, N'No', N'No', N'', 1, NULL, CAST(N'2021-09-30T11:04:49.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4005, N'UTC-AC-105-MC5AM', N'SAM01', N'priya ', N'123456', N'34, new delhi', N'34, new delhi', N'Delhi', N'110001', N'+919589897890', NULL, NULL, N'test123@infobyd.in', N'12-10-2021', N'12-10-2021', N'Pick And Drop (ONE WAY)', N'No', N'Samsung', N'Air Conditioner', N'Split AC', NULL, N'Split AC', N'RRB54', N'Test-1', N'Correct address-1', N'Correct address-2', N'Mumbai', N'MAHARASHTRA', N'400070', N'JH', N'+918652241429', NULL, N'No', N'No', N'0.00', 1, NULL, CAST(N'2021-10-12T12:31:52.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5005, N'UTC-TV-101-FEODK', N'SAM0117', N'asdf erty', N'123456', N'26', N'gandhinagar', N'Bangalore', N'560030', N'9899989878', NULL, NULL, N'radhiambika@gmail.com', N'18-10-2021', N'18-10-2021', N'Pick And Drop (One Way)', N'No', N'Others', N'Television', N'LCD Television', NULL, N'32', N'KRL4', N'Test-1', N'Correct address-1', N'Correct address-2', N'Mumbai', N'MAHARASHTRA', N'400070', N'JH', N'+918652241429', NULL, N'No', N'No', N'', 1, NULL, CAST(N'2021-10-18T16:26:03.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6005, N'UTC-AC-101-XRODX', N'SAM011', N'Test 1 ', N'123456', N'64, vijay nagar', N'indore', N'Banglore', N'560027', N'+918880907770', NULL, NULL, N'test123@infobyd.in', N'28-10-2021', N'28-10-2021', N'Pick And Drop (ONE WAY)', N'No', N'Samsung', N'Air Conditioner', N'Window AC', NULL, N'Window AC', N'DZW124', N'Test-1', N'Correct address-1', N'Correct address-2', N'Mumbai', N'MAHARASHTRA', N'400070', N'JH', N'+918652241429', NULL, N'No', N'No', N'0.00', 1, NULL, CAST(N'2021-10-28T20:34:20.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicket] ([Id], [BizlogTicketNo], [SponsrorOrderNo], [ConsumerName], [ConsumerComplaintNumber], [AddressLine1], [AddressLine2], [City], [Pincode], [TelephoneNumber], [RetailerPhoneNo], [AlternateTelephoneNumber], [EmailId], [DateOfPurchase], [DateOfComplaint], [NatureOfComplaint], [IsUnderWarranty], [Brand], [ProductCategory], [ProductName], [ProductCode], [Model], [IdentificationNo], [DropLocation], [DropLocAddress1], [DropLocAddress2], [DropLocCity], [DropLocState], [DropLocPincode], [DropLocContactPerson], [DropLocContactNo], [DropLocAlternateNo], [PhysicalEvaluation], [TechEvalRequired], [Value], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6006, N'UTC-AC-101-D825G', N'SAM1001', N'priya ', N'123456', N'62, new anjani nagar', N'bhamori , indore', N'indore', N'560027', N'+918808987856', NULL, NULL, N'priyanka@infobyd.in', N'22-11-2021', N'22-11-2021', N'Pick And Drop (ONE WAY)', N'No', N'LG', N'Air Conditioner', N'Split AC', NULL, N'Split AC', N'NCL927', N'Test-1', N'Correct address-1', N'Correct address-2', N'Mumbai', N'MAHARASHTRA', N'400070', N'JH', N'+918652241429', NULL, N'No', N'No', N'0.00', 1, NULL, CAST(N'2021-11-22T11:27:07.000' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblBizlogTicket] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBizlogTicketStatus] ON 
GO
INSERT [dbo].[tblBizlogTicketStatus] ([Id], [BizlogTicketNo], [Status], [Remarks], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'UTC-AC-105-MC5AM', N'Cancelled', N'Ticket Cancelled', 1, NULL, CAST(N'2021-10-12T12:34:06.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicketStatus] ([Id], [BizlogTicketNo], [Status], [Remarks], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'UTC-TV-101-FEODK', N'8N', N'In Transit', 1, NULL, CAST(N'2021-10-18T16:28:22.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblBizlogTicketStatus] ([Id], [BizlogTicketNo], [Status], [Remarks], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1002, N'UTC-AC-101-XRODX', N'8', N'PRODUCT_PICKED_FROM_CUSTOMER.', 1, NULL, CAST(N'2021-10-28T20:44:35.000' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblBizlogTicketStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBrand] ON 
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Samsung', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'LG', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'Hitachi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'Voltas', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2002, N'Bosch', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2003, N'IFB', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2004, N'Godrej', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2005, N'Whirlpool', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2006, N'Panasonic', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2007, N'Sony', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBrand] ([Id], [Name], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2008, N'Others', 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblBrand] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBusinessPartner] ON 
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Arjay Retail', NULL, N'BSH-BLR-01', N'ABB/ABBRegistration?BUId=1&BPId=1', NULL, NULL, NULL, NULL, NULL, N'#50/A, 32nd Cross, Jayanagar 4th block, ', NULL, N'560011', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'Arjay Retail', NULL, N'BSH-BLR-02', N'ABB/ABBRegistration?BUId=1&BPId=2', NULL, NULL, NULL, NULL, NULL, N'MOHAN MOTI SYMBIOSIS, Defence colony, Binnamangala layout , HAL 2nd stage, Indiranagar ,100 ft Road,', NULL, NULL, N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'Intent Trading Enterprises', NULL, N'BSH-BLR-03', N'ABB/ABBRegistration?BUId=1&BPId=3', NULL, NULL, NULL, NULL, NULL, N'65/1A,Opposite Bata Showroom,Above Vodafone store, Kaikondrahalli,Near Wipro Office,', NULL, N'560035', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'Aditya Retail', NULL, N'BSH-BLR-04', N'ABB/ABBRegistration?BUId=1&BPId=4', NULL, NULL, NULL, NULL, NULL, N'#21/1,16th Cross, Sampige Road, Malleshwaram', NULL, N'560003', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, N'RDS and Company', NULL, N'BSH-BLR-05', N'ABB/ABBRegistration?BUId=1&BPId=5', NULL, NULL, NULL, NULL, NULL, N'44/2, Dickenson Road, Near Manipal Centre', NULL, N'560042', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, N'Index Marketing', NULL, N'BSH-BLR-06', N'ABB/ABBRegistration?BUId=1&BPId=6', NULL, NULL, NULL, NULL, NULL, N'Mayflower Apartments,64,Wheeler Road, Cox Town', NULL, N'560005', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7, N'Aditya Retail', NULL, N'BSH-BLR-07', N'ABB/ABBRegistration?BUId=1&BPId=7', NULL, NULL, NULL, NULL, NULL, N'229, 7th Main Rd,Bhadrappa Layout,HRBR Layout,2nd Block, Kalyan Nagar', NULL, N'560043', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8, N'Haritha Enterprises', NULL, N'BSH-BLR-08', N'ABB/ABBRegistration?BUId=1&BPId=8', NULL, NULL, NULL, NULL, NULL, N'687/48/G001, REGENT PRIME, Whitefield Main Road, Opposite to Ganesh Temple,', NULL, N'560066', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (9, N'Haritha Enterprises', NULL, N'BSH-BLR-09', N'ABB/ABBRegistration?BUId=1&BPId=9', NULL, NULL, NULL, NULL, NULL, N'#46/1,PN Plaza First Floor,Near TC Palya gate, Old Madras Road,Virgonagar, post Battarahalli,', NULL, N'560049', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10, N'Abhyudaya Retail Inc', NULL, N'BSH-BLR-10', N'ABB/ABBRegistration?BUId=1&BPId=10', NULL, NULL, NULL, NULL, NULL, N' # 607,Dr Muthuraj road,7th Block, 2nd Phase,Banashankari 3rd Stage, ', NULL, N'560085', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (11, N'RDS and Company', NULL, N'BSH-BLR-11', N'ABB/ABBRegistration?BUId=1&BPId=11', NULL, NULL, NULL, NULL, NULL, N'691,MIG A sector, 3rd A cross ,Opposite Seshadripuram College, Yelahanka 3rd stage,', NULL, N'560064', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (12, N'Trapasol India Pvt. Ltd.', NULL, N'BSH-BLR-12', N'ABB/ABBRegistration?BUId=1&BPId=12', NULL, NULL, NULL, NULL, NULL, N'No. 316, Mallathahalli main road, Opposite AIT College, Nagarabhavi,', NULL, N'560040', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (13, N'Trapasol India Pvt. Ltd.', NULL, N'BSH-BLR-13', N'ABB/ABBRegistration?BUId=1&BPId=13', NULL, NULL, NULL, NULL, NULL, N'Bannerghatta Road no.1010, 5th cross, 12th main  Vijaya bank layout , Billekahalli ', NULL, N'560040', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (14, N'Vistar Retail', NULL, N'BSH-BLR-14', N'ABB/ABBRegistration?BUId=1&BPId=14', NULL, NULL, NULL, NULL, NULL, N'587,  DwarkaNagar, 100 ft main Road , Balaji Layout,', NULL, N'560062', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (15, N'Smart Kitchen Solutions ', NULL, N'BSH-BLR-16', N'ABB/ABBRegistration?BUId=1&BPId=15', NULL, NULL, NULL, NULL, NULL, N'#317/17,Konappana Agrahara, Electronic City-Phase II, Next to Maruti Suzuki Showroom, Opp Wipro & Infosys ', NULL, N'560100', N'Bangalore ', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (16, N'Maharaja Gruhavaibhav', NULL, N'BSH-SMET-R1', N'ABB/ABBRegistration?BUId=1&BPId=16', NULL, NULL, NULL, NULL, NULL, N'H.E. Commercials, Vasan Eye care building,Near Nanjappa Hospital,Kuvempu road  ', NULL, N'577201', N'Shivamogga', N'Karnataka', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (17, N'Venu''s Digital Arcade', NULL, N'BSH-TCR-R1', N'ABB/ABBRegistration?BUId=1&BPId=17', NULL, NULL, NULL, NULL, NULL, N'Kanjhani Road,West Fort ', NULL, N'680004', N'Thrissur', N'Kerala', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (18, N'Venu''s Digital Arcade', NULL, N'BSH-TCR-R2', N'ABB/ABBRegistration?BUId=1&BPId=18', NULL, NULL, NULL, NULL, NULL, N'Opposite Devamatha School, Patturaikal', NULL, N'680022', N'Thrissur', N'Kerala', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (19, N'Pittappillil Agencies', NULL, N'BSH-KTYM-R1', N'ABB/ABBRegistration?BUId=1&BPId=19', NULL, NULL, NULL, NULL, NULL, N'58/A1-A10 PUKADIYIL COMMERCIAL COMPLEX, NAGAMBADAM, ', NULL, N'686006', N'Kottayam', N'Kerala', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (20, N'Design Décor', NULL, N'BSH-COK-01', N'ABB/ABBRegistration?BUId=1&BPId=20', NULL, NULL, NULL, NULL, NULL, N'37/3167, Near  Edapally Post Office, Opposite Metro Pillar  No 436, Palarivattom, Edapally Road, Edapally', NULL, N'682024', N'Kochi', N'Kerala', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (21, N'SAS Electronics', NULL, N'BSH-MAA-01', N'ABB/ABBRegistration?BUId=1&BPId=21', NULL, NULL, NULL, NULL, NULL, N'No: 100, P.H.Road, Nerkundram', NULL, N'600107', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (22, N'SAS Electronics', NULL, N'BSH-MAA-02', N'ABB/ABBRegistration?BUId=1&BPId=22', NULL, NULL, NULL, NULL, NULL, N'No.7, Dr. Kannan Tower, Opposite Naidu Hall,  Arcot Road,Porur', NULL, N'600016', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (23, N'SAS Electronics', NULL, N'BSH-MAA-03', N'ABB/ABBRegistration?BUId=1&BPId=23', NULL, NULL, NULL, NULL, NULL, N'32, GST Road, Tambaram Sanatorium,', NULL, N'600047', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (24, N'SAS Electronics', NULL, N'BSH-MAA-11', N'ABB/ABBRegistration?BUId=1&BPId=24', NULL, NULL, NULL, NULL, NULL, N'Ground Floor, Door No. 112, GST Main Road, Urapakkam, ', NULL, N'603210', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (25, N'SAS Electronics', NULL, N'BSH-MAA-12', N'ABB/ABBRegistration?BUId=1&BPId=25', NULL, NULL, NULL, NULL, NULL, N'No. 256, Vellore Main Road, Katpadi', NULL, N'632007', N'Vellore', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (26, N'Adorn', NULL, N'BSH-MAA-05', N'ABB/ABBRegistration?BUId=1&BPId=26', NULL, NULL, NULL, NULL, NULL, N'NEW 119 / OLD 57, TTK ROAD,ALWARPET', NULL, N'600018', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (27, N'E Khareedo', NULL, N'BSH-MAA-06', N'ABB/ABBRegistration?BUId=1&BPId=27', NULL, NULL, NULL, NULL, NULL, N'320, Valluvar Kottam High Road, Nungambakkam', NULL, N'600034', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (28, N'Lakshmi Electronics', NULL, N'BSH-MAA-07', N'ABB/ABBRegistration?BUId=1&BPId=28', NULL, NULL, NULL, NULL, NULL, N'No:199A,Velachery Main Road,Pallikarnai,Medavakkam,', NULL, N'600100', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (29, N'RK Home Appliances', NULL, N'BSH-MAA-08', N'ABB/ABBRegistration?BUId=1&BPId=29', NULL, NULL, NULL, NULL, NULL, N'No:21/1, Jawaharlal Nehru Salai,Vadapalani', NULL, N'600026', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (30, N'Kriya', NULL, N'BSH-MAA-09', N'ABB/ABBRegistration?BUId=1&BPId=30', NULL, NULL, NULL, NULL, NULL, N'No: 218,Paper Mills Road,Perambur', NULL, N'600011', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (31, N'General Pumps Pvt Ltd', NULL, N'BSH-MAA-10', N'ABB/ABBRegistration?BUId=1&BPId=31', NULL, NULL, NULL, NULL, NULL, N'No:20A,Dr.Kalaignar Karunanidhi Salai,ECR-OMR Link Road,Akkarai Junction', NULL, N'600119', N'Chennai', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (32, N'Mani Furniture ', NULL, N'BSH-PNY-01', N'ABB/ABBRegistration?BUId=1&BPId=32', NULL, NULL, NULL, NULL, NULL, N'No.71,Villianur Road,Reddiyarpalayam', NULL, N'605010', N'Puducherry', N'Puducherry', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (33, N'Ideal Appliances', NULL, N'BSH-CJB-01', N'ABB/ABBRegistration?BUId=1&BPId=33', NULL, NULL, NULL, NULL, NULL, N'Near MTP Road Signal Opposite Chinthamani Super Market, R.S.Puram,', NULL, N'641002', N'Coimbatore', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (34, N'TNS Appliances', NULL, N'BSH-CJB-02', N'ABB/ABBRegistration?BUId=1&BPId=34', NULL, NULL, NULL, NULL, NULL, N'NO: 461/2 Vivekanandar street, Sathy Rd, Ganapathy, Gopalakrishnapuram, Ganapathypudu', NULL, N'641006', N'Coimbatore', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (35, N'Bharat Electronics and Appliances', NULL, N'BSH-ED-R1', N'ABB/ABBRegistration?BUId=1&BPId=35', NULL, NULL, NULL, NULL, NULL, N'142/1, Near Sakthi Mahal, Perundurai Main Road, ', NULL, N'638011', N'Erode', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (36, N'Kaliamman & Co.', NULL, N'BSH-TUP-R1', N'ABB/ABBRegistration?BUId=1&BPId=36', NULL, NULL, NULL, NULL, NULL, N'#105,Opposite SAP theatre,Avinashi Road,Tiruppur', NULL, N'641603', N'Tiruppur', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (37, N'Bharat Electronics and Appliances', NULL, N'BSH-SXV-01', N'ABB/ABBRegistration?BUId=1&BPId=37', NULL, NULL, NULL, NULL, NULL, N'36/16, Meyyanur Main Road, Near 3 Roads,', NULL, N'636004', N' Salem ', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (38, N'Sri Navaladi Associates', NULL, N'BSH-TRZ-01', N'ABB/ABBRegistration?BUId=1&BPId=38', NULL, NULL, NULL, NULL, NULL, N'D-28, 7th Cross street East,Thillainagar, ', NULL, N'620018', N'Trichy ', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (39, N'Sri Navaladi Associates', NULL, N'BSH-KRR-R1', N'ABB/ABBRegistration?BUId=1&BPId=39', NULL, NULL, NULL, NULL, NULL, N' No. 20, Covai Road, Opp to Amaravathi Hospital, Vaiyapuri Nagar, ', NULL, N'639002', N'Karur', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (40, N'Noor Traders', NULL, N'BSH-TJV-01', N'ABB/ABBRegistration?BUId=1&BPId=40', NULL, NULL, NULL, NULL, NULL, N'No. 2886, PLA complex, Srinivasan Pillai Road', NULL, N'613001', N'Thanjavur', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (41, N'Adya at Home', NULL, N'BSH-DG-R1', N'ABB/ABBRegistration?BUId=1&BPId=41', NULL, NULL, NULL, NULL, NULL, N'No. 91, Sona Tower , Ground Floor, New Agraharam Palani Road', NULL, N'624001', N'Dindigul', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (42, N'Veerappa Nadar & sons', NULL, N'BSH-SVKS-R1', N'ABB/ABBRegistration?BUId=1&BPId=42', NULL, NULL, NULL, NULL, NULL, N'No. 117, South Car street, ', NULL, N'626123', N'Sivakasi', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (43, N'KSN electronics', NULL, N'BSH-HSRA-R1', N'ABB/ABBRegistration?BUId=1&BPId=43', NULL, NULL, NULL, NULL, NULL, N'64/13B shop no 8&9,  Narasimha complex , ', NULL, N'635109', N'Hosur', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (44, N'Oxygen Home Appliances ', NULL, N'BSH-NCJ-R1', N'ABB/ABBRegistration?BUId=1&BPId=44', NULL, NULL, NULL, NULL, NULL, N'121E, Chettikulam Junction', NULL, N'629001', N'Nagercoil', N'Tamilnadu', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (45, N'Super Power', NULL, N'BSH-VGA-01', N'ABB/ABBRegistration?BUId=1&BPId=45', NULL, NULL, NULL, NULL, NULL, N'#52-1/1-5/3A,Ring Road Service Road,Veterinary colony,Opposite Tata Motors Showroom, ', NULL, N'520008', N'Vijayawada', N'Andhra Pradesh', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (46, N'Karthik Agencies', NULL, N'BSH-RJA-01', N'ABB/ABBRegistration?BUId=1&BPId=46', NULL, NULL, NULL, NULL, NULL, N'D No. 6-13-16,Kankatala vari street, Road cum Rail Bridge, T Nagar, ', NULL, N'533101', N'Rajajmundry', N'Andhra Pradesh', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (47, N'Indu Enterprises ', NULL, N'BSH-TIR-01', N'ABB/ABBRegistration?BUId=1&BPId=47', NULL, NULL, NULL, NULL, NULL, N'#20-1-137/D,Maruthi Nagar,Tirumala Bypass Road, Near Leelamahal Circle,', NULL, N'517501', N'Tirupati', N'Andhra Pradesh', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (48, N'Jyot Electronics', NULL, N'BSH-BOM-01', N'ABB/ABBRegistration?BUId=1&BPId=48', NULL, NULL, NULL, NULL, NULL, N'Dreamland Building,23-27, New Queens Road, Opera House', NULL, N'400004', N'Mumbai', N'Maharashtra', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (49, N'Dhrubojai Electronics LLP', NULL, N'BSH-PNQ-01', N'ABB/ABBRegistration?BUId=1&BPId=49', NULL, NULL, NULL, NULL, NULL, N'Shop No 1, 24K World Residences, S. No 198/1b Nagar Road, Viman Nagar', NULL, N'411047', N'Pune', N'Maharashtra', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (50, N'Bapat Shop', NULL, N'BSH-NAG-01', N'ABB/ABBRegistration?BUId=1&BPId=50', NULL, NULL, NULL, NULL, NULL, N'Shop No-3, Chaitra Apartment, Plot #5, Ring Road, Pratap Nagar,', NULL, N'440010', N' Nagpur', N'Maharashtra', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (51, N'Roopam Electronics', NULL, N'BSH-NAG-02', N'ABB/ABBRegistration?BUId=1&BPId=51', NULL, NULL, NULL, NULL, NULL, N'Dhantoli Stadium', NULL, N'440012', N' Nagpur', N'Maharashtra', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (52, N'Shree Mathura Electronics', NULL, N'BSH-IXU-01', N'ABB/ABBRegistration?BUId=1&BPId=52', NULL, NULL, NULL, NULL, NULL, N'3/4/33/34, Jyotirmay Complex,  Near Hotel Athiti, Jalna road', NULL, N'431001', N'Aurangabad', N'Maharashtra', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (53, N'Mundada Corporation', NULL, N'BSH-ISK-01', N'ABB/ABBRegistration?BUId=1&BPId=53', NULL, NULL, NULL, NULL, NULL, N'SN 860/2,3,4, Karma Pinacle, Ashoka Marg, Nashik, Maharashtra 422011', NULL, N'422011', N'Nasik', N'Maharashtra', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (54, N'T.D. Parodkar & Co.', NULL, N'BSH-GOI-01', N'ABB/ABBRegistration?BUId=1&BPId=54', NULL, NULL, NULL, NULL, NULL, N'Shop 3, Angel Anne Arcade,Opposite Government Quarters, Near Bazilio Gym, St Inez', NULL, N'403001', N'Panjim', N'Goa', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (55, N'Siddheshwari Traders', NULL, N'BSH-KADI-R1', N'ABB/ABBRegistration?BUId=1&BPId=55', NULL, NULL, NULL, NULL, NULL, N'C/1-5,Ashwamegh Arcade,Opp.GEB,, Kadi Kalol Road, Near HDFC Bank,', NULL, N'382715', N'Kadi', N'Gujarat', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (56, N'Shree Parshwanath Enterprise ', NULL, N'BSH-BDQ-01', N'ABB/ABBRegistration?BUId=1&BPId=56', NULL, NULL, NULL, NULL, NULL, N'3,Parshwadarshan Complex ,Near Shrenik Park Cross Road, Akota, ', NULL, N'390020', N'Vadodara', N'Gujarat', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (57, N'Ghiya''s', NULL, N'BSH-JAI-01', N'ABB/ABBRegistration?BUId=1&BPId=57', NULL, NULL, NULL, NULL, NULL, N'Shop No. 131,Opp Chankaya Restaurant,M.I.Road,', NULL, N'302001', N'Jaipur', N'Rajasthan', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (58, N'Manglam', NULL, N'BSH-JAI-02', N'ABB/ABBRegistration?BUId=1&BPId=58', NULL, NULL, NULL, NULL, NULL, N'Opposite BMB Mishtan Bhandar, Near Dr Tripathi Dental Clinic,  Sanghi Farm, Tonk Road', NULL, N'302018', N'Jaipur', N'Rajasthan', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (59, N'Aditya Sales ', NULL, N'BSH-UDR-01', N'ABB/ABBRegistration?BUId=1&BPId=59', NULL, NULL, NULL, NULL, NULL, N' Bapu bazar, Town hall main road, ', NULL, N'313001', N'Udaipur', N'Rajasthan', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (60, N'Shanti Enterprises ', NULL, N'BSH-KQH-01', N'ABB/ABBRegistration?BUId=1&BPId=60', NULL, NULL, NULL, NULL, NULL, N'Shree Nagar Road, Near Martindale bridge', NULL, N'305001', N'Ajmer', N'Rajasthan', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (61, N'Audio Voice India (P) Ltd.', NULL, N'BSH-DEL-01', N'ABB/ABBRegistration?BUId=1&BPId=61', NULL, NULL, NULL, NULL, NULL, N'#2, Vaishali, Kohat Enclave, Opposite Metro pillar no. 354,Pitampura', NULL, N'110034', N'N. Delhi', N'Delhi NCR', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (62, N'Audio Voice (I) Pvt. Ltd.', NULL, N'BSH-DEL-02', N'ABB/ABBRegistration?BUId=1&BPId=62', NULL, NULL, NULL, NULL, NULL, N'D-27,Central Market,Lajpat Nagar-2', NULL, N'110024', N'N. Delhi', N'Delhi NCR', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (63, N'Navrang Electronics ', NULL, N'BSH-DEL-03', N'ABB/ABBRegistration?BUId=1&BPId=63', NULL, NULL, NULL, NULL, NULL, N'F3/5 Krishna Nagar, ', NULL, N'110051', N'N. Delhi', N'Delhi NCR', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (64, N'Ghaziabad Power Tools', NULL, N'BSH-DEL-04', N'ABB/ABBRegistration?BUId=1&BPId=64', NULL, NULL, NULL, NULL, NULL, N'H-6, Patel Nagar-III,', NULL, N'201010', N'Ghaziabad', N'Uttar Pradesh', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (65, N'Sai Ji Furniture and Wood Products', NULL, N'BSH-BEK-01', N'ABB/ABBRegistration?BUId=1&BPId=65', NULL, NULL, NULL, NULL, NULL, N'GF-08,Tulip Grand,Pilhibit Bypass road,', NULL, N'233001', N'Bareilly', N'Uttar Pradesh', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (66, N'The Home Square', NULL, N'BSH-IXC-01', N'ABB/ABBRegistration?BUId=1&BPId=66', NULL, NULL, NULL, NULL, NULL, N'S.C.O-87, Sector-35C', NULL, N'160022', N'Chandigarh', N'Chandigarh', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (67, N'National Agencies', NULL, N'BSH-PTA-R1', N'ABB/ABBRegistration?BUId=1&BPId=67', NULL, NULL, NULL, NULL, NULL, N'Sheran Wala Gate ', NULL, N'147001', N'Patiala', N'Punjab', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (68, N'Deekay Electrovision', NULL, N'BSH-LUH-01', N'ABB/ABBRegistration?BUId=1&BPId=68', NULL, NULL, NULL, NULL, NULL, N'Ghumar Mandi chowk,opposite Roop square', NULL, N'141001', N'Ludhiana', N'Punjab', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (69, N'Behl Associates', NULL, N'BSH-DED-01', N'ABB/ABBRegistration?BUId=1&BPId=69', NULL, NULL, NULL, NULL, NULL, N'26-E, Rajpur Road, ', NULL, N'248001', N'Dehradun', N'Uttarakhand', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (70, N'Ideas', NULL, N'BSH-PGH-01', N'ABB/ABBRegistration?BUId=1&BPId=70', NULL, NULL, NULL, NULL, NULL, N'Opposite  Taxi Stand, Nanital Road.', NULL, N'263139', N'Haldwani', N'Uttarakhand', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (71, N'Chowdhary Electronics', NULL, N'BSH-IXJ-01', N'ABB/ABBRegistration?BUId=1&BPId=71', NULL, NULL, NULL, NULL, NULL, N'Apsara Road, Near Apsara Theatre, Gandhi Nagar', NULL, N'180004', N'Jammu', N'Jammu & Kashmir', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (72, N'Multichannel Electronics Pvt. Ltd.', NULL, N'BSH-CCU-01', N'ABB/ABBRegistration?BUId=1&BPId=72', NULL, NULL, NULL, NULL, NULL, N'2/9 B,Sahid nagar, Gr. floor, Garfa,', NULL, N'700031', N'Kolkata', N'West Bengal', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (73, N'BRL Retail', NULL, N'BSH-IXB-01', N'ABB/ABBRegistration?BUId=1&BPId=73', NULL, NULL, NULL, NULL, NULL, N'Jeevandeep Building, Ground floor, Sevoke Road, Near Honda Showroom, Salugara', NULL, N'734008', N'Siliguri', N'West Bengal', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (74, N'Khosla Electronics Pvt Ltd.', NULL, N'BSH-CCU-02', N'ABB/ABBRegistration?BUId=1&BPId=74', NULL, NULL, NULL, NULL, NULL, N'25/1, Shakespeare Sarani ', NULL, N'700017', N'Kolkata', N'West Bengal', 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessPartner] ([BusinessPartnerId], [Name], [Description], [StoreCode], [QRCodeURL], [QRImage], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [BusinessUnitId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (75, N'test store', N'na', N'BSH-00150', N'ABB/ABBRegistration?BUId=1&BPId=75', NULL, N'priya', N'patel', N'7415247205', N'priyanka@infobyd.in', N'62, anjani nagar', N'vijay nagar', N'452001', N'indore', N'Madhya Pradesh', 1, 1, NULL, CAST(N'2022-01-25T08:48:25.000' AS DateTime), NULL, CAST(N'2022-01-25T08:48:25.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[tblBusinessPartner] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBusinessUnit] ON 
GO
INSERT [dbo].[tblBusinessUnit] ([BusinessUnitId], [Name], [Description], [RegistrationNumber], [QRCodeURL], [LogoName], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [LoginId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'BSH-Stores
', NULL, NULL, NULL, N'Bosch-Logo.png', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessUnit] ([BusinessUnitId], [Name], [Description], [RegistrationNumber], [QRCodeURL], [LogoName], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [LoginId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'Samsung
', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblBusinessUnit] ([BusinessUnitId], [Name], [Description], [RegistrationNumber], [QRCodeURL], [LogoName], [ContactPersonFirstName], [ContactPersonLastName], [PhoneNumber], [Email], [AddressLine1], [AddressLine2], [Pincode], [City], [State], [LoginId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'TATA CLiQ
', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblBusinessUnit] OFF
GO
SET IDENTITY_INSERT [dbo].[tblCurrentAuthtoken] ON 
GO
INSERT [dbo].[tblCurrentAuthtoken] ([CurrentAuthtokenId], [CurrentAuthTokenName], [CurrentAuthToken], [CreatedDate]) VALUES (1, N'ZohoDesk', NULL, CAST(N'2021-10-09T12:07:05.060' AS DateTime))
GO
INSERT [dbo].[tblCurrentAuthtoken] ([CurrentAuthtokenId], [CurrentAuthTokenName], [CurrentAuthToken], [CreatedDate]) VALUES (2, N'ZohoDesk', NULL, CAST(N'2021-10-09T12:08:19.223' AS DateTime))
GO
INSERT [dbo].[tblCurrentAuthtoken] ([CurrentAuthtokenId], [CurrentAuthTokenName], [CurrentAuthToken], [CreatedDate]) VALUES (3, N'ZohoDesk', NULL, CAST(N'2021-10-09T12:11:23.830' AS DateTime))
GO
INSERT [dbo].[tblCurrentAuthtoken] ([CurrentAuthtokenId], [CurrentAuthTokenName], [CurrentAuthToken], [CreatedDate]) VALUES (4, N'ZohoDesk', NULL, CAST(N'2021-10-09T12:12:57.640' AS DateTime))
GO
INSERT [dbo].[tblCurrentAuthtoken] ([CurrentAuthtokenId], [CurrentAuthTokenName], [CurrentAuthToken], [CreatedDate]) VALUES (5, N'ZohoDesk', NULL, CAST(N'2021-10-16T12:37:47.313' AS DateTime))
GO
INSERT [dbo].[tblCurrentAuthtoken] ([CurrentAuthtokenId], [CurrentAuthTokenName], [CurrentAuthToken], [CreatedDate]) VALUES (1002, N'ZohoDesk', N'1000.d847156be18dbd2a5b8d82ca76555412.504e02bac8452734138a54f8adea9832', CAST(N'2022-01-27T16:47:11.503' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[tblCurrentAuthtoken] OFF
GO
SET IDENTITY_INSERT [dbo].[tblCustomerDetails] ON 
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7030, N'AlokTest', N'Patel', N'aloktest@infobyd.in', N'Bangalore', N'560026', N'26, MG road', N'magarpatta', N'9967586990', NULL, 1, NULL, CAST(N'2021-09-06T15:19:43.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8024, N'AlokTest', N'Patel', N'aloktest@infobyd.in', N'Bangalore', N'560026', N'26, MG road', N'magarpatta', N'9967586990', NULL, 1, NULL, CAST(N'2021-09-13T19:46:35.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (9024, N'AlokTest', N'Patel', N'aloktest@infobyd.in', N'Bangalore', N'560026', N'26, MG road', N'magarpatta', N'9967586990', NULL, 1, NULL, CAST(N'2021-09-16T10:03:31.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10024, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-20T12:07:01.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10025, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-20T12:25:27.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10026, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-20T16:16:27.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10027, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-20T16:31:11.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10028, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-21T10:23:51.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10029, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-21T10:26:53.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10030, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-21T10:29:31.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10031, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-21T10:35:54.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10032, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-21T10:40:54.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10033, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-21T10:43:33.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10034, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-21T14:49:55.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10035, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-09-21T15:20:00.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10036, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-10-16T12:36:48.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10037, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-10-16T13:14:21.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10038, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-10-16T13:27:16.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblCustomerDetails] ([Id], [FirstName], [LastName], [Email], [City], [ZipCode], [Address1], [Address2], [PhoneNumber], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10039, N'asdf', N'erty', N'radhiambika@gmail.com', N'Bangalore', N'560030', N'26', N'gandhinagar', N'9899989878', NULL, 1, NULL, CAST(N'2021-10-18T16:20:38.000' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblCustomerDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[tblErrorLog] ON 
GO
INSERT [dbo].[tblErrorLog] ([ErrorLogId], [ClassName], [MethodName], [SponsorOrderNo], [ErrorMessage], [CreatedDate]) VALUES (1, N'SponserManager', N'AddSponser', NULL, N'
Content :{"code":3002,"error":{"Estimate_Delivery_Date":"Enter a valid date format for Estimate Delivery Date"}}', CAST(N'2021-09-20T16:19:25.830' AS DateTime))
GO
INSERT [dbo].[tblErrorLog] ([ErrorLogId], [ClassName], [MethodName], [SponsorOrderNo], [ErrorMessage], [CreatedDate]) VALUES (2, N'SponserManager', N'AddSponser', N'11534880850', N'
Content :{"code":3002,"error":{"Estimate_Delivery_Date":"Enter a valid date format for Estimate Delivery Date"}}', CAST(N'2021-09-20T16:31:52.170' AS DateTime))
GO
INSERT [dbo].[tblErrorLog] ([ErrorLogId], [ClassName], [MethodName], [SponsorOrderNo], [ErrorMessage], [CreatedDate]) VALUES (3, N'SponserManager', N'AddSponser', N'11167142022', N'
Content :{"code":3002,"error":{"Estimate_Delivery_Date":"Enter a valid date format for Estimate Delivery Date"}}', CAST(N'2021-09-21T10:41:36.020' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[tblErrorLog] OFF
GO
SET IDENTITY_INSERT [dbo].[tblEVCApproved] ON 
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'4186686000000279003', N'Uzer Malik E-Waste Scrap Trading Pvt. Ltd.', N'DEL004', N'Shamim Alam', N'+918010224818', N'shamimmalik6666@gmail.com', N'Plot No. G-60,Gali No. 13,', N'Bhagirathi Vihar, Delhi', N'110094', N'Delhi', N'DELHI', N'', N'Plot No. G-60,Gali No. 13,, Bhagirathi Vihar, Delhi 110094', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000279003/Upload_GST_Registration/download?filepath=1627278875091_WhatsApp_Image_2021-07-26_at_10.32.00.jpeg', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000279003/Copy_of_Cancelled_Cheque/download?filepath=1627278874803_WhatsApp_Image_2021-07-26_at_10.32.01.jpeg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'4186686000000244003', N'Kavya bhoomi group ', N'DEL003', N'Nazar nawaz', N'+918171111113', N'Rajanawazr90@gmail.com', N'1st floor Hmt plaza gola kuan meerut ', N'', N'250002', N'Meerut ', N'UTTAR PRADESH', N'', N'104 Azad Nagar gola kuan meerut ', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000244003/Upload_GST_Registration/download?filepath=1626584667661_image.jpg', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000244003/Copy_of_Cancelled_Cheque/download?filepath=1626584667186_image.jpg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'4186686000000242003', N'', N'', N'RISHAB ELECTRONICS', N'+917987682595', N'', N'', N'', N'', N'', N'', N'15925.00', N'', N'', N'', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'4186686000000433003', N'PRABH SIMRAN ELECTRONICS', N'HYD005', N'HARMIT SINGH', N'+919866755547', N'prabhsimranelectronics@gmail.com', N'4-3-216 to 265, GUJARATHI GALLI, KOTHI, Hyderabad, Telangana, 500029', N'', N'500029', N'Hyderabad', N'TELANGANA', N'', N'4-3-216 to 265, GUJARATHI GALLI, KOTHI, Hyderabad, Telangana, 500029', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000433003/Upload_GST_Registration/download?filepath=1629362149927_AA360317011792C_RC18072018.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000433003/Copy_of_Cancelled_Cheque/download?filepath=1629362149838_WhatsApp_Image_2021-08-19_at_14.00.53.jpeg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, N'4186686000000431015', N'Greenscape Eco Management Private Limited', N'DEL006', N'Amit Gupta', N'+919971255000', N'amitg@greenscape-eco.com', N'Greenscape Eco Management Private Limited', N'Plot no. 348, Patparganj Industrial Area, Delhi', N'110092', N'', N'RAJASTHAN', N'', N'F588 TO F-591,M.I. AREA, Alwar, Rajasthan
', N'', N'', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, N'4186686000000431003', N'Sneha enterprises', N'HYD004', N'Rajendra jain', N'+919246333361', N'jain.snehaenterprises@gmail.com', N'10-4-41 /A/7,Golconda Apartments, humayunnagar, Hyd 500028', N'', N'500028', N'', N'TELANGANA', N'', N'10-4-41 /A/7,Golconda Apartments, humayunnagar, Hyd 500028
', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000431003/Upload_GST_Registration/download?filepath=1629353453532_GST-REG-06__2_.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000431003/Copy_of_Cancelled_Cheque/download?filepath=1629353453428_WhatsApp_Image_2021-08-18_at_15.50.30.jpeg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7, N'4186686000000420021', N'info', N'', N'Alok test', N'+918770904426', N'Vineet@infobyd.in', N'Place: 353, Shayam Nagar', N'', N'452010', N'Indore', N'', N'', N'Place: 353, Shayam Nagar', N'', N'', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8, N'4186686000000416003', N'TEST Customer004', N'XYZ004', N'XYZ004', N'+919999999999', N'chinu737c@gmail.com', N'', N'', N'', N'', N'MAHARASHTRA', N'', N'', N'', N'', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (9, N'4186686000000383003', N'SHARMA ENTERPRISE', N'DEL005', N'PARAS SHARMA', N'+919818464699', N'psah21@gmail.com', N'PLOT NO-28 A/B/C, SHIV VIHAR, MAHALAKSMI', N'ENCLAVE, North West Delhi, Delhi, 110094', N'110094', N'North West Delhi', N'DELHI', N'', N'PLOT NO-28 A/B/C, SHIV VIHAR, MAHALAKSMI
ENCLAVE, North West Delhi, Delhi, 110094', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000383003/Upload_GST_Registration/download?filepath=1628835360514_GST_CERTIFICATE_SHARMA.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000383003/Copy_of_Cancelled_Cheque/download?filepath=1628835360409_CamScanner_08-06-2020_10.35.23.pdf', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10, N'4186686000000241003', N'', N'', N'XYZ001-Test EVC-1', N'+917987682595', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (11, N'4186686000000226067', N'Test EVC-3', N'ZAX762', N'Test EVC-3', N'+917777799999', N'rashi@infobyd.in', N'Hyderabad', N'opposite bank', N'500005', N'Hyderabad', N'TELANGANA', N'95650.00', N'Hyderabad', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000226067/Upload_GST_Registration/download?filepath=1626341181927_INV-2122-00013.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000226067/Copy_of_Cancelled_Cheque/download?filepath=1626341181820_INV-2122-00013.pdf', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (12, N'4186686000000226055', N'Test EVC-2', N'ZXY001', N'Test EVC-2', N'+919999988888', N'testanand587@gmail.com', N'Andheri', N'Mumbai', N'100001', N'Delhi', N'DELHI', N'205000.00', N'Delhi', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000226055/Upload_GST_Registration/download?filepath=1626340788276_INV-2122-00013.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000226055/Copy_of_Cancelled_Cheque/download?filepath=1626340788163_INV-2122-00013__1_.pdf', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (13, N'4186686000000226047', N'Test EVC-1', N'XYZ001', N'Test EVC-1', N'+911234567890', N'anand2012c@gmail.com', N'Mumbai', N'', N'400001', N'Mumbai', N'MAHARASHTRA', N'92000.00', N'Mumbai', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000226047/Upload_GST_Registration/download?filepath=1626338129324_INV-2122-00013.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000226047/Copy_of_Cancelled_Cheque/download?filepath=1626338129212_INV-2122-00013.pdf', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (14, N'4186686000000207003', N'NEW GALAXY HOME APPLIANCES', N'DEL002', N'NAHIM KHAN', N'+919891466729', N'newgalaxyhome@gmail.com', N'SHOP NO 4 ANKHEER BADKHAL ROAD NEAR SHIV MANDIR OPPOSITE SECTOR 21D ANKHEER FARIDABAD 121012', N'', N'121012', N'Faridabad', N'HARYANA', N'', N'SHOP NO 4 ANKHEER BADKHAL ROAD NEAR SHIV MANDIR OPPOSITE SECTOR 21D ANKHEER FARIDABAD 121012', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000207003/Upload_GST_Registration/download?filepath=1625739396506_WhatsApp_Image_2021-07-08_at_15.43.42.jpeg', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000207003/Copy_of_Cancelled_Cheque/download?filepath=1625739396418_WhatsApp_Image_2021-07-08_at_15.43.41.jpeg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (15, N'4186686000000200003', N'ProConnect Trading & Services', N'DEL001', N'Anand R Krishnan', N'+919999024849', N'anand@proconnectts.com', N'B-49,GF, Shop no-21 and 24, Mehtab Complex, Joshi Colony, I.P. EXTN, Delhi East 110092', N'', N'110092', N'Delhi', N'DELHI', N'', N'B-49,GF, Shop no-21 and 24, Mehtab Complex, Joshi Colony, I.P. EXTN, Delhi East 110092', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000200003/Upload_GST_Registration/download?filepath=1625555969515_New_Doc_2021-06-24_12.47.52.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000200003/Copy_of_Cancelled_Cheque/download?filepath=1625555969418_New_Doc_2021-06-24_12.47.52.pdf', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (16, N'4186686000000190003', N'Vimal Home Appliances', N'HYD003', N'Rajkumar Jain', N'+919502036256', N'raj645310@gmail.com', N'H.no 268,lal bazar, Trimulgheery', N'', N'500015', N'Secunderabad', N'TELANGANA', N'', N's.no.268, lal bazar, secunderabad', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000190003/Upload_GST_Registration/download?filepath=1625211351698_1625211185798680551820218859858.jpg', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000190003/Copy_of_Cancelled_Cheque/download?filepath=1625211351485_16252107831218122260753289988747.jpg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (17, N'4186686000000168003', N'Universal trading corporation', N'IDR001', N'MOHAMMED ARIF', N'+919301666667', N'utcindore54@gmail.com', N'C-20 pakiza life style new khajrana indore', N'', N'452010', N'Indore', N'MADHYA PRADESH', N'', N'PAKIZA LIFE STYLE, C-20, NEW KHAJRANA, INDORE,
Indore, Madhya Pradesh, 452016', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000168003/Upload_GST_Registration/download?filepath=1624947583821_Universal_Trading_Corporation-GST_Certificate__1_.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000168003/Copy_of_Cancelled_Cheque/download?filepath=1624947583727_Universal_Trading_Corporation-_Cancelled_Cheque.pdf', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (18, N'4186686000000150003', N'VEERABHADRA TRADERS', N'HYD002', N'M.Anjaneyulu Gupta', N'+919908590078', N'anjanvms@gmail.com', N'PLOT NO.985, STREET NO.18, HMT COLONY, AMEENPUR, MIYAPUR, HYDERABAD.Telengana State.', N'', N'500049', N'Hyderabad', N'MAHARASHTRA', N'', N'PLOT NO.985, STREET NO.18, HMT COLONY, AMEENPUR, MIYAPUR, HYDERABAD.Telengana State. 500049', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000150003/Upload_GST_Registration/download?filepath=1624614457812_Veerbhadra_Traders_GST.jpeg', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000150003/Copy_of_Cancelled_Cheque/download?filepath=1624614457741_Verbhadra_Traders_Cheque.jpeg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (19, N'4186686000000148003', N'BALAJI ELECTRONICS', N'BOM002', N'NITIN AMRUTLAL PARMAR', N'+919869012447', N'Nitinparmar447@gmail.com', N'DEVENDRA SHRITI, SHOP NO 002, CARTER ROAD NO 2, BORIVALI EAST, MUMBAI, Mumbai Suburban, Maharashtra, 400066', N'', N'400066', N'Mumbai', N'MAHARASHTRA', N'', N'DEVENDRA SHRITI, SHOP NO 002, CARTER ROAD NO 2,
BORIVALI EAST, MUMBAI, Mumbai Suburban, Maharashtra,
400066', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000148003/Upload_GST_Registration/download?filepath=1624597271748_Balaji_Elect._GST_Cert.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000148003/Copy_of_Cancelled_Cheque/download?filepath=1624597271677_Balaji_Elect_Cancelled_Cheque.jpeg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (20, N'4186686000000133043', N'Frontier Services', N'BOM001', N'Shahid', N'+918291707679', N'frontierservices1982@gmail.com', N'H.NO.623, GALA 2, BABAN BUVA COMPOUND, BEHIND MODI HONDA GODOWN, DIVE ANJUR, BHIWANDI, Thane, Maharashtra, 421302', N'', N'421302', N'Thane', N'MAHARASHTRA', N'', N'Chawl No.9, Sindhi Camp Patra Chl,, Sable Nager ,Kurla east, Mumbai,
Mumbai Suburban, Maharashtra, 400024', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000133043/Upload_GST_Registration/download?filepath=1624525195981_Frontier_GST_Certificate.pdf', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000133043/Copy_of_Cancelled_Cheque/download?filepath=1624525195909_Frontier_Cancelled_Cheque.jpeg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (21, N'4186686000000133039', N'MBR Trading Company', N'IXC001', N'B D Singh', N'+919781382750', N'mbrtc37@gmail.com', N'Near Chandigarh zirakpur barrier, Chandigarh road ,zirakpur ,Dist Mohali', N'', N'140603', N'Mohali', N'PUNJAB', N'', N'Near Chandigarh zirakpur barrier, Chandigarh road ,zirakpur ,Dist Mohali
Punjab 140603', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000133039/Upload_GST_Registration/download?filepath=1624519549830_MBR_Trading_GST_Certificate.jpeg', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000133039/Copy_of_Cancelled_Cheque/download?filepath=1624519549753_MBR_Trading_Cancelled_Cheque.jpeg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (22, N'4186686000000133027', N'SONI TRADERS', N'JLR001', N'Agam soni', N'+919407181942', N'sonitraders19@gmail.com', N'Shop no 12 jeewan chayya appartment gate no 4 Wright town jabalpur', N'', N'482001', N'Jabalpur', N'MADHYA PRADESH', N'', N'Shop no 12 jeewan chayya appartment gate no 4 Wright town jabalpur, Madhya Pradesh 482001', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000133027/Upload_GST_Registration/download?filepath=1624517424730_SONI_Traders_GST_Certificate.jpg', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000133027/Copy_of_Cancelled_Cheque/download?filepath=1624517424642_SONI_Traders_PAN_Card.jpg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
INSERT [dbo].[tblEVCApproved] ([Id], [ZohoEVCApprovedId], [BussinessName], [EVCRegdNo], [ContactPerson], [EVCMobileNumber], [EmailID], [RegdAddressLine1], [RegdAddressLine2], [PinCode], [City], [State], [EVCWalletAmount], [ContactPersonAddress], [UploadGSTRegistration], [CopyofCancelledCheque], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (23, N'4186686000000118257', N'Metro enterprises ', N'HYD001', N'Ali', N'+919642309993', N'rahmathalibablu@gmail.com', N'2-4-135, Lingareddy Mansion, opp. Hanuman Temple. Tadbund, Secunderabad ', N'', N'500009', N'Secunderabad ', N'TELANGANA', N'', N'As above', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000118257/Upload_GST_Registration/download?filepath=1624430118037_image.jpg', N'/api/v2/infobyd_digimart/digi2l/report/All_Evc_Masters/4186686000000118257/Copy_of_Cancelled_Cheque/download?filepath=1624430117517_6BF1513C-4058-4BAB-BD45-4D40049BA1D9.jpeg', 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:34.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[tblEVCApproved] OFF
GO
SET IDENTITY_INSERT [dbo].[tblExchangeOrder] ON 
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5023, N'Samsung', N'4186686000001193011', N'Cancelled', 7030, 4, 1, N'600', N'SAM006', N'08-Sep-2021', N'Good', 1, NULL, 1, NULL, CAST(N'2021-09-06T15:19:43.000' AS DateTime), NULL, CAST(N'2021-09-06T15:22:54.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6018, N'Samsung', N'4186686000000921005', N'Order Created', 8024, 11, 1, N'600', N'SAM0016', N'14-Sep-2021', N'Good', 1, NULL, 1, NULL, CAST(N'2021-09-13T19:46:35.000' AS DateTime), NULL, CAST(N'2021-09-13T19:46:35.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7018, N'Samsung', N'4186686000000922005', N'Order Created', 9024, 11, 1, N'600', N'SAM0017', N'14-Sep-2021', N'Good', 1, NULL, 1, NULL, CAST(N'2021-09-16T10:03:31.000' AS DateTime), NULL, CAST(N'2021-09-16T10:03:31.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8018, N'Samsung', N'4186686000000810005', N'Order Created', 10024, 10, 2008, N'0', N'11008908731', N'20-Sep-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-20T12:07:01.000' AS DateTime), NULL, CAST(N'2021-09-20T12:07:01.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8019, N'Samsung', N'4186686000000808011', N'Order Created', 10025, 10, 2008, N'0', N'SAM0146', N'20-Sep-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-20T12:25:27.000' AS DateTime), NULL, CAST(N'2021-09-20T12:25:27.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8020, N'Samsung', NULL, N'Order Created', 10026, 10, 2008, N'0', N'SAM0147', N'25-09-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-20T16:16:27.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8021, N'Samsung', NULL, N'Order Created', 10027, 10, 2008, N'0', N'11534880850', N'25-09-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-20T16:31:11.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8022, N'Samsung', N'4186686000000821005', N'Order Created', 10028, 10, 2008, N'0', N'SAM0111', N'25-Sep-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-21T10:23:51.000' AS DateTime), NULL, CAST(N'2021-09-21T10:23:51.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8023, N'Samsung', N'4186686000000822005', N'Order Created', 10029, 10, 2008, N'0', N'SAM0112', N'25-Sep-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-21T10:26:53.000' AS DateTime), NULL, CAST(N'2021-09-21T10:26:53.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8024, N'Samsung', N'4186686000000819011', N'Order Created', 10030, 10, 2008, N'0', N'11167142020', N'25-Sep-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-21T10:29:31.000' AS DateTime), NULL, CAST(N'2021-09-21T10:29:31.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8025, N'Samsung', NULL, N'Order Created', 10031, 10, 2008, N'0', N'11167142021', N'25-09-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-21T10:35:54.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8026, N'Samsung', NULL, N'Order Created', 10032, 10, 2008, N'0', N'11167142022', N'25-09-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-21T10:40:54.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8027, N'Samsung', N'4186686000000818011', N'Order Created', 10033, 10, 2008, N'0', N'11167142023', N'25-Sep-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-21T10:43:33.000' AS DateTime), NULL, CAST(N'2021-09-21T10:43:33.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8028, N'Samsung', N'4186686000000832005', N'Delivered', 10034, 10, 2008, N'0', N'SAM0113', N'25-Sep-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-21T14:49:55.000' AS DateTime), NULL, CAST(N'2021-09-21T14:59:52.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8029, N'Samsung', N'4186686000000823011', N'Delivered', 10035, 10, 2008, N'0', N'SAM0114', N'25-Sep-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-09-21T15:20:00.000' AS DateTime), NULL, CAST(N'2021-09-21T16:54:24.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8030, N'Samsung', NULL, N'Order Created', 10036, 12, 2008, N'0', N'SAM0110', N'25-Oct-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-10-16T12:36:48.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8031, N'Samsung', NULL, N'Order Created', 10037, 12, 2008, N'0', N'SAM0115', N'25-Oct-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-10-16T13:14:21.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8032, N'Samsung', N'4186686000001038005', N'Order Created', 10038, 12, 2008, N'0', N'SAM0116', N'25-Oct-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-10-16T13:27:16.000' AS DateTime), NULL, CAST(N'2021-10-16T13:27:16.000' AS DateTime))
GO
INSERT [dbo].[tblExchangeOrder] ([Id], [CompanyName], [ZohoSponsorOrderId], [OrderStatus], [CustomerDetailsId], [ProductTypeId], [BrandId], [Bonus], [SponsorOrderNumber], [EstimatedDeliveryDate], [ProductCondition], [LoginID], [ExchPriceCode], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8033, N'Samsung', N'4186686000001053005', N'Order Created', 10039, 12, 2008, N'0', N'SAM0117', N'25-Oct-2021', N'Excellent', 1, NULL, 1, NULL, CAST(N'2021-10-18T16:20:38.000' AS DateTime), NULL, CAST(N'2021-10-18T16:20:38.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[tblExchangeOrder] OFF
GO
SET IDENTITY_INSERT [dbo].[tblMessageDetail] ON 
GO
INSERT [dbo].[tblMessageDetail] ([MessageDetailId], [Code], [PhoneNumber], [Message], [ResponseJSON], [SendDate], [MessageType], [Email]) VALUES (1, N'6768', N'8319010116', N'Dear Customer - OTP for registration for the UTC Assured Buyback Program is 6768 by TUTC', N'', CAST(N'2022-01-27T16:58:05.710' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblMessageDetail] ([MessageDetailId], [Code], [PhoneNumber], [Message], [ResponseJSON], [SendDate], [MessageType], [Email]) VALUES (2, N'6797', N'7415247205', N'Dear Customer - OTP for registration for the UTC Assured Buyback Program is 6797 by TUTC', N'', CAST(N'2022-01-27T16:45:21.467' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblMessageDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[tblModOfPayment] ON 
GO
INSERT [dbo].[tblModOfPayment] ([Id], [ModeofPayment], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Cash', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblModOfPayment] ([Id], [ModeofPayment], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'UPI', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblModOfPayment] ([Id], [ModeofPayment], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'Bank Transfer', 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblModOfPayment] OFF
GO
SET IDENTITY_INSERT [dbo].[tblPickupStatus] ON 
GO
INSERT [dbo].[tblPickupStatus] ([Id], [Status], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'pending', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPickupStatus] ([Id], [Status], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'processing', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPickupStatus] ([Id], [Status], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'completed', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPickupStatus] ([Id], [Status], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'cancelled ', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPickupStatus] ([Id], [Status], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, N'accepted', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPickupStatus] ([Id], [Status], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, N'rejected', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPickupStatus] ([Id], [Status], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7, N'not yet confirm', 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblPickupStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[tblPinCode] ON 
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 110001, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, 110002, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, 110003, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, 110004, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, 110005, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, 110006, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7, 110007, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8, 110008, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (9, 110009, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10, 110010, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (11, 110011, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (12, 110012, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (13, 110013, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (14, 110014, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (15, 110015, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (16, 110016, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (17, 110017, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (18, 110018, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (19, 110019, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (20, 110020, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (21, 110021, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (22, 110022, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (23, 110023, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (24, 110024, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (25, 110025, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (26, 110026, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (27, 110027, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (28, 110028, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (29, 110029, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (30, 110030, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (31, 110031, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (32, 110032, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (33, 110033, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (34, 110034, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (35, 110035, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (36, 110036, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (37, 110037, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (38, 110038, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (39, 110039, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (40, 110040, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (41, 110041, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (42, 110042, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (43, 110043, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (44, 110044, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (45, 110045, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (46, 110046, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (47, 110047, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (48, 110048, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (49, 110049, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (50, 110050, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (51, 110051, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (52, 110052, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (53, 110053, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (54, 110054, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (55, 110055, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (56, 110056, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (57, 110057, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (58, 110058, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (59, 110059, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (60, 110060, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (61, 110061, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (62, 110062, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (63, 110063, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (64, 110064, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (65, 110065, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (66, 110066, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (67, 110067, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (68, 110068, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (69, 110069, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (70, 110070, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (71, 110071, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (72, 110072, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (73, 110073, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (74, 110074, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (75, 110075, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (76, 110076, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (77, 110077, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (78, 110078, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (79, 110079, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (80, 110080, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (81, 110081, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (82, 110082, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (83, 110083, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (84, 110084, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (85, 110085, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (86, 110086, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (87, 110087, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (88, 110088, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (89, 110089, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (90, 110090, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (91, 110091, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (92, 110092, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (93, 110093, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (94, 110094, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (95, 110095, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (96, 110096, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (97, 110097, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (98, 110098, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (99, 110099, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (100, 110101, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (101, 110102, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (102, 110103, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (103, 110104, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (104, 110105, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (105, 110106, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (106, 110107, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (107, 110108, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (108, 110109, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (109, 110110, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (110, 110112, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (111, 110113, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (112, 110114, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (113, 110115, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (114, 110116, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (115, 110117, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (116, 110118, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (117, 110119, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (118, 110120, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (119, 110121, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (120, 110122, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (121, 110124, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (122, 110125, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (123, 110301, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (124, 110302, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (125, 110401, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (126, 110402, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (127, 110403, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (128, 110501, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (129, 110502, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (130, 110503, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (131, 110504, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (132, 110505, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (133, 110506, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (134, 110507, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (135, 110508, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (136, 110509, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (137, 110510, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (138, 110511, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (139, 110512, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (140, 110601, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (141, 110602, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (142, 110603, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (143, 110604, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (144, 110605, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (145, 110606, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (146, 110607, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (147, 110608, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (148, 110609, N'Delhi', N'Delhi', N'Delhi', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (149, 120001, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (150, 120002, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (151, 121000, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (152, 121001, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (153, 121002, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (154, 121003, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (155, 121004, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (156, 121005, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (157, 121006, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (158, 121007, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (159, 121008, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (160, 121009, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (161, 121010, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (162, 121011, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (163, 121012, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (164, 121013, N'Faridabad', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (165, 122000, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (166, 122001, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (167, 122002, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (168, 122003, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (169, 122004, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (170, 122005, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (171, 122006, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (172, 122007, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (173, 122008, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (174, 122009, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (175, 122010, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (176, 122011, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (177, 122012, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (178, 122013, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (179, 122014, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (180, 122015, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (181, 122016, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (182, 122017, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (183, 122018, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (184, 122019, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (185, 122020, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (186, 122021, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (187, 122022, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (188, 122023, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (189, 122024, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (190, 122025, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (191, 122026, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (192, 122027, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (193, 122028, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (194, 122029, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (195, 122030, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (196, 122031, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (197, 122051, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (198, 122101, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (199, 122102, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (200, 122203, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (201, 122204, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (202, 122206, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (203, 122207, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (204, 122208, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (205, 122209, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (206, 122210, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (207, 122211, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (208, 122213, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (209, 122214, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (210, 122215, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (211, 122216, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (212, 122217, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (213, 122218, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (214, 122219, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (215, 122220, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (216, 122223, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (217, 122224, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (218, 122225, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (219, 122226, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (220, 122227, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (221, 122228, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (222, 122229, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (223, 122230, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (224, 122231, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (225, 122232, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (226, 122233, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (227, 122234, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (228, 123505, N'Gurgaon', N'Delhi', N'Haryana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (229, 201301, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (230, 201302, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (231, 201303, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (232, 201304, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (233, 201305, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (234, 201306, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (235, 201307, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (236, 201308, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (237, 201309, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (238, 201310, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (239, 201311, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (240, 201312, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (241, 201313, N'Noida', N'Delhi', N'Uttar Pradesh', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (242, 400001, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (243, 400002, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (244, 400003, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (245, 400004, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (246, 400005, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (247, 400006, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (248, 400007, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (249, 400008, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (250, 400009, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (251, 400010, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (252, 400011, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (253, 400012, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (254, 400013, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (255, 400014, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (256, 400015, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (257, 400016, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (258, 400017, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (259, 400018, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (260, 400019, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (261, 400020, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (262, 400021, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (263, 400022, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (264, 400023, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (265, 400024, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (266, 400025, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (267, 400026, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (268, 400027, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (269, 400028, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (270, 400029, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (271, 400030, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (272, 400031, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (273, 400032, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (274, 400033, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (275, 400034, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (276, 400035, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (277, 400036, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (278, 400037, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (279, 400038, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (280, 400039, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (281, 400040, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (282, 400041, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (283, 400042, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (284, 400043, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (285, 400044, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (286, 400045, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (287, 400046, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (288, 400047, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (289, 400048, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (290, 400049, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (291, 400050, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (292, 400051, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (293, 400052, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (294, 400053, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (295, 400054, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (296, 400055, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (297, 400056, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (298, 400057, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (299, 400058, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (300, 400059, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (301, 400060, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (302, 400061, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (303, 400062, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (304, 400063, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (305, 400064, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (306, 400065, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (307, 400066, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (308, 400067, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (309, 400068, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (310, 400069, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (311, 400070, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (312, 400071, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (313, 400072, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (314, 400073, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (315, 400074, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (316, 400075, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (317, 400076, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (318, 400077, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (319, 400078, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (320, 400079, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (321, 400080, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (322, 400081, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (323, 400082, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (324, 400083, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (325, 400084, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (326, 400085, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (327, 400086, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (328, 400087, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (329, 400088, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (330, 400089, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (331, 400090, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (332, 400091, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (333, 400092, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (334, 400093, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (335, 400094, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (336, 400095, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (337, 400096, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (338, 400097, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (339, 400098, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (340, 400099, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (341, 400100, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (342, 400101, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (343, 400102, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (344, 400103, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (345, 400104, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (346, 400105, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (347, 400201, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (348, 400206, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (349, 400207, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (350, 400601, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (351, 400602, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (352, 400603, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (353, 400604, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (354, 400605, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (355, 400606, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (356, 400607, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (357, 400608, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (358, 400609, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (359, 400610, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (360, 400612, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (361, 400614, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (362, 400615, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (363, 400701, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (364, 400703, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (365, 400705, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (366, 400706, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (367, 400708, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (368, 400709, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (369, 400710, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (370, 400901, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (371, 401101, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (372, 401102, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (373, 401103, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (374, 401104, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (375, 401105, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (376, 401106, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (377, 401107, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (378, 401201, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (379, 401202, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (380, 401203, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (381, 401205, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (382, 401207, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (383, 401208, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (384, 401209, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (385, 401301, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (386, 401302, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (387, 401303, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (388, 401304, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (389, 401305, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (390, 410208, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (391, 410210, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (392, 410218, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (393, 421001, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (394, 421002, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (395, 421003, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (396, 421004, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (397, 421005, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (398, 421201, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (399, 421202, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (400, 421203, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (401, 421204, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (402, 421205, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (403, 421206, N'Mumbai', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (404, 421301, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (405, 421302, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (406, 421308, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (407, 421501, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (408, 421502, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (409, 421504, N'Thane', N'Mumbai', N'Maharashtra', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (410, 500001, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (411, 500002, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (412, 500003, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (413, 500004, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (414, 500006, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (415, 500007, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (416, 500008, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (417, 500009, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (418, 500010, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (419, 500011, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (420, 500012, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (421, 500013, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (422, 500014, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (423, 500015, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (424, 500016, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (425, 500017, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (426, 500018, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (427, 500019, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (428, 500020, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (429, 500021, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (430, 500022, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (431, 500023, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (432, 500024, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (433, 500025, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (434, 500026, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (435, 500027, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (436, 500028, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (437, 500029, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (438, 500030, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (439, 500031, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (440, 500032, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (441, 500033, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (442, 500034, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (443, 500035, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (444, 500036, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (445, 500037, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (446, 500038, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (447, 500039, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (448, 500040, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (449, 500041, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (450, 500042, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (451, 500044, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (452, 500045, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (453, 500046, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (454, 500047, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (455, 500048, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (456, 500049, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (457, 500050, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (458, 500051, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (459, 500052, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (460, 500053, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (461, 500054, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (462, 500055, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (463, 500056, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (464, 500057, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (465, 500058, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (466, 500059, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (467, 500060, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (468, 500061, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (469, 500062, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (470, 500063, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (471, 500064, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (472, 500065, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (473, 500066, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (474, 500067, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (475, 500068, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (476, 500069, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (477, 500070, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (478, 500071, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (479, 500072, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (480, 500073, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (481, 500074, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (482, 500075, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (483, 500076, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (484, 500077, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (485, 500079, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (486, 500080, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (487, 500081, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (488, 500082, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (489, 500083, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (490, 500084, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (491, 500085, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (492, 500086, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (493, 500087, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (494, 500088, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (495, 500089, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (496, 500090, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (497, 500091, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (498, 500092, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (499, 500093, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (500, 500094, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (501, 500095, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (502, 500096, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (503, 500097, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (504, 500098, N'Hyderabad', N'Hyderabad', N'Telengana', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (505, 560001, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (506, 560002, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (507, 560003, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (508, 560004, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (509, 560005, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (510, 560006, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (511, 560007, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (512, 560008, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (513, 560009, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (514, 560010, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (515, 560011, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (516, 560012, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (517, 560013, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (518, 560014, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (519, 560015, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (520, 560016, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (521, 560017, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (522, 560018, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (523, 560019, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (524, 560020, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (525, 560021, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (526, 560022, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (527, 560023, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (528, 560024, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (529, 560025, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (530, 560026, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (531, 560027, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (532, 560028, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (533, 560029, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (534, 560030, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (535, 560031, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (536, 560032, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (537, 560033, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (538, 560034, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (539, 560035, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (540, 560036, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (541, 560037, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (542, 560038, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (543, 560039, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (544, 560040, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (545, 560041, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (546, 560042, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (547, 560043, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (548, 560044, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (549, 560045, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (550, 560046, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (551, 560047, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (552, 560048, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (553, 560049, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (554, 560050, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (555, 560051, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (556, 560052, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (557, 560053, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (558, 560054, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (559, 560055, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (560, 560056, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (561, 560057, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (562, 560058, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (563, 560059, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (564, 560060, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (565, 560061, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (566, 560062, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (567, 560063, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (568, 560064, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (569, 560065, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (570, 560066, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (571, 560067, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (572, 560068, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (573, 560069, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (574, 560070, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (575, 560071, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (576, 560072, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (577, 560073, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (578, 560075, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (579, 560076, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (580, 560077, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (581, 560078, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (582, 560079, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (583, 560080, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (584, 560081, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (585, 560082, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (586, 560083, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (587, 560084, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (588, 560085, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (589, 560086, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (590, 560087, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (591, 560090, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (592, 560091, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (593, 560092, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (594, 560093, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (595, 560094, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (596, 560095, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (597, 560096, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (598, 560097, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (599, 560098, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (600, 560099, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (601, 560100, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (602, 560102, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (603, 560103, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (604, 560104, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (605, 560105, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (606, 560106, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (607, 560107, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (608, 560109, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (609, 561229, N'Bangalore', N'Bangalore', N'Karnataka', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (610, 600001, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (611, 600002, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (612, 600003, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (613, 600004, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (614, 600005, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (615, 600006, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (616, 600007, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (617, 600008, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (618, 600009, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (619, 600010, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (620, 600011, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (621, 600012, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (622, 600013, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (623, 600014, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (624, 600015, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (625, 600016, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (626, 600017, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (627, 600018, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (628, 600019, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (629, 600020, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (630, 600021, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (631, 600022, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (632, 600023, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (633, 600024, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (634, 600025, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (635, 600026, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (636, 600027, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (637, 600028, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (638, 600029, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (639, 600030, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (640, 600031, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (641, 600032, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (642, 600033, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (643, 600034, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (644, 600035, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (645, 600036, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (646, 600037, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (647, 600038, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (648, 600039, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (649, 600040, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (650, 600041, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (651, 600042, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (652, 600043, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (653, 600044, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (654, 600045, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (655, 600046, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (656, 600047, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (657, 600048, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (658, 600049, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (659, 600050, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (660, 600051, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (661, 600052, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (662, 600053, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (663, 600054, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (664, 600055, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (665, 600056, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (666, 600057, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (667, 600058, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (668, 600059, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (669, 600060, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (670, 600061, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (671, 600062, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (672, 600063, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (673, 600064, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (674, 600065, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (675, 600066, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (676, 600067, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (677, 600068, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (678, 600069, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (679, 600070, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (680, 600071, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (681, 600072, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (682, 600073, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (683, 600074, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (684, 600075, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (685, 600076, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (686, 600077, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (687, 600078, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (688, 600079, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (689, 600080, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (690, 600081, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (691, 600082, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (692, 600083, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (693, 600084, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (694, 600085, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (695, 600086, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (696, 600087, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (697, 600088, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (698, 600089, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (699, 600090, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (700, 600091, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (701, 600092, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (702, 600093, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (703, 600094, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (704, 600095, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (705, 600096, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (706, 600097, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (707, 600098, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (708, 600099, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (709, 600100, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (710, 600101, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (711, 600102, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (712, 600103, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (713, 600104, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (714, 600105, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (715, 600106, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (716, 600107, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (717, 600108, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (718, 600109, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (719, 600110, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (720, 600111, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (721, 600112, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (722, 600113, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (723, 600114, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (724, 600115, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (725, 600116, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (726, 600117, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (727, 600118, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (728, 600119, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (729, 600123, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (730, 600125, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (731, 600126, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (732, 602101, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (733, 602102, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (734, 603103, N'Chennai', N'Chennai', N'Tamil Nadu', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (735, 700001, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (736, 700002, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (737, 700003, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (738, 700004, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (739, 700005, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (740, 700006, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (741, 700007, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (742, 700008, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (743, 700009, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (744, 700010, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (745, 700011, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (746, 700012, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (747, 700013, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (748, 700014, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (749, 700015, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (750, 700016, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (751, 700017, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (752, 700018, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (753, 700019, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (754, 700020, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (755, 700021, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (756, 700022, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (757, 700023, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (758, 700024, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (759, 700025, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (760, 700026, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (761, 700027, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (762, 700028, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (763, 700029, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (764, 700030, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (765, 700031, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (766, 700032, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (767, 700033, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (768, 700034, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (769, 700035, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (770, 700036, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (771, 700037, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (772, 700038, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (773, 700039, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (774, 700040, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (775, 700041, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (776, 700042, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (777, 700043, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (778, 700044, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (779, 700045, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (780, 700046, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (781, 700047, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (782, 700048, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (783, 700049, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (784, 700050, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (785, 700051, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (786, 700052, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (787, 700053, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (788, 700054, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (789, 700055, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (790, 700056, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (791, 700057, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (792, 700058, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (793, 700059, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (794, 700060, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (795, 700061, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (796, 700062, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (797, 700063, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (798, 700064, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (799, 700065, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (800, 700066, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (801, 700067, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (802, 700068, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (803, 700069, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (804, 700070, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (805, 700071, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (806, 700072, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (807, 700073, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (808, 700074, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (809, 700075, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (810, 700076, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (811, 700077, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (812, 700078, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (813, 700079, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (814, 700080, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (815, 700081, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (816, 700082, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (817, 700083, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (818, 700084, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (819, 700085, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (820, 700086, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (821, 700087, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (822, 700088, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (823, 700089, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (824, 700090, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (825, 700092, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (826, 700094, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (827, 700095, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (828, 700099, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (829, 700107, N'Kolkata', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (830, 711001, N'Howrah', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (831, 711101, N'Howrah', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (832, 711102, N'Howrah', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (833, 711108, N'Howrah', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (834, 711111, N'Howrah', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (835, 711113, N'Howrah', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (836, 711103, N'Howrah', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (837, 711104, N'Howrah', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblPinCode] ([Id], [ZipCode], [Location], [HubControl], [State], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (838, 711106, N'Howrah', N'Kolkata', N'West Bengal', 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblPinCode] OFF
GO
SET IDENTITY_INSERT [dbo].[tblPriceMaster] ON 
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'4186686000001409007', N'SSG-PRICE-001', 1, N'Refrigerator', 1, N'Single Door', N'RDC', N'Samsung', N'LG', N'Bosch', NULL, N'1800.00', N'1200.00', N'1000.00', N'0.00', N'1650.00', N'1200.00', N'1000.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'4186686000001409011', N'SSG-PRICE-001', 1, N'Refrigerator', 2, N'Double Door', N'RF2', N'Samsung', N'LG', N'Bosch', NULL, N'2800.00', N'2000.00', N'1200.00', N'0.00', N'2550.00', N'2000.00', N'1200.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'4186686000001409015', N'SSG-PRICE-001', 1, N'Refrigerator', 3, N'Triple Door', N'RF3', N'Samsung', N'LG', N'Bosch', NULL, N'3200.00', N'2200.00', N'1200.00', N'0.00', N'2900.00', N'2200.00', N'1200.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'4186686000001409023', N'SSG-PRICE-001', 1, N'Refrigerator', 4, N'Side by Side', N'RSX', N'Samsung', N'LG', N'Bosch', NULL, N'9900.00', N'5000.00', N'1200.00', N'0.00', N'9000.00', N'5000.00', N'1200.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, N'4186686000001409019', N'SSG-PRICE-001', 1, N'Refrigerator', 5, N'Bottom Mounted', N'RBM', N'Samsung', N'LG', N'Bosch', NULL, N'3200.00', N'2200.00', N'1200.00', N'0.00', N'2900.00', N'2200.00', N'1200.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, N'4186686000001409043', N'SSG-PRICE-001', 4, N'Air Conditioner', 9, N'Window AC', N'ACW', N'Samsung', N'LG', N'Bosch', NULL, N'3300.00', N'2000.00', N'1000.00', N'0.00', N'3000.00', N'2000.00', N'1000.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7, N'4186686000001409039', N'SSG-PRICE-001', 4, N'Air Conditioner', 10, N'Split AC', N'ACS', N'Samsung', N'LG', N'Bosch', NULL, N'5500.00', N'3000.00', N'1500.00', N'0.00', N'5000.00', N'3000.00', N'1500.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8, N'4186686000001409027', N'SSG-PRICE-001', 2, N'Washing Machine', 6, N'Semi Automatic', N'WSA', N'Samsung', N'LG', N'Bosch', NULL, N'1200.00', N'800.00', N'500.00', N'0.00', N'1100.00', N'800.00', N'500.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (9, N'4186686000001409031', N'SSG-PRICE-001', 2, N'Washing Machine', 7, N'Fully Automatic Top load', N'WFA', N'Samsung', N'LG', N'Bosch', NULL, N'1800.00', N'1400.00', N'1000.00', N'0.00', N'1650.00', N'1400.00', N'1000.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10, N'4186686000001409035', N'SSG-PRICE-001', 2, N'Washing Machine', 8, N'Fully Automatic Front load', N'WFL', N'Samsung', N'LG', N'Bosch', NULL, N'2500.00', N'1500.00', N'1000.00', N'0.00', N'2250.00', N'1500.00', N'1000.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (11, N'4186686000001409047', N'SSG-PRICE-001', 3, N'Television', 11, N'LCD', N'TLC23', N'Samsung', N'LG', N'Bosch', NULL, N'1500.00', N'800.00', N'500.00', N'0.00', N'1350.00', N'800.00', N'500.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (12, N'4186686000001409051', N'SSG-PRICE-001', 3, N'Television', 12, N'LCD', N'TLC32', N'Samsung', N'LG', N'Bosch', NULL, N'2000.00', N'1200.00', N'1000.00', N'0.00', N'1800.00', N'1200.00', N'1000.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (13, N'4186686000001409055', N'SSG-PRICE-001', 3, N'Television', 13, N'LCD', N'TLC43', N'Samsung', N'LG', N'Bosch', NULL, N'2400.00', N'1500.00', N'1000.00', N'0.00', N'2150.00', N'1500.00', N'1000.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (14, N'4186686000001409059', N'SSG-PRICE-001', 3, N'Television', 14, N'LCD', N'TLC55', N'Samsung', N'LG', N'Bosch', NULL, N'3200.00', N'1800.00', N'1000.00', N'0.00', N'2900.00', N'1800.00', N'1000.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (15, N'4186686000001409063', N'SSG-PRICE-001', 3, N'Television', 15, N'LCD', N'TLC99', N'Samsung', N'LG', N'Bosch', NULL, N'4000.00', N'2000.00', N'1000.00', N'0.00', N'3650.00', N'2000.00', N'1000.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (16, N'4186686000001409067', N'SSG-PRICE-001', 3, N'Television', 17, N'LED', N'TLE23', N'Samsung', N'LG', N'Bosch', NULL, N'1600.00', N'800.00', N'500.00', N'0.00', N'1450.00', N'800.00', N'500.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (17, N'4186686000001409071', N'SSG-PRICE-001', 3, N'Television', 18, N'LED', N'TLE32', N'Samsung', N'LG', N'Bosch', NULL, N'2400.00', N'1500.00', N'1100.00', N'0.00', N'2150.00', N'1500.00', N'1100.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (18, N'4186686000001409075', N'SSG-PRICE-001', 3, N'Television', 19, N'LED', N'TLE43', N'Samsung', N'LG', N'Bosch', NULL, N'2800.00', N'2000.00', N'1400.00', N'0.00', N'2550.00', N'2000.00', N'1400.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (19, N'4186686000001409079', N'SSG-PRICE-001', 3, N'Television', 20, N'LED', N'TLE55', N'Samsung', N'LG', N'Bosch', NULL, N'5000.00', N'4000.00', N'1600.00', N'0.00', N'4500.00', N'4000.00', N'1600.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (20, N'4186686000001409083', N'SSG-PRICE-001', 3, N'Television', 21, N'LED', N'TLE99', N'Samsung', N'LG', N'Bosch', NULL, N'8000.00', N'5000.00', N'2000.00', N'0.00', N'7200.00', N'5000.00', N'2000.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (21, N'4186686000001409087', N'SSG-PRICE-001', 3, N'Television', 23, N'Smart TV', N'TSM23', N'Samsung', N'LG', N'Bosch', NULL, N'2400.00', N'800.00', N'500.00', N'0.00', N'2150.00', N'800.00', N'500.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (22, N'4186686000001409091', N'SSG-PRICE-001', 3, N'Television', 24, N'Smart TV', N'TSM32', N'Samsung', N'LG', N'Bosch', NULL, N'2800.00', N'1500.00', N'1100.00', N'0.00', N'2500.00', N'1500.00', N'1100.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (23, N'4186686000001409095', N'SSG-PRICE-001', 3, N'Television', 25, N'Smart TV', N'TSM43', N'Samsung', N'LG', N'Bosch', NULL, N'3500.00', N'2500.00', N'1500.00', N'0.00', N'3150.00', N'2500.00', N'1500.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (24, N'4186686000001409099', N'SSG-PRICE-001', 3, N'Television', 26, N'Smart TV', N'TSM55', N'Samsung', N'LG', N'Bosch', NULL, N'7000.00', N'6000.00', N'2400.00', N'0.00', N'6300.00', N'6000.00', N'2400.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:31.000' AS DateTime))
GO
INSERT [dbo].[tblPriceMaster] ([Id], [ZohoPriceMasterId], [ExchPriceCode], [ProductCategoryId], [ProductCat], [ProductTypeId], [ProductType], [ProductTypeCode], [BrandName-1], [BrandName-2], [BrandName-3], [BrandName-4], [Quote-P-High], [Quote-Q-High], [Quote-R-High], [Quote-S-High], [Quote-P], [Quote-Q], [Quote-R], [Quote-S], [PriceStartDate], [PriceEndDate], [OtherBrand], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (25, N'4186686000001409103', N'SSG-PRICE-001', 3, N'Television', 27, N'Smart TV', N'TSM99', N'Samsung', N'LG', N'Bosch', NULL, N'9000.00', N'7000.00', N'2500.00', N'0.00', N'8100.00', N'7000.00', N'2500.00', N'0.00', N'14-Dec-2021', N'31-Dec-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2022-01-04T12:38:30.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[tblPriceMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[tblProductCategory] ON 
GO
INSERT [dbo].[tblProductCategory] ([Id], [Name], [Description], [Code], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'RF', N'Refrigerator', N'RF', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductCategory] ([Id], [Name], [Description], [Code], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'WM', N'Washing machine', N'WM', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductCategory] ([Id], [Name], [Description], [Code], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'TV', N'Television', N'TV', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductCategory] ([Id], [Name], [Description], [Code], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'AC', N'Air Conditioner', N'AC', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductCategory] ([Id], [Name], [Description], [Code], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, N'DW', N'Dishwasher', N'DW', 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblProductCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[tblProductType] ON 
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'RDC', N'Single Door', N'RDC', NULL, 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'RF2', N'Double Door', N'RF2', NULL, 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'RF3', N'Triple Door', N'RF3', NULL, 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'RSX', N'Side by Side', N'RSX', NULL, 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (5, N'RBM', N'Bottom Mounted', N'RBM', NULL, 1, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (6, N'WSA', N'Semi Automatic', N'WSA', NULL, 2, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (7, N'WFA', N'Fully Automatic Top load', N'WFA', NULL, 2, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (8, N'WFL', N'Fully Automatic Front load', N'WFL', NULL, 2, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (9, N'ACW', N'Window AC', N'ACW', NULL, 4, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (10, N'ACS', N'Split AC', N'ACS', NULL, 4, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (11, N'TLC23', N'LCD', N'TLC23', N'<23 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (12, N'TLC32', N'LCD', N'TLC32', N'24 - 32 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (13, N'TLC43', N'LCD', N'TLC43', N'33 - 43 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (14, N'TLC55', N'LCD', N'TLC55', N'44 - 55 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (15, N'TLC99', N'LCD', N'TLC99', N'>55 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (17, N'TLE23', N'LED', N'TLE23', N'<23 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (18, N'TLE32', N'LED', N'TLE32', N'24 - 32 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (19, N'TLE43', N'LED', N'TLE43', N'33 - 43 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (20, N'TLE55', N'LED', N'TLE55', N'44 - 55 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (21, N'TLE99', N'LED', N'TLE99', N'>55 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (23, N'TSM23', N'Smart TV', N'TSM23', N'<23 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (24, N'TSM32', N'Smart TV', N'TSM32', N'24 - 32 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (25, N'TSM43', N'Smart TV', N'TSM43', N'33 - 43 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (26, N'TSM55', N'Smart TV', N'TSM55', N'44 - 55 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tblProductType] ([Id], [Name], [Description], [Code], [Size], [ProductCatId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (27, N'TSM99', N'Smart TV', N'TSM99', N'>55 inches', 3, 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblProductType] OFF
GO
SET IDENTITY_INSERT [dbo].[tblProgramMaster] ON 
GO
INSERT [dbo].[tblProgramMaster] ([Id], [ZohoProgramMasterID], [LoginCredentials], [SponsorBussinessName], [ProgramCode], [Exch], [ExchPriceCode], [PaymentTo], [ABB], [ABBPriceCode], [ProgStartDate], [ProgEndDate], [PreeQC], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'4186686000000229003', N'anand@digi2L.in', N'Test Sponsor', N'TEST-PG-001', N'Y', N'TEST-PR-001', N'Sponsor (9R)', N'Y', N'', N'15-Jul-2021', N'01-Jan-2022', NULL, 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:31.000' AS DateTime))
GO
INSERT [dbo].[tblProgramMaster] ([Id], [ZohoProgramMasterID], [LoginCredentials], [SponsorBussinessName], [ProgramCode], [Exch], [ExchPriceCode], [PaymentTo], [ABB], [ABBPriceCode], [ProgStartDate], [ProgEndDate], [PreeQC], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'4186686000000214003', N'', N'TATA CLiQ', N'TCQ-ABB-001', N'N', N'', N'Sponsor (9R)', N'Y', N'TCQ-ABB-PR-001', N'12-Jul-2021', N'31-Dec-2021', NULL, 1, NULL, NULL, NULL, CAST(N'2021-08-19T18:21:31.000' AS DateTime))
GO
INSERT [dbo].[tblProgramMaster] ([Id], [ZohoProgramMasterID], [LoginCredentials], [SponsorBussinessName], [ProgramCode], [Exch], [ExchPriceCode], [PaymentTo], [ABB], [ABBPriceCode], [ProgStartDate], [ProgEndDate], [PreeQC], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, N'4186686000000401131', N'', N'TCQ', N'TCQ-ABB-001', N'N', N'', N'Sponsor', N'Y', N'TCQ-ABB-PR-001', N'12-Jul-2021', N'31-Dec-2021', NULL, 1, NULL, CAST(N'2021-08-16T12:06:25.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[tblProgramMaster] ([Id], [ZohoProgramMasterID], [LoginCredentials], [SponsorBussinessName], [ProgramCode], [Exch], [ExchPriceCode], [PaymentTo], [ABB], [ABBPriceCode], [ProgStartDate], [ProgEndDate], [PreeQC], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, N'4186686000000401127', N'', N'TEST', N'TEST-PG-001', N'Y', N'TEST-PR-001', N'Sponsor', N'Y', N'', N'15-Jul-2021', N'01-Jan-2022', NULL, 1, NULL, CAST(N'2021-08-16T12:09:15.000' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tblProgramMaster] OFF
GO
ALTER TABLE [dbo].[tblABBRegistration]  WITH CHECK ADD  CONSTRAINT [FK__tblABBReg__BP__2B0A656D] FOREIGN KEY([BusinessPartnerId])
REFERENCES [dbo].[tblBusinessPartner] ([BusinessPartnerId])
GO
ALTER TABLE [dbo].[tblABBRegistration] CHECK CONSTRAINT [FK__tblABBReg__BP__2B0A656D]
GO
ALTER TABLE [dbo].[tblABBRegistration]  WITH CHECK ADD  CONSTRAINT [FK__tblABBReg__Brand__2B0A656D] FOREIGN KEY([NewBrandId])
REFERENCES [dbo].[tblBrand] ([Id])
GO
ALTER TABLE [dbo].[tblABBRegistration] CHECK CONSTRAINT [FK__tblABBReg__Brand__2B0A656D]
GO
ALTER TABLE [dbo].[tblABBRegistration]  WITH CHECK ADD  CONSTRAINT [FK__tblABBReg__PC__2B0A656D] FOREIGN KEY([NewProductCategoryId])
REFERENCES [dbo].[tblProductCategory] ([Id])
GO
ALTER TABLE [dbo].[tblABBRegistration] CHECK CONSTRAINT [FK__tblABBReg__PC__2B0A656D]
GO
ALTER TABLE [dbo].[tblABBRegistration]  WITH CHECK ADD  CONSTRAINT [FK__tblABBReg__PT__2B0A656D] FOREIGN KEY([NewProductCategoryTypeId])
REFERENCES [dbo].[tblProductType] ([Id])
GO
ALTER TABLE [dbo].[tblABBRegistration] CHECK CONSTRAINT [FK__tblABBReg__PT__2B0A656D]
GO
ALTER TABLE [dbo].[tblABBRegistration]  WITH CHECK ADD  CONSTRAINT [FK_tblABBRegistration_BusinessUnitId] FOREIGN KEY([BusinessUnitId])
REFERENCES [dbo].[tblBusinessUnit] ([BusinessUnitId])
GO
ALTER TABLE [dbo].[tblABBRegistration] CHECK CONSTRAINT [FK_tblABBRegistration_BusinessUnitId]
GO
ALTER TABLE [dbo].[tblBusinessPartner]  WITH CHECK ADD  CONSTRAINT [FK_tblBusinessPartner_BusinessUnitId] FOREIGN KEY([BusinessUnitId])
REFERENCES [dbo].[tblBusinessUnit] ([BusinessUnitId])
GO
ALTER TABLE [dbo].[tblBusinessPartner] CHECK CONSTRAINT [FK_tblBusinessPartner_BusinessUnitId]
GO
ALTER TABLE [dbo].[tblBusinessUnit]  WITH CHECK ADD  CONSTRAINT [FK_tblBusinessUnit_LoginId] FOREIGN KEY([LoginId])
REFERENCES [dbo].[Login] ([id])
GO
ALTER TABLE [dbo].[tblBusinessUnit] CHECK CONSTRAINT [FK_tblBusinessUnit_LoginId]
GO
ALTER TABLE [dbo].[tblExchangeOrder]  WITH CHECK ADD FOREIGN KEY([BrandId])
REFERENCES [dbo].[tblBrand] ([Id])
GO
ALTER TABLE [dbo].[tblExchangeOrder]  WITH CHECK ADD FOREIGN KEY([CustomerDetailsId])
REFERENCES [dbo].[tblCustomerDetails] ([Id])
GO
ALTER TABLE [dbo].[tblExchangeOrder]  WITH CHECK ADD  CONSTRAINT [FK__tblSponso__Produ__2057CCD0] FOREIGN KEY([ProductTypeId])
REFERENCES [dbo].[tblProductType] ([Id])
GO
ALTER TABLE [dbo].[tblExchangeOrder] CHECK CONSTRAINT [FK__tblSponso__Produ__2057CCD0]
GO
ALTER TABLE [dbo].[tblImages]  WITH CHECK ADD FOREIGN KEY([BizlogTicketId])
REFERENCES [dbo].[tblBizlogTicket] ([Id])
GO
ALTER TABLE [dbo].[tblImages]  WITH CHECK ADD FOREIGN KEY([SponsorId])
REFERENCES [dbo].[tblExchangeOrder] ([Id])
GO
ALTER TABLE [dbo].[tblPriceMaster]  WITH CHECK ADD  CONSTRAINT [FK__tblProduc__Produ__1B9317B3] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[tblProductCategory] ([Id])
GO
ALTER TABLE [dbo].[tblPriceMaster] CHECK CONSTRAINT [FK__tblProduc__Produ__1B9317B3]
GO
ALTER TABLE [dbo].[tblPriceMaster]  WITH CHECK ADD  CONSTRAINT [FK__tblProduc__Produ__1C873BEC] FOREIGN KEY([ProductTypeId])
REFERENCES [dbo].[tblProductType] ([Id])
GO
ALTER TABLE [dbo].[tblPriceMaster] CHECK CONSTRAINT [FK__tblProduc__Produ__1C873BEC]
GO
ALTER TABLE [dbo].[tblProductType]  WITH CHECK ADD  CONSTRAINT [FK__tblProduc__Produ__2B0A656D] FOREIGN KEY([ProductCatId])
REFERENCES [dbo].[tblProductCategory] ([Id])
GO
ALTER TABLE [dbo].[tblProductType] CHECK CONSTRAINT [FK__tblProduc__Produ__2B0A656D]
GO
/****** Object:  StoredProcedure [dbo].[LoginByUsernamePassword]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoginByUsernamePassword]   
    @username varchar(50),  
    @password varchar(50)  
AS  
BEGIN  
    SELECT id, username, password  
    FROM Login  
    WHERE username = @username  
    AND password = @password  
END  
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBrabdList]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_GetBrabdList]
AS
Begin
Select Distinct B.* From tblBrand B
Inner Join tblPriceMaster P On (B.Name = P.[BrandName-1] OR B.Name = P.[BrandName-2] OR B.Name = P.[BrandName-3] OR B.Name = P.[BrandName-4] OR B.Name = P.[OtherBrand])
End

GO
/****** Object:  StoredProcedure [dbo].[sp_GetClientCredentials]    Script Date: 1/27/2022 5:10:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[sp_GetClientCredentials]
@username nvarchar(200) = null,
@password nvarchar(200) = null
AS
BEGIN
SELECT [Id], [EmailId], [BusinessName], [ClientId], [SecretId] FROM tblClientCredentials Where ClientId = @username AND SecretId = @password
END 
GO
USE [master]
GO
ALTER DATABASE [Digi2l_DB] SET  READ_WRITE 
GO

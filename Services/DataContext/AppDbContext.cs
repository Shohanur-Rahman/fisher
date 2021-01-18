using Services.Domain;
using Services.Domain.Accounts.Models;
using Services.Domain.Accounts.Models.FarmRents;
using Services.Domain.Accounts.Models.Feed;
using Services.Domain.Accounts.Models.FeedDistribute;
using Services.Domain.Accounts.Models.Fish;
using Services.Domain.Company;
using Services.Domain.Company.Models.CompanyAdmin;
using Services.Domain.Company.SPModels;
using Services.Domain.CostReport;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.CreateSegement.Models.Fish;
using Services.Domain.CreateSegement.Models.Project;
using Services.Domain.FarmExpend;
using Services.Domain.FeedPurchase;
using Services.Domain.UserBilling;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Services.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {

        }

        public virtual DbSet<CreateSegmentFish> CreateSegmentFish { get; set; }

        public virtual DbSet<CreateSegmentFishSeller> CreateSegmentFishSeller { get; set; }

        public virtual DbSet<CreateSegmentFeed> CreateSegmentFeed { get; set; }

        public virtual DbSet<CreateSegmentFeedCategories> CreateSegmentFeedCategories { get; set; }

        public virtual DbSet<CreateSegmentArea> CreateSegmentArea { get; set; }

        public virtual DbSet<CreateSegmentProject> CreateSegmentProject { get; set; }

        public virtual DbSet<FishSellingReportTableDomain> AccountsSellsFishSellCalculation { get; set; }
        public virtual DbSet<FishSellingReportMapperTable> FishSellingReportMapperTable { get; set; }
        public virtual DbSet<FishAmountDueReportMapperTable> FishAmountDueReportMapperTable { get; set; }

        //public virtual DbSet <FishSellingReportMapperDomainService> FishSellingReportMapperDomainService { get; set; }

        public virtual DbSet<FeedPurcaseTable> FeedPurcaseTable { get; set; }
        public virtual DbSet<FeedPurchaseReportTable> FeedPurchaseReportTable { get; set; }
        public virtual DbSet<FeedPurchaseDueReportMapperTable> FeedPurchaseDueReportMapperTable { get; set; }
        public virtual DbSet<FeedSellingReportTableDomain> FeedSellingReportTableDomain { get; set; }

        public virtual DbSet<FeedDistributionTableDoamain> FeedDistributionTableDoamain { get; set; }

        public virtual DbSet<FarmRentsCostsTable> FarmRentsCostsTable { get; set; }

        public virtual DbSet<FishBillingHistoryTable> FishBillingHistoryTable { get; set; }

        public virtual DbSet<CostExpandTable> CostExpandTable { get; set; }
        public virtual DbSet<ExpendReportMapper> ExpendReportMapper { get; set; }
        public virtual DbSet<ExpendReportTable> ExpendReportTable { get; set; }
        
        public virtual DbSet<User> Users { get; set; } 

        #region "For Multi User"
        public virtual DbSet<CompanyAdmin> CompanyAdmin { get; set; }

        #endregion


        


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
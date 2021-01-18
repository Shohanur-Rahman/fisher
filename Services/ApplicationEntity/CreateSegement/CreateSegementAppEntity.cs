using Services.DomainServices.CreateSegement.Area;
using Services.DomainServices.CreateSegement.Feed;
using Services.DomainServices.CreateSegement.Fish;
using Services.DomainServices.CreateSegement.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationEntity.CreateSegement
{
   public class CreateSegementAppEntity
    {
        public FishSegmentDomainService fishSegmentDomainService { get; set; }
        
        public FeedSegmentDomainService feedSegmentDomainService { get; set; }

        public AreaSegmentDomainService areaSegmentDomainService { get; set; }

        public ProjectSegmentDomainService projectSegmentDomainService { get; set; }
    }
}

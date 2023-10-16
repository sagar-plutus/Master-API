using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.MessageQueuePayloads
{
    public class DesignationPayload
    {

        public Guid MessageId { get; set; }

        public String TenantId { get; set; }

        public String AuthKey { get; set; }

        //table properties
        public Int32 IdDesignation { get; set; }
       public Int32 CreatedBy { get; set; }
       public Int32 UpdatedBy { get; set; }
       public Int32 NoticePeriodInMonth { get; set; }
       public Int32 IsVisible { get; set; }
       public DateTime CreatedOn { get; set; }
       public DateTime UpdatedOn { get; set; }
       public String DesignationDesc { get; set; }
       public String Remark { get; set; }
       public Int32 DeactivatedBy { get; set; }
       public DateTime DeactivatedOn { get; set; }

    }
}

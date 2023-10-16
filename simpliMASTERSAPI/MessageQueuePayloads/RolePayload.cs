using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.MessageQueuePayloads
{
    public class RolePayload
    {

        public Guid MessageId { get; set; }
        public String TenantId { get; set; }
        public String AuthKey { get; set; }

        //table properties
        public Int32 IdRole { get; set; }
        public String RoleDesc { get; set; }
        public Int32 IsActive { get; set; }
        public Int32 IsSystem { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public Int32 EnableAreaAlloc { get; set; }
        public Int32 OrgStructureId { get; set; }
        public Int32 RoleTypeId { get; set; }
        public Int32 DeptId { get; set; }
        public Int32 DesignationId { get; set; }
        //removed properties
 
        //        public Int32 DeptId { get; set; 
    }


    }


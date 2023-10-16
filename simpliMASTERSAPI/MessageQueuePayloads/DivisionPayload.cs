using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rabbitMessaging;
namespace simpliMASTERSAPI.MessageQueuePayloads
{
    public class DivisionPayload
    {
        public Guid MessageId { get; set; }
        public String TenantId { get; set; }
        //table properties
        public Int32 IdDept { get; set; }
        public Int32 ParentDeptId { get; set; }
        public Int32 DeptTypeId { get; set; }
        public Int32 OrgUnitId { get; set; }
        public Int32 IsVisible { get; set; }
        public String DeptCode { get; set; }
        public String DeptDisplayName { get; set; }
        public String DeptDesc { get; set; }
        public String AuthKey { get; set; }
    }
}

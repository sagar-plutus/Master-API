using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.MessageQueuePayloads
{
    public class OrgStructurePayload
    {
        public Guid MessageId { get; set; }
        public String TenantId { get; set; }

        //table properties
        public Int32 IdOrgStructure { get; set; }
        public Int32 ParentOrgStructureId { get; set; }
        public Int32 DeptId { get; set; }
        public Int32 DesignationId{ get; set; }
    public String orgStructureDesc { get; set; }
        public Int32 CreatedBy { get; set; }
        public Int32 UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Int16 IsNewAdded { get; set; }
        public Int16 IsActive { get; set; }
        public Int32 LevelId { get; set; }


        //removed properties

        //Int32 deptTypeId;
        //String employeeName;
        //Int32 employeeId;
        //Int32 actualOrgStructureId;
        //String parentOrgDisplayId;
        //String tempOrgStructId;
        //Int16 isDept;
        //String designationName;
        //String departmentName;
        //String positionName;
        //Int32 reportingTypeId;
        //String reportingName;
        //String levelName;
        //Int16 isEmptyPosition;
        //Int16 isPosition;
        ////Int32 technicalParentId;
        //Int16 isAddDept;
        //Int32 roleId;
        //Int32 roleTypeId;
    }
}

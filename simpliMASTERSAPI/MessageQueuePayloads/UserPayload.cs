using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.MessageQueuePayloads
{
    public class UserPayload
    {

        public Guid MessageId { get; set; }
        public String TenantId { get; set; }
        public String AuthKey { get; set; }
        public Int32 IdUser { get; set; }
        public Int32 DesignationId { get; set; }

        public String DesignationName { get; set; }

        public String UserLogin { get; set; }
        public String UserPasswd { get; set; }
        public String UserDisplayName { get; set; }
        public Int32 isActive { get; set; }

        public Int32 ReportingToUserId { get; set; } 

        //personTO
        public Int32 Salutation { get; set; } 
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String MobileNo { get; set; }
        public String AltMobileNo { get; set; }
        public String PhoneNo { get; set; }
        public String PrimaryEmail { get; set; }
        public String AltEmail { get; set; }
        public Int32 UserTypeId { get; set; }

        public DateTime DateOfBirth { get; set; }
        public Int32 RoleId { get; set; }
        public String RoleDesc { get; set; }
        public String SalutationDesc { get; set; } 
        public String UserTypeDesc { get; set; } 
        public Int32 CreatedBy { get; set; } //
        public DateTime CreatedOn { get; set; } //
        public String ReportingToUserName { get; set; } //ReportingTo userId
        public Int32 DeptId { get; set; } 
        public String DeptName { get; set; } 
        public String RegisteredDeviceId { get; set; }
        public DateTime DeactivatedOn { get; set; }
        public Int32 DeactivatedBy { get; set; }
        public String ImeiNumber { get; set; }




    }
}

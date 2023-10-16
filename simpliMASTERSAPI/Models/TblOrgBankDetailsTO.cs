
using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblOrgBankDetailsTO
    {
        #region Declarations
        Int32 orgId;
        Int32 bankOrgId;
        String ifscCode;
        String nameOnCheque;
        Int32 isDefault;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 idOrgBankDtls;
        String remark;
        String bankName;
        Int32 idAccountType;
    
        DateTime activatedFromDate;
        Int32 currencyId;
        String swiftCode;
        String bgLimit;
        String customerId;
        String branchCode;
        String branchName;
        String accountTypeName;
        String currencyName;
        
        #endregion

        #region Constructor
        public TblOrgBankDetailsTO()
        {
        }
        #endregion


        #region GetSet

        public int OrgId { get => orgId; set => orgId = value; }
        public int BankOrgId { get => bankOrgId; set => bankOrgId = value; }
        public string NameOnCheque { get => nameOnCheque; set => nameOnCheque = value; }
        public int IsDefault { get => isDefault; set => isDefault = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public int UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public int IdOrgBankDtls { get => idOrgBankDtls; set => idOrgBankDtls = value; }
        public string Remark { get => remark; set => remark = value; }
        public string BankName { get => bankName; set => bankName = value; }
        public string IfscCode { get => ifscCode; set => ifscCode = value; }
        public string AccountNo { get; set; }
        public int IdAccountType { get => idAccountType; set => idAccountType = value; }
        public DateTime ActivatedFromDate { get => activatedFromDate; set => activatedFromDate = value; }
        public int CurrencyId { get => currencyId; set => currencyId = value; }
        public string SwiftCode { get => swiftCode; set => swiftCode = value; }
        public string BgLimit { get => bgLimit; set => bgLimit = value; }
        public string PrimaryAccNo { get; set ; }
        public string CustomerId { get => customerId; set => customerId = value; }
        public string BranchCode { get => branchCode; set => branchCode = value; }
        public string BranchName { get => branchName; set => branchName = value; }
        public string AccountTypeName { get => accountTypeName; set => accountTypeName = value; }
        public string CurrencyName { get => currencyName; set => currencyName = value; }






        #endregion


    }
}

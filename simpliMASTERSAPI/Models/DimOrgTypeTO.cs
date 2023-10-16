using System;

namespace ODLMWebAPI.Models
{
    public class DimOrgTypeTO
    {
        #region Declarations
        Int32 idOrgType;
        Int32 isSystem;
        Int32 isActive;
        Int32 createUserYn;
        Int32 defaultRoleId;
        Int32 isTransferToSAP;
        string exportRptTemplateName;
        public Int32 IsOwnerMandatory
        {
            get;
            set;
        }

        public string ExportRptTemplateName
        {
            get { return exportRptTemplateName; }
            set { exportRptTemplateName = value; }
        }

        public Int32 IsTransferToSAP
        {
            get { return isTransferToSAP; }
            set { isTransferToSAP = value; }
        }
        public Int32 IsBankMandatory
        {
            get;
            set;
        }

        public Int32 IsAddressMandatory
        {
            get;
            set;
        }
        String orgType;
        int isKycYn;
        int isKycMandatory;
        #endregion

        #region Constructor
        public DimOrgTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOrgType
        {
            get { return idOrgType; }
            set { idOrgType = value; }
        }
        public Int32 IsSystem
        {
            get { return isSystem; }
            set { isSystem = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 CreateUserYn
        {
            get { return createUserYn; }
            set { createUserYn = value; }
        }
        public Int32 DefaultRoleId
        {
            get { return defaultRoleId; }
            set { defaultRoleId = value; }
        }
        public String OrgType
        {
            get { return orgType; }
            set { orgType = value; }
        }

        public int IsKycYn { get => isKycYn; set => isKycYn = value; }
        public int IsKycMandatory { get => isKycMandatory; set => isKycMandatory = value; }
        public int IsSendAPKLink { get; set; }
        #endregion
    }
}

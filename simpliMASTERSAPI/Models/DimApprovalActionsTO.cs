using System;
using System.Collections.Generic;
using System.Text;

namespace simpliMASTERSAPI.TO
{
    public class DimApprovalActionsTO
    {
        #region Declarations
        Int32 idApprovalActions;
        Int32 approvalId;
        Int32 sysElementId;
        Int32 isActive;
        Int32 sequanceNo;
        String bootstrapIconName;
        String toottip;
        int actionVal;
        string actionQuery;
        string actionId;
        string actionHeading;
        Int32 dimApprovalActionsTypeId;
        string baseAPIUrlForAction;
        string apiMethodForAction;
        #endregion

        #region Constructor
        public DimApprovalActionsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdApprovalActions
        {
            get { return idApprovalActions; }
            set { idApprovalActions = value; }
        }
        public Int32 ApprovalId
        {
            get { return approvalId; }
            set { approvalId = value; }
        }

        public Int32 ActionVal
        {
            get { return actionVal; }
            set { actionVal = value; }
        }
        public Int32 SysElementId
        {
            get { return sysElementId; }
            set { sysElementId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 SequanceNo
        {
            get { return sequanceNo; }
            set { sequanceNo = value; }
        }
        public String BootstrapIconName
        {
            get { return bootstrapIconName; }
            set { bootstrapIconName = value; }
        }
        public String Toottip
        {
            get { return toottip; }
            set { toottip = value; }
        }

        public String ActionHeading
        {
            get { return actionHeading; }
            set { actionHeading = value; }
        }
        public String ActionQuery
        {
            get { return actionQuery; }
            set { actionQuery = value; }
        }
        public String ActionId
        {
            get { return actionId; }
            set { actionId = value; }
        }

        public int DimApprovalActionsTypeId { get => dimApprovalActionsTypeId; set => dimApprovalActionsTypeId = value; }
        public string BaseAPIUrlForAction { get => baseAPIUrlForAction; set => baseAPIUrlForAction = value; }
        public string ApiMethodForAction { get => apiMethodForAction; set => apiMethodForAction = value; }
        #endregion
    }
}

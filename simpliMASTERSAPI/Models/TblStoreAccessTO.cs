using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblStoreAccessTO
    {
        #region Declarations
        Int32 idStoreAccess;
        Int32 deptId;
        Int32 userId;
        Int32 warehouseId;
        Int32 createdBy;
        DateTime createdOn;
        String permission;

        String deptDisplayName;
        String locationDesc;
        String userDisplayName;
        String createdByUser;
        String parentDesc;
        String mappedTxnId;
        Int32 parentLocId;

        #endregion

        #region Constructor
        public TblStoreAccessTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStoreAccess
        {
            get { return idStoreAccess; }
            set { idStoreAccess = value; }
        }
        public Int32 DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 WarehouseId
        {
            get { return warehouseId; }
            set { warehouseId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String Permission
        {
            get { return permission; }
            set { permission = value; }
        }

        public string DeptDisplayName { get => deptDisplayName; set => deptDisplayName = value; }
        public string LocationDesc { get => locationDesc; set => locationDesc = value; }
        public string UserDisplayName { get => userDisplayName; set => userDisplayName = value; }
        public string CreatedByUser { get => createdByUser; set => createdByUser = value; }
        public string ParentDesc { get => parentDesc; set => parentDesc = value; }
        public string MappedTxnId { get => mappedTxnId; set => mappedTxnId = value; }
        public int ParentLocId { get => parentLocId; set => parentLocId = value; }
        #endregion
    }
}

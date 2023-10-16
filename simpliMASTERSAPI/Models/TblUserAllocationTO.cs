using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblUserAllocationTO
    {
        #region Declarations
        Int32 idUserAlloc;
        Int32 userId;
        Int32 refId;
        Int32 allocTypeId;
        Int32 isActive;
        Int32 updatedBy;
        Int32 createdBy;
        DateTime updatedOn;
        DateTime createdOn;
        String userDisplayName;
        #endregion

        #region Constructor
        public TblUserAllocationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUserAlloc
        {
            get { return idUserAlloc; }
            set { idUserAlloc = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 RefId
        {
            get { return refId; }
            set { refId = value; }
        }
        public Int32 AllocTypeId
        {
            get { return allocTypeId; }
            set { allocTypeId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String UserDisplayName
        {
            get { return userDisplayName; }
            set { userDisplayName = value; }
        }
        
        #endregion
    }
}

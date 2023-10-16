using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblUserExtTO
    {
        #region Declarations
        Int32 userId;
        Int32 personId;
        Int32 addressId;
        Int32 createdBy;
        DateTime createdOn;
        String comments;
        Int32 organizationId;
        String userDisplayName;
        Decimal advLimit;
        Int32 updatedBy;
        DateTime updatedOn;
        Int32 lagId;

        #endregion

        #region Constructor
        public TblUserExtTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }
        public Int32 AddressId
        {
            get { return addressId; }
            set { addressId = value; }
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
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public Int32 LagId
        {
            get { return lagId; }
            set { lagId = value; }
        }

        public string UserDisplayName { get => userDisplayName; set => userDisplayName = value; }
        public decimal AdvLimit { get => advLimit; set => advLimit = value; }
        public int UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }

        #endregion
    }
}

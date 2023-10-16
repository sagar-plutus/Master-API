using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblLocationTO
    {
        #region Declarations
        Int32 idLocation;
        Int32 parentLocId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String locationDesc;
        Int32 idAddress;
        String compartmentNo;
        String compartmentSize;
        Int32 mateHandlSystemId;
        String mateHandlSystem;
        String parentLocationDesc;
        String mappedTxnId;
        String parentMappedTxnId;
        Int32 stateId;
        Int32 countryId;
        String stateName;
        String countryName;
        Int32 organizationId;
        String orgAddress;
        String displayLocationDesc;
        #endregion

        #region Constructor
        public TblLocationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLocation
        {
            get { return idLocation; }
            set { idLocation = value; }
        }
        public Int32 ParentLocId
        {
            get { return parentLocId; }
            set { parentLocId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String LocationDesc
        {
            get { return locationDesc; }
            set { locationDesc = value; }
        }

        public String CompartmentNo
        {
            get { return compartmentNo; }
            set { compartmentNo = value; }
        }
        public String CompartmentSize
        {
            get { return compartmentSize; }
            set { compartmentSize = value; }
        }
        public Int32 MateHandlSystemId
        {
            get { return mateHandlSystemId; }
            set { mateHandlSystemId = value; }
        }

        public String MateHandlSystem
        {
            get { return mateHandlSystem; }
            set { mateHandlSystem = value; }
        }
        public String DisplayLocationDesc
        {
            get { return displayLocationDesc; }
            set { displayLocationDesc = value; }
        }
        
        public string ParentLocationDesc { get => parentLocationDesc; set => parentLocationDesc = value; }
        public string MappedTxnId { get => mappedTxnId; set => mappedTxnId = value; }
        public string ParentMappedTxnId { get => parentMappedTxnId; set => parentMappedTxnId = value; }
        public int StateId { get => stateId; set => stateId = value; }
        public int CountryId { get => countryId; set => countryId = value; }
        public string StateName { get => stateName; set => stateName = value; }
        public string CountryName { get => countryName; set => countryName = value; }
        public int OrganizationId { get => organizationId; set => organizationId = value; }
        public string OrgAddress { get => orgAddress; set => orgAddress = value; }
        public int IdAddress { get => idAddress; set => idAddress = value; }
        public int IsActive { get; set; }
        #endregion
    }
}
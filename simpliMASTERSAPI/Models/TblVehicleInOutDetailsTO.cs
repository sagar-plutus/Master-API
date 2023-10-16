using System;
using System.Collections.Generic;
using System.Text;

namespace simpliMASTERSAPI.TO
{
    public class TblVehicleInOutDetailsTO
    {
        #region Declarations
        Int32 idTblVehicleInOutDetails;
        Int32 moduleId;
        Int32 transactionTypeId;
        Int32 transactionStatusId;
        Int32 nextStatusId;
        Int32 partyId;
        Int32 transporterId;
        Int32 supervisorId;
        Int32 nextExpectedActionId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime transactionDate;
        DateTime transactionStatusDate;
        DateTime createdOn;
        DateTime updatedOn;
        String vehicleNo;
        String transactionNo;
        String transactionDisplayNo;
        String applicationRouting;
        String scheduleRefNo;
        String partyName;
        String transporterName;
        String supervisorName;
        String transactionStatusName;
        String nextStatusName;
        int isTareWeightTaken;
        #endregion

        #region Constructor
        public TblVehicleInOutDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTblVehicleInOutDetails
        {
            get { return idTblVehicleInOutDetails; }
            set { idTblVehicleInOutDetails = value; }
        }

        public Int32 IsTareWeightTaken
        {
            get { return isTareWeightTaken; }
            set { isTareWeightTaken = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public Int32 TransactionTypeId
        {
            get { return transactionTypeId; }
            set { transactionTypeId = value; }
        }
        public Int32 TransactionStatusId
        {
            get { return transactionStatusId; }
            set { transactionStatusId = value; }
        }
        public Int32 NextStatusId
        {
            get { return nextStatusId; }
            set { nextStatusId = value; }
        }
        public Int32 PartyId
        {
            get { return partyId; }
            set { partyId = value; }
        }
        public Int32 TransporterId
        {
            get { return transporterId; }
            set { transporterId = value; }
        }
        public Int32 SupervisorId
        {
            get { return supervisorId; }
            set { supervisorId = value; }
        }
        public Int32 NextExpectedActionId
        {
            get { return nextExpectedActionId; }
            set { nextExpectedActionId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        public DateTime TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }
        public DateTime TransactionStatusDate
        {
            get { return transactionStatusDate; }
            set { transactionStatusDate = value; }
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
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String TransactionNo
        {
            get { return transactionNo; }
            set { transactionNo = value; }
        }
        public String TransactionDisplayNo
        {
            get { return transactionDisplayNo; }
            set { transactionDisplayNo = value; }
        }
        public String ApplicationRouting
        {
            get { return applicationRouting; }
            set { applicationRouting = value; }
        }
        public String ScheduleRefNo
        {
            get { return scheduleRefNo; }
            set { scheduleRefNo = value; }
        }
        public String NextStatusName
        {
            get { return nextStatusName; }
            set { nextStatusName = value; }
        }
        public String TransactionStatusName
        {
            get { return transactionStatusName; }
            set { transactionStatusName = value; }
        }
        public String SupervisorName
        {
            get { return supervisorName; }
            set { supervisorName = value; }
        }
        public String TransporterName
        {
            get { return transporterName; }
            set { transporterName = value; }
        }
        public String PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }

        public int TechnicalInspectorId { get;  set; }
        public string TechnicalInspectorName { get; set; }
        #endregion
    }

    public class VehicleNumber
    {
        #region Declaration

        String stateCode;
        String districtCode;
        String uniqueLetters;
        Int32 vehicleNo;

        #endregion

        #region Get Set
        public string StateCode
        {
            get
            {
                return stateCode;
            }

            set
            {
                stateCode = value;
            }
        }

        public string DistrictCode
        {
            get
            {
                return districtCode;
            }

            set
            {
                districtCode = value;
            }
        }

        public string UniqueLetters
        {
            get
            {
                return uniqueLetters;
            }

            set
            {
                uniqueLetters = value;
            }
        }

        public int VehicleNo
        {
            get
            {
                return vehicleNo;
            }

            set
            {
                vehicleNo = value;
            }
        }

        #endregion
    }

    public class TblCommericalDocStatusHistoryTO
    {
        #region Declarations
        Int32 statusId;
        Int32 isComment;
        Int32 createdBy;
        DateTime statusDate;
        DateTime createdOn;
        Int64 idCommerDocStatusHistory;
        Int64 commercialDocumentId;
        String statusRemark;
        string createdByName;
        #endregion

        #region Constructor
        public TblCommericalDocStatusHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 IsComment
        {
            get { return isComment; }
            set { isComment = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Int64 IdCommerDocStatusHistory
        {
            get { return idCommerDocStatusHistory; }
            set { idCommerDocStatusHistory = value; }
        }
        public Int64 CommercialDocumentId
        {
            get { return commercialDocumentId; }
            set { commercialDocumentId = value; }
        }
        public String StatusRemark
        {
            get { return statusRemark; }
            set { statusRemark = value; }
        }
        public String CreatedByName
        {
            get { return createdByName; }
            set { createdByName = value; }
        }
        #endregion
    }
}

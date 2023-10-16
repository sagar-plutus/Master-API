using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class TblVehicleInOutTONew
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
        Int32 technicalInspectorId;
        string technicalInspectorName;
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
        #endregion

        #region Constructor
       
        #endregion

        #region GetSet
        public Int32 IdTblVehicleInOutDetails
        {
            get { return idTblVehicleInOutDetails; }
            set { idTblVehicleInOutDetails = value; }
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

        public Int32 TechnicalInspectorId
        {
            get { return technicalInspectorId; }
            set { technicalInspectorId = value; }
        }

        public string TechnicalInspectorName
        {
            get { return technicalInspectorName; }
            set { technicalInspectorName = value; }
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
        #endregion
    }
}

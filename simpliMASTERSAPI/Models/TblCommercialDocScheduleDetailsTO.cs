using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblCommercialDocScheduleDetailsTO
    {
        #region Declarations
        Int32 idCommercialDocScheduleDetails;
        Int64 commercialDocScheduleId;
        Int32 productItemId;
        Double txnQty;
        Double scheduledQty;
        Int64 commecialDocumentId;
        Int64 commericalDocItemDtlsId;
        double pendingScheduleQty;
        double balQty;

        // Parent fields
        Int32 transactionTypeId;
        Int32 transporterOrgId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 statusId;
        Int32 statusBy;
        DateTime scheduleDate;
        DateTime createdOn;
        DateTime updatedOn;
        DateTime statusDate;
        String vehicleNo;
        String transporterName;
        int isActive;
        int isActiveItem;
        #endregion

        #region Constructor
        public TblCommercialDocScheduleDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdCommercialDocScheduleDetails
        {
            get { return idCommercialDocScheduleDetails; }
            set { idCommercialDocScheduleDetails = value; }
        }
        public Int64 CommercialDocScheduleId
        {
            get { return commercialDocScheduleId; }
            set { commercialDocScheduleId = value; }
        }
        public Int32 ProductItemId
        {
            get { return productItemId; }
            set { productItemId = value; }
        }

        public Int32 IsActiveItem
        {
            get { return isActiveItem; }
            set { isActiveItem = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Double TxnQty
        {
            get { return txnQty; }
            set { txnQty = value; }
        }
        public Double BalQty
        {
            get { return balQty; }
            set { balQty = value; }
        }
        public Double ScheduledQty
        {
            get { return scheduledQty; }
            set { scheduledQty = value; }
        }


        public Double PendingScheduleQty
        {
            get { return pendingScheduleQty; }
            set { pendingScheduleQty = value; }
        }
        
        public Int64 CommecialDocumentId
        {
            get { return commecialDocumentId; }
            set { commecialDocumentId = value; }
        }
        public Int64 CommericalDocItemDtlsId
        {
            get { return commericalDocItemDtlsId; }
            set { commericalDocItemDtlsId = value; }
        }

        public Int32 TransactionTypeId
        {
            get { return transactionTypeId; }
            set { transactionTypeId = value; }
        }
        public Int32 TransporterOrgId
        {
            get { return transporterOrgId; }
            set { transporterOrgId = value; }
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
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 StatusBy
        {
            get { return statusBy; }
            set { statusBy = value; }
        }
        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
            set { scheduleDate = value; }
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
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String TransporterName
        {
            get { return transporterName; }
            set { transporterName = value; }
        }
        #endregion
    }
}

using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.TO;
using System;
using System.Collections.Generic;
using System.Text;
using TO;

namespace ODLMWebAPI.Models
{
    public class TblPurchaseVehicleSpotEntryTO
    {
        #region Declarations
        Int32 idVehicleSpotEntry;
        Int32 supplierId;
        Int32 vehicleTypeId;
        Int32 statusId;
        Int32 createdBy;
        DateTime statusDate;
        DateTime createdOn;
        Double vehicleQtyMT;
        String location;
        String vehicleNo;
        String driverName;
        String transportorName;
        String remark;
        Int32 locationId;
        Double spotVehicleQty;
        String supplierName;
        String vehicleTypeDesc;

        Int32 purchaseEnquiryId;

        Int32 prodClassId;

        String driverContactNo;
        Int32 stateId;

        Int32 isLinkToExistingSauda;
        Int32 purchaseScheduleSummaryId;
        Int32 moduleId;
        Int64 transportOrgId;
        string prodClassType;
        string poNo;
        Int32 commercialScheduleid;

        List<TblSpotVehMatDtlsTO> spotVehMatDtlsTOList;
        TblPurchaseEnquiryTO bookingTO;

        List<TblRecycleDocumentTO> recycleDocumentsTOList=new List<TblRecycleDocumentTO>();
        List<TblPODetailsAgainstSpotEntryTO> tblPODetailsAgainstSpotEntryList = new List<TblPODetailsAgainstSpotEntryTO>();

        #endregion

        #region Constructor
        public TblPurchaseVehicleSpotEntryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVehicleSpotEntry
        {
            get { return idVehicleSpotEntry; }
            set { idVehicleSpotEntry = value; }
        }
        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }

        public Int32 CommercialScheduleid
        {
            get { return commercialScheduleid; }
            set { commercialScheduleid = value; }
        }

        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public Int64 TransportOrgId
        {
            get { return transportOrgId; }
            set { transportOrgId = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
        }

        public Int32 LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
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
        public Double VehicleQtyMT
        {
            get { return vehicleQtyMT; }
            set { vehicleQtyMT = value; }
        }
        public String Location
        {
            get { return location; }
            set { location = value; }
        }
        public String PoNo
        {
            get { return poNo; }
            set { poNo = value; }
        }

        public String TransportorName
        {
            get { return transportorName; }
            set { transportorName = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public String SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }

        public String VehicleTypeDesc
        {
            get { return vehicleTypeDesc; }
            set { vehicleTypeDesc = value; }
        }

        public Int32 PurchaseEnquiryId
        {
            get { return purchaseEnquiryId; }
            set { purchaseEnquiryId = value; }
        }


        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }
        public String DriverContactNo
        {
            get { return driverContactNo; }
            set { driverContactNo = value; }
        }

        public Int32 StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }

        public Double SpotVehicleQty
        {
            get { return spotVehicleQty; }
            set { spotVehicleQty = value; }
        }
        public List<TblSpotVehMatDtlsTO> SpotVehMatDtlsTOList
        {
            get { return spotVehMatDtlsTOList; }
            set { spotVehMatDtlsTOList = value; }

        }

        public string ProdClassType
        {
            get { return prodClassType; }
            set { prodClassType = value; }
        }

        public Int32 IsLinkToExistingSauda
        {
            get { return isLinkToExistingSauda; }
            set { isLinkToExistingSauda = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }

        public TblPurchaseEnquiryTO BookingTO
        {
            get { return bookingTO; }
            set { bookingTO = value; }
        }

          public List<TblRecycleDocumentTO> RecycleDocumentsTOList
        {
            get { return recycleDocumentsTOList; }
            set { recycleDocumentsTOList = value; }
        }
        public List<TblPODetailsAgainstSpotEntryTO> TblPODetailsAgainstSpotEntryList
        {
            get { return tblPODetailsAgainstSpotEntryList; }
            set { tblPODetailsAgainstSpotEntryList = value; }
        }

        #endregion
    }
}

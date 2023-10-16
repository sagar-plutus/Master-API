using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static ODLMWebAPI.StaticStuff.Constants;

namespace ODLMWebAPI.Models
{
    public class TblPurchaseEnquiryTO
    {
        #region Declarations
        //for history table
        Int32 idPurchaseEnquiryHistory;
        Int32 idPurchaseEnquiry;
        Int32 userId;
        Int32 cOrNCId;
        Int32 globalRatePurchaseId;
        Double bookingQty;
        Double bookingRate;
        Int32 supplierId;
        Int32 stateId;
        Int32 rateBandDeclarationPurchaseId;
        Int32 isConfirmed;
        Int32 prodClassId;
        Int32 statusId;
        Double calculatedMetalCost;
        Double baseMetalCost;
        Double padta;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        String comments;
        String vehicleNo;
        Int32 isConvertToSauda;
        String createdByName;
        String updatedByName;

        String supplierName;
        String purchaseManagerName;
        double rateForC;
        double rateForNC;
        String statusName;
        String prodClassDesc;
        Double rateBandCosting;
        Int32 vehicleSpotEntryId;

        Double pendingBookingQty;

        Int32 isSpotedVehicle;

        DateTime saudaCreatedOn;
        String saudaCreatedOnStr;
        List<TblPurchaseEnquiryDetailsTO> purchaseEnquiryDetailsTOList;
        List<TblPurchaseEnquiryScheduleTO> purchaseEnquiryScheduleTOLst;
        List<TblGradeWiseTargetQtyTO> gradeWiseTargetQtyTOList;

        TblPurchaseVehicleSpotEntryTO purchaseVehicleSpotEntryTO;
        List<TblPurchaseScheduleSummaryTO> bookingScheduleTOList = new List<TblPurchaseScheduleSummaryTO>();

        Double demandedRate;
        String authReasons;

        Int32 authReasonId;
        Int32 isOpenQtySauda;
        Int32 isUpdateGradeDtls;

        //Prajakta[2019-11-20] Added to get enquiry display no
        string enqDisplayNo;
        Int32 finYear;
        Int32 enqNo;

        //Priyanka [03-01-2019]
        Int32 idBooking;
        Int32 cnFOrgId;
        Int32 dealerOrgId;
        Int32 deliveryDays;
        Int32 noOfDeliveries;

        Int32 isJointDelivery;
        Int32 isSpecialRequirement;
        Double cdStructure;

        Int32 isWithinQuotaLimit;
        Int32 globalRateId;
        Int32 quotaDeclarationId;
        Int32 quotaQtyBforBooking;
        Int32 quotaQtyAftBooking;

        DateTime bookingDatetime;
        DateTime statusDate;


        String cnfName;
        String dealerName;

        List<TblBookingDelAddrTO> deliveryAddressLst;
        List<TblBookingExtTO> orderDetailsLst;
        String status;
        Double pendingQty;
        Double loadingQty;

        Int32 isDeleted;
        Int32 cdStructureId;
        Int32 parityId;
        Double orcAmt;
        String orcMeasure;
        String billingName;
        String poNo;
        String statusRemark;
        Int32 transporterScopeYn;

        double overdueAmt;

        Int32 brandId;
        String brandName;
        Double freightAmt;
        String pOFileBase64;
        double enquiryAmt;
        String projectName;
        Int32 statusBy;
        String bookingStatus;
        Double bookingpmrate;
        List<TblBookingScheduleTO> bookingScheduleTOLst;

        //Priyanka [07-01-2019]
        Int32 noOfVehicleSched;
        String remark;

        //Prajakta[2019-04-24] Added
        double freight;

        double wtRateApprovalDiff;
        Int32 isFixed;

        string saudaCloseRemark;
        double transportAmtPerMT;

        double consumedQty;
        double optionalPendingQty;

        double wtActualRate;
        #endregion

        #region Constructor
        public TblPurchaseEnquiryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseEnquiryHistory
        {
            get { return idPurchaseEnquiryHistory; }
            set { idPurchaseEnquiryHistory = value; }
        }

        public Int32 IsSpotedVehicle
        {
            get { return isSpotedVehicle; }
            set { isSpotedVehicle = value; }
        }

        public Int32 IdPurchaseEnquiry
        {
            get { return idPurchaseEnquiry; }
            set { idPurchaseEnquiry = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 COrNCId
        {
            get { return cOrNCId; }
            set { cOrNCId = value; }
        }
        public Int32 GlobalRatePurchaseId
        {
            get { return globalRatePurchaseId; }
            set { globalRatePurchaseId = value; }
        }
        public Double BookingQty
        {
            get { return bookingQty; }
            set { bookingQty = value; }
        }
        public Double RateForC
        {
            get { return rateForC; }
            set { rateForC = value; }
        }
        public Double RateForNC
        {
            get { return rateForNC; }
            set { rateForNC = value; }
        }
        public Double BookingRate
        {
            get { return bookingRate; }
            set { bookingRate = value; }
        }
        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }
        public Int32 StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }
        public Int32 RateBandDeclarationPurchaseId
        {
            get { return rateBandDeclarationPurchaseId; }
            set { rateBandDeclarationPurchaseId = value; }
        }
        public Int32 IsConfirmed
        {
            get { return isConfirmed; }
            set { isConfirmed = value; }
        }
        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Double CalculatedMetalCost
        {
            get { return calculatedMetalCost; }
            set { calculatedMetalCost = value; }
        }
        public Double BaseMetalCost
        {
            get { return baseMetalCost; }
            set { baseMetalCost = value; }
        }

        public Double RateBandCosting
        {
            get { return rateBandCosting; }
            set { rateBandCosting = value; }
        }
        public Double Padta
        {
            get { return padta; }
            set { padta = value; }
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
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public Int32 IsConvertToSauda
        {
            get { return isConvertToSauda; }
            set { isConvertToSauda = value; }
        }

        public String SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        public String PurchaseManagerName
        {
            get { return purchaseManagerName; }
            set { purchaseManagerName = value; }
        }
        public String StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
        public String ProdClassDesc
        {
            get { return prodClassDesc; }
            set { prodClassDesc = value; }
        }

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        public string SaudaCreatedOnStr
        {
            get { return saudaCreatedOn.ToString(Constants.DefaultDateFormat); }
        }
        public String UpdatedOnStr
        {
            get { return updatedOn.ToString(Constants.DefaultDateFormat); }
        }

        public List<TblPurchaseEnquiryDetailsTO> PurchaseEnquiryDetailsTOList { get => purchaseEnquiryDetailsTOList; set => purchaseEnquiryDetailsTOList = value; }

        public List<TblPurchaseEnquiryScheduleTO> PurchaseEnquiryScheduleTOLst
        {
            get { return purchaseEnquiryScheduleTOLst; }
            set { purchaseEnquiryScheduleTOLst = value; }
        }
        public List<TblGradeWiseTargetQtyTO> GradeWiseTargetQtyTOList
        {
            get { return gradeWiseTargetQtyTOList; }
            set { gradeWiseTargetQtyTOList = value; }
        }

        public List<TblPurchaseScheduleSummaryTO> BookingScheduleTOList
        {
            get { return bookingScheduleTOList; }
            set { bookingScheduleTOList = value; }
        }
        public Int32 VehicleSpotEntryId
        {
            get { return vehicleSpotEntryId; }
            set { vehicleSpotEntryId = value; }
        }

        public TblPurchaseVehicleSpotEntryTO PurchaseVehicleSpotEntryTO
        {
            get { return purchaseVehicleSpotEntryTO; }
            set { purchaseVehicleSpotEntryTO = value; }
        }

        public Double PendingBookingQty
        {
            get { return pendingBookingQty; }
            set { pendingBookingQty = value; }
        }
        public Int32 IsOpenQtySauda
        {
            get { return isOpenQtySauda; }
            set { isOpenQtySauda = value; }
        }

        public string EnqDisplayNo
        {
            get { return enqDisplayNo; }
            set { enqDisplayNo = value; }
        }

        public Int32 FinYear
        {
            get { return finYear; }
            set { finYear = value; }
        }

        public Int32 EnqNo
        {
            get { return enqNo; }
            set { enqNo = value; }
        }

        public DateTime SaudaCreatedOn
        {
            get { return saudaCreatedOn; }
            set { saudaCreatedOn = value; }
        }

        public Int32 AuthReasonId
        {
            get { return authReasonId; }
            set { authReasonId = value; }
        }

        public Int32 IsUpdateGradeDtls
        {
            get { return isUpdateGradeDtls; }
            set { isUpdateGradeDtls = value; }
        }

        public double Freight
        {
            get { return freight; }
            set { freight = value; }
        }

        public Int32 IsFixed
        {
            get { return isFixed; }
            set { isFixed = value; }
        }
        public double TransportAmtPerMT
        {
            get { return transportAmtPerMT; }
            set { transportAmtPerMT = value; }
        }


        public string SaudaCloseRemark
        {
            get { return saudaCloseRemark; }
            set { saudaCloseRemark = value; }
        }

        public double ConsumedQty
        {
            get { return consumedQty; }
            set { consumedQty = value; }
        }

        public double OptionalPendingQty
        {
            get { return optionalPendingQty; }
            set { optionalPendingQty = value; }
        }

        public double WtRateApprovalDiff
        {
            get { return wtRateApprovalDiff; }
            set { wtRateApprovalDiff = value; }
        }

        public double WtActualRate
        {
            get { return wtActualRate; }
            set { wtActualRate = value; }
        }



        /// <summary>
        /// Sanjay [03 Oct 2018] Demanded Price added. suggested From Rajuri Still-MetaRoll-YKP Email dated 25 Sept
        /// </summary>
        public double DemandedRate { get => demandedRate; set => demandedRate = value; }
        public string AuthReasons { get => authReasons; set => authReasons = value; }
        public int IdBooking { get => idBooking; set => idBooking = value; }
        public int CnFOrgId { get => cnFOrgId; set => cnFOrgId = value; }
        public int DealerOrgId { get => dealerOrgId; set => dealerOrgId = value; }
        public int DeliveryDays { get => deliveryDays; set => deliveryDays = value; }
        public int NoOfDeliveries { get => noOfDeliveries; set => noOfDeliveries = value; }
        public int IsJointDelivery { get => isJointDelivery; set => isJointDelivery = value; }
        public int IsSpecialRequirement { get => isSpecialRequirement; set => isSpecialRequirement = value; }
        public double CdStructure { get => cdStructure; set => cdStructure = value; }
        public int IsWithinQuotaLimit { get => isWithinQuotaLimit; set => isWithinQuotaLimit = value; }
        public int GlobalRateId { get => globalRateId; set => globalRateId = value; }
        public int QuotaDeclarationId { get => quotaDeclarationId; set => quotaDeclarationId = value; }
        public int QuotaQtyBforBooking { get => quotaQtyBforBooking; set => quotaQtyBforBooking = value; }
        public int QuotaQtyAftBooking { get => quotaQtyAftBooking; set => quotaQtyAftBooking = value; }
        public DateTime BookingDatetime { get => bookingDatetime; set => bookingDatetime = value; }
        public DateTime StatusDate { get => statusDate; set => statusDate = value; }
        public string CnfName { get => cnfName; set => cnfName = value; }
        public string DealerName { get => dealerName; set => dealerName = value; }
        public List<TblBookingDelAddrTO> DeliveryAddressLst { get => deliveryAddressLst; set => deliveryAddressLst = value; }
        public List<TblBookingExtTO> OrderDetailsLst { get => orderDetailsLst; set => orderDetailsLst = value; }
        public string Status { get => status; set => status = value; }
        public double PendingQty { get => pendingQty; set => pendingQty = value; }
        public double LoadingQty { get => loadingQty; set => loadingQty = value; }
        public int IsDeleted { get => isDeleted; set => isDeleted = value; }
        public int CdStructureId { get => cdStructureId; set => cdStructureId = value; }
        public int ParityId { get => parityId; set => parityId = value; }
        public double OrcAmt { get => orcAmt; set => orcAmt = value; }
        public string OrcMeasure { get => orcMeasure; set => orcMeasure = value; }
        public string BillingName { get => billingName; set => billingName = value; }
        public string PoNo { get => poNo; set => poNo = value; }
        public string StatusRemark { get => statusRemark; set => statusRemark = value; }
        public int TransporterScopeYn { get => transporterScopeYn; set => transporterScopeYn = value; }
        public double OverdueAmt { get => overdueAmt; set => overdueAmt = value; }
        public int BrandId { get => brandId; set => brandId = value; }
        public string BrandName { get => brandName; set => brandName = value; }
        public double FreightAmt { get => freightAmt; set => freightAmt = value; }
        public string POFileBase64 { get => pOFileBase64; set => pOFileBase64 = value; }
        public double EnquiryAmt { get => enquiryAmt; set => enquiryAmt = value; }
        public string ProjectName { get => projectName; set => projectName = value; }
        public int StatusBy { get => statusBy; set => statusBy = value; }
        public string BookingStatus { get => bookingStatus; set => bookingStatus = value; }
        public double Bookingpmrate { get => bookingpmrate; set => bookingpmrate = value; }
        public List<TblBookingScheduleTO> BookingScheduleTOLst { get => bookingScheduleTOLst; set => bookingScheduleTOLst = value; }
        public int NoOfVehicleSched { get => noOfVehicleSched; set => noOfVehicleSched = value; }
        public string Remark { get => remark; set => remark = value; }
        public string CreatedByName { get => createdByName; set => createdByName = value; }
        public string UpdatedByName { get => updatedByName; set => updatedByName = value; }
        #endregion

        #region Methods
        public TblPurchaseBookingBeyondQuotaTO GetBookingBeyondQuotaTO()
        {
            TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTO = new Models.TblPurchaseBookingBeyondQuotaTO();
            tblBookingBeyondQuotaTO.BookingId = this.idBooking;
            tblBookingBeyondQuotaTO.DeliveryPeriod = this.deliveryDays;
            tblBookingBeyondQuotaTO.Quantity = this.bookingQty;
            tblBookingBeyondQuotaTO.CdStructure = this.cdStructure;
            tblBookingBeyondQuotaTO.CdStructureId = this.cdStructureId;
            tblBookingBeyondQuotaTO.OrcAmt = this.orcAmt;
            tblBookingBeyondQuotaTO.Quantity = this.bookingQty;
            tblBookingBeyondQuotaTO.Rate = this.bookingRate;
            tblBookingBeyondQuotaTO.StatusId = this.statusId;
            tblBookingBeyondQuotaTO.StatusDate = this.statusDate;
            return tblBookingBeyondQuotaTO;
        }
        public TblPurchaseEnquiryHistoryTO GetEnquiryHistoryTO()
        {
            TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO = new Models.TblPurchaseEnquiryHistoryTO();
            tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiry = this.idPurchaseEnquiry;
            tblPurchaseEnquiryHistoryTO.GlobalRatePurchaseId = this.globalRatePurchaseId;
            tblPurchaseEnquiryHistoryTO.BookingQty = this.bookingQty;
            tblPurchaseEnquiryHistoryTO.BookingRate = this.bookingRate;
            tblPurchaseEnquiryHistoryTO.CreatedBy = this.createdBy;
            tblPurchaseEnquiryHistoryTO.CreatedOn = this.createdOn;
            tblPurchaseEnquiryHistoryTO.StatusId = this.statusId;
            tblPurchaseEnquiryHistoryTO.Comments = this.comments;

            return tblPurchaseEnquiryHistoryTO;
        }

        #endregion
    }
}

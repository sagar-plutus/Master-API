using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblProductAndRmConfigurationTO
    {
        #region Declarations
 
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 isActive;
        //Int32 codeTypeId;
       
        #endregion

        #region Constructor
        public TblProductAndRmConfigurationTO()
        {
        }

        #endregion

        #region GetSet
        public int IdProdItemRmToFgConfig { get; set; }
      //  public int CodeTypeId { get => codeTypeId; set => codeTypeId = value; }
        public int FgProductItemId { get; set; }
        public int ProdCatId { get; set; }
        public int ProdSpecId { get; set; }
        public int MaterialId { get; set; }
        public int FgUomId { get; set; }
        public int RmProductItemId { get; set; }
        public int RmUomId { get; set; }
        public decimal RmToFgConversionRatio { get; set; }

      //  p.itemName,p.itemDesc,pc.prodCateDesc,dp.prodSpecDesc,m.materialSubType,um.weightMeasurUnitDesc

        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string ProdCateDesc { get; set; }
        public string ProdSpecDesc { get; set; }
        public string MaterialSubType { get; set; }
        public string WeightMeasurUnitDesc { get; set; }
        public string WeightMeasurUnitDescForRM { get; set; }
        public string ItemNameRM { get; set; }
        public string ItemDescRM { get; set; }
        public List<TblRMProductTO> TblRMProductTOLst { get; set; }
        //
        public int IsActive { get => isActive; set => isActive = value; }
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

        public Int32 BrandId { get; set; }
        public Int32 IsFgToFgMapping { get; set; }
        

        #endregion

    }

    public class TblPurchaseRequestTo
    {

        public int IdPurchaseRequest { get; set; }
        public int DeptId { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int RequestTypeId { get; set; }
        public int RequisitionReasonId { get; set; }
        public DateTime ReqDate { get; set; }
        public int StatusId { get; set; }
        public int StatusUpdatedBy { get; set; }
        public DateTime StatusUpdatedOn { get; set; }
        public string OrderRef { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string UserDisplayName { get; set; }
        public string StatusName { get; set; }
        public string RequestTypeDesc { get; set; }
        public string RequisitionReasonDesc { get; set; }
        public string LocationDesc { get; set; }
        public string DeptDisplayName { get; set; }
        public int FinYearId { get; set; }
        public string PrEntityStr { get; set; }
        public int IsConsolidate { get; set; }
        public List<TblPurchaseItemTO> TblPurchaseItemTOLst { get; set; }
        public int ApprovalActionValue { get; set; }



        //  List<TblPurchaseItemTO> tblPurchaseItemTOLst;


        #region Constructor
        public TblPurchaseRequestTo()
        {
        }

        #endregion

        // #region GetSet

        //public List<TblPurchaseItemTO> TblPurchaseItemTOLst
        //{
        //    get;
        //    //{
        //    //    return bookingScheduleTOLst;
        //    //}

        //    set;
        //    //{
        //    //    bookingScheduleTOLst = value;
        //    //}
        //}

        // #endregion
    }
    public class TblPurchaseItemTO
    {
        public int IdPurchaseItem { get; set; }
        public int PurchaseRequestId { get; set; }
        public int ProdItemId { get; set; }
        public int ReqQty { get; set; }
        public int InStockQty { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string ItemName { get; set; }
        public string ConversionUnitOfMeasure { get; set; }
        public string WeightMeasurUnitDesc { get; set; }
        public string OrderItem { get; set; } // Aniket [14-5-2019]
    }
    public class TblRMProductTO
    {
        public int RmProductItemId { get; set; }
        public int RmToFgConversionRatio { get; set; }
        public string RMProductItemDesc { get; set; }
        public int RmUomId { get; set; }
    }
}

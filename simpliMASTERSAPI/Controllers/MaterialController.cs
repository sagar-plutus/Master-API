using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json;
using System.Net;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.BL.Interfaces;
using ODLMWebAPI.TO;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{

    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
        private readonly ITblMaterialBL _iTblMaterialBL;
        private readonly ITblProductInfoBL _iTblProductInfoBL;
        private readonly ITblParitySummaryBL _iTblParitySummaryBL;
        private readonly ITblParityDetailsBL _iTblParityDetailsBL;
        private readonly ITblProdClassificationBL _iTblProdClassificationBL;
        private readonly ITblProductItemBL _iTblProductItemBL;
        private readonly ITblItemDocumentsBL _iTblItemDocumentsBL;
        private readonly IDimensionBL _iDimensionBL;// working
        private readonly ICommon _iCommon;
        private readonly ITblProdItemMakeBrandExtBL _iTblProdItemMakeBrandExtBL;
        

        public MaterialController(ITblItemDocumentsBL iTblItemDocumentsBL, IDimensionBL iDimensionBL, ITblProductItemBL iTblProductItemBL, ITblProdClassificationBL iTblProdClassificationBL, ITblParityDetailsBL iTblParityDetailsBL, ICommon iCommon, ITblMaterialBL iTblMaterialBL, ITblProductInfoBL iTblProductInfoBL, ITblParitySummaryBL iTblParitySummaryBL
            , ITblProdItemMakeBrandExtBL iTblProdItemMakeBrandExtBL)
        {
            _iTblProdItemMakeBrandExtBL = iTblProdItemMakeBrandExtBL;
            _iTblItemDocumentsBL = iTblItemDocumentsBL;
            _iTblMaterialBL = iTblMaterialBL;
            _iTblProductInfoBL = iTblProductInfoBL;
            _iTblParitySummaryBL = iTblParitySummaryBL;
            _iTblParityDetailsBL = iTblParityDetailsBL;
            _iTblProdClassificationBL = iTblProdClassificationBL;
            _iTblProductItemBL = iTblProductItemBL;
            _iDimensionBL = iDimensionBL;
            _iCommon = iCommon;

        }

        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetGradeDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetGradeDropDownList(Int32 specificationId)
        {
            return _iTblProductItemBL.GetGradeDropDownList(specificationId);
        }

        [Route("GetMaterialDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetMaterialDropDownList()
        {
            return _iTblMaterialBL.SelectAllMaterialListForDropDown();
        }

        [Route("GetProductAndSpecsList")]
        [HttpGet]
        public List<TblProductInfoTO> GetProductAndSpecsList(Int32 prodCatId)
        {
            return _iTblProductInfoBL.SelectAllEmptyProductInfoList(prodCatId);

        }

        [Route("GetMaterialList")]
        [HttpGet]
        public List<TblMaterialTO> GetMaterialList()
        {
            return _iTblMaterialBL.SelectAllTblMaterialList();
        }

        /// <summary>
        /// Sanjay [2017-04-25] If parityId=0 then will return latest parity details if exist 
        /// and if parityId <>0 then parity details of given parityId
        /// </summary>
        /// <param name="parityId"></param>
        /// <param name="prodSpecId"> Added On 24/07/2017. After discussion with Nitin K [Meeting Ref. 21/07/2017 Pune] parity will be against prod Spec also.</param>
        /// <returns></returns>
        [Route("GetParityDetails")]
        [HttpGet]
        public TblParitySummaryTO GetParityDetails(Int32 stateId, Int32 prodSpecId = 0, Int32 brandId = 0)
        {
            TblParitySummaryTO latestParityTO = _iTblParitySummaryBL.SelectStatesActiveParitySummaryTO(stateId, brandId);
            int parityId = 0;
            if (latestParityTO == null)
            {
                latestParityTO = new TblParitySummaryTO();

            }
            else
            {
                parityId = latestParityTO.IdParity;
            }

            //Sanjay [2017-06-25] Changes as Statewsie latest all spec wise needs to show
            //List<TblParityDetailsTO> list = _iTblParityDetailsBL.SelectAllTblParityDetailsList(parityId, prodSpecId, stateId);
            List<TblParityDetailsTO> list = null;
            if (list == null || list.Count == 0)
            {
                list = _iTblParityDetailsBL.SelectAllEmptyParityDetailsList(prodSpecId, stateId, brandId);
                list = list.OrderBy(a => a.ProdCatId).ThenBy(a => a.MaterialId).ToList();
                latestParityTO.ParityDetailList = list;
            }
            else
                latestParityTO.ParityDetailList = list;
            return latestParityTO;
        }

        [Route("GetProductClassificationList")]
        [HttpGet]
        public List<TblProdClassificationTO> GetProductClassificationList(string prodClassType = "")
        {
            return _iTblProdClassificationBL.SelectAllTblProdClassificationList(prodClassType);
        }
        /// <summary>
        /// Priyanka [17-06-2019] : Modify existing product hierarchy as per requirement.
        /// </summary>
        /// <param name="prodClassType"></param>
        /// <returns></returns>
        [Route("GetProductClassificationListByParentId")]
        [HttpGet]
        public List<TblProdClassificationTO> GetProductClassificationListByParentId(Int32 prodClassId = 0, Int32 itemProdCatId = 0)
        {
            return _iTblProdClassificationBL.SelectAllTblProdClassificationListByParentId(prodClassId, itemProdCatId);
        }

        [Route("GetProdClassesForDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetProdClassesForDropDownList(Int32 parentClassId = 0)
        {
            return _iTblProdClassificationBL.SelectAllProdClassificationForDropDown(parentClassId);
        }

        //@ Hudekar Priyanka [04-march-2019]
        [Route("GetProductItemPurchaseData")]
        [HttpGet]
        public TblPurchaseItemMasterTO GetProductItemPurchaseData(Int32 prodItemId)
        {
            return _iTblProductItemBL.SelectTblPurchaseItemMasterTO(prodItemId);
        }
        /// <summary>
        /// Priyanka [02-07-2019] : Added to get the all supplier list against purchase item data.
        /// </summary>
        /// <param name="prodItemId"></param>
        /// <returns></returns>
        [Route("GetProductItemPurchaseDataAllList")]
        [HttpGet]
        public List<TblPurchaseItemMasterTO> GetProductItemPurchaseDataAllList(Int32 prodItemId)
        {
            return _iTblProductItemBL.SelectProductItemPurchaseDataAllList(prodItemId);
        }
        //Samadhan [13-05-2022]
        [Route("GetItemLinkedStoreLocList")]
        [HttpGet]
        public List<StoresLocationTO> GetItemLinkedStoreLocList(Int32 prodItemId)
        {
            return _iTblProductItemBL.SelectItemStoreLocList(prodItemId);
        }
        /// <summary>
        /// Deepali [22-04-2021] : Added to get the all make brand list against purchase item data.
        /// </summary>
        /// <param name="prodItemId"></param>
        /// <returns></returns>
        [Route("SelectAllTblProdItemMakeBrandExtByProdItem")]
        [HttpGet]
        public List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExtByProdItem(Int32 prodItemId)
        {
            return _iTblProdItemMakeBrandExtBL.SelectAllTblProdItemMakeBrandExtByProdItem(prodItemId);
        }

        [Route("GetProductClassificationDetails")]
        [HttpGet]
        public TblProdClassificationTO GetProductClassificationDetails(Int32 idProdClass)
        {
            return _iTblProdClassificationBL.SelectTblProdClassificationTO(idProdClass);
        }

        [Route("GetProductItemList")]
        [HttpGet]
        public List<TblProductItemTO> GetProductItemList(Int32 specificationId = 0)
        {
            return _iTblProductItemBL.SelectAllTblProductItemList(specificationId);
        }
        [Route("GetMakeItemList")]
        [HttpGet]
        public List<TblProductItemTO> GetMakeItemList(Int32 BOMTypeId, String bomStatusIdStr,Int32 idProdItem=0, Int32 parentProdItemId = 0)
        {
            return _iTblProductItemBL.GetMakeItemList(BOMTypeId, bomStatusIdStr, idProdItem,parentProdItemId);
        }
        //Reshma
        [Route("PostBOMItemList")]
        [HttpPost]
        public ResultMessage PostBOMItemList(int parentBomId)
        {
            return _iTblProductItemBL.SaveBOMItemList(parentBomId);
        }
        //Reshma
        [Route("GetBOMData")]
        [HttpGet]
        public List<TblProductItemBomTO> GetBOMData(Int32 parentItemId)
        {
            return _iTblProductItemBL.GetBOMData(parentItemId);
        }
        /// <summary>
        /// Priyanka Admane [14-June-2019] To Get List Of Items against given subgroup or base items
        /// </summary>
        /// <param name="specificationId"> If base item is not given then it will return either all base items or under given subgroup base items</param>
        /// <param name="baseItemId">if baseitem is >0 then it will return list of all make items for given base </param>
        /// <returns></returns>
        [Route("GetBaseOrMakeProductItemList")]
        [HttpGet]
        public List<TblProductItemTO> GetBaseProductItemList(Int32 prodClassId = 0, int baseItemId = 0,int NonListedType=1)
        {
            return _iTblProductItemBL.SelectAllProductItemListWrtSubGroupOrBaseItem(prodClassId, baseItemId, NonListedType);
        }


        [Route("GetProductItemDetails")]
        [HttpGet]
        public TblProductItemTO GetProductItemDetails(Int32 idProdItem)
        {
            return _iTblProductItemBL.SelectTblProductItemTO(idProdItem);
        }
        [Route("GetProductItemDetailsForPurchaseItem")]
        [HttpGet]
        public List<TblProductItemTO> GetProductItemDetailsForPurchaseItem(string idProdItemstr)
        {
            return _iTblProductItemBL.GetProductItemDetailsForPurchaseItem(idProdItemstr);
        }
        [Route("GetMaxPriorityItemSupplier")]
        [HttpGet]
        public List<DropDownTO> GetMaxPriorityItemSupplier(string idProdItemstr)
        {
            return _iTblProductItemBL.GetMaxPriorityItemSupplier(idProdItemstr);
        }

        /// <summary>
        /// GJ@20170818 : Get the Prouct Master Info List by LoadingSlipExt Ids for Bundles calculation
        /// </summary>
        /// <param name="strLoadingSlipExtIds"></param>
        /// <param name="strLoadingSlipExtIds">Added to know the Loading Slip Ext Ids</param>
        /// <returns></returns>
        [Route("GetProductSpecificationListByLoadingSlipExtIds")]
        [HttpGet]
        public List<TblProductInfoTO> GetProductSpecificationListByLoadingSlipExtIds(string strLoadingSlipExtIds)
        {
            return _iTblProductInfoBL.SelectProductInfoListByLoadingSlipExtIds(strLoadingSlipExtIds);
        }

        /// <summary>
        /// Saket [2017-01-17] Added.
        /// </summary>
        /// <returns></returns>
        [Route("GetAllProductSpecificationList")]
        [HttpGet]
        public List<TblProductInfoTO> GetAllProductSpecificationList()
        {
            return _iTblProductInfoBL.SelectAllTblProductInfoListLatest();
        }
        /// <summary>
        /// Vijaymala[12-09-2017] Added To Get Material Type List
        /// </summary>
        /// <returns></returns>
        [Route("GetMaterialTypeDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetMaterialTypeDropDownList()
        {
            return _iTblMaterialBL.SelectMaterialTypeDropDownList();
        }

        [Route("GetAllSupplierList")]
        [HttpGet]
        public List<TblPurchaseItemMasterTO> GetAllSupplierList(Int32 prodItemId, Int32 purchaseItemMasterId)
        {
            return _iTblProductItemBL.SelectAllTblPurchaseItemMasterTOList(prodItemId, purchaseItemMasterId);
        }

        /// <summary>
        /// Sanjay [2018-02-19] To Show all Item Prod Catg List
        /// </summary>
        /// <returns></returns>
        /// <remarks>Retrives All Item Product Categories for e.g. FG,Scrap,Service Items etc</remarks>
        [Route("GetItemProductCategoryList")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetItemProductCategoryList()
        {
            try
            {
                List<DropDownTO> list = _iDimensionBL.GetItemProductCategoryListForDropDown();
                if (list != null)
                {
                    if (list.Count == 0)
                        return NoContent();
                    return Ok(list);
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (System.Exception exc)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Sanjay [2018-02-19] To Retrive All the Product Classfication List By Item Product Category Enum
        /// </summary>
        /// <param name="itemProdCategoryE"></param>
        /// <returns></returns>
        [Route("GetProductClassListByItemCatg")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblProdClassificationTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetProductClassListByItemCatg(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            try
            {
                List<TblProdClassificationTO> list = _iTblProdClassificationBL.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);

                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (System.Exception exc)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("SelectMaterialListForDropDown")]
        [HttpGet]
        public List<DropDownTO> SelectMaterialListForDropDown(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            List<DropDownTO> list = _iTblProdClassificationBL.SelectMaterialListForDropDown(itemProdCategoryE);
            return list;
        }
        //Get GetDefaultProductClassification List
        [Route("GetDefaultProductClassification")]
        [HttpGet]
        public List<Int32> GetDefaultProductClassification()
        {
            return _iTblProdClassificationBL.GetDefaultProductClassification();
        }

        /// <summary>
        ///Sudhir[15-Mar-2018] Added for Get ProductItem List Based Product Classification Id.  
        /// </summary>
        /// <param name="idProdClass"></param>
        /// <returns></returns>
        [Route("GetProductItemByProdClass")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblProductItemTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetProductItemByProdClass(Int32 idProdClass)
        {
            try
            {
                List<TblProductItemTO> productItemList = _iTblProductItemBL.SelectProductItemList(idProdClass);
                if (productItemList != null)
                {
                    if (productItemList.Count > 0)
                        return Ok(productItemList);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(productItemList);
                }

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// Priyanka H [09/07/2019]Added for Get ProductItem List Based Product Classification Id Or subCategoryId Or prodItemId
        /// </summary>
        /// <param name="idProdClass"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        //[Route("GetProductItemByProdClass")]
        //[HttpGet]
        //[ProducesResponseType(typeof(List<TblProductItemTO>), 200)]
        //[ProducesResponseType(typeof(void), 500)]
        //[ProducesResponseType(typeof(EmptyResult), 204)]
        //[Produces("application/json")]
        //public IActionResult GetProductItemByProdClass(Int32 idProdClass, Int32 subCategoryId = 0, Int32 itemID = 0)
        //{
        //    try
        //    {
        //        List<TblProductItemTO> productItemList = _iTblProductItemBL.SelectProductItemList(idProdClass, subCategoryId, itemID);
        //        if (productItemList != null)
        //        {
        //            if (productItemList.Count > 0)
        //                return Ok(productItemList);
        //            else
        //                return NoContent();
        //        }
        //        else
        //        {
        //            return NotFound(productItemList);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError);
        //    }
        //}




        /// <summary>
        ///Sudhir[20-Mar-2018] Added for Get ParityDetails List
        /// </summary>
        /// <param name="idProdClass"></param>
        /// <returns></returns>
        //[Route("GetParityDetailsList")]
        //[HttpGet]
        //public List<TblParityDetailsTO> GetParityDetailsList(Int32 productItemId, Int32 brandId, Int32 prodCatId, Int32 prodSpecId, Int32 materialId)
        //{
        //    try
        //    {
        //        List<TblParityDetailsTO> list = _iTblParityDetailsBL.SelectAllParityDetailsOnProductItemId(productItemId, brandId, prodCatId, prodSpecId, materialId);
        //        if (list != null)
        //            return list;
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        /// <summary>
        /// Priyanka [29-08-18] : Added for Get ParityDetails List.  
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="productItemId"></param>
        /// <param name="prodCatId"></param>
        /// <param name="stateId"></param>
        /// <param name="currencyId"></param>
        /// <param name="productSpecInfoListTo"></param>
        /// <param name="districtId"></param>
        /// <param name="talukaId"></param>
        /// <returns></returns>
        [Route("GetParityDetailsList")]
        [HttpGet]
        public List<TblParityDetailsTO> GetParityDetailsList(Int32 brandId, Int32 productItemId, String prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo = 0, Int32 productSpecForRegular = 0, Int32 districtId = 0, Int32 talukaId = 0, string selectedStoresList = "")
        {
            try
            { 
                List<TblParityDetailsTO> list = _iTblParityDetailsBL.SelectAllParityDetailsOnProductItemId(brandId, productItemId, prodCatId, stateId, currencyId, productSpecInfoListTo, productSpecForRegular, districtId, talukaId,selectedStoresList);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // Aniket [21-Jan-2019] added to fetch ParityDetailsList against brand
        //[Route("GetParityDetailsListFromBrand")]
        //[HttpGet]
        //public List<TblParityDetailsTO> GetParityDetailsListFromBrand(Int32 fromBrand,Int32 toBrand,Int32 currencyId,Int32 categoryId, Int32 stateId)
        //{
        //    try
        //    {
        //        List<TblParityDetailsTO> list = BL.TblParityDetailsBL.SelectBrandForCopy(fromBrand, toBrand, currencyId, categoryId, stateId);
        //         if (list != null)
        //            return list;
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        ///Vijaymala[29-08-2018]added to get productr classification list by using item id
        /// </summary>

        /// <param name="productItemId">Added to know the Loading Slip Ext Ids</param>
        /// <returns></returns>
        [Route("GetProductClassificationListByProductItem")]
        [HttpGet]
        public List<TblProdClassificationTO> GetProductClassificationListByProductItem(Int32 productItemId)
        {
            return _iTblProdClassificationBL.SelectProductClassificationListByProductItemId(productItemId);
        }

        /// <summary>
        /// Yogesh added[24/05/2019] added to get product item List For DropDown
        /// </summary>
        /// <returns></returns>
        [Route("GetProductItemDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetProductItemDropDownList()
        {
            List<DropDownTO> userList = _iTblProductItemBL.GetProductItemDropDownList();
            return userList;
        }
        //@Priyanka H [15-03-2019] 
        [Route("GetSearchProductItem")] 
        [HttpGet]
        public List<TblProductItemTO> GetSearchProductItem(string itemName = "", Int32 itemNo = 0, Int32 categoryNo = 0, Int32 subCategoryId = 0, Int32 itemID = 0, string warehouseIds = "", Int32 isMakeOrBuyOrAll = 0, Int32 ProductTypeId = 0, Int32 isShowListed = 0, Int32 isShowMinLevelItem = 1)
        {
            // return BL.TblProductItemBL.SearchProductItemList(itemName, itemNo, categoryNo);
            return _iTblProductItemBL.SearchProductItemList(itemName, itemNo, categoryNo, subCategoryId, itemID,warehouseIds, isMakeOrBuyOrAll, ProductTypeId, isShowListed, isShowMinLevelItem);

        }
        //Reshma[12-03-21] Added For Stock view report
        [Route("GetSearchProductItemForStockView")]
        [HttpGet]
        public List<TblProductItemTO> GetSearchProductItemForStockView(string itemName = "", Int32 itemNo = 0, Int32 categoryNo = 0, Int32 subCategoryId = 0, Int32 itemID = 0, string warehouseIds = "", Int32 isMakeOrBuyOrAll = 0)
        {
            // return BL.TblProductItemBL.SearchProductItemList(itemName, itemNo, categoryNo);
            return _iTblProductItemBL.GetSearchProductItemForStockView(itemName, itemNo, categoryNo, subCategoryId, itemID, warehouseIds, isMakeOrBuyOrAll);

        }
        [Route("GetSearchProductGroupItem")]
        [HttpGet]
        public List<TblProductItemTO> GetSearchProductGroupItem(string searchString, Int32 ProductTypeId = 0)
        {
            // return BL.TblProductItemBL.SearchProductItemList(itemName, itemNo, categoryNo);
            return _iTblProductItemBL.SelectListOfProductItemTOOnSearchString(searchString, ProductTypeId);

        }
        [Route("GetDocumentTypeList")]
        [HttpGet]
        public List<DropDownTO> GetDocumentTypeList()
        {
            return _iTblItemDocumentsBL.GetDocumentTypeList();
        }

        [Route("SelectTblItemDocumentsByItemId")]
        [HttpGet]
        public List<TblItemDocumentsTO> SelectTblItemDocumentsByItemId(Int32 prodItemId ,Boolean  isShowImagesOnly=false)
        {
            return _iTblItemDocumentsBL.SelectTblItemDocumentsByItemId(prodItemId,isShowImagesOnly);
        }



        /// <summary>
        /// Harshala[11-09-2019] Added to get Parity History Details for each product to show when and by whom parity is changed
        /// </summary>
        [Route("GetParityHistoryDetails")]
        [HttpGet]
        public List<TblParityDetailsTO> GetParityHistoryDetails(Int32 brandId,Int32 materialId, Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 prodSpecId)
        {
            try
            {
                return _iTblParityDetailsBL.SelectAllParityDetailsHistory(brandId,materialId, productItemId, prodCatId, stateId, currencyId, prodSpecId);
             
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GetTblParityDetails")]
        public TblParityDetailsTO GetTblParityDetails(Int32 ProdCatId, Int32 ProdSpecId, Int32 MaterialId, Int32 BrandId, Int32 StateId, Int32 ProdItemId)
        {
            TblParityDetailsTO tblParityTO = new TblParityDetailsTO();
            tblParityTO.ProdCatId = ProdCatId;
            tblParityTO.ProdSpecId = ProdSpecId;
            tblParityTO.MaterialId = MaterialId;
            tblParityTO.BrandId = BrandId;
            tblParityTO.StateId = StateId;
            tblParityTO.ProdItemId = ProdItemId;
            TblParityDetailsTO tblParityDetailsTO = _iTblParityDetailsBL.GetTblParityDetails(tblParityTO);
            if (tblParityDetailsTO != null)
                return tblParityDetailsTO;
            else
                return tblParityTO;
        }

        //Dhananjay [2021-07-28] added for check item is properly configured
        [HttpGet]
        [Route("IsItemProperlyConfigured")]
        public ResultMessage IsItemProperlyConfigured(Int32 idProdItem)
        {
            return _iTblProductItemBL.IsItemProperlyConfigured(idProdItem);
        }


        #endregion

        #region Post

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }



        [Route("PostItemWiseDocuments")]
        [HttpPost]
        public ResultMessage PostItemWiseDocuments([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblItemDocumentsTO> TblItemDocumentsTOList = JsonConvert.DeserializeObject<List<TblItemDocumentsTO>>(data["TblItemDocumentsTOList"].ToString());
                
                if (TblItemDocumentsTOList == null || TblItemDocumentsTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : TblItemDocumentsTOList Found NULL";
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                for (int i = 0; i < TblItemDocumentsTOList.Count; i++)
                {
                    TblItemDocumentsTOList[i].CreatedOn = createdDate;
                }
                ResultMessage rMessage = new ResultMessage();
                rMessage.Result = _iTblItemDocumentsBL.InsertTblItemDocuments(TblItemDocumentsTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostProductInformation";
                return resultMessage;
            }
        }

        

     


        [Route("DeleteItemWiseDocuments")]
        [HttpPost]
        public ResultMessage DeleteItemWiseDocuments([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblItemDocumentsTO TblItemDocumentsTO = JsonConvert.DeserializeObject<TblItemDocumentsTO>(data["TblItemDocumentsTO"].ToString());

                if (TblItemDocumentsTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : TblItemDocumentsTO Found NULL";
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;

                TblItemDocumentsTO.UpdatedOn = createdDate;
                TblItemDocumentsTO.IsActive = 0;

                ResultMessage rMessage = new ResultMessage();
                rMessage.Result = _iTblItemDocumentsBL.UpdateTblItemDocuments(TblItemDocumentsTO);
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method DeleteItemWiseDocuments";
                return resultMessage;
            }
        }



        [Route("PostProductInformation")]
        [HttpPost]
        public ResultMessage PostProductInformation([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblProductInfoTO> productInfoTOList = JsonConvert.DeserializeObject<List<TblProductInfoTO>>(data["productInfoTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (productInfoTOList == null || productInfoTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : productInfoTOList Found NULL";
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                for (int i = 0; i < productInfoTOList.Count; i++)
                {
                    productInfoTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    productInfoTOList[i].CreatedOn = createdDate;
                }
                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblProductInfoBL.SaveProductInformation(productInfoTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostProductInformation";
                return resultMessage;
            }
        }


        /// <summary>
        /// Sanjay [2017-04-21] To Save Material Sizewise Parity Details
        /// Will Deactivate all Prev Parity Details and Inserts New Parity Details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostParityDetails")]
        [HttpPost]
        public ResultMessage PostParityDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblParitySummaryTO paritySummaryTO = JsonConvert.DeserializeObject<TblParitySummaryTO>(data["parityTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : paritySummaryTO Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO.ParityDetailList == null || paritySummaryTO.ParityDetailList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : ParityDetailList Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO.StateId <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Selected State Not Found";
                    resultMessage.DisplayMessage = "Records could not be updated ";
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                paritySummaryTO.CreatedOn = createdDate;
                paritySummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                paritySummaryTO.IsActive = 1;

                return _iTblParitySummaryBL.SaveParitySettings(paritySummaryTO);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostParityDetails";
                return resultMessage;
            }
        }

        [Route("PostNewProductClassification")]
        [HttpPost]
        public ResultMessage PostNewProductClassification([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProdClassificationTO prodClassificationTO = JsonConvert.DeserializeObject<TblProdClassificationTO>(data["prodClassificationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (prodClassificationTO == null)
                {
                    resultMessage.DefaultBehaviour("API : prodClassificationTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                prodClassificationTO.CreatedOn = createdDate;
                prodClassificationTO.CreatedBy = Convert.ToInt32(loginUserId);
                prodClassificationTO.IsActive = 1;
              //  ResultMessage rMessage = new ResultMessage();
                resultMessage = _iTblProdClassificationBL.InsertProdClassification(prodClassificationTO);
                if(resultMessage.MessageType != ResultMessageE.Information)
                {
                    //resultMessage.DefaultBehaviour();

                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.Tag = prodClassificationTO.IdProdClass;
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewProductClassification");
                return resultMessage;
            }
        }

        [Route("PostUpdateProductClassification")]
        [HttpPost]
        public ResultMessage PostUpdateProductClassification([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProdClassificationTO prodClassificationTO = JsonConvert.DeserializeObject<TblProdClassificationTO>(data["prodClassificationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (prodClassificationTO == null)
                {
                    resultMessage.DefaultBehaviour("API : prodClassificationTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                prodClassificationTO.UpdatedOn = createdDate;
                prodClassificationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblProdClassificationBL.UpdateProdClassification(prodClassificationTO);
                if (rMessage.Result != -1)
                {
                    rMessage.DefaultSuccessBehaviour();
                    rMessage.Tag = prodClassificationTO;
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While UpdateTblProdClassification");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateProductClassification");
                return resultMessage;
            }
        }

        //[Route("PostNewProductItem")]
        //[HttpPost]
        //public ResultMessage PostNewProductItem([FromBody] JObject data)
        //{
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    try
        //    {

        //        TblProductItemTO productItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["productItemTO"].ToString());
        //        List<TblPurchaseItemMasterTO> purchaseItemMasterTOList = JsonConvert.DeserializeObject<List<TblPurchaseItemMasterTO>>(data["tblPurchaseItemMasterTO"].ToString());

        //        var loginUserId = data["loginUserId"].ToString();

        //        if (Convert.ToInt32(loginUserId) <= 0)
        //        {
        //            resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
        //            return resultMessage;
        //        }

        //        if (productItemTO == null)
        //        {
        //            resultMessage.DefaultBehaviour("API : productItemTO Found NULL");
        //            return resultMessage;
        //        }

        //        DateTime createdDate = _iCommon.ServerDateTime;
        //        productItemTO.CreatedOn = createdDate;
        //        productItemTO.CreatedBy = Convert.ToInt32(loginUserId);

        //       // purchaseItemMasterTO.UpdatedBy = Convert.ToInt32(loginUserId);
        //      //  purchaseItemMasterTO.UpdatedOn = _iCommon.ServerDateTime;
        //        productItemTO.IsActive = 1;

        //        ResultMessage rMessage = new ResultMessage();
        //        rMessage = _iTblProductItemBL.InsertTblProductItem(productItemTO, purchaseItemMasterTOList);
        //        //if (result == 1)
        //        //{
        //        //    rMessage.DefaultSuccessBehaviour();
        //        //}
        //        //else
        //        //{
        //        //    rMessage.DefaultBehaviour("Error While InsertTblProductItem");
        //        //}
        //        return rMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "PostNewProductItem");
        //        return resultMessage;
        //    }
        //}

        [Route("PostNewProductItemByName")]
        [HttpPost]
        public ResultMessage PostNewProductItemByName([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblProductItemTO productItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["productItemTO"].ToString());

                ResultMessage rMessage = new ResultMessage();
                DateTime createdDate = _iCommon.ServerDateTime;
                productItemTO.CreatedOn = createdDate;
                productItemTO.CreatedBy = Convert.ToInt32(1);
                productItemTO.IsActive = 1;

                rMessage = _iTblProductItemBL.InsertTblProductItemByName(productItemTO);
                //rMessage.Tag = productItemTO;  //productItemTO.IdProdItem;
                return rMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewProductItem");
                return resultMessage;
            }
        }



        /// <summary>
        /// Priyanka [05-07-2019] : Added to save new product item.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewProductItem")]
        [HttpPost]
        public ResultMessage PostNewProductItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProductItemTO productItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["productItemTO"].ToString());
                List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList = JsonConvert.DeserializeObject<List<TblPurchaseItemMasterTO>>(data["tblPurchaseItemMasterTOList"].ToString());
                List<TblProdItemMakeBrandExtTO> tblProdItemMakeBrandExtTOList = JsonConvert.DeserializeObject<List<TblProdItemMakeBrandExtTO>>(data["tblProdItemMakeBrandExtTOList"].ToString());
                
                //AmolG[2020-Dec-16] For Warehouse wise inventory
                List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOList = JsonConvert.DeserializeObject<List<WareHouseWiseItemDtlsTO>>(data["WareHouseDetailsList"].ToString());

                //Samadhan[2022-May-09] For StoresList
                //List<StoresLocationTO> StoresLocationTOList  = JsonConvert.DeserializeObject<List<StoresLocationTO>>(data["selectedStores"].ToString());
                List<StoresLocationTO> StoresLocationTOList = new List<StoresLocationTO>();
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (productItemTO == null)
                {
                    resultMessage.DefaultBehaviour("API : productItemTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                productItemTO.CreatedOn = createdDate;
                productItemTO.CreatedBy = Convert.ToInt32(loginUserId);
                productItemTO.IsActive = 1;

                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblProductItemBL.InsertTblProductItem(productItemTO, tblPurchaseItemMasterTOList, wareHouseWiseItemDtlsTOList, StoresLocationTOList, tblProdItemMakeBrandExtTOList);
                rMessage.Tag = productItemTO;  //productItemTO.IdProdItem;
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewProductItem");
                return resultMessage;
            }
        }
        [Route("PostUpdateProductItem")]
        [HttpPost]
        public ResultMessage PostUpdateProductItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProductItemTO productItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["productItemTO"].ToString());
                List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList = JsonConvert.DeserializeObject<List<TblPurchaseItemMasterTO>>(data["tblPurchaseItemMasterTOList"].ToString());
                //Deepali[2021-Apr-22] For multipleMakeBrand 

                List<TblProdItemMakeBrandExtTO> tblProdItemMakeBrandExtTOList = JsonConvert.DeserializeObject<List<TblProdItemMakeBrandExtTO>>(data["tblProdItemMakeBrandExtTOList"].ToString());
                //AmolG[2020-Dec-16] For Warehouse wise inventory
                List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOList = JsonConvert.DeserializeObject<List<WareHouseWiseItemDtlsTO>>(data["WareHouseDetailsList"].ToString());

                //Samadhan[2022-May-09] For StoresList 
                //List<StoresLocationTO> StoresLocationTOList  = JsonConvert.DeserializeObject<List<StoresLocationTO>>(data["selectedStores"].ToString());
                List<StoresLocationTO> StoresLocationTOList = new List<StoresLocationTO>();
                //TblPurchaseItemMasterTO purchaseItemMasterTOForSupplierPriority = JsonConvert.DeserializeObject<TblPurchaseItemMasterTO>(data["changePurchaseItemMasterIsSupplierPriority"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                productItemTO.IsActive = 1;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (productItemTO == null)
                {
                    resultMessage.DefaultBehaviour("API : productItemTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                productItemTO.UpdatedOn = createdDate;
                productItemTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblProductItemBL.UpdateTblProductItem(productItemTO, tblPurchaseItemMasterTOList, wareHouseWiseItemDtlsTOList, StoresLocationTOList,  tblProdItemMakeBrandExtTOList);
                rMessage.Tag = productItemTO; //productItemTO.IdProdItem;
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateProductItem");
                return resultMessage;
            }
        }
        //chetan[2020-10-03] added for copy past item fo scrap  CopyPastMakeItem
        [Route("CopyPastItemScrapItem")]
        [HttpGet]
        public ResultMessage CopyPastItemScrapItem(int productItemId,int userId)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblProductItemBL.CopyPastItemScrapItem(productItemId,userId);
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateProductItem");
                return resultMessage;
            }
        }

        [Route("CopyPastMakeItem")]
        [HttpGet]
        public ResultMessage CopyPastMakeItem(int prodItemId,long baseProdItem,int userId)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblProductItemBL.CopyPastMakeItem(prodItemId, baseProdItem,userId);
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateProductItem");
                return resultMessage;
            }
        }
        /// <summary>
        /// Vijaymala[12-09-2017] Added To save Material Size
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewMaterial")]
        [HttpPost]
        public ResultMessage PostNewMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null)
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }
                //tblMaterialTO.MateCompOrgId = 19;
                // tblMaterialTO.MateSubCompOrgId = 20;
                tblMaterialTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblMaterialTO.CreatedOn = _iCommon.ServerDateTime;
                tblMaterialTO.IsActive = 1;

                int result = _iTblMaterialBL.InsertTblMaterial(tblMaterialTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewMaterial");
                return resultMessage;
            }

        }

        [Route("PostNewTestCertificateOfMaterial")]
        [HttpPost]
        public ResultMessage PostNewTestCertificateOfMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null)
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }
                //tblMaterialTO.MateCompOrgId = 19;
                // tblMaterialTO.MateSubCompOrgId = 20;
                tblMaterialTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblMaterialTO.CreatedOn = _iCommon.ServerDateTime;
                tblMaterialTO.IsActive = 1;
                tblMaterialTO.MaterialId = tblMaterialTO.IdMaterial;
                int result = _iTblMaterialBL.InsertSizeTestingDtl(tblMaterialTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewMaterial");
                return resultMessage;
            }

        }
        /// <summary>
        /// Vijaymala[12-09-2017] Added To Update Material Size
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateMaterial")]
        [HttpPost]
        public ResultMessage PostUpdateMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null)
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO != null)
                {
                    tblMaterialTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblMaterialTO.UpdatedOn = _iCommon.ServerDateTime;

                    result = _iTblMaterialBL.UpdateTblMaterial(tblMaterialTO);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be updated");
                        return resultMessage;
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateMaterial");
                return resultMessage;
            }

        }


        /// <summary>
        /// Vijaymala[12-09-2017] Added To Update Material Size
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeactivateMaterial")]
        [HttpPost]
        public ResultMessage PostDeactivateMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null)
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO != null)
                {
                    tblMaterialTO.DeactivatedBy = Convert.ToInt32(loginUserId);
                    tblMaterialTO.DeactivatedOn = _iCommon.ServerDateTime;
                    tblMaterialTO.IsActive = 0;
                    result = _iTblMaterialBL.UpdateTblMaterial(tblMaterialTO);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                        return resultMessage;
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateMaterial");
                return resultMessage;
            }

        }


        /// <summary>
        ///  Priyanka [21-02-2018] : Added to Deactivate the Category from product classification list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostDeactivateCategory")]
        [HttpPost]
        public ResultMessage PostDeactivateCategory([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblProdClassificationTO tblprodClassificationTO = JsonConvert.DeserializeObject<TblProdClassificationTO>(data["prodClassificationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblprodClassificationTO == null)
                {
                    resultMessage.DefaultBehaviour("tblprodClassificationTO Found NULL");
                    return resultMessage;
                }

                if (tblprodClassificationTO != null)
                {
                    resultMessage = _iTblProdClassificationBL.DeactivateCategoryAndRespItem(tblprodClassificationTO.IdProdClass, Convert.ToInt32(loginUserId));
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateCategory");
                return resultMessage;
            }
        }

        /// <summary>
        ///  Priyanka [22-02-2018] : Added to Deactivate Item/Product in Product Classification.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostDeactivateItemOrProduct")]
        [HttpPost]
        public ResultMessage PostDeactivateItemOrProduct([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var IdProdClass = data["idProdClass"].ToString();
                var BaseProdItemId = data["baseProdItemId"].ToString();
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (IdProdClass == null)
                {
                    resultMessage.DefaultBehaviour("tblprodItemTO Found NULL");
                    return resultMessage;
                }

                resultMessage = _iTblProductItemBL.DeactivateBaseOrMakeItem(Convert.ToInt32(IdProdClass), Convert.ToInt32(BaseProdItemId), Convert.ToInt32(loginUserId));
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateItemOrProduct");
                return resultMessage;
            }
        }

        //Aniket [29-01-2019] added to copy parity values from one brand to multiple brands
        [Route("PostCopyParityValuesForMultiBrands")]
        [HttpPost]
        public ResultMessage PostCopyParityValuesForMultiBrands([FromBody] JObject data)
        {
            int brandId = JsonConvert.DeserializeObject<Int32>(data["brandId"].ToString());
            List<DropDownToForParity> selectedBrands = JsonConvert.DeserializeObject<List<DropDownToForParity>>(data["selectedBrands"].ToString());
            List<DropDownToForParity> selectedStates = JsonConvert.DeserializeObject<List<DropDownToForParity>>(data["selectedStates"].ToString());
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage = _iTblParityDetailsBL.GetParityDetialsForCopyBrand(brandId, selectedBrands, selectedStates);
            return resultMessage;

        }

        /// <summary>
        /// Sudhir[21-MARCH-2018] Added  for Insert Parity Details New Req. Data insert only in tblParityDetails.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("SaveParityDetailsOtherItem")]
        [HttpPost]
        public ResultMessage SaveParityDetailsOtherItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblParitySummaryTO paritySummaryTO = JsonConvert.DeserializeObject<TblParitySummaryTO>(data["parityTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : paritySummaryTO Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO.ParityDetailList == null || paritySummaryTO.ParityDetailList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : ParityDetailList Found NULL";
                    return resultMessage;
                }

                Int32 isForUpdate = (Convert.ToInt32(data["isForUpdate"].ToString()));

                //if (paritySummaryTO.StateId <= 0)
                //{
                //    resultMessage.DefaultBehaviour();
                //    resultMessage.Text = "API : Selected State Not Found";
                //    resultMessage.DisplayMessage = "Records could not be updated ";
                //    return resultMessage;
                //}

                DateTime createdDate = _iCommon.ServerDateTime;
                paritySummaryTO.CreatedOn = createdDate;
                paritySummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                paritySummaryTO.IsActive = 1;

                return _iTblParityDetailsBL.SaveParityDetailsOtherItem(paritySummaryTO, isForUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostParityDetails";
                return resultMessage;
            }
        }

        //@ Hudekar Priyanka [04-march-19]
        [Route("PostPurchaseItemMaster")]
        [HttpPost]
        public ResultMessage PostPurchaseItemMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblPurchaseItemMasterTO purchaseItemMasterTO = JsonConvert.DeserializeObject<TblPurchaseItemMasterTO>(data["generalItemMaster"].ToString());
                var loginUserId = data["loginId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (purchaseItemMasterTO == null)
                {
                    resultMessage.DefaultBehaviour("API :purchaseItemMasterTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                purchaseItemMasterTO.CreatedOn = createdDate;
                purchaseItemMasterTO.CreatedBy = Convert.ToInt32(loginUserId);

                purchaseItemMasterTO.UpdatedBy = Convert.ToInt32(loginUserId);
                purchaseItemMasterTO.UpdatedOn = _iCommon.ServerDateTime;

                purchaseItemMasterTO.IsActive = 1;
                ResultMessage rMessage = new ResultMessage();
                //int result = BL.TblProductItemBL.InsertPurchaseItemMaster(purchaseItemMasterTO);
                int result = _iTblProductItemBL.InsertTblPurchaseItemMaster(purchaseItemMasterTO);
                //  int result = iTblProductItemBL.InsertTblPurchaseItemMaster(purchaseItemMasterTO); //using interface
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While InsertPurchaseItemMasterTO");
                }

                return rMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostPurchaseItemMaster");
                return resultMessage;
            }
        }
        //@  Hudekar Priyanka [04-march-2019]
        [Route("PostUpdatePurchaseItemMaster")]
        [HttpPost]
        public ResultMessage PostUpdatePurchaseItemMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblPurchaseItemMasterTO purchaseItemMasterTO = JsonConvert.DeserializeObject<TblPurchaseItemMasterTO>(data["generalItemMaster"].ToString());
                var loginUserId = data["loginId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (purchaseItemMasterTO == null)
                {
                    resultMessage.DefaultBehaviour("API : generalItemMaster Found NULL");
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                purchaseItemMasterTO.UpdatedOn = createdDate;
                purchaseItemMasterTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProductItemBL.UpdateTblPurchaseItemMasterTO(purchaseItemMasterTO);
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While UpdateTblProductItem");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdatePurchaseItemMaster");
                return resultMessage;
            }
        }
        //PriyankaH [14-03-2019] 
        [Route("PostUpdateProductItemSequenceNo")]
        [HttpPost]
        public ResultMessage PostUpdateProductItemSequenceNo([FromBody] JObject data)
        {
            int res = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblProductItemTO> tblProductItemTOList = JsonConvert.DeserializeObject<List<TblProductItemTO>>(data["productItemTOList"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblProductItemTOList == null || tblProductItemTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblProductItemTOList Found NULL";
                    return resultMessage;
                }

                DateTime createdDate = _iCommon.ServerDateTime;
                for (int i = 0; i < tblProductItemTOList.Count; i++)
                {
                    // tblProductItemTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    //  tblProductItemTOList[i].CreatedOn = createdDate;
                    tblProductItemTOList[i].UpdatedOn = createdDate;
                    tblProductItemTOList[i].UpdatedBy = Convert.ToInt32(loginUserId);

                }
                //  ResultMessage rMessage = new ResultMessage();
                // resultMessage = BL.TblProductItemBL.UpdateProductItemSequenceNo(tblProductItemTOList);
                res = _iTblProductItemBL.UpdateProductItemSequenceNo(tblProductItemTOList);
                //resultMessage = _iTblProductItemBL.UpdateProductItemSequenceNo(tblProductItemTOList);
                if (res == 1)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While PostUpdateProductItemSequenceNo";
                    return resultMessage;

                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostProductItemSequenceNo";
                return resultMessage;
            }
        }

        ////PriyankaH [14-03-2019] 
        //[Route("PostUpdateIsPriorityOfPurchaseItem")]
        //[HttpPost]
        //public ResultMessage PostUpdateIsPriorityOfPurchaseItem([FromBody] JObject data)
        //{
        //    //int res = 0;
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    try
        //    {

        //        TblPurchaseItemMasterTO purchaseItemMasterTO = JsonConvert.DeserializeObject<TblPurchaseItemMasterTO>(data["purchaseItemMaster"].ToString());
        //        var loginUserId = data["loginUserId"].ToString();

        //        if (Convert.ToInt32(loginUserId) <= 0)
        //        {
        //            resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
        //            return resultMessage;
        //        }

        //        if (purchaseItemMasterTO == null)
        //        {
        //            resultMessage.DefaultBehaviour("API : purchaseItemMaster Found NULL");
        //            return resultMessage;
        //        }

        //        DateTime createdDate = _iCommon.ServerDateTime;
        //        purchaseItemMasterTO.UpdatedOn = createdDate;
        //        purchaseItemMasterTO.UpdatedBy = Convert.ToInt32(loginUserId);
        //        ResultMessage rMessage = new ResultMessage();
        //       // int result = _iTblProductItemBL.UpdateTblPurchaseItemMasterTO(purchaseItemMasterTO);
        //        int result =_iTblProductItemBL.UpdateIsPriorityOfPurchaseItem(purchaseItemMasterTO);
        //        if (result == 1)
        //        {
        //            rMessage.DefaultSuccessBehaviour();
        //        }
        //        else
        //        {
        //            rMessage.DefaultBehaviour("Error While UpdateTblProductItem");
        //        }
        //        return rMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "PostUpdatePurchaseItemMaster");
        //        return resultMessage;
        //    }
        //}
        /// <summary>
        /// Yogesh Acharya [01-10-2019] : Added to save new product item supplier.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewItemSupplier")]
        [HttpPost]
        public ResultMessage PostNewItemSupplier([FromBody] JArray data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList = JsonConvert.DeserializeObject<List<TblPurchaseItemMasterTO>>(data.ToString());
                return _iTblProductItemBL.PostNewItemSupplier(tblPurchaseItemMasterTOList);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewItemSupplier");
                return resultMessage;
            }
        }

        [Route("UpdateInvalidItems")]
        [HttpPost]
        public ResultMessage UpdateInvalidItems(Int32 itemProdCatId)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return _iTblProductItemBL.UpdateInvalidItems(itemProdCatId);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in AddConsolidation";
                return resultMessage;
            }
        }

        /// <summary>
        /// Aditee[30-09-2020] Added To save BOM of Item
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewProductItemBOM")]
        [HttpPost]
        public ResultMessage PostNewProductItemBOM([FromBody] List<TblProductItemBomTO> tblProductItemBomTOList)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                //List<TblProductItemBomTO> tblProductItemBomTOList = JsonConvert.DeserializeObject<List<TblProductItemBomTO>>(data.ToString());

                //var loginUserId = data["loginUserId"].ToString();

                //if (Convert.ToInt32(loginUserId) <= 0)
                //{
                //    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                //    return resultMessage;
                //}

                if (tblProductItemBomTOList == null)
                {
                    resultMessage.DefaultBehaviour("tblProductItemBomTO Found NULL");
                    return resultMessage;
                }
               
                
                return _iTblProductItemBL.InsertTblProductItemBom(tblProductItemBomTOList);
               
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewProductItemBOM");
                return resultMessage;
            }

        }

        
        [Route("UpdateProductItemBOM")]
        [HttpPost]
        public ResultMessage UpdateProductItemBOM([FromBody] List<TblProductItemBomTO> tblProductItemBomTOList)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                if (tblProductItemBomTOList == null)
                {
                    resultMessage.DefaultBehaviour("tblProductItemBomTO Found NULL");
                    return resultMessage;
                }
                return _iTblProductItemBL.UpdateTblProductItemBom(tblProductItemBomTOList);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProductItemBOM");
                return resultMessage;
            }

        }
        [Route("DeleteProductItemBOM")]
        [HttpPost]
        public ResultMessage DeleteProductItemBOM([FromBody] TblProductItemBomTO tblProductItemBomTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                if (tblProductItemBomTO == null)
                {
                    resultMessage.DefaultBehaviour("tblProductItemBomTO Found NULL");
                    return resultMessage;
                }
                return _iTblProductItemBL.DeleteTblProductItemBom(tblProductItemBomTO);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteProductItemBOM");
                return resultMessage;
            }

        }
        [Route("FinalizedProductItemBOM")]
        [HttpPost]
        public ResultMessage FinalizedProductItemBOM([FromBody] TblProductItemTO finalizeItemBomTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                if (finalizeItemBomTO == null)
                {
                    resultMessage.DefaultBehaviour("tblProductItemBomTO Found NULL");
                    return resultMessage;
                }
                return _iTblProductItemBL.FinalizedProductItemBOM(finalizeItemBomTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProductItemBOM");
                return resultMessage;
            }

        }

        [Route("RevisedProductItemBOM")]
        [HttpPost]
        public ResultMessage RevisedProductItemBOM([FromBody] TblProductItemTO tblProductItemTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                if (tblProductItemTO == null)
                {
                    resultMessage.DefaultBehaviour("tblProductItemBomTO Found NULL");
                    return resultMessage;
                }
                //return _iTblProductItemBL.RevisedProductItemBOM(tblProductItemTO);//[17-03-2021] Deepali For new revision functionality
                return _iTblProductItemBL.RevisedProductItemBOMV2(tblProductItemTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProductItemBOM");
                return resultMessage;
            }

        }


        [Route("PostDeactivateNonListedItems")]
        [HttpPost]
        public ResultMessage PostDeactivateNonListedItems()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {                
               return _iTblProductItemBL.PostDeactivateNonListedItems();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProductItemBOM");
                return resultMessage;
            }

        }


        [Route("PostInsertItemCategory")]
        [HttpPost]
        public ResultMessage PostInsertItemCategory([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                DimItemProdCategTO dimItemProdCategTO = new DimItemProdCategTO();

                dimItemProdCategTO = JsonConvert.DeserializeObject<DimItemProdCategTO>(data["dimItemProdCategTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (dimItemProdCategTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : dimItemProdCategTO Found NULL";
                    return resultMessage;
                }
                List<DimItemProdCategTO> tempList = _iDimensionBL.SelectDimItemProdCateg(dimItemProdCategTO);
                if (tempList != null && tempList.Count > 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Item Product Category already exists With same name";
                    return resultMessage;
                }

                int result = _iDimensionBL.InsertDimItemProdCateg(dimItemProdCategTO);

                if(result> 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProductItemBOM");
                return resultMessage;
            }

        }


        [Route("PostUpdateItemCategory")]
        [HttpPost]
        public ResultMessage PostUpdateItemCategory([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                DimItemProdCategTO dimItemProdCategTO = new DimItemProdCategTO();

                dimItemProdCategTO = JsonConvert.DeserializeObject<DimItemProdCategTO>(data["dimItemProdCategTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (dimItemProdCategTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : dimItemProdCategTO Found NULL";
                    return resultMessage;
                }

                List<DimItemProdCategTO> tempList = _iDimensionBL.SelectDimItemProdCateg(dimItemProdCategTO);
                if(tempList != null && tempList.Count>1)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Item Product Category already exists With same name";
                    return resultMessage;
                }

                int result = _iDimensionBL.UpdateDimItemProdCateg(dimItemProdCategTO);

                if (result > 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProductItemBOM");
                return resultMessage;
            }

        }

        [Route("getCountOfListedAndNonListedItems")]
        [HttpGet]
        public List<DropDownTO> getCountOfListedAndNonListedItems()
        {
            return _iTblProductItemBL.getCountOfListedAndNonListedItems();            
        }


        [Route("getDimItemProdCategTO")]
        [HttpGet]
        public DimItemProdCategTO getDimItemProdCategTO(int idProdCat)
        {
            return _iDimensionBL.getDimItemProdCategTO(idProdCat);
        }

        [Route("SetMissingFieldsInSap")]
        [HttpGet]
        public ResultMessage SetMissingFieldsInSap()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblProductItemBL.SetMissingFieldsInSap();
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateProductItem");
                return resultMessage;
            }
        }


        //Deepali Added [02-06-2021] for task no 1109
        [Route("SelectAllItemListForExportToExcel")]
        [HttpGet]
        public ResultMessage SelectAllItemListForExportToExcel(int materialListedType = 1)
        {
            return _iTblProductItemBL.SelectAllItemListForExportToExcel(materialListedType);
        }

        #endregion

        #region Put

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion

        #region Delete

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion
    }
}
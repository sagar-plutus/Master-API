using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
 
namespace ODLMWebAPI.BL
{  
    public class TblWeighingMeasuresBL : ITblWeighingMeasuresBL
    { 
        private readonly ITblWeighingMeasuresDAO _iTblWeighingMeasuresDAO;

        private readonly ITblProductItemDAO _iTblProductItemDAO;
        private readonly ITblStockDetailsDAO _iTblStockDetailsDAO;

        private readonly ITblProductInfoDAO _iTblProductInfoDAO;

        private readonly IDimensionBL _iDimensionBL;

        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;

        private readonly ITblOrganizationDAO _iTblOrganizationDAO;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;

        private readonly IConnectionString _iConnectionString;
        public TblWeighingMeasuresBL( ITblAlertInstanceBL iTblAlertInstanceBL, ITblUserDAO iTblUserDAO, ITblOrganizationDAO iTblOrganizationDAO,  ITblConfigParamsDAO iTblConfigParamsDAO, IDimensionBL iDimensionBL, ITblProductInfoDAO iTblProductInfoDAO, ITblStockDetailsDAO iTblStockDetailsDAO, ITblProductItemDAO iTblProductItemDAO, IConnectionString iConnectionString, ITblWeighingMeasuresDAO iTblWeighingMeasuresDAO)
        {
            _iTblWeighingMeasuresDAO = iTblWeighingMeasuresDAO;
            _iTblProductItemDAO = iTblProductItemDAO;
            _iTblStockDetailsDAO = iTblStockDetailsDAO;
            _iTblProductInfoDAO = iTblProductInfoDAO;
            _iDimensionBL = iDimensionBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblOrganizationDAO = iTblOrganizationDAO;
            _iTblUserDAO = iTblUserDAO;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresList()
        {
            return _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasures();
        }
        
        public List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByTareWeight(DateTime fromDate, DateTime toDate)
        {
            return _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByTareWeight(fromDate, toDate);
        }
        
        //public List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId)
        //{
        //    List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
        //    tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(loadingId);
        //    if(tblWeighingMeasuresTOList.Count > 0)
        //    {
        //         tblWeighingMeasuresTOList.OrderByDescending(p=>p.CreatedOn);
        //    }
        //    return tblWeighingMeasuresTOList;
        //}

        //public List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId, SqlConnection conn, SqlTransaction tran)
        //{
        //    List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
        //    tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(loadingId, conn, tran);
        //    if (tblWeighingMeasuresTOList.Count > 0)
        //    {
        //       tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
        //    }
        //    return tblWeighingMeasuresTOList;
        //}
        /*GJ@20170829 : Get the All weighing Measurement by Loading Ids : */
        public List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(string loadingId, Boolean isUnloading)
        {
            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingIds(loadingId, isUnloading);
            if (tblWeighingMeasuresTOList!=null &&  tblWeighingMeasuresTOList.Count > 0)
            {
                //tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
                tblWeighingMeasuresTOList.OrderByDescending(p => p.IdWeightMeasure);
            }
            return tblWeighingMeasuresTOList;
        }
        /*GJ@20170828 : Get the All weighing Measurement by Vehicle No : */
        public List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByVehicleNo(string vehicleNo)
        {
            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByVehicleNo(vehicleNo);
            if (tblWeighingMeasuresTOList.Count > 0)
            {
                tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
            }
            return tblWeighingMeasuresTOList;
        }
        public TblWeighingMeasuresTO SelectTblWeighingMeasuresTO(Int32 idWeightMeasure)
        {
            return _iTblWeighingMeasuresDAO.SelectTblWeighingMeasures(idWeightMeasure);
        }

        

        #endregion
        
        #region Insertion
        public int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO)
        {
            return _iTblWeighingMeasuresDAO.InsertTblWeighingMeasures(tblWeighingMeasuresTO);
        }

        public int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingMeasuresDAO.InsertTblWeighingMeasures(tblWeighingMeasuresTO, conn, tran);
        }

        /*GJ@20170816 : Insert Weighing Machine Measurement : START*/
        public ResultMessage SaveNewWeighinMachineMeasurement(TblWeighingMeasuresTO tblWeighingMeasuresTO, List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList, List<TblUnLoadingItemDetTO>  tblUnLoadingItemDetTOList=null)
        {
            //SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            //SqlTransaction tran = null;
            //int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            //resultMessage.MessageType = ResultMessageE.None;
            //try
            //{
            //    conn.Open();
            //    tran = conn.BeginTransaction();
            //    if(tblWeighingMeasuresTO == null)
            //    {
            //        tran.Rollback();
            //        resultMessage.Text = "WeighinMachine Mesurement Found Null : SaveNewWeighinMachineMeasurement";
            //        resultMessage.MessageType = ResultMessageE.Error;
            //        resultMessage.Result = 0;
            //        return resultMessage;
            //    }
            //    #region 0. Check Tare taken againest machine Id
            //    if (!tblWeighingMeasuresTO.IsUpdateTareWt)//Vijaymala added [10-04-2018]
            //    {
            //        if (tblWeighingMeasuresTO.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT)
            //        {
            //            List<TblWeighingMeasuresTO> weighingMeasuresToList = new List<TblWeighingMeasuresTO>();
            //            // TblWeighingMeasuresTO tblWeighingMeasureTo = new TblWeighingMeasuresTO();
            //            weighingMeasuresToList = _iCircularDependencyBL.SelectAllTblWeighingMeasuresListByLoadingId(tblWeighingMeasuresTO.LoadingId, conn, tran);


            //            #region Send Notification

            //            //If tare weight send notification
            //            if (weighingMeasuresToList == null || weighingMeasuresToList.Count == 0)
            //            {
            //                resultMessage = SendNotificationOfVehicaleIn(tblWeighingMeasuresTO, conn, tran);
            //                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
            //                {
            //                    return resultMessage;
            //                }

            //            }

            //            #endregion

            //            if (weighingMeasuresToList.Count > 0)
            //            {
            //                var vRes = weighingMeasuresToList.Where(p => p.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT
            //                && p.WeighingMachineId == tblWeighingMeasuresTO.WeighingMachineId).FirstOrDefault();
            //                if(vRes != null)
            //                {
            //                    tran.Rollback();
            //                    resultMessage.DefaultBehaviour("Tare already occured this machine");
            //                    resultMessage.DisplayMessage = "Tare weight already taken againest this Machine.";
            //                    return resultMessage;
            //                }
            //            }
            //        }
            //        #endregion
            //        #region 1. Save the Weighing Machine Mesurement 

            //        result = _iTblWeighingMeasuresDAO.InsertTblWeighingMeasures(tblWeighingMeasuresTO, conn, tran);
            //        if (result < 0)
            //        {
            //            tran.Rollback();
            //            resultMessage.Text = "";
            //            resultMessage.MessageType = ResultMessageE.Error;
            //            resultMessage.Result = 0;
            //            return resultMessage;
            //        }
            //    }

            //    #endregion

            //    #region 2. Update the Loading Slip Ext Weighing Machine Measurement
            //    if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0)
            //    {
            //        foreach (var tblLoadingSlipExtTo in tblLoadingSlipExtTOList)
            //        {
            //            tblLoadingSlipExtTo.WeightMeasureId = tblWeighingMeasuresTO.IdWeightMeasure;
            //            tblLoadingSlipExtTo.UpdatedBy = tblWeighingMeasuresTO.CreatedBy;
            //            result = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTo, conn, tran);
            //            if (result < 0)
            //            {
            //                tran.Rollback();
            //                resultMessage.Text = "";
            //                resultMessage.MessageType = ResultMessageE.Error;
            //                resultMessage.Result = 0;
            //                return resultMessage;
            //            }
            //        }
            //    }
            //    #endregion

            //    // Vaibhav [30-Mar-2018] Added to update unloading weight. 
            //    #region 3. Update the UnLoading ItemDetails.
            //    if (tblUnLoadingItemDetTOList != null && tblUnLoadingItemDetTOList.Count > 0)
            //    {
            //        foreach (var tblUnLoadingItemDetTO in tblUnLoadingItemDetTOList)
            //        {
            //            tblUnLoadingItemDetTO.WeightMeasureId = tblWeighingMeasuresTO.IdWeightMeasure;
            //            tblUnLoadingItemDetTO.UpdatedBy = tblWeighingMeasuresTO.CreatedBy;
            //            tblUnLoadingItemDetTO.LoadedWeight = tblUnLoadingItemDetTO.UnLoadedWT;

            //            result = _iTblUnLoadingItemDetDAO.UpdateTblUnLoadingItemDet(tblUnLoadingItemDetTO, conn, tran);
            //            if (result < 0)
            //            {
            //                tran.Rollback();
            //                resultMessage.Text = "Error while updating unLoading item details.";
            //                resultMessage.MessageType = ResultMessageE.Error;
            //                resultMessage.Result = 0;
            //                return resultMessage;
            //            }

            //            if (tblUnLoadingItemDetTO.IsLastItem)
            //            {
            //                TblWeighingMeasuresTO tareWeightTO = new TblWeighingMeasuresTO();
            //                tareWeightTO = tblWeighingMeasuresTO;

            //                tareWeightTO.WeightMeasurTypeId = 1;
            //                tareWeightTO.WeightMT = tblUnLoadingItemDetTO.TareWt;

            //                result = _iTblWeighingMeasuresDAO.InsertTblWeighingMeasures(tblWeighingMeasuresTO, conn, tran);
            //                if (result < 0)
            //                {
            //                    tran.Rollback();
            //                    resultMessage.Text = "Error while InsertTblWeighingMeasures";
            //                    resultMessage.MessageType = ResultMessageE.Error;
            //                    resultMessage.Result = 0;
            //                    return resultMessage;
            //                }
            //            }

            //            if (tblUnLoadingItemDetTO.ProductId > 0 || tblUnLoadingItemDetTO.MaterialId > 0)
            //            {

            //                if (tblUnLoadingItemDetTO.ProductId > 0)
            //                {
            //                    TblProductItemTO tblProductItemTO = _iTblProductItemDAO.SelectTblProductItem(tblUnLoadingItemDetTO.ProductId, conn, tran);

            //                    if (tblUnLoadingItemDetTO.ProductId > 0 && tblProductItemTO != null && tblProductItemTO.IsStockRequire != 1)
            //                    {
            //                        continue;
            //                    }
            //                }

            //                // Stock-Up Functionality.
            //                List<TblStockDetailsTO> stockDetailsTOList = _iTblStockDetailsDAO.SelectTblStockDetailsList(tblUnLoadingItemDetTO.MaterialId, tblUnLoadingItemDetTO.ProductCatId, tblUnLoadingItemDetTO.ProductSpecId, tblUnLoadingItemDetTO.BrandId, tblUnLoadingItemDetTO.CompartmentId, tblUnLoadingItemDetTO.ProductId);
            //                TblStockDetailsTO tblStockDetailsTO = new TblStockDetailsTO();

            //                if (stockDetailsTOList != null && stockDetailsTOList.Count > 0)
            //                {
            //                    tblStockDetailsTO = stockDetailsTOList[0];
            //                }
            //                else
            //                {
            //                    // Set default compartment.
            //                    if (tblUnLoadingItemDetTO.CompartmentId == 0)
            //                    {
            //                        List<DropDownTO> compartmentList = _iDimensionBL.GetLocationWiseCompartmentList();
            //                        tblUnLoadingItemDetTO.CompartmentId = compartmentList[0].Value;
            //                    }                               

            //                    //Insert New Stock Details
            //                    tblStockDetailsTO = new TblStockDetailsTO();
            //                    tblStockDetailsTO.LocationId =  tblUnLoadingItemDetTO.CompartmentId;
            //                    tblStockDetailsTO.MaterialId = tblUnLoadingItemDetTO.MaterialId;
            //                    tblStockDetailsTO.ProdCatId = tblUnLoadingItemDetTO.ProductCatId;
            //                    tblStockDetailsTO.ProdSpecId = tblUnLoadingItemDetTO.ProductSpecId;
            //                    tblStockDetailsTO.ProdItemId = tblUnLoadingItemDetTO.ProductId;
            //                    tblStockDetailsTO.BrandId = tblUnLoadingItemDetTO.BrandId;

            //                    tblStockDetailsTO.CreatedBy = tblUnLoadingItemDetTO.CreatedBy;
            //                    tblStockDetailsTO.CreatedOn = tblUnLoadingItemDetTO.CreatedOn;
            //                    tblStockDetailsTO.BalanceStock = 0;
            //                    tblStockDetailsTO.TotalStock = 0;
            //                    tblStockDetailsTO.StockSummaryId = 1; // as discussed with saket set to 1; 

            //                    result = _iTblStockDetailsDAO.InsertTblStockDetails(tblStockDetailsTO, conn, tran);
            //                    if (result != 1)
            //                    {
            //                        resultMessage.DefaultBehaviour();
            //                        resultMessage.Text = "Error : While InsertTblStockDetails for stock details of unloadingslip";
            //                        return resultMessage;
            //                    }
            //                }

            //                // Insert Stock Consumption Record
            //                TblStockConsumptionTO stockConsumptionTO = new TblStockConsumptionTO();
            //                stockConsumptionTO.BeforeStockQty = tblStockDetailsTO.BalanceStock;
            //                stockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock + System.Math.Round((tblUnLoadingItemDetTO.UnLoadedWT / 1000), 2);
            //                stockConsumptionTO.CreatedBy = tblStockDetailsTO.CreatedBy;
            //                stockConsumptionTO.CreatedOn = tblStockDetailsTO.CreatedOn;
            //                stockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
            //                stockConsumptionTO.TxnQty = System.Math.Round((tblUnLoadingItemDetTO.UnLoadedWT / 1000), 2);
            //                stockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
            //                stockConsumptionTO.Remark = "Unloading Items Stock Up";

            //                stockConsumptionTO.TranId = tblUnLoadingItemDetTO.IdUnloadingItemDet;
            //                stockConsumptionTO.TranTypeId = (int)Constants.TransactionTypeE.UNLOADING;

            //                result = _iTblStockConsumptionDAO.InsertTblStockConsumption(stockConsumptionTO, conn, tran);
            //                if (result != 1)
            //                {
            //                    resultMessage.DefaultBehaviour();
            //                    resultMessage.Text = "Error : While InsertTblStockConsumption for stock details of unloadingslip";
            //                    return resultMessage;
            //                }

            //                tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + System.Math.Round((tblUnLoadingItemDetTO.UnLoadedWT / 1000), 2);
            //                tblStockDetailsTO.TotalStock = tblStockDetailsTO.TotalStock + System.Math.Round((tblUnLoadingItemDetTO.UnLoadedWT / 1000), 2);


            //                // Calculate MT to bundles.
            //                if (tblUnLoadingItemDetTO.MaterialId > 0 && tblUnLoadingItemDetTO.ProductSpecId > 0 && tblUnLoadingItemDetTO.ProductCatId > 0 && tblUnLoadingItemDetTO.BrandId > 0)
            //                {
            //                    List<TblProductInfoTO> productList = _iTblProductInfoDAO.SelectAllLatestProductInfo(conn, tran);

            //                    var productInfo = productList.Where(p => p.MaterialId == tblUnLoadingItemDetTO.MaterialId
            //                                                    && p.ProdCatId == tblUnLoadingItemDetTO.ProductCatId
            //                                                    && p.ProdSpecId == tblUnLoadingItemDetTO.ProductSpecId
            //                                                    && p.BrandId == tblUnLoadingItemDetTO.BrandId).OrderByDescending(d => d.CreatedOn).FirstOrDefault();                          
            //                    if (productInfo != null)
            //                    {
            //                        tblStockDetailsTO.NoOfBundles = (tblStockDetailsTO.TotalStock) / productInfo.AvgBundleWt;
            //                    }
            //                }

            //                tblStockDetailsTO.UpdatedBy = tblStockDetailsTO.CreatedBy;
            //                tblStockDetailsTO.UpdatedOn = tblStockDetailsTO.CreatedOn;

            //                result = _iTblStockDetailsDAO.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
            //                if (result != 1)
            //                {
            //                    resultMessage.DefaultBehaviour();
            //                    resultMessage.Text = "Error : While to UpdateTblStockDetails for update stock details of unloadingslip";
            //                    return resultMessage;
            //                }
            //            }
            //        }
            //    }

            //    #endregion


            //    //Sanjay [2017-09-17] Call For Auto Invoice
            //    if (tblWeighingMeasuresTO.IsLoadingCompleted == 1)
            //    {
            //        TblLoadingTO loadingTO = _iTblLoadingDAO.SelectTblLoading(tblWeighingMeasuresTO.LoadingId, conn, tran);
            //        if (loadingTO != null)
            //        {
            //            resultMessage = _iTblInvoiceBL.PrepareAndSaveNewTaxInvoice(loadingTO, conn, tran);
            //            if (resultMessage.MessageType!= ResultMessageE.Information)
            //            {
            //                tran.Rollback();
            //                return resultMessage;
            //            }
            //        }
            //        TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_DEFAULT_WEIGHING_SCALE, conn, tran);
            //        if (tblConfigParamsTO == null)
            //        {
            //            tran.Rollback();
            //            resultMessage.DefaultBehaviour("Default Weighing scale setting is not found.");
            //            return resultMessage;
            //        }
            //        //Sanjay [2017-10-08] On every final item weight auto gross for any count of weighin measurement
            //        //Discusse With Nitin K sir
            //        //if(Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) > 1)
            //        //{
            //            //TblWeighingMeasuresTO cloneTo = tblWeighingMeasuresTO.Clone()
            //            //tblWeighingMeasuresTO.WeightMeasurTypeId = (int)Constants.TransMeasureTypeE.GROSS_WEIGHT;
            //            //result = DAL._iTblWeighingMeasuresDAO.InsertTblWeighingMeasures(tblWeighingMeasuresTO, conn, tran);
            //            //if (result < 0)
            //            //{
            //            //    tran.Rollback();
            //            //    resultMessage.Text = "";
            //            //    resultMessage.MessageType = ResultMessageE.Error;
            //            //    resultMessage.Result = 0;
            //            //    return resultMessage;
            //            //}
            //        //}
                    
            //    }

            //    //GJ [2017-09-30] Call for To check Invoice Number Generated Againest Vehicle No
            //    if(tblWeighingMeasuresTO.IsCheckInvoiceGenerated == 1)
            //    {
            //        resultMessage = _iCircularDependencyBL.CheckInvoiceNoGeneratedByVehicleNo(tblWeighingMeasuresTO.VehicleNo, conn, tran);
            //        if (resultMessage.MessageType != ResultMessageE.Information)
            //        {
            //            tran.Rollback();
            //            return resultMessage;
            //        }
            //    }
            //    tran.Commit();
            //    resultMessage.MessageType = ResultMessageE.Information;
            //    resultMessage.Text = "Record Saved Sucessfully";
            //    resultMessage.Result = 1;
                return resultMessage;

            //}
            //catch (Exception ex)
            //{

            //    resultMessage.Text = "Exception Error While Record Save : SaveNewWeighinMachineMeasurement";
            //    resultMessage.MessageType = ResultMessageE.Error;
            //    resultMessage.Exception = ex;
            //    resultMessage.Result = -1;
            //    return resultMessage;
            //}
            //finally
            //{

            //}
        }

        public ResultMessage SendNotificationOfVehicaleIn(TblWeighingMeasuresTO tblWeighingMeasuresTO , SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resMessage = new StaticStuff.ResultMessage();
            //Int32 loadingId = tblWeighingMeasuresTO.LoadingId;
            //try
            //{

            //    TblLoadingTO tblLoadingTO = _iTblLoadingDAO.SelectTblLoading(loadingId, conn, tran);

            //    if (tblLoadingTO == null)
            //    {
            //        throw new Exception("tblLoadingTO == null for loadingId - " + loadingId);
            //    }

            //    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();

            //    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

            //    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_IN_FOR_DELIVERY;
            //    tblAlertInstanceTO.AlertAction = "VEHICLE_IN_FOR_DELIVERY";
            //    tblAlertInstanceTO.AlertComment = "Your Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  of Vehicle No " + tblLoadingTO.VehicleNo + " is Gate In";
            //    tblAlertInstanceTO.SourceDisplayId = "VEHICLE_IN_FOR_DELIVERY";
            //    tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

            //    //SMS to C & F

            //    List<TblLoadingSlipTO> list = _iTblLoadingSlipDAO.SelectAllTblLoadingSlip(tblLoadingTO.IdLoading, conn, tran);

            //    if (list != null && list.Count > 0)
            //    {
            //        List<TblLoadingSlipTO> listTemp = list.GroupBy(g => g.CnfOrgId).Select(s => s.FirstOrDefault()).ToList();

            //        if (listTemp != null && listTemp.Count > 0)
            //        {
            //            for (int k = 0; k < listTemp.Count; k++)
            //            {
            //                Dictionary<int, string> cnfDCT = _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(listTemp[k].CnfOrgId.ToString(), conn, tran);
            //                if (cnfDCT != null)
            //                {
            //                    foreach (var item in cnfDCT.Keys)
            //                    {
            //                        TblSmsTO smsTO = new TblSmsTO();
            //                        smsTO.MobileNo = cnfDCT[item];
            //                        smsTO.SourceTxnDesc = "VEHICLE_IN_FOR_DELIVERY";
            //                        smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
            //                        tblAlertInstanceTO.SmsTOList.Add(smsTO);
            //                    }
            //                }

            //                List<TblUserTO> cnfUserList = _iTblUserDAO.SelectAllTblUser(listTemp[k].CnfOrgId, conn, tran);
            //                if (cnfUserList != null && cnfUserList.Count > 0)
            //                {
            //                    for (int a = 0; a < cnfUserList.Count; a++)
            //                    {
            //                        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
            //                        tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
            //                        tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
            //                        tblAlertUsersTOList.Add(tblAlertUsersTO);
            //                    }
            //                }

            //            }
            //        }
            //    }
            //    //SMS to Dealer
            //    Dictionary<int, string> dealerDCT = _iTblLoadingSlipDAO.SelectRegMobileNoDCTForLoadingDealers(tblLoadingTO.IdLoading.ToString(), conn, tran);
            //    if (dealerDCT != null)
            //    {
            //        foreach (var item in dealerDCT.Keys)
            //        {
            //            TblSmsTO smsTO = new TblSmsTO();
            //            smsTO.MobileNo = dealerDCT[item];
            //            smsTO.SourceTxnDesc = "VEHICLE_IN_FOR_DELIVERY";
            //            smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
            //            tblAlertInstanceTO.SmsTOList.Add(smsTO);
            //        }
            //    }

                

            //    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

            //    tblAlertInstanceTO.EffectiveFromDate = tblWeighingMeasuresTO.CreatedOn;
            //    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
            //    tblAlertInstanceTO.IsActive = 1;
            //    tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
            //    tblAlertInstanceTO.RaisedBy = tblWeighingMeasuresTO.CreatedBy;
            //    tblAlertInstanceTO.RaisedOn = tblWeighingMeasuresTO.CreatedOn;
            //    tblAlertInstanceTO.IsAutoReset = 1;
            //    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
            //    if (rMessage.MessageType != ResultMessageE.Information)
            //    {
            //        //tran.Rollback();
            //        rMessage.MessageType = ResultMessageE.Error;
            //        rMessage.Text = "Error While SaveNewAlertInstance In Method SendNotificationOfVehicaleIn";
            //        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
            //        rMessage.Tag = tblAlertInstanceTO;
            //        return rMessage;
            //    }


            //    resMessage.DefaultSuccessBehaviour();
                return resMessage;
            //}
            //catch (Exception ex)
            //{
            //    resMessage.DefaultExceptionBehaviour(ex, "SendNotificationOfVehicaleIn");
            //    return resMessage;
            //}
        }


        /*GJ@20170930 : Prepare method to check Invoice No is generated or Not*/
        //public ResultMessage CheckInvoiceNoGeneratedByVehicleNo(string vehicleNo,  SqlConnection conn, SqlTransaction tran, Boolean isForOutOnly = false)
        //{
        //    ResultMessage resMessage = new StaticStuff.ResultMessage(); 
        //    try
        //    {
        //        List<TblLoadingTO> loadingList = new List<TblLoadingTO>();
        //        if (isForOutOnly)
        //            loadingList = _iTblLoadingDAO.SelectAllLoadingListByVehicleNoForDelOut(vehicleNo, conn, tran);
        //        else
        //            loadingList = _iTblLoadingDAO.SelectAllLoadingListByVehicleNo(vehicleNo, false, conn, tran);

        //        if (loadingList == null || loadingList.Count == 0)
        //        {
        //            resMessage.DefaultBehaviour("Loading To Found Null againest Vehicle No");
        //            return resMessage;
        //        }
        //        List<TblLoadingSlipTO> loadingSlipTOList = new List<TblLoadingSlipTO>();
        //        for (int i = 0; i < loadingList.Count; i++)
        //        {
        //            TblLoadingTO loadingEle = loadingList[i];
        //            List<TblLoadingSlipTO> loadingSlipTOListById = new List<TblLoadingSlipTO>();
        //            loadingSlipTOListById = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails(loadingEle.IdLoading, conn, tran);
        //            if(loadingSlipTOListById == null || loadingSlipTOListById.Count == 0)
        //            {
        //                resMessage.DefaultBehaviour("Loading Slip List Found Null againest Loading Id");
        //                return resMessage;
        //            }
        //            loadingSlipTOList.AddRange(loadingSlipTOListById);
        //        }

        //        if (loadingSlipTOList == null || loadingSlipTOList.Count == 0)
        //        {
        //            resMessage.DefaultBehaviour("Loading Slip List Found Null againest Vehicle No");
        //            return resMessage;
        //        }
        //        string resultStr = "Invoices are not authorized for selected Vehicle "+ vehicleNo + " Pending Loading slips are :  ";
        //        string LoadingSlipNos = string.Empty;
        //        for (int i = 0; i < loadingSlipTOList.Count; i++)
        //        {
        //            TblInvoiceTO invoiceTo = new TblInvoiceTO();
        //            invoiceTo = _iTblInvoiceBL.SelectInvoiceTOFromLoadingSlipId(loadingSlipTOList[i].IdLoadingSlip,conn,tran);
        //            if (invoiceTo == null || invoiceTo.StatusId != (int) Constants.InvoiceStatusE.AUTHORIZED )
        //            {                       
        //                    LoadingSlipNos = string.IsNullOrEmpty(LoadingSlipNos) ? loadingSlipTOList[i].LoadingSlipNo : LoadingSlipNos + "," + loadingSlipTOList[i].LoadingSlipNo;
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(LoadingSlipNos))
        //        {
        //            resMessage.MessageType = ResultMessageE.Error;
        //            resMessage.DisplayMessage = resultStr + LoadingSlipNos;
        //            resMessage.Text = resMessage.DisplayMessage;
        //            resMessage.Result = 0;
        //            return resMessage;
        //        }

        //        resMessage.DefaultSuccessBehaviour();
        //        return resMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        resMessage.DefaultExceptionBehaviour(ex, "CheckInvoiceNoGeneratedByVehicleNo");
        //        return resMessage;
        //    }
        //}

        /*GJ@20170816 : Insert Weighing Machine Measurement : END*/

        public ResultMessage UpdateLoadingSlipExtTo(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            //try
            //{
            //    conn.Open();
            //    tran = conn.BeginTransaction();
            //    if (tblLoadingSlipExtTO == null)
            //    {
            //        tran.Rollback();
            //        resultMessage.Text = "tblLoadingSlipExtTO Found Null : UpdateLoadingSlipExtTo";
            //        resultMessage.MessageType = ResultMessageE.Error;
            //        resultMessage.Result = 0;
            //        return resultMessage;
            //    }
            //    result = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO);
            //    if (result < 0)
            //    {
            //        tran.Rollback();
            //        resultMessage.Text = "";
            //        resultMessage.MessageType = ResultMessageE.Error;
            //        resultMessage.Result = 0;
            //        return resultMessage;
            //    }
            //    tran.Commit();
            //    resultMessage.MessageType = ResultMessageE.Information;
            //    resultMessage.Text = "Record Saved Sucessfully";
            //    resultMessage.Result = 1;
                return resultMessage;

            //}
            //catch (Exception ex)
            //{

            //    resultMessage.Text = "Exception Error While Record Save : UpdateLoadingSlipExtTo";
            //    resultMessage.MessageType = ResultMessageE.Error;
            //    resultMessage.Exception = ex;
            //    resultMessage.Result = -1;
            //    return resultMessage;
            //}
            //finally
            //{

            //}
        }

        #endregion

        #region Updation
        public int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO)
        {
            return _iTblWeighingMeasuresDAO.UpdateTblWeighingMeasures(tblWeighingMeasuresTO);
        }

        public int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingMeasuresDAO.UpdateTblWeighingMeasures(tblWeighingMeasuresTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblWeighingMeasures(Int32 idWeightMeasure)
        {
            return _iTblWeighingMeasuresDAO.DeleteTblWeighingMeasures(idWeightMeasure);
        }

        public int DeleteTblWeighingMeasures(Int32 idWeightMeasure, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingMeasuresDAO.DeleteTblWeighingMeasures(idWeightMeasure, conn, tran);
        }
        
        #endregion

    }
}

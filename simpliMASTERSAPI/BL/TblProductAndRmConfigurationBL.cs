using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

using System.Linq;
using ODLMWebAPI.StaticStuff;


namespace ODLMWebAPI.BL
{
    public class TblProductAndRmConfigurationBL: ITblProductAndRmConfigurationBL
   
    {
        private readonly ITblProductAndRmConfigurationDAO _iTblProductAndRmConfigurationDAO;
        private readonly IConnectionString _iConnectionString;
        public TblProductAndRmConfigurationBL(ITblProductAndRmConfigurationDAO iTblProductAndRmConfigurationDAO, IConnectionString iConnectionString)
        {

            _iTblProductAndRmConfigurationDAO = iTblProductAndRmConfigurationDAO;
            _iConnectionString = iConnectionString;

        }
        #region Selection

        public List<TblProductAndRmConfigurationTO> SelectAllTblProductAndRmConfigurationList()
        {
              return _iTblProductAndRmConfigurationDAO.SelectAllTblProductAndRmConfigurationList();

        }
        //public TblPurchaseRequestTo GetProductAndRMConfigurationById(int bookingId)
        //{
        //    List<TblBookingScheduleTO> list = _iCircularDependencyBL.SelectBookingScheduleByBookingId(bookingId);
        //    List<TblProductAndRmConfigurationTO> tblProductAndRmConfigurationTOList = _iTblProductAndRmConfigurationDAO.SelectAllTblProductAndRmConfigurationList();
        //    TblPurchaseRequestTo tblPurchaseRequestTo = new TblPurchaseRequestTo();
        //    tblPurchaseRequestTo.TblPurchaseItemTOLst = new List<TblPurchaseItemTO>();
        //    foreach (TblBookingScheduleTO tblBookingScheduleTo in list)
        //    {
        //        foreach (var item in tblBookingScheduleTo.OrderDetailsLst)
        //        {
        //            List<TblProductAndRmConfigurationTO> rmItem = tblProductAndRmConfigurationTOList.Where(w => w.FgProductItemId == item.ProdItemId).ToList();
        //            if (rmItem == null || rmItem.Count == 0)
        //            {
        //                rmItem = tblProductAndRmConfigurationTOList.Where(w => w.FgProductItemId == 0).ToList();

        //            }

        //            if (rmItem != null && rmItem.Count > 0)
        //            {
        //                for (int i = 0; i < rmItem.Count; i++)
        //                {
        //                    TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO = rmItem[i];
        //                    TblPurchaseItemTO tblPurchaseItemTO = new TblPurchaseItemTO();

        //                    tblPurchaseItemTO.ProdItemId = tblProductAndRmConfigurationTO.RmProductItemId;
        //                    Double perUnitQty = Convert.ToDouble(tblProductAndRmConfigurationTO.RmToFgConversionRatio);

        //                    Double totalQty = Math.Round(item.BookedQty * perUnitQty);

        //                    tblPurchaseItemTO.ReqQty = Convert.ToInt32(totalQty);
        //                    tblPurchaseItemTO.ItemName = rmItem[i].ItemDescRM;
        //                    tblPurchaseItemTO.OrderItem = rmItem[i].ItemName;
        //                    tblPurchaseRequestTo.TblPurchaseItemTOLst.Add(tblPurchaseItemTO);

        //                }
        //            }


        //        }
        //    }
        //    return tblPurchaseRequestTo;
        //}
        #endregion

        #region Insertion
        public ResultMessage InsertTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                
                #region Save

                if (tblProductAndRmConfigurationTO.TblRMProductTOLst != null && tblProductAndRmConfigurationTO.TblRMProductTOLst.Count > 0)
                {
                    for (int i = 0; i < tblProductAndRmConfigurationTO.TblRMProductTOLst.Count; i++)
                    {
                        if (tblProductAndRmConfigurationTO.TblRMProductTOLst[i].RmToFgConversionRatio != 0)
                        {
                            tblProductAndRmConfigurationTO.RmProductItemId = tblProductAndRmConfigurationTO.TblRMProductTOLst[i].RmProductItemId;
                            tblProductAndRmConfigurationTO.RmUomId = tblProductAndRmConfigurationTO.TblRMProductTOLst[i].RmUomId;
                            tblProductAndRmConfigurationTO.RmToFgConversionRatio = tblProductAndRmConfigurationTO.TblRMProductTOLst[i].RmToFgConversionRatio;
                            tblProductAndRmConfigurationTO.CreatedBy = tblProductAndRmConfigurationTO.CreatedBy;
                            tblProductAndRmConfigurationTO.CreatedOn = tblProductAndRmConfigurationTO.CreatedOn;

                            result = _iTblProductAndRmConfigurationDAO.InsertTblProductAndRmConfiguration(tblProductAndRmConfigurationTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While InsertTblPurchaseItem ");
                                return resultMessage;
                            }
                        }
                    }
                }

                #endregion

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewPurchaseRequest");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public int InsertTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductAndRmConfigurationDAO.InsertTblProductAndRmConfiguration(tblProductAndRmConfigurationTO, conn, tran);
        }
        #endregion

        #region Updation
        public ResultMessage UpdateTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO)
        {
            ResultMessage resultMessage =  new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (tblProductAndRmConfigurationTO.TblRMProductTOLst != null && tblProductAndRmConfigurationTO.TblRMProductTOLst.Count > 0)
                {
                    for (int i = 0; i < tblProductAndRmConfigurationTO.TblRMProductTOLst.Count; i++)
                    {
                        if (tblProductAndRmConfigurationTO.TblRMProductTOLst[i].RmProductItemId != 0 && tblProductAndRmConfigurationTO.TblRMProductTOLst[i].RmToFgConversionRatio != 0)
                        {
                            // TblRMProductTO tblRMProductTO = tblProductAndRmConfigurationTO.TblRMProductTOLst[i];
                            tblProductAndRmConfigurationTO.RmProductItemId = tblProductAndRmConfigurationTO.TblRMProductTOLst[i].RmProductItemId;
                            tblProductAndRmConfigurationTO.RmUomId = tblProductAndRmConfigurationTO.TblRMProductTOLst[i].RmUomId;
                            tblProductAndRmConfigurationTO.RmToFgConversionRatio = tblProductAndRmConfigurationTO.TblRMProductTOLst[i].RmToFgConversionRatio;  

                            result = _iTblProductAndRmConfigurationDAO.UpdateTblProductAndRmConfiguration(tblProductAndRmConfigurationTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While InsertTblPurchaseItem ");
                                return resultMessage;
                            }
                        }
                    }
                }
                if (tblProductAndRmConfigurationTO.RmProductItemId!=0 && tblProductAndRmConfigurationTO.IdProdItemRmToFgConfig!=0 && tblProductAndRmConfigurationTO.IsActive ==0)
                {
                    tblProductAndRmConfigurationTO.IsActive = 0;
                    result = _iTblProductAndRmConfigurationDAO.UpdateTblProductAndRmConfiguration(tblProductAndRmConfigurationTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While UpdateTblPurchaseItem ");
                        return resultMessage;
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostPurchaseRequestUpdate");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }


           // return _iTblProductAndRmConfigurationDAO.UpdateTblProductAndRmConfiguration(tblProductAndRmConfigurationTO);
        }

        public int UpdateTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductAndRmConfigurationDAO.UpdateTblProductAndRmConfiguration(tblProductAndRmConfigurationTO, conn, tran);
        }

        #endregion
    }
}

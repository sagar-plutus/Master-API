using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using System.Linq;
namespace ODLMWebAPI.BL
{
    public class TblReceiptLinkingBL: ITblReceiptLinkingBL
    {
        private readonly ITblReceiptLinkingDAO _iTblReceiptLinkingDAO;
        private readonly IConnectionString _iConnectionString;
        public TblReceiptLinkingBL(ITblReceiptLinkingDAO iTblReceiptLinkingDAO, IConnectionString iConnectionString)
        {
            _iTblReceiptLinkingDAO = iTblReceiptLinkingDAO;
            //_iTblBrsBankStatementDtlBL = iTblBrsBankStatementDtlBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public  DataTable SelectAllTblReceiptLinking()
        {
            return _iTblReceiptLinkingDAO.SelectAllTblReceiptLinking();
        }

        public List<TblReceiptLinkingTO> SelectAllTblReceiptLinkingList()
        {
            DataTable tblReceiptLinkingTODT = _iTblReceiptLinkingDAO.SelectAllTblReceiptLinking();
            return ConvertDTToList(tblReceiptLinkingTODT);
        }

        public  TblReceiptLinkingTO SelectTblReceiptLinkingTO(Int32 idReceiptLinking)
        {
            DataTable tblReceiptLinkingTODT = _iTblReceiptLinkingDAO.SelectTblReceiptLinking(idReceiptLinking);
            List<TblReceiptLinkingTO> tblReceiptLinkingTOList = ConvertDTToList(tblReceiptLinkingTODT);
            if(tblReceiptLinkingTOList != null && tblReceiptLinkingTOList.Count == 1)
                return tblReceiptLinkingTOList[0];
            else
                return null;
        }

        public  List<TblReceiptLinkingTO> ConvertDTToList(DataTable tblReceiptLinkingTODT )
        {
            List<TblReceiptLinkingTO> tblReceiptLinkingTOList = new List<TblReceiptLinkingTO>();
            if (tblReceiptLinkingTODT != null)
            {
                for (int rowCount = 0; rowCount < tblReceiptLinkingTODT.Rows.Count; rowCount++)
                {
                    TblReceiptLinkingTO tblReceiptLinkingTONew = new TblReceiptLinkingTO();
                    if(tblReceiptLinkingTODT.Rows[rowCount]["idReceiptLinking"] != DBNull.Value)
                        tblReceiptLinkingTONew.IdReceiptLinking = Convert.ToInt32( tblReceiptLinkingTODT.Rows[rowCount]["idReceiptLinking"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["brsBankStatementDtlId"] != DBNull.Value)
                        tblReceiptLinkingTONew.BrsBankStatementDtlId = Convert.ToInt32( tblReceiptLinkingTODT.Rows[rowCount]["brsBankStatementDtlId"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["bookingId"] != DBNull.Value)
                        tblReceiptLinkingTONew.BookingId = Convert.ToInt32( tblReceiptLinkingTODT.Rows[rowCount]["bookingId"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["paySchId"] != DBNull.Value)
                        tblReceiptLinkingTONew.PaySchId = Convert.ToInt32( tblReceiptLinkingTODT.Rows[rowCount]["paySchId"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["refReceiptLinkingId"] != DBNull.Value)
                        tblReceiptLinkingTONew.RefReceiptLinkingId = Convert.ToInt32( tblReceiptLinkingTODT.Rows[rowCount]["refReceiptLinkingId"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblReceiptLinkingTONew.CreatedBy = Convert.ToInt32( tblReceiptLinkingTODT.Rows[rowCount]["createdBy"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["returnBy"] != DBNull.Value)
                        tblReceiptLinkingTONew.ReturnBy = Convert.ToInt32( tblReceiptLinkingTODT.Rows[rowCount]["returnBy"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["lastSplitedBy"] != DBNull.Value)
                        tblReceiptLinkingTONew.LastSplitedBy = Convert.ToInt32( tblReceiptLinkingTODT.Rows[rowCount]["lastSplitedBy"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblReceiptLinkingTONew.CreatedOn = Convert.ToDateTime( tblReceiptLinkingTODT.Rows[rowCount]["createdOn"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["returnOn"] != DBNull.Value)
                        tblReceiptLinkingTONew.ReturnOn = Convert.ToDateTime( tblReceiptLinkingTODT.Rows[rowCount]["returnOn"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["lastSplitedOn"] != DBNull.Value)
                        tblReceiptLinkingTONew.LastSplitedOn = Convert.ToDateTime( tblReceiptLinkingTODT.Rows[rowCount]["lastSplitedOn"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["isSplited"] != DBNull.Value)
                        tblReceiptLinkingTONew.IsSplited = Convert.ToBoolean( tblReceiptLinkingTODT.Rows[rowCount]["isSplited"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["isReturn"] != DBNull.Value)
                        tblReceiptLinkingTONew.IsReturn = Convert.ToBoolean( tblReceiptLinkingTODT.Rows[rowCount]["isReturn"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        tblReceiptLinkingTONew.IsActive = Convert.ToBoolean( tblReceiptLinkingTODT.Rows[rowCount]["isActive"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["linkedAmt"] != DBNull.Value)
                        tblReceiptLinkingTONew.LinkedAmt = Convert.ToDouble( tblReceiptLinkingTODT.Rows[rowCount]["linkedAmt"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["actualLinkedAmt"] != DBNull.Value)
                        tblReceiptLinkingTONew.ActualLinkedAmt = Convert.ToDouble( tblReceiptLinkingTODT.Rows[rowCount]["actualLinkedAmt"].ToString());
                    if(tblReceiptLinkingTODT.Rows[rowCount]["splitedAmt"] != DBNull.Value)
                        tblReceiptLinkingTONew.SplitedAmt = Convert.ToDouble( tblReceiptLinkingTODT.Rows[rowCount]["splitedAmt"].ToString());
                    tblReceiptLinkingTOList.Add(tblReceiptLinkingTONew);
                }
            }
            return tblReceiptLinkingTOList;
        }



        #endregion
        
        #region Insertion
        public  int InsertTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO)
        {
            return _iTblReceiptLinkingDAO.InsertTblReceiptLinking(tblReceiptLinkingTO);
        }
        public ResultMessage InsertTblReceiptLinking(List<TblReceiptLinkingTO> tblReceiptLinkingTOList)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            ResultMessage resultMessage = new ResultMessage();
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                for (int i = 0; i < tblReceiptLinkingTOList.Count; i++)
                {
                    TblReceiptLinkingTO tblReceiptLinkingTO = tblReceiptLinkingTOList[i];
                    result = InsertTblReceiptLinking(tblReceiptLinkingTO, conn, tran);
                    if(result<1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error in insert receipt linking table";
                        return resultMessage;
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblReceiptLinking");
                return resultMessage;
            }
        }
        public  int InsertTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReceiptLinkingDAO.InsertTblReceiptLinking(tblReceiptLinkingTO, conn, tran);
        }
        
        #endregion

        #region Updation
        public  int UpdateTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO)
        {
            return _iTblReceiptLinkingDAO.UpdateTblReceiptLinking(tblReceiptLinkingTO);
        }

        public  int UpdateTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReceiptLinkingDAO.UpdateTblReceiptLinking(tblReceiptLinkingTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblReceiptLinking(Int32 idReceiptLinking)
        {
            return _iTblReceiptLinkingDAO.DeleteTblReceiptLinking(idReceiptLinking);
        }

        public  int DeleteTblReceiptLinking(Int32 idReceiptLinking, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReceiptLinkingDAO.DeleteTblReceiptLinking(idReceiptLinking, conn, tran);
        }

        #endregion

        #region Brs Bank Statement Dtl table actions 

        public ResultMessage UpdateReceiptStatementDtlStatus(TblBrsBankStatementDtlTO tblBrsBankStatementDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = _iTblReceiptLinkingDAO.UpdateReceiptStatementDtlStatus(tblBrsBankStatementDtlTO, conn, tran);
                if (result < 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Error in update Receipt Statement Dtl Status";
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateReceiptStatementDtlStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdateReceiptStatementDtlStatus(TblBrsBankStatementDtlTO tblBrsBankStatementDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReceiptLinkingDAO.UpdateReceiptStatementDtlStatus(tblBrsBankStatementDtlTO, conn, tran);
        }
        public List<TblBrsBankStatementDtlTO> SelectAllReceiptStatementDtl()
        {

            try
            {
                DataTable brsBankStatementDtlTODT = _iTblReceiptLinkingDAO.SelectAllReceiptStatementDtl();
                List<TblBrsBankStatementDtlTO> tblBrsBankStatementDtlTOList = ConvertBrsBankStatementDtlDTToList(brsBankStatementDtlTODT);
                List<TblBrsBankStatementDtlTO> tblBrsBankStatementDtlTOFinalList = new List<TblBrsBankStatementDtlTO>();
                if (tblBrsBankStatementDtlTOList != null && tblBrsBankStatementDtlTOList.Count > 0)
                {
                    List<TblBrsBankStatementDtlTO> tblBrsBankStatementDtlTOTempList = tblBrsBankStatementDtlTOList.Where(w => w.CheckForCredit == true && (!string.IsNullOrEmpty(w.BrsBankStatementRecordValue.Trim()))).ToList();
                    if (tblBrsBankStatementDtlTOTempList != null && tblBrsBankStatementDtlTOTempList.Count > 0)
                    {

                        for (int i = 0; i < tblBrsBankStatementDtlTOTempList.Count; i++)
                        {
                            TblBrsBankStatementDtlTO tblBrsBankStatementDtlTOTemp = tblBrsBankStatementDtlTOTempList[i];

                            List<TblBrsBankStatementDtlTO> tblBrsBankStatementWiseDtlTOList = tblBrsBankStatementDtlTOList.Where(w => w.BrsBankStatementId == tblBrsBankStatementDtlTOTemp.BrsBankStatementId && w.RowNO == tblBrsBankStatementDtlTOTemp.RowNO).ToList();
                            if (tblBrsBankStatementWiseDtlTOList != null && tblBrsBankStatementWiseDtlTOList.Count > 0)
                            {
                                TblBrsBankStatementDtlTO tblBrsBankStatementDtlTO = new TblBrsBankStatementDtlTO();
                                tblBrsBankStatementDtlTO.BankName = tblBrsBankStatementWiseDtlTOList[0].BankName;
                                tblBrsBankStatementDtlTO.ReceiptByName = tblBrsBankStatementWiseDtlTOList[0].ReceiptByName;
                                tblBrsBankStatementDtlTO.ReceiptTypeId = tblBrsBankStatementDtlTOTemp.ReceiptTypeId;
                                tblBrsBankStatementDtlTO.BankLedgerId = tblBrsBankStatementDtlTOTemp.BankLedgerId;

                                for (int j = 0; j < tblBrsBankStatementWiseDtlTOList.Count; j++)
                                {
                                    TblBrsBankStatementDtlTO tblBrsBankStatementWiseDtlTO = tblBrsBankStatementWiseDtlTOList[j];
                                    if (string.IsNullOrEmpty(tblBrsBankStatementWiseDtlTO.BrsBankStatementRecordValue.Trim()))
                                        continue;
                                    if (tblBrsBankStatementWiseDtlTO.CheckForChequeNo)
                                        tblBrsBankStatementDtlTO.ReceiptNo = tblBrsBankStatementWiseDtlTO.BrsBankStatementRecordValue;
                                    else if (tblBrsBankStatementWiseDtlTO.CheckForValueDate)
                                        //tblBrsBankStatementDtlTO.ReceiptDate = DateTime.Now;
                                    //Convert.ToDateTime(tblBrsBankStatementWiseDtlTO.BrsBankStatementRecordValue.ToString(Constants.AzureDateFormat));
                                        tblBrsBankStatementDtlTO.ReceiptDate = Convert.ToDateTime(Convert.ToDateTime(tblBrsBankStatementWiseDtlTO.BrsBankStatementRecordValue).ToString(Constants.AzureDateFormat));

                                    else if (tblBrsBankStatementWiseDtlTO.CheckForCredit)
                                    {
                                        tblBrsBankStatementDtlTO.Amount = Convert.ToDouble(tblBrsBankStatementWiseDtlTO.BrsBankStatementRecordValue);
                                        tblBrsBankStatementDtlTO.IdBrsBankStatementDtl = tblBrsBankStatementWiseDtlTO.IdBrsBankStatementDtl;
                                    }
                                    else if (tblBrsBankStatementWiseDtlTO.CheckForBankNar)
                                        tblBrsBankStatementDtlTO.Narration = tblBrsBankStatementWiseDtlTO.BrsBankStatementRecordValue;
                                    else if (tblBrsBankStatementWiseDtlTO.CheckForTransLoc)
                                        tblBrsBankStatementDtlTO.Location = tblBrsBankStatementWiseDtlTO.BrsBankStatementRecordValue;

                                }
                                tblBrsBankStatementDtlTOFinalList.Add(tblBrsBankStatementDtlTO);
                            }
                        }
                    }
                    return tblBrsBankStatementDtlTOFinalList;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<TblBrsBankStatementDtlTO> ConvertBrsBankStatementDtlDTToList(DataTable brsBankStatementDtlTODT)
        {
            List<TblBrsBankStatementDtlTO> brsBankStatementDtlTOList = new List<TblBrsBankStatementDtlTO>();
            if (brsBankStatementDtlTODT != null)
            {
                for (int rowCount = 0; rowCount < brsBankStatementDtlTODT.Rows.Count; rowCount++)
                {
                    TblBrsBankStatementDtlTO brsBankStatementDtlTONew = new TblBrsBankStatementDtlTO();
                    if (brsBankStatementDtlTODT.Rows[rowCount]["idBrsBankStatementDtl"] != DBNull.Value)
                        brsBankStatementDtlTONew.IdBrsBankStatementDtl = Convert.ToInt32(brsBankStatementDtlTODT.Rows[rowCount]["idBrsBankStatementDtl"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["brsBankStatementId"] != DBNull.Value)
                        brsBankStatementDtlTONew.BrsBankStatementId = Convert.ToInt32(brsBankStatementDtlTODT.Rows[rowCount]["brsBankStatementId"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["brsTemplateDtlId"] != DBNull.Value)
                        brsBankStatementDtlTONew.BrsTemplateDtlId = Convert.ToInt32(brsBankStatementDtlTODT.Rows[rowCount]["brsTemplateDtlId"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["brsBankStatementRecordDate"] != DBNull.Value)
                        brsBankStatementDtlTONew.BrsBankStatementRecordDate = Convert.ToDateTime(brsBankStatementDtlTODT.Rows[rowCount]["brsBankStatementRecordDate"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["isReconciled"] != DBNull.Value)
                        brsBankStatementDtlTONew.IsReconciled = Convert.ToBoolean(brsBankStatementDtlTODT.Rows[rowCount]["isReconciled"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["brsBankStatementRecordValue"] != DBNull.Value)
                        brsBankStatementDtlTONew.BrsBankStatementRecordValue = Convert.ToString(brsBankStatementDtlTODT.Rows[rowCount]["brsBankStatementRecordValue"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["referenceNo"] != DBNull.Value)
                        brsBankStatementDtlTONew.ReferenceNo = Convert.ToString(brsBankStatementDtlTODT.Rows[rowCount]["referenceNo"].ToString());

                    if (brsBankStatementDtlTODT.Rows[rowCount]["checkForMackingYn"] != DBNull.Value)
                        brsBankStatementDtlTONew.CheckForMackingYn = Convert.ToBoolean(brsBankStatementDtlTODT.Rows[rowCount]["checkForMackingYn"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["checkForValueDate"] != DBNull.Value)
                        brsBankStatementDtlTONew.CheckForValueDate = Convert.ToBoolean(brsBankStatementDtlTODT.Rows[rowCount]["checkForValueDate"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["checkForChequeNo"] != DBNull.Value)
                        brsBankStatementDtlTONew.CheckForChequeNo = Convert.ToBoolean(brsBankStatementDtlTODT.Rows[rowCount]["checkForChequeNo"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["checkForBankNar"] != DBNull.Value)
                        brsBankStatementDtlTONew.CheckForBankNar = Convert.ToBoolean(brsBankStatementDtlTODT.Rows[rowCount]["checkForBankNar"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["checkForCredit"] != DBNull.Value)
                        brsBankStatementDtlTONew.CheckForCredit = Convert.ToBoolean(brsBankStatementDtlTODT.Rows[rowCount]["checkForCredit"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["checkForDebit"] != DBNull.Value)
                        brsBankStatementDtlTONew.CheckForDebit = Convert.ToBoolean(brsBankStatementDtlTODT.Rows[rowCount]["checkForDebit"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["checkForTransLoc"] != DBNull.Value)
                        brsBankStatementDtlTONew.CheckForTransLoc = Convert.ToBoolean(brsBankStatementDtlTODT.Rows[rowCount]["checkForTransLoc"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["ledgerName"] != DBNull.Value)
                        brsBankStatementDtlTONew.BankName = Convert.ToString(brsBankStatementDtlTODT.Rows[rowCount]["ledgerName"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["userDisplayName"] != DBNull.Value)
                        brsBankStatementDtlTONew.ReceiptByName = Convert.ToString(brsBankStatementDtlTODT.Rows[rowCount]["userDisplayName"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["receiptTypeId"] != DBNull.Value)
                        brsBankStatementDtlTONew.ReceiptTypeId = Convert.ToInt32(brsBankStatementDtlTODT.Rows[rowCount]["receiptTypeId"].ToString());
                         if (brsBankStatementDtlTODT.Rows[rowCount]["rowNO"] != DBNull.Value)
                        brsBankStatementDtlTONew.RowNO = Convert.ToInt32(brsBankStatementDtlTODT.Rows[rowCount]["rowNO"].ToString());
                    if (brsBankStatementDtlTODT.Rows[rowCount]["finLedgerId"] != DBNull.Value)
                        brsBankStatementDtlTONew.BankLedgerId = Convert.ToInt32(brsBankStatementDtlTODT.Rows[rowCount]["finLedgerId"].ToString());
                    brsBankStatementDtlTOList.Add(brsBankStatementDtlTONew);
                }
            }
            return brsBankStatementDtlTOList;
        }

        #endregion

    }
}

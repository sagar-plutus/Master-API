using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using simpliMASTERSAPI.BL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblPaymentRequestDtlBL: ITblPaymentRequestDtlBL
    {
        private readonly ITblPaymentRequestDtlDAO _iTblPaymentRequestDtlDAO;
        public TblPaymentRequestDtlBL(ITblPaymentRequestDtlDAO iTblPaymentRequestDtlDAO)
        {
            _iTblPaymentRequestDtlDAO = iTblPaymentRequestDtlDAO;
        }
        #region Selection
        public  DataTable SelectAllTblPaymentRequestDtl()
        {
            return _iTblPaymentRequestDtlDAO.SelectAllTblPaymentRequestDtl();
        }

        public  List<TblPaymentRequestDtlTO> SelectAllTblPaymentRequestDtlList()
        {
            DataTable tblPaymentRequestDtlTODT = _iTblPaymentRequestDtlDAO.SelectAllTblPaymentRequestDtl();
            return ConvertDTToList(tblPaymentRequestDtlTODT);
        }

        public  TblPaymentRequestDtlTO SelectTblPaymentRequestDtlTO(Int32 idPayReqDtl)
        {
            DataTable tblPaymentRequestDtlTODT = _iTblPaymentRequestDtlDAO.SelectTblPaymentRequestDtl(idPayReqDtl);
            List<TblPaymentRequestDtlTO> tblPaymentRequestDtlTOList = ConvertDTToList(tblPaymentRequestDtlTODT);
            if(tblPaymentRequestDtlTOList != null && tblPaymentRequestDtlTOList.Count == 1)
                return tblPaymentRequestDtlTOList[0];
            else
                return null;
        }

        public  List<TblPaymentRequestDtlTO> ConvertDTToList(DataTable tblPaymentRequestDtlTODT )
        {
            List<TblPaymentRequestDtlTO> tblPaymentRequestDtlTOList = new List<TblPaymentRequestDtlTO>();
            if (tblPaymentRequestDtlTODT != null)
            {
                for (int rowCount = 0; rowCount < tblPaymentRequestDtlTODT.Rows.Count; rowCount++)
                {
                    TblPaymentRequestDtlTO tblPaymentRequestDtlTONew = new TblPaymentRequestDtlTO();
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["idPayReqDtl"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.IdPayReqDtl = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["idPayReqDtl"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["payReqId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.PayReqId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["payReqId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["payTypeId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.PayTypeId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["payTypeId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["refNo"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.RefNo = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["refNo"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["txnTypeId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.TxnTypeId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["txnTypeId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["statusId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.StatusId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["statusId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["supplierId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.SupplierId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["supplierId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["userId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.UserId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["userId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["expenseId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.ExpenseId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["expenseId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["advanceId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.AdvanceId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["advanceId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["departmentId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.DepartmentId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["departmentId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["payBankId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.PayBankId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["payBankId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["paymentTypeId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.PaymentTypeId = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["paymentTypeId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["payById"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.PayById = Convert.ToInt32( tblPaymentRequestDtlTODT.Rows[rowCount]["payById"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["paymentDate"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.PaymentDate = Convert.ToDateTime( tblPaymentRequestDtlTODT.Rows[rowCount]["paymentDate"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["payOn"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.PayOn = Convert.ToDateTime( tblPaymentRequestDtlTODT.Rows[rowCount]["payOn"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["amount"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.Amount = Convert.ToDouble( tblPaymentRequestDtlTODT.Rows[rowCount]["amount"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["grnId"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.GrnId = Convert.ToInt64( tblPaymentRequestDtlTODT.Rows[rowCount]["grnId"].ToString());
                    if(tblPaymentRequestDtlTODT.Rows[rowCount]["paymentNarration"] != DBNull.Value)
                        tblPaymentRequestDtlTONew.PaymentNarration = Convert.ToString( tblPaymentRequestDtlTODT.Rows[rowCount]["paymentNarration"].ToString());
                    tblPaymentRequestDtlTOList.Add(tblPaymentRequestDtlTONew);
                }
            }
            return tblPaymentRequestDtlTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO)
        {
            return _iTblPaymentRequestDtlDAO.InsertTblPaymentRequestDtl(tblPaymentRequestDtlTO);
        }

        public  int InsertTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentRequestDtlDAO.InsertTblPaymentRequestDtl(tblPaymentRequestDtlTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO)
        {
            return _iTblPaymentRequestDtlDAO.UpdateTblPaymentRequestDtl(tblPaymentRequestDtlTO);
        }

        public  int UpdateTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentRequestDtlDAO.UpdateTblPaymentRequestDtl(tblPaymentRequestDtlTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPaymentRequestDtl(Int32 idPayReqDtl)
        {
            return _iTblPaymentRequestDtlDAO.DeleteTblPaymentRequestDtl(idPayReqDtl);
        }

        public  int DeleteTblPaymentRequestDtl(Int32 idPayReqDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentRequestDtlDAO.DeleteTblPaymentRequestDtl(idPayReqDtl, conn, tran);
        }

        #endregion
        
    }
}

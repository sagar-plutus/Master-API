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
    public class TblPaymentRequestBL: ITblPaymentRequestBL
    {
        private readonly ITblPaymentRequestDAO _iTblPaymentRequestDAO;
        public TblPaymentRequestBL(ITblPaymentRequestDAO iTblPaymentRequestDAO)
        {
            _iTblPaymentRequestDAO = iTblPaymentRequestDAO;
        }
        #region Selection
        public  DataTable SelectAllTblPaymentRequest()
        {
            return _iTblPaymentRequestDAO.SelectAllTblPaymentRequest();
        }

        public  List<TblPaymentRequestTO> SelectAllTblPaymentRequestList()
        {
            DataTable tblPaymentRequestTODT = _iTblPaymentRequestDAO.SelectAllTblPaymentRequest();
            return ConvertDTToList(tblPaymentRequestTODT);
        }

        public  TblPaymentRequestTO SelectTblPaymentRequestTO(Int32 idPayReq)
        {
            DataTable tblPaymentRequestTODT = _iTblPaymentRequestDAO.SelectTblPaymentRequest(idPayReq);
            List<TblPaymentRequestTO> tblPaymentRequestTOList = ConvertDTToList(tblPaymentRequestTODT);
            if(tblPaymentRequestTOList != null && tblPaymentRequestTOList.Count == 1)
                return tblPaymentRequestTOList[0];
            else
                return null;
        }

        public  List<TblPaymentRequestTO> ConvertDTToList(DataTable tblPaymentRequestTODT )
        {
            List<TblPaymentRequestTO> tblPaymentRequestTOList = new List<TblPaymentRequestTO>();
            if (tblPaymentRequestTODT != null)
            {
                for (int rowCount = 0; rowCount < tblPaymentRequestTODT.Rows.Count; rowCount++)
                {
                    TblPaymentRequestTO tblPaymentRequestTONew = new TblPaymentRequestTO();
                    if(tblPaymentRequestTODT.Rows[rowCount]["idPayReq"] != DBNull.Value)
                        tblPaymentRequestTONew.IdPayReq = Convert.ToInt32( tblPaymentRequestTODT.Rows[rowCount]["idPayReq"].ToString());
                    if(tblPaymentRequestTODT.Rows[rowCount]["paymentAllocationId"] != DBNull.Value)
                        tblPaymentRequestTONew.PaymentAllocationId = Convert.ToInt32( tblPaymentRequestTODT.Rows[rowCount]["paymentAllocationId"].ToString());
                    if(tblPaymentRequestTODT.Rows[rowCount]["payTypeId"] != DBNull.Value)
                        tblPaymentRequestTONew.PayTypeId = Convert.ToInt32( tblPaymentRequestTODT.Rows[rowCount]["payTypeId"].ToString());
                    if(tblPaymentRequestTODT.Rows[rowCount]["refNo"] != DBNull.Value)
                        tblPaymentRequestTONew.RefNo = Convert.ToInt32( tblPaymentRequestTODT.Rows[rowCount]["refNo"].ToString());
                    if(tblPaymentRequestTODT.Rows[rowCount]["txnTypeId"] != DBNull.Value)
                        tblPaymentRequestTONew.TxnTypeId = Convert.ToInt32( tblPaymentRequestTODT.Rows[rowCount]["txnTypeId"].ToString());
                    if(tblPaymentRequestTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblPaymentRequestTONew.CreatedBy = Convert.ToInt32( tblPaymentRequestTODT.Rows[rowCount]["createdBy"].ToString());
                    if(tblPaymentRequestTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblPaymentRequestTONew.CreatedOn = Convert.ToDateTime( tblPaymentRequestTODT.Rows[rowCount]["createdOn"].ToString());
                    if(tblPaymentRequestTODT.Rows[rowCount]["amount"] != DBNull.Value)
                        tblPaymentRequestTONew.Amount = Convert.ToDouble( tblPaymentRequestTODT.Rows[rowCount]["amount"].ToString());
                    tblPaymentRequestTOList.Add(tblPaymentRequestTONew);
                }
            }
            return tblPaymentRequestTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO)
        {
            return _iTblPaymentRequestDAO.InsertTblPaymentRequest(tblPaymentRequestTO);
        }

        public  int InsertTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentRequestDAO.InsertTblPaymentRequest(tblPaymentRequestTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO)
        {
            return _iTblPaymentRequestDAO.UpdateTblPaymentRequest(tblPaymentRequestTO);
        }

        public  int UpdateTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentRequestDAO.UpdateTblPaymentRequest(tblPaymentRequestTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPaymentRequest(Int32 idPayReq)
        {
            return _iTblPaymentRequestDAO.DeleteTblPaymentRequest(idPayReq);
        }

        public int DeleteTblPaymentRequest(Int32 idPayReq, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentRequestDAO.DeleteTblPaymentRequest(idPayReq, conn, tran);
        }

        #endregion
        
    }
}

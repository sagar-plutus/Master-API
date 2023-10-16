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
    public class TblBankAmountBL: ITblBankAmountBL
    {
        private readonly ITblBankAmountDAO _iTblBankAmountDAO;
        public TblBankAmountBL(ITblBankAmountDAO iTblBankAmountDAO)
        {
            _iTblBankAmountDAO = iTblBankAmountDAO;
        }
        #region Selection
        public  DataTable SelectAllTblBankAmount()
        {
            return _iTblBankAmountDAO.SelectAllTblBankAmount();
        }

        public  List<TblBankAmountTO> SelectAllTblBankAmountList()
        {
            DataTable tblBankAmountTODT = _iTblBankAmountDAO.SelectAllTblBankAmount();
            return ConvertDTToList(tblBankAmountTODT);
        }

        public  TblBankAmountTO SelectTblBankAmountTO(Int32 idBankAmount)
        {
            DataTable tblBankAmountTODT = _iTblBankAmountDAO.SelectTblBankAmount(idBankAmount);
            List<TblBankAmountTO> tblBankAmountTOList = ConvertDTToList(tblBankAmountTODT);
            if(tblBankAmountTOList != null && tblBankAmountTOList.Count == 1)
                return tblBankAmountTOList[0];
            else
                return null;
        }

        public  List<TblBankAmountTO> ConvertDTToList(DataTable tblBankAmountTODT )
        {
            List<TblBankAmountTO> tblBankAmountTOList = new List<TblBankAmountTO>();
            if (tblBankAmountTODT != null)
            {
                for (int rowCount = 0; rowCount < tblBankAmountTODT.Rows.Count; rowCount++)
                {
                    TblBankAmountTO tblBankAmountTONew = new TblBankAmountTO();
                    if(tblBankAmountTODT.Rows[rowCount]["idBankAmount"] != DBNull.Value)
                        tblBankAmountTONew.IdBankAmount = Convert.ToInt32( tblBankAmountTODT.Rows[rowCount]["idBankAmount"].ToString());
                    if(tblBankAmountTODT.Rows[rowCount]["bankLedgerId"] != DBNull.Value)
                        tblBankAmountTONew.BankLedgerId = Convert.ToInt32( tblBankAmountTODT.Rows[rowCount]["bankLedgerId"].ToString());
                    if(tblBankAmountTODT.Rows[rowCount]["amountTakenBy"] != DBNull.Value)
                        tblBankAmountTONew.AmountTakenBy = Convert.ToInt32( tblBankAmountTODT.Rows[rowCount]["amountTakenBy"].ToString());
                    if(tblBankAmountTODT.Rows[rowCount]["paymentAllocationId"] != DBNull.Value)
                        tblBankAmountTONew.PaymentAllocationId = Convert.ToInt32( tblBankAmountTODT.Rows[rowCount]["paymentAllocationId"].ToString());
                    if(tblBankAmountTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblBankAmountTONew.CreatedBy = Convert.ToInt32( tblBankAmountTODT.Rows[rowCount]["createdBy"].ToString());
                    if(tblBankAmountTODT.Rows[rowCount]["amountTakenOn"] != DBNull.Value)
                        tblBankAmountTONew.AmountTakenOn = Convert.ToDateTime( tblBankAmountTODT.Rows[rowCount]["amountTakenOn"].ToString());
                    if(tblBankAmountTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblBankAmountTONew.CreatedOn = Convert.ToDateTime( tblBankAmountTODT.Rows[rowCount]["createdOn"].ToString());
                    if(tblBankAmountTODT.Rows[rowCount]["isLatestEntry"] != DBNull.Value)
                        tblBankAmountTONew.IsLatestEntry = Convert.ToBoolean( tblBankAmountTODT.Rows[rowCount]["isLatestEntry"].ToString());
                    if(tblBankAmountTODT.Rows[rowCount]["bankAmt"] != DBNull.Value)
                        tblBankAmountTONew.BankAmt = Convert.ToDouble( tblBankAmountTODT.Rows[rowCount]["bankAmt"].ToString());
                    tblBankAmountTOList.Add(tblBankAmountTONew);
                }
            }
            return tblBankAmountTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertTblBankAmount(TblBankAmountTO tblBankAmountTO)
        {
            return _iTblBankAmountDAO.InsertTblBankAmount(tblBankAmountTO);
        }

        public  int InsertTblBankAmount(TblBankAmountTO tblBankAmountTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBankAmountDAO.InsertTblBankAmount(tblBankAmountTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblBankAmount(TblBankAmountTO tblBankAmountTO)
        {
            return _iTblBankAmountDAO.UpdateTblBankAmount(tblBankAmountTO);
        }

        public  int UpdateTblBankAmount(TblBankAmountTO tblBankAmountTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBankAmountDAO.UpdateTblBankAmount(tblBankAmountTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblBankAmount(Int32 idBankAmount)
        {
            return _iTblBankAmountDAO.DeleteTblBankAmount(idBankAmount);
        }

        public  int DeleteTblBankAmount(Int32 idBankAmount, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBankAmountDAO.DeleteTblBankAmount(idBankAmount, conn, tran);
        }

        #endregion
        
    }
}

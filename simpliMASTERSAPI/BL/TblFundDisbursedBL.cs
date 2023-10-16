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
    public class TblFundDisbursedBL: ITblFundDisbursedBL
    {
        public ITblFundDisbursedDAO _iTblFundDisbursedDAO;
        public TblFundDisbursedBL(ITblFundDisbursedDAO iTblFundDisbursedDAO)
        {
            _iTblFundDisbursedDAO = iTblFundDisbursedDAO;
        }

        #region Selection
        public DataTable SelectAllTblFundDisbursed()
        {
            return _iTblFundDisbursedDAO.SelectAllTblFundDisbursed();
        }

        public List<TblFundDisbursedTO> SelectAllTblFundDisbursedList()
        {
            DataTable tblFundDisbursedTODT = _iTblFundDisbursedDAO.SelectAllTblFundDisbursed();
            return ConvertDTToList(tblFundDisbursedTODT);
        }

        public TblFundDisbursedTO SelectTblFundDisbursedTO(Int32 idFundDisbursed)
        {
            DataTable tblFundDisbursedTODT = _iTblFundDisbursedDAO.SelectTblFundDisbursed(idFundDisbursed);
            List<TblFundDisbursedTO> tblFundDisbursedTOList = ConvertDTToList(tblFundDisbursedTODT);
            if (tblFundDisbursedTOList != null && tblFundDisbursedTOList.Count == 1)
                return tblFundDisbursedTOList[0];
            else
                return null;
        }

        public List<TblFundDisbursedTO> ConvertDTToList(DataTable tblFundDisbursedTODT)
        {
            List<TblFundDisbursedTO> tblFundDisbursedTOList = new List<TblFundDisbursedTO>();
            if (tblFundDisbursedTODT != null)
            {
                for (int rowCount = 0; rowCount < tblFundDisbursedTODT.Rows.Count; rowCount++)
                {
                    TblFundDisbursedTO tblFundDisbursedTONew = new TblFundDisbursedTO();
                    if (tblFundDisbursedTODT.Rows[rowCount]["idFundDisbursed"] != DBNull.Value)
                        tblFundDisbursedTONew.IdFundDisbursed = Convert.ToInt32(tblFundDisbursedTODT.Rows[rowCount]["idFundDisbursed"].ToString());
                    if (tblFundDisbursedTODT.Rows[rowCount]["paymentAllocationId"] != DBNull.Value)
                        tblFundDisbursedTONew.PaymentAllocationId = Convert.ToInt32(tblFundDisbursedTODT.Rows[rowCount]["paymentAllocationId"].ToString());
                    if (tblFundDisbursedTODT.Rows[rowCount]["payTypeId"] != DBNull.Value)
                        tblFundDisbursedTONew.PayTypeId = Convert.ToInt32(tblFundDisbursedTODT.Rows[rowCount]["payTypeId"].ToString());
                    if (tblFundDisbursedTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblFundDisbursedTONew.CreatedBy = Convert.ToInt32(tblFundDisbursedTODT.Rows[rowCount]["createdBy"].ToString());
                    if (tblFundDisbursedTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblFundDisbursedTONew.CreatedOn = Convert.ToDateTime(tblFundDisbursedTODT.Rows[rowCount]["createdOn"].ToString());
                    if (tblFundDisbursedTODT.Rows[rowCount]["disbursedAmt"] != DBNull.Value)
                        tblFundDisbursedTONew.DisbursedAmt = Convert.ToDouble(tblFundDisbursedTODT.Rows[rowCount]["disbursedAmt"].ToString());
                    if (tblFundDisbursedTODT.Rows[rowCount]["balanceDisbursedAmt"] != DBNull.Value)
                        tblFundDisbursedTONew.BalanceDisbursedAmt = Convert.ToDouble(tblFundDisbursedTODT.Rows[rowCount]["balanceDisbursedAmt"].ToString());
                    if (tblFundDisbursedTODT.Rows[rowCount]["balanceAmt"] != DBNull.Value)
                        tblFundDisbursedTONew.BalanceAmt = Convert.ToDouble(tblFundDisbursedTODT.Rows[rowCount]["balanceAmt"].ToString());
                    tblFundDisbursedTOList.Add(tblFundDisbursedTONew);
                }
            }
            return tblFundDisbursedTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO)
        {
            return _iTblFundDisbursedDAO.InsertTblFundDisbursed(tblFundDisbursedTO);
        }

        public int InsertTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFundDisbursedDAO.InsertTblFundDisbursed(tblFundDisbursedTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO)
        {
            return _iTblFundDisbursedDAO.UpdateTblFundDisbursed(tblFundDisbursedTO);
        }

        public int UpdateTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFundDisbursedDAO.UpdateTblFundDisbursed(tblFundDisbursedTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblFundDisbursed(Int32 idFundDisbursed)
        {
            return _iTblFundDisbursedDAO.DeleteTblFundDisbursed(idFundDisbursed);
        }

        public int DeleteTblFundDisbursed(Int32 idFundDisbursed, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFundDisbursedDAO.DeleteTblFundDisbursed(idFundDisbursed, conn, tran);
        }

        #endregion

    }
}

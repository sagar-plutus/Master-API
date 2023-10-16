using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;

namespace ODLMWebAPI.BL
{
    public class TblReceiptChargesBL: ITblReceiptChargesBL
    {
        private readonly ITblReceiptChargesDAO _iTblReceiptChargesDAO;
        public TblReceiptChargesBL(ITblReceiptChargesDAO iTblReceiptChargesDAO)
        {
            _iTblReceiptChargesDAO = iTblReceiptChargesDAO;
        }
        #region Selection
        public  DataTable SelectAllTblReceiptCharges()
        {
            return _iTblReceiptChargesDAO.SelectAllTblReceiptCharges();
        }

        public  List<TblReceiptChargesTO> SelectAllTblReceiptChargesList()
        {
            DataTable tblReceiptChargesTODT = _iTblReceiptChargesDAO.SelectAllTblReceiptCharges();
            return ConvertDTToList(tblReceiptChargesTODT);
        }

        public  TblReceiptChargesTO SelectTblReceiptChargesTO(Int32 idReceiptCharges)
        {
            DataTable tblReceiptChargesTODT = _iTblReceiptChargesDAO.SelectTblReceiptCharges(idReceiptCharges);
            List<TblReceiptChargesTO> tblReceiptChargesTOList = ConvertDTToList(tblReceiptChargesTODT);
            if(tblReceiptChargesTOList != null && tblReceiptChargesTOList.Count == 1)
                return tblReceiptChargesTOList[0];
            else
                return null;
        }

        public  List<TblReceiptChargesTO> ConvertDTToList(DataTable tblReceiptChargesTODT )
        {
            List<TblReceiptChargesTO> tblReceiptChargesTOList = new List<TblReceiptChargesTO>();
            if (tblReceiptChargesTODT != null)
            {
                for (int rowCount = 0; rowCount < tblReceiptChargesTODT.Rows.Count; rowCount++)
                {
                    TblReceiptChargesTO tblReceiptChargesTONew = new TblReceiptChargesTO();
                    if(tblReceiptChargesTODT.Rows[rowCount]["idReceiptCharges"] != DBNull.Value)
                        tblReceiptChargesTONew.IdReceiptCharges = Convert.ToInt32( tblReceiptChargesTODT.Rows[rowCount]["idReceiptCharges"].ToString());
                    if(tblReceiptChargesTODT.Rows[rowCount]["brsBankStatementDtlId"] != DBNull.Value)
                        tblReceiptChargesTONew.BrsBankStatementDtlId = Convert.ToInt32( tblReceiptChargesTODT.Rows[rowCount]["brsBankStatementDtlId"].ToString());
                    if(tblReceiptChargesTODT.Rows[rowCount]["otherChargesTypeId"] != DBNull.Value)
                        tblReceiptChargesTONew.OtherChargesTypeId = Convert.ToInt32( tblReceiptChargesTODT.Rows[rowCount]["otherChargesTypeId"].ToString());
                    if(tblReceiptChargesTODT.Rows[rowCount]["amount"] != DBNull.Value)
                        tblReceiptChargesTONew.Amount = Convert.ToDouble( tblReceiptChargesTODT.Rows[rowCount]["amount"].ToString());
                    tblReceiptChargesTOList.Add(tblReceiptChargesTONew);
                }
            }
            return tblReceiptChargesTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO)
        {
            return _iTblReceiptChargesDAO.InsertTblReceiptCharges(tblReceiptChargesTO);
        }

        public  int InsertTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReceiptChargesDAO.InsertTblReceiptCharges(tblReceiptChargesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO)
        {
            return _iTblReceiptChargesDAO.UpdateTblReceiptCharges(tblReceiptChargesTO);
        }

        public  int UpdateTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReceiptChargesDAO.UpdateTblReceiptCharges(tblReceiptChargesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblReceiptCharges(Int32 idReceiptCharges)
        {
            return _iTblReceiptChargesDAO.DeleteTblReceiptCharges(idReceiptCharges);
        }

        public  int DeleteTblReceiptCharges(Int32 idReceiptCharges, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReceiptChargesDAO.DeleteTblReceiptCharges(idReceiptCharges, conn, tran);
        }

        #endregion
        
    }
}

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
using ODLMWebAPI.StaticStuff;
using System.Reflection;

namespace ODLMWebAPI.BL
{
    public class TblPurchaseCompetitorExtBL : ITblPurchaseCompetitorExtBL
    {
        private readonly ITblPurchaseCompetitorExtDAO _iTblPurchaseCompetitorExtDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly Itblpurchasecompetitorupdatesdao _itblpurchasecompetitorupdatesdao;
        public TblPurchaseCompetitorExtBL(IConnectionString iConnectionString, ITblPurchaseCompetitorExtDAO iTblPurchaseCompetitorExtDAO, Itblpurchasecompetitorupdatesdao itblpurchasecompetitorupdatesdao)
        {
            _iTblPurchaseCompetitorExtDAO = iTblPurchaseCompetitorExtDAO;
            _iConnectionString = iConnectionString;
            _itblpurchasecompetitorupdatesdao = itblpurchasecompetitorupdatesdao;
        }
        #region Selection

        public List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList()
        {
            return  _iTblPurchaseCompetitorExtDAO.SelectAllTblPurchaseCompetitorExt();
        }
        public List<TblGlobalRatePurchaseTO> SelectAllComptitorExtDtls(DateTime fromDate, DateTime toDate)
        {
            return _iTblPurchaseCompetitorExtDAO.SelectAllComptitorExtDtls(fromDate, toDate);
        }

        public TblPurchaseCompetitorExtTO SelectTblPurchaseCompetitorExtTO(Int32 idPurCompetitorExt)
        {
            return  _iTblPurchaseCompetitorExtDAO.SelectTblPurchaseCompetitorExt(idPurCompetitorExt);
        }

        #endregion
        
        #region Insertion
        public int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO)
        {
            return _iTblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO);
        }

        public int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO, conn, tran);
        }

        /// <summary>
        ///  Priyanka [16-02-18]: Added to get Purchase Competitor Details
        /// </summary>
        /// <returns></returns>
        public List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList(Int32 organizationId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblPurchaseCompetitorExtList(organizationId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList(Int32 organizationId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.SelectAllTblPurchaseCompetitorExt(organizationId, conn, tran);

        }

        #endregion

        #region Updation
        public int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO)
        {
            return _iTblPurchaseCompetitorExtDAO.UpdateTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO);
        }

        public int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.UpdateTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt)
        {
            return _iTblPurchaseCompetitorExtDAO.DeleteTblPurchaseCompetitorExt(idPurCompetitorExt);
        }

        public int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.DeleteTblPurchaseCompetitorExt(idPurCompetitorExt, conn, tran);
        }

        #endregion

        public DataTable GetCompititorAndRateHistoryDtls(DateTime fromDate, DateTime toDate)
        {
            DataTable dtRate = new DataTable();
            DataTable dtCompitior = new DataTable();

            List<TblGlobalRatePurchaseTO> tblPurchaseCompetitorExtTOList = _iTblPurchaseCompetitorExtDAO.SelectAllComptitorExtDtls(fromDate, toDate);

            List<TblPurchaseCompetitorUpdatesTO> tblPurchaseCompetitorUpdatesTOList = _itblpurchasecompetitorupdatesdao.SelectAllComptitorUpdateDtls(fromDate, toDate);
            /*
            simpliMASTERSAPI.BL.ListtoDataTableConverter converter = new simpliMASTERSAPI.BL.ListtoDataTableConverter();
            dtRate = converter.ToDataTable(tblPurchaseCompetitorExtTOList);
            dtCompitior = converter.ToDataTable(tblPurchaseCompetitorUpdatesTOList);

            foreach (DataRow dr in dtCompitior.Rows)
            {
                DataRow row = dtRate.NewRow();
                row["CreatedOn"] = dr["CreatedOn"].ToString();
                dtRate.Rows.Add(row);
                DataColumn dValue = new DataColumn();
                dValue.ColumnName = dr["firmName"].ToString();
                dValue.DataType = Type.GetType("System.Decimal");
                dtRate.Columns.Add(dValue);
            }
            if (dtRate != null && dtRate.Rows.Count > 0)
            {
                foreach (DataRow row in dtRate.Rows)
                {
                    string CreatedOn = row["CreatedOn"].ToString();
                    string Rate = row["Rate"].ToString();

                }
            }
            DataView dv = dtRate.DefaultView;
            dv.Sort = "CreatedOn ASC";
            DataTable sortedDT = dv.ToTable();
            //dt.DefaultView.Sort = "Parameter_Name";
            //dt = dt.DefaultView.ToTable();            
            */  
            return dtRate;
        }

       


    }
}

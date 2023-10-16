using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using simpliMASTERSAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.BL
{
    public class TblPaymentAllocationBL: ITblPaymentAllocationBL
    {
        private readonly ITblPaymentAllocationDAO _iTblPaymentAllocationDAO;
        public TblPaymentAllocationBL(ITblPaymentAllocationDAO iTblPaymentAllocationDAO)
        {
            _iTblPaymentAllocationDAO = iTblPaymentAllocationDAO;
        }
        #region Selection
        public  DataTable SelectAllTblPaymentAllocation()
        {
            return _iTblPaymentAllocationDAO.SelectAllTblPaymentAllocation();
        }

        public  List<TblPaymentAllocationTO> SelectAllTblPaymentAllocationList()
        {
            DataTable tblPaymentAllocationTODT = _iTblPaymentAllocationDAO.SelectAllTblPaymentAllocation();
            return ConvertDTToList(tblPaymentAllocationTODT);
        }

        public  TblPaymentAllocationTO SelectTblPaymentAllocationTO(Int32 idPaymentAllocation)
        {
            DataTable tblPaymentAllocationTODT = _iTblPaymentAllocationDAO.SelectTblPaymentAllocation(idPaymentAllocation);
            List<TblPaymentAllocationTO> tblPaymentAllocationTOList = ConvertDTToList(tblPaymentAllocationTODT);
            if(tblPaymentAllocationTOList != null && tblPaymentAllocationTOList.Count == 1)
                return tblPaymentAllocationTOList[0];
            else
                return null;
        }
        
        public  List<TblPaymentAllocationTO> ConvertDTToList(DataTable tblPaymentAllocationTODT )
        {
            List<TblPaymentAllocationTO> tblPaymentAllocationTOList = new List<TblPaymentAllocationTO>();
            if (tblPaymentAllocationTODT != null)
            {
                for (int rowCount = 0; rowCount < tblPaymentAllocationTODT.Rows.Count; rowCount++)
                {
                    TblPaymentAllocationTO tblPaymentAllocationTONew = new TblPaymentAllocationTO();
                    if(tblPaymentAllocationTODT.Rows[rowCount]["idPaymentAllocation"] != DBNull.Value)
                        tblPaymentAllocationTONew.IdPaymentAllocation = Convert.ToInt32( tblPaymentAllocationTODT.Rows[rowCount]["idPaymentAllocation"].ToString());
                    if(tblPaymentAllocationTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblPaymentAllocationTONew.CreatedBy = Convert.ToInt32( tblPaymentAllocationTODT.Rows[rowCount]["createdBy"].ToString());
                    if(tblPaymentAllocationTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblPaymentAllocationTONew.CreatedOn = Convert.ToDateTime( tblPaymentAllocationTODT.Rows[rowCount]["createdOn"].ToString());
                    if(tblPaymentAllocationTODT.Rows[rowCount]["isLatestEntry"] != DBNull.Value)
                        tblPaymentAllocationTONew.IsLatestEntry = Convert.ToBoolean( tblPaymentAllocationTODT.Rows[rowCount]["isLatestEntry"].ToString());
                    if(tblPaymentAllocationTODT.Rows[rowCount]["bankAmt"] != DBNull.Value)
                        tblPaymentAllocationTONew.BankAmt = Convert.ToDouble( tblPaymentAllocationTODT.Rows[rowCount]["bankAmt"].ToString());
                    if(tblPaymentAllocationTODT.Rows[rowCount]["fundAllocationAmt"] != DBNull.Value)
                        tblPaymentAllocationTONew.FundAllocationAmt = Convert.ToDouble( tblPaymentAllocationTODT.Rows[rowCount]["fundAllocationAmt"].ToString());
                    tblPaymentAllocationTOList.Add(tblPaymentAllocationTONew);
                }
            }
            return tblPaymentAllocationTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO)
        {
            return _iTblPaymentAllocationDAO.InsertTblPaymentAllocation(tblPaymentAllocationTO);
        }

        public  int InsertTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentAllocationDAO.InsertTblPaymentAllocation(tblPaymentAllocationTO, conn, tran);
        }
        


       public ResultMessage InsertUpdatePaymentRequestAndDetail()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertUpdatePaymentRequestAndDetail");
                return resultMessage;
            }
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO)
        {
            return _iTblPaymentAllocationDAO.UpdateTblPaymentAllocation(tblPaymentAllocationTO);
        }

        public  int UpdateTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentAllocationDAO.UpdateTblPaymentAllocation(tblPaymentAllocationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPaymentAllocation(Int32 idPaymentAllocation)
        {
            return _iTblPaymentAllocationDAO.DeleteTblPaymentAllocation(idPaymentAllocation);
        }

        public  int DeleteTblPaymentAllocation(Int32 idPaymentAllocation, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPaymentAllocationDAO.DeleteTblPaymentAllocation(idPaymentAllocation, conn, tran);
        }

        #endregion
        
    }
}

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

namespace ODLMWebAPI.BL
{ 
    public class TblUserExtBL : ITblUserExtBL
    {
        #region Selection
        private readonly ITblUserExtDAO _iTblUserExtDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblUserExtBL(ICommon iCommon, IConnectionString iConnectionString, ITblUserExtDAO iTblUserExtDAO)
        {
            _iTblUserExtDAO = iTblUserExtDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        public List<TblUserExtTO> SelectAllTblUserExtList()
        {
            return _iTblUserExtDAO.SelectAllTblUserExt();
        }

        public TblUserExtTO SelectTblUserExtTO(Int32 userId)
        {
            return _iTblUserExtDAO.SelectTblUserExt(userId);
        }
        //Priyanka [04-09-2019]
        public TblUserExtTO SelectTblUserExtByOrganizationId(Int32 organizationId)
        {
            return _iTblUserExtDAO.SelectTblUserExtByOrganizationId(organizationId);
        }
        public List<TblUserExtTO> GetAllUserSettingsList()
        {
            return _iTblUserExtDAO.GetAllUserSettingsList();
        }

        #endregion 

        #region Insertion
        public int InsertTblUserExt(TblUserExtTO tblUserExtTO)
        {
            return _iTblUserExtDAO.InsertTblUserExt(tblUserExtTO);
        }

        public int InsertTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserExtDAO.InsertTblUserExt(tblUserExtTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblUserSettings(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserExtDAO.UpdateTblUserExtSettings(tblUserExtTO, conn, tran);
        }
        public ResultMessage UpdateUserSettings(List<TblUserExtTO> tblUserExtTOList)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                DateTime ServerDateTime = _iCommon.ServerDateTime;
                Int32 result = 0;
                #region Update UserSetting
                if (tblUserExtTOList == null || tblUserExtTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Invalid User Setting List");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();
                for (int i = 0; i < tblUserExtTOList.Count; i++)
                {
                    TblUserExtTO TblUserExtTO = tblUserExtTOList[i];
                    TblUserExtTO.UpdatedOn = ServerDateTime;
                    result = UpdateTblUserSettings(TblUserExtTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to updated user setting Details.User Id - " + TblUserExtTO.UserId);
                        return resultMessage;
                    }
                }
                #endregion
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        public int UpdateTblUserExt(TblUserExtTO tblUserExtTO)
        {
            return _iTblUserExtDAO.UpdateTblUserExt(tblUserExtTO);
        }

        public int UpdateTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserExtDAO.UpdateTblUserExt(tblUserExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblUserExt()
        {
            return _iTblUserExtDAO.DeleteTblUserExt();
        }

        public int DeleteTblUserExt(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserExtDAO.DeleteTblUserExt(conn, tran);
        }

        #endregion
        
    }
}

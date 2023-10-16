using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.Models;

namespace simpliMASTERSAPI.DAL
{
    public class TblGradingDAO : ITblGradingDAO
    {
        private readonly IConnectionString _iConnectionString;
        String sqlConnStr = null;
        String sqlSelectQry = "SELECT * FROM [tblGrading]";

        public TblGradingDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        }

        #region Selection
        public List<TblGradingTo> GetAllGrading(bool? isActive = null)
        {
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = sqlSelectQry + (isActive == null ? "" : isActive == false ? " WHERE isActive = 0" : " WHERE isActive = 1 or isActive = null ");
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblGradingTo> ConvertDTToList(SqlDataReader tblGradingTODT)
        {
            List<TblGradingTo> tblGradingTOList = null;
            if (tblGradingTODT != null)
            {
                tblGradingTOList = new List<TblGradingTo>();
                while (tblGradingTODT.Read())
                {
                    TblGradingTo tblGradingTONew = new TblGradingTo();
                    if (tblGradingTODT["idGrading"] != DBNull.Value)
                        tblGradingTONew.idGrading = Convert.ToInt32(tblGradingTODT["idGrading"].ToString());
                  
                    if (tblGradingTODT["isActive"] != DBNull.Value)
                        tblGradingTONew.isActive = Convert.ToInt32(tblGradingTODT["isActive"].ToString());

                    if (tblGradingTODT["gradingName"] != DBNull.Value)
                        tblGradingTONew.gradingName = Convert.ToString(tblGradingTODT["gradingName"].ToString());

                    if (tblGradingTODT["gradingDes"] != DBNull.Value)
                        tblGradingTONew.gradingDes = Convert.ToString(tblGradingTODT["gradingDes"].ToString());

                    tblGradingTOList.Add(tblGradingTONew);
                }
            }
            return tblGradingTOList;
        }
        #endregion
    }
}   

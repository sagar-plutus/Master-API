using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.DAL
{
    public class TblCRMLanguageDAO : ITblCRMLanguageDAO
    {
        IConnectionString _iConnectionString;
        public TblCRMLanguageDAO(IConnectionString iConnetionString)
        {
            _iConnectionString = iConnetionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = "SELECT * FROM tblCRMLanguage ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection

        public List<tblCRMLanguageTO> SelectAllTblCRMLanguage()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<tblCRMLanguageTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region DTToListConvertors

        public List<tblCRMLanguageTO> ConvertDTToList(SqlDataReader LanguageTODT)
        {
            List<tblCRMLanguageTO> CRMLanguageTOList = new List<tblCRMLanguageTO>();
            if (LanguageTODT != null)
            {
                while (LanguageTODT.Read())
                {
                    tblCRMLanguageTO CRMLanguageTONew = new tblCRMLanguageTO();

                    if (LanguageTODT["idLanguage"] != DBNull.Value)
                        CRMLanguageTONew.idLanguage = Convert.ToInt32(LanguageTODT["idLanguage"]);

                    if (LanguageTODT["lagName"] != DBNull.Value)
                        CRMLanguageTONew.lagName = Convert.ToString(LanguageTODT["lagName"]);

                    if (LanguageTODT["isDefault"] != DBNull.Value)
                        CRMLanguageTONew.isDefault = Convert.ToInt32(LanguageTODT["isDefault"]);

                    if (LanguageTODT["isActive"] != DBNull.Value)
                        CRMLanguageTONew.isActive = Convert.ToInt32(LanguageTODT["isActive"]);

                    if (LanguageTODT["createdBy"] != DBNull.Value)
                        CRMLanguageTONew.createdBy = Convert.ToInt32(LanguageTODT["createdBy"]);

                    if (LanguageTODT["createdOn"] != DBNull.Value)
                        CRMLanguageTONew.createdOn = Convert.ToDateTime(LanguageTODT["createdOn"]);

                    if (LanguageTODT["updatedBy"] != DBNull.Value)
                        CRMLanguageTONew.updatedBy = Convert.ToInt32(LanguageTODT["updatedBy"]);

                    if (LanguageTODT["updatedOn"] != DBNull.Value)
                        CRMLanguageTONew.updatedOn = Convert.ToDateTime(LanguageTODT["updatedOn"]);

                    CRMLanguageTOList.Add(CRMLanguageTONew);
                }
            }
            return CRMLanguageTOList;
        }

        #endregion
    }
}

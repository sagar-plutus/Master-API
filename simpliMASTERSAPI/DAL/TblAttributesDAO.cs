using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblAttributesDAO : ITblAttributesDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblAttributesDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods

        public String SqlSelectQuery()
        {
            String sqlSelectQry = "SELECT attr.*,attrConfig.pageId FROM tblAttributes attr"
                                  + " LEFT JOIN dimAttributeConfig attrConfig ON attr.attributeConfigId = attrConfig.idAttributeConfig ";

            return sqlSelectQry;
        }

        #endregion

        #region Selection

        public List<TblAttributesTO> SelectAllAttributes()
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
                List<TblAttributesTO> list = ConvertDTToList(rdr);
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

        public List<TblAttributesTO> SelectAllAttributesForPage(int pageId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + " where attrConfig.pageId =" + pageId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAttributesTO> list = ConvertDTToList(rdr);
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

        public TblAttributesTO SelectAttributesById(int attrId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE attr.idAttribute = " + attrId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAttributesTO> list = ConvertDTToList(rdr);
                if (list.Any())
                {
                    return list[0];
                }
                else
                {
                    return null;
                }
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

        public TblAttributesTO SelectAttributesByName(string attrName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE attr.attributeName = " + attrName;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAttributesTO> list = ConvertDTToList(rdr);
                if (list.Any())
                {
                    return list[0];
                }
                else
                {
                    return null;
                }
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

        public List<TblAttributesTO> ConvertDTToList(SqlDataReader AttributesTODT)
        {
            List<TblAttributesTO> AttributesTOList = new List<TblAttributesTO>();
            if (AttributesTODT != null)
            {
                while (AttributesTODT.Read())
                {
                    TblAttributesTO AttributesTONew = new TblAttributesTO();

                    if (AttributesTODT["idAttribute"] != DBNull.Value)
                        AttributesTONew.idAttribute = Convert.ToInt32(AttributesTODT["idAttribute"]);

                    if (AttributesTODT["attributeDisplayName"] != DBNull.Value)
                        AttributesTONew.attributeDisplayName = Convert.ToString(AttributesTODT["attributeDisplayName"]);

                    if (AttributesTODT["attributeName"] != DBNull.Value)
                        AttributesTONew.attributeName = Convert.ToString(AttributesTODT["attributeName"]);

                    if (AttributesTODT["pageId"] != DBNull.Value)
                        AttributesTONew.pageId = Convert.ToInt32(AttributesTODT["pageId"]);

                    AttributesTOList.Add(AttributesTONew);
                }
            }
            return AttributesTOList;
        }

        #endregion

    }
}

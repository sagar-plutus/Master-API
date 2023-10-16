using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimUserTypeDAO:IDimUserTypeDAO
    {

        private readonly IConnectionString _iConnectionString;
       public  DimUserTypeDAO (IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimUserType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<DropDownTO> SelectAllDimUserType()
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

             SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public DropDownTO SelectDimUserType(int idUserType,SqlConnection con,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() +"Where idUserType = "+idUserType;
                cmdSelect.Connection = con;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Transaction = tran;
                 sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();

            }
        }


        public List<DropDownTO> ConvertDTToList(SqlDataReader dimUserTypeTODT)
        {
            List<DropDownTO> userTypeList = new List<DropDownTO>();
            if (dimUserTypeTODT != null)
            {
                while (dimUserTypeTODT.Read())
                {
                    DropDownTO userTypeTO = new DropDownTO();
                    if (dimUserTypeTODT["typeDesc"] != DBNull.Value)
                        userTypeTO.Text = Convert.ToString(dimUserTypeTODT["typeDesc"].ToString());
                    if (dimUserTypeTODT["idUserType"] != DBNull.Value)
                        userTypeTO.Value = Convert.ToInt32(dimUserTypeTODT["idUserType"].ToString());
                userTypeList.Add(userTypeTO);
                }
            }
            return userTypeList;
        }
        #endregion
        
     
    }
}

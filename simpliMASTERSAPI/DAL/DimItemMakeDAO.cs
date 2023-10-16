using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
//using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.DAL
{
    public class DimItemMakeDAO: IDimItemMakeDAO
    {

        private readonly IConnectionString _iConnectionString;
        public DimItemMakeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
   
        #region Insertion

        public int InsertDimItemMake(DimItemMakeTO dimItemMakeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteInsertCommand(dimItemMakeTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public List<DropDownTO> CheckMakeExistsOrNot(String itemMakeDesc)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimItemMake WHERE itemMakeDesc= @ItemMakeDesc AND isActive = 1";
                cmdSelect = new SqlCommand(sqlQuery, conn);
                cmdSelect.Parameters.Add("@ItemMakeDesc", System.Data.SqlDbType.NVarChar).Value = itemMakeDesc;
                
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idItemMake"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idItemMake"].ToString());
                    if (dateReader["itemMakeDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["itemMakeDesc"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public int ExecuteInsertCommand(DimItemMakeTO dimItemMakeTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [dimItemMake]( " +

                                "  [itemMakeDesc]" +
                                " ,[isActive]" +
                                " )" +
                    " VALUES (" +

                                "  @ItemMakeDesc " +
                                " ,@IsActive " +

                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdItemMake", System.Data.SqlDbType.Int).Value = dimItemMakeTO.idItemMake;
            cmdInsert.Parameters.Add("@ItemMakeDesc", System.Data.SqlDbType.NVarChar).Value = dimItemMakeTO.ItemMakeDesc;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimItemMakeTO.IsActive;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                dimItemMakeTO.IdItemMake = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;


        }

      
        #endregion
    }
}

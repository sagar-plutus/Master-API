using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;

namespace ODLMWebAPI.DAL
{
    public class DimensionDAO : IDimensionDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimensionDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public List<DropDownTO> SelectDeliPeriodForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimDelPeriod WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idDelPeriod"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idDelPeriod"].ToString());
                    if (dateReader["deliveryPeriod"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["deliveryPeriod"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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
        //Priyanka [13-09-2019]
        public List<DropDownTO> GetSupplierDivisionGroupDDL()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimSuppDivGroup WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSuppDivGroup"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSuppDivGroup"].ToString());
                    if (dateReader["suppDivGroupDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["suppDivGroupDesc"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();
                return dropDownTOList;
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

        //Priyanka [23-04-2019] : Added for get drop down list for SAP
        public List<DropDownTO> GetSAPMasterDropDown(Int32 dimensionId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String SqlQuery = "SELECT * FROM dimGenericMaster WHERE dimensionId =" + dimensionId + " AND isActive=1";

                cmdSelect = new SqlCommand(SqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {

                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idGenericMaster"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idGenericMaster"].ToString());
                    if (dateReader["value"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["value"].ToString());
                    if (dateReader["dimensionId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["dimensionId"].ToString());
                    if (dateReader["mappedTxnId"] != DBNull.Value)
                        dropDownTONew.MappedTxnId = Convert.ToString(dateReader["mappedTxnId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public DropDownTO GetSAPMasterByIdGenericMaster(Int32 idGenericMaster)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String SqlQuery = "SELECT * FROM dimGenericMaster WHERE idGenericMaster =" + idGenericMaster + " AND isActive=1";

                cmdSelect = new SqlCommand(SqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //  DropDownTO dropDownTOList = new Models.DropDownTO();
                DropDownTO dropDownTONew = new DropDownTO();

                while (dateReader.Read())
                {

                    if (dateReader["idGenericMaster"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idGenericMaster"].ToString());
                    if (dateReader["value"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["value"].ToString());
                    if (dateReader["dimensionId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["dimensionId"].ToString());
                    if (dateReader["mappedTxnId"] != DBNull.Value)
                        dropDownTONew.MappedTxnId = Convert.ToString(dateReader["mappedTxnId"].ToString());

                    // dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTONew;
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
        public List<Dictionary<string, string>> GetColumnName(string tablename, Int32 tableValue)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlCommand cmdtblSelect = null;
            String aqlQuery = null;
            try
            {
                conn.Open();
                if (tableValue > 0)
                {
                    aqlQuery = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + "'" + tablename + "'" + " ORDER BY ORDINAL_POSITION;" +
                               "SELECT sapMaster.* from " + tablename + " sapMaster " +
                               "LEFT JOIN tblMasterDimension mstDimension ON mstDimension.idDimension = sapMaster.dimensionId " +
                               "WHERE sapMaster.dimensionId = " + tableValue;
                }
                else
                {
                    aqlQuery = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + "'" + tablename + "'" + " ORDER BY ORDINAL_POSITION; SELECT * from " + tablename + "";

                }
                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<string> columnName = new List<string>();
                List<Dictionary<string, string>> main = new List<Dictionary<string, string>>();
                while (dateReader.Read())
                {
                    if (dateReader["COLUMN_NAME"] != DBNull.Value)
                        columnName.Add(Convert.ToString(dateReader["COLUMN_NAME"]));
                }
                if (dateReader.NextResult())
                {
                    if (dateReader.HasRows)
                    {
                        while (dateReader.Read())
                        {
                            Dictionary<string, string> hh = new Dictionary<string, string>();
                            for (int i = 0; i < columnName.Count; i++)
                            {
                                hh.Add(columnName[i], Convert.ToString(dateReader[columnName[i]]));
                            }
                            main.Add(hh);
                        }
                    }
                    else
                    {
                        Dictionary<string, string> hh = new Dictionary<string, string>();
                        for (int i = 0; i < columnName.Count; i++)
                        {
                            hh.Add(columnName[i], null);
                        }
                        main.Add(hh);
                    }

                }

                if (dateReader != null)
                    dateReader.Dispose();

                return main;
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

        public Int32 InsertdimentionalData(string tableQuery, Boolean IsInsertion, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                if (conn != null)
                {
                    cmdInsert.Connection = conn;
                    cmdInsert.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdInsert.Connection = conn;
                    conn.Open();
                }
                cmdInsert.CommandText = tableQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                int isInserted = cmdInsert.ExecuteNonQuery();
                if (IsInsertion == true)
                {
                    Int32 idTask = 0;
                    if (isInserted > 0)
                    {
                        idTask = 1;
                        //cmdInsert.CommandText = "Select SCOPE_IDENTITY()";
                        //idTask = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    }
                    return idTask;
                }
                else
                {
                    return isInserted;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (tran == null)
                {
                    conn.Close();
                }
                cmdInsert.Dispose();
            }
        }

        public List<DimensionTO> SelectAllMasterDimensionList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblMasterDimension";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimensionTO> dropDownTOList = new List<Models.DimensionTO>();
                while (dateReader.Read())
                {
                    DimensionTO dimensionTONew = new DimensionTO();
                    if (dateReader["idDimension"] != DBNull.Value)
                        dimensionTONew.IdDimension = Convert.ToInt32(dateReader["idDimension"].ToString());
                    if (dateReader["displayName"] != DBNull.Value)
                        dimensionTONew.DisplayName = Convert.ToString(dateReader["displayName"].ToString());
                    if (dateReader["dimensionValue"] != DBNull.Value)
                        dimensionTONew.DimensionValue = Convert.ToString(dateReader["dimensionValue"].ToString());
                    if (dateReader["isActive"] != DBNull.Value)
                        dimensionTONew.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());
                    if (dateReader["isApplyDimension"] != DBNull.Value)
                        dimensionTONew.IsApplyDimension = Convert.ToInt32(dateReader["IsApplyDimension"].ToString());
                    dropDownTOList.Add(dimensionTONew);
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

        public Int32 getidentityOfTable(string Query)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                cmdSelect = new SqlCommand(Query, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (dateReader.HasRows)
                {
                    return 1;
                }
                dateReader.Dispose();
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public Int32 getMaxCountOfTable(string CoulumName, string tableName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                string Query = " select max(" + CoulumName + ") as cnt from " + tableName;
                cmdSelect = new SqlCommand(Query, conn);
                object dateReader = cmdSelect.ExecuteScalar();
                return Convert.ToInt32(dateReader);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public Int32 GetMaxCountOfSAPTable(string CoulumName, string tableName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                string Query = " select max(" + CoulumName + ") as cnt from " + tableName;
                cmdSelect = new SqlCommand(Query, conn);
                object dateReader = cmdSelect.ExecuteScalar();
                return Convert.ToInt32(dateReader);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DropDownTO> SelectCDStructureForDropDown(Int32 isRsOrPerncent, int moduleId, int orgTypeId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimCdStructure WHERE isActive=1";

                if (orgTypeId == (int)Constants.OrgTypeE.DEALER)
                {
                    moduleId = 1;
                }

                if (isRsOrPerncent == 1)
                {
                    sqlQuery += " AND isPercent=0";
                }
                else if (isRsOrPerncent == 2)
                {
                    sqlQuery += " AND isPercent=1";
                }
                if (moduleId != 0)
                {
                    sqlQuery += " AND moduleId=" + moduleId;
                }
                else
                {
                    sqlQuery += " AND isnull(moduleId,0)= 0";
                }

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCdStructure"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCdStructure"].ToString());
                    if (dateReader["cdValue"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["cdValue"].ToString());
                    if (dateReader["isPercent"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["isPercent"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public List<DropDownTO> SelectCountriesForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCountry";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCountry"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCountry"].ToString());
                    if (dateReader["countryName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["countryName"].ToString());
                    if (dateReader["countryCode"] != DBNull.Value)
                        dropDownTONew.Code = Convert.ToString(dateReader["countryCode"].ToString());


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

        public List<DropDownTO> SelectOrgLicensesForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCommerLicenceInfo WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idLicense"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idLicense"].ToString());
                    if (dateReader["licenseName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["licenseName"].ToString());

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

        public List<DropDownTO> SelectSalutationsForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimSalutation";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSalutation"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSalutation"].ToString());
                    if (dateReader["salutationDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["salutationDesc"].ToString());

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


        public DropDownTO SelectSalutationOnId(int idSalutation)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimSalutation where idSalutation =" + idSalutation;

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSalutation"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSalutation"].ToString());
                    if (dateReader["salutationDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["salutationDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dropDownTOList != null && dropDownTOList.Count == 1)
                    return dropDownTOList[0];
                else return null;
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
        /// <summary>
        /// Hrishikesh[27 - 03 - 2018] Added to get district by state
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>

        public List<StateMasterTO> SelectDistrictForStateMaster(int stateId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (stateId > 0)

                    sqlQuery = "SELECT * FROM dimDistrict WHERE stateId=" + stateId + " AND isActive = 1 ";
                else
                    sqlQuery = "SELECT * FROM dimDistrict where isActive=1 ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<StateMasterTO> dropDownTOList = new List<Models.StateMasterTO>();
                while (dateReader.Read())
                {
                    StateMasterTO dropDownTONew = new StateMasterTO();
                    if (dateReader["idDistrict"] != DBNull.Value)
                        dropDownTONew.Id = Convert.ToInt32(dateReader["idDistrict"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Name = Convert.ToString(dateReader["districtName"].ToString());
                    if (dateReader["stateId"] != DBNull.Value)
                        dropDownTONew.ParentId = Convert.ToInt32(dateReader["stateId"].ToString());
                    if (dateReader["districtCode"] != DBNull.Value)
                        dropDownTONew.Code = Convert.ToString(dateReader["districtCode"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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
        public List<DropDownTO> SelectDistrictForDropDown(int stateId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (stateId > 0)
                    sqlQuery = "SELECT * FROM dimDistrict WHERE stateId=" + stateId + " AND isActive = 1 ";
                else
                    sqlQuery = "SELECT * FROM dimDistrict ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idDistrict"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idDistrict"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["districtName"].ToString());
                    if (dateReader["stateId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["stateId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public List<DropDownTO> SelectStatesForDropDown(int countryId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (countryId > 0)
                    sqlQuery = "SELECT * FROM dimState WHERE countryId=" + countryId;  //No where condition. As we dont have country column in states
                else
                    sqlQuery = "SELECT * FROM dimState ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idState"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idState"].ToString());
                    if (dateReader["stateName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["stateName"].ToString());
                    if (dateReader["stateOrUTCode"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["stateOrUTCode"].ToString());
                    if (dateReader["stateCode"] != DBNull.Value)
                        dropDownTONew.Code = Convert.ToString(dateReader["stateCode"].ToString());
                    if (dateReader["mappedTxnId"] != DBNull.Value)
                        dropDownTONew.MappedTxnId = Convert.ToString(dateReader["mappedTxnId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public List<DropDownTO> SelectTalukaForDropDown(int districtId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (districtId > 0)
                    sqlQuery = "SELECT * FROM dimTaluka WHERE districtId=" + districtId + " AND isActive = 1 ";
                else
                    sqlQuery = "SELECT * FROM dimTaluka WHERE isActive=1 ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idTaluka"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idTaluka"].ToString());
                    if (dateReader["talukaName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["talukaName"].ToString());

                    if (dateReader["districtId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["districtId"].ToString());
                    dropDownTOList.Add(dropDownTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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
        /// <summary>
        /// Deepali[19-10-2018]added :to get Department wise Users

        public List<DropDownTO> GetUserListDepartmentWise(string deptId, int roleTypeId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "select distinct(U.idUser),URD.*,U.userDisplayName,person.primaryEmail,UExt.organizationId from tblUserReportingDetails URD left join tblOrgStructure OS"
                                   + " on OS.idOrgStructure = URD.orgStructureId "
                                   + " left join tblUser U on u.idUser = URD.userId "
                                   + " left join tblUserExt UExt on u.idUser = UExt.userId "
                                   + " left join tblPerson person on UExt.personId = person.idPerson "
                                   + " left join tblUserRole ur on ur.userId = u.idUser "
                                   + " left join tblRole role on role.idRole = ur.roleId "
                                   + "where U.isActive = 1 and OS.isActive = 1 and URD.isActive = 1 ";

                if (deptId != null)
                {
                    aqlQuery = aqlQuery + "and OS.deptId IN (" + deptId + ")";
                }
                if (roleTypeId != 0)
                {
                    aqlQuery = aqlQuery + " and role.roleTypeId = " + roleTypeId;
                }

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idUser"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());
                    if (dateReader["primaryEmail"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["primaryEmail"].ToString());
                    if (dateReader["organizationId"] != DBNull.Value)
                        dropDownTONew.Code = Convert.ToString(dateReader["organizationId"].ToString());

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

        /// <summary>
        /// Hrishikesh[27 - 03 - 2018] Added to get taluka by district
        /// 
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public List<StateMasterTO> SelectTalukaForStateMaster(int districtId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (districtId > 0)
                    sqlQuery = "SELECT * FROM dimTaluka WHERE districtId=" + districtId + " AND isActive = 1 ";
                else
                    sqlQuery = "SELECT * FROM dimTaluka WHERE isActive=1";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<StateMasterTO> dropDownTOList = new List<Models.StateMasterTO>();
                while (dateReader.Read())
                {
                    StateMasterTO dropDownTONew = new StateMasterTO();
                    if (dateReader["idTaluka"] != DBNull.Value)
                        dropDownTONew.Id = Convert.ToInt32(dateReader["idTaluka"].ToString());
                    if (dateReader["talukaName"] != DBNull.Value)
                        dropDownTONew.Name = Convert.ToString(dateReader["talukaName"].ToString());

                    if (dateReader["districtId"] != DBNull.Value)
                        dropDownTONew.ParentId = Convert.ToInt32(dateReader["districtId"].ToString());

                    if (dateReader["talukaCode"] != DBNull.Value)
                        dropDownTONew.Code = Convert.ToString(dateReader["talukaCode"].ToString());
                    dropDownTOList.Add(dropDownTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblRole WHERE enableAreaAlloc=1 AND isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

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


        public List<DropDownTO> SelectAllSystemRoleListForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblRole WHERE  isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

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

        public List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = " SELECT distinct districtId,dimDistrict.districtName FROM tblOrganization  " +
                           " LEFT JOIN tblOrgAddress ON idOrganization = organizationId " +
                           " LEFT JOIN tblAddress ON idAddr = addressId " +
                           " LEFT JOIN dimDistrict ON idDistrict = districtId " +
                           " WHERE tblOrganization.isActive=1 AND tblOrganization.idOrganization IN(SELECT dealerOrgId FROM tblCnfDealers WHERE cnfOrgId=" + cnfOrgId + " and isActive=1) " +
                           " ORDER BY dimDistrict.districtName ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["districtId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["districtId"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["districtName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public List<DropDownTO> SelectAllTransportModeForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimTransportMode WHERE  isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idTransMode"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idTransMode"].ToString());
                    if (dateReader["transportMode"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["transportMode"].ToString());

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

        public List<DropDownTO> SelectInvoiceTypeForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceTypes WHERE  isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvoiceType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvoiceType"].ToString());
                    if (dateReader["invoiceTypeDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["invoiceTypeDesc"].ToString());

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

        public List<DropDownTO> SelectInvoiceModeForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceMode WHERE  1=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvoiceMode"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvoiceMode"].ToString());
                    if (dateReader["invoiceMode"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["invoiceMode"].ToString());

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

        public List<DropDownTO> SelectCurrencyForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCurrency";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCurrency"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCurrency"].ToString());
                    if (dateReader["currencyName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["currencyName"].ToString());
                    if (dateReader["currencySymbol"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["currencySymbol"].ToString());
                    if (dateReader["fa_fa_Icon"] != DBNull.Value)
                        dropDownTONew.Icon = Convert.ToString(dateReader["fa_fa_Icon"].ToString());
                    if (dateReader["currnecyCode"] != DBNull.Value)
                        dropDownTONew.Code = Convert.ToString(dateReader["currnecyCode"].ToString());
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
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public DropDownTO SelectCurrencyById(Int32 currencyId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCurrency where idCurrency=" + currencyId + "";
                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO dropDownTONew = new DropDownTO();
                while (dateReader.Read())
                {
                    if (dateReader["idCurrency"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCurrency"].ToString());
                    if (dateReader["currencyName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["currencyName"].ToString());
                    if (dateReader["currnecyCode"] != DBNull.Value)
                        dropDownTONew.Code = Convert.ToString(dateReader["currnecyCode"].ToString());
                }
                if (dateReader != null)
                    dateReader.Dispose();
                return dropDownTONew;
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

        public List<DropDownTO> GetInvoiceStatusForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCurrency";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCurrency"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCurrency"].ToString());
                    if (dateReader["currencyName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["currencyName"].ToString());
                    if (dateReader["currencySymbol"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["currencySymbol"].ToString());

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

        public List<DimFinYearTO> SelectAllMstFinYearList(SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                String aqlQuery = "SELECT * FROM dimFinYear ";

                cmdSelect = new SqlCommand(aqlQuery, conn, tran);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimFinYearTO> finYearTOList = new List<DimFinYearTO>();
                while (dateReader.Read())
                {
                    DimFinYearTO finYearTO = new DimFinYearTO();
                    if (dateReader["idFinYear"] != DBNull.Value)
                        finYearTO.IdFinYear = Convert.ToInt32(dateReader["idFinYear"].ToString());
                    if (dateReader["finYearDisplayName"] != DBNull.Value)
                        finYearTO.FinYearDisplayName = Convert.ToString(dateReader["finYearDisplayName"].ToString());
                    if (dateReader["finYearStartDate"] != DBNull.Value)
                        finYearTO.FinYearStartDate = Convert.ToDateTime(dateReader["finYearStartDate"].ToString());
                    if (dateReader["finYearEndDate"] != DBNull.Value)
                        finYearTO.FinYearEndDate = Convert.ToDateTime(dateReader["finYearEndDate"].ToString());

                    finYearTOList.Add(finYearTO);
                }

                return finYearTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                cmdSelect.Dispose();
            }

        }

        // Vaibhav [27-Sep-2017] added to select reporting type list
        public List<DropDownTO> SelectReportingType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM dimReportingType WHERE isActive= 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reportingTypeTO = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (reportingTypeTO.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (reportingTypeTO["idReportingType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(reportingTypeTO["idReportingType"].ToString());
                    if (reportingTypeTO["reportingTypeName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(reportingTypeTO["reportingTypeName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectReportingType");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Vaibhav [3-Oct-2017] added to select visit issue reason list
        public List<DimVisitIssueReasonsTO> SelectVisitIssueReasonsList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimVisitIssueReasons WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DimVisitIssueReasonsTO> visitIssueReasonTOList = new List<DimVisitIssueReasonsTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DimVisitIssueReasonsTO dimVisitIssueReasonsTONew = new DimVisitIssueReasonsTO();
                    if (visitIssueReasonTODT["idVisitIssueReasons"] != DBNull.Value)
                        dimVisitIssueReasonsTONew.IdVisitIssueReasons = Convert.ToInt32(visitIssueReasonTODT["idVisitIssueReasons"].ToString());
                    if (visitIssueReasonTODT["issueTypeId"] != DBNull.Value)
                        dimVisitIssueReasonsTONew.IssueTypeId = Convert.ToInt32(visitIssueReasonTODT["issueTypeId"].ToString());
                    if (visitIssueReasonTODT["visitIssueReasonName"] != DBNull.Value)
                        dimVisitIssueReasonsTONew.VisitIssueReasonName = Convert.ToString(visitIssueReasonTODT["visitIssueReasonName"].ToString());

                    visitIssueReasonTOList.Add(dimVisitIssueReasonsTONew);
                }
                return visitIssueReasonTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectReportingType");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectBrandList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimBrand WHERE isActive=1 ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idBrand"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idBrand"].ToString());
                    if (dateReader["brandName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["brandName"].ToString());
                    //[05-09-2018]Vijaymala added to get default brand data for other item
                    if (dateReader["isDefault"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["isDefault"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        /// <summary>
        /// [2017-01-02]Vijaymala:Added to get loading layer list 
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectLoadingLayerList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimLoadingLayers WHERE isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idLoadingLayer"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idLoadingLayer"].ToString());
                    if (dateReader["layerDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["layerDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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


        // Vijaymala [09-11-2017] added to get state code
        public DropDownTO SelectStateCode(Int32 stateId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = "select idState,stateOrUTCode from dimState  WHERE  idState = " + stateId;

                conn.Open();
                SqlDataReader departmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO dropDownTO = new DropDownTO();
                if (departmentTODT != null)
                {
                    while (departmentTODT.Read())
                    {
                        if (departmentTODT["idState"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(departmentTODT["idState"].ToString());
                        if (departmentTODT["stateOrUTCode"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(departmentTODT["stateOrUTCode"].ToString());
                    }
                }
                return dropDownTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Sanjay[2018-02-19] To Get dropdown list of Item Product Categories in the system
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetItemProductCategoryListForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimItemProdCateg WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idItemProdCat"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idItemProdCat"].ToString());
                    if (visitIssueReasonTODT["itemProdCategory"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["itemProdCategory"].ToString());
                    if (visitIssueReasonTODT["itemProdCategoryDesc"] != DBNull.Value)
                        dropDownTO.Tag = Convert.ToString(visitIssueReasonTODT["itemProdCategoryDesc"].ToString());
                    if (visitIssueReasonTODT["isFixedAsset"] != DBNull.Value)
                        dropDownTO.Code = Convert.ToString(visitIssueReasonTODT["isFixedAsset"].ToString());
                    if (visitIssueReasonTODT["isScrapProdItem"] != DBNull.Value)
                        dropDownTO.isScrapProdItem = Convert.ToBoolean(visitIssueReasonTODT["isScrapProdItem"].ToString());
                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetItemProductCategoryListForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Sudhir[22-01-2018] Added for getStatusofInvoice.
        public List<DropDownTO> GetInvoiceStatusDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceStatus";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvStatus"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvStatus"].ToString());
                    if (dateReader["statusName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["statusName"].ToString());
                    if (dateReader["statusDesc"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["statusDesc"].ToString());

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

        //Sudhir[07-MAR-2018] Added for All Firm Types List.
        public List<DropDownTO> SelectAllFirmTypesForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                conn.Open();
                sqlQuery = "SELECT * FROM dimFirmType WHERE isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idFirmType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idFirmType"].ToString());
                    if (dateReader["firmName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["firmName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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


        //Sudhir[07-MAR-2018] Added for All Firm Types List.
        public List<DropDownTO> SelectAllInfluencerTypesForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM dimInfluencerType WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInfluencerType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInfluencerType"].ToString());
                    if (dateReader["influencerTypeName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["influencerTypeName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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


        //Sudhir[15-MAR-2018] Added for Select All Enquiry Channels  
        public List<DropDownTO> SelectAllEnquiryChannels()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                //DataTableExample();

                conn.Open();
                sqlQuery = "SELECT * FROM dimEnqChannel WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idEnqChanel"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idEnqChanel"].ToString());
                    if (dateReader["enqChannelDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["enqChannelDesc"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        //Sudhir[15-MAR-2018] Added for Select All Industry Sector.
        public List<DropDownTO> SelectAllIndustrySector()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                conn.Open();
                sqlQuery = "SELECT * FROM dimIndustrySector WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idIndustrySector"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idIndustrySector"].ToString());
                    if (dateReader["industrySectorDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["industrySectorDesc"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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
        public List<DropDownTO> GetCallBySelfForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimCallBySelfTo WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idCallBySelf"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idCallBySelf"].ToString());
                    if (visitIssueReasonTODT["callBySelfDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["callBySelfDesc"].ToString());

                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetCallBySelfForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetArrangeForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimArrangeFor WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idArrangeFor"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idArrangeFor"].ToString());
                    if (visitIssueReasonTODT["arrangeForDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["arrangeForDesc"].ToString());

                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetArrangeForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetArrangeVisitToDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimArrangeVisitTo WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idArrangeVisitTo"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idArrangeVisitTo"].ToString());
                    if (visitIssueReasonTODT["arrangeVisitToDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["arrangeVisitToDesc"].ToString());
                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetArrangeVisitToDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectAllOrganizationType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = "SELECT * FROM dimOrgType WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idOrgType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idOrgType"].ToString());
                    if (dateReader["OrgType"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["OrgType"].ToString());
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DropDownTO> SelectAddressTypeListForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimAddressType  where isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idAddressType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idAddressType"].ToString());
                    if (dateReader["addressType"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["addressType"].ToString());
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

            //vijaymala added[21-06-2018]to get cd dropdown


        }
        //vijaymala added[21-06-2018]to get cd dropdown
        public DropDownTO SelectCDDropDown(Int32 cdStructureId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = "SELECT * FROM dimCdStructure WHERE isActive=1 and idCdStructure = " + cdStructureId;

                conn.Open();
                SqlDataReader cdTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO dropDownTO = new DropDownTO();
                if (cdTODT != null)
                {
                    while (cdTODT.Read())
                    {
                        if (cdTODT["idCdStructure"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(cdTODT["idCdStructure"].ToString());
                        if (cdTODT["isPercent"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(cdTODT["isPercent"].ToString());
                    }
                }
                return dropDownTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }



        }
        public List<DropDownTO> SelectAllVisitTypeListForDropDown()
        {
            {

                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                SqlCommand cmdSelect = null;
                SqlDataReader dateReader = null;
                try
                {
                    conn.Open();
                    String aqlQuery = "SELECT * FROM dimVisitType WHERE  isActive=1";

                    cmdSelect = new SqlCommand(aqlQuery, conn);
                    dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                    while (dateReader.Read())
                    {
                        DropDownTO dropDownTONew = new DropDownTO();
                        if (dateReader["idVisit"] != DBNull.Value)
                            dropDownTONew.Value = Convert.ToInt32(dateReader["idVisit"].ToString());
                        if (dateReader["visitType"] != DBNull.Value)
                            dropDownTONew.Text = Convert.ToString(dateReader["visitType"].ToString());

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

        }
        public List<DropDownTO> SelectDefaultRoleListForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT Role.* FROM tblRole Role INNER JOIN  dimOrgType OrgType  ON Role.idROle = OrgType.defaultRoleId AND Role.isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

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

        public List<DropDownTO> SelectItemMakeDropdownList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimItemMake WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idItemMake"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idItemMake"].ToString());
                    if (dateReader["itemMakeDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["itemMakeDesc"].ToString());
                    if (dateReader["isDefault"] != DBNull.Value)
                        dropDownTONew.Flag = Convert.ToInt32(dateReader["isDefault"].ToString());

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
        public List<DropDownTO> GetFinVoucherTypeList(String VoucherType = "")
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimFinVoucherType WHERE isActive=1";

                if (!String.IsNullOrEmpty(VoucherType))
                {
                    sqlQuery += " and type IN(" + VoucherType + ")";
                }
                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idfinVoucherType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idfinVoucherType"].ToString());
                    if (dateReader["FinVoucherType"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["FinVoucherType"].ToString());

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
        public List<DropDownTO> GetVoucherNoteReasonList(String IdVoucherNoteReasonStr = "")
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimVoucherNoteReason WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idVoucherNoteReason"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idVoucherNoteReason"].ToString());
                    if (dateReader["reasonDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["reasonDesc"].ToString());
                    if (dateReader["voucherTypeId"] != DBNull.Value)
                        dropDownTONew.MappedTxnId = Convert.ToString(dateReader["voucherTypeId"].ToString());
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

        public List<DropDownTO> GetAllGstCodeTaxPercentageList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "select taxPct from tblGstCodeDtls where isActive = 1 and taxPct > 0 group by taxPct order by taxPct";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["taxPct"] != DBNull.Value)
                        dropDownTONew.MappedTxnId = Convert.ToString(dateReader["taxPct"].ToString());
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
        public List<DropDownTO> RemoveSupplierFromSAP()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["CardCode"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["CardCode"].ToString());
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

        public List<DropDownTO> SelectItemBrandDropdownList(int itemMakeId = 0)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = string.Empty;
                if (itemMakeId == 0)
                    sqlQuery = "SELECT * FROM dimItemBrand WHERE isActive=1";
                else
                    sqlQuery = "SELECT * FROM dimItemBrand WHERE isActive=1 AND itemMakeId=" + itemMakeId;


                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idItemBrand"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idItemBrand"].ToString());
                    if (dateReader["itemBrandDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["itemBrandDesc"].ToString());

                    if (dateReader["itemMakeId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["itemMakeId"].ToString());

                    if (dateReader["isDefault"] != DBNull.Value)
                        dropDownTONew.Flag = Convert.ToInt32(dateReader["isDefault"].ToString());

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

        /// <summary>
        /// Vijaymala[08-09-2018]added:to get state from booking
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectStatesForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = "select distinct addr.stateId as idState,stateD.stateName ,stateD.stateOrUTCode from tblAddress addr" +
                    "  left join dimState stateD on stateD.idState = addr.stateId " +
                    "  left join tblOrgAddress orgAddr on orgAddr.addressId = addr.idAddr " +
                    "  left join tblOrganization org on orgAddr.organizationId = org.idOrganization " +
                    "  left join tblBookings booking on booking.dealerOrgId = org.idOrganization ";


                String sqlAllQuery = String.Empty;
                if (countryId > 0)
                    sqlAllQuery = sqlQuery + "where CAST(booking.createdOn AS DATE) BETWEEN @fromDate AND @toDate"; //No where condition. As we dont have country column in states
                else
                    sqlAllQuery = sqlQuery + "where CAST(booking.createdOn AS DATE) BETWEEN @fromDate AND @toDate";



                cmdSelect = new SqlCommand(sqlAllQuery, conn);
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idState"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idState"].ToString());
                    if (dateReader["stateName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["stateName"].ToString());
                    if (dateReader["stateOrUTCode"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["stateOrUTCode"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        /// <summary>
        /// Vijaymala[08-09-2018]:added to get district from booking
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDistrictForDropDownAccToBooking(int stateId, DateTime fromDate, DateTime toDate)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "select distinct addr.districtId as idDistrict,dist.districtName,dist.stateId from tblAddress addr" +
                    "  left join dimDistrict dist on dist.idDistrict = addr.districtId " +
                    "  left join tblOrgAddress orgAddr on orgAddr.addressId = addr.idAddr " +
                    "  left join tblOrganization org on orgAddr.organizationId = org.idOrganization " +
                    "  left join tblBookings booking on booking.dealerOrgId = org.idOrganization ";


                String sqlAllQuery = string.Empty;
                if (stateId > 0)
                    sqlAllQuery = sqlQuery + "where CAST(booking.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND stateId=" + stateId;
                else
                    sqlAllQuery = sqlQuery + "where CAST(booking.createdOn AS DATE) BETWEEN @fromDate AND @toDate  ";



                cmdSelect = new SqlCommand(sqlAllQuery, conn);
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idDistrict"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idDistrict"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["districtName"].ToString());
                    if (dateReader["stateId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["stateId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public List<DropDownTO> GetFixedDropDownList()
        {
            List<DropDownTO> dropDownToList = new List<DropDownTO>()
            {
                new DropDownTO {Text=" USER ",Value=1,Tag=1},
                new DropDownTO {Text=" STATE ",Value=2,Tag=0},
                new DropDownTO {Text=" DISTRICT",Value=3,Tag=0}
            };
            return dropDownToList;
        }

        public List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM  [dbo].[tblCRMSiteType] WHERE parentSiteTypeId=" + parentSiteTypeId;

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSiteType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSiteType"].ToString());
                    if (dateReader["siteTypeDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["siteTypeDisplayName"].ToString());
                    if (dateReader["parentSiteTypeId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["parentSiteTypeId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public List<DropDownTO> SelectAllInvoiceCopyList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM  [dbo].[dimInvoiceCopy] WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvoiceCopy"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvoiceCopy"].ToString());
                    if (dateReader["invoiceCopyName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["invoiceCopyName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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
        public Int32 getMaxCountOfSAPTable(string CoulumName, string tableName, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                string Query = " select max(" + CoulumName + ") as cnt from " + tableName;
                cmdSelect = new SqlCommand(Query, conn, tran);
                cmdSelect.CommandType = CommandType.Text;
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (dateReader.Read())
                {
                    return Convert.ToInt32(dateReader["cnt"]);
                }

                return -1;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (dateReader != null) dateReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public Int32 GetSAPUOMGroupUGPEntry(string ugpCode, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                string Query = "SELECT UgpEntry FROM OUGP WHERE   ugpcode = '" + ugpCode + "'";

                cmdSelect = new SqlCommand(Query, conn, tran);
                cmdSelect.CommandType = CommandType.Text;
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (dateReader.Read())
                {
                    return Convert.ToInt32(dateReader["UgpEntry"]);
                }

                return -1;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (dateReader != null) dateReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> getUomGropConversionFromSAP(Int32 baseUom, Int32 conversionUOM, Double altQty, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select UGP1.* from OUGP OUGP " +
                " JOIN UGP1 UGP1 on UGP1.UgpEntry = OUGP.UgpEntry " +
                " where OUGP.BaseUom = @BaseUom AND UGP1.UomEntry = @ConversionUOM ";
                if (altQty > 0)
                {
                    sqlQuery += " AND UGP1.AltQty = @AltQty";//Reshma[24-12-2020] Update COnversion Factor
                }

                cmdSelect = new SqlCommand(sqlQuery, conn, tran);
                cmdSelect.Parameters.Add("@BaseUom", System.Data.SqlDbType.Int).Value = baseUom;
                cmdSelect.Parameters.Add("@ConversionUOM", System.Data.SqlDbType.Int).Value = conversionUOM;
                cmdSelect.Parameters.Add("@AltQty", System.Data.SqlDbType.Decimal).Value = altQty;
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["UgpEntry"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["UgpEntry"].ToString());
                    if (dateReader["AltQty"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["AltQty"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        //Reshma Added
        public Int32 UpdateSAPUOMGroupUGPEntry(Int32 UgpEntry, Double altQty, Int32 UomEntry, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = null;
            SqlDataReader dateReader = null;
            try
            {
                string Query = "update UGP1   set AltQty =" + altQty + "  where UgpEntry =" + UgpEntry + "  and AltQty !=1  And UomEntry= " + UomEntry + "";

                cmdUpdate = new SqlCommand(Query, conn, tran);
                cmdUpdate.CommandType = CommandType.Text;
                return cmdUpdate.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
            }
        }
        public List<DropDownTO> getUomGropConversion(Int32 baseUom, Int32 conversionUOM, Double altQty, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                sqlQuery = "select dimUomGroupConversion.* from dimUomGroup " +
                " JOIN dimUomGroupConversion dimUomGroupConversion on dimUomGroupConversion.uomGroupId = dimUomGroup.idUomGroup " +
                " where dimUomGroup.baseUomId = @BaseUom AND dimUomGroupConversion.uomId = @ConversionUOM AND dimUomGroupConversion.altQty = @AltQty";

                cmdSelect = new SqlCommand(sqlQuery, conn, tran);
                cmdSelect.Parameters.Add("@BaseUom", System.Data.SqlDbType.Int).Value = baseUom;
                cmdSelect.Parameters.Add("@ConversionUOM", System.Data.SqlDbType.Int).Value = conversionUOM;
                cmdSelect.Parameters.Add("@AltQty", System.Data.SqlDbType.Decimal).Value = altQty;
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["uomGroupId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["uomGroupId"].ToString());
                    if (dateReader["altQty"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["altQty"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        //Aniket[1-7-2019]
        public List<DropDownTO> SelectAllSystemRoleListForDropDownByUserId(Int32 userId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "select idRole,roleDesc from tblRole r " +
                " where r.isActive = 1 and r.idRole IN " +
                " (select roleId from tblUserRole where userId = @UserId and isActive = 1)";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                cmdSelect.Parameters.AddWithValue("@UserId", DbType.Int32).Value = userId;
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

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
        //Priyanka [16-09-2019] : Added to get the district by text.
        public List<DropDownTO> SelectDistrictByName(string district, Int32 stateId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                String aqlQuery = "SELECT * FROM dimDistrict WHERE districtName LIKE '%" + district + "%' AND stateId=" + stateId;

                cmdSelect.CommandText = aqlQuery;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (reader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (reader["idDistrict"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(reader["idDistrict"].ToString());
                    if (reader["districtName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(reader["districtName"].ToString());
                    if (reader["stateId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(reader["stateId"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (reader != null)
                    reader.Dispose();
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        //Priyanka [16-09-2019] : Added to get the taluka by text.
        public List<DropDownTO> SelectTalukaByName(string taluka, Int32 districtId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                String aqlQuery = "SELECT * FROM dimTaluka WHERE talukaName LIKE '%" + taluka + "%' AND districtId =" + districtId;

                cmdSelect.CommandText = aqlQuery;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (reader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (reader["idTaluka"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(reader["idTaluka"].ToString());
                    if (reader["talukaName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(reader["talukaName"].ToString());

                    if (reader["districtId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(reader["districtId"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (reader != null)
                    reader.Dispose();
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Harshala[24 - 09 - 2019] Added to get Country List for state master
        /// 
        /// </summary>
        public List<DimCountryTO> SelectCountryForStateMaster()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM dimCountry where isActive=1 order by countryName asc";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimCountryTO> DimCountryTOList = new List<Models.DimCountryTO>();
                while (dateReader.Read())
                {
                    DimCountryTO DimCountryTONew = new DimCountryTO();

                    if (dateReader["idCountry"] != DBNull.Value)
                        DimCountryTONew.IdCountry = Convert.ToInt32(dateReader["idCountry"].ToString());
                    if (dateReader["countryCode"] != DBNull.Value)
                        DimCountryTONew.CountryCode = Convert.ToString(dateReader["countryCode"].ToString());

                    if (dateReader["countryName"] != DBNull.Value)
                        DimCountryTONew.CountryName = Convert.ToString(dateReader["countryName"].ToString());

                    if (dateReader["isActive"] != DBNull.Value)
                        DimCountryTONew.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());

                    if (dateReader["countryMobileCode"] != DBNull.Value)
                        DimCountryTONew.CountryMobileCode = Convert.ToInt32(dateReader["countryMobileCode"].ToString());

                    if (dateReader["mobNoDigitLength"] != DBNull.Value)
                        DimCountryTONew.MobNoDigitLength = Convert.ToInt32(dateReader["mobNoDigitLength"].ToString());

                    DimCountryTOList.Add(DimCountryTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DimCountryTOList;
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
        #region Insertion

        public int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                String sqlQuery = @" INSERT INTO [dimTaluka]( " +
                            "  [districtId]" +
                            " ,[talukaCode]" +
                            " ,[talukaName]" +
                            " )" +
                " VALUES (" +
                            "  @districtId " +
                            " ,@talukaCode " +
                            " ,@talukaName " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                String sqlSelectIdentityQry = "Select @@Identity";

                cmdInsert.Parameters.Add("@districtId", System.Data.SqlDbType.Int).Value = commonDimensionsTO.ParentId;
                cmdInsert.Parameters.Add("@talukaCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionCode);
                cmdInsert.Parameters.Add("@talukaName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionName);
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    cmdInsert.CommandText = sqlSelectIdentityQry;
                    commonDimensionsTO.IdDimension = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return 1;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                String sqlQuery = @" INSERT INTO [dimDistrict]( " +
                            "  [stateId]" +
                            " ,[districtCode]" +
                            " ,[districtName]" +
                            " )" +
                " VALUES (" +
                            "  @stateId " +
                            " ,@districtCode " +
                            " ,@districtName " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                String sqlSelectIdentityQry = "Select @@Identity";

                cmdInsert.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = commonDimensionsTO.ParentId;
                cmdInsert.Parameters.Add("@districtCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionCode);
                cmdInsert.Parameters.Add("@districtName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionName);
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    cmdInsert.CommandText = sqlSelectIdentityQry;
                    commonDimensionsTO.IdDimension = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return 1;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int InsertMstFinYear(DimFinYearTO newMstFinYearTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                String sqlQuery = @" INSERT INTO [dimFinYear]( " +
                            "  [idFinYear]" +
                            " ,[finYearDisplayName]" +
                            " ,[finYearStartDate]" +
                            " ,[finYearEndDate]" +
                            " )" +
                " VALUES (" +
                            "  @idFinYear " +
                            " ,@finYearDisplayName " +
                            " ,@finYearStartDate " +
                            " ,@finYearEndDate " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@idFinYear", System.Data.SqlDbType.Int).Value = newMstFinYearTO.IdFinYear;
                cmdInsert.Parameters.Add("@finYearDisplayName", System.Data.SqlDbType.NVarChar).Value = newMstFinYearTO.FinYearDisplayName;
                cmdInsert.Parameters.Add("@finYearStartDate", System.Data.SqlDbType.DateTime).Value = newMstFinYearTO.FinYearStartDate;
                cmdInsert.Parameters.Add("@finYearEndDate", System.Data.SqlDbType.DateTime).Value = newMstFinYearTO.FinYearEndDate;
                return cmdInsert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public List<DropDownTO> CheckManuifacturerExistsOrNot(String value, Int32 dimensionId, SqlConnection conn, SqlTransaction tran)
        {
            //String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            //SqlConnection conn = new SqlConnection(sqlConnStr);

            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                String sqlQuery = "SELECT * FROM dimGenericMaster WHERE value= @Value AND dimensionId = @DimensionId AND isActive = 1";
                //cmdSelect = new SqlCommand(sqlQuery, conn);
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Parameters.Add("@Value", System.Data.SqlDbType.NVarChar).Value = value;
                cmdSelect.Parameters.Add("@DimensionId", System.Data.SqlDbType.Int).Value = dimensionId;
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {

                    DropDownTO dropDownTONew = new DropDownTO();

                    if (dateReader["idGenericMaster"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idGenericMaster"].ToString());
                    if (dateReader["value"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["value"].ToString());
                    if (dateReader["dimensionId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["dimensionId"].ToString());
                    if (dateReader["mappedTxnId"] != DBNull.Value)
                        dropDownTONew.MappedTxnId = Convert.ToString(dateReader["mappedTxnId"].ToString());


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
                if (dateReader != null)
                {
                    dateReader.Dispose();
                }
                cmdSelect.Dispose();
            }
        }
        public int InsertManufacturer(DimGenericMasterTO dimGenericMasterTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteManufacturerInsertCommand(dimGenericMasterTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteManufacturerInsertCommand(DimGenericMasterTO dimGenericMasterTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [dimGenericMaster]( " +

                                "  [value]" +
                                " ,[isActive]" +
                                " ,[dimensionId]" +
                                " ,[mappedTxnId]" +

                                " )" +
                    " VALUES (" +

                                "  @Value " +
                                " ,@IsActive " +
                                " ,@DimensionId " +
                                " ,@MappedTxnId " +

                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdItemMake", System.Data.SqlDbType.Int).Value = dimItemMakeTO.idItemMake;
            cmdInsert.Parameters.Add("@Value", System.Data.SqlDbType.NVarChar).Value = dimGenericMasterTO.Value;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimGenericMasterTO.IsActive;
            cmdInsert.Parameters.Add("@DimensionId", System.Data.SqlDbType.Int).Value = dimGenericMasterTO.DimensionId;
            cmdInsert.Parameters.Add("@MappedTxnId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimGenericMasterTO.MappedTxnId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                dimGenericMasterTO.IdGenericMaster = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        public int UpdateManufacturer(DimGenericMasterTO dimGenericMasterTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                return ExecuteUpdateManufacturer(dimGenericMasterTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        private int ExecuteUpdateManufacturer(DimGenericMasterTO dimGenericMasterTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimGenericMaster] SET " +
                            " [value] =@Value " +
                            " ,[isActive] =@IsActive " +
                            " ,[dimensionId] =@DimensionId " +
                            " ,[mappedTxnId] =@MappedTxnId " +

                            " WHERE  [idGenericMaster] = @IdGenericMaster";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@IdGenericMaster", System.Data.SqlDbType.Int).Value = dimGenericMasterTO.IdGenericMaster;
            cmdUpdate.Parameters.Add("@Value", System.Data.SqlDbType.NVarChar).Value = dimGenericMasterTO.Value;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimGenericMasterTO.IsActive;
            cmdUpdate.Parameters.Add("@DimensionId", System.Data.SqlDbType.Int).Value = dimGenericMasterTO.DimensionId;
            cmdUpdate.Parameters.Add("@MappedTxnId", System.Data.SqlDbType.NVarChar).Value = dimGenericMasterTO.MappedTxnId;

            return cmdUpdate.ExecuteNonQuery();
        }

        //Harshala
        public int InsertDimCountry(DimCountryTO countryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(countryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        //Harshala
        public int ExecuteInsertionCommand(DimCountryTO countryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimCountry]( " +

            " [countryCode]" +
            " ,[countryName]" +
            " ,[isActive]" +

            " )" +
" VALUES (" +
            //"  @IdState " +
            " @CountryCode " +
            " ,@CountryName " +
            " ,@IsActive " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
            cmdInsert.Parameters.Add("@CountryCode", System.Data.SqlDbType.NVarChar).Value = countryTO.CountryCode;
            cmdInsert.Parameters.Add("@CountryName", System.Data.SqlDbType.NVarChar).Value = countryTO.CountryName;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 1;
            return cmdInsert.ExecuteNonQuery();
        }


        #region Insertion dimItemProdCategTO

        public DimItemProdCategTO SelectDimItemProdCateg(Int32 idItemProdCat)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = " SELECT * FROM [dimItemProdCateg] WHERE idItemProdCat = " + idItemProdCat + " ";

                conn.Open();
                SqlDataReader dimItemProdCategDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimItemProdCategTO> dimItemProdCategTOlist = ConvertDTToListCategory(dimItemProdCategDT);
                if (dimItemProdCategTOlist != null)
                {
                    return dimItemProdCategTOlist[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimItemProdCategTO> SelectDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = " SELECT * FROM [dimItemProdCateg] WHERE isActive = 1 AND itemProdCategory like '" + dimItemProdCategTO.ItemProdCategory + "'";

                conn.Open();
                SqlDataReader dimItemProdCategDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimItemProdCategTO> dimItemProdCategTOlist = ConvertDTToListCategory(dimItemProdCategDT);
                if (dimItemProdCategTOlist != null && dimItemProdCategTOlist.Count > 0)
                {
                    return dimItemProdCategTOlist;
                }
                return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimItemProdCategTO> ConvertDTToListCategory(SqlDataReader dimItemProdCategTODT)
        {
            List<DimItemProdCategTO> dimItemProdCategTOList = new List<DimItemProdCategTO>();
            if (dimItemProdCategTODT != null)
            {
                while (dimItemProdCategTODT.Read())
                {
                    DimItemProdCategTO dimItemProdCategTONew = new DimItemProdCategTO();
                    if (dimItemProdCategTODT["idItemProdCat"] != DBNull.Value)
                        dimItemProdCategTONew.IdItemProdCat = Convert.ToInt32(dimItemProdCategTODT["idItemProdCat"].ToString());
                    if (dimItemProdCategTODT["isSystem"] != DBNull.Value)
                        dimItemProdCategTONew.IsSystem = Convert.ToInt32(dimItemProdCategTODT["isSystem"].ToString());
                    if (dimItemProdCategTODT["isActive"] != DBNull.Value)
                        dimItemProdCategTONew.IsActive = Convert.ToInt32(dimItemProdCategTODT["isActive"].ToString());
                    if (dimItemProdCategTODT["isScrapProdItem"] != DBNull.Value)
                        dimItemProdCategTONew.IsScrapProdItemb = Convert.ToBoolean(dimItemProdCategTODT["isScrapProdItem"].ToString());
                    dimItemProdCategTONew.IsScrapProdItem = 0;
                    if (dimItemProdCategTONew.IsScrapProdItemb == true)
                    {
                        dimItemProdCategTONew.IsScrapProdItem = 1;
                    }
                    if (dimItemProdCategTODT["isFixedAsset"] != DBNull.Value)
                        dimItemProdCategTONew.IsFixedAssetb = Convert.ToBoolean(dimItemProdCategTODT["isFixedAsset"].ToString());
                    dimItemProdCategTONew.IsFixedAsset = 0;
                    if (dimItemProdCategTONew.IsFixedAssetb == true)
                    {
                        dimItemProdCategTONew.IsFixedAsset = 1;
                    }
                    if (dimItemProdCategTODT["itemProdCategory"] != DBNull.Value)
                        dimItemProdCategTONew.ItemProdCategory = Convert.ToString(dimItemProdCategTODT["itemProdCategory"].ToString());
                    if (dimItemProdCategTODT["itemProdCategoryDesc"] != DBNull.Value)
                        dimItemProdCategTONew.ItemProdCategoryDesc = Convert.ToString(dimItemProdCategTODT["itemProdCategoryDesc"].ToString());
                    dimItemProdCategTOList.Add(dimItemProdCategTONew);
                }
            }
            return dimItemProdCategTOList;
        }

        public int InsertDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimItemProdCategTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimItemProdCategTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(DimItemProdCategTO dimItemProdCategTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimItemProdCateg]( " +
            "  [idItemProdCat]" +
            " ,[isSystem]" +
            " ,[isActive]" +
            " ,[isScrapProdItem]" +
            " ,[isFixedAsset]" +
            " ,[itemProdCategory]" +
            " ,[itemProdCategoryDesc]" +
            " )" +
" VALUES (" +
            "  (select max(idItemProdCat)+1 from dimItemProdCateg where isActive=1) " +
            " ,@IsSystem " +
            " ,@IsActive " +
            " ,@IsScrapProdItem " +
            " ,@IsFixedAsset " +
            " ,@ItemProdCategory " +
            " ,@ItemProdCategoryDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdItemProdCat", System.Data.SqlDbType.Int).Value = dimItemProdCategTO.IdItemProdCat;
            cmdInsert.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = dimItemProdCategTO.IsSystem;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimItemProdCategTO.IsActive;
            cmdInsert.Parameters.Add("@IsScrapProdItem", System.Data.SqlDbType.Bit).Value = dimItemProdCategTO.IsScrapProdItem;
            cmdInsert.Parameters.Add("@IsFixedAsset", System.Data.SqlDbType.Bit).Value = dimItemProdCategTO.IsFixedAsset;
            cmdInsert.Parameters.Add("@ItemProdCategory", System.Data.SqlDbType.NVarChar).Value = dimItemProdCategTO.ItemProdCategory;
            cmdInsert.Parameters.Add("@ItemProdCategoryDesc", System.Data.SqlDbType.NVarChar).Value = dimItemProdCategTO.ItemProdCategoryDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #endregion

        #region Updation

        #region Updation dimItemProdCateg
        public int UpdateDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimItemProdCategTO, cmdUpdate);
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

        public int UpdateDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimItemProdCategTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(DimItemProdCategTO dimItemProdCategTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimItemProdCateg] SET " +
            "  [isSystem]= @IsSystem" +
            " ,[isActive]= @IsActive" +
            " ,[isScrapProdItem]= @IsScrapProdItem" +
            " ,[isFixedAsset]= @IsFixedAsset" +
            " ,[itemProdCategory]= @ItemProdCategory" +
            " ,[itemProdCategoryDesc] = @ItemProdCategoryDesc" +
            " WHERE 1 = 1 ";

            cmdUpdate.CommandText = sqlQuery + " AND idItemProdCat = @IdItemProdCat";
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdItemProdCat", System.Data.SqlDbType.Int).Value = dimItemProdCategTO.IdItemProdCat;
            cmdUpdate.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = dimItemProdCategTO.IsSystem;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimItemProdCategTO.IsActive;
            cmdUpdate.Parameters.Add("@IsScrapProdItem", System.Data.SqlDbType.Bit).Value = dimItemProdCategTO.IsScrapProdItem;
            cmdUpdate.Parameters.Add("@IsFixedAsset", System.Data.SqlDbType.Bit).Value = dimItemProdCategTO.IsFixedAsset;
            cmdUpdate.Parameters.Add("@ItemProdCategory", System.Data.SqlDbType.NVarChar).Value = dimItemProdCategTO.ItemProdCategory;
            cmdUpdate.Parameters.Add("@ItemProdCategoryDesc", System.Data.SqlDbType.NVarChar).Value = dimItemProdCategTO.ItemProdCategoryDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        public int UpdateDimCountry(DimCountryTO dimCountryTo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdateCommand(dimCountryTo, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }


        }
        public int ExecuteUpdateCommand(DimCountryTO dimCountryTo, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimCountry] SET " +
            //"  [idState] = @IdState" +
            " [countryName]= @conName" +
            ",[countryCode]=@conCode" +
            " WHERE [idCountry] =  @Idcon ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@conName", System.Data.SqlDbType.NVarChar).Value = dimCountryTo.CountryName;
            cmdUpdate.Parameters.Add("@Idcon", System.Data.SqlDbType.Int).Value = dimCountryTo.IdCountry;
            cmdUpdate.Parameters.Add("@conCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(dimCountryTo.CountryCode);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Execute Command

        public int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandText = cmdStr;

                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -2;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        #endregion



        public List<DropDownTO> SelectDimMasterValues(Int32 masterId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimMasterValue WHERE isActive = 1 AND masterId = " + masterId + " ORDER BY seqNo ";


                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idMasterValue"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idMasterValue"].ToString());

                    if (dateReader["masterValueName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["masterValueName"].ToString());

                    if (dateReader["sapMappedId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToInt32(dateReader["sapMappedId"].ToString());

                    if (dateReader["flag"] != DBNull.Value)
                        dropDownTONew.Flag = Convert.ToInt32(dateReader["flag"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        public List<DropDownTO> SelectAllTransTypeValues()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimTransactionType WHERE isActive = 1";


                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idTransactionType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idTransactionType"].ToString());
                    if (dateReader["transactionName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["transactionName"].ToString());
                    if (dateReader["transactionDesc"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["transactionDesc"].ToString());


                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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


        /// <summary>
        /// Priyanka [19-02-2019]
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDimMasterValuesByParentMasterValueId(Int32 parentMasterValueId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimMasterValue WHERE isActive = 1 AND parentMasterValueId = " + parentMasterValueId + " ORDER BY seqNo ";


                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idMasterValue"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idMasterValue"].ToString());
                    if (dateReader["masterValueName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["masterValueName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        //Harshala[17/09/2020] added to get GstTaxCategoryType for Item Master

        public List<DimGstTaxCategoryTypeTO> SelectDimGstTaxCategoryType()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM dimGstTaxCategoryType where isActive=1 ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimGstTaxCategoryTypeTO> DimGstTaxCategoryTypeTOList = new List<DimGstTaxCategoryTypeTO>();
                while (dateReader.Read())
                {
                    DimGstTaxCategoryTypeTO DimGstTaxCategoryTypeTONew = new DimGstTaxCategoryTypeTO();

                    if (dateReader["sequenceNo"] != DBNull.Value)
                        DimGstTaxCategoryTypeTONew.SequenceNo = Convert.ToInt32(dateReader["sequenceNo"].ToString());
                    if (dateReader["gstTaxCategoryTypeId"] != DBNull.Value)
                        DimGstTaxCategoryTypeTONew.GSTTaxCategoryTypeId = Convert.ToInt32(dateReader["gstTaxCategoryTypeId"].ToString());

                    if (dateReader["gstTaxCategoryTypeName"] != DBNull.Value)
                        DimGstTaxCategoryTypeTONew.GSTTaxCategoryTypeName = Convert.ToString(dateReader["gstTaxCategoryTypeName"].ToString());

                    if (dateReader["isActive"] != DBNull.Value)
                        DimGstTaxCategoryTypeTONew.IsActive = Convert.ToBoolean(dateReader["isActive"].ToString());

                    if (dateReader["isService"] != DBNull.Value)
                        DimGstTaxCategoryTypeTONew.IsService = Convert.ToBoolean(dateReader["isService"].ToString());


                    DimGstTaxCategoryTypeTOList.Add(DimGstTaxCategoryTypeTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DimGstTaxCategoryTypeTOList;
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



        //Added by minal 30/12/2020
        public List<DropDownTO> SelectNCOrC()
        {
            List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
            DropDownTO dropDownTONew;
            try
            {
                dropDownTONew = new DropDownTO();
                dropDownTONew.Value = 0;
                dropDownTONew.Text = "Enquiry";
                dropDownTOList.Add(dropDownTONew);

                dropDownTONew = new DropDownTO();
                dropDownTONew.Value = 1;
                dropDownTONew.Text = "Order";
                dropDownTOList.Add(dropDownTONew);

                //dropDownTONew = new DropDownTO();
                //dropDownTONew.Value = 2;
                //dropDownTONew.Text = "All";
                //dropDownTOList.Add(dropDownTONew);

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }
        //Harshala[17/09/2020] added to get GstType

        public List<DropDownTO> SelectDimGstType()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM dimGstType where isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> DimGstTypeTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO DropDownTONew = new DropDownTO();

                    if (dateReader["idDimGstType"] != DBNull.Value)
                        DropDownTONew.Value = Convert.ToInt32(dateReader["idDimGstType"].ToString());


                    if (dateReader["gstTypeName"] != DBNull.Value)
                        DropDownTONew.Text = Convert.ToString(dateReader["gstTypeName"].ToString());

                    DimGstTypeTOList.Add(DropDownTONew);

                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DimGstTypeTOList;
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

        public List<DropDownTO> SelectDimOrgGroupType(Int32 orgTypeId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM dimOrgGroupType where isActive=1 and orgTypeId= " + orgTypeId;


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> DropDownTOTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO DropDownTONew = new DropDownTO();

                    if (dateReader["idOrgGrpType"] != DBNull.Value)
                        DropDownTONew.Value = Convert.ToInt32(dateReader["idOrgGrpType"].ToString());
                    if (dateReader["orgGroupName"] != DBNull.Value)
                        DropDownTONew.Text = Convert.ToString(dateReader["orgGroupName"].ToString());

                    if (dateReader["mappedSapGroupId"] != DBNull.Value)
                        DropDownTONew.MappedTxnId = Convert.ToString(dateReader["mappedSapGroupId"].ToString());

                    if (dateReader["acctPayableFinLedgerId"] != DBNull.Value)
                        DropDownTONew.Tag = Convert.ToString(dateReader["acctPayableFinLedgerId"].ToString());

                    DropDownTOTOList.Add(DropDownTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DropDownTOTOList;
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


        //Aditee [25/09/2020] added to get BOM Type 
        public List<DropDownTO> GetBomTypeList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM dimBOMType where isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> DimBOMTypeList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO DropDownTONew = new DropDownTO();

                    if (dateReader["idBomType"] != DBNull.Value)
                        DropDownTONew.Value = Convert.ToInt32(dateReader["idBomType"].ToString());


                    if (dateReader["bomTypeName"] != DBNull.Value)
                        DropDownTONew.Text = Convert.ToString(dateReader["bomTypeName"].ToString());

                    DimBOMTypeList.Add(DropDownTONew);

                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DimBOMTypeList;
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


        public List<DropDownTO> SelectDimAssessetype()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM dimAssessetype where isActive=1 and isForVendor=1";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> DropDownTOTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO DropDownTONew = new DropDownTO();

                    if (dateReader["idAssesseeType"] != DBNull.Value)
                        DropDownTONew.Value = Convert.ToInt32(dateReader["idAssesseeType"].ToString());
                    if (dateReader["assesseeName"] != DBNull.Value)
                        DropDownTONew.Text = Convert.ToString(dateReader["assesseeName"].ToString());

                    if (dateReader["sapMapAssesseeTypeId"] != DBNull.Value)
                        DropDownTONew.MappedTxnId = Convert.ToString(dateReader["sapMapAssesseeTypeId"].ToString());

                    DropDownTOTOList.Add(DropDownTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DropDownTOTOList;
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
        //Added by minal 
        public List<DropDownTO> SelectSpotEntryOrSauda()
        {
            List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
            DropDownTO dropDownTONew;
            try
            {
                dropDownTONew = new DropDownTO();
                dropDownTONew.Value = 0;
                dropDownTONew.Text = "SpotEntry";
                dropDownTOList.Add(dropDownTONew);

                dropDownTONew = new DropDownTO();
                dropDownTONew.Value = 1;
                dropDownTONew.Text = "Sauda";
                dropDownTOList.Add(dropDownTONew);

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }

        }


        public List<TblWithHoldingTaxTO> SelectTblWithHoldingTax(int assesseeTypeId = 0)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM tblwithholdingtax where isActive=1 ";

                if (assesseeTypeId > 0)
                    sqlQuery += " and assesseeTypeId=" + assesseeTypeId;
                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWithHoldingTaxTO> DropDownTOTOList = new List<TblWithHoldingTaxTO>();
                while (dateReader.Read())
                {
                    TblWithHoldingTaxTO DropDownTONew = new TblWithHoldingTaxTO();

                    if (dateReader["idWithholdTax"] != DBNull.Value)
                        DropDownTONew.IdWithHoldTax = Convert.ToInt32(dateReader["idWithholdTax"].ToString());

                    if (dateReader["taxName"] != DBNull.Value)
                        DropDownTONew.TaxName = Convert.ToString(dateReader["taxName"].ToString());

                    if (dateReader["assesseeTypeId"] != DBNull.Value)
                        DropDownTONew.AssesseeTypeId = Convert.ToInt32(dateReader["assesseeTypeId"].ToString());

                    if (dateReader["tdsTypeId"] != DBNull.Value)
                        DropDownTONew.TdsTypeId = Convert.ToInt32(dateReader["tdsTypeId"].ToString());

                    if (dateReader["locationId"] != DBNull.Value)
                        DropDownTONew.LocationId = Convert.ToInt32(dateReader["locationId"].ToString());

                    if (dateReader["threasholdAmt"] != DBNull.Value)
                        DropDownTONew.ThreasholdAmt = Convert.ToDouble(dateReader["threasholdAmt"].ToString());

                    if (dateReader["surchargeAmt"] != DBNull.Value)
                        DropDownTONew.SurchargeAmt = Convert.ToDouble(dateReader["surchargeAmt"].ToString());

                    DropDownTOTOList.Add(DropDownTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DropDownTOTOList;
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

        public List<TblAssetClassTO> SelectTblAssetClassTOList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM tblAssetClass ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAssetClassTO> DropDownTOTOList = new List<TblAssetClassTO>();
                while (dateReader.Read())
                {
                    TblAssetClassTO DropDownTONew = new TblAssetClassTO();

                    if (dateReader["idAssetClass"] != DBNull.Value)
                        DropDownTONew.IdAssetClass = Convert.ToInt32(dateReader["idAssetClass"].ToString());

                    if (dateReader["assetName"] != DBNull.Value)
                        DropDownTONew.AssetName = Convert.ToString(dateReader["assetName"].ToString());

                    if (dateReader["assetDesc"] != DBNull.Value)
                        DropDownTONew.AssetDesc = Convert.ToString(dateReader["assetDesc"].ToString());

                    if (dateReader["assetTypeId"] != DBNull.Value)
                        DropDownTONew.AssetTypeId = Convert.ToInt32(dateReader["assetTypeId"].ToString());

                    if (dateReader["mapSapId"] != DBNull.Value)
                        DropDownTONew.MapSapId = Convert.ToString(dateReader["mapSapId"].ToString());

                    DropDownTOTOList.Add(DropDownTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DropDownTOTOList;
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

        //ReshmaP Added For Inter Transfer
        public int SaveDimGenericMasterData(DimGenericMasterTO DimGenericMasterTO)
        {
            int result = 0;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                string smdInsert = "insert into tblStoreDepartmentMachineMater (value,isActive,dimensionId,parentIdGenericMaster) " +
                    "  values ( @value,@isActive,@dimensionId,@parentIdGenericMaster )";

                cmdInsert.CommandText = smdInsert;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@value", System.Data.SqlDbType.NVarChar).Value = DimGenericMasterTO.Value;
                cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = DimGenericMasterTO.IsActive;
                cmdInsert.Parameters.Add("@dimensionId", System.Data.SqlDbType.Int).Value = DimGenericMasterTO.DimensionId;
                cmdInsert.Parameters.Add("@parentIdGenericMaster", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(DimGenericMasterTO.ParentIdGenericMaster);
                return cmdInsert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
            return result;
        }
        public int UpdateDimGenericMasterData(DimGenericMasterTO DimGenericMasterTO)
        {
            int result = 0;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                string smdInsert = "update tblStoreDepartmentMachineMater set value =@Values,isActive=@isActive,parentIdGenericMaster=@parentIdGenericMaster where idGenericMaster =@idGenericMaster";

                cmdInsert.CommandText = smdInsert;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@Values", System.Data.SqlDbType.NVarChar).Value = DimGenericMasterTO.Value;
                cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = DimGenericMasterTO.IsActive;
                cmdInsert.Parameters.Add("@parentIdGenericMaster", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(DimGenericMasterTO.ParentIdGenericMaster);
                cmdInsert.Parameters.Add("@idGenericMaster", System.Data.SqlDbType.Int).Value = DimGenericMasterTO.IdGenericMaster;
                return cmdInsert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
            return result;
        }
        public List<DimGenericMasterTO> GetGenericMasterData(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = @"select tblStoreDepartmentMachineMater.*,parent.value  as ParentName from tblStoreDepartmentMachineMater tblStoreDepartmentMachineMater
                left join tblStoreDepartmentMachineMater parent on tblStoreDepartmentMachineMater.parentIdGenericMaster=parent.idGenericMaster
                WHERE  tblStoreDepartmentMachineMater.dimensionId =  " + IdDimension;
                if (SkipIsActiveFilter == 0)
                {
                    sqlQuery += " AND tblStoreDepartmentMachineMater.isActive = 1";
                }
                if (ParentIdGenericMaster > 0)
                {
                    sqlQuery += " AND tblStoreDepartmentMachineMater.parentIdGenericMaster = " + ParentIdGenericMaster;
                }
                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimGenericMasterTO> dropDownTOList = new List<DimGenericMasterTO>();
                while (dateReader.Read())
                {
                    DimGenericMasterTO dropDownTONew = new DimGenericMasterTO();
                    if (dateReader["value"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToString(dateReader["value"].ToString());
                    if (dateReader["isActive"] != DBNull.Value)
                        dropDownTONew.IsActive = Convert.ToInt16(dateReader["isActive"].ToString());
                    if (dateReader["dimensionId"] != DBNull.Value)
                        dropDownTONew.DimensionId = Convert.ToInt16(dateReader["dimensionId"].ToString());
                    if (dateReader["idGenericMaster"] != DBNull.Value)
                        dropDownTONew.IdGenericMaster = Convert.ToInt16(dateReader["idGenericMaster"].ToString());
                    if (dateReader["parentIdGenericMaster"] != DBNull.Value)
                        dropDownTONew.ParentIdGenericMaster = Convert.ToInt16(dateReader["parentIdGenericMaster"].ToString());
                    if (dateReader["ParentName"] != DBNull.Value)
                        dropDownTONew.ParentName = Convert.ToString(dateReader["ParentName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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


        public List<DimGenericMasterTO> GetGenericMasterDataForDept(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = @"select tblStoreDepartmentMachineMater.*,locationDesc  as ParentName  ,CONCAT(tblStoreDepartmentMachineMater.value  ,'-',locationDesc)  as  DeptName from tblStoreDepartmentMachineMater tblStoreDepartmentMachineMater
                LEFT JOIN tblLocation tblLocation on tblLocation.idLocation  = tblStoreDepartmentMachineMater.parentIdGenericMaster
                WHERE  tblStoreDepartmentMachineMater.dimensionId =  " + IdDimension;
                if (SkipIsActiveFilter == 0)
                {
                    sqlQuery += " AND tblStoreDepartmentMachineMater.isActive = 1";
                }
                if (ParentIdGenericMaster > 0)
                {
                    sqlQuery += " AND tblStoreDepartmentMachineMater.parentIdGenericMaster = " + ParentIdGenericMaster;
                }
                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimGenericMasterTO> dropDownTOList = new List<DimGenericMasterTO>();
                while (dateReader.Read())
                {
                    DimGenericMasterTO dropDownTONew = new DimGenericMasterTO();
                    if (dateReader["value"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToString(dateReader["value"].ToString());
                    if (dateReader["isActive"] != DBNull.Value)
                        dropDownTONew.IsActive = Convert.ToInt16(dateReader["isActive"].ToString());
                    if (dateReader["dimensionId"] != DBNull.Value)
                        dropDownTONew.DimensionId = Convert.ToInt16(dateReader["dimensionId"].ToString());
                    if (dateReader["idGenericMaster"] != DBNull.Value)
                        dropDownTONew.IdGenericMaster = Convert.ToInt16(dateReader["idGenericMaster"].ToString());
                    if (dateReader["parentIdGenericMaster"] != DBNull.Value)
                        dropDownTONew.ParentIdGenericMaster = Convert.ToInt16(dateReader["parentIdGenericMaster"].ToString());
                    if (dateReader["ParentName"] != DBNull.Value)
                        dropDownTONew.ParentName = Convert.ToString(dateReader["ParentName"].ToString());
                    if (dateReader["DeptName"] != DBNull.Value)
                        dropDownTONew.DeptName = Convert.ToString(dateReader["DeptName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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
        List<DropDownTO> GetApprovalUserList(String idUserStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT idUser,userDisplayName FROM tblUser WHERE idUser IN(" + idUserStr + ") AND isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dropDownTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                if (dropDownTODT != null)
                {
                    while (dropDownTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (dropDownTODT["idUser"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(dropDownTODT["idUser"].ToString());
                        if (dropDownTODT["userDisplayName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(dropDownTODT["userDisplayName"].ToString());
                        list.Add(dropDownTO);
                    }
                }
                if (dropDownTODT != null)
                    dropDownTODT.Dispose();
                if (list != null)
                    return list;
                else
                    return null;
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

        public Boolean CheckAleradyAvailableData(DimGenericMasterTO DimGenericMasterTO)
        {
            Boolean isAvailable = false;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblStoreDepartmentMachineMater where parentIdGenericMaster =" + DimGenericMasterTO.ParentIdGenericMaster  +
                    " and UPPER(value)=UPPER ('" + DimGenericMasterTO.Value + "') ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    isAvailable = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
            return isAvailable;
        }

        public Boolean CheckExistingData(DimGenericMasterTO DimGenericMasterTO)
        {
            Boolean isAvailable = false;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblStoreDepartmentMachineMater  where idGenericMaster !=" + DimGenericMasterTO.IdGenericMaster +
                " AND dimensionId = " + DimGenericMasterTO.DimensionId + " and UPPER(value)=UPPER('" + DimGenericMasterTO.Value + "')";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    isAvailable = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
            return isAvailable;
        }

    }
}

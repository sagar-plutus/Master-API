using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.BL;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using StackExchange.Redis;
using System.Collections.Concurrent;
using simpliMASTERSAPI;

namespace ODLMWebAPI.DAL
{  
    public class TblPersonDAO : ITblPersonDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPersonDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT  person.*,sal.salutationDesc, orgPersonDtls.* ,OtherDesignations.name as  multiroleDesc FROM tblPerson person " +
                " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId " +
                " LEFT JOIN tblOtherDesignations OtherDesignations on OtherDesignations.idOtherDesignation=person.otherDesignationId " +
                "LEFT JOIN tblOrgPersonDtls orgPersonDtls on orgPersonDtls.personId = person.idPerson";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPersonTO> SelectAllTblPerson()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //Hrushikesh Added [13/02/2019]
        public  List<TblPersonTO> selectPersonsForOffline()
        {
            String sqlQuery = "SELECT  idPersonType,personType,person.*,orgPersonDtls.organizationId,org.orgTypeId," +
                    "  orgPersonDtls.personTypeId, sal.salutationDesc FROM tblPerson person" +
                    "  LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId  " +
                    "  LEFT JOIN tblOrgPersonDtls orgPersonDtls ON person.idPerson = orgPersonDtls.personId  " +
                    "  LEFT JOIN dimPersonType personType ON personType.idPersonType = orgPersonDtls.personTypeId " +
                    "  LEFT JOIN tblOrganization org ON orgPersonDtls.organizationId = org.idOrganization ";

                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                SqlCommand cmdSelect = new SqlCommand();
                SqlDataReader sqlReader = null;
                try
                {
                    conn.Open();
                    cmdSelect.CommandText = sqlQuery;
                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                    sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    List<TblPersonTO> list = ConvertDTToListForOffline(sqlReader);
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (sqlReader != null)
                        sqlReader.Dispose();
                    conn.Close();
                    cmdSelect.Dispose();
                }
        }

        public List<DropDownTO> selectPersonDropdownListOffline()
        {
            String sqlQuery = " SELECT personId AS idPerson , concat(firstName, ' ', lastName, ', ', roleDesc) as personName,15 as " +
                " orgTypeId FROM tblUser JOIN tblUserExt ON tblUser.idUser = tblUserExt.userId " +
                "   JOIN tblUserRole ON tblUser.idUser = tblUserRole.userId" +
                "   JOIN tblRole ON tblUserRole.roleId = tblRole.idRole" +
                "   JOIN tblPerson ON tblPerson.idPerson = tblUserExt.personId" +
                " WHERE tblUserRole.isActive = 1 AND tblUser.isActive = 1" +
                " UNION ALL " +
                                     " SELECT DISTINCT person.idPerson ," +
                                     " CASE " +
                                     " WHEN organization.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization.firmName" +
                                     " WHEN organization3.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization3.firmName" +
                                     " WHEN organization.firmName IS NULL THEN person.firstName + ' ' + person.lastName" +
                                   " END " +
                                    " AS personName," +
                                    " CASE " +
                                    " WHEN organization3.orgTypeId IS NOT NULL THEN organization3.orgTypeId " +
                                    "  WHEN organization.orgTypeId IS NOT NULL THEN organization.orgTypeId " +
                                    " END " +
                                    "AS orgTypeId " +
                                    " FROM tblPerson person " +
                                    " LEFT JOIN tblOrganization organization ON organization.firstOwnerPersonId = person.idPerson OR organization.secondOwnerPersonId = person.idPerson " +
                                    " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                                    " LEFT JOIN tblOrganization organization3 ON orgPersonDtls.organizationId = organization3.idOrganization ";

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = ConvertDTToListDropdownForOffline(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Aniket [13-03-2019]
        public List<TblPersonTOEmail> SelectblOrganizationForEmail(Int32 organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select org.idOrganization,org.firmName,org.emailAddr from tblOrganization org where idOrganization=" + organizationId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTOEmail> list = ConvertDTToListForEmail1(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblPersonTOEmail> SelectAllTblPersonOrganizationForEmail(Int32 organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " select  person.idPerson,person.primaryEmail,person.alternateEmail,person.salutationId from tblOrganization org "+
                    " LEFT JOIN tblperson person on person.idPerson in(select firstOwnerPersonId from tblOrganization where idOrganization = @idOrg) "+
                    " or person.idPerson in(select secondOwnerPersonId from tblOrganization where idOrganization = @idOrg) "+
                    " where idOrganization = " + organizationId;
                cmdSelect.Connection = conn;
                cmdSelect.Parameters.AddWithValue("@idOrg", DbType.Int32).Value = organizationId;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTOEmail> list = ConvertDTToListForEmail2(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblPersonTO> SelectAllTblPersonByOrganization(Int32 organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                //cmdSelect.CommandText = " SELECT org.cdStructureId,org.delPeriodId,org.emailAddr as EmailAddr,tblRole.roleDesc as  multiroleDesc, person.*,sal.salutationDesc,orgPersonDtls.personTypeId,userExt.userId ,users.userLogin as userLoginName,users.isActive as userIsActive FROM tblPerson person " +
                //    " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId " +
                //    " LEFT JOIN tblOrganization org on org.firstOwnerPersonId=person.idPerson and org.isActive=1" +
                //    " LEFT JOIN tblOtherDesignations OtherDesignations on OtherDesignations.idOtherDesignation=person.otherDesignationId " +
                //    " LEFT JOIN tblOrgPersonDtls orgPersonDtls on orgPersonDtls.personId =person.idPerson and orgPersonDtls.isActive=1" + // Aditee changed join.. >>LEFT JOIN tblOrgPersonDtls orgPersonDtls on orgPersonDtls.organizationId = org.idOrganization
                //    " Left join tblUserExt userExt on userExt.personId=person.idPerson" +
                //    " Left join tblUser users on users.idUser= userExt.userId " +
                //    " Left join tblUserRole tblUserRole on tblUserRole.userId= userExt.userId " +
                //    " Left join tblRole tblRole on tblRole.idRole= tblUserRole.roleId " +
                //    " WHERE idPerson IN(SELECT firstOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") " +
                //    " OR idPerson IN(SELECT secondOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ")";


                cmdSelect.CommandText = " SELECT org.cdStructureId,org.delPeriodId,org.emailAddr as EmailAddr,tblRole.roleDesc as  multiroleDesc, person.*,sal.salutationDesc,orgPersonDtls.personTypeId,userExt.userId ,users.userLogin as userLoginName,users.isActive as userIsActive FROM tblPerson person " +
                                    " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId " +
                                    " LEFT JOIN tblOrganization org on org.firstOwnerPersonId=person.idPerson and org.isActive=1" +
                " LEFT JOIN tblOtherDesignations OtherDesignations on OtherDesignations.idOtherDesignation=person.otherDesignationId " +
                " LEFT JOIN(SELECT *, ROW_NUMBER() OVER(PARTITION BY personId ORDER BY createdOn desc) RowNumber FROM tblOrgPersonDtls) orgPersonDtls on orgPersonDtls.personId = person.idPerson and orgPersonDtls.isActive = 1 " +
                                    " Left join tblUserExt userExt on userExt.personId=person.idPerson" +
                " Left join tblUser users on users.idUser= userExt.userId " +
                " Left join tblUserRole tblUserRole on tblUserRole.userId= userExt.userId " +
                " Left join tblRole tblRole on tblRole.idRole= tblUserRole.roleId " +
                " WHERE (idPerson IN(SELECT firstOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") " +
                                    " OR idPerson IN(SELECT secondOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ")) and orgPersonDtls.RowNumber = 1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertPrsnByOrgDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPersonTO> SelectMultipleTblPersonByOrganization(Int32 organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT org.emailAddr as EmailAddr, tblRole.roleDesc as  multiroleDesc, person.*,orgPerson.personTypeId as prsnTypeId ,sal.salutationDesc,userExt.userId,users.userLogin as userLoginName,users.isActive as userIsActive FROM tblPerson person " +
                    " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId " +
                    " LEFT JOIN tblOrgPersonDtls orgPerson ON orgPerson.personId = person.idPerson " +
                    " LEFT JOIN tblOrganization org on org.idOrganization = person.idPerson " +
                    " LEFT JOIN tblOtherDesignations OtherDesignations on OtherDesignations.idOtherDesignation=person.otherDesignationId " +
                    " Left join tblUserExt userExt on userExt.personId=person.idPerson " +
                    " Left join tblUser users on users.idUser= userExt.userId " +
                    " Left join tblUserRole tblUserRole on tblUserRole.userId= userExt.userId " +
                    " Left join tblRole tblRole on tblRole.idRole= tblUserRole.roleId " +
                    " WHERE orgPerson.organizationId = " + organizationId + " ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertprsnMulipleToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetUserIdFromOrgIdDetails(Int32 organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " select userExt.userId from tblOrganization ORG "
                                        +" INNER JOIN tblUserExt userExt ON userExt.personId = ORG.firstOwnerPersonId "
                                        +" WHERE ORG.idOrganization = " + organizationId + " ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> list = new List<DropDownTO>();
                while (sqlReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (sqlReader["userId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(sqlReader["userId"].ToString());
                   
                    list.Add(dropDownTONew);
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public int GetPersonIdOnUserId(int userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblUserExt where userId=" + userId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                TblPersonTO tblPersonTONew = new TblPersonTO();
                while (sqlReader.Read())
                {
                    if (sqlReader["personId"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(sqlReader["personId"].ToString());
                }
                return tblPersonTONew.IdPerson;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPersonTO> SelectAllTblPersonByRoleType(Int32 roleTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                string sqlQuery = SqlSelectQuery()+
                    " left join tblUserExt ext on ext.personId = person.idPerson " +
                    " left join tblUserRole userRole on ext.userId = userRole.userId" +
                    " left join tblRole rle on rle.idRole = userRole.roleId where rle.roleTypeId= " + roleTypeId;
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //public List<TblPersonTO> SelectAllPersonByOrganizations(Int32 organizationId, Int32 personTypeId)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    SqlDataReader sqlReader = null;
        //    String whereCond = String.Empty;
        //    try
        //    {
        //        if (personTypeId > 0)
        //            whereCond = " AND personTypeId=" + personTypeId;
        //        else
        //            whereCond = String.Empty;
        //        conn.Open();
        //        cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPerson IN(SELECT firstOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") " +
        //                                " OR idPerson IN(SELECT secondOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ")" +
        //                                " OR idPerson IN(SELECT personId FROM tblOrgPersonDtls WHERE organizationId = " + organizationId +
        //                                whereCond +")";

        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblPersonTO> list = ConvertDTToList(sqlReader);
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (sqlReader != null)
        //            sqlReader.Dispose();
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        public List<TblPersonTO> SelectAllPersonByOrganizations(Int32 organizationId, Int32 personTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String whereCond = String.Empty;
            try
            {
                if (personTypeId > 0)
                    whereCond = "  ) " + "AND personTypeId = " + personTypeId;
                //"OR idPerson IN (SELECT personId FROM tblCRMPersonLinking WHERE PersonTypeId="+personTypeId+") ";
                else
                    whereCond = ")";
                String strPersonType = String.Empty;
                if (personTypeId == 1 || personTypeId == 0)
                {
                    strPersonType = " OR idPerson IN(SELECT firstOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") " +
                        " OR idPerson IN(SELECT secondOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") ";
                }
                conn.Open();
                cmdSelect.CommandText = " SELECT " +
                    //"  CASE WHEN CRMpersonType.idPersonType IS NULL THEN CRMpersonType.personType ELSE CRMpersonType.idPersonType END AS idPersonType, " +
                    //"  CASE WHEN CRMpersonType.personType IS NULL THEN personType.personType ELSE CRMpersonType.personType END AS personType "+
                    " idPersonType,personType,person.*,sal.salutationDesc,tblUserExt.userId FROM tblPerson person  " +
                    " LEFT JOIN tblUserExt tblUserExt ON tblUserExt.personId = person.idPerson" +
                    " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId  " +
                    " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON person.idPerson = orgPersonDtls.personId " +
                    " LEFT JOIN dimPersonType personType ON personType.idPersonType = orgPersonDtls.personTypeId " +
                    //" LEFT JOIN tblCRMPersonLinking CRMLinking ON CRMLinking.personId=person.idPerson " +
                    //" LEFT JOIN dimPersonType CRMpersonType ON CRMpersonType.idPersonType = CRMLinking.personTypeId "+
                    " WHERE " +
                    " idPerson IN (SELECT personId FROM tblOrgPersonDtls where organizationId=" + organizationId +
                    whereCond + " " + strPersonType;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToListByOrg(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblPersonTO SelectTblPerson(Int32 idPerson)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPerson = " + idPerson + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count > 0)
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        /// <summary>
        /// Priyanka [19-08-2019]
        /// </summary>
        /// <param name="idPerson"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public TblPersonTO SelectTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPerson = " + idPerson + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblPerson");
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }



        /// <summary>
        /// Sudhir[21-JUNE-2018] 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDimPersonTypesDropdownList(int isDesignation=0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                if(isDesignation == 1)
                {
                    cmdSelect.CommandText = " SELECT personType as Text , idPersonType as Value from dimPersonType where isDesignation="+ isDesignation;
                }
                else
                cmdSelect.CommandText = " SELECT personType as Text , idPersonType as Value from dimPersonType ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["Value"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["Value"].ToString());
                    if (dateReader["Text"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["Text"].ToString());
                    
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        /// <summary>
        /// Sudhir[21-JUNE-2018] 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDropDownListOnPersonId(int personId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT OrgpersonDtls.organizationId,Organization.firmName,OrgpersonDtls.personTypeId FROM tblPerson person " +
                    " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId " +
                    " LEFT JOIN tblOrgpersonDtls OrgpersonDtls ON person.idPErson = OrgpersonDtls.personId " +
                    " LEFT JOIN tblOrganization Organization ON OrgpersonDtls.organizationId = Organization.idOrganization " +
                    " WHERE idPerson = " + personId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["organizationId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["organizationId"].ToString());
                    if (dateReader["firmName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["firmName"].ToString());
                    if (dateReader["personTypeId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["personTypeId"].ToString());

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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        /// <summary>
        /// Sudhir[23-APR-2018] Added for Get Persons Based on Orgaization Type.
        /// </summary>
        /// <param name="tblPersonTODT"></param>
        /// <returns></returns>
        /// 
        public List<DropDownTO> SelectPersonsBasedOnOrgType(Int32 OrgType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {

                //person.idPerson,person.firstName+''+person.lastName  AS personName
                conn.Open();

                if (OrgType == (int)Constants.OrgTypeE.USERS)
                {

                    #region
                    //cmdSelect.CommandText = "SELECT personId AS idPerson,concat(firstName,' ',lastName,', ',roleDesc) as personName,tblUser.idUser FROM tblUser " +
                    //                        " JOIN tblUserExt ON tblUser.idUser = tblUserExt.userId " +
                    //                        " JOIN tblUserRole ON tblUser.idUser = tblUserRole.userId  " +
                    //                        " JOIN tblRole ON tblUserRole.roleId = tblRole.idRole " +
                    //                        " JOIN tblPerson ON tblPerson.idPerson = tblUserExt.personId ";
                    #endregion

                    cmdSelect.CommandText = "SELECT personId AS idPerson,concat(firstName,' ',lastName,', ',roleDesc) as personName,tblUser.idUser  FROM tblUser " +
                        " JOIN tblUserExt ON tblUser.idUser = tblUserExt.userId " +
                        " JOIN tblUserRole ON tblUser.idUser = tblUserRole.userId  " +
                        " JOIN tblRole ON tblUserRole.roleId = tblRole.idRole " +
                        " JOIN tblPerson ON tblPerson.idPerson = tblUserExt.personId " +
                        " WHERE tblUserRole.isActive=1 AND tblUser.isActive=1";
                }
                else
                {
                    #region Old Code Commented on 25-July-2018 By Sudhir
                    /*cmdSelect.CommandText = " SELECT person.idPerson ," +
                                        " CASE " +
                                        " WHEN organization.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization.firmName " +
                                        " WHEN organization2.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization2.firmName " +
                                        " WHEN organization3.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization3.firmName " +
                                        " WHEN organization.firmName IS NULL AND organization2.firmName IS NULL THEN person.firstName + ' ' + person.lastName " +
                                        " END " +
                                        " AS personName FROM tblPerson person " +
                                        " LEFT JOIN tblOrganization organization ON organization.firstOwnerPersonId = person.idPerson " +
                                        " LEFT JOIN tblOrganization organization2 ON organization2.secondOwnerPersonId = person.idPerson " +
                                        " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                                        " LEFT JOIN tblOrganization organization3 ON orgPersonDtls.organizationId = organization3.idOrganization " +
                                        " WHERE orgPersonDtls.organizationId IN(SELECT idOrganization FROM tblOrganization WHERE orgTypeId = " + OrgType + ") " +
                                        " OR organization.orgTypeId =" + OrgType + " OR organization2.orgTypeId = " + OrgType; */
                    #endregion

                    cmdSelect.CommandText = " SELECT DISTINCT person.idPerson ," +
                        " CASE " +
                        " WHEN organization.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization.firmName " +
                        " WHEN organization3.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization3.firmName " +
                        " WHEN organization.firmName IS NULL THEN person.firstName + ' ' + person.lastName " +
                        " END " +
                        " AS personName,NULL as idUser  FROM tblOrganization organization" +
                        " INNER JOIN tblPerson person   ON organization.firstOwnerPersonId = person.idPerson OR organization.secondOwnerPersonId = person.idPerson " +
                        " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                        " LEFT JOIN tblOrganization organization3 ON orgPersonDtls.organizationId = organization3.idOrganization " +
                        " WHERE orgPersonDtls.organizationId IN(SELECT idOrganization FROM tblOrganization WHERE orgTypeId = " + OrgType + " and isActive=1)  OR (organization.orgTypeId = " + OrgType + " AND organization.isActive=1)";

                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (sqlReader.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (sqlReader["idPerson"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(sqlReader["idPerson"].ToString());
                    if (sqlReader["personName"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(sqlReader["personName"].ToString());
                    if (sqlReader["idUser"] != DBNull.Value)
                        dropDownTO.Tag = Convert.ToInt32(sqlReader["idUser"].ToString());
                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> ConvertDTToListDropdownForOffline(SqlDataReader tblPersonTODT)
        {
            //hrushikesh
            //adding uniqueness to personid as there are same person for diffrent orgType
            List<DropDownTO> tblPersonTOList = new List<DropDownTO>();
            if (tblPersonTODT != null)
            {
                while (tblPersonTODT.Read())
                {
                    DropDownTO tblPersonTONew = new DropDownTO();
                    if ((tblPersonTODT["idPerson"] != DBNull.Value) && (tblPersonTODT["orgTypeId"] != DBNull.Value))
                        tblPersonTONew.Value = Convert.ToInt32(tblPersonTODT["idPerson"].ToString() + (tblPersonTODT["orgTypeId"].ToString()));
                    else if ((tblPersonTODT["idPerson"] != DBNull.Value))
                        tblPersonTONew.Value = Convert.ToInt32(tblPersonTODT["idPerson"].ToString());
                    if (tblPersonTODT["personName"] != DBNull.Value)
                        tblPersonTONew.Text = Convert.ToString(tblPersonTODT["personName"].ToString());
                    if (tblPersonTODT["orgTypeId"] != DBNull.Value)
                        tblPersonTONew.Tag = Convert.ToString(tblPersonTODT["orgTypeId"].ToString());
                    tblPersonTOList.Add(tblPersonTONew);
                }
            }
            return tblPersonTOList;
        }


        public  List<TblPersonTO> ConvertDTToListForOffline(SqlDataReader tblPersonTODT)
        {
            List<TblPersonTO> tblPersonTOList = new List<TblPersonTO>();
            if (tblPersonTODT != null)
            {
                while (tblPersonTODT.Read())
                {
                    TblPersonTO tblPersonTONew = new TblPersonTO();
                    if (tblPersonTODT["idPerson"] != DBNull.Value && tblPersonTODT["personTypeId"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(tblPersonTODT["idPerson"].ToString() + Convert.ToInt32(tblPersonTODT["personTypeId"].ToString()));
                    else if (tblPersonTODT["idPerson"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(tblPersonTODT["idPerson"].ToString());
                    if (tblPersonTODT["salutationId"] != DBNull.Value)
                        tblPersonTONew.SalutationId = Convert.ToInt32(tblPersonTODT["salutationId"].ToString());
                    if (tblPersonTODT["mobileNo"] != DBNull.Value)
                        tblPersonTONew.MobileNo = Convert.ToString(tblPersonTODT["mobileNo"].ToString());
                    if (tblPersonTODT["alternateMobNo"] != DBNull.Value)
                        tblPersonTONew.AlternateMobNo = Convert.ToString(tblPersonTODT["alternateMobNo"].ToString());
                    if (tblPersonTODT["phoneNo"] != DBNull.Value)
                        tblPersonTONew.PhoneNo = Convert.ToString(tblPersonTODT["phoneNo"].ToString());
                    if (tblPersonTODT["createdBy"] != DBNull.Value)
                        tblPersonTONew.CreatedBy = Convert.ToInt32(tblPersonTODT["createdBy"].ToString());
                    if (tblPersonTODT["dateOfBirth"] != DBNull.Value)
                        tblPersonTONew.DateOfBirth = Convert.ToDateTime(tblPersonTODT["dateOfBirth"].ToString());
                    if (tblPersonTODT["createdOn"] != DBNull.Value)
                        tblPersonTONew.CreatedOn = Convert.ToDateTime(tblPersonTODT["createdOn"].ToString());
                    if (tblPersonTODT["firstName"] != DBNull.Value)
                        tblPersonTONew.FirstName = Convert.ToString(tblPersonTODT["firstName"].ToString());
                    if (tblPersonTODT["midName"] != DBNull.Value)
                        tblPersonTONew.MidName = Convert.ToString(tblPersonTODT["midName"].ToString());
                    if (tblPersonTODT["lastName"] != DBNull.Value)
                        tblPersonTONew.LastName = Convert.ToString(tblPersonTODT["lastName"].ToString());
                    if (tblPersonTODT["primaryEmail"] != DBNull.Value)
                        tblPersonTONew.PrimaryEmail = Convert.ToString(tblPersonTODT["primaryEmail"].ToString());
                    if (tblPersonTODT["alternateEmail"] != DBNull.Value)
                        tblPersonTONew.AlternateEmail = Convert.ToString(tblPersonTODT["alternateEmail"].ToString());
                    if (tblPersonTODT["comments"] != DBNull.Value)
                        tblPersonTONew.Comments = Convert.ToString(tblPersonTODT["comments"].ToString());
                    if (tblPersonTODT["salutationDesc"] != DBNull.Value)
                        tblPersonTONew.SalutationName = Convert.ToString(tblPersonTODT["salutationDesc"].ToString());
                    if (tblPersonTODT["dateOfAnniversary"] != DBNull.Value)
                        tblPersonTONew.DateOfAnniversary = Convert.ToDateTime(tblPersonTODT["dateOfAnniversary"].ToString());
                    if (tblPersonTODT["otherDesignationId"] != DBNull.Value)
                        tblPersonTONew.OtherDesignationId = Convert.ToInt32(tblPersonTODT["otherDesignationId"].ToString());
                    if (tblPersonTODT["organizationId"] != DBNull.Value)
                        tblPersonTONew.OrganizationId = Convert.ToInt32(tblPersonTODT["organizationId"].ToString());
                    if (tblPersonTODT["personTypeId"] != DBNull.Value)
                        tblPersonTONew.PersonTypeId = Convert.ToInt32(tblPersonTODT["personTypeId"].ToString());
                    if (tblPersonTODT["orgTypeId"] != DBNull.Value)
                        tblPersonTONew.OrgTypeId = Convert.ToInt32(tblPersonTODT["orgTypeId"].ToString());

                    if (tblPersonTODT["commonAttachId"] != DBNull.Value)
                        tblPersonTONew.CommonAttachId = Convert.ToString(tblPersonTODT["commonAttachId"].ToString());

                    if (tblPersonTODT["mobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.MobNoCntryCode = Convert.ToString(tblPersonTODT["mobNoCntryCode"].ToString());

                    if (tblPersonTODT["altMobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.AltMobNoCntryCode = Convert.ToString(tblPersonTODT["altMobNoCntryCode"].ToString());

                    if (tblPersonTONew.DateOfBirth != DateTime.MinValue)
                    {
                        tblPersonTONew.DobDay = tblPersonTONew.DateOfBirth.Day;
                        tblPersonTONew.DobMonth = tblPersonTONew.DateOfBirth.Month;
                        tblPersonTONew.DobYear = tblPersonTONew.DateOfBirth.Year;

                    }
                    tblPersonTOList.Add(tblPersonTONew);
                }
            }
            return tblPersonTOList;
        }

        //Aniket
        public List<TblPersonTOEmail> ConvertDTToListForEmail2(SqlDataReader tblPersonToEmail)
        {
            List<TblPersonTOEmail> tblPersonToList = new List<TblPersonTOEmail>();
            if (tblPersonToEmail != null)
            {
                while (tblPersonToEmail.Read())
                {
                    TblPersonTOEmail tblperson = new TblPersonTOEmail();
                   if (tblPersonToEmail["idPerson"] != DBNull.Value)
                        tblperson.IdPerson = Convert.ToInt32(tblPersonToEmail["idPerson"].ToString());
                    if (tblPersonToEmail["primaryEmail"] != DBNull.Value)
                        tblperson.PrimaryEmail = tblPersonToEmail["primaryEmail"].ToString();
                    if (tblPersonToEmail["alternateEmail"] != DBNull.Value)
                        tblperson.AlternateEmail = tblPersonToEmail["alternateEmail"].ToString();
                    if (tblPersonToEmail["salutationId"] != DBNull.Value)
                        tblperson.SalutationId = Convert.ToInt32(tblPersonToEmail["salutationId"].ToString());

                    tblPersonToList.Add(tblperson);
                }
            }
            return tblPersonToList;
        }
        public List<TblPersonTOEmail> ConvertDTToListForEmail1(SqlDataReader tblPersonToEmail)
        {
            List<TblPersonTOEmail> tblPersonToList = new List<TblPersonTOEmail>();
            if (tblPersonToEmail != null)
            {
                while (tblPersonToEmail.Read())
                {
                    TblPersonTOEmail tblperson = new TblPersonTOEmail();
                    if (tblPersonToEmail["idOrganization"] != DBNull.Value)
                        tblperson.IdOrganization = Convert.ToInt32(tblPersonToEmail["idOrganization"].ToString());
                    if (tblPersonToEmail["firmName"] != DBNull.Value)
                        tblperson.FirmName = tblPersonToEmail["firmName"].ToString();
                    if (tblPersonToEmail["emailAddr"] != DBNull.Value)
                        tblperson.EmailAddr = tblPersonToEmail["emailAddr"].ToString();
                  tblPersonToList.Add(tblperson);
                }
            }
            return tblPersonToList;
        }
        public List<TblPersonTO> ConvertDTToList(SqlDataReader tblPersonTODT)
              {
            List<TblPersonTO> tblPersonTOList = new List<TblPersonTO>();
            if (tblPersonTODT != null)
            {
                while (tblPersonTODT.Read())
                {

                    TblPersonTO tblPersonTONew = new TblPersonTO();
                    if (tblPersonTODT["emailAddr"] != DBNull.Value)
                        tblPersonTONew.EmailAddr = Convert.ToString(tblPersonTODT["emailAddr"].ToString());
                    if (tblPersonTODT["idPerson"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(tblPersonTODT["idPerson"].ToString());
                    if (tblPersonTODT["salutationId"] != DBNull.Value)
                        tblPersonTONew.SalutationId = Convert.ToInt32(tblPersonTODT["salutationId"].ToString());
                    if (tblPersonTODT["mobileNo"] != DBNull.Value)
                        tblPersonTONew.MobileNo = Convert.ToString(tblPersonTODT["mobileNo"].ToString());
                    if (tblPersonTODT["alternateMobNo"] != DBNull.Value)
                        tblPersonTONew.AlternateMobNo = Convert.ToString(tblPersonTODT["alternateMobNo"].ToString());
                    if (tblPersonTODT["phoneNo"] != DBNull.Value)
                        tblPersonTONew.PhoneNo = Convert.ToString(tblPersonTODT["phoneNo"].ToString());
                    if (tblPersonTODT["createdBy"] != DBNull.Value)
                        tblPersonTONew.CreatedBy = Convert.ToInt32(tblPersonTODT["createdBy"].ToString());
                    if (tblPersonTODT["dateOfBirth"] != DBNull.Value)
                        tblPersonTONew.DateOfBirth = Convert.ToDateTime(tblPersonTODT["dateOfBirth"].ToString());
                    if (tblPersonTODT["createdOn"] != DBNull.Value)
                        tblPersonTONew.CreatedOn = Convert.ToDateTime(tblPersonTODT["createdOn"].ToString());
                    if (tblPersonTODT["firstName"] != DBNull.Value)
                        tblPersonTONew.FirstName = Convert.ToString(tblPersonTODT["firstName"].ToString());
                    if (tblPersonTODT["midName"] != DBNull.Value)
                        tblPersonTONew.MidName = Convert.ToString(tblPersonTODT["midName"].ToString());
                    if (tblPersonTODT["lastName"] != DBNull.Value)
                        tblPersonTONew.LastName = Convert.ToString(tblPersonTODT["lastName"].ToString());
                    if (tblPersonTODT["primaryEmail"] != DBNull.Value)
                        tblPersonTONew.PrimaryEmail = Convert.ToString(tblPersonTODT["primaryEmail"].ToString());
                    if (tblPersonTODT["alternateEmail"] != DBNull.Value)
                        tblPersonTONew.AlternateEmail = Convert.ToString(tblPersonTODT["alternateEmail"].ToString());
                    if (tblPersonTODT["comments"] != DBNull.Value)
                        tblPersonTONew.Comments = Convert.ToString(tblPersonTODT["comments"].ToString());
                    if (tblPersonTODT["salutationDesc"] != DBNull.Value)
                        tblPersonTONew.SalutationName = Convert.ToString(tblPersonTODT["salutationDesc"].ToString());
                    if (tblPersonTODT["photoBase64"] != DBNull.Value)
                        tblPersonTONew.PhotoBase64 = Convert.ToString(tblPersonTODT["photoBase64"].ToString());
                    if (tblPersonTODT["dateOfAnniversary"] != DBNull.Value)
                        tblPersonTONew.DateOfAnniversary = Convert.ToDateTime(tblPersonTODT["dateOfAnniversary"].ToString());
                    if (tblPersonTODT["otherDesignationId"] != DBNull.Value)
                        tblPersonTONew.OtherDesignationId = Convert.ToInt32(tblPersonTODT["otherDesignationId"].ToString());
                    if (tblPersonTODT["multiroleDesc"] != DBNull.Value)
                        tblPersonTONew.MultiroleDesc = Convert.ToString(tblPersonTODT["multiroleDesc"].ToString());
                    if (tblPersonTODT["commonAttachId"] != DBNull.Value)
                        tblPersonTONew.CommonAttachId = Convert.ToString(tblPersonTODT["commonAttachId"].ToString());
                    if (tblPersonTODT["mobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.MobNoCntryCode = Convert.ToString(tblPersonTODT["mobNoCntryCode"].ToString());
                    if (tblPersonTODT["altMobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.AltMobNoCntryCode = Convert.ToString(tblPersonTODT["altMobNoCntryCode"].ToString());
                    if (tblPersonTODT["personTypeId"] != DBNull.Value)
                        tblPersonTONew.PersonTypeId = Convert.ToInt32(tblPersonTODT["personTypeId"].ToString());
                    if (tblPersonTONew.DateOfBirth != DateTime.MinValue)
                    {
                        tblPersonTONew.DobDay = tblPersonTONew.DateOfBirth.Day;
                        tblPersonTONew.DobMonth = tblPersonTONew.DateOfBirth.Month;
                        tblPersonTONew.DobYear = tblPersonTONew.DateOfBirth.Year;

                    }

                    //if (tblPersonTODT["delPeriodId"] != DBNull.Value)
                    //    tblPersonTONew.DelPeriodId = Convert.ToInt32(tblPersonTODT["delPeriodId"].ToString());

                    //if (tblPersonTODT["cdStructureId"] != DBNull.Value)
                    //    tblPersonTONew.CdStructureId = Convert.ToInt32(tblPersonTODT["cdStructureId"].ToString());

                    tblPersonTOList.Add(tblPersonTONew);
                }
            }
            return tblPersonTOList;
        }
        public List<TblPersonTO> ConvertPrsnByOrgDTToList(SqlDataReader tblPersonTODT)
        {
            List<TblPersonTO> tblPersonTOList = new List<TblPersonTO>();
            if (tblPersonTODT != null)
            {
                while (tblPersonTODT.Read())
                {

                    TblPersonTO tblPersonTONew = new TblPersonTO();
                    if (tblPersonTODT["emailAddr"] != DBNull.Value)
                        tblPersonTONew.EmailAddr = Convert.ToString(tblPersonTODT["emailAddr"].ToString());
                    if (tblPersonTODT["idPerson"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(tblPersonTODT["idPerson"].ToString());
                    if (tblPersonTODT["salutationId"] != DBNull.Value)
                        tblPersonTONew.SalutationId = Convert.ToInt32(tblPersonTODT["salutationId"].ToString());
                    if (tblPersonTODT["mobileNo"] != DBNull.Value)
                        tblPersonTONew.MobileNo = Convert.ToString(tblPersonTODT["mobileNo"].ToString());
                    if (tblPersonTODT["alternateMobNo"] != DBNull.Value)
                        tblPersonTONew.AlternateMobNo = Convert.ToString(tblPersonTODT["alternateMobNo"].ToString());
                    if (tblPersonTODT["phoneNo"] != DBNull.Value)
                        tblPersonTONew.PhoneNo = Convert.ToString(tblPersonTODT["phoneNo"].ToString());
                    if (tblPersonTODT["createdBy"] != DBNull.Value)
                        tblPersonTONew.CreatedBy = Convert.ToInt32(tblPersonTODT["createdBy"].ToString());
                    if (tblPersonTODT["dateOfBirth"] != DBNull.Value)
                        tblPersonTONew.DateOfBirth = Convert.ToDateTime(tblPersonTODT["dateOfBirth"].ToString());
                    if (tblPersonTODT["createdOn"] != DBNull.Value)
                        tblPersonTONew.CreatedOn = Convert.ToDateTime(tblPersonTODT["createdOn"].ToString());
                    if (tblPersonTODT["firstName"] != DBNull.Value)
                        tblPersonTONew.FirstName = Convert.ToString(tblPersonTODT["firstName"].ToString());
                    if (tblPersonTODT["midName"] != DBNull.Value)
                        tblPersonTONew.MidName = Convert.ToString(tblPersonTODT["midName"].ToString());
                    if (tblPersonTODT["lastName"] != DBNull.Value)
                        tblPersonTONew.LastName = Convert.ToString(tblPersonTODT["lastName"].ToString());
                    if (tblPersonTODT["primaryEmail"] != DBNull.Value)
                        tblPersonTONew.PrimaryEmail = Convert.ToString(tblPersonTODT["primaryEmail"].ToString());
                    if (tblPersonTODT["alternateEmail"] != DBNull.Value)
                        tblPersonTONew.AlternateEmail = Convert.ToString(tblPersonTODT["alternateEmail"].ToString());
                    if (tblPersonTODT["comments"] != DBNull.Value)
                        tblPersonTONew.Comments = Convert.ToString(tblPersonTODT["comments"].ToString());
                    if (tblPersonTODT["salutationDesc"] != DBNull.Value)
                        tblPersonTONew.SalutationName = Convert.ToString(tblPersonTODT["salutationDesc"].ToString());
                    if (tblPersonTODT["photoBase64"] != DBNull.Value)
                        tblPersonTONew.PhotoBase64 = Convert.ToString(tblPersonTODT["photoBase64"].ToString());
                    if (tblPersonTODT["dateOfAnniversary"] != DBNull.Value)
                        tblPersonTONew.DateOfAnniversary = Convert.ToDateTime(tblPersonTODT["dateOfAnniversary"].ToString());
                    if (tblPersonTODT["otherDesignationId"] != DBNull.Value)
                        tblPersonTONew.OtherDesignationId = Convert.ToInt32(tblPersonTODT["otherDesignationId"].ToString());
                    if (tblPersonTODT["multiroleDesc"] != DBNull.Value)
                        tblPersonTONew.MultiroleDesc = Convert.ToString(tblPersonTODT["multiroleDesc"].ToString());
                    if (tblPersonTODT["commonAttachId"] != DBNull.Value)
                        tblPersonTONew.CommonAttachId = Convert.ToString(tblPersonTODT["commonAttachId"].ToString());
                    if (tblPersonTODT["mobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.MobNoCntryCode = Convert.ToString(tblPersonTODT["mobNoCntryCode"].ToString());
                    if (tblPersonTODT["altMobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.AltMobNoCntryCode = Convert.ToString(tblPersonTODT["altMobNoCntryCode"].ToString());
                    if (tblPersonTODT["personTypeId"] != DBNull.Value)
                        tblPersonTONew.PersonTypeId = Convert.ToInt32(tblPersonTODT["personTypeId"].ToString());
                    if (tblPersonTONew.DateOfBirth != DateTime.MinValue)
                    {
                        tblPersonTONew.DobDay = tblPersonTONew.DateOfBirth.Day;
                        tblPersonTONew.DobMonth = tblPersonTONew.DateOfBirth.Month;
                        tblPersonTONew.DobYear = tblPersonTONew.DateOfBirth.Year;

                    }

                    if (tblPersonTODT["delPeriodId"] != DBNull.Value)
                        tblPersonTONew.DelPeriodId = Convert.ToInt32(tblPersonTODT["delPeriodId"].ToString());
                    if (tblPersonTODT["userLoginName"] != DBNull.Value)
                        tblPersonTONew.UserLoginName = Convert.ToString(tblPersonTODT["userLoginName"].ToString());
                    if (tblPersonTODT["userId"] != DBNull.Value)
                    {
                        tblPersonTONew.IsUserCreate = 1;
                        tblPersonTONew.UserId = Convert.ToInt32(tblPersonTODT["userId"].ToString());

                    }
                    if (tblPersonTODT["userIsActive"] != DBNull.Value)
                        tblPersonTONew.UserIsActive = Convert.ToInt32(tblPersonTODT["userIsActive"].ToString());
                    tblPersonTOList.Add(tblPersonTONew);
                }
            }
            return tblPersonTOList;
        }


        private List<TblPersonTO> ConvertprsnMulipleToList(SqlDataReader tblPersonTODT)
        {
            List<TblPersonTO> tblPersonTOList = new List<TblPersonTO>();
            if (tblPersonTODT != null)
            {
                while (tblPersonTODT.Read())
                {

                    TblPersonTO tblPersonTONew = new TblPersonTO();
                    if (tblPersonTODT["emailAddr"] != DBNull.Value)
                        tblPersonTONew.EmailAddr = Convert.ToString(tblPersonTODT["emailAddr"].ToString());
                    if (tblPersonTODT["idPerson"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(tblPersonTODT["idPerson"].ToString());
                    if (tblPersonTODT["salutationId"] != DBNull.Value)
                        tblPersonTONew.SalutationId = Convert.ToInt32(tblPersonTODT["salutationId"].ToString());
                    if (tblPersonTODT["mobileNo"] != DBNull.Value)
                        tblPersonTONew.MobileNo = Convert.ToString(tblPersonTODT["mobileNo"].ToString());
                    if (tblPersonTODT["alternateMobNo"] != DBNull.Value)
                        tblPersonTONew.AlternateMobNo = Convert.ToString(tblPersonTODT["alternateMobNo"].ToString());
                    if (tblPersonTODT["phoneNo"] != DBNull.Value)
                        tblPersonTONew.PhoneNo = Convert.ToString(tblPersonTODT["phoneNo"].ToString());
                    if (tblPersonTODT["createdBy"] != DBNull.Value)
                        tblPersonTONew.CreatedBy = Convert.ToInt32(tblPersonTODT["createdBy"].ToString());
                    if (tblPersonTODT["dateOfBirth"] != DBNull.Value)
                        tblPersonTONew.DateOfBirth = Convert.ToDateTime(tblPersonTODT["dateOfBirth"].ToString());
                    if (tblPersonTODT["createdOn"] != DBNull.Value)
                        tblPersonTONew.CreatedOn = Convert.ToDateTime(tblPersonTODT["createdOn"].ToString());
                    if (tblPersonTODT["firstName"] != DBNull.Value)
                        tblPersonTONew.FirstName = Convert.ToString(tblPersonTODT["firstName"].ToString());
                    if (tblPersonTODT["midName"] != DBNull.Value)
                        tblPersonTONew.MidName = Convert.ToString(tblPersonTODT["midName"].ToString());
                    if (tblPersonTODT["lastName"] != DBNull.Value)
                        tblPersonTONew.LastName = Convert.ToString(tblPersonTODT["lastName"].ToString());
                    if (tblPersonTODT["primaryEmail"] != DBNull.Value)
                        tblPersonTONew.PrimaryEmail = Convert.ToString(tblPersonTODT["primaryEmail"].ToString());
                    if (tblPersonTODT["alternateEmail"] != DBNull.Value)
                        tblPersonTONew.AlternateEmail = Convert.ToString(tblPersonTODT["alternateEmail"].ToString());
                    if (tblPersonTODT["comments"] != DBNull.Value)
                        tblPersonTONew.Comments = Convert.ToString(tblPersonTODT["comments"].ToString());
                    if (tblPersonTODT["salutationDesc"] != DBNull.Value)
                        tblPersonTONew.SalutationName = Convert.ToString(tblPersonTODT["salutationDesc"].ToString());
                    if (tblPersonTODT["photoBase64"] != DBNull.Value)
                        tblPersonTONew.PhotoBase64 = Convert.ToString(tblPersonTODT["photoBase64"].ToString());
                    if (tblPersonTODT["dateOfAnniversary"] != DBNull.Value)
                        tblPersonTONew.DateOfAnniversary = Convert.ToDateTime(tblPersonTODT["dateOfAnniversary"].ToString());
                    if (tblPersonTODT["otherDesignationId"] != DBNull.Value)
                        tblPersonTONew.OtherDesignationId = Convert.ToInt32(tblPersonTODT["otherDesignationId"].ToString());
                    if (tblPersonTODT["prsnTypeId"] != DBNull.Value)
                        tblPersonTONew.PersonTypeId = Convert.ToInt32(tblPersonTODT["prsnTypeId"].ToString());
                    if (tblPersonTODT["multiroleDesc"] != DBNull.Value)
                        tblPersonTONew.MultiroleDesc = Convert.ToString(tblPersonTODT["multiroleDesc"].ToString());

                    if (tblPersonTODT["commonAttachId"] != DBNull.Value)
                        tblPersonTONew.CommonAttachId = Convert.ToString(tblPersonTODT["commonAttachId"].ToString());
                    if (tblPersonTODT["mobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.MobNoCntryCode = Convert.ToString(tblPersonTODT["mobNoCntryCode"].ToString());
                    if (tblPersonTODT["altMobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.AltMobNoCntryCode = Convert.ToString(tblPersonTODT["altMobNoCntryCode"].ToString());
                    if (tblPersonTODT["userLoginName"] != DBNull.Value)
                        tblPersonTONew.UserLoginName = Convert.ToString(tblPersonTODT["userLoginName"].ToString());
                    if (tblPersonTODT["userId"] != DBNull.Value)
                    {
                        tblPersonTONew.IsUserCreate = 1;
                        tblPersonTONew.UserId = Convert.ToInt32(tblPersonTODT["userId"].ToString());

                    }
                    if (tblPersonTODT["userIsActive"] != DBNull.Value)
                        tblPersonTONew.UserIsActive = Convert.ToInt32(tblPersonTODT["userIsActive"].ToString());
                    
                    if (tblPersonTONew.DateOfBirth != DateTime.MinValue)
                    {
                        tblPersonTONew.DobDay = tblPersonTONew.DateOfBirth.Day;
                        tblPersonTONew.DobMonth = tblPersonTONew.DateOfBirth.Month;
                        tblPersonTONew.DobYear = tblPersonTONew.DateOfBirth.Year;

                    }
                    tblPersonTOList.Add(tblPersonTONew);
                }
            }
            return tblPersonTOList;
        }

        public List<TblPersonTO>ConvertDTToListByOrg(SqlDataReader tblPersonTODT)
        {
            List<TblPersonTO> tblPersonTOList = new List<TblPersonTO>();
            if (tblPersonTODT != null)
            {
                while (tblPersonTODT.Read())
                {

                    TblPersonTO tblPersonTONew = new TblPersonTO();
                    if (tblPersonTODT["idPerson"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(tblPersonTODT["idPerson"].ToString());
                   //Deepali added to get user id against person Task no 1143
                    if (tblPersonTODT["userId"] != DBNull.Value)
                        tblPersonTONew.UserId = Convert.ToInt32(tblPersonTODT["userId"].ToString());
                    if (tblPersonTODT["salutationId"] != DBNull.Value)
                        tblPersonTONew.SalutationId = Convert.ToInt32(tblPersonTODT["salutationId"].ToString());
                    if (tblPersonTODT["mobileNo"] != DBNull.Value)
                        tblPersonTONew.MobileNo = Convert.ToString(tblPersonTODT["mobileNo"].ToString());
                    if (tblPersonTODT["alternateMobNo"] != DBNull.Value)
                        tblPersonTONew.AlternateMobNo = Convert.ToString(tblPersonTODT["alternateMobNo"].ToString());
                    if (tblPersonTODT["phoneNo"] != DBNull.Value)
                        tblPersonTONew.PhoneNo = Convert.ToString(tblPersonTODT["phoneNo"].ToString());
                    if (tblPersonTODT["createdBy"] != DBNull.Value)
                        tblPersonTONew.CreatedBy = Convert.ToInt32(tblPersonTODT["createdBy"].ToString());
                    if (tblPersonTODT["dateOfBirth"] != DBNull.Value)
                        tblPersonTONew.DateOfBirth = Convert.ToDateTime(tblPersonTODT["dateOfBirth"].ToString());
                    if (tblPersonTODT["createdOn"] != DBNull.Value)
                        tblPersonTONew.CreatedOn = Convert.ToDateTime(tblPersonTODT["createdOn"].ToString());
                    if (tblPersonTODT["firstName"] != DBNull.Value)
                        tblPersonTONew.FirstName = Convert.ToString(tblPersonTODT["firstName"].ToString());
                    if (tblPersonTODT["midName"] != DBNull.Value)
                        tblPersonTONew.MidName = Convert.ToString(tblPersonTODT["midName"].ToString());
                    if (tblPersonTODT["lastName"] != DBNull.Value)
                        tblPersonTONew.LastName = Convert.ToString(tblPersonTODT["lastName"].ToString());
                    if (tblPersonTODT["primaryEmail"] != DBNull.Value)
                        tblPersonTONew.PrimaryEmail = Convert.ToString(tblPersonTODT["primaryEmail"].ToString());
                    if (tblPersonTODT["alternateEmail"] != DBNull.Value)
                        tblPersonTONew.AlternateEmail = Convert.ToString(tblPersonTODT["alternateEmail"].ToString());
                    if (tblPersonTODT["comments"] != DBNull.Value)
                        tblPersonTONew.Comments = Convert.ToString(tblPersonTODT["comments"].ToString());
                    if (tblPersonTODT["salutationDesc"] != DBNull.Value)
                        tblPersonTONew.SalutationName = Convert.ToString(tblPersonTODT["salutationDesc"].ToString());
                    if (tblPersonTODT["photoBase64"] != DBNull.Value)
                        tblPersonTONew.PhotoBase64 = Convert.ToString(tblPersonTODT["photoBase64"].ToString());
                    if (tblPersonTODT["dateOfAnniversary"] != DBNull.Value)
                        tblPersonTONew.DateOfAnniversary = Convert.ToDateTime(tblPersonTODT["dateOfAnniversary"].ToString());
                    if (tblPersonTODT["otherDesignationId"] != DBNull.Value)
                        tblPersonTONew.OtherDesignationId = Convert.ToInt32(tblPersonTODT["otherDesignationId"].ToString());
                    if (tblPersonTODT["mobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.MobNoCntryCode = Convert.ToString(tblPersonTODT["mobNoCntryCode"].ToString());
                    if (tblPersonTODT["altMobNoCntryCode"] != DBNull.Value)
                        tblPersonTONew.AltMobNoCntryCode = Convert.ToString(tblPersonTODT["altMobNoCntryCode"].ToString());
                    if (tblPersonTODT["commonAttachId"] != DBNull.Value)
                        tblPersonTONew.CommonAttachId = Convert.ToString(tblPersonTODT["commonAttachId"].ToString());
                    if (tblPersonTONew.DateOfBirth != DateTime.MinValue)
                    {
                        tblPersonTONew.DobDay = tblPersonTONew.DateOfBirth.Day;
                        tblPersonTONew.DobMonth = tblPersonTONew.DateOfBirth.Month;
                        tblPersonTONew.DobYear = tblPersonTONew.DateOfBirth.Year;

                    }
                    tblPersonTOList.Add(tblPersonTONew);
                }
            }
            return tblPersonTOList;
        }

        #endregion


        public List<TblPersonTO> SelectAllTblPersonByOrganizationId(Int32 organizationId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                String sqlSelectQry = " SELECT person.*,sal.salutationDesc FROM tblPerson person " +
                                 " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId ";

                cmdSelect.CommandText = sqlSelectQry + " WHERE idPerson IN(SELECT firstOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") " +
                                        " OR idPerson IN(SELECT secondOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ")";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #region Insertion
        public int InsertTblPerson(TblPersonTO tblPersonTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPersonTO, cmdInsert);
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

        public int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPersonTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPersonTO tblPersonTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPerson]( " +
                "  [salutationId]" +
                " ,[mobileNo]" +
                " ,[alternateMobNo]" +
                " ,[phoneNo]" +
                " ,[createdBy]" +
                " ,[dateOfBirth]" +
                " ,[createdOn]" +
                " ,[firstName]" +
                " ,[midName]" +
                " ,[lastName]" +
                " ,[primaryEmail]" +
                " ,[alternateEmail]" +
                " ,[comments]" +
                " ,[photoBase64]" +
                " ,[dateOfAnniversary]" +
                " ,[otherDesignationId]" +
                " ,[commonAttachId]" +
                " ,[mobNoCntryCode]" +
                " ,[altMobNoCntryCode]" +
                " )" +
                " VALUES (" +
                "  @SalutationId " +
                " ,@MobileNo " +
                " ,@AlternateMobNo " +
                " ,@PhoneNo " +
                " ,@CreatedBy " +
                " ,@DateOfBirth " +
                " ,@CreatedOn " +
                " ,@FirstName " +
                " ,@MidName " +
                " ,@LastName " +
                " ,@PrimaryEmail " +
                " ,@AlternateEmail " +
                " ,@Comments " +
                " ,@photoBase64 " +
                " ,@dateOfAnniversary" +
                " ,@OtherDesignationId" +
                " ,@CommonAttachId" +
                " ,@MobNoCntryCode " +
                " ,@AltMobNoCntryCode " +
                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPerson", System.Data.SqlDbType.Int).Value = tblPersonTO.IdPerson;
            cmdInsert.Parameters.Add("@SalutationId", System.Data.SqlDbType.Int).Value = tblPersonTO.SalutationId;
            cmdInsert.Parameters.Add("@MobileNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MobileNo);
            cmdInsert.Parameters.Add("@AlternateMobNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateMobNo);
            cmdInsert.Parameters.Add("@PhoneNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhoneNo);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPersonTO.CreatedBy;
            cmdInsert.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.DateOfBirth);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.CreatedOn);
            cmdInsert.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.FirstName);
            cmdInsert.Parameters.Add("@MidName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MidName);
            cmdInsert.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.LastName);
            cmdInsert.Parameters.Add("@PrimaryEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PrimaryEmail);
            cmdInsert.Parameters.Add("@AlternateEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateEmail);
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.Comments);
            cmdInsert.Parameters.Add("@photoBase64", System.Data.SqlDbType.NText).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhotoBase64);
            cmdInsert.Parameters.Add("@dateOfAnniversary", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.DateOfAnniversary);
            cmdInsert.Parameters.Add("@OtherDesignationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.OtherDesignationId);
            cmdInsert.Parameters.Add("@CommonAttachId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.CommonAttachId);
            cmdInsert.Parameters.Add("@MobNoCntryCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MobNoCntryCode);
            cmdInsert.Parameters.Add("@AltMobNoCntryCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AltMobNoCntryCode);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblPersonTO.IdPerson = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblPerson(TblPersonTO tblPersonTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPersonTO, cmdUpdate);
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

        public int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPersonTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPersonTO tblPersonTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPerson] SET " +
                "  [salutationId]= @SalutationId" +
                " ,[mobileNo]= @MobileNo" +
                " ,[alternateMobNo]= @AlternateMobNo" +
                " ,[phoneNo]= @PhoneNo" +
                " ,[dateOfBirth]= @DateOfBirth" +
                " ,[firstName]= @FirstName" +
                " ,[midName]= @MidName" +
                " ,[lastName]= @LastName" +
                " ,[primaryEmail]= @PrimaryEmail" +
                " ,[alternateEmail]= @AlternateEmail" +
                " ,[comments] = @Comments" +
                " ,[photoBase64] = @photoBase64" +
                " ,[dateOfAnniversary]=@dateOfAnniversary" +
                " ,[otherDesignationId]=@OtherDesignationId" +
                " ,[commonAttachId]=@CommonAttachId" +
                 " ,[altMobNoCntryCode]= @AltMobNoCntryCode" +
                  " ,[mobNoCntryCode]= @MobNoCntryCode" +
                " WHERE  [idPerson] = @IdPerson";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPerson", System.Data.SqlDbType.Int).Value = tblPersonTO.IdPerson;
            cmdUpdate.Parameters.Add("@SalutationId", System.Data.SqlDbType.Int).Value = tblPersonTO.SalutationId;
            cmdUpdate.Parameters.Add("@MobileNo", System.Data.SqlDbType.NVarChar, 20).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MobileNo);
            cmdUpdate.Parameters.Add("@AlternateMobNo", System.Data.SqlDbType.NVarChar, 20).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateMobNo);
            cmdUpdate.Parameters.Add("@PhoneNo", System.Data.SqlDbType.NVarChar, 20).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhoneNo);
            cmdUpdate.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.DateOfBirth);
            cmdUpdate.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = tblPersonTO.FirstName;
            cmdUpdate.Parameters.Add("@MidName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MidName);
            cmdUpdate.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.LastName);
            cmdUpdate.Parameters.Add("@PrimaryEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PrimaryEmail);
            cmdUpdate.Parameters.Add("@AlternateEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateEmail);
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.Comments);
            cmdUpdate.Parameters.Add("@photoBase64", System.Data.SqlDbType.NText).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhotoBase64);
            cmdUpdate.Parameters.Add("@dateOfAnniversary", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.DateOfAnniversary);
            cmdUpdate.Parameters.Add("@OtherDesignationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.OtherDesignationId);
            cmdUpdate.Parameters.Add("@CommonAttachId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.CommonAttachId);
            cmdUpdate.Parameters.Add("@AltMobNoCntryCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AltMobNoCntryCode);
            cmdUpdate.Parameters.Add("@MobNoCntryCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MobNoCntryCode);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPerson(Int32 idPerson)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPerson, cmdDelete);
            }
            catch (Exception ex)
            {

                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPerson, cmdDelete);
            }
            catch (Exception ex)
            {

                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idPerson, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPerson] " +
                " WHERE idPerson = " + idPerson + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPerson", System.Data.SqlDbType.Int).Value = tblPersonTO.IdPerson;
            return cmdDelete.ExecuteNonQuery();
        }

        /// Birthdays or anniversory notifications - Tejaswini
        public List<BirthdayAlertTO> SelectAllTblPersonByBirthdayAnniversory(DateTime Today, Int32 upcomingDays, Int32 IsBirthday)
        {
            string BirthdaysCondition = null;
            if (upcomingDays == 0)
            {
                if (IsBirthday == 1)
                {
                    BirthdaysCondition = " DATEPART(d , Person.dateOfBirth) = " + Today.Day + " AND DATEPART(m , Person.dateOfBirth) = " + Today.Month;
                }
                else if (IsBirthday == 0)
                {
                    BirthdaysCondition = " DATEPART(d , Person.dateOfAnniversary) = " + Today.Day + " AND DATEPART(m , Person.dateOfAnniversary) = " + Today.Month;
                }
                else
                {
                    BirthdaysCondition = " DATEPART(d , Person.dateOfBirth) = " + Today.Day + " AND DATEPART(m , Person.dateOfBirth) = " + Today.Month +
                        " OR DATEPART(d, Person.dateOfAnniversary) =  " + Today.Day + "  AND DATEPART(m, Person.dateOfAnniversary) = " + Today.Month;
                }
            }
            else
            {
                // BirthdaysCondition = " Person.dateOfBirth between convert(datetime,'" + fromDate + "',105) AND convert(datetime,'" + toDate +
                //     "',105) OR Person.dateOfAnniversary between convert(datetime,'" + fromDate + "',105) AND convert(datetime,'" + toDate + "',105)";

                if (IsBirthday == 1)
                {
                    BirthdaysCondition = " 1 = (FLOOR(DATEDIFF(dd, Person.dateOfBirth,GETDATE()+ " + upcomingDays + ") / 365.25))- (FLOOR(DATEDIFF(dd, Person.dateOfBirth,GETDATE()) / 365.25)) ";
                    //BirthdaysCondition = "Person.dateOfBirth IS NOT NULL AND MONTH(Person.dateOfBirth)="+Today.Month + " AND DAY(Person.dateOfBirth) BETWEEN 9 AND 13";

                }
                else if (IsBirthday == 0)
                {
                    BirthdaysCondition = " 1 = (FLOOR(DATEDIFF(dd, Person.dateOfAnniversary,GETDATE()+ " + upcomingDays + ") / 365.25))- (FLOOR(DATEDIFF(dd, Person.dateOfAnniversary,GETDATE()) / 365.25)) ";

                }
                else
                {
                    BirthdaysCondition = " 1 = (FLOOR(DATEDIFF(dd, Person.dateOfBirth,GETDATE()+ " + upcomingDays + ") / 365.25))- (FLOOR(DATEDIFF(dd, Person.dateOfBirth,GETDATE()) / 365.25)) " +
                                    " OR 1 = (FLOOR(DATEDIFF(dd, Person.dateOfAnniversary,GETDATE()+ " + upcomingDays + ") / 365.25))- (FLOOR(DATEDIFF(dd, Person.dateOfAnniversary,GETDATE()) / 365.25))";

                }
            }
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                //cmdSelect.CommandText = "SELECT Person.*, Salutation.salutationDesc,RoleDtls.idRole, RoleDtls.roleDesc, OrgPersonDtls.organizationId , Organization.* ,PersonType.idPersonType, PersonType.personType FROM tblPerson Person " +
                //    "LEFT JOIN tblUserExt UserExt ON   UserExt.personId = Person.idPerson " +
                //    " LEFT JOIN tblUser sysUser ON   UserExt.userId = sysUser.idUser AND sysUser.isActive=1 " +
                //    "LEFT JOIN tbluserrole Userrole ON   Userrole.userId = UserExt.userId " +
                //    "LEFT JOIN tblrole RoleDtls ON   RoleDtls.idRole = Userrole.roleId " +
                //    "LEFT JOIN tblOrgPersonDtls OrgPersonDtls ON OrgPersonDtls.personId = Person.idPerson AND OrgPersonDtls.isActive=1" +
                //    "LEFT JOIN tblOrganization Organization ON  Organization.idOrganization = OrgPersonDtls.organizationId AND Organization.isActive=1" +
                //    "LEFT JOIN dimPersonType PersonType ON PersonType.idPersonType = OrgPersonDtls.personTypeId " +
                //    "LEFT JOIN dimSalutation Salutation ON Salutation.idSalutation = Person.salutationId " +
                //    " WHERE " +
                cmdSelect.CommandText = "select person.*, Salutation.salutationDesc,RoleDtls.idRole, RoleDtls.roleDesc,allOrg.* ,PersonType.idPersonType,PersonType.personType  from tblPerson person " +
              "  LEFT JOIN (select ext.personId,users.* from  tblUser users left join tblUserext ext ON ext.userId=users.idUser and users.isActive=1) allUser on allUser.personId = person.idPerson" +
              " LEFT JOIN (select org.*, OrgPersonDtls.personId,OrgPersonDtls.organizationId,OrgPersonDtls.personTypeId  from tblOrgPersonDtls OrgPersonDtls left join tblOrganization org on org.idOrganization= OrgPersonDtls.organizationId) allOrg on allOrg.personId = person.idPerson " +
              " LEFT JOIN tbluserrole Userrole ON   Userrole.userId = allUser.idUser  " +
              "LEFT JOIN tblrole RoleDtls ON   RoleDtls.idRole = Userrole.roleId " +
              "LEFT JOIN dimPersonType PersonType ON PersonType.idPersonType = allOrg.personTypeId " +
              "LEFT JOIN dimSalutation Salutation ON Salutation.idSalutation = Person.salutationId" +
              " WHERE  allOrg.isActive = 1 and " +
              BirthdaysCondition +
              " UNION ALL " +
              "select person.*, Salutation.salutationDesc,RoleDtls.idRole, RoleDtls.roleDesc,allOrg.* ,PersonType.idPersonType,PersonType.personType  from tblPerson person " +
              "  LEFT JOIN (select ext.personId,users.* from  tblUser users left join tblUserext ext ON ext.userId=users.idUser and users.isActive=1) allUser on allUser.personId = person.idPerson" +
              " LEFT JOIN (select org.*, OrgPersonDtls.personId,OrgPersonDtls.organizationId,OrgPersonDtls.personTypeId  from tblOrgPersonDtls OrgPersonDtls left join tblOrganization org on org.idOrganization= OrgPersonDtls.organizationId) allOrg on allOrg.personId = person.idPerson " +
              " LEFT JOIN tbluserrole Userrole ON   Userrole.userId = allUser.idUser  " +
              "LEFT JOIN tblrole RoleDtls ON   RoleDtls.idRole = Userrole.roleId " +
              "LEFT JOIN dimPersonType PersonType ON PersonType.idPersonType = allOrg.personTypeId " +
              "LEFT JOIN dimSalutation Salutation ON Salutation.idSalutation = Person.salutationId" +
              " WHERE  allUser.isActive = 1 and " +
              BirthdaysCondition;
              ; // + " AND Organization.isActive = 1 AND OrgPersonDtls.isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<BirthdayAlertTO> list = ConvertBirthdayDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Convert to BirthdayAlertTO list - Tejaswini
        public List<BirthdayAlertTO> ConvertBirthdayDTToList(SqlDataReader tblBirthdayPersonTODT)
        {
            List<BirthdayAlertTO> tblPersonTOList = new List<BirthdayAlertTO>();
            if (tblBirthdayPersonTODT != null)
            {
                while (tblBirthdayPersonTODT.Read())
                {
                    BirthdayAlertTO tblBirthdayTONew = new BirthdayAlertTO();
                    if (tblBirthdayPersonTODT["idPerson"] != DBNull.Value)
                        tblBirthdayTONew.IdPerson = Convert.ToInt32(tblBirthdayPersonTODT["idPerson"].ToString());
                    if (tblBirthdayPersonTODT["salutationId"] != DBNull.Value)
                        tblBirthdayTONew.SalutationId = Convert.ToInt32(tblBirthdayPersonTODT["salutationId"].ToString());
                    if (tblBirthdayPersonTODT["mobileNo"] != DBNull.Value)
                        tblBirthdayTONew.MobileNo = Convert.ToString(tblBirthdayPersonTODT["mobileNo"].ToString());
                    if (tblBirthdayPersonTODT["alternateMobNo"] != DBNull.Value)
                        tblBirthdayTONew.AlternateMobNo = Convert.ToString(tblBirthdayPersonTODT["alternateMobNo"].ToString());
                    if (tblBirthdayPersonTODT["phoneNo"] != DBNull.Value)
                        tblBirthdayTONew.PhoneNo = Convert.ToString(tblBirthdayPersonTODT["phoneNo"].ToString());
                    if (tblBirthdayPersonTODT["createdBy"] != DBNull.Value)
                        tblBirthdayTONew.CreatedBy = Convert.ToInt32(tblBirthdayPersonTODT["createdBy"].ToString());
                    if (tblBirthdayPersonTODT["dateOfBirth"] != DBNull.Value)
                        tblBirthdayTONew.DateOfBirth = Convert.ToDateTime(tblBirthdayPersonTODT["dateOfBirth"].ToString());
                    if (tblBirthdayPersonTODT["createdOn"] != DBNull.Value)
                        tblBirthdayTONew.CreatedOn = Convert.ToDateTime(tblBirthdayPersonTODT["createdOn"].ToString());
                    if (tblBirthdayPersonTODT["firstName"] != DBNull.Value)
                        tblBirthdayTONew.FirstName = Convert.ToString(tblBirthdayPersonTODT["firstName"].ToString());
                    if (tblBirthdayPersonTODT["midName"] != DBNull.Value)
                        tblBirthdayTONew.MidName = Convert.ToString(tblBirthdayPersonTODT["midName"].ToString());
                    if (tblBirthdayPersonTODT["lastName"] != DBNull.Value)
                        tblBirthdayTONew.LastName = Convert.ToString(tblBirthdayPersonTODT["lastName"].ToString());
                    if (tblBirthdayPersonTODT["primaryEmail"] != DBNull.Value)
                        tblBirthdayTONew.PrimaryEmail = Convert.ToString(tblBirthdayPersonTODT["primaryEmail"].ToString());
                    if (tblBirthdayPersonTODT["alternateEmail"] != DBNull.Value)
                        tblBirthdayTONew.AlternateEmail = Convert.ToString(tblBirthdayPersonTODT["alternateEmail"].ToString());
                    if (tblBirthdayPersonTODT["comments"] != DBNull.Value)
                        tblBirthdayTONew.Comments = Convert.ToString(tblBirthdayPersonTODT["comments"].ToString());
                    if (tblBirthdayPersonTODT["salutationDesc"] != DBNull.Value)
                        tblBirthdayTONew.SalutationName = Convert.ToString(tblBirthdayPersonTODT["salutationDesc"].ToString());
                    if (tblBirthdayPersonTODT["photoBase64"] != DBNull.Value)
                        tblBirthdayTONew.PhotoBase64 = Convert.ToString(tblBirthdayPersonTODT["photoBase64"].ToString());
                    if (tblBirthdayPersonTODT["dateOfAnniversary"] != DBNull.Value)
                        tblBirthdayTONew.DateOfAnniversary = Convert.ToDateTime(tblBirthdayPersonTODT["dateOfAnniversary"].ToString());
                    if (tblBirthdayPersonTODT["otherDesignationId"] != DBNull.Value)
                        tblBirthdayTONew.OtherDesignationId = Convert.ToInt32(tblBirthdayPersonTODT["otherDesignationId"].ToString());
                    if (tblBirthdayPersonTODT["commonAttachId"] != DBNull.Value)
                        tblBirthdayTONew.CommonAttachId = Convert.ToString(tblBirthdayPersonTODT["commonAttachId"].ToString());
                    if (tblBirthdayPersonTODT["mobNoCntryCode"] != DBNull.Value)
                        tblBirthdayTONew.MobNoCntryCode = Convert.ToString(tblBirthdayPersonTODT["mobNoCntryCode"].ToString());
                    if (tblBirthdayPersonTODT["altMobNoCntryCode"] != DBNull.Value)
                        tblBirthdayTONew.AltMobNoCntryCode = Convert.ToString(tblBirthdayPersonTODT["altMobNoCntryCode"].ToString());
                    if (tblBirthdayTONew.DateOfBirth != DateTime.MinValue)
                    {
                        tblBirthdayTONew.DobDay = tblBirthdayTONew.DateOfBirth.Day;
                        tblBirthdayTONew.DobMonth = tblBirthdayTONew.DateOfBirth.Month;
                        tblBirthdayTONew.DobYear = tblBirthdayTONew.DateOfBirth.Year;
                    }
                    if (tblBirthdayTONew.DateOfAnniversary != DateTime.MinValue)
                    {
                        tblBirthdayTONew.AnniDay = tblBirthdayTONew.DateOfAnniversary.Day;
                        tblBirthdayTONew.AnniMonth = tblBirthdayTONew.DateOfAnniversary.Month;
                        tblBirthdayTONew.AnniYear = tblBirthdayTONew.DateOfAnniversary.Year;
                    }
                    if (tblBirthdayPersonTODT["idOrganization"] != DBNull.Value)
                        tblBirthdayTONew.IdOrganization = Convert.ToInt32(tblBirthdayPersonTODT["idOrganization"].ToString());

                    if (tblBirthdayPersonTODT["firmName"] != DBNull.Value)
                        tblBirthdayTONew.FirmName = Convert.ToString(tblBirthdayPersonTODT["firmName"].ToString());
                    if (tblBirthdayPersonTODT["website"] != DBNull.Value)
                        tblBirthdayTONew.Website = Convert.ToString(tblBirthdayPersonTODT["website"].ToString());
                    if (tblBirthdayPersonTODT["personType"] != DBNull.Value)
                        tblBirthdayTONew.PersonType = Convert.ToString(tblBirthdayPersonTODT["personType"].ToString());
                    if (tblBirthdayPersonTODT["idPersonType"] != DBNull.Value)
                        tblBirthdayTONew.IdPersonType = Convert.ToInt32(tblBirthdayPersonTODT["idPersonType"].ToString());
                    if (tblBirthdayPersonTODT["idRole"] != DBNull.Value)
                        tblBirthdayTONew.IdRole = Convert.ToInt32(tblBirthdayPersonTODT["idRole"].ToString());
                    if (tblBirthdayPersonTODT["roleDesc"] != DBNull.Value)
                        tblBirthdayTONew.RoleDesc = Convert.ToString(tblBirthdayPersonTODT["roleDesc"].ToString());

                    tblPersonTOList.Add(tblBirthdayTONew);
                }
            }
            return tblPersonTOList;
        }
        #endregion

    }
}
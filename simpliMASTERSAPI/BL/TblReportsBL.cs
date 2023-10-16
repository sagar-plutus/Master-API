using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using simpliMASTERSAPI.Models;
using System.Reflection;
using System.Dynamic;

namespace ODLMWebAPI.BL
{
    public class TblReportsBL : ITblReportsBL
    {
        private readonly ITblReportsDAO _iTblReportsDAO;
        private readonly ITblFilterReportBL _iTblFilterReportBL;
        private readonly ITblOrgStructureBL _iTblOrgStructureBL;
        private readonly ITblUserBL _itblUserBL;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        private readonly ITblLoginBL _iTblLoginBL;
        public TblReportsBL(ICommon iCommon, IConnectionString iConnectionString, ITblReportsDAO iTblReportsDAO, ITblFilterReportBL iTblFilterReportBL, ITblOrgStructureBL iTblOrgStructureBL,ITblUserBL itblUserBL, ITblLoginBL iTblLoginBL)
        {
            _iTblReportsDAO = iTblReportsDAO;
            _iTblFilterReportBL = iTblFilterReportBL;
            _iTblOrgStructureBL = iTblOrgStructureBL;
            _itblUserBL = itblUserBL;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
            _iTblLoginBL = iTblLoginBL;
        }

        #region Selection
        //public List<TblReportsTO> SelectAllTblReports()
        //{
        //    return _iTblReportsDAO.SelectAllTblReports();
        //}
        public List<TblReportsTO> SelectAllModuleWiseReport(Int32 moduleId, Int32 transId,Int32 loginUserId)
        {
            List<TblReportsTO> tblReportsTOReturnList = new List<TblReportsTO>();
            List<TblReportsTO> tblReportsTOList = _iTblReportsDAO.SelectAllModuleWiseReport(moduleId,transId);
            Parallel.ForEach(tblReportsTOList, element =>
            {
                element.SqlQuery = null;
            }
            );

            TblUserTO tblUserTO = _iTblLoginBL.getPermissionsOnModule(loginUserId, 0);
            tblReportsTOReturnList = tblReportsTOList;

            if (tblReportsTOList != null && tblReportsTOList.Count > 0 && tblUserTO != null)
            {
                tblReportsTOReturnList = new List<TblReportsTO>();

                List<TblReportsTO> nonPermissionList = tblReportsTOList.Where(a => a.SysEleId <= 0).ToList();
                if (nonPermissionList != null && nonPermissionList.Count > 0)
                {
                    tblReportsTOReturnList.AddRange(nonPermissionList);
                }

                List<TblReportsTO> permissionList = tblReportsTOList.Where(a => a.SysEleId > 0).ToList();
                if (permissionList != null && permissionList.Count > 0)
                {
                    //tblReportsTOReturnList = new List<TblReportsTO>();

                    for (int i = 0; i < permissionList.Count; i++)
                    {
                        Int32 sysEleId = permissionList[i].SysEleId;
                        if (tblUserTO.SysEleAccessDCT.ContainsKey(sysEleId))
                            {
                            if (tblUserTO.SysEleAccessDCT[sysEleId] == "RW")
                            {
                                tblReportsTOReturnList.Add(permissionList[i]);
                            }
                        }

                    }
                }
            }

            return tblReportsTOReturnList;
        }

        public List<TblReportsTO> SelectAllTblReportsList()
        {
            List<TblReportsTO> tblReportsTODT = _iTblReportsDAO.SelectAllTblReports();
            Parallel.ForEach(tblReportsTODT, element =>
            {
                element.SqlQuery = null;
            }
            );
            return tblReportsTODT;
        }

        public TblReportsTO SelectTblReportsTO(Int32 idReports)
        {
            TblReportsTO tblReportsTODT = _iTblReportsDAO.SelectTblReports(idReports);
            if (tblReportsTODT != null)
            {
                List<TblFilterReportTO> tblFilterReportList = _iTblFilterReportBL.SelectTblFilterReportList(idReports);
                if (tblFilterReportList != null && tblFilterReportList.Count > 0)
                {
                    tblReportsTODT.TblFilterReportTOList1 = tblFilterReportList;

                }
                return tblReportsTODT;
            }

            else
                return null;
        }

        #endregion

        #region Insertion
        public int InsertTblReports(TblReportsTO tblReportsTO)
        {
            return _iTblReportsDAO.InsertTblReports(tblReportsTO);
        }

        public int InsertTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReportsDAO.InsertTblReports(tblReportsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblReports(TblReportsTO tblReportsTO)
        {
            return _iTblReportsDAO.UpdateTblReports(tblReportsTO);
        }

        public int UpdateTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReportsDAO.UpdateTblReports(tblReportsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblReports(Int32 idReports)
        {
            return _iTblReportsDAO.DeleteTblReports(idReports);
        }

        public int DeleteTblReports(Int32 idReports, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReportsDAO.DeleteTblReports(idReports, conn, tran);
        }

        #endregion

        //public List<DynamicReportTO> GetDynamicData(string cmdText, params SqlParameter[] commandParameters)
        //{
        //    try
        //    {
        //        List<DynamicReportTO> data = _iTblReportsDAO.GetDynamicSqlData(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING), "SELECT * FROM dimOrgType");
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        public IEnumerable<dynamic> GetDynamicData(string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                IEnumerable<dynamic> dynamicList = _iCommon.GetDynamicSqlData(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING), cmdText, commandParameters);
                if (dynamicList != null)
                {
                    return dynamicList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<dynamic> CreateDynamicQuery(TblReportsTO tblReportsTO)
        {
            string createdArr = string.Empty;
            try
            {
                if (tblReportsTO != null)
                {
                    TblReportsTO temptblReportsTO = SelectTblReportsTO(tblReportsTO.IdReports);
                    List<TblFilterReportTO> filterReportlist = new List<TblFilterReportTO>();
                    if (tblReportsTO.TblFilterReportTOList1 != null)
                    {
                        filterReportlist = tblReportsTO.TblFilterReportTOList1.OrderBy(element => element.OrderArguments).ToList();
                    }
                    String sqlQuery = temptblReportsTO.SqlQuery;
                    temptblReportsTO.RoleTypeCond = tblReportsTO.RoleTypeCond;
                    int count = filterReportlist.Count;
                    SqlParameter[] commandParameters = new SqlParameter[count];
                    for (int i = 0; i < filterReportlist.Count; i++)
                    {
                        TblFilterReportTO tblFilterReportTO = filterReportlist[i];
                        if(tblFilterReportTO.ShowDateTime==0 && tblFilterReportTO.SqlDbTypeValue==33 && tblFilterReportTO.SqlParameterName=="FromDate")
                        {
                            //Saket [2021-08-24] Added
                            //tblFilterReportTO.OutputValue = DateTime.Now.Date.ToString();
                            tblFilterReportTO.OutputValue = Convert.ToDateTime(tblFilterReportTO.OutputValue).Date.ToString();
                        }
                        if (tblFilterReportTO.OutputValue != null && tblFilterReportTO.OutputValue != string.Empty && tblFilterReportTO.IsOptional == 1)
                        {
                            sqlQuery += tblFilterReportTO.WhereClause;
                        }
                        if (tblFilterReportTO.IsRequired == 0 && temptblReportsTO.WhereClause != String.Empty)
                        {

                            if (!_iCommon.UserExistInCommaSeparetedStr(tblFilterReportTO.AdminUserIds, tblReportsTO.CreatedBy))
                            {
                                object listofUsers = _iTblOrgStructureBL.ChildUserListOnUserId(tblReportsTO.CreatedBy, 1, (int)Constants.ReportingTypeE.ADMINISTRATIVE, 1);  //this method is call for get Child User Id's From Organzization Structure.
                                List<int> userIdList = new List<int>();
                                if (listofUsers != null)
                                {
                                    userIdList = (List<int>)listofUsers;
                                }
                                else
                                {
                                    userIdList.Add(tblReportsTO.CreatedBy);
                                }
                                createdArr = string.Join<int>(",", userIdList);

                                temptblReportsTO.WhereClause = temptblReportsTO.WhereClause.Replace(tblFilterReportTO.SqlParameterName, createdArr);
                                sqlQuery += temptblReportsTO.WhereClause;
                            }
                        }
                        SqlDbType sqlDbType = (SqlDbType)tblFilterReportTO.SqlDbTypeValue;
                        commandParameters[i] = new SqlParameter("@" + tblFilterReportTO.SqlParameterName, sqlDbType);
                        commandParameters[i].Value = tblFilterReportTO.OutputValue;
                    }

                    if (!String.IsNullOrEmpty(temptblReportsTO.OrderByStr))
                        sqlQuery += temptblReportsTO.OrderByStr;

                   
                    sqlQuery = sqlQuery.Replace("@roleTypeCond", temptblReportsTO.RoleTypeCond);

                    IEnumerable<dynamic> dynamicList = GetDynamicData(sqlQuery, commandParameters);
                    
                    //CalculateValueFromAggregateFunctions(dynamicList,tblReportsTO);
                    return dynamicList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        //
        //public IEnumerable<dynamic> CalculateValueFromAggregateFunctions(IEnumerable<dynamic> dynamicList, TblReportsTO tblReportsTO)
        //{
        //    try
        //    {
        //        List<dynamic> list = dynamicList.ToList();

        //        var aggregateFunctionObj = new AggregateFunctionTO();
        //        var expandoObject = new ExpandoObject() as IDictionary<string, object>;
        //        var lstSum = new List<string>();
        //        JObject javascriptObject = JObject.Parse(tblReportsTO.TotalMetaData);
        //        if (javascriptObject != null)
        //        {
        //            foreach (var item in javascriptObject)
        //            {
        //                if(item.Key == "SUM")
        //                {
        //                    var sumObj = new ExpandoObject() as IDictionary<string, object>;
        //                    decimal total1 = 0;


        //                    IEnumerable<dynamic> lidt = dynamicList;





        //                    foreach (var val in item.Value)
        //                    {

        //                        foreach (var obj in lidt)
        //                        {
        //                            //foreach (var item1 in obj)
        //                            foreach (var item1 in (obj as IEnumerable<dynamic>))
        //                            {
        //                                //if(val == item1.Key)
        //                                //{
        //                                //    total1 = total1 + item1.Value;
        //                                //}

        //                                foreach (var v in (item1 as IEnumerable<dynamic>))
        //                                {
        //                                    if (v is KeyValuePair<string, string>)
        //                                    {
        //                                        // Do stuff
        //                                    }

        //                                    else throw new InvalidOperationException();
        //                                }


        //                            }
        //                        }


        //                       sumObj.Add(val.ToString(), total1);
        //                    }
        //                }
        //            }
        //            // parse the alphabets list (if any)
        //            //if (javascriptObject.GetValue("SUM") != null)
        //            //{

        //            //    var tempAlphabets = ((JArray)javascriptObject["SUM"]);
        //            //    if (tempAlphabets != null && tempAlphabets.Count > 0)
        //            //    {
        //            //        foreach (var item in tempAlphabets)
        //            //        {
        //            //            lstSum.Add(Convert.ToString(item));
        //            //        }
        //            //        aggregateFunctionObj.SUM = lstSum;
        //            //    }
        //            //}
        //        }
        //        //decimal total1 = 0;
        //        //decimal total2 = 0;
        //        //for (int i = 0; i < lstSum.Count; i++)
        //        //{
        //        //    var value1 = aggregateFunctionObj.SUM[i];
        //        //    foreach (var item in dynamicList)
        //        //    {
        //        //        foreach (var item1 in item)
        //        //        {
        //        //            if(lstSum.Contains(item1.Key))
        //        //            {
        //        //                total1 = total1 + item1.Value;
        //        //            }

        //        //        }

        //        //    }

        //        //}


        //        //                string js = @"[
        //        //  {
        //        //    'Title': 'Json.NET is awesome!',
        //        //    'Author': {
        //        //      'Name': 'James Newton-King',
        //        //      'Twitter': '@JamesNK',
        //        //      'Picture': '/jamesnk.png'
        //        //    },
        //        //    'Date': '2013-01-23T19:30:00',
        //        //    'BodyHtml': '&lt;h3&gt;Title!&lt;/h3&gt;\r\n&lt;p&gt;Content!&lt;/p&gt;'
        //        //  }
        //        //]";
        //        //var data = Newtonsoft.Json.JsonConvert.DeserializeObject(tblReportsTO.TotalMetaData?.Replace(@"/",""));
        //        //var t1 = data["SUM"].ToString();
        //        //string t2 = data["SUM"][1];

        //        //for (var i = 0; i < data.Count(); i++)
        //        //{
        //        //    string s1 = data["SUM"][i];
        //        //}

        //        //dynamic blogPosts = JArray.Parse(json);

        //        //dynamic blogPost = blogPosts[0];
        //        //for (var i = 0; i < data.count; i++)
        //        //{
        //        //    if (data[i])
        //        //}
        //        //if()


        //        //string title = data["SUM"][0];
        //        //string t2 = data["SUM"][1];
        //        // string t = data.AggregateFunctions["SUM"][0];


        //        //Console.WriteLine(title);
        //        // Json.NET is awesome!

        //        // string author = blogPost.Author.Name;

        //        //Console.WriteLine(author);
        //        // James Newton-King

        //        //DateTime postDate = blogPost.Date;

        //        //Console.WriteLine(postDate);
        //        // 23/01/2013 7:30:00 p.m.

        //        return dynamicList;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {

        //    }

        //}

        //Added by minal 02 March 2021

        public List<dynamic> SelectTallyStockTransferReportList(DateTime frmDt, DateTime toDt,String roleTypeCond)
        {
            DataTable resultDT = new DataTable();
            List<dynamic> tallyStockTransferReportTODynamicList = new List<dynamic>();

            resultDT = _iTblReportsDAO.SelectTallyStockTransferReportList(frmDt,toDt, roleTypeCond);
            if (resultDT != null && resultDT.Rows.Count > 0)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    DataRow reportDtlsRow = resultDT.Rows[i];

                    dynamic tallyStockTransferReportTO = new JObject();

                    tallyStockTransferReportTO["DATE"] = reportDtlsRow["Date"].ToString();
                    tallyStockTransferReportTO["VOUCHER TYPE"] = reportDtlsRow["VOUCHER TYPE"].ToString();
                    tallyStockTransferReportTO["STOCK ITEM"] = reportDtlsRow["STOCK ITEM"].ToString();
                    tallyStockTransferReportTO["GODOWN [C & F]"] = reportDtlsRow["GODOWN [C & F]"].ToString();
                    tallyStockTransferReportTO["QTY"] = reportDtlsRow["QTY"].ToString();
                    tallyStockTransferReportTO["RATE"] = reportDtlsRow["RATE"].ToString();
                    tallyStockTransferReportTO["AMOUNT"] = reportDtlsRow["AMOUNT"].ToString();
                    tallyStockTransferReportTO[" STOCK ITEM"] = reportDtlsRow["STOCK ITEM A"].ToString();
                    tallyStockTransferReportTO[" GODOWN"] = reportDtlsRow["GODOWN A"].ToString();
                    tallyStockTransferReportTO[" QTY"] = reportDtlsRow["QTY A"].ToString();
                    tallyStockTransferReportTO[" RATE"] = reportDtlsRow["RATE A"].ToString();
                    tallyStockTransferReportTO[" AMOUNT"] = reportDtlsRow["AMOUNT A"].ToString();
                    tallyStockTransferReportTO["NARRATION"] = reportDtlsRow["NARRATION"].ToString();
                    tallyStockTransferReportTO["isTotalRow"] = reportDtlsRow["isTotalRow"].ToString();

                    tallyStockTransferReportTODynamicList.Add(tallyStockTransferReportTO);
                }
            }
            return tallyStockTransferReportTODynamicList;

        }

        public List<TblWBRptTO> SelectWBForPurchaseSaleUnloadReportList(DateTime frmDt, DateTime toDt)
        {
            List<TblWBRptTO> tblWBRptTOList = new List<TblWBRptTO>();
            List<TblWBRptTO> tblWBRptTOListForSale = new List<TblWBRptTO>();
            List<TblWBRptTO> tblWBRptTOListForUnload = new List<TblWBRptTO>();

            tblWBRptTOList = _iTblReportsDAO.SelectWBForPurchaseReportList(frmDt, toDt);
            if(tblWBRptTOList == null)
                tblWBRptTOList = new List<TblWBRptTO>();
            tblWBRptTOListForSale = _iTblReportsDAO.SelectWBForSaleReportList(frmDt, toDt);
            if (tblWBRptTOListForSale.Count > 0 && tblWBRptTOListForSale != null)
            {
                tblWBRptTOList.AddRange(tblWBRptTOListForSale);
            }


            tblWBRptTOListForUnload = _iTblReportsDAO.SelectWBForUnloadReportList(frmDt, toDt);
            if (tblWBRptTOListForUnload.Count > 0 && tblWBRptTOListForUnload != null)
            {
                tblWBRptTOList.AddRange(tblWBRptTOListForUnload);
            }


            return tblWBRptTOList;
        }

        //Added by minal
    }
}

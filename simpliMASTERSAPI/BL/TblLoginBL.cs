using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace ODLMWebAPI.BL
{     
    public class TblLoginBL : ITblLoginBL
    { 
        private readonly ITblLoginDAO _iTblLoginDAO;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblUserRoleDAO _iTblUserRoleDAO;
        private readonly ITblSysElementsBL _iTblSysElementsBL;
        private readonly ITblMenuStructureDAO _iTblMenuStructureDAO;
        private readonly ITblModuleDAO _iTblModuleDAO; 
        private readonly ITblModuleBL _iTblModuleBL; 
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblUserAllocationDAO _iTblUserAllocationDAO;
        private readonly ICommon _iCommon;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblPersonDAO _iTblPersonDAO;
        private readonly ITblPmUserBL _iTblPmUserBL;


        public TblLoginBL(ITblPersonDAO iTblPersonDAO,ITblModuleBL _iTblModuleBL,ITblModuleDAO iTblModuleDAO, ITblSysElementsBL iTblSysElementsBL, ITblConfigParamsDAO iTblConfigParamsDAO, ICommon iCommon, ITblLoginDAO iTblLoginDAO, ITblUserDAO iTblUserDAO, ITblUserRoleDAO iTblUserRoleDAO, ITblMenuStructureDAO iTblMenuStructureDAO, ITblUserAllocationDAO iTblUserAllocationDAO, IConnectionString iConnectionString, ITblPmUserBL iITblPmUserBL)
        {
            _iTblLoginDAO = iTblLoginDAO;
            _iTblUserDAO = iTblUserDAO;
            _iTblUserRoleDAO = iTblUserRoleDAO;
            _iTblSysElementsBL = iTblSysElementsBL;
            _iTblMenuStructureDAO = iTblMenuStructureDAO;
            _iTblModuleDAO = iTblModuleDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iCommon = iCommon;
            this._iTblModuleBL=_iTblModuleBL;
            _iTblUserAllocationDAO = iTblUserAllocationDAO;
            _iConnectionString = iConnectionString;
            _iTblPersonDAO = iTblPersonDAO;
            _iTblPmUserBL = iITblPmUserBL;

        }
        #region Selection

        public List<TblLoginTO> SelectAllTblLoginList()
        {
            return _iTblLoginDAO.SelectAllTblLogin();
        }

        public TblLoginTO SelectTblLoginTO(Int32 idLogin)
        {
            return _iTblLoginDAO.SelectTblLogin(idLogin);
        }


        public TblUserTO getPermissionsOnModule(int userId, int moduleId)
        {
            TblUserTO userExistUserTO = new TblUserTO();
            userExistUserTO.IdUser = userId;
            try
            {
                userExistUserTO.UserRoleList = _iTblUserRoleDAO.SelectAllActiveUserRole(userExistUserTO.IdUser);
                List<TblModuleTO> allModuleList = _iTblModuleDAO.SelectTblModuleList().ToList();

                if (userExistUserTO.UserRoleList != null || userExistUserTO.UserRoleList.Count > 0)
                {
                    int[] list = userExistUserTO.UserRoleList.Where(a => a.IsActive == 1).Select(s => s.RoleId).ToArray();
                    String roleId = string.Join(",", list.ToArray());
                    userExistUserTO.SysEleAccessDCT = _iTblSysElementsBL.SelectSysElementUserMultiRoleEntitlementDCT(userExistUserTO.IdUser, roleId, moduleId);
                    userExistUserTO.ModuleTOList = new List<TblModuleTO>();

                    Int32 IsUserWiseModuleView = 0;
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_USER_WISE_MODULE_VIEW);
                    if (tblConfigParamsTO != null)
                    {
                        if (!String.IsNullOrEmpty(tblConfigParamsTO.ConfigParamVal))
                        {
                            IsUserWiseModuleView = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                        }
                        userExistUserTO.FirmNameE = Convert.ToInt16(tblConfigParamsTO.ConfigParamVal);
                    }

                    for (int m = 0; m < allModuleList.Count; m++)
                    {
                        if (allModuleList[m].IsSubscribe == 1) //Sudhir[30-08-2018] Added for checking IsSubscribe or Not. 
                        {
                            if (userExistUserTO.SysEleAccessDCT.ContainsKey(allModuleList[m].SysElementId))
                            {
                                if (userExistUserTO.SysEleAccessDCT[allModuleList[m].SysElementId] != "RW")
                                    allModuleList[m].NavigateUrl = null; //Added Sudhir For Set NavigateURL NULL
                            }
                            else if (IsUserWiseModuleView == 1)
                            {
                                allModuleList[m].NavigateUrl = null;
                            }
                        }
                        else
                        {
                            allModuleList[m].NavigateUrl = null; //Added Sudhir For Set NavigateURL NULL
                        }
                        if (IsUserWiseModuleView == 0)
                        {
                            userExistUserTO.ModuleTOList.Add(allModuleList[m]);
                        }
                        else if (allModuleList[m].NavigateUrl != null && IsUserWiseModuleView == 1)
                        {
                            userExistUserTO.ModuleTOList.Add(allModuleList[m]);
                        }
                    }
                }

                for (int i = 0;i< allModuleList.Count;i++)
                {
                    if (allModuleList[i].ModeId == (int)Constants.ApplicationModeTypeE.BASIC_MODE)
                    {
                        List<TblSysElementsTO> tblSysElementsTOList = _iTblSysElementsBL.SelectTblSysElementsByModulId(allModuleList[i].IdModule);

                        if (tblSysElementsTOList != null && tblSysElementsTOList.Count > 0)
                        {
                            for (int syse = 0; syse < tblSysElementsTOList.Count; syse++)
                            {
                                if (userExistUserTO.SysEleAccessDCT.ContainsKey(tblSysElementsTOList[syse].IdSysElement) && tblSysElementsTOList[syse].BasicModeApplicable != 1)
                                {
                                    userExistUserTO.SysEleAccessDCT.Remove(tblSysElementsTOList[syse].IdSysElement);
                                }

                                TblModuleTO moduleTO = userExistUserTO.ModuleTOList.Where(w => w.SysElementId == tblSysElementsTOList[syse].IdSysElement).FirstOrDefault();

                                if (moduleTO != null && tblSysElementsTOList[syse].BasicModeApplicable != 1)
                                {
                                    userExistUserTO.ModuleTOList.Remove(moduleTO);
                                }
                            }
                        }
                    }

                }


                return userExistUserTO;
            }
            catch (Exception ex)
            {
                return userExistUserTO;
            }
        }


        #endregion

        #region Insertion
        public int InsertTblLogin(TblLoginTO tblLoginTO)
        {
            return _iTblLoginDAO.InsertTblLogin(tblLoginTO);
        }

        public int InsertTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoginDAO.InsertTblLogin(tblLoginTO, conn, tran);
        }



        public ResultMessage LogIn(TblUserTO tblUserTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                #region Check Current Active Users Count and Configration Value @KM 13/02/2019
                Boolean isProceedToCreate = GetUsersAvailability();
                if (!isProceedToCreate)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Login Limits Exceeded, Please Contact Administrative authorities";
                    return resultMessage;
                }
                #endregion
                #region 1. Check Is This user Exists And Active  

                TblUserTO userExistUserTO = _iTblUserDAO.SelectTblUser(tblUserTO.UserLogin, tblUserTO.UserPasswd);
                if (userExistUserTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Invalid Credentials";
                    return resultMessage;
                }

                //Sanjay [25-Feb-2019] To Identify between invalid credentials and inactive account
                if (userExistUserTO.IsActive == 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Inactive Account since :" + userExistUserTO.DeactivatedOn;
                    resultMessage.DisplayMessage = "This account is inactive. Please contact your technical administrative authorities";
                    return resultMessage;
                }

                if (!string.IsNullOrEmpty(userExistUserTO.RegisteredDeviceId) && !string.IsNullOrEmpty(tblUserTO.RegisteredDeviceId))
                {
                    if (tblUserTO.RegisteredDeviceId != userExistUserTO.RegisteredDeviceId)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Hey , Not Allowed. Current Log In Device and Registered Device Not Matching";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }

                #endregion

                if (userExistUserTO != null)
                {
                    userExistUserTO.UserRoleList = _iTblUserRoleDAO.SelectAllActiveUserRole(userExistUserTO.IdUser);
                    if (userExistUserTO.UserRoleList == null || userExistUserTO.UserRoleList.Count == 0)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Your Role Is Not Defined In The System , Please contact your system admin";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    //  int roleId = userExistUserTO.UserRoleList.Where(a => a.IsActive == 1).FirstOrDefault().RoleId;
                    //[Hrushikesh]--For bringing all permision against multiple role for same user

                    int[] list = userExistUserTO.UserRoleList.Where(a => a.IsActive == 1).Select(s => s.RoleId).ToArray();
                    String roleId = string.Join(",", list.ToArray());
                    userExistUserTO.SysEleAccessDCT = _iTblSysElementsBL.SelectSysElementUserMultiRoleEntitlementDCT(userExistUserTO.IdUser, roleId, null);
                    if (userExistUserTO.SysEleAccessDCT == null || userExistUserTO.SysEleAccessDCT.Count == 0)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "User has No Permissions.";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    //Prajakta[2019-04-30] Added to get PM - User List
                    //userExistUserTO.PmUserList = _iTblPmUserBL.SelectAllPMForUser(userExistUserTO.IdUser);

                    List<TblMenuStructureTO> allMenuList = _iTblMenuStructureDAO.SelectAllTblMenuStructure().OrderBy(s => s.SerNo).ToList();
                    userExistUserTO.MenuStructureTOList = new List<TblMenuStructureTO>();

                    for (int m = 0; m < allMenuList.Count; m++)
                    {
                        if (userExistUserTO.SysEleAccessDCT.ContainsKey(allMenuList[m].SysElementId))
                        {
                            if (userExistUserTO.SysEleAccessDCT[allMenuList[m].SysElementId] == "RW")
                                userExistUserTO.MenuStructureTOList.Add(allMenuList[m]);
                        }
                    }

                    List<TblModuleTO> allModuleList = _iTblModuleDAO.SelectTblModuleList().ToList();
                    TblConfigParamsTO tblConfigParamsTOPermission = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.Allow_Module_On_Permisssion);
                    if (tblConfigParamsTOPermission != null && tblConfigParamsTOPermission.ConfigParamVal == "1")
                    {
                        allModuleList = _iTblModuleDAO.SelectTblModuleListWithPermission().ToList();
                    }
                    userExistUserTO.ModuleTOList = new List<TblModuleTO>();
                    Int32 IsUserWiseModuleView = 0;
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_USER_WISE_MODULE_VIEW);
                    if (tblConfigParamsTO != null)
                    {
                        if (!String.IsNullOrEmpty(tblConfigParamsTO.ConfigParamVal))
                        {
                            IsUserWiseModuleView = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                        }
                        userExistUserTO.FirmNameE = Convert.ToInt16(tblConfigParamsTO.ConfigParamVal);
                    }
                    for (int m = 0; m < allModuleList.Count; m++)
                    {
                        if (allModuleList[m].IsSubscribe == 1) //Sudhir[30-08-2018] Added for checking IsSubscribe or Not. 
                        {
                            if (userExistUserTO.SysEleAccessDCT.ContainsKey(allModuleList[m].SysElementId))
                            {
                                if (userExistUserTO.SysEleAccessDCT[allModuleList[m].SysElementId] != "RW")
                                    allModuleList[m].NavigateUrl = null; //Added Sudhir For Set NavigateURL NULL

                            }
                            else if (IsUserWiseModuleView == 1)
                            {
                                allModuleList[m].NavigateUrl = null;
                            }
                        }
                        else
                        {
                            allModuleList[m].NavigateUrl = null; //Added Sudhir For Set NavigateURL NULL
                        }
                        if (IsUserWiseModuleView == 0)
                        {
                            userExistUserTO.ModuleTOList.Add(allModuleList[m]);
                        }
                        else if (allModuleList[m].NavigateUrl != null && IsUserWiseModuleView == 1)
                        {
                            userExistUserTO.ModuleTOList.Add(allModuleList[m]);
                        }
                    }


                    for (int m = 0; m < allModuleList.Count; m++)
                    {
                        if (allModuleList[m].ModeId == (int)Constants.ApplicationModeTypeE.BASIC_MODE)
                        {
                            List<TblSysElementsTO> tblSysElementsTOList = _iTblSysElementsBL.SelectTblSysElementsByModulId(allModuleList[m].IdModule);

                            if (tblSysElementsTOList != null && tblSysElementsTOList.Count > 0)
                            {
                                for (int syse = 0; syse < tblSysElementsTOList.Count; syse++)
                                {
                                    if (userExistUserTO.SysEleAccessDCT.ContainsKey(tblSysElementsTOList[syse].IdSysElement) && tblSysElementsTOList[syse].BasicModeApplicable != 1)
                                    {
                                        userExistUserTO.SysEleAccessDCT.Remove(tblSysElementsTOList[syse].IdSysElement);
                                    }


                                    TblMenuStructureTO tblMenuStructureTO = userExistUserTO.MenuStructureTOList.Where(w => w.SysElementId == tblSysElementsTOList[syse].IdSysElement).FirstOrDefault();

                                    if (tblMenuStructureTO != null && tblSysElementsTOList[syse].BasicModeApplicable != 1)
                                    {
                                        userExistUserTO.MenuStructureTOList.Remove(tblMenuStructureTO);
                                    }

                                    TblModuleTO moduleTO = userExistUserTO.ModuleTOList.Where(w => w.SysElementId == tblSysElementsTOList[syse].IdSysElement).FirstOrDefault();

                                    if (moduleTO != null && tblSysElementsTOList[syse].BasicModeApplicable != 1)
                                    {
                                        userExistUserTO.ModuleTOList.Remove(moduleTO);
                                    }
                                }
                            }
                        }

                    }

                    //List<TblModuleTO> allModuleList = BL._iTblModuleBL.SelectTblModuleList().ToList();
                    //userExistUserTO.ModuleTOList = new List<TblModuleTO>();

                    //for (int m = 0; m < allModuleList.Count; m++)
                    //{
                    //    if (userExistUserTO.SysEleAccessDCT.ContainsKey(allModuleList[m].SysElementId))
                    //    {
                    //        if (userExistUserTO.SysEleAccessDCT[allModuleList[m].SysElementId] != "RW")
                    //            allModuleList[m].NavigateUrl = null;
                    //    }
                    //    userExistUserTO.ModuleTOList.Add(allModuleList[m]);
                    //}

                    //// Vaibhav [05-Mar-2018] Added to get access token
                    //userExistUserTO.AuthorizationTO = Authentication.Authentication.getAccessToken(tblUserTO.UserLogin,tblUserTO.UserPasswd);

                    //// Vaibhav [05-Mar-2018] Added to get all product module list.
                    //userExistUserTO.ModuleTOList = BL._iTblModuleBL.SelectAllTblModuleList();
                }

                //Deepali added to get personTO [16-12-2020]
                if (userExistUserTO != null)
                {
                    userExistUserTO.UserPersonTO = _iTblPersonDAO.SelectTblPerson(userExistUserTO.PersonId);
                }

                //Saket [2019-12-06] Added. 
                //Added by minal [9 March 2021]

                List<TblUserAllocationTO> userAllocationList = _iTblUserAllocationDAO.GetUserAllocationList(userExistUserTO.IdUser, null, null);
                userExistUserTO.UserAllocationList = userAllocationList;
                if (userAllocationList != null && userAllocationList.Count > 0)
                {
                    userExistUserTO.PmUserList = GetPMUserList(userAllocationList);
                }
                //Added by minal
                #region 2. Mark Login Entry
                userExistUserTO.LoginTO = tblUserTO.LoginTO;
                userExistUserTO.LoginTO.LoginDate = _iCommon.ServerDateTime;
                userExistUserTO.LoginTO.UserId = userExistUserTO.IdUser;
                userExistUserTO.LoginTO.DeviceId = tblUserTO.DeviceId;
                int result = InsertTblLogin(userExistUserTO.LoginTO);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Could not login. Some error occured while login";
                    resultMessage.Tag = "Error While InsertTblLogin In Method Login";
                    resultMessage.Result = 0;
                    return resultMessage;
                }


                #endregion

                #region 3. Update Device Id for New Registration

                if (String.IsNullOrEmpty(userExistUserTO.RegisteredDeviceId)
                    && String.IsNullOrEmpty(userExistUserTO.DeviceId))
                {
                    if (!string.IsNullOrEmpty(tblUserTO.RegisteredDeviceId)
                        && !string.IsNullOrEmpty(tblUserTO.DeviceId))
                    {
                        userExistUserTO.RegisteredDeviceId = tblUserTO.RegisteredDeviceId;
                        userExistUserTO.ImeiNumber = tblUserTO.DeviceId;
                        _iTblUserDAO.UpdateTblUser(userExistUserTO);
                    }
                }

                #endregion

                //Vijaymala Added[14-02-2017]:To set current company Id

                TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_CURRENT_COMPANY);
                if (configParamsTO != null)
                {
                    userExistUserTO.FirmNameE = Convert.ToInt16(configParamsTO.ConfigParamVal);
                }

                tblUserTO = userExistUserTO;

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "User Logged In Sucessfully";
                resultMessage.Tag = userExistUserTO;
                resultMessage.Result = 1;

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Could not login. Some error occured while login";
                resultMessage.Tag = "Exception Error While LogIn at BL Level";
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        public List<TblPmUserTO> GetPMUserList(List<TblUserAllocationTO> userAllocationList)
        {
            
            List<TblPmUserTO> tblPmUserTOList = new List<TblPmUserTO>();
            if (userAllocationList == null || userAllocationList.Count == 0)
            {
                return null;
            }

            //allocTypeId = 2; PM User Assistent
            List<TblUserAllocationTO> userAllocationListTemp = userAllocationList.Where(a => a.AllocTypeId == (int)Constants.AllocationTypeE.PurchaseManger).ToList();

            for (int k = 0; k < userAllocationListTemp.Count; k++)
            {
                TblPmUserTO tblPmUserTO = new TblPmUserTO();
                tblPmUserTO.PmId = Convert.ToInt32(userAllocationListTemp[k].RefId);
                tblPmUserTO.UserId = Convert.ToInt32(userAllocationListTemp[k].UserId);
                tblPmUserTO.UserDisplayName = userAllocationListTemp[k].UserDisplayName;
                tblPmUserTO.IsActive = Convert.ToInt32(userAllocationListTemp[k].IsActive);
                tblPmUserTO.CreatedBy = Convert.ToInt32(userAllocationListTemp[k].CreatedBy);
                tblPmUserTO.CreatedOn = Convert.ToDateTime(userAllocationListTemp[k].CreatedOn);
                tblPmUserTO.UpdatedBy = Convert.ToInt32(userAllocationListTemp[k].UpdatedBy);
                tblPmUserTO.UpdatedOn = Convert.ToDateTime(userAllocationListTemp[k].UpdatedOn);
                tblPmUserTOList.Add(tblPmUserTO);
            }
            return tblPmUserTOList;
        }

        public List<TblLoginTO> GetCurrentActiveUsers()
        {
           return _iTblLoginDAO.GetCurrentActiveUsers();
        }

        public dimUserConfigrationTO GetUsersConfigration(int ConfigDesc)
        {
            return _iTblLoginDAO.GetUsersConfigration(ConfigDesc);
        }
        public dimUserConfigrationTO GetUsersConfigration(int ConfigDesc,SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoginDAO.GetUsersConfigration(ConfigDesc,conn,tran);
        }
        //YA - Shifted in tblUserBL 28/2/2019
        //public Boolean GetUsersQuata(SqlConnection conn, SqlTransaction tran)
        //{
        //    Boolean isValid = true;
        //    dimUserConfigrationTO dimUserConfigrationTO = _iTblLoginDAO.GetUsersConfigration((int)Constants.UsersConfigration.USER_CONFIG, conn, tran);
        //    if(dimUserConfigrationTO != null)
        //    {
        //        List<TblUserTO> list = _iTblUserDAO.SelectAllTblUser(true);
        //        if(list != null && list.Count > 0)
        //        {
        //            if(Convert.ToInt32(dimUserConfigrationTO.ConfigValue) <= list.Count)
        //            {
        //                isValid = false;
        //            }
        //        }
        //    }
        //    return isValid;
        //}
        public Boolean GetUsersAvailability()
        {
            Boolean isValid = true;
            dimUserConfigrationTO dimUserConfigrationTO = _iTblLoginDAO.GetUsersConfigration((int)Constants.UsersConfigration.USER_CONFIG);
            if (dimUserConfigrationTO != null)
            {
                List<TblLoginTO> list = GetCurrentActiveUsers();
                if (list != null && list.Count > 0)
                {
                    if (Convert.ToInt32(dimUserConfigrationTO.ConfigValue) <= list.Count)
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
        
        public ResultMessage LogOut(TblUserTO tblUserTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                #region 1. Check Is This user Exists First

                TblUserTO userExistUserTO = _iTblUserDAO.SelectTblUser(tblUserTO.UserLogin, tblUserTO.UserPasswd);
                if (userExistUserTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "User Not Found";
                    return resultMessage;
                }

                #endregion
                //Added By Vipul User Tracking [15/03/2019]
                #region Update TblModuleCommHis
TblModuleCommHisTO tblModuleCommHisTO=new TblModuleCommHisTO();
tblModuleCommHisTO.LoginId=tblUserTO.LoginTO.IdLogin;
tblModuleCommHisTO.OutTime=_iCommon.ServerDateTime;
tblUserTO.TblModuleCommHisTO=tblModuleCommHisTO;
try{
        _iTblModuleBL.UpdateTblModuleCommHis(tblUserTO.TblModuleCommHisTO,null,null);
}
catch(Exception ex)
{
resultMessage.MessageType = ResultMessageE.Error;
 resultMessage.Text = "Error While Update TblModuleCommHis In Method LogOut";
 return resultMessage;
}




#endregion
                //End
                #region 2. Update Login Entry
                TblLoginTO loginTO = tblUserTO.LoginTO;
                loginTO.LogoutDate = _iCommon.ServerDateTime;
                int result = UpdateTblLogin(loginTO);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLogin In Method LogOut";
                    return resultMessage;
                }

                #endregion

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "User Logged Out Sucessfully";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method LogOut";
                resultMessage.Tag = ex;
                return resultMessage;
            }
        }

        public ResultMessage LogOutForHRGrid(Int32 IdUser)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
                SqlTransaction tran = null;
                ResultMessage rMessage = new ResultMessage();
                DateTime serverDateTime = _iCommon.ServerDateTime;
                // get All Login Id For Session Out
                string LoginIds = "";           
                LoginIds = _iCommon.SelectAllLoginEntries(IdUser);

                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;

                if (!string.IsNullOrEmpty(LoginIds))
                {
                    string[] x = LoginIds.Split(',');
                    foreach (string item in x)
                    {
                        TblModuleCommHisTO TblModuleCommHis = new TblModuleCommHisTO();
                        TblModuleCommHis.LoginId = Convert.ToInt32(item);
                        result = _iTblModuleBL.UpdateTblModuleCommHisBeforeLoginForAPK(TblModuleCommHis, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While UpdateTblLogin  Method UpdateUser ";
                            rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return rMessage;
                        }
                    }
                }
                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method LogOut";
                resultMessage.Tag = ex;
                return resultMessage;
            }
        }


        #endregion

        #region Updation
        public int UpdateTblLogin(TblLoginTO tblLoginTO)
        {
            return _iTblLoginDAO.UpdateTblLogin(tblLoginTO);
        }

        public int UpdateTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoginDAO.UpdateTblLogin(tblLoginTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblLogin(Int32 idLogin)
        {
            return _iTblLoginDAO.DeleteTblLogin(idLogin);
        }

        public int DeleteTblLogin(Int32 idLogin, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoginDAO.DeleteTblLogin(idLogin, conn, tran);
        }





        #endregion

        #region Wishlist Login 
        public ResultMessage LogInWishlist(TblUserTO tblUserTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                #region 1. Check Is This user Exists And Active  

                TblUserTO userExistUserTO = _iTblUserDAO.SelectTblWishlistUser(tblUserTO.UserLogin, tblUserTO.UserPasswd);
                if (userExistUserTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    //resultMessage.Tag = HttpStatusCode.Unauthorized;
                    resultMessage.Text = "Invalid Credentials";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                #endregion

                #region Login User
                //userExistUserTO.LoginTO = tblUserTO.LoginTO;
                //int result = InsertTblLogin(userExistUserTO.LoginTO);
                //if (result != 1)
                //{
                //    resultMessage.MessageType = ResultMessageE.Error;
                //    resultMessage.Text = "Could not login. Some error occured while login";
                //    resultMessage.Tag = "Error While InsertTblLogin In Method Login";
                //    resultMessage.Result = 0;
                //    return resultMessage;
                //}
                #endregion

                tblUserTO = userExistUserTO;

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "User Logged In Sucessfully";
                resultMessage.Tag = userExistUserTO;
                resultMessage.Result = 1;
                resultMessage.data = tblUserTO.IdUser;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Could not login. Some error occured while login";
                resultMessage.Tag = "Exception Error While LogIn at BL level";
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        #endregion
    }
}


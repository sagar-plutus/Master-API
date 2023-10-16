using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        
        private readonly ITblUserAllocationDAO _iTblUserAllocationDAO;
        private readonly ITblUserBL _iTblUserBL;
        private readonly ITblRoleBL _iTblRoleBL;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ITblFeedbackBL _iTblFeedbackBL;
        private readonly ITblLoginBL _iTblLoginBL;
        private readonly ITblUserAreaAllocationBL _iTblUserAreaAllocationBL;
        private readonly ITblSysElementsBL _iTblSysElementsBL; 
        private readonly ITblUserExtBL _iTblUserExtBL;
        private readonly ITblUserBrandBL _iTblUserBrandBL;
        private readonly ITblDocumentDetailsBL _iTblDocumentDetailsBL;
        private readonly ITblModuleBL _iTblModuleBL;
        private readonly IDimRoleTypeBL _iDimRoleTypeBL;
        private readonly ITblPurchaseManagerSupplierBL _iTblPurchaseManagerSupplierBL;
        private readonly ITblAttributeStatusBL _iTblAttributeStatusBL;
        private readonly ICommon _iCommon;
        public UserController(ITblDocumentDetailsBL iTblDocumentDetailsBL, ITblPurchaseManagerSupplierBL iTblPurchaseManagerSupplierBL, ITblModuleBL iTblModuleBL, IDimRoleTypeBL iDimRoleTypeBL, ITblUserBrandBL iTblUserBrandBL, ITblUserExtBL iTblUserExtBL, ITblSysElementsBL iTblSysElementsBL, ITblUserAreaAllocationBL iTblUserAreaAllocationBL, 
            ITblLoginBL iTblLoginBL, ITblFeedbackBL iTblFeedbackBL, ITblUserBL iTblUserBL, 
            ICommon iCommon, ITblUserRoleBL iTblUserRoleBL, ITblPersonBL iTblPersonBL, ITblAttributeStatusBL iTblAttributeStatusBL, ITblRoleBL iTblRoleBL,
            ITblUserAllocationDAO iTblUserAllocationDAO)
        {
            _iTblUserAllocationDAO = iTblUserAllocationDAO;
            _iTblRoleBL = iTblRoleBL;
            _iTblUserBL = iTblUserBL;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblPersonBL = iTblPersonBL;
            _iTblFeedbackBL = iTblFeedbackBL;
            _iTblLoginBL = iTblLoginBL;
            _iTblUserAreaAllocationBL = iTblUserAreaAllocationBL;
            _iTblPurchaseManagerSupplierBL = iTblPurchaseManagerSupplierBL;
            _iTblSysElementsBL = iTblSysElementsBL;
            _iTblUserExtBL = iTblUserExtBL;
            _iTblUserBrandBL = iTblUserBrandBL;
            _iTblDocumentDetailsBL = iTblDocumentDetailsBL;
            _iTblModuleBL = iTblModuleBL;
            _iDimRoleTypeBL = iDimRoleTypeBL;
            _iCommon = iCommon;
            _iTblAttributeStatusBL = iTblAttributeStatusBL;
        }
        #region GET
        [Route("GetAllUserSettingsList")]
        [HttpGet]
        public List<TblUserExtTO> GetAllUserSettingsList()
        {
            return _iTblUserExtBL.GetAllUserSettingsList();
        }
        [Route("GetAllRoleList")]
        [HttpGet]
        public List<TblRoleTO> GetAllRoleList()
        {
            return _iTblRoleBL.GetAllRoleList();
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("getAllTblModuleCommHis")]
        [HttpGet]
        public List<TblModuleCommHisTO> getAllTblModuleCommHis(int userId)
        {
            return _iTblModuleBL.GetAlltblModuleCommHis(userId);
        }
        [Route("GetUser")]
        [HttpGet]
        public TblUserTO GetUser(String userLogin, String userPwd)
        {
            String encPwd = Encrypt("123");
            String decPwd = Decrypt(encPwd);

            TblUserTO tblUserTO = _iTblUserBL.SelectTblUserTO(userLogin, userPwd);
            if (tblUserTO != null)
            {
                //Sanjay [25-Feb-2019] To Identify between invalid credentials and inactive account
                if (tblUserTO.IsActive == 0)
                {
                    return null;
                }
                tblUserTO.UserRoleList = _iTblUserRoleBL.SelectAllActiveUserRoleList(tblUserTO.IdUser);
            }
            return tblUserTO;
        }

        [Route("GetAllActiveRolesOnUserId")]
        [HttpGet]
        public List<TblUserRoleTO> getAllActiveRoles( int userId)
        {
            return  _iTblUserRoleBL.SelectAllActiveUserRoleList(userId);
           
        }



        [Route("GetPersonOnPersonId")]
        [HttpGet]
        public TblPersonTO GetPersonOnPersonId(Int32 personId)
        {
            return _iTblPersonBL.SelectTblPersonTO(personId);
        }

        [Route("GetPermissionsOnModuleId")]
        [HttpGet]
        public TblUserTO getPermissionsOnModule(int userId, int moduleId)
        {

            return _iTblLoginBL.getPermissionsOnModule(userId, moduleId);


        }

        [Route("GetPersonOnUserId")]
        [HttpGet]
        public TblPersonTO GetPersonOnUserId(Int32 UserId)
        {
            return _iTblPersonBL.GetPersonOnUserId(UserId);
        }

        [Route("GetUsersFromRoleForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetUsersFromRoleForDropDown(Int32 roleId)
        {
            List<DropDownTO> userList = _iTblUserRoleBL.SelectUsersFromRoleForDropDown(roleId);
            return userList;
        }

        [Route("GetAllActiveUsersForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetAllActiveUsersForDropDown()
        {
            List<DropDownTO> activeUserList = _iTblUserBL.GetAllActiveUsersForDropDown();
            return activeUserList;
        }

        /// <summary>
        /// Vijaymala added[21/12/2018] added to get RM List from roletypeid
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [Route("GetUsersFromRoleTypeForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetUsersFromRoleTypeForDropDown(Int32 roleTypeId)
        {
            List<DropDownTO> userList = _iTblUserRoleBL.SelectUsersFromRoleTypeForDropDown(roleTypeId);
            return userList;
        }

        [Route("GetUsersFromRoleIds")]
        [HttpGet]
        public List<DropDownTO> GetUsersFromRoleIds(String roleId)
        {
            List<DropDownTO> userList = _iTblUserRoleBL.SelectUsersFromRoleIdsForDropDown(roleId);
            return userList;
        }

        [Route("GetActiveUserDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetActiveUserDropDownList(int moduleId = 0)
        {
            List<DropDownTO> userList = _iTblUserBL.SelectAllActiveUsersForDropDown(moduleId);
            return userList;
        }
        /// <summary>
        /// Priyanka H [30/07/2019] Get User List For Item Issue Note
        /// </summary>
        /// <returns></returns>
        [Route("GetUserListForIssue")]
        [HttpGet]
        public List<DropDownTO> GetUserListForIssue()
        {
            List<DropDownTO> issueUserList = _iTblUserBL.SelectUserListForIssue();
            return issueUserList;
        }


        [Route("GetCurrentActiveUsers")]
        [HttpGet]
        public List<TblLoginTO> GetCurrentActiveUsers()
        {
            List<TblLoginTO> activeUserList = _iTblLoginBL.GetCurrentActiveUsers();
            return activeUserList;
        }

        [Route("GetUsersConfigration")]
        [HttpGet]
        public dimUserConfigrationTO GetUsersConfigration(int ConfigDesc)
        {
            dimUserConfigrationTO UserConfigrationTO = _iTblLoginBL.GetUsersConfigration(ConfigDesc);
            return UserConfigrationTO;
        }

        [Route("GetFeedbackList")]
        [HttpGet]
        public List<TblFeedbackTO> GetFeedbackList(int userId, string fromDate, string toDate)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);
            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.AddDays(-7);
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime;

            return _iTblFeedbackBL.SelectAllTblFeedbackList(userId, frmDt, toDt);
        }

        [Route("GetUserAllocatedAreaList")]
        [HttpGet]
        public List<TblUserAreaAllocationTO> GetUserAllocatedAreaList(int userId)
        {
            return _iTblUserAreaAllocationBL.SelectAllTblUserAreaAllocationList(userId);
        }

        [Route("GetRoleOrUserPermissionList")]
        [HttpGet]
        public List<PermissionTO> GetRoleOrUserPermissionList(int menuPageId, int roleId, int userId, int moduleId)
        {
            return _iTblSysElementsBL.SelectAllPermissionList(menuPageId, roleId, userId, moduleId);
        }

        [Route("GetAllSystemUserList")]
        [HttpGet]
        public List<TblUserTO> GetAllSystemUserList(Int32 roleId =0, Int32 firmId=0,Int32 userTypeId=0 ,String deptId="")
        {
            List<TblUserTO> list = _iTblUserBL.SelectAllTblUserList(true , deptId);
            if (list != null)
            {
                List<TblUserRoleTO> userRoleList = _iTblUserRoleBL.SelectAllTblUserRoleList();
                for (int i = 0; i < list.Count; i++)
                {
                    var roleList = userRoleList.Where(r => r.UserId == list[i].IdUser && r.IsActive == 1).ToList();
                    if(roleList!=null && roleList.Count >0)
                    {
                        list[i].UserRoleList = roleList;
                    }
                }
                // list = list.OrderBy(o => o.UserRoleList[0].RoleDesc).ThenBy(o => o.UserDisplayName).ToList();
              
                var tempList = list.Where(o => o.UserRoleList==null);

                //list = list.Where(o => o.UserRoleList !=null && o.UserRoleList.Count > 0).OrderBy(o => o.UserRoleList[0].RoleDesc).ThenBy(o => o.UserDisplayName).ToList();

                list = list.OrderBy(o => o.UserDisplayName).ToList();
                list.AddRange(tempList);

                if (roleId > 0)
                {
                    List<TblUserTO> listRole = new List<TblUserTO>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        var roleList = userRoleList.Where(r => r.RoleId == roleId && r.UserId == list[i].IdUser).ToList();
                        if (roleList != null && roleList.Count > 0)
                        {
                            listRole.Add(list[i]);
                        }
                    }

                    list = listRole;
                }
                if(firmId > 0)
                {
                    List<TblUserTO> listRole = new List<TblUserTO>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].OrganizationId != 0 && list[i].OrganizationId == firmId)
                        {
                            listRole.Add(list[i]);
                        }
                    }
                    list = listRole;
                }
                if(userTypeId>0)
                {
                    list = list.Where(res => res.UserTypeId == userTypeId).ToList();
                }
            }
            
            return list;
           
        }

        [Route("GetUserDetails")]
        [HttpGet]
        public TblUserTO GetUserDetails(Int32 userId)
        {
            TblUserTO userTO = _iTblUserBL.SelectTblUserTO(userId);
            if (userTO != null)
            {
                userTO.UserExtTO = _iTblUserExtBL.SelectTblUserExtTO(userId);
                if (userTO.UserExtTO != null)
                    userTO.UserPersonTO = _iTblPersonBL.SelectTblPersonTO(userTO.UserExtTO.PersonId);

                userTO.UserRoleList = _iTblUserRoleBL.SelectAllActiveUserRoleList(userId);
                if (userTO.UserRoleList != null && userTO.UserRoleList.Count > 0)
                {
 
                    userTO.UserRoleList.ForEach(userole => {
                        TblRoleTO roleTO = _iTblRoleBL.SelectTblRoleTO(userole.RoleId);
                        if (roleTO.OrgStructureId >0)
                        {
                            userTO.OrgStructId = roleTO.OrgStructureId;

                        }
                    });

                }

            }

            return userTO;
        }
        /// <summary>
        /// Priyanka [27-12-2018] : Added to gete the all active user brand list.
        /// </summary>
        /// <returns></returns>

        [Route("GetAllUserBrandList")]
        [HttpGet]
        public List<TblUserBrandTO> GetAllUserBrandList()
        {
            return _iTblUserBrandBL.SelectAllTblUserBrand(1);
        }

        //Deepali Added [02-06-2021] for Allocation task
        [Route("GetUserAllocationList")]
        [HttpGet]
        public List<TblUserAllocationTO> GetUserAllocationList(Int32 idUser)
        {
            return _iTblUserAllocationDAO.GetUserAllocationList(idUser, null, null);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("GetModulePageList")]
        [HttpGet]
        public List<AttributePageTO> GetModulePageList(Int32 moduleId)
        {
            return _iTblModuleBL.SelectModulePageList(moduleId);
        }

        [Route("GetAttributeLanguageDetails")]
        [HttpGet]
        public List<TblCRMLabelTO> GetAttributeLanguageDetails(Int32 attrId)
        {
            return _iTblAttributeStatusBL.GetAttributeLanguageDetails(attrId);
        }

        #endregion

        #region POST
        [Route("UpdateUserSettings")]
        [HttpPost]
        public ResultMessage UpdateUserSettings([FromBody] List<TblUserExtTO> tblUserExtTOList)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return _iTblUserExtBL.UpdateUserSettings(tblUserExtTOList);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in UpdateUserSettings";
                return resultMessage;
            }
        }
        [Route("UpdateRoleSettings")]
        [HttpPost]
        public ResultMessage UpdateRoleSettings([FromBody] List<TblRoleTO> tblRoleTOList)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return _iTblRoleBL.UpdateRoleSettings(tblRoleTOList);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in UpdateUserSettings";
                return resultMessage;
            }
        }

        //[Route("PostLogin")]
        //[HttpPost]
        //public TblUserTO PostLogin([FromBody] TblUserTO userTO)
        //{
        //    try
        //    {

        //        if (userTO == null)
        //        {
        //            return null;
        //        }
        //        //String[] devices = { "dr5RvjV8_hk:APA91bFrDgE0NFAI8u5-eTVGrG4BGGJywIbHYywxrrLmmTLrC2-pjQLhhA48Tc7WF32hJTkd_Ik60MkfzZJXhcuJupu1hIshP-3ri-FrSQAQQHimCj4CWBfVsmIZB8K8qom3mzLS3x5S" };
        //        //String body = "ddd"; String title = "dddddd";
        //        //string ss = VitplNotify.NotifyToRegisteredDevices(devices,body,title);
        //        //BL.TblLoadingBL.CancelAllNotConfirmedLoadingSlips();
        //        ResultMessage rMessage = new ResultMessage();
        //        rMessage = BL._iTblLoginBL.LogIn(userTO);
        //        if (rMessage.MessageType != ResultMessageE.Information)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            userTO = (TblUserTO)rMessage.Tag;
        //            return userTO;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        [Route("PostLogin")]
        [HttpPost]
        public ResultMessage PostLogin([FromBody] TblUserTO userTO)
        {
            try
            {

                if (userTO == null)
                {
                    return null;
                }
                //String[] devices = { "dr5RvjV8_hk:APA91bFrDgE0NFAI8u5-eTVGrG4BGGJywIbHYywxrrLmmTLrC2-pjQLhhA48Tc7WF32hJTkd_Ik60MkfzZJXhcuJupu1hIshP-3ri-FrSQAQQHimCj4CWBfVsmIZB8K8qom3mzLS3x5S" };
                //String body = "ddd"; String title = "dddddd";
                //string ss = VitplNotify.NotifyToRegisteredDevices(devices,body,title);
                //BL.TblLoadingBL.CancelAllNotConfirmedLoadingSlips();
                ResultMessage rMessage = new ResultMessage();
                rMessage =_iTblLoginBL.LogIn(userTO);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    return rMessage;
                }
                else
                {
                    return rMessage;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("PostChangeCrdentials")]
        [HttpPost]
        public ResultMessage PostChangeCrdentials([FromBody] TblUserTO TblUserTO)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (TblUserTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "userTO Found NULL";
                    return resultMessage;
                }

                ResultMessage rMessage = new ResultMessage();

                TblUserTO userTo = _iTblUserBL.SelectTblUserTO(TblUserTO.IdUser);
                if (userTo == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "userTO Found NULL";
                    return resultMessage;
                }

                TblUserPwdHistoryTO tblUserPwdHistoryTO = new TblUserPwdHistoryTO();
                tblUserPwdHistoryTO.OldPwd = userTo.UserPasswd;
                tblUserPwdHistoryTO.NewPwd = TblUserTO.UserPasswd;
                tblUserPwdHistoryTO.CreatedOn = _iCommon.ServerDateTime;
                tblUserPwdHistoryTO.CreatedBy = TblUserTO.UserExtTO.CreatedBy; //Pass from GUI
                tblUserPwdHistoryTO.UserId = userTo.IdUser;

                userTo.UserPasswd = TblUserTO.UserPasswd;

                TblUserTO userTOForRabbitMessage = TblUserTO;
                return _iTblUserBL.UpdateTblUser(userTo, tblUserPwdHistoryTO, userTOForRabbitMessage);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error At API Level";
                return resultMessage;
            }
        }

        [Route("PostFeedback")]
        [HttpPost]
        public ResultMessage PostFeedback([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblFeedbackTO feedbackTO = JsonConvert.DeserializeObject<TblFeedbackTO>(data["feedbackTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (feedbackTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Feedback Object Found Null";
                    return returnMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                feedbackTO.CreatedOn = _iCommon.ServerDateTime;
                feedbackTO.CreatedBy = Convert.ToInt32(loginUserId);

                int result = _iTblFeedbackBL.InsertTblFeedback(feedbackTO);
                if (result == 1)
                {
                    returnMsg.MessageType = ResultMessageE.Information;
                    returnMsg.Result = 1;
                    returnMsg.Text = "Feedback Saved Successfully";
                    return returnMsg;
                }
                else
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "Error While InsertTblFeedback ";
                    return returnMsg;
                }

            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostFeedback";
                return returnMsg;
            }
        }

        [Route("PostLogOut")]
        [HttpPost]
        public int PostLogOut([FromBody] TblUserTO userTO)
        {
            try
            {

                if (userTO == null)
                {
                    return 0;
                }

                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblLoginBL.LogOut(userTO);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        [Route("PostLogOutForHrGrid")]
        [HttpPost]
        public int PostLogOutForHrGrid(Int32 IdUser)
        {
            try
            {

                if (IdUser == 0)
                {
                    return 0;
                }

                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblLoginBL.LogOutForHRGrid(IdUser);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        [Route("ActivateOrDeactivateUser")]
        [HttpPost]
        public int ActivateOrDeactivateUser([FromBody] JObject data)
        {
            int result = 0;
            try
            {
                TblUserTO userTO = JsonConvert.DeserializeObject<TblUserTO>(data["userTO"].ToString());

                if (userTO == null)
                {
                    return 0;
                }              
                result = _iTblUserBL.ActivateOrDeactivateUser(userTO);
                if (result!=1)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        [Route("GetModuleDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetModuleDropDownList()
        {
            List<DropDownTO> userList = _iTblModuleBL.SelectAllTblModuleList();
            return userList;
        }

        [Route("GetMappedModuleDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetMappedModuleDropDownList()
        {
            List<DropDownTO> userList = _iTblModuleBL.GetMappedModuleDropDownList();
            return userList;
        }

        /// <summary>
        /// Vijaymala [04-12-2018] added to select all role type list
        /// </summary>
        /// <param ></param>
        [Route("GetAllDimRoleTypeList")]
        [HttpGet]
        public List<DimRoleTypeTO> GetAllDimRoleTypeList()
        {
            return _iDimRoleTypeBL.SelectAllDimRoleTypeList();
        }

        [Route("PostUserAreaAllocation")]
        [HttpPost]
        public ResultMessage PostUserAreaAllocation([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<TblUserAreaAllocationTO> userAreaAllocationTOList = JsonConvert.DeserializeObject<List<TblUserAreaAllocationTO>>(data["userAreaAllocationTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (userAreaAllocationTOList == null || userAreaAllocationTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : userAreaAllocationTOList Found Null";
                    return returnMsg;
                }
                //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]
                var vRes = userAreaAllocationTOList.GroupBy(ele => ele.BrandId).Select(s => s.FirstOrDefault()).ToList();

                var vRes1 = userAreaAllocationTOList.GroupBy(ele => ele.DistrictId).Select(s => s.FirstOrDefault()).ToList();

                if (vRes.Count > 1 && vRes1.Count > 1)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "User Contains Brand and Area wise allocation .Either Select Area Allocation or Sales Engineer Allocation";
                    return returnMsg;

                }

                DateTime confirmedDate = _iCommon.ServerDateTime;
                for (int i = 0; i < userAreaAllocationTOList.Count; i++)
                {
                    userAreaAllocationTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    userAreaAllocationTOList[i].CreatedOn = confirmedDate;
                    userAreaAllocationTOList[i].IsActive = 1;
                }

                ResultMessage resMsg = _iTblUserAreaAllocationBL.SaveUserAreaAllocation(userAreaAllocationTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUserAreaAllocation";
                return returnMsg;
            }
        }

        [Route("PostUserOrRolePermission")]
        [HttpPost]
        public ResultMessage PostUserOrRolePermission([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                PermissionTO permissionTO = JsonConvert.DeserializeObject<PermissionTO>(data["permissionTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (permissionTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : permissionTO Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = _iCommon.ServerDateTime;
                permissionTO.CreatedBy = Convert.ToInt32(loginUserId);
                permissionTO.CreatedOn = confirmedDate;
                //Newly Added
                ResultMessage resMsg = new ResultMessage();
                if (permissionTO.GivePermissionToAllUser == 1)
                {
                    resMsg = _iTblSysElementsBL.SaveAllUserPermission(permissionTO);
                }
                else
                {
                    resMsg = _iTblSysElementsBL.SaveRoleOrUserPermission(permissionTO);

                }

                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUserOrRolePermission";
                return returnMsg;
            }
        }

        [Route("UpdateUserWiseDashboardView")]
        [HttpPost]
        public ResultMessage UpdateUserWiseDashboardView([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                PermissionTO permissionTO = JsonConvert.DeserializeObject<PermissionTO>(data.ToString());
                if (permissionTO.UserId <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (permissionTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : permissionTO Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = _iCommon.ServerDateTime;
                permissionTO.CreatedBy = Convert.ToInt32(permissionTO.UserId);
                permissionTO.CreatedOn = confirmedDate;

                ResultMessage resMsg = _iTblSysElementsBL.SaveRoleOrUserPermission(permissionTO);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUserOrRolePermission";
                return returnMsg;
            }
        }

        [Route("PostNewUser")]
        [HttpPost]
        public ResultMessage PostNewUser([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblUserTO userTO = JsonConvert.DeserializeObject<TblUserTO>(data["userTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (userTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : userTO Found Null";
                    return returnMsg;
                }

                int userId = Convert.ToInt32(loginUserId);
                ResultMessage resMsg = _iTblUserBL.SaveNewUser(userTO, userId);
                if(resMsg.Result > 0)
                {
                    resMsg.Tag = userTO.IdUser;
                }
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostNewUser";
                return returnMsg;
            }
        }

        [Route("PostUpdateUserDtl")]
        [HttpPost]
        public ResultMessage PostUpdateUserDtl([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblUserTO userTO = JsonConvert.DeserializeObject<TblUserTO>(data["userTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (userTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : userTO Found Null";
                    return returnMsg;
                }

                int userId = Convert.ToInt32(loginUserId);
                if (userTO.IsActive == 0)
                {
                    returnMsg = _iTblUserBL.PostProFlowUser(userTO, 2);
                    if (returnMsg == null || returnMsg.MessageType != ResultMessageE.Information)
                    {
                        return returnMsg;
                    }
                    userTO.DeactivatedBy = userId;
                    userTO.DeactivatedOn = _iCommon.ServerDateTime;
                    int result = _iTblUserBL.UpdateTblUser(userTO, returnMsg, userId);
                    if (result == 1)
                    {
                        returnMsg.MessageType = ResultMessageE.Information;
                        returnMsg.Result = 1;
                        returnMsg.Text = "Record Updated Successfully";
                        returnMsg.DisplayMessage = "Record Updated Successfully";
                        return returnMsg;
                    }
                    else
                    {
                        returnMsg.MessageType = ResultMessageE.Error;
                        returnMsg.Result = 0;
                        returnMsg.Text = "API : Error In Method UpdateTblUser";
                        returnMsg.DisplayMessage = Constants.DefaultErrorMsg;
                        return returnMsg;
                    }
                }
                else
                {
                    ResultMessage resMsg = _iTblUserBL.UpdateUser(userTO, userId);
                    return resMsg;
                }
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUpdateUserDtl";
                return returnMsg;
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) { }

        //Vijaymala[27-06-2018] Added To remove user allocation.
        [Route("PostRemoveUserAreaAllocation")]
        [HttpPost]
        public ResultMessage PostRemoveUserAreaAllocation([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                TblUserAreaAllocationTO tblUserAreaAllocationTO = JsonConvert.DeserializeObject<TblUserAreaAllocationTO>(data["userAreaAllocationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblUserAreaAllocationTO == null)
                {
                    resultMessage.DefaultBehaviour("tblUserAreaAllocationTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                tblUserAreaAllocationTO.IsActive = 0;
                tblUserAreaAllocationTO.UserId = Convert.ToInt32(loginUserId);
                result = _iTblUserAreaAllocationBL.UpdateTblUserAreaAllocation(tblUserAreaAllocationTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeleteState");
                return resultMessage;
            }
        }

        /// <summary>
        ///Sudhir[24-APR-2018] Added for Uploading Image  
        /// </summary>
        /// <param name="tblDocumentDetailsTOTblDocumentDetailsTO"></param>
        /// <returns></returns>
        [Route("UploadUserProfilePicture")]
        [HttpPost]
        public ResultMessage UploadUserProfilePicture([FromBody] JObject data)
        {
            TblDocumentDetailsTO tblDocumentDetailsTO = JsonConvert.DeserializeObject<TblDocumentDetailsTO>(data["data"].ToString());

            //TblDocumentDetailsTO tblDocumentDetailsTO = data;
            return _iTblDocumentDetailsBL.UploadUserProfilePicture(tblDocumentDetailsTO);
        }

        /// <summary>
        /// Priyanka [27-12-2018] : Added to post the user brand details.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUserBrandDetails")]
        [HttpPost]
        public ResultMessage PostUserBrandDetails([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblUserBrandTO tblUserBrandTO = JsonConvert.DeserializeObject<TblUserBrandTO>(data["userBrandTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (tblUserBrandTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : userTO Found Null";
                    return returnMsg;
                }

                int userId = Convert.ToInt32(loginUserId);
                ResultMessage resMsg = _iTblUserBrandBL.SaveUserWithBrand(tblUserBrandTO, userId);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUserBrandDetails";
                return returnMsg;
            }
        }

        /// <summary>
        /// Priyanka [27-12-2018] : Added to post the user brand details.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeleteUserBrandDetails")]
        [HttpPost]
        public ResultMessage PostDeleteUserBrandDetails([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblUserBrandTO tblUserBrandTO = JsonConvert.DeserializeObject<TblUserBrandTO>(data["userBrandTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (tblUserBrandTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : userTO Found Null";
                    return returnMsg;
                }

                int userId = Convert.ToInt32(loginUserId);
                ResultMessage resMsg = _iTblUserBrandBL.UpdateTblUserBrandDetails(tblUserBrandTO, userId);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUserBrandDetails";
                return returnMsg;
            }
        }


        /// <summary>
        /// code added by swati
        /// Assign supplier to purchase manager
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostSupplierToPurchaseManager")]
        [HttpPost]
        public ResultMessage PostSupplierToPurchaseManager([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<TblPurchaseManagerSupplierTO> assignSupplierList = JsonConvert.DeserializeObject<List<TblPurchaseManagerSupplierTO>>(data["permissionTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }
                int result = 0;
                DateTime serverDate = _iCommon.ServerDateTime;
                var userId = data["userId"].ToString();
                if (assignSupplierList != null && assignSupplierList.Count > 0)
                {
                    for (int q = 0; q < assignSupplierList.Count; q++)
                    {
                        TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO = new TblPurchaseManagerSupplierTO();
                        tblPurchaseManagerSupplierTO.UserId = Convert.ToInt32(userId);
                        tblPurchaseManagerSupplierTO.OrganizationId = assignSupplierList[q].OrganizationId;
                        tblPurchaseManagerSupplierTO.CreatedOn = serverDate;
                        tblPurchaseManagerSupplierTO.CreatedBy = Convert.ToInt32(loginUserId);
                        tblPurchaseManagerSupplierTO.IsActive = (assignSupplierList[q].IsChecked ? 1 : 0);
                        result = _iTblPurchaseManagerSupplierBL.InsertUpdateTblPurchaseManagerSupplier(tblPurchaseManagerSupplierTO);
                    }

                }

                if (result == 1)
                {
                    returnMsg.MessageType = ResultMessageE.Information;
                    returnMsg.Result = 1;
                    returnMsg.Text = "Suppliers Updated Successfully";
                    returnMsg.DisplayMessage = "Suppliers Updated Successfully";

                    return returnMsg;
                }
                else
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API :Record Not Saved";
                    return returnMsg;
                }

            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUserOrRolePermission";
                return returnMsg;
            }
        }

        [Route("PostEditAttributeNameByLanguage")]
        [HttpPost]
        public int PostEditAttributeNameByLanguage([FromBody] TblCRMLabelTO tblCRMLabelTO)
        {
            return _iTblAttributeStatusBL.PostEditAttributeNameByLanguage(tblCRMLabelTO);
        }

        [Route("PostSetModeIdAgainstModule")]
        [HttpPost]
        public ResultMessage PostSetModeIdAgainstModule([FromBody] JObject data)
        {
            int moduleId =Convert.ToInt32(data["moduleId"].ToString());

            return _iTblModuleBL.PostSetModeIdAgainstModule(moduleId);
        }


        #endregion

        #region PUT

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        #endregion

        #region DELETE

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }

        #endregion

        #region OTHER FUNCTION
        [Route("EncryptPassword")]
        [HttpGet]
        public string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                //var saltedPassword = string.Format("{0}{1}", salt, password);
                var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }

        [Route("Encrypt")]
        [HttpGet]
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        [Route("Decrypt")]
        [HttpGet]
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        /// <summary>
        ///Sudhir[24-APR-2018] Added for Uploading Image  
        /// </summary>
        /// <param name="tblDocumentDetailsTOTblDocumentDetailsTO"></param>
        /// <returns></returns>
        [Route("UploadUserProfilePictureAsync")]
        [HttpPost]
        public async Task<ResultMessage> UploadUserProfilePictureAsync([FromBody] JObject data)
        {

            TblDocumentDetailsTO tblDocumentDetailsTO = JsonConvert.DeserializeObject<TblDocumentDetailsTO>(data["data"].ToString());
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                Task<ResultMessage> resultMessag = _iTblDocumentDetailsBL.UploadFileAsync(tblDocumentDetailsTO);
                resultMessage = await resultMessag;
                return resultMessage;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// Get birthday or anniversary notifications - Tejaswini
        [Route("GetBirthdays/{Date}/{UpcomingDays}/{IsBirthday}")]
        [HttpGet]
        public List<BirthdayAlertTO> GetTodaysBirthdays(string Date, Int32 UpcomingDays, Int32 IsBirthday)
        {
            DateTime date = DateTime.MinValue;
            if (Date != null)
            {
                //fromDate = DateTimeOffset.Parse(FromDate).UtcDateTime;
                date = Convert.ToDateTime(Date);
            }
            return _iTblPersonBL.SelectAllPersonBirthday(date, UpcomingDays, IsBirthday);
        }

        /// <summary>
        /// Harshala [20-07-2019] : Added to give all permissions to user or role.
        /// </summary>
        /// <returns></returns>
        [Route("giveAllPermission")]
        [HttpPost]
        public ResultMessage giveAllPermission([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                PermissionTO permissionTO = JsonConvert.DeserializeObject<PermissionTO>(data["permissionTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (permissionTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : permissionTO Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = _iCommon.ServerDateTime;
                permissionTO.CreatedBy = Convert.ToInt32(loginUserId);
                permissionTO.CreatedOn = confirmedDate;

                ResultMessage resMsg = _iTblSysElementsBL.SavegiveAllPermission(permissionTO);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUserOrRolePermission";
                return returnMsg;
            }
        }


        /// <summary>
        /// Harshala [27-08-2019] :added to copy permissions of selected role or user to another role or user.
        /// </summary>
        /// <returns></returns>
        [Route("CopyAllPermission")]
        [HttpPost]
        public ResultMessage CopyAllPermission([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                CopyFromToPermissionTO copyFromToPermissionTO = JsonConvert.DeserializeObject<CopyFromToPermissionTO>(data["copyFromToPermissionTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }
                if (copyFromToPermissionTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : copyFromToPermissionTO Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = _iCommon.ServerDateTime;
                copyFromToPermissionTO.CreatedBy = Convert.ToInt32(loginUserId);
                copyFromToPermissionTO.CreatedOn = confirmedDate;


                ResultMessage resMsg = _iTblSysElementsBL.SaveCopyPermissions(copyFromToPermissionTO);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While CopyAllPermission";
                return returnMsg;
            }
        }



        /// <summary>
        /// Harshala [26-07-2019] : Added to check all permissions.
        /// </summary>
        /// <returns></returns>
        [Route("CheckAllPermissionGiven")]
        [HttpGet]
        public int CheckAllPermissionGiven(int roleId, int userId)
        {
            return _iTblSysElementsBL.checkUserOrRolePermissions(roleId, userId);
        }

        /// <summary>
        /// Harshala : [02/08/2019] added to get permissions with respect to user or role.
        /// </summary>
        /// <returns></returns>
        [Route("GetPermissionswrtRole")]
        [HttpGet]
        public List<tblViewPermissionTO> GetPermissionswrtRole(int roleId, int userId)
        {
            return _iTblSysElementsBL.selectPermissionswrtRole(roleId, userId);

        }

        /// <summary>
        /// Harshala [08-08-2019] : Added to get all permissions for dropdown.
        /// </summary>
        /// <returns></returns>
        [Route("GetAllPermissionDropdownList")]
        [HttpGet]
        public List<DropDownTO> GetAllPermissionDropdownList()
        {
            List<DropDownTO> permissionList = _iTblSysElementsBL.SelectAllPermissionDropdownList();
            return permissionList;
        }


        /// <summary>
        /// Harshala [08-08-2019] : Added to get user and role which have selected permission
        /// </summary>
        /// <returns></returns>
        [Route("GetUserRolewrtPermission")]
        [HttpGet]
        public tblViewPermissionTO GetUserRolewrtPermission(int idSysElement)
        {
            tblViewPermissionTO UserRoleList = _iTblSysElementsBL.SelectAllUserRolewrtPermission(idSysElement);
            return UserRoleList;
        }


        /// <summary>
        /// swati pisal
        /// Get List of all supplier list
        /// </summary>
        /// <param name="userId"></param> Here userId means PMId
        /// <returns></returns>
        [Route("GetSupplierAndPurchaseManagerList")]
        [HttpGet]
        public List<TblPurchaseManagerSupplierTO> GetSupplierAndPurchaseManagerList(int userId)
        {
            return _iTblPurchaseManagerSupplierBL.SelectAllActivePurchaseManagerSupplierList(userId);
        }

        /// <summary>
        /// Swati Pisal
        /// Get Purchase List
        /// </summary>
        /// <returns></returns>
        [Route("GetActivePurchaseDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetActivePurchaseDropDownList()
        {
            List<DropDownTO> userList = _iTblPurchaseManagerSupplierBL.SelectPurchaseFromRoleForDropDown();
            return userList;
        }

        [Route("GetSupplierByPMDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetSupplierByPMDropDownList(int userId)
        {
            List<DropDownTO> userList = _iTblPurchaseManagerSupplierBL.GetSupplierByPMDropDownList(userId);
            return userList;
        }


        /// <summary>
        /// Harshala [17-10-2019] added to get all attribute status List to Show hide attributes on org form
        /// </summary>
        /// <returns></returns>
        [Route("GetAllAttributeStatusList")]
        [HttpGet]
        public List<AttributeStatusTO> GetAllAttributeStatusList(Int32 pageId, int orgTypeId,int userId = 0)
        {
            //Added by minal For first Priority to AttributeDisplayName in AttributeStatus
            return _iTblAttributeStatusBL.GetAllAttributeStatusList(pageId,orgTypeId, userId);
        }


        /// <summary>
        /// hrushikesh [28-11-2019] added to get all attribute  List to Show UI
        /// </summary>
        /// <returns></returns>
        [Route("GetAllAttributeList")]
        [HttpGet]
        public List<AttributeStatusTO> GetAllAttributeList(Int32 pageId, int srcId = 0)
        {
            //Added by minal For first Priority to AttributeDisplayName in AttributeStatus
            return _iTblAttributeStatusBL.AllAttributeListForUI(pageId, srcId);
        }


        /// <summary>
        /// hrushikesh [28-11-2019] added to get AttributePageList
        /// </summary>
        /// <returns></returns>
        [Route("GetAllAttPageList")]
        [HttpGet]
        public List<AttributePageTO> GetAllAttributePages()
        {
            return _iTblAttributeStatusBL.AllAttributePages();
        }


        [Route("InsertAttributeList")]
        [HttpPost]
        public ResultMessage HideShowAttributes([FromBody] AttributeStatusTO attributeTO)
        {
            //Added by minal For first Priority to AttributeDisplayName in AttributeStatus
            return _iTblAttributeStatusBL.InsertAttributeList(attributeTO);
        }



        [Route("InsertAttributeMultiList")]
        [HttpPost]
        public ResultMessage HideShowMutliAttributes([FromBody] List<AttributeStatusTO> attributeTO)
        {
            return _iTblAttributeStatusBL.InsertAttributeMultiList(attributeTO);
        }




        /// <summary>
        /// hrushikesh [28-11-2019] added to get AttributePageList
        /// </summary>
        /// <returns></returns>
        [Route("GetAllAttSrcList")]
        [HttpPost]
        public List<DropDownTO> GetAttributeSrcList([FromBody] AttributePageTO attrSrcTO)
        {
            return _iTblAttributeStatusBL.GetAttributeSrcList(attrSrcTO);
        }


        /// <summary>
        /// Harshala [05-12-2019] added to post Edited Attribute Display Name
        /// </summary>
        /// <returns></returns>
        [Route("PostEditAttributeName")]
        [HttpPost]
        public int PostEditAttributeName([FromBody] AttributeStatusTO attributeTO)
        {
            //Added by minal For first Priority to AttributeDisplayName in AttributeStatus
            return _iTblAttributeStatusBL.PostEditAttributeName(attributeTO);
        }


        /// <summary>
        /// Harshala [04-11-2020] added to get  attribute status by Name
        /// </summary>
        /// <returns></returns>
        [Route("GeAttributesByName")]
        [HttpGet]
        public TblAttributesTO GeAttributesByName(string attributeName)
        {
            return _iTblAttributeStatusBL.SelectAttributesByName(attributeName);
        }

        #endregion

        #region Whishlist User Login

        [Route("PostLoginWishlist")]
        [HttpPost]
        public ResultMessage PostLoginWishlist([FromBody] TblUserTO userTO)
        {
            try
            {
                if (userTO == null)
                {
                    return null;
                }

                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblLoginBL.LogInWishlist(userTO);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    return rMessage;
                }
                else
                {
                    return rMessage;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
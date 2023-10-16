using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.BL.Interfaces;
using System.Linq;
using ODLMWebAPI.BL;

namespace simpliMASTERSAPI.BL
{
    public class TblQuotaDeclarationBL : ITblQuotaDeclarationBL
    {
        private readonly ITblGlobalRateBL _tblGlobalRateBL;
        private readonly ITblQuotaDeclarationDAO _tblQuotaDeclarationDAO;
        private readonly ITblPersonBL _tblPersonBL;
        private readonly ITblConfigParamsBL _tblConfigParamsBL;
        private readonly ITblUserBL _tblUserBL;
        private readonly ITblAlertInstanceBL _tblAlertInstanceBL;
        private readonly ITblBookingActionsBL _tblBookingActionsBL;

        public TblQuotaDeclarationBL(ITblGlobalRateBL tblGlobalRateBL, ITblQuotaDeclarationDAO tblQuotaDeclarationDAO, ITblPersonBL tblPersonBL, ITblConfigParamsBL tblConfigParamsBL, ITblUserBL tblUserBL, ITblAlertInstanceBL tblAlertInstanceBL, ITblBookingActionsBL tblBookingActionsBL) 
        {
            _tblGlobalRateBL= tblGlobalRateBL;
            _tblQuotaDeclarationDAO = tblQuotaDeclarationDAO;
            _tblPersonBL = tblPersonBL;
            _tblConfigParamsBL= tblConfigParamsBL;
            _tblUserBL= tblUserBL;
            _tblAlertInstanceBL = tblAlertInstanceBL;
            _tblBookingActionsBL = tblBookingActionsBL;
        }
        public int SaveDeclaredRateAndAllocatedQuota(List<TblQuotaDeclarationTO> quotaExtList, List<TblQuotaDeclarationTO> quotaList, TblGlobalRateTO tblGlobalRateTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save the Declared Rate

                Boolean isRateAlreadyDeclare = _tblGlobalRateBL.IsRateAlreadyDeclaredForTheDate(tblGlobalRateTO.CreatedOn, conn, tran);

                //This condition means if new declared quota is not found then new rate can not be declared
                if (quotaList != null && quotaList.Count > 0)
                {
                    if (tblGlobalRateTO.RateReasonDesc != "Other")
                        tblGlobalRateTO.Comments = tblGlobalRateTO.RateReasonDesc;

                    result = _tblGlobalRateBL.InsertTblGlobalRate(tblGlobalRateTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                #endregion

                #region 2. Deactivate All Previous Declared Quota

                result = _tblQuotaDeclarationDAO.DeactivateAllDeclaredQuota(tblGlobalRateTO.CreatedBy, conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    return 0;
                }

                #endregion

                #region 3. Update Existing Quota for Validity

                for (int i = 0; i < quotaExtList.Count; i++)
                {
                    TblQuotaDeclarationTO tblQuotaDeclarationTO = quotaExtList[i];

                    result = _tblQuotaDeclarationDAO.UpdateQuotaDeclarationValidity(tblQuotaDeclarationTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                #endregion

                #region 4. Save C&F Allocated Quota

                List<TblSmsTO> smsTOList = new List<TblSmsTO>();
                for (int i = 0; i < quotaList.Count; i++)
                {
                    TblQuotaDeclarationTO tblQuotaDeclarationTO = quotaList[i];
                    tblQuotaDeclarationTO.GlobalRateId = tblGlobalRateTO.IdGlobalRate;
                    tblQuotaDeclarationTO.BalanceQty = tblQuotaDeclarationTO.AllocQty;
                    tblQuotaDeclarationTO.CalculatedRate = tblGlobalRateTO.Rate - tblQuotaDeclarationTO.RateBand;
                    result = _tblQuotaDeclarationDAO.InsertTblQuotaDeclaration(tblQuotaDeclarationTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }

                    //[17/01/2018]Added to send sms for cnf subsidary number
                    List<String> mobileNoList = new List<String>();

                    if (tblQuotaDeclarationTO.Tag != null && tblQuotaDeclarationTO.Tag.GetType() == typeof(TblOrganizationTO))
                    {
                        //if (!string.IsNullOrEmpty(((TblOrganizationTO)tblQuotaDeclarationTO.Tag).RegisteredMobileNos))
                        if (true)
                        {
                            List<TblPersonTO> tblPersonTOList = _tblPersonBL.SelectAllPersonListByOrganizationId(tblQuotaDeclarationTO.OrgId);

                            if (tblPersonTOList == null || tblPersonTOList.Count == 0)
                            {
                                tblPersonTOList = new List<TblPersonTO>();
                            }
                            TblPersonTO tblPersonTORegMobNo = new TblPersonTO();
                            tblPersonTORegMobNo.MobileNo = ((TblOrganizationTO)tblQuotaDeclarationTO.Tag).RegisteredMobileNos;

                            if (!String.IsNullOrEmpty(tblPersonTORegMobNo.MobileNo))
                            {
                                tblPersonTOList.Add(tblPersonTORegMobNo);
                            }

                            if (tblPersonTOList != null && tblPersonTOList.Count > 0)
                            {
                                for (int k = 0; k < tblPersonTOList.Count; k++)
                                {

                                    if (!mobileNoList.Contains(tblPersonTOList[k].MobileNo))
                                    {
                                        TblSmsTO smsTO = new TblSmsTO();
                                        smsTO.MobileNo = tblPersonTOList[k].MobileNo;
                                        PrepareSmsObject(tblGlobalRateTO, isRateAlreadyDeclare, tblQuotaDeclarationTO, mobileNoList, smsTO);
                                        smsTOList.Add(smsTO);
                                    }
                                    if (!mobileNoList.Contains(tblPersonTOList[k].AlternateMobNo))
                                    {
                                        TblSmsTO smsTO = new TblSmsTO();
                                        smsTO.MobileNo = tblPersonTOList[k].AlternateMobNo;
                                        PrepareSmsObject(tblGlobalRateTO, isRateAlreadyDeclare, tblQuotaDeclarationTO, mobileNoList, smsTO);
                                        smsTOList.Add(smsTO);
                                    }
                                }
                            }
                        }
                    }
                }
                //[17/01/2018]Added to send sms for role manager,director,loading person mobile number


                //These Role are use to send reason other wise use alert configuration to send sms
                Dictionary<Int32, List<string>> roleDCT = new Dictionary<int, List<string>>();

                //Commented by PRIYANKA [22-08-2018] and added the rolesIds on the setting basis.
                //String roleIds = ((int) Constants.SystemRolesE.DIRECTOR + "," + (int)Constants.SystemRolesE.LOADING_PERSON + "," + (int)Constants.SystemRolesE.REGIONAL_MANAGER);

                string roleIds = string.Empty;
                TblConfigParamsTO tblConfigParamsTO = _tblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ROLES_TO_SEND_SMS_ABOUT_RATE_AND_QUOTA);
                if (tblConfigParamsTO != null)
                {
                    roleIds = tblConfigParamsTO.ConfigParamVal;
                }

                roleDCT = _tblUserBL.SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(roleIds, false, conn, tran);

                if (roleDCT != null)
                {
                    foreach (var item in roleDCT.Keys)
                    {
                        List<string> list = roleDCT[item];
                        if (list != null && list.Count > 0)
                        {
                            for (int mn = 0; mn < list.Count; mn++)
                            {
                                TblSmsTO smsTOExist = smsTOList.Where(w => w.MobileNo == list[mn]).FirstOrDefault();
                                if (smsTOExist == null)
                                {
                                    TblSmsTO smsTO = new TblSmsTO();
                                    smsTO.MobileNo = list[mn];
                                    smsTO.SourceTxnDesc = "Quota & Rate Declaration";
                                    String reasonDesc = tblGlobalRateTO.RateReasonDesc;
                                    if (tblGlobalRateTO.RateReasonDesc == "Other")
                                        reasonDesc = tblGlobalRateTO.Comments;

                                    if (isRateAlreadyDeclare)
                                        smsTO.SmsTxt = "New Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc + " , Your Quota : " + 0 + " MT";
                                    else
                                        smsTO.SmsTxt = "Today's Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc + " , Your Quota : " + 0 + " MT";

                                    smsTOList.Add(smsTO);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region 5. Send Notifications Via SMS Or Email To All C&F

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.NEW_RATE_AND_QUOTA_DECLARED;
                tblAlertInstanceTO.AlertAction = "NEW_RATE_AND_QUOTA_DECLARED";
                if (!isRateAlreadyDeclare)
                    tblAlertInstanceTO.AlertComment = "Today's Rate and Quota is Declared. Rate = " + tblGlobalRateTO.Rate + " (Rs/MT)";
                else
                    tblAlertInstanceTO.AlertComment = "New Rate and Quota is Declared. Rate = " + tblGlobalRateTO.Rate + " (Rs/MT)";

                tblAlertInstanceTO.EffectiveFromDate = tblGlobalRateTO.CreatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "NEW_RATE_AND_QUOTA_DECLARED";
                tblAlertInstanceTO.SourceEntityId = tblGlobalRateTO.IdGlobalRate;
                tblAlertInstanceTO.RaisedBy = tblGlobalRateTO.CreatedBy;
                tblAlertInstanceTO.RaisedOn = tblGlobalRateTO.CreatedOn;
                tblAlertInstanceTO.IsAutoReset = 1;
                if (smsTOList != null)
                {
                    tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                    tblAlertInstanceTO.SmsTOList = smsTOList;
                }

                String alertDefIds = (int)NotificationConstants.NotificationsE.NEW_RATE_AND_QUOTA_DECLARED + "," + (int)NotificationConstants.NotificationsE.BOOKINGS_CLOSED;
                result = _tblAlertInstanceBL.ResetAlertInstanceByDef(alertDefIds, conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    return 0;
                }

                ResultMessage rMessage = _tblAlertInstanceBL.SaveNewAlertInstanceForDelevery(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return 0;
                }
                #endregion

                #region 6. Update booking Status As OPEN

                TblBookingActionsTO existinBookingActionsTO = _tblBookingActionsBL.SelectLatestBookingActionTO(conn, tran);
                if (existinBookingActionsTO == null || existinBookingActionsTO.BookingStatus == "CLOSE")
                {
                    TblBookingActionsTO bookingActionTO = new TblBookingActionsTO();
                    bookingActionTO.BookingStatus = "OPEN";
                    bookingActionTO.IsAuto = 1;
                    bookingActionTO.StatusBy = tblGlobalRateTO.CreatedBy;
                    bookingActionTO.StatusDate = tblGlobalRateTO.CreatedOn;

                    result = _tblBookingActionsBL.InsertTblBookingActions(bookingActionTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }
                #endregion

                tran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }


        private static void PrepareSmsObject(TblGlobalRateTO tblGlobalRateTO, bool isRateAlreadyDeclare, TblQuotaDeclarationTO tblQuotaDeclarationTO, List<string> mobileNoList, TblSmsTO smsTO)
        {
            smsTO.SourceTxnDesc = "Quota & Rate Declaration";
            String reasonDesc = tblGlobalRateTO.RateReasonDesc;
            if (tblGlobalRateTO.RateReasonDesc == "Other")
                reasonDesc = tblGlobalRateTO.Comments;

            if (isRateAlreadyDeclare)
                smsTO.SmsTxt = "New Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc + " , Your Quota : " + tblQuotaDeclarationTO.AllocQty + " MT";
            else
                smsTO.SmsTxt = "Today's Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc + " , Your Quota : " + tblQuotaDeclarationTO.AllocQty + " MT";

            mobileNoList.Add(smsTO.MobileNo);
        }
    }
}

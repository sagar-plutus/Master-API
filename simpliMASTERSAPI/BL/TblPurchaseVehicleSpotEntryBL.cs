using ODLMWebAPI.BL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TO;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.Models;
using ODLMWebAPI.TO;

namespace ODLMWebAPI.BL
{
    public class TblPurchaseVehicleSpotEntryBL : ITblPurchaseVehicleSpotEntryBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly INotification notify;
        private readonly ITblRecycleDocumentBL _iTblRecycleDocumentBL;
        private readonly ITblSpotVehicleMaterialDtlsBL _iTblSpotVehicleMaterialDtlsBL;
        private readonly ITblPurchaseManagerSupplierBL _iTblPurchaseManagerSupplierBL;
        private readonly ITblSpotVehicleMaterialDtlsDAO _iTblSpotVehicleMaterialDtlsDAO;
        private readonly ITblPurchaseVehicleSpotEntryDAO _iTblPurchaseVehicleSpotEntryDAO;
        private readonly ICommon _iCommonDAO;

        public TblPurchaseVehicleSpotEntryBL(

            INotification inotify,
            IConnectionString iConnectionString,
            ICommon icommondao, ITblRecycleDocumentBL iTblRecycleDocumentBL,
            ITblPurchaseVehicleSpotEntryDAO iTblPurchaseVehicleSpotEntryDAO, ITblSpotVehicleMaterialDtlsDAO iTblSpotVehicleMaterialDtlsDAO
                                             , ITblPurchaseManagerSupplierBL iTblPurchaseManagerSupplierBL, ITblSpotVehicleMaterialDtlsBL iTblSpotVehicleMaterialDtlsBL)
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblSpotVehicleMaterialDtlsBL = iTblSpotVehicleMaterialDtlsBL;
            notify = inotify;
            _iTblRecycleDocumentBL = iTblRecycleDocumentBL;
            _iTblPurchaseManagerSupplierBL = iTblPurchaseManagerSupplierBL;
            _iTblSpotVehicleMaterialDtlsDAO = iTblSpotVehicleMaterialDtlsDAO;
            _iTblPurchaseVehicleSpotEntryDAO = iTblPurchaseVehicleSpotEntryDAO;
        }
        #region Selection
        public List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntry()
        {
            return _iTblPurchaseVehicleSpotEntryDAO.SelectAllTblPurchaseVehicleSpotEntry();
        }


        // public  List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseVehicleSpotEntryList()
        // {
        //     return _iTblPurchaseVehicleSpotEntryDAO.SelectAllTblPurchaseVehicleSpotEntryList();
        // }

        public List<VehicleNumber> SelectAllVehicles()
        {
            return _iTblPurchaseVehicleSpotEntryDAO.SelectAllVehicles();
        }

        public TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTOByRootId(Int32 rootScheduleId)
        {
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryReturnTO = new TblPurchaseVehicleSpotEntryTO();
            tblPurchaseVehicleSpotEntryReturnTO = _iTblPurchaseVehicleSpotEntryDAO.SelectTblPurchaseVehicleSpotEntryTOByRootId(rootScheduleId);
            return tblPurchaseVehicleSpotEntryReturnTO;

        }


        public TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTO(Int32 idVehicleSpotEntry)
        {
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryReturnTO = new TblPurchaseVehicleSpotEntryTO();
            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = _iTblPurchaseVehicleSpotEntryDAO.SelectTblPurchaseVehicleSpotEntry(idVehicleSpotEntry);

            if (tblPurchaseVehicleSpotEntryTOList != null && tblPurchaseVehicleSpotEntryTOList.Count == 1)
            {
                tblPurchaseVehicleSpotEntryReturnTO = tblPurchaseVehicleSpotEntryTOList[0];
                tblPurchaseVehicleSpotEntryReturnTO.SpotVehMatDtlsTOList = _iTblSpotVehicleMaterialDtlsBL.SelectAllSpotVehMatDtlsBySpotVehId(tblPurchaseVehicleSpotEntryReturnTO.IdVehicleSpotEntry);
                return tblPurchaseVehicleSpotEntryReturnTO;
            }
            else
                return null;
        }

        public TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTO(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran)
        {
            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = _iTblPurchaseVehicleSpotEntryDAO.SelectTblPurchaseVehicleSpotEntry(idVehicleSpotEntry, conn, tran);
            if (tblPurchaseVehicleSpotEntryTOList != null && tblPurchaseVehicleSpotEntryTOList.Count == 1)
            {
                return tblPurchaseVehicleSpotEntryTOList[0];
            }
            else
                return null;
        }

        public TblPurchaseVehicleSpotEntryTO GetSpotEntryVehicleDetailsWithMaterials(Int32 idVehicleSpotEntry)
        {
            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = _iTblPurchaseVehicleSpotEntryDAO.SelectTblPurchaseVehicleSpotEntry(idVehicleSpotEntry);
            if (tblPurchaseVehicleSpotEntryTOList != null && tblPurchaseVehicleSpotEntryTOList.Count == 1)
            {
                List<TblSpotVehMatDtlsTO> tblSpotVehMatDtlsList = _iTblSpotVehicleMaterialDtlsDAO.SelectTblSpotVehMatDtls(idVehicleSpotEntry);
                if (tblSpotVehMatDtlsList != null && tblSpotVehMatDtlsList.Count > 0)
                {
                    tblPurchaseVehicleSpotEntryTOList[0].SpotVehMatDtlsTOList = tblSpotVehMatDtlsList;
                }
                else
                    tblPurchaseVehicleSpotEntryTOList[0].SpotVehMatDtlsTOList = new List<TblSpotVehMatDtlsTO>();

                return tblPurchaseVehicleSpotEntryTOList[0];
            }

            else
                return null;
        }

        public List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotEntryVehicles(DateTime fromDate, DateTime toDate,Int32 moduleId, String loginUserId, Int32 id, bool skipDatetime)
        {
            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = _iTblPurchaseVehicleSpotEntryDAO.SelectAllSpotEntryVehicles(fromDate, toDate,moduleId, loginUserId, id, skipDatetime);
            if (tblPurchaseVehicleSpotEntryTOList != null && tblPurchaseVehicleSpotEntryTOList.Count > 0)
            {
                if(moduleId == 10)
                {
                    for(int i=0;i< tblPurchaseVehicleSpotEntryTOList.Count;i++)
                    {
                        List<TblPODetailsAgainstSpotEntryTO> list = _iTblPurchaseVehicleSpotEntryDAO.selectPODetailsAgainstSpotEntry(tblPurchaseVehicleSpotEntryTOList[i].IdVehicleSpotEntry);

                        if(list != null && list.Count > 0)
                        {
                            tblPurchaseVehicleSpotEntryTOList[i].TblPODetailsAgainstSpotEntryList = new List<TblPODetailsAgainstSpotEntryTO>();
                            tblPurchaseVehicleSpotEntryTOList[i].PoNo = "";
                            for (int j=0;j<list.Count;j++)
                            {
                                tblPurchaseVehicleSpotEntryTOList[i].TblPODetailsAgainstSpotEntryList.Add(list[j]);
                                if (tblPurchaseVehicleSpotEntryTOList[i].PoNo == "")
                                {
                                    tblPurchaseVehicleSpotEntryTOList[i].PoNo = list[j].PoNo;
                                }
                                else
                                {
                                    tblPurchaseVehicleSpotEntryTOList[i].PoNo = tblPurchaseVehicleSpotEntryTOList[i].PoNo+","+ list[j].PoNo;
                                }
                            }
                        }
                    }
                }
                //Prajakta[2019-03-08] Commented for performance issue
                // for (int i = 0; i < tblPurchaseVehicleSpotEntryTOList.Count; i++)
                // {
                //     if (tblPurchaseVehicleSpotEntryTOList[i].PurchaseEnquiryId > 0)
                //         tblPurchaseVehicleSpotEntryTOList[i].BookingTO = BL.TblPurchaseEnquiryBL.SelectTblPurchaseEnquiry(tblPurchaseVehicleSpotEntryTOList[i].PurchaseEnquiryId);
                // }
            }
            return tblPurchaseVehicleSpotEntryTOList;
        }


        public DropDownTO SelectAllSpotEntryVehiclesCount(int pmId, int supplierId, int materialTypeId)
        {
            DropDownTO DropDownTOCount = _iTblPurchaseVehicleSpotEntryDAO.SelectAllSpotEntryVehiclesCount(pmId, supplierId, materialTypeId);
            return DropDownTOCount;
        }



        /// <summary>
        /// Prajakta [21 Sept 2018] To Save Spot entry Vehicles
        /// </summary>
        /// <param name="tblPurchaseVehicleSpotEntryTO"></param>
        /// <returns></returns>
        public ResultMessage SaveVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime createdDate = _iCommonDAO.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region Priyanka [14-03-2019] Added to check whether the same vehicle number is already present in premises.
                tblPurchaseVehicleSpotEntryTO.VehicleNo = tblPurchaseVehicleSpotEntryTO.VehicleNo.Trim();
                List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = SelectAllSpotEntryVehiclesPending(tblPurchaseVehicleSpotEntryTO.VehicleNo);
                if (tblPurchaseVehicleSpotEntryTOList != null && tblPurchaseVehicleSpotEntryTOList.Count > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DisplayMessage = "Vehicle No. - " + tblPurchaseVehicleSpotEntryTOList[0].VehicleNo + " is already spotted with VehicleId  #" + tblPurchaseVehicleSpotEntryTOList[0].IdVehicleSpotEntry;
                    return resultMessage;
                }
                #endregion

                #region Priyanka [04-03-2019] Added to check whether the vehicle is already in process.
                List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
                tblPurchaseScheduleSummaryTOList = _iTblRecycleDocumentBL.GetPurchaseScheduleSummaryTOByVehicleNo(tblPurchaseVehicleSpotEntryTO.VehicleNo, 0);
                if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DisplayMessage = "Vehicle is already entered in premises.";
                    return resultMessage;
                }
                #endregion

                #region 1. Insert Spot Entry Data in Table

                tblPurchaseVehicleSpotEntryTO.CreatedOn = createdDate;
                tblPurchaseVehicleSpotEntryTO.StatusDate = createdDate;
                tblPurchaseVehicleSpotEntryTO.CreatedBy = Convert.ToInt32(loginUserId);

                result = InsertTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error While InsertTblPurchaseVehicleSpotEntry : SaveVehicleSpotEntry";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                else
                {
                    TblSpotVehMatDtlsTO tblSpotVehMatDtlsTO = new TblSpotVehMatDtlsTO();
                    if (tblPurchaseVehicleSpotEntryTO.SpotVehMatDtlsTOList != null && tblPurchaseVehicleSpotEntryTO.SpotVehMatDtlsTOList.Count > 0)
                    {
                        for (int i = 0; i < tblPurchaseVehicleSpotEntryTO.SpotVehMatDtlsTOList.Count; i++)
                        {
                            tblPurchaseVehicleSpotEntryTO.SpotVehMatDtlsTOList[i].VehSpotEntryId = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                            tblSpotVehMatDtlsTO = tblPurchaseVehicleSpotEntryTO.SpotVehMatDtlsTOList[i];
                            result = _iTblSpotVehicleMaterialDtlsBL.InsertTblSpotVehicleMaterialDtls(tblSpotVehMatDtlsTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                throw new Exception("Error while inserting in - InsertTblSpotVehicleMaterialDtls");
                            }

                        }
                    }

                }

                #endregion
                if(result >= 1)
                {
                    if(tblPurchaseVehicleSpotEntryTO.TblPODetailsAgainstSpotEntryList != null && tblPurchaseVehicleSpotEntryTO.TblPODetailsAgainstSpotEntryList.Count>0)
                    {
                        for (int dtls = 0; dtls < tblPurchaseVehicleSpotEntryTO.TblPODetailsAgainstSpotEntryList.Count; dtls++)
                        {
                            TblPODetailsAgainstSpotEntryTO TODetails = new TblPODetailsAgainstSpotEntryTO();
                            TODetails = tblPurchaseVehicleSpotEntryTO.TblPODetailsAgainstSpotEntryList[dtls];
                            TODetails.CreatedOn = createdDate;
                            TODetails.PurchaseVehicleSpotEntryId  = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                            result = InsertTblPurchasePOSpotEntryDetails(TODetails,conn,tran);
                        }
                    }
                   
                }


                //Prajakta[2018-12-30] Added to save uploaded image list
                #region  Save Uploaded Images


                #endregion
                if (result >= 1)
                {
                    if (tblPurchaseVehicleSpotEntryTO.RecycleDocumentsTOList != null && tblPurchaseVehicleSpotEntryTO.RecycleDocumentsTOList.Count > 0)
                    {
                        for (int p = 0; p < tblPurchaseVehicleSpotEntryTO.RecycleDocumentsTOList.Count; p++)
                        {
                            tblPurchaseVehicleSpotEntryTO.RecycleDocumentsTOList[p].TxnId = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                            tblPurchaseVehicleSpotEntryTO.RecycleDocumentsTOList[p].CreatedBy = loginUserId;
                            tblPurchaseVehicleSpotEntryTO.RecycleDocumentsTOList[p].CreatedOn = createdDate;
                            result = _iTblRecycleDocumentBL.InsertTblRecycleDocument(tblPurchaseVehicleSpotEntryTO.RecycleDocumentsTOList[p], conn, tran);
                            if (result != 1)
                            {
                                throw new Exception("Error while inserting in - InsertTblRecycleDocument");
                            }
                        }
                    }

                }

                #region 2. Send Notifications & SMSs to Purchase Managers Defined or undefined

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                //get purchase manager of supplier
                List<DropDownTO> PurchaseManagerList = new List<DropDownTO>();

                if (tblPurchaseVehicleSpotEntryTO.SupplierId > 0)
                {
                    PurchaseManagerList = _iTblPurchaseManagerSupplierBL.GetPurchaseManagerListOfSupplierForDropDown(tblPurchaseVehicleSpotEntryTO.SupplierId, conn, tran);

                    if (PurchaseManagerList == null || PurchaseManagerList.Count == 0)  //if supplier don't have any PM bring all PM list.
                    {
                        PurchaseManagerList = _iTblPurchaseManagerSupplierBL.SelectPurchaseFromRoleForDropDown(conn, tran);
                    }

                }
                else
                {
                    PurchaseManagerList = _iTblPurchaseManagerSupplierBL.SelectPurchaseFromRoleForDropDown(conn, tran);
                }

                if (PurchaseManagerList != null && PurchaseManagerList.Count > 0)
                {
                    for (int k = 0; k < PurchaseManagerList.Count; k++)
                    {
                        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                        tblAlertUsersTO.UserId = PurchaseManagerList[k].Value;
                        tblAlertUsersTO.RaisedOn = tblPurchaseVehicleSpotEntryTO.CreatedOn;
                        tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.SPOT_ENTRY_VEHICLE_REPORTED;
                        tblAlertUsersTOList.Add(tblAlertUsersTO);
                    }
                }

                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.SPOT_ENTRY_VEHICLE_REPORTED;
                tblAlertInstanceTO.AlertAction = "SPOT_ENTRY_VEHICLE_REPORTED";
                tblAlertInstanceTO.AlertComment = "SPOT Entry vehicle is reported. Vehicle No: " + tblPurchaseVehicleSpotEntryTO.VehicleNo;
                tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                tblAlertInstanceTO.EffectiveFromDate = tblPurchaseVehicleSpotEntryTO.CreatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "SPOT_ENTRY_VEHICLE_REPORTED";
                tblAlertInstanceTO.SourceEntityId = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                tblAlertInstanceTO.RaisedBy = tblPurchaseVehicleSpotEntryTO.CreatedBy;
                tblAlertInstanceTO.RaisedOn = tblPurchaseVehicleSpotEntryTO.CreatedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                //Sanjay [21 sept 2018] Below code is commented and common notification API is called

                notify.SendNotificationToUsers(tblAlertInstanceTO);

                //ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                //if (rMessage.MessageType != ResultMessageE.Information)
                //{
                //    tran.Rollback();
                //    resultMessage.DefaultBehaviour();
                //    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                //    resultMessage.Text = "Error While Generating Notification";
                //    return resultMessage;
                //}



                #endregion

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour("Vehicle Details Saved Successfully. Vehicle Id - " + tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry + " ");
                resultMessage.Tag = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "SaveVehicleSpotEntry");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// Priyanka [14-3-2019] : Added to get the check the same vehicle number is already present in premises.
        /// </summary>
        /// <returns></returns>
        public List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotEntryVehiclesPending(string vehicleNo)
        {
            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = _iTblPurchaseVehicleSpotEntryDAO.SelectAllSpotEntryVehiclesPending(vehicleNo);

            return tblPurchaseVehicleSpotEntryTOList;
        }
        #endregion

        #region Insertion       

        public int InsertTblRecycleDocuments(TblRecycleDocumentTO tblIssueDocumentsTO)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.InsertTblRecyleDocuments(tblIssueDocumentsTO);
        }
        public int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.InsertTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO);
        }

        public int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.InsertTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO, conn, tran);
        }

        public int InsertTblPurchasePOSpotEntryDetails(TblPODetailsAgainstSpotEntryTO tblPODetailsAgainstSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.InsertTblPurchasePOSpotEntryDetails(tblPODetailsAgainstSpotEntryTO, conn, tran);
        }

        public int InsertTblInsertTblVehicleInOutDetails(TblVehicleInOutTONew VehicleInOutTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.InsertTblVehicleInOutDetails(VehicleInOutTO, conn, tran);
        }


        #endregion

        #region Updation
        public int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.UpdateTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO);
        }

        public int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.UpdateTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.DeleteTblPurchaseVehicleSpotEntry(idVehicleSpotEntry);
        }

        public int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.DeleteTblPurchaseVehicleSpotEntry(idVehicleSpotEntry, conn, tran);
        }

        #endregion


        public List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnquiryVehicleEntryTO(Int32 purchaseEnquiryId) //, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.SelectTblPurchaseEnquiryVehicleEntryTO(purchaseEnquiryId);

        }
        public List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnqVehEntryTOList(Int32 purchaseEnquiryId) //, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.SelectTblPurchaseEnqVehEntryTOList(purchaseEnquiryId);

        }


        public List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnqVehEntryTOList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran) //, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.SelectTblPurchaseEnqVehEntryTOList(purchaseEnquiryId, conn, tran);

        }

        public TblPurchaseVehicleSpotEntryTO SelectSpotVehicleAgainstScheduleId(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran) //, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.SelectSpotVehicleAgainstScheduleId(purchaseScheduleSummaryId, conn, tran);

        }

        public ResultMessage UpdateBookingDtlsForSpotVehicle(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, Boolean isForRevertLink)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //check for schedule
                string vehicleNo = "";
                if (isForRevertLink)
                {
                    Boolean isScheduleNotExists = false;

                    List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = SelectTblPurchaseEnqVehEntryTOList(tblPurchaseVehicleSpotEntryTO.PurchaseEnquiryId, conn, tran);
                    if (tblPurchaseVehicleSpotEntryTOList != null && tblPurchaseVehicleSpotEntryTOList.Count > 0)
                    {
                        for (int i = 0; i < tblPurchaseVehicleSpotEntryTOList.Count; i++)
                        {
                            if (tblPurchaseVehicleSpotEntryTOList[i].PurchaseScheduleSummaryId == 0)
                            {
                                isScheduleNotExists = true;
                                vehicleNo = tblPurchaseVehicleSpotEntryTOList[i].VehicleNo;
                                break;
                            }
                        }
                    }

                    if (isScheduleNotExists)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Vehicle No : " + vehicleNo + " already linked to sauda";
                        resultMessage.Result = -1;
                        return resultMessage;
                    }
                }

                result = UpdateTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO, conn, tran);
                if (result >= 1)
                {
                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Record Updated Successfully";
                    resultMessage.Result = 1;
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateBookingDtlsForSpotVehicle";
                    resultMessage.Result = -1;
                    return resultMessage;


                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Error While UpdateBookingDtlsForSpotVehicle";
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public ResultMessage UpdateSpotEntryVehicleSupplier(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDateTime = _iCommonDAO.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                DateTime createdDate = _iCommonDAO.ServerDateTime;

                result = UpdateSpotEntryVehicleSupplier(tblPurchaseVehicleSpotEntryTO, conn, tran);

                if(result >= 0)
                {
                    if(tblPurchaseVehicleSpotEntryTO.TblPODetailsAgainstSpotEntryList != null && tblPurchaseVehicleSpotEntryTO.TblPODetailsAgainstSpotEntryList.Count > 0)
                    {
                        result = DeletePreviousAddedPO(tblPurchaseVehicleSpotEntryTO,conn,tran);
                        for(int i = 0;i< tblPurchaseVehicleSpotEntryTO.TblPODetailsAgainstSpotEntryList.Count;i++)
                        {
                            TblPODetailsAgainstSpotEntryTO TODetails = new TblPODetailsAgainstSpotEntryTO();
                            TODetails = tblPurchaseVehicleSpotEntryTO.TblPODetailsAgainstSpotEntryList[i];
                            TODetails.CreatedOn = createdDate;
                            TODetails.PurchaseVehicleSpotEntryId = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                            result = InsertTblPurchasePOSpotEntryDetails(TODetails, conn, tran);
                        }
                    }
                }
                if (result < 1)
                {
                    throw new Exception("Error while updating tblPurchaseVehicleSpotEntryTO");

                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " UpdateScheduleVehicleNoOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO)");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public int UpdateSpotEntryVehicleSupplier(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.UpdateSpotEntryVehicleSupplier(tblPurchaseVehicleSpotEntryTO, conn, tran);
        }

        public int DeletePreviousAddedPO(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleSpotEntryDAO.DeletePreviousAddedPO(tblPurchaseVehicleSpotEntryTO, conn, tran);
        }

    }
}

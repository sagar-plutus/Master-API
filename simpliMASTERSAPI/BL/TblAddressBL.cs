using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{     
    public class TblAddressBL : ITblAddressBL
    {
        private readonly ITblAddressDAO _iTblAddressDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblOrgLicenseDtlDAO _iTblOrgLicenseDtlDAO;
        private readonly ITblOrganizationDAO _iTblOrganizationDAO;

        public TblAddressBL(ITblAddressDAO iTblAddressDAO, ITblOrganizationDAO iTblOrganizationDAO, ITblOrgLicenseDtlDAO iTblOrgLicenseDtlDAO, IConnectionString iConnectionString)
        {
            _iTblAddressDAO = iTblAddressDAO;
            _iTblOrganizationDAO = iTblOrganizationDAO;
            _iTblOrgLicenseDtlDAO = iTblOrgLicenseDtlDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public List<TblAddressTO> SelectAllTblAddressList()
        {
            return  _iTblAddressDAO.SelectAllTblAddress();
        }

        public TblAddressTO SelectTblAddressTO(Int32 idAddr)
        {
            return  _iTblAddressDAO.SelectTblAddress(idAddr);
        
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Get Specific Address Details of the Given Organization.
        /// It can be dealer,C&F agent Or Competitor
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="addressTypeE"></param>
        /// <returns></returns>
        public TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS)
        {
            try
            {
                TblAddressTO tblAddressTO = _iTblAddressDAO.SelectOrgAddressWrtAddrType(orgId, addressTypeE);
                tblAddressTO.AddressTypeE = addressTypeE;
                return tblAddressTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE ,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblAddressDAO.SelectOrgAddressWrtAddrType(orgId, addressTypeE, conn, tran);
        }

        public List<TblAddressTO> SelectOrgAddressList(Int32 orgId)
        {
            return _iTblAddressDAO.SelectOrgAddressList(orgId);
        }

        public List<TblAddressTO> SelectDefaultOrgAddressList(Int32 orgId)
        {
            return _iTblAddressDAO.SelectDefaultOrgAddressList(orgId);
        }


        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization address list of particular type;
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="addressTypeE"></param>
        /// <returns></returns>
        public List <TblAddressTO> SelectOrgAddressDetailOfRegion(string orgId, StaticStuff.Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblAddressTO> tblAddressTOList = _iTblAddressDAO.SelectOrgAddressDetailOfRegion(orgId, addressTypeE, conn, tran);
                return tblAddressTOList;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        //Added by Gokul 
        public List<TblBookingDelAddrTO> SelectDeliveryAddrListFromDealer(Int32 addrSourceTypeId, Int32 entityId)
        {
            List<TblBookingDelAddrTO> list = new List<TblBookingDelAddrTO>();
            List<TblBookingDelAddrTO> templist = new List<TblBookingDelAddrTO>();
            if (addrSourceTypeId == (int)Constants.AddressSourceTypeE.FROM_BOOKINGS)
            {
                List<TblBookingScheduleTO> listTemp = _iTblAddressDAO.SelectAllTblBookingScheduleList(entityId);

                if (listTemp != null && listTemp.Count > 0)
                {
                    listTemp = listTemp.OrderBy(o => o.ScheduleDate).ToList();

                    for (int k = 0; k < listTemp.Count; k++)
                    {
                        TblBookingScheduleTO tblBookingScheduleTO = listTemp[k];

                        templist = SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);
                        list.AddRange(templist);
                    }
                }
                //list = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrList(entityId);


            }
            else
            {
                list = new List<TblBookingDelAddrTO>();
                Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS;
                TblAddressTO tblAddressTO = _iTblAddressDAO.SelectOrgAddressWrtAddrType(entityId, addressTypeE);
                if (tblAddressTO == null)
                    return null;

                TblBookingDelAddrTO bookingDelAddrTO = new TblBookingDelAddrTO();
                String address = string.Empty;
                if (!string.IsNullOrEmpty(tblAddressTO.PlotNo))
                    address += tblAddressTO.PlotNo + ",";
                if (!string.IsNullOrEmpty(tblAddressTO.StreetName))
                    address += tblAddressTO.StreetName + ",";
                if (!string.IsNullOrEmpty(tblAddressTO.AreaName))
                    address += tblAddressTO.AreaName + ",";
                if (!string.IsNullOrEmpty(tblAddressTO.VillageName))
                    address += tblAddressTO.VillageName + ",";

                bookingDelAddrTO.Address = address;

                //Sanjay 16-09-2019 Commented as not along with name contact name should also required.
                //hence new method is introduced
                //bookingDelAddrTO.BillingName = _iTblOrganizationDAO.SelectFirmNameOfOrganiationById(entityId);
                OrgBasicInfo orgBasicInfo = _iTblOrganizationDAO.GetOrganizationBasicInfo(entityId);
                if (orgBasicInfo != null)
                {
                    bookingDelAddrTO.BillingName = orgBasicInfo.FirmName;
                    bookingDelAddrTO.ContactNo = orgBasicInfo.MobileNo;
                }

                bookingDelAddrTO.DistrictName = tblAddressTO.DistrictName;
                bookingDelAddrTO.TalukaName = tblAddressTO.TalukaName;
                bookingDelAddrTO.VillageName = tblAddressTO.VillageName;
                bookingDelAddrTO.StateId = tblAddressTO.StateId;
                bookingDelAddrTO.State = tblAddressTO.StateName;
                bookingDelAddrTO.Pincode = tblAddressTO.Pincode;

                List<TblOrgLicenseDtlTO> licList = _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(entityId);
                if (licList != null)
                {
                    var gstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                    if (gstNoTO == null || string.IsNullOrEmpty(gstNoTO.LicenseValue))
                    {
                        var tempGstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.SGST_NO).FirstOrDefault();
                        if (tempGstNoTO != null && !string.IsNullOrEmpty(tempGstNoTO.LicenseValue))
                        {
                            if (tempGstNoTO.LicenseValue != "0")
                                bookingDelAddrTO.GstNo = tempGstNoTO.LicenseValue;
                        }
                    }
                    else if (gstNoTO.LicenseValue == "0")
                    {
                        var tempGstNoTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.SGST_NO).FirstOrDefault();
                        if (tempGstNoTO != null && !string.IsNullOrEmpty(tempGstNoTO.LicenseValue))
                        {
                            if (tempGstNoTO.LicenseValue != "0")
                                bookingDelAddrTO.GstNo = tempGstNoTO.LicenseValue;
                        }
                    }
                    else
                        bookingDelAddrTO.GstNo = gstNoTO.LicenseValue;

                    var panTO = licList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                    if (panTO != null && !string.IsNullOrEmpty(panTO.LicenseValue))
                    {
                        if (panTO.LicenseValue != "0")
                            bookingDelAddrTO.PanNo = panTO.LicenseValue;
                    }
                }

                list.Add(bookingDelAddrTO);
            }

            return list;
        }


        //Added By Gokul
        public List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId)
        {
            return _iTblAddressDAO.SelectAllTblBookingDelAddrListBySchedule(scheduleId);
        }


        #endregion

        #region Insertion
        public int InsertTblAddress(TblAddressTO tblAddressTO)
        {
            return _iTblAddressDAO.InsertTblAddress(tblAddressTO);
        }

        public int InsertTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddressDAO.InsertTblAddress(tblAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAddress(TblAddressTO tblAddressTO)
        {
            return _iTblAddressDAO.UpdateTblAddress(tblAddressTO);
        }

        public int UpdateTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddressDAO.UpdateTblAddress(tblAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAddress(Int32 idAddr)
        {
            return _iTblAddressDAO.DeleteTblAddress(idAddr);
        }

        public int DeleteTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddressDAO.DeleteTblAddress(idAddr, conn, tran);
        }

        #endregion
        
    }
}

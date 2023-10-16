using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblAddressTO
    {
        #region Declarations
        Int32 idAddr;
        Int32 talukaId;
        Int32 districtId;
        Int32 stateId;
        Int32 countryId;
        Int32 pincode;
        Int32 createdBy;
        DateTime createdOn;
        String plotNo;
        String streetName;
        String areaName;
        String villageName;
        String comments;
        String talukaName;
        String districtName;
        String stateName;
        String stateCode;
        Int32 isDefault;
        List<TblOrgLicenseDtlTO> orgLicenseDtlTOList;
        public Int32 IdOrgAddr { set; get; }
       public Int32 IsActive { set; get; }
        public String CountryCode { set; get; }
        Int32 addrTypeId;
        String countryName;
        int gstTypeId;
        #endregion

        #region Constructor
        public TblAddressTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdAddr
        {
            get { return idAddr; }
            set { idAddr = value; }
        }
        public Int32 TalukaId
        {
            get { return talukaId; }
            set { talukaId = value; }
        }
        public Int32 DistrictId
        {
            get { return districtId; }
            set { districtId = value; }
        }
        public Int32 StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }
        public Int32 CountryId
        {
            get { return countryId; }
            set { countryId = value; }
        }
        public Int32 Pincode
        {
            get { return pincode; }
            set { pincode = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String PlotNo
        {
            get { return plotNo; }
            set { plotNo = value; }
        }
        public String StreetName
        {
            get { return streetName; }
            set { streetName = value; }
        }
        public String AreaName
        {
            get { return areaName; }
            set { areaName = value; }
        }
        public String VillageName
        {
            get { return villageName; }
            set { villageName = value; }
        }
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public String TalukaName
        {
            get { return talukaName; }
            set { talukaName = value; }
        }

        public String DistrictName
        {
            get { return districtName; }
            set { districtName = value; }
        }

        public String StateName
        {
            get { return stateName; }
            set { stateName = value; }
        }

        public Int32 AddrTypeId
        {
            get { return addrTypeId; }
            set { addrTypeId = value; }
        }

        public Int32 GstTypeId
        {
            get { return gstTypeId; }
            set { gstTypeId = value; }
        }

        public Constants.AddressTypeE AddressTypeE
        {
            get
            {
                Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS;
                if (Enum.IsDefined(typeof(Constants.AddressTypeE), addrTypeId))
                {
                    addressTypeE = (Constants.AddressTypeE)addrTypeId;
                }
                return addressTypeE;

            }
            set
            {
                addrTypeId = (int)value;
            }
        }

        public string StateCode { get => stateCode; set => stateCode = value; }
        public int IsDefault { get => isDefault; set => isDefault = value; }
        public string CountryName { get => countryName; set => countryName = value; }
        public List<TblOrgLicenseDtlTO> OrgLicenseDtlTOList { get => orgLicenseDtlTOList; set => orgLicenseDtlTOList = value; }
        public string GstTypeName { get;  set; }

        #endregion

        #region Methods

        public TblOrgAddressTO GetTblOrgAddressTO()
        {
            TblOrgAddressTO tblOrgAddressTO = new Models.TblOrgAddressTO();
            tblOrgAddressTO.AddressId = this.idAddr;
            tblOrgAddressTO.AddrTypeId = this.addrTypeId;
            tblOrgAddressTO.CreatedBy = this.createdBy;
            tblOrgAddressTO.CreatedOn = this.createdOn;
            return tblOrgAddressTO;
        }
        #endregion
    }
}

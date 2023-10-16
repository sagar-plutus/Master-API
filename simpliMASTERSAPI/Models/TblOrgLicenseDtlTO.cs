using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblOrgLicenseDtlTO
    {
        #region Declarations
        Int32 idOrgLicense;
        Int32 organizationId;
        Int32 licenseId;
        Int32 createdBy;
        DateTime createdOn;
        String licenseValue;
        String licenseName;
        Int32 addressId;
        Int32 gstTypeId;
        String gstTypeName;

        #endregion

        #region Constructor
        public TblOrgLicenseDtlTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOrgLicense
        {
            get { return idOrgLicense; }
            set { idOrgLicense = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public Int32 LicenseId
        {
            get { return licenseId; }
            set { licenseId = value; }
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
        public String LicenseValue
        {
            get { return licenseValue; }
            set { licenseValue = value; }
        }

        public String LicenseName { get => licenseName; set => licenseName = value; }
        public int AddressId { get => addressId; set => addressId = value; }
        public int GstTypeId { get => gstTypeId; set => gstTypeId = value; }
        public string GstTypeName { get => gstTypeName; set => gstTypeName = value; }
        #endregion
    }
}

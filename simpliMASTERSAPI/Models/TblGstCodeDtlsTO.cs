using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;
using Microsoft.AspNetCore.Http;

namespace ODLMWebAPI.Models
{
    public class TblGstCodeDtlsTO
    {
        #region Declarations
        Int32 idGstCode;
        Int32 codeTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime effectiveFromDt;
        DateTime effectiveToDt;
        DateTime createdOn;
        DateTime updatedOn;
        Double taxPct;
        String codeDesc;
        String codeNumber;
        Int32 isActive;
        Double merchantExportTaxPct;
        Double directTypeCatTaxPct;

        string sapHsnCode;

        List<TblTaxRatesTO> taxRatesTOList;
        Int32 absEntry;
        String chapter;
        String heading;
        String subHeading;
        String chapterID;
        String taxPctStr;
        #endregion

        #region Constructor
        public TblGstCodeDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGstCode
        {
            get { return idGstCode; }
            set { idGstCode = value; }
        }
        public Int32 CodeTypeId
        {
            get { return codeTypeId; }
            set { codeTypeId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime EffectiveFromDt
        {
            get { return effectiveFromDt; }
            set { effectiveFromDt = value; }
        }
        public DateTime EffectiveToDt
        {
            get { return effectiveToDt; }
            set { effectiveToDt = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Double TaxPct
        {
            get { return taxPct; }
            set { taxPct = value; }
        }
        public String CodeDesc
        {
            get { return codeDesc; }
            set { codeDesc = value; }
        }
        public String CodeNumber
        {
            get { return codeNumber; }
            set { codeNumber = value; }
        }

        public String EffectiveFromDtStr
        {
            get { return effectiveFromDt.ToString(Constants.DefaultDateFormat); }
        }
        public String EffectiveToDtStr
        {
            get
            {
                if (effectiveToDt != DateTime.MinValue)
                    return effectiveToDt.ToString(Constants.DefaultDateFormat);
                else
                    return string.Empty;
            }

        }

        public List<TblTaxRatesTO> TaxRatesTOList { get => taxRatesTOList; set => taxRatesTOList = value; }
        public int IsActive { get => isActive; set => isActive = value; }

        /// <summary>
        /// Sanjay [20-May-2019] SAP HSN Code Mapped from table OCHP
        /// </summary>
        public string SapHsnCode { get => sapHsnCode; set => sapHsnCode = value; }
        public string ChapterID { get => chapterID; set => chapterID = value; }
        public int AbsEntry { get => absEntry; set => absEntry = value; }
        public string Chapter { get => chapter; set => chapter = value; }
        public string Heading { get => heading; set => heading = value; }
        public string SubHeading { get => subHeading; set => subHeading = value; }
        public double MerchantExportTaxPct { get => merchantExportTaxPct; set => merchantExportTaxPct = value; }
        public double DirectTypeCatTaxPct { get => directTypeCatTaxPct; set => directTypeCatTaxPct = value; }
        public string TaxPctStr { get => taxPctStr; set => taxPctStr = value; }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class DimUomGroupConversionTO
    {
        #region Declarations
        Int32 idUomConversion;
        Int32 uomGroupId;
        Int32 uomId;
        Double altQty;
        Double baseQty;
        #endregion

        #region Constructor
        public DimUomGroupConversionTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUomConversion
        {
            get { return idUomConversion; }
            set { idUomConversion = value; }
        }
        public Int32 UomGroupId
        {
            get { return uomGroupId; }
            set { uomGroupId = value; }
        }
        public Int32 UomId
        {
            get { return uomId; }
            set { uomId = value; }
        }
        public Double AltQty
        {
            get { return altQty; }
            set { altQty = value; }
        }
        public Double BaseQty
        {
            get { return baseQty; }
            set { baseQty = value; }
        }

        public string WeightMeasurUnitDesc { get; internal set; }
        #endregion
    }
}

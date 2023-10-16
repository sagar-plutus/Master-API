using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class DimUomGroupTO
    {
        #region Declarations
        Int32 idUomGroup;
        Int32 baseUomId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 isActive;
        DateTime createdOn;
        DateTime updatedOn;
        String uomGroupCode;
        String uomGroupName;
        DimUomGroupConversionTO uomGroupConversionTO;
        String mappedUomGroupId;
        double conversionFactor;
        Int32 conversionUnitOfMeasure;
        string baseUOMName;

        #endregion

        #region Constructor
        public DimUomGroupTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUomGroup
        {
            get { return idUomGroup; }
            set { idUomGroup = value; }
        }
        public Int32 BaseUomId
        {
            get { return baseUomId; }
            set { baseUomId = value; }
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
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        public String UomGroupCode
        {
            get { return uomGroupCode; }
            set { uomGroupCode = value; }
        }
        public String UomGroupName
        {
            get { return uomGroupName; }
            set { uomGroupName = value; }
        }

        public DimUomGroupConversionTO UomGroupConversionTO { get => uomGroupConversionTO; set => uomGroupConversionTO = value; }
        public string MappedUomGroupId { get => mappedUomGroupId; set => mappedUomGroupId = value; }
        public string BaseUOMName { get => baseUOMName;  set => baseUOMName=value; }
        public Int32 ConversionUnitOfMeasure { get => conversionUnitOfMeasure;  set => conversionUnitOfMeasure = value; }
        public double ConversionFactor { get => conversionFactor; set => conversionFactor = value; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class DimOtherChargesTypeTO
    {
        #region Declarations
        Int32 idOtherChargesType;
        Boolean isActive;
        String otherChargesName;
        #endregion

        #region Constructor
        public DimOtherChargesTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOtherChargesType
        {
            get { return idOtherChargesType; }
            set { idOtherChargesType = value; }
        }
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String OtherChargesName
        {
            get { return otherChargesName; }
            set { otherChargesName = value; }
        }
        #endregion
    }
}

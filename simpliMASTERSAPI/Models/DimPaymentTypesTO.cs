using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class DimPaymentTypesTO
    {
        #region Declarations
        Int32 idPayType;
        Int32 permissionId;
        Boolean isActive;
        String payTypeName;
        String payTypeDec;
        #endregion

        #region Constructor
        public DimPaymentTypesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPayType
        {
            get { return idPayType; }
            set { idPayType = value; }
        }
        public Int32 PermissionId
        {
            get { return permissionId; }
            set { permissionId = value; }
        }
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String PayTypeName
        {
            get { return payTypeName; }
            set { payTypeName = value; }
        }
        public String PayTypeDec
        {
            get { return payTypeDec; }
            set { payTypeDec = value; }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class DimTransactionTypeTO
    {
        #region Declarations
        Int32 idTransType;
        Int32 isActive;
        String transName;
        String transDesc;
        #endregion

        #region Constructor
        public DimTransactionTypeTO()
        {
        }


        #endregion

        #region GetSet
        public Int32 IdTransType
        {
            get { return idTransType; }
            set { idTransType = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String TransName
        {
            get { return transName; }
            set { transName = value; }
        }
        public String TransDesc
        {
            get { return transDesc; }
            set { transDesc = value; }
        }
        #endregion
    }
}

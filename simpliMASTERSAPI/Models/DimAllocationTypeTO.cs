using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class DimAllocationTypeTO
    {
        #region Declarations
        Int32 idAllocType;
        Int32 isActive;
        String allocTypeName;
        String allocTypeDescription;
        String query;
        #endregion

        #region Constructor
        public DimAllocationTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdAllocType
        {
            get { return idAllocType; }
            set { idAllocType = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String AllocTypeName
        {
            get { return allocTypeName; }
            set { allocTypeName = value; }
        }
        public String AllocTypeDescription
        {
            get { return allocTypeDescription; }
            set { allocTypeDescription = value; }
        }
        public String Query
        {
            get { return query; }
            set { query = value; }
        }
        #endregion
    }
}

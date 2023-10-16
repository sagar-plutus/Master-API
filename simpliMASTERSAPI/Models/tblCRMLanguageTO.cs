using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class tblCRMLanguageTO
    {

        #region Constructor
        public tblCRMLanguageTO()
        {
        }

        #endregion

        #region GetSet

        public Int32 idLanguage { get; set; }
        public String lagName { get; set; }
        public Int32 isDefault { get; set; }
        public Int32 isActive { get; set; }
        public Int32 createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public Int32 updatedBy { get; set; }
        public DateTime updatedOn { get; set; }

        #endregion
    }
}

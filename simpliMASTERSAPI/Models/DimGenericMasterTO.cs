using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class DimGenericMasterTO
    {

        int idGenericMaster;
        string value;
        int isActive;
        int dimensionId;
        string mappedTxnId;

        #region Constructor
        public DimGenericMasterTO()
        {
        }

        #endregion


        public int IdGenericMaster { get => idGenericMaster; set => idGenericMaster = value; }
        public string Value { get => value; set => this.value = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public int DimensionId { get => dimensionId; set => dimensionId = value; }
        public string MappedTxnId { get => mappedTxnId; set => mappedTxnId = value; }
        public int ParentIdGenericMaster { get; set; }
        public string ParentName { get; set; }
        public string DeptName { get; set; }
    }
}

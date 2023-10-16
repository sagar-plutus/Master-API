using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblItemMasterFieldsTO
    {
        #region Declarations
        Int32 idTblItemMasterFields;
        Int32 isActive;
        Int32 isMandatory;
        Int32 isDisabled;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String name;
        String descField;
        Int32 sequanceNo;
        #endregion

        #region Constructor
        public TblItemMasterFieldsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTblItemMasterFields
        {
            get { return idTblItemMasterFields; }
            set { idTblItemMasterFields = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Int32 SequanceNo
        {
            get { return sequanceNo; }
            set { sequanceNo = value; }
        }
        public Int32 IsMandatory
        {
            get { return isMandatory; }
            set { isMandatory = value; }
        }
        public Int32 IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
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
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String DescField
        {
            get { return descField; }
            set { descField = value; }
        }
        #endregion
    }
}

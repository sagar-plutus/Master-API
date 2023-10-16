using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static ODLMWebAPI.StaticStuff.Constants;
namespace ODLMWebAPI.Models
{

    public class AttributePageTO
    {
      public  Int32 PageId { get; set; }  
       public String PageName { get; set; }

       public Int32 IsSrcFilter { get; set; }
        public String IdParam { get; set; }

        public String NameParam { get; set; }

        public String TableName { get; set; }


    }
    public class AttributeStatusTO
    {
        #region Declarations

        String attributeName;
        String attributeDisplayName;
        Int32 idAttributesStatus;
        Int32 pageId;
        Int32 orgTypeId;
        Int32 attributeId;
        Int32 isVisible;
        Int32 isMandatory;
        Int32 isActive;
        public Int32 IsSrcFilter { get; set; }
        public Int32 CreatedBy { get; set; }
        public Int32 UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
        public Int32 lagId { get; set; }
        public String valueLabel { get; set; }





        #endregion


        #region Constructor
        public AttributeStatusTO()
        {

        }

        #endregion


        #region GetSet

        public string AttributeName { get => attributeName; set => attributeName = value; }
        public int IdAttributesStatus { get => idAttributesStatus; set => idAttributesStatus = value; }
        public int PageId { get => pageId; set => pageId = value; }
        public int OrgTypeId { get => orgTypeId; set => orgTypeId = value; }
        public int AttributeId { get => attributeId; set => attributeId = value; }
        public int IsVisible { get => isVisible; set => isVisible = value; }
        public int IsMandatory { get => isMandatory; set => isMandatory = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public string AttributeDisplayName { get => attributeDisplayName; set => attributeDisplayName = value; }

        #endregion



    }
}

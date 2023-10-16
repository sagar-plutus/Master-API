using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblPagesTO
    {
        #region Declarations
        Int32 idPage;
        Int32 moduleId;
        DateTime createdOn;
        String pageName;
        String pageDesc;
        Int32 isActive;
        #endregion

        #region Constructor
        public TblPagesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPage
        {
            get { return idPage; }
            set { idPage = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }
        public String PageDesc
        {
            get { return pageDesc; }
            set { pageDesc = value; }
        }

        public int IsActive { get => isActive; set => isActive = value; }
        #endregion
    }
}

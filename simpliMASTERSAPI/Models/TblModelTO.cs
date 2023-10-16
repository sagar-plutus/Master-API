using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static ODLMWebAPI.StaticStuff.Constants;
namespace ODLMWebAPI.Models
{
    public class TblModelTO
    {
        #region Declarations
        DateTime createdOn;
        DateTime finalizedOn;
        int idModel;
        int prodItemId;
        int versionNo;
        int createdBy;
        int finalizedBy;
        int revisionNo;
        #endregion

        #region Constructor
        public TblModelTO()
        {
        }

        #endregion

        #region GetSet
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime FinalizedOn
        {
            get { return finalizedOn; }
            set { finalizedOn = value; }
        }
        public int IdModel
        {
            get { return idModel; }
            set { idModel = value; }
        }
        public int ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public int VersionNo
        {
            get { return versionNo; }
            set { versionNo = value; }
        }

        public int RevisionNo
        {
            get { return revisionNo; }
            set { revisionNo = value; }
        }
        public int CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public int FinalizedBy
        {
            get { return finalizedBy; }
            set { finalizedBy = value; }
        }
        #endregion
    }
}

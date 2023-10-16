using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.TO
{
    public class TblItemDocumentsTO
    {
        #region Declarations
        DateTime createdOn;
        DateTime updatedOn;
        int idItemDocuments;
        int itemId;
        int documentId;
        int documentTypeId;
        int isActive;
        int createdBy;
        int updatedBy;
        string path;
        string docTypeName;
        string docDesc;
        string createdOnStr;
        int isShowImagesForItem;

        #endregion

        #region Constructor
        public TblItemDocumentsTO()
        {
        }

        #endregion

        #region GetSet

        public string CreatedOnStr
        {
            get { return createdOnStr; }
            set { createdOnStr = value; }
        }
        public string DocTypeName
        {
            get { return docTypeName; }
            set { docTypeName = value; }
        }
        public string DocDesc
        {
            get { return docDesc; }
            set { docDesc = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
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
        public int IdItemDocuments
        {
            get { return idItemDocuments; }
            set { idItemDocuments = value; }
        }

        public int IsShowImagesForItem
        {
            get { return isShowImagesForItem; }
            set { isShowImagesForItem = value; }
        }
        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }
        public int DocumentId
        {
            get { return documentId; }
            set { documentId = value; }
        }
        public int DocumentTypeId
        {
            get { return documentTypeId; }
            set { documentTypeId = value; }
        }
        public int IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public int CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public int UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        #endregion
    }
}

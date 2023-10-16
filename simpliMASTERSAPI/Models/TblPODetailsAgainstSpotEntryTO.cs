using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.TO
{
    public class TblPODetailsAgainstSpotEntryTO
    {
        #region Declarations
        Int32 idtblPODetailsAgainstSpotEntry;
        Int32 purchaseVehicleSpotEntryId;
        DateTime createdOn;
        Int64 commercialDocId;
        String poNo;
        #endregion


        #region Constructor
        public TblPODetailsAgainstSpotEntryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdtblPODetailsAgainstSpotEntry
        {
            get { return idtblPODetailsAgainstSpotEntry; }
            set { idtblPODetailsAgainstSpotEntry = value; }
        }
        public Int32 PurchaseVehicleSpotEntryId
        {
            get { return purchaseVehicleSpotEntryId; }
            set { purchaseVehicleSpotEntryId = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Int64 CommercialDocId
        {
            get { return commercialDocId; }
            set { commercialDocId = value; }
        }
        public String PoNo
        {
            get { return poNo; }
            set { poNo = value; }
        }
        #endregion


      
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class TblWBRptTO
    {
        #region Declarations

        String wBID;
        String userID;
        String orignalRSTNo;
        String additionalRSTNo;
        String date;
        String time;
        String materialType;
        String materialSubType;
        Decimal grossWeight;
        Decimal firstWeight;
        Decimal secondWeight;
        Decimal thirdWeight;
        Decimal forthWeight;
        Decimal fifthWeight;
        Decimal sixthWeight;
        Decimal seventhWeight;
        Decimal tareWeight;
        Decimal netWeight;
        String loadOrUnload;
        String fromLocation;
        String toLocation;
        String transactionType;
        String vehicleNumber;
        String vehicleStatus;
        String billType;
        String vehicleID;
        #endregion

        #region Constructor


        #endregion

        #region GetSet

        [JsonProperty("WB ID")]
        public String WBID
        {
            get { return wBID; }
            set { wBID = value; }
        }

        [JsonProperty("User ID")]
        public String UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        [JsonProperty("Orignal RST No")]
        public String OrignalRSTNo
        {
            get { return orignalRSTNo; }
            set { orignalRSTNo = value; }
        }

        [JsonProperty("Additional RST No")]
        public String AdditionalRSTNo
        {
            get { return additionalRSTNo; }
            set { additionalRSTNo = value; }
        }
        public String Date
        {
            get { return date; }
            set { date = value; }
        }
        public String Time
        {
            get { return time; }
            set { time = value; }
        }

        [JsonProperty("Material Type")]
        public String MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }

        [JsonProperty("Material Sub Type")]
        public String MaterialSubType
        {
            get { return materialSubType; }
            set { materialSubType = value; }
        }

        [JsonProperty("Gross Weight")]
        public Decimal GrossWeight
        {
            get { return grossWeight; }
            set { grossWeight = value; }
        }

        [JsonProperty("1st Weight")]
        public Decimal FirstWeight
        {
            get { return firstWeight; }
            set { firstWeight = value; }
        }

        [JsonProperty("2nd Weight")]
        public Decimal SecondWeight
        {
            get { return secondWeight; }
            set { secondWeight = value; }
        }

        [JsonProperty("3rd Weight")]
        public Decimal ThirdWeight
        {
            get { return thirdWeight; }
            set { thirdWeight = value; }
        }

        [JsonProperty("4th Weight")]
        public Decimal ForthWeight
        {
            get { return forthWeight; }
            set { forthWeight = value; }
        }

        [JsonProperty("5th Weight")]
        public Decimal FifthWeight
        {
            get { return fifthWeight; }
            set { fifthWeight = value; }
        }

        [JsonProperty("6th Weight")]
        public Decimal SixthWeight
        {
            get { return sixthWeight; }
            set { sixthWeight = value; }
        }

        [JsonProperty("7th Weight")]
        public Decimal SeventhWeight
        {
            get { return seventhWeight; }
            set { seventhWeight = value; }
        }

        [JsonProperty("Tare Weight")]
        public Decimal TareWeight
        {
            get { return tareWeight; }
            set { tareWeight = value; }
        }

        [JsonProperty("Net Weight")]
        public Decimal NetWeight
        {
            get { return netWeight; }
            set { netWeight = value; }
        }

        [JsonProperty("Load Or Unload")]
        public String LoadOrUnload
        {
            get { return loadOrUnload; }
            set { loadOrUnload = value; }
        }

        [JsonProperty("From Location")]
        public String FromLocation
        {
            get { return fromLocation; }
            set { fromLocation = value; }
        }

        [JsonProperty("To Location")]
        public String ToLocation
        {
            get { return toLocation; }
            set { toLocation = value; }
        }

        [JsonProperty("Transaction Type")]
        public String TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

        [JsonProperty("Vehicle Number")]
        public String VehicleNumber
        {
            get { return vehicleNumber; }
            set { vehicleNumber = value; }
        }

        [JsonProperty("Vehicle Status")]
        public String VehicleStatus
        {
            get { return vehicleStatus; }
            set { vehicleStatus = value; }
        }

        [JsonProperty("Bill Type")]
        public String BillType
        {
            get { return billType; }
            set { billType = value; }
        }

        [JsonProperty("Vehicle ID")]
        public String VehicleID
        {
            get { return vehicleID; }
            set { vehicleID = value; }
        }       

        #endregion
    }
}

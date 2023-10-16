using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ODLMWebAPI.BL;
using ODLMWebAPI.Models;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using simpliMASTERSAPI;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;


namespace ODLMWebAPI.StaticStuff
{
    public class Constants
    {

        public enum ApplicationModeTypeE
        {
            NORMAL_MODE = 1,
            BASIC_MODE = 2,
        }
        public enum ItemClassPropertiesTypeE
        {
            Class_A = 1,
            Class_B = 2,
            Class_C = 3,
        }

        #region Common Methods
        /// <summary>
        /// if it is integer and zero then will return DBNull.Value
        /// if it is double and zero then will return DBNull.Value
        /// if it is datetime and is 1/1/1 then will return DBNull.Value
        /// 
        /// </summary>
        /// <param name="cSharpDataValue"></param>
        /// <returns></returns>
        public static object GetSqlDataValueNullForBaseValue(object cSharpDataValue)
        {
            if (cSharpDataValue == null)
            {
                return DBNull.Value;
            }
            else
            {
                if (cSharpDataValue.GetType() == typeof(DateTime))
                {
                    DateTime dt = (DateTime)cSharpDataValue;
                    if (dt.Year == 1 && dt.Month == 1 && dt.Day == 1)
                    {
                        return DBNull.Value;
                    }
                }
                else if (cSharpDataValue.GetType() == typeof(int))
                {
                    int intValue = (int)cSharpDataValue;
                    if (intValue == 0)
                    {
                        return DBNull.Value;
                    }
                }
                else if (cSharpDataValue.GetType() == typeof(Double))
                {
                    Double douValue = (Double)cSharpDataValue;
                    if (douValue == 0)
                    {
                        return DBNull.Value;
                    }
                }
                else if (cSharpDataValue.GetType() == typeof(Int64))
                {
                    Int64 bigIntValue = (Int64)cSharpDataValue;
                    if (bigIntValue == 0)
                    {
                        return DBNull.Value;
                    }
                }
                return cSharpDataValue;
            }
        }
        #endregion
        //vipul[06/05/2019] user subscription active count setting
        public enum UsersSubscriptionActiveCntCalSetting
        {
            ByTab = 1,
            WithOutTab = 2,
        }
        #region Enumerations
        public enum dimApprovalAreaE
        {
            PUNE = 1,
            QUOTATION_COMPARISON = 2,
            PURCHASE_REQUEST = 3,
            VOUCHER_NOTE = 4,
            Divert_Qty_Authorization=5
        }
        public enum dimEnquiryTypeEnum
        {
            Project = 1,
            Services = 2,
            Spares = 3
        }
        public enum VoucherNoteStatusE
        {
            NEW = 1065,
            AUTHORIZED = 1066,
            IMPORTED = 1067
        }
        public enum MobileDupChecktypeE
        {
            OrganizationRegMobNo=1,
            ContactDetailsMobNo=2,
            PersonMobNo=3,
            AltPersonMobNo=4
        }
        public enum ConfirmTypeE
        {
            CONFIRM = 1,
            NONCONFIRM = 0,
            BOTH = 2

        }

        public enum SystemRolesE
        {
            SYSTEM_ADMIN = 1,
            DIRECTOR = 2,
            C_AND_F_AGENT = 3,
            LOADING_PERSON = 4,
            MARKETING_FRONTIER = 5,
            MARKETING_BACK_OFFICE = 6,
            FIELD_OFFICER = 7,
            REGIONAL_MANAGER = 8,
            VICE_PRESIDENT_MARKETING = 9,
            ACCOUNTANT = 10,
            SECURITY_OFFICER = 11,
            SUPERWISOR = 179,
            PURCHASE_MANAGER = 176,
            INSIDE_GRADER = 178,
        }

        public enum OrgTypeE
        {
            C_AND_F_AGENT = 1,
            DEALER = 2,
            COMPETITOR = 3,
            TRANSPOTER = 4,
            INTERNAL = 5,
            OTHER = 0,
            //Vaibhav 
            //INFLUENCER = 1006,
            PURCHASE_COMPETITOR = 7,
            INFLUENCER = 8,
            PROJECT_ORGANIZATIONS = 9,
            USERS = 15,
            SUPPLIER = 6,
            BANK = 16,
            

        }
        public enum UserTypeE
        {
            EMPLOYEE = 1,
            USER = 2
        }

        public enum RabbitTransactionsE
        {
            USER = 1

        }
        public enum LogoutValueE
        {
            LogoutWithTimer=1,
            DirectLogout = 2

        }
        public enum UserMappingTransModeE
        {
            DEACTIVATE = 3,
            CREATE = 1,
            UPDATE = 2
        }

        public enum UsersConfigration
        {
            USER_CONFIG = 1
        }
        public enum AddressTypeE
        {
            OFFICE_ADDRESS = 1,
            FACTORY_ADDRESS = 2,
            WORKS_ADDRESS = 3,
            GODOWN_ADDRESS = 4,
            //Vijaymla[01-11-2017] added to save organization new address of type office supply address
            OFFICE_SUPPLY_ADDRESS = 5
        }

        public enum TransactionTypeE
        {
            BOOKING = 1,
            LOADING = 2,
            DELIVERY = 3,
            SPECIAL_REQUIREMENT = 4,
           
            DealerCat = 9,
            UNLOADING = 5,
            PURCHASE_ORDER=13
           
        }
        public enum dimBOMTypeE
        {
            Standard_BOM = 0,
            Optional_BOM = 1,
            Dismantal_BOM = 2
        }
        //public static DateTime ServerDateTime
        //{
        //    get
        //    {
        //        return DAL.CommonDAO.SelectServerDateTime();
        //    }
        //}

        private static ILogger loggerObj;

        public static ILogger LoggerObj
        {
            get
            {
                return loggerObj;
            }

            set
            {
                loggerObj = value;
            }
        }

        public static Int32 OrgAlertDefId = 1535;

        public enum LedgerStatusE  //Added Dhananjay [26-12-2020]
        {
            LEDGER_NEW = 1504,
            LEDGER_AUTHORIZED = 1505,
            LEDGER_REJECTED = 1506,
            LEDGER_DELETED = 1507,
        }

        public enum bomStatusEnum
        {
            New = 1101,
            Pending = 1102,
            Finalized = 1103
        }

        public enum TranStatusE
        {
            BOOKING_NEW = 1,
            BOOKING_APPROVED = 2,
            //BOOKING_APPROVED_DIRECTORS = 3, //Saket [2017-11-10] Commented For SRJ.
            BOOKING_APPROVED_FINANCE = 3,
            LOADING_NEW = 4,
            LOADING_NOT_CONFIRM = 5,
            LOADING_WAITING = 6,
            LOADING_CONFIRM = 7,
            BOOKING_REJECTED_BY_FINANCE = 8,
            BOOKING_APPROVED_BY_MARKETING = 9,
            BOOKING_REJECTED_BY_MARKETING = 10,
            BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR = 11,
            BOOKING_REJECTED_BY_ADMIN_OR_DIRECTOR = 12,
            BOOKING_DELETE = 13,
            LOADING_REPORTED_FOR_LOADING = 14,
            LOADING_GATE_IN = 15,
            LOADING_COMPLETED = 16,
            LOADING_DELIVERED = 17,
            LOADING_CANCEL = 18,
            LOADING_POSTPONED = 19,
            LOADING_VEHICLE_CLERANCE_TO_SEND_IN = 20, // It will be given by Loading Incharge to Security Officer
            //GJ@20170915 : Added the Unloading Status
            UNLOADING_NEW = 21,
            UNLOADING_COMPLETED = 22,
            UNLOADING_CANCELED = 23,
            BOOKING_PENDING_FOR_DIRECTOR_APPROVAL = 24,  //Sanjay [2017-12-19] New Status when Finance Forward Booking to Director Approval.
            BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR = 25                            //Priyanka [2018-30-07] Added for adding new status in booking.

                 , VEHICLE_CANCELED = 529            //Prajakta [2019-04-15] Added
    , VEHICLE_PENDING_FOR_YARD_MANAGER = 530
    , VEHICLE_OUT = 510 //Prajakta[04-Oct-2018] Added
          ,  New = 501
    , DELETE_VEHICLE = 526                       //Priyanka [05-03-2019]

    , VEHICLE_REJECTED_AFTER_WEIGHING = 531
    , VEHICLE_REJECTED_BEFORE_WEIGHING = 532
    , WEIGHING_COMPLETED = 533
    , VEHICLE_REJECTED_AFTER_GROSS_WEIGHT = 534
    , SPOT_ENTRY_VEHICLE_REJECTED = 507
         ,   PURCHASE_ORDER_STATUS_AUTHORIZED = 1042,
            PURCHASE_ORDER_STATUS_PARTIALY = 1043,
            New_Schedule = 1083,
            Reported = 1084,
            Clearance_for_sent_in = 1085,
            Sent_in = 1086,
            Vehicle_Out_Common=1087,
            Unloading_is_in_progress=1088,
            Unloading_Completed=1089,
            Vehicle_Rejected_while_GRN=1094,
            Vehicle_Rejected_and_Out=1095,
            PURCHASE_ORDER_STATUS_AUTHORIZATION_1 = 1050,
            PURCHASE_ORDER_STATUS_AUTHORIZATION_2 = 1051,
            PURCHASE_ORDER_STATUS_AUTHORIZATION_3 = 1052,

        }

        public enum LoadingLayerE
        {
            BOTTOM = 1,
            MIDDLE1 = 2,
            MIDDLE2 = 3,
            MIDDLE3 = 4,
            TOP = 5
        }

        /// <summary>
        /// Sanjay [2017-03-06] To Maintain the historical record for any transactional records
        /// </summary>
        public enum TxnOperationTypeE
        {
            NONE = 0,
            OPENING = 1,
            IN = 2,
            OUT = 3,
            UPDATE = 4
        }


        /// <summary>
        /// Sanjay [29 Nov 2018] Commented the Enum as it should not be used for any check.
        /// It should be derived from RoleType Enum with roleids configured in DB. From Org Structure implementations this is
        /// changed as roles become dynamic
        /// </summary>
        //public enum SystemRolesE
        //{
        //    SYSTEM_ADMIN = 1,
        //    DIRECTOR = 2,
        //    C_AND_F_AGENT = 3,
        //    LOADING_PERSON = 4,
        //    MARKETING_FRONTIER = 5,
        //    MARKETING_BACK_OFFICE = 6,
        //    FIELD_OFFICER = 7,
        //    REGIONAL_MANAGER = 8,
        //    VICE_PRESIDENT_MARKETING = 9,
        //    ACCOUNTANT = 10,
        //    SECURITY_OFFICER = 11,
        //    SUPERWISOR = 12,

        //    //Priyanka [07-03-2018]
        //    WEIGHING_OFFICER = 13,
        //    BILLING_OFFICER = 14,
        //    TRANSPORTER = 15
        //}

        public enum SystemRoleTypeE
        {
            SYSTEM_ADMIN = 1,
            DIRECTOR = 2,
            C_AND_F_AGENT = 3,
            LOADING_PERSON = 4,
            MARKETING_FRONTIER = 5,
            MARKETING_BACK_OFFICE = 6,
            FIELD_OFFICER = 7,
            REGIONAL_MANAGER = 8,
            VICE_PRESIDENT_MARKETING = 9,
            ACCOUNTANT = 10,
            SECURITY_OFFICER = 11,
            SUPERWISOR = 12,

            //Priyanka [07-03-2018]
            WEIGHING_OFFICER = 13,
            BILLING_OFFICER = 14,
            TRANSPORTER = 15,
            Dealer = 16,
            PURCHASE_MANAGER = 20,
            STORE_INCHARGE=23,
            Store_HOD = 25,
            UNLOADING_MANAGER= 30,
        }

        public enum ProductCategoryE
        {
            NONE = 0,
            TMT = 1,
            PLAIN = 2
        }

        public enum ProductSpecE
        {
            NONE = 0,
            STRAIGHT = 1,
            BEND = 2,
            RK_SHORT = 3,
            RK_LONG = 4,
            TUKADA = 5,
            COIL = 6,
        }

        public enum BookingActionE
        {
            OPEN,
            CLOSE
        }

        public enum CommercialLicenseE
        {
            PAN_NO = 1,
            VAT_NO = 2,
            TIN_NO = 3,
            CST_NO = 4,
            EXCISE_REG_NO = 5,
            SGST_NO = 6,  //Prov GSTIN No
            IGST_NO = 7,  //Permenent GSTIN No
            CGST_NO = 8,
            CIN_NO = 9,
            AADHAR_NO = 10
        }

        public enum TxnDeliveryAddressTypeE
        {
            BILLING_ADDRESS = 1,
            CONSIGNEE_ADDRESS = 2,
            SHIPPING_ADDRESS = 3
        }

        public enum AddressSourceTypeE
        {
            FROM_BOOKINGS = 1,
            FROM_DEALER = 2,
            FROM_CNF = 3,
            NEW_ADDRESS = 4,
            SELECT_FROM_EXISTING_BOOKINGS = 5       //Priyanka [14-12-2018]
        }

        public enum InvoiceTypeE
        {
            REGULAR_TAX_INVOICE = 1,
            EXPORT_INVOICE = 2,
            DEEMED_EXPORT_INVOICE = 3,
            SEZ_WITH_DUTY = 4,
            SEZ_WITHOUT_DUTY = 5
        }


        public enum InvoiceStatusE
        {
            NEW = 1,
            PENDING_FOR_AUTHORIZATION = 2,
            AUTHORIZED_BY_DIRECTOR = 3,
            REJECTED_BY_DIRECTOR = 4,
            PENDING_FOR_ACCEPTANCE = 5,
            ACCEPTED_BY_DISTRIBUTOR = 6,
            REJECTED_BY_DISTRIBUTOR = 7,
            CANCELLED = 8,
            AUTHORIZED = 9,
        }

        /*GJ@20170913 : Added Enum for Loading Slip Type*/
        public enum LoadingTypeE
        {
            REGULAR = 1,
            OTHER = 2,
        }
        /*GJ@20170913 : Added Enum for Tax Type*/
        public enum TaxTypeE
        {
            IGST = 1,
            CGST = 2,
            SGST = 3,
        }
        public enum SAPTaxCodePerEnum
        {
            ZERO_PER = 0,
            TWELEVE_PER = 12,
            EIGHTEEN_PER = 18,
            TWENTY_EIGHT_PER = 28,
            FIVE_PER = 5
        }
        public static class SAPTaxCodeEnum
        {
            public static String ZERO_PER_IGST { get; set; } = "IGST0";
            public static String ZERO_PER_SCGST { get; set; } = "SCGST0";
            public static String TWELEVE_PER_IGST { get; set; } = "IGST12";
            public static String TWELEVE_PER_SCGST { get; set; } = "SCGST12";
            public static String EIGHTEEN_PER_IGST { get; set; } = "IGST18";
            public static String EIGHTEEN_PER_SCGST { get; set; } = "SCGST18";
            public static String TWENTY_EIGHT_PER_IGST { get; set; } = "IGST28";
            public static String TWENTY_EIGHT_PER_SCGST { get; set; } = "SCGST28";
            public static String FIVE_PER_IGST { get; set; } = "IGST5";
            public static String FIVE_PER_SCGST { get; set; } = "SCGST5";
        }
        /*GJ@20170913 : Added Enum for Invoice Mod Type*/
        public enum InvoiceModeE
        {
            AUTO_INVOICE = 1,
            AUTO_INVOICE_EDIT = 2,
            MANUAL_INVOICE = 3,
        }

        /*GJ@20171007 : Weighing Measure Type*/
        public enum TransMeasureTypeE
        {
            TARE_WEIGHT = 1,
            INTERMEDIATE_WEIGHT = 2,
            GROSS_WEIGHT = 3,
            NET_WEIGHT = 4
        }
        // Vaibhav [18-Sep-2017] Added to department master

        public enum DepartmentTypeE
        {
            DIVISION = 1,
            DEPARTMENT = 2,
            SUB_DEPARTMENT = 3,
            BOM = 4,
        }

        // Vaibhav [7-Oct-2017] Added to visit persons
        public enum VisitPersonE
        {
            SITE_OWNER = 1,
            SITE_ARCHITECT = 2,
            SITE_STRUCTURAL_ENGG = 3,
            SITE_CONTRACTOR = 4,
            SITE_STEEL_PURCHASE_AUTHORITY = 5,
            DEALER = 6,
            DEALER_MEETING_WITH = 7,
            DEALER_VISIT_ALONG_WITH = 8,
            SITE_COMPLAINT_REFRRED_BY = 9,
            COMMUNICATION_WITH_AT_SITE = 10,
            INFLUENCER_VISITED_BY = 11,
            INFLUENCER_RECOMMANDEDED_BY = 12,
            SITE_EXECUTOR = 13,
            FIRST_OWNER = 15,
            SECOND_OWNER = 16,
            SITE_DONE_WITH = 17,
            SITE_COMPETITOR = 18,
            BUSINESS_AGENT = 19
        }

        // Vaibhav [7-Oct-2017] Added to visit follow up roles
        public enum VisitFollowUpActionE
        {
            SHARE_INFO_TO = 1,
            CALL_BY_SELF_TO = 2,
            ARRANGE_VISIT_OF = 3,
            ARRANGE_VISIT_TO = 4,
            ARRANGE_FOR = 5,
            POSSIBILITY_OF = 6
        }

        // Vaibhav [10-Oct-2017] added to visit issues 
        public enum VisitIssueTypeE
        {
            DELIVERY_ISSUE = 1,
            Quality_ISSUE = 2,
            PRICE_ISSUE = 3,
            ACCOUNT_ISSUE = 4,
            INFLUENCER_ISSUE = 5
        }

        // Vaibhav [23-Oct-2017] added to visit site type
        public enum VisitSiteTypeE
        {
            SITE_TYPE = 1,
            SITE_CATEGORY = 2,
            SITE_SUBCATEGORY = 3
        }

        // Vaibhav [24-Oct-2017] added to visit project type
        public enum VisitProjectTypeE
        {
            KEY_PROJECT = 1,
            CURRENT_PROJECT = 2
        }

        // Vaibhav [27-Oct-2017] added to follow up roles
        public enum VisitFollowUpRoleE
        {
            SHARE_INFO_TO = 1,
            CALL_BY_SELF_TO_WHOM = 2,
            ARRANGE_VISIT_OF = 3,
            ARRANGE_VISIT_TO = 4,
            ARRANGE_VISIT_FOR = 5,
            POSSIBILITY_OF = 6
        }

        /// <summary>
        /// Sanjay [2018-02-19] To Define Item Product Categories
        /// Was Required to distiguish between Finished Good & Scrap
        /// </summary>
        /// <remarks> Enum for Item Product Categories Of The System</remarks>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ItemProdCategoryE
        {
            REGULAR_RM = 1,
            FINISHED_GOODS = 2,
            SEMI_FINISHED_GOODS = 3,
            CAPITAL_GOODS = 4,
            SERVICE_CATG_ITEMS = 5,
            SCRAP_OR_WASTE = 6,

        }

        public enum FirmTypeE
        {
            Proprieter = 1,
            Partnership = 2,
            Company = 3,
        }

        //Priyanka [10-08-2018] : Added for transaction action types.
        public enum TranActionTypeE
        {
            READ = 1,
            DELETE = 2,

        }
        

        public enum SalutationE
        {
            MR = 1,
            MRS = 2,
            MISS = 3,
            DR = 4,
        }

        /// <summary>
        /// Vijaymala[31-10-2017]Added To Set Details Type for invoice other details
        /// </summary>

        public enum invoiceOtherDetailsTypeE
        {
            DESCRIPTION = 1,
            TERMSANDCONDITION = 2
        }

        public enum SAPMasters
        {
            UOM_Group = 1,
            Price_List = 2,
            Manufacturer = 3,
            Shipping_Type = 4,
            Warranty = 5,
            Location_Compartment = 6,
            State = 7
        }

        public enum bookingFilterTypeE
        {
            ALL = 0,
            CONFIRMED = 1,
            NOTCONFIRMED = 2

        }

        public enum RouteTypeE
        {
            ACTUAL = 1,
            SUGGESTED = 2
        }
        public enum InvoiceGenerateModeE
        {
            REGULAR = 0,
            BRMTOBM = 1,
            BMTOCUSTOMER = 2
        }

        /// <summary>
        /// Vijaymala[06-02-2018]Added To Set Firm Name
        /// </summary>
        public enum FirmNameE
        {
            BHAGYALAXMI = 1,
            SRJ = 2,
            KALIKA = 3,


        }
        //Sudhir[23-01-2017] Added for PageId for Support Details Entry.
        public enum SupportPageTypE
        {
            BILLING = 16,
            LOADING_SLIP = 5
        }

        public enum ReportingTypeE
        {
            ADMINISTRATIVE = 1,
            TECHNICAL = 2
        }

        //Priyanka [12-03-2018] : Added for Select Type of list in view booking summary list
        public enum SelectTypeE
        {
            DISTRICT = 1,
            STATE = 2,
            CNF = 3,
            DEALER = 4
        }


        public enum OtherTaxTypeE
        {
            PF = 1,
            FREIGHT = 2,
            CESS = 3,
            AFTERCESS = 4,
            TCS = 5,
            INSURANCE_AND_OTHER_EXPENSE = 6,
            TRANSPORTER_ADVANCE = 7,
            TDS = 10
        }

        // Vaibhva [25-April-2018] Added to seperate transaction table
        public enum TranTableType
        {
            TEMP = 1,
            FINAL = 2
        }

        // Vijaymala [22-06-2018] Added
        public enum CdType
        {
            IsPercent = 1,
            IsRs = 0
        }

        // Vijaymala [17-08-2018] Added 
        public enum BookingType
        {
            IsRegular = 1,
            IsOther = 2
        }
        public enum DataExtractionTypeE
        {
            IsRegular = 1,
            IsDelete = 2
        }


        public enum CurrencyE
        {
            INR = 1,
            USD = 2

        }

        public enum ReportE
        {
            NONE = 1,
            EXCEL = 2,
            PDF = 3,
            BOTH = 4,
            PDF_DONT_OPEN = 5,
            EXCEL_DONT_OPEN = 6
        }

        public enum pageElements
        {
            SKIP_BOOKING_APPROVAL = 265
        }

        //Harshala[27-08-2019] added by Harshala to maintain type Role and User
        public enum PermissionTypeE
        {
            Role = 1,
            User = 2
        }

        /// <summary>
        /// Priyanka [26-04-2019] : Added Enums for SAP.
        /// </summary>
        public enum ManageItemByIdE
        {
            NONE = 0,
            SERIAL_NO = 1,
            BATCHES = 2
        }

        public enum ItemMasterTypeE
        {
            LISTED = 0,
            NON_LISTED = 1,
            ALL = 2,
        }

        public enum GSTCodeDtlsEnum {
            CODE = 1,
            PERCENTAGE = 2,
            DESCRIPTION = 3,
        }

        public enum FinAccountTypeE
        {
            REVENUE = 1,
            EXPENSE = 2,
            OTHER = 3
        }
        public enum FinLedgerTypeE
        {
            GROUP = 1,
            LEDGER = 2,
        }

        public enum ModuleType
        {
            SIMPLIDELIVER = 1,
        }
        public enum ConsumableOrFixedAssetE
        {
            CONSUMABLE = 1,
            FIXED_ASSET=2
        }

        public enum AggregateFunctions
        {
            AVG = 1,
            COUNT = 2,
            MIN = 3,
            MAX = 4,
            SUM = 5,
        }
        #endregion

        public static DateTime ServerDateTime
        {
            get
            {
                return DAL.CommonDAO.SelectServerDateTime();
            }
        }

        #region Constants Or Static Strings
        public static String Local_URL = "http://localhost:4203";
        public static Boolean Local_API = Startup.IsLocalAPI;
        public static String CONNECTION_STRING = "ConnectionString";
        public static String AZURE_CONNECTION_STRING = "AzureConnectionStr";
        public static String REQUEST_ORIGIN_STRING = "RequestOriginString";
        public static Int32 SIMPLI_CHAT_ALERT_DEFINITION_ID = 1501;

        public static String SAP_CONNECTION_STRING = "DefaultSAPConnection";

        public static String TENANT_ID = "Tenant_Id";
        public static String SAP_HANDSHAKE_CONNECTION_STRING = "SAPHandShakeConnection";

        public static String IdentityColumnQuery = "SELECT @@IDENTITY";
        public static String DefaultCountry = "India";
        public static Int32 DefaultCountryID = 101;
        public static Int32 defaultDistrictID = 342;
        public static Int32 defaultstateID = 15;
        public static Int32 defaultPinCode = 0;

        public static String DefaultDateFormat = "dd-MM-yyyy HH:mm tt";
        public static String AzureDateFormat = "yyyy-MM-dd HH:mm tt";
        public static String DefaultPassword = "123";
        public static String DefaultErrorMsg = "Error - 00 : Record Could Not Be Saved";
        public static String DefaultSuccessMsg = "Success - Record Saved Successfully";

        //Default Currency Id and Rate is Indain
        public static int DefaultCurrencyID = 1;
        public static int DefaultCurrencyRate = 1;
         public static Int32 DefaultModuleID = 1;

        // Vaibhav [26-Sep-2017] added to set default company id to Bhagyalaxmi Rolling Mills
        public static int DefaultCompanyId = 19;
        public static int DefaultSalutationId = 1;

        // Vaibhav [17-Dec-2017] Added to file encrypt descrypt and upload to azure
        //public static string AzureConnectionStr = "DefaultEndpointsProtocol=https;AccountName=apkupdates;AccountKey=IvC+sc8RLDl3DeH8uZ97A4jX978v78bVFHRQk/qxg2C/J8w/DRslJlLsK7JTF+KhOM0MNUZg443GCVXe3jIanA==";
        //public static string EncryptDescryptKey = "MAKV2SPBNI99212";
        public static string AzureSourceContainerName = "simplimaster";
        public static string AzureTargetContainerName = "newdocuments";
        public static string ExcelSheetName = "TranData";
        public static string ExcelFileName = "Tran";
        public static int LoadingCountForDataExtraction = 50;
        public static String ENTITY_RANGE_NC_LOADINGSLIP_COUNT = "NC_LOADINGSLIP_COUNT";
        public static int FinYear = 2017;
        public static String ENTITY_RANGE_C_LOADINGSLIP_COUNT = "C_LOADINGSLIP_COUNT";
        public static String ENTITY_RANGE_LOADING_COUNT = "LOADING_COUNT";
        //Pandurang [2018-02-21] Added to take files from Azure 
        public static string AzureDocumentContainerName = "documentation";
        public static string HELP_DOCUMENT_CONFIG = "HELP_DOCUMENT_CONFIG";
        /// Samadhan[2022-May-25] Show STORLOCEWISE ITEM In Material Request
        public static string CP_SHOW_STORLOCEWISE_ITEM_REQUEST = "CP_SHOW_STORLOCEWISE_ITEM_REQUEST";// Value Either 0 or 1

        //Sudhir[24-04-2018] Added for CRM Documents.
        //public static string AzureSourceContainerNameForDocument = "kalikadocuments";

        //Sudhir[19-07-2018] Added for CRM Testing Documents.
        //public static string AzureSourceContainerNameForTestingDocument = "testingdocuments";
        public static int PreviousRecordDeletionDays = -2; //Use for Delete Previous 2 Days Records.

        //Hrushikesh added 
        //Permissions 
        public static String ReadWrite = "RW";
        public static String NotApplicable = "NA";

        public static string AzureTemplateContainerName = "invoicetemplates";

        public static string DEFAULT_FETCH_SUCCESS_MSG = "Record Fetch Succesfully";

        public static string DEFAULT_NOTFOUND_MSG = " Record Could not be found";
        public static string Organisation_Report_NAME = "OrganisationReport";
        public static string MASTER_IS_SAME_ORGANISATION_TEMPLATE_USE = "MASTER_IS_SAME_ORGANISATION_TEMPLATE_USE";
        public static string Dealer_Report_Name = "DealerReport";
        public static string Distributor_Report_Name = "DistributorReport";
        public static string Transporter_Report_Name = "TransporterReport";
        public static string Competitor_Report_Name = "CompetitorReport";
        public static string PurchaseCompetitor_Report_Name = "PurchaseCompetitorReport";
        public static string Supplier_Report_Name = "SupplierReport";
        public static string Supervisor_Report_Name = "SupervisorReport";
        public static string Influencer_Report_Name = "InfluencerReport";
        public static string InternalOrganization_Report_Name = "InternalOrganizationReport";
        public static string Bank_Report_Name = "BankReport";
        public static string ADDRESS_COUNT_PRINT_ORGANISATION_MASTER_TEMPLATE = "ADDRESS_COUNT_PRINT_ORGANISATION_MASTER_TEMPLATE";
        public static string ADDRESS_TYPE_PRINT_ORGANISATION_MASTER_TEMPLATE = "ADDRESS_TYPE_PRINT_ORGANISATION_MASTER_TEMPLATE";
        public static string PERSON_COUNT_PRINT_ORGANISATION_MASTER_TEMPLATE = "PERSON_COUNT_PRINT_ORGANISATION_MASTER_TEMPLATE";
        public static string PERSON_TYPE_PRINT_ORGANISATION_MASTER_TEMPLATE = "PERSON_TYPE_PRINT_ORGANISATION_MASTER_TEMPLATE";
        public static int Fun_Ref_Id = 3;

        public static string VOUCHER_NOTE = "VOUCHER_NOTE";
        public static string CP_USER_WISE_MODULE_VIEW = "CP_USER_WISE_MODULE_VIEW";
        public static string Allow_Module_On_Permisssion = "Allow_Module_On_Permisssion";
        public static string SERVER_DATETIME_QUERY_STRING = "SERVER_DATETIME_QUERY_STRING";
        public static string IS_LOCAL_API = "IS_LOCAL_API";
        public static string SAP_LOGIN_DETAILS = "SAP_LOGIN_DETAILS";

        #endregion

        #region Configuration Sections

        public static String IS_MAP_MY_INDIA = "IS_MAP_MY_INDIA";
        public static string CP_MAX_ALLOWED_DEL_PERIOD = "MAX_ALLOWED_DEL_PERIOD";
        public static string LOADING_SLIP_DEFAULT_SIZES = "LOADING_SLIP_DEFAULT_SIZES";
        public static string LOADING_SLIP_DEFAULT_SPECIFICATION = "LOADING_SLIP_DEFAULT_SPECIFICATION";
        public static string LOADING_SLIP_DEFAULT_CATEGORY = "LOADING_SLIP_DEFAULT_CATEGORY";
        public static string SMS_SUBSCRIPTION_ACTIVATION = "SMS_SUBSCRIPTION_ACTIVATION";
        public static string CP_AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION = "AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION";
        public static string CP_SYTEM_ADMIN_USER_ID = "SYTEM_ADMIN_USER_ID";
        public static string CP_COMPETITOR_TO_SHOW_IN_HISTORY = "COMPETITOR_TO_SHOW_IN_HISTORY";
        public static string CP_DELETE_ALERT_BEFORE_DAYS = "DELETE_ALERT_BEFORE_DAYS";
        public static string CP_MIN_AND_MAX_RATE_DEFAULT_VALUES = "MIN_AND_MAX_RATE_DEFAULT_VALUES";
        public static string CP_WEIGHT_TOLERANCE_IN_KGS = "WEIGHT_TOLERANCE_IN_KGS";
        public static string CP_WEIGHING_WEIGHT_TOLERANCE_IN_PERC = "WEIGHING_WEIGHT_TOLERANCE_IN_PERC";
        public static string CP_BOOKING_RATE_MIN_AND_MAX_BAND = "BOOKING_RATE_MIN_AND_MAX_BAND";
        public static string CP_MAX_ALLOWED_CD_STRUCTURE = "MAX_ALLOWED_CD_STRUCTURE";
        public static string CP_LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS = "LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS";
        public static string CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID = "LOADING_SLIPS_AUTO_POSTPONED_STATUS_IDS";
        public static string CP_LOADING_DEFAULT_ALLOWED_UPTO_TIME = "LOADING_DEFAULT_ALLOWED_UPTO_TIME";
        public static string CP_LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS = "LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS";
        public static string CP_DEFAULT_MATE_COMP_ORGID = "DEFAULT_MATE_COMP_ORGID";
        public static string CP_DEFAULT_MATE_SUB_COMP_ORGID = "DEFAULT_MATE_SUB_COMP_ORGID";
        public static string CP_APP_CONFIGURATION_AUTHENTICATION = "APP_CONFIGURATION_AUTHENTICATION";
        public static string CP_FRIEGHT_OTHER_TAX_ID = "FRIEGHT_OTHER_TAX_ID";
        public static string CP_REVERSE_CHARGE_MECHANISM = "REVERSE_CHARGE_MECHANISM";
        public static string CP_DEFAULT_WEIGHING_SCALE = "DEFAULT_WEIGHING_SCALE";
        public static string CP_BILLING_NOT_CONFIRM_AUTHENTICATION = "BILLING_NOT_CONFIRM_AUTHENTICATION";
        public static string CONSOLIDATE_STOCK = "CONSOLIDATE_STOCK";
        public static String ENTITY_RANGE_REGULAR_TAX_INVOICE_BMM = "REGULAR_TAX_INVOICE_BMM";

        public static string CP_BRAND_WISE_INVOICE = "BRAND_WISE_INVOICE";
        public static string CP_SKIP_LOADING_APPROVAL = "SKIP_LOADING_APPROVAL";
        public static string CP_SKIP_INVOICE_APPROVAL = "SKIP_INVOICE_APPROVAL";
        public static string CP_AUTO_MERGE_INVOICE = "AUTO_MERGE_INVOICE";

        public static string CP_INTERNALTXFER_INVOICE_ORG_ID = "INTERNALTXFER_INVOICE_ORG_ID";
        public static string CP_ADD_CNF_AGENT_IN_INVOICE = "ADD_CNF_AGENT_IN_INVOICE";

        public static string CP_EVERY_AUTO_INVOICE_WITH_EDIT = "EVERY_AUTO_INVOICE_WITH_EDIT";

        //Priyanka [2018-04-16] Added
        public static string CP_ALLOW_TO_CANCEL_LOADING_IF_TARE_WT_TAKEN = "ALLOW_TO_CANCEL_LOADING_IF_TARE_WT_TAKEN";

        // Vaibhav [29-Dec-2017] Added to config days to delete previous stock and quotadeclaration
        public static string CP_DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS = "DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS";
        public static string CP_MIGRATE_ENQUIRY_DATA = "MIGRATE_ENQUIRY_DATA";
        public static string CP_MIGRATE_BEFORE_DAYS = "MIGRATE_BEFORE_DAYS";
        //Pandurang[2018-10-03]Added for Delete Dispatch data
        public static string CP_DELETE_BEFORE_DAYS = "MIGRATE_BEFORE_DAYS";
        public static string CP_DATA_EXTRACTION_TYPE = "DATA_EXTRACTION_TYPE";

        // Vijaymala[14-02-2018] Added to Set Current Company
        public static string CP_CURRENT_COMPANY = "CURRENT_COMPANY";
        public static string CP_FIRBASE_ANDROID_NOTIFICATION_SETTINGS = "FIRBASE_ANDROID_NOTIFICATION_SETTINGS";

        // Vijaymala[14-02-2018] Added to Set Display Brand On Invoice
        public static string CP_DISPLAY_BRAND_ON_INVOICE = "DISPLAY_BRAND_ON_INVOICE";

        public static string CP_SIZEWISE_LOADING_REPORT_STATUS_IDS = "SIZEWISE_LOADING_REPORT_STATUS_IDS";


        // Vaibhav [21-Mar-2018] Added to get authentication server config params
        public static string CP_AUTHENTICATION_URL = "AUTHENTICATION_URL";
        public static string CP_CLIENT_ID = "CLIENT_ID";
        public static string CP_CLIENT_SECRET = "CLIENT_SECRET";
        public static string CP_SCOPE = "SCOPE";
        public static string CP_ISRABBITMQ_ENABLED = "ENABLE_RABBIT_MQ_MESSAGE_BROCKER";


        //Vijaymala [22-March-2018] Added to set Sales Ledger name in report 
        public static string CP_SALES_LEDGER_NAME = "SALES_LEDGER_NAME";

        // Vaibhav [11-April-2018] Added to set invoice date
        public static string CP_TARE_WEIGHT_DATE_AS_INV_DATE = "TARE_WEIGHT_DATE_AS_INV_DATE";
        public static string CP_CREATE_NC_DATA_FILE = "CREATE_NC_DATA_FILE";
        //Saket [2018-06-04] Added if true then invoice auth date as invoice date
        public static string CP_AUTHORIZATION_DATE_AS_INV_DATE = "AUTHORIZATION_DATE_AS_INV_DATE";

        public static string CP_DAFAULT_SAP_MAPPED_LOCATION_ID_FOR_PURCHASE_REQUEST = "DAFAULT_SAP_MAPPED_LOCATION_ID_FOR_PURCHASE_REQUEST";

        //Deepali [25-03-2021]
        public static string DEFAULT_TIME_TO_DEACTIVATE_NON_LISTED_ITEMS = "DEFAULT_TIME_TO_DEACTIVATE_NON_LISTED_ITEMS";
        
        //Vijaymala [02-05-2018] Added to set Dealer name in notification 
        public static string CP_ADD_DEALER_IN_NOTIFICATION = "ADD_DEALER_IN_NOTIFICATION";
        //Vijaymala[15 - 05 - 2018] Added to display discount on sales invoice export
        public static string CP_SHOW_DISCOUNT_ON_SALES_INVOICE_EXPORT = "SHOW_DISCOUNT_ON_SALES_INVOICE_EXPORT";

        //Vijaymala[15 - 05 - 2018] Added to export or print sales  export report
        public static string CP_PRINT_SALES_EXPORT_DIRECT_TO_PRINTER = "PRINT_SALES_EXPORT_DIRECT_TO_PRINTER";

        //Priyanka added [29-05-2018]
        public static string CP_MASTER_SUB_MASTER_BUNDLE_FACILITY = "MASTER_SUB_MASTER_BUNDLE_FACILITY";

        //Priyanka [07-06-2018] : Added for SHIVANGI
        public static string CP_PARTY_WISE_BLOCKING = "PARTY_WISE_BLOCKING";
        public static string CP_ENQUIRY_WISE_BLOCKING = "ENQUIRY_WISE_BLOCKING";
        public static string CP_DISPLAY_MATERIAL_DETAILS_ON_PAGE = "DISPLAY_MATERIAL_DETAILS_ON_PAGE";

        //Vijaymala[19 - 06 - 2018] Added to HIDE RATE BAND DECLARATION
        public static string CP_DISPLAY_RATE_BAND_DECLARATION = "DISPLAY_RATE_BAND_DECLARATION";
        public static string CP_RATE_DECLARATION_FOR_ENQUIRY = "DAILY_RATE_DECLARATION_FOR_ENQUIRY";
        public static string CP_CD_STRUCTURE_IN_PERCENTAGE = "CD_STRUCTURE_IN_PERCENTAGE";
        public static string CP_CD_STRUCTURE_IN_RS = "CD_STRUCTURE_IN_RS";
        public static string CP_FROM_LOCATION = "FROM_LOCATION";
        public static string CP_EDIT_BOOKING_DETAILS = "EDIT_BOOKING_DETAILS";
        public static string CP_AUTO_GATE_IN_VEHICLE = "AUTO_GATE_IN_VEHICLE";
        public static string CP_DEFAULT_FOR_VALUE = "DEFAULT_FOR_VALUE";
        //Harshkunj[26 - 06 - 2018] Added to set booking end time
        public static string CP_BOOKING_END_TIME = "BOOKING_END_TIME";

        //Priyanka [27-06-2018] Added to set view loading days.
        public static string CP_VIEW_LOADING_SLIP_DAYS = "VIEW_LOADING_SLIP_DAYS";
        public static string CP_DASHBOARD_ENQ_QTY_STATUSES = "DASHBOARD_ENQ_QTY_STATUSES";

        public static string CP_AUTO_FINANCE_APPROVAL_FOR_ENQUIRY = "AUTO_FINANCE_APPROVAL_FOR_ENQUIRY";

        //Sanjay [2017-07-04] Tax Calculations Inclusive Of Taxes Or Exclusive Of Taxes. Reported From Customer Shivangi Rolling Mills
        public static string CP_RATE_CALCULATIONS_TAX_INCLUSIVE = "RATE_CALCULATIONS_TAX_INCLUSIVE";

        //Priyanka [16-07-2018] Added for SHIVANGI (additional discount, displaying freight, convert freight into required format)  
        public static string CP_IS_ADDITIONAL_DISCOUNT_APPLICABLE = "IS_ADDITIONAL_DISCOUNT_APPLICABLE";
        public static string CP_DISPLAY_FREIGHT_ON_INVOICE = "DISPLAY_FREIGHT_ON_INVOICE";
        public static string CP_MINUS_FREIGHT_FROM_CALCULATION = "MINUS_FREIGHT_FROM_CALCULATION";

        //Priyanka [02-08-2018] Added for show the todays stock on dashboard.
        public static string CP_TODAYS_BOOKING_OPENING_BALANCE = "TODAYS_BOOKING_OPENING_BALANCE";

        //Priyanka [02-08-2018] Added for set the status for view enquiry statistics graphs
        public static string CP_ENQUIRY_STATISTICS_REPORT_STATUS = "ENQUIRY_STATISTICS_REPORT_STATUS";

        //Priyanka [06-09-2018] Added for set the view pending enquiries.(Show all or on date filter)
        public static string CP_VIEW_ALL_PENDING_ENQUIRIES = "VIEW_ALL_PENDING_ENQUIRIES";


        //Pandurang[2018-09-10] added for Stop web services
        public static string STOP_WEB_API_SERVICE_KEYS = "STOP_WEB_API_SERVICE_KEYS";
        public static string STOP_WEB_GUI_SERVICE_KEYS = "STOP_WEB_GUI_SERVICE_KEYS";

        public static string CP_STATUS_TO_CALCULATE_ENQUIRY_OPENING_BALANCE = "STATUS_TO_CALCULATE_ENQUIRY_OPENING_BALANCE";

        public static string CP_DISPLAY_DEALER_NAME_ON_WEIGHMENT_SLIP = "DISPLAY_DEALER_NAME_ON_WEIGHMENT_SLIP";
        public static string CP_EWAY_NUMBER_COMPULSARY_BEFORE_INVOICE_PRINT = "EWAY_NUMBER_COMPULSARY_BEFORE_INVOICE_PRINT";
        public static string CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION = "AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION";

        public static string CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION_TESTING = "AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION_TESTING";

        public static string CP_TRANSPORTER_MANDATORY_FOR_LOADING = "TRANSPORTER_MANDATORY_FOR_LOADING";
        public static string CP_DEFAULT_TRANSPORTER_SCOPE_FOR_BOOKING = "DEFAULT_TRANSPORTER_SCOPE_FOR_BOOKING";

        //Priyanka [29-10-2018] Added to allow the loading without stock.
        public static string CP_ALLOW_LOADING_WITHOUT_STOCK = "ALLOW_LOADING_WITHOUT_STOCK";

        //Priyanka [14-11-2018] Added to enable or disble the not-confirm option.
        public static string CP_HIDE_NOT_CONFIRM_OPTION = "HIDE_NOT_CONFIRM_OPTION";

        public static string CP_DISPLAY_SHIPPING_ADDRESS = "DISPLAY_SHIPPING_ADDRESS";


        //Priyanka [14-11-2018] Added to enable or disble the number if bundles on Invoice and loading slip.
        public static string CP_HIDE_BUNDLES_ON_INVOICE = "HIDE_BUNDLES_ON_INVOICE";
        public static string CP_HIDE_BUNDLES_ON_LOADING_SLIP = "HIDE_BUNDLES_ON_LOADING_SLIP";

        //Priyanka [19-11-2018] Added to set the default weight tolerance.
        public static string CP_DEFAULT_WEIGHING_TOLERANCE = "DEFAULT_WEIGHING_TOLERANCE";
        public static string CP_STATUS_AFTER_SAVE_BOOKING = "STATUS_AFTER_SAVE_BOOKING";
        public static string CP_VALIDATE_RATEBAND_FOR_BOOKING = "VALIDATE_RATEBAND_FOR_BOOKING";
        public static string CP_STATUS_FOR_SAVE_BOOKING_VALIDATION = "STATUS_FOR_BOOKING_VALIDATION";


        public static string CP_IS_SHIPPING_ADDRESS_VALIDATION_MANDATORY = "IS_SHIPPING_ADDRESS_VALIDATION_MANDATORY";

        public static string CP_DISPLAY_SEZ_FOR_BOOKING = "DISPLAY_SEZ_FOR_BOOKING";

        public static string CP_ROLES_TO_SEND_SMS_ABOUT_RATE_AND_QUOTA = "ROLES_TO_SEND_SMS_ABOUT_RATE_AND_QUOTA";


        public static string CP_IS_INVOICE_RATE_ROUNDED = "IS_INVOICE_RATE_ROUNDED";

        //Priyanka[17-12-2018]
        public static string CP_HIDE_SOLD_UNSOLD_STOCK = "HIDE_SOLD_UNSOLD_STOCK";

        public static string CP_EXISTING_ADDRESS_COUNT_FOR_BOOKING = "EXISTING_ADDRESS_COUNT_FOR_BOOKING";

        //Priyanka [20-12-2018] : Added to check the setting of sms template including sizes or not
        public static string CP_SMS_TEMPLATE_INCLUDING_SIZE = "SMS_TEMPLATE_INCLUDING_SIZE";

        //Priyanka [25-12-2018] : Added to display or hide the company name & address on weighment receipt
        public static string CP_SHOW_ADDRESS_ON_WEIGHMENT_RECEIPT = "SHOW_ADDRESS_ON_WEIGHMENT_RECEIPT";

        //Priyanka [26-12-2018] : Added to display or hide the final dispatch section on print loading slip.
        public static string CP_SHOW_FINAL_DISPATCH_ON_PRINT_LOADING_SLIP = "SHOW_FINAL_DISPATCH_ON_PRINT_LOADING_SLIP";

        // Aniket K[3-Jan-2019] : Added to set rate declaration history in days.
        public static string QUOTA_RATE_DECLARATION_HISTORY_IN_DAYS = "QUOTA_RATE_DECLARATION_HISTORY_IN_DAYS";

        public static string CP_IS_PRODUCTION_ENVIRONMENT_ACTIVE = "IS_PRODUCTION_ENVIRONMENT_ACTIVE";

        //Vijaymala [09-01-2019] Added 
        public static string CP_DISPLAY_CONSIGNEE_ADDRESS_ON_PRINTABLE_INVOICE = "DISPLAY_CONSIGNEE_ADDRESS_ON_PRINTABLE_INVOICE";

        // Aniket [05-02-2019] added to check brandwise invoice number generate or not
        public static string GENERATE_MANUALLY_BRANDWISE_INVOICENO = "GENERATE_MANUALLY_BRANDWISE_INVOICENO";
        public static string CP_VIEW_COMMERCIAL_DETAILS = "VIEW_COMMERCIAL_DETAILS";
        // Aniket [18-02-2019] added to check Other material Qty display on dashoboard or not
        public static string IS_OTHER_MATERIAL_QTY_HIDE_ON_DASHBOARD = "IS_OTHER_MATERIAL_QTY_HIDE_ON_DASHBOARD";
        //Aniket [27-02-2019] added to check whether allow or restrict CNF beyond booking quota 
        public static string ANNOUNCE_RATE_WITH_RATEBAND_CURRENT_QUOTA = "ANNOUNCE_RATE_WITH_RATEBAND_CURRENT_QUOTA";
        public static string RESTRICT_CNF_BEYOND_BOOKING_QUOTA = "RESTRICT_CNF_BEYOND_BOOKING_QUOTA";
        //Aniket [06-03-2019] added to check Math.Round() function to be used in tax calculations or not
        public static string IS_ROUND_OFF_TAX_INVOICE_CALCULATION = "IS_ROUND_OFF_TAX_INVOICE_CALCULATION";

        //Aniket [25-3-2019] added to check which statusId booking details exclude from CNC report
        public static string CNF_BOOKING_REPORT_EXCLUDE_STATUSID = "CNF_BOOKING_REPORT_EXCLUDE_STATUSID";

        //Aniket [22-4-2019] added to check whether vehicle suggestion is should show or hide
        public static string IS_HIDE_VEHICLE_LIST_SUGGESTION = "IS_HIDE_VEHICLE_LIST_SUGGESTION";

        //Aniket [10-6-2019]
        public static string IS_BALAJI_CLIENT = "IS_BALAJI_CLIENT";
        public static string SAPB1_SERVICES_ENABLE = "SAPB1_SERVICES_ENABLE";
        public static string VOUCHER_NOTE_PENDING_FOR_IMPORT_STATUS_LIST = "VOUCHER_NOTE_PENDING_FOR_IMPORT_STATUS_LIST";
        public static string CP_LICENSE_AGAINST_ADDRESS = "CP_LICENSE_AGAINST_ADDRESS";
        public static string IGNORE_ROLEIDS_FOR_ISSUE = "IGNORE_ROLEIDS_FOR_ISSUE";
        public static string IS_SEARCH_ITEM_FILITER_WHERE_CONDITIONS ="IS_SEARCH_ITEM_FILITER_WHERE_CONDITIONS";

        public static string PROJECT_SUBGROUP_ID = "PROJECT_SUBGROUP_ID";
        public static string CP_JSON_FOR_ORDER_ITEM_CREATION_PROJECT_WISE = "CP_JSON_FOR_ORDER_ITEM_CREATION_PROJECT_WISE";
        public static string CP_JSON_FOR_ORDER_ITEM_CREATION_SERVICE_WISE = "CP_JSON_FOR_ORDER_ITEM_CREATION_SERVICE_WISE";
        public static string CP_JSON_FOR_ORDER_ITEM_CREATION_SPARE_WISE = "CP_JSON_FOR_ORDER_ITEM_CREATION_SPARE_WISE";
        

        public static string CP_STOCK_IGNORE_USERS_OF_ROLEIDS_AGAINST_REQUISITION = "CP_STOCK_IGNORE_USERS_OF_ROLEIDS_AGAINST_REQUISITION";

        public static string CURRENCY_EXCHANGE_RATE_URL = "CURRENCY_EXCHANGE_RATE_URL";

        public static string ADD_GROUP_LEVEL_LIMIT = "ADD_GROUP_LEVEL_LIMIT";
        public static string ADD_LEDGER_LEVEL_LIMIT = "ADD_LEDGER_LEVEL_LIMIT";
        public static string CP_AUTHORIZE_LEDGER = "CP_AUTHORIZE_LEDGER";
        public static string RABBIT_MQ_CONFIGURATION_DETAILS = "RABBIT_MQ_CONFIGURATION_DETAILS";
        public static string IS_SEND_ALL_TYPE__USER_TO_HRGIRD = "IS_SEND_ALL_TYPE__USER_TO_HRGIRD";
        public static string CP_Scrap_Revaluation_Decrement_Account = "CP_Scrap_Revaluation_Decrement_Account";
        public static string CP_Scrap_Revaluation_Increment_Account = "CP_Scrap_Revaluation_Increment_Account";
        public static string GOOGLE_MAP_API_URL_FOR_LAT_LONG="GOOGLE_MAP_API_URL_FOR_LAT_LONG";
        public static string MAP_MY_INDIA_URL_FOR_myLocationAddress = "MAP_MY_INDIA_URL_FOR_myLocationAddress";
        public static string GOOGLE_MAP_API_URL_FOR_ADDRESS = "GOOGLE_MAP_API_URL_FOR_ADDRESS";
        public static string MAP_MY_INDIA_URL_FOR_getMatrixAPIUsingMapMyIndia = "MAP_MY_INDIA_URL_FOR_getMatrixAPIUsingMapMyIndia";
        public static string IS_SHOW_UOM_AND_COUM_DDL = "IS_SHOW_UOM_AND_COUM_DDL";

        public static string CP_CHART_OF_ACCOUNTS_WITH_SEGMENT_YN = "CP_CHART_OF_ACCOUNTS_WITH_SEGMENT_YN";
        public static string IS_ALLOW_DUPLICATE_ORGANISATION_FIRM_NAME = "IS_ALLOW_DUPLICATE_ORGANISATION_FIRM_NAME";
        public static string CP_USER_PRO_FLOW_INTEGRATION_CONFIGURATION = "CP_USER_PRO_FLOW_INTEGRATION_CONFIGURATION";
        public static string WHATS_APP_SEND_MESSAGE_INTEGRATION_API = "WHATS_APP_SEND_MESSAGE_INTEGRATION_API";
        public static string WHATS_APP_SEND_MESSAGE_REQUEST_JSON = "WHATS_APP_SEND_MESSAGE_REQUEST_JSON";
        public static string WHATS_APP_API_KEY = "WHATS_APP_API_KEY";
        public static string WHATS_APP_SEND_MESSAGE_REQUEST_HEADER_JSON = "WHATS_APP_SEND_MESSAGE_REQUEST_HEADER_JSON";
        public static string CP_SHOW_ONLY_DEALER_LIST_CNF_AGENT= "CP_SHOW_ONLY_DEALER_LIST_CNF_AGENT";
        public static string DEFAULT_GST_CODEID = "DEFAULT_GST_CODEID";

        #endregion

        #region Common functions

        public static Boolean IsNeedToRemoveFromList(string[] sizeList, Int32 materialId)
        {
            for (int i = 0; i < sizeList.Length; i++)
            {
                int sizeId = Convert.ToInt32(sizeList[i]);
                if (sizeId == materialId)
                {
                    return false;
                }
            }

            return true;
        }

        public static Boolean IsDateTime(String value)
        {
            try
            {
                Convert.ToDateTime(value);
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }

        }

        public static Boolean IsInteger(String value)
        {
            try
            {
                Convert.ToInt32(value);
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }

        }

        public static void SetNullValuesToEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        pi.SetValue(myObject, string.Empty);
                    }
                }
            }
        }
        public static DateTime GetStartDateTimeOfYear(DateTime dateTime)
        {
            if (dateTime.Month < 4)
                return GetStartDateTime(new DateTime(dateTime.Year - 1, 4, 1)); //1 Apr
            else
                return GetStartDateTime(new DateTime(dateTime.Year, 4, 1)); //1 Apr
        }

        public static DateTime GetEndDateTimeOfYear(DateTime dateTime)
        {
            if (dateTime.Month > 3)
                return GetEndDateTime(new DateTime(dateTime.Year + 1, 3, 31)); //31 March
            else
                return GetEndDateTime(new DateTime(dateTime.Year, 3, 31)); //31 March

        }

        public static DateTime GetStartDateTime(DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            return new DateTime(year, month, day, 0, 0, 0);
        }

        public static DateTime GetEndDateTime(DateTime dateTime)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            return new DateTime(year, month, day, 23, 59, 59);
        }

        public static List<string> GetChangedProperties(Object A, Object B)
        {
            if (A.GetType() != B.GetType())
            {
                throw new System.InvalidOperationException("Objects of different Type");
            }
            List<string> changedProperties = ElaborateChangedProperties(A.GetType().GetProperties(), B.GetType().GetProperties(), A, B);
            return changedProperties;
        }

        public static List<string> ElaborateChangedProperties(PropertyInfo[] pA, PropertyInfo[] pB, Object A, Object B)
        {
            List<string> changedProperties = new List<string>();
            foreach (PropertyInfo info in pA)
            {
                object propValueA = info.GetValue(A, null);
                object propValueB = info.GetValue(B, null);
                if (propValueA != null && propValueB != null)
                {
                    if (propValueA.ToString() != propValueB.ToString())
                    {
                        changedProperties.Add(info.Name);
                    }
                }
                else
                {
                    if (propValueA == null && propValueB != null)
                    {
                        changedProperties.Add(info.Name);
                    }
                    else if (propValueA != null && propValueB == null)
                    {
                        changedProperties.Add(info.Name);
                    }
                }
            }
            return changedProperties;
        }

        #endregion


        #region RabbitMQ Transactions
        public static String RABBIT_DEPARTMENT_ADD = "DepartmentAdded";
        public static String RABBIT_DEPARTMENT_UPDATE = "DepartmentUpdated";
        public static String RABBIT_DEPARTMENT_DEACTIVATED = "DepartmentDeactivated";

        public static String RABBIT_DESIGNATION_ADD = "DesignationAdded";
        public static String RABBIT_DESIGNATION_UPDATE = "DesignationUpdated";
        public static String RABBIT_DESIGNATION__DEACTIVATED = "DesignationDeactivated";

        public static String RABBIT_ROLE_DEACTIVATED = "RoleDeactivated";
        public static String RABBIT_ROLE_ADD = "RoleAdded";
        public static String RABBIT_ROLE_UPDATE = "RoleUpdated";

        public static String RABBIT_ORGSTRUCTURE_ADD = "OrgstructureAdded";
        public static String RABBIT_ORGSTRUCTURE_UPDATE = "OrgstructureUpdated";
        public static String RABBIT_ORGSTRUCTURE_DEACTIVATED = "OrgstructureDeactivated";

        public static String RABBIT_USER_ADD = "UserAdded";
        public static String RABBIT_USER_UPDATE = "UserUpdated";
        public static String RABBIT_USER_DEACTIVATED = "UserDeactivated";
        #endregion


        #region TenantConfig

        //Configuration of Auth keys of HR gird on Tenant

        public static Dictionary<string, TenantAuthTO> TenantDict = new Dictionary<string, TenantAuthTO>
    {
        {"Virgo_Tenant", new TenantAuthTO("GRDFE45JUHNB","https://virgo.hrgird.com/")},
    };



        #endregion

       public enum ProcurementTypeE
        {
            ALL = 0,
            MAKE = 1,
            BUY = 2,
        }

        //Added by minal
        public enum AllocationTypeE
        {
            InternalOrgAllocation = 1,
            PurchaseManger = 2,

        }

        //Gokul [18-03-21] Replace more than one spacecs
        public static string removeUnwantedSpaces(string str)                         //create function
        {

            string pattern = "\\s+";

            string replacement = " ";                       // replacement pattern

            Regex rx = new Regex(pattern);

            string result = rx.Replace(str, replacement); // call to replace space
            return result;
        }



        #region [Priyanka : 26-04-2019] : SAP Enum Methods for getting their name. 
        public static SAPbobsCOM.BoIssueMethod GetSAPIssueMethodEnum(int enumId)
        {
            SAPbobsCOM.BoIssueMethod enumValue = SAPbobsCOM.BoIssueMethod.im_Manual; //Default value will be manual
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.BoIssueMethod.im_Backflush:
                    enumValue = SAPbobsCOM.BoIssueMethod.im_Backflush;
                    break;
                case (Int32)SAPbobsCOM.BoIssueMethod.im_Manual:
                    enumValue = SAPbobsCOM.BoIssueMethod.im_Manual;
                    break;
            }
            return enumValue;
        }
        public static SAPbobsCOM.ItemTypeEnum GetSAPItemTypeEnum(int enumId)
        {
            SAPbobsCOM.ItemTypeEnum enumValue = new SAPbobsCOM.ItemTypeEnum();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.ItemTypeEnum.itFixedAssets:
                    enumValue = SAPbobsCOM.ItemTypeEnum.itFixedAssets;
                    break;
                case (Int32)SAPbobsCOM.ItemTypeEnum.itItems:
                    enumValue = SAPbobsCOM.ItemTypeEnum.itItems;
                    break;
                case (Int32)SAPbobsCOM.ItemTypeEnum.itLabor:
                    enumValue = SAPbobsCOM.ItemTypeEnum.itLabor;
                    break;
                case (Int32)SAPbobsCOM.ItemTypeEnum.itTravel:
                    enumValue = SAPbobsCOM.ItemTypeEnum.itTravel;
                    break;
            }
            return enumValue;
        }
        public static SAPbobsCOM.BoMRPComponentWarehouse GetMRPComponentWarehouseEnum(int enumId)
        {
            SAPbobsCOM.BoMRPComponentWarehouse enumValue = new SAPbobsCOM.BoMRPComponentWarehouse();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.BoMRPComponentWarehouse.bomcw_BOM:
                    enumValue = SAPbobsCOM.BoMRPComponentWarehouse.bomcw_BOM;
                    break;
                case (Int32)SAPbobsCOM.BoMRPComponentWarehouse.bomcw_Parent:
                    enumValue = SAPbobsCOM.BoMRPComponentWarehouse.bomcw_Parent;
                    break;
            }
            return enumValue;
        }
        public static SAPbobsCOM.BoGLMethods GetGLMethodsEnum(int enumId)
        {
            SAPbobsCOM.BoGLMethods enumValue = new SAPbobsCOM.BoGLMethods();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.BoGLMethods.glm_ItemClass:
                    enumValue = SAPbobsCOM.BoGLMethods.glm_ItemClass;
                    break;
                case (Int32)SAPbobsCOM.BoGLMethods.glm_ItemLevel:
                    enumValue = SAPbobsCOM.BoGLMethods.glm_ItemLevel;
                    break;
                case (Int32)SAPbobsCOM.BoGLMethods.glm_WH:
                    enumValue = SAPbobsCOM.BoGLMethods.glm_WH;
                    break;
            }
            return enumValue;
        }
        public static SAPbobsCOM.BoPlanningSystem GetPlanningSystemEnum(int enumId)
        {
            SAPbobsCOM.BoPlanningSystem enumValue = new SAPbobsCOM.BoPlanningSystem();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.BoPlanningSystem.bop_MRP:
                    enumValue = SAPbobsCOM.BoPlanningSystem.bop_MRP;
                    break;
                case (Int32)SAPbobsCOM.BoPlanningSystem.bop_None:
                    enumValue = SAPbobsCOM.BoPlanningSystem.bop_None;
                    break;
            }
            return enumValue;
        }

        public static SAPbobsCOM.BoProcurementMethod GetProcurementMethodEnum(int enumId)
        {
            SAPbobsCOM.BoProcurementMethod enumValue = new SAPbobsCOM.BoProcurementMethod();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.BoProcurementMethod.bom_Buy:
                    enumValue = SAPbobsCOM.BoProcurementMethod.bom_Buy;
                    break;
                case (Int32)SAPbobsCOM.BoProcurementMethod.bom_Make:
                    enumValue = SAPbobsCOM.BoProcurementMethod.bom_Make;
                    break;
            }
            return enumValue;
        }
        public static SAPbobsCOM.BoManageMethod GetManageMethodEnum(int enumId)
        {
            SAPbobsCOM.BoManageMethod enumValue = new SAPbobsCOM.BoManageMethod();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.BoManageMethod.bomm_OnEveryTransaction:
                    enumValue = SAPbobsCOM.BoManageMethod.bomm_OnEveryTransaction;
                    break;
                case (Int32)SAPbobsCOM.BoManageMethod.bomm_OnReleaseOnly:
                    enumValue = SAPbobsCOM.BoManageMethod.bomm_OnReleaseOnly;
                    break;
            }
            return enumValue;
        }
        public static SAPbobsCOM.BoMaterialTypes GetMaterialTypesEnum(int enumId)
        {
            SAPbobsCOM.BoMaterialTypes enumValue = new SAPbobsCOM.BoMaterialTypes();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_ConsumerMaterial:
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_ConsumerMaterial;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_FinishedGoods:  // Finished Goods
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_FinishedGoods;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_FixedAsset:  //Capital Goods
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_FixedAsset;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_GoodsForReseller:
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_GoodsForReseller;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_GoodsInProcess:
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_GoodsInProcess;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_IntermediateMaterial:
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_IntermediateMaterial;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_Other:
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_Other;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_OtherInput:
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_OtherInput;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_Package:
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_Package;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_RawMaterial: // Raw Material
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_RawMaterial;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_Service:
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_Service;
                    break;
                case (Int32)SAPbobsCOM.BoMaterialTypes.mt_SubProduct:
                    enumValue = SAPbobsCOM.BoMaterialTypes.mt_SubProduct;
                    break;
            }
            return enumValue;
        }
        public static SAPbobsCOM.BoYesNoEnum GetYesNoEnum(int enumId)
        {
            SAPbobsCOM.BoYesNoEnum enumValue = new SAPbobsCOM.BoYesNoEnum();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.BoYesNoEnum.tNO:
                    enumValue = SAPbobsCOM.BoYesNoEnum.tNO;
                    break;
                case (Int32)SAPbobsCOM.BoYesNoEnum.tYES:
                    enumValue = SAPbobsCOM.BoYesNoEnum.tYES;
                    break;
            }
            return enumValue;
        }

        public static SAPbobsCOM.BoInventorySystem GetBoInventorySystemEnum(int enumId)
        {
            SAPbobsCOM.BoInventorySystem enumValue = new SAPbobsCOM.BoInventorySystem();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.BoInventorySystem.bis_FIFO:
                    enumValue = SAPbobsCOM.BoInventorySystem.bis_FIFO;
                    break;
                case (Int32)SAPbobsCOM.BoInventorySystem.bis_MovingAverage:
                    enumValue = SAPbobsCOM.BoInventorySystem.bis_MovingAverage;
                    break;
                case (Int32)SAPbobsCOM.BoInventorySystem.bis_SNB:
                    enumValue = SAPbobsCOM.BoInventorySystem.bis_SNB;
                    break;
                case (Int32)SAPbobsCOM.BoInventorySystem.bis_Standard:
                    enumValue = SAPbobsCOM.BoInventorySystem.bis_Standard;
                    break;
            }
            return enumValue;
        }
        //Priyanka [20-06-2019] : Added for GST Tax category type in product item
        public static SAPbobsCOM.GSTTaxCategoryEnum GetGstTaxCtg(int enumId)
        {
            SAPbobsCOM.GSTTaxCategoryEnum enumValue = new SAPbobsCOM.GSTTaxCategoryEnum();
            switch (enumId)
            {
                case (Int32)SAPbobsCOM.GSTTaxCategoryEnum.gtc_Regular:
                    enumValue = SAPbobsCOM.GSTTaxCategoryEnum.gtc_Regular;
                    break;
                case (Int32)SAPbobsCOM.GSTTaxCategoryEnum.gtc_NilRated:
                    enumValue = SAPbobsCOM.GSTTaxCategoryEnum.gtc_NilRated;
                    break;
                case (Int32)SAPbobsCOM.GSTTaxCategoryEnum.gtc_Exempt:
                    enumValue = SAPbobsCOM.GSTTaxCategoryEnum.gtc_Exempt;
                    break;
            }
            return enumValue;
        }
        #endregion
    }
    /// <summary>
    /// Sanjay [25-Nov-2019] To Divide given list into multiple units wrt given size criteria
    /// </summary>
    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }




}

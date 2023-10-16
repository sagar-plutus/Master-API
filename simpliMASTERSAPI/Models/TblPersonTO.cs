using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblPersonTO
    {
        #region Declarations
        Int32 idPerson;
        Int32 salutationId;
        String mobileNo;
        Int32 userTypeId;
        String alternateMobNo;
        String phoneNo;
        Int32 createdBy;
        DateTime dateOfBirth;
        DateTime createdOn;
        String firstName;
        String midName;
        String lastName;
        String primaryEmail;
        String alternateEmail;
        String comments;
        Int32 seqNo;
        String salutationName;
        String photoBase64;
        Int32 dobDay;
        Int32 dobMonth;
        Int32 dobYear;
        Int32 orgTypeId;
        DateTime dateOfAnniversary;
        Int32 otherDesignationId;
        Int32 organizationId;
        Int32 personTypeId;
        string emailAddr;
        string multiroleDesc;
        String mobNoCntryCode;
        String altMobNoCntryCode;
        Int32 delPeriodId;
        String userLoginName;
        Int32 userId;
        Int32 userIsActive;
        Int32 cdStructureId;
        Int32 isDefault1;
        Int32 isDefault2;
        public string CommonAttachId { get; set; }
        #endregion

        #region Constructor
        public TblPersonTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPerson
        {
            get { return idPerson; }
            set { idPerson = value; }
        }

         public Int32 UserTypeId
        {
            get { return userTypeId; }
            set { userTypeId = value; }
        }

        public Int32 IsUserCreate { get; set; }
        public Int32 isActive { get; set; }
        public Int32 OrgTypeId
        {
            get { return orgTypeId; }
            set { orgTypeId = value; }
        }
        
        public Int32 SalutationId
        {
            get { return salutationId; }
            set { salutationId = value; }
        }
        public String MobileNo
        {
            get { return mobileNo; }
            set { mobileNo = value; }
        }
        public String AlternateMobNo
        {
            get { return alternateMobNo; }
            set { alternateMobNo = value; }
        }
        public String PhoneNo
        {
            get { return phoneNo; }
            set { phoneNo = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public String MidName
        {
            get { return midName; }
            set { midName = value; }
        }
        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public String PrimaryEmail
        {
            get { return primaryEmail; }
            set { primaryEmail = value; }
        }
        public String AlternateEmail
        {
            get { return alternateEmail; }
            set { alternateEmail = value; }
        }
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public Int32 SeqNo
        {
            get { return seqNo; }
            set { seqNo = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public Int32 PersonTypeId
        {
            get { return personTypeId; }
            set { personTypeId = value; }
        }
        public Int32 IsDefault1
        {
            get { return isDefault1; }
            set { isDefault1 = value; }
        }

        public Int32 IsDefault2
        {
            get { return isDefault2; }
            set { isDefault2 = value; }
        }


        public Int32 DelPeriodId
        {
            get { return delPeriodId; }
            set { delPeriodId = value; }
        }
        public Int32 CdStructureId
        {
            get { return cdStructureId; }
            set { cdStructureId = value; }
        }

        



        public string SalutationName { get => salutationName; set => salutationName = value; }
        public string PhotoBase64 { get => photoBase64; set => photoBase64 = value; }
        public String DateOfBirthStr
        {
            get
            {
                if (dateOfBirth != DateTime.MinValue)
                    return dateOfBirth.ToString("dd-MM-yyyy");
                else return string.Empty;
            }
        }

        public int DobDay { get => dobDay; set => dobDay = value; }
        public int DobMonth { get => dobMonth; set => dobMonth = value; }
        public int DobYear { get => dobYear; set => dobYear = value; }
        public DateTime DateOfAnniversary { get => dateOfAnniversary; set => dateOfAnniversary = value; }
        public int OtherDesignationId { get => otherDesignationId; set => otherDesignationId = value; }
        public string EmailAddr { get => emailAddr; set => emailAddr = value; }
        public string MultiroleDesc { get => multiroleDesc; set => multiroleDesc = value; }
        public string MobNoCntryCode { get => mobNoCntryCode; set => mobNoCntryCode = value; }
        public string AltMobNoCntryCode { get => altMobNoCntryCode; set => altMobNoCntryCode = value; }
        public string UserLoginName { get => userLoginName; set => userLoginName = value; }
        public int UserId { get => userId; set => userId = value; }
        public int UserIsActive { get => userIsActive; set => userIsActive = value; }
        #endregion
    }

    public class TblPersonTOEmail
    {
        #region Declarations
        Int32 idOrganization;
        Int32 idPerson;
        string firmName;
        string emailAddr;
        string primaryEmail;
        string alternateEmail;
        Int32 salutationId;
        #endregion

        #region GetSet
        public int IdOrganization { get => idOrganization; set => idOrganization = value; }
        public int IdPerson { get => idPerson; set => idPerson = value; }
        public string FirmName { get => firmName; set => firmName = value; }
        public string EmailAddr { get => emailAddr; set => emailAddr = value; }
        public string PrimaryEmail { get => primaryEmail; set => primaryEmail = value; }
        public string AlternateEmail { get => alternateEmail; set => alternateEmail = value; }
        public int SalutationId { get => salutationId; set => salutationId = value; }
        #endregion
    }
}

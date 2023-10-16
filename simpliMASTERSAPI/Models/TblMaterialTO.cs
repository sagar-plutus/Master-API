using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblMaterialTO
    {
        #region Declarations
        Int32 idMaterial;
        Int32 mateCompOrgId;
        Int32 mateSubCompOrgId;
        Int32 materialTypeId;
        Int32 createdBy;
        DateTime createdOn;
        String materialSubType;
        String userDisplayName;
        Int32 updatedBy;
        DateTime updatedOn;
        Int32 deactivatedBy;
        DateTime deactivatedOn;
        Int32 isActive;
        #endregion

        #region Declarations
        DateTime createOn;
        int idTestDtl;
        int materialId;
        //int createdBy;
        //int isActive;
        DateTime testingDate;
        decimal chemC;
        decimal chemS;
        decimal chemP;
        decimal mechProof;
        decimal mechTen;
        decimal mechElon;
        decimal mechTEle;
        string castNo;
        string grade;
        #endregion
        #region Constructor
        public TblMaterialTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdMaterial
        {
            get { return idMaterial; }
            set { idMaterial = value; }
        }
        public Int32 MateCompOrgId
        {
            get { return mateCompOrgId; }
            set { mateCompOrgId = value; }
        }
        public Int32 MateSubCompOrgId
        {
            get { return mateSubCompOrgId; }
            set { mateSubCompOrgId = value; }
        }
        public Int32 MaterialTypeId
        {
            get { return materialTypeId; }
            set { materialTypeId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String MaterialSubType
        {
            get { return materialSubType; }
            set { materialSubType = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Int32 DeactivatedBy
        {
            get { return deactivatedBy; }
            set { deactivatedBy = value; }
        }
        public DateTime DeactivatedOn
        {
            get { return deactivatedOn; }
            set { deactivatedOn = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public string UserDisplayName { get => userDisplayName; set => userDisplayName = value; }

        #endregion
        #region GetSet
        public decimal ChemCE
        {
            get; set;
        }
        public decimal ChemT
        {
            get; set;
        }
        public DateTime CreateOn
        {
            get { return createOn; }
            set { createOn = value; }
        }
        public int IdTestDtl
        {
            get { return idTestDtl; }
            set { idTestDtl = value; }
        }
        public int MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public DateTime TestingDate
        {
            get { return testingDate; }
            set { testingDate = value; }
        }
        public decimal ChemC
        {
            get { return chemC; }
            set { chemC = value; }
        }
        public decimal ChemS
        {
            get { return chemS; }
            set { chemS = value; }
        }
        public decimal ChemP
        {
            get { return chemP; }
            set { chemP = value; }
        }
        public decimal MechProof
        {
            get { return mechProof; }
            set { mechProof = value; }
        }
        public decimal MechTen
        {
            get { return mechTen; }
            set { mechTen = value; }
        }
        public decimal MechElon
        {
            get { return mechElon; }
            set { mechElon = value; }
        }
        public decimal MechTEle
        {
            get { return mechTEle; }
            set { mechTEle = value; }
        }
        public string CastNo
        {
            get { return castNo; }
            set { castNo = value; }
        }
        public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        #endregion
    }
    public class SizeTestingDtlTO
    {
        #region Declarations
        DateTime  createOn;
        int idTestDtl;
        int materialId;
        int createdBy;
        int isActive;
        DateTime testingDate;
        decimal chemC;
        decimal chemS;
        decimal chemP;
        decimal mechProof;
        decimal mechTen;
        decimal mechElon;
        decimal mechTEle;
        string castNo;
        string grade;
        #endregion

        #region Constructor
        public SizeTestingDtlTO()
        {
        }

        #endregion

        #region GetSet
        public DateTime  CreateOn
        {
            get { return createOn; }
            set { createOn = value; }
        }
        public int IdTestDtl
        {
            get { return idTestDtl; }
            set { idTestDtl = value; }
        }
        public int MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public int CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public int IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime TestingDate
        {
            get { return testingDate; }
            set { testingDate = value; }
        }
        public decimal ChemC
        {
            get { return chemC; }
            set { chemC = value; }
        }
        public decimal ChemS
        {
            get { return chemS; }
            set { chemS = value; }
        }
        public decimal ChemP
        {
            get { return chemP; }
            set { chemP = value; }
        }
        public decimal MechProof
        {
            get { return mechProof; }
            set { mechProof = value; }
        }
        public decimal MechTen
        {
            get { return mechTen; }
            set { mechTen = value; }
        }
        public decimal MechElon
        {
            get { return mechElon; }
            set { mechElon = value; }
        }
        public decimal MechTEle
        {
            get { return mechTEle; }
            set { mechTEle = value; }
        }
        public decimal ChemCE
        {
            get;set;
        }
        public decimal ChemT
        {
            get;set;
        }
        public string CastNo
        {
            get { return castNo; }
            set { castNo = value; }
        }
        public string  Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        #endregion
    }
}

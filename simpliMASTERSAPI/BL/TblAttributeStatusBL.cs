using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL
{
    public class TblAttributeStatusBL : ITblAttributeStatusBL
    {
        private readonly ITblAttributeStatusDAO _iTblAttributeStatusDAO;
        private readonly ITblCRMLabelDAO _iTblCRMLabelDAO;
        private readonly ITblAttributesDAO _iTblAttributesDAO;
        private readonly ITblCRMLanguageDAO _iTblCRMLanguageDAO;
        private readonly ICommon _iCommon;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblUserExtDAO _iTblUserExtDAO;
        public TblAttributeStatusBL(ITblAttributeStatusDAO iTblAttributeStatusDAO, IConnectionString iConnectionString, ICommon iCommon, ITblCRMLabelDAO iTblCRMLabelDAO, ITblAttributesDAO iTblAttributesDAO, ITblCRMLanguageDAO iTblCRMLanguageDAO
            , ITblUserExtDAO iTblUserExtDAO)
        {
            _iCommon = iCommon;
            _iConnectionString = iConnectionString;
            _iTblAttributeStatusDAO = iTblAttributeStatusDAO;
            _iTblCRMLabelDAO = iTblCRMLabelDAO;
            _iTblAttributesDAO = iTblAttributesDAO;
            _iTblCRMLanguageDAO = iTblCRMLanguageDAO;
            _iTblUserExtDAO = iTblUserExtDAO;
        }
        #region Selection

        public List<AttributeStatusTO> GetAllAttributeStatusList(Int32 pageId, int orgTypeId, int userId)
        {
            Int32 selectedLanguageId = 0;

            List<AttributeStatusTO> AllAttributeStatusTO = _iTblAttributeStatusDAO.SelectAllAttributeStatusLabelList(pageId, orgTypeId);

            List<TblAttributesTO> attributesList = _iTblAttributesDAO.SelectAllAttributesForPage(pageId);

            #region Get default language for user

            if (userId > 0)
            {
                TblUserExtTO tblUserExtTO = _iTblUserExtDAO.SelectTblUserExt(userId);
                if (tblUserExtTO != null) { selectedLanguageId = tblUserExtTO.LagId; }
            }
            if (selectedLanguageId == 0)
            {
                List<tblCRMLanguageTO> languageList = _iTblCRMLanguageDAO.SelectAllTblCRMLanguage();
                if (languageList != null)
                {
                    selectedLanguageId = languageList.Where(x => x.isDefault == 1).Select(x => x.idLanguage).FirstOrDefault();


                    if (selectedLanguageId == 0)
                    {
                        selectedLanguageId = languageList.FirstOrDefault().idLanguage;
                    }
                }

            }

            #endregion

            List<AttributeStatusTO> AllAttributeLanguageStatusTO = new List<AttributeStatusTO>();

            foreach (TblAttributesTO attr in attributesList)
            {
                AttributeStatusTO AttributeStatusTONew;

                AttributeStatusTONew = AllAttributeStatusTO.Where(x => x.AttributeId == attr.idAttribute && x.lagId == selectedLanguageId).FirstOrDefault();

                if (AttributeStatusTONew != null)
                {
                    AttributeStatusTONew.AttributeDisplayName = AttributeStatusTONew.valueLabel;
                }
                else
                {
                    AttributeStatusTONew = AllAttributeStatusTO.Where(x => x.AttributeId == attr.idAttribute).FirstOrDefault();
                }

                if (AttributeStatusTONew == null)
                {
                    AttributeStatusTONew = new AttributeStatusTO();
                    AttributeStatusTONew.AttributeDisplayName = attr.attributeDisplayName;
                    AttributeStatusTONew.AttributeName = attr.attributeName;
                    AttributeStatusTONew.AttributeId = attr.idAttribute;
                    AttributeStatusTONew.PageId = pageId;
                    AttributeStatusTONew.OrgTypeId = orgTypeId;

                }

                if (AttributeStatusTONew != null)
                {
                    AllAttributeLanguageStatusTO.Add(AttributeStatusTONew);
                }
            }

            return AllAttributeLanguageStatusTO;
        }


        public List<AttributePageTO> AllAttributePages()
        {
            return _iTblAttributeStatusDAO.SelectAllAttributePages();

        }


        public List<AttributeStatusTO> AllAttributeListForUI(Int32 pageId, int orgTypeId)
        {
            return _iTblAttributeStatusDAO.AllAttributeListForUI(pageId, orgTypeId);
        }

        public List<TblCRMLabelTO> GetAttributeLanguageDetails(Int32 attrId)
        {
            List<tblCRMLanguageTO> languageList = _iTblCRMLanguageDAO.SelectAllTblCRMLanguage();

            TblAttributesTO attributesTO = _iTblAttributesDAO.SelectAttributesById(attrId);

            List<TblCRMLabelTO> labelList = _iTblCRMLabelDAO.SelectAllTblCRMLabelForAttribute(attrId);

            List<TblCRMLabelTO> labelLanguageList = new List<TblCRMLabelTO>();

            foreach (tblCRMLanguageTO CRMLanguageTO in languageList)
            {
                TblCRMLabelTO labelLanguageListNew;

                labelLanguageListNew = labelList.Where(x => x.LagId == CRMLanguageTO.idLanguage).FirstOrDefault();

                if (labelLanguageListNew != null)
                {
                    labelLanguageListNew.LagName = CRMLanguageTO.lagName;
                    labelLanguageList.Add(labelLanguageListNew);
                }
                else
                {
                    labelLanguageListNew = new TblCRMLabelTO();
                    labelLanguageListNew.LagId = CRMLanguageTO.idLanguage;
                    labelLanguageListNew.LagName = CRMLanguageTO.lagName;
                    labelLanguageListNew.KeyLabel = attributesTO.attributeName;
                    labelLanguageListNew.ValueLabel = null;
                    labelLanguageListNew.AttributeId = attributesTO.idAttribute;
                    labelLanguageListNew.PageId = attributesTO.pageId;
                    labelLanguageList.Add(labelLanguageListNew);
                }
            }

            return labelLanguageList;
        }


        public TblAttributesTO SelectAttributesByName(string attrName)
        {
            return _iTblAttributesDAO.SelectAttributesByName(attrName);
        }
        #endregion


        #region Insertion


        /// <summary>
        /// Hrushikesh Added 
        /// </summary>
        /// <param name="attributeTOList"></param>
        /// <returns></returns>
        public ResultMessage InsertAttributeMultiList(List<AttributeStatusTO> attributeTOList)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {


                List<Task> tskList = new List<Task>();
                for (int i = 0; i < attributeTOList.Count; i++)
                {
                    AttributeStatusTO attrTO = attributeTOList[i];
                    Task task1 = Task.Factory.StartNew(() => InsertAttributeList(attrTO));
                    tskList.Add(task1);
                }
                Task.WaitAll(tskList.ToArray());
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;


            }

            catch (Exception e)
            {

                resultMessage.DefaultExceptionBehaviour(e, "Error Inserting Attributes");
                return resultMessage;
            }

        }



        public ResultMessage InsertAttributeList(AttributeStatusTO attributeTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {

                int result = 0;

                conn.Open();
                tran = conn.BeginTransaction();

                if (attributeTO != null)
                {
                    List<AttributeStatusTO> existingList = _iTblAttributeStatusDAO.CheckDataExistsOrNot(attributeTO);
                    if (existingList != null && existingList.Count > 0)
                    {
                        var element = existingList[0];
                        if (element.IdAttributesStatus > 0)
                        {
                            attributeTO.IdAttributesStatus = element.IdAttributesStatus;
                            attributeTO.AttributeDisplayName = element.AttributeDisplayName;
                            attributeTO.UpdatedBy = attributeTO.CreatedBy;
                            attributeTO.IsActive = element.IsActive;
                            attributeTO.UpdatedOn = _iCommon.ServerDateTime;
                            result = _iTblAttributeStatusDAO.UpdateTblAttributeStatus(attributeTO, conn, tran);
                            if (result != 1)
                            {
                                throw new Exception("Error Updating TblAttributeStatus : Attribute Name" + attributeTO.AttributeName);
                            }
                        }
                    }
                }

                if (attributeTO.IdAttributesStatus == 0)
                {
                    attributeTO.CreatedOn = _iCommon.ServerDateTime;
                    attributeTO.CreatedBy = attributeTO.CreatedBy;
                    
                    result = _iTblAttributeStatusDAO.InsertTblAttributeStatus(attributeTO, conn, tran);
                    if (result != 1)
                    {
                        throw new Exception("Error Inserting TblAttributeStatus: Attribute Name" + attributeTO.AttributeName);
                    }
                }
                //else
                //{
                //    attributeTO.UpdatedOn = _iCommon.ServerDateTime;
                //    result = _iTblAttributeStatusDAO.UpdateTblAttributeStatus(attributeTO, conn, tran);
                //    if (result != 1)
                //    {
                //        throw new Exception("Error Updating TblAttributeStatus : Attribute Name" + attributeTO.AttributeName);
                //    }
                //}

                tran.Commit();

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception e)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(e, "Error In Method InsertAttributeList");
                return resultMessage;
            }
            finally
            {

                conn.Close();
            }


        }


        public List<DropDownTO> GetAttributeSrcList(AttributePageTO attrSrcTO)
        {

            return _iTblAttributeStatusDAO.GetAttributeSrcList(attrSrcTO);
        }


        public int PostEditAttributeName(AttributeStatusTO attributeTO)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                int result = 0;
                conn.Open();
                tran = conn.BeginTransaction();

                if (attributeTO.AttributeId != 0)
                {
                    result = _iTblAttributeStatusDAO.UpdateEditedAttributeName(attributeTO, conn, tran);
                    if (result != 1)
                    {
                        throw new Exception("Error Updating TblAttributes: Attribute Name" + attributeTO.AttributeDisplayName);
                    }

                    if (attributeTO.IdAttributesStatus != 0)
                    {
                        result = _iTblAttributeStatusDAO.UpdateEditedAttributeNameForAttrStatus(attributeTO, conn, tran);
                        if (result != 1)
                        {
                            throw new Exception("Error Updating tblAttributeStatus: Attribute Name" + attributeTO.AttributeDisplayName);
                        }
                    }
                }

                tran.Commit();

                return 1;
            }
            catch (Exception e)
            {
                tran.Rollback();
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }

        public int PostEditAttributeNameByLanguage(TblCRMLabelTO tblCRMLabelTO)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                int result = 0;
                conn.Open();
                tran = conn.BeginTransaction();

                if (tblCRMLabelTO.IdLabel != 0)
                {
                    tblCRMLabelTO.UpdatedOn = _iCommon.ServerDateTime;
                    result = _iTblCRMLabelDAO.UpdateTblCRMLabelValue(tblCRMLabelTO, conn, tran);
                    if (result != 1)
                    {
                        throw new Exception("Error Updating TblCRMLabel: KeyLabel" + tblCRMLabelTO.KeyLabel);
                    }
                }
                else
                {
                    tblCRMLabelTO.CreatedOn = _iCommon.ServerDateTime;
                    tblCRMLabelTO.IsActive = 1;
                    result = _iTblCRMLabelDAO.InsertTblCRMLabel(tblCRMLabelTO, conn, tran);

                    if (result != 1)
                    {
                        throw new Exception("Error Adding TblCRMLabel: KeyLabel" + tblCRMLabelTO.KeyLabel);
                    }
                }

                tran.Commit();

                return 1;
            }
            catch (Exception e)
            {
                tran.Rollback();
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

    }
}


using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static ODLMWebAPI.StaticStuff.Constants;
namespace ODLMWebAPI.Models
{
    public class CopyFromToPermissionTO
        {
          #region Declarations
          Int32 idFrom;
          Int32 roleUserFromId;
          Int32 idTo;
          Int32 roleUserToId;
          Int32 createdBy;
          DateTime createdOn;
          #endregion
         
         
          #region Constructor
           public CopyFromToPermissionTO()
           {
           }
          #endregion
        
        
        #region GetSet
        public int IdFrom { get => idFrom; set => idFrom = value; }
        public int RoleUserFromId { get => roleUserFromId; set => roleUserFromId = value; }
        public int IdTo { get => idTo; set => idTo = value; }
        public int RoleUserToId { get => roleUserToId; set => roleUserToId = value; }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        #endregion



    }
}
using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;
using Microsoft.AspNetCore.Http;
using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.Models
{
    public class TblWishListDtlsTO
    {
        #region Declarations
        Int32 wishlistId;
        Int32 userId;
        String wishlist;
        DateTime createdDate;
        bool isDelete;
        #endregion

        #region Constructor
        public TblWishListDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 WishlistId
        {
            get { return wishlistId; }
            set { wishlistId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public String Wishlist
        {
            get { return wishlist; }
            set { wishlist = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }
        public bool IsDelete
        { 
            get { return isDelete; }
            set{ isDelete = value; } 
        }

        #endregion
    }
}

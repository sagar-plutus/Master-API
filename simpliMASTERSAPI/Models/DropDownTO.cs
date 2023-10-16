using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{

    /// <summary>
    /// Sanjay [2017-02-10] This Model is used to return the values to caller
    /// when dimensions needs to be shown in DropDown
    /// </summary>
    public class DropDownTO
    {

        #region

        Int32 value;
        String text;
        Object tag;
        String code;
        String mappedTxnId;
        String icon;
        Int32 flag;
        #endregion

        #region Get Set

        /// <summary>
        /// Sanjay [2017-02-10] To Hold the Id of the Dropdown
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Hold the Text to be shown in dropdown
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public object Tag
        {
            get
            {
                return tag;
            }

            set
            {
                tag = value;
            }
        }
        public Int32 Flag
        {
            get
            {
                return flag;
            }

            set
            {
                flag = value;
            }
        }

        /// <summary>
        /// Sanjay [25-apr-2019] Defined to carry code values of masters if any.
        /// for e.g. stagecode MH,KT etc
        /// </summary>
        public string Code { get => code; set => code = value; }
        public string MappedTxnId { get => mappedTxnId; set => mappedTxnId = value; }
        public string Icon { get => icon; set => icon = value; }
        public Boolean isScrapProdItem { get; set; }
        #endregion
    }


    public class DropDownToForParity
    {
        #region

        Int32 id;
        String itemName;


        #endregion
        #region Get
        public string ItemName
        {
            get
            {
                return itemName;
            }

            set
            {
                itemName = value;
            }
        }
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                this.id = value;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class TblAssetClassTO
    {
        #region Declaration
        int idAssetClass;
        string assetName;
        string assetDesc;
        int assetTypeId;
        string mapSapId;
        #endregion

        public TblAssetClassTO()
        {
        }

        public int IdAssetClass
        {
            get { return idAssetClass; }
            set { idAssetClass = value; }
        }
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        public string AssetDesc
        {
            get { return assetDesc; }
            set { assetDesc = value; }
        }
        public int AssetTypeId
        {
            get { return assetTypeId; }
            set { assetTypeId = value; }
        }
        public string MapSapId
        {
            get { return mapSapId; }
            set { mapSapId = value; }
        }

    }
}

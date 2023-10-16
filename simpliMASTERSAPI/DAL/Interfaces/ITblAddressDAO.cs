using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblAddressDAO
    { 
        String SqlSelectQuery();
        List<TblAddressTO> SelectAllTblAddress();
        TblAddressTO SelectTblAddress(Int32 idAddr);
        TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE, SqlConnection conn = null, SqlTransaction tran = null);
        List<TblAddressTO> SelectOrgAddressList(Int32 orgId);
        List<TblAddressTO> ConvertDTToList(SqlDataReader tblAddressTODT);
        List<TblAddressTO> SelectOrgAddressDetailOfRegion(string orgId, StaticStuff.Constants.AddressTypeE addressTypeE, SqlConnection conn, SqlTransaction tran);
        int InsertTblAddress(TblAddressTO tblAddressTO);
        int InsertTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAddressTO tblAddressTO, SqlCommand cmdInsert);
        int UpdateTblAddress(TblAddressTO tblAddressTO);
        int UpdateTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAddressTO tblAddressTO, SqlCommand cmdUpdate);
        int DeleteTblAddress(Int32 idAddr);
        int DeleteTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idAddr, SqlCommand cmdDelete);
        //Priyanka [19-08-2019]
        TblAddressTO SelectTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran);

        List<TblAddressTO> SelectDefaultOrgAddressList(Int32 orgId);

        //Added By Gokul
        List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId);
        List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId);
        int UpdateTblAddressType(int idOrgAddr);
    }
}
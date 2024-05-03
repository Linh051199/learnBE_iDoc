using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;


namespace Demo.Lab.Errors
{
	public partial class ErrDemoLab
	{
		#region // Common:
		// Common:
		public const string NoError = Error.NoError; //// Thực hiện thành công.
		public const string ErrDemoLabPrefix = "ErrDemo.Lab."; //// Lỗi Demo.Lab.
		public const string CmSys_ServiceInit = "ErrDemo.Lab.CmSys_ServiceInit"; //// Lỗi Hệ thống, khi khởi tạo SystemService.
		public const string CmSys_InvalidTid = "ErrDemo.Lab.CmSys_InvalidTid"; //// Lỗi Hệ thống, Mã giao dịch không hợp lệ.
		public const string CmSys_GatewayAuthenticateFailed = "ErrDemo.Lab.CmSys_GatewayAuthenticateFailed"; //// Lỗi Hệ thống, khi truy nhập trái phép vào Cổng giao dịch.
		public const string CmSys_SessionPreInitFailed = "ErrDemo.Lab.CmSys_SessionPreInitFailed"; //// Lỗi Hệ thống, khi khởi tạo Phiên làm việc.
		public const string CmSys_SessionNotFound = "ErrDemo.Lab.CmSys_SessionNotFound"; //// Lỗi Hệ thống, Phiên làm việc không hợp lệ.
		public const string CmSys_SessionExpired = "ErrDemo.Lab.CmSys_SessionExpired"; //// Lỗi Hệ thống, Phiên làm việc đã hết hạn.
		public const string CmSys_InvalidServiceCode = "ErrDemo.Lab.CmSys_InvalidServiceCode"; //// //CmSys_InvalidServiceCode
		public const string CmSys_InvalidBizSpecialPw = "ErrDemo.Lab.CmSys_InvalidBizSpecialPw"; //// //CmSys_InvalidBizSpecialPw

		// CmApp_Mst_Common:
		public const string CmApp_Mst_Common_TableNotSupported = "ErrDemo.Lab.CmApp_Mst_Common_TableNotSupported"; //// //CmApp_Mst_Common_TableNotSupported

		#endregion

		#region // Ins_Claim:
		// Ins_Claim_CheckDB:
		public const string Ins_Claim_CheckDB_ClmNoNotFound = "ErrDemo.Lab.Ins_Claim_CheckDB_ClmNoNotFound"; //// //Ins_Claim_CheckDB_ClmNoNotFound
		public const string Ins_Claim_CheckDB_ClmNoExist = "ErrDemo.Lab.Ins_Claim_CheckDB_ClmNoExist"; //// //Ins_Claim_CheckDB_ClmNoExist
		public const string Ins_Claim_CheckDB_ClmStatusNotMatched = "ErrDemo.Lab.Ins_Claim_CheckDB_ClmStatusNotMatched"; //// //Ins_Claim_CheckDB_ClmStatusNotMatched
		public const string Ins_Claim_CheckDB_InvalidDLCode = "ErrDemo.Lab.Ins_Claim_CheckDB_InvalidDLCode"; //// //Ins_Claim_CheckDB_InvalidDLCode
		public const string Ins_Claim_CheckDB_InvalidUserCode = "ErrDemo.Lab.Ins_Claim_CheckDB_InvalidUserCode"; //// //Ins_Claim_CheckDB_InvalidUserCode

		// Ins_Claim_CheckAbility:
		public const string Ins_Claim_CheckAbility_TableInfoBlank = "ErrDemo.Lab.Ins_Claim_CheckAbility_TableInfoBlank"; //// //Ins_Claim_CheckAbility_TableInfoBlank
		public const string Ins_Claim_CheckAbility_Deny = "ErrDemo.Lab.Ins_Claim_CheckAbility_Deny"; //// //Ins_Claim_CheckAbility_Deny

		// Ins_Claim_GetX:
		public const string Ins_Claim_GetX = "ErrDemo.Lab.Ins_Claim_GetX"; //// //Ins_Claim_GetX

		// Ins_Claim_Add:
		public const string Ins_Claim_Add = "ErrDemo.Lab.Ins_Claim_Add"; //// //Ins_Claim_Add
		public const string Ins_Claim_Add_InvalidClmNo = "ErrDemo.Lab.Ins_Claim_Add_InvalidClmNo"; //// //Ins_Claim_Add_InvalidClmNo
		public const string Ins_Claim_Add_InputTblDtlNotFound = "ErrDemo.Lab.Ins_Claim_Add_InputTblDtlNotFound"; //// //Ins_Claim_Add_InputTblDtlNotFound

		// Ins_Claim_AddX:
		public const string Ins_Claim_AddX = "ErrDemo.Lab.Ins_Claim_AddX"; //// //Ins_Claim_AddX
		public const string Ins_Claim_AddX_AccessDeny = "ErrDemo.Lab.Ins_Claim_AddX_AccessDeny"; //// //Ins_Claim_AddX_AccessDeny
		public const string Ins_Claim_AddX_InvalidUserCode = "ErrDemo.Lab.Ins_Claim_AddX_InvalidUserCode"; //// //Ins_Claim_AddX_InvalidUserCode
		public const string Ins_Claim_AddX_InvalidClmNo = "ErrDemo.Lab.Ins_Claim_AddX_InvalidClmNo"; //// //Ins_Claim_AddX_InvalidClmNo
		public const string Ins_Claim_AddX_InputTblDtlNotFound = "ErrDemo.Lab.Ins_Claim_AddX_InputTblDtlNotFound"; //// //Ins_Claim_AddX_InputTblDtlNotFound

		// Ins_Claim_UpdX:
		public const string Ins_Claim_UpdX = "ErrDemo.Lab.Ins_Claim_UpdX"; //// //Ins_Claim_UpdX
		public const string Ins_Claim_UpdX_InvalidClmNo = "ErrDemo.Lab.Ins_Claim_UpdX_InvalidClmNo"; //// //Ins_Claim_UpdX_InvalidClmNo
		public const string Ins_Claim_UpdX_InputTblDtlNotFound = "ErrDemo.Lab.Ins_Claim_UpdX_InputTblDtlNotFound"; //// //Ins_Claim_UpdX_InputTblDtlNotFound

		// Ins_Claim_Approve:
		public const string Ins_Claim_Approve = "ErrDemo.Lab.Ins_Claim_Approve"; //// //Ins_Claim_Approve
		public const string Ins_Claim_Approve_InvalidClmNo = "ErrDemo.Lab.Ins_Claim_Approve_InvalidClmNo"; //// //Ins_Claim_Approve_InvalidClmNo

        // Ins_Claim_Cancel:
        public const string Ins_Claim_Cancel = "ErrDemo.Lab.Ins_Claim_Cancel"; //// //Ins_Claim_Cancel
        public const string Ins_Claim_Cancel_InvalidClmNo = "ErrDemo.Lab.Ins_Claim_Cancel_InvalidClmNo"; //// //Ins_Claim_Cancel_InvalidClmNo


		// Ins_Claim_DelX:
		public const string Ins_Claim_DelX = "ErrDemo.Lab.Ins_Claim_DelX"; //// //Ins_Claim_DelX
		public const string Ins_Claim_DelX_InvalidClmNo = "ErrDemo.Lab.Ins_Claim_DelX_InvalidClmNo"; //// //Ins_Claim_DelX_InvalidClmNo

		#endregion

		#region // Ins_ClaimDocTypeRemark:
		// Ins_ClaimDocTypeRemark_SaveX:
		public const string Ins_ClaimDocTypeRemark_SaveX = "ErrDemo.Lab.Ins_ClaimDocTypeRemark_SaveX"; //// //Ins_ClaimDocTypeRemark_SaveX
		public const string Ins_ClaimDocTypeRemark_SaveX_InvalidClmNo = "ErrDemo.Lab.Ins_ClaimDocTypeRemark_SaveX_InvalidClmNo"; //// //Ins_ClaimDocTypeRemark_SaveX_InvalidClmNo

		#endregion

		#region // Ins_ClaimDoc:
		// Ins_ClaimDoc_CheckDB:
		public const string Ins_ClaimDoc_CheckDB_ClmDocCodeNotFound = "ErrDemo.Lab.Ins_ClaimDoc_CheckDB_ClmDocCodeNotFound"; //// //Ins_ClaimDoc_CheckDB_ClmDocCodeNotFound
		public const string Ins_ClaimDoc_CheckDB_ClmDocCodeExist = "ErrDemo.Lab.Ins_ClaimDoc_CheckDB_ClmDocCodeExist"; //// //Ins_ClaimDoc_CheckDB_ClmDocCodeExist

		// Ins_ClaimDoc_Get
		public const string Ins_ClaimDoc_Get = "ErrDemo.Lab.Ins_ClaimDoc_Get"; //// //Ins_Contract_Get

		// Ins_ClaimDoc_SaveX:
		public const string Ins_ClaimDoc_SaveX = "ErrDemo.Lab.Ins_ClaimDoc_SaveX"; //// //Ins_ClaimDoc_SaveX
		public const string Ins_ClaimDoc_SaveX_InvalidClmDocCode = "ErrDemo.Lab.Ins_ClaimDoc_SaveX_InvalidClmDocCode"; //// //Ins_ClaimDoc_SaveX_InvalidClmDocCode
        public const string Ins_ClaimDoc_SaveX_InvalidClmNo = "ErrDemo.Lab.Ins_ClaimDoc_SaveX_InvalidClmNo"; //// //Ins_ClaimDoc_SaveX_InvalidClmNo
        
		
		#endregion

		#region // Ins_Contract:
		// Ins_Contract_CheckDB:
		public const string Ins_Contract_CheckDB_ContractNoNotFound = "ErrDemo.Lab.Ins_Contract_CheckDB_ContractNoNotFound"; //// //Ins_Contract_CheckDB_ContractNoNotFound
		public const string Ins_Contract_CheckDB_ContractNoExist = "ErrDemo.Lab.Ins_Contract_CheckDB_ContractNoExist"; //// //Ins_Contract_CheckDB_ContractNoExist
		public const string Ins_Contract_CheckDB_FlagActiveNotMatched = "ErrDemo.Lab.Ins_Contract_CheckDB_FlagActiveNotMatched"; //// //Ins_Contract_CheckDB_FlagActiveNotMatched

		// Ins_Contract_Get:
		public const string Ins_Contract_Get = "ErrDemo.Lab.Ins_Contract_Get"; //// //Ins_Contract_Get

        //Ins_Contract_AddMulti
        public const string Ins_Contract_AddMulti = "ErrDemo.Lab.Ins_Contract_AddMulti"; //// //Ins_Contract_AddMulti
        public const string Ins_Contract_AddMulti_InputTblDtlNotFound = "ErrDemo.Lab.Ins_Contract_AddMulti_InputTblDtlNotFound"; //// //Ins_Contract_AddMulti_InputTblDtlNotFound

        //Ins_Contract_Upd
        public const string Ins_Contract_Upd = "ErrDemo.Lab.Ins_Contract_Upd"; //// //Ins_Contract_Upd
        public const string Ins_Contract_Upd_InvalidConstractNo = "ErrDemo.Lab.Ins_Contract_Upd_InvalidConstractNo"; //// //Ins_Contract_Upd_InvalidConstractNo
        public const string Ins_Contract_Upd_InvalidobjDLCode = "ErrDemo.Lab.Ins_Contract_Upd_InvalidobjDLCode"; //// //Ins_Contract_Upd_InvalidobjDLCode
        public const string Ins_Contract_Upd_InvalidobjCtmCode = "ErrDemo.Lab.Ins_Contract_Upd_InvalidobjCtmCode"; //// //Ins_Contract_Upd_InvalidobjCtmCode
        public const string Ins_Contract_Upd_InvalidobjFlagActive = "ErrDemo.Lab.Ins_Contract_Upd_InvalidobjFlagActive"; //// //Ins_Contract_Upd_InvalidobjFlagActive
		#endregion

		#region // MasterData:
		// Mst_Common_Get:
		public const string Mst_Common_Get = "ErrDemo.Lab.Mst_Common_Get"; //// //Mst_Common_Get
		public const string Mst_Common_Get_NotSupportTable = "ErrDemo.Lab.Mst_Common_Get_NotSupportTable"; //// //Mst_Common_Get_NotSupportTable

		// Cm_ExecSql:
		public const string Cm_ExecSql = "ErrDemo.Lab.Cm_ExecSql"; //// //Cm_ExecSql
		public const string Cm_ExecSql_ParamMissing = "ErrDemo.Lab.Cm_ExecSql_ParamMissing"; //// //Cm_ExecSql_ParamMissing

		#endregion

        #region // Mst_AreaMarket:
        // Mst_AreaMarket_CheckDB:
        public const string Mst_AreaMarket_CheckDB_AreaMarketNotFound = "ErrDemoLab.Mst_AreaMarket_CheckDB_AreaMarketNotFound"; //// Không tìm thấy Vùng thị trường trong cơ sở dữ liệu ///
        public const string Mst_AreaMarket_CheckDB_AreaMarketExist = "ErrDemoLab.Mst_AreaMarket_CheckDB_AreaMarketExist"; //// Vùng thị trường đã tồn tại ////
        public const string Mst_AreaMarket_CheckDB_AreaMarketStatusNotMatched = "ErrDemoLab.Mst_AreaMarket_CheckDB_AreaMarketStatusNotMatched"; //// Trạng thái Vùng thị trường không hợp lệ ////
        public const string Mst_AreaMarket_CheckDB_FlagRootNotMatched = "ErrDemoLab.Mst_AreaMarket_CheckDB_FlagRootNotMatched"; //// //Mst_AreaMarket_CheckDB_FlagRootNotMatched

        // Mst_AreaMarket_Get:
        public const string Mst_AreaMarket_Get = "ErrDemoLab.Mst_AreaMarket_Get"; //// Mã lỗi: Mst_AreaMarket_Get ////

        // Mst_AreaMarket_Create:
        public const string Mst_AreaMarket_Create = "ErrDemoLab.Mst_AreaMarket_Create"; //// Mã lỗi: Mst_AreaMarket_Create ////
        public const string Mst_AreaMarket_Create_InvalidAreaCode = "ErrDemoLab.Mst_AreaMarket_Create_InvalidAreaCode"; //// Mã Vùng thị trường không hợp lệ ////
        public const string Mst_AreaMarket_Create_InvalidAreaCodeParent = "ErrDemoLab.Mst_AreaMarket_Create_InvalidAreaCodeParent"; //// Mã Vùng thị trường cấp trên không hợp lệ ////
        public const string Mst_AreaMarket_Create_InvalidAreaBUCode = "ErrDemoLab.Mst_AreaMarket_Create_InvalidAreaBUCode"; //// Mã lỗi: Mst_AreaMarket_Create_InvalidAreaBUCode ////
        public const string Mst_AreaMarket_Create_InvalidAreaBUPattern = "ErrDemoLab.Mst_AreaMarket_Create_InvalidAreaBUPattern"; //// Mã lỗi: Mst_AreaMarket_Create_InvalidAreaBUPattern ////
        public const string Mst_AreaMarket_Create_InvalidAreaLevel = "ErrDemoLab.Mst_AreaMarket_Create_InvalidAreaLevel"; //// Cấp độ Vùng thị trường không hợp lệ ////
        public const string Mst_AreaMarket_Create_InvalidAreaDesc = "ErrDemoLab.Mst_AreaMarket_Create_InvalidAreaDesc"; //// Mã lỗi: Mst_AreaMarket_Create_InvalidAreaDesc ////
        public const string Mst_AreaMarket_Create_InvalidAreaStatus = "ErrDemoLab.Mst_AreaMarket_Create_InvalidAreaStatus"; ////  Trạng thái Vùng thị trường không hợp lệ ////

        // Mst_AreaMarket_Update:
        public const string Mst_AreaMarket_Update = "ErrDemoLab.Mst_AreaMarket_Update"; //// Mã lỗi: Mst_AreaMarket_Update ////
        public const string Mst_AreaMarket_Update_InvalidAreaCode = "ErrDemoLab.Mst_AreaMarket_Update_InvalidAreaCode"; //// Mã Vùng thị trường không hợp lệ ////
        public const string Mst_AreaMarket_Update_InvalidAreaCodeParent = "ErrDemoLab.Mst_AreaMarket_Update_InvalidAreaCodeParent"; //// Mã Nhà phân phối cấp trên không hợp lệ ////
        public const string Mst_AreaMarket_Update_InvalidAreaBUCode = "ErrDemoLab.Mst_AreaMarket_Update_InvalidAreaBUCode"; //// Mã lỗi: Mst_AreaMarket_Update_InvalidAreaBUCode ////
        public const string Mst_AreaMarket_Update_InvalidAreaBUPattern = "ErrDemoLab.Mst_AreaMarket_Update_InvalidAreaBUPattern"; //// Mã lỗi: Mst_AreaMarket_Update_InvalidAreaBUPattern ////
        public const string Mst_AreaMarket_Update_InvalidAreaLevel = "ErrDemoLab.Mst_AreaMarket_Update_InvalidAreaLevel"; //// Cấp Vùng thị trường không hợp lệ ////
        public const string Mst_AreaMarket_Update_InvalidAreaDesc = "ErrDemoLab.Mst_AreaMarket_Update_InvalidAreaDesc"; //// Mã lỗi: Mst_AreaMarket_Create_InvalidAreaDesc ////
        public const string Mst_AreaMarket_Update_InvalidAreaStatus = "ErrDemoLab.Mst_AreaMarket_Update_InvalidAreaStatus"; ////  Trạng thái Vùng thị trường không hợp lệ ////
		public const string Mst_AreaMarket_Update_ExistAreaMarketChildActive = "ErrDemoLab.Mst_AreaMarket_Update_ExistAreaMarketChildActive"; //// // Mst_AreaMarket_Update_ExistAreaMarketChildActive 
		public const string Mst_AreaMarket_Update_ExistDistributorActive = "ErrDemoLab.Mst_AreaMarket_Update_ExistDistributorActive"; //// // Mst_AreaMarket_Update_ExistDistributorActive 
		public const string Mst_AreaMarket_Update_ExistOutLetActive = "ErrDemoLab.Mst_AreaMarket_Update_ExistOutLetActive"; //// // Mst_AreaMarket_Update_ExistOutLetActive 

        // Mst_AreaMarket_Delete:
        public const string Mst_AreaMarket_Delete = "ErrDemoLab.Mst_AreaMarket_Delete"; //// Mã lỗi: Mst_AreaMarket_Delete ////

        #endregion

        #region // Mst_Distributor:
        // Mst_Distributor_CheckDB:
        public const string Mst_Distributor_CheckDB_DBCodeNotFound = "ErrDemoLab.Mst_Distributor_CheckDB_DBCodeNotFound"; //// Không tìm thấy Mã nhà phân phối trong cơ sở dữ liệu ////
        public const string Mst_Distributor_CheckDB_DBCodeExist = "ErrDemoLab.Mst_Distributor_CheckDB_DBCodeExist"; //// Mã nhà phân phối đã tồn tại ////
        public const string Mst_Distributor_CheckDB_DBStatusNotMatched = "ErrDemoLab.Mst_Distributor_CheckDB_DBStatusNotMatched"; ////  Trạng thái Nhà phân phối không hợp lệ ////

        // Mst_Distributor_Get:
        public const string Mst_Distributor_Get = "ErrDemoLab.Mst_Distributor_Get"; //// Mã lỗi: Mst_Distributor_Get ////

        // Mst_Distributor_Create:
        public const string Mst_Distributor_Create = "ErrDemoLab.Mst_Distributor_Create"; //// Mã lỗi: Mst_Distributor_Create ////
        public const string Mst_Distributor_Create_InvalidDBCode = "ErrDemoLab.Mst_Distributor_Create_InvalidDBCode"; //// Mã Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBCodeParent = "ErrDemoLab.Mst_Distributor_Create_InvalidDBCodeParent"; //// Mã Nhà phân phối cấp trên không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBBUCode = "ErrDemoLab.Mst_Distributor_Create_InvalidDBBUCode"; //// Mã lỗi: lMst_Distributor_Create_InvalidDBBUCode ////
        public const string Mst_Distributor_Create_InvalidDBBUPattern = "ErrDemoLab.Mst_Distributor_Create_InvalidDBBUPattern"; //// Mã lỗi: Mst_Distributor_Create_InvalidDBBUPattern ////
        public const string Mst_Distributor_Create_InvalidDBLevel = "ErrDemoLab.Mst_Distributor_Create_InvalidDBLevel"; //// Cấp Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBName = "ErrDemoLab.Mst_Distributor_Create_InvalidDBName"; //// Tên Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidAreaCode = "ErrDemoLab.Mst_Distributor_Create_InvalidAreaCode"; //// Mã Vùng thị trương không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBAddress = "ErrDemoLab.Mst_Distributor_Create_InvalidDBAddress"; //// Địa chỉ Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBContactName = "ErrDemoLab.Mst_Distributor_Create_InvalidDBContactName"; //// Tên người liên lạc của Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBPhoneNo = "ErrDemoLab.Mst_Distributor_Create_InvalidDBPhoneNo"; //// Số điện thoại của Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBFaxNo = "ErrDemoLab.Mst_Distributor_Create_InvalidDBFaxNo"; //// Số fax của Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBMobilePhoneNo = "ErrDemoLab.Mst_Distributor_Create_InvalidDBMobilePhoneNo"; //// Số điện thoại di động của Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBSMSPhoneNo = "ErrDemoLab.Mst_Distributor_Create_InvalidDBSMSPhoneNo"; //// Số điện thoại di động để nhận tin nhắn SMS của Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBTaxCode = "ErrDemoLab.Mst_Distributor_Create_InvalidDBTaxCode"; //// Mã số thuế của Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidRemark = "ErrDemoLab.Mst_Distributor_Create_InvalidRemark"; //// Ghi chú không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidDBStatus = "ErrDemoLab.Mst_Distributor_Create_InvalidDBStatus"; //// Trạng thái Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidCreateDate = "ErrDemoLab.Mst_Distributor_Create_InvalidCreateDate"; //// Ngày tạo không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidCreateBy = "ErrDemoLab.Mst_Distributor_Create_InvalidCreateBy"; //// Mã lỗi: Mst_Distributor_Create_InvalidCreateBy ////
        public const string Mst_Distributor_Create_InvalidCancelDate = "ErrDemoLab.Mst_Distributor_Create_InvalidCancelDate"; //// Ngày hủy không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidCancelBy = "ErrDemoLab.Mst_Distributor_Create_InvalidCancelBy"; //// Mã lỗi: Mst_Distributor_Create_InvalidCancelBy ////
        public const string Mst_Distributor_Create_InvalidDiscountPercent = "ErrDemoLab.Mst_Distributor_Create_InvalidDiscountPercent"; //// Tỉ lệ chiết khấu cho Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Create_InvalidBalance = "ErrDemoLab.Mst_Distributor_Create_InvalidBalance"; //// Mã lỗi: Mst_Distributor_Create_InvalidBalance ////
        public const string Mst_Distributor_Create_InvalidOverdraftThreshold = "ErrDemoLab.Mst_Distributor_Create_InvalidOverdraftThreshold"; //// Mã lỗi: Mst_Distributor_Create_InvalidOverdraftThreshold ////


        // Mst_Distributor_Update:
        public const string Mst_Distributor_Update = "ErrDemoLab.Mst_Distributor_Update"; //// Mã lỗi: Mst_Distributor_Update ////
        public const string Mst_Distributor_Update_InvalidDBCode = "ErrDemoLab.Mst_Distributor_Update_InvalidDBCode"; //// Mã Nhà phân phối không hợp lệ ////
        public const string Mst_Distributor_Update_InvalidDBCodeParent = "ErrDemoLab.Mst_Distributor_Update_InvalidDBCodeParent"; //// Mã Nhà phân phối cấp trên không hợp lệ ////
        public const string Mst_Distributor_Update_InvalidDBBUCode = "ErrDemoLab.Mst_Distributor_Update_InvalidDBBUCode"; //// Mã lỗi: Mst_Distributor_Update_InvalidDBBUCode ////
        public const string Mst_Distributor_Update_InvalidDBBUPattern = "ErrDemoLab.Mst_Distributor_Update_InvalidDBBUPattern"; //// Mã lỗi: Mst_Distributor_Update_InvalidDBBUPattern ////
        public const string Mst_Distributor_Update_InvalidDBLevel = "ErrDemoLab.Mst_Distributor_Update_InvalidDBLevel"; //// Cấp Nhà phân phối không hợp lệ //// 
        public const string Mst_Distributor_Update_InvalidDBName = "ErrDemoLab.Mst_Distributor_Update_InvalidDBName"; //// Tên Nhà phối phân phối không hợp lệ ////
        public const string Mst_Distributor_Update_InvalidAreaCode = "ErrDemoLab.Mst_Distributor_Update_InvalidAreaCode"; //// //Mst_Distributor_Update_InvalidAreaCode
        public const string Mst_Distributor_Update_InvalidDBAddress = "ErrDemoLab.Mst_Distributor_Update_InvalidDBAddress"; //// //Mst_Distributor_Update_InvalidDBAddress
        public const string Mst_Distributor_Update_InvalidDBContactName = "ErrDemoLab.Mst_Distributor_Update_InvalidDBContactName"; //// //Mst_Distributor_Update_InvalidDBContactName
        public const string Mst_Distributor_Update_InvalidDBPhoneNo = "ErrDemoLab.Mst_Distributor_Update_InvalidDBPhoneNo"; //// //Mst_Distributor_Update_InvalidDBPhoneNo
        public const string Mst_Distributor_Update_InvalidDBFaxNo = "ErrDemoLab.Mst_Distributor_Update_InvalidDBFaxNo"; //// //Mst_Distributor_Update_InvalidDBFaxNo
        public const string Mst_Distributor_Update_InvalidDBMobilePhoneNo = "ErrDemoLab.Mst_Distributor_Update_InvalidDBMobilePhoneNo"; //// //Mst_Distributor_Update_InvalidDBMobilePhoneNo
        public const string Mst_Distributor_Update_InvalidDBSMSPhoneNo = "ErrDemoLab.Mst_Distributor_Update_InvalidDBSMSPhoneNo"; //// //Mst_Distributor_Update_InvalidDBSMSPhoneNo
        public const string Mst_Distributor_Update_InvalidDBTaxCode = "ErrDemoLab.Mst_Distributor_Update_InvalidDBTaxCode"; //// //Mst_Distributor_Update_InvalidDBTaxCode
        public const string Mst_Distributor_Update_InvalidRemark = "ErrDemoLab.Mst_Distributor_Update_InvalidRemark"; //// //Mst_Distributor_Update_InvalidRemark
        public const string Mst_Distributor_Update_InvalidDBStatus = "ErrDemoLab.Mst_Distributor_Update_InvalidDBStatus"; //// //Mst_Distributor_Update_InvalidDBStatus
        public const string Mst_Distributor_Update_InvalidCreateDate = "ErrDemoLab.Mst_Distributor_Update_InvalidCreateDate"; //// //Mst_Distributor_Update_InvalidCreateDate
        public const string Mst_Distributor_Update_InvalidCreateBy = "ErrDemoLab.Mst_Distributor_Update_InvalidCreateBy"; //// //Mst_Distributor_Update_InvalidCreateBy
        public const string Mst_Distributor_Update_InvalidCancelDate = "ErrDemoLab.Mst_Distributor_Update_InvalidCancelDate"; //// //Mst_Distributor_Update_InvalidCancelDate
        public const string Mst_Distributor_Update_InvalidCancelBy = "ErrDemoLab.Mst_Distributor_Update_InvalidCancelBy"; //// //Mst_Distributor_Update_InvalidCancelBy
        public const string Mst_Distributor_Update_InvalidDiscountPercent = "ErrDemoLab.Mst_Distributor_Update_InvalidDiscountPercent"; //// //Mst_Distributor_Update_InvalidDiscountPercent
        public const string Mst_Distributor_Update_InvalidBalance = "ErrDemoLab.Mst_Distributor_Update_InvalidBalance"; //// //Mst_Distributor_Update_InvalidBalance
        public const string Mst_Distributor_Update_InvalidOverdraftThreshold = "ErrDemoLab.Mst_Distributor_Update_InvalidOverdraftThreshold"; //// //Mst_Distributor_Update_InvalidOverdraftThreshold
		public const string Mst_Distributor_Update_ExistOutletActive = "ErrDemoLab.Mst_Distributor_Update_ExistOutletActive"; //// // Mst_Distributor_Update_ExistOutletActive

        // Mst_Distributor_Delete:
        public const string Mst_Distributor_Delete = "ErrDemoLab.Mst_Distributor_Delete"; //// //Mst_Distributor_Delete

        #endregion

        #region // Mst_Outlet:
        // Mst_Outlet_CheckDB:
        public const string Mst_Outlet_CheckDB_OutletNotFound = "ErrDemoLab.Mst_Outlet_CheckDB_OutletNotFound"; //// //Mst_Outlet_CheckDB_OutletNotFound
        public const string Mst_Outlet_CheckDB_OutletExist = "ErrDemoLab.Mst_Outlet_CheckDB_OutletExist"; //// //Mst_Outlet_CheckDB_OutletExist
        public const string Mst_Outlet_CheckDB_OutletStatusNotMatched = "ErrDemoLab.Mst_Outlet_CheckDB_OutletStatusNotMatched"; //// //Mst_Outlet_CheckDB_OutletStatusNotMatched

        // Mst_Outlet_Get:
        public const string Mst_Outlet_Get = "ErrDemoLab.Mst_Outlet_Get"; //// //Mst_Outlet_Get

        // Mst_Outlet_GetByRouting:
        public const string Mst_Outlet_GetByRouting = "ErrDemoLab.Mst_Outlet_GetByRouting"; //// //Mst_Outlet_GetByRouting

        // Mst_Outlet_Create:
        public const string Mst_Outlet_Create = "ErrDemoLab.Mst_Outlet_Create"; //// //Mst_Outlet_Create
        public const string Mst_Outlet_Create_InvalidOLCode = "ErrDemoLab.Mst_Outlet_Create_InvalidOLCode"; //// //Mst_Outlet_Create_InvalidOLCode
        public const string Mst_Outlet_Create_InvalidDBCode = "ErrDemoLab.Mst_Outlet_Create_InvalidDBCode"; //// //Mst_Outlet_Create_InvalidDBCode
        public const string Mst_Outlet_Create_InvalidOLName = "ErrDemoLab.Mst_Outlet_Create_InvalidOLName"; //// //Mst_Outlet_Create_InvalidOLName
        public const string Mst_Outlet_Create_InvalidOLAddress = "ErrDemoLab.Mst_Outlet_Create_InvalidOLAddress"; //// //Mst_Outlet_Create_InvalidOLAddress
        public const string Mst_Outlet_Create_InvalidOLContactName = "ErrDemoLab.Mst_Outlet_Create_InvalidOLContactName"; //// //Mst_Outlet_Create_InvalidOLContactName
        public const string Mst_Outlet_Create_InvalidOLPhoneNo = "ErrDemoLab.Mst_Outlet_Create_InvalidOLPhoneNo"; //// //Mst_Outlet_Create_InvalidOLPhoneNo
        public const string Mst_Outlet_Create_InvalidOLFaxNo = "ErrDemoLab.Mst_Outlet_Create_InvalidOLFaxNo"; //// //Mst_Outlet_Create_InvalidOLFaxNo
        public const string Mst_Outlet_Create_InvalidOLMobilePhoneNo = "ErrDemoLab.Mst_Outlet_Create_InvalidOLMobilePhoneNo"; //// //Mst_Outlet_Create_InvalidOLMobilePhoneNo
        public const string Mst_Outlet_Create_InvalidOLSMSPhoneNo = "ErrDemoLab.Mst_Outlet_Create_InvalidOLSMSPhoneNo"; //// //Mst_Outlet_Create_InvalidOLSMSPhoneNo
        public const string Mst_Outlet_Create_InvalidOLTaxCode = "ErrDemoLab.Mst_Outlet_Create_InvalidOLTaxCode"; //// //Mst_Outlet_Create_InvalidOLTaxCode
        public const string Mst_Outlet_Create_InvalidRemark = "ErrDemoLab.Mst_Outlet_Create_InvalidRemark"; //// //Mst_Outlet_Create_InvalidRemark
        public const string Mst_Outlet_Create_InvalidOLStatus = "ErrDemoLab.Mst_Outlet_Create_InvalidOLStatus"; //// //Mst_Outlet_Create_InvalidOLStatus
        public const string Mst_Outlet_Create_InvalidCreateDate = "ErrDemoLab.Mst_Outlet_Create_InvalidCreateDate"; //// //Mst_Outlet_Create_InvalidCreateDate
        public const string Mst_Outlet_Create_InvalidCreateBy = "ErrDemoLab.Mst_Outlet_Create_InvalidCreateBy"; //// //Mst_Outlet_Create_InvalidCreateBy
        public const string Mst_Outlet_Create_InvalidApproveDate = "ErrDemoLab.Mst_Outlet_Create_InvalidApproveDate"; //// //Mst_Outlet_Create_InvalidApproveDate
        public const string Mst_Outlet_Create_InvalidApproveBy = "ErrDemoLab.Mst_Outlet_Create_InvalidApproveBy"; //// //Mst_Outlet_Create_InvalidApproveBy
        public const string Mst_Outlet_Create_InvalidCancelDate = "ErrDemoLab.Mst_Outlet_Create_InvalidCancelDate"; //// //Mst_Outlet_Create_InvalidCancelDate
        public const string Mst_Outlet_Create_InvalidCancelBy = "ErrDemoLab.Mst_Outlet_Create_InvalidCancelBy"; //// //Mst_Outlet_Create_InvalidCancelBy
        public const string Mst_Outlet_Create_UserNotBeDBAdmin = "ErrDemoLab.Mst_Outlet_Create_UserNotBeDBAdmin"; //// //Mst_Outlet_Create_UserNotBeDBAdmin

        public const string Mst_Outlet_Create_InvalidUserSaleMan = "ErrDemoLab.Mst_Outlet_Create_InvalidUserSaleMan"; //// //Mst_Outlet_Create_InvalidUserSaleMan

        // Mst_Outlet_Update:
        public const string Mst_Outlet_Update = "ErrDemoLab.Mst_Outlet_Update"; //// //Mst_Outlet_Update
        public const string Mst_Outlet_Update_InvalidOLCode = "ErrDemoLab.Mst_Outlet_Update_InvalidOLCode"; //// //Mst_Outlet_Update_InvalidOLCode
        public const string Mst_Outlet_Update_InvalidDBCode = "ErrDemoLab.Mst_Outlet_Update_InvalidDBCode"; //// //Mst_Outlet_Update_InvalidDBCode
        public const string Mst_Outlet_Update_InvalidOLName = "ErrDemoLab.Mst_Outlet_Update_InvalidOLName"; //// //Mst_Outlet_Update_InvalidOLName
        public const string Mst_Outlet_Update_InvalidOLAddress = "ErrDemoLab.Mst_Outlet_Update_InvalidOLAddress"; //// //Mst_Outlet_Update_InvalidOLAddress
        public const string Mst_Outlet_Update_InvalidOLContactName = "ErrDemoLab.Mst_Outlet_Update_InvalidOLContactName"; //// //Mst_Outlet_Update_InvalidOLContactName
        public const string Mst_Outlet_Update_InvalidOLPhoneNo = "ErrDemoLab.Mst_Outlet_Update_InvalidOLPhoneNo"; //// //Mst_Outlet_Update_InvalidOLPhoneNo
        public const string Mst_Outlet_Update_InvalidOLFaxNo = "ErrDemoLab.Mst_Outlet_Update_InvalidOLFaxNo"; //// //Mst_Outlet_Update_InvalidOLFaxNo
        public const string Mst_Outlet_Update_InvalidOLMobilePhoneNo = "ErrDemoLab.Mst_Outlet_Update_InvalidOLMobilePhoneNo"; //// //Mst_Outlet_Update_InvalidOLMobilePhoneNo
        public const string Mst_Outlet_Update_InvalidOLSMSPhoneNo = "ErrDemoLab.Mst_Outlet_Update_InvalidOLSMSPhoneNo"; //// //Mst_Outlet_Update_InvalidOLSMSPhoneNo
        public const string Mst_Outlet_Update_InvalidOLTaxCode = "ErrDemoLab.Mst_Outlet_Update_InvalidOLTaxCode"; //// //Mst_Outlet_Update_InvalidOLTaxCode
        public const string Mst_Outlet_Update_InvalidRemark = "ErrDemoLab.Mst_Outlet_Update_InvalidRemark"; //// //Mst_Outlet_Update_InvalidRemark
        public const string Mst_Outlet_Update_InvalidOLStatus = "ErrDemoLab.Mst_Outlet_Update_InvalidOLStatus"; //// //Mst_Outlet_Update_InvalidOLStatus
        public const string Mst_Outlet_Update_InvalidCreateDate = "ErrDemoLab.Mst_Outlet_Update_InvalidCreateDate"; //// //Mst_Outlet_Update_InvalidCreateDate
        public const string Mst_Outlet_Update_InvalidCreateBy = "ErrDemoLab.Mst_Outlet_Update_InvalidCreateBy"; //// //Mst_Outlet_Update_InvalidCreateBy
        public const string Mst_Outlet_Update_InvalidApproveDate = "ErrDemoLab.Mst_Outlet_Update_InvalidApproveDate"; //// //Mst_Outlet_Update_InvalidApproveDate
        public const string Mst_Outlet_Update_InvalidApproveBy = "ErrDemoLab.Mst_Outlet_Update_InvalidApproveBy"; //// //Mst_Outlet_Update_InvalidApproveBy
        public const string Mst_Outlet_Update_InvalidCancelDate = "ErrDemoLab.Mst_Outlet_Update_InvalidCancelDate"; //// //Mst_Outlet_Update_InvalidCancelDate
        public const string Mst_Outlet_Update_InvalidCancelBy = "ErrDemoLab.Mst_Outlet_Update_InvalidCancelBy"; //// //Mst_Outlet_Update_InvalidCancelBy

        // Mst_Outlet_Delete:
        public const string Mst_Outlet_Delete = "ErrDemoLab.Mst_Outlet_Delete"; //// //Mst_Outlet_Delete

        #endregion

        #region // Mst_Province:
        // Mst_Province_CheckDB:
        public const string Mst_Province_CheckDB_ProvinceNotFound = "ErrDemoLab.Mst_Province_CheckDB_ProvinceNotFound"; //// //Mst_Province_CheckDB_ProvinceNotFound
        public const string Mst_Province_CheckDB_ProvinceExist = "ErrDemoLab.Mst_Province_CheckDB_ProvinceExist"; //// //Mst_Provincet_CheckDB_ProvinceExist
        public const string Mst_Province_CheckDB_FlagActiveNotMatched = "ErrDemoLab.Mst_Province_CheckDB_FlagActiveNotMatched"; //// //Mst_Province_CheckDB_FlagActiveNotMatched

        // Mst_Province_Get:
        public const string Mst_Province_Get = "ErrDemoLab.Mst_Province_Get"; //// //Mst_Province_Get

        // Mst_Province_Create:
        public const string Mst_Province_Create = "ErrDemoLab.Mst_Province_Create"; //// //Mst_Province_Create
        public const string Mst_Province_Create_InvalidProvinceCode = "ErrDemoLab.Mst_Province_Create_InvalidProvinceCode"; //// //Mst_Province_Create_InvalidProvinceCode
        public const string Mst_Province_Create_InvalidProvinceName = "ErrDemoLab.Mst_Province_Create_InvalidProvinceName"; //// //Mst_Province_Create_InvalidProvinceName        

        // Mst_Province_Update:
        public const string Mst_Province_Update = "ErrDemoLab.Mst_Province_Update"; //// //Mst_Province_Update                
        public const string Mst_Province_Update_InvalidProvinceName = "ErrDemoLab.Mst_Province_Update_InvalidProvinceName"; //// //Mst_Province_Update_InvalidProvinceName                       

        // Mst_Province_Delete:
        public const string Mst_Province_Delete = "ErrDemoLab.Mst_Province_Delete"; //// Mã lỗi: Mst_Province_Delete ////

        #endregion 

		#region // Mst_District:
		// Mst_District_CheckDB:
		public const string Mst_District_CheckDB_DistrictNotFound = "ErrDemoLab.Mst_District_CheckDB_DistrictNotFound"; //// //Mst_District_CheckDB_DistrictNotFound
        public const string Mst_District_CheckDB_DistrictExist = "ErrDemoLab.Mst_District_CheckDB_DistrictExist"; //// //Mst_District_CheckDB_DistrictExist
        public const string Mst_District_CheckDB_FlagActiveNotMatched = "ErrDemoLab.Mst_District_CheckDB_FlagActiveNotMatched"; //// //Mst_District_CheckDB_FlagActiveNotMatched

        // Mst_District_Get:
        public const string Mst_District_Get = "ErrDemoLab.Mst_District_Get"; //// //Mst_District_Get

        // Mst_District_Create:
        public const string Mst_District_Create = "ErrDemoLab.Mst_District_Create"; //// //Mst_District_Create
        public const string Mst_District_Create_InvalidDistrictCode = "ErrDemoLab.Mst_District_Create_InvalidDistrictCode"; //// //Mst_District_Create_InvalidDistrictCode
        public const string Mst_District_Create_InvalidDistrictName = "ErrDemoLab.Mst_District_Create_InvalidDistrictName"; //// //Mst_District_Create_InvalidDistrictName        

        // Mst_District_Update:
        public const string Mst_District_Update = "ErrDemoLab.Mst_District_Update"; //// //Mst_District_Update                
        public const string Mst_District_Update_InvalidDistrictName = "ErrDemoLab.Mst_District_Update_InvalidDistrictName"; //// //Mst_District_Update_InvalidDistrictName                       

        // Mst_District_Delete:
        public const string Mst_District_Delete = "ErrDemoLab.Mst_District_Delete"; //// //Mst_District_Delete                

        #endregion

        #region // Mst_ImageType:
        // Mst_ImageType_CheckDB:
        public const string Mst_ImageType_CheckDB_ImageTypeNotFound = "ErrDemoLab.Mst_ImageType_CheckDB_ImageTypeNotFound"; //// //Mst_ImageType_CheckDB_ImageTypeNotFound
        public const string Mst_ImageType_CheckDB_ImageTypeExist = "ErrDemoLab.Mst_ImageType_CheckDB_ImageTypeExist"; //// //Mst_ImageType_CheckDB_ImageTypeExist
        public const string Mst_ImageType_CheckDB_FlagActiveNotMatched = "ErrDemoLab.Mst_ImageType_CheckDB_FlagActiveNotMatched"; //// //Mst_ImageType_CheckDB_FlagActiveNotMatched

        // Mst_ImageType_Get:
        public const string Mst_ImageType_Get = "ErrDemoLab.Mst_ImageType_Get"; //// //Mst_ImageType_Get

        // Mst_ImageType_Create:
        public const string Mst_ImageType_Create = "ErrDemoLab.Mst_ImageType_Create"; //// //Mst_ImageType_Create
        public const string Mst_ImageType_Create_InvalidImageType = "ErrDemoLab.Mst_ImageType_Create_InvalidImageType"; //// //Mst_ImageType_Create_InvalidImageType
        public const string Mst_ImageType_Create_InvalidImageDesc = "ErrDemoLab.Mst_ImageType_Create_InvalidImageDesc"; //// //Mst_ImageType_Create_InvalidImageDesc        

        // Mst_ImageType_Update:
        public const string Mst_ImageType_Update = "ErrDemoLab.Mst_ImageType_Update"; //// //Mst_ImageType_Update                
        public const string Mst_ImageType_Update_InvalidImageDesc = "ErrDemoLab.Mst_ImageType_Update_InvalidImageDesc"; //// //Mst_ImageType_Update_InvalidImageDesc                       

        // Mst_ImageType_Delete:
        public const string Mst_ImageType_Delete = "ErrDemoLab.Mst_ImageType_Delete"; //// //Mst_ImageType_Delete                

        #endregion

        #region // Mst_ActionType:
        // Mst_ActionType_CheckDB:
        public const string Mst_ActionType_CheckDB_ActionTypeNotFound = "ErrDemoLab.Mst_ActionType_CheckDB_ActionTypeNotFound"; //// //Mst_ActionType_CheckDB_ActionTypeNotFound
        public const string Mst_ActionType_CheckDB_ActionTypeExist = "ErrDemoLab.Mst_ActionType_CheckDB_ActionTypeExist"; //// //Mst_ActionType_CheckDB_ActionTypeExist
        public const string Mst_ActionType_CheckDB_FlagActiveNotMatched = "ErrDemoLab.Mst_ActionType_CheckDB_FlagActiveNotMatched"; //// //Mst_ActionType_CheckDB_FlagActiveNotMatched

        // Mst_ActionType_Get:
        public const string Mst_ActionType_Get = "ErrDemoLab.Mst_ActionType_Get"; //// //Mst_ActionType_Get

        // Mst_ActionType_Create:
        public const string Mst_ActionType_Create = "ErrDemoLab.Mst_ActionType_Create"; //// //Mst_ActionType_Create        
        public const string Mst_ActionType_Create_InvalidActionType = "ErrDemoLab.Mst_ActionType_Create_InvalidActionType"; //// //Mst_ActionType_Create_InvalidActionType        
        public const string Mst_ActionType_Create_InvalidActionTypeDesc = "ErrDemoLab.Mst_ActionType_Create_InvalidActionTypeDesc"; //// //Mst_ActionType_Create_InvalidActionTypeDesc        
        public const string Mst_ActionType_Create_InvalidAvgScoreValStart = "ErrDemoLab.Mst_ActionType_Create_InvalidAvgScoreValStart"; //// //Mst_ActionType_Create_InvalidAvgScoreValStart        
        public const string Mst_ActionType_Create_InvalidAvgScoreValStartExists = "ErrDemoLab.Mst_ActionType_Create_InvalidAvgScoreValStartExists"; //// //Mst_ActionType_Create_InvalidAvgScoreValStartExists        

        // Mst_ActionType_Update:
        public const string Mst_ActionType_Update = "ErrDemoLab.Mst_ActionType_Update"; //// //Mst_ActionType_Update                        
        public const string Mst_ActionType_Update_InvalidActionTypeDesc = "ErrDemoLab.Mst_ActionType_Update_InvalidActionTypeDesc"; //// //Mst_ActionType_Update_InvalidActionTypeDesc         
        public const string Mst_ActionType_Update_InvalidAvgScoreValStart = "ErrDemoLab.Mst_ActionType_Update_InvalidAvgScoreValStart"; //// //Mst_ActionType_Update_InvalidAvgScoreValStart        
        public const string Mst_ActionType_Update_InvalidAvgScoreValStartExists = "ErrDemoLab.Mst_ActionType_Update_InvalidAvgScoreValStartExists"; //// //Mst_ActionType_Update_InvalidAvgScoreValStartExists                       

        // Mst_ActionType_Delete:
        public const string Mst_ActionType_Delete = "ErrDemoLab.Mst_ActionType_Delete"; //// //Mst_ActionType_Delete                

        #endregion

		#region // Mst_Channel:
		// Mst_Channel_CheckDB:
		public const string Mst_Channel_CheckDB_ChannelCodeNotFound = "ErrDemo.Lab.Mst_Channel_CheckDB_ChannelCodeNotFound"; //// //Mst_Channel_CheckDB_ChannelCodeNotFound
		public const string Mst_Channel_CheckDB_ChannelCodeExist = "ErrDemo.Lab.Mst_Channel_CheckDB_ChannelCodeExist"; //// //Mst_Channel_CheckDB_ChannelCodeExist
		public const string Mst_Channel_CheckDB_FlagActiveNotMatched = "ErrDemo.Lab.Mst_Channel_CheckDB_FlagActiveNotMatched"; //// //Mst_Channel_CheckDB_FlagActiveNotMatched

		// Mst_Channel_Get:
		public const string Mst_Channel_Get = "ErrDemo.Lab.Mst_Channel_Get"; //// //Mst_Channel_Get

		// Mst_Channel_Create:
		public const string Mst_Channel_Create = "ErrDemo.Lab.Mst_Channel_Create"; //// //Mst_Channel_Create
		public const string Mst_Channel_Create_InvalidChannelCode = "ErrDemo.Lab.Mst_Channel_Create_InvalidChannelCode"; //// //Mst_Channel_Create_InvalidChannelCode
		public const string Mst_Channel_Create_InvalidChannelName = "ErrDemo.Lab.Mst_Channel_Create_InvalidChannelName"; //// //Mst_Channel_Create_InvalidChannelName

		// Mst_Channel_Update:
		public const string Mst_Channel_Update = "ErrDemo.Lab.Mst_Channel_Update"; //// //Mst_Channel_Update
		public const string Mst_Channel_Update_InvalidChannelCode = "ErrDemo.Lab.Mst_Channel_Update_InvalidChannelCode"; //// //Mst_Channel_Update_InvalidChannelCode
		public const string Mst_Channel_Update_InvalidChannelName = "ErrDemo.Lab.Mst_Channel_Update_InvalidChannelName"; //// //Mst_Channel_Update_InvalidChannelName

		// Mst_Channel_Delete:
		public const string Mst_Channel_Delete = "ErrDemo.Lab.Mst_Channel_Delete"; //// //Mst_Channel_Delete
		#endregion

		#region // Mst_ChannelDealer:
		// Mst_ChannelDealer_Save:
		public const string Mst_ChannelDealer_Save = "ErrDemo.Lab.Mst_ChannelDealer_Save"; //// //Mst_ChannelDealer_Save
		public const string Mst_ChannelDealer_Save_InputTblDtlNotFound = "ErrDemo.Lab.Mst_ChannelDealer_Save_InputTblDtlNotFound"; //// //Mst_ChannelDealer_Save_InputTblDtlNotFound

		// Mst_ChannelDealer_Add:
		public const string Mst_ChannelDealer_Add = "ErrDemo.Lab.Mst_ChannelDealer_Add"; //// //Mst_ChannelDealer_Add

		// Mst_ChannelDealer_Del:
		public const string Mst_ChannelDealer_Del = "ErrDemo.Lab.Mst_ChannelDealer_Del"; //// //Mst_ChannelDealer_Del
		public const string Mst_ChannelDealer_Del_ChannelDealerNotFound = "ErrDemo.Lab.Mst_ChannelDealer_Del_ChannelDealerNotFound"; //// //Mst_ChannelDealer_Del_ChannelDealerNotFound
		#endregion

		#region // Mst_CalendarType:
		// Mst_CalendarType_CheckDB:
		public const string Mst_CalendarType_CheckDB_CalendarTypeNotFound = "ErrDemo.Lab.Mst_CalendarType_CheckDB_CalendarTypeNotFound"; //// //Mst_CalendarType_CheckDB_CalendarTypeNotFound
		public const string Mst_CalendarType_CheckDB_CalendarTypeExist = "ErrDemo.Lab.Mst_CalendarType_CheckDB_CalendarTypeExist"; //// //Mst_CalendarType_CheckDB_CalendarTypeExist
		public const string Mst_CalendarType_CheckDB_FlagActiveNotMatched = "ErrDemo.Lab.Mst_CalendarType_CheckDB_FlagActiveNotMatched"; //// //Mst_CalendarType_CheckDB_FlagActiveNotMatched
		#endregion

		#region // Mst_Calendar:
		// Mst_Calendar_CheckDB:
		public const string Mst_Calendar_CheckDB_DateNotFound = "ErrDemo.Lab.Mst_Calendar_CheckDB_DateNotFound"; //// //Mst_Calendar_CheckDB_DateNotFound
		public const string Mst_Calendar_CheckDB_DateExist = "ErrDemo.Lab.Mst_Calendar_CheckDB_DateExist"; //// //Mst_Calendar_CheckDB_DateExist
		public const string Mst_Calendar_CheckDB_DateStatusNotMatched = "ErrDemo.Lab.Mst_Calendar_CheckDB_DateStatusNotMatched"; //// //Mst_Calendar_CheckDB_DateStatusNotMatched

		// Mst_Calendar_Get:
		public const string Mst_Calendar_Get = "ErrDemo.Lab.Mst_Calendar_Get"; //// //Mst_Dealer_Get
		public const string Mst_Calendar_GetDayOfMonth = "ErrDemo.Lab.Mst_Calendar_GetDayOfMonth"; //// //Mst_Calendar_GetDayOfMonth

		// Mst_Calendar_ResetYear:
		public const string Mst_Calendar_ResetYear = "ErrDemo.Lab.Mst_Calendar_ResetYear"; //// //Mst_Calendar_ResetYear
		public const string Mst_Calendar_ResetYear_InvalidYear = "ErrDemo.Lab.Mst_Calendar_ResetYear_InvalidYear"; //// //Mst_Calendar_ResetYear_InvalidYear
		public const string Mst_Calendar_ResetYear_InvalidCalendarType = "ErrDemo.Lab.Mst_Calendar_ResetYear_InvalidCalendarType"; //// //Mst_Calendar_ResetYear_InvalidCalendarType

		// Mst_Calendar_UpdateDateStatus:
		public const string Mst_Calendar_UpdateDateStatus = "ErrDemo.Lab.Mst_Calendar_UpdateDateStatus"; //// //Mst_Calendar_UpdateDateStatus
		public const string Mst_Calendar_UpdateDateStatus_InvalidCalendarType = "ErrDemo.Lab.Mst_Calendar_UpdateDateStatus_InvalidCalendarType"; //// //Mst_Calendar_UpdateDateStatus_InvalidCalendarType
		public const string Mst_Calendar_UpdateDateStatus_InvalidDate = "ErrDemo.Lab.Mst_Calendar_UpdateDateStatus_InvalidDate"; //// //Mst_Calendar_UpdateDateStatus_InvalidDate
		#endregion

		#region // Wk_Task:
		// Wk_Task_CheckDB:
		public const string Wk_Task_CheckDB_TaskCodeNotFound = "ErrDemo.Lab.Wk_Task_CheckDB_TaskCodeNotFound"; //// //Wk_Task_CheckDB_TaskCodeNotFound
		public const string Wk_Task_CheckDB_TaskCodeExist = "ErrDemo.Lab.Wk_Task_CheckDB_TaskCodeExist"; //// //Wk_Task_CheckDB_TaskCodeExist
		public const string Wk_Task_CheckDB_TaskStatusNotMatched = "ErrDemo.Lab.Wk_Task_CheckDB_TaskStatusNotMatched"; //// //Wk_Task_CheckDB_TaskStatusNotMatched


		// Wk_Task_Get:
		public const string Wk_Task_Get = "ErrDemo.Lab.Wk_Task_Get"; //// //Wk_Task_Get

		// Wk_Task_Create:
		public const string Wk_Task_Create = "ErrDemo.Lab.Wk_Task_Create"; //// //Wk_Task_Create
		public const string Wk_Task_Create_InvalidTaskCode = "ErrDemo.Lab.Wk_Task_Create_InvalidTaskCode"; //// //Wk_Task_Create_InvalidTaskCode

		// Wk_Task_Update:
		public const string Wk_Task_Update = "ErrDemo.Lab.Wk_Task_Update"; //// //Wk_Task_Update
		public const string Wk_Task_Update_InvalidTaskCode = "ErrDemo.Lab.Wk_Task_Update_InvalidTaskCode"; //// //Wk_Task_Update_InvalidTaskCode


		// Wk_Task_Delete:
		public const string Wk_Task_Delete = "ErrDemo.Lab.Wk_Task_Delete"; //// //Wk_Task_Delete
		#endregion

		#region // Wk_UserSchedule:
		// Wk_UserSchedule_CheckDB:
		public const string Wk_UserSchedule_CheckDB_SchCodeNotFound = "ErrDemo.Lab.Wk_UserSchedule_CheckDB_SchCodeNotFound"; //// //Wk_UserSchedule_CheckDB_SchCodeNotFound
		public const string Wk_UserSchedule_CheckDB_SchCodeExist = "ErrDemo.Lab.Wk_UserSchedule_CheckDB_SchCodeExist"; //// //Wk_UserSchedule_CheckDB_SchCodeExist
		public const string Wk_UserSchedule_CheckDB_USStatusNotMatched = "ErrDemo.Lab.Wk_UserSchedule_CheckDB_USStatusNotMatched"; //// //Wk_UserSchedule_CheckDB_USStatusNotMatched


		// Wk_UserSchedule_Get:
		public const string Wk_UserSchedule_GetX = "ErrDemo.Lab.Wk_UserSchedule_GetX"; //// //Wk_UserSchedule_GetX

		// Wk_UserSchedule_AddX:
		public const string Wk_UserSchedule_AddX = "ErrDemo.Lab.Wk_UserSchedule_AddX"; //// //Wk_UserSchedule_AddX
		public const string Wk_UserSchedule_AddX_InvalidSchCode = "ErrDemo.Lab.Wk_Wk_UserSchedule_AddX_InvalidSchCode"; //// //Wk_Wk_UserSchedule_AddX_InvalidSchCode
		public const string Wk_UserSchedule_AddX_InvalidUserCode = "ErrDemo.Lab.Wk_Wk_UserSchedule_AddX_InvalidUserCode"; //// //Wk_Wk_UserSchedule_AddX_InvalidUserCode
		public const string Wk_UserSchedule_AddX_InvalidEffDTimeStart = "ErrDemo.Lab.Wk_UserSchedule_AddX_InvalidEffDTimeStart"; //// //Wk_UserSchedule_AddX_InvalidEffDTimeStart
		public const string Wk_UserSchedule_AddX_InvalidEffDTimeEnd = "ErrDemo.Lab.Wk_UserSchedule_AddX_InvalidEffDTimeEnd"; //// //Wk_UserSchedule_AddX_InvalidEffDTimeEnd
		public const string Wk_UserSchedule_AddX_InvalidEffDTimeStartBeforeEffDTimeEnd = "ErrDemo.Lab.Wk_UserSchedule_AddX_InvalidEffDTimeStartBeforeEffDTimeEnd"; //// //Wk_UserSchedule_AddX_InvalidEffDTimeStartBeforeEffDTimeEnd
		public const string Wk_UserSchedule_AddX_InvalidEffDTimeStartAfterEffSysDate = "ErrDemo.Lab.Wk_UserSchedule_AddX_InvalidEffDTimeStartAfterEffSysDate"; //// //Wk_UserSchedule_AddX_InvalidEffDTimeStartAfterEffSysDate
		public const string Wk_UserSchedule_AddX_Input_Wk_UserScheduleDtlNotFound = "ErrDemo.Lab.Wk_UserSchedule_AddX_Input_Wk_UserScheduleDtlNotFound"; //// //Wk_UserSchedule_AddX_Input_Wk_UserScheduleDtlNotFound
		public const string Wk_UserSchedule_AddX_Input_Wk_UserScheduleExistConflictDealerDateInput = "ErrDemo.Lab.Wk_UserSchedule_AddX_Input_Wk_UserScheduleExistConflictDealerDateInput"; //// //Wk_UserSchedule_AddX_Input_Wk_UserScheduleExistConflictDealerDateInput

		// Wk_UserSchedule_UpdateX:
		public const string Wk_UserSchedule_UpdateX = "ErrDemo.Lab.Wk_UserSchedule_UpdateX"; //// //Wk_UserSchedule_UpdateX
		public const string Wk_UserSchedule_UpdateX_InvalidSchCode = "ErrDemo.Lab.Wk_UserSchedule_UpdateX_InvalidSchCode"; //// //Wk_UserSchedule_UpdateX_InvalidSchCode
		public const string Wk_UserSchedule_UpdateX_InvalidEffDTimeStart = "ErrDemo.Lab.Wk_UserSchedule_UpdateX_InvalidEffDTimeStart"; //// //Wk_UserSchedule_UpdateX_InvalidEffDTimeStart
		public const string Wk_UserSchedule_UpdateX_InvalidEffDTimeEnd = "ErrDemo.Lab.Wk_UserSchedule_UpdateX_InvalidEffDTimeEnd"; //// //Wk_UserSchedule_UpdateX_InvalidEffDTimeEnd
		public const string Wk_UserSchedule_UpdateX_InvalidEffDTimeStartBeforeEffDTimeEnd = "ErrDemo.Lab.Wk_UserSchedule_UpdateX_InvalidEffDTimeStartBeforeEffDTimeEnd"; //// //Wk_UserSchedule_UpdateX_InvalidEffDTimeStartBeforeEffDTimeEnd
		public const string Wk_UserSchedule_UpdateX_InvalidEffDTimeStartAfterEffSysDate = "ErrDemo.Lab.Wk_UserSchedule_UpdateX_InvalidEffDTimeStartAfterEffSysDate"; //// //Wk_UserSchedule_UpdateX_InvalidEffDTimeStartAfterEffSysDate

		// Wk_UserSchedule_Delete:
		public const string Wk_UserSchedule_Delete = "ErrDemo.Lab.Wk_UserSchedule_Delete"; //// //Wk_UserSchedule_Delete
		#endregion

		#region // Wk_UserScheduleDtl:
		// Wk_UserScheduleDtl_AddX:
		public const string Wk_UserScheduleDtl_AddX = "ErrDemo.Lab.Wk_UserScheduleDtl_AddX"; //// //Wk_UserScheduleDtl_AddX
		public const string Wk_UserScheduleDtl_AddX_InvalidSchCode = "ErrDemo.Lab.Wk_UserScheduleDtl_AddX_InvalidSchCode"; //// //Wk_UserScheduleDtl_AddX_InvalidSchCode
		public const string Wk_UserScheduleDtl_AddX_InvalidUserCode = "ErrDemo.Lab.Wk_UserScheduleDtl_AddX_InvalidUserCode"; //// //Wk_UserScheduleDtl_AddX_InvalidUserCode
		public const string Wk_UserScheduleDtl_AddX_Input_Wk_UserScheduleExistConflictDealerDateInput = "ErrDemo.Lab.Wk_UserScheduleDtl_AddX_Input_Wk_UserScheduleExistConflictDealerDateInput"; //// //Wk_UserScheduleDtl_AddX_Input_Wk_UserScheduleExistConflictDealerDateInput

		// Wk_UserScheduleDtl_UpdX:
		public const string Wk_UserScheduleDtl_UpdX = "ErrDemo.Lab.Wk_UserScheduleDtl_UpdX"; //// //Wk_UserScheduleDtl_UpdX
		public const string Wk_UserScheduleDtl_UpdX_UserScheduleDtlNotFound = "ErrDemo.Lab.Wk_UserScheduleDtl_UpdX_UserScheduleDtlNotFound"; //// //Wk_UserScheduleDtl_UpdX_UserScheduleDtlNotFound

		// Wk_UserScheduleDtl_DelX:
		public const string Wk_UserScheduleDtl_DelX = "ErrDemo.Lab.Wk_UserScheduleDtl_DelX"; //// //Wk_UserScheduleDtl_DelX
		public const string Wk_UserScheduleDtl_DelX_UserScheduleDtlNotFound = "ErrDemo.Lab.Wk_UserScheduleDtl_DelX_UserScheduleDtlNotFound"; //// //Wk_UserScheduleDtl_DelX_UserScheduleDtlNotFound
		public const string Wk_UserScheduleDtl_DelX_UserScheduleInstExist = "ErrDemo.Lab.Wk_UserScheduleDtl_DelX_UserScheduleInstExist"; //// //Wk_UserScheduleDtl_DelX_UserScheduleInstExist

		// Wk_UserScheduleDtl_Approve:
		public const string Wk_UserScheduleDtl_Approve = "ErrDemo.Lab.Wk_UserScheduleDtl_Approve"; //// //Wk_UserScheduleDtl_Approve
		public const string Wk_UserScheduleDtl_Approve_UserScheduleDtlNotFound = "ErrDemo.Lab.Wk_UserScheduleDtl_Approve_UserScheduleDtlNotFound"; //// //Wk_UserScheduleDtl_Approve_UserScheduleDtlNotFound

		// Wk_UserScheduleDtl_Cancel:
		public const string Wk_UserScheduleDtl_Cancel = "ErrDemo.Lab.Wk_UserScheduleDtl_Cancel"; //// //Wk_UserScheduleDtl_Cancel
		public const string Wk_UserScheduleDtl_Cancel_UserScheduleDtlNotFound = "ErrDemo.Lab.Wk_UserScheduleDtl_Cancel_UserScheduleDtlNotFound"; //// //Wk_UserScheduleDtl_Cancel_UserScheduleDtlNotFound
		#endregion

		#region // Wk_UserScheduleInst:
		// Wk_UserScheduleInst_UpdX:
		public const string Wk_UserScheduleInst_UpdX = "ErrDemo.Lab.Wk_UserScheduleInst_UpdX"; //// //Wk_UserScheduleInst_UpdX
		public const string Wk_UserScheduleInst_UpdX_Wk_UserScheduleInstNotFound = "ErrDemo.Lab.Wk_UserScheduleInst_UpdX_Wk_UserScheduleInstNotFound"; //// //Wk_UserScheduleInst_UpdX_Wk_UserScheduleInstNotFound
		// Wk_UserScheduleInst_SaveX:
		public const string Wk_UserScheduleInst_SaveX = "ErrDemo.Lab.Wk_UserScheduleInst_SaveX"; //// //Wk_UserScheduleInst_SaveX
		public const string Wk_UserScheduleInst_SaveX_Wk_UserScheduleDtlNotFound = "ErrDemo.Lab.Wk_UserScheduleInst_SaveX_Wk_UserScheduleDtlNotFound"; //// //Wk_UserScheduleInst_SaveX_Wk_UserScheduleDtlNotFound
		public const string Wk_UserScheduleInst_SaveX_Wk_UserScheduleInstNotFound = "ErrDemo.Lab.Wk_UserScheduleInst_SaveX_Wk_UserScheduleInstNotFound"; //// //Wk_UserScheduleInst_SaveX_Wk_UserScheduleInstNotFound
		public const string Wk_UserScheduleInst_SaveX_InvalidEffDTimeStartBeforeEffCreateTime = "ErrDemo.Lab.Wk_UserScheduleInst_AddX_InvalidEffDTimeStartBeforeEffCreateTime"; //// //Wk_UserScheduleInst_AddX_InvalidEffDTimeStartBeforeEffCreateTime
		public const string Wk_UserScheduleInst_SaveX_UserScheduleDtl_InvalidUSStatusDtl = "ErrDemo.Lab.Wk_UserScheduleInst_SaveX_UserScheduleDtl_InvalidUSStatusDtl"; //// //Wk_UserScheduleInst_SaveX_UserScheduleDtl_InvalidUSStatusDtl

		// Wk_UserScheduleInst_GetX:
		public const string Wk_UserScheduleInst_GetX = "ErrDemo.Lab.Wk_UserScheduleInst_GetX"; //// //Wk_UserScheduleInst_GetX
		#endregion

		#region // Report:
		// Rpt_OLCampaignSummary_01:
		public const string Rpt_OLCampaignSummary_01 = "ErrDemo.Lab.Rpt_OLCampaignSummary_01"; //// //Rpt_OLCampaignSummary_01

		// Rpt_AuditUserCampaignSummary_02:
		public const string Rpt_AuditUserCampaignSummary_02 = "ErrDemo.Lab.Rpt_AuditUserCampaignSummary_02"; //// //Rpt_AuditUserCampaignSummary_02

		// Rpt_AuditUserCampaignSummary_01:
		public const string Rpt_AuditUserCampaignSummary_01 = "ErrDemo.Lab.Rpt_AuditUserCampaignSummary_01"; //// //Rpt_AuditUserCampaignSummary_01

		// Rpt_OLSignBoardsRequestSummary_01:
		public const string Rpt_OLSignBoardsRequestSummary_01 = "ErrDemo.Lab.Rpt_OLSignBoardsRequestSummary_01"; //// //Rpt_OLSignBoardsRequestSummary_01

		// Rpt_OLRevenueAwardSummary_01:
		public const string Rpt_OLRevenueAwardSummary_01 = "ErrDemo.Lab.Rpt_OLRevenueAwardSummary_01"; //// //Rpt_OLRevenueAwardSummary_01

		// Rpt_StarShopFluctuationSummary_01:
		public const string Rpt_StarShopFluctuationSummary_01 = "ErrDemo.Lab.Rpt_StarShopFluctuationSummary_01"; //// //Rpt_StarShopFluctuationSummary_01
		#endregion

		#region // Ord_Order:
		// Ord_Order_CheckDB:
		public const string Ord_Order_CheckDB_OrdCodeNotFound = "ErrDemo.Lab.Ord_Order_CheckDB_OrdCodeNotFound"; //// //Ord_Order_CheckDB_OrdCodeNotFound
		public const string Ord_Order_CheckDB_OrdCodeExist = "ErrDemo.Lab.Ord_Order_CheckDB_OrdCodeExist"; //// //Ord_Order_CheckDB_OrdCodeExist
		public const string Ord_Order_CheckDB_OrdStatusNotMatched = "ErrDemo.Lab.Ord_Order_CheckDB_OrdStatusNotMatched"; //// //Ord_Order_CheckDB_OrdStatusNotMatched

		// Ord_Order_GetX:
		public const string Ord_Order_GetX = "ErrDemo.Lab.Ord_Order_GetX"; //// //Ord_Order_GetX

		// Ord_Order_SaveX:
		public const string Ord_Order_SaveX = "ErrDemo.Lab.Ord_Order_SaveX"; //// //Ord_Order_SaveX
		public const string Ord_Order_SaveX_InputOrderDtlNotFound = "ErrDemo.Lab.Ord_Order_SaveX_InputOrderDtlNotFound"; //// //Ord_Order_SaveX_InputOrderDtlNotFound
		public const string Ord_Order_SaveX_InvalidDtlQtyOrd = "ErrDemo.Lab.Ord_Order_SaveX_InvalidDtlQtyOrd"; //// //Ord_Order_SaveX_InvalidDtlQtyOrd

		// Ord_Order_Approve
		public const string Ord_Order_Approve = "ErrDemo.Lab.Ord_Order_Approve"; //// //Ord_Order_Approve
		public const string Ord_Order_Approve_InputOrderDtlNotFound = "ErrDemo.Lab.Ord_Order_Approve_InputOrderDtlNotFound"; //// //Ord_Order_Approve_InputOrderDtlNotFound
		public const string Ord_Order_Approve_InvalidDtlQtyOrd = "ErrDemo.Lab.Ord_Order_Approve_InvalidDtlQtyOrd"; //// //Ord_Order_Approve_InvalidDtlQtyOrd

		// Ord_Order_Cancel
		public const string Ord_Order_Cancel = "ErrDemo.Lab.Ord_Order_Cancel"; //// //Ord_Order_Cancel
		#endregion

		#region // Ord_OrderDtl:
		public const string Ord_OrderDtl_CancelX = "ErrDemo.Lab.Ord_OrderDtl_CancelX"; //// //Ord_OrderDtl_CancelX
		#endregion

		#region // Report:
		public const string Rpt_UserSchedule_01 = "ErrDemo.Lab.Rpt_UserSchedule_01"; //// //Rpt_UserSchedule_01

		public const string Rpt_SalesSM_01 = "ErrDemo.Lab.Rpt_SalesSM_01"; //// //Rpt_SalesSM_01

		public const string Rpt_SalesDL_01 = "ErrDemo.Lab.Rpt_SalesDL_01"; //// //Rpt_SalesDL_01

		public const string Rpt_DensityAM_01 = "ErrDemo.Lab.Rpt_DensityAM_01"; //// //Rpt_DensityAM_01

		public const string Rpt_DensityChannel_01 = "ErrDemo.Lab.Rpt_DensityChannel_01"; //// //Rpt_DensityChannel_01
		#endregion

		#region // License:
		// Lic_Session_Get:
		public const string Lic_Session_Get = "ErrDemo.Lab.Lic_Session_Get"; //// //Lic_Session_Get

		// Lic_Session_Del:
		public const string Lic_Session_Del = "ErrDemo.Lab.Lic_Session_Del"; //// //Lic_Session_Del

		#endregion

		#region // Seq_Common:
		// Seq_Common_MyGet:
		public const string Seq_Common_MyGet_InvalidSequenceType = "ErrDemo.Lab.Seq_Common_MyGet_InvalidSequenceType"; //// //Seq_Common_MyGet_InvalidSequenceType

		// Seq_Common_Get:
		public const string Seq_Common_Get = "ErrDemo.Lab.Seq_Common_Get"; //// //Seq_Common_Get

		#endregion

        #region // Sys_Group:
        // Sys_Group_CheckDB:
        public const string Sys_Group_CheckDB_GroupCodeNotFound = "ErrDemoLab.Sys_Group_CheckDB_GroupCodeNotFound"; //// //Sys_Group_CheckDB_GroupCodeNotFound
        public const string Sys_Group_CheckDB_GroupCodeExist = "ErrDemoLab.Sys_Group_CheckDB_GroupCodeExist"; //// //Sys_Group_CheckDB_GroupCodeExist
        public const string Sys_Group_CheckDB_FlagActiveNotMatched = "ErrDemoLab.Sys_Group_CheckDB_FlagActiveNotMatched"; //// //Sys_Group_CheckDB_FlagActiveNotMatched
        public const string Sys_Group_CheckDB_FlagPublicNotMatched = "ErrDemoLab.Sys_Group_CheckDB_FlagPublicNotMatched"; //// //Sys_Group_CheckDB_FlagPublicNotMatched

        // Sys_Group_Get:
        public const string Sys_Group_Get = "ErrDemoLab.Sys_Group_Get"; //// //Sys_Group_Get

        // Sys_Group_Create:
        public const string Sys_Group_Create = "ErrDemoLab.Sys_Group_Create"; //// //Sys_Group_Create
        public const string Sys_Group_Create_InvalidGroupCode = "ErrDemoLab.Sys_Group_Create_InvalidGroupCode"; //// //Sys_Group_Create_InvalidGroupCode
        public const string Sys_Group_Create_InvalidGroupName = "ErrDemoLab.Sys_Group_Create_InvalidGroupName"; //// //Sys_Group_Create_InvalidGroupName

        // Sys_Group_Update:
        public const string Sys_Group_Update = "ErrDemoLab.Sys_Group_Update"; //// //Sys_Group_Update
        public const string Sys_Group_Update_InvalidGroupCode = "ErrDemoLab.Sys_Group_Update_InvalidGroupCode"; //// //Sys_Group_Update_InvalidGroupCode
        public const string Sys_Group_Update_InvalidGroupName = "ErrDemoLab.Sys_Group_Update_InvalidGroupName"; //// //Sys_Group_Update_InvalidGroupName

        // Sys_Group_Delete:
        public const string Sys_Group_Delete = "ErrDemoLab.Sys_Group_Delete"; //// //Sys_Group_Delete

        #endregion

        #region // Sys_User:
        // Sys_User_CheckDB:
        public const string Sys_User_CheckDB_UserCodeNotFound = "ErrDemoLab.Sys_User_CheckDB_UserCodeNotFound"; //// //Sys_User_CheckDB_UserCodeNotFound
        public const string Sys_User_CheckDB_UserCodeExist = "ErrDemoLab.Sys_User_CheckDB_UserCodeExist"; //// //Sys_User_CheckDB_UserCodeExist
        public const string Sys_User_CheckDB_FlagActiveNotMatched = "ErrDemoLab.Sys_User_CheckDB_FlagActiveNotMatched"; //// //Sys_User_CheckDB_FlagActiveNotMatched
        public const string Sys_User_CheckDB_FlagSysAdminNotMatched = "ErrDemoLab.Sys_User_CheckDB_FlagSysAdminNotMatched"; //// //Sys_User_CheckDB_FlagSysAdminNotMatched
        public const string Sys_User_CheckDB_BizInvalidUserAbility = "ErrDemoLab.Sys_User_CheckDB_BizInvalidUserAbility"; //// //Sys_User_CheckDB_BizInvalidUserAbility

        // Sys_User_BizInvalidUserAbility:
        public const string Sys_User_BizInvalidUserAbility = "ErrDemoLab.Sys_User_BizInvalidUserAbility"; //// //Sys_User_BizInvalidUserAbility

        // Sys_User_ChangePassword:
        public const string Sys_User_ChangePassword = "ErrDemoLab.Sys_User_ChangePassword"; //// //Sys_User_ChangePassword
        public const string Sys_User_ChangePassword_InvalidPasswordOld = "ErrDemoLab.Sys_User_ChangePassword_InvalidPasswordOld"; //// //Sys_User_ChangePassword_InvalidPasswordOld

        // Sys_User_GetForCurrentUser:
        public const string Sys_User_GetForCurrentUser = "ErrDemoLab.Sys_User_GetForCurrentUser"; //// //Sys_User_GetForCurrentUser

        // Sys_User_Login:
        public const string Sys_User_Login = "ErrDemoLab.Sys_User_Login"; //// //Sys_User_Login
        public const string Sys_User_Login_Checking = "ErrDemoLab.Sys_User_Login_Checking"; //// //Sys_User_Login_Checking
        public const string Sys_User_Login_InvalidPassword = "ErrDemoLab.Sys_User_Login_InvalidPassword"; //// //Sys_User_Login_InvalidPassword

        // Sys_User_Get:
        public const string Sys_User_Get = "ErrDemoLab.Sys_User_Get"; //// //Sys_User_Get
        public const string Sys_User_Get_01 = "ErrDemoLab.Sys_User_Get_01"; //// //Sys_User_Get_01

        // Sys_User_Logout:
        public const string Sys_User_Logout = "ErrDemoLab.Sys_User_Logout"; //// //Sys_User_Logout

        // Sys_User_GetByDB:
        public const string Sys_User_GetByDB = "ErrDemoLab.Sys_User_GetByDB"; //// //Sys_User_GetByDB

        // Sys_User_Create:
        public const string Sys_User_Create = "ErrDemoLab.Sys_User_Create"; //// //Sys_User_Create
        public const string Sys_User_Create_InvalidUserCode = "ErrDemoLab.Sys_User_Create_InvalidUserCode"; //// //Sys_User_Create_InvalidUserCode
        public const string Sys_User_Create_InvalidDBCode = "ErrDemoLab.Sys_User_Create_InvalidDBCode"; //// //Sys_User_Create_InvalidDBCode
        public const string Sys_User_Create_InvalidAreaCode = "ErrDemoLab.Sys_User_Create_InvalidAreaCode"; //// //Sys_User_Create_InvalidAreaCode
        public const string Sys_User_Create_InvalidUserName = "ErrDemoLab.Sys_User_Create_InvalidUserName"; //// //Sys_User_Create_InvalidUserName
        public const string Sys_User_Create_InvalidUserPassword = "ErrDemoLab.Sys_User_Create_InvalidUserPassword"; //// //Sys_User_Create_InvalidUserPassword
        public const string Sys_User_Create_InvalidFlagSysAdmin = "ErrDemoLab.Sys_User_Create_InvalidFlagSysAdmin"; //// //Sys_User_Create_InvalidFlagSysAdmin
        public const string Sys_User_Create_InvalidFlagDBAdmin = "ErrDemoLab.Sys_User_Create_InvalidFlagDBAdmin"; //// //Sys_User_Create_InvalidFlagDBAdmin

        // Sys_User_Update:
        public const string Sys_User_Update = "ErrDemoLab.Sys_User_Update"; //// //Sys_User_Update
        public const string Sys_User_Update_InvalidUserCode = "ErrDemoLab.Sys_User_Update_InvalidUserCode"; //// //Sys_User_Update_InvalidUserCode
        public const string Sys_User_Update_InvalidDBCode = "ErrDemoLab.Sys_User_Update_InvalidDBCode"; //// //Sys_User_Update_InvalidDBCode
        public const string Sys_User_Update_InvalidAreaCode = "ErrDemoLab.Sys_User_Update_InvalidAreaCode"; //// //Sys_User_Update_InvalidAreaCode
        public const string Sys_User_Update_InvalidUserName = "ErrDemoLab.Sys_User_Update_InvalidUserName"; //// //Sys_User_Update_InvalidUserName
        public const string Sys_User_Update_InvalidUserPassword = "ErrDemoLab.Sys_User_Update_InvalidUserPassword"; //// //Sys_User_Update_InvalidUserPassword
        public const string Sys_User_Update_InvalidFlagSysAdmin = "ErrDemoLab.Sys_User_Update_InvalidFlagSysAdmin"; //// //Sys_User_Update_InvalidFlagSysAdmin
        public const string Sys_User_Update_InvalidFlagDBAdmin = "ErrDemoLab.Sys_User_Update_InvalidFlagDBAdmin"; //// //Sys_User_Update_InvalidFlagDBAdmin

        // Sys_User_Delete:
        public const string Sys_User_Delete = "ErrDemoLab.Sys_User_Delete"; //// //Sys_User_Delete

        #endregion

        #region // Sys_UserInGroup:
        // Sys_UserInGroup_Save:
        public const string Sys_UserInGroup_Save = "ErrDemoLab.Sys_UserInGroup_Save"; //// //Sys_UserInGroup_Save
        public const string Sys_UserInGroup_Save_InputTblDtlNotFound = "ErrDemoLab.Sys_UserInGroup_Save_InputTblDtlNotFound"; //// //Sys_UserInGroup_Save_InputTblDtlNotFound

        #endregion

        #region // Sys_Access:
        // Sys_Access_CheckDeny:
        public const string Sys_Access_CheckDeny = "Sys_Access_CheckDeny"; //// //Sys_Access_CheckDeny
        // Sys_ViewAbility_Deny:
        public const string Sys_ViewAbility_Deny = "Sys_ViewAbility_Deny"; //// //Sys_ViewAbility_Deny
        public const string Sys_ViewAbility_NotExactUser = "Sys_ViewAbility_NotExactUser"; //// //Sys_ViewAbility_NotExactUser

        // Sys_Access_Get:
        public const string Sys_Access_Get = "ErrDemoLab.Sys_Access_Get"; //// //Sys_Access_Get

        // Sys_Access_Save:
        public const string Sys_Access_Save = "ErrDemoLab.Sys_Access_Save"; //// //Sys_Access_Save
        public const string Sys_Access_Save_InputTblDtlNotFound = "ErrDemoLab.Sys_Access_Save_InputTblDtlNotFound"; //// //Sys_Access_Save_InputTblDtlNotFound

        #endregion

        #region // Sys_Object:
        // Sys_Object_Get:
        public const string Sys_Object_Get = "ErrDemoLab.Sys_Object_Get"; //// //Sys_Object_Get
        #endregion

		#region // Wk_Record:
		// Wk_Record_CheckDB:
		public const string Wk_Record_CheckDB_RCNoNotFound = "ErrDemo.Lab.Wk_Record_CheckDB_RCNoNotFound"; //// //Wk_Record_CheckDB_RCNoNotFound
		public const string Wk_Record_CheckDB_RCNoExist = "ErrDemo.Lab.Wk_Record_CheckDB_RCNoExist"; //// //Wk_Record_CheckDB_RCNoExist
		public const string Wk_Record_CheckDB_RCStatusNotMatched = "ErrDemo.Lab.Wk_Record_CheckDB_RCStatusNotMatched"; //// //Wk_Record_CheckDB_RCStatusNotMatched
		public const string Wk_Record_CheckDB_InvalidDLCode = "ErrDemo.Lab.Wk_Record_CheckDB_InvalidDLCode"; //// //Wk_Record_CheckDB_InvalidDLCode
		public const string Wk_Record_CheckDB_InvalidUserCode = "ErrDemo.Lab.Wk_Record_CheckDB_InvalidUserCode"; //// //Wk_Record_CheckDB_InvalidUserCode

		// Wk_Record_GetX:
		public const string Wk_Record_GetX = "ErrDemo.Lab.Wk_Record_GetX"; //// //Wk_Record_GetX

		// Wk_Record_UpdX:
		public const string Wk_Record_UpdX = "ErrDemo.Lab.Wk_Record_UpdX"; //// //Wk_Record_UpdX
		public const string Wk_Record_UpdX_InvalidRCNo = "ErrDemo.Lab.Wk_Record_UpdX_InvalidRCNo"; //// //Wk_Record_UpdX_InvalidRCNo

		// Wk_Record_Add:
		public const string Wk_Record_Add = "ErrDemo.Lab.Wk_Record_Add"; //// //Wk_Record_Add
		public const string Wk_Record_Add_InvalidRCNo = "ErrDemo.Lab.Wk_Record_Add_InvalidRCNo"; //// //Wk_Record_Add_InvalidRCNo

		// Wk_Record_Del:
        public const string Wk_Record_DelX = "ErrDemo.Lab.Wk_Record_DelX"; //// //Wk_Record_DelX
        public const string Wk_Record_Del_InvalidRCNo = "ErrDemo.Lab.Wk_Record_Del_InvalidRCNo"; //// //Wk_Record_Del_InvalidRCNo

        // Wk_Record_Approve
        public const string Wk_Record_Approve = "ErrDemo.Lab.Wk_Record_Approve"; //// //Wk_Record_Approve
        public const string Wk_Record_Approve_InvalidRCNo = "ErrDemo.Lab.Wk_Record_Approve_InvalidRCNo"; //// //Wk_Record_Approve_InvalidRCNo
		#endregion

		#region // Wk_UserLocation:
		// Wk_UserLocation_Get:
        public const string Wk_UserLocation_GetX = "ErrDemo.Lab.Wk_UserLocation_GetX"; //// //Wk_UserLocation_GetX

		// Wk_UserLocation_Save:
		public const string Wk_UserLocation_InsertX = "ErrDemo.Lab.Wk_UserLocation_InsertX"; //// //Wk_UserLocation_InsertX
		public const string Wk_UserLocation_InsertX_UserLocationNotFound = "ErrDemo.Lab.Wk_UserLocation_InsertX_UserLocationNotFound"; //// //Wk_UserLocation_InsertX_UserLocationNotFound

		// Wk_UserLocation_Del:
		public const string Wk_UserLocation_Del = "ErrDemo.Lab.Wk_UserLocation_Del"; //// //Wk_UserLocation_Del

		#endregion

        #region // Report
        // Rpt_Ins_Claim_Summary:
        public const string Rpt_Ins_Claim_Summary = "ErrDemo.Lab.Rpt_Ins_Claim_Summary"; //// //Rpt_Ins_Claim_Summary

        // Rpt_Ins_Claim_ByContractType:
        public const string Rpt_Ins_Claim_ByContractType = "ErrDemo.Lab.Rpt_Ins_Claim_ByContractType"; //// //Rpt_Ins_Claim_ByContractType

        // Rpt_Mst_Garage_Summary:
        public const string Rpt_Mst_Garage_Summary = "ErrDemo.Lab.Rpt_Mst_Garage_Summary"; //// //Rpt_Mst_Garage_Summary
        #endregion

        #region // Mst_Garage:
        // Mst_Garage_CheckDB:
        public const string Mst_Garage_CheckDB_GRCodeNotFound = "ErrDemo.Lab.Mst_Garage_CheckDB_GRCodeNotFound"; //// //Mst_Garage_CheckDB_GRCodeNotFound
        public const string Mst_Garage_CheckDB_GRCodeExist = "ErrDemo.Lab.Mst_Garage_CheckDB_GRCodeExist"; //// //Mst_Garage_CheckDB_GRCodeExist
        public const string Mst_Garage_CheckDB_FlagActiveNotMatched = "ErrDemo.Lab.Mst_Garage_CheckDB_FlagActiveNotMatched"; //// //Mst_Garage_CheckDB_FlagActiveNotMatched

        // Mst_Garage_Get:
        public const string Mst_Garage_Get = "ErrDemo.Lab.Mst_Garage_Get"; //// //Mst_Garage_Get

        // Mst_Garage_Create:
        public const string Mst_Garage_Create = "ErrDemo.Lab.Mst_Garage_Create"; //// //Mst_Garage_Create
        public const string Mst_Garage_Create_InvalidGRCode = "ErrDemo.Lab.Mst_Garage_Create_InvalidGRCode"; //// //Mst_Garage_Create_InvalidGRCode
        public const string Mst_Garage_Create_InvalidGRName = "ErrDemo.Lab.Mst_Garage_Create_InvalidGRName"; //// //Mst_Garage_Create_InvalidGRName

        // Mst_Garage_Update:
        public const string Mst_Garage_Update = "ErrDemo.Lab.Mst_Garage_Update"; //// //Mst_Garage_Update
        public const string Mst_Garage_Update_InvalidGRCode = "ErrDemo.Lab.Mst_Garage_Update_InvalidGRCode"; //// //Mst_Garage_Update_InvalidGRCode
        public const string Mst_Garage_Update_InvalidGRName = "ErrDemo.Lab.Mst_Garage_Update_InvalidGRName"; //// //Mst_Garage_Update_InvalidGRName

        // Mst_Garage_Delete:
        public const string Mst_Garage_Delete = "ErrDemo.Lab.Mst_Garage_Delete"; //// //Mst_Garage_Delete
		#endregion

		#region // Mst_StarShopHist:
		// Mst_StarShopHist_CheckDB:
		public const string Mst_StarShopHist_CheckDB_StarShopHistNotFound = "ErrDemoLab.Mst_StarShopHist_CheckDB_StarShopHistNotFound"; //// //Mst_StarShopHist_CheckDB_StarShopHistNotFound
		public const string Mst_StarShopHist_CheckDB_StarShopHistExist = "ErrDemoLab.Mst_StarShopHist_CheckDB_StarShopHistExist"; //// //Mst_StarShopHist_CheckDB_StarShopHistExist

		// Mst_StarShopHist_Create:
		public const string Mst_StarShopHist_Create = "ErrDemoLab.Mst_StarShopHist_Create"; //// //Mst_StarShopHist_Create
		public const string Mst_StarShopHist_Create_InvalidEffDateStart = "ErrDemoLab.Mst_StarShopHist_Create_InvalidEffDateStart"; //// //Mst_StarShopHist_Create_InvalidEffDateStart
		public const string Mst_StarShopHist_Create_InvalidEffDateStartAfterSysDate = "ErrDemoLab.Mst_StarShopHist_Create_InvalidEffDateStartAfterSysDate"; //// //Mst_StarShopHist_Create_InvalidEffDateStartAfterSysDate
		public const string Mst_StarShopHist_Create_InvalidExistEffDateStart = "ErrDemoLab.Mst_StarShopHist_Create_InvalidExistEffDateStart"; //// //Mst_StarShopHist_Create_InvalidExistEffDateStart	

		#endregion

		#region // Mst_POSMType: 
		// Mst_POSMType_CheckDB:
		public const string Mst_POSMType_CheckDB_POSMTypeNotFound = "ErrDemoLab.Mst_POSMType_CheckDB_POSMTypeNotFound"; //// //Mst_POSMType_CheckDB_POSMTypeNotFound
		public const string Mst_POSMType_CheckDB_POSMTypeExist = "Mst_POSMType_CheckDB_POSMTypeExist"; //// //Mst_POSMType_CheckDB_POSMTypeExist
		public const string Mst_POSMType_CheckDB_FlagActiveNotMatched = "Mst_POSMType_CheckDB_FlagActiveNotMatched"; //// //Mst_POSMType_CheckDB_FlagActiveNotMatched

		// Mst_POSMType_Get
		public const string Mst_POSMType_Get = "ErrDemoLab.Mst_POSMType_Get"; //// //Mst_POSMType_Get

		// Mst_POSMType_Create
		public const string Mst_POSMType_Create = "ErrDemoLab.Mst_POSMType_Create"; //// //Mst_POSMType_Create
		public const string Mst_POSMType_Create_InvalidPOSMType = "ErrDemoLab.Mst_POSMType_Create_InvalidPOSMType"; //// //Mst_POSMType_Create_InvalidPOSMType
		public const string Mst_POSMType_Create_InvalidPOSMTypeName = "ErrDemoLab.Mst_POSMType_Create_InvalidPOSMTypeName"; //// //Mst_POSMType_Create_InvalidPOSMTypeName    

		// Mst_POSMType_Update:
		public const string Mst_POSMType_Update = "ErrDemoLab.Mst_POSMType_Update"; //// //Mst_POSMType_Update                
		public const string Mst_POSMType_Update_InvalidPOSMTypeName = "ErrDemoLab.Mst_POSMType_Update_InvalidPOSMTypeName"; //// //Mst_POSMType_Update_InvalidPOSMTypeName                       

		// Mst_POSMType_Delete:
		public const string Mst_POSMType_Delete = "ErrDemoLab.Mst_POSMType_Delete"; //// Mã lỗi: Mst_POSMType_Delete ////

		#endregion

		#region // Mst_POSMUnitType:
		// Mst_POSMUnitType_CheckDB:
		public const string Mst_POSMUnitType_CheckDB_POSMUnitTypeNotFound = "ErrDemoLab.Mst_POSMUnitType_CheckDB_POSMUnitTypeNotFound"; //// //Mst_POSMUnitType_CheckDB_POSMUnitTypeNotFound
		public const string Mst_POSMUnitType_CheckDB_POSMUnitTypeExist = "ErrDemoLab.Mst_POSMUnitType_CheckDB_POSMUnitTypeExist"; //// //Mst_POSMUnitType_CheckDB_POSMUnitTypeExist
		public const string Mst_POSMUnitType_CheckDB_FlagActiveNotMatched = "ErrDemoLab.Mst_POSMUnitType_CheckDB_FlagActiveNotMatched"; //// //Mst_POSMUnitType_CheckDB_FlagActiveNotMatched

		// Mst_POSMUnitType_Get
		public const string Mst_POSMUnitType_Get = "ErrDemoLab.Mst_POSMUnitType_Get"; //// //Mst_POSMUnitType_Get

		// Mst_POSMUnitType_Create
		public const string Mst_POSMUnitType_Create = "ErrDemoLab.Mst_POSMUnitType_Create"; //// //Mst_POSMUnitType_Create
		public const string Mst_POSMUnitType_Create_InvalidPOSMUnitType = "ErrDemoLab.Mst_POSMUnitType_Create_InvalidPOSMUnitType"; //// //Mst_POSMUnitType_Create_InvalidPOSMUnitType
		public const string Mst_POSMUnitType_Create_InvalidPOSMUnitTypeName = "ErrDemoLab.Mst_POSMUnitType_Create_InvalidPOSMUnitTypeName"; //// //Mst_POSMUnitType_Create_InvalidPOSMUnitTypeName    

		// Mst_POSMUnitType_Update:
		public const string Mst_POSMUnitType_Update = "ErrDemoLab.Mst_POSMUnitType_Update"; //// //Mst_POSMUnitType_Update                
		public const string Mst_POSMUnitType_Update_InvalidPOSMUnitTypeName = "ErrDemoLab.Mst_POSMUnitType_Update_InvalidPOSMUnitTypeName"; //// //Mst_POSMUnitType_Update_InvalidPOSMUnitTypeName                       

		// Mst_POSMUnitType_Delete:
		public const string Mst_POSMUnitType_Delete = "ErrDemoLab.Mst_POSMUnitType_Delete"; //// Mã lỗi: Mst_POSMUnitType_Delete ////

		#endregion

		#region // Mst_POSM:
		// Mst_POSM_CheckDB:
		public const string Mst_POSM_CheckDB_POSMNotFound = "ErrDemoLab.Mst_POSM_CheckDB_POSMNotFound"; //// //Mst_POSM_CheckDB_POSMNotFound
		public const string Mst_POSM_CheckDB_POSMExist = "ErrDemoLab.Mst_POSM_CheckDB_POSMExist"; //// //Mst_POSM_CheckDB_POSMExist
		public const string Mst_POSM_CheckDB_FlagActiveNotMatched = "ErrDemoLab.Mst_POSM_CheckDB_FlagActiveNotMatched"; //// //Mst_POSM_CheckDB_FlagActiveNotMatched

		// Mst_POSM_Get:
		public const string Mst_POSM_Get = "ErrDemoLab.Mst_POSM_Get"; //// //Mst_POSM_Get

		// Mst_POSM_Create:
		public const string Mst_POSM_Create = "ErrDemoLab.Mst_POSM_Create"; //// //Mst_POSM_Create
		public const string Mst_POSM_Create_InvalidPOSMCode = "ErrDemoLab.Mst_POSM_Create_InvalidPOSMCode"; //// //Mst_POSM_Create_InvalidPOSMCode
		public const string Mst_POSM_Create_InvalidPOSMName = "ErrDemoLab.Mst_POSM_Create_InvalidPOSMName"; //// //Mst_POSM_Create_InvalidPOSMName        

		// Mst_POSM_Update:
		public const string Mst_POSM_Update = "ErrDemoLab.Mst_POSM_Update"; //// //Mst_POSM_Update                
		public const string Mst_POSM_Update_InvalidPOSMName = "ErrDemoLab.Mst_POSM_Update_InvalidPOSMName"; //// //Mst_POSM_Update_InvalidPOSMName                       

		// Mst_POSM_Delete:
		public const string Mst_POSM_Delete = "ErrDemoLab.Mst_POSM_Delete"; //// Mã lỗi: Mst_POSM_Delete ////

		#endregion

		#region // Mst_StarShopGroup:
		// Mst_StarShopGroup_CheckDB:
		public const string Mst_StarShopGroup_CheckDB_StarShopGroupNotFound = "ErrDemoLab.Mst_StarShopGroup_CheckDB_StarShopGroupNotFound"; //// //Mst_StarShopGroup_CheckDB_StarShopGroupNotFound
		public const string Mst_StarShopGroup_CheckDB_StarShopGroupExist = "Mst_StarShopGroup_CheckDB_StarShopGroupExist"; //// //Mst_StarShopGroup_CheckDB_StarShopGroupExist
		public const string Mst_StarShopGroup_CheckDB_FlagActiveNotMatched = "Mst_StarShopGroup_CheckDB_FlagActiveNotMatched"; //// //Mst_StarShopGroup_CheckDB_FlagActiveNotMatched

		// Mst_StarShopGroup_Get
		public const string Mst_StarShopGroup_Get = "ErrDemoLab.Mst_StarShopGroup_Get"; //// //Mst_StarShopGroup_Get

		// Mst_StarShopGroup_Create
		public const string Mst_StarShopGroup_Create = "ErrDemoLab.Mst_StarShopGroup_Create"; //// //Mst_StarShopGroup_Create
		public const string Mst_StarShopGroup_Create_InvalidSSGrpCode = "ErrDemoLab.Mst_StarShopGroup_Create_InvalidSSGrpCode"; //// //Mst_StarShopGroup_Create_InvalidSSGrpCode
		public const string Mst_StarShopGroup_Create_InvalidSSGrpName = "ErrDemoLab.Mst_StarShopGroup_Create_InvalidSSGrpName"; //// //Mst_StarShopGroup_Create_InvalidSSGrpName    

		// Mst_StarShopGroup_Update:
		public const string Mst_StarShopGroup_Update = "ErrDemoLab.Mst_StarShopGroup_Update"; //// //Mst_StarShopGroup_Update                
		public const string Mst_StarShopGroup_Update_InvalidSSGrpName = "ErrDemoLab.Mst_StarShopGroup_Update_InvalidSSGrpName"; //// //Mst_StarShopGroup_Update_InvalidSSGrpName                       

		// Mst_StarShopGroup_Delete:
		public const string Mst_StarShopGroup_Delete = "ErrDemoLab.Mst_StarShopGroup_Delete"; //// Mã lỗi: Mst_StarShopGroup_Delete ////

		#endregion

		#region // Mst_StarShopBrand:
		public const string Mst_StarShopBrand_CheckDB_Mst_StarShopBrandNotFound = "ErrDemoLab.Mst_StarShopBrand_CheckDB_Mst_StarShopBrandNotFound"; //// //Mst_StarShopBrand_CheckDB_Mst_StarShopBrandNotFound
		public const string Mst_StarShopBrand_CheckDB_Mst_StarShopBrandExist = "Mst_StarShopBrand_CheckDB_Mst_StarShopBrandExist"; //// //Mst_StarShopBrand_CheckDB_Mst_StarShopBrandExist
		public const string Mst_StarShopBrand_CheckDB_FlagActiveNotMatched = "Mst_StarShopBrand_CheckDB_FlagActiveNotMatched"; //// //Mst_StarShopBrand_CheckDB_FlagActiveNotMatched

		// Mst_StarShopBrand_Get
		public const string Mst_StarShopBrand_Get = "ErrDemoLab.Mst_StarShopBrand_Get"; //// //Mst_StarShopBrand_Get

		// Mst_StarShopBrand_Create
		public const string Mst_StarShopBrand_Create = "ErrDemoLab.Mst_StarShopBrand_Create"; //// //Mst_StarShopBrand_Create
		public const string Mst_StarShopBrand_Create_InvalidSSBrandCode = "ErrDemoLab.Mst_StarShopBrand_Create_InvalidSSBrandCode"; //// //Mst_StarShopBrand_Create_InvalidSSBrandCode
		public const string Mst_StarShopBrand_Create_InvalidSSBrandName = "ErrDemoLab.Mst_StarShopBrand_Create_InvalidSSBrandName"; //// //Mst_StarShopBrand_Create_InvalidSSBrandName    

		// Mst_StarShopBrand_Update:
		public const string Mst_StarShopBrand_Update = "ErrDemoLab.Mst_StarShopBrand_Update"; //// //Mst_StarShopBrand_Update                
		public const string Mst_StarShopBrand_Update_InvalidSSBrandName = "ErrDemoLab.Mst_StarShopBrand_Update_InvalidSSBrandName"; //// //Mst_StarShopBrand_Update_InvalidSSBrandName                       

		// Mst_StarShopBrand_Delete:
		public const string Mst_StarShopBrand_Delete = "ErrDemoLab.Mst_StarShopBrand_Delete"; //// Mã lỗi: Mst_StarShopBrand_Delete ////

		#endregion

		#region // Mst_StarShopType:
		public const string Mst_StarShopType_CheckDB_Mst_StarShopTypeNotFound = "ErrDemoLab.Mst_StarShopType_CheckDB_Mst_StarShopTypeNotFound"; //// //Mst_StarShopType_CheckDB_Mst_StarShopTypeNotFound
		public const string Mst_StarShopType_CheckDB_Mst_StarShopTypeExist = "Mst_StarShopType_CheckDB_Mst_StarShopTypeExist"; //// //Mst_StarShopType_CheckDB_Mst_StarShopTypeExist
		public const string Mst_StarShopType_CheckDB_FlagActiveNotMatched = "Mst_StarShopType_CheckDB_FlagActiveNotMatched"; //// //Mst_StarShopType_CheckDB_FlagActiveNotMatched

		// Mst_StarShopType_Get
		public const string Mst_StarShopType_Get = "ErrDemoLab.Mst_StarShopType_Get"; //// //Mst_StarShopType_Get

		// Mst_StarShopType_Create
		public const string Mst_StarShopType_Create = "ErrDemoLab.Mst_StarShopType_Create"; //// //Mst_StarShopType_Create
		public const string Mst_StarShopType_Create_InvalidSSGrpCode = "ErrDemoLab.Mst_StarShopType_Create_InvalidSSGrpCode"; //// //Mst_StarShopType_Create_InvalidSSGrpCode
		public const string Mst_StarShopType_Create_InvalidSSBrandCode = "ErrDemoLab.Mst_StarShopType_Create_InvalidSSBrandCode"; //// //Mst_StarShopType_Create_InvalidSSBrandCode
		public const string Mst_StarShopType_Create_InvalidSSTypeName = "ErrDemoLab.Mst_StarShopType_Create_InvalidSSTypeName"; //// //Mst_StarShopType_Create_InvalidSSTypeName
		public const string Mst_StarShopType_Create_InvalidSSRate = "ErrDemoLab.Mst_StarShopType_Create_InvalidSSRate"; //// //Mst_StarShopType_Create_InvalidSSRate

		// Mst_StarShopType_Update:
		public const string Mst_StarShopType_Update = "ErrDemoLab.Mst_StarShopType_Update"; //// //Mst_StarShopType_Update                
		public const string Mst_StarShopType_Update_InvalidSSTypeName = "ErrDemoLab.Mst_StarShopType_Update_InvalidSSTypeName"; //// //Mst_StarShopType_Update_InvalidSSTypeName
		public const string Mst_StarShopType_Update_InvalidSSRate = "ErrDemoLab.Mst_StarShopType_Update_InvalidSSRate"; //// //Mst_StarShopType_Update_InvalidSSRate

		// Mst_StarShopType_Delete:
		public const string Mst_StarShopType_Delete = "ErrDemoLab.Mst_StarShopType_Delete"; //// Mã lỗi: Mst_StarShopType_Delete ////

		#endregion

	}
}

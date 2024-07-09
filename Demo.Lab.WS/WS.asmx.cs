using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.IO;

using CmUtils = CommonUtils;
//using TDAL = EzDAL.MyDB;
//using TDALUtils = EzDAL.Utils;
using TConst = Demo.Lab.Constants;
using TUtils = Demo.Lab.Utils;
using TError = Demo.Lab.Errors;
using TBiz = Demo.Lab.Biz;



namespace Demo.Lab.WS
{
	/// <summary>
	/// Summary description for WS
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class WS : System.Web.Services.WebService
	{
		#region // Constructors and Destructors:
		public WS()
		{
			try
			{
				//Uncomment the following line if using designed components 
				//InitializeComponent();

				// Myinit:
				LoadConfig();
			}
			catch (Exception exc)
			{
				_mdsInitError = CmUtils.CMyDataSet.NewMyDataSet(DateTime.Now.ToString("yyyyMMdd.HHmmss.ffffff"));
				CmUtils.CMyDataSet.SetErrorCode(ref _mdsInitError, TError.ErrDemoLab.CmSys_ServiceInit);
				CmUtils.CMyDataSet.AppendErrorParams(
					ref _mdsInitError
					, "Exception.Message", exc.Message
					, "Exception.StackTrace", exc.StackTrace
					);
				_mdsInitError.AcceptChanges();
			}
		}

		private TBiz.BizDemoLab _biz = null;
		private DataSet _mdsInitError = null;
		private void LoadConfig()
		{
			_biz = new TBiz.BizDemoLab();
			_biz.LoadConfig(ConfigurationManager.AppSettings, "WS");
		}

		private object[] WSReturn(DataSet ds)
		{
			return CmUtils.ConvertUtils.DataSet2Array(ds);
		}

		#endregion

		#region // Common (Cm):
		[WebMethod]
		public string Cm_Reinit(
			string strBiz_SpecialPw
			)
		{
			// CmSys_InvalidBizSpecialPw:
			if (!CmUtils.StringUtils.StringEqual(ConfigurationManager.AppSettings["Biz_SpecialPw"], strBiz_SpecialPw))
				return TError.ErrDemoLab.CmSys_InvalidBizSpecialPw;

			// Reinit:
			TBiz.BizDemoLab.Cm_Reinit();

			// Return Good:
			return TError.Error.NoError;
		}
		[WebMethod]
		public string Cm_Test(
			)
		{
			// Check _mdsInitError:
			if (_mdsInitError != null) return CmUtils.XmlUtils.DataSet2Xml(_mdsInitError);

			// Return Good:
			return _biz.Cm_Test();
		}
		[WebMethod]
		public object[] Cm_GetId(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			)
		{
			// Temp:
			string strFunctionName = "Cm_GetId";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});

			try
			{
				// CheckGatewayAuthentication:
				TUtils.CConnectionManager.CheckGatewayAuthentication(
					_biz._cf.nvcParams // nvcParams
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Cm_GetId(
				strTid
				));
		}
		[WebMethod]
		public object[] Cm_GetDTime(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			)
		{
			// Temp:
			string strFunctionName = "Cm_GetDTime";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});

			try
			{
				// CheckGatewayAuthentication:
				TUtils.CConnectionManager.CheckGatewayAuthentication(
					_biz._cf.nvcParams // nvcParams
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Cm_GetDTime(
				strTid
				));
		}
		[WebMethod]
		public object[] Mst_Common_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, string strTableName
			, object objFilter0
			, object objFilter1
			, object objFilter2
			)
		{
			// Temp:
			string strFunctionName = "Mst_Common_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Common_Get(
			strTid
			, drSession
			////
			, strTableName
			, objFilter0
			, objFilter1
			, objFilter2
			));
		}

		[WebMethod]
		public object[] Cm_Test_Load(
			object[] arrobjParams
			)
		{
			return arrobjParams;
		}
		#endregion

		#region // Seq_Common:
		[WebMethod]
		public object[] Seq_Common_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, string strSequenceType
			, string strParam_Prefix
			, string strParam_Postfix
			)
		{
			// Temp:
			string strFunctionName = "Seq_Common_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Seq_Common_Get(
			strTid
			, drSession
			////
			, strSequenceType
			, strParam_Prefix
			, strParam_Postfix
			));
		}

		#endregion

		#region // Mst_AreaMarket:
		[WebMethod]
		public object[] Mst_AreaMarket_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_AreaMarket
			)
		{
			// Temp:
			string strFunctionName = "Mst_AreaMarket_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_AreaMarket_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_AreaMarket
			));
		}

		[WebMethod]
		public object[] Mst_AreaMarket_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objAreaCode
			, object objAreaCodeParent
			, object objAreaDesc
			)
		{
			// Temp:
			string strFunctionName = "Mst_AreaMarket_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_AreaMarket_Create(
			strTid
			, drSession
			//// 
			, objAreaCode
			, objAreaCodeParent
			, objAreaDesc
			));
		}

		[WebMethod]
		public object[] Mst_AreaMarket_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objAreaCode
			, object objAreaCodeParent
			, object objAreaDesc
			, object objAreaStatus
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_AreaMarket_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_AreaMarket_Update(
			strTid
			, drSession
			//// 
			, objAreaCode
			, objAreaCodeParent
			, objAreaDesc
			, objAreaStatus
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_AreaMarket_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objAreaCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_AreaMarket_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_AreaMarket_Delete(
			strTid
			, drSession
			//// 
			, objAreaCode
			));
		}
		#endregion

		#region // Mst_Distributor:
		[WebMethod]
		public object[] Mst_Distributor_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_Distributor
			)
		{
			// Temp:
			string strFunctionName = "Mst_Distributor_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Distributor_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_Distributor
			));
		}

		[WebMethod]
		public object[] Mst_Distributor_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objDBCode
			, object objDBCodeParent
			, object objDBName
			, object objAreaCode
			, object objProvinceCode
			, object objDBAddress
			, object objDBContactName
			, object objDBPhoneNo
			, object objDBFaxNo
			, object objDBMobilePhoneNo
			, object objDBSMSPhoneNo
			, object objDBTaxCode
			, object objRemark
			, object objDiscountPercent
			, object objBalance
			, object objOverdraftThreshold
			)
		{
			// Temp:
			string strFunctionName = "Mst_Distributor_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Distributor_Create(
			strTid
			, drSession
			//// 
			, objDBCode
			, objDBCodeParent
			, objDBName
			, objAreaCode
			, objProvinceCode
			, objDBAddress
			, objDBContactName
			, objDBPhoneNo
			, objDBFaxNo
			, objDBMobilePhoneNo
			, objDBSMSPhoneNo
			, objDBTaxCode
			, objRemark
			, objDiscountPercent
			, objBalance
			, objOverdraftThreshold
			));
		}

		[WebMethod]
		public object[] Mst_Distributor_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objDBCode
		   , object objDBCodeParent
		   , object objDBName
		   , object objAreaCode
		   , object objProvinceCode
		   , object objDBAddress
		   , object objDBContactName
		   , object objDBPhoneNo
		   , object objDBFaxNo
		   , object objDBMobilePhoneNo
		   , object objDBSMSPhoneNo
		   , object objDBTaxCode
		   , object objRemark
		   , object objDBStatus
		   , object objDiscountPercent
		   , object objBalance
		   , object objOverdraftThreshold
		   , object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_Distributor_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Distributor_Update(
			strTid
			, drSession
			//// 
			, objDBCode
		   , objDBCodeParent
		   , objDBName
		   , objAreaCode
		   , objProvinceCode
		   , objDBAddress
		   , objDBContactName
		   , objDBPhoneNo
		   , objDBFaxNo
		   , objDBMobilePhoneNo
		   , objDBSMSPhoneNo
		   , objDBTaxCode
		   , objRemark
		   , objDBStatus
		   , objDiscountPercent
		   , objBalance
		   , objOverdraftThreshold
		   , objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_Distributor_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objDBCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_Distributor_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Distributor_Delete(
			strTid
			, drSession
			//// 
			, objDBCode
			));
		}
		#endregion

		#region // Mst_Outlet:
		[WebMethod]
		public object[] Mst_Outlet_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_Outlet
			, string strRt_Cols_Mst_StarShopHist
			, string strRt_Cols_OL_SignBoardsHist
			)
		{
			// Temp:
			string strFunctionName = "Mst_Outlet_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Outlet_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_Outlet
			, strRt_Cols_Mst_StarShopHist
			, strRt_Cols_OL_SignBoardsHist
			));
		}

		[WebMethod]
		public object[] Mst_Outlet_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objOLCode
			, object objDBCode
			, object objDistrictCode
			, object objOLName
			, object objOLAddress
			, object objOLOwnerName
			, object objOLOwnerDateBirth
			, object objOLOwnerSignImagePath
			, object objOLTaxCode
			, object objOLOwnerIdCardNo
			, object objOLOwnerIdCardFrontImagePath
			, object objOLOwnerIdCardRearImagePath
			, object objOLOwnerIdAddress
			, object objOLPhoneNo
			, object objOLFaxNo
			, object objOLMobilePhoneNo
			, object objOLSMSPhoneNo
			, object objOLBankAccountNo
			, object objOLBankName
			, object objRemark
			)
		{
			// Temp:
			string strFunctionName = "Mst_Outlet_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Outlet_Create(
			strTid
			, drSession
			//// 
			, objOLCode
			, objDBCode
			, objDistrictCode
			, objOLName
			, objOLAddress
			, objOLOwnerName
			, objOLOwnerDateBirth
			, objOLOwnerSignImagePath
			, objOLTaxCode
			, objOLOwnerIdCardNo
			, objOLOwnerIdCardFrontImagePath
			, objOLOwnerIdCardRearImagePath
			, objOLOwnerIdAddress
			, objOLPhoneNo
			, objOLFaxNo
			, objOLMobilePhoneNo
			, objOLSMSPhoneNo
			, objOLBankAccountNo
			, objOLBankName
			, objRemark
			));
		}

		[WebMethod]
		public object[] Mst_Outlet_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
		   ////
		   , object objOLCode
			, object objDBCode
			, object objDistrictCode
			, object objOLName
			, object objOLAddress
			, object objOLOwnerName
			, object objOLOwnerDateBirth
			, object objOLOwnerSignImagePath
			, object objOLTaxCode
			, object objOLOwnerIdCardNo
			, object objOLOwnerIdCardFrontImagePath
			, object objOLOwnerIdCardRearImagePath
			, object objOLOwnerIdAddress
			, object objOLPhoneNo
			, object objOLFaxNo
			, object objOLMobilePhoneNo
			, object objOLSMSPhoneNo
			, object objOLBankAccountNo
			, object objOLBankName
			, object objRemark
			, object objOLStatus
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_Outlet_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Outlet_Update(
			strTid
			, drSession
			   //// 
			   , objOLCode
			, objDBCode
			, objDistrictCode
			, objOLName
			, objOLAddress
			, objOLOwnerName
			, objOLOwnerDateBirth
			, objOLOwnerSignImagePath
			, objOLTaxCode
			, objOLOwnerIdCardNo
			, objOLOwnerIdCardFrontImagePath
			, objOLOwnerIdCardRearImagePath
			, objOLOwnerIdAddress
			, objOLPhoneNo
			, objOLFaxNo
			, objOLMobilePhoneNo
			, objOLSMSPhoneNo
			, objOLBankAccountNo
			, objOLBankName
			, objRemark
			, objOLStatus
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_Outlet_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objOLCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_Outlet_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Outlet_Delete(
			strTid
			, drSession
			//// 
			, objOLCode
			));
		}
		#endregion

		#region // Mst_Province:
		[WebMethod]
		public object[] Mst_Province_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_Province
			)
		{
			// Temp:
			string strFunctionName = "Mst_Province_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Province_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_Province
			));
		}

		[WebMethod]
		public object[] Mst_Province_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objProvinceCode
			, object objProvinceName
			)
		{
			// Temp:
			string strFunctionName = "Mst_Province_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Province_Create(
			strTid
			, drSession
			//// 
			, objProvinceCode
			, objProvinceName
			));
		}

		[WebMethod]
		public object[] Mst_Province_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objProvinceCode
			, object objProvinceName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_Province_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Province_Update(
			strTid
			, drSession
			//// 
			, objProvinceCode
			, objProvinceName
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_Province_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objProvinceCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_Province_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_Province_Delete(
			strTid
			, drSession
			//// 
			, objProvinceCode
			));
		}
		#endregion

		#region // Mst_District:
		[WebMethod]
		public object[] Mst_District_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_District
			)
		{
			// Temp:
			string strFunctionName = "Mst_District_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_District_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_District
			));
		}

		[WebMethod]
		public object[] Mst_District_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objDistrictCode
			, object objProvinceCode
			, object objDistrictName
			)
		{
			// Temp:
			string strFunctionName = "Mst_District_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_District_Create(
			strTid
			, drSession
		   //// 
		   , objDistrictCode
		   , objProvinceCode
			, objDistrictName
			));
		}

		[WebMethod]
		public object[] Mst_District_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			 ////
			 , object objDistrictCode
			, object objProvinceCode
			, object objDistrictName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_District_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_District_Update(
			strTid
			, drSession
			//// 
			, objDistrictCode
			, objProvinceCode
			, objDistrictName
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_District_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
		   ////
		   , object objDistrictCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_District_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_District_Delete(
			strTid
			, drSession
			//// 
			, objDistrictCode
			));
		}
		#endregion

		#region // Mst_ActionType:
		[WebMethod]
		public object[] Mst_ActionType_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_ActionType
			)
		{
			// Temp:
			string strFunctionName = "Mst_ActionType_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_ActionType_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_ActionType
			));
		}

		[WebMethod]
		public object[] Mst_ActionType_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objActionType
			, object objActionTypeDesc
			, object objAvgScoreValStart
			)
		{
			// Temp:
			string strFunctionName = "Mst_ActionType_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_ActionType_Create(
			strTid
			, drSession
			//// 
			, objActionType
			, objActionTypeDesc
			, objAvgScoreValStart
			));
		}

		[WebMethod]
		public object[] Mst_ActionType_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objActionType
			, object objActionTypeDesc
			, object objAvgScoreValStart
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_ActionType_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_ActionType_Update(
			strTid
			, drSession
			//// 
			, objActionType
			, objActionTypeDesc
			, objAvgScoreValStart
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}
		#endregion

		#region // Lic_License:
		[WebMethod]
		public object[] Lic_Session_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Lic_Session
			)
		{
			// Temp:
			string strFunctionName = "Lic_Session_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Lic_Session_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Lic_Session
			));
		}

		[WebMethod]
		public object[] Lic_Session_Del(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object strSessionId_Del
			)
		{
			// Temp:
			string strFunctionName = "Lic_Session_Del";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Lic_Session_Del(
			strTid
			, drSession
			////
			, strSessionId_Del
			));
		}


		#endregion

		#region // Sys_User:
		[WebMethod]
		public object[] Sys_User_ChangePassword(
				string strGwUserCode
				, string strGwPassword
				, string strTid
				, string strSessionId
				, string strUserPasswordOld
				, string strUserPasswordNew
			)
		{
			// Temp:
			string strFunctionName = "Sys_User_ChangePassword";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_User_ChangePassword(
				strTid
				, drSession
				, strUserPasswordOld
				, strUserPasswordNew
				));
		}
		[WebMethod]
		public object[] Sys_User_GetForCurrentUser(
				string strGwUserCode
				, string strGwPassword
				, string strTid
				, string strSessionId
			)
		{
			// Temp:
			string strFunctionName = "Sys_User_GetForCurrentUser";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_User_GetForCurrentUser(
				strTid
				, drSession
				));
		}
		[WebMethod]
		public object[] Sys_User_Login(
				string strGwUserCode
				, string strGwPassword
				, string strTid
				, string strRootSvCode
				, string strRootUserCode
				, string strServiceCode
				, string strUserCode
				, string strLanguageCode
				, string strUserPassword
				, string strOtherInfo
			)
		{
			// Temp:
			string strFunctionName = "Sys_User_Login";

			//#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});

			try
			{
				// Refine:
				strRootSvCode = TUtils.CUtils.StdParam(strRootSvCode);
				strRootUserCode = TUtils.CUtils.StdParam(strRootUserCode);
				strServiceCode = TUtils.CUtils.StdParam(strServiceCode);
				strUserCode = TUtils.CUtils.StdParam(strUserCode);

				// CheckGatewayAuthentication:
				TUtils.CConnectionManager.CheckGatewayAuthentication(
					_biz._cf.nvcParams // nvcParams
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					);

				// Check ServiceCode:
				if (strServiceCode.Length <= 0
					|| !_biz._cf.nvcParams["Biz_ServiceCodeList"].Contains(string.Format("|{0}|", strServiceCode))
					)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.ServiceCode", strServiceCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.CmSys_InvalidServiceCode
						, null
						, alParamsCoupleError.ToArray()
						);
				}

				// Sign in:
				mdsFinal = _biz.Sys_User_Login(
					strTid // strTid
					, strRootSvCode // strRootSvCode
					, strRootUserCode // strRootUserCode
					, strServiceCode // strServiceCode
					, strUserCode // strUserCode
					, strLanguageCode // strLanguageCode
					, strUserPassword // strUserPassword
					, strOtherInfo // strOtherInfo
					);
				if (CmUtils.CMyDataSet.HasError(mdsFinal)) return WSReturn(mdsFinal);

				// CleanSessionExpired:
				TUtils.CConnectionManager.CleanSessionExpired(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // sess
					);

				// Register new Session:
				string strSessionId = _biz._cf.sess.Register(
					TSession.Core.CSession.c_strRegMode_UnlimitMulti // strRegisterModeCode
					, strRootSvCode // strRootSvCode
					, strRootUserCode // strRootUserCode
					, strServiceCode // strServiceCode
					, strUserCode // strUserCode
					, strLanguageCode // strLanguageCode
					, "" // strInfoInternal
					, strOtherInfo // strInfoExternal
					);
				CmUtils.CMyDataSet.SetRemark(ref mdsFinal, string.Format("{0}|{1}|{2}", strSessionId, _biz._cf.nvcParams["Biz_Name"], strUserCode));
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					));
			}

			// Return Good:
			mdsFinal.AcceptChanges();
			return WSReturn(mdsFinal);
		}
		[WebMethod]
		public object[] Sys_User_Logout(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			)
		{
			// Temp:
			string strFunctionName = "Sys_User_Logout";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_User_Logout(
			strTid
			, drSession
			////
			, strSessionId
			));
		}
		[WebMethod]
		public object[] Sys_User_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_User
			, string strRt_Cols_Sys_UserInGroup
			)
		{
			// Temp:
			string strFunctionName = "Sys_User_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_User_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Sys_User
			, strRt_Cols_Sys_UserInGroup
			));
		}

		[WebMethod]
		public object[] Sys_User_Get_01(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_User
			, string strRt_Cols_Sys_UserInGroup
			)
		{
			// Temp:
			string strFunctionName = "Sys_User_Get_01";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_User_Get_01(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Sys_User
			, strRt_Cols_Sys_UserInGroup
			));
		}


		[WebMethod]
		public object[] Sys_User_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objUserCode
			, object objDBCode
			, object objAreaCode
			, object objUserName
			, object objUserPassword
			, object objFlagSysAdmin
			, object objFlagDBAdmin
			)
		{
			// Temp:
			string strFunctionName = "Sys_User_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_User_Create(
			strTid
			, drSession
			////
			, objUserCode
			, objDBCode
			, objAreaCode
			, objUserName
			, objUserPassword
			, objFlagSysAdmin
			, objFlagDBAdmin
			));
		}
		[WebMethod]
		public object[] Sys_User_Upd(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objUserCode
			, object objDBCode
			, object objAreaCode
			, object objUserName
			, object objUserPassword
			, object objFlagSysAdmin
			, object objFlagDBAdmin
			, object objFlagActive
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Sys_User_Upd";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_User_Update(
			strTid
			, drSession
			////
			, objUserCode
			, objDBCode
			, objAreaCode
			, objUserName
			, objUserPassword
			, objFlagSysAdmin
			, objFlagDBAdmin
			, objFlagActive
			, objFt_Cols_Upd
			));
		}
		[WebMethod]
		public object[] Sys_User_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objUserCode
			)
		{
			// Temp:
			string strFunctionName = "Sys_User_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_User_Delete(
			strTid
			, drSession
			////
			, objUserCode
			));
		}

		#endregion

		#region // Sys_Group:
		[WebMethod]
		public object[] Sys_Group_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_Group
			, string strRt_Cols_Sys_UserInGroup
			)
		{
			// Temp:
			string strFunctionName = "Sys_Group_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_Group_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Sys_Group
			, strRt_Cols_Sys_UserInGroup
			));
		}
		[WebMethod]
		public object[] Sys_Group_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objGroupCode
			, object objGroupName
			)
		{
			// Temp:
			string strFunctionName = "Sys_Group_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_Group_Create(
			strTid
			, drSession
			////
			, objGroupCode
			, objGroupName
			));
		}
		[WebMethod]
		public object[] Sys_Group_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objGroupCode
			, object objGroupName
			, object objFlagActive
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Sys_Group_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_Group_Update(
			strTid
			, drSession
			////
			, objGroupCode
			, objGroupName
			, objFlagActive
			, objFt_Cols_Upd
			));
		}
		[WebMethod]
		public object[] Sys_Group_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objGroupCode
			)
		{
			// Temp:
			string strFunctionName = "Sys_Group_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_Group_Delete(
			strTid
			, drSession
		   ////
		   , objGroupCode
			));
		}

		#endregion

		#region // Sys_UserInGroup:
		[WebMethod]
		public object[] Sys_UserInGroup_Save(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objGroupCode
			, object[] arrobjDSData
			)
		{
			// Temp:
			string strFunctionName = "Sys_UserInGroup_Save";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_UserInGroup_Save(
			strTid
			, drSession
			////
			, objGroupCode
			, arrobjDSData
			));
		}

		#endregion

		#region // Sys_Access:
		[WebMethod]
		public object[] Sys_Access_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_Access
			)
		{
			// Temp:
			string strFunctionName = "Sys_Access_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_Access_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Sys_Access
			));
		}
		[WebMethod]
		public object[] Sys_Access_Save(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objGroupCode
			, object[] arrobjDSData
			)
		{
			// Temp:
			string strFunctionName = "Sys_Access_Save";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_Access_Save(
			strTid
			, drSession
			////
			, objGroupCode
			, arrobjDSData
			));
		}

		#endregion

		#region // Sys_Object
		[WebMethod]
		public object[] Sys_Object_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_Access
			)
		{
			// Temp:
			string strFunctionName = "Sys_Object_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Sys_Object_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Sys_Access
			));
		}
		#endregion

		#region // Mst_POSMType:
		[WebMethod]
		public object[] Mst_POSMType_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_POSMType
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSMType_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSMType_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_POSMType
			));
		}

		[WebMethod]
		public object[] Mst_POSMType_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objPOSMType
			, object objPOSMTypeName
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSMType_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSMType_Create(
			strTid
			, drSession
			//// 
			, objPOSMType
			, objPOSMTypeName
			));
		}

		[WebMethod]
		public object[] Mst_POSMType_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objPOSMType
			, object objPOSMTypeName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSMType_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSMType_Update(
			strTid
			, drSession
			//// 
			, objPOSMType
			, objPOSMTypeName
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_POSMType_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objPOSMType
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSMType_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSMType_Delete(
			strTid
			, drSession
			//// 
			, objPOSMType
			));
		}
		#endregion

		#region // Mst_POSMUnitType:
		[WebMethod]
		public object[] Mst_POSMUnitType_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_POSMUnitType
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSMUnitType_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSMUnitType_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_POSMUnitType
			));
		}

		[WebMethod]
		public object[] Mst_POSMUnitType_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objPOSMUnitType
			, object objPOSMUnitTypeName
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSMUnitType_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSMUnitType_Create(
			strTid
			, drSession
			//// 
			, objPOSMUnitType
			, objPOSMUnitTypeName
			));
		}

		[WebMethod]
		public object[] Mst_POSMUnitType_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objPOSMUnitType
			, object objPOSMUnitTypeName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSMUnitType_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSMUnitType_Update(
			strTid
			, drSession
			//// 
			, objPOSMUnitType
			, objPOSMUnitTypeName
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_POSMUnitType_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objPOSMUnitType
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSMUnitType_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSMUnitType_Delete(
			strTid
			, drSession
			//// 
			, objPOSMUnitType
			));
		}
		#endregion

		#region // Mst_POSM:
		[WebMethod]
		public object[] Mst_POSM_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_POSM
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSM_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSM_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_POSM
			));
		}

		[WebMethod]
		public object[] Mst_POSM_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objPOSMCode
			, object objPOSMType
			, object objPOSMUnitType
			, object objPOSMName
			, object objPOSMDesc
			, object objPOSMImageFilePath
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSM_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSM_Create(
			strTid
			, drSession
			//// 
			, objPOSMCode
			, objPOSMType
			, objPOSMUnitType
			, objPOSMName
			, objPOSMDesc
			, objPOSMImageFilePath
			));
		}

		[WebMethod]
		public object[] Mst_POSM_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objPOSMCode
			, object objPOSMType
			, object objPOSMUnitType
			, object objPOSMName
			, object objPOSMDesc
			, object objPOSMImageFilePath
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSM_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSM_Update(
			strTid
			, drSession
			//// 
			, objPOSMCode
			, objPOSMType
			, objPOSMUnitType
			, objPOSMName
			, objPOSMDesc
			, objPOSMImageFilePath
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_POSM_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objPOSMCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_POSM_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_POSM_Delete(
			strTid
			, drSession
			//// 
			, objPOSMCode
			));
		}
		#endregion

		#region // Mst_StarShopGroup:
		[WebMethod]
		public object[] Mst_StarShopGroup_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_StarShopGroup
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopGroup_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopGroup_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_StarShopGroup
			));
		}

		[WebMethod]
		public object[] Mst_StarShopGroup_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objSSGrpCode
			, object objSSGrpName
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopGroup_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopGroup_Create(
			strTid
			, drSession
			//// 
			, objSSGrpCode
			, objSSGrpName
			));
		}

		[WebMethod]
		public object[] Mst_StarShopGroup_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objSSGrpCode
			, object objSSGrpName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopGroup_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopGroup_Update(
			strTid
			, drSession
			//// 
			, objSSGrpCode
			, objSSGrpName
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_StarShopGroup_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objSSGrpCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopGroup_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopGroup_Delete(
			strTid
			, drSession
			//// 
			, objSSGrpCode
			));
		}
		#endregion

		#region // Mst_StarShopBrand:
		[WebMethod]
		public object[] Mst_StarShopBrand_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_StarShopBrand
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopBrand_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopBrand_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_StarShopBrand
			));
		}

		[WebMethod]
		public object[] Mst_StarShopBrand_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objSSBrandCode
			, object objSSBrandName
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopBrand_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopBrand_Create(
			strTid
			, drSession
			//// 
			, objSSBrandCode
			, objSSBrandName
			));
		}

		[WebMethod]
		public object[] Mst_StarShopBrand_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objSSBrandCode
			, object objSSBrandName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopBrand_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopBrand_Update(
			strTid
			, drSession
			//// 
			, objSSBrandCode
			, objSSBrandName
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_StarShopBrand_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objSSBrandCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopBrand_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopBrand_Delete(
			strTid
			, drSession
			//// 
			, objSSBrandCode
			));
		}
		#endregion

		#region // Mst_StarShopType:
		[WebMethod]
		public object[] Mst_StarShopType_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_StarShopType
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopType_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopType_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_StarShopType
			));
		}

		[WebMethod]
		public object[] Mst_StarShopType_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objSSGrpCode
			, object objSSBrandCode
			, object objSSTypeName
			, object objSSRate
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopType_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopType_Create(
			strTid
			, drSession
			//// 
			, objSSGrpCode
			, objSSBrandCode
			, objSSTypeName
			, objSSRate
			));
		}

		[WebMethod]
		public object[] Mst_StarShopType_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objSSGrpCode
			, object objSSBrandCode
			, object objSSTypeName
			, object objSSRate
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopType_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopType_Update(
			strTid
			, drSession
			//// 
			, objSSGrpCode
			, objSSBrandCode
			, objSSTypeName
			, objSSRate
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_StarShopType_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objSSGrpCode
			, object objSSBrandCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopType_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopType_Delete(
			strTid
			, drSession
			//// 
			, objSSGrpCode
			, objSSBrandCode
			));
		}
		#endregion

		#region // Mst_StarShopHist:

		[WebMethod]
		public object[] Mst_StarShopHist_Create(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objOLCode
			, object objSSGrpCode
			, object objSSBrandCode
			, object objEffDateStart
			)
		{
			// Temp:
			string strFunctionName = "Mst_StarShopHist_Create";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_StarShopHist_Create(
			strTid
			, drSession
			//// 
			, objOLCode
			, objSSGrpCode
			, objSSBrandCode
			, objEffDateStart
			));
		}
		#endregion

		#region // Mst_CampainCriteria:
		[WebMethod]
		public object[] Mst_CampainCriteria_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_CampainCriteria
			, string strRt_Cols_Mst_CampainCriteriaScope
			)
		{
			// Temp:
			string strFunctionName = "Mst_CampainCriteria_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_CampainCriteria_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_CampainCriteria
			, strRt_Cols_Mst_CampainCriteriaScope
			));
		}

		[WebMethod]
		public object[] Mst_CampainCriteria_Update(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objCampaignCrCode
			, object objCampaignCrName
			, object objCampainCriteriaType
			, object objCreateDTime
			, object objCreateBy
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			// Temp:
			string strFunctionName = "Mst_CampainCriteria_Update";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_CampainCriteria_Update(
			strTid
			, drSession
			//// 
			, objCampaignCrCode
			, objCampaignCrName
			, objCampainCriteriaType
			, objCreateDTime
			, objCreateBy
			, objFlagActive
			////
			, objFt_Cols_Upd
			));
		}

		[WebMethod]
		public object[] Mst_CampainCriteria_Delete(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objCampaignCrCode
			)
		{
			// Temp:
			string strFunctionName = "Mst_CampainCriteria_Delete";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_CampainCriteria_Delete(
			strTid
			, drSession
			//// 
			, objCampaignCrCode
			));
		}

		[WebMethod]
		public object[] Mst_CampainCriteria_Save(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objFlagIsDelete
			////
			, object objCampaignCrCode
			, object objCampaignCrName
			, object objCampainCriteriaType
			, object[] arrobjDSData
			)
		{
			// Temp:
			string strFunctionName = "Mst_CampainCriteria_Save";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_CampainCriteria_Save(
			strTid
			, drSession
			//// 
			, objFlagIsDelete
			////
			, objCampaignCrCode
			, objCampaignCrName
			, objCampainCriteriaType
			, arrobjDSData
			));
		}
		#endregion

		#region // Mst_CampainCriteriaScope:
		[WebMethod]
		public object[] Mst_CampainCriteriaScope_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_CampainCriteriaScope
			, string strRt_Cols_Mst_CampainExhibitedPOSM
			, string strRt_Cols_Mst_CampainCriteriaAward
			, string strRt_Cols_Mst_CampainCriteriaAwardDtl
			)
		{
			// Temp:
			string strFunctionName = "Mst_CampainCriteriaScope_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Mst_CampainCriteriaScope_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Mst_CampainCriteriaScope
			, strRt_Cols_Mst_CampainExhibitedPOSM
			, strRt_Cols_Mst_CampainCriteriaAward
			, strRt_Cols_Mst_CampainCriteriaAwardDtl
			));
		}
		#endregion

		#region // Aud_Campaign:
		[WebMethod]
		public object[] Aud_Campaign_Save(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objFlagIsDelete
			////
			, object objCampaignCode
			, object objCampaignCrCode
			, object objCrtrScoreVerCode
			, object objCrtrScoreVerAUCode
			, object objCampaignName
			, object objEffDTimeStart
			, object objEffDTimeEnd
			, object objQtyCheck
			, object objQtySuccess
			, object objMinIntervalDays
			, object objReportEndDate
			, object[] arrobjDSData
			)
		{
			// Temp:
			string strFunctionName = "Aud_Campaign_Save";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Aud_Campaign_Save(
			strTid
			, drSession
			//// 
			, objFlagIsDelete
			////
			, objCampaignCode
			, objCampaignCrCode
			, objCrtrScoreVerCode
			, objCrtrScoreVerAUCode
			, objCampaignName
			, objEffDTimeStart
			, objEffDTimeEnd
			, objQtyCheck
			, objQtySuccess
			, objMinIntervalDays
			, objReportEndDate
			, arrobjDSData
			));
		}

		[WebMethod]
		public object[] Aud_Campaign_Approve(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// 
			, object objCampaignCode
			, object objRemark
			)
		{
			// Temp:
			string strFunctionName = "Aud_Campaign_Approve";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Aud_Campaign_Approve(
			strTid
			, drSession
			////
			, objCampaignCode
			, objRemark
			));
		}
		[WebMethod]
		public object[] Aud_Campaign_Cancel(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// 
			, object objCampaignCode
			, object objRemark
			)
		{
			// Temp:
			string strFunctionName = "Aud_Campaign_Cancel";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Aud_Campaign_Cancel(
			strTid
			, drSession
			////
			, objCampaignCode
			, objRemark
			));
		}

		[WebMethod]
		public object[] Aud_Campaign_Get(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Aud_Campaign
			, string strRt_Cols_Aud_CampaignDoc
			, string strRt_Cols_Aud_CampaignDBPOSMDtl
			)
		{
			// Temp:
			string strFunctionName = "Aud_Campaign_Get";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Aud_Campaign_Get(
			strTid
			, drSession
			//// Filter:
			, strFt_RecordStart
			, strFt_RecordCount
			, strFt_WhereClause
			//// Return:
			, strRt_Cols_Aud_Campaign
			, strRt_Cols_Aud_CampaignDoc
			, strRt_Cols_Aud_CampaignDBPOSMDtl
			));
		}
		#endregion

		#region // Aud_CampaignDBReceive:
		[WebMethod]
		public object[] Aud_CampaignDBReceive_Save(
			string strGwUserCode
			, string strGwPassword
			, string strTid
			, string strSessionId
			////
			, object objFlagIsDelete
			////
			, object objDBReceiveNo
			, object objCampaignCode
			, object[] arrobjDSData
			)
		{
			// Temp:
			string strFunctionName = "Aud_CampaignDBReceive_Save";

			#region // Check:
			// Check Init:
			if (_mdsInitError != null) return WSReturn(_mdsInitError);
			string strErrorCodeDefault = TError.ErrDemoLab.CmSys_SessionPreInitFailed;
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				});
			DataRow drSession = null;

			try
			{
				// Check all:
				TUtils.CConnectionManager.CheckAllCondition(
					_biz._cf.nvcParams // nvcParams
					, _biz._cf.sess // ss
					, ref alParamsCoupleError // alParamsCoupleError
					, strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, strSessionId // strSessionId
					, out drSession // drSession
					);

				// Init SessionInfo:
				_biz._cf.sinf = new TBiz.CSessionInfo(drSession);
			}
			catch (Exception exc)
			{
				return WSReturn(TUtils.CProcessExc.Process(
					ref mdsFinal // mdsFinal
					, exc // exc
					, strErrorCodeDefault // strErrorCode
					, alParamsCoupleError.ToArray() // arrobjErrorParams
					));
			}
			#endregion

			// Return Good:
			return WSReturn(_biz.Aud_CampaignDBReceive_Save(
			strTid
			, drSession
			//// 
			, objFlagIsDelete
			////
			, objDBReceiveNo
			, objCampaignCode
			, arrobjDSData
			));
		}

		#endregion

	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;
using System.Collections;
using System.Xml;
using System.Linq;
using System.Threading;
//using System.Xml.Linq;

using CmUtils = CommonUtils;
using TDAL = EzDAL.MyDB;
using TDALUtils = EzDAL.Utils;
using TConst = Demo.Lab.Constants;
using TUtils = Demo.Lab.Utils;
using TError = Demo.Lab.Errors;


namespace Demo.Lab.Biz
{
	public class CConfig
	{
		// ConfigInfo:
		public System.Collections.Specialized.NameValueCollection nvcParams = null;

		// DB:
		public TDAL.IEzDAL db = null;
		public TDAL.IEzDAL db_Sys = null;

		// Session:
		public TSession.Core.CSession sess = null;
		public CSessionInfo sinf = null;

		// Log:
		public TLog.Core.CLog log = null;

		// Methods:
		public CConfig MyClone()
		{
			// Init:
			CConfig cf = new CConfig();
			cf.nvcParams = this.nvcParams;
			cf.db = (TDAL.IEzDAL)this.db.Clone();
			cf.db_Sys = (TDAL.IEzDAL)this.db_Sys.Clone();
			cf.sess = this.sess;
			cf.log = this.log;

			// Return Good:
			return cf;
		}

		public void ProcessBizReq(
			object strTid
			, object strFunctionName
			, ArrayList alParamsCoupleError
			)
		{
			// Write RequestLog:
			this.log.WriteLog(
				this.nvcParams["Biz_Name"] // strGwUserCode
				, this.nvcParams["Biz_LogPw"] // strGwPassword
				, "1" // strFlagDelayForLazy
				, strTid // strAppTid
				, this.sinf.drSession["RootSvCode"] // strAppRootSvCode
				, this.sinf.drSession["RootUserCode"] // strAppRootUserCode
				, this.sinf.drSession["ServiceCode"] // strAppServiceCode
				, this.sinf.drSession["UserCode"] // strAppUserCode
				, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") // strAppTDateTime
				, "0" // strAppErrorCode = NoError
				, strFunctionName // strAppLogType1
				, "RQ" // strAppLogType2 = Request
				, "" // strAppLogType3
				, this.sinf.drSession["LanguageCode"] // strAppLanguageCode
				, "" // strAppRemark
				, alParamsCoupleError.ToArray() // arrobjParamsCouple
				);
		}

		public DataSet ProcessBizReturn(
			ref DataSet mdsFinal
			, object strTid
			, object strFunctionName
			)
		{
			// Init:
			object[] arrobjParamsCouple = null;
			object strAppLogType3 = null;
			if (string.Equals(CmUtils.CMyDataSet.GetErrorCode(mdsFinal), TError.Error.NoError))
			{
				strAppLogType3 = (CmUtils.CMyDataSet.HasWarning(mdsFinal) ? "WARNING" : null);
				arrobjParamsCouple = CmUtils.CMyDataSet.GetWarningParams(mdsFinal);
			}
			else
			{
				strAppLogType3 = null;
				arrobjParamsCouple = CmUtils.CMyDataSet.GetErrorParams(mdsFinal);
			}

			// Write ReturnLog:
			this.log.WriteLog(
				this.nvcParams["Biz_Name"] // strGwUserCode
				, this.nvcParams["Biz_LogPw"] // strGwPassword
				, "1" // strFlagDelayForLazy
				, strTid // strAppTid
				, this.sinf.drSession["RootSvCode"] // strAppRootSvCode
				, this.sinf.drSession["RootUserCode"] // strAppRootUserCode
				, this.sinf.drSession["ServiceCode"] // strAppServiceCode
				, this.sinf.drSession["UserCode"] // strAppUserCode
				, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") // strAppTDateTime
				, CmUtils.CMyDataSet.GetErrorCode(mdsFinal) // strAppErrorCode
				, strFunctionName // strAppLogType1
				, "RS" // strAppLogType2 = Result
				, strAppLogType3 // strAppLogType3
				, this.sinf.drSession["LanguageCode"] // strAppLanguageCode
				, CmUtils.CMyDataSet.GetRemark(mdsFinal) // strAppRemark
				, arrobjParamsCouple // arrobjParamsCouple
				);

			// Return:
			mdsFinal.AcceptChanges();
			return mdsFinal;
		}

	}
	public class CSessionInfo
	{
		public DataRow drSession = null;
		public string strUserCode = "";

		public CSessionInfo(DataRow drSessionInit)
		{
			this.drSession = drSessionInit;
			this.strUserCode = Convert.ToString(drSessionInit["UserCode"]);
		}
	}

	public class CBGProcess
	{
		public int BizUpdStatus_nSleepMax = 5000; // millisecond
		public int BizUpdStatus_nSleepStep = 20; // millisecond
		public int BizUpdStatus_nForceProcess = 0;
		public System.Threading.Thread BizUpdStatus_t = null;

	}

}

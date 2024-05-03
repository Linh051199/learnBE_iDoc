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
	public partial class BizDemoLab
	{
		#region //// License:
		public DataSet Lic_Session_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Lic_Session
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			bool bNeedTransaction = false;
			string strFunctionName = "Lic_Session_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Lic_Session_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Lic_Session", strRt_Cols_Lic_Session
					});
			#endregion

			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				if (bNeedTransaction) _cf.db.BeginTransaction();

				// Write RequestLog:
				_cf.ProcessBizReq(
					strTid // strTid
					, strFunctionName // strFunctionName
					, alParamsCoupleError // alParamsCoupleError
					);

				// Check Access/Deny:
				Sys_Access_CheckDeny(
					ref alParamsCoupleError
					, strFunctionName
					);
				#endregion

				#region // Check:
				// Refine:
				long nFilterRecordStart = Convert.ToInt64(strFt_RecordStart);
				long nFilterRecordEnd = nFilterRecordStart + Convert.ToInt64(strFt_RecordCount) - 1;
				//bool bGet_Lic_Session = (strRt_Cols_Lic_Session != null && strRt_Cols_Lic_Session.Length > 0);

				//// drAbilityOfUser:
				DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);

				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.sess.Get(
					//// Filter:
					strFt_RecordStart // strFt_RecordStart
					, strFt_RecordCount // strFt_RecordCount
					, strFt_WhereClause // strFt_WhereClause
					//// Return:
					, strRt_Cols_Lic_Session // strRt_Cols_Lic_Session
					);
				string strTableName = "";
				strTableName = "Lic_Session_Summary";
				if (dsGetData.Tables.Contains(strTableName))
				{
					DataTable dtMove = dsGetData.Tables[strTableName];
					CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dtMove);
				}
				strTableName = "Lic_Session";
				if (dsGetData.Tables.Contains(strTableName))
				{
					DataTable dtMove = dsGetData.Tables[strTableName];
					CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dtMove);
				}
				#endregion

				// Return Good:
				TDALUtils.DBUtils.CommitSafety(_cf.db);
				mdsFinal.AcceptChanges();
				return mdsFinal;
			}
			catch (Exception exc)
			{
				#region // Catch of try:
				// Rollback:
				TDALUtils.DBUtils.RollbackSafety(_cf.db);

				// Return Bad:
				return TUtils.CProcessExc.Process(
					ref mdsFinal
					, exc
					, strErrorCodeDefault
					, alParamsCoupleError.ToArray()
					);
				#endregion
			}
			finally
			{
				#region // Finally of try:
				// Rollback and Release resources:
				TDALUtils.DBUtils.RollbackSafety(_cf.db);
				TDALUtils.DBUtils.ReleaseAllSemaphore(_cf.db_Sys, true);

				// Write ReturnLog:
				_cf.ProcessBizReturn(
					ref mdsFinal // mdsFinal
					, strTid // strTid
					, strFunctionName // strFunctionName
					);
				#endregion
			}
		}
		public DataSet Lic_Session_Del(
			string strTid
			, DataRow drSession
			////
			, object strSessionId_Del
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			bool bNeedTransaction = false;
			string strFunctionName = "Lic_Session_Del";
			string strErrorCodeDefault = TError.ErrDemoLab.Lic_Session_Del;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// 
					, "strSessionId_Del", strSessionId_Del
					});
			#endregion

			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				if (bNeedTransaction) _cf.db.BeginTransaction();

				// Write RequestLog:
				_cf.ProcessBizReq(
					strTid // strTid
					, strFunctionName // strFunctionName
					, alParamsCoupleError // alParamsCoupleError
					);

				// Check Access/Deny:
				Sys_Access_CheckDeny(
					ref alParamsCoupleError
					, strFunctionName
					);
				#endregion

				#region // Check:
				// Refine:

				//// drAbilityOfUser:
				DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);

				#endregion

				#region // Del:
				_cf.sess.Remove(false, strSessionId_Del);
				#endregion

				// Return Good:
				TDALUtils.DBUtils.CommitSafety(_cf.db);
				mdsFinal.AcceptChanges();
				return mdsFinal;
			}
			catch (Exception exc)
			{
				#region // Catch of try:
				// Rollback:
				TDALUtils.DBUtils.RollbackSafety(_cf.db);

				// Return Bad:
				return TUtils.CProcessExc.Process(
					ref mdsFinal
					, exc
					, strErrorCodeDefault
					, alParamsCoupleError.ToArray()
					);
				#endregion
			}
			finally
			{
				#region // Finally of try:
				// Rollback and Release resources:
				TDALUtils.DBUtils.RollbackSafety(_cf.db);
				TDALUtils.DBUtils.ReleaseAllSemaphore(_cf.db_Sys, true);

				// Write ReturnLog:
				_cf.ProcessBizReturn(
					ref mdsFinal // mdsFinal
					, strTid // strTid
					, strFunctionName // strFunctionName
					);
				#endregion
			}
		}

		#endregion
	}
}

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
		#region // Constructors and Destructors:
		public BizDemoLab()
		{

		}

		private static bool init_bSuccess = false;
		private static object init_objLock = new object();
		private static CConfig init_cf = null;
		private static CmUtils.SeqUtils init_seq = new CmUtils.SeqUtils();
		private static CBGProcess init_bgp = new CBGProcess();
		public CConfig _cf = null;
		public void LoadConfig(
			System.Collections.Specialized.NameValueCollection nvcParams
			, string strSvCode
			)
		{
			// Init:
			if (!init_bSuccess)
			{
				lock (init_objLock)
				{
					if (!init_bSuccess)
					{
						// Init:
						CConfig cf = new CConfig();

						// Params:
						cf.nvcParams = nvcParams;

						// DB:
						cf.db = new TDAL.EzDALSqlSv(cf.nvcParams["Biz_DBConnStr"]);
						cf.db.LogAutoMode = "";
						cf.db.LogUserId = "";
						cf.db.InitCacheManual();
						cf.db_Sys = (TDAL.IEzDAL)cf.db.Clone();

						// Session:
						cf.sess = new TSession.Core.CSession(cf.nvcParams["Biz_LicenseCode"], cf.nvcParams["Biz_DBConnStr"]);

						// Log:
						cf.log = new TLog.Core.CLog(
							cf.nvcParams["TLog_ConnStr"] // strConnStr
							, cf.nvcParams["TLog_AccountList"] // strAccountList
							, Convert.ToInt32(cf.nvcParams["TLog_DelayForLazyMS"]) // nDelayForLazy
							);
						cf.log.StartBackGroundProcess();

						// Assign:
						init_cf = cf;

						// init_LoadBackgroundProcess:
						init_LoadBackgroundProcess();

						// Mark:
						init_bSuccess = true;
					}
				}
			}

			// Assign:
			_cf = init_cf.MyClone();
		}

		#endregion

		#region // bgp_BizUpdStatus:
		private static void init_LoadBackgroundProcess()
		{
			// BizUpdStatus_t:
			if (init_bgp.BizUpdStatus_t != null && init_bgp.BizUpdStatus_t.IsAlive)
			{
				init_bgp.BizUpdStatus_t.Abort();
			}
			init_bgp.BizUpdStatus_t = new System.Threading.Thread(bgp_BizUpdStatus);
			init_bgp.BizUpdStatus_t.Name = "init_bgp.BizUpdStatus_t";
			init_bgp.BizUpdStatus_t.IsBackground = true;
			init_bgp.BizUpdStatus_t.Start();

			// Other ....
		}

		private static void bgp_BizUpdStatus()
		{
			while (true)
			{
				// Process:
				try
				{
					bgp_BizUpdStatus_Process();
				}
				catch (Exception exc)
				{
					string strExc = string.Format("\r\nexc.Message = {0}\r\nexc.StackTrace = {1}", exc.Message, exc.StackTrace);
					System.Console.WriteLine(strExc);
				}
				if (init_bgp.BizUpdStatus_nForceProcess > 0) init_bgp.BizUpdStatus_nForceProcess--;

				// Sleep:
				for (int nSleep = 0; init_bgp.BizUpdStatus_nForceProcess <= 0 && nSleep < init_bgp.BizUpdStatus_nSleepMax; nSleep += init_bgp.BizUpdStatus_nSleepStep)
				{
					Thread.Sleep(init_bgp.BizUpdStatus_nSleepStep);
				}
			}
		}
		private static void bgp_BizUpdStatus_ForceProcess()
		{
			init_bgp.BizUpdStatus_nForceProcess++;
		}
		private static void bgp_BizUpdStatus_Process()
		{
			// Init:
			CConfig cf = init_cf.MyClone();

			//// Put your task here.

		}

		#endregion

		#region // Common:
		public static void Cm_Reinit()
		{
			init_bSuccess = false;
		}
		public string Cm_Test()
		{
			return DateTime.Now.ToString("yyyyMMdd.HHmmss.ffffff");
		}
		public DataSet Cm_GetId(
			string strTid
			)
		{
			// Init:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			CmUtils.CMyDataSet.SetRemark(ref mdsFinal, init_seq.GetSeqDateBased("{0:yyyyMMdd.HHmmss}.{1:000}", 1000));
			mdsFinal.AcceptChanges();

			// Return Good:
			return mdsFinal;
		}
		public DataSet Cm_GetDTime(
			string strTid
			)
		{
			// Init:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			CmUtils.CMyDataSet.SetRemark(ref mdsFinal, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
			mdsFinal.AcceptChanges();

			// Return Good:
			return mdsFinal;
		}
		public DataSet Cm_ExecSql(
			string strTid
			, DataRow drSession
			, string strBiz_SpecialPw
			, string strSql
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Cm_ExecSql";
			string strErrorCodeDefault = TError.ErrDemoLab.Cm_ExecSql;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "strSql", strSql
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
				//strTableName = TUtils.CUtils.StdParam(strTableName);

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = mySys_User_GetAbilityViewBankOfUser(_cf.sinf.strUserCode);

				// CmSys_InvalidBizSpecialPw:
				if (!CmUtils.StringUtils.StringEqual(_cf.nvcParams["Biz_SpecialPw"], strBiz_SpecialPw))
				{
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.CmSys_InvalidBizSpecialPw
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				// strSql:
				int nPosErr_ParamMissing = strSql.ToUpper().IndexOf("zzzzClause".ToUpper());
				if (nPosErr_ParamMissing >= 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.nPosErr_ParamMissing", nPosErr_ParamMissing
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Cm_ExecSql_ParamMissing
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				#endregion

				#region // Get Data:
				DataSet dsData = _cf.db.ExecQuery(strSql);
				for (int nIdxScan = 0; nIdxScan < dsData.Tables.Count; nIdxScan++)
				{
					dsData.Tables[nIdxScan].TableName = string.Format("MyTable{0}", nIdxScan);
				}
				CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dsData);
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
		public DataSet Mst_Common_Get(
			string strTid
			, DataRow drSession
			////
			, string strTableName
			, object objFilter0
			, object objFilter1
			, object objFilter2
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = false;
			string strFunctionName = "Mst_Common_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Common_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				, "strTableName", strTableName
				, "objFilter0", objFilter0
				, "objFilter1", objFilter1
				, "objFilter2", objFilter2
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

				//// Check Access/Deny:
				//Sys_Access_CheckDeny(
				//	ref alParamsCoupleError
				//	, strFunctionName
				//	);
				#endregion

				#region // Check:
				// Refine:
				strTableName = TUtils.CUtils.StdParam(strTableName);
				////

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = mySys_User_GetAbilityViewBankOfUser(_cf.sinf.strUserCode);

				#endregion

				#region // Get Data:
				////
				string strSqlGetData = "";
				string zzzzClauseTableCheck = "";
				////
				zzzzClauseTableCheck = "Ins_ClaimDocType";
				if (CmUtils.StringUtils.StringEqualIgnoreCase(strTableName, zzzzClauseTableCheck))
				{
					strSqlGetData = CmUtils.StringUtils.Replace(@"
							select
								*
							from zzzzClauseTableCheck t --//[mylock]
							where (1=1)
								--and t.ClmDocType = @objFilter0
                            order by t.Idx
							;
						"
						, "zzzzClauseTableCheck", zzzzClauseTableCheck
						);
				}
				////
				zzzzClauseTableCheck = "Ins_ContractType";
				if (CmUtils.StringUtils.StringEqualIgnoreCase(strTableName, zzzzClauseTableCheck))
				{
					strSqlGetData = CmUtils.StringUtils.Replace(@"
							select
								*
							from zzzzClauseTableCheck t --//[mylock]
							where (1=1)
								--and t.ContractType = @objFilter0
							;
						"
						, "zzzzClauseTableCheck", zzzzClauseTableCheck
						);
				}
				////
				zzzzClauseTableCheck = "Mst_Param";
				if (CmUtils.StringUtils.StringEqualIgnoreCase(strTableName, zzzzClauseTableCheck))
				{
					strSqlGetData = CmUtils.StringUtils.Replace(@"
							select
								*
							from zzzzClauseTableCheck t --//[mylock]
							where (1=1)
								--and t.ParamCode = @objFilter0
							;
						"
						, "zzzzClauseTableCheck", zzzzClauseTableCheck
						);
				}
				////
				zzzzClauseTableCheck = "Sys_ObjectType";
				if (CmUtils.StringUtils.StringEqualIgnoreCase(strTableName, zzzzClauseTableCheck))
				{
					strSqlGetData = CmUtils.StringUtils.Replace(@"
							select
								*
							from zzzzClauseTableCheck t --//[mylock]
							where (1=1)
								--and t.ObjectType = @objFilter0
							;
						"
						, "zzzzClauseTableCheck", zzzzClauseTableCheck
						);
				}
				////
				zzzzClauseTableCheck = "Sys_Service";
				if (CmUtils.StringUtils.StringEqualIgnoreCase(strTableName, zzzzClauseTableCheck))
				{
					strSqlGetData = CmUtils.StringUtils.Replace(@"
							select
								*
							from zzzzClauseTableCheck t --//[mylock]
							where (1=1)
								--and t.ServiceCode = @objFilter0
							;
						"
						, "zzzzClauseTableCheck", zzzzClauseTableCheck
						);
				}
				////
				if (strSqlGetData.Length <= 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.strTableName", strTableName
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_Common_Get_NotSupportTable
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				////
				DataTable dtGetData = _cf.db.ExecQuery(
					strSqlGetData
					, "@objFilter0", TUtils.CUtils.IsNullSql(objFilter0)
					, "@objFilter1", TUtils.CUtils.IsNullSql(objFilter1)
					, "@objFilter2", TUtils.CUtils.IsNullSql(objFilter2)
					).Tables[0];
				dtGetData.TableName = zzzzClauseTableCheck;
				CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dtGetData);
				////
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

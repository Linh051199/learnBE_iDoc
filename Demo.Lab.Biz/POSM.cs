using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using System.Net;
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
		#region // Mst_POSMType:
		public void Mst_POSMType_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objPOSMType
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_POSMType
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_POSMType t --//[mylock]
					where (1=1)
						and t.POSMType = @objPOSMType
					;
				");
			dtDB_Mst_POSMType = _cf.db.ExecQuery(
				strSqlExec
				, "@objPOSMType", objPOSMType
				).Tables[0];
			dtDB_Mst_POSMType.TableName = "Mst_POSMType";

			// strFlagExistToCheck
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_POSMType.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.POSMType", objPOSMType
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_POSMType_CheckDB_POSMTypeNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_POSMType.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.POSMType", objPOSMType
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_POSMType_CheckDB_POSMTypeExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_POSMType.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.POSMType", objPOSMType
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_POSMType.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_POSMType_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_POSMType_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return: 
			, string strRt_Cols_Mst_POSMType
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSMType_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSMType_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_POSMType", strRt_Cols_Mst_POSMType
					});
			#endregion

			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

				// Write RequestLog:
				_cf.ProcessBizReq(
					strTid //strTid
					, strFunctionName //strFunctionName
					, alParamsCoupleError
					);

				// Check Access/Deny:
				Sys_Access_CheckDeny(
					ref alParamsCoupleError
					, strFunctionName
					);
				#endregion

				#region // Check:
				//// Refine:
				long nFilterRecordStart = Convert.ToInt64(strFt_RecordStart);
				long nFilterRecordEnd = nFilterRecordStart + Convert.ToInt64(strFt_RecordCount) - 1;
				bool bGet_Mst_POSMType = (strRt_Cols_Mst_POSMType != null && strRt_Cols_Mst_POSMType.Length > 0);

				//// drAbitiltyOfUser
				//DataRow drAbilityOfUser = myCache_ViewAbility_GetUserInfo(_cf.sinf.strUserCode);

				#endregion

				#region // Build Sql:
				////
				ArrayList alParamsCoupleSql = new ArrayList();
				alParamsCoupleSql.AddRange(new object[]{
					"@nFilterRecordStart", nFilterRecordStart
					, "@nFilterRecordEnd", nFilterRecordEnd
					});
				////
				//myCache_ViewAbility_GetDealerInfo(drAbitiltyOfUser)
				////
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Mst_POSMType_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mpt.POSMType
						into #tbl_Mst_POSMType_Filter_Draft
						from Mst_POSMType mpt --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mpt.POSMType asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_POSMType_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_POSMType_Filter:
						select
							t.*
						into #tbl_Mst_POSMType_Filter
						from #tbl_Mst_POSMType_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_POSMType --------:
						zzB_Select_Mst_POSMType_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_POSMType_Filter_Draft;
						--drop table #tbl_Mst_POSMType_Filter;
					"
					);
				////
				string zzB_Select_Mst_POSMType_zzE = "-- Nothing.";
				if (bGet_Mst_POSMType)
				{
					#region // bGet_Mst_POSMType:
					zzB_Select_Mst_POSMType_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_POSMType:
							select
								t.MyIdxSeq
								, mpt.*
							from #tbl_Mst_POSMType_Filter t --//[mylock]
								inner join Mst_POSMType mpt --//[mylock]
									on t.POSMType = mpt.POSMType
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzB_Where_strFilter_zzE = "";
				{
					Hashtable htSpCols = new Hashtable();
					{
						#region // htSpCols:
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_POSMType" // strTableNameDB
							, "Mst_POSMType." // strPrefixStd
							, "mpt." // strPrefixAlias
							);
						////
						#endregion
					}
					zzB_Where_strFilter_zzE = CmUtils.SqlUtils.BuildWhere(
						htSpCols
						, strFt_WhereClause // strClause
						, "@p_" // strParamPrefix
						, ref alParamsCoupleSql //alParamsCoupleSql
						);
					zzB_Where_strFilter_zzE = (zzB_Where_strFilter_zzE.Length < 0 ? "" : string.Format(" and ({0})", zzB_Where_strFilter_zzE));
					alParamsCoupleError.AddRange(new object[]{
						"zzB_Where_strFilter_zzE", zzB_Where_strFilter_zzE
						});
				}
				////
				strSqlGetData = CmUtils.StringUtils.Replace(
					strSqlGetData
					, "zzB_Where_strFilter_zzE", zzB_Where_strFilter_zzE
					, "zzB_Select_Mst_POSMType_zzE", zzB_Select_Mst_POSMType_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_POSMType)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_POSMType";
				}
				CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dsGetData);
				#endregion

				// Return Good:
				TDALUtils.DBUtils.RollbackSafety(_cf.db); // Always Rollback.
				mdsFinal.AcceptChanges();
				return mdsFinal;
			}
			catch (Exception exc)
			{
				#region // Catch of try:
				// RollBack:
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
				#region // Finnaly of try:
				// Rollback and release resources:
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
		public DataSet Mst_POSMType_Create(
			string strTid
			, DataRow drSession
			////
			, object objPOSMType
			, object objPOSMTypeName
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			// init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSMType_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSMType_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objPOSMType", objPOSMType
					, "objPOSMTypeName", objPOSMTypeName
					});
			#endregion

			try
			{
				#region // Convert Input:

				#endregion

				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

				// Write RequestLog:
				_cf.ProcessBizReq(
					strTid // strTid
					, strFunctionName // strFunctionName
					, alParamsCoupleError // alParmasCoupleError
					);

				// Check Access/Deny
				Sys_Access_CheckDeny(
					ref alParamsCoupleError
					, strFunctionName
					);
				#endregion

				#region // Refine and Check Input:
				////
				string strPOSMType = TUtils.CUtils.StdParam(objPOSMType);
				string strPOSMTypeName = string.Format("{0}", objPOSMTypeName).Trim();

				// drAbilityOfUser
				//DataRow drAbilityOfUser =  Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_POSMType = null;
				{
					////
					if (strPOSMType == null || strPOSMType.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strPOSMType", strPOSMType
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_POSMType_Create_InvalidPOSMType
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_POSMType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strPOSMType // objPOSMType
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_POSMType // dtDB_Mst_POSMType
						);
					////
					if (strPOSMTypeName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strPOSMTypeName", strPOSMTypeName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_POSMType_Create_InvalidPOSMTypeName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB MST_POSMType:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_POSMType.NewRow();
					strFN = "POSMType"; drDB[strFN] = strPOSMType;
					strFN = "POSMTypeName"; drDB[strFN] = strPOSMTypeName;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_POSMType.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_POSMType"
						, dtDB_Mst_POSMType
						// , alColumnEffective.ToArray()
						);
				}

				#endregion

				// Return Good:
				TDALUtils.DBUtils.CommitSafety(_cf.db);
				mdsFinal.AcceptChanges();
				return mdsFinal;
			}
			catch (Exception exc)
			{
				#region // Catch of try
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
				#region // Finally of try
				// Rollback and Release recource:
				TDALUtils.DBUtils.RollbackSafety(_cf.db);
				TDALUtils.DBUtils.ReleaseAllSemaphore(_cf.db_Sys, true);

				// Write ReturnLog:
				_cf.ProcessBizReturn(
					ref mdsFinal // mdsFinal
					, strTid // strTid
					, strFunctionName //strFunctionName
					);
				#endregion
			}
		}
		public DataSet Mst_POSMType_Update(
			string strTid
			, DataRow drSession
			////
			, object objPOSMType
			, object objPOSMTypeName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			// init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSMType_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSMType_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objPOSMType", objPOSMType
					, "objPOSMTypeName", objPOSMTypeName
					, "objFlagActive", objFlagActive
                    ////
					, "objFt_Cols_Upd", objFt_Cols_Upd
					});
			#endregion

			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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

				#region // Refine and Check Input:
				////
				string strFt_Cols_Upd = TUtils.CUtils.StdParam(objFt_Cols_Upd);
				strFt_Cols_Upd = (strFt_Cols_Upd == null ? "" : strFt_Cols_Upd);
				////
				string strPOSMType = TUtils.CUtils.StdParam(objPOSMType);
				string strPOSMTypeName = TUtils.CUtils.StdParam(objPOSMTypeName);
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_POSMTypeName = strFt_Cols_Upd.Contains("Mst_POSMType.POSMTypeName".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_POSMType.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_POSMType = null;
				{
					////
					Mst_POSMType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strPOSMType // objPOSMType 
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_POSMType // dtDB_Mst_POSMType
						);
					////
					if (bUpd_POSMTypeName && string.IsNullOrEmpty(strPOSMTypeName))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strPOSMTypeName", strPOSMTypeName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_POSMType_Update_InvalidPOSMTypeName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // Save Mst_POSMType
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_POSMType.Rows[0];
					if (bUpd_POSMTypeName) { strFN = "POSMTypeName"; drDB[strFN] = strPOSMTypeName; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_POSMType"
						, dtDB_Mst_POSMType
						, alColumnEffective.ToArray()
						);
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
		public DataSet Mst_POSMType_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objPOSMType
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSMType_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSMType_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objPOSMType", objPOSMType
					});
			#endregion

			try
			{

				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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

				#region // Refine and Check Input:
				////
				string strPOSMType = TUtils.CUtils.StdParam(objPOSMType);
				////
				DataTable dtDB_Mst_POSMType = null;
				{
					////
					Mst_POSMType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strPOSMType // objPOSMType
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_POSMType // dtDB_Mst_POSMType
						);
				}
				#endregion

				#region // SaveDB Mst_POSMType:
				{
					// Init:
					dtDB_Mst_POSMType.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_POSMType"
						, dtDB_Mst_POSMType
						);
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
		#endregion

		#region // Mst_POSMUnitType:
		public void Mst_POSMUnitType_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objPOSMUnitType
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_POSMUnitType
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_POSMUnitType t --//[mylock]
					where (1=1)
						and t.POSMUnitType = @objPOSMUnitType
					;
				");
			dtDB_Mst_POSMUnitType = _cf.db.ExecQuery(
				strSqlExec
				, "@objPOSMUnitType", objPOSMUnitType
				).Tables[0];
			dtDB_Mst_POSMUnitType.TableName = "Mst_POSMUnitType";

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_POSMUnitType.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.POSMUnitType", objPOSMUnitType
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_POSMUnitType_CheckDB_POSMUnitTypeNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_POSMUnitType.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.POSMUnitType", objPOSMUnitType
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_POSMUnitType_CheckDB_POSMUnitTypeExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck:
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_POSMUnitType.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.POSMUnitType", objPOSMUnitType
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_POSMUnitType.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_POSMUnitType_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}

		}
		public DataSet Mst_POSMUnitType_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_POSMUnitType
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSMUnitType_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSMUnitType_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_POSMUnitType", strRt_Cols_Mst_POSMUnitType
					});
			#endregion
			
			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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
				//// Refine:
				long nFilterRecordStart = Convert.ToInt64(strFt_RecordStart);
				long nFilterRecordEnd = nFilterRecordStart + Convert.ToInt64(strFt_RecordCount) - 1;
				bool bGet_Mst_POSMUnitType = (strRt_Cols_Mst_POSMUnitType != null && strRt_Cols_Mst_POSMUnitType.Length > 0);

				//// drAbilityOfUser:
				//DataRow drAbilityOfUser = myCache_ViewAbility_GetUserInfo(_cf.sinf.strUserCode);

				#endregion

				#region // Build Sql:
				////
				ArrayList alParamsCoupleSql = new ArrayList();
				alParamsCoupleSql.AddRange(new object[] {
					"@nFilterRecordStart", nFilterRecordStart
					, "@nFilterRecordEnd", nFilterRecordEnd
					});
				////
				//myCache_ViewAbility_GetDealerInfo(drAbilityOfUser);
				////
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Mst_POSMUnitType_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mput.POSMUnitType
						into #tbl_Mst_POSMUnitType_Filter_Draft
						from Mst_POSMUnitType mput --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mput.POSMUnitType asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_POSMUnitType_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_POSMUnitType_Filter:
						select
							t.*
						into #tbl_Mst_POSMUnitType_Filter
						from #tbl_Mst_POSMUnitType_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_POSMUnitType --------:
						zzB_Select_Mst_POSMUnitType_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_POSMUnitType_Filter_Draft;
						--drop table #tbl_Mst_POSMUnitType_Filter;
					"
					);
				////
				string zzB_Select_Mst_POSMUnitType_zzE = "-- Nothing.";
				if (bGet_Mst_POSMUnitType)
				{
					#region // bGet_Mst_POSMUnitType:
					zzB_Select_Mst_POSMUnitType_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_POSMUnitType:
							select
								t.MyIdxSeq
								, mput.*
							from #tbl_Mst_POSMUnitType_Filter t --//[mylock]
								inner join Mst_POSMUnitType mput --//[mylock]
									on t.POSMUnitType = mput.POSMUnitType
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzB_Where_strFilter_zzE = "";
				{
					Hashtable htSpCols = new Hashtable();
					{
						#region // htSpCols:
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_POSMUnitType" // strTableNameDB
							, "Mst_POSMUnitType." // strPrefixStd
							, "mput." // strPrefixAlias
							);
						////
						#endregion
					}
					zzB_Where_strFilter_zzE = CmUtils.SqlUtils.BuildWhere(
						htSpCols // htSpCols
						, strFt_WhereClause // strClause
						, "@p_" // strParamPrefix
						, ref alParamsCoupleSql // alParamsCoupleSql
						);
					zzB_Where_strFilter_zzE = (zzB_Where_strFilter_zzE.Length <= 0 ? "" : string.Format(" and ({0})", zzB_Where_strFilter_zzE));
					alParamsCoupleError.AddRange(new object[]{
						"zzB_Where_strFilter_zzE", zzB_Where_strFilter_zzE
						});
				}
				////
				strSqlGetData = CmUtils.StringUtils.Replace(
					strSqlGetData
					, "zzB_Where_strFilter_zzE", zzB_Where_strFilter_zzE
					, "zzB_Select_Mst_POSMUnitType_zzE", zzB_Select_Mst_POSMUnitType_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_POSMUnitType)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_POSMUnitType";
				}
				CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dsGetData);
				#endregion

				// Return Good:
				TDALUtils.DBUtils.RollbackSafety(_cf.db); // Always Rollback.
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
		public DataSet Mst_POSMUnitType_Create(
			string strTid
			, DataRow drSession
			////
			, object objPOSMUnitType
			, object objPOSMUnitTypeName
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSMUnitType_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSMUnitType_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
					, "objPOSMUnitType", objPOSMUnitType
					, "objPOSMUnitTypeName", objPOSMUnitTypeName
					});
			#endregion

			try
			{
				#region // Convert Input:
				#endregion

				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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

				#region // Refine and Check Input:
				////
				string strPOSMUnitType = TUtils.CUtils.StdParam(objPOSMUnitType);
				string strPOSMUnitTypeName = string.Format("{0}", objPOSMUnitTypeName).Trim();

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_POSMUnitType = null;
				{
					////
					if (strPOSMUnitType == null || strPOSMUnitType.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strPOSMUnitType", strPOSMUnitType
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_POSMUnitType_Create_InvalidPOSMUnitType
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_POSMUnitType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strPOSMUnitType // objPOSMUnitType
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_POSMUnitType // dtDB_Mst_POSMUnitType
						);
					////
					if (strPOSMUnitTypeName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strPOSMUnitTypeName", strPOSMUnitTypeName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_POSMUnitType_Create_InvalidPOSMUnitTypeName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_POSMUnitType:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_POSMUnitType.NewRow();
					strFN = "POSMUnitType"; drDB[strFN] = strPOSMUnitType;
					strFN = "POSMUnitTypeName"; drDB[strFN] = strPOSMUnitTypeName;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_POSMUnitType.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_POSMUnitType"
						, dtDB_Mst_POSMUnitType
						//, alColumnEffective.ToArray()
						);
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
				#region // Finally of try
				// Rollback and Release recource:
				TDALUtils.DBUtils.RollbackSafety(_cf.db);
				TDALUtils.DBUtils.ReleaseAllSemaphore(_cf.db_Sys, true);

				// Write ReturnLog:
				_cf.ProcessBizReturn(
					ref mdsFinal // mdsFinal
					, strTid // strTid
					, strFunctionName //strFunctionName
					);
				#endregion
			}
		}
		public DataSet Mst_POSMUnitType_Update(
			string strTid
			, DataRow drSession
			////
			, object objPOSMUnitType
			, object objPOSMUnitTypeName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSMUnitType_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSMUnitType_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objPOSMUnitType", objPOSMUnitType
					, "objPOSMUnitTypeName", objPOSMUnitTypeName
					, "objFlagActive", objFlagActive
                    ////
					, "objFt_Cols_Upd", objFt_Cols_Upd
					});
			#endregion

			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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

				#region // Refine and Check Input:
				////
				string strFt_Cols_Upd = TUtils.CUtils.StdParam(objFt_Cols_Upd);
				strFt_Cols_Upd = (strFt_Cols_Upd == null ? "" : strFt_Cols_Upd);
				////
				string strPOSMUnitType = TUtils.CUtils.StdParam(objPOSMUnitType);
				string strPOSMUnitTypeName = TUtils.CUtils.StdParam(objPOSMUnitTypeName);
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_POSMUnitTypeName = strFt_Cols_Upd.Contains("Mst_POSMUnitType.POSMUnitTypeName".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_POSMUnitType.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_POSMUnitType = null;
				{
					////
					Mst_POSMUnitType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strPOSMUnitType // objPOSMUnitType 
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_POSMUnitType // dtDB_Mst_POSMUnitType
						);
					////
					if (bUpd_POSMUnitTypeName && string.IsNullOrEmpty(strPOSMUnitTypeName))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strPOSMUnitTypeName", strPOSMUnitTypeName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_POSMUnitType_Update_InvalidPOSMUnitTypeName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // Save Mst_POSMUnitType:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_POSMUnitType.Rows[0];
					if (bUpd_POSMUnitTypeName) { strFN = "POSMUnitTypeName"; drDB[strFN] = strPOSMUnitTypeName; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_POSMUnitType"
						, dtDB_Mst_POSMUnitType
						, alColumnEffective.ToArray()
						);
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
		public DataSet Mst_POSMUnitType_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objPOSMUnitType
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSMUnitType_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSMUnitType_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
						"strFunctionName", strFunctionName
						, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
						////
						, "objPOSMUnitType", objPOSMUnitType
						});
			#endregion

			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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

				#region // Refine and Check Input:
				////
				string strPOSMUnitType = TUtils.CUtils.StdParam(objPOSMUnitType);
				////
				DataTable dtDB_Mst_POSMUnitType = null;
				{
					////
					Mst_POSMUnitType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strPOSMUnitType // objPOSMUnitType
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_POSMUnitType // dtDB_Mst_POSMUnitType
						);
				}
				#endregion

				#region // SaveDB Mst_POSMUnitType:
				{
					// Init:
					dtDB_Mst_POSMUnitType.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_POSMUnitType"
						, dtDB_Mst_POSMUnitType
						);
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
		#endregion

		#region // Mst_POSM:
		public void Mst_POSM_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objPOSMCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_POSM
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_POSM t --//[mylock]
					where (1=1)
						and t.POSMCode = @objPOSMCode
					;
				");
			dtDB_Mst_POSM = _cf.db.ExecQuery(
				strSqlExec
				, "@objPOSMCode", objPOSMCode
				).Tables[0];
			dtDB_Mst_POSM.TableName = "Mst_POSM";

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_POSM.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.POSMCode", objPOSMCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_POSM_CheckDB_POSMNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_POSM.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.POSMCode", objPOSMCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_POSM_CheckDB_POSMExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck:
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_POSM.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.POSMCode", objPOSMCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_POSM.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_POSM_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_POSM_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_POSM
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSM_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSM_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_POSM", strRt_Cols_Mst_POSM
					});
			#endregion

			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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
				//// Refine:
				long nFilterRecordStart = Convert.ToInt64(strFt_RecordStart);
				long nFilterRecordEnd = nFilterRecordStart + Convert.ToInt64(strFt_RecordCount) - 1;
				bool bGet_Mst_POSM = (strRt_Cols_Mst_POSM != null && strRt_Cols_Mst_POSM.Length > 0);

				//// drAbilityOfUser:
				//DataRow drAbilityOfUser = myCache_ViewAbility_GetUserInfo(_cf.sinf.strUserCode);

				#endregion

				#region // Build Sql:
				////
				ArrayList alParamsCoupleSql = new ArrayList();
				alParamsCoupleSql.AddRange(new object[] {
					"@nFilterRecordStart", nFilterRecordStart
					, "@nFilterRecordEnd", nFilterRecordEnd
					});
				////
				//myCache_ViewAbility_GetDealerInfo(drAbilityOfUser);
				////
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Mst_POSM_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mposm.POSMCode
						into #tbl_Mst_POSM_Filter_Draft
						from Mst_POSM mposm --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mposm.POSMCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_POSM_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_POSM_Filter:
						select
							t.*
						into #tbl_Mst_POSM_Filter
						from #tbl_Mst_POSM_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_POSM --------:
						zzB_Select_Mst_POSM_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_POSM_Filter_Draft;
						--drop table #tbl_Mst_POSM_Filter;
					"
					);
				////
				string zzB_Select_Mst_POSM_zzE = "-- Nothing.";
				if (bGet_Mst_POSM)
				{
					#region // bGet_Mst_POSM:
					zzB_Select_Mst_POSM_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_POSM:
							select
								t.MyIdxSeq
								, mposm.*
							    , mpt.POSMType
								, mput.POSMUnitType
							from #tbl_Mst_POSM_Filter t --//[mylock]
								inner join Mst_POSM mposm --//[mylock]
									on t.POSMCode = mposm.POSMCode
							    left join Mst_POSMType mpt --//[mylock]
							        on mposm.POSMType = mpt.POSMType
							    left join Mst_POSMUnitType mput --//[mylock]
							        on mposm.POSMUnitType = mput.POSMUnitType
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzB_Where_strFilter_zzE = "";
				{
					Hashtable htSpCols = new Hashtable();
					{
						#region // htSpCols:
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_POSM" // strTableNameDB
							, "Mst_POSM." // strPrefixStd
							, "mposm." // strPrefixAlias
							);
						////
						#endregion
					}
					zzB_Where_strFilter_zzE = CmUtils.SqlUtils.BuildWhere(
						htSpCols // htSpCols
						, strFt_WhereClause // strClause
						, "@p_" // strParamPrefix
						, ref alParamsCoupleSql // alParamsCoupleSql
						);
					zzB_Where_strFilter_zzE = (zzB_Where_strFilter_zzE.Length <= 0 ? "" : string.Format(" and ({0})", zzB_Where_strFilter_zzE));
					alParamsCoupleError.AddRange(new object[]{
						"zzB_Where_strFilter_zzE", zzB_Where_strFilter_zzE
						});
				}
				////
				strSqlGetData = CmUtils.StringUtils.Replace(
					strSqlGetData
					, "zzB_Where_strFilter_zzE", zzB_Where_strFilter_zzE
					, "zzB_Select_Mst_POSM_zzE", zzB_Select_Mst_POSM_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_POSM)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_POSM";
				}
				CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dsGetData);
				#endregion

				// Return Good:
				TDALUtils.DBUtils.RollbackSafety(_cf.db); // Always Rollback.
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
		public DataSet Mst_POSM_Create(
			string strTid
			, DataRow drSession
			////
			, object objPOSMCode
			, object objPOSMType
			, object objPOSMUnitType
			, object objPOSMName
			, object objPOSMDesc
			, object objPOSMImageFilePath
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSM_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSM_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objPOSMCode", objPOSMCode
					, "objPOSMType", objPOSMType
					, "objPOSMUnitType", objPOSMUnitType
					, "objPOSMName", objPOSMName
					, "objPOSMDesc", objPOSMDesc
					, "objPOSMImageFilePath", objPOSMImageFilePath
					});
			#endregion

			try
			{
				#region // Convert Input:
				#endregion

				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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

				#region // Refine and Check Input:
				////
				string strPOSMCode = TUtils.CUtils.StdParam(objPOSMCode);
				string strPOSMType = TUtils.CUtils.StdParam(objPOSMType);
				string strPOSMUnitType = TUtils.CUtils.StdParam(objPOSMUnitType);
				string strPOSMName = string.Format("{0}", objPOSMName).Trim();
				string strPOSMDesc = string.Format("{0}", objPOSMDesc).Trim();
				string strPOSMImageFilePath = string.Format("{0}", objPOSMImageFilePath).Trim();

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_POSM = null;
				DataTable dtDB_Mst_POSMType = null;
				DataTable dtDB_Mst_POSMUnitType = null;
				{

					////
					if (strPOSMCode == null || strPOSMCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strPOSMCode", strPOSMCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_POSM_Create_InvalidPOSMCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_POSM_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objPOSMCode // objPOSMCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_POSM // dtDB_Mst_POSM
						);
					////
					Mst_POSMType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strPOSMType // strPOSMType
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.Flag.Active // strFlagActiveListToCheck
						, out dtDB_Mst_POSMType // dtDB_Mst_POSMType
						);
					// //
					Mst_POSMUnitType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strPOSMUnitType // strPOSMUnitType
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.Flag.Active // strFlagActiveListToCheck
						, out dtDB_Mst_POSMUnitType // dtDB_Mst_POSMUnitType
						);

					////
					if (strPOSMName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strPOSMName", strPOSMName
							 });
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_POSM_Create_InvalidPOSMName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_POSM:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_POSM.NewRow();
					strFN = "POSMCode"; drDB[strFN] = strPOSMCode;
					strFN = "POSMType"; drDB[strFN] = strPOSMType;
					strFN = "POSMUnitType"; drDB[strFN] = strPOSMUnitType;
					strFN = "POSMName"; drDB[strFN] = strPOSMName;
					strFN = "POSMDesc"; drDB[strFN] = strPOSMDesc;
					strFN = "POSMImageFilePath"; drDB[strFN] = strPOSMImageFilePath;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_POSM.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_POSM"
						, dtDB_Mst_POSM
						//, alColumnEffective.ToArray()
						);
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
		public DataSet Mst_POSM_Update(
			string strTid
			, DataRow drSession
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
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSM_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSM_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objPOSMCode", objPOSMCode
					, "objPOSMType", objPOSMType
					, "objPOSMUnitType", objPOSMUnitType
					, "objPOSMName", objPOSMName
					, "objPOSMDesc", objPOSMDesc
					, "objPOSMImageFilePath", objPOSMImageFilePath
					, "objFlagActive", objFlagActive
					, "objFt_Cols_Upd", objFt_Cols_Upd
					});
			#endregion

			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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

				#region // Refine and Check Input:
				////
				string strFt_Cols_Upd = TUtils.CUtils.StdParam(objFt_Cols_Upd);
				strFt_Cols_Upd = (strFt_Cols_Upd == null ? "" : strFt_Cols_Upd);
				////
				string strPOSMCode = TUtils.CUtils.StdParam(objPOSMCode);
				string strPOSMType = TUtils.CUtils.StdParam(objPOSMType);
				string strPOSMUnitType = TUtils.CUtils.StdParam(objPOSMUnitType);
				string strPOSMName = string.Format("{0}", objPOSMName).Trim();
				string strPOSMDesc = string.Format("{0}", objPOSMDesc).Trim();
				string strPOSMImageFilePath = string.Format("{0}", objPOSMImageFilePath).Trim();
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_POSMName = strFt_Cols_Upd.Contains("Mst_POSM.POSMName".ToUpper());
				bool bUpd_POSMDesc = strFt_Cols_Upd.Contains("Mst_POSM.POSMDesc".ToUpper());
				bool bUpd_POSMImageFilePath = strFt_Cols_Upd.Contains("Mst_POSM.POSMImageFilePath".ToUpper());
				bool bUpd_POSMType = strFt_Cols_Upd.Contains("Mst_POSM.POSMType".ToUpper());
				bool bUpd_POSMUnitType = strFt_Cols_Upd.Contains("Mst_POSM.POSMUnitType".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_POSM.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_POSM = null;
				DataTable dtDB_Mst_POSMType = null;
				DataTable dtDB_Mst_POSMUnitType = null;
				{
					////
					Mst_POSM_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strPOSMCode // objPOSMCode 
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_POSM // dtDB_Mst_POSM
						);
					////
					if (bUpd_POSMName && string.IsNullOrEmpty(strPOSMName))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strPOSMName", strPOSMName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_POSM_Update_InvalidPOSMName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (bUpd_POSMType)
					{
						Mst_POSMType_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, strPOSMType // objPOSMType
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_POSMType // dtDB_Mst_POSMType
							);
					}
					if (bUpd_POSMUnitType)
					{
						Mst_POSMUnitType_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, strPOSMUnitType // objPOSMUnitType
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_POSMUnitType // dtDB_Mst_POSMUnitType
							);
					}
				}
				#endregion

				#region // Save Mst_POSM:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_POSM.Rows[0];
					if (bUpd_POSMType) { strFN = "POSMType"; drDB[strFN] = strPOSMType; alColumnEffective.Add(strFN); }
					if (bUpd_POSMUnitType) { strFN = "POSMUnitType"; drDB[strFN] = strPOSMUnitType; alColumnEffective.Add(strFN); }
					if (bUpd_POSMName) { strFN = "POSMName"; drDB[strFN] = strPOSMName; alColumnEffective.Add(strFN); }
					if (bUpd_POSMDesc) { strFN = "POSMDesc"; drDB[strFN] = strPOSMDesc; alColumnEffective.Add(strFN); }
					if (bUpd_POSMImageFilePath) { strFN = "POSMImageFilePath"; drDB[strFN] = strPOSMImageFilePath; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_POSM"
						, dtDB_Mst_POSM
						, alColumnEffective.ToArray()
						);
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
		public DataSet Mst_POSM_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objPOSMCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_POSM_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_POSM_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objPOSMCode", objPOSMCode
					});
			#endregion

			try
			{
				#region // Init:
				_cf.db.LogUserId = _cf.sinf.strUserCode;
				_cf.db.BeginTransaction();

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

				#region // Refine and Check Input:
				////
				string strPOSMCode = TUtils.CUtils.StdParam(objPOSMCode);
				////
				DataTable dtDB_Mst_POSM = null;
				{
					////
					Mst_POSM_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objPOSMCode // objPOSMCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_POSM // dtDB_Mst_POSM
						);
				}
				#endregion

				#region // SaveDB Mst_POSM:
				{
					// Init:
					dtDB_Mst_POSM.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_POSM"
						, dtDB_Mst_POSM
						);
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
		#endregion
	}
}

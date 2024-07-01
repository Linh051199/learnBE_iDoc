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
		#region // Mst_StarShopGroup:
		public void Mst_StarShopGroup_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objSSGrpCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_StarShopGroup
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_StarShopGroup t --//[mylock]
					where (1=1)
						and t.SSGrpCode = @objSSGrpCode
					;
				");
			dtDB_Mst_StarShopGroup = _cf.db.ExecQuery(
				strSqlExec
				, "@objSSGrpCode", objSSGrpCode
				).Tables[0];
			dtDB_Mst_StarShopGroup.TableName = "Mst_StarShopGroup";

			// strFlagExistToCheck
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_StarShopGroup.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.SSGrpCode", objSSGrpCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_StarShopGroup_CheckDB_StarShopGroupNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_StarShopGroup.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.SSGrpCode", objSSGrpCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_StarShopGroup_CheckDB_StarShopGroupExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_StarShopGroup.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.SSGrpCode", objSSGrpCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_StarShopGroup.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_StarShopGroup_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_StarShopGroup_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return: 
			, string strRt_Cols_Mst_StarShopGroup
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopGroup_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopGroup_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_StarShopGroup", strRt_Cols_Mst_StarShopGroup
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
				bool bGet_Mst_StarShopGroup = (strRt_Cols_Mst_StarShopGroup != null && strRt_Cols_Mst_StarShopGroup.Length > 0);

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
						---- #tbl_Mst_StarShopGroup_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mssg.SSGrpCode
						into #tbl_Mst_StarShopGroup_Filter_Draft
						from Mst_StarShopGroup mssg --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mssg.SSGrpCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_StarShopGroup_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_StarShopGroup_Filter:
						select
							t.*
						into #tbl_Mst_StarShopGroup_Filter
						from #tbl_Mst_StarShopGroup_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_StarShopGroup --------:
						zzB_Select_Mst_StarShopGroup_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_StarShopGroup_Filter_Draft;
						--drop table #tbl_Mst_StarShopGroup_Filter;
					"
					);
				////
				string zzB_Select_Mst_StarShopGroup_zzE = "-- Nothing.";
				if (bGet_Mst_StarShopGroup)
				{
					#region // bGet_Mst_StarShopGroup:
					zzB_Select_Mst_StarShopGroup_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_StarShopGroup:
							select
								t.MyIdxSeq
								, mssg.*
							from #tbl_Mst_StarShopGroup_Filter t --//[mylock]
								inner join Mst_StarShopGroup mssg --//[mylock]
									on t.SSGrpCode = mssg.SSGrpCode
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
							, "Mst_StarShopGroup" // strTableNameDB
							, "Mst_StarShopGroup." // strPrefixStd
							, "mssg." // strPrefixAlias
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
					, "zzB_Select_Mst_StarShopGroup_zzE", zzB_Select_Mst_StarShopGroup_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_StarShopGroup)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_StarShopGroup";
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
		public DataSet Mst_StarShopGroup_Create(
			string strTid
			, DataRow drSession
			////
			, object objSSGrpCode
			, object objSSGrpName
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			// init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopGroup_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopGroup_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objSSGrpCode", objSSGrpCode
					, "objSSGrpName", objSSGrpName
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
				string strSSGrpCode = TUtils.CUtils.StdParam(objSSGrpCode);
				string strSSGrpName = string.Format("{0}", objSSGrpName).Trim();

				// drAbilityOfUser
				//DataRow drAbilityOfUser =  Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_StarShopGroup = null;
				{
					////
					if (strSSGrpCode == null || strSSGrpCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSGrpCode", strSSGrpCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopGroup_Create_InvalidSSGrpCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_StarShopGroup_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strSSGrpCode // objSSGrpCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopGroup // dtDB_Mst_StarShopGroup
						);
					////
					if (strSSGrpName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSGrpName", strSSGrpName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopGroup_Create_InvalidSSGrpName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_StarShopGroup:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_StarShopGroup.NewRow();
					strFN = "SSGrpCode"; drDB[strFN] = strSSGrpCode;
					strFN = "SSGrpName"; drDB[strFN] = strSSGrpName;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_StarShopGroup.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_StarShopGroup"
						, dtDB_Mst_StarShopGroup
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
		public DataSet Mst_StarShopGroup_Update(
			string strTid
			, DataRow drSession
			////
			, object objSSGrpCode
			, object objSSGrpName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			// init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopGroup_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopGroup_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objSSGrpCode", objSSGrpCode
					, "objSSGrpName", objSSGrpName
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
				string strSSGrpCode = TUtils.CUtils.StdParam(objSSGrpCode);
				string strSSGrpName = string.Format("{0}", objSSGrpName).Trim();
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_SSGrpName = strFt_Cols_Upd.Contains("Mst_StarShopGroup.SSGrpName".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_StarShopGroup.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_StarShopGroup = null;
				{
					////
					Mst_StarShopGroup_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strSSGrpCode // objSSGrpCode 
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopGroup // dtDB_Mst_StarShopGroup
						);
					////
					if (bUpd_SSGrpName && string.IsNullOrEmpty(strSSGrpName))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSGrpName", strSSGrpName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopGroup_Update_InvalidSSGrpName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // Save Mst_StarShopGroup:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_StarShopGroup.Rows[0];
					if (bUpd_SSGrpName) { strFN = "SSGrpName"; drDB[strFN] = strSSGrpName; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_StarShopGroup"
						, dtDB_Mst_StarShopGroup
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
		public DataSet Mst_StarShopGroup_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objSSGrpCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopGroup_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopGroup_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objSSGrpCode", objSSGrpCode
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
				string strSSGrpCode = TUtils.CUtils.StdParam(objSSGrpCode);
				////
				DataTable dtDB_Mst_StarShopGroup = null;
				{
					////
					Mst_StarShopGroup_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strSSGrpCode // objSSGrpCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopGroup // dtDB_Mst_StarShopGroup
						);
				}
				#endregion

				#region // SaveDB Mst_StarShopGroup:
				{
					// Init:
					dtDB_Mst_StarShopGroup.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_StarShopGroup"
						, dtDB_Mst_StarShopGroup
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

		#region // Mst_StarShopBrand:
		public void Mst_StarShopBrand_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objSSBrandCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_StarShopBrand
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_StarShopBrand t --//[mylock]
					where (1=1)
						and t.SSBrandCode = @objSSBrandCode
					;
				");
			dtDB_Mst_StarShopBrand = _cf.db.ExecQuery(
				strSqlExec
				, "@objSSBrandCode", objSSBrandCode
				).Tables[0];
			dtDB_Mst_StarShopBrand.TableName = "Mst_StarShopBrand";

			// strFlagExistToCheck
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_StarShopBrand.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.SSBrandCode", objSSBrandCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_StarShopBrand_CheckDB_StarShopBrandNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_StarShopBrand.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.SSBrandCode", objSSBrandCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_StarShopBrand_CheckDB_StarShopBrandExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_StarShopBrand.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.SSBrandCode", objSSBrandCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_StarShopBrand.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_StarShopBrand_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_StarShopBrand_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return: 
			, string strRt_Cols_Mst_StarShopBrand
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopBrand_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopBrand_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_StarShopBrand", strRt_Cols_Mst_StarShopBrand
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
				bool bGet_Mst_StarShopBrand = (strRt_Cols_Mst_StarShopBrand != null && strRt_Cols_Mst_StarShopBrand.Length > 0);

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
						---- #tbl_Mst_StarShopBrand_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mssb.SSBrandCode
						into #tbl_Mst_StarShopBrand_Filter_Draft
						from Mst_StarShopBrand mssb --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mssb.SSBrandCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_StarShopBrand_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_StarShopBrand_Filter:
						select
							t.*
						into #tbl_Mst_StarShopBrand_Filter
						from #tbl_Mst_StarShopBrand_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_StarShopBrand --------:
						zzB_Select_Mst_StarShopBrand_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_StarShopBrand_Filter_Draft;
						--drop table #tbl_Mst_StarShopBrand_Filter;
					"
					);
				////
				string zzB_Select_Mst_StarShopBrand_zzE = "-- Nothing.";
				if (bGet_Mst_StarShopBrand)
				{
					#region // bGet_Mst_StarShopBrand:
					zzB_Select_Mst_StarShopBrand_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_StarShopBrand:
							select
								t.MyIdxSeq
								, mssb.*
							from #tbl_Mst_StarShopBrand_Filter t --//[mylock]
								inner join Mst_StarShopBrand mssb --//[mylock]
									on t.SSBrandCode = mssb.SSBrandCode
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
							, "Mst_StarShopBrand" // strTableNameDB
							, "Mst_StarShopBrand." // strPrefixStd
							, "mssb." // strPrefixAlias
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
					, "zzB_Select_Mst_StarShopBrand_zzE", zzB_Select_Mst_StarShopBrand_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_StarShopBrand)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_StarShopBrand";
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
		public DataSet Mst_StarShopBrand_Create(
			string strTid
			, DataRow drSession
			////
			, object objSSBrandCode
			, object objSSBrandName
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			// init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopBrand_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopBrand_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objSSBrandCode", objSSBrandCode
					, "objSSBrandName", objSSBrandName
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
				string strSSBrandCode = TUtils.CUtils.StdParam(objSSBrandCode);
				string strSSBrandName = string.Format("{0}", objSSBrandName).Trim();

				// drAbilityOfUser
				//DataRow drAbilityOfUser =  Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_StarShopBrand = null;
				{
					////
					if (strSSBrandCode == null || strSSBrandCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSBrandCode", strSSBrandCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopBrand_Create_InvalidSSBrandCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_StarShopBrand_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strSSBrandCode // objSSBrandCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopBrand // dtDB_Mst_StarShopBrand
						);
					////
					if (strSSBrandName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSBrandName", strSSBrandName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopBrand_Create_InvalidSSBrandName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_StarShopBrand:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_StarShopBrand.NewRow();
					strFN = "SSBrandCode"; drDB[strFN] = strSSBrandCode;
					strFN = "SSBrandName"; drDB[strFN] = strSSBrandName;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_StarShopBrand.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_StarShopBrand"
						, dtDB_Mst_StarShopBrand
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
		public DataSet Mst_StarShopBrand_Update(
			string strTid
			, DataRow drSession
			////
			, object objSSBrandCode
			, object objSSBrandName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			// init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopBrand_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopBrand_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objSSBrandCode", objSSBrandCode
					, "objSSBrandName", objSSBrandName
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
				string strSSBrandCode = TUtils.CUtils.StdParam(objSSBrandCode);
				string strSSBrandName = string.Format("{0}", objSSBrandName).Trim();
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_SSBrandName = strFt_Cols_Upd.Contains("Mst_StarShopBrand.SSBrandName".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_StarShopBrand.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_StarShopBrand = null;
				{
					////
					Mst_StarShopBrand_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strSSBrandCode // objSSBrandCode 
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopBrand // dtDB_Mst_StarShopBrand
						);
					////
					if (bUpd_SSBrandName && string.IsNullOrEmpty(strSSBrandName))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSBrandName", strSSBrandName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopBrand_Update_InvalidSSBrandName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // Save Mst_StarShopBrand
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_StarShopBrand.Rows[0];
					if (bUpd_SSBrandName) { strFN = "SSBrandName"; drDB[strFN] = strSSBrandName; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_StarShopBrand"
						, dtDB_Mst_StarShopBrand
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
		public DataSet Mst_StarShopBrand_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objSSBrandCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopBrand_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopBrand_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objSSBrandCode", objSSBrandCode
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
				string strSSBrandCode = TUtils.CUtils.StdParam(objSSBrandCode);
				////
				DataTable dtDB_Mst_StarShopBrand = null;
				{
					////
					Mst_StarShopBrand_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strSSBrandCode // objSSBrandCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopBrand // dtDB_Mst_StarShopBrand
						);
				}
				#endregion

				#region // SaveDB Mst_StarShopBrand:
				{
					// Init:
					dtDB_Mst_StarShopBrand.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_StarShopBrand"
						, dtDB_Mst_StarShopBrand
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

		#region // Mst_StarShopType:
		public void Mst_StarShopType_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objSSGrpCode
			, object objSSBrandCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_StarShopType
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_StarShopType t --//[mylock]
					where (1=1)
						and t.SSGrpCode = @objSSGrpCode
						and t.SSBrandCode = @objSSBrandCode
					;
				");
			dtDB_Mst_StarShopType = _cf.db.ExecQuery(
				strSqlExec
				, "@objSSGrpCode", objSSGrpCode
				, "@objSSBrandCode", objSSBrandCode
				).Tables[0];
			dtDB_Mst_StarShopType.TableName = "Mst_StarShopType";

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_StarShopType.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.SSGrpCode", objSSGrpCode
						, "Check.SSBrandCode", objSSBrandCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_StarShopType_CheckDB_StarShopTypeNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_StarShopType.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.SSGrpCode", objSSGrpCode
						, "Check.SSBrandCode", objSSBrandCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_StarShopType_CheckDB_StarShopTypeExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck:
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_StarShopType.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.SSGrpCode", objSSGrpCode
					, "Check.SSBrandCode", objSSBrandCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_StarShopType.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_StarShopType_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_StarShopType_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_StarShopType
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopType_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopType_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_StarShopType", strRt_Cols_Mst_StarShopType
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
				bool bGet_Mst_StarShopType = (strRt_Cols_Mst_StarShopType != null && strRt_Cols_Mst_StarShopType.Length > 0);

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
						---- #tbl_Mst_StarShopType_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, msst.SSGrpCode
							, msst.SSBrandCode
						into #tbl_Mst_StarShopType_Filter_Draft
						from Mst_StarShopType msst --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by 
							msst.SSGrpCode asc
							, msst.SSBrandCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_StarShopType_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_StarShopType_Filter:
						select
							t.*
						into #tbl_Mst_StarShopType_Filter
						from #tbl_Mst_StarShopType_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_StarShopType --------:
						zzB_Select_Mst_StarShopType_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_StarShopType_Filter_Draft;
						--drop table #tbl_Mst_StarShopType_Filter;
					"
					);
				////
				string zzB_Select_Mst_StarShopType_zzE = "-- Nothing.";
				if (bGet_Mst_StarShopType)
				{
					#region // bGet_Mst_StarShopType:
					zzB_Select_Mst_StarShopType_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_StarShopType:
							select
								t.MyIdxSeq
								, msst.*
							    , mssg.SSGrpCode
								, mssb.SSBrandCode
							from #tbl_Mst_StarShopType_Filter t --//[mylock]
								inner join Mst_StarShopType msst --//[mylock]
									on t.SSGrpCode = msst.SSGrpCode
										and t.SSBrandCode = msst.SSBrandCode
							    left join Mst_StarShopGroup mssg --//[mylock]
							        on msst.SSGrpCode = mssg.SSGrpCode
							    left join Mst_StarShopBrand mssb --//[mylock]
							        on msst.SSBrandCode = mssb.SSBrandCode
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
							, "Mst_StarShopType" // strTableNameDB
							, "Mst_StarShopType." // strPrefixStd
							, "msst." // strPrefixAlias
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
					, "zzB_Select_Mst_StarShopType_zzE", zzB_Select_Mst_StarShopType_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_StarShopType)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_StarShopType";
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
		public DataSet Mst_StarShopType_Create(
			string strTid
			, DataRow drSession
			////
			, object objSSGrpCode
			, object objSSBrandCode
			, object objSSTypeName
			, object objSSRate 
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopType_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopType_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objSSGrpCode", objSSGrpCode
					, "objSSBrandCode", objSSBrandCode
					, "objSSTypeName", objSSTypeName
					, "objSSRate", objSSRate
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
				string strSSGrpCode = TUtils.CUtils.StdParam(objSSGrpCode);
				string strSSBrandCode = TUtils.CUtils.StdParam(objSSBrandCode);
				string strSSTypeName = string.Format("{0}", objSSTypeName).Trim();
				string strSSRate = TUtils.CUtils.StdParam(objSSRate);

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_StarShopType = null;
				DataTable dtDB_Mst_StarShopGroup = null;
				DataTable dtDB_Mst_StarShopBrand = null;
				{

					////
					if (strSSGrpCode == null || strSSGrpCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSGrpCode", strSSGrpCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopType_Create_InvalidSSGrpCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					// //
					if (strSSBrandCode == null || strSSBrandCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSBrandCode", strSSBrandCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopType_Create_InvalidSSBrandCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_StarShopType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objSSGrpCode // objSSGrpCode
						, objSSBrandCode // objSSBrandCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopType // dtDB_Mst_StarShopType
						);
					////
					Mst_StarShopGroup_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strSSGrpCode // objSSGrpCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.Flag.Active // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopGroup // dtDB_Mst_StarShopGroup
						);
					// //
					Mst_StarShopBrand_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strSSBrandCode // objSSBrandCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.Flag.Active // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopBrand // dtDB_Mst_StarShopBrand
						);

					////
					if (strSSTypeName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSTypeName", strSSTypeName
							 });
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopType_Create_InvalidSSTypeName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					// //
					if (strSSRate.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSRate", strSSRate
							 });
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopType_Create_InvalidSSRate
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_StarShopType:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_StarShopType.NewRow();
					strFN = "SSGrpCode"; drDB[strFN] = strSSGrpCode;
					strFN = "SSBrandCode"; drDB[strFN] = strSSBrandCode;
					strFN = "SSTypeName"; drDB[strFN] = strSSTypeName;
					strFN = "SSRate"; drDB[strFN] = strSSRate;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_StarShopType.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_StarShopType"
						, dtDB_Mst_StarShopType
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
		public DataSet Mst_StarShopType_Update(
			string strTid
			, DataRow drSession
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
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopType_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopType_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objSSGrpCode", objSSGrpCode
					, "objSSBrandCode", objSSBrandCode
					, "objSSTypeName", objSSTypeName
					, "objSSRate", objSSRate
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
				string strSSGrpCode = TUtils.CUtils.StdParam(objSSGrpCode);
				string strSSBrandCode = TUtils.CUtils.StdParam(objSSBrandCode);
				string strSSTypeName = string.Format("{0}", objSSTypeName).Trim();
				string strSSRate = TUtils.CUtils.StdParam(objSSRate);
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_SSTypeName = strFt_Cols_Upd.Contains("Mst_StarShopType.SSTypeName".ToUpper());
				bool bUpd_SSRate = strFt_Cols_Upd.Contains("Mst_StarShopType.SSRate".ToUpper()); 
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_StarShopType.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_StarShopType = null; 
				{
					////
					Mst_StarShopType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strSSGrpCode // objSSGrpCode 
						, strSSBrandCode // objSSBrandCode 
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopType // dtDB_Mst_StarShopType
						);
					////
					if (bUpd_SSTypeName && string.IsNullOrEmpty(strSSTypeName))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSTypeName", strSSTypeName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopType_Update_InvalidSSTypeName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					// //
					if (bUpd_SSRate && string.IsNullOrEmpty(strSSRate))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSSRate", strSSRate
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_StarShopType_Update_InvalidSSRate
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // Save Mst_StarShopType:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_StarShopType.Rows[0];
					if (bUpd_SSTypeName) { strFN = "SSTypeName"; drDB[strFN] = strSSTypeName; alColumnEffective.Add(strFN); }
					if (bUpd_SSRate) { strFN = "SSRate"; drDB[strFN] = strSSRate; alColumnEffective.Add(strFN); } 
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_StarShopType"
						, dtDB_Mst_StarShopType
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
		public DataSet Mst_StarShopType_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objSSGrpCode
			, object objSSBrandCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_StarShopType_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_StarShopType_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objSSGrpCode", objSSGrpCode
					, "objSSBrandCode", objSSBrandCode
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
				string strSSGrpCode = TUtils.CUtils.StdParam(objSSGrpCode);
				string strSSBrandCode = TUtils.CUtils.StdParam(objSSBrandCode);
				////
				DataTable dtDB_Mst_StarShopType = null;
				{
					////
					Mst_StarShopType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objSSGrpCode // objSSGrpCode
						, objSSBrandCode // objSSBrandCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_StarShopType // dtDB_Mst_StarShopType
						);
				}
				#endregion

				#region // SaveDB Mst_StarShopType:
				{
					// Init:
					dtDB_Mst_StarShopType.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_StarShopType"
						, dtDB_Mst_StarShopType
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

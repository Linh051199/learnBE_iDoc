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
using Demo.Lab.Constants;



namespace Demo.Lab.Biz
{
	public partial class BizDemoLab
	{
		#region // Mst_CampainCriteriaScope:
		public void Mst_CampainCriteriaScope_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objCampaignCrCode
			, object objSSGrpCode
			, object objSSBrandCode
			, object objLevelCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_CampainCriteriaScope
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_CampainCriteriaScope t --//[mylock]
					where (1=1)
						and t.objCampaignCrCode = @objCampaignCrCode
						and t.objSSGrpCode = @objSSGrpCode
						and t.objSSBrandCode = @objSSBrandCode
						and t.objLevelCode = @objLevelCode
					;
				");
			dtDB_Mst_CampainCriteriaScope = _cf.db.ExecQuery(
				strSqlExec
				, "@objCampaignCrCode", objCampaignCrCode
				, "@objSSGrpCode", objSSGrpCode
				, "@objSSBrandCode", objSSBrandCode
				, "@objLevelCode", objLevelCode
				).Tables[0];
			dtDB_Mst_CampainCriteriaScope.TableName = "Mst_CampainCriteriaScope";

			// strFlagExistToCheck
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_CampainCriteriaScope.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCrCode", objCampaignCrCode
						, "Check.SSGrpCode", objSSGrpCode
						, "Check.SSBrandCode", objSSBrandCode
						, "Check.LevelCode", objLevelCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_CampainCriteriaScope_CheckDB_CampainCriteriaScopeNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_CampainCriteriaScope.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCrCode", objCampaignCrCode
						, "Check.SSGrpCode", objSSGrpCode
						, "Check.SSBrandCode", objSSBrandCode
						, "Check.LevelCode", objLevelCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_CampainCriteriaScope_CheckDB_CampainCriteriaScopeExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_CampainCriteriaScope.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CampaignCrCode", objCampaignCrCode
					, "Check.SSGrpCode", objSSGrpCode
					, "Check.SSBrandCode", objSSBrandCode
					, "Check.LevelCode", objLevelCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_CampainCriteriaScope.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_CampainCriteriaScope_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}

		#endregion

		#region // Mst_CampainCriteria:
		public void Mst_CampainCriteria_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objCampaignCrCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_CampainCriteria
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_CampainCriteria t --//[mylock]
					where (1=1)
						and t.CampaignCrCode = @objCampaignCrCode
					;
				");
			dtDB_Mst_CampainCriteria = _cf.db.ExecQuery(
				strSqlExec
				, "@objCampaignCrCode", objCampaignCrCode
				).Tables[0];
			dtDB_Mst_CampainCriteria.TableName = "Mst_CampainCriteria";

			// strFlagExistToCheck
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_CampainCriteria.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCrCode", objCampaignCrCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_CampainCriteria_CheckDB_CampainCriteriaNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_CampainCriteria.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCrCode", objCampaignCrCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_CampainCriteria_CheckDB_CampainCriteriaExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_CampainCriteria.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CampaignCrCode", objCampaignCrCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_CampainCriteria.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_CampainCriteria_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_CampainCriteria_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return: 
			, string strRt_Cols_Mst_CampainCriteria
			, string strRt_Cols_Mst_CampainCriteriaScope
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_CampainCriteria_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_CampainCriteria_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_CampainCriteria", strRt_Cols_Mst_CampainCriteria                  
					, "strRt_Cols_Mst_CampainCriteriaScope", strRt_Cols_Mst_CampainCriteriaScope
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
				bool bGet_Mst_CampainCriteria = (strRt_Cols_Mst_CampainCriteria != null && strRt_Cols_Mst_CampainCriteria.Length > 0);
				bool bGet_Mst_CampainCriteriaScope = (strRt_Cols_Mst_CampainCriteriaScope != null && strRt_Cols_Mst_CampainCriteriaScope.Length > 0);

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
						---- #tbl_Mst_CampainCriteria_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mcc.CampaignCrCode
						into #tbl_Mst_CampainCriteria_Filter_Draft
						from Mst_CampainCriteria mcc --//[mylock]
							left join Mst_CampainCriteriaScope mccs --//[mylock]
								on mcc.CampaignCrCode = mccs.CampaignCrCode
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mcc.CampaignCrCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_CampainCriteria_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_CampainCriteria_Filter:
						select
							t.*
						into #tbl_Mst_CampainCriteria_Filter
						from #tbl_Mst_CampainCriteria_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_CampainCriteria --------:
						zzB_Select_Mst_CampainCriteria_zzE
						----------------------------------------

						-------- Mst_CampainCriteriaScope --------:
						zzB_Select_Mst_CampainCriteriaScope_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_CampainCriteria_Filter_Draft;
						--drop table #tbl_Mst_CampainCriteria_Filter;
					"
					);
				////
				string zzB_Select_Mst_CampainCriteria_zzE = "-- Nothing.";
				if (bGet_Mst_CampainCriteria)
				{
					#region // bGet_Mst_CampainCriteria:
					zzB_Select_Mst_CampainCriteria_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_CampainCriteria:
							select
								t.MyIdxSeq
								, mcc.*
							from #tbl_Mst_CampainCriteria_Filter t --//[mylock]
								inner join Mst_CampainCriteria mcc --//[mylock]
									on t.CampaignCrCode = mcc.CampaignCrCode
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzB_Select_Mst_CampainCriteriaScope_zzE = "-- Nothing.";
				if (bGet_Mst_CampainCriteria)
				{
					#region // bGet_Mst_CampainCriteriaScope:
					zzB_Select_Mst_CampainCriteriaScope_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_CampainCriteria:
							select
								t.MyIdxSeq
								, mccs.*
								, mccs.LevelCode mccs_LevelCode
								, mccs.CampainCritScopeDesc mccs_CampainCritScopeDesc
								, msst.SSTypeName msst_SSTypeName
							from #tbl_Mst_CampainCriteria_Filter t --//[mylock]
								inner join Mst_CampainCriteria mcc --//[mylock]
									on t.CampaignCrCode = mcc.CampaignCrCode
								inner join Mst_CampainCriteriaScope mccs --//[mylock]
									on mcc.CampaignCrCode = mccs.CampaignCrCode
								left join Mst_StarShopType msst --//[mylock]
									on mccs.SSGrpCode = msst.SSGrpCode
										and mccs.SSBrandCode = msst.SSBrandCode
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
							, "Mst_CampainCriteria" // strTableNameDB
							, "Mst_CampainCriteria." // strPrefixStd
							, "mcc." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_CampainCriteriaScope" // strTableNameDB
							, "Mst_CampainCriteriaScope." // strPrefixStd
							, "mccs." // strPrefixAlias
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
					, "zzB_Select_Mst_CampainCriteria_zzE", zzB_Select_Mst_CampainCriteria_zzE
					, "zzB_Select_Mst_CampainCriteriaScope_zzE", zzB_Select_Mst_CampainCriteriaScope_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_CampainCriteria)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_CampainCriteria";
				}
				if (bGet_Mst_CampainCriteriaScope)
				{
					dsGetData.Tables[nIdxTable++].TableName = "bGet_Mst_CampainCriteriaScope";
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
		public DataSet Mst_CampainCriteria_Update(
			string strTid
			, DataRow drSession
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
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			// init nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_CampainCriteria_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_CampainCriteria_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objCampaignCrCode", objCampaignCrCode
					, "objCampaignCrName", objCampaignCrName
					, "objCampainCriteriaType", objCampainCriteriaType
					, "objCreateDTime", objCreateDTime
					, "objCreateBy", objCreateBy
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
				string strCampaignCrCode = TUtils.CUtils.StdParam(objCampaignCrCode); 
				string strCampaignCrName = TUtils.CUtils.StdParam(objCampaignCrName); 
				string strCampainCriteriaType = TUtils.CUtils.StdParam(objCampainCriteriaType); 
				string strCreateDTime = TUtils.CUtils.StdParam(objCreateDTime); 
				string strCreateBy = TUtils.CUtils.StdParam(objCreateBy);
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_CampaignCrName = strFt_Cols_Upd.Contains("Mst_CampainCriteria.CampaignCrName".ToUpper());
				bool bUpd_CampainCriteriaType = strFt_Cols_Upd.Contains("Mst_CampainCriteria.CampainCriteriaType".ToUpper());
				bool bUpd_CreateDTime = strFt_Cols_Upd.Contains("Mst_CampainCriteria.CreateDTime".ToUpper());
				bool bUpd_CreateBy = strFt_Cols_Upd.Contains("Mst_CampainCriteria.CreateBy".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_CampainCriteria.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_CampainCriteria = null;
				{
					////
					Mst_CampainCriteria_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCampaignCrCode // objCampaignCrCode 
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_CampainCriteria // dtDB_Mst_CampainCriteria
						);
					////
					if (bUpd_CampaignCrName && string.IsNullOrEmpty(strCampaignCrName))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strCampaignCrName", strCampaignCrName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_CampainCriteria_Update_InvalidCampaignCrName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					// //
					if (bUpd_CampainCriteriaType && string.IsNullOrEmpty(strCampainCriteriaType))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strCampainCriteriaType", strCampainCriteriaType
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_CampainCriteria_Update_InvalidCampainCriteriaType
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					// //
					if (bUpd_CreateDTime && string.IsNullOrEmpty(strCreateDTime))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strCreateDTime", strCreateDTime
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_CampainCriteria_Update_InvalidCreateDTime
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					// //
					if (bUpd_CreateBy && string.IsNullOrEmpty(strCreateBy))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strCreateBy", strCreateBy
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_CampainCriteria_Update_InvalidCreateBy
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // Save Mst_CampainCriteria
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_CampainCriteria.Rows[0];
					if (bUpd_CampaignCrName) { strFN = "CampaignCrName"; drDB[strFN] = strCampaignCrName; alColumnEffective.Add(strFN); }
					if (bUpd_CampainCriteriaType) { strFN = "CampainCriteriaType"; drDB[strFN] = strCampainCriteriaType; alColumnEffective.Add(strFN); }
					if (bUpd_CreateDTime) { strFN = "CreateDTime"; drDB[strFN] = strCreateDTime; alColumnEffective.Add(strFN); }
					if (bUpd_CreateBy) { strFN = "CreateBy"; drDB[strFN] = strCreateBy; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_CampainCriteria"
						, dtDB_Mst_CampainCriteria
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
		public DataSet Mst_CampainCriteria_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objCampaignCrCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_CampainCriteria_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_CampainCriteria_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objCampaignCrCode", objCampaignCrCode
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
				string strCampaignCrCode = TUtils.CUtils.StdParam(objCampaignCrCode);
				////
				DataTable dtDB_Mst_CampainCriteria = null;
				{
					////
					Mst_CampainCriteria_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCampaignCrCode // objCampaignCrCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_CampainCriteria // dtDB_Mst_CampainCriteria
						);
				}
				#endregion

				#region // SaveDB Mst_CampainCriteria:
				{
					// Init:
					dtDB_Mst_CampainCriteria.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_CampainCriteria"
						, dtDB_Mst_CampainCriteria
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
		public DataSet Mst_CampainCriteria_Save(
			string strTid
			, DataRow drSession
			////
			, object objFlagIsDelete
			////
			, object objCampaignCrCode
			, object objCampaignCrName
			, object objCampainCriteriaType 
			, object[] arrobjDSData
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bMyDebugSql = false;
			DateTime dtimeSys = DateTime.Now;
			bool bNeedTransaction = true;
			string strFunctionName = "Mst_CampainCriteria_Save";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_CampainCriteria_Save;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objFlagIsDelete", objFlagIsDelete
					////
					, "objCampaignCrCode", objCampaignCrCode
					, "objCampaignCrName", objCampaignCrName
					, "objCampainCriteriaType", objCampainCriteriaType
					});
			//ArrayList alPCErrEx = new ArrayList();
			#endregion

			try
			{
				#region // Convert Input:
				DataSet dsData = TUtils.CUtils.StdDS(arrobjDSData);
				if (dsData == null) dsData = new DataSet("dsData");
				dsData.AcceptChanges();
				alParamsCoupleError.AddRange(new object[]{
					"Check.dsData", CmUtils.XmlUtils.DataSet2XmlSimple(dsData)
					});
				#endregion

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

				#region //// Refine and Check Mst_CampainCriteria:
				////
				bool bIsDelete = CmUtils.StringUtils.StringEqual(objFlagIsDelete, TConst.Flag.Yes);
				////
				string strCampaignCrCode = TUtils.CUtils.StdParam(objCampaignCrCode); 
				string strCampaignCrName = string.Format("{0}", objCampaignCrName).Trim();
				string strCampainCriteriaType = string.Format("{0}", objCampainCriteriaType).Trim().ToUpper(); 
				string strCreateDTime = null;
				string strCreateBy = null;

				////
				DataTable dtDB_Mst_CampainCriteria = null;
				{

					Mst_CampainCriteria_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCampaignCrCode // objCampaignCrCode
						, "" // strFlagExistToCheck
						, "" // strStatusListToCheck
						, out dtDB_Mst_CampainCriteria // dtDB_Mst_CampainCriteria
						);
					if (dtDB_Mst_CampainCriteria.Rows.Count < 1) // Chưa Tồn tại Order.
					{
						if (bIsDelete)
						{
							goto MyCodeLabel_Done; // Thành công.
						}
						else
						{
							// Nothing.
						}
					}
					else // Đã Tồn tại.
					{
						if (!CmUtils.StringUtils.StringEqualIgnoreCase(dtDB_Mst_CampainCriteria.Rows[0]["FlagActive"], TConst.Flag.Active))
						{
							alParamsCoupleError.AddRange(new object[]{
								"Check.DB.FlagActive", dtDB_Mst_CampainCriteria.Rows[0]["FlagActive"]
								,"Check.FlagActive", TConst.Flag.Active
								});
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Mst_CampainCriteria_Save_InvalidFlagActive
								, null
								, alParamsCoupleError.ToArray()
								);
						}
						else
						{
							strCreateDTime = TUtils.CUtils.StdDTime(dtDB_Mst_CampainCriteria.Rows[0]["CreateDTime"]);
							strCreateBy = TUtils.CUtils.StdParam(dtDB_Mst_CampainCriteria.Rows[0]["CreateBy"]);
						}

					}
					////
					strCreateDTime = string.IsNullOrEmpty(strCreateDTime) ? dtimeSys.ToString("yyyy-MM-dd HH:mm:ss") : strCreateDTime;
					strCreateBy = string.IsNullOrEmpty(strCreateBy) ? _cf.sinf.strUserCode : strCreateBy;
					////
					if (strCampaignCrName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strCampaignCrName", strCampaignCrName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_CampainCriteria_Save_InvalidCampaignCrName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (strCampainCriteriaType.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strCampainCriteriaType", strCampainCriteriaType
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_CampainCriteria_Save_InvalidCampainCriteriaType
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region //// SaveTemp Mst_CampainCriteria:
				if (!bIsDelete)
				{
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db
						, "#input_Mst_CampainCriteria"
						, new object[]{
							"CampaignCrCode", TConst.BizMix.Default_DBColType,
							"CampaignCrName", TConst.BizMix.Default_DBColType,
							"CampainCriteriaType", TConst.BizMix.Default_DBColType,
							"CreateDTime", TConst.BizMix.Default_DBColType,
							"CreateBy", TConst.BizMix.Default_DBColType,
							"FlagActive", TConst.BizMix.Default_DBColType,
							"LogLUDTime", TConst.BizMix.Default_DBColType,
							"LogLUBy", TConst.BizMix.Default_DBColType,
							}
						, new object[]{
							new object[]{
								strCampaignCrCode, // CampaignCrCode
								strCampaignCrName, // CampaignCrName
								strCampainCriteriaType, // CampainCriteriaType 
								strCreateDTime, // CreateDTime
								strCreateBy, // CreateBy
								TConst.Flag.Active, // FlagActive
								dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"), // LogLUDTime
								_cf.sinf.strUserCode, // LogLUBy
								}	
							}
						);
				}
				#endregion

				#region //// Refine and Check Mst_CampainCriteriaScope:
				////
				DataTable dtInput_Mst_CampainCriteriaScope = null;
				if (!bIsDelete)
				{
					////
					string strTableCheck = "Mst_CampainCriteriaScope";
					////
					if (!dsData.Tables.Contains(strTableCheck))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.TableName", strTableCheck
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_CampainCriteria_Save_Input_CampainCriteriaScopeTblNotFound
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					dtInput_Mst_CampainCriteriaScope = dsData.Tables[strTableCheck];
					////
					TUtils.CUtils.StdDataInTable(
						dtInput_Mst_CampainCriteriaScope // dtData
						, "StdParam", "SSGrpCode" // arrstrCouple
						, "StdParam", "SSBrandCode" // arrstrCouple
						, "StdParam", "LevelCode" // arrstrCouple
						, "", "CampainCritScopeDesc" // arrstrCouple 
						);
					//// 
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Mst_CampainCriteriaScope, "CampaignCrCode", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Mst_CampainCriteriaScope, "FlagActive", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Mst_CampainCriteriaScope, "LogLUDTime", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Mst_CampainCriteriaScope, "LogLUBy", typeof(object));
					////
					for (int nScan = 0; nScan < dtInput_Mst_CampainCriteriaScope.Rows.Count; nScan++)
					{
						////
						DataRow drScan = dtInput_Mst_CampainCriteriaScope.Rows[nScan];

						////
						DataTable dtDB_Mst_StarShopType = null;

						Mst_StarShopType_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, drScan["SSGrpCode"] // objSSGrpCode
							, drScan["SSBrandCode"] // objSSBrandCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_StarShopType // dtDB_Mst_StarShopType
							);
						////
						DataTable dtDB_Mst_StarShopGroup = null;

						Mst_StarShopGroup_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, drScan["SSGrpCode"] // objSSGrpCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_StarShopGroup // dtDB_Mst_StarShopGroup
							);
						////
						DataTable dtDB_Mst_StarShopBrand = null;

						Mst_StarShopBrand_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, drScan["SSBrandCode"] // objSSBrandCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_StarShopBrand // dtDB_Mst_StarShopBrand
							);

						////
						drScan["CampaignCrCode"] = strCampaignCrCode;
						drScan["FlagActive"] = TConst.Flag.Active;
						drScan["LogLUDTime"] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
						drScan["LogLUBy"] = _cf.sinf.strUserCode;
						////
					}
				}
				#endregion

				#region //// SaveTemp Mst_CampainCriteriaScope:
				if (!bIsDelete)
				{
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db
						, "#input_Mst_CampainCriteriaScope"
						, new object[]{
							"CampaignCrCode", TConst.BizMix.Default_DBColType,
							"SSGrpCode", TConst.BizMix.Default_DBColType,
							"SSBrandCode", TConst.BizMix.Default_DBColType,
							"LevelCode", TConst.BizMix.Default_DBColType,
							"FlagActive", TConst.BizMix.Default_DBColType,
							"CampainCritScopeDesc", TConst.BizMix.Default_DBColType,
							"LogLUDTime", TConst.BizMix.Default_DBColType,
							"LogLUBy", TConst.BizMix.Default_DBColType,
							}
						, dtInput_Mst_CampainCriteriaScope
						);
				}
				#endregion

				#region //// Save:
				//// Clear All:
				{
					string strSqlDelete = CmUtils.StringUtils.Replace(@"
								---- Mst_CampainCriteriaScope:
								delete t
								from Mst_CampainCriteriaScope t 
								where (1=1)
									and t.CampaignCrCode = @strCampaignCrCode
								;

								---- Mst_CampainCriteria:
								delete t
								from Mst_CampainCriteria t 
								where (1=1)
									and t.CampaignCrCode = @strCampaignCrCode
								;

							");
					_cf.db.ExecQuery(
						strSqlDelete
						, "@strCampaignCrCode", strCampaignCrCode
						);
				}

				//// Insert All:
				if (!bIsDelete)
				{
					////
					string zzzzClauseInsert_Mst_CampainCriteria_zSave = CmUtils.StringUtils.Replace(@"
							---- Mst_CampainCriteria:
							insert into Mst_CampainCriteria
							(
								CampaignCrCode
								, CampaignCrName
								, CampainCriteriaType
								, CreateDTime
								, CreateBy
								, FlagActive
								, LogLUDTime
								, LogLUBy
							)
							select 
								t.CampaignCrCode
								, t.CampaignCrName
								, t.CampainCriteriaType
								, t.CreateDTime
								, t.CreateBy
								, t.FlagActive
								, t.LogLUDTime
								, t.LogLUBy
							from #input_Mst_CampainCriteria t --//[mylock]
							;
						");
					////
					string zzzzClauseInsert_Mst_CampainCriteriaScope_zSave = CmUtils.StringUtils.Replace(@"
							---- Mst_CampainCriteriaScope:
							insert into Mst_CampainCriteriaScope
							(
								CampaignCrCode
								, SSGrpCode
								, SSBrandCode
								, LevelCode
								, CampainCritScopeDesc
								, FlagActive 
								, LogLUDTime
								, LogLUBy
							)
							select 
								t.CampaignCrCode
								, t.SSGrpCode
								, t.SSBrandCode
								, t.LevelCode
								, t.CampainCritScopeDesc
								, t.FlagActive 
								, t.LogLUDTime
								, t.LogLUBy
							from #input_Mst_CampainCriteriaScope t --//[mylock]
							;
						");
					////
					string strSqlExec = CmUtils.StringUtils.Replace(@"
							----
							zzzzClauseInsert_Mst_CampainCriteria_zSave
							----
							zzzzClauseInsert_Mst_CampainCriteriaScope_zSave
							----
						"
						, "zzzzClauseInsert_Mst_CampainCriteria_zSave", zzzzClauseInsert_Mst_CampainCriteria_zSave
						, "zzzzClauseInsert_Mst_CampainCriteriaScope_zSave", zzzzClauseInsert_Mst_CampainCriteriaScope_zSave
						);
					////
					if (bMyDebugSql)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strSqlExec", strSqlExec
							});
					}
					DataSet dsExec = _cf.db.ExecQuery(strSqlExec);
				}
				#endregion

				// Return Good:
				MyCodeLabel_Done:
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
				//alParamsCoupleError.AddRange(alPCErrEx);
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

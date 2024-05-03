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
		#region // Mst_AreaMarket:
		private void Mst_AreaMarket_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objAreaCode
			, string strFlagExistToCheck
			, string objAreaMarketStatusListToCheck
			, out DataTable dtDB_Mst_AreaMarket
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_AreaMarket t --//[mylock]
					where (1=1)
						and t.AreaCode = @objAreaCode
					;
				");
			dtDB_Mst_AreaMarket = _cf.db.ExecQuery(
				strSqlExec
				, "@objAreaCode", objAreaCode
				).Tables[0];
			dtDB_Mst_AreaMarket.TableName = "Mst_AreaMarket";

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_AreaMarket.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.AreaCode", objAreaCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_AreaMarket_CheckDB_AreaMarketNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_AreaMarket.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.AreaCode", objAreaCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_AreaMarket_CheckDB_AreaMarketExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// objAreaMarketStatusListToCheck:
			if (objAreaMarketStatusListToCheck.Length > 0 && !objAreaMarketStatusListToCheck.Contains(Convert.ToString(dtDB_Mst_AreaMarket.Rows[0]["AreaStatus"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.AreaCode", objAreaCode
					, "Check.AreaMarketStatusListToCheck", objAreaMarketStatusListToCheck
					, "DB.AreaStatus", dtDB_Mst_AreaMarket.Rows[0]["AreaStatus"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_AreaMarket_CheckDB_AreaMarketStatusNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}

		private void Mst_AreaMarket_UpdBU()
		{
			string strSqlPostSave = CmUtils.StringUtils.Replace(@"
                    declare @strAreaCode_Root nvarchar(100); select @strAreaCode_Root = 'VN';

                    update t
                    set
	                    t.AreaBUCode = @strAreaCode_Root
	                    , t.AreaBUPattern = @strAreaCode_Root + '%'
	                    , t.AreaLevel = 1
                    from Mst_AreaMarket t
	                    left join Mst_AreaMarket t_Parent
		                    on t.AreaCodeParent = t_Parent.AreaCode
                    where (1=1)
	                    and t.AreaCode in (@strAreaCode_Root)
                    ;

                    declare @nDeepAreaMarket int; select @nDeepAreaMarket = 0;
                    while (@nDeepAreaMarket <= 6)
                    begin
	                    select @nDeepAreaMarket = @nDeepAreaMarket + 1;
	
	                    update t
	                    set
		                    t.AreaBUCode = IsNull(t_Parent.AreaBUCode + '.', '') + t.AreaCode
		                    , t.AreaBUPattern = IsNull(t_Parent.AreaBUCode + '.', '') + t.AreaCode + '%'
		                    , t.AreaLevel = IsNull(t_Parent.AreaLevel, 0) + 1
	                    from Mst_AreaMarket t
		                    left join Mst_AreaMarket t_Parent
			                    on t.AreaCodeParent = t_Parent.AreaCode
	                    where (1=1)
		                    and t.AreaCode not in (@strAreaCode_Root)
	                    ;
                    end;
                ");
			DataSet dsPostSave = _cf.db.ExecQuery(strSqlPostSave);
		}

		public DataSet Mst_AreaMarket_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_AreaMarket
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Mst_AreaMarket_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_AreaMarket_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_AreaMarket", strRt_Cols_Mst_AreaMarket
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
				bool bGet_Mst_AreaMarket = (strRt_Cols_Mst_AreaMarket != null && strRt_Cols_Mst_AreaMarket.Length > 0);

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = mySys_User_GetAbilityViewBankOfUser(_cf.sinf.strUserCode);

				#endregion

				#region // Build Sql:
				ArrayList alParamsCoupleSql = new ArrayList();
				//alParamsCoupleSql.AddRange(new object[] { "@strAbilityOfUser", drAbilityOfUser["MBBankBUPattern"] });
				alParamsCoupleSql.AddRange(new object[] { 
					"@nFilterRecordStart", nFilterRecordStart
					, "@nFilterRecordEnd", nFilterRecordEnd
					, "@Today", DateTime.Today.ToString("yyyy-MM-dd")
					});
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Mst_AreaMarket_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mam.AreaCode
						into #tbl_Mst_AreaMarket_Filter_Draft
						from Mst_AreaMarket mam --//[mylock]
						where (1=1)
							zzzzClauseWhere_FilterAbilityOfUser -- Filter the AbilityOfUser
							zzzzClauseWhere_strFilterWhereClause
						order by mam.AreaCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_AreaMarket_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_AreaMarket_Filter:
						select
							t.*
						into #tbl_Mst_AreaMarket_Filter
						from #tbl_Mst_AreaMarket_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_AreaMarket --------:
						zzzzClauseSelect_Mst_AreaMarket_zOut
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_AreaMarket_Filter_Draft;
						--drop table #tbl_Mst_AreaMarket_Filter;
					"
					, "zzzzClauseWhere_FilterAbilityOfUser", ""
					);
				////
				string zzzzClauseSelect_Mst_AreaMarket_zOut = "-- Nothing.";
				if (bGet_Mst_AreaMarket)
				{
					#region // bGet_Mst_AreaMarket:
					zzzzClauseSelect_Mst_AreaMarket_zOut = CmUtils.StringUtils.Replace(@"
							---- Mst_AreaMarket:
							select
								t.MyIdxSeq
								, mam.*
							from #tbl_Mst_AreaMarket_Filter t --//[mylock]
								inner join Mst_AreaMarket mam --//[mylock]
									on t.AreaCode = mam.AreaCode
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzzzClauseWhere_strFilterWhereClause = "";
				{
					Hashtable htSpCols = new Hashtable();
					{
						#region // htSpCols:
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_AreaMarket" // strTableNameDB
							, "Mst_AreaMarket." // strPrefixStd
							, "mam." // strPrefixAlias
							);
						////
						#endregion
					}
					zzzzClauseWhere_strFilterWhereClause = CmUtils.SqlUtils.BuildWhere(
						htSpCols // htSpCols
						, strFt_WhereClause // strClause
						, "@p_" // strParamPrefix
						, ref alParamsCoupleSql // alParamsCoupleSql
						);
					zzzzClauseWhere_strFilterWhereClause = (zzzzClauseWhere_strFilterWhereClause.Length <= 0 ? "" : string.Format(" and ({0})", zzzzClauseWhere_strFilterWhereClause));
					alParamsCoupleError.AddRange(new object[]{
						"zzzzClauseWhere_strFilterWhereClause", zzzzClauseWhere_strFilterWhereClause
						});
				}
				////
				strSqlGetData = CmUtils.StringUtils.Replace(
					strSqlGetData
					, "zzzzClauseWhere_strFilterWhereClause", zzzzClauseWhere_strFilterWhereClause
					, "zzzzClauseSelect_Mst_AreaMarket_zOut", zzzzClauseSelect_Mst_AreaMarket_zOut
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_AreaMarket)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_AreaMarket";
				}
				CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dsGetData);
				#endregion

				// Return Good:
				TDALUtils.DBUtils.RollbackSafety(_cf.db);
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

		public DataSet Mst_AreaMarket_Create(
			string strTid
			, DataRow drSession
			////
			, object objAreaCode
			, object objAreaCodeParent
			, object objAreaDesc
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Mst_AreaMarket_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_AreaMarket_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objAreaCode", objAreaCode
					, "objAreaCodeParent", objAreaCodeParent
                    , "objAreaDesc", objAreaDesc
					});
			#endregion

			try
			{
				#region // Convert Input:
				DateTime dtimeTDate = DateTime.Now;
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

				#region // Refine and Check Input:
				////
				string strAreaCode = TUtils.CUtils.StdParam(objAreaCode);
				string strAreaCodeParent = TUtils.CUtils.StdParam(objAreaCodeParent);
				string strAreaDesc = string.Format("{0}", objAreaDesc).Trim();
				////
				DataTable dtDB_Mst_AreaMarket = null;
				DataTable dtDB_Mst_AreaMarketParrent = null;
				{
					////
					if (strAreaCode == null || strAreaCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strAreaCode", strAreaCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_AreaMarket_Create_InvalidAreaCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_AreaMarket_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objAreaCode // objAreaCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // objAreaMarketStatusListToCheck                        
						, out dtDB_Mst_AreaMarket // dtDB_Mst_AreaMarket
						);
					////
					DataTable dtDB_Mst_AreaMarketParent = null;
					Mst_AreaMarket_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strAreaCodeParent // objAreaCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // objAreaMarketStatusListToCheck                        
						, out dtDB_Mst_AreaMarketParent // dtDB_Mst_AreaMarket
						);
					//// 
					if (strAreaCodeParent != null && strAreaCodeParent.Length > 0)
					{
						Mst_AreaMarket_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, objAreaCodeParent // objAreaCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.StatusCm.Yes // objAreaMarketStatusListToCheck                        
							, out dtDB_Mst_AreaMarketParrent // dtDB_Mst_AreaMarket
						);
					}

					////
					if (strAreaDesc.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strAreaDesc", strAreaDesc
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_AreaMarket_Create_InvalidAreaDesc
							, null
							, alParamsCoupleError.ToArray()
							);
					}

					////
				}
				#endregion

				#region // SaveDB AreaMarket:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_AreaMarket.NewRow();
					strFN = "AreaCode"; drDB[strFN] = strAreaCode;
					strFN = "AreaCodeParent"; drDB[strFN] = strAreaCodeParent;
					strFN = "AreaBUCode"; drDB[strFN] = "X";
					strFN = "AreaBUPattern"; drDB[strFN] = "X";
					strFN = "AreaLevel"; drDB[strFN] = 1;
					strFN = "AreaDesc"; drDB[strFN] = strAreaDesc;
					strFN = "AreaStatus"; drDB[strFN] = TConst.StatusCm.Yes;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeTDate.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_AreaMarket.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_AreaMarket"
						, dtDB_Mst_AreaMarket
						//, alColumnEffective.ToArray()
						);
				}
				#endregion

				#region // Post Save:
				{
					Mst_AreaMarket_UpdBU();
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

		public DataSet Mst_AreaMarket_Update(
			string strTid
			, DataRow drSession
			////
			, object objAreaCode
			, object objAreaCodeParent
			, object objAreaDesc
			, object objAreaStatus
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Mst_AreaMarket_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_AreaMarket_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objAreaCode", objAreaCode
					, "objAreaCodeParent", objAreaCodeParent
                    , "objAreaDesc", objAreaDesc
					, "objAreaStatus", objAreaStatus
					, "objFt_Cols_Upd", objFt_Cols_Upd
					});
			#endregion

			try
			{
				#region // Convert Input:
				DateTime dtimeTDate = DateTime.Now;
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

				#region // Refine and Check Input:
				////
				string strFt_Cols_Upd = TUtils.CUtils.StdParam(objFt_Cols_Upd);
				strFt_Cols_Upd = (strFt_Cols_Upd == null ? "" : strFt_Cols_Upd);
				////
				string strAreaCode = TUtils.CUtils.StdParam(objAreaCode);
				string strAreaCodeParent = TUtils.CUtils.StdParam(objAreaCodeParent);
				string strAreaDesc = string.Format("{0}", objAreaDesc).Trim();
				string strAreaStatus = (CmUtils.StringUtils.StringEqual(objAreaStatus, TConst.StatusCm.No) ? TConst.StatusCm.No : TConst.StatusCm.Yes);
				////
				DataTable dtDB_Mst_AreaMarket = null;
				DataTable dtDB_Mst_AreaMarketParrent = null;
				bool bUpd_AreaCodeParent = strFt_Cols_Upd.Contains("Mst_AreaMarket.AreaCodeParent".ToUpper());
				bool bUpd_AreaDesc = strFt_Cols_Upd.Contains("Mst_AreaMarket.AreaDesc".ToUpper());
				bool bUpd_AreaStatus = strFt_Cols_Upd.Contains("Mst_AreaMarket.AreaStatus".ToUpper());
				////
				{
					////
					Mst_AreaMarket_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objAreaCode // objAreaCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // objAreaMarketStatusListToCheck                        
						, out dtDB_Mst_AreaMarket // dtDB_Mst_AreaMarket
						);

					////
					if (bUpd_AreaCodeParent)
					{
						Mst_AreaMarket_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objAreaCodeParent // objAreaCodeParent
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.StatusCm.Yes // objAreaMarketStatusListToCheck                        
						, out dtDB_Mst_AreaMarketParrent // dtDB_Mst_AreaMarketParrent
						);
					}

					////
					if (bUpd_AreaDesc && strAreaDesc.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strAreaDesc", strAreaDesc
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_AreaMarket_Update_InvalidAreaDesc
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_AreaMarket:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_AreaMarket.Rows[0];
					if (bUpd_AreaCodeParent) { strFN = "AreaCodeParent"; drDB[strFN] = objAreaCodeParent; alColumnEffective.Add(strFN); }
					if (bUpd_AreaDesc) { strFN = "AreaDesc"; drDB[strFN] = objAreaDesc; alColumnEffective.Add(strFN); }
					if (bUpd_AreaStatus) { strFN = "AreaStatus"; drDB[strFN] = strAreaStatus; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeTDate.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_AreaMarket"
						, dtDB_Mst_AreaMarket
						, alColumnEffective.ToArray()
						);
				}
				#endregion

				#region // Post Save:
				if (bUpd_AreaCodeParent)
				{
					Mst_AreaMarket_UpdBU();
				}
				#endregion

				#region // Check InActive Area, DB, OL:
				{
					if (bUpd_AreaStatus && CmUtils.StringUtils.StringEqual(strAreaStatus, TConst.Flag.Inactive))
					{
						string strSqlCheck = CmUtils.StringUtils.Replace(@"
								---- Mst_AreaMarket:
								select 
									mam.*
								from Mst_AreaMarket mam --//[mylock]
								where (1=1)
									--and mam.AreaBUCode like 'VN%'
									and mam.AreaBUCode like '@strAreaBUPattern'
									--and mam.AreaCode <> '@strAreaCode'
									and mam.AreaStatus = '@strFlagActive'
								;

								---- Mst_Distributor:
								select distinct
									md.*
								from Mst_AreaMarket mam --//[mylock]
									inner join Mst_Distributor md --//[mylock]
										on mam.AreaCode = md.AreaCode
								where (1=1)
									and mam.AreaBUCode like '@strAreaBUPattern'
									or (mam.AreaCode = 'strAreaCode')
									and mam.AreaStatus = '@strFlagActive'
								;

								---- Mst_Outlet:
								select distinct
									mo.*
								from Mst_AreaMarket mam --//[mylock]
									inner join Mst_Distributor md --//[mylock]
										on mam.AreaCode = md.AreaCode
									inner join Mst_Outlet mo --//[mylock]
										on md.DBCode = mo.DBCode
								where (1=1)
									and mam.AreaBUCode like '@strAreaBUPattern'
									and mam.AreaStatus = '@strFlagActive'
								;
						"
						, "@strAreaBUPattern", dtDB_Mst_AreaMarket.Rows[0]["AreaBUPattern"]
						, "@strAreaCode", strAreaCode
						, "@strFlagActive", TConst.Flag.Active
						);
						DataSet dsCheck = _cf.db.ExecQuery(strSqlCheck);
						////
						DataTable dtDB_Mst_Outlet = dsCheck.Tables[dsCheck.Tables.Count - 1];
						dtDB_Mst_Outlet.TableName = "Mst_Outlet";
						DataTable dtDB_Mst_Distributor = dsCheck.Tables[dsCheck.Tables.Count - 2];
						dtDB_Mst_Distributor.TableName = "Mst_Distributor";
						DataTable dtDB_Mst_AreaMarketCheck = dsCheck.Tables[dsCheck.Tables.Count - 3];
						dtDB_Mst_AreaMarketCheck.TableName = "Mst_AreaMarket";

						if (dtDB_Mst_AreaMarketCheck.Rows.Count > 0)
						{
							alParamsCoupleError.AddRange(new object[]{
								"Check.strAreaCode", strAreaCode
								, "Check.AreaStatus.Expected", TConst.Flag.Inactive
								, "Check.DB.AreaMarketChild", dtDB_Mst_AreaMarketCheck.Rows[0]["AreaCode"]
								, "Check.DB.AreaMarketStatus", dtDB_Mst_AreaMarketCheck.Rows[0]["AreaStatus"]
								});
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Mst_AreaMarket_Update_ExistAreaMarketChildActive
								, null
								, alParamsCoupleError.ToArray()
								);
						}
						////
						if (dtDB_Mst_Distributor.Rows.Count > 0)
						{
							alParamsCoupleError.AddRange(new object[]{
								"Check.strAreaCode", strAreaCode
								, "Check.DB.DBCode", dtDB_Mst_Distributor.Rows[0]["DBCode"]
								, "Check.DB.DBStatus", dtDB_Mst_Distributor.Rows[0]["DBStatus"]
								});
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Mst_AreaMarket_Update_ExistDistributorActive
								, null
								, alParamsCoupleError.ToArray()
								);
						}
						////
						if (dtDB_Mst_Outlet.Rows.Count > 0)
						{
							alParamsCoupleError.AddRange(new object[]{
								"Check.strAreaCode", strAreaCode
								, "Check.DB.OLCode", dtDB_Mst_Outlet.Rows[0]["OLCode"]
								, "Check.DB.OLStatus", dtDB_Mst_Outlet.Rows[0]["OLStatus"]
								});
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Mst_AreaMarket_Update_ExistOutLetActive
								, null
								, alParamsCoupleError.ToArray()
								);
						}
					}
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

		public DataSet Mst_AreaMarket_Delete(
			string strTid
			, DataRow drSession
			////
			, object objAreaCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Mst_AreaMarket_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_AreaMarket_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objAreaCode", objAreaCode
					});
			#endregion

			try
			{
				#region // Convert Input:
				DateTime dtimeTDate = DateTime.Now;
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

				#region // Refine and Check Input:
				////
				string strAreaCode = TUtils.CUtils.StdParam(objAreaCode);
				////
				DataTable dtDB_Mst_AreaMarket = null;
				{
					////
					Mst_AreaMarket_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objAreaCode // objAreaCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // objAreaMarketStatusListToCheck                        
						, out dtDB_Mst_AreaMarket // dtDB_Mst_AreaMarket
						);
					////
				}
				#endregion

				#region // SaveDB AreaMarket:
				{
					// Init:
					dtDB_Mst_AreaMarket.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_AreaMarket"
						, dtDB_Mst_AreaMarket
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

		#region // Mst_Distributor:
		private void Mst_Distributor_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objDBCode
			, string strFlagExistToCheck
			, string objDBStatusListToCheck
			, out DataTable dtDB_Mst_Distributor
			)
		{
			// GetInfo:
			dtDB_Mst_Distributor = TDALUtils.DBUtils.GetTableContents(
				_cf.db // db
				, "Mst_Distributor" // strTableName
				, "top 1 *" // strColumnList
				, "" // strClauseOrderBy
				, "DBCode", "=", objDBCode // arrobjParamsTriple item
				);

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_Distributor.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.DBCode", objDBCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_Distributor_CheckDB_DBCodeNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_Distributor.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.DBCode", objDBCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_Distributor_CheckDB_DBCodeExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// objDistributorStatusListToCheck:
			if (objDBStatusListToCheck.Length > 0 && !objDBStatusListToCheck.Contains(Convert.ToString(dtDB_Mst_Distributor.Rows[0]["DBStatus"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.DBCode", objDBCode
					, "Check.DBStatusListToCheck", objDBStatusListToCheck
					, "DB.DBStatus", dtDB_Mst_Distributor.Rows[0]["DBStatus"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_Distributor_CheckDB_DBStatusNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		private void Mst_Distributor_UpdBU()
		{
			string sqlPostSave = CmUtils.StringUtils.Replace(@"
                    declare @strDBCode_Root nvarchar(100); select @strDBCode_Root = 'DVN';
    
                    update t
                    set
	                    t.DBBUCode = @strDBCode_Root
	                    , t.DBBUPattern = @strDBCode_Root + '%'
	                    , t.DBLevel = 1
                    from Mst_Distributor t
	                    left join Mst_Distributor t_Parent
		                    on t.DBCodeParent = t_Parent.DBCode
                    where (1=1)
	                    and t.DBCode in (@strDBCode_Root)
                    ;

                    declare @nDeepDistributor int; select @nDeepDistributor = 0;
                    while (@nDeepDistributor <= 6)
                    begin
	                    select @nDeepDistributor = @nDeepDistributor + 1;
	
	                    update t
	                    set
		                    t.DBBUCode = IsNull(t_Parent.DBBUCode + '.', '') + t.DBCode
		                    , t.DBBUPattern = IsNull(t_Parent.DBBUCode + '.', '') + t.DBCode + '%'
		                    , t.DBLevel = IsNull(t_Parent.DBLevel, 0) + 1
	                    from Mst_Distributor t
		                    left join Mst_Distributor t_Parent
			                    on t.DBCodeParent = t_Parent.DBCode
	                    where (1=1)
		                    and t.DBCode not in (@strDBCode_Root)
	                    ;
                    end;
                ");
			DataSet dsPostSave = _cf.db.ExecQuery(sqlPostSave);
		}

		public DataSet Mst_Distributor_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_Distributor
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Mst_Distributor_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Distributor_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_Distributor", strRt_Cols_Mst_Distributor
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
				bool bGet_Mst_Distributor = (strRt_Cols_Mst_Distributor != null && strRt_Cols_Mst_Distributor.Length > 0);

				// drAbilityOfUser:
				DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);

				#endregion

				#region // Build Sql:
				ArrayList alParamsCoupleSql = new ArrayList();
				//alParamsCoupleSql.AddRange(new object[] { "@strAbilityOfUser", drAbilityOfUser["MBBankBUPattern"] });
				alParamsCoupleSql.AddRange(new object[] { 
					"@nFilterRecordStart", nFilterRecordStart
					, "@nFilterRecordEnd", nFilterRecordEnd
					, "@Today", DateTime.Today.ToString("yyyy-MM-dd")
					});
				myCache_Mst_Distributor_ViewAbility_Get(drAbilityOfUser);

				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Mst_Distributor_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, md.DBCode
						into #tbl_Mst_Distributor_Filter_Draft
						from #tbl_Mst_Distributor_ViewAbility va_md --//[mylock]
							inner join Mst_Distributor md --//[mylock]
								on va_md.DBCode = md.DBCode
							left join Mst_AreaMarket mam --//[mylock]
								on md.AreaCode = mam.AreaCode
						where (1=1)
							zzzzClauseWhere_strFilterWhereClause
						order by md.DBCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_Distributor_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_Distributor_Filter:
						select
							t.*
						into #tbl_Mst_Distributor_Filter
						from #tbl_Mst_Distributor_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_Distributor --------:
						zzzzClauseSelect_Mst_Distributor_zOut
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_Distributor_Filter_Draft;
						--drop table #tbl_Mst_Distributor_Filter;
					"
					);
				////
				string zzzzClauseSelect_Mst_Distributor_zOut = "-- Nothing.";
				if (bGet_Mst_Distributor)
				{
					#region // bGet_Mst_Distributor:
					zzzzClauseSelect_Mst_Distributor_zOut = CmUtils.StringUtils.Replace(@"
							---- Mst_Distributor:
							select
								t.MyIdxSeq
								, md.*
								, mam.AreaCode mam_AreaCode 
								, mam.AreaCodeParent mam_AreaCodeParent 
								, mam.AreaBUCode mam_AreaBUCode 
								, mam.AreaBUPattern mam_AreaBUPattern 
								, mam.AreaLevel mam_AreaLevel 
								, mam.AreaDesc mam_AreaDesc 
								, mam.AreaStatus mam_AreaStatus 
							from #tbl_Mst_Distributor_Filter t --//[mylock]
								inner join Mst_Distributor md --//[mylock]
									on t.DBCode = md.DBCode
								left join Mst_AreaMarket mam --//[mylock]
									on md.AreaCode = mam.AreaCode
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzzzClauseWhere_strFilterWhereClause = "";
				{
					Hashtable htSpCols = new Hashtable();
					{
						#region // htSpCols:
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_Distributor" // strTableNameDB
							, "Mst_Distributor." // strPrefixStd
							, "md." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_AreaMarket" // strTableNameDB
							, "Mst_AreaMarket." // strPrefixStd
							, "mam." // strPrefixAlias
							);
						////
						#endregion
					}
					zzzzClauseWhere_strFilterWhereClause = CmUtils.SqlUtils.BuildWhere(
						htSpCols // htSpCols
						, strFt_WhereClause // strClause
						, "@p_" // strParamPrefix
						, ref alParamsCoupleSql // alParamsCoupleSql
						);
					zzzzClauseWhere_strFilterWhereClause = (zzzzClauseWhere_strFilterWhereClause.Length <= 0 ? "" : string.Format(" and ({0})", zzzzClauseWhere_strFilterWhereClause));
					alParamsCoupleError.AddRange(new object[]{
						"zzzzClauseWhere_strFilterWhereClause", zzzzClauseWhere_strFilterWhereClause
						});
				}

				strSqlGetData = CmUtils.StringUtils.Replace(
					strSqlGetData
					, "zzzzClauseWhere_strFilterWhereClause", zzzzClauseWhere_strFilterWhereClause
					, "zzzzClauseSelect_Mst_Distributor_zOut", zzzzClauseSelect_Mst_Distributor_zOut
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_Distributor)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_Distributor";
				}
				CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dsGetData);
				#endregion

				// Return Good:
				TDALUtils.DBUtils.RollbackSafety(_cf.db);
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

		public DataSet Mst_Distributor_Create(
			string strTid
			, DataRow drSession
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
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_Distributor_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Distributor_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					, "objDBCode", objDBCode
                    , "objDBCodeParent", objDBCodeParent
                    , "objDBName", objDBName
                    , "objAreaCode", objAreaCode
                    , "objDBAddress", objDBAddress
                    , "objDBContactName", objDBContactName
                    , "objDBPhoneNo", objDBPhoneNo
                    , "objDBFaxNo", objDBFaxNo
                    , "objDBMobilePhoneNo", objDBMobilePhoneNo
                    , "objDBSMSPhoneNo", objDBSMSPhoneNo
                    , "objDBTaxCode", objDBTaxCode
                    , "objRemark", objRemark
                    , "objDiscountPercent", objDiscountPercent
                    , "objBalance", objBalance
                    , "objOverdraftThreshold", objOverdraftThreshold
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
				string strDBCode = TUtils.CUtils.StdParam(objDBCode);
				string strDBCodeParent = TUtils.CUtils.StdParam(objDBCodeParent);
				string strDBName = string.Format("{0}", objDBName).Trim();
				string strAreaCode = TUtils.CUtils.StdParam(objAreaCode);
				string strProvinceCode = TUtils.CUtils.StdParam(objProvinceCode);
				string strDBAddress = string.Format("{0}", objDBAddress).Trim();
				string strDBContactName = string.Format("{0}", objDBContactName).Trim();
				string strDBPhoneNo = string.Format("{0}", objDBPhoneNo).Trim();
				string strDBFaxNo = string.Format("{0}", objDBFaxNo).Trim();
				string strDBMobilePhoneNo = string.Format("{0}", objDBMobilePhoneNo).Trim();
				string strDBSMSPhoneNo = string.Format("{0}", objDBSMSPhoneNo).Trim();
				string strDBTaxCode = string.Format("{0}", objDBTaxCode).Trim();
				string strRemark = string.Format("{0}", objRemark).Trim();
				double douDiscountPercent = Convert.ToDouble(string.Format("{0}", objDiscountPercent).Trim());
				double douBalance = Convert.ToDouble(string.Format("{0}", objBalance).Trim());
				double douOverdraftThreshold = Convert.ToDouble(string.Format("{0}", objOverdraftThreshold).Trim());
				////
				DataTable dtDB_Mst_Distributor = null;
				DataTable dtDB_Mst_Distributor_Parent = null;
				DataTable dtDB_Mst_AreaMarket = null;
				DataTable dtDB_Mst_Province = null;
				{
					////
					if (strDBCode == null || strDBCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strDBCode", strDBCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_Distributor_Create_InvalidDBCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_Distributor_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objDBCode // objAreaCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // objDistributorStatusListToCheck
						, out dtDB_Mst_Distributor // dtDB_Mst_Distributor
						);
					////
					if (strDBCodeParent.Length > 0)
					{
						Mst_Distributor_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, objDBCodeParent // objAreaCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.StatusCm.Yes // objDistributorStatusListToCheck
							, out dtDB_Mst_Distributor_Parent // dtDB_Mst_Distributor
						);
					}

					////
					if (strAreaCode.Length > 0)
					{
						Mst_AreaMarket_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, objAreaCode // objAreaCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.StatusCm.Yes // objDistributorStatusListToCheck                            
							, out dtDB_Mst_AreaMarket // dtDB_Mst_Distributor
						);
					}
					////
					Mst_Province_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strProvinceCode // objProvinceCode
						, TConst.Flag.Yes // strFlagActiveListToCheck
						, TConst.Flag.Active // strFlagActiveListToCheck
						, out dtDB_Mst_Province // dtDB_Mst_Province
						);
					////
					if (strDBName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strDBName", strDBName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_Distributor_Create_InvalidDBName
							, null
							, alParamsCoupleError.ToArray()
							);
					}


				}
				#endregion

				#region // SaveDB Distributor:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_Distributor.NewRow();
					strFN = "DBCode"; drDB[strFN] = strDBCode;
					strFN = "DBCodeParent"; drDB[strFN] = strDBCodeParent;
					strFN = "DBBUCode"; drDB[strFN] = "X";
					strFN = "DBBUPattern"; drDB[strFN] = "X";
					strFN = "DBLevel"; drDB[strFN] = 1;
					strFN = "DBName"; drDB[strFN] = strDBName;
					strFN = "ProvinceCode"; drDB[strFN] = strProvinceCode;
					strFN = "AreaCode"; drDB[strFN] = strAreaCode;
					strFN = "DBAddress"; drDB[strFN] = strDBAddress;
					strFN = "DBContactName"; drDB[strFN] = strDBContactName;
					strFN = "DBPhoneNo"; drDB[strFN] = strDBPhoneNo;
					strFN = "DBFaxNo"; drDB[strFN] = strDBFaxNo;
					strFN = "DBMobilePhoneNo"; drDB[strFN] = strDBMobilePhoneNo;
					strFN = "DBSMSPhoneNo"; drDB[strFN] = strDBSMSPhoneNo;
					strFN = "DBTaxCode"; drDB[strFN] = strDBTaxCode;
					strFN = "Remark"; drDB[strFN] = strRemark;
					strFN = "DiscountPercent"; drDB[strFN] = douDiscountPercent;
					strFN = "Balance"; drDB[strFN] = douBalance;
					strFN = "OverdraftThreshold"; drDB[strFN] = douOverdraftThreshold;

					strFN = "DBStatus"; drDB[strFN] = TConst.StatusCm.Yes;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_Distributor.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_Distributor"
						, dtDB_Mst_Distributor
						//, alColumnEffective.ToArray()
						);
				}
				#endregion

				#region // Post Save:
				{
					Mst_Distributor_UpdBU();
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

		public DataSet Mst_Distributor_Update(
		   string strTid
		   , DataRow drSession
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
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_Distributor_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Distributor_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
                    , "objDBCode", objDBCode
                    , "objDBCodeParent", objDBCodeParent                
                    , "objDBName", objDBName
                    , "objAreaCode", objAreaCode
                    , "objProvinceCode", objProvinceCode
                    , "objDBAddress", objDBAddress
                    , "objDBContactName", objDBContactName
                    , "objDBPhoneNo", objDBPhoneNo
                    , "objDBFaxNo", objDBFaxNo
                    , "objDBMobilePhoneNo", objDBMobilePhoneNo
                    , "objDBSMSPhoneNo", objDBSMSPhoneNo
                    , "objDBTaxCode", objDBTaxCode
                    , "objRemark", objRemark
                    , "objDBStatus", objDBStatus
                    , "objDiscountPercent", objDiscountPercent
                    , "objBalance", objBalance
                    , "objOverdraftThreshold", objOverdraftThreshold
					, "objFt_Cols_Upd", objFt_Cols_Upd
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
				string strFt_Cols_Upd = TUtils.CUtils.StdParam(objFt_Cols_Upd);
				strFt_Cols_Upd = (strFt_Cols_Upd == null ? "" : strFt_Cols_Upd);
				////
				string strDBCode = TUtils.CUtils.StdParam(objDBCode);
				string strDBCodeParent = TUtils.CUtils.StdParam(objDBCodeParent);
				string strDBName = string.Format("{0}", objDBName).Trim();
				string strAreaCode = TUtils.CUtils.StdParam(objAreaCode);
				string strProvinceCode = TUtils.CUtils.StdParam(objProvinceCode);
				string strDBAddress = string.Format("{0}", objDBAddress).Trim();
				string strDBContactName = string.Format("{0}", objDBContactName).Trim();
				string strDBPhoneNo = string.Format("{0}", objDBPhoneNo).Trim();
				string strDBFaxNo = string.Format("{0}", objDBFaxNo).Trim();
				string strDBMobilePhoneNo = string.Format("{0}", objDBMobilePhoneNo).Trim();
				string strDBSMSPhoneNo = string.Format("{0}", objDBSMSPhoneNo).Trim();
				string strDBTaxCode = string.Format("{0}", objDBTaxCode).Trim();
				string strRemark = string.Format("{0}", objRemark).Trim();
				double douDiscountPercent = Convert.ToDouble(string.Format("{0}", objDiscountPercent).Trim());
				double douBalance = Convert.ToDouble(string.Format("{0}", objBalance).Trim());
				double douOverdraftThreshold = Convert.ToDouble(string.Format("{0}", objOverdraftThreshold).Trim());

				string strDBStatus = (CmUtils.StringUtils.StringEqual(objDBStatus, TConst.StatusCm.No) ? TConst.StatusCm.No : TConst.StatusCm.Yes);
				////                
				bool bUpd_DBCode = strFt_Cols_Upd.Contains("Mst_Distributor.DBCode".ToUpper());
				bool bUpd_DBCodeParent = strFt_Cols_Upd.Contains("Mst_Distributor.DBCodeParent".ToUpper());
				bool bUpd_DBBUCode = strFt_Cols_Upd.Contains("Mst_Distributor.DBBUCode".ToUpper());
				bool bUpd_DBBUPattern = strFt_Cols_Upd.Contains("Mst_Distributor.DBBUPattern".ToUpper());
				bool bUpd_DBLevel = strFt_Cols_Upd.Contains("Mst_Distributor.DBLevel".ToUpper());
				bool bUpd_DBName = strFt_Cols_Upd.Contains("Mst_Distributor.DBName".ToUpper());
				bool bUpd_AreaCode = strFt_Cols_Upd.Contains("Mst_Distributor.AreaCode".ToUpper());
				bool bUpd_ProvinceCode = strFt_Cols_Upd.Contains("Mst_Distributor.ProvinceCode".ToUpper());
				bool bUpd_DBAddress = strFt_Cols_Upd.Contains("Mst_Distributor.DBAddress".ToUpper());
				bool bUpd_DBContactName = strFt_Cols_Upd.Contains("Mst_Distributor.DBContactName".ToUpper());
				bool bUpd_DBPhoneNo = strFt_Cols_Upd.Contains("Mst_Distributor.DBPhoneNo".ToUpper());
				bool bUpd_DBFaxNo = strFt_Cols_Upd.Contains("Mst_Distributor.DBFaxNo".ToUpper());
				bool bUpd_DBMobilePhoneNo = strFt_Cols_Upd.Contains("Mst_Distributor.DBMobilePhoneNo".ToUpper());
				bool bUpd_DBSMSPhoneNo = strFt_Cols_Upd.Contains("Mst_Distributor.DBSMSPhoneNo".ToUpper());
				bool bUpd_DBTaxCode = strFt_Cols_Upd.Contains("Mst_Distributor.DBTaxCode".ToUpper());
				bool bUpd_Remark = strFt_Cols_Upd.Contains("Mst_Distributor.Remark".ToUpper());
				bool bUpd_DBStatus = strFt_Cols_Upd.Contains("Mst_Distributor.DBStatus".ToUpper());
				bool bUpd_DiscountPercent = strFt_Cols_Upd.Contains("Mst_Distributor.DiscountPercent".ToUpper());
				bool bUpd_Balance = strFt_Cols_Upd.Contains("Mst_Distributor.Balance".ToUpper());
				bool bUpd_OverdraftThreshold = strFt_Cols_Upd.Contains("Mst_Distributor.OverdraftThreshold".ToUpper());
				////
				DataTable dtDB_Mst_Distributor = null;
				DataTable dtDB_Mst_Distributor_Parent = null;
				DataTable dtDB_Mst_AreaMarket = null;
				DataTable dtDB_Mst_Province = null;
				{
					////
					Mst_Distributor_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objDBCode // objDBCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // objDistributorStatusListToCheck
						, out dtDB_Mst_Distributor // dtDB_Mst_Distributor
						);
					////
					if (bUpd_DBCodeParent && strDBCodeParent.Length > 0)
					{
						Mst_Distributor_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, objDBCodeParent // objAreaCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.StatusCm.Yes // objDistributorStatusListToCheck
							, out dtDB_Mst_Distributor_Parent // dtDB_Mst_Distributor
						);
					}

					////
					if (bUpd_AreaCode && strAreaCode.Length > 0)
					{
						Mst_AreaMarket_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, objAreaCode // objAreaCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.StatusCm.Yes // objDistributorStatusListToCheck                            
							, out dtDB_Mst_AreaMarket // dtDB_Mst_Distributor
						);
					}
					////
					if (bUpd_ProvinceCode && strProvinceCode.Length > 1)
					{
						Mst_Province_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, strProvinceCode // objProvinceCode
							, TConst.Flag.Yes // strFlagActiveListToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_Province // dtDB_Mst_Province
							);
					}
					////
					if (bUpd_DBName && strDBName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strDBName", strDBName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_Distributor_Update_InvalidDBName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (bUpd_DBStatus && CmUtils.StringUtils.StringEqual(strDBStatus, TConst.Flag.Inactive))
					{
						string strSqlCheck = CmUtils.StringUtils.Replace(@"
									---- Mst_Outlet:
									select top 1
										t.*
									from Mst_Outlet t --//[mylock]
									where (1=1)
										and t.DBCode = @strDBCode
										and t.OLStatus = '1'
									;
							");
						DataTable dtDB_Check = _cf.db.ExecQuery(
							strSqlCheck
							, "@strDBCode", strDBCode
							).Tables[0];
						if (dtDB_Check.Rows.Count > 0)
						{
							alParamsCoupleError.AddRange(new object[]{
								"Check.DB.DBCode", strDBCode
								, "Check.DB.OLCode", dtDB_Check.Rows[0]["OLCode"]
								, "Check.DB.OLCode", dtDB_Check.Rows[0]["OLStatus"]
								});
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Mst_Distributor_Update_ExistOutletActive
								, null
								, alParamsCoupleError.ToArray()
								);
						}
					}
				}
				#endregion

				#region // SaveDB Mst_Distributor:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_Distributor.Rows[0];
					if (bUpd_DBCode) { strFN = "DBCode"; drDB[strFN] = objDBCode; alColumnEffective.Add(strFN); }
					if (bUpd_DBCodeParent) { strFN = "DBCodeParent"; drDB[strFN] = objDBCodeParent; alColumnEffective.Add(strFN); }
					if (bUpd_DBName) { strFN = "DBName"; drDB[strFN] = objDBName; alColumnEffective.Add(strFN); }
					if (bUpd_AreaCode) { strFN = "AreaCode"; drDB[strFN] = strAreaCode; alColumnEffective.Add(strFN); }
					if (bUpd_ProvinceCode) { strFN = "ProvinceCode"; drDB[strFN] = strProvinceCode; alColumnEffective.Add(strFN); }
					if (bUpd_DBAddress) { strFN = "DBAddress"; drDB[strFN] = objDBAddress; alColumnEffective.Add(strFN); }
					if (bUpd_DBContactName) { strFN = "DBContactName"; drDB[strFN] = objDBContactName; alColumnEffective.Add(strFN); }
					if (bUpd_DBPhoneNo) { strFN = "DBPhoneNo"; drDB[strFN] = objDBPhoneNo; alColumnEffective.Add(strFN); }
					if (bUpd_DBFaxNo) { strFN = "DBFaxNo"; drDB[strFN] = objDBFaxNo; alColumnEffective.Add(strFN); }
					if (bUpd_DBMobilePhoneNo) { strFN = "DBMobilePhoneNo"; drDB[strFN] = objDBMobilePhoneNo; alColumnEffective.Add(strFN); }
					if (bUpd_DBSMSPhoneNo) { strFN = "DBSMSPhoneNo"; drDB[strFN] = objDBSMSPhoneNo; alColumnEffective.Add(strFN); }
					if (bUpd_DBTaxCode) { strFN = "DBTaxCode"; drDB[strFN] = objDBTaxCode; alColumnEffective.Add(strFN); }
					if (bUpd_Remark) { strFN = "Remark"; drDB[strFN] = objRemark; alColumnEffective.Add(strFN); }
					if (bUpd_DiscountPercent) { strFN = "DiscountPercent"; drDB[strFN] = objDiscountPercent; alColumnEffective.Add(strFN); }
					if (bUpd_Balance) { strFN = "Balance"; drDB[strFN] = objBalance; alColumnEffective.Add(strFN); }
					if (bUpd_OverdraftThreshold) { strFN = "OverdraftThreshold"; drDB[strFN] = objOverdraftThreshold; alColumnEffective.Add(strFN); }
					if (bUpd_DBStatus) { strFN = "DBStatus"; drDB[strFN] = strDBStatus; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_Distributor"
						, dtDB_Mst_Distributor
						, alColumnEffective.ToArray()
						);
				}
				#endregion

				#region // Post Save:
				if (bUpd_DBCodeParent)
				{
					Mst_Distributor_UpdBU();
				}
				#endregion

				#region // Check OL In DB:
				{
					if (bUpd_DBStatus && CmUtils.StringUtils.StringEqual(strDBStatus, TConst.Flag.Inactive))
					{
						string strSqlGet_Mst_Outlet = CmUtils.StringUtils.Replace(@"
								select 
									mo.*
								from Mst_Outlet mo --//[mylock]
									inner join Mst_Distributor md --//[mylock]
										on mo.DBCode = md.DBCode
								where (1=1)
									and mo.DBCode = '@strDBCode'
									and mo.OLStatus = '@strFlagActive'
							;
							"
							, "@strDBCode", strDBCode
							, "@strFlagActive", TConst.Flag.Active
							);
						DataTable dtDB_Mst_Outlet = _cf.db.ExecQuery(strSqlGet_Mst_Outlet).Tables[0];

						if (dtDB_Mst_Outlet.Rows.Count > 0)
						{
							alParamsCoupleError.AddRange(new object[]{
								"Check.strDBCode", strDBCode
								, "Check.DB.OLCode", dtDB_Mst_Outlet.Rows[0]["OLCode"]
								});
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Mst_Distributor_Update_ExistOutletActive
								, null
								, alParamsCoupleError.ToArray()
								);
						}
					}
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

		public DataSet Mst_Distributor_Delete(
			string strTid
			, DataRow drSession
			////
			, object objDBCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_Distributor_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Distributor_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
					, "objDBCode", objDBCode
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
				string strDBCode = TUtils.CUtils.StdParam(objDBCode);
				////
				DataTable dtDB_Mst_Distributor = null;
				{
					////
					Mst_Distributor_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objDBCode // objDBCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // objDBStatusListToCheck                    
						, out dtDB_Mst_Distributor // dtDB_Mst_Distributor
						);
					////
				}
				#endregion

				#region // SaveDB Mst_Distributor:
				{
					// Init:
					dtDB_Mst_Distributor.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_Distributor"
						, dtDB_Mst_Distributor
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

		#region // Mst_Outlet:
		private void Mst_Outlet_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objOLCode
			, string strFlagExistToCheck
			, string objOutletStatusListToCheck
			, out DataTable dtDB_Mst_Outlet
			)
		{
			// GetInfo:
			dtDB_Mst_Outlet = TDALUtils.DBUtils.GetTableContents(
				_cf.db // db
				, "Mst_Outlet" // strTableName
				, "top 1 *" // strColumnList
				, "" // strClauseOrderBy
				, "OLCode", "=", objOLCode // arrobjParamsTriple item
				);

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_Outlet.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.OLCode", objOLCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_Outlet_CheckDB_OutletNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_Outlet.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.OLCode", objOLCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_Outlet_CheckDB_OutletExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// objOutletStatusListToCheck:
			if (objOutletStatusListToCheck.Length > 0 && !objOutletStatusListToCheck.Contains(Convert.ToString(dtDB_Mst_Outlet.Rows[0]["OLStatus"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.OLCode", objOLCode
					, "Check.OutletStatusListToCheck", objOutletStatusListToCheck
					, "DB.OLStatus", dtDB_Mst_Outlet.Rows[0]["OLStatus"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_Outlet_CheckDB_OutletStatusNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}

		public DataSet Mst_Outlet_Get(
			string strTid
			, DataRow drSession
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
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			bool bNeedTransaction = true;
			string strFunctionName = "Mst_Outlet_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Outlet_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_Outlet", strRt_Cols_Mst_Outlet
                    , "strRt_Cols_Mst_StarShopHist", strRt_Cols_Mst_StarShopHist
                    , "strRt_Cols_Mst_StarShopHist", strRt_Cols_OL_SignBoardsHist
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
				bool bGet_Mst_Outlet = (strRt_Cols_Mst_Outlet != null && strRt_Cols_Mst_Outlet.Length > 0);
				bool bGet_Mst_StarShopHist = (strRt_Cols_Mst_StarShopHist != null && strRt_Cols_Mst_StarShopHist.Length > 0);
				bool bGet_OL_SignBoardsHist = (strRt_Cols_OL_SignBoardsHist != null && strRt_Cols_OL_SignBoardsHist.Length > 0);

				// drAbilityOfUser:
				DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);

				#endregion

				#region // Build Sql:
				ArrayList alParamsCoupleSql = new ArrayList();
				//alParamsCoupleSql.AddRange(new object[] { "@strAbilityOfUser", drAbilityOfUser["MBBankBUPattern"] });
				alParamsCoupleSql.AddRange(new object[] { 
					"@nFilterRecordStart", nFilterRecordStart
					, "@nFilterRecordEnd", nFilterRecordEnd
					, "@Today", DateTime.Today.ToString("yyyy-MM-dd")
					});
				string zzzzClauseSelect_Mst_Outlet_ViewAbility = zzzzClauseSelect_Mst_Outlet_ViewAbility_Get(
					drAbilityOfUser // drAbilityOfUser
					, ref alParamsCoupleSql // alParamsCoupleSql
					);


				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- ViewAbility:
						zzzzClauseSelect_Mst_Outlet_ViewAbility

						---- #tbl_Mst_Outlet_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mo.OLCode
						into #tbl_Mst_Outlet_Filter_Draft
						from #tbl_Mst_Outlet_ViewAbility va_mo --//[mylock]
							inner join Mst_Outlet mo --//[mylock]
								on va_mo.OLCode = mo.OLCode
                            left join Mst_StarShopHist mssh --//[mylock]
		                        on  mo.OLCode = mssh.OLCode
                            left join OL_SignBoardsHist osbh --//[mylock]
		                        on  mo.OLCode = osbh.OLCode
                            left join Mst_StarShopType msst --//[mylock]
		                        on mssh.SSGrpCode = msst.SSGrpCode
			                        and mssh.SSBrandCode = msst.SSBrandCode
							left join Mst_Distributor md --//[mylock]
								on mo.DBCode = md.DBCode
							left join Mst_AreaMarket mam --//[mylock]
								on md.AreaCode = mam.AreaCode
						where (1=1)
							zzzzClauseWhere_strFilterWhereClause
						order by mo.OLCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_Outlet_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_Outlet_Filter:
						select
							t.*
						into #tbl_Mst_Outlet_Filter
						from #tbl_Mst_Outlet_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_Outlet --------:
						zzzzClauseSelect_Mst_Outlet_zOut
						----------------------------------------

                        -------- Mst_StarShopHist --------:
						zzzzClauseSelect_Mst_StarShopHist_zOut
						----------------------------------------

                        -------- OL_SignBoardsHist --------:
						zzzzClauseSelect_OL_SignBoardsHist_zOut
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_Outlet_Filter_Draft;
						--drop table #tbl_Mst_Outlet_Filter;
					"
					, "zzzzClauseSelect_Mst_Outlet_ViewAbility", zzzzClauseSelect_Mst_Outlet_ViewAbility
					);
				////
				string zzzzClauseSelect_Mst_Outlet_zOut = "-- Nothing.";
				if (bGet_Mst_Outlet)
				{
					#region // bGet_Mst_Outlet:
					zzzzClauseSelect_Mst_Outlet_zOut = CmUtils.StringUtils.Replace(@"
							---- #tbl_Mst_StarShopHist:
							select 
								t.OLCode
								, mssh.SSGrpCode
								, mssh.SSBrandCode
								, msst.SSTypeName
							into #tbl_Mst_StarShopHist
							from Mst_Outlet t --//[mylock]
								inner join Mst_StarShopHist mssh --//[mylock]
									on t.OLCode = mssh.OLCode
								left join Mst_StarShopType msst --//[mylock]
									on mssh.SSGrpCode = msst.SSGrpCode
										and mssh.SSBrandCode = msst.SSBrandCode
							where (1=1)
								and mssh.EffDateStart <= '@strSysDate' and '@strSysDate' <= mssh.EffDateEnd
							;
							
							-- select null #tbl_Mst_StarShopHist , * from #tbl_Mst_StarShopHist t --//[mylock];

							---- Mst_Outlet:
							select
								t.MyIdxSeq
								, mo.*
                                --, SMCode = (select top 1 uvo.UserCode from [Sys_UserViewOutlet] uvo where uvo.OLCode=mo.OLCode)
                                --, SMName = (select top 1 u.UserName from [Sys_User] u  where u.UserCode in (select top 1 uvo.UserCode from [Sys_UserViewOutlet] uvo where uvo.OLCode=mo.OLCode))
                                , md.DBCode md_DBCode
	                            , md.DBName md_DBName
	                            , mam.AreaCode mam_AreaCode
	                            , mam.AreaDesc mam_AreaDesc
								, (
									case 
										when msst.OLCode is not null then msst.SSGrpCode
										else null
									end 	
								) msst_SSGrpCode
								, (
									case 
										when msst.OLCode is not null then msst.SSBrandCode
										else null
									end 	
								) msst_SSBrandCode 
								, (
									case 
										when msst.OLCode is not null then msst.SSTypeName
										else null
									end 	
								) msst_SSTypeName
							from #tbl_Mst_Outlet_Filter t --//[mylock]
								inner join Mst_Outlet mo --//[mylock]
									on t.OLCode = mo.OLCode
								left join #tbl_Mst_StarShopHist msst --//[mylock]
									on mo.OLCode = msst.OLCode
                                left join Mst_Distributor md --//[mylock]
		                            on mo.DBCode = md.DBCode
	                            left join Mst_AreaMarket mam --//[mylock]
		                            on md.AreaCode = mam.AreaCode
							order by t.MyIdxSeq asc
							;
						"
						, "@strSysDate", dtimeSys.ToString("yyyy-MM-dd")
						);
					#endregion
				}
				////
				string zzzzClauseSelect_Mst_StarShopHist_zOut = "-- Nothing.";
				if (bGet_Mst_StarShopHist)
				{
					#region // bGet_Mst_StarShopHist:
					zzzzClauseSelect_Mst_StarShopHist_zOut = CmUtils.StringUtils.Replace(@"
							---- Mst_Outlet:
							select
								t.MyIdxSeq
								, mssh.*
                                , mo.OLCode mo_OLCode
                                , mo.OLName mo_OLName
								, mo.OLAddress mo_OLAddress
								, mo.OLOwnerName mo_OLOwnerName
								, mo.OLOwnerDateBirth mo_OLOwnerDateBirth
								, mo.OLOwnerSignImagePath mo_OLOwnerSignImagePath
								, mo.OLTaxCode mo_OLTaxCode
								, mo.OLOwnerIdCardNo mo_OLOwnerIdCardNo
								, mo.OLOwnerIdCardFrontImagePath mo_OLOwnerIdCardFrontImagePath
								, mo.OLOwnerIdCardRearImagePath mo_OLOwnerIdCardRearImagePath
								, mo.OLOwnerIdAddress mo_OLOwnerIdAddress
								, mo.OLPhoneNo mo_OLPhoneNo
								, mo.OLFaxNo mo_OLFaxNo
								, mo.OLMobilePhoneNo mo_OLMobilePhoneNo
								, mo.OLSMSPhoneNo mo_OLSMSPhoneNo
								, mo.OLBankAccountNo mo_OLBankAccountNo
								, mo.OLBankName mo_OLBankName
								, mo.Remark mo_Remark
								, mo.OLStatus mo_OLStatus
								, msst.SSTypeName msst_SSTypeName
								, md.DBCode md_DBCode
								, md.DBName md_DBName
							from #tbl_Mst_Outlet_Filter t --//[mylock]
								inner join Mst_Outlet mo --//[mylock]
									on t.OLCode = mo.OLCode
                                inner join Mst_StarShopHist mssh --//[mylock]
		                            on  mo.OLCode = mssh.OLCode
								inner join Mst_StarShopType msst --//[mylock]
									on mssh.SSGrpCode = msst.SSGrpCode
										and  mssh.SSBrandCode = msst.SSBrandCode 
                                left join Mst_Distributor md --//[mylock]
		                            on mo.DBCode = md.DBCode
	                            left join Mst_AreaMarket mam --//[mylock]
		                            on md.AreaCode = mam.AreaCode
							where (1=1)
								zzzzClauseWhere_strFilterWhereClause
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzzzClauseSelect_OL_SignBoardsHist_zOut = "-- Nothing.";
				if (bGet_OL_SignBoardsHist)
				{
					#region // bGet_OL_SignBoardsHist:
					zzzzClauseSelect_OL_SignBoardsHist_zOut = CmUtils.StringUtils.Replace(@"
							---- Mst_Outlet:
							select
								t.MyIdxSeq
								, osbh.*
                                , mo.OLCode mo_OLCode
                                , mo.OLName mo_OLName
							from #tbl_Mst_Outlet_Filter t --//[mylock]
								inner join Mst_Outlet mo --//[mylock]
									on t.OLCode = mo.OLCode
                                inner join OL_SignBoardsHist osbh --//[mylock]
		                            on  mo.OLCode = osbh.OLCode
                                left join Mst_Distributor md --//[mylock]
		                            on mo.DBCode = md.DBCode
	                            left join Mst_AreaMarket mam --//[mylock]
		                            on md.AreaCode = mam.AreaCode
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzzzClauseWhere_strFilterWhereClause = "";
				{
					Hashtable htSpCols = new Hashtable();
					{
						#region // htSpCols:
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_Outlet" // strTableNameDB
							, "Mst_Outlet." // strPrefixStd
							, "mo." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_Distributor" // strTableNameDB
							, "Mst_Distributor." // strPrefixStd
							, "md." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_AreaMarket" // strTableNameDB
							, "Mst_AreaMarket." // strPrefixStd
							, "mam." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_StarShopHist" // strTableNameDB
							, "Mst_StarShopHist." // strPrefixStd
							, "mssh." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Mst_StarShopType" // strTableNameDB
							, "Mst_StarShopType." // strPrefixStd
							, "msst." // strPrefixAlias
							);

						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "OL_SignBoardsHist" // strTableNameDB
							, "OL_SignBoardsHist." // strPrefixStd
							, "osbh." // strPrefixAlias
							);
						////
						#endregion
					}
					zzzzClauseWhere_strFilterWhereClause = CmUtils.SqlUtils.BuildWhere(
						htSpCols // htSpCols
						, strFt_WhereClause // strClause
						, "@p_" // strParamPrefix
						, ref alParamsCoupleSql // alParamsCoupleSql
						);
					zzzzClauseWhere_strFilterWhereClause = (zzzzClauseWhere_strFilterWhereClause.Length <= 0 ? "" : string.Format(" and ({0})", zzzzClauseWhere_strFilterWhereClause));
					alParamsCoupleError.AddRange(new object[]{
						"zzzzClauseWhere_strFilterWhereClause", zzzzClauseWhere_strFilterWhereClause
						});
				}
				////
				if (bGet_Mst_StarShopHist)
				{
					zzzzClauseSelect_Mst_StarShopHist_zOut = CmUtils.StringUtils.Replace(
						zzzzClauseSelect_Mst_StarShopHist_zOut
						, "zzzzClauseWhere_strFilterWhereClause", zzzzClauseWhere_strFilterWhereClause
						);
				}
				////
				strSqlGetData = CmUtils.StringUtils.Replace(
					strSqlGetData
					, "zzzzClauseWhere_strFilterWhereClause", zzzzClauseWhere_strFilterWhereClause
					, "zzzzClauseSelect_Mst_Outlet_zOut", zzzzClauseSelect_Mst_Outlet_zOut
					, "zzzzClauseSelect_Mst_StarShopHist_zOut", zzzzClauseSelect_Mst_StarShopHist_zOut
					, "zzzzClauseSelect_OL_SignBoardsHist_zOut", zzzzClauseSelect_OL_SignBoardsHist_zOut
					);
				
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_Outlet)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_Outlet";
				}
				if (bGet_Mst_StarShopHist)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_StarShopHist";
				}
				if (bGet_OL_SignBoardsHist)
				{
					dsGetData.Tables[nIdxTable++].TableName = "OL_SignBoardsHist";
				}
				CmUtils.DataUtils.MoveDataTable(ref mdsFinal, ref dsGetData);
				#endregion

				// Return Good:
				TDALUtils.DBUtils.RollbackSafety(_cf.db);
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

		public DataSet Mst_Outlet_Create(
			string strTid
			, DataRow drSession
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
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_Outlet_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Outlet_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
					, "objOLCode", objOLCode
					, "objDBCode", objDBCode
                    , "objDistrictCode", objDistrictCode                    
                    , "objOLName", objOLName
                    , "objOLAddress", objOLAddress
                    , "objOLOwnerName", objOLOwnerName
                    , "objOLOwnerDateBirth", objOLOwnerDateBirth
                    , "objOLOwnerSignImage", objOLOwnerSignImagePath
                    , "objOLTaxCode", objOLTaxCode            
                    , "objOLOwnerIdCardNo", objOLOwnerIdCardNo            
                    , "objOLOwnerIdCardFrontImage", objOLOwnerIdCardFrontImagePath            
                    , "objOLOwnerIdCardRearImage", objOLOwnerIdCardRearImagePath            
                    , "objOLOwnerIdAddress", objOLOwnerIdAddress              
                    , "objOLPhoneNo", objOLPhoneNo              
                    , "objOLFaxNo", objOLFaxNo              
                    , "objOLMobilePhoneNo", objOLMobilePhoneNo              
                    , "objOLSMSPhoneNo", objOLSMSPhoneNo              
                    , "objOLBankAccountNo", objOLBankAccountNo                
                    , "objOLBankName", objOLBankName                
                    , "objRemark", objRemark                
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
				string strOLCode = TUtils.CUtils.StdParam(objOLCode);
				string strDBCode = TUtils.CUtils.StdParam(objDBCode);
				string strDistrictCode = TUtils.CUtils.StdParam(objDistrictCode);
				string strOLName = string.Format("{0}", objOLName).Trim();
				string strOLAddress = string.Format("{0}", objOLAddress).Trim();
				string strOLOwnerName = string.Format("{0}", objOLOwnerName).Trim();
				string strOLOwnerDateBirth = TUtils.CUtils.StdDate(objOLOwnerDateBirth);
				string strOLOwnerSignImagePath = string.Format("{0}", objOLOwnerSignImagePath).Trim();
				string strOLTaxCode = string.Format("{0}", objOLTaxCode).Trim();
				string strOLOwnerIdCardNo = string.Format("{0}", objOLOwnerIdCardNo).Trim();
				string strOLOwnerIdCardFrontImagePath = string.Format("{0}", objOLOwnerIdCardFrontImagePath).Trim();
				string strOLOwnerIdCardRearImagePath = string.Format("{0}", objOLOwnerIdCardRearImagePath).Trim();
				string strOLOwnerIdAddress = string.Format("{0}", objOLOwnerIdAddress).Trim();
				string strOLPhoneNo = string.Format("{0}", objOLPhoneNo).Trim();
				string strOLFaxNo = string.Format("{0}", objOLFaxNo).Trim();
				string strOLMobilePhoneNo = string.Format("{0}", objOLMobilePhoneNo).Trim();
				string strOLSMSPhoneNo = string.Format("{0}", objOLSMSPhoneNo).Trim();
				string strOLBankAccountNo = string.Format("{0}", objOLBankAccountNo).Trim();
				string strOLBankName = string.Format("{0}", objOLBankName).Trim();
				string strRemark = string.Format("{0}", objRemark).Trim();
				//string strUserCode = _cf.sinf.strUserCode;

				// drAbilityOfUser:
				DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_Outlet = null;
				{

					////
					if (strOLCode == null || strOLCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strOLCode", strOLCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_Outlet_Create_InvalidOLCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_Outlet_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objOLCode // objAreaCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // objOutletStatusListToCheck
						, out dtDB_Mst_Outlet // dtDB_Mst_Outlet
						);


					////
					if (strOLName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strOLName", strOLName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_Outlet_Create_InvalidOLName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Outlet:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_Outlet.NewRow();
					strFN = "OLCode"; drDB[strFN] = strOLCode;
					strFN = "DBCode"; drDB[strFN] = strDBCode;
					strFN = "DistrictCode"; drDB[strFN] = strDistrictCode;
					strFN = "OLName"; drDB[strFN] = strOLName;
					strFN = "OLAddress"; drDB[strFN] = strOLAddress;
					strFN = "OLOwnerName"; drDB[strFN] = strOLName;
					strFN = "OLOwnerDateBirth"; drDB[strFN] = strOLOwnerDateBirth;
					strFN = "OLOwnerSignImagePath"; drDB[strFN] = strOLOwnerSignImagePath;
					strFN = "OLTaxCode"; drDB[strFN] = strOLTaxCode;
					strFN = "OLOwnerIdCardNo"; drDB[strFN] = strOLOwnerIdCardNo;
					strFN = "OLOwnerIdCardFrontImagePath"; drDB[strFN] = strOLOwnerIdCardFrontImagePath;
					strFN = "OLOwnerIdCardRearImagePath"; drDB[strFN] = strOLOwnerIdCardRearImagePath;
					strFN = "OLOwnerIdAddress"; drDB[strFN] = strOLOwnerIdAddress;
					strFN = "OLPhoneNo"; drDB[strFN] = strOLPhoneNo;
					strFN = "OLFaxNo"; drDB[strFN] = strOLFaxNo;
					strFN = "OLMobilePhoneNo"; drDB[strFN] = strOLMobilePhoneNo;
					strFN = "OLSMSPhoneNo"; drDB[strFN] = strOLSMSPhoneNo;
					strFN = "OLBankAccountNo"; drDB[strFN] = strOLBankAccountNo;
					strFN = "OLBankName"; drDB[strFN] = strOLBankName;
					strFN = "Remark"; drDB[strFN] = strRemark;
					strFN = "OLStatus"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_Outlet.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_Outlet"
						, dtDB_Mst_Outlet
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

		public DataSet Mst_Outlet_Update(
		   string strTid
		   , DataRow drSession
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
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_Outlet_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Outlet_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
					, "objOLCode", objOLCode
					, "objDBCode", objDBCode
                    , "objDistrictCode", objDistrictCode                    
                    , "objOLName", objOLName
                    , "objOLAddress", objOLAddress
                    , "objOLOwnerName", objOLOwnerName
                    , "objOLOwnerDateBirth", objOLOwnerDateBirth
                    , "objOLOwnerSignImage", objOLOwnerSignImagePath
                    , "objOLTaxCode", objOLTaxCode            
                    , "objOLOwnerIdCardNo", objOLOwnerIdCardNo            
                    , "objOLOwnerIdCardFrontImage", objOLOwnerIdCardFrontImagePath            
                    , "objOLOwnerIdCardRearImage", objOLOwnerIdCardRearImagePath            
                    , "objOLOwnerIdAddress", objOLOwnerIdAddress              
                    , "objOLPhoneNo", objOLPhoneNo              
                    , "objOLFaxNo", objOLFaxNo              
                    , "objOLMobilePhoneNo", objOLMobilePhoneNo              
                    , "objOLSMSPhoneNo", objOLSMSPhoneNo              
                    , "objOLBankAccountNo", objOLBankAccountNo                
                    , "objOLBankName", objOLBankName                
                    , "objRemark", objRemark    
                    , "objOLStatus", objOLStatus    
                    , "objFt_Cols_Upd", objFt_Cols_Upd    
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
				string strFt_Cols_Upd = TUtils.CUtils.StdParam(objFt_Cols_Upd);
				strFt_Cols_Upd = (strFt_Cols_Upd == null ? "" : strFt_Cols_Upd);
				////
				string strOLCode = TUtils.CUtils.StdParam(objOLCode);
				string strDBCode = TUtils.CUtils.StdParam(objDBCode);
				string strDistrictCode = TUtils.CUtils.StdParam(objDistrictCode);
				string strOLName = string.Format("{0}", objOLName).Trim();
				string strOLAddress = string.Format("{0}", objOLAddress).Trim();
				string strOLOwnerName = string.Format("{0}", objOLOwnerName).Trim();
				string strOLOwnerDateBirth = TUtils.CUtils.StdDate(objOLOwnerDateBirth);
				string strOLOwnerSignImagePath = string.Format("{0}", objOLOwnerSignImagePath).Trim();
				string strOLTaxCode = string.Format("{0}", objOLTaxCode).Trim();
				string strOLOwnerIdCardNo = string.Format("{0}", objOLOwnerIdCardNo).Trim();
				string strOLOwnerIdCardFrontImagePath = string.Format("{0}", objOLOwnerIdCardFrontImagePath).Trim();
				string strOLOwnerIdCardRearImagePath = string.Format("{0}", objOLOwnerIdCardRearImagePath).Trim();
				string strOLOwnerIdAddress = string.Format("{0}", objOLOwnerIdAddress).Trim();
				string strOLPhoneNo = string.Format("{0}", objOLPhoneNo).Trim();
				string strOLFaxNo = string.Format("{0}", objOLFaxNo).Trim();
				string strOLMobilePhoneNo = string.Format("{0}", objOLMobilePhoneNo).Trim();
				string strOLSMSPhoneNo = string.Format("{0}", objOLSMSPhoneNo).Trim();
				string strOLBankAccountNo = string.Format("{0}", objOLBankAccountNo).Trim();
				string strOLBankName = string.Format("{0}", objOLBankName).Trim();
				string strRemark = string.Format("{0}", objRemark).Trim();
				string strOLStatus = (CmUtils.StringUtils.StringEqual(objOLStatus, TConst.StatusCm.No) ? TConst.StatusCm.No : TConst.StatusCm.Yes);
				////
				DataTable dtDB_Mst_Outlet = null;
				bool bUpd_DBCode = strFt_Cols_Upd.Contains("Mst_Outlet.DBCode".ToUpper());
				bool bUpd_DistrictCode = strFt_Cols_Upd.Contains("Mst_Outlet.DistrictCode".ToUpper());
				bool bUpd_ProvinceCode = strFt_Cols_Upd.Contains("Mst_Outlet.ProvinceCode".ToUpper());
				bool bUpd_OLName = strFt_Cols_Upd.Contains("Mst_Outlet.OLName".ToUpper());
				bool bUpd_OLAddress = strFt_Cols_Upd.Contains("Mst_Outlet.OLAddress".ToUpper());
				bool bUpd_OLOwnerName = strFt_Cols_Upd.Contains("Mst_Outlet.OLOwnerName".ToUpper());
				bool bUpd_OLOwnerDateBirth = strFt_Cols_Upd.Contains("Mst_Outlet.OLOwnerDateBirth".ToUpper());
				bool bUpd_OLOwnerSignImagePath = strFt_Cols_Upd.Contains("Mst_Outlet.OLOwnerSignImagePath".ToUpper());
				bool bUpd_OLTaxCode = strFt_Cols_Upd.Contains("Mst_Outlet.OLTaxCode".ToUpper());
				bool bUpd_OLOwnerIdCardNo = strFt_Cols_Upd.Contains("Mst_Outlet.OLOwnerIdCardNo".ToUpper());
				bool bUpd_OLOwnerIdCardFrontImagePath = strFt_Cols_Upd.Contains("Mst_Outlet.OLOwnerIdCardFrontImagePath".ToUpper());
				bool bUpd_OLOwnerIdCardRearImagePath = strFt_Cols_Upd.Contains("Mst_Outlet.OLOwnerIdCardRearImagePath".ToUpper());
				bool bUpd_OLOwnerIdAddress = strFt_Cols_Upd.Contains("Mst_Outlet.OLOwnerIdAddress".ToUpper());
				bool bUpd_OLPhoneNo = strFt_Cols_Upd.Contains("Mst_Outlet.OLPhoneNo".ToUpper());
				bool bUpd_OLFaxNo = strFt_Cols_Upd.Contains("Mst_Outlet.OLFaxNo".ToUpper());
				bool bUpd_OLMobilePhoneNo = strFt_Cols_Upd.Contains("Mst_Outlet.OLMobilePhoneNo".ToUpper());
				bool bUpd_OLSMSPhoneNo = strFt_Cols_Upd.Contains("Mst_Outlet.OLSMSPhoneNo".ToUpper());
				bool bUpd_OLBankAccountNo = strFt_Cols_Upd.Contains("Mst_Outlet.OLBankAccountNo".ToUpper());
				bool bUpd_OLBankName = strFt_Cols_Upd.Contains("Mst_Outlet.OLBankName".ToUpper());
				bool bUpd_Remark = strFt_Cols_Upd.Contains("Mst_Outlet.Remark".ToUpper());
				bool bUpd_OLStatus = strFt_Cols_Upd.Contains("Mst_Outlet.OLStatus".ToUpper());
				////
				{
					////
					Mst_Outlet_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objOLCode // objOLCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // objOutletStatusListToCheck
						, out dtDB_Mst_Outlet // dtDB_Mst_Outlet
						);



					////
					if (bUpd_OLName && strOLName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strOLName", strOLName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_Outlet_Update_InvalidOLName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_Outlet:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_Outlet.Rows[0];
					if (bUpd_DBCode) { strFN = "DBCode"; drDB[strFN] = strDBCode; alColumnEffective.Add(strFN); }
					if (bUpd_DistrictCode) { strFN = "DistrictCode"; drDB[strFN] = strDistrictCode; alColumnEffective.Add(strFN); }
					if (bUpd_OLName) { strFN = "OLName"; drDB[strFN] = strOLName; alColumnEffective.Add(strFN); }
					if (bUpd_OLAddress) { strFN = "OLAddress"; drDB[strFN] = strOLAddress; alColumnEffective.Add(strFN); }
					if (bUpd_OLOwnerName) { strFN = "OLOwnerName"; drDB[strFN] = strOLName; alColumnEffective.Add(strFN); }
					if (bUpd_OLOwnerDateBirth) { strFN = "OLOwnerDateBirth"; drDB[strFN] = strOLOwnerDateBirth; alColumnEffective.Add(strFN); }
					if (bUpd_OLOwnerSignImagePath) { strFN = "OLOwnerSignImagePath"; drDB[strFN] = strOLOwnerSignImagePath; alColumnEffective.Add(strFN); }
					if (bUpd_OLTaxCode) { strFN = "OLTaxCode"; drDB[strFN] = strOLTaxCode; alColumnEffective.Add(strFN); }
					if (bUpd_OLOwnerIdCardNo) { strFN = "OLOwnerIdCardNo"; drDB[strFN] = strOLOwnerIdCardNo; alColumnEffective.Add(strFN); }
					if (bUpd_OLOwnerIdCardFrontImagePath) { strFN = "OLOwnerIdCardFrontImagePath"; drDB[strFN] = strOLOwnerIdCardFrontImagePath; alColumnEffective.Add(strFN); }
					if (bUpd_OLOwnerIdCardRearImagePath) { strFN = "OLOwnerIdCardRearImagePath"; drDB[strFN] = strOLOwnerIdCardRearImagePath; alColumnEffective.Add(strFN); }
					if (bUpd_OLOwnerIdAddress) { strFN = "OLOwnerIdAddress"; drDB[strFN] = strOLOwnerIdAddress; alColumnEffective.Add(strFN); }
					if (bUpd_OLPhoneNo) { strFN = "OLPhoneNo"; drDB[strFN] = strOLPhoneNo; alColumnEffective.Add(strFN); }
					if (bUpd_OLFaxNo) { strFN = "OLFaxNo"; drDB[strFN] = strOLFaxNo; alColumnEffective.Add(strFN); }
					if (bUpd_OLMobilePhoneNo) { strFN = "OLMobilePhoneNo"; drDB[strFN] = strOLMobilePhoneNo; alColumnEffective.Add(strFN); }
					if (bUpd_OLSMSPhoneNo) { strFN = "OLSMSPhoneNo"; drDB[strFN] = strOLSMSPhoneNo; alColumnEffective.Add(strFN); }
					if (bUpd_OLBankAccountNo) { strFN = "OLBankAccountNo"; drDB[strFN] = strOLBankAccountNo; alColumnEffective.Add(strFN); }
					if (bUpd_OLBankName) { strFN = "OLBankName"; drDB[strFN] = strOLBankName; alColumnEffective.Add(strFN); }
					if (bUpd_Remark) { strFN = "Remark"; drDB[strFN] = objRemark; alColumnEffective.Add(strFN); }
					if (bUpd_OLStatus) { strFN = "OLStatus"; drDB[strFN] = strOLStatus; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_Outlet"
						, dtDB_Mst_Outlet
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

		public DataSet Mst_Outlet_Delete(
			string strTid
			, DataRow drSession
			////
			, object objOLCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_Outlet_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Outlet_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
					, "objOLCode", objOLCode
					});
			#endregion

			try
			{
				#region // Convert Input:
				DateTime dtimeTDate = DateTime.Now;
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
				string strOLCode = TUtils.CUtils.StdParam(objOLCode);
				////
				DataTable dtDB_Mst_Outlet = null;
				{
					////
					Mst_Outlet_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strOLCode // objOLCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // objOutletStatusListToCheck
						, out dtDB_Mst_Outlet// dtDB_Mst_Outlet
						);
					////
				}
				#endregion

				#region // SaveDB Mst_Outlet:
				{
					// Init:
					dtDB_Mst_Outlet.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_Outlet"
						, dtDB_Mst_Outlet
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

		#region // Mst_Province:
		private void Mst_Province_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objProvinceCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_Province
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_Province t --//[mylock]
					where (1=1)
						and t.ProvinceCode = @objProvinceCode
					;
				");
			dtDB_Mst_Province = _cf.db.ExecQuery(
				strSqlExec
				, "@objProvinceCode", objProvinceCode
				).Tables[0];
			dtDB_Mst_Province.TableName = "Mst_Province";

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_Province.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.ProvinceCode", objProvinceCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_Province_CheckDB_ProvinceNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_Province.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.ProvinceCode", objProvinceCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_Province_CheckDB_ProvinceExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck:
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_Province.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.ProvinceCode", objProvinceCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_Province.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_Province_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_Province_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_Province
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_Province_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Province_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_Province", strRt_Cols_Mst_Province
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
				bool bGet_Mst_Province = (strRt_Cols_Mst_Province != null && strRt_Cols_Mst_Province.Length > 0);

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
						---- #tbl_Mst_Province_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mp.ProvinceCode
						into #tbl_Mst_Province_Filter_Draft
						from Mst_Province mp --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mp.ProvinceCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_Province_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_Province_Filter:
						select
							t.*
						into #tbl_Mst_Province_Filter
						from #tbl_Mst_Province_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_Province --------:
						zzB_Select_Mst_Province_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_Province_Filter_Draft;
						--drop table #tbl_Mst_Province_Filter;
					"
					);
				////
				string zzB_Select_Mst_Province_zzE = "-- Nothing.";
				if (bGet_Mst_Province)
				{
					#region // bGet_Mst_Province:
					zzB_Select_Mst_Province_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_Province:
							select
								t.MyIdxSeq
								, mp.*
							from #tbl_Mst_Province_Filter t --//[mylock]
								inner join Mst_Province mp --//[mylock]
									on t.ProvinceCode = mp.ProvinceCode
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
							, "Mst_Province" // strTableNameDB
							, "Mst_Province." // strPrefixStd
							, "mp." // strPrefixAlias
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
					, "zzB_Select_Mst_Province_zzE", zzB_Select_Mst_Province_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_Province)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_Province";
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
		public DataSet Mst_Province_Create(
			string strTid
			, DataRow drSession
			////
			, object objProvinceCode
			, object objProvinceName
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_Province_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Province_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
					, "objProvinceCode", objProvinceCode
					, "objProvinceName", objProvinceName					
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
				string strProvinceCode = TUtils.CUtils.StdParam(objProvinceCode);
				string strProvinceName = string.Format("{0}", objProvinceName).Trim();

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_Province = null;
				{
					////
					if (strProvinceCode == null || strProvinceCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strProvinceCode", strProvinceCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_Province_Create_InvalidProvinceCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_Province_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strProvinceCode // objProvinceCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_Province // dtDB_Mst_Province
						);
					////
					if (strProvinceName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strProvinceName", strProvinceName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_Province_Create_InvalidProvinceName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_Province:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_Province.NewRow();
					strFN = "ProvinceCode"; drDB[strFN] = strProvinceCode;
					strFN = "ProvinceName"; drDB[strFN] = strProvinceName;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_Province.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_Province"
						, dtDB_Mst_Province
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
		public DataSet Mst_Province_Update(
			string strTid
			, DataRow drSession
			////
			, object objProvinceCode
			, object objProvinceName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_Province_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_Province_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objProvinceCode", objProvinceCode
					, "objProvinceName", objProvinceName
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
				string strProvinceCode = TUtils.CUtils.StdParam(objProvinceCode);
				string strProvinceName = TUtils.CUtils.StdParam(objProvinceName);
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_ProvinceName = strFt_Cols_Upd.Contains("Mst_Province.ProvinceName".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_Province.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_Province = null;
				{
					////
					Mst_Province_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strProvinceCode // objProvinceCode 
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_Province // dtDB_Mst_Province
						);
					////
					if (bUpd_ProvinceName && string.IsNullOrEmpty(strProvinceName))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strProvinceName", strProvinceName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_Province_Update_InvalidProvinceName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // Save Mst_Province:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_Province.Rows[0];
					if (bUpd_ProvinceName) { strFN = "ProvinceName"; drDB[strFN] = strProvinceName; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_Province"
						, dtDB_Mst_Province
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
		public DataSet Mst_Province_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objProvinceCode
			)
		{
			#region // Temp:
				DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
				//int nTidSeq = 0;
				DateTime dtimeSys = DateTime.Now;
				string strFunctionName = "Mst_Province_Delete";
				string strErrorCodeDefault = TError.ErrDemoLab.Mst_Province_Delete;
				ArrayList alParamsCoupleError = new ArrayList(new object[]{
						"strFunctionName", strFunctionName
						, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
						////
						, "objProvinceCode", objProvinceCode
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
				string strProvinceCode = TUtils.CUtils.StdParam(objProvinceCode);
				////
				DataTable dtDB_Mst_Province = null;
				{
					////
					Mst_Province_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strProvinceCode // objProvinceCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_Province // dtDB_Mst_Province
						);
				}
				#endregion

				#region // SaveDB Mst_Province:
				{
					// Init:
					dtDB_Mst_Province.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_Province"
						, dtDB_Mst_Province
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

		#region // Mst_District:
		private void Mst_District_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objDistrictCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_District
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_District t --//[mylock]
					where (1=1)
						and t.DistrictCode = @objDistrictCode
					;
				");
			dtDB_Mst_District = _cf.db.ExecQuery(
				strSqlExec
				, "@objDistrictCode", objDistrictCode
				).Tables[0];
			dtDB_Mst_District.TableName = "Mst_District";

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_District.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.DistrictCode", objDistrictCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_District_CheckDB_DistrictNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_District.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.DistrictCode", objDistrictCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_District_CheckDB_DistrictExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck:
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_District.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.DistrictCode", objDistrictCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_District.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_District_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_District_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_District
			)
		{
			#region // Temdt:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_District_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_District_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_District", strRt_Cols_Mst_District
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
				bool bGet_Mst_District = (strRt_Cols_Mst_District != null && strRt_Cols_Mst_District.Length > 0);

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
						---- #tbl_Mst_District_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mdt.DistrictCode
						into #tbl_Mst_District_Filter_Draft
						from Mst_District mdt --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mdt.DistrictCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_District_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_District_Filter:
						select
							t.*
						into #tbl_Mst_District_Filter
						from #tbl_Mst_District_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_District --------:
						zzB_Select_Mst_District_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_District_Filter_Draft;
						--drop table #tbl_Mst_District_Filter;
					"
					);
				////
				string zzB_Select_Mst_District_zzE = "-- Nothing.";
				if (bGet_Mst_District)
				{
					#region // bGet_Mst_District:
					zzB_Select_Mst_District_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_District:
							select
								t.MyIdxSeq
								, mdt.*
                                , mp.ProvinceCode
							from #tbl_Mst_District_Filter t --//[mylock]
								inner join Mst_District mdt --//[mylock]
									on t.DistrictCode = mdt.DistrictCode
                                left join Mst_Province mp --//[mylock]
		                            on mdt.ProvinceCode = mp.ProvinceCode
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
							, "Mst_District" // strTableNameDB
							, "Mst_District." // strPrefixStd
							, "mdt." // strPrefixAlias
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
					, "zzB_Select_Mst_District_zzE", zzB_Select_Mst_District_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_District)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_District";
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
		public DataSet Mst_District_Create(
			string strTid
			, DataRow drSession
			////
			, object objDistrictCode
			, object objProvinceCode
			, object objDistrictName
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_District_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_District_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
					, "objDistrictCode", objDistrictCode
                    , "objProvinceCode", objProvinceCode
					, "objDistrictName", objDistrictName					
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
				string strDistrictCode = TUtils.CUtils.StdParam(objDistrictCode);
				string strProvinceCode = TUtils.CUtils.StdParam(objProvinceCode);
				string strDistrictName = string.Format("{0}", objDistrictName).Trim();

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_District = null;
				DataTable dtDB_Mst_Province = null;
				{

					////
					if (strDistrictCode == null || strDistrictCode.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strDistrictCode", strDistrictCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_District_Create_InvalidDistrictCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_District_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objDistrictCode // objDistrictCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_District // dtDB_Mst_District
						);
					////
					Mst_Province_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strProvinceCode // objProvinceCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.Flag.Active // strFlagActiveListToCheck
						, out dtDB_Mst_Province // dtDB_Mst_Province
						);

					////
					if (strDistrictName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strDistrictName", strDistrictName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_District_Create_InvalidDistrictName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_District:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_District.NewRow();
					strFN = "DistrictCode"; drDB[strFN] = strDistrictCode;
					strFN = "ProvinceCode"; drDB[strFN] = strProvinceCode;
					strFN = "DistrictName"; drDB[strFN] = strDistrictName;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_District.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_District"
						, dtDB_Mst_District
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
		public DataSet Mst_District_Update(
			string strTid
			, DataRow drSession
			////
			, object objDistrictCode
			, object objProvinceCode
			, object objDistrictName
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_District_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_District_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objDistrictCode", objDistrictCode
                    , "objProvinceCode", objProvinceCode
					, "objDistrictName", objDistrictName
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
				string strDistrictCode = TUtils.CUtils.StdParam(objDistrictCode);
				string strProvinceCode = TUtils.CUtils.StdParam(objProvinceCode);
				string strDistrictName = string.Format("{0}", objDistrictName).Trim();
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_DistrictName = strFt_Cols_Upd.Contains("Mst_District.DistrictName".ToUpper());
				bool bUpd_ProvinceCode = strFt_Cols_Upd.Contains("Mst_District.ProvinceCode".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_District.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_District = null;
				DataTable dtDB_Mst_Province = null;
				{
					////
					Mst_District_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strDistrictCode // objDistrictCode 
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_District // dtDB_Mst_District
						);
					////
					if (bUpd_DistrictName && string.IsNullOrEmpty(strDistrictName))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strDistrictName", strDistrictName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_District_Update_InvalidDistrictName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (bUpd_ProvinceCode)
					{
						Mst_Province_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, strProvinceCode // objProvinceCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_Province // dtDB_Mst_Province
							);
					}
				}
				#endregion

				#region // Save Mst_District:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_District.Rows[0];
					if (bUpd_ProvinceCode) { strFN = "ProvinceCode"; drDB[strFN] = strProvinceCode; alColumnEffective.Add(strFN); }
					if (bUpd_DistrictName) { strFN = "DistrictName"; drDB[strFN] = strDistrictName; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_District"
						, dtDB_Mst_District
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
		public DataSet Mst_District_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objDistrictCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_District_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_District_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objDistrictCode", objDistrictCode
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
				string strDistrictCode = TUtils.CUtils.StdParam(objDistrictCode);
				////
				DataTable dtDB_Mst_District = null;
				{
					////
					Mst_District_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objDistrictCode // objDistrictCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_District // dtDB_Mst_District
						);
				}
				#endregion

				#region // SaveDB Mst_District:
				{
					// Init:
					dtDB_Mst_District.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_District"
						, dtDB_Mst_District
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

		#region // Mst_ImageType:
		private void Mst_ImageType_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objImageType
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_ImageType
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_ImageType t --//[mylock]
					where (1=1)
						and t.ImageType = @objImageType
					;
				");
			dtDB_Mst_ImageType = _cf.db.ExecQuery(
				strSqlExec
				, "@objImageType", objImageType
				).Tables[0];
			dtDB_Mst_ImageType.TableName = "Mst_ImageType";

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_ImageType.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.ImageType", objImageType
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_ImageType_CheckDB_ImageTypeNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_ImageType.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.ImageType", objImageType
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_ImageType_CheckDB_ImageTypeExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck:
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_ImageType.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.ImageType", objImageType
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_ImageType.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_ImageType_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_ImageType_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_ImageType
			)
		{
			#region // Temit:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_ImageType_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_ImageType_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_ImageType", strRt_Cols_Mst_ImageType
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
				bool bGet_Mst_ImageType = (strRt_Cols_Mst_ImageType != null && strRt_Cols_Mst_ImageType.Length > 0);

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
						---- #tbl_Mst_ImageType_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mit.ImageType
						into #tbl_Mst_ImageType_Filter_Draft
						from Mst_ImageType mit --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mit.ImageType asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_ImageType_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_ImageType_Filter:
						select
							t.*
						into #tbl_Mst_ImageType_Filter
						from #tbl_Mst_ImageType_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_ImageType --------:
						zzB_Select_Mst_ImageType_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_ImageType_Filter_Draft;
						--drop table #tbl_Mst_ImageType_Filter;
					"
					);
				////
				string zzB_Select_Mst_ImageType_zzE = "-- Nothing.";
				if (bGet_Mst_ImageType)
				{
					#region // bGet_Mst_ImageType:
					zzB_Select_Mst_ImageType_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_ImageType:
							select
								t.MyIdxSeq
								, mit.*
							from #tbl_Mst_ImageType_Filter t --//[mylock]
								inner join Mst_ImageType mit --//[mylock]
									on t.ImageType = mit.ImageType
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
							, "Mst_ImageType" // strTableNameDB
							, "Mst_ImageType." // strPrefixStd
							, "mit." // strPrefixAlias
							);
						////
						#endregion
					}
					zzB_Where_strFilter_zzE = CmUtils.SqlUtils.BuildWhere(
						htSpCols // htSpCols
						, strFt_WhereClause // strClause
						, "@p_" // strParamitrefix
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
					, "zzB_Select_Mst_ImageType_zzE", zzB_Select_Mst_ImageType_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_ImageType)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_ImageType";
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
		public DataSet Mst_ImageType_Create(
			string strTid
			, DataRow drSession
			////
			, object objImageType
			, object objImageDesc
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_ImageType_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_ImageType_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
					, "objImageType", objImageType
					, "objImageDesc", objImageDesc					
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
				string strImageType = TUtils.CUtils.StdParam(objImageType);
				string strImageDesc = string.Format("{0}", objImageDesc).Trim();

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_ImageType = null;
				{

					////
					if (strImageType == null || strImageType.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strImageType", strImageType
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_ImageType_Create_InvalidImageType
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Mst_ImageType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objImageType // objImageType
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_ImageType // dtDB_Mst_ImageType
						);


					////
					if (strImageDesc.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strImageDesc", strImageDesc
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_ImageType_Create_InvalidImageDesc
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // SaveDB Mst_ImageType:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_ImageType.NewRow();
					strFN = "ImageType"; drDB[strFN] = strImageType;
					strFN = "ImageDesc"; drDB[strFN] = strImageDesc;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_ImageType.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_ImageType"
						, dtDB_Mst_ImageType
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
		public DataSet Mst_ImageType_Update(
			string strTid
			, DataRow drSession
			////
			, object objImageType
			, object objImageDesc
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_ImageType_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_ImageType_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objImageType", objImageType
					, "objImageDesc", objImageDesc
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
				string strImageType = TUtils.CUtils.StdParam(objImageType);
				string strImageDesc = TUtils.CUtils.StdParam(objImageDesc);
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_ImageDesc = strFt_Cols_Upd.Contains("Mst_ImageType.ImageDesc".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_ImageType.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_ImageType = null;
				{
					////
					Mst_ImageType_CheckDB(
						 ref alParamsCoupleError // alParamsCoupleError
						 , strImageType // objImageType 
						 , TConst.Flag.Yes // strFlagExistToCheck
						 , "" // strFlagActiveListToCheck
						 , out dtDB_Mst_ImageType // dtDB_Mst_ImageType
						);
					////
					if (bUpd_ImageDesc && string.IsNullOrEmpty(strImageDesc))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strImageDesc", strImageDesc
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_ImageType_Update_InvalidImageDesc
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // Save Mst_ImageType:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_ImageType.Rows[0];
					if (bUpd_ImageDesc) { strFN = "ImageDesc"; drDB[strFN] = strImageDesc; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_ImageType"
						, dtDB_Mst_ImageType
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
		public DataSet Mst_ImageType_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objImageType
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_ImageType_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_ImageType_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objImageType", objImageType
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
				string strImageType = TUtils.CUtils.StdParam(objImageType);
				////
				DataTable dtDB_Mst_ImageType = null;
				{
					////
					Mst_ImageType_CheckDB(
						 ref alParamsCoupleError // alParamsCoupleError
						 , objImageType // objImageType
						 , TConst.Flag.Yes // strFlagExistToCheck
						 , "" // strFlagActiveListToCheck
						 , out dtDB_Mst_ImageType // dtDB_Mst_ImageType
						);
				}
				#endregion

				#region // SaveDB Mst_ImageType:
				{
					// Init:
					dtDB_Mst_ImageType.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_ImageType"
						, dtDB_Mst_ImageType
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

		#region // Mst_ActionType:
		private void Mst_ActionType_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objActionType
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Mst_ActionType
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Mst_ActionType t --//[mylock]
					where (1=1)
						and t.ActionType = @objActionType
					;
				");
			dtDB_Mst_ActionType = _cf.db.ExecQuery(
				strSqlExec
				, "@objActionType", objActionType
				).Tables[0];
			dtDB_Mst_ActionType.TableName = "Mst_ActionType";

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Mst_ActionType.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.ActionType", objActionType
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_ActionType_CheckDB_ActionTypeNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Mst_ActionType.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.ActionType", objActionType
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Mst_ActionType_CheckDB_ActionTypeExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck:
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Mst_ActionType.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.ActionType", objActionType
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Mst_ActionType.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Mst_ActionType_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Mst_ActionType_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Mst_ActionType
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_ActionType_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_ActionType_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Mst_ActionType", strRt_Cols_Mst_ActionType
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
				bool bGet_Mst_ActionType = (strRt_Cols_Mst_ActionType != null && strRt_Cols_Mst_ActionType.Length > 0);

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
						---- #tbl_Mst_ActionType_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, mat.ActionType
						into #tbl_Mst_ActionType_Filter_Draft
						from Mst_ActionType mat --//[mylock]
						where (1=1)
							zzB_Where_strFilter_zzE
						order by mat.ActionType asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Mst_ActionType_Filter_Draft t --//[mylock]
						;

						---- #tbl_Mst_ActionType_Filter:
						select
							t.*
						into #tbl_Mst_ActionType_Filter
						from #tbl_Mst_ActionType_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Mst_ActionType --------:
						zzB_Select_Mst_ActionType_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Mst_ActionType_Filter_Draft;
						--drop table #tbl_Mst_ActionType_Filter;
					"
					);
				////
				string zzB_Select_Mst_ActionType_zzE = "-- Nothing.";
				if (bGet_Mst_ActionType)
				{
					#region // bGet_Mst_Dealer:
					zzB_Select_Mst_ActionType_zzE = CmUtils.StringUtils.Replace(@"
							---- Mst_ActionType:
							select
								t.MyIdxSeq
								, mat.ActionType ActionTypeCode
	                            , mat.ActionTypeDesc 
	                            , mat.AvgScoreValStart 
	                            , mat.AvgScoreValEnd 
	                            , mat.FlagActive 
	                            , mat.LogLUDTime 
	                            , mat.LogLUBy
							from #tbl_Mst_ActionType_Filter t --//[mylock]
								inner join Mst_ActionType mat --//[mylock]
									on t.ActionType = mat.ActionType
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
							, "Mst_ActionType" // strTableNameDB
							, "Mst_ActionType." // strPrefixStd
							, "mat." // strPrefixAlias
							);
						////
						#endregion
					}
					zzB_Where_strFilter_zzE = CmUtils.SqlUtils.BuildWhere(
						htSpCols // htSpCols
						, strFt_WhereClause // strClause
						, "@p_" // strParamatrefix
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
					, "zzB_Select_Mst_ActionType_zzE", zzB_Select_Mst_ActionType_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Mst_ActionType)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Mst_ActionType";
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
		public DataSet Mst_ActionType_Create(
			string strTid
			, DataRow drSession
			////
			, object objActionType
			, object objActionTypeDesc
			, object objAvgScoreValStart
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_ActionType_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_ActionType_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
                    , "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
                    ////
					, "objActionType", objActionType
					, "objActionTypeDesc", objActionTypeDesc					
                    , "objAvgScoreValStart", objAvgScoreValStart					                    
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
				string strActionType = TUtils.CUtils.StdParam(objActionType);
				string strActionTypeDesc = string.Format("{0}", objActionTypeDesc).Trim();
				double dblAvgScoreValStart = Convert.ToDouble(objAvgScoreValStart);
				double dblAvgScoreValEnd = 0.0;

				// drAbilityOfUser:
				//DataRow drAbilityOfUser = Sys_User_GetAbilityViewOfUser(_cf.sinf.strUserCode);
				////
				DataTable dtDB_Mst_ActionType = null;
				{

					////
					if (strActionType == null || strActionType.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strActionType", strActionType
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_ActionType_Create_InvalidActionType
							, null
							, alParamsCoupleError.ToArray()
							);
					}

					Mst_ActionType_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strActionType // objActionType
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_ActionType // dtDB_Mst_ActionType
						);

					////
					if (strActionTypeDesc.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strActionTypeDesc", strActionTypeDesc
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_ActionType_Create_InvalidActionTypeDesc
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					//// 0 <= dblAvgScoreValStart < 100 
					if (dblAvgScoreValStart < 0 || dblAvgScoreValStart >= 100)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.dblAvgScoreValStart", dblAvgScoreValStart
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_ActionType_Create_InvalidAvgScoreValStart
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					//// Check AvgScoreValStart Exists
					string strSql_Mst_ActionTypeCheck = CmUtils.StringUtils.Replace(@"
                            ---- Mst_ActionType:
                            select top 1
	                             t.*
                            from Mst_ActionType t --//[mylock]
                            where (1=1)
	                            and t.AvgScoreValStart = @objAvgScoreValStart
                            ;	
                        "
						, "@objAvgScoreValStart", dblAvgScoreValStart
						);
					DataTable dtDB_Mst_ActionTypeCheck = _cf.db.ExecQuery(strSql_Mst_ActionTypeCheck).Tables[0];
					if (dtDB_Mst_ActionTypeCheck.Rows.Count > 0)
					{
						alParamsCoupleError.AddRange(new object[]{
                            "Check.ActionType", dtDB_Mst_ActionTypeCheck.Rows[0]["ActionType"]
							, "Check.ExistAvgScoreValStart", dtDB_Mst_ActionTypeCheck.Rows[0]["AvgScoreValStart"]
                            , "Check.dblAvgScoreValStart", dblAvgScoreValStart
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_ActionType_Create_InvalidAvgScoreValStartExists
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region // Save Previous:
				{
					DataTable dtDB_Mst_ActionTypePrevious = null;
					//// Check AvgScoreValStart Exists
					string strSql_Mst_ActionTypeCheck = CmUtils.StringUtils.Replace(@"
                            ---- Mst_ActionType:
                            select top 1
	                             t.*
                            from Mst_ActionType t --//[mylock]
                            where (1=1)
	                            and t.AvgScoreValStart < @objAvgScoreValStart
                            order by 
                                t.AvgScoreValStart desc
                            ;
                        "
						, "@objAvgScoreValStart", dblAvgScoreValStart
						);
					dtDB_Mst_ActionTypePrevious = _cf.db.ExecQuery(strSql_Mst_ActionTypeCheck).Tables[0];
					if (dtDB_Mst_ActionTypePrevious.Rows.Count > 0)
					{
						ArrayList alColumnEffective = new ArrayList();
						dtDB_Mst_ActionTypePrevious.Rows[0]["AvgScoreValEnd"] = dblAvgScoreValStart - 1; alColumnEffective.Add("AvgScoreValEnd");
						// Save:
						_cf.db.SaveData(
							"Mst_ActionType"
							, dtDB_Mst_ActionTypePrevious
							, alColumnEffective.ToArray()
						);
					}
				}
				#endregion

				#region // Set AvgScoreValEnd:
				{
					DataTable dtDB_Mst_ActionTypeNext = null;
					string strSql_Mst_ActionTypeCheck = CmUtils.StringUtils.Replace(@"
                            ---- Mst_ActionType:
                            select top 1
	                             t.*
                            from Mst_ActionType t --//[mylock]
                            where (1=1)
	                            and t.AvgScoreValStart > @objAvgScoreValStart
                            order by 
                                t.AvgScoreValStart asc
                            ;
                        "
						, "@objAvgScoreValStart", dblAvgScoreValStart
						);
					dtDB_Mst_ActionTypeNext = _cf.db.ExecQuery(strSql_Mst_ActionTypeCheck).Tables[0];
					if (dtDB_Mst_ActionTypeNext.Rows.Count > 0)
					{
						double dblAvgScoreValStartExists = Convert.ToDouble(dtDB_Mst_ActionTypeNext.Rows[0]["AvgScoreValStart"]);
						dblAvgScoreValEnd = dblAvgScoreValStartExists - 1;
					}
					else
					{
						dblAvgScoreValEnd = 100.0;
					}
				}
				#endregion

				#region // SaveDB Mst_ActionType:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_ActionType.NewRow();
					strFN = "ActionType"; drDB[strFN] = strActionType;
					strFN = "ActionTypeDesc"; drDB[strFN] = strActionTypeDesc;
					strFN = "AvgScoreValStart"; drDB[strFN] = dblAvgScoreValStart;
					strFN = "AvgScoreValEnd"; drDB[strFN] = dblAvgScoreValEnd;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Active;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Mst_ActionType.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Mst_ActionType"
						, dtDB_Mst_ActionType
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
		public DataSet Mst_ActionType_Update(
			string strTid
			, DataRow drSession
			////
			, object objActionType
			, object objActionTypeDesc
			, object objAvgScoreValStart
			, object objFlagActive
			////
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_ActionType_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_ActionType_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objActionType", objActionType
					, "objActionTypeDesc", objActionTypeDesc
                    , "objAvgScoreValStart", objAvgScoreValStart
					, "objFlagActive", objFlagActive
                    ////
					, "objFt_Cols_Upd", objFt_Cols_Upd
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
				string strFt_Cols_Upd = TUtils.CUtils.StdParam(objFt_Cols_Upd);
				strFt_Cols_Upd = (strFt_Cols_Upd == null ? "" : strFt_Cols_Upd);
				////
				string strActionType = TUtils.CUtils.StdParam(objActionType);
				string strActionTypeDesc = TUtils.CUtils.StdParam(objActionTypeDesc);
				double dblAvgScoreValStart = Convert.ToInt32(objAvgScoreValStart);
				string strFlagActive = TUtils.CUtils.StdFlag(objFlagActive);
				////
				bool bUpd_ActionTypeDesc = strFt_Cols_Upd.Contains("Mst_ActionType.ActionTypeDesc".ToUpper());
				bool bUpd_AvgScoreValStart = strFt_Cols_Upd.Contains("Mst_ActionType.AvgScoreValStart".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Mst_ActionType.FlagActive".ToUpper());

				////
				DataTable dtDB_Mst_ActionType = null;
				{
					////
					Mst_ActionType_CheckDB(
						 ref alParamsCoupleError // alParamsCoupleError
						 , strActionType // objActionType 
						 , TConst.Flag.Yes // strFlagExistToCheck
						 , "" // strFlagActiveListToCheck
						 , out dtDB_Mst_ActionType // dtDB_Mst_ActionType
						);
					////
					if (bUpd_ActionTypeDesc && string.IsNullOrEmpty(strActionTypeDesc))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strActionTypeDesc", strActionTypeDesc
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Mst_ActionType_Update_InvalidActionTypeDesc
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (bUpd_AvgScoreValStart)
					{
						//// 0 <= douAvgScoreValStart < 100 
						if (dblAvgScoreValStart < 0 || dblAvgScoreValStart >= 100)
						{
							alParamsCoupleError.AddRange(new object[]{
							    "Check.dblAvgScoreValStart", dblAvgScoreValStart
							    });
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Mst_ActionType_Update_InvalidAvgScoreValStart
								, null
								, alParamsCoupleError.ToArray()
								);
						}

						//// Check AvgScoreValStart Exists
						string strSql_Mst_ActionTypeCheck = CmUtils.StringUtils.Replace(@"
                            ---- Mst_ActionType:
                            select 
	                            top 1 t.*
                            from Mst_ActionType t --//[mylock]
                            where (1=1)
	                            and t.AvgScoreValStart = @objAvgScoreValStart
								 and t.ActionType <> '@objActionType'
                            ;	
                        "
							, "@objAvgScoreValStart", dblAvgScoreValStart
							, "@objActionType", strActionType
							);
						DataTable dtDB_Mst_ActionTypeCheck = _cf.db.ExecQuery(strSql_Mst_ActionTypeCheck).Tables[0];
						if (dtDB_Mst_ActionTypeCheck.Rows.Count > 0)
						{
							alParamsCoupleError.AddRange(new object[]{
                               "Check.ActionType", dtDB_Mst_ActionTypeCheck.Rows[0]["ActionType"]
							    , "Check.ExistAvgScoreValStart", dtDB_Mst_ActionTypeCheck.Rows[0]["AvgScoreValStart"]
                                , "Check.dblAvgScoreValStart", dblAvgScoreValStart
							    });
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Mst_ActionType_Create_InvalidAvgScoreValStartExists
								, null
								, alParamsCoupleError.ToArray()
								);
						}

						#region // Re Update Mst_ActionType:
						{
							string strSqlSelect = CmUtils.StringUtils.Replace(@"
                                ---- Mst_ActionType:
                                select 
	                                 t.*
                                from Mst_ActionType t --//[mylock]
                                where (1=1)	   
                                    and t.ActionType <> '@objActionType'
                                order by 
                                    t.AvgScoreValStart asc                             
                                ;	
                                ", "@objActionType", strActionType
								);
							DataTable dtDB_Mst_ActionType_Update = _cf.db.ExecQuery(strSqlSelect).Tables[0];
							if (dtDB_Mst_ActionType_Update.Rows.Count > 1)
							{
								////
								int nRowCount = dtDB_Mst_ActionType_Update.Rows.Count;
								////
								for (int nScan = 0; nScan < dtDB_Mst_ActionType_Update.Rows.Count - 1; nScan++)
								{
									////
									DataRow drScanFirst = dtDB_Mst_ActionType_Update.Rows[nScan];
									DataRow drScanSecond = dtDB_Mst_ActionType_Update.Rows[nScan + 1];


									////
									double dblAvgScoreValEnd = Convert.ToInt32(drScanSecond["AvgScoreValStart"]) - 1;

									//// SaveDB Mst_ActionType:
									string zzzzClauseUpdate_Mst_ActionType = CmUtils.StringUtils.Replace(@"
                                        update t
                                        set 
	                                        t.AvgScoreValEnd = @objAvgScoreValEnd
                                        from Mst_ActionType t
                                        where (1=1)
	                                        and t.ActionType = '@objActionType'
                                    "
										, "@objActionType", drScanFirst["ActionType"]
										, "@objAvgScoreValEnd", dblAvgScoreValEnd
										);
									_cf.db.ExecQuery(zzzzClauseUpdate_Mst_ActionType);
								}

								//// Update Latest:
								{
									string zzzzClauseUpdate_Mst_ActionType = CmUtils.StringUtils.Replace(@"
                                        update t
                                        set 
	                                        t.AvgScoreValEnd = @objAvgScoreValEnd
                                        from Mst_ActionType t
                                        where (1=1)
	                                        and t.ActionType = '@objActionType'
                                    "
										, "@objActionType", dtDB_Mst_ActionType_Update.Rows[nRowCount - 1]["ActionType"]
										, "@objAvgScoreValEnd", 100.0
										);
									_cf.db.ExecQuery(zzzzClauseUpdate_Mst_ActionType);
								}
							}
							else if (dtDB_Mst_ActionType_Update.Rows.Count == 1)
							{
								string zzzzClauseUpdate_Mst_ActionType = CmUtils.StringUtils.Replace(@"
                                        update t
                                        set 
	                                        t.AvgScoreValEnd = @objAvgScoreValEnd
                                        from Mst_ActionType t
                                        where (1=1)
	                                        and t.ActionType = '@objActionType'
                                    "
										, "@objActionType", dtDB_Mst_ActionType_Update.Rows[0]["ActionType"]
										, "@objAvgScoreValEnd", 100.0
										);
								_cf.db.ExecQuery(zzzzClauseUpdate_Mst_ActionType);
							}
						}
						#endregion
					}
				}
				#endregion

				#region // Save Mst_ActionType:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Mst_ActionType.Rows[0];
					if (bUpd_ActionTypeDesc) { strFN = "ActionTypeDesc"; drDB[strFN] = strActionTypeDesc; alColumnEffective.Add(strFN); }
					if (bUpd_AvgScoreValStart)
					{
						strFN = "AvgScoreValStart"; drDB[strFN] = dblAvgScoreValStart; alColumnEffective.Add(strFN);

						#region // Save Previous:
						{
							DataTable dtDB_Mst_ActionTypePrevious = null;
							//// Check AvgScoreValStart Exists
							string strSql_Mst_ActionTypeCheck = CmUtils.StringUtils.Replace(@"
                                ---- Mst_ActionType:
                                select top 1
	                                 t.*
                                from Mst_ActionType t --//[mylock]
                                where (1=1)
	                                and t.AvgScoreValStart < @objAvgScoreValStart
                                    and t.ActionType <> '@objActionType'
                                order by 
                                    t.AvgScoreValStart desc
                                ;
                                "
								, "@objAvgScoreValStart", dblAvgScoreValStart
								, "@objActionType", strActionType
								);
							dtDB_Mst_ActionTypePrevious = _cf.db.ExecQuery(strSql_Mst_ActionTypeCheck).Tables[0];
							if (dtDB_Mst_ActionTypePrevious.Rows.Count > 0)
							{
								ArrayList alColumnEffectivePre = new ArrayList();
								dtDB_Mst_ActionTypePrevious.Rows[0]["AvgScoreValEnd"] = dblAvgScoreValStart - 1; alColumnEffectivePre.Add("AvgScoreValEnd");
								// Save:
								_cf.db.SaveData(
									"Mst_ActionType"
									, dtDB_Mst_ActionTypePrevious
									, alColumnEffectivePre.ToArray()
								);
							}
						}
						#endregion

						#region // Set AvgScoreValEnd:
						{
							DataTable dtDB_Mst_ActionTypeNext = null;
							string strSql_Mst_ActionTypeCheck = CmUtils.StringUtils.Replace(@"
                                ---- Mst_ActionType:
                                select top 1
	                                 t.*
                                from Mst_ActionType t --//[mylock]
                                where (1=1)
	                                and t.AvgScoreValStart > @objAvgScoreValStart
                                    and t.ActionType <> '@objActionType'
                                order by 
                                    t.AvgScoreValStart asc
                                ;
                            "
									, "@objAvgScoreValStart", dblAvgScoreValStart
									, "@objActionType", strActionType
								);
							dtDB_Mst_ActionTypeNext = _cf.db.ExecQuery(strSql_Mst_ActionTypeCheck).Tables[0];
							if (dtDB_Mst_ActionTypeNext.Rows.Count > 0)
							{
								double dblAvgScoreValStartExists = Convert.ToDouble(dtDB_Mst_ActionTypeNext.Rows[0]["AvgScoreValStart"]);
								strFN = "AvgScoreValEnd"; drDB[strFN] = dblAvgScoreValStartExists - 1; alColumnEffective.Add(strFN);
							}
							else
							{
								strFN = "AvgScoreValEnd"; drDB[strFN] = 100; alColumnEffective.Add(strFN);
							}
						}
						#endregion
					}
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Mst_ActionType"
						, dtDB_Mst_ActionType
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
		public DataSet Mst_ActionType_Delete(
			string strTid
			, DataRow drSession
			/////
			, object objActionType
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Mst_ActionType_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Mst_ActionType_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objActionType", objActionType
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
				string strActionType = TUtils.CUtils.StdParam(objActionType);
				////
				DataTable dtDB_Mst_ActionType = null;
				{
					////
					Mst_ActionType_CheckDB(
						 ref alParamsCoupleError // alParamsCoupleError
						 , objActionType // objActionType
						 , TConst.Flag.Yes // strFlagExistToCheck
						 , "" // strFlagActiveListToCheck
						 , out dtDB_Mst_ActionType // dtDB_Mst_ActionType
						);

					#region // Re Update AvgScoreValStart:
					{
						string strSqlSelect = CmUtils.StringUtils.Replace(@"
                                ---- Mst_ActionType:
                                select 
	                                 t.*
                                from Mst_ActionType t --//[mylock]
                                where (1=1)	   
                                    and t.ActionType <> '@objActionType'
                                order by 
                                    t.AvgScoreValStart asc                             
                                ;	
                                ", "@objActionType", strActionType
							);
						DataTable dtDB_Mst_ActionType_Update = _cf.db.ExecQuery(strSqlSelect).Tables[0];
						if (dtDB_Mst_ActionType_Update.Rows.Count > 1)
						{
							for (int nScan = 0; nScan < dtDB_Mst_ActionType_Update.Rows.Count - 1; nScan++)
							{
								////
								DataRow drScanFirst = dtDB_Mst_ActionType_Update.Rows[nScan];
								DataRow drScanSecond = dtDB_Mst_ActionType_Update.Rows[nScan + 1];


								////
								double dblAvgScoreValEnd = Convert.ToInt32(drScanSecond["AvgScoreValStart"]) - 1;

								//// SaveDB Mst_ActionType:
								string zzzzClauseUpdate_Mst_ActionType = CmUtils.StringUtils.Replace(@"
                                        update t
                                        set 
	                                        t.AvgScoreValEnd = @objAvgScoreValEnd
                                        from Mst_ActionType t
                                        where (1=1)
	                                        and t.ActionType = '@objActionType'
                                    "
									, "@objActionType", drScanFirst["ActionType"]
									, "@objAvgScoreValEnd", dblAvgScoreValEnd
									);
								_cf.db.ExecQuery(zzzzClauseUpdate_Mst_ActionType);
							}
						}
						else if (dtDB_Mst_ActionType_Update.Rows.Count == 1)
						{
							string zzzzClauseUpdate_Mst_ActionType = CmUtils.StringUtils.Replace(@"
                                        update t
                                        set 
	                                        t.AvgScoreValEnd = @objAvgScoreValEnd
                                        from Mst_ActionType t
                                        where (1=1)
	                                        and t.ActionType = '@objActionType'
                                    "
									, "@objActionType", dtDB_Mst_ActionType_Update.Rows[0]["ActionType"]
									, "@objAvgScoreValEnd", 100.0
									);
							_cf.db.ExecQuery(zzzzClauseUpdate_Mst_ActionType);
						}
					}
					#endregion
				}
				#endregion

				#region // SaveDB Mst_ActionType:
				{
					// Init:
					dtDB_Mst_ActionType.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Mst_ActionType"
						, dtDB_Mst_ActionType
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

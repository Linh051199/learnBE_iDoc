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
		#region // Aud_Campaign:
		private void Aud_Campaign_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objCampaignCode
			, string strFlagExistToCheck
			, string strStatusListToCheck
			, out DataTable dtDB_Aud_Campaign
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Aud_Campaign t --//[mylock]
					where (1=1)
						and t.CampaignCode = @objCampaignCode
					;
				");
			dtDB_Aud_Campaign = _cf.db.ExecQuery(
				strSqlExec
				, "@objCampaignCode", objCampaignCode
				).Tables[0];
			dtDB_Aud_Campaign.TableName = "Aud_Campaign";

			// strFlagExistToCheck
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Aud_Campaign.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCode", objCampaignCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Aud_Campaign_CheckDB_CampaignNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Aud_Campaign.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCode", objCampaignCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Aud_Campaign_CheckDB_CampaignExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strStatusListToCheck
			if (strStatusListToCheck.Length > 0 && !strStatusListToCheck.Contains(Convert.ToString(dtDB_Aud_Campaign.Rows[0]["CampaignStatus"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CampaignCode", objCampaignCode
					, "Check.strStatusListToCheck", strStatusListToCheck
					, "DB.CampaignStatus", dtDB_Aud_Campaign.Rows[0]["CampaignStatus"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Aud_Campaign_CheckDB_CampaignStatusNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public DataSet Aud_Campaign_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return: 
			, string strRt_Cols_Aud_Campaign
			, string strRt_Cols_Aud_CampaignDBPOSMDtl
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
					, "strRt_Cols_Aud_Campaign", strRt_Cols_Aud_Campaign
					, "strRt_Cols_Aud_CampaignDBPOSMDtl", strRt_Cols_Aud_CampaignDBPOSMDtl
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
				bool bGet_Aud_Campaign = (strRt_Cols_Aud_Campaign != null && strRt_Cols_Aud_Campaign.Length > 0);
				bool bGet_Aud_CampaignDBPOSMDtl = (strRt_Cols_Aud_CampaignDBPOSMDtl != null && strRt_Cols_Aud_CampaignDBPOSMDtl.Length > 0);

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
						---- #tbl_Aud_Campaign_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, ac.CampaignCode
							, acdbd.DBCode 
							, md.AreaCode
						into #tbl_Aud_Campaign_Filter_Draft
						from Aud_Campaign ac --//[mylock]
							left join Aud_CampaignDBDtl acdbd --//[mylock]
								on ac.CampaignCode = acdbd.CampaignCode
							left join Mst_Distributor md --//[mylock]
								on acdbd.DBCode = md.DBCode
							left join Aud_CampaignDBPOSMDtl acdbpd --//[mylock]
								on ac.CampaignCode = acdbpd.CampaignCode
						where (1=1)
							zzB_Where_strFilter_zzE
						order by ac.CampaignCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Aud_Campaign_Filter_Draft t --//[mylock]
						;

						---- #tbl_Aud_Campaign_Filter:
						select
							t.*
						into #tbl_Aud_Campaign_Filter
						from #tbl_Aud_Campaign_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Aud_Campaign --------:
						zzB_Select_Aud_Campaign_zzE
						----------------------------------------

						-------- Aud_CampaignDBPOSMDtl --------:
						zzB_Select_Aud_CampaignDBPOSMDtl_zzE
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Aud_Campaign_Filter_Draft;
						--drop table #tbl_Aud_Campaign_Filter;
					"
					);
				////
				string zzB_Select_Aud_Campaign_zzE = "-- Nothing.";
				if (bGet_Aud_Campaign)
				{
					#region // bGet_Aud_Campaign:
					zzB_Select_Aud_Campaign_zzE = CmUtils.StringUtils.Replace(@"
							---- Aud_Campaign:
							select
								t.MyIdxSeq
								, ac.*
							from #tbl_Aud_Campaign_Filter t --//[mylock]
								inner join Aud_Campaign ac --//[mylock]
									on t.CampaignCode = ac.CampaignCode
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzB_Select_Aud_CampaignDBPOSMDtl_zzE = "-- Nothing.";
				if (bGet_Aud_CampaignDBPOSMDtl)
				{
					#region // bGet_Aud_Campaign:
					zzB_Select_Aud_CampaignDBPOSMDtl_zzE = CmUtils.StringUtils.Replace(@"
							---- Aud_Campaign:
							select distinct
								t.MyIdxSeq
								, md.AreaCode md_AreaCode  
								, acdbd.DBCode acdbd_DBCode
								, acdbpd.POSMCode acdbpd_POSMCode
								, mp.POSMName mp_POSMName
								, acdbpd.QtyDeliver acdbpd_QtyDeliver
							from #tbl_Aud_Campaign_Filter t --//[mylock]
								inner join Aud_Campaign ac --//[mylock]
									on t.CampaignCode = ac.CampaignCode
								left join Aud_CampaignDBDtl acdbd --//[mylock]
									on ac.CampaignCode = acdbd.CampaignCode
										and t.DBCode = acdbd.DBCode
								inner join Mst_Distributor md --//[mylock]
									on acdbd.DBCode = md.DBCode
								left join Aud_CampaignDBPOSMDtl acdbpd --//[mylock]
									on acdbd.CampaignCode = acdbpd.CampaignCode
								left join Mst_POSM mp --//[mylock]
									on acdbpd.POSMCode = mp.POSMCode
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
							, "Aud_Campaign" // strTableNameDB
							, "Aud_Campaign." // strPrefixStd
							, "ac." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Aud_CampaignDBPOSMDtl" // strTableNameDB
							, "Aud_CampaignDBPOSMDtl." // strPrefixStd
							, "acdbpd." // strPrefixAlias
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
					, "zzB_Select_Aud_Campaign_zzE", zzB_Select_Aud_Campaign_zzE
					, "zzB_Select_Aud_CampaignDBPOSMDtl_zzE", zzB_Select_Aud_CampaignDBPOSMDtl_zzE
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Aud_Campaign)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Aud_Campaign";
				}
				if (bGet_Aud_CampaignDBPOSMDtl)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Aud_CampaignDBPOSMDtl";
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
		public DataSet Aud_Campaign_Save(
			string strTid
			, DataRow drSession
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
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bMyDebugSql = false;
			DateTime dtimeSys = DateTime.Now;
			bool bNeedTransaction = true;
			string strFunctionName = "Aud_Campaign_Save";
			string strErrorCodeDefault = TError.ErrDemoLab.Aud_Campaign_Save;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objFlagIsDelete", objFlagIsDelete
					////
					, "objCampaignCode", objCampaignCode
					, "objCampaignCrCode", objCampaignCrCode
					, "objCrtrScoreVerCode", objCrtrScoreVerCode
					, "objCrtrScoreVerAUCode", objCrtrScoreVerAUCode
					, "objCampaignName", objCampaignName
					, "objEffDTimeStart", objEffDTimeStart
					, "objEffDTimeEnd", objEffDTimeEnd
					, "objQtyCheck", objQtyCheck
					, "objQtySuccess", objQtySuccess
					, "objMinIntervalDays", objMinIntervalDays
					, "objReportEndDate", objReportEndDate
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

				#region //// Refine and Check Aud_Campaign:
				////
				bool bIsDelete = CmUtils.StringUtils.StringEqual(objFlagIsDelete, TConst.Flag.Yes);
				////
				string strCampaignCode = TUtils.CUtils.StdParam(objCampaignCode);
				string strCampaignCrCode = TUtils.CUtils.StdParam(objCampaignCrCode);
				string strCrtrScoreVerCode = TUtils.CUtils.StdParam(objCrtrScoreVerCode);
				string strCrtrScoreVerAUCode = TUtils.CUtils.StdParam(objCrtrScoreVerAUCode);
				string strCampaignName = string.Format("{0}", objCampaignName).Trim();
				string strEffDTimeStart = TUtils.CUtils.StdDTimeBeginDay(objEffDTimeStart);
				string strEffDTimeEnd = TUtils.CUtils.StdDTimeEndDay(objEffDTimeEnd);
				Int16 intQtyCheck = Convert.ToInt16(objQtyCheck);
				Int16 intQtySuccess = Convert.ToInt16(objQtySuccess);
				Int16 intMinIntervalDays = Convert.ToInt16(objMinIntervalDays);
				Int16 intReportEndDate = Convert.ToInt16(objReportEndDate);
				string strRemark = null;
				int intMinImagesPerCheck = 1;
				int intMaxImagesPerCheck = 1000;
				string strCreateDTime = null;
				string strCreateBy = null;

				////
				DataTable dtDB_Aud_Campaign = null;
				{

					Aud_Campaign_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCampaignCode // objCampaignCode
						, "" // strFlagExistToCheck
						, "" // strStatusListToCheck
						, out dtDB_Aud_Campaign // dtDB_Aud_Campaign
						);
					if (dtDB_Aud_Campaign.Rows.Count < 1) // Chưa Tồn tại Order.
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
						if (!CmUtils.StringUtils.StringEqualIgnoreCase(dtDB_Aud_Campaign.Rows[0]["CampaignStatus"], TConst.CampaignStatus.Pending))
						{
							alParamsCoupleError.AddRange(new object[]{
								"Check.DB.CampaignStatus", dtDB_Aud_Campaign.Rows[0]["CampaignStatus"]
								, "Check.CampaignStatus.Expected", TConst.CampaignStatus.Pending
							});
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Aud_Campaign_Save_InvalidCampaignStatus
								, null
								, alParamsCoupleError.ToArray()
								);
						}
						else
						{
							strCreateDTime = TUtils.CUtils.StdDTime(dtDB_Aud_Campaign.Rows[0]["CreateDTime"]);
							strCreateBy = TUtils.CUtils.StdParam(dtDB_Aud_Campaign.Rows[0]["CreateBy"]);
						}
					}
					////
					strCreateDTime = string.IsNullOrEmpty(strCreateDTime) ? dtimeSys.ToString("yyyy-MM-dd HH:mm:ss") : strCreateDTime;
					strCreateBy = string.IsNullOrEmpty(strCreateBy) ? _cf.sinf.strUserCode : strCreateBy;
					////
					DataTable dtDB_Mst_CampainCriteria = null;

					Mst_CampainCriteria_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCampaignCrCode // objCampaignCrCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.Flag.Active // strFlagActiveListToCheck
						, out dtDB_Mst_CampainCriteria // dtDB_Mst_CampainCriteria
						);
					////
					DataTable dtDB_Mst_CriteriaScoreVersion = null;

					Mst_CriteriaScoreVersion_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCrtrScoreVerCode // objCrtrScoreVerCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_CriteriaScoreVersion // dtDB_Mst_CriteriaScoreVersion
						);
					////
					DataTable dtDB_Mst_CriteriaScoreVersionAuditUser = null;

					Mst_CriteriaScoreVersionAuditUser_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCrtrScoreVerAUCode // objCrtrScoreVerAUCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Mst_CriteriaScoreVersionAuditUser // dtDB_Mst_CriteriaScoreVersionAuditUser
						);

					//// 
					if (strCampaignName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strCampaignName", strCampaignName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Save_InvalidCampaignName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (strEffDTimeStart.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strEffDTimeStart", strEffDTimeStart
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Save_InvalidEffDTimeStart
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (strEffDTimeEnd.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strEffDTimeEnd", strEffDTimeEnd
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Save_InvalidEffDTimeEnd
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (intQtyCheck <= 0)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.intQtyCheck", intQtyCheck
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Save_InvalidQtyCheck
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (intQtySuccess <= 0)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.intQtySuccess", intQtySuccess
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Save_InvalidQtySuccess
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (intMinIntervalDays <= 0)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.intMinIntervalDays", intMinIntervalDays
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Save_InvalidMinIntervalDays
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (intReportEndDate <= 0)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.intReportEndDate", intReportEndDate
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Save_InvalidReportEndDate
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region //// SaveTemp Aud_Campaign:
				if (!bIsDelete)
				{
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db
						, "#input_Aud_Campaign"
						, new object[]{
							"CampaignCode", TConst.BizMix.Default_DBColType,
							"CampaignCrCode", TConst.BizMix.Default_DBColType,
							"CrtrScoreVerCode", TConst.BizMix.Default_DBColType,
							"CrtrScoreVerAUCode", TConst.BizMix.Default_DBColType,
							"CampaignName", TConst.BizMix.Default_DBColType,
							"EffDTimeStart", TConst.BizMix.Default_DBColType,
							"EffDTimeEnd", TConst.BizMix.Default_DBColType,
							"CreateDTime", TConst.BizMix.Default_DBColType,
							"CreateBy", TConst.BizMix.Default_DBColType,
							"QtyCheck", "int",
							"QtySuccess", "int",
							"MinIntervalDays", "int",
							"MinImagesPerCheck", "int",
							"MaxImagesPerCheck", "int",
							"ReportEndDate", "float",
							"Appr1DTime", TConst.BizMix.Default_DBColType,
							"Appr1By", TConst.BizMix.Default_DBColType,
							"Appr2DTime", TConst.BizMix.Default_DBColType,
							"Appr2By", TConst.BizMix.Default_DBColType,
							"FinishDTime", TConst.BizMix.Default_DBColType,
							"FinishBy", TConst.BizMix.Default_DBColType,
							"CancelDTime", TConst.BizMix.Default_DBColType,
							"CancelBy", TConst.BizMix.Default_DBColType,
							"CampaignStatus", TConst.BizMix.Default_DBColType,
							"Remark", TConst.BizMix.Default_DBColType,
							"LogLUDTime", TConst.BizMix.Default_DBColType,
							"LogLUBy", TConst.BizMix.Default_DBColType,
							}
						, new object[]{
							new object[]{
								strCampaignCode, // CampaignCode
								strCampaignCrCode, // CampaignCrCode
								strCrtrScoreVerCode, // CrtrScoreVerCode 
								strCrtrScoreVerAUCode, // CrtrScoreVerAUCode
								strCampaignName, // CampaignName
								strEffDTimeStart, // EffDTimeStart
								strEffDTimeEnd, // EffDTimeEnd
								strCreateDTime, // CreateDTime
								strCreateBy, // CreateBy
								intQtyCheck, // QtyCheck
								intQtySuccess, // QtySuccess
								intMinIntervalDays, // MinIntervalDays
								intMinImagesPerCheck, // MinImagesPerCheck
								intMaxImagesPerCheck, // MaxImagesPerCheck
								intReportEndDate, // ReportEndDate
								null, // Appr1DTime
								null, // Appr1By
								null, // Appr2DTime
								null, // Appr2By
								null, // FinishDTime
								null, // FinishBy
								null, // CancelDTime
								null, // CancelBy
								TConst.CampaignStatus.Pending, // CampaignStatus
								strRemark, // Remark
								dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"), // LogLUDTime
								_cf.sinf.strUserCode, // LogLUBy 
								}
							}
						);
				}
				#endregion

				#region //// Refine and Aud_CampaignDoc:
				////
				DataTable dtInput_Aud_CampaignDoc = null;
				if (!bIsDelete)
				{
					////
					string strTableCheck = "Aud_CampaignDoc";
					////
					if (!dsData.Tables.Contains(strTableCheck))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.TableName", strTableCheck
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Save_Input_Aud_CampaignDocTblNotFound
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					dtInput_Aud_CampaignDoc = dsData.Tables[strTableCheck];
					////
					TUtils.CUtils.StdDataInTable(
						dtInput_Aud_CampaignDoc // dtData
						, "", "FilePath" // arrstrCouple 
						);
					//// 
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDoc, "CampaignCode", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDoc, "CreateDTime", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDoc, "CreateBy", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDoc, "LogLUDTime", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDoc, "LogLUBy", typeof(object));
					////
					for (int nScan = 0; nScan < dtInput_Aud_CampaignDoc.Rows.Count; nScan++)
					{
						////
						DataRow drScan = dtInput_Aud_CampaignDoc.Rows[nScan];

						////
						drScan["CampaignCode"] = strCampaignCode;
						drScan["CreateDTime"] = strCreateDTime;
						drScan["CreateBy"] = strCreateBy;
						drScan["LogLUDTime"] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
						drScan["LogLUBy"] = _cf.sinf.strUserCode;
						////
					}
				}
				#endregion

				#region //// SaveTemp Aud_CampaignDoc:
				if (!bIsDelete)
				{
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db
						, "#input_Aud_CampaignDoc"
						, new object[]{
							"CampaignCode", TConst.BizMix.Default_DBColType,
							"FilePath", TConst.BizMix.Default_DBColType,
							"CreateDTime", TConst.BizMix.Default_DBColType,
							"CreateBy", TConst.BizMix.Default_DBColType,
							"LogLUDTime", TConst.BizMix.Default_DBColType,
							"LogLUBy", TConst.BizMix.Default_DBColType,
							}
						, dtInput_Aud_CampaignDoc
						);
				}
				#endregion

				#region //// Refine and Aud_CampaignDBPOSMDtl:
				////
				DataTable dtInput_Aud_CampaignDBPOSMDtl = null;
				if (!bIsDelete)
				{
					////
					string strTableCheck = "Aud_CampaignDBPOSMDtl";
					////
					if (!dsData.Tables.Contains(strTableCheck))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.TableName", strTableCheck
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Save_Input_Aud_CampaignDBPOSMDtlTblNotFound
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					dtInput_Aud_CampaignDBPOSMDtl = dsData.Tables[strTableCheck];
					////
					TUtils.CUtils.StdDataInTable(
						dtInput_Aud_CampaignDBPOSMDtl // dtData
						, "StdParam", "DBCode" // arrstrCouple 
						, "StdParam", "POSMCode" // arrstrCouple 
						, "", "QtyDeliver" // arrstrCouple 
						);
					//// 
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBPOSMDtl, "CampaignCode", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBPOSMDtl, "QtyRetrieve", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBPOSMDtl, "DateDBRetrieve", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBPOSMDtl, "FlagActive", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBPOSMDtl, "LogLUDTime", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBPOSMDtl, "LogLUBy", typeof(object));
					////
					for (int nScan = 0; nScan < dtInput_Aud_CampaignDBPOSMDtl.Rows.Count; nScan++)
					{
						////
						DataRow drScan = dtInput_Aud_CampaignDBPOSMDtl.Rows[nScan];

						////
						DataTable dtDB_Mst_Distributor = null;

						Mst_Distributor_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, drScan["DBCode"] // objDBCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_Distributor // dtDB_Mst_Distributor
							);
						////
						DataTable dtDB_Mst_POSM = null;

						Mst_POSM_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, drScan["POSMCode"] // objPOSMCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_POSM // dtDB_Mst_POSM
							);

						////
						drScan["CampaignCode"] = strCampaignCode;
						drScan["QtyRetrieve"] = 0;
						drScan["DateDBRetrieve"] = null;
						drScan["FlagActive"] = TConst.Flag.Active;
						drScan["LogLUDTime"] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
						drScan["LogLUBy"] = _cf.sinf.strUserCode;
						////
					}
				}
				#endregion

				#region //// SaveTemp Aud_CampaignDBPOSMDtl:
				if (!bIsDelete)
				{
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db
						, "#input_Aud_CampaignDBPOSMDtl"
						, new object[]{
							"CampaignCode", TConst.BizMix.Default_DBColType,
							"DBCode", TConst.BizMix.Default_DBColType,
							"POSMCode", TConst.BizMix.Default_DBColType,
							"QtyDeliver", "float",
							"QtyRetrieve", "float",
							"DateDBRetrieve", TConst.BizMix.Default_DBColType,
							"FlagActive", TConst.BizMix.Default_DBColType,
							"LogLUDTime", TConst.BizMix.Default_DBColType,
							"LogLUBy", TConst.BizMix.Default_DBColType,
							}
						, dtInput_Aud_CampaignDBPOSMDtl
						);
				}
				#endregion

				#region //// SaveTemp Aud_CampaignDBDtl:
				if (!bIsDelete)
				{
					string strSqlSave = CmUtils.StringUtils.Replace(@"
							select distinct
								t.CampaignCode
								, t.DBCode
								, '@objCampaignDBStatusDtl' CampaignDBStatusDtl 
								, t.LogLUDTime
								, t.LogLUBy
							into #input_Aud_CampaignDBDtl
							from #input_Aud_CampaignDBPOSMDtl t with(nolock)
							where (1=1)
								and t.CampaignCode = '@objCampaignCode'
							;

						"
						, "@objCampaignCode", strCampaignCode
						, "@objCampaignDBStatusDtl", TConst.Flag.Active
						);

					_cf.db.ExecQuery(
						strSqlSave
						);
				}
				#endregion

				#region //// Save:
				//// Clear All:
				{
					string strSqlDelete = CmUtils.StringUtils.Replace(@"
								---- Aud_CampaignDoc:
								delete t
								from Aud_CampaignDoc t 
								where (1=1)
									and t.CampaignCode = @strCampaignCode
								;

								---- Aud_CampaignDBPOSMDtl:
								delete t
								from Aud_CampaignDBPOSMDtl t 
								where (1=1)
									and t.CampaignCode = @strCampaignCode
								;

								---- Aud_CampaignDBDtl:
								delete t
								from Aud_CampaignDBDtl t 
								where (1=1)
									and t.CampaignCode = @strCampaignCode
								;

								---- Aud_Campaign:
								delete t
								from Aud_Campaign t 
								where (1=1)
									and t.CampaignCode = @strCampaignCode
								;

							");
					_cf.db.ExecQuery(
						strSqlDelete
						, "@strCampaignCode", strCampaignCode
						);
				}

				//// Insert All:
				if (!bIsDelete)
				{
					////
					string zzzzClauseInsert_Aud_Campaign_zSave = CmUtils.StringUtils.Replace(@"
							---- Aud_Campaign:
							insert into Aud_Campaign
							(
								CampaignCode 
								, CampaignCrCode
								, CrtrScoreVerCode
								, CrtrScoreVerAUCode
								, CampaignName
								, EffDTimeStart
								, EffDTimeEnd
								, CreateDTime
								, CreateBy
								, QtyCheck
								, QtySuccess
								, MinIntervalDays
								, MinImagesPerCheck
								, MaxImagesPerCheck
								, ReportEndDate
								, Appr1DTime
								, Appr1By
								, Appr2DTime
								, Appr2By
								, FinishDTime
								, FinishBy
								, CancelDTime
								, CancelBy
								, CampaignStatus
								, Remark
								, LogLUDTime
								, LogLUBy
							)
							select 
								t.CampaignCode 
								, t.CampaignCrCode
								, t.CrtrScoreVerCode
								, t.CrtrScoreVerAUCode
								, t.CampaignName
								, t.EffDTimeStart
								, t.EffDTimeEnd
								, t.CreateDTime
								, t.CreateBy
								, t.QtyCheck
								, t.QtySuccess
								, t.MinIntervalDays
								, t.MinImagesPerCheck
								, t.MaxImagesPerCheck
								, t.ReportEndDate
								, t.Appr1DTime
								, t.Appr1By
								, t.Appr2DTime
								, t.Appr2By
								, t.FinishDTime
								, t.FinishBy
								, t.CancelDTime
								, t.CancelBy
								, t.CampaignStatus
								, t.Remark
								, t.LogLUDTime
								, t.LogLUBy
							from #input_Aud_Campaign t --//[mylock]
							;
						");
					////
					string zzzzClauseInsert_Aud_CampaignDoc_zSave = CmUtils.StringUtils.Replace(@"
							---- Aud_CampaignDoc:
							insert into Aud_CampaignDoc
							(
								CampaignCode
								, FilePath
								, CreateDTime
								, CreateBy
								, LogLUDTime
								, LogLUBy
							)
							select 
								t.CampaignCode
								, t.FilePath
								, t.CreateDTime
								, t.CreateBy
								, t.LogLUDTime
								, t.LogLUBy
							from #input_Aud_CampaignDoc t --//[mylock]
							;
						");
					////
					string zzzzClauseInsert_Aud_CampaignDBDtl_zSave = CmUtils.StringUtils.Replace(@"
							---- Aud_CampaignDBDtl:
							insert into Aud_CampaignDBDtl
							(
								CampaignCode
								, DBCode
								, CampaignDBStatusDtl
								, LogLUDTime
								, LogLUBy
							)
							select
								t.CampaignCode
								, t.DBCode
								, t.CampaignDBStatusDtl
								, t.LogLUDTime
								, t.LogLUBy
							from #input_Aud_CampaignDBDtl t --//[mylock]
							;
						");
					////
					string zzzzClauseInsert_Aud_CampaignDBPOSMDtl_zSave = CmUtils.StringUtils.Replace(@"
							---- Aud_CampaignDBPOSMDtl:
							insert into Aud_CampaignDBPOSMDtl
							(
								CampaignCode
								, DBCode
								, POSMCode
								, QtyDeliver
								, QtyRetrieve
								, DateDBRetrieve
								, FlagActive
								, LogLUDTime
								, LogLUBy
							)
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyDeliver
								, t.QtyRetrieve
								, t.DateDBRetrieve
								, t.FlagActive
								, t.LogLUDTime
								, t.LogLUBy
							from #input_Aud_CampaignDBPOSMDtl t --//[mylock]
							;
						");
					////
					string strSqlExec = CmUtils.StringUtils.Replace(@"
							----
							zzzzClauseInsert_Aud_Campaign_zSave
							----
							zzzzClauseInsert_Aud_CampaignDoc_zSave
							----
							zzzzClauseInsert_Aud_CampaignDBDtl_zSave
							----
							zzzzClauseInsert_Aud_CampaignDBPOSMDtl_zSave
							----
						"
						, "zzzzClauseInsert_Aud_Campaign_zSave", zzzzClauseInsert_Aud_Campaign_zSave
						, "zzzzClauseInsert_Aud_CampaignDoc_zSave", zzzzClauseInsert_Aud_CampaignDoc_zSave
						, "zzzzClauseInsert_Aud_CampaignDBDtl_zSave", zzzzClauseInsert_Aud_CampaignDBDtl_zSave
						, "zzzzClauseInsert_Aud_CampaignDBPOSMDtl_zSave", zzzzClauseInsert_Aud_CampaignDBPOSMDtl_zSave
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
		public DataSet Aud_Campaign_Approve(
			string strTid
			, DataRow drSession
			/////
			, object objCampaignCode
			, object objRemark
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Aud_Campaign_Approve";
			string strErrorCodeDefault = TError.ErrDemoLab.Aud_Campaign_Approve;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objCampaignCode", objCampaignCode
					, "objRemark", objRemark
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
				string strCampaignCode = TUtils.CUtils.StdParam(objCampaignCode);
				string strRemark = string.Format("{0}", objRemark).Trim();
				////
				DataTable dtDB_Aud_Campaign = null;
				{
					////
					Aud_Campaign_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCampaignCode // objCampaignCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.CampaignStatus.Pending // strStatusListToCheck
						, out dtDB_Aud_Campaign // dtDB_Aud_Campaign
						);
					////
					string strSqlCheck_Aud_CampaignDBDtl = CmUtils.StringUtils.Replace(@"
							select top 1
								t.*
							from Aud_CampaignDBDtl t --//[mylock]
							where (1=1)
								and t.CampaignCode = '@strCampaignCode'
							;
						"
						, "@strCampaignCode", strCampaignCode
						);

					DataTable dtDB_Check_Aud_CampaignDBDtl = _cf.db.ExecQuery(strSqlCheck_Aud_CampaignDBDtl).Tables[0];
					////
					if (dtDB_Check_Aud_CampaignDBDtl.Rows.Count < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strCampaignCode", strCampaignCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Approve_CampaignDBDtlNotFound
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region //// SaveTemp Aud_Campaign:
				{
					////
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db
						, "#input_Aud_Campaign"
						, TConst.BizMix.Default_DBColType // strDefaultType
						, new object[]{
							"CampaignCode"
							, "Appr1DTime"
							, "Appr1By"
							, "CampaignStatus"
							, "Remark"
							, "LogLUDTime"
							, "LogLUBy"
							}
						, new object[]{
							new object[]{
								strCampaignCode, // CampaignCode
								dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"), // Appr1DTime
								_cf.sinf.strUserCode, // Appr1By
								TConst.CampaignStatus.Approve1, // CampaignStatus
								strRemark, // Remark
								dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"), // LogLUDTime
								_cf.sinf.strUserCode, // LogLUBy
								}
							}
						);
				}
				#endregion

				#region // SaveDB:
				{
					////
					string zzB_Update_Aud_Campaign_ClauseSet_zzE = @"
								t.LogLUDTime = f.LogLUDTime
								, t.LogLUBy = f.LogLUBy
								, t.Appr1DTime = f.Appr1DTime
								, t.Appr1By = f.Appr1By
								, t.CampaignStatus = f.CampaignStatus
								, t.Remark = f.Remark
								";
					//// 
					string zzB_Update_Aud_Campaign_zzE = CmUtils.StringUtils.Replace(@"
							---- Aud_Campaign:
							update t
							set 
								zzB_Update_Aud_Campaign_ClauseSet_zzE
							from Aud_Campaign t --//[mylock]
								inner join #input_Aud_Campaign f --//[mylock]
									on t.CampaignCode = f.CampaignCode
							where (1=1)
							;
						"
						, "zzB_Update_Aud_Campaign_ClauseSet_zzE", zzB_Update_Aud_Campaign_ClauseSet_zzE
						);
					//// 
					string strSql_SaveOnDB = CmUtils.StringUtils.Replace(@"
							----
							zzB_Update_Aud_Campaign_zzE
							---- 
						"
						, "zzB_Update_Aud_Campaign_zzE", zzB_Update_Aud_Campaign_zzE
						);

					DataSet dsDB_Check = _cf.db.ExecQuery(
						strSql_SaveOnDB
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
		public DataSet Aud_Campaign_Cancel(
			string strTid
			, DataRow drSession
			/////
			, object objCampaignCode
			, object objRemark
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			DateTime dtimeSys = DateTime.Now;
			string strFunctionName = "Aud_Campaign_Cancel";
			string strErrorCodeDefault = TError.ErrDemoLab.Aud_Campaign_Cancel;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objCampaignCode", objCampaignCode
					, "objRemark", objRemark
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
				string strCampaignCode = TUtils.CUtils.StdParam(objCampaignCode);
				string strRemark = string.Format("{0}", objRemark).Trim();
				////
				DataTable dtDB_Aud_Campaign = null;
				{
					////
					Aud_Campaign_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCampaignCode // objCampaignCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.CampaignStatus.Pending // strStatusListToCheck
						, out dtDB_Aud_Campaign // dtDB_Aud_Campaign
						);
					////
					string strSqlCheck_Aud_CampaignDBDtl = CmUtils.StringUtils.Replace(@"
							select top 1
								t.*
							from Aud_CampaignDBDtl t --//[mylock]
							where (1=1)
								and t.CampaignCode = '@strCampaignCode'
							;
						"
						, "@strCampaignCode", strCampaignCode
						);

					DataTable dtDB_Check_Aud_CampaignDBDtl = _cf.db.ExecQuery(strSqlCheck_Aud_CampaignDBDtl).Tables[0];
					////
					if (dtDB_Check_Aud_CampaignDBDtl.Rows.Count < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strCampaignCode", strCampaignCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_Campaign_Cancel_CampaignDBDtlNotFound
							, null
							, alParamsCoupleError.ToArray()
							);
					}
				}
				#endregion

				#region //// SaveTemp Aud_Campaign:
				{
					////
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db
						, "#input_Aud_Campaign"
						, TConst.BizMix.Default_DBColType // strDefaultType
						, new object[]{
							"CampaignCode"
							, "CancelDTime"
							, "CancelBy"
							, "CampaignStatus"
							, "Remark"
							, "LogLUDTime"
							, "LogLUBy"
							}
						, new object[]{
							new object[]{
								strCampaignCode, // CampaignCode
								dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"), // CancelDTime
								_cf.sinf.strUserCode, // CancelBy
								TConst.CampaignStatus.Cancel, // CampaignStatus
								strRemark, // Remark
								dtimeSys.ToString("yyyy-MM-dd HH:mm:ss"), // LogLUDTime
								_cf.sinf.strUserCode, // LogLUBy
								}
							}
						);
				}
				#endregion

				#region // SaveDB:
				{
					////
					string zzB_Update_Aud_Campaign_ClauseSet_zzE = @"
								t.LogLUDTime = f.LogLUDTime
								, t.LogLUBy = f.LogLUBy
								, t.CancelDTime = f.CancelDTime
								, t.CancelBy = f.CancelBy
								, t.CampaignStatus = f.CampaignStatus
								, t.Remark = f.Remark
								";
					////
					string zzB_Update_Aud_Campaign_zzE = CmUtils.StringUtils.Replace(@"
							---- Aud_Campaign:
							update t
							set 
								zzB_Update_Aud_Campaign_ClauseSet_zzE
							from Aud_Campaign t --//[mylock]
								inner join #input_Aud_Campaign f --//[mylock]
									on t.CampaignCode = f.CampaignCode
							where (1=1)
							;
						"
						, "zzB_Update_Aud_Campaign_ClauseSet_zzE", zzB_Update_Aud_Campaign_ClauseSet_zzE
						);
					////
					string strSql_SaveOnDB = CmUtils.StringUtils.Replace(@"
							----
							zzB_Update_Aud_Campaign_zzE
							----
						"
						, "zzB_Update_Aud_Campaign_zzE", zzB_Update_Aud_Campaign_zzE
						);

					DataSet dsDB_Check = _cf.db.ExecQuery(
						strSql_SaveOnDB
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

		#region // Aud_CampaignDBReceive:
		public DataSet Aud_CampaignDBReceive_Save(
			string strTid
			, DataRow drSession
			////
			, object objFlagIsDelete
			////	
			, object objDBReceiveNo
			, object objCampaignCode 
			, object[] arrobjDSData
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bMyDebugSql = false;
			DateTime dtimeSys = DateTime.Now;
			bool bNeedTransaction = true;
			string strFunctionName = "Aud_CampaignDBReceive_Save";
			string strErrorCodeDefault = TError.ErrDemoLab.Aud_CampaignDBReceive_Save;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "dtimeSys", dtimeSys.ToString("yyyy-MM-dd HH:mm:ss")
					////
					, "objFlagIsDelete", objFlagIsDelete
					////
					, "objDBReceiveNo", objDBReceiveNo
					, "objCampaignCode", objCampaignCode
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

				#region //// Refine and Check Aud_Campaign:
				////
				bool bIsDelete = CmUtils.StringUtils.StringEqual(objFlagIsDelete, TConst.Flag.Yes);
				////
				string strDBReceiveNo = TUtils.CUtils.StdParam(objDBReceiveNo);
				string strCampaignCode = TUtils.CUtils.StdParam(objCampaignCode);

				////
				DataTable dtDB_Aud_Campaign = null;
				{

					Aud_Campaign_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, strCampaignCode // objCampaignCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, TConst.CampaignStatus.Approve1 // strStatusListToCheck
						, out dtDB_Aud_Campaign // dtDB_Aud_Campaign
						);
				}
				#endregion

				#region //// SaveTemp Aud_Campaign:
				if (!bIsDelete)
				{
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db
						, "#input_Aud_Campaign"
						, new object[]{
							"CampaignCode", TConst.BizMix.Default_DBColType,
							}
						, new object[]{
							new object[]{
								strCampaignCode, // strCampaignCode
								}
							}
						);
				}
				#endregion

				#region //// Refine and Check Aud_CampaignDBReceive:
				DataTable dtInput_Aud_CampaignDBReceive = null;
				if (!bIsDelete)
				{
					////
					string strTableCheck = "Aud_CampaignDBReceive";
					////
					if (!dsData.Tables.Contains(strTableCheck))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.TableName", strTableCheck
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_CampaignDBReceive_Save_Input_CampaignDBReceiveTblNotFound
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					dtInput_Aud_CampaignDBReceive = dsData.Tables[strTableCheck];
					////
					TUtils.CUtils.StdDataInTable(
						dtInput_Aud_CampaignDBReceive // dtData
						, "StdParam", "DBCode" // arrstrCouple
						, "StdParam", "POSMCode" // arrstrCouple
						, "", "QtyDBRec" // arrstrCouple
						);
					////
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBReceive, "DBReceiveNo", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBReceive, "CampaignCode", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBReceive, "DateDBReceive", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBReceive, "CreateDTime", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBReceive, "CreateBy", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBReceive, "LogLUDTime", typeof(object));
					TUtils.CUtils.MyForceNewColumn(ref dtInput_Aud_CampaignDBReceive, "LogLUBy", typeof(object));
					////
					for (int nScan = 0; nScan < dtInput_Aud_CampaignDBReceive.Rows.Count; nScan++)
					{
						////
						DataRow drScan = dtInput_Aud_CampaignDBReceive.Rows[nScan];

						////
						DataTable dtDB_Aud_CampaignDBPOSMDtl = null;

						/*Aud_CampaignDBPOSMDtl_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, drScan["CampaignCode"] // drScan["CampaignCode"]
							, drScan["DBCode"] // drScan["DBCode"]
							, drScan["POSMCode"] // drScan["POSMCode"]
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Aud_CampaignDBPOSMDtl // dtDB_Aud_CampaignDBPOSMDtl
							);*/
						////
						DataTable dtDB_Aud_CampaignDBDtl = null;

						/*Aud_CampaignDBDtl_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, drScan["CampaignCode"] // drScan["CampaignCode"]
							, drScan["DBCode"] // drScan["DBCode"]
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strStatusListToCheck
							, out dtDB_Aud_CampaignDBDtl // dtDB_Aud_CampaignDBDtl
							);*/
						////
						DataTable dtDB_Mst_Distributor = null;

						Mst_Distributor_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, drScan["DBCode"] // drScan["DBCode"]
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strStatusListToCheck
							, out dtDB_Mst_Distributor // dtDB_Mst_Distributor
							);
						////
						DataTable dtDB_Mst_POSM = null;

						Mst_POSM_CheckDB(
							 ref alParamsCoupleError // alParamsCoupleError
							, drScan["POSMCode"] // drScan["POSMCode"]
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.Flag.Active // strFlagActiveListToCheck
							, out dtDB_Mst_POSM // dtDB_Mst_POSM
							);
						//// 
						double dbQtyDBRec = Convert.ToDouble(drScan["QtyDBRec"]);
						if (dbQtyDBRec <= 0.0)
						{
							alParamsCoupleError.AddRange(new object[]{
								"Check.dbQtyDBRec", dbQtyDBRec
								});
							throw CmUtils.CMyException.Raise(
								TError.ErrDemoLab.Aud_CampaignDBReceive_Save_InvalidValue
								, null
								, alParamsCoupleError.ToArray()
								);
						}

						////
						drScan["DBReceiveNo"] = strDBReceiveNo;
						drScan["CampaignCode"] = strCampaignCode;
						drScan["DateDBReceive"] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
						drScan["CreateDTime"] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
						drScan["CreateBy"] = _cf.sinf.strUserCode;
						drScan["LogLUDTime"] = dtimeSys.ToString("yyyy-MM-dd HH:mm:ss");
						drScan["LogLUBy"] = _cf.sinf.strUserCode;
						////
					}
				}
				#endregion

				#region //// SaveTemp Aud_CampaignDBReceive:
				if (!bIsDelete)
				{
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db
						, "#input_Aud_CampaignDBReceive"
						, new object[]{
							"DBReceiveNo", TConst.BizMix.Default_DBColType,
							"CampaignCode", TConst.BizMix.Default_DBColType,
							"DBCode", TConst.BizMix.Default_DBColType,
							"POSMCode", TConst.BizMix.Default_DBColType,
							"QtyDBRec", "float",
							"DateDBReceive", TConst.BizMix.Default_DBColType,
							"CreateDTime", TConst.BizMix.Default_DBColType,
							"CreateBy", TConst.BizMix.Default_DBColType,
							"LogLUDTime", TConst.BizMix.Default_DBColType,
							"LogLUBy", TConst.BizMix.Default_DBColType,
							}
						, dtInput_Aud_CampaignDBReceive
						);
				}
				#endregion

				#region //// Save:
				//// Clear All:
				{
					string strSqlDelete = CmUtils.StringUtils.Replace(@"
								---- Aud_CampaignDBReceive:
								delete t
								from Aud_CampaignDBReceive  t 
								where (1=1)
									and t.DBReceiveNo = @strDBReceiveNo
									and t.CampaignCode = @strCampaignCode
								;

							");
					_cf.db.ExecQuery(
						strSqlDelete
						, "@strDBReceiveNo", strDBReceiveNo
						, "@strCampaignCode", strCampaignCode
						);
				}

				//// Insert All:
				if (!bIsDelete)
				{
					////
					string zzzzClauseInsert_Aud_CampaignDBReceive_zSave = CmUtils.StringUtils.Replace(@"
							---- Aud_CampaignDBReceive:
							insert into Aud_CampaignDBReceive
							(
								DBReceiveNo 
								, CampaignCode
								, DBCode
								, POSMCode
								, QtyDBRec
								, DateDBReceive 
								, CreateDTime
								, CreateBy 
								, LogLUDTime
								, LogLUBy
							)
							select 
								t.DBReceiveNo 
								, t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyDBRec
								, t.DateDBReceive
								, t.CreateDTime
								, t.CreateBy
								, t.LogLUDTime
								, t.LogLUBy
							from #input_Aud_CampaignDBReceive t --//[mylock]
							;
						");
					////
					string strSqlExec = CmUtils.StringUtils.Replace(@"
							----
							zzzzClauseInsert_Aud_CampaignDBReceive_zSave
							----
						"
						, "zzzzClauseInsert_Aud_CampaignDBReceive_zSave", zzzzClauseInsert_Aud_CampaignDBReceive_zSave
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

				#region //// Check Total QtyDBRec <= QtyDeliver:
				if (!bIsDelete)
				{
					string strSqlCheck = CmUtils.StringUtils.Replace(@"
					        ---- #tbl_Aud_CampaignDBReceive_TotalQtyDBRec:
					        select 
                                t.CampaignCode
                                , t.DBCode
                                , t.POSMCode
                                , Sum(t.QtyDBRec) TotalQtyDBRec
                            into #tbl_Aud_CampaignDBReceive_TotalQtyDBRec
                            from Aud_CampaignDBReceive t --//[mylock]
                                inner join #input_Aud_Campaign f --//[mylock]
                                    on t.CampaignCode = f.CampaignCode
                            where (1=1)
                            group by
                                t.CampaignCode
                                , t.DBCode
                                , t.POSMCode
                            ;

					        --- Check:
					        select
                                t.CampaignCode
                                , t.DBCode
                                , t.POSMCode
                                , t.QtyDeliver 
                                , isnull(f.TotalQtyDBRec, 0) TotalQtyDBRec
                            from Aud_CampaignDBPOSMDtl t --//[mylock]
                                left join #tbl_Aud_CampaignDBReceive_TotalQtyDBRec f  --//[mylock]
                                    on t.CampaignCode = f.CampaignCode
				                        and t.DBCode = f.DBCode
				                        and t.POSMCode = f.POSMCode
                            where (1=1)
                                and (t.QtyDeliver < isnull(f.TotalQtyDBRec, 0))
                            ;

					        ---- Clear for Debug:
					        drop table #tbl_Aud_CampaignDBReceive_TotalQtyDBRec;
				        ");

					DataTable dtCheck = _cf.db.ExecQuery(strSqlCheck).Tables[0];
					////
					if (dtCheck.Rows.Count > 0)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.CampaignCode", dtCheck.Rows[0]["CampaignCode"]
							, "Check.DBCode", dtCheck.Rows[0]["DBCode"]
							, "Check.POSMCode", dtCheck.Rows[0]["POSMCode"]
							, "Check.QtyDeliver", dtCheck.Rows[0]["QtyDeliver"]
							, "Check.TotalQtyDBRec", dtCheck.Rows[0]["TotalQtyDBRec"]
							, "Check.ConditionRaiseError", "(t.QtyDBRec > f.TotalQtyDBRec)"
							, "Check.ErrRows.Count", dtCheck.Rows.Count
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Aud_CampaignDBReceive_Save_InvalidQtyDBRec
							, null
							, alParamsCoupleError.ToArray()
							);
					}
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

		#region // Aud_CampaignDoc:
		private void Aud_CampaignDoc_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objCampaignCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Aud_CampaignDoc
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Aud_CampaignDoc t --//[mylock]
					where (1=1)
						and t.CampaignCode = @objCampaignCode
					;
				");
			dtDB_Aud_CampaignDoc = _cf.db.ExecQuery(
				strSqlExec
				, "@objCampaignCode", objCampaignCode
				).Tables[0];
			dtDB_Aud_CampaignDoc.TableName = "Aud_CampaignDoc";

			// strFlagExistToCheck
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Aud_CampaignDoc.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCode", objCampaignCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Aud_CampaignDoc_CheckDB_CampaignDocNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Aud_CampaignDoc.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCode", objCampaignCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Aud_CampaignDoc_CheckDB_CampaignDocExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Aud_CampaignDoc.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CampaignCode", objCampaignCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Aud_CampaignDoc.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Aud_CampaignDoc_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		#endregion

		#region // Aud_CampaignDBDtl:
		private void Aud_CampaignDBDtl_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objCampaignCode
			, object objDBCode
			, string strFlagExistToCheck
			, string strStatusListToCheck
			, out DataTable dtDB_Aud_CampaignDBDtl
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Aud_CampaignDBDtl t --//[mylock]
					where (1=1)
						and t.CampaignCode = @objCampaignCode
						and t.DBCode = @objDBCode
					;
				");
			dtDB_Aud_CampaignDBDtl = _cf.db.ExecQuery(
				strSqlExec
				, "@objCampaignCode", objCampaignCode
				, "@objDBCode", objDBCode
				).Tables[0];
			dtDB_Aud_CampaignDBDtl.TableName = "Aud_CampaignDBDtl";

			// strFlagExistToCheck
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Aud_CampaignDBDtl.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCode", objCampaignCode
						, "Check.DBCode", objDBCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Aud_CampaignDBDtl_CheckDB_CampaignDBDtlNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Aud_CampaignDBDtl.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCode", objCampaignCode
						, "Check.DBCode", objDBCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Aud_CampaignDBDtl_CheckDB_CampaignDBDtlExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strStatusListToCheck:
			if (strStatusListToCheck.Length > 0 && !strStatusListToCheck.Contains(Convert.ToString(dtDB_Aud_CampaignDBDtl.Rows[0]["CampaignDBStatusDtl"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CampaignCode", objCampaignCode
					, "Check.DBCode", objDBCode
					, "Check.DBStatusListToCheck", strStatusListToCheck
					, "DB.CampaignDBStatusDtl", dtDB_Aud_CampaignDBDtl.Rows[0]["CampaignDBStatusDtl"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Aud_CampaignDBDtl_CheckDB_DBStatusNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		#endregion

		#region // Aud_CampaignDBPOSMDtl:
		private void Aud_CampaignDBPOSMDtl_CheckDB(
			ref ArrayList alParamsCoupleError
			, object objCampaignCode
			, object objDBCode
			, object objPOSMCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Aud_CampaignDBPOSMDtl
			)
		{
			// GetInfo:
			string strSqlExec = CmUtils.StringUtils.Replace(@"
					select top 1
						t.*
					from Aud_CampaignDBPOSMDtl t --//[mylock]
					where (1=1)
						and t.CampaignCode = @objCampaignCode
						and t.DBCode = @objDBCode
						and t.POSMCode = @objPOSMCode
					;
				");
			dtDB_Aud_CampaignDBPOSMDtl = _cf.db.ExecQuery(
				strSqlExec
				, "@objCampaignCode", objCampaignCode
				, "@objDBCode", objDBCode
				, "@objPOSMCode", objPOSMCode
				).Tables[0];
			dtDB_Aud_CampaignDBPOSMDtl.TableName = "Aud_CampaignDBPOSMDtl";

			// strFlagExistToCheck
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Aud_CampaignDBPOSMDtl.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCode", objCampaignCode
						, "Check.DBCode", objDBCode
						, "Check.POSMCode", objPOSMCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Aud_CampaignDBPOSMDtl_CheckDB_CampaignDBPOSMDtlNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Aud_CampaignDBPOSMDtl.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.CampaignCode", objCampaignCode   
						, "Check.DBCode", objDBCode
						, "Check.POSMCode", objPOSMCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Aud_CampaignDBPOSMDtl_CheckDB_CampaignDBPOSMDtlExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Aud_CampaignDBPOSMDtl.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CampaignCode", objCampaignCode
					, "Check.DBCode", objDBCode
					, "Check.POSMCode", objPOSMCode
					, "Check.strFlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Aud_CampaignDBPOSMDtl.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Aud_CampaignDBPOSMDtl_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		#endregion

		#region MyRegion

		#endregion

	}
}

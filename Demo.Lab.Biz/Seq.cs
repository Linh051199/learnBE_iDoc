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
		///*
		private long Seq_Common_Raw(string strTableName)
		{
			// Init:
			string strSql = string.Format(@"
					insert into {0} values (null);
					delete {0} where AutoID = @@Identity;
					select @@Identity Tid;
				", strTableName
			);
			DataSet ds = _cf.db.ExecQuery(strSql);

			// Return Good:
			return Convert.ToInt64(ds.Tables[0].Rows[0][0]);
		}
		private string Seq_Common_MyGet(
			ref ArrayList alParamsCoupleError
			, string strSequenceType
			, string strParam_Prefix
			, string strParam_Postfix
			)
		{
			#region // Get and Check Map:
			Hashtable htMap = new Hashtable();
			htMap.Add(TConst.SeqType.Id, new string[] { "Seq_Id", "{0}{1}{2}", "999000000000" });
			//htMap.Add(TConst.SeqType.DiscountDBCode, new string[] { "Seq_MasterDataId", "{0}DCDB.{3}.{1:0000}{2}", "10000" });
			htMap.Add(TConst.SeqType.InsuranceClaimDocCode, new string[] { "Seq_TransactionDataId", "{0}ICDC.{3}.{1:00000}{2}", "100000" });
			htMap.Add(TConst.SeqType.InsuranceClaimNo, new string[] { "Seq_TransactionDataId", "{0}ICN.{3}.{1:00000}{2}", "100000" });
			htMap.Add(TConst.SeqType.WorkingRecordNo, new string[] { "Seq_TransactionDataId", "{0}WRN.{3}.{1:00000}{2}", "100000" });
			htMap.Add(TConst.SeqType.LevelCode, new string[] { "Seq_TransactionDataId", "{0}LVC.{3}.{1:00000}{2}", "100000" });
			htMap.Add(TConst.SeqType.CampaignCrAwardCode, new string[] { "Seq_TransactionDataId", "{0}CCRAC.{3}.{1:00000}{2}", "100000" });
			htMap.Add(TConst.SeqType.CampaignCode, new string[] { "Seq_TransactionDataId", "{0}CCRAC.{3}.{1:00000}{2}", "100000" });
			htMap.Add(TConst.SeqType.CICode, new string[] { "Seq_TransactionDataId", "{0}CIC.{3}.{1:00000}{2}", "100000" });
			if (!htMap.ContainsKey(strSequenceType))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.strSequenceType", strSequenceType
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Seq_Common_MyGet_InvalidSequenceType
					, null
					, alParamsCoupleError.ToArray()
					);
			}
			string[] arrstrMap = (string[])htMap[strSequenceType];
			string strTableName = arrstrMap[0];
			string strFormat = arrstrMap[1];
			long nMaxSeq = Convert.ToInt64(arrstrMap[2]);

			#endregion

			#region // SequenceGet:
			string strResult = "";
			long nSeq = Seq_Common_Raw(strTableName);
			string strMyEncrypt = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			strResult = string.Format(
				strFormat // Format
				, strParam_Prefix // {0}
				, nSeq % nMaxSeq // {1}
				, strParam_Postfix // {2}
				, string.Format("{0}{1}{2}", strMyEncrypt[DateTime.Now.Year - TConst.BizMix.Default_RootYear], strMyEncrypt[DateTime.Now.Month], strMyEncrypt[DateTime.Now.Day]) // {3}
				);
			#endregion

			// Return Good:
			return strResult;
		}

		public DataSet Seq_Common_Get(
			string strTid
			, DataRow drSession
			////
			, string strSequenceType
			, string strParam_Prefix
			, string strParam_Postfix
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Seq_Common_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Seq_Common_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "strSequenceType", strSequenceType
					, "strParam_Prefix", strParam_Prefix
					, "strParam_Postfix", strParam_Postfix
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
				#endregion

				#region // Refine and Check Input:
				////
				strSequenceType = TUtils.CUtils.StdParam(strSequenceType);
				strParam_Prefix = TUtils.CUtils.StdParam(strParam_Prefix);
				strParam_Postfix = TUtils.CUtils.StdParam(strParam_Postfix);
				////
				#endregion

				#region // SequenceGet:
				////
				string strResult = Seq_Common_MyGet(
					ref alParamsCoupleError // alParamsCoupleError
					, strSequenceType // strSequenceType
					, strParam_Prefix // strParam_Prefix
					, strParam_Postfix // strParam_Postfix
					);
				CmUtils.CMyDataSet.SetRemark(ref mdsFinal, strResult);
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
		//*/

	}
}

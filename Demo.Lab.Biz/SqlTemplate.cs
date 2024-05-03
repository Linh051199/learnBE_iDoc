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
	public class SqlTemplate_Aud_Campaign_Old_Old_20140507
	{
		public static string zzB_tbl_Aud_CampaignDBReceive_POSM_Total_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_Aud_CampaignDBReceive_POSM_Total:
							select 
								acdbrec.CampaignCode
								, acdbrec.DBCode
								, acdbrecdt.POSMCode
								, IsNull(Sum(acdbrecdt.QtyDBRec), 0.0) TotalQtyDBRec
							into #tbl_Aud_CampaignDBReceive_POSM_Total
							from Aud_CampaignDBReceive acdbrec --//[mylock]
								inner join Aud_CampaignDBReceiveDtl acdbrecdt  --//[mylock]
									on acdbrec.DBReceiveNo = acdbrecdt.DBReceiveNo
							where (1=1)
							group by
								acdbrec.CampaignCode
								, acdbrec.DBCode
								, acdbrecdt.POSMCode
							;

							select null tbl_Aud_CampaignDBReceive_POSM_Total, * from #tbl_Aud_CampaignDBReceive_POSM_Total;
				");
			return strSql;
		}
		public static string zzB_tbl_Aud_CampaignOLReceive_POSM_Total_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							select 
								acoldt.CampaignCode
								, acoldt.DBCode
								, acolposmdt.POSMCode
								, IsNull(Sum(acolposmdt.QtyOLRec), 0.0) TotalQtyOLRec
							into #tbl_Aud_CampaignOLReceive_POSM_Total
							from Aud_CampaignOLDtl acoldt --//[mylock]
								left join Aud_CampaignOLPOSMDtl acolposmdt --//[mylock]
									on acoldt.CampaignCode = acolposmdt.CampaignCode
										and acoldt.OLCode = acolposmdt.OLCode
							where (1=1)
							group by
								acoldt.CampaignCode
								, acoldt.DBCode
								, acolposmdt.POSMCode
							;

							select null tbl_Aud_CampaignOLReceive_POSM_Total, * from #tbl_Aud_CampaignOLReceive_POSM_Total;
						
				");
			return strSql;
		}
		public static string zzB_tbl_CheckLimit_zzE(string strDBCode)
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_CheckLimit:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.TotalQty
								, IsNull(f.TotalQtyDBRec, 0.0) TotalQtyDBRec
							into #tbl_CheckLimit
							from #tbl_Aud_Campaign_DBPOSM_Total t --//[mylock]
								left join #tbl_Aud_CampaignDBReceive_POSM_Total f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.DBCode = f.DBCode
										and t.POSMCode = f.POSMCode
							where (1=1)
								and t.DBCode = '@strDBCode'
								and t.TotalQty < f.TotalQtyDBRec
							;

							select null tbl_CheckLimit, * from #tbl_CheckLimit;						
				"
				, "@strDBCode", strDBCode
				);
			return strSql;
		}
		public static string zzB_tbl_CheckLimitDBRec_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_CheckLimitDBRec:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyDeliver
								, IsNull(f.TotalQtyDBRec, 0.0) TotalQtyDBRec
							into #tbl_CheckLimitDBRec
							from Aud_CampaignDBPOSMDtl t --//[mylock]
								left join #tbl_Aud_CampaignDBReceive_POSM_Total f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.DBCode = f.DBCode
										and t.POSMCode = f.POSMCode
							where (1=1)
								--and t.DBCode = '@strDBCode'
								and t.QtyDeliver < f.TotalQtyDBRec
							;

							select null tbl_CheckLimitDBRec, * from #tbl_CheckLimitDBRec;					
				"
				);
			return strSql;
		}
		public static string zzB_tbl_CheckLimitOLRec_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_CheckLimitOLRec:
							select 
								t.CampaignCode
								, t.OLCode
								, t.POSMCode
								, t.QtyDeliver
								, IsNull(t.QtyOLRec, 0.0) QtyOLRec
							into #tbl_CheckLimitOLRec
							from Aud_CampaignOLPOSMDtl t --//[mylock]
							where (1=1)
								and t.QtyDeliver < t.QtyOLRec
							;

							select null tbl_CheckLimitOLRec, * from #tbl_CheckLimitOLRec;					
				"
				);
			return strSql;
		}
		public static string zzB_tbl_CheckLimitDBRetrieve_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_CheckLimitDBRetrieve_Draft:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyRetrieve
								, IsNull(f.TotalQtyDBRec, 0.0) TotalQtyDBRec
							into #tbl_CheckLimitDBRetrieve_Draft
							from Aud_CampaignDBPOSMDtl t --//[mylock]
								left join #tbl_Aud_CampaignDBReceive_POSM_Total f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.DBCode = f.DBCode
										and t.POSMCode = f.POSMCode
							where (1=1)
								--and t.DBCode = '@strDBCode'
								--and t.QtyRetrieve > f.TotalQtyDBRec
							;
		
							select null tbl_CheckLimitDBRetrieve_Draft, * from #tbl_CheckLimitDBRetrieve_Draft;

							---- #tbl_CheckLimitDBRetrieve:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyRetrieve
								, t.TotalQtyDBRec
							into #tbl_CheckLimitDBRetrieve
							from #tbl_CheckLimitDBRetrieve_Draft t --//[mylock]
							where (1=1)
								--and t.DBCode = '@strDBCode'
								and t.QtyRetrieve > t.TotalQtyDBRec
							;

							select null tbl_CheckLimitDBRetrieve, * from #tbl_CheckLimitDBRetrieve;					
				"
				);
			return strSql;
		}
		public static string zzB_tbl_CheckLimitDBDeliverOLOverLimtPOSM_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_Aud_CampaignOLPOSMDtl:
							select 
								acoldt.CampaignCode CampaignCode
								, acoldt.DBCode DBCode
								, acolposmdt.POSMCode POSMCode
								, Sum(acolposmdt.QtyDeliver) QtyDBDeliverOL
							into #tbl_Aud_CampaignOLPOSMDtl
							from Aud_CampaignOLDtl acoldt --//[mylock]
								left join Aud_CampaignOLPOSMDtl acolposmdt --//[mylock]
									on acoldt.CampaignCode = acolposmdt.CampaignCode
										and acoldt.OLCode = acolposmdt.OLCode
							where(1=1)
							group by 
								acoldt.CampaignCode
								, acoldt.DBCode
								, acolposmdt.POSMCode
								, acolposmdt.QtyDeliver
							;
							select null tbl_Aud_CampaignOLPOSMDtl, * from #tbl_Aud_CampaignOLPOSMDtl;

							---- #tbl_CheckLimitDBDeliverOLOverLimtPOSM:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyDeliver
								, IsNull(f.QtyDBDeliverOL, 0.0) QtyDBDeliverOL
							into #tbl_CheckLimitDBDeliverOLOverLimtPOSM
							from Aud_CampaignDBPOSMDtl t --//[mylock]
								left join #tbl_Aud_CampaignOLPOSMDtl f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.DBCode = f.DBCode
										and t.POSMCode = f.POSMCode
							where (1=1)
							;

							select 
								t.*
							from #tbl_CheckLimitDBDeliverOLOverLimtPOSM t --//[mylock]
							where (1=1)
								and t.QtyDeliver < t.QtyDBDeliverOL
							;					
				"
				);
			return strSql;
		}
		public static string zzB_tbl_Aud_Campaign_DBReqOLDeliver_Total_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_Aud_Campaign_DBReqOLDeliver_Total:
							select
								t.CampaignCode
								, t.DBCode
								, f.POSMCode
								, Sum(f.QtyDeliver) TotalDBReqOLDeliver 
							into #tbl_Aud_Campaign_DBReqOLDeliver_Total
							from Aud_CampaignOLDtl t --//[mylock]
								inner join Aud_CampaignOLPOSMDtl f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.OLCode = f.OLCode
							where (1=1)
							group by
								t.CampaignCode
								, t.DBCode
								, f.POSMCode
							;

							select null tbl_Aud_Campaign_DbReqOLDeliver_Total, * from #tbl_Aud_Campaign_DbReqOLDeliver_Total;					
				"
				);
			return strSql;
		}
	}

	public class SqlTemplate_Aud_Campaign
	{
		public static string zzB_tbl_Aud_CampaignDBReceive_POSM_Total_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_Aud_CampaignDBReceive_POSM_Total:
							select
								acdbrec.CampaignCode
								, acdbrec.DBCode
								, acdbrec.POSMCode
								, IsNull(Sum(acdbrec.QtyDBRec), 0.0) TotalQtyDBRec
							into #tbl_Aud_CampaignDBReceive_POSM_Total
							from Aud_CampaignDBReceive acdbrec --//[mylock]
							where (1=1)
							group by
								acdbrec.CampaignCode
								, acdbrec.DBCode
								, acdbrec.POSMCode
							;
							select null tbl_Aud_CampaignDBReceive_POSM_Total, * from #tbl_Aud_CampaignDBReceive_POSM_Total;
				");
			return strSql;
		}
		public static string zzB_tbl_Aud_CampaignOLReceive_POSM_Total_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							select 
								acoldt.CampaignCode
								, acoldt.DBCode
								, acolposmdt.POSMCode
								, IsNull(Sum(acolposmdt.QtyOLRec), 0.0) TotalQtyOLRec
							into #tbl_Aud_CampaignOLReceive_POSM_Total
							from Aud_CampaignOLDtl acoldt --//[mylock]
								left join Aud_CampaignOLPOSMDtl acolposmdt --//[mylock]
									on acoldt.CampaignCode = acolposmdt.CampaignCode
										and acoldt.OLCode = acolposmdt.OLCode
							where (1=1)
							group by
								acoldt.CampaignCode
								, acoldt.DBCode
								, acolposmdt.POSMCode
							;

							select null tbl_Aud_CampaignOLReceive_POSM_Total, * from #tbl_Aud_CampaignOLReceive_POSM_Total;
						
				");
			return strSql;
		}
		public static string zzB_tbl_CheckLimit_zzE(string strDBCode)
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_CheckLimit:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.TotalQty
								, IsNull(f.TotalQtyDBRec, 0.0) TotalQtyDBRec
							into #tbl_CheckLimit
							from #tbl_Aud_Campaign_DBPOSM_Total t --//[mylock]
								left join #tbl_Aud_CampaignDBReceive_POSM_Total f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.DBCode = f.DBCode
										and t.POSMCode = f.POSMCode
							where (1=1)
								and t.DBCode = '@strDBCode'
								and t.TotalQty < f.TotalQtyDBRec
							;

							select null tbl_CheckLimit, * from #tbl_CheckLimit;						
				"
				, "@strDBCode", strDBCode
				);
			return strSql;
		}
		public static string zzB_tbl_CheckLimitDBRec_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_CheckLimitDBRec:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyDeliver
								, IsNull(f.TotalQtyDBRec, 0.0) TotalQtyDBRec
							into #tbl_CheckLimitDBRec
							from Aud_CampaignDBPOSMDtl t --//[mylock]
								left join #tbl_Aud_CampaignDBReceive_POSM_Total f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.DBCode = f.DBCode
										and t.POSMCode = f.POSMCode
							where (1=1)
								--and t.DBCode = '@strDBCode'
								and t.QtyDeliver < f.TotalQtyDBRec
							;

							select null tbl_CheckLimitDBRec, * from #tbl_CheckLimitDBRec;					
				"
				);
			return strSql;
		}
		public static string zzB_tbl_CheckLimitOLRec_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_CheckLimitOLRec:
							select 
								t.CampaignCode
								, t.OLCode
								, t.POSMCode
								, t.QtyDeliver
								, IsNull(t.QtyOLRec, 0.0) QtyOLRec
							into #tbl_CheckLimitOLRec
							from Aud_CampaignOLPOSMDtl t --//[mylock]
							where (1=1)
								and t.QtyDeliver < t.QtyOLRec
							;

							select null tbl_CheckLimitOLRec, * from #tbl_CheckLimitOLRec;					
				"
				);
			return strSql;
		}
		public static string zzB_tbl_CheckLimitDBRetrieve_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_CheckLimitDBRetrieve_Draft:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyRetrieve
								, IsNull(f.TotalQtyDBRec, 0.0) TotalQtyDBRec
							into #tbl_CheckLimitDBRetrieve_Draft
							from Aud_CampaignDBPOSMDtl t --//[mylock]
								left join #tbl_Aud_CampaignDBReceive_POSM_Total f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.DBCode = f.DBCode
										and t.POSMCode = f.POSMCode
							where (1=1)
								--and t.DBCode = '@strDBCode'
								--and t.QtyRetrieve > f.TotalQtyDBRec
							;
		
							select null tbl_CheckLimitDBRetrieve_Draft, * from #tbl_CheckLimitDBRetrieve_Draft;

							---- #tbl_CheckLimitDBRetrieve:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyRetrieve
								, t.TotalQtyDBRec
							into #tbl_CheckLimitDBRetrieve
							from #tbl_CheckLimitDBRetrieve_Draft t --//[mylock]
							where (1=1)
								--and t.DBCode = '@strDBCode'
								and t.QtyRetrieve > t.TotalQtyDBRec
							;

							select null tbl_CheckLimitDBRetrieve, * from #tbl_CheckLimitDBRetrieve;					
				"
				);
			return strSql;
		}
		public static string zzB_tbl_CheckLimitDBDeliverOLOverLimtPOSM_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_Aud_CampaignOLPOSMDtl:
							select 
								acoldt.CampaignCode CampaignCode
								, acoldt.DBCode DBCode
								, acolposmdt.POSMCode POSMCode
								, Sum(acolposmdt.QtyDeliver) QtyDBDeliverOL
							into #tbl_Aud_CampaignOLPOSMDtl
							from Aud_CampaignOLDtl acoldt --//[mylock]
								left join Aud_CampaignOLPOSMDtl acolposmdt --//[mylock]
									on acoldt.CampaignCode = acolposmdt.CampaignCode
										and acoldt.OLCode = acolposmdt.OLCode
							where(1=1)
							group by 
								acoldt.CampaignCode
								, acoldt.DBCode
								, acolposmdt.POSMCode
								, acolposmdt.QtyDeliver
							;
							select null tbl_Aud_CampaignOLPOSMDtl, * from #tbl_Aud_CampaignOLPOSMDtl;

							---- #tbl_CheckLimitDBDeliverOLOverLimtPOSM:
							select 
								t.CampaignCode
								, t.DBCode
								, t.POSMCode
								, t.QtyDeliver
								, IsNull(f.QtyDBDeliverOL, 0.0) QtyDBDeliverOL
							into #tbl_CheckLimitDBDeliverOLOverLimtPOSM
							from Aud_CampaignDBPOSMDtl t --//[mylock]
								left join #tbl_Aud_CampaignOLPOSMDtl f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.DBCode = f.DBCode
										and t.POSMCode = f.POSMCode
							where (1=1)
							;

							select 
								t.*
							from #tbl_CheckLimitDBDeliverOLOverLimtPOSM t --//[mylock]
							where (1=1)
								and t.QtyDeliver < t.QtyDBDeliverOL
							;					
				"
				);
			return strSql;
		}
		public static string zzB_tbl_Aud_Campaign_DBReqOLDeliver_Total_zzE()
		{
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_Aud_Campaign_DBReqOLDeliver_Total:
							select
								t.CampaignCode
								, t.DBCode
								, f.POSMCode
								, Sum(f.QtyDeliver) TotalDBReqOLDeliver 
							into #tbl_Aud_Campaign_DBReqOLDeliver_Total
							from Aud_CampaignOLDtl t --//[mylock]
								inner join Aud_CampaignOLPOSMDtl f --//[mylock]
									on t.CampaignCode = f.CampaignCode
										and t.OLCode = f.OLCode
							where (1=1)
							group by
								t.CampaignCode
								, t.DBCode
								, f.POSMCode
							;

							select null tbl_Aud_Campaign_DbReqOLDeliver_Total, * from #tbl_Aud_Campaign_DbReqOLDeliver_Total;					
				"
				);
			return strSql;
		}
	}
}

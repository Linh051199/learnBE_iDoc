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
		#region // Sys Mix:
		private DataRow Sys_User_GetAbilityViewOfUser(
			object strUserCode
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
					select
						su.UserCode
						, su.DBCode
						, su.AreaCode
						, su.UserName
						, su.FlagSysAdmin
						, su.FlagDBAdmin
						, su.FlagActive
						, mb.DBCode MBDBCode
						, mb.DBCodeParent MBDBCodeParent
						, mb.DBBUCode MBDBBUCode
						, mb.DBBUPattern MBDBBUPattern
						, mb.DBLevel MBDBLevel
						, mb.DBName MBDBName
						, mb.AreaCode MBAreaCode
						, mam.AreaCode MAMAreaCode
						, mam.AreaCodeParent MAMAreaCodeParent
						, mam.AreaBUCode MAMAreaBUCode
						, mam.AreaBUPattern MAMAreaBUPattern
						, mam.AreaLevel MAMAreaLevel
						, mam.AreaDesc MAMAreaDesc
					from Sys_User su --//[mylock]
						left join Mst_Distributor mb --//[mylock]
							on su.DBCode = mb.DBCode
						left join Mst_AreaMarket mam --//[mylock]
							on su.AreaCode = mam.AreaCode
					where
						su.UserCode = @strUserCode
					;
				"
				);
			DataTable dt = _cf.db.ExecQuery(
				strSql // strSqlQuery
				, "@strUserCode", strUserCode // arrParams couple items
				).Tables[0];
			if (dt.Rows.Count < 1)
			{
				throw new Exception(TError.ErrDemoLab.Sys_User_BizInvalidUserAbility);
			}

			// Return Good:
			return dt.Rows[0];
		}
		private void myCache_Mst_AreaMarket_ViewAbility_Get(
			DataRow drAbilityOfUser
			, string zzB_tbl_Mst_AreaMarket_ViewAbility_zzE = "#tbl_Mst_AreaMarket_ViewAbility"
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
						select 
							Cast(null as nvarchar(400)) AreaCode
						into zzB_tbl_Mst_AreaMarket_ViewAbility_zzE
						where (0=1)
						;
				"
				, "zzB_tbl_Mst_AreaMarket_ViewAbility_zzE", zzB_tbl_Mst_AreaMarket_ViewAbility_zzE
				);
			_cf.db.ExecQuery(strSql);

			// Cases:
			if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["FlagSysAdmin"], TConst.Flag.Active))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						insert into zzB_tbl_Mst_AreaMarket_ViewAbility_zzE(AreaCode)
						select distinct
							mam.AreaCode 
						from Mst_AreaMarket mam --//[mylock]
						where (1=1)
						;
					"
					, "zzB_tbl_Mst_AreaMarket_ViewAbility_zzE", zzB_tbl_Mst_AreaMarket_ViewAbility_zzE
					);
				_cf.db.ExecQuery(
					strSql
					);
			}
			else if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						select 
							mam.AreaBUPattern
						into #tbl_Sys_User_AreaBUPattern
						from Sys_User su --//[mylock]
							left join Mst_AreaMarket mam --//[mylock]
								on su.AreaCode = mam.AreaCode
						where (1=1)
							and su.UserCode = @strUserCode
						;

						insert into zzB_tbl_Mst_AreaMarket_ViewAbility_zzE(AreaCode)
						select distinct
							mam.AreaCode 
						from Mst_AreaMarket mam --//[mylock]
							left join #tbl_Sys_User_AreaBUPattern f --//[mylock]
								on (1=1)
						where (1=1)
							and (mam.AreaBUCode like f.AreaBUPattern)
						;
					"
					, "zzB_tbl_Mst_AreaMarket_ViewAbility_zzE", zzB_tbl_Mst_AreaMarket_ViewAbility_zzE
					);
				_cf.db.ExecQuery(
					strSql
					, "@strUserCode", drAbilityOfUser["UserCode"]
					);
			}
		}
		private DataRow myCache_Mst_AreaMarket_ViewAbility_CheckAccessAreaMarket(
			ref ArrayList alParamsCoupleError
			, DataRow drAbilityOfUser
			, object strAreaCode
			, string zzB_tbl_Mst_AreaMarket_ViewAbility_zzE = "#tbl_Mst_AreaMarket_ViewAbility"
			)
		{ 
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
					select 
						*
					from zzB_tbl_Mst_AreaMarket_ViewAbility_zzE t --//[mylock]
					where 
						t.AreaCode = @strAreaCode
					;
				"
				, "zzB_tbl_Mst_AreaMarket_ViewAbility_zzE", zzB_tbl_Mst_AreaMarket_ViewAbility_zzE
				);
			DataTable dt = _cf.db.ExecQuery(
				strSql // strSqlQuery
				, "@strAreaCode", strAreaCode // arrParams couple items
				).Tables[0];
			if (dt.Rows.Count < 1)
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CurrUser.AreaCode", drAbilityOfUser["MAMAreaCode"]
					, "Check.CurrUser.AreaBUCode", drAbilityOfUser["MAMAreaBUCode"]
					, "Check.CurrUser.AreaBUPattern", drAbilityOfUser["MAMAreaBUPattern"]
					, "Check.strAreaCode", strAreaCode
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Sys_ViewAbility_Deny
					, null
					, alParamsCoupleError.ToArray()
					);
			}

			// Return Good:
			return dt.Rows[0];
		}
		private void myCache_Mst_Distributor_ViewAbility_Get(
			DataRow drAbilityOfUser
			, string zzB_tbl_Mst_Distributor_ViewAbility_zzE = "#tbl_Mst_Distributor_ViewAbility"
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
						select 
							Cast(null as nvarchar(400)) DBCode
						into zzB_tbl_Mst_Distributor_ViewAbility_zzE
						where (0=1)
						;
				"
				, "zzB_tbl_Mst_Distributor_ViewAbility_zzE", zzB_tbl_Mst_Distributor_ViewAbility_zzE
				);
			_cf.db.ExecQuery(strSql);

			// Cases:
			if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["FlagSysAdmin"], TConst.Flag.Active))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						insert into zzB_tbl_Mst_Distributor_ViewAbility_zzE(DBCode)
						select distinct
							md.DBCode
						from Mst_Distributor md --//[mylock]
						where (1=1)
						;
					"
					, "zzB_tbl_Mst_Distributor_ViewAbility_zzE", zzB_tbl_Mst_Distributor_ViewAbility_zzE
					);
				_cf.db.ExecQuery(
					strSql
					);
			}
			else if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						insert into zzB_tbl_Mst_Distributor_ViewAbility_zzE(DBCode)
						select distinct
							md.DBCode
						from Mst_AreaMarket mam --//[mylock]
							inner join Mst_Distributor md --//[mylock]
								on mam.AreaCode = md.AreaCode
						where (1=1)
							and (mam.AreaBUCode like @p_va_Distributor_AreaBUPattern)
						;
					"
					, "zzB_tbl_Mst_Distributor_ViewAbility_zzE", zzB_tbl_Mst_Distributor_ViewAbility_zzE
					);
				_cf.db.ExecQuery(
					strSql
					, "@p_va_Distributor_AreaBUPattern", drAbilityOfUser["MAMAreaBUPattern"]
					);
			}
			else if (!CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						insert into zzB_tbl_Mst_Distributor_ViewAbility_zzE(DBCode)
						select distinct
							md.DBCode
						from Mst_Distributor md --//[mylock]
						where (1=1)
							and (md.DBBUCode = @p_va_Distributor_BUCode)
						;
					"
					, "zzB_tbl_Mst_Distributor_ViewAbility_zzE", zzB_tbl_Mst_Distributor_ViewAbility_zzE
					);
				_cf.db.ExecQuery(
					strSql
					, "@p_va_Distributor_BUCode", drAbilityOfUser["MBDBBUCode"]
					);
			}

			// Return Good:
			//return strSql;
		}
		private DataRow myCache_ViewAbility_CheckAccessDistributor(
			ref ArrayList alParamsCoupleError
			, DataRow drAbilityOfUser
			, object strDBCode
			, string zzB_tbl_Mst_Distributor_ViewAbility_zzE = "#tbl_Mst_Distributor_ViewAbility"
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
					select
						*
					from zzB_tbl_Mst_Distributor_ViewAbility_zzE t --//[mylock]
					where
						t.DBCode = @strDBCode
					;
				"
				, "zzB_tbl_Mst_Distributor_ViewAbility_zzE", zzB_tbl_Mst_Distributor_ViewAbility_zzE
				);
			DataTable dt = _cf.db.ExecQuery(
				strSql // strSqlQuery
				, "@strDBCode", strDBCode // arrParams couple items
				).Tables[0];
			if (dt.Rows.Count < 1)
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CurrUser.DBCode", drAbilityOfUser["DBCode"]
					, "Check.CurrUser.MBDBBUCode", drAbilityOfUser["MBDBBUCode"]
					, "Check.CurrUser.DBBUPattern", drAbilityOfUser["MBDBBUPattern"]
					, "Check.CurrUser.AreaCode", drAbilityOfUser["MAMAreaCode"]
					, "Check.CurrUser.AreaBUCode", drAbilityOfUser["MAMAreaBUCode"]
					, "Check.CurrUser.AreaBUPattern", drAbilityOfUser["MAMAreaBUPattern"]
					, "Check.strDBCode", strDBCode
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Sys_ViewAbility_Deny
					, null
					, alParamsCoupleError.ToArray()
					);
			}

			// Return Good:
			return dt.Rows[0];
		}
		private void myCache_ViewAbility_CheckExactUser(
			ref ArrayList alParamsCoupleError
			, DataRow drAbilityOfUser
			, object objUserCodeExact
			)
		{
			if (!CmUtils.StringUtils.StringEqualIgnoreCase(objUserCodeExact, _cf.sinf.strUserCode))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CurrUser.UserCode", _cf.sinf.strUserCode
					, "Check.objUserCodeExact", objUserCodeExact
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Sys_ViewAbility_NotExactUser
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		private void myCache_ViewAbility_GetOutletInfo(
			DataRow drAbilityOfUser
			, string zzB_tbl_Mst_Outlet_ViewAbility_zzE = "#tbl_Mst_Outlet_ViewAbility"
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
						select 
							Cast(null as nvarchar(400)) OLCode
						into zzB_tbl_Mst_Outlet_ViewAbility_zzE
						where (0=1)
						;
				"
				, "zzB_tbl_Mst_Outlet_ViewAbility_zzE", zzB_tbl_Mst_Outlet_ViewAbility_zzE
				);
			_cf.db.ExecQuery(strSql);

			// Cases:
			if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["FlagSysAdmin"], TConst.Flag.Active))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						insert into zzB_tbl_Mst_Outlet_ViewAbility_zzE(OLCode)
						select distinct
							mo.OLCode
						from Mst_Outlet mo --//[mylock]
						where (1=1)
						;
					"
					, "zzB_tbl_Mst_Outlet_ViewAbility_zzE", zzB_tbl_Mst_Outlet_ViewAbility_zzE
					);
				_cf.db.ExecQuery(
					strSql
					);
			}
			else if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						insert into zzB_tbl_Mst_Outlet_ViewAbility_zzE(OLCode)
						select distinct
							mo.OLCode
						from Mst_AreaMarket mam --//[mylock]
							inner join Mst_Distributor md --//[mylock]
								on mam.AreaCode = md.AreaCode
							inner join Mst_Outlet mo --//[mylock]
								on md.DBCode = mo.DBCode
						where (1=1)
							and (mam.AreaBUCode like @p_va_Outlet_AreaBUPattern)
						;
					"
					, "zzB_tbl_Mst_Outlet_ViewAbility_zzE", zzB_tbl_Mst_Outlet_ViewAbility_zzE
					);
				_cf.db.ExecQuery(
					strSql
					, "@p_va_Outlet_AreaBUPattern", drAbilityOfUser["MAMAreaBUPattern"]
				);
			}
			else if (!CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						insert into zzB_tbl_Mst_Outlet_ViewAbility_zzE(OLCode)
						select distinct
							mo.OLCode
						from Mst_Outlet mo --//[mylock]
							left join Mst_Distributor md --//[mylock]
								on mo.DBCode = md.DBCode
						where (1=1)
							and (md.DBBUCode = @p_va_Dealer_BUCode)
						;
					"
					, "zzB_tbl_Mst_Outlet_ViewAbility_zzE", zzB_tbl_Mst_Outlet_ViewAbility_zzE
					);
				_cf.db.ExecQuery(
					strSql
					, "@p_va_Dealer_BUCode", drAbilityOfUser["MBDBBUCode"]
					);
			}
		}
		private DataRow myCache_ViewAbility_CheckAccessOutlet(
			ref ArrayList alParamsCoupleError
			, DataRow drAbilityOfUser
			, object strOLCode
			, string zzB_tbl_Mst_Outlet_ViewAbility_zzE = "#tbl_Mst_Outlet_ViewAbility"
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
				select 
					*
				from zzB_tbl_Mst_Outlet_ViewAbility_zzE
				where
					t.OLCode = @strOLCode
				;
				"
				, "zzB_tbl_Mst_Outlet_ViewAbility_zzE", zzB_tbl_Mst_Outlet_ViewAbility_zzE
				);
			DataTable dt = _cf.db.ExecQuery(
				strSql // strSqlQuery
				, "@strOLCode", strOLCode // arrParams couple items
				).Tables[0];
			if (dt.Rows.Count < 1)
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.CurrUser.DBCode", drAbilityOfUser["DBCode"]
					, "Check.CurrUser.MBDBBUCode", drAbilityOfUser["MBDBBUCode"]
					, "Check.CurrUser.DBBUPattern", drAbilityOfUser["MBDBBUPattern"]
					, "Check.CurrUser.AreaCode", drAbilityOfUser["MAMAreaCode"]
					, "Check.CurrUser.AreaBUCode", drAbilityOfUser["MAMAreaBUCode"]
					, "Check.CurrUser.AreaBUPattern", drAbilityOfUser["MAMAreaBUPattern"]
					, "Check.strOLCode", strOLCode
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Sys_ViewAbility_Deny
					, null
					, alParamsCoupleError.ToArray()
					);
			}
			// Return Good:
			return dt.Rows[0];
		}
		private string zzzzClauseSelect_Mst_Outlet_ViewAbility_Get(
			DataRow drAbilityOfUser
			, ref ArrayList alParamsCoupleSql
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
						select distinct
							mo.OLCode
						into #tbl_Mst_Outlet_ViewAbility
						from Mst_Outlet mo --//[mylock]
						where (0=1)
						;
				");

			// Cases:
			if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["FlagSysAdmin"], TConst.Flag.Active))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						select distinct
							mo.OLCode
						into #tbl_Mst_Outlet_ViewAbility
						from Mst_Outlet mo --//[mylock]
						where (1=1)
						;
					");
			}
			else if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						select distinct
							mo.OLCode
						into #tbl_Mst_Outlet_ViewAbility
						from Mst_AreaMarket mam --//[mylock]
							inner join Mst_Distributor md --//[mylock]
								on mam.AreaCode = md.AreaCode
							inner join Mst_Outlet mo --//[mylock]
								on md.DBCode = mo.DBCode
						where (1=1)
							and (mam.AreaBUCode like @p_va_Outlet_AreaBUPattern)
						;
					");
				alParamsCoupleSql.AddRange(new object[]{
					"@p_va_Outlet_AreaBUPattern", drAbilityOfUser["MAMAreaBUPattern"]
					});
			}
			else if (!CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot)
				)
			{
				strSql = CmUtils.StringUtils.Replace(@"
						select distinct
							mo.OLCode
						into #tbl_Mst_Outlet_ViewAbility
						from Mst_Outlet mo --//[mylock]
						where (1=1)
							and (mo.DBCode = @p_va_Outlet_DBCode)
						;
					");
				alParamsCoupleSql.AddRange(new object[]{
					"@p_va_Outlet_DBCode", drAbilityOfUser["DBCode"]
					});
			}

			// Return Good:
			return strSql;
		}
		private string zzzzClauseSelect_Mst_Outlet_ByRouting_ViewAbility_Get(
			DataRow drAbilityOfUser
			, ref ArrayList alParamsCoupleSql
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
						select distinct
							mo.OLCode
						into #tbl_Mst_Outlet_ViewAbility
						from Mst_Outlet mo --//[mylock]
						where (0=1)
						;
				");

			// Cases:
			if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["FlagSysAdmin"], TConst.Flag.Active))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						select distinct
							mo.OLCode
						into #tbl_Mst_Outlet_ViewAbility
						from Mst_Outlet mo --//[mylock]
						where (1=1)
						;
					");
			}
			else if (CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot))
			{
				strSql = CmUtils.StringUtils.Replace(@"
						select distinct
							mo.OLCode
						into #tbl_Mst_Distributor_ViewAbility
						from Mst_AreaMarket mam --//[mylock]
							inner join Mst_Distributor md --//[mylock]
								on mam.AreaCode = md.AreaCode
							inner join Mst_Outlet mo --//[mylock]
								on md.DBCode = mo.DBCode
						where (1=1)
							and (mam.AreaBUCode like @p_va_Outlet_AreaBUPattern)
						;
					");
				alParamsCoupleSql.AddRange(new object[]{
					"@p_va_Outlet_AreaBUPattern", drAbilityOfUser["MAMAreaBUPattern"]
					});
			}
			else if (!CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot)
						&& CmUtils.StringUtils.StringEqual(drAbilityOfUser["FlagDBAdmin"], TConst.Flag.Yes)
				)
			{
				strSql = CmUtils.StringUtils.Replace(@"
						select distinct
							mo.OLCode
						into #tbl_Mst_Outlet_ViewAbility
						from Mst_Outlet mo --//[mylock]
						where (1=1)
							and (mo.DBCode = @p_va_Outlet_DBCode)
						;
					");
				alParamsCoupleSql.AddRange(new object[]{
					"@p_va_Outlet_DBCode", drAbilityOfUser["DBCode"]
					});
			}
			else if (!CmUtils.StringUtils.StringEqual(drAbilityOfUser["DBCode"], TConst.BizMix.DBCodeRoot)
						&& !CmUtils.StringUtils.StringEqual(drAbilityOfUser["FlagDBAdmin"], TConst.Flag.Yes)
				)
			{
				strSql = CmUtils.StringUtils.Replace(@"
						select distinct
							mo.OLCode
						into #tbl_Mst_Outlet_ViewAbility
						from Mst_Outlet mo --//[mylock]
							inner join DB_Routing dbr --//[mylock]
								on mo.DBCode = dbr.DBCode
							inner join DB_RoutingDetail dbrd --//[mylock]
								on dbr.RTCode = dbrd.RTCode
						where (1=1)
							and (mo.DBCode = @p_va_Outlet_DBCode)
							and (mo.OLStatus = '1')
							and (dbr.UserCodeSM = @p_va_Outlet_UserCode)
							and (dbr.RTStatus = '1')
						;
					");
				alParamsCoupleSql.AddRange(new object[]{
					"@p_va_Outlet_DBCode", drAbilityOfUser["DBCode"]
					, "@p_va_Outlet_UserCode", drAbilityOfUser["UserCode"]
					});
			}

			// Return Good:
			return strSql;
		}
		private string zzzzClauseSelect_DB_LimitPrdScope_Latest_Get10(
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_DB_LimitPrdScope_Filter_Draft01:
							select distinct
								dblps.LPCode
								, dblps.DBCode
								, dblp.ProductCode
								, dblp.CreateDTime
							into #tbl_DB_LimitPrdScope_Filter_Draft01
							from #tbl_Mst_Distributor_ViewAbility va_md --//[mylock]
								inner join DB_LimitPrdScope dblps --//[mylock]
									on va_md.DBCode  = dblps.DBCode -- Filter Ability
								inner join DB_LimitPrd dblp --//[mylock]
									on dblp.LPCode  = dblps.LPCode
							where (1=1)
								and (dblp.EffDateStart <= @strRefDate and @strRefDate <= dblp.EffDateEnd)
							;
						
							---- #tbl_DB_LimitPrdScope_Filter_Draft02:
							select
								t.DBCode
								, t.ProductCode
								, Max(t.CreateDTime) CreateDTime
							into #tbl_DB_LimitPrdScope_Filter_Draft02
							from #tbl_DB_LimitPrdScope_Filter_Draft01 t --//[mylock]
							group by
								t.DBCode
								, t.ProductCode
							;

							---- #tbl_DB_LimitPrdScope_Filter_Draft:
							select
								t.LPCode
								, t.DBCode
								, t.ProductCode
								, t.CreateDTime
							into #tbl_DB_LimitPrdScope_Filter_Draft
							from #tbl_DB_LimitPrdScope_Filter_Draft01 t --//[mylock]
								inner join #tbl_DB_LimitPrdScope_Filter_Draft02 t2 --//[mylock]
									on t.DBCode = t2.DBCode and t.CreateDTime = t2.CreateDTime
							;
				");

			// Return Good:
			return strSql;
		}
		private string zzzzClauseSelect_DB_LimitPrdScope_Latest_Get20_GetOODBDFilter(
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
							---- #tbl_OODBD_Filter:
							select distinct
								oodbd.ProductCode
								, oodb.DBCode
							into #tbl_OODBD_Filter
							from Ord_OrderDBDetail oodbd --//[mylock]
								inner join Ord_OrderDB oodb --//[mylock]
									on oodbd.OrderDBNo = oodb.OrderDBNo
							where (1=1)
								and (oodbd.OrderDBNo = @strOrderDBNo)
							;
				");

			// Return Good:
			return strSql;
		}
		private string zzzzClauseSelect_DB_LimitPrdScope_Latest_Get30_GetOverItems(
			)
		{
			// Init:
			string strSql = CmUtils.StringUtils.Replace(@"
							---- Get OverItems:
							select
								s.*
								, pp.ProductDesc pp_ProductDesc
								, f.CreateDTime dblps_CreateDTime
								, dblps.LPCode dblps_LPCode
								, dblps.DBCode dblps_DBCode
								, dblps.QtyLimit dblps_QtyLimit
							from #tbl_OODBD_Sum s --//[mylock]
								inner join #tbl_DB_LimitPrdScope_Filter_Draft f --//[mylock]
									on s.ProductCode = f.ProductCode and s.DBCode = f.DBCode
								inner join DB_LimitPrdScope dblps --//[mylock]
									on f.LPCode = dblps.LPCode and f.DBCode = dblps.DBCode
								inner join Prd_Product pp --//[mylock]
									on s.ProductCode = pp.ProductCode
							where (1=1)
								and s.SumQty > dblps.QtyLimit
							;
				");

			// Return Good:
			return strSql;
		}

		#endregion

		#region // Sys_Access:
		private void Sys_Access_CheckDB(
			ref ArrayList alParamsCoupleError
			, string strFunctionName
			)
		{

		}
		private void Sys_Access_CheckDeny(
			ref ArrayList alParamsCoupleError
			, object strObjectCode
			)
		{
			#region // Build Sql:
			ArrayList alParamsCoupleSql = new ArrayList();
			alParamsCoupleSql.AddRange(new object[] { 
					"@strUserCode", _cf.sinf.strUserCode
					, "@strObjectCode", strObjectCode
					});
			string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- Sys_Access:
						select distinct
							so.ObjectCode
						from Sys_User su --//[mylock]
							inner join Sys_UserInGroup suig --//[mylock]
								on su.UserCode = suig.UserCode
							inner join Sys_Group sg --//[mylock]
								on suig.GroupCode = sg.GroupCode and sg.FlagActive = '1'
							inner join Sys_Access sa --//[mylock]
								on sg.GroupCode = sa.GroupCode
							inner join Sys_Object so --//[mylock]
								on sa.ObjectCode = so.ObjectCode and so.FlagActive = '1'
						where (1=1)
							and su.UserCode = @strUserCode
							and su.FlagActive = '1'
							and so.ObjectCode = @strObjectCode
							and so.FlagActive = '1'
						union -- distinct
						select distinct
							so.ObjectCode
						from Sys_User su --//[mylock]
							inner join Sys_Object so --//[mylock]
								on (1=1)
									and su.FlagSysAdmin = '1' 
									and su.UserCode = @strUserCode
									and su.FlagActive = '1'
									and so.ObjectCode = @strObjectCode
									and so.FlagActive = '1'
						;
					"
				);
			#endregion

			#region // Get Data and Check:
			DataTable dtGetData = _cf.db.ExecQuery(
				strSqlGetData
				, alParamsCoupleSql.ToArray()
				).Tables[0];
			if (dtGetData.Rows.Count < 1)
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.strObjectCode", strObjectCode
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Sys_Access_CheckDeny
					, null
					, alParamsCoupleError.ToArray()
					);
			}
			#endregion

		}

		public DataSet Sys_Access_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_Access
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_Access_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_Access_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Sys_Access", strRt_Cols_Sys_Access
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
				bool bGet_Sys_Access = (strRt_Cols_Sys_Access != null && strRt_Cols_Sys_Access.Length > 0);

				// drAbilityOfAccess:
				//DataRow drAbilityOfAccess = mySys_Access_GetAbilityViewBankOfAccess(_cf.sinf.strAccessCode);

				#endregion

				#region // Build Sql:
				ArrayList alParamsCoupleSql = new ArrayList();
				//alParamsCoupleSql.AddRange(new object[] { "@strAbilityOfAccess", drAbilityOfAccess["MBBankBUPattern"] });
				alParamsCoupleSql.AddRange(new object[] { 
					"@nFilterRecordStart", nFilterRecordStart
					, "@nFilterRecordEnd", nFilterRecordEnd
					, "@Today", DateTime.Today.ToString("yyyy-MM-dd")
					});
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Sys_Access_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, sa.GroupCode
                            , sa.ObjectCode
						into #tbl_Sys_Access_Filter_Draft
						from Sys_Access sa --//[mylock]
						where (1=1)
							zzzzClauseWhere_FilterAbilityOfUser -- Filter the AbilityOfAccess
							zzzzClauseWhere_strFilterWhereClause
						order by sa.GroupCode asc
                                , sa.ObjectCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Sys_Access_Filter_Draft t --//[mylock]
						;

						---- #tbl_Sys_Access_Filter:
						select
							t.*
						into #tbl_Sys_Access_Filter
						from #tbl_Sys_Access_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Sys_Access --------:
						zzzzClauseSelect_Sys_Access_zOut
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Sys_Access_Filter_Draft;
						--drop table #tbl_Sys_Access_Filter;
					"
					, "zzzzClauseWhere_FilterAbilityOfUser", ""
					);
				////
				string zzzzClauseSelect_Sys_Access_zOut = "-- Nothing.";
				if (bGet_Sys_Access)
				{
					#region // bGet_Sys_Access:
					zzzzClauseSelect_Sys_Access_zOut = CmUtils.StringUtils.Replace(@"
							---- Sys_Access:
							select
								t.MyIdxSeq
								, sa.*
								, so.ObjectCode so_ObjectCode
                                , so.ObjectName so_ObjectName 
                                , so.ServiceCode so_ServiceCode 
                                , so.ObjectType so_ObjectType 
                                , so.FlagExecModal so_FlagExecModal 
                                , so.FlagActive so_FlagActive 
							from #tbl_Sys_Access_Filter t --//[mylock]
								inner join Sys_Access sa --//[mylock]
									on t.GroupCode = sa.GroupCode
                                        and t.ObjectCode = sa.ObjectCode
                                left join Sys_Object so --//[mylock]
                                    on t.ObjectCode = so.ObjectCode
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
							, "Sys_Access" // strTableNameDB
							, "Sys_Access." // strPrefixStd
							, "sa." // strPrefixAlias
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
					, "zzzzClauseSelect_Sys_Access_zOut", zzzzClauseSelect_Sys_Access_zOut
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Sys_Access)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Sys_Access";
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

		public DataSet Sys_Access_Save(
			string strTid
			, DataRow drSession
			////
			, object objGroupCode
			, object[] arrobjDSData
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_Access_Save";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_Access_Save;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objGroupCode", objGroupCode
					});
			#endregion

			try
			{
				#region // Convert Input:
				DateTime dtimeTDate = DateTime.Now;
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

				#region // Refine and Check Input Master:
				////
				string strGroupCode = TUtils.CUtils.StdParam(objGroupCode);
				////
				DataTable dtDB_Sys_Group = null;
				////
				{
					////
					Sys_Group_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objGroupCode // objGroupCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Sys_Group // dtDB_Sys_Group
						);
					////
					dtDB_Sys_Group.Rows[0]["LogLUDTime"] = dtimeTDate.ToString("yyyy-MM-dd HH:mm:ss");
					dtDB_Sys_Group.Rows[0]["LogLUBy"] = _cf.sinf.strUserCode;
					//// Upload:
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db // db
						, "#tbl_Sys_Group" // strTableName
						, TConst.BizMix.Default_DBColType // strDefaultType
						, new object[] { "GroupCode", "LogLUDTime", "LogLUBy" } // arrSingleStructure
						, dtDB_Sys_Group // dtData
						);
					////
				}
				#endregion

				#region // Refine and Check Input Detail:
				////
				DataTable dtInput_Sys_Access = null;
				////
				{
					////
					string strTableCheck = "Sys_Access";
					////
					if (!dsData.Tables.Contains(strTableCheck))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.TableName", strTableCheck
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_Access_Save_InputTblDtlNotFound
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					dtInput_Sys_Access = dsData.Tables[strTableCheck];
					TUtils.CUtils.StdDataInTable(
						dtInput_Sys_Access // dtData
						, "StdParam", "ObjectCode" // arrstrCouple
						);
					//// Upload:
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db // db
						, "#tbl_Sys_Access" // strTableName
						, TConst.BizMix.Default_DBColType // strDefaultType
						, new object[] { "ObjectCode" } // arrSingleStructure
						, dtInput_Sys_Access // dtData
						);
					////
				}
				#endregion

				#region // SaveDB Sys_Access:
				{
					string strSql_Exec = CmUtils.StringUtils.Replace(@"
						---- Clear All:
						delete t
						from Sys_Access t --//[mylock]
							inner join #tbl_Sys_Group t_sg --//[mylock]
								on t.GroupCode = t_sg.GroupCode
						where (1=1)
						;

						---- Insert All:
						insert into Sys_Access(
							GroupCode
							, ObjectCode
							, LogLUDTime
							, LogLUBy
							)
						select
							t_sg.GroupCode
							, t_sa.ObjectCode
							, t_sg.LogLUDTime
							, t_sg.LogLUBy
						from #tbl_Sys_Group t_sg --//[mylock]
							inner join #tbl_Sys_Access t_sa --//[mylock]
								on (1=1)
						;
					");
					DataSet dsDB_Check = _cf.db.ExecQuery(
						strSql_Exec
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

		#region // Sys_Group:
		private void Sys_Group_CheckDB(
			ref ArrayList alParamsCoupleError
			, object strGroupCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dtDB_Sys_Group
			)
		{
			// GetInfo:
			dtDB_Sys_Group = TDALUtils.DBUtils.GetTableContents(
				_cf.db // db
				, "Sys_Group" // strTableName
				, "top 1 *" // strColumnList
				, "" // strClauseOrderBy
				, "GroupCode", "=", strGroupCode // arrobjParamsTriple item
				);

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dtDB_Sys_Group.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.GroupCodeNotFound", strGroupCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Sys_Group_CheckDB_GroupCodeNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dtDB_Sys_Group.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.GroupCodeExist", strGroupCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Sys_Group_CheckDB_GroupCodeExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck:
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dtDB_Sys_Group.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.GroupCode", strGroupCode
					, "Check.FlagActiveListToCheck", strFlagActiveListToCheck
					, "DB.FlagActive", dtDB_Sys_Group.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Sys_Group_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}

		}

		public DataSet Sys_Group_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_Group
			, string strRt_Cols_Sys_UserInGroup
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_Group_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_Group_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Sys_Group", strRt_Cols_Sys_Group
					, "strRt_Cols_Sys_UserInGroup", strRt_Cols_Sys_UserInGroup
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
				bool bGet_Sys_Group = (strRt_Cols_Sys_Group != null && strRt_Cols_Sys_Group.Length > 0);
				bool bGet_Sys_UserInGroup = (strRt_Cols_Sys_UserInGroup != null && strRt_Cols_Sys_UserInGroup.Length > 0);

				// drAbilityOfGroup:
				//DataRow drAbilityOfGroup = mySys_Group_GetAbilityViewBankOfGroup(_cf.sinf.strGroupCode);

				#endregion

				#region // Build Sql:
				ArrayList alParamsCoupleSql = new ArrayList();
				//alParamsCoupleSql.AddRange(new object[] { "@strAbilityOfGroup", drAbilityOfGroup["MBBankBUPattern"] });
				alParamsCoupleSql.AddRange(new object[] { 
					"@nFilterRecordStart", nFilterRecordStart
					, "@nFilterRecordEnd", nFilterRecordEnd
					, "@Today", DateTime.Today.ToString("yyyy-MM-dd")
					});
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Sys_Group_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, sg.GroupCode
						into #tbl_Sys_Group_Filter_Draft
						from Sys_Group sg --//[mylock]
							left join Sys_UserInGroup suig --//[mylock]
								on sg.GroupCode = suig.GroupCode
							left join Sys_User su --//[mylock]
								on suig.UserCode = su.UserCode
						where (1=1)
							zzzzClauseWhere_FilterAbilityOfUser -- Filter the AbilityOfGroup
							zzzzClauseWhere_strFilterWhereClause
						order by sg.GroupCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Sys_Group_Filter_Draft t --//[mylock]
						;

						---- #tbl_Sys_Group_Filter:
						select
							t.*
						into #tbl_Sys_Group_Filter
						from #tbl_Sys_Group_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Sys_Group --------:
						zzzzClauseSelect_Sys_Group_zOut
						----------------------------------------

						-------- Sys_UserInGroup --------:
						zzzzClauseSelect_Sys_UserInGroup_zOut
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Sys_Group_Filter_Draft;
						--drop table #tbl_Sys_Group_Filter;
					"
					, "zzzzClauseWhere_FilterAbilityOfUser", ""
					);
				////
				string zzzzClauseSelect_Sys_Group_zOut = "-- Nothing.";
				if (bGet_Sys_Group)
				{
					#region // bGet_Sys_Group:
					zzzzClauseSelect_Sys_Group_zOut = CmUtils.StringUtils.Replace(@"
							---- Sys_Group:
							select
								t.MyIdxSeq
								, sg.*
							from #tbl_Sys_Group_Filter t --//[mylock]
								inner join Sys_Group sg --//[mylock]
									on t.GroupCode = sg.GroupCode
							order by t.MyIdxSeq asc
							;
						"
						);
					#endregion
				}
				////
				string zzzzClauseSelect_Sys_UserInGroup_zOut = "-- Nothing.";
				if (bGet_Sys_UserInGroup)
				{
					#region // bGet_Sys_UserInGroup:
					zzzzClauseSelect_Sys_UserInGroup_zOut = CmUtils.StringUtils.Replace(@"
							---- Sys_UserInGroup:
							select
								t.MyIdxSeq
								, suig.*
								, su.UserCode su_UserCode
								, su.UserName su_UserName 
								, su.FlagSysAdmin su_FlagSysAdmin 
                                , su.FlagDBAdmin su_FlagDBAdmin 
								, su.FlagActive su_FlagActive 
								, sg.GroupCode sg_GroupCode
								, sg.GroupName sg_GroupName 
								, sg.FlagActive sg_FlagActive 
							from #tbl_Sys_Group_Filter t --//[mylock]
								inner join Sys_UserInGroup suig --//[mylock]
									on t.GroupCode = suig.GroupCode
								left join Sys_User su --//[mylock]
									on suig.UserCode = su.UserCode
								left join Sys_Group sg --//[mylock]
									on suig.GroupCode = sg.GroupCode
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
							, "Sys_Group" // strTableNameDB
							, "Sys_Group." // strPrefixStd
							, "sg." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Sys_UserInGroup" // strTableNameDB
							, "Sys_UserInGroup." // strPrefixStd
							, "suig." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Sys_User" // strTableNameDB
							, "Sys_User." // strPrefixStd
							, "su." // strPrefixAlias
							);
						htSpCols.Remove("Sys_User.UserPassword".ToUpper());
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
					, "zzzzClauseSelect_Sys_Group_zOut", zzzzClauseSelect_Sys_Group_zOut
					, "zzzzClauseSelect_Sys_UserInGroup_zOut", zzzzClauseSelect_Sys_UserInGroup_zOut
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Sys_Group)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Sys_Group";
				}
				if (bGet_Sys_UserInGroup)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Sys_UserInGroup";
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
		public DataSet Sys_Group_Create(
			string strTid
			, DataRow drSession
			////
			, object objGroupCode
			, object objGroupName
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_Group_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_Group_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objGroupCode", objGroupCode
					, "objGroupName", objGroupName
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
				string strGroupCode = TUtils.CUtils.StdParam(objGroupCode);
				string strGroupName = string.Format("{0}", objGroupName).Trim();
				////
				DataTable dtDB_Sys_Group = null;
				{
					////
					if (strGroupCode == null || strGroupCode.Length <= 0)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strGroupCode", strGroupCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_Group_Create_InvalidGroupCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Sys_Group_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objGroupCode // objGroupCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagPublicListToCheck
						, out dtDB_Sys_Group // dtDB_Sys_Group
						);
					////
					if (strGroupName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strGroupName", strGroupName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_Group_Create_InvalidGroupName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
				}
				#endregion

				#region // SaveDB GroupCode:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Sys_Group.NewRow();
					strFN = "GroupCode"; drDB[strFN] = strGroupCode;
					strFN = "GroupName"; drDB[strFN] = strGroupName;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Yes;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeTDate.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Sys_Group.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Sys_Group"
						, dtDB_Sys_Group
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
		public DataSet Sys_Group_Update(
			string strTid
			, DataRow drSession
			////
			, object objGroupCode
			, object objGroupName
			, object objFlagActive
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_Group_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_Group_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objGroupCode", objGroupCode
					, "objGroupName", objGroupName
					, "objFlagActive", objFlagActive
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
				string strGroupCode = TUtils.CUtils.StdParam(objGroupCode);
				string strGroupName = string.Format("{0}", objGroupName).Trim();
				string strFlagActive = (CmUtils.StringUtils.StringEqual(objFlagActive, TConst.StatusCm.No) ? TConst.StatusCm.No : TConst.StatusCm.Yes);
				////
				DataTable dtDB_Sys_Group = null;
				bool bUpd_GroupName = strFt_Cols_Upd.Contains("Sys_Group.GroupName".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Sys_Group.FlagActive".ToUpper());
				////
				{
					////
					Sys_Group_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objGroupCode // objGroupCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Sys_Group // dtDB_Sys_Group
						);
					////
					if (bUpd_GroupName && strGroupName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strGroupName", strGroupName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_Group_Update_InvalidGroupName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
				}
				#endregion

				#region // SaveDB Sys_Group:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Sys_Group.Rows[0];
					if (bUpd_GroupName) { strFN = "GroupName"; drDB[strFN] = strGroupName; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeTDate.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Sys_Group"
						, dtDB_Sys_Group
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
		public DataSet Sys_Group_Delete(
			string strTid
			, DataRow drSession
			////
			, object objGroupCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_Group_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_Group_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objGroupCode", objGroupCode
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
				string strGroupCode = TUtils.CUtils.StdParam(objGroupCode);
				////
				DataTable dtDB_Sys_Group = null;
				{
					////
					Sys_Group_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objGroupCode // objGroupCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Sys_Group // dtDB_Sys_Group
						);
					//// Delete Sys_GroupInGroup:
					Sys_UserInGroup_Delete_ByGroup(
						strGroupCode // strGroupCode
						);
					////
				}
				#endregion

				#region // SaveDB GroupCode:
				{
					// Init:
					dtDB_Sys_Group.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Sys_Group"
						, dtDB_Sys_Group
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

		#region // Sys_User:
		private void Sys_User_CheckDB(
			ref ArrayList alParamsCoupleError
			, object strUserCode
			, string strFlagExistToCheck
			, string strFlagActiveListToCheck
			, out DataTable dt_Sys_User
			)
		{
			// GetInfo:
			dt_Sys_User = TDALUtils.DBUtils.GetTableContents(
				_cf.db // db
				, "Sys_User" // strTableName
				, "top 1 *" // strColumnList
				, "" // strClauseOrderBy
				, "UserCode", "=", strUserCode // arrobjParamsTriple item
				);

			// strFlagExistToCheck:
			if (strFlagExistToCheck.Length > 0)
			{
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Active) && dt_Sys_User.Rows.Count < 1)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.UserCodeNotFound", strUserCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Sys_User_CheckDB_UserCodeNotFound
						, null
						, alParamsCoupleError.ToArray()
						);
				}
				if (CmUtils.StringUtils.StringEqual(strFlagExistToCheck, TConst.Flag.Inactive) && dt_Sys_User.Rows.Count > 0)
				{
					alParamsCoupleError.AddRange(new object[]{
						"Check.UserCodeExist", strUserCode
						});
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Sys_User_CheckDB_UserCodeExist
						, null
						, alParamsCoupleError.ToArray()
						);
				}
			}

			// strFlagActiveListToCheck:
			if (strFlagActiveListToCheck.Length > 0 && !strFlagActiveListToCheck.Contains(Convert.ToString(dt_Sys_User.Rows[0]["FlagActive"])))
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.UserCodeError", strUserCode
					, "Check.FlagActiveListToCheck", strFlagActiveListToCheck
					, "Check.FlagActiveCurrent", dt_Sys_User.Rows[0]["FlagActive"]
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.Sys_User_CheckDB_FlagActiveNotMatched
					, null
					, alParamsCoupleError.ToArray()
					);
			}

		}

		public DataSet Sys_User_Login(
			string strTid
			, string strRootSvCode
			, string strRootUserCode
			, string strServiceCode
			, string strUserCode
			, string strLanguageCode
			, string strUserPassword
			, string strOtherInfo
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = false;
			string strFunctionName = "Sys_User_Login";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_User_Login;
			strUserCode = TUtils.CUtils.StdParam(strUserCode);
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				, "strTid", strTid
				, "strRootSvCode", strRootSvCode
				, "strRootUserCode", strRootUserCode
				, "strServiceCode", strServiceCode
				, "strUserCode", strUserCode
				, "strLanguageCode", strLanguageCode
				, "strOtherInfo", strOtherInfo
				});

			// Manual SessionInfo:
			DataRow drSessionInfo = TSession.Core.CSession.s_myGetSchema_Lic_Session().NewRow();
			drSessionInfo["RootSvCode"] = strRootSvCode;
			drSessionInfo["RootUserCode"] = strRootUserCode;
			drSessionInfo["ServiceCode"] = strServiceCode;
			drSessionInfo["UserCode"] = strUserCode;
			drSessionInfo["LanguageCode"] = strLanguageCode;
			_cf.sinf = new CSessionInfo(drSessionInfo);
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

				#endregion

				#region // Process:
				// Sys_User_CheckDB:
				DataTable dt_Sys_User = null;
				Sys_User_CheckDB(
					ref alParamsCoupleError // alParamsCoupleError
					, strUserCode // strUserCode
					, TConst.Flag.Active // strFlagExistToCheck
					, TConst.Flag.Active // strFlagActiveListToCheck
					, out dt_Sys_User // dt_Sys_User
					);

				// CheckPassword:
				if (!CmUtils.StringUtils.StringEqual(strUserPassword, dt_Sys_User.Rows[0]["UserPassword"]))
				{
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Sys_User_Login_InvalidPassword // strErrorCode
						, null // excInner
						, alParamsCoupleError.ToArray() // arrobjParamsCouple
						);
				}

				// Assign:
				CmUtils.CMyDataSet.SetRemark(ref mdsFinal, dt_Sys_User.Rows[0]["UserCode"]);
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
		public DataSet Sys_User_Logout(
			string strTid
			, DataRow drSession
			////
			, object strSessionId
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = false;
			string strFunctionName = "Sys_User_Logout";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_User_Logout;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
				"strFunctionName", strFunctionName
				, "strTid", strTid
				////
                , "strSessionId", strSessionId
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

				#endregion

				#region // Logout:
				_cf.sess.Remove(false, strSessionId);
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
		public DataSet Sys_User_GetForCurrentUser(
			string strTid
			, DataRow drSession
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_User_GetForCurrentUser";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_User_GetForCurrentUser;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
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

				#endregion

				#region // Build Sql:
				ArrayList alParamsCoupleSql = new ArrayList();
				alParamsCoupleSql.AddRange(new object[] { 
					"@strUserCode", _cf.sinf.strUserCode
					});
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- Sys_User:
						select
							su.UserCode
							, su.DBCode
                            , su.AreaCode
							, su.UserName
							, su.FlagSysAdmin
							, su.FlagDBAdmin
							, su.FlagActive
							, md.DBCode md_DBCode 
							, md.DBCodeParent md_DBCodeParent 
							, md.DBName md_DBName 
							, md.DBLevel md_DBLevel 
							, md.DBStatus md_DBStatus 
                            , mam.AreaCode mam_AreaCode
	                        , mam.AreaCodeParent mam_AreaCodeParent
	                        , mam.AreaDesc mam_AreaDesc
	                        , mam.AreaLevel mam_AreaLevel
	                        , mam.AreaStatus mam_AreaStatus
						into #tbl_Sys_User
						from Sys_User su --//[mylock]
							left join Mst_Distributor md --//[mylock]
								on su.DBCode = md.DBCode
                            left join Mst_AreaMarket mam --//[mylock] 
		                        on md.AreaCode = mam.AreaCode
						where
							su.UserCode = @strUserCode
						;
						select * from #tbl_Sys_User t --//[mylock]
						;

						---- Sys_Access:
						select distinct
							sa.ObjectCode
						into #tbl_Sys_Access
						from Sys_User su --//[mylock]
							inner join Sys_UserInGroup suig --//[mylock]
								on su.UserCode = suig.UserCode
							inner join Sys_Group sg --//[mylock]
								on suig.GroupCode = sg.GroupCode and sg.FlagActive = '1'
							inner join Sys_Access sa --//[mylock]
								on sg.GroupCode = sa.GroupCode
							inner join Sys_Object so --//[mylock]
								on sa.ObjectCode = so.ObjectCode and so.FlagActive = '1'
						where (1=1)
							and su.UserCode = @strUserCode
							and su.FlagActive = '1'
						union -- distinct
						select distinct
							so.ObjectCode
						from #tbl_Sys_User f --//[mylock]
							inner join Sys_Object so --//[mylock]
								on f.FlagSysAdmin = '1' and f.FlagActive = '1' and so.FlagActive = '1'
						;
						select 
							so.*
						from #tbl_Sys_Access f --//[mylock]
							inner join Sys_Object so --//[mylock]
								on f.ObjectCode = so.ObjectCode
						;
					"
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				dsGetData.Tables[0].TableName = "Sys_User";
				dsGetData.Tables[1].TableName = "Sys_Access";
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
		public DataSet Sys_User_ChangePassword(
			string strTid
			, DataRow drSession
			, string strUserPasswordOld
			, string strUserPasswordNew
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_User_ChangePassword";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_User_ChangePassword;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
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

				// Sys_User_CheckDB:
				DataTable dt_Sys_User = null;
				Sys_User_CheckDB(
					ref alParamsCoupleError // alParamsCoupleError
					, _cf.sinf.strUserCode // strUserCode
					, TConst.Flag.Active // strFlagExistToCheck
					, TConst.Flag.Active // strFlagActiveListToCheck
					, out dt_Sys_User
					);

				// CheckPassword:
				if (!CmUtils.StringUtils.StringEqual(strUserPasswordOld, dt_Sys_User.Rows[0]["UserPassword"]))
				{
					throw CmUtils.CMyException.Raise(
						TError.ErrDemoLab.Sys_User_ChangePassword_InvalidPasswordOld // strErrorCode
						, null // excInner
						, alParamsCoupleError.ToArray() // arrobjParamsCouple
						);
				}

				#endregion

				#region // dt_Sys_User:
				ArrayList alColumnEffective = new ArrayList();
				dt_Sys_User.Rows[0]["UserPassword"] = strUserPasswordNew; alColumnEffective.Add("UserPassword");
				_cf.db.SaveData("Sys_User", dt_Sys_User, alColumnEffective.ToArray());
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

		public DataSet Sys_User_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_User
			, string strRt_Cols_Sys_UserInGroup
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_User_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_User_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Sys_User", strRt_Cols_Sys_User
					, "strRt_Cols_Sys_UserInGroup", strRt_Cols_Sys_UserInGroup
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
				bool bGet_Sys_User = (strRt_Cols_Sys_User != null && strRt_Cols_Sys_User.Length > 0);
				bool bGet_Sys_UserInGroup = (strRt_Cols_Sys_UserInGroup != null && strRt_Cols_Sys_UserInGroup.Length > 0);

				//// drAbilityOfUser:
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
				////
				myCache_Mst_Distributor_ViewAbility_Get(drAbilityOfUser);

				myCache_Mst_AreaMarket_ViewAbility_Get(drAbilityOfUser);
				////
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Sys_User_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, su.UserCode
						into #tbl_Sys_User_Filter_Draft
						from Sys_User su --//[mylock]
							left join Sys_UserInGroup suig --//[mylock]
								on su.UserCode = suig.UserCode
							left join Sys_Group sg --//[mylock]
								on suig.GroupCode = sg.GroupCode
                            left join Mst_Distributor md --//[mylock]
								on su.DBCode = md.DBCode
                            left join Mst_AreaMarket mam --//[mylock]
								on su.AreaCode = mam.AreaCode
							inner join #tbl_Mst_AreaMarket_ViewAbility va_mam --//[mylock]
								on mam.AreaCode = va_mam.AreaCode
						where (1=1)
							zzzzClauseWhere_FilterAbilityOfUser -- Filter the AbilityOfUser
							zzzzClauseWhere_strFilterWhereClause
						order by su.UserCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Sys_User_Filter_Draft t --//[mylock]
						;

						---- #tbl_Sys_User_Filter:
						select
							t.*
						into #tbl_Sys_User_Filter
						from #tbl_Sys_User_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Sys_User --------:
						zzzzClauseSelect_Sys_User_zOut
						----------------------------------------

						-------- Sys_UserInGroup --------:
						zzzzClauseSelect_Sys_UserInGroup_zOut
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Sys_User_Filter_Draft;
						--drop table #tbl_Sys_User_Filter;
					"
					, "zzzzClauseWhere_FilterAbilityOfUser", ""
					);
				////
				string zzzzClauseSelect_Sys_User_zOut = "-- Nothing.";
				if (bGet_Sys_User)
				{
					#region // bGet_Sys_User:
					zzzzClauseSelect_Sys_User_zOut = CmUtils.StringUtils.Replace(@"
							---- Sys_User:
							select
								t.MyIdxSeq
								, su.UserCode
                                , su.DBCode
                                , su.AreaCode
								, su.UserName
								, 'zzzzClausePVal_Default_PasswordMask' UserPassword
								, su.FlagSysAdmin
                                , su.FlagDBAdmin
								, su.FlagActive
								, md.DBCode md_DBCode
								, mam.AreaCode mam_AreaCode
							from #tbl_Sys_User_Filter t --//[mylock]
								inner join Sys_User su --//[mylock]
									on t.UserCode = su.UserCode
								left join Mst_Distributor md --//[mylock]
									on su.DBCode = md.DBCode
								left join Mst_AreaMarket mam --//[mylock]
									on su.AreaCode = mam.AreaCode
							order by t.MyIdxSeq asc
							;
						"
						, "zzzzClausePVal_Default_PasswordMask", TConst.BizMix.Default_PasswordMask
						);
					#endregion
				}
				////
				string zzzzClauseSelect_Sys_UserInGroup_zOut = "-- Nothing.";
				if (bGet_Sys_UserInGroup)
				{
					#region // bGet_Sys_UserInGroup:
					zzzzClauseSelect_Sys_UserInGroup_zOut = CmUtils.StringUtils.Replace(@"
							---- Sys_UserInGroup:
							select
								t.MyIdxSeq
								, suig.*
								, su.UserCode su_UserCode
								, su.UserName su_UserName 
								, su.FlagSysAdmin su_FlagSysAdmin 
								, su.FlagActive su_FlagActive 
								, sg.GroupCode sg_GroupCode
								, sg.GroupName sg_GroupName 
								, sg.FlagActive sg_FlagActive 
							from #tbl_Sys_User_Filter t --//[mylock]
								inner join Sys_UserInGroup suig --//[mylock]
									on t.UserCode = suig.UserCode
								left join Sys_User su --//[mylock]
									on suig.UserCode = su.UserCode
								left join Sys_Group sg --//[mylock]
									on suig.GroupCode = sg.GroupCode
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
							, "Sys_User" // strTableNameDB
							, "Sys_User." // strPrefixStd
							, "su." // strPrefixAlias
							);
						htSpCols.Remove("Sys_User.UserPassword".ToUpper());
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Sys_UserInGroup" // strTableNameDB
							, "Sys_UserInGroup." // strPrefixStd
							, "suig." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Sys_Group" // strTableNameDB
							, "Sys_Group." // strPrefixStd
							, "sg." // strPrefixAlias
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
					, "zzzzClauseSelect_Sys_User_zOut", zzzzClauseSelect_Sys_User_zOut
					, "zzzzClauseSelect_Sys_UserInGroup_zOut", zzzzClauseSelect_Sys_UserInGroup_zOut
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Sys_User)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Sys_User";
				}
				if (bGet_Sys_UserInGroup)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Sys_UserInGroup";
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

		public DataSet Sys_User_Get_01(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_User
			, string strRt_Cols_Sys_UserInGroup
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_User_Get_01";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_User_Get_01;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Sys_User", strRt_Cols_Sys_User
					, "strRt_Cols_Sys_UserInGroup", strRt_Cols_Sys_UserInGroup
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
				bool bGet_Sys_User = (strRt_Cols_Sys_User != null && strRt_Cols_Sys_User.Length > 0);
				bool bGet_Sys_UserInGroup = (strRt_Cols_Sys_UserInGroup != null && strRt_Cols_Sys_UserInGroup.Length > 0);

				//// drAbilityOfUser:
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
				////
				myCache_Mst_Distributor_ViewAbility_Get(drAbilityOfUser);
				////
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Sys_User_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
							, su.UserCode
						into #tbl_Sys_User_Filter_Draft
						from Sys_User su --//[mylock]
							left join Sys_UserInGroup suig --//[mylock]
								on su.UserCode = suig.UserCode
							left join Sys_Group sg --//[mylock]
								on suig.GroupCode = sg.GroupCode
                            left join Mst_Distributor md --//[mylock]
								on su.DBCode = md.DBCode
                            inner join #tbl_Mst_Distributor_ViewAbility va_md --//[mylock]
								on md.DBCode = va_md.DBCode
							left join Mst_AreaMarket mam --//[mylock]
								on md.AreaCode = mam.AreaCode 
							left join Aud_CampaignOLDtl acoldt --//[mylock] 
								on su.UserCode = acoldt.AuditUserCode
						where (1=1)
							zzzzClauseWhere_FilterAbilityOfUser -- Filter the AbilityOfUser
							zzzzClauseWhere_strFilterWhereClause
						order by su.UserCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Sys_User_Filter_Draft t --//[mylock]
						;

						---- #tbl_Sys_User_Filter:
						select
							t.*
						into #tbl_Sys_User_Filter
						from #tbl_Sys_User_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Sys_User --------:
						zzzzClauseSelect_Sys_User_zOut
						----------------------------------------

						-------- Sys_UserInGroup --------:
						zzzzClauseSelect_Sys_UserInGroup_zOut
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Sys_User_Filter_Draft;
						--drop table #tbl_Sys_User_Filter;
					"
					, "zzzzClauseWhere_FilterAbilityOfUser", ""
					);
				////
				string zzzzClauseSelect_Sys_User_zOut = "-- Nothing.";
				if (bGet_Sys_User)
				{
					#region // bGet_Sys_User:
					zzzzClauseSelect_Sys_User_zOut = CmUtils.StringUtils.Replace(@"
							---- Sys_User:
							select distinct
								t.MyIdxSeq
								, su.UserCode
                                , su.DBCode
                                , su.AreaCode
								, su.UserName
								, 'zzzzClausePVal_Default_PasswordMask' UserPassword
								, su.FlagSysAdmin
                                , su.FlagDBAdmin
								, su.FlagActive
								, md.DBCode md_DBCode
								, mam.AreaCode mam_AreaCode
							from #tbl_Sys_User_Filter t --//[mylock]
								inner join Sys_User su --//[mylock]
									on t.UserCode = su.UserCode
								inner join Aud_CampaignOLDtl acoldt --//[mylock] 
									on su.UserCode = acoldt.AuditUserCode
								left join Mst_Distributor md --//[mylock]
									on su.DBCode = md.DBCode
								left join Mst_AreaMarket mam --//[mylock]
									on su.AreaCode = mam.AreaCode
							order by t.MyIdxSeq asc
							;
						"
						, "zzzzClausePVal_Default_PasswordMask", TConst.BizMix.Default_PasswordMask
						);
					#endregion
				}
				////
				string zzzzClauseSelect_Sys_UserInGroup_zOut = "-- Nothing.";
				if (bGet_Sys_UserInGroup)
				{
					#region // bGet_Sys_UserInGroup:
					zzzzClauseSelect_Sys_UserInGroup_zOut = CmUtils.StringUtils.Replace(@"
							---- Sys_UserInGroup:
							select
								t.MyIdxSeq
								, suig.*
								, su.UserCode su_UserCode
								, su.UserName su_UserName 
								, su.FlagSysAdmin su_FlagSysAdmin 
								, su.FlagActive su_FlagActive 
								, sg.GroupCode sg_GroupCode
								, sg.GroupName sg_GroupName 
								, sg.FlagActive sg_FlagActive 
							from #tbl_Sys_User_Filter t --//[mylock]
								inner join Sys_UserInGroup suig --//[mylock]
									on t.UserCode = suig.UserCode
								left join Sys_User su --//[mylock]
									on suig.UserCode = su.UserCode
								left join Sys_Group sg --//[mylock]
									on suig.GroupCode = sg.GroupCode
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
							, "Sys_User" // strTableNameDB
							, "Sys_User." // strPrefixStd
							, "su." // strPrefixAlias
							);
						htSpCols.Remove("Sys_User.UserPassword".ToUpper());
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Sys_UserInGroup" // strTableNameDB
							, "Sys_UserInGroup." // strPrefixStd
							, "suig." // strPrefixAlias
							);
						////
						TUtils.CUtils.MyBuildHTSupportedColumns(
							_cf.db // db
							, ref htSpCols // htSupportedColumns
							, "Sys_Group" // strTableNameDB
							, "Sys_Group." // strPrefixStd
							, "sg." // strPrefixAlias
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
							, "Aud_CampaignOLDtl" // strTableNameDB
							, "Aud_CampaignOLDtl." // strPrefixStd
							, "acoldt." // strPrefixAlias
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
					, "zzzzClauseSelect_Sys_User_zOut", zzzzClauseSelect_Sys_User_zOut
					, "zzzzClauseSelect_Sys_UserInGroup_zOut", zzzzClauseSelect_Sys_UserInGroup_zOut
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Sys_User)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Sys_User";
				}
				if (bGet_Sys_UserInGroup)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Sys_UserInGroup";
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

		public DataSet Sys_User_Create(
			string strTid
			, DataRow drSession
			////
			, object objUserCode
			, object objDBCode
			, object objAreaCode
			, object objUserName
			, object objUserPassword
			, object objFlagSysAdmin
			, object objFlagDBAdmin
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_User_Create";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_User_Create;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objUserCode", objUserCode
                    , "objDBCode", objDBCode
                    , "objAreaCode", objAreaCode
					, "objUserName", objUserName
					, "objUserPassword", objUserPassword
					, "objFlagSysAdmin", objFlagSysAdmin
                    , "objFlagDBAdmin", objFlagDBAdmin
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
				string strUserCode = TUtils.CUtils.StdParam(objUserCode);
				string strDBCode = TUtils.CUtils.StdParam(objDBCode);
				string strAreaCode = TUtils.CUtils.StdParam(objAreaCode);
				string strUserName = string.Format("{0}", objUserName).Trim();
				string strUserPassword = string.Format("{0}", objUserPassword);
				string strFlagSysAdmin = CmUtils.StringUtils.StringEqual(objFlagSysAdmin, TConst.Flag.Yes) ? TConst.Flag.Yes : TConst.Flag.No;
				string strFlagDBAdmin = CmUtils.StringUtils.StringEqual(objFlagDBAdmin, TConst.Flag.Yes) ? TConst.Flag.Yes : TConst.Flag.No;
				////
				DataTable dtDB_Sys_User = null;
				DataTable dtDB_Mst_Distributor = null;
				DataTable dtDB_Mst_AreaMarket = null;
				{
					////
					if (strUserCode == null || strUserCode.Length <= 0)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strUserCode", strUserCode
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_User_Create_InvalidUserCode
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					Sys_User_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objUserCode // objUserCode
						, TConst.Flag.No // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Sys_User // dtDB_Sys_User
						);

					////
					if (strDBCode.Length > 0)
					{
						Mst_Distributor_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, objDBCode // objDBCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.StatusCm.Yes // strFlagActiveListToCheck
							, out dtDB_Mst_Distributor // dtDB_Mst_Distributor
						);
					}

					////
					if (strAreaCode.Length > 0)
					{
						Mst_AreaMarket_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, objAreaCode // objAreaCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.StatusCm.Yes // strFlagActiveListToCheck                            
							, out dtDB_Mst_AreaMarket // dtDB_Mst_AreaMarket
						);
					}
					////
					if (strUserName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strUserName", strUserName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_User_Create_InvalidUserName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (strUserPassword.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strUserPassword", strUserPassword
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_User_Create_InvalidUserPassword
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (strFlagSysAdmin.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strFlagSysAdmin", strFlagSysAdmin
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_User_Create_InvalidFlagSysAdmin
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
					if (strFlagDBAdmin.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strFlagDBAdmin", strFlagDBAdmin
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_User_Create_InvalidFlagDBAdmin
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
				}
				#endregion

				#region // SaveDB UserCode:
				{
					// Init:
					//ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Sys_User.NewRow();
					strFN = "UserCode"; drDB[strFN] = strUserCode;
					strFN = "AreaCode"; drDB[strFN] = strAreaCode;
					strFN = "DBCode"; drDB[strFN] = strDBCode;
					strFN = "UserName"; drDB[strFN] = strUserName;
					strFN = "UserPassword"; drDB[strFN] = strUserPassword;
					strFN = "FlagSysAdmin"; drDB[strFN] = strFlagSysAdmin;
					strFN = "FlagDBAdmin"; drDB[strFN] = strFlagDBAdmin;
					strFN = "FlagActive"; drDB[strFN] = TConst.Flag.Yes;
					strFN = "LogLUDTime"; drDB[strFN] = dtimeTDate.ToString("yyyy-MM-dd HH:mm:ss");
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode;
					dtDB_Sys_User.Rows.Add(drDB);

					// Save:
					_cf.db.SaveData(
						"Sys_User"
						, dtDB_Sys_User
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
		public DataSet Sys_User_Update(
			string strTid
			, DataRow drSession
			////
			, object objUserCode
			, object objDBCode
			, object objAreaCode
			, object objUserName
			, object objUserPassword
			, object objFlagSysAdmin
			, object objFlagDBAdmin
			, object objFlagActive
			, object objFt_Cols_Upd
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_User_Update";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_User_Update;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objUserCode", objUserCode
                    , "objDBCode", objDBCode
                    , "objAreaCode", objAreaCode
					, "objUserName", objUserName
					, "objUserPassword", objUserPassword
					, "objFlagSysAdmin", objFlagSysAdmin
                    , "objFlagDBAdmin", objFlagDBAdmin
					, "objFlagActive", objFlagActive
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
				string strUserCode = TUtils.CUtils.StdParam(objUserCode);
				string strDBCode = TUtils.CUtils.StdParam(objDBCode);
				string strAreaCode = TUtils.CUtils.StdParam(objAreaCode);
				string strUserName = string.Format("{0}", objUserName).Trim();
				string strUserPassword = string.Format("{0}", objUserPassword);
				string strFlagSysAdmin = (CmUtils.StringUtils.StringEqual(objFlagSysAdmin, TConst.StatusCm.Yes) ? TConst.StatusCm.Yes : TConst.StatusCm.No);
				string strFlagDBAdmin = (CmUtils.StringUtils.StringEqual(objFlagDBAdmin, TConst.StatusCm.Yes) ? TConst.StatusCm.Yes : TConst.StatusCm.No);
				string strFlagActive = (CmUtils.StringUtils.StringEqual(objFlagActive, TConst.StatusCm.No) ? TConst.StatusCm.No : TConst.StatusCm.Yes);
				////
				DataTable dtDB_Sys_User = null;
				DataTable dtDB_Mst_Distributor = null;
				DataTable dtDB_Mst_AreaMarket = null;
				bool bUpd_UserName = strFt_Cols_Upd.Contains("Sys_User.UserName".ToUpper());
				bool bUpd_DBCode = strFt_Cols_Upd.Contains("Sys_User.DBCode".ToUpper());
				bool bUpd_AreaCode = strFt_Cols_Upd.Contains("Sys_User.AreaCode".ToUpper());
				bool bUpd_UserPassword = strFt_Cols_Upd.Contains("Sys_User.UserPassword".ToUpper());
				bool bUpd_FlagSysAdmin = strFt_Cols_Upd.Contains("Sys_User.FlagSysAdmin".ToUpper());
				bool bUpd_FlagDBAdmin = strFt_Cols_Upd.Contains("Sys_User.FlagDBAdmin".ToUpper());
				bool bUpd_FlagActive = strFt_Cols_Upd.Contains("Sys_User.FlagActive".ToUpper());
				////
				{
					////
					Sys_User_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objUserCode // objUserCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Sys_User // dtDB_Sys_User
						);
					////
					if (bUpd_DBCode && strDBCode.Length > 0)
					{
						Mst_Distributor_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, objDBCode // objDBCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.StatusCm.Yes // strFlagActiveListToCheck
							, out dtDB_Mst_Distributor // dtDB_Mst_Distributor
						);
					}

					////
					if (bUpd_AreaCode && strAreaCode.Length > 0)
					{
						Mst_AreaMarket_CheckDB(
							ref alParamsCoupleError // alParamsCoupleError
							, objAreaCode // objAreaCode
							, TConst.Flag.Yes // strFlagExistToCheck
							, TConst.StatusCm.Yes // strFlagActiveListToCheck                            
							, out dtDB_Mst_AreaMarket // dtDB_Mst_AreaMarket
						);
					}
					////
					if (bUpd_UserName && strUserName.Length < 1)
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.strUserName", strUserName
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_User_Update_InvalidUserName
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					////
				}
				#endregion

				#region // SaveDB Sys_User:
				{
					// Init:
					ArrayList alColumnEffective = new ArrayList();
					string strFN = "";
					DataRow drDB = dtDB_Sys_User.Rows[0];
					if (bUpd_DBCode) { strFN = "DBCode"; drDB[strFN] = strDBCode; alColumnEffective.Add(strFN); }
					if (bUpd_AreaCode) { strFN = "AreaCode"; drDB[strFN] = strAreaCode; alColumnEffective.Add(strFN); }
					if (bUpd_UserName) { strFN = "UserName"; drDB[strFN] = strUserName; alColumnEffective.Add(strFN); }
					if (bUpd_UserPassword) { strFN = "UserPassword"; drDB[strFN] = strUserPassword; alColumnEffective.Add(strFN); }
					if (bUpd_FlagSysAdmin) { strFN = "FlagSysAdmin"; drDB[strFN] = strFlagSysAdmin; alColumnEffective.Add(strFN); }
					if (bUpd_FlagDBAdmin) { strFN = "FlagDBAdmin"; drDB[strFN] = strFlagDBAdmin; alColumnEffective.Add(strFN); }
					if (bUpd_FlagActive) { strFN = "FlagActive"; drDB[strFN] = strFlagActive; alColumnEffective.Add(strFN); }
					strFN = "LogLUDTime"; drDB[strFN] = dtimeTDate.ToString("yyyy-MM-dd HH:mm:ss"); alColumnEffective.Add(strFN);
					strFN = "LogLUBy"; drDB[strFN] = _cf.sinf.strUserCode; alColumnEffective.Add(strFN);

					// Save:
					_cf.db.SaveData(
						"Sys_User"
						, dtDB_Sys_User
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
		public DataSet Sys_User_Delete(
			string strTid
			, DataRow drSession
			////
			, object objUserCode
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_User_Delete";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_User_Delete;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objUserCode", objUserCode
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
				string strUserCode = TUtils.CUtils.StdParam(objUserCode);
				////
				DataTable dtDB_Sys_User = null;
				{
					////
					Sys_User_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objUserCode // objUserCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Sys_User // dtDB_Sys_User
						);
					//// Delete Sys_UserInGroup:
					Sys_UserInGroup_Delete_ByUser(
						strUserCode // strUserCode
						);
					////
				}
				#endregion

				#region // SaveDB UserCode:
				{
					// Init:
					dtDB_Sys_User.Rows[0].Delete();

					// Save:
					_cf.db.SaveData(
						"Sys_User"
						, dtDB_Sys_User
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

		#region // Sys_UserInGroup:
		private void Sys_UserInGroup_Delete_ByUser(
			object strUserCode
			)
		{
			string strSql_Exec = CmUtils.StringUtils.Replace(@"
					delete t
					from Sys_UserInGroup t --//[mylock]
					where (1=1)
						and t.UserCode = @strUserCode
					;
				");
			DataSet dsDB_Check = _cf.db.ExecQuery(
				strSql_Exec
				, "@strUserCode", strUserCode
				);
		}
		private void Sys_UserInGroup_Delete_ByGroup(
			object strGroupCode
			)
		{
			string strSql_Exec = CmUtils.StringUtils.Replace(@"
					delete t
					from Sys_UserInGroup t --//[mylock]
					where (1=1)
						and t.GroupCode = @strGroupCode
					;
				");
			DataSet dsDB_Check = _cf.db.ExecQuery(
				strSql_Exec
				, "@strGroupCode", strGroupCode
				);
		}

		public DataSet Sys_UserInGroup_Save(
			string strTid
			, DataRow drSession
			////
			, object objGroupCode
			, object[] arrobjDSData
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_UserInGroup_Save";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_UserInGroup_Save;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
					, "objGroupCode", objGroupCode
					});
			#endregion

			try
			{
				#region // Convert Input:
				DateTime dtimeTDate = DateTime.Now;
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

				#region // Refine and Check Input Master:
				////
				string strGroupCode = TUtils.CUtils.StdParam(objGroupCode);
				////
				DataTable dtDB_Sys_Group = null;
				////
				{
					////
					Sys_Group_CheckDB(
						ref alParamsCoupleError // alParamsCoupleError
						, objGroupCode // objGroupCode
						, TConst.Flag.Yes // strFlagExistToCheck
						, "" // strFlagActiveListToCheck
						, out dtDB_Sys_Group // dtDB_Sys_Group
						);
					////
					dtDB_Sys_Group.Rows[0]["LogLUDTime"] = dtimeTDate.ToString("yyyy-MM-dd HH:mm:ss");
					dtDB_Sys_Group.Rows[0]["LogLUBy"] = _cf.sinf.strUserCode;
					//// Upload:
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db // db
						, "#tbl_Sys_Group" // strTableName
						, TConst.BizMix.Default_DBColType // strDefaultType
						, new object[] { "GroupCode", "LogLUDTime", "LogLUBy" } // arrSingleStructure
						, dtDB_Sys_Group // dtData
						);
					////
				}
				#endregion

				#region // Refine and Check Input Detail:
				////
				DataTable dtInput_Sys_UserInGroup = null;
				////
				{
					////
					string strTableCheck = "Sys_UserInGroup";
					////
					if (!dsData.Tables.Contains(strTableCheck))
					{
						alParamsCoupleError.AddRange(new object[]{
							"Check.TableName", strTableCheck
							});
						throw CmUtils.CMyException.Raise(
							TError.ErrDemoLab.Sys_UserInGroup_Save_InputTblDtlNotFound
							, null
							, alParamsCoupleError.ToArray()
							);
					}
					dtInput_Sys_UserInGroup = dsData.Tables[strTableCheck];
					TUtils.CUtils.StdDataInTable(
						dtInput_Sys_UserInGroup // dtData
						, "StdParam", "UserCode" // arrstrCouple
						);
					//// Upload:
					TUtils.CUtils.MyBuildDBDT_Common(
						_cf.db // db
						, "#tbl_Sys_UserInGroup" // strTableName
						, TConst.BizMix.Default_DBColType // strDefaultType
						, new object[] { "UserCode" } // arrSingleStructure
						, dtInput_Sys_UserInGroup // dtData
						);
					////
				}
				#endregion

				#region // SaveDB Sys_UserInGroup:
				{
					string strSql_Exec = CmUtils.StringUtils.Replace(@"
						---- Clear All:
						delete t
						from Sys_UserInGroup t --//[mylock]
							inner join #tbl_Sys_Group t_sg --//[mylock]
								on t.GroupCode = t_sg.GroupCode
						where (1=1)
						;

						---- Insert All:
						insert into Sys_UserInGroup(
							GroupCode
							, UserCode
							, LogLUDTime
							, LogLUBy
							)
						select
							t_sg.GroupCode
							, t_suig.UserCode
							, t_sg.LogLUDTime
							, t_sg.LogLUBy
						from #tbl_Sys_Group t_sg --//[mylock]
							inner join #tbl_Sys_UserInGroup t_suig --//[mylock]
								on (1=1)
						;
					");
					DataSet dsDB_Check = _cf.db.ExecQuery(
						strSql_Exec
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

		#region // Sys_Object:
		public DataSet Sys_Object_Get(
			string strTid
			, DataRow drSession
			//// Filter:
			, string strFt_RecordStart
			, string strFt_RecordCount
			, string strFt_WhereClause
			//// Return:
			, string strRt_Cols_Sys_Object
			)
		{
			#region // Temp:
			DataSet mdsFinal = CmUtils.CMyDataSet.NewMyDataSet(strTid);
			//int nTidSeq = 0;
			bool bNeedTransaction = true;
			string strFunctionName = "Sys_Object_Get";
			string strErrorCodeDefault = TError.ErrDemoLab.Sys_Object_Get;
			ArrayList alParamsCoupleError = new ArrayList(new object[]{
					"strFunctionName", strFunctionName
			//// Filter
					, "strFt_RecordStart", strFt_RecordStart
					, "strFt_RecordCount", strFt_RecordCount
					, "strFt_WhereClause", strFt_WhereClause
			//// Return
					, "strRt_Cols_Sys_Object", strRt_Cols_Sys_Object
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

				#endregion

				#region // Check:
				// Refine:
				long nFilterRecordStart = Convert.ToInt64(strFt_RecordStart);
				long nFilterRecordEnd = nFilterRecordStart + Convert.ToInt64(strFt_RecordCount) - 1;
				bool bGet_Sys_Object = (strRt_Cols_Sys_Object != null && strRt_Cols_Sys_Object.Length > 0);

				// drAbilityOfAccess:
				//DataRow drAbilityOfAccess = mySys_Object_GetAbilityViewBankOfAccess(_cf.sinf.strAccessCode);

				#endregion

				#region // Build Sql:
				ArrayList alParamsCoupleSql = new ArrayList();
				//alParamsCoupleSql.AddRange(new object[] { "@strAbilityOfAccess", drAbilityOfAccess["MBBankBUPattern"] });
				alParamsCoupleSql.AddRange(new object[] { 
					"@nFilterRecordStart", nFilterRecordStart
					, "@nFilterRecordEnd", nFilterRecordEnd
					, "@Today", DateTime.Today.ToString("yyyy-MM-dd")
					});
				string strSqlGetData = CmUtils.StringUtils.Replace(@"
						---- #tbl_Sys_Object_Filter_Draft:
						select distinct
							identity(bigint, 0, 1) MyIdxSeq
                            , so.ObjectCode
						into #tbl_Sys_Object_Filter_Draft
						from Sys_Object so --//[mylock]
						where (1=1)
							zzzzClauseWhere_FilterAbilityOfUser -- Filter the AbilityOfAccess
							zzzzClauseWhere_strFilterWhereClause
						order by so.ObjectCode asc
						;

						---- Summary:
						select Count(0) MyCount from #tbl_Sys_Object_Filter_Draft t --//[mylock]
						;

						---- #tbl_Sys_Object_Filter:
						select
							t.*
						into #tbl_Sys_Object_Filter
						from #tbl_Sys_Object_Filter_Draft t --//[mylock]
						where
							(t.MyIdxSeq >= @nFilterRecordStart)
							and (t.MyIdxSeq <= @nFilterRecordEnd)
						;

						-------- Sys_Object --------:
						zzzzClauseSelect_Sys_Object_zOut
						----------------------------------------

						---- Clear for debug:
						--drop table #tbl_Sys_Object_Filter_Draft;
						--drop table #tbl_Sys_Object_Filter;
					"
					, "zzzzClauseWhere_FilterAbilityOfUser", ""
					);
				////
				string zzzzClauseSelect_Sys_Object_zOut = "-- Nothing.";
				if (bGet_Sys_Object)
				{
					#region // bGet_Sys_Object:
					zzzzClauseSelect_Sys_Object_zOut = CmUtils.StringUtils.Replace(@"
							---- Sys_Object:
							select
								t.MyIdxSeq
								, so.*
							from #tbl_Sys_Object_Filter t --//[mylock]
								inner join Sys_Object so --//[mylock]
                                     on  t.ObjectCode = so.ObjectCode
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
							, "Sys_Object" // strTableNameDB
							, "Sys_Object." // strPrefixStd
							, "so." // strPrefixAlias
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
					, "zzzzClauseSelect_Sys_Object_zOut", zzzzClauseSelect_Sys_Object_zOut
					);
				#endregion

				#region // Get Data:
				DataSet dsGetData = _cf.db.ExecQuery(
					strSqlGetData
					, alParamsCoupleSql.ToArray()
					);
				int nIdxTable = 0;
				dsGetData.Tables[nIdxTable++].TableName = "MySummaryTable";
				if (bGet_Sys_Object)
				{
					dsGetData.Tables[nIdxTable++].TableName = "Sys_Object";
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
		#endregion
	}
}

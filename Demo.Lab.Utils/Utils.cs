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
//using TUtils = Demo.Lab.Utils;
using TError = Demo.Lab.Errors;


namespace Demo.Lab.Utils
{
	public class CUtils
	{
		public static string TidNext(object strTidOrg, ref int n)
		{
			return string.Format("{0}.{1}", strTidOrg, n++);
		}
		public static string StdParam(object objParam)
		{
			if (objParam == null || objParam == DBNull.Value) return null;
			string str = Convert.ToString(objParam).Trim().ToUpper();
			if (str.Length <= 0) return null;
			return str;
		}
		public static object StdInt(object objParam)
		{
			if (objParam == null || objParam == DBNull.Value) return null;
			return Convert.ToInt32(objParam);
		}
		public static object StdDouble(object objParam)
		{
			if (objParam == null || objParam == DBNull.Value) return null;
			return Convert.ToDouble(objParam);
		}
		public static string StdDate(object objDate)
		{
			if (objDate == null || objDate == DBNull.Value) return null;
			DateTime dtime = Convert.ToDateTime(objDate);
			if (dtime < Convert.ToDateTime("1900-01-01 00:00:00") || dtime > Convert.ToDateTime("2100-01-01 23:59:59"))
				throw new Exception("MyBiz.DateTimeOutOfRange");
			return dtime.ToString("yyyy-MM-dd");
		}
		public static string StdDTime(object objDTime)
		{
			if (objDTime == null || objDTime == DBNull.Value || objDTime.ToString().Length == 0) return null;
			DateTime dtime = Convert.ToDateTime(objDTime);
			if (dtime < Convert.ToDateTime("1900-01-01 00:00:00") || dtime > Convert.ToDateTime("2100-01-01 23:59:59"))
				throw new Exception("MyBiz.DateTimeOutOfRange");
			return dtime.ToString("yyyy-MM-dd HH:mm:ss");
		}
		public static string StdDTimeBeginDay(object objDTime)
		{
			string strDTime = StdDate(objDTime);
			if (strDTime == null) return null;
			return string.Format("{0} 00:00:00", strDTime);
		}
		public static string StdDTimeEndDay(object objDTime)
		{
			string strDTime = StdDate(objDTime);
			if (strDTime == null) return null;
			return string.Format("{0} 23:59:59", strDTime);
		}
		public static string StdFlag(object strFlagRaw)
		{
			return (CmUtils.StringUtils.StringEqual(strFlagRaw, TConst.Flag.Active) ? TConst.Flag.Active : TConst.Flag.Inactive);
		}
		public static string StdMonth(object objMonth)
		{
			if (objMonth == null || objMonth == DBNull.Value) return null;
			DateTime dtime = Convert.ToDateTime(objMonth);
			if (dtime < Convert.ToDateTime("1900-01-01 00:00:00") || dtime > Convert.ToDateTime("2100-01-01 23:59:59"))
				throw new Exception("MyBiz.DateTimeOutOfRange");
			return (string.Format("{0}-01", dtime.ToString("yyyy-MM")));
		}
		public static DataTable StdDataInTable(DataTable dtData, params string[] arrstrCouple)
		{
			// Check:
			if (dtData == null
				|| dtData.Rows.Count <= 0
				|| arrstrCouple == null
				|| arrstrCouple.Length <= 0
				)
				return dtData;

			// Init:
			CmUtils.DataTableUtils.DataTableCore_BeginUpdate(dtData);
			foreach (DataRow drScan in dtData.Rows)
			{
				for (int nScan = 0; nScan < arrstrCouple.Length; nScan += 2)
				{
					if (CmUtils.StringUtils.StringEqualIgnoreCase(arrstrCouple[nScan], "StdParam"))
					{
						drScan[arrstrCouple[nScan + 1]] = StdParam(drScan[arrstrCouple[nScan + 1]]);
					}
					else if (CmUtils.StringUtils.StringEqualIgnoreCase(arrstrCouple[nScan], "StdDate"))
					{
						drScan[arrstrCouple[nScan + 1]] = StdDate(drScan[arrstrCouple[nScan + 1]]);
					}
					else if (CmUtils.StringUtils.StringEqualIgnoreCase(arrstrCouple[nScan], "StdDTime"))
					{
						drScan[arrstrCouple[nScan + 1]] = StdDTime(drScan[arrstrCouple[nScan + 1]]);
					}
					else if (CmUtils.StringUtils.StringEqualIgnoreCase(arrstrCouple[nScan], "StdFlag"))
					{
						drScan[arrstrCouple[nScan + 1]] = StdFlag(drScan[arrstrCouple[nScan + 1]]);
					}
					else if (CmUtils.StringUtils.StringEqualIgnoreCase(arrstrCouple[nScan], "StdMonth"))
					{
						drScan[arrstrCouple[nScan + 1]] = StdMonth(drScan[arrstrCouple[nScan + 1]]);
					}
					else if (CmUtils.StringUtils.StringEqualIgnoreCase(arrstrCouple[nScan], "StdInt"))
					{
						drScan[arrstrCouple[nScan + 1]] = StdInt(drScan[arrstrCouple[nScan + 1]]);
					}
					else if (CmUtils.StringUtils.StringEqualIgnoreCase(arrstrCouple[nScan], "StdDouble"))
					{
						drScan[arrstrCouple[nScan + 1]] = StdDouble(drScan[arrstrCouple[nScan + 1]]);
					}
				}
			}
			CmUtils.DataTableUtils.DataTableCore_EndUpdate(dtData);

			// Return Good:
			return dtData;
		}
		public static DataSet StdDS(object[] arrobjDS)
		{
			try
			{
				return CmUtils.ConvertUtils.Array2DataSet(arrobjDS);
			}
			catch (Exception exc)
			{
				throw new Exception("MyBiz.DataSetRawInvalidFormat", exc);
			}
		}
		public static object IsNullSql(object objParam)
		{ 
			return (objParam == null ? DBNull.Value: objParam);
		}

		public static Hashtable MyBuildHTSupportedColumns(
			EzSql.IDBEngine db
			, ref Hashtable htSupportedColumns
			, string strTableNameDB
			, string strPrefixStd
			, string strPrefixAlias
			, params object[] arrstrExcept
			)
		{
			DataTable dt = EzDAL.Utils.DBUtils.GetSchema(db, strTableNameDB).Tables[0];
			return CmUtils.SqlUtils.BuildSupportedColumnsInfo(
				ref htSupportedColumns // htSupportedColumns
				, dt // dtSource
				, strPrefixStd // strPrefixStd
				, strPrefixAlias // strPrefixAlias
				, arrstrExcept // arrstrExcept
				);
		}
		public static string MyBuildSql_GetTempTable(string zzzzClauseTable, string zzzzClauseColumn)
		{
			// Init:
			string strSql_TempTbl = CmUtils.StringUtils.Replace(@"
							select 
								identity(bigint, 0, 1) MyIdxSeq
								, zzzzClauseColumn
							into #tbl_zzzzClauseTable
							from zzzzClauseTable t --//[mylock]
							where (0=1)
							;
							select * from #tbl_zzzzClauseTable t --//[mylock]
							;
						"
						, "zzzzClauseTable", zzzzClauseTable
						, "zzzzClauseColumn", zzzzClauseColumn
						);

			// Return Good:
			return strSql_TempTbl;
		}
		public static DataTable MyBuildDBDT_CreateTableParam(EzSql.IDBEngine db, string zzzzClauseColumn, string zzzzClauseTable)
		{
			// Init:
			string strSql_TempTbl = CmUtils.StringUtils.Replace(@"
					select 
						zzzzClauseColumn
					into zzzzClauseTable
					where (0=1)
					;
					select * from zzzzClauseTable t --//[mylock]
					;
				"
				, "zzzzClauseColumn", zzzzClauseColumn
				, "zzzzClauseTable", zzzzClauseTable
				);
			DataTable dtDB = db.ExecQuery(strSql_TempTbl).Tables[0];

			// Return Good:
			return dtDB;
		}
		public static DataTable MyBuildDBDT_CreateTableParam(EzSql.IDBEngine db)
		{
			// Init:
			string zzzzClauseColumn = @"
					Cast(null as nvarchar(200)) PCODE0, Cast(null as nvarchar(400)) PVAL0
					, Cast(null as nvarchar(200)) PCODE1, Cast(null as nvarchar(400)) PVAL1
					, Cast(null as nvarchar(200)) PCODE2, Cast(null as nvarchar(400)) PVAL2
					, Cast(null as nvarchar(200)) PCODE3, Cast(null as nvarchar(400)) PVAL3
					, Cast(null as nvarchar(200)) PCODE4, Cast(null as nvarchar(400)) PVAL4
					, Cast(null as nvarchar(200)) PCODE5, Cast(null as nvarchar(400)) PVAL5
				";
			string zzzzClauseTable = "#tbl_Param";
			DataTable dtDB = MyBuildDBDT_CreateTableParam(db, zzzzClauseColumn, zzzzClauseTable);

			// Return Good:
			return dtDB;
		}
		public static DataTable MyBuildDBDT_Common(
			EzSql.IDBEngine db
			, string strTableName
			, object strDefaultType
			, object[] arrSingleStructure
			, object[] arrArrParamValue
			)
		{
			// Init:
			ArrayList alCoupleStructure = new ArrayList();
			foreach (object objScan in arrSingleStructure)
			{
				alCoupleStructure.Add(objScan);
				alCoupleStructure.Add(strDefaultType);
			}

			// Return Good:
			return MyBuildDBDT_Common(db, strTableName, alCoupleStructure.ToArray(), arrArrParamValue);
		}
		public static DataTable MyBuildDBDT_Common(
			EzSql.IDBEngine db
			, string strTableName
			, object strDefaultType
			, object[] arrSingleStructure
			, DataTable dtData
			)
		{
			// Init:
			ArrayList alCoupleStructure = new ArrayList();
			foreach (object objScan in arrSingleStructure)
			{
				alCoupleStructure.Add(objScan);
				alCoupleStructure.Add(strDefaultType);
			}

			// Return Good:
			return MyBuildDBDT_Common(db, strTableName, alCoupleStructure.ToArray(), dtData);
		}
		public static DataTable MyBuildDBDT_Common(
			EzSql.IDBEngine db
			, string strTableName
			, object[] arrCoupleStructure
			, object[] arrArrParamValue
			)
		{
			#region // Build Structure:
			DataTable dtDB = null;
			{
				// Init:
				StringBuilder strbdTemp = new StringBuilder(1000);
				string strMySeparator = ", ";

				// Scan:
				for (int nScan = 0; nScan < arrCoupleStructure.Length; nScan += 2)
				{
					// Ex: ", Cast(null as nvarchar(200)) PCODE1"
					strbdTemp.AppendFormat(
						"{0}Cast(null as {1}) {2}"
						, strMySeparator // {0}
						, arrCoupleStructure[nScan + 1] // {1}
						, Convert.ToString(arrCoupleStructure[nScan]).ToUpper() // {2}
						);
				}
				string zzzzClauseColumn = strbdTemp.ToString(strMySeparator.Length, strbdTemp.Length - strMySeparator.Length);

				// Build Table:
				string strSql_TempTbl = CmUtils.StringUtils.Replace(@"
						select 
							zzzzClauseColumn
						into zzzzClauseTable
						where (0=1)
						;
						select * from zzzzClauseTable t --//[mylock]
						;
					"
					, "zzzzClauseColumn", zzzzClauseColumn
					, "zzzzClauseTable", strTableName
					);
				dtDB = db.ExecQuery(strSql_TempTbl).Tables[0];
			}
			#endregion

			#region // ProcessData:
			if (arrArrParamValue == null || arrArrParamValue.Length <= 0)
			{
				return dtDB;
			}
			else
			{
				// Fill Data:
				for (int nScan = 0; nScan < arrArrParamValue.Length; nScan++)
				{
					object[] arrItems = (object[])arrArrParamValue[nScan];
					dtDB.Rows.Add(arrItems);
				}
				dtDB.AcceptChanges();

				// Save to DB:
				db.InsertHuge(strTableName, dtDB);
			}
			#endregion

			// Return Good:
			return dtDB;
		}
		public static DataTable MyBuildDBDT_Common(
			EzSql.IDBEngine db
			, string strTableName
			, object[] arrCoupleStructure
			, DataTable dtData
			)
		{
			////
			ArrayList alCols = new ArrayList();
			for (int nScan = 0; nScan < arrCoupleStructure.Length; nScan += 2)
			{
				alCols.Add(Convert.ToString(arrCoupleStructure[nScan]).ToUpper());
			}
			////
			ArrayList alArrParamValue = new ArrayList();
			if (dtData != null && dtData.Rows.Count > 0)
			{
				foreach (DataRow drScan in dtData.Rows)
				{
					ArrayList alItems = new ArrayList();
					foreach (string strCol in alCols)
					{
						alItems.Add(drScan[strCol]);
					}
					alArrParamValue.Add(alItems.ToArray());
				}
			}
			////

			// Return Good:
			return MyBuildDBDT_Common(db, strTableName, arrCoupleStructure, alArrParamValue.ToArray());
		}
		public static void MyBuildDBDT_InsertTableParam(EzSql.IDBEngine db, DataTable dtDB_Param_Template, object[] arrArrayParams)
		{
			// Init:
			DataTable dtDB_Param = dtDB_Param_Template.Clone();
			CmUtils.DataTableUtils.DataTableCore_BeginUpdate(dtDB_Param);
			foreach (object[] arrItems in arrArrayParams)
			{
				DataRow drScan = dtDB_Param.NewRow();
				for (int nScan = 0; nScan < arrItems.Length; nScan++)
				{
					drScan[nScan] = arrItems[nScan];
				}
				dtDB_Param.Rows.Add(drScan);
			}
			CmUtils.DataTableUtils.DataTableCore_EndUpdate(dtDB_Param);

			List<string> lstMapFN = new List<string>();
			for (int nScan = 0; nScan < dtDB_Param.Columns.Count; nScan++)
			{
				lstMapFN.AddRange(new string[] { dtDB_Param.Columns[nScan].ColumnName, dtDB_Param.Columns[nScan].ColumnName });
			}

			// Save:
			db.InsertHuge(
				"#tbl_Param" // strTableName
				, dtDB_Param // dtData
				, lstMapFN.ToArray() // arrstrColumnsCouple
				);
		}
		public static void MyBuildDBDT_InsertTableParam(EzSql.IDBEngine db, object[] arrArrayParams)
		{
			// Init:
			DataTable dtDB_Param_Template = new DataTable("dtDB_Param");
			dtDB_Param_Template.Columns.Add("PCODE0", typeof(string)); dtDB_Param_Template.Columns.Add("PVAL0", typeof(string));
			dtDB_Param_Template.Columns.Add("PCODE1", typeof(string)); dtDB_Param_Template.Columns.Add("PVAL1", typeof(string));
			dtDB_Param_Template.Columns.Add("PCODE2", typeof(string)); dtDB_Param_Template.Columns.Add("PVAL2", typeof(string));
			dtDB_Param_Template.Columns.Add("PCODE3", typeof(string)); dtDB_Param_Template.Columns.Add("PVAL3", typeof(string));
			dtDB_Param_Template.Columns.Add("PCODE4", typeof(string)); dtDB_Param_Template.Columns.Add("PVAL4", typeof(string));
			dtDB_Param_Template.Columns.Add("PCODE5", typeof(string)); dtDB_Param_Template.Columns.Add("PVAL5", typeof(string));

			// Return Good:
			MyBuildDBDT_InsertTableParam(db, dtDB_Param_Template, arrArrayParams);
		}

		public static DataTable MyForceNewColumn(ref DataTable dt, string strColumnName, Type t)
		{
			if (dt == null) return dt;
			if (dt.Columns.Contains(strColumnName)) dt.Columns.Remove(strColumnName);
			dt.Columns.Add(strColumnName, t);
			return dt;
		}

		public static void ProcessMyDS(DataSet mdsFinal)
		{
			ServiceException svException = GenServiceException(mdsFinal);

			if (svException.HasError())
			{
				//MessageBox.Show(svException.ErrorMessage);
				//CommonForms.CFormInputParam.ShowMessage(svException.ErrorCode, svException.ErrorDetail);
				throw svException;
			}
		}

        public static ServiceException GenServiceException(DataSet ds)
        {
            ServiceException ex = new ServiceException();
            if (ds == null)
            {
                ex.ErrorCode = "ERR0001";
                ex.ErrorMessage = "Null dataset return";
                ex.ErrorDetail = "Null dataset return";
                ex.Tag = "";
                return ex;
            }
            //
            string errorCode = Convert.ToString(CmUtils.CMyDataSet.GetErrorCode(ds));
            string errorMessage = Convert.ToString(CmUtils.CMyDataSet.GetErrorCode(ds));
            StringBuilder sbDetail = new StringBuilder();

            sbDetail.Append(string.Format("Error Code: {0}", errorCode));
            sbDetail.Append("<br/>--------------------------------------------------------<br/>");
            object[] arrObj = CmUtils.CMyDataSet.GetErrorParams(ds);
            if (arrObj != null && arrObj.Length > 1)
            {
                for (int i = 0; i < arrObj.Length; i++)
                {
                    string val = Convert.ToString(arrObj[i]);

                    val = System.Net.WebUtility.HtmlEncode(val);
                    sbDetail.Append(val.Replace("\n", "<br/>"));
                    if (i % 2 == 0)
                        sbDetail.Append(" = ");
                    else
                        sbDetail.Append("<br/>--------------------------------------------------------<br/>");
                }
            }
            ex.ErrorCode = errorMessage;
            ex.ErrorMessage = errorCode;

            if (ErrorCodes.DIC_ERROR_CODES.ContainsKey(errorCode))
            {
                ex.ErrorMessage = ErrorCodes.DIC_ERROR_CODES[errorCode];
            }

            ex.ErrorDetail = sbDetail.ToString();
            ex.Tag = "";
            return ex;
        }

	}

	public class CProcessExc
	{
		public static DataSet Process(
			ref DataSet mdsFinal
			, Exception exc
			, string strErrorCode
			, params object[] arrobjErrorParams
			)
		{
			// Process:
			if (CmUtils.CMyException.IsMyException(exc))
			{
				CmUtils.CMyDataSet.SetErrorCode(ref mdsFinal, CmUtils.CMyException.GetErrorCode(exc));
				CmUtils.CMyDataSet.AppendErrorParams(ref mdsFinal, CmUtils.CMyException.GetErrorParams(exc));
			}
			else
			{
				CmUtils.CMyDataSet.SetErrorCode(ref mdsFinal, strErrorCode);
				CmUtils.CMyDataSet.AppendErrorParams(ref mdsFinal, arrobjErrorParams);
				if (exc.Data != null)
				{
					foreach (object objDataKey in exc.Data.Keys)
					{
						CmUtils.CMyDataSet.AppendErrorParams(ref mdsFinal, string.Format("ExcData.{0}", objDataKey), string.Format("{0}", exc.Data[objDataKey]));
					}
				}
				CmUtils.CMyDataSet.AppendErrorParams(
					ref mdsFinal
					, "ExceptionContents", string.Format("{0}\r\n{1}", exc.Message, exc.StackTrace)
					);
			}

			// Always return Bad:
			mdsFinal.AcceptChanges();
			return mdsFinal;
		}
	}

	public class CConnectionManager
	{
		#region // Utils:
		private static string s_strNameTimeoutMillisecond = "Biz_TimeoutMS";
		private static bool myCheckExpired(
			System.Collections.Specialized.NameValueCollection nvcParams
			, DataRow drCheck
			)
		{
			bool bExpired = ((DateTime.Now - Convert.ToDateTime(drCheck["DateTimeLastAccess"])).TotalMilliseconds > Convert.ToDouble(nvcParams[s_strNameTimeoutMillisecond]));
			return bExpired;
		}
		#endregion

		#region // Public members:
		public static void CheckGatewayAuthentication(
			System.Collections.Specialized.NameValueCollection nvcParams
			, ref ArrayList alParamsCoupleError
			, string strGwUserCode
			, string strGwPassword
			)
		{
			// Check GatewayAuthentication:
			if (!CmUtils.StringUtils.StringEqualIgnoreCase(strGwUserCode, nvcParams["Biz_GwUserCode"])
				|| !CmUtils.StringUtils.StringEqual(strGwPassword, nvcParams["Biz_GwPassword"])
				)
			{
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.CmSys_GatewayAuthenticateFailed
					, null
					, alParamsCoupleError.ToArray()
					);
			}
		}
		public static void CleanSessionExpired(
			System.Collections.Specialized.NameValueCollection nvcParams
			, TSession.Core.CSession sess
			)
		{
			sess.CleanExpired(
				true // bLazyMode
				, (DateTime.Now.AddMilliseconds(-Convert.ToDouble(nvcParams[s_strNameTimeoutMillisecond]))) // objDateTimeExpiredForLastAccess
				);
		}
		public static DataRow CheckSessionInfo(
			TSession.Core.CSession sess
			, ref ArrayList alParamsCoupleError
			, string strSessionId
			)
		{
			DataRow drSession = sess.GetFromId(strSessionId);
			if (drSession == null)
			{
				alParamsCoupleError.AddRange(new object[]{
					"Check.SessionId", strSessionId
					});
				throw CmUtils.CMyException.Raise(
					TError.ErrDemoLab.CmSys_SessionNotFound
					, null
					, alParamsCoupleError.ToArray()
					);
			}
			return drSession;
		}
		public static void RaiseSessionExpired(
			System.Collections.Specialized.NameValueCollection nvcParams
			, ref ArrayList alParamsCoupleError
			, DataRow drSession
			)
		{
			// Check ExpiredSession:
			alParamsCoupleError.AddRange(new object[]{
				"Check.DateTimeLastAccess", drSession["DateTimeLastAccess"]
				, "Check.DateTimeNow", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
				, "Check.TotalMilliseconds", (DateTime.Now - Convert.ToDateTime(drSession["DateTimeLastAccess"])).TotalMilliseconds
				, "Check.TimeoutMillisecond", nvcParams[s_strNameTimeoutMillisecond]
				});
			throw CmUtils.CMyException.Raise(
				TError.ErrDemoLab.CmSys_SessionExpired
				, null
				, alParamsCoupleError.ToArray()
				);
		}
		public static void CheckAllCondition(
			System.Collections.Specialized.NameValueCollection nvcParams
			, TSession.Core.CSession sess
			, ref ArrayList alParamsCoupleError
			, string strGwUserCode
			, string strGwPassword
			, string strSessionId
			, out DataRow drSession
			)
		{
			// Init:
			drSession = null;

			// Check GatewayAuthentication:
			CheckGatewayAuthentication(
				nvcParams // nvcParams
				, ref alParamsCoupleError // alParamsCoupleError
				, strGwUserCode // strGwUserCode
				, strGwPassword // strGwPassword
				);

			// Check Session:
			drSession = CheckSessionInfo(
				sess // sess
				, ref alParamsCoupleError // alParamsCoupleError
				, strSessionId // strSessionId
			);

			// Check ExpiredSession:
			if (myCheckExpired(nvcParams, drSession))
			{
				CleanSessionExpired(nvcParams, sess);
				RaiseSessionExpired(nvcParams, ref alParamsCoupleError, drSession);
			}

			// Reset LastAccess:
			sess.UpdateLastAccess(
				false // bLazyMode
				, strSessionId // strSessionId
				, DateTime.Now // objDateTimeLastAccess
				);
		}

		#endregion
	}



	public class ServiceException : Exception
	{
		public static string ERROR_CODE_CLIENT = "ERROR_CLIENT";
		//
		private string _errorMessage;
		private string _errorDetail;
		private string _errorCode;
		//
		private object _tag;
		//
		public Boolean isWarning = false;

		public string ErrorDetail
		{
			set { _errorDetail = value; }
			get { return _errorDetail; }
		}

		public string ErrorCode
		{
			set { _errorCode = value; }
			get { return _errorCode; }
		}

		public string ErrorMessage
		{
			set { _errorMessage = value; }
			get { return _errorMessage; }
		}

		public object Tag
		{
			set { _tag = value; }
			get { return _tag; }
		}

		public bool HasError()
		{
			return (this.ErrorCode != null && this.ErrorCode.Length > 0);
		}
	}

}

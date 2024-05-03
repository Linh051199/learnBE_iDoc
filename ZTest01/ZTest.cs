using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.Xml;
//using System.Xml.Linq;
using System.Threading;
//using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

using CmUtils = CommonUtils;
using CommonUtils;
//using TDAL = EzDAL.MyDB;
//using TDALUtils = EzDAL.Utils;
//using TConst = IDM.Constants;
//using TUtils = IDM.Utils;
//using TError = IDM.Errors;


namespace ZTest01
{
	public partial class ZTest : Form
	{
		#region // Constructors and Destructors:
		public ZTest()
		{
			InitializeComponent();
		}

		#endregion

		#region // TestMix:
		private void btnTestMix_Click(object sender, EventArgs e)
		{
			try
			{
				TestMix_01_CallService();

			}
			catch (Exception exc)
			{
				CommonForms.Utils.ProcessExc(exc);
			}
		}

		private void TestMix_01_CallService()
		{
			////
			WSDemoLab.WS ws = new WSDemoLab.WS();
			string strUrl = ws.Url;
			//string strUrl = "http://118.70.129.122:12608/idocNet.Test.Demo.Lab.Sv.V10/WS.asmx";
			CmUtils.WebServiceUtils.Refine(ws, strUrl, 123456000, true);
			////
			object objResult = ws.Cm_Test();
			////
			string strGwUserCode = "idocNet.Demo.Lab.STD.Sv";
			string strGwPassword = "idocNet.Demo.Lab.STD.Sv";
			//string strTid = DateTime.Now.ToString("yyyyMMdd.HHmmss.ffffff");
			string strRootSvCode = "WEBAPP";
			string strRootUserCode = "SYSADMIN";
			string strServiceCode = "WEBAPP";
			string strUserCode = "SYSADMIN";
			string strLanguageCode = "EN-US";
			string strUserPassword = "1";
			string strOtherInfo = "strOtherInfo = 123456";
			bool bTest = false;
			string strFt_RecordStart = "0";
			string strFt_RecordCount = "12345600";
			DataSet mdsResult = null;
			DataTable dtTable0 = null;
			DataTable dtTable1 = null;
			DataTable dtTable2 = null;
			int nSeq = 0;

			#region // Login:
			mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Sys_User_Login(
				strGwUserCode // strGwUserCode
				, strGwPassword // strGwPassword
				, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
				, strRootSvCode // strRootSvCode
				, strRootUserCode // strRootUserCode
				, strServiceCode // strServiceCode
				, strUserCode // strUserCode
				, strLanguageCode // strLanguageCode
				, strUserPassword // strUserPassword
				, strOtherInfo // strOtherInfo
				));
			if (CmUtils.CMyDataSet.HasError(mdsResult))
			{
				CommonForms.Utils.ProcessMyDS(mdsResult);
				return;
			}
			string strResult_Remark = Convert.ToString(CmUtils.CMyDataSet.GetRemark(mdsResult));
			string[] arrstrResult = strResult_Remark.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
			string strSessionId = arrstrResult[0];
			string strBizName = arrstrResult[1];

			// Pause:
			System.Threading.Thread.Sleep(10);
			#endregion

			#region // Seq_Common_Get:
			if (bTest)
			{
				string strSequenceType = "CAMPAIGNCRAWARDCODE";
				string strParam_Prefix = "Hello";
				string strParam_Postfix = "Bye";
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Seq_Common_Get(
					strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
					, strSessionId // strSessionId
					, strSequenceType
					, strParam_Prefix
					, strParam_Postfix
					));
				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_Common_Get:
			if (bTest)
			{
				string strTableName = "Ins_ClaimDocType";
				object objFilter0 = null;
				object objFilter1 = null;
				object objFilter2 = null;
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_Common_Get(
					strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
					, strSessionId // strSessionId
					, strTableName // strTableName
					, objFilter0 // objFilter0
					, objFilter1 // objFilter1
					, objFilter2 // objFilter2
					));
				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			////
			Thread.Sleep(10);
			////

		}

		private void TestMix_SimpleLoop()
		{
			bool bTest = false;

			#region // Loop Table:
			if (!bTest)
			{
				////
				DataTable dtData_InvF_AccessoryInExtDtl = new DataTable("InvF_AccessoryInExtDtl");
				dtData_InvF_AccessoryInExtDtl.Columns.Add("PartCode", typeof(object));
				dtData_InvF_AccessoryInExtDtl.Columns.Add("Qty", typeof(object));
				dtData_InvF_AccessoryInExtDtl.Columns.Add("Price", typeof(object));
				dtData_InvF_AccessoryInExtDtl.Columns.Add("Remark", typeof(object));

				////
				dtData_InvF_AccessoryInExtDtl.Rows.Add("01-01-001", 20.5, 2000000, "1");
				dtData_InvF_AccessoryInExtDtl.Rows.Add("FF98", 10, 1000000, "1");
				dtData_InvF_AccessoryInExtDtl.Rows.Add("FF996", 10, 1000000, "1");
				dtData_InvF_AccessoryInExtDtl.Rows.Add("01-01-004", 10, 1000000, "1");
				dtData_InvF_AccessoryInExtDtl.Rows.Add("01-01-005", 10, 1000000, "1");

				//for (int nScan = 0; nScan < dtData_InvF_AccessoryInExtDtl.Rows.Count; nScan++)
				//{
				//	////
				//	DataRow drScan = dtData_InvF_AccessoryInExtDtl.Rows[nScan];

				//	string strPartCode = 
				//}
			}
			#endregion
		}

		#endregion

		private void ZTest_Load(object sender, EventArgs e)
		{

		}

	}
}

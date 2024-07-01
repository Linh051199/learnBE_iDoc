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
			WSDVNAUD.WS ws = new WSDVNAUD.WS();
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

			#region // Mst_POSMType_Create:
			if (bTest)
			{
				string strTid = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++);
				//// 
				string strPOSMType = "PTD";
				string strPOSMTypeName = "Treo dán";

				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_POSMType_Create(
					strGwUserCode       // strGwUserCode
					, strGwPassword     // strGwPassword
					, strTid            // strTid
					, strSessionId      // strSessionId
										////
					, strPOSMType       // objPOSMType
					, strPOSMTypeName   // objPOSMTypeName
					));

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_POSMUnitType_Create:
			if (bTest)
			{
				string strTid = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++);
				//// 
				string strPOSMUnitType = "SHEET";
				string strPOSMUnitTypeName = "Tờ";

				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_POSMUnitType_Create(
					strGwUserCode           // strGwUserCode
					, strGwPassword         // strGwPassword
					, strTid                // strTid
					, strSessionId          // strSessionId
											////
					, strPOSMUnitType       // objPOSMType
					, strPOSMUnitTypeName   // objPOSMTypeName
					));

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_POSM_Create:
			if (bTest)
			{
				string strTid = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++);
				//// 
				string strPOSMCode = "GB";
				string strPOSMType = "PTB";
				string strPOSMUnitType = "UNIT";
				string strPOSMName = "Giá trưng bày SP";
				string strPOSMDesc = "Kích thước lớn";
				string strPOSMImageFilePath = "Đường dẫn hình ảnh"; 

				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_POSM_Create(
					strGwUserCode            // strGwUserCode
					, strGwPassword          // strGwPassword
					, strTid                 // strTid
					, strSessionId           // strSessionId
											 ////
					, strPOSMCode           //objPOSMCode
					, strPOSMType           //objPOSMType
					, strPOSMUnitType       //objPOSMUnitType
					, strPOSMName           //objPOSMName
					, strPOSMDesc           //objPOSMDesc
					, strPOSMImageFilePath  //objPOSMImageFilePath
					));

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_StarShopGroup_Create:
			if (bTest)
			{
				string strTid = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++);
				//// 
				string strSSGrpCode = "1";
				string strSSGrpName = "SSGD";

				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_StarShopGroup_Create(
					strGwUserCode       // strGwUserCode
					, strGwPassword     // strGwPassword
					, strTid            // strTid
					, strSessionId      // strSessionId
										////
					, strSSGrpCode      // objSSGrpCode
					, strSSGrpName      // objSSGrpName
					));

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_StarShopBrand_Create:
			if (bTest)
			{ 
				string strTid = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++);
				//// 
				string strSSBrandCode = "1";
				string strSSBrandName = "SSD";

				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_StarShopBrand_Create(
					strGwUserCode           // strGwUserCode
					, strGwPassword         // strGwPassword
					, strTid                // strTid
					, strSessionId          // strSessionId
											////
					, strSSBrandCode        // objSSBrandCode
					, strSSBrandName        // objSSBrandName
					));

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_StarShopType_Create:
			if (bTest)
			{
				string strTid = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++);
				//// 
				string strSSGrpCode = "1";
				string strSSBrandCode = "1";
				string strSSTypeName = "SSType Name 1";
				string strSSRate = "2";

				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_StarShopType_Create(
					strGwUserCode           // strGwUserCode
					, strGwPassword         // strGwPassword
					, strTid                // strTid
					, strSessionId          // strSessionId
											////
					, strSSGrpCode          // objSSGrpCode
					, strSSBrandCode        // objSSBrandCode
					, strSSTypeName         // objSSTypeName
					, strSSRate             // objSSRate
					));

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_CampainCriteria_Save: 
			if (bTest)
			{
				////
				object objFlagIsDelete = "0";
				object objCampaignCrCode = "ThangDepZai";
				object objCampaignCrName = "1000OL";
				object objCampainCriteriaType = "PhichThu";

				DataSet dsData = new DataSet();

				////
				DataTable dtData_Mst_CampainCriteriaScope = new DataTable("Mst_CampainCriteriaScope");
				dtData_Mst_CampainCriteriaScope.Columns.Add("SSGrpCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("SSBrandCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("LevelCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("CampainCritScopeDesc", typeof(object));

				////
				dtData_Mst_CampainCriteriaScope.Rows.Add("NORMALOUTLET", "NORMALOUTLET", "", "LEVEL 11");
				////

				dsData.Tables.Add(dtData_Mst_CampainCriteriaScope); 
				object[] arrobjDSData = CmUtils.ConvertUtils.DataSet2Array(dsData);
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_CampainCriteria_Save(
						strGwUserCode // strGwUserCode
						, strGwPassword // strGwPassword
						, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
						, strSessionId // strSessionId
									   ////
						, objFlagIsDelete
						////
						, objCampaignCrCode
						, objCampaignCrName
						, objCampainCriteriaType 
						, arrobjDSData
					));

				dtTable0 = mdsResult.Tables[0];
				dtTable1 = mdsResult.Tables[1];
				dtTable2 = mdsResult.Tables[2];

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_CampainCriteria_Save: 
			if (bTest)
			{
				////
				object objFlagIsDelete = "0";
				object objCampaignCrCode = "ThangDepZai";
				object objCampaignCrName = "1000OL";
				object objCampainCriteriaType = "PhichThu";

				DataSet dsData = new DataSet();

				////
				DataTable dtData_Mst_CampainCriteriaScope = new DataTable("Mst_CampainCriteriaScope");
				dtData_Mst_CampainCriteriaScope.Columns.Add("SSGrpCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("SSBrandCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("LevelCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("CampainCritScopeDesc", typeof(object));

				////
				dtData_Mst_CampainCriteriaScope.Rows.Add("NORMALOUTLET", "NORMALOUTLET", "", "LEVEL 11");
				////

				dsData.Tables.Add(dtData_Mst_CampainCriteriaScope); 
				object[] arrobjDSData = CmUtils.ConvertUtils.DataSet2Array(dsData);
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_CampainCriteria_Save(
						strGwUserCode // strGwUserCode
						, strGwPassword // strGwPassword
						, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
						, strSessionId // strSessionId
									   ////
						, objFlagIsDelete
						////
						, objCampaignCrCode
						, objCampaignCrName
						, objCampainCriteriaType 
						, arrobjDSData
					));

				dtTable0 = mdsResult.Tables[0];
				dtTable1 = mdsResult.Tables[1];
				dtTable2 = mdsResult.Tables[2];

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_CampainCriteria_Save: 
			if (bTest)
			{
				////
				object objFlagIsDelete = "0";
				object objCampaignCrCode = "ThangDepZai";
				object objCampaignCrName = "1000OL";
				object objCampainCriteriaType = "PhichThu";

				DataSet dsData = new DataSet();

				////
				DataTable dtData_Mst_CampainCriteriaScope = new DataTable("Mst_CampainCriteriaScope");
				dtData_Mst_CampainCriteriaScope.Columns.Add("SSGrpCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("SSBrandCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("LevelCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("CampainCritScopeDesc", typeof(object));

				////
				dtData_Mst_CampainCriteriaScope.Rows.Add("NORMALOUTLET", "NORMALOUTLET", "", "LEVEL 11");
				////

				dsData.Tables.Add(dtData_Mst_CampainCriteriaScope);
				object[] arrobjDSData = CmUtils.ConvertUtils.DataSet2Array(dsData);
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_CampainCriteria_Save(
						strGwUserCode // strGwUserCode
						, strGwPassword // strGwPassword
						, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
						, strSessionId // strSessionId
									   ////
						, objFlagIsDelete
						////
						, objCampaignCrCode
						, objCampaignCrName
						, objCampainCriteriaType
						, arrobjDSData
					));

				dtTable0 = mdsResult.Tables[0];
				dtTable1 = mdsResult.Tables[1];
				dtTable2 = mdsResult.Tables[2];

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}

			#region // Mst_CampainCriteria_Save: 
			if (bTest)
			{
				////
				object objFlagIsDelete = "0";
				object objCampaignCrCode = "ThangDepZai";
				object objCampaignCrName = "1000OL";
				object objCampainCriteriaType = "PhichThu";

				DataSet dsData = new DataSet();

				////
				DataTable dtData_Mst_CampainCriteriaScope = new DataTable("Mst_CampainCriteriaScope");
				dtData_Mst_CampainCriteriaScope.Columns.Add("SSGrpCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("SSBrandCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("LevelCode", typeof(object));
				dtData_Mst_CampainCriteriaScope.Columns.Add("CampainCritScopeDesc", typeof(object));

				////
				dtData_Mst_CampainCriteriaScope.Rows.Add("NORMALOUTLET", "NORMALOUTLET", "", "LEVEL 11");
				////

				dsData.Tables.Add(dtData_Mst_CampainCriteriaScope);
				object[] arrobjDSData = CmUtils.ConvertUtils.DataSet2Array(dsData);
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_CampainCriteria_Save(
						strGwUserCode // strGwUserCode
						, strGwPassword // strGwPassword
						, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
						, strSessionId // strSessionId
									   ////
						, objFlagIsDelete
						////
						, objCampaignCrCode
						, objCampaignCrName
						, objCampainCriteriaType
						, arrobjDSData
					));

				dtTable0 = mdsResult.Tables[0];
				dtTable1 = mdsResult.Tables[1];
				dtTable2 = mdsResult.Tables[2];

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion
			#endregion

			#region // Aud_Campaign_Save: 
			if (bTest)
			{
				////
				object objFlagIsDelete = "0";
				object objCampaignCode = "AC20220912.555555"; 
				object objCampaignName = "1000OL";
				object objEffDTimeStart = "2014-06-25";
				object objEffDTimeEnd = "2014-07-08";
				object objReportEndDate = "2";
				object objQtyCheck = "2";
				object objMinIntervalDays = "12";
				object objQtySuccess = "1";
				object objCrtrScoreVerCode = "VER1.0";
				object objCrtrScoreVerAUCode = "VER1.0";
				object objCampaignCrCode = "20140623.170158"; 

				DataSet dsData = new DataSet();

				////
				DataTable dtData_Aud_CampaignDoc = new DataTable("Aud_CampaignDoc");
				dtData_Aud_CampaignDoc.Columns.Add("FilePath", typeof(object)); 

				////
				dtData_Aud_CampaignDoc.Rows.Add("/TEMPFILES/2014623152955/LISTOUTLET1.XLSX");
				dtData_Aud_CampaignDoc.Rows.Add("/TEMPFILES/2014623152955/LISTOUTLET2.XLSX");
				////
				DataTable dtData_Aud_CampaignDBPOSMDtl = new DataTable("Aud_CampaignDBPOSMDtl");
				dtData_Aud_CampaignDBPOSMDtl.Columns.Add("DBCode", typeof(object));
				dtData_Aud_CampaignDBPOSMDtl.Columns.Add("POSMCode", typeof(object));
				dtData_Aud_CampaignDBPOSMDtl.Columns.Add("QtyDeliver", typeof(object));

				////
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN001", "GB", "1000"); 
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN001", "HT", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN001", "KB", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN001", "GH", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN001", "TT", "1000");
				// //
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN002", "GB", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN002", "HT", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN002", "KB", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN002", "GH", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN002", "TT", "1000");
				// //
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN003", "GB", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN003", "HT", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN003", "KB", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN003", "GH", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN003", "TT", "1000");
				// //
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN004", "GB", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN004", "HT", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN004", "KB", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN004", "GH", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN004", "TT", "1000");
				// //
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN005", "GB", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN005", "HT", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN005", "KB", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN005", "GH", "1000");
				dtData_Aud_CampaignDBPOSMDtl.Rows.Add("DBVN005", "TT", "1000");
				////

				dsData.Tables.Add(dtData_Aud_CampaignDoc);
				dsData.Tables.Add(dtData_Aud_CampaignDBPOSMDtl);
				object[] arrobjDSData = CmUtils.ConvertUtils.DataSet2Array(dsData);
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Aud_Campaign_Save(
					strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
					, strSessionId // strSessionId
								   ////
					, objFlagIsDelete
					////
					, objCampaignCode
					, objCampaignCrCode
					, objCrtrScoreVerCode
					, objCrtrScoreVerAUCode
					, objCampaignName
					, objEffDTimeStart
					, objEffDTimeEnd
					, objQtyCheck
					, objQtySuccess
					, objMinIntervalDays
					, objReportEndDate
					, arrobjDSData
					));

				dtTable0 = mdsResult.Tables[0];
				dtTable1 = mdsResult.Tables[1];
				dtTable2 = mdsResult.Tables[2];

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Aud_Campaign_Approve: 
			if (bTest)
			{
				////
				object objCampaignCode = "AC20220912.555555"; 
				object objRemark = "LinhPham";

				 
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Aud_Campaign_Approve(
					strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
					, strSessionId // strSessionId
								   //// 
					, objCampaignCode
					, objRemark
					));

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Aud_Campaign_Cancel: 
			if (bTest)
			{
				////
				object objCampaignCode = "AC20220912.888888";
				object objRemark = "LinhPham";


				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Aud_Campaign_Cancel(
					strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
					, strSessionId // strSessionId
								   //// 
					, objCampaignCode
					, objRemark
					));

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Aud_CampaignDBReceive_Save: 
			if (bTest)
			{
				////
				object objFlagIsDelete = "0";
				object objDBReceiveNo = "555555OL1";
				object objCampaignCode = "AC20220912.555555";

				DataSet dsData = new DataSet();

				////
				DataTable dtData_Aud_CampaignDBReceive = new DataTable("Aud_CampaignDBReceive");
				dtData_Aud_CampaignDBReceive.Columns.Add("DBCode", typeof(object));
				dtData_Aud_CampaignDBReceive.Columns.Add("POSMCode", typeof(object));
				dtData_Aud_CampaignDBReceive.Columns.Add("QtyDBRec", typeof(object));

				////
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN001", "GB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN001", "GH", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN001", "HT", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN001", "KB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN001", "TT", "400");

				////
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN002", "GB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN002", "GH", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN002", "HT", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN002", "KB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN002", "TT", "400");

				////
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN003", "GB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN003", "GH", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN003", "HT", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN003", "KB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN003", "TT", "400");

				////
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN004", "GB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN004", "GH", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN004", "HT", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN004", "KB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN004", "TT", "400");

				////
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN005", "GB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN005", "GH", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN005", "HT", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN005", "KB", "400");
				dtData_Aud_CampaignDBReceive.Rows.Add("DBVN005", "TT", "400");

				//// 

				dsData.Tables.Add(dtData_Aud_CampaignDBReceive);
				object[] arrobjDSData = CmUtils.ConvertUtils.DataSet2Array(dsData);
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Aud_CampaignDBReceive_Save(
					strGwUserCode // strGwUserCode
					, strGwPassword // strGwPassword
					, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid
					, strSessionId // strSessionId
								   ////
					, objFlagIsDelete
					////
					, objDBReceiveNo
					, objCampaignCode
					, arrobjDSData
					));

				dtTable0 = mdsResult.Tables[0];
				dtTable1 = mdsResult.Tables[1];
				dtTable2 = mdsResult.Tables[2];

				if (CmUtils.CMyDataSet.HasError(mdsResult))
				{
					CommonForms.Utils.ProcessMyDS(mdsResult);
				}

				// Pause:
				System.Threading.Thread.Sleep(10);
			}
			#endregion

			#region // Mst_POSMType_Get:
			if (!bTest)
			{
				string strFt_WhereClause = "Mst_POSMType.POSMType = 'PMH'";
				mdsResult = CmUtils.ConvertUtils.Array2DataSet(ws.Mst_POSMType_Get(
					strGwUserCode // strGwUserCode	
					, strGwPassword // strGwPassword	
					, string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), nSeq++) // strTid	
					, strSessionId // strSessionId	
								   ////	
					, strFt_RecordStart  // strFt_RecordStart	
					, strFt_RecordCount  // strFt_RecordCount	
					, strFt_WhereClause  // strFt_WhereClause	
										 ////	
					, "*" // strRt_Cols_Mst_POSMType	
					));

				dtTable0 = mdsResult.Tables[0];
				dtTable1 = mdsResult.Tables[1];
				dtTable2 = mdsResult.Tables[2];

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

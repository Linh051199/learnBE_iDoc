using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;


namespace Demo.Lab.Constants
{
	public class BizMix
	{
		public const string Default_PasswordMask = "*********";
		public const string Default_DBColType = "nvarchar(400)";
		public const int Default_RootYear = 2010;
		public const string DealerCodeRoot = "HO";
        public const string DBCodeRoot = "DVN";
        public const string AreaCodeRoot = "VN";
	}

	public class DataRowMyState
	{
		public const string Added = "ADDED";
		public const string Deleted = "DELETED";
		public const string Detached = "DETACHED";
		public const string Modified = "MODIFIED";
		public const string Unchanged = "UNCHANGED";
	}

	public class Flag
	{
		public const string Active = "1";
		public const string Inactive = "0";
		public const string Auto = "A";
		public const string Manual = "M";
		public const string Yes = Active;
		public const string No = Inactive;
	}

	public class Language
	{
		public const string VI_VN = "VI-VN";
		public const string EN_US = "EN-US";
	}

	public class SeqType
	{
		public const string Id = "ID";
		public const string InsuranceClaimNo = "INSURANCECLAIMNO";
		public const string InsuranceClaimDocCode = "INSURANCECLAIMDOCCODE";
		public const string WorkingRecordNo = "WORKINGRECORDNO";
		public const string LevelCode = "LEVELCODE";
		public const string CampaignCrAwardCode = "CAMPAIGNCRAWARDCODE";
		public const string CampaignCode = "CAMPAIGNCODE";
		public const string CICode = "CICODE";
	}

	public class CalendarType
	{
		public const string WorkingDay = "WORKINGDAY";
		//public const string DepositDuty = "DEPOSITDUTY";

	}

	public class DateTimeSpecial
	{
		public const string DateMin = "1900-01-01";
		public const string DateMax = "2100-01-01";
	}

    public class ValSpecial
    {
        public const double DouValStart = 0;
        public const double DouValEnd = 100;
    }

	public class CampainCriteriaType
	{
		public const string POSM = "POSM";
		public const string StarShop = "STARSHOP";
	}

    public class StarShopTypeNormalOutlet
    {
        public const string SSGrpCode = "NormalOutlet";
        public const string SSBrandCode = "NormalOutlet";
    }

	public class StarShopTypeVirtualOutlet
	{
		public const string SSGrpCode = "VirtualOutlet";
		public const string SSBrandCode = "VirtualOutlet";
	}

	public class CampaignInstEval
	{
		public const string Pending = "X";
		public const string Approve = "1";
		public const string Cancel = "0";
	}

	public class EstimationType
	{
		public const string ExhibitedAward = "EXHIBITEDAWARD";
		public const string RevenueAward = "REVENUEAWARD";
	}

	public class StatusUserScheduleDtl
	{
		public const string Pending = "PENDING";
		public const string Working = "WORKING";
		public const string Approve = "APPROVE";
		public const string Reject = "REJECT";
		public const string Cancel = "CANCEL";
	}

	public class AwardPolicyStatus
	{
		public const string Pending = "PENDING";
		public const string Cancel = "CANCEL";
		public const string Active = "ACTIVE";
		public const string Stop = "STOP";
		public const string Finish = "FINISH";
	}

	public class EvaluationCriteriaAuditUser
	{
		public const string CheatBonusPayment = "CHEATBONUSPAYMENT";
		public const string ReportWrongInfo = "REPORTWRONGINFO";
	}

	public class EvaluationCriteria
	{
		public const string CheatingReport = "CHEATINGREPORT";
	}

	public class Task
	{
		public const string COMMON = "COMMON";
	}

	public class SchInstType
	{
		public const string ImageInput = "IMAGEINPUT";
		public const string ImageOutput = "IMAGEOUTPUT";
	}
    public class ImageType
    {
        public const string Exhibitedinside = "EXHIBITEDINSIDE";
        public const string Exhibitedmaterial = "EXHIBITEDMATERIAL";
        public const string Panoramic = "PANORAMIC";
    }
	public class CampaignStatus
	{
		public const string Pending = "PENDING";
		public const string Cancel = "CANCEL";
		public const string Approve1 = "APPROVE1";
		public const string Approve2 = "APPROVE2";
		public const string Active = "ACTIVE";
		public const string Stop = "STOP";
		public const string Finish = "FINISH";
		public const string Error = "ERROR";
	}

	public class SignBoardsReqStatus
	{
		public const string Pending = "PENDING";
		public const string Approve = "APPROVE";
		public const string Cancel = "CANCEL";
	}

	public class CIStatus
	{
		public const string Pending = "PENDING";
		public const string Approve = "APPROVE";
		public const string Cancel = "CANCEL";
		public const string Pass = "PASS";
		public const string Fail = "FAIL";
	}

	public class OrdStatus
	{
		public const string Pending = "PENDING";
		public const string Approve = "APPROVE";
		public const string Cancel = "CANCEL";
	}

	public class RevOLStatus
	{
		public const string Pending = "PENDING";
		public const string Approve = "APPROVE";
		public const string Cancel = "CANCEL";
	}

	public class EstimationStatus
	{
		public const string Pending = "PENDING";
		public const string Import = "IMPORT";
		public const string Approve = "APPROVE";
		public const string Cancel = "CANCEL";
	}

	public class StatusCm
	{
		public const string Null = "NULL";
		public const string Pending = "PENDING";
		public const string Approve = "APPROVE";
        public const string Cancel = "CANCEL";
		public const string Yes = "1";
		public const string No = "0";
	}
	public class StatusClm
	{
		public const string Pending = "PENDING";
		public const string Approve = "APPROVE";
		public const string Cancel = "CANCEL";
	}
	public class StatusWkContract
	{
		public const string Consult = "CONSULT";
		public const string Negotiate = "NEGOTIATE";
		public const string Signed = "SIGNED";
	}
	public class WebConstants
    {
        public const string ServiceException = "ServiceException";
    }

    public class TableName
    {
        public const string Sys_User = "Sys_User";
        public const string Sys_Group = "Sys_Group";
        public const string Sys_Access = "Sys_Access";
        public const string Sys_Object = "Sys_Object";
        public const string Sys_UserInGroup = "Sys_UserInGroup";
		public const string Mst_ProductGroup = "Mst_ProductGroup";
		public const string Mst_PrdUnit = "Mst_PrdUnit";
		public const string Mst_PrdUnitDtl = "Mst_PrdUnitDtl";
		public const string Mst_Product = "Mst_Product";
		public const string Wk_UserSchedule = "Wk_UserSchedule";
		public const string Wk_UserScheduleDtl = "Wk_UserScheduleDtl";
		public const string Wk_UserScheduleInst = "Wk_UserScheduleInst";
		public const string Mst_AreaMarket = "Mst_AreaMarket";
        public const string Mst_Province = "Mst_Province";
        public const string Mst_District = "Mst_District";
		public const string Mst_Calendar = "Mst_Calendar";
		public const string Mst_Dealer = "Mst_Dealer";
		public const string Lic_Session = "Lic_Session";
        public const string Mst_ChannelDealer = "Mst_ChannelDealer";
        public const string Mst_Channel = "Mst_Channel";
        public const string Mst_Distributor = "Mst_Distributor";
		public const string Ord_Order = "Ord_Order";
		public const string Ord_OrderDtl = "Ord_OrderDtl";
        public const string Rpt_SalesSM_01 = "Rpt_SalesSM_01";
        public const string Rpt_SalesDL_01 = "Rpt_SalesDL_01";
        public const string Rpt_DensityAM_01 = "Rpt_DensityAM_01";
        public const string Rpt_DensityChannel_01 = "Rpt_DensityChannel_01";
        public const string Rpt_UserSchedule_01 = "Rpt_UserSchedule_01";
        public const string Prd_FGType = "Prd_FGType";
        public const string Prd_Brand = "Prd_Brand";
        public const string Mst_POSMType = "Mst_POSMType";
        public const string Mst_POSM = "Mst_POSM";
        public const string Mst_Outlet = "Mst_Outlet";
        public const string Mst_StarShopBrand = "Mst_StarShopBrand";
        public const string Mst_StarShopGroup = "Mst_StarShopGroup";
        public const string Mst_StarShopHist = "Mst_StarShopHist";
        public const string Mst_StarShopType = "Mst_StarShopType";
        public const string Mst_EvaluationCriteriaGroup = "Mst_EvaluationCriteriaGroup";
        public const string Mst_EvaluationCriteria = "Mst_EvaluationCriteria";
        public const string Mst_CriteriaScoreVersion = "Mst_CriteriaScoreVersion";
        public const string Mst_CriteriaScoreVersionAuditUser = "Mst_CriteriaScoreVersionAuditUser";
        public const string Mst_CriteriaScore = "Mst_CriteriaScore";
        public const string Mst_ActionType = "Mst_ActionType";
        public const string Mst_AwardPolicy = "Mst_AwardPolicy";
        public const string OL_SignBoardsHist = "OL_SignBoardsHist";
        public const string OL_SignBoardsRequest = "OL_SignBoardsRequest";
        public const string OL_SignBoardsPosition = "OL_SignBoardsPosition";
        public const string OL_SignBoardsType = "OL_SignBoardsType";
        public const string Aud_Campaign = "Aud_Campaign";
        public const string Aw_Estimation = "Aw_Estimation";
        public const string Aw_EstimationCampaignOL = "Aw_EstimationCampaignOL";
        public const string Aw_EstimationRevenueOL = "Aw_EstimationRevenueOL";
        public const string Aud_CampaignDBDtl = "Aud_CampaignDBDtl";
        public const string Aud_CampaignDBPOSMDtl = "Aud_CampaignDBPOSMDtl";
        public const string Aud_CampaignDBReceive = "Aud_CampaignDBReceive";
        public const string Aud_CampaignDBReceiveDtl = "Aud_CampaignDBReceiveDtl";
        public const string Aud_CampaignOLDtl = "Aud_CampaignOLDtl";
        public const string Aud_CampaignOLPOSMDtl = "Aud_CampaignOLPOSMDtl";
        public const string Aud_CampaignDoc = "Aud_CampaignDoc";
		public const string Aud_EvalAuditUser = "Aud_EvalAuditUser";
        public const string Aud_CampaignDtl = "Aud_CampaignDtl";
        public const string Aud_CampaignInst = "Aud_CampaignInst";
        public const string Aud_CampaignInstDtl = "Aud_CampaignInstDtl";
        public const string Aud_CampaignInstEval = "Aud_CampaignInstEval";
        public const string Aud_CampaignOLReceive = "Aud_CampaignOLReceive";
        public const string Aud_CampaignOLReceiveDtl = "Aud_CampaignOLReceiveDtl";
        public const string Aud_CampaignOLRevenue = "Aud_CampaignOLRevenue";
        public const string Aud_CampaignPOSMDtl = "Aud_CampaignPOSMDtl";
        public const string Aud_CampaignRootInst = "Aud_CampaignRootInst";
		public const string Mst_CampainCriteria = "Mst_CampainCriteria";
		public const string Mst_CampainCriteriaScope = "Mst_CampainCriteriaScope";
		public const string Mst_CampainCriteriaAward = "Mst_CampainCriteriaAward";
		public const string Mst_CampainCriteriaAwardDtl = "Mst_CampainCriteriaAwardDtl";
		public const string Mst_CampainExhibitedPOSM = "Mst_CampainExhibitedPOSM";
        public const string Rev_RevenueOL = "Rev_RevenueOL";
        public const string Rev_RevenueOLDtl = "Rev_RevenueOLDtl";
		public const string Aud_EvalAuditUser_Pivot = "Aud_EvalAuditUser_Pivot";
    }
	public class ClaimDocTypeName
	{
		public const string DOCUMENT_ALL = "DOCUMENT_ALL";
	}
}

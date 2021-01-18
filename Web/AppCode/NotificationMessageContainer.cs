using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.AppCode
{
    public class NotificationMessageContainer
    {
        #region "COSHH Related Message"
        public const string COSHH_ADDED_SUCCESS = "COSHH assessment created.";
        public const string COSHH_EDIT_SUCCESS = "COSHH assessment updated.";
        public const string COSHH_DELETED_SUCCESS = "COSHH assessment has been deleted.";
        public const string COSHH_EXTRA_PDF_DELETED_SUCCESS = "PDF attachment has been deleted.";
        #endregion

        #region "Project COSHH Related Message"
        public const string EXISTING_COSHH_CHECK_FOR_PROJECT = "You already added this COSHH for this project.";
        public const string COSHH_REMOVE_FROM_PROJECT = "COSHH assessment has been removed from this project.";
        #endregion

        #region "COSHH Activity Related Message"
        public const string COSHH_ACTIVITY_ADDED_SUCCESS = "COSHH activity created.";
        public const string COSHH_ACTIVITY_EDIT_SUCCESS = "COSHH activity updated.";
        public const string COSHH_ACTIVITY_DELETED_SUCCESS = "COSHH activity has been deleted.";
        #endregion

        #region "Project Related Message"
        public const string PROJECT_ADDED_SUCCESS = "New RAMS document has been created.";
        public const string PROJECT_EDIT_SUCCESS = "RAMs  document has been updated.";
        public const string PROJECT_DELETED_SUCCESS = "RAMS document has been deleted.";
        public const string EXTRA_PDF_DELETED_SUCCESS = "PDF attachment has been deleted.";
        public const string PROJECT_ACTIVITY_EDIT_SUCCESS = "Risk assessment has been updated.";
        public const string PROJECT_SEQUENCE_OF_WORKS_EDIT_SUCCESS = "Sequence of Works has beeen updated.";
        public const string PROJECT_EXTRA_PDF_DELETED_SUCCESS = "PDF attachment has been deleted.";
        #endregion


        #region "Captcha Related Message"
        public const string CAPTHCA_SECRET_PARAM_MISSING = "The secret parameter is missing.";
        public const string CAPTHCA_SECRET_PARAM_INVALID = "The secret parameter is invalid or malformed.";
        public const string CAPTHCA_INCORRECT_MESSAGE = "Please give captcha correctly.";
        public const string CAPTHCA_RESPONSE_PARAM_INVALID = "The response parameter is invalid or malformed.";
        public const string CAPTHCA_DEFAULT_ERROR_MESSAGE = "Error occured. Please try again.";
        #endregion


        #region "Project Hazard Related Message"
        public const string PROJECT_HAZARD_ADDED_SUCCESS = "Risk assessment has been created.";
        public const string PROJECT_HAZARD_EDIT_SUCCESS = "Risk assessment has been updated.";
        public const string PROJECT_HAZARD_DELETED_SUCCESS = "Risk assessment has been deleted.";
        #endregion


        #region Vibration noise Tools Related Message"

        public const string VIBRATION_NOISE_TOOLS_ADDED_SUCCESS = "Equipment successfully added.";
        public const string VIBRATION_NOISE_TOOLS_EDIT_SUCCESS = "Equipment successfully updated.";
        public const string VIBRATION_NOISE_TOOLS_DELETE_SUCCESS = "Equipment has been deleted.";

        #endregion 

        #region Vibration Noise Assessment Related Message"

        public const string VIBRATION_NOISE_ASSESSMENT_ADDED_SUCCESS = "Hand arm vibration assessment successfully added.";
        public const string VIBRATION_NOISE_ASSESSMENT_EDIT_SUCCESS = "Hand arm vibration assessment successfully updated.";
        public const string VIBRATION_NOISE_ASSESSMENT_DELETE_SUCCESS = "Hand arm vibration assessment has been deleted.";

        #endregion

        #region Manual Handling Risk Assessment Related Message"

        public const string MANUAL_HANDLING_ASSESSMENT_ADDED_SUCCESS = "Manual handling risk assessment successfully added.";
        public const string MANUAL_HANDLING_ASSESSMENT_EDIT_SUCCESS = "Manual handling risk assessment successfully updated.";
        public const string MANUAL_HANDLING_ASSESSMENT_DELETE_SUCCESS = "Manual handling risk assessment has been deleted.";

        #endregion

        #region "Forms & Records Related Message"
        public const string FORMS_AND_RECORDS_ADDED_SUCCESS = "Form and records document has been created.";
        public const string FORMS_AND_RECORDS_EDIT_SUCCESS = "Form and records document has been updated.";
        public const string FORMS_AND_RECORDS_DELETED_SUCCESS = "Form and records document has been deleted.";
        #endregion

        #region "Trial User Message"
        public const string USER_TRIAL_EXPIRED = "Your trial period has been expired.";
        public const string USER_TRIAL_NOT_START = "Your trial period does not start yet.";
        #endregion

        #region "Company User Message"
        public const string COMPANY_LICENSE_EXPIRED = "Your license period has been expired.";
        public const string COMPANY_LICENSE_NOT_START = "Your license period does not start yet.";
        #endregion

        #region "Email Send Message"
        public const string EMAIL_SEND_SUCCESS = "Email send successfully done.";
        public const string EMAIL_SEND_FAILED = "Email not send.";
        #endregion

        #region "Package Upgrade Related Message"
        public const string UPGRADED_PACKAGE_MESSAGE = "Your RAMS package plan has been upgraded.";
        public const string SAME_PACKAGE_MESSAGE = "You are in the same RAMS package plan.";
        public const string DOWNGRADED_PACKAGE_MESSAGE = "This RAMS package is downgraded for you.";
        public const string HAS_NO_STRIPE_PLAN_MESSAGE = "You are created by admin.Please contact with RAMS";
        public const string DOWNGRADE_PACKAGE_MESSAGE = "Your account will be downgraded in your next payment cycle.";
        #endregion

        #region "Paid User expired message"

        public const string PAID_USER_EXPIRED = "Your period has been expired.";
        #endregion

        #region "Super Admin Related Message"
        public const string ADMIN_HAZARD_ADDED_SUCCESS = "Hazard category successfully added.";
        public const string ADMIN_HAZARD_EDIT_SUCCESS = "Hazard category successfully updated.";
        public const string ADMIN_HAZARD_DELETE_SUCCESS = "Hazard category has been deleted.";


        public const string ADMIN_HAZARD_GROUP_ADDED_SUCCESS = "Hazard group successfully added.";
        public const string ADMIN_HAZARD_GROUP_EDIT_SUCCESS = "Hazard group successfully updated.";
        public const string ADMIN_HAZARD_GROUP_DELETE_SUCCESS = "Hazard group has been deleted.";


        public const string ADMIN_METHOD_STATEMENT_GROUP_ADDED_SUCCESS = "Method statement group successfully added.";
        public const string ADMIN_METHOD_STATEMENT_GROUP_EDIT_SUCCESS = "Method statement group successfully updated.";
        public const string ADMIN_METHOD_STATEMENT_GROUP_DELETE_SUCCESS = "Method statement group has been deleted.";


        public const string ADMIN_LEGISLATION_ADDED_SUCCESS = "Legislation successfully added.";
        public const string ADMIN_LEGISLATION_EDIT_SUCCESS = "Legislation successfully updated.";
        public const string ADMIN_LEGISLATION_DELETE_SUCCESS = "Legislation has been deleted.";


        public const string ADMIN_HAZARD_STATEMENT_ADDED_SUCCESS = "Hazard statement successfully added.";
        public const string ADMIN_HAZARD_STATEMENT_EDIT_SUCCESS = "Hazard statement successfully updated.";
        public const string ADMIN_HAZARD_STATEMENT_DELETE_SUCCESS = "Hazard statement has been deleted.";


        public const string ADMIN_PRECAUTION_STATEMENT_ADDED_SUCCESS = "Precaution statement successfully added.";
        public const string ADMIN_PRECAUTION_STATEMENT_EDIT_SUCCESS = "Precaution statement successfully updated.";
        public const string ADMIN_PRECAUTION_STATEMENT_DELETE_SUCCESS = "Precaution statement has been deleted.";


        public const string ADMIN_ACTIVITY_ADDED_SUCCESS = "Substance activity successfully added.";
        public const string ADMIN_ACTIVITY_EDIT_SUCCESS = "Substance activity successfully updated.";
        public const string ADMIN_ACTIVITY_DELETE_SUCCESS = "Substance activity has been deleted.";


        public const string ADMIN_TOOLBOX_TALKS_ADDED_SUCCESS = "Toolbox talks successfully added.";
        public const string ADMIN_TOOLBOX_TALKS_EDIT_SUCCESS = "Toolbox talks successfully updated.";
        public const string ADMIN_TOOLBOX_TALKS_DELETE_SUCCESS = "Toolbox talks has been deleted.";


        public const string ADMIN_GHS_ADDED_SUCCESS = "GHS Image successfully added.";
        public const string ADMIN_GHS_EDIT_SUCCESS = "GHS Image successfully updated.";
        public const string ADMIN_GHS_DELETE_SUCCESS = "GHS Image has been deleted.";

        public const string ADMIN_IMAGE_ADDED_SUCCESS = "Image successfully added.";
        public const string ADMIN_IMAGE_EDIT_SUCCESS = "Image successfully updated.";
        public const string ADMIN_IMAGE_DELETE_SUCCESS = "Image has been deleted.";


        public const string ADMIN_RISK_ASSESSMENT_ADDED_SUCCESS = "Risk assessment successfully added.";
        public const string ADMIN_RISK_ASSESSMENT_EDIT_SUCCESS = "Risk assessment successfully updated.";
        public const string ADMIN_RISK_ASSESSMENT_DELETE_SUCCESS = "Risk assessment has been deleted.";


        public const string ADMIN_TRADE_ADDED_SUCCESS = "Trade group successfully added.";
        public const string ADMIN_TRADE_EDIT_SUCCESS = "Trade group successfully updated.";
        public const string ADMIN_TRADE_DELETE_SUCCESS = "Trade group has been deleted.";


        #region "Admin Trade Activity Related Message"
        public const string ADMIN_TRADE_ACTIVITY_ADDED_SUCCESS = "Trade activity has been created.";
        public const string ADMIN_TRADE_ACTIVITY_EDITED_SUCCESS = "Trade activity has been updated.";
        public const string ADMIN_TRADE_ACTIVITY_DELETE_SUCCESS = "Trade activity has been deleted.";
        public const string ADMIN_SIDE_ACTIVITY_ORDER_UPDATE_SUCCESS = "Trade activities order updated.";
        #endregion

        #region "Admin Trade Activity Sequence Related Message"
        public const string ADMIN_TRADE_ACTIVITY_SEQUENCE_ADDED_SUCCESS = "Trade activity sequence has been created.";
        public const string ADMIN_TRADE_ACTIVITY_SEQUENCE_EDITED_SUCCESS = "Trade activity sequence has been updated.";
        public const string ADMIN_TRADE_ACTIVITY_SEQUENCE_DELETE_SUCCESS = "Trade activity sequence has been deleted.";
        public const string ADMIN_SIDE_ACTIVITY_SEQUENCE_ORDER_UPDATE_SUCCESS = "Trade activity sequences order updated.";
        #endregion



        //public const string ADMIN_TRADE_DELETE_SUCCESS = "Trade has been deleted.";
        //public const string ADMIN_TRADE_DELETE_SUCCESS = "Trade has been deleted.";


        public const string ADMIN_USER_ADDED_SUCCESS = "User has been successfully created.";
        public const string ADMIN_USER_PERSONAL_INFO_EDIT_SUCCESS = "User personal information has been updated.";
        public const string ADMIN_USER_PASSWORD_EDIT_SUCCESS = "User password has been updated.";
        public const string ADMIN_USER_COMPANY_INFO_EDIT_SUCCESS = "User company information has been updated.";
        public const string ADMIN_USER_ACTIVITY_INFO_EDIT_SUCCESS = "User trade group has been updated.";
        public const string ADMIN_USER_DELETE_SUCCESS = "User has been deleted.";
        public const string ADMIN_USER_IP_ADDRESS_INFO_EDIT_SUCCESS = "User has been successfully blocked with all IP address.";


        public const string ADMIN_SUBTANCE_IMAGE_ADDED_SUCCESS = "Substance Image successfully added.";
        public const string ADMIN_SUBTANCE_IMAGE_EDIT_SUCCESS = "Substance Image successfully updated.";
        public const string ADMIN_SUBTANCE_IMAGE_DELETE_SUCCESS = "Substance Image has been deleted.";

        public const string ADMIN_BLOCK_NEW_USER_IP_SUCCESS = "User IP successfully added in  block list.";

        public const string COMPANY_ADMIN_ADDED_SUCCESS = "Company admin has been successfully created.";
        public const string COMPANY_ADMIN_EDIT_SUCCESS = "Company admin has been successfully updated.";
        public const string COMPANY_ADMIN_DELETE_SUCCESS = "Company admin has been deleted.";


        public const string COMPANY_USER_ADDED_SUCCESS = "Company user has been successfully created.";
        public const string COMPANY_USER_EDIT_SUCCESS = "Company user has been successfully updated.";
        public const string COMPANY_USER_DELETE_SUCCESS = "Company user has been deleted.";
        #endregion
        //#region "Project Trade, Sequence, SubSequence Related Message"
        //public const string PROJECT_ADDED_SUCCESS = "Risk Assessment successfully added.";
        //#endregion
    }
}

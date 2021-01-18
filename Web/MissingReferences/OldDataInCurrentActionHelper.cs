using Services.Domain.UserBilling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.MissingReferences
{
    public class OldDataInCurrentActionHelper
    {
        private static BillingAmountHistoryBySellerId _billingAmountHistoryBySellerId;
        public static BillingAmountHistoryBySellerId BillingAmountHistoryBySellerIdForAction
        {
            internal set
            {
                _billingAmountHistoryBySellerId = value;
            }
            get
            {
                try
                {
                    if (_billingAmountHistoryBySellerId != null)
                        return _billingAmountHistoryBySellerId;
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GUKV
{
    /// <summary>
    /// Summary description for RentingAgreementValidator
    /// </summary>
    public class RentingAgreementValidator : ValidatorBase
    {
        public RentingAgreementValidator()
            : this(null, string.Empty)
        {

        }

        public RentingAgreementValidator(System.Web.UI.Page curpage, string validationGroup/*, Dictionary<string, Control> ctls*/)
        {
            page = curpage;
            //controls = ctls;

            AddControl("EditAgreementNum", validationGroup, "agreement_num", "Необхідно вказати номер договору оренди");
            AddControl("EditAgreementDate", validationGroup, "agreement_date", "Необхідно вказати дату укладання договору");
            AddControl("EditStartDate", validationGroup, "rent_start_date", "Необхідно вказати дату початку оренди");
            AddControl("ComboPaymentType", validationGroup, "payment_type_id", "Необхідно вибрати вид оплати за договором");
        }
    }
}
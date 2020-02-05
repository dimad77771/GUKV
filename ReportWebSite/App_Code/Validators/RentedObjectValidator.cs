using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GUKV
{
    /// <summary>
    /// Summary description for RentedObjectValidator
    /// </summary>
    public class RentedObjectValidator : ValidatorBase
    {
        public RentedObjectValidator()
            : this(null, string.Empty)
        {

        }

        public RentedObjectValidator(System.Web.UI.Page curpage, string validationGroup/*, Dictionary<string, Control> ctls*/)
        {
            page = curpage;
            //controls = ctls;

            AddControl("EditAgrNum", validationGroup, "agreement_num", "Необхідно вказати номер договору орендування");
            AddControl("EditAgrDate", validationGroup, "agreement_date", "Необхідно вказати дату укладання договору");
            AddControl("EditAgrStartDate", validationGroup, "rent_start_date", "Необхідно вказати дату початку оренди");
        }
    }
}
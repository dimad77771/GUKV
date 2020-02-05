using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GUKV
{
    /// <summary>
    /// Summary description for BalanceObjectValidator
    /// </summary>
    public class BalansObjectValidator : ValidatorBase
    {
        public BalansObjectValidator()
            : this(null, string.Empty)
        {

        }

        public BalansObjectValidator(System.Web.UI.Page curpage, string validationGroup/*, Dictionary<string, Control> ctls*/)
        {
            page = curpage;
            //controls = ctls;

            AddControl("EditBuildingSqrTotal", validationGroup, "sqr_total", "Необхідно вказати загальну площу будинку");
            AddControl("ComboObjStatus", validationGroup, "obj_status_id", "Характеристики об'єкту: необхідно вказати поточне використання приміщення");
            AddControl("EditObjBtiReestrNo", validationGroup, "reestr_no", "Характеристики об'єкту: необхідно вказати інвентарний номер об'єкту");
            AddControl("EditObjSqrTotal", validationGroup, "sqr_total", "Площа об'єкту: необхідно вказати площу нежилих приміщень на балансі");
            AddControl("ComboOwnershipDocKind", validationGroup, "ownership_doc_type", "Документ, що встановлює право користування об’єктом: необхідно вибрати тип документу");
            AddControl("EditOwnershipDocNum", validationGroup, "ownership_doc_num", "Документ, що встановлює право користування об’єктом: необхідно вказати номер документу");
            AddControl("EditOwnershipDocDate", validationGroup, "ownership_doc_date", "Документ, що встановлює право користування об’єктом: необхідно вказати дату документу");
            AddControl("ComboBalansDocKind", validationGroup, "balans_doc_type", "Документ, що підтверджує передачу об’єкту на баланс: необхідно вибрати тип документу");
            AddControl("EditBalansDocNum", validationGroup, "balans_doc_num", "Документ, що підтверджує передачу об’єкту на баланс: необхідно вказати номер документу");
            AddControl("EditBalansDocDate", validationGroup, "balans_doc_date", "Документ, що підтверджує передачу об’єкту на баланс: необхідно вказати дату документу");
        }
    }
}
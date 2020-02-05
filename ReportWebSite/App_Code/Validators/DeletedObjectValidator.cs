using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GUKV
{
    /// <summary>
    /// Summary description for DeletedObjectValidator
    /// </summary>
    public class DeletedObjectValidator : ValidatorBase
    {
        public DeletedObjectValidator()
            : this(null, string.Empty)
        {

        }

        public DeletedObjectValidator(System.Web.UI.Page curpage, string validationGroup/*, Dictionary<string, Control> ctls*/)
        {
            page = curpage;
            //controls = ctls;

            AddControl("ComboVidchType", validationGroup, "vidch_type_id", "Необхідно вибрати спосіб відчуження об'єкту");
            AddControl("EditVidchSquare", validationGroup, "sqr_total", "Необхідно вказати площу відчуження");
            AddControl("EditObjBtiReestrNoDeleted", validationGroup, "reestr_no", "Необхідно вказати інвентарний номер об'єкту");
            AddControl("ComboVidchDocKind", validationGroup, "vidch_doc_type", "Документ, що підтверджує відчуження об'єкту: необхідно вибрати тип документу");
            AddControl("EditVidchDocNum", validationGroup, "vidch_doc_num", "Документ, що підтверджує відчуження об'єкту: необхідно вказати номер документу");
            AddControl("EditVidchDocDate", validationGroup, "vidch_doc_date", "Документ, що підтверджує відчуження об'єкту: необхідно вказати дату документу");
        }
    }
}
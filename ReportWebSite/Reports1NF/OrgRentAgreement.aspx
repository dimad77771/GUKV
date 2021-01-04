<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgRentAgreement.aspx.cs" Inherits="Reports1NF_OrgRentAgreement"
    MasterPageFile="~/NoMenu.master" Title="Картка Договору Оренди" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
<%@ Register src="ReportCommentViewer.ascx" tagname="ReportCommentViewer" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<style type="text/css">
    .SpacingPara
    {
        font-size: 10px;
        margin-top: 4px;
        margin-bottom: 0px;
        padding-top: 0px;
        padding-bottom: 0px;
    }

    .classPanelCollectionDebitKvart
    {
        margin-left:4px;
    }

    .editForm999 .dxgvEditFormTable_DevEx 
    {
         width:1800px !important;
    }

    .dxflGroup_DevEx td.dxflCaptionCellSys
    {
        width: 550px !important;
        min-width: 550px !important;
    }

    .dxflCaptionCell_DevEx
    {
        white-space: normal !important;
    }

    a.dxbButton_DevEx {
        margin:0px !important;
    }


</style>

    <script type="text/javascript" language="javascript">
		var editFreeSquareMode = <%= EditFreeSquareMode.ToString().ToLower() %>;

    // <![CDATA[
        var period;
        document.addEventListener("DOMContentLoaded", ready);

        var IsReadOnlyForm = <%=IsReadOnlyForm.ToString().ToLower() %>;
        console.log("IsReadOnlyForm", IsReadOnlyForm);

        var argReportID = <%=ReportID.ToString()%>;
        var argRentAgreementID = <%=RentAgreementID.ToString()%>;
        console.log("argReportID", argReportID);
		console.log("argRentAgreementID", argRentAgreementID);

        var avance_plat_0 = 0;
        function ready(event) {
            HidePnl();
            setTimeout(function () {
				InitCalcCollectionDebtZvit();
            }, 100);
            if (IsReadOnlyForm) {
                clientBtnAddPaymentDocument.SetEnabled(false);
            }
        }

		function InitCalcCollectionDebtZvit() {
			console.log("InitCalcCollectionDebtZvit");
			if (edit_debtkvart_0.validFormattedNumberRegExp != null) {
				avance_plat_0 = round(nn(edit_avance_plat.GetValue()));
				CalcCollectionDebtZvit();
			} else {
				setTimeout(InitCalcCollectionDebtZvit, 100);
			}
		}

		function afterCPMainPanelCallback(event) {
			setTimeout(CalcCollectionDebtZvit, 100);
		}

        function HidePnl() {
            if (document.getElementById('<%=PaymentForm.ClientID %>' + '_NeededPeriodCombo').innerHTML != "") {
                period = document.getElementById('<%=PaymentForm.ClientID %>' + '_NeededPeriodCombo').innerHTML;
            }
            document.getElementById('<%=PaymentForm.ClientID %>' + '_NeededPeriodCombo').style.display = "none";
        }

        var lastFocusedControlId = "";
        var lastFocusedControlTitle = "";

        function EnableCollectionControls(s, e) {
            var isEnable = CheckNoDebt.GetChecked();

            EditCollectionDebtTotal.SetValue(null);
            EditCollectionDebtZvit.SetValue(null);
            EditCollectionDebt3Month.SetValue(null);
            EditCollectionDebt12Month.SetValue(null);
            EditCollectionDebt3Years.SetValue(null);
            EditCollectionDebtOver3Years.SetValue(null);
			Edit_avance_debt.SetValue(null);

            EditCollectionDebtVMezhahVitrat.SetValue(null);
            EditCollectionDebtSpysano.SetValue(null);
            EditCollectionNumZahodivTotal.SetValue(null);
            EditCollectionNumZahodivZvit.SetValue(null);
            EditCollectionNumPozovTotal.SetValue(null);
            EditCollectionNumPozovZvit.SetValue(null);
            EditCollectionPozovZadovTotal.SetValue(null);
            EditCollectionPozovZadovZvit.SetValue(null);
            EditCollectionPozovVikonTotal.SetValue(null);
            EditCollectionPozovVikonZvit.SetValue(null);
            EditCollectionDebtPayedTotal.SetValue(null);
            EditCollectionDebtPayedZvit.SetValue(null);

        }

        function round(arg) {
			return Math.round(arg * 100) / 100;
		}

        function CalcDebt() {
			var vv1 = round(nn(edit_debtkvart_0.GetValue()));
            EditCollectionDebt3Month.SetValue(vv1);

			var vvz = round( nn(edit_debtkvart_0.GetValue()) + nn(edit_debtkvart_1.GetValue()) + nn(edit_debtkvart_2.GetValue()) );
			EditCollectionDebtZvit.SetValue(vvz);

			var vv2 = round(nn(edit_debtkvart_1.GetValue()) + nn(edit_debtkvart_2.GetValue()) + nn(edit_debtkvart_3.GetValue()));
			EditCollectionDebt12Month.SetValue(vv2);

            var vv3 =
				round(
                    nn(edit_debtkvart_4.GetValue()) + nn(edit_debtkvart_5.GetValue()) + nn(edit_debtkvart_6.GetValue()) + nn(edit_debtkvart_7.GetValue()) +
                    nn(edit_debtkvart_8.GetValue()) + nn(edit_debtkvart_9.GetValue()) + nn(edit_debtkvart_10.GetValue()) + nn(edit_debtkvart_11.GetValue())
                );
            EditCollectionDebt3Years.SetValue(vv3);

			var vv4 = round(nn(edit_debtkvart_13.GetValue()));
			EditCollectionDebtOver3Years.SetValue(vv4);

			var sval = round(vv1 + vv2 + vv3 + vv4);
            EditCollectionDebtTotal.SetValue(sval);

			var vv99 = round(nn(edit_avance_plat.GetValue()) - nn(avance_saldo.GetValue()) - nn(avance_paymentnar.GetValue()));
            if (vv99 <= 0) vv99 = null;
			Edit_avance_debt.SetValue(vv99);

            /*
            var DebtZvit = EditCollectionDebtZvit.GetValue();
            var D3Month = EditCollectionDebt3Month.GetValue();
            var D12Month = EditCollectionDebt12Month.GetValue();
            var D3Years = EditCollectionDebt3Years.GetValue();
            var DO3Years = EditCollectionDebtOver3Years.GetValue();
            var sval = D3Month + D12Month + D3Years + DO3Years;
            EditCollectionDebtTotal.SetValue(sval);
            */
        }

        function nn(arg) {
            if (arg == null) return 0;
            return arg;
        }

		function getPaymentNarah() {
            var v1 = clEditPaymentNarah_orndpymnt.GetValue();
            var v2 = edit_znyato_nadmirno_narah.GetValue();
			var v3 = edit_znyato_from_avance.GetValue();
            if (v1 == null) v1 = 0;
            if (v2 == null) v2 = 0;
			if (v3 == null) v3 = 0;
			return v1 - v2 - v3;
		}

        function CalcCollectionDebtZvit() {
            console.log("run CalcCollectionDebtZvit()");

            var use_calc_debt = edit_use_calc_debt.GetChecked();
			//EditCollectionDebtZvit.SetEnabled(!use_calc_debt);
			//EditCollectionDebt3Month.SetEnabled(!use_calc_debt);
            //EditCollectionDebt12Month.SetEnabled(!use_calc_debt);
			//edit_return_orend_payed.SetEnabled(!use_calc_debt);
			edit_debtkvart_0.SetEnabled(!use_calc_debt);

			if (use_calc_debt) {
				var v1 = getPaymentNarah();                         //"Нараховано орендної плати за звітний період, грн. (без ПДВ)"
                var v2 = Zvit_orndpymnt.GetValue();                 //"Надходження орендної плати з нарахованої за звітний період"
                var v3 = clEditPaymentSaldo_orndpymnt.GetValue();   //"Сальдо (переплата) на початок року (незмінна впродовж року величина), грн. (без ПДВ)"
                var v4 = edit_debtkvart_1.GetValue();               //"2020, 1кв.:"
				var v5 = edit_debtkvart_2.GetValue();               //"2020, 2кв.:"
                if (v1 == null) v1 = 0;
                if (v2 == null) v2 = 0;
                if (v3 == null) v3 = 0;
                if (v4 == null) v4 = 0;
				if (v5 == null) v5 = 0;

				var rez = v1 - v2 - v3 - v4 - v5;
                rez = Math.round(rez * 100) / 100;
                if (rez <= 0) rez = null;
				edit_debtkvart_0.SetValue(rez);

                var reportingPeriodInfo = ReportingPeriodCombo.GetItem(ReportingPeriodCombo.GetSelectedIndex());
                if (reportingPeriodInfo != null) {
                    var reportingPeriodText = reportingPeriodInfo.text;
                    var s1 = "3 місяці ";
                    if (reportingPeriodText.substring(0, s1.length) == s1) {
                        EditCollectionDebt3Month.SetValue(rez);
                        EditCollectionDebt12Month.SetValue(null);
                    } else {
                        EditCollectionDebt3Month.SetValue(null);
                        EditCollectionDebt12Month.SetValue(rez);
                    }
                }

                CalcDebt();
                CalcEditReturnOrendPayed();
            } else {
                CalcDebt();
				CalcEditReturnOrendPayed();
            }

            Calc_tr_alert_edit_avance_plat();
        }

        function Calc_tr_alert_edit_avance_plat() {
            //var avance_plat_now = round(nn(edit_avance_plat.GetValue()));
            //if (avance_plat_now > 0 && avance_plat_0 == 0) {
			//	$("#tr_alert_edit_avance_plat").show();
            //} else {
			//	$("#tr_alert_edit_avance_plat").hide();
			//}
        }

		function CalcEditReturnOrendPayed() {
			var v1 = Received_orndpymnt.GetValue();                     //"Надходження орендної плати за звітний період, всього, грн. (без ПДВ)"
            var v2 = Zvit_orndpymnt.GetValue();                         //"- у тому числі, з нарахованої за звітний період (без боргів та переплат)"	
            var v3 = avance_paymentnar.GetValue();                      //"- у тому числі, з нарахованої авансової орендної плати, грн."
			var v4 = clEditPaymentOldDebtsPayed_orndpymnt.GetValue();   //"Погашення заборгованості минулих періодів, грн."
			if (v1 == null) v1 = 0;
			if (v2 == null) v2 = 0;
            if (v3 == null) v3 = 0;
			if (v4 == null) v4 = 0;

			var rez = v1 - v2 - v3 - v4;
            rez = Math.round(rez * 100) / 100;
			if (rez <= 0) rez = null;
            edit_return_orend_payed.SetValue(rez);


            var v11 = round(nn(edit_return_orend_payed.GetValue()));
			var v12 = round(nn(clEditPaymentSaldo_orndpymnt.GetValue()));
            var rez2 = round(v11 + v12);
            if (rez2 <= 0) rez2 = null;
			edit_total_pereplata.SetValue(rez2);
        }

        function CheckTotalPaidSum() {
			var v1 = Received_orndpymnt.GetValue();                     //"Надходження орендної плати за звітний період, всього, грн. (без ПДВ)"
			var v2 = Zvit_orndpymnt.GetValue();                         //"- у тому числі, з нарахованої за звітний період (без боргів та переплат)"	
			var v3 = avance_paymentnar.GetValue();                      //"- у тому числі, з нарахованої авансової орендної плати, грн."
			var v4 = clEditPaymentOldDebtsPayed_orndpymnt.GetValue();   //"Погашення заборгованості минулих періодів, грн."
			if (v1 == null) v1 = 0;
			if (v2 == null) v2 = 0;
			if (v3 == null) v3 = 0;
			if (v4 == null) v4 = 0;

			var rez = v1 - v2 - v3 - v4;
			rez = Math.round(rez * 100) / 100;
            if (rez < 0) {
				document.getElementById('valError').style.display = '';
				document.getElementById('valError').innerHTML = 'Надходження орендної плати за звітний період менше ніж сума "- у тому числі"';
				return false;
            }
			return true;
		}


        
		function CalcEditReturnOrendPayed__old() {
			var v1 = getPaymentNarah();                                  //"Нараховано орендної плати за звітний період, грн. (без ПДВ)"
			var v2 = clEditPaymentSaldo_orndpymnt.GetValue();            //"Сальдо (переплата) на початок року (незмінна впродовж року величина), грн. (без ПДВ)"	
			//var v3 = Zvit_orndpymnt.GetValue();                          //"- у тому числі, з нарахованої за звітний період (без боргів та переплат)"	
			var v3 = Received_orndpymnt.GetValue();                          //"Надходження орендної плати за звітний період, всього, грн. (без ПДВ)"	
            var v4 = EditCollectionDebtTotal.GetValue();                 //"Загальна заборгованість по орендній платі"
            var v5 = clEditPaymentOldDebtsPayed_orndpymnt.GetValue();    //"Погашення заборгованості минулих періодів, грн."
			var v6 = avance_paymentnar.GetValue();    //"- у тому числі, з нарахованої авансової орендної плати, грн."
			if (v1 == null) v1 = 0;
			if (v2 == null) v2 = 0;
            if (v3 == null) v3 = 0;
            if (v4 == null) v4 = 0;
            if (v5 == null) v5 = 0;
			if (v6 == null) v6 = 0;

			var rez = v2 + v3 - v1 - v4 - v5 - v6;
			rez = Math.round(rez * 100) / 100;
			if (rez <= 0) rez = null;
			edit_return_orend_payed.SetValue(rez);
		}



        function PerformAllValidations(s, e) {
            var errorsFound = false;
            var activeTabIndex = CardPageControl.GetActiveTabIndex();
            var tabPageCount = CardPageControl.GetTabCount();

            for (var i = 0; i < tabPageCount; i++) {

                CardPageControl.SetActiveTabIndex(i);

                if (!ASPxClientEdit.ValidateEditorsInContainer(CardPageControl.GetMainElement())) {
                    errorsFound = true;
                    break;
                }
            }

            if (!errorsFound) {
                CardPageControl.SetActiveTabIndex(activeTabIndex);
            }

            return !errorsFound;
        }

        function EnableInsuranceControls(s, e) {

            var isInsured = is_insured.GetChecked();
            insurance_start.SetEnabled(isInsured);
            insurance_end.SetEnabled(isInsured);
            insurance_sum.SetEnabled(isInsured);
        }

        function OnValidateInsuranceSum(s, e) {

            if (is_insured.GetChecked()) {

                var val = insurance_sum.GetValue();

                if (val == null || val == undefined || val <= 0) {
                    e.isValid = false;
                    e.errorText = "Необхідно заповнити поле 'Сума страхування'";
                }
                else {
                    e.isValid = true;
                }
            }
            else {
                e.isValid = true;
            }
        }

        function OnValidateInsuranceStart(s, e) {

            if (is_insured.GetChecked()) {

                var val = insurance_start.GetValue();

                if (val == null || val == undefined) {
                    e.isValid = false;
                    e.errorText = "Необхідно заповнити поле 'Дата початку періоду страхування'";
                }
                else {
                    e.isValid = true;
                }
            }
            else {
                e.isValid = true;
            }
        }
        function OnValidateInsuranceEnd(s, e) {

            if (is_insured.GetChecked()) {

                var val = insurance_end.GetValue();

                if (val == null || val == undefined) {
                    e.isValid = false;
                    e.errorText = "Необхідно заповнити поле 'Дата закінчення періоду страхування'";
                }
                else {
                    e.isValid = true;
                }
            }
            else {
                e.isValid = true;
            }
        }

        function OnSaveClick(s, e) {
            GridViewPaymentDocuments.UpdateEdit();
        }
        function OnCancelClick(s, e) {
            GridViewPaymentDocuments.CancelEdit();
        }
        function OnSaveDecisionClick(s, e) {
            GridViewDecisions.UpdateEdit();
        }
        function OnCancelDecisionClick(s, e) {
            GridViewDecisions.CancelEdit();
		}
        function OnSaveSubleaseClick(s, e) {
            GridViewSubleases.UpdateEdit();
        }
        function OnCancelSubleaseClick(s, e) {
            GridViewSubleases.CancelEdit();
        }
        function OnSaveNoteClick(s, e) {
            GridViewNotes.UpdateEdit();
        }
        function OnCancelNoteClick(s, e) {
            GridViewNotes.CancelEdit();
        }

        function OnComboGiverOrgSelIndexChanged(s, e) {

            var orgGiverId = ComboGiverOrg.GetValue();

            if (orgGiverId != null && orgGiverId != undefined && orgGiverId != 27065) {
                LabelGiverComment.SetVisible(true);
                EditGiverComment.SetVisible(true);
            }
            else {
                LabelGiverComment.SetVisible(false);
                EditGiverComment.SetVisible(false);
            }
        }

        function OnValidateEditGiverComment(s, e) {

            if (EditGiverComment.GetVisible()) {

                var val = '' + EditGiverComment.GetText();

                if (val == '') {
                    e.isValid = false;
                    e.errorText = "Необхідно заповнити поле 'Коментар щодо вибору Орендодавця'";
                }
                else {
                    e.isValid = true;
                }
            }
            else {
                e.isValid = true;
            }
        }

        function updateReportingPeriodComboStyles() {
            console.dir(ReportingPeriodCombo);
            HideValidator();
        }

        function useValidationRules() {
            var val_znyato_nadmirno_narah = nn(edit_znyato_nadmirno_narah.GetValue());
			if (val_znyato_nadmirno_narah > 0) {
                return false;
            }
            return true;
		}

        function ReportingPeriodCombovalidate() {
            if (IsReadOnlyForm) {
                return true;
            }

            var cdate = new Date();
//////////            if ((cdate.getMonth() == 0 || cdate.getMonth() == 3 || cdate.getMonth() == 6 || cdate.getMonth() == 9) && (cdate.getDate() >= 1 && cdate.getDate() <= 20)) 
            //////////	{
            var error = false;
              var element = document.getElementById('<%=PaymentForm.ClientID %>' + '_ReportingPeriodCombo_I'); //ctl00_MainContent_CPMainPanel_CardPageControl_PaymentForm_ReportingPeriodCombo_I
            //var period = document.getElementById('<%=PaymentForm.ClientID %>' + '_NeededPeriodCombo'); 
            if (element.value != period)
                   error = true;

                var rbt1 = document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S');//ctl00_MainContent_CPMainPanel_CardPageControl_OrganizationsForm_PanelAgreement
            if ((rbt1.value == "C") && (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "грошова оплата" || document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "погодинно") && error == true) {
                if (useValidationRules()) {
                    document.getElementById('valError').style.display = '';
                    document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» та “Вид оплати”-«ГРОШОВА ОПЛАТА», «ПОГОДИННО», повинен бути встановлений діючий Звітний період";
                    return false;
                }
            }


                if (!ComboPaymentTypeValidate())
                    return false;

                if (!EditCollectionDebtTotalValidate())
                    return false;

                if (!CheckRadioAgreementActiveValidate())
                    return false;

			    if (!CheckEndOfDogogor())
				    return false;

			    if (!CheckZaborgMensheNadzhodg())
	    			return false;

			    if (!CheckReturnOrendPayed())
                    return false;

			    if (!CheckReceivedLessNarazh())
                    return false;

			    if (!CheckNarazhAndZaborgSum())
				    return false;
			    
			    if (!CheckReturnAllOrendPayed())
                    return false;

	    		if (!CheckTotalPaidSum())
    				return false;


//                if (!CheckRadioAgreementAndSquare())
//                    return false;

  
//////////            }
           HideValidator();
            return true;
        }

        function HideValidator() {
            document.getElementById('valError').style.display = 'none';
        }
        
        function ComboPaymentTypeValidate() {
            if ((document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "грошова оплата" || (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "погодинно") &&
                document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S').value == "C")) {

				var EditPaymentNarah = getPaymentNarah();
                var EditPaymentNarZvit = document.getElementById('<%=PaymentForm.ClientID %>' + '_PanelRentPaymentDocuments_CPRentPayment_EditPaymentNarZvit_orndpymnt_I').value;
				console.log("EditPaymentNarah(1)=", EditPaymentNarah);
                if (EditPaymentNarah < parseFloat(EditPaymentNarZvit.replace(",", "."))) {
                    if (useValidationRules()) {
                        document.getElementById('valError').style.display = '';
                        document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» та “Вид оплати”-«ГРОШОВА ОПЛАТА», «ПОГОДИННО», значення поля «Нараховано орендної плати за звітний період, грн. (без ПДВ)» повинно бути більше, або дорівнювати значенню поля «у тому числі, з нарахованої за звітний період (без боргів та переплат)»";
                        return false;
                    }
                }
                
            }
            return true;
        }

        function EditCollectionDebtTotalValidate() {

//            if (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "грошова оплата" &&
//              document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S').value == "C") {

            if ((document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "грошова оплата"
                || document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "погодинно") &&
              document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S').value == "C") {

                
                var EditCollectionDebtZvit = document.getElementById('<%=CollectionForm.ClientID %>' + '_PanelCollection_EditCollectionDebtZvit_I').value; //- за звітний період 
                var EditPaymentNarZvit = document.getElementById('<%=PaymentForm.ClientID %>' + '_PanelRentPaymentDocuments_CPRentPayment_EditPaymentNarZvit_orndpymnt_I').value;
                var EditPaymentSaldo = document.getElementById('<%=PaymentForm.ClientID %>' + '_PanelRentPaymentDocuments_CPRentPayment_EditPaymentSaldo_orndpymnt_I').value; //Сальдо на початок року 
				var EditPaymentNarah = getPaymentNarah();
				console.log("EditPaymentNarah(2)=", EditPaymentNarah);

                if (EditCollectionDebtZvit == "") EditCollectionDebtZvit = "0";
                if (EditPaymentNarZvit == "") EditPaymentNarZvit = "0";
                //if (EditPaymentNarah == "") EditPaymentNarah = "0";
                if (EditPaymentSaldo == "") EditPaymentSaldo = "0";

                var paymentOldDebtsPayed_orndpymnt = clEditPaymentOldDebtsPayed_orndpymnt.GetValue();
                if (paymentOldDebtsPayed_orndpymnt == null) paymentOldDebtsPayed_orndpymnt = 0;

                //if (EditPaymentNarah == "" && EditCollectionDebtZvit != "" && EditPaymentNarZvit != "" && EditPaymentSaldo != "")
                //    EditPaymentNarah = "0";
				console.log( 11, EditPaymentNarah );
				console.log( 31, Math.round(parseFloat(parseFloat(EditCollectionDebtZvit.replace(",", ".")) + parseFloat(EditPaymentNarZvit.replace(",", ".")) + parseFloat(EditPaymentSaldo.replace(",", ".")) + paymentOldDebtsPayed_orndpymnt) * 1000) / 1000 );
                if (EditPaymentNarah > Math.round(parseFloat(parseFloat(EditCollectionDebtZvit.replace(",", ".")) + parseFloat(EditPaymentNarZvit.replace(",", ".")) + parseFloat(EditPaymentSaldo.replace(",", ".")) + paymentOldDebtsPayed_orndpymnt) * 1000) / 1000) {
                    if (useValidationRules()) {
                        document.getElementById('valError').style.display = '';
                        document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» та «Вид оплати»-«ГРОШОВА ОПЛАТА», «ПОГОДИННО», значення поля «Нараховано орендної плати за звітний період, грн. (без ПДВ)» не повинно перевищувати суму значень полів «у тому числі, з нарахованої за звітний період (без боргів та переплат)» та «Заборгованість по орендній платі, грн. - (без ПДВ): - за звітний період», «Погашення заборгованості минулих періодів, грн.» та «Сальдо на початок року (незмінна впродовж року величина)».";
                        return false;
                    }
                }
            }
            return true;
        }


        function CheckRadioAgreementActiveValidate() {
            if (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S').value == "C" && 
                document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_EditActualFinishDate_I').value != "") {
                if (useValidationRules()) {
                    document.getElementById('valError').style.display = '';
                    document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» значення поля «Фактична дата закінчення договору» повинно бути порожнім";
                    return false;
                }
             }
            if ((document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementToxic_S').value == "C" || (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementContinuedByAnother_S').value == "C"))&&
                document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_EditActualFinishDate_I').value == "") {
                    if (useValidationRules()) {
                        document.getElementById('valError').style.display = '';
                        document.getElementById('valError').innerHTML = "По договору, що має статус «Договір закінчився» значення поля «Фактична дата закінчення договору» не повинно бути порожнім";
                        return false;
                    }
             }
            return true;
        }

        function CheckRadioAgreementAndSquare() {
            var square = document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_EditRentSquare_I').value;
            if (square == "") square = "0";

            if (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S').value == "C" &&
                parseFloat(square.replace(",", ".")) == 0) {
                if (useValidationRules()) {
                    document.getElementById('valError').style.display = '';
                    document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» значення поля «Площа що використовується всього, кв. м» не повинно дорівнювати 0 ";
                    return false;
                }
            }
            return true;
        }

        function CheckEndOfDogogor() {
            //var reportingPeriod = ReportingPeriodCombo.GetValue();
            //var reportingPeriodText = ReportingPeriodCombo.GetItem(reportingPeriod);
            var reportingPeriodInfo = ReportingPeriodCombo.GetItem(ReportingPeriodCombo.GetSelectedIndex());
            if (reportingPeriodInfo != null) {
				var reportingPeriodText = reportingPeriodInfo.text;
                var s1 = "3 місяці ";
                if (reportingPeriodText.substring(0, s1.length) == s1) {
                    var yy = reportingPeriodText.replace(s1, "");

                    var actualFinishDate = clEditActualFinishDate.GetDate();
                    if (actualFinishDate != null) {
                        if (actualFinishDate.getFullYear() == yy - 1) {
							var radioAgreementToxic = clRadioAgreementToxic.GetValue();
                            if (radioAgreementToxic == true) {
                                var editCollectionDebtTotal = EditCollectionDebtTotal.GetValue();
                                if (editCollectionDebtTotal == 0 || editCollectionDebtTotal == null) {
									var v1 = getPaymentNarah();
									var v2 = clEditPaymentSaldo_orndpymnt.GetValue();
									var v3 = Received_orndpymnt.GetValue();
									var v4 = Zvit_orndpymnt.GetValue();
                                    var v5 = clEditPaymentOldDebtsPayed_orndpymnt.GetValue();
                                    //if (v1 > 0 || v2 > 0 || v3 > 0 || v4 > 0 || v5 > 0) {
                                    if (v1 > 0 || v3 > 0 || v4 > 0 || v5 > 0) {
                                        if (useValidationRules()) {
                                            document.getElementById('valError').style.display = '';
                                            document.getElementById('valError').innerHTML = 'Надходження по цьому договору за звітний період НЕ можливі. Потрібно очистить поля блока "Надходження орендної плати" закладки "Плата за використання"';
                                            return false;
                                        }
									}
								}
							}
							
						}
					}
                }
            }
			return true;
        }

        function CheckZaborgMensheNadzhodg() {
            var v1 = Received_orndpymnt.GetValue();
			var v2 = getPaymentNarah();
            var v3 = EditCollectionDebtZvit.GetValue();
            if (v1 == null) v1 = 0;
            if (v2 == null) v2 = 0;
            if (v3 == null) v3 = 0;
            if (v3 > v2) {
                if (useValidationRules()) {
                    document.getElementById('valError').style.display = '';
                    document.getElementById('valError').innerHTML = 'Значення показника  "Заборгованість по орендній платі, грн. (без ПДВ): - за звітний період",  перевищує  значення показника: "Нараховано орендної плати за звітний період, грн. (без ПДВ)"';
                    return false;
                }
            }
			return true;
        }


        function CheckReturnOrendPayed() {
			return true;
            /*
            var v1 = clEditPaymentSaldo_orndpymnt.GetValue();
            var v2 = edit_return_orend_payed.GetValue();
            if (v1 == null) v1 = 0;
			if (v2 == null) v2 = 0;
            if (v1 - v2 < 0) {
                if (useValidationRules()) {
                    document.getElementById('valError').style.display = '';
			        document.getElementById('valError').innerHTML = 'Повернення більше переплати неможливо';
                    return false;
                }
            }
			return true;
            */
        }

		function CheckReceivedLessNarazh() {
            var v1 = Received_orndpymnt.GetValue();
			var v2 = Zvit_orndpymnt.GetValue();
            if (v1 < v2) {
                if (useValidationRules()) {
                    document.getElementById('valError').style.display = '';
                    document.getElementById('valError').innerHTML = 'Надходження орендної плати за звітний період, всього, не може бути меньше ніж   - у тому числі, з нарахованої за звітний період';
                    return false;
                }
			}
			return true;
		}

        function CheckNarazhAndZaborgSum() {
			return true;

			var v1 = Received_orndpymnt.GetValue();
            var v2 = EditCollectionDebtZvit.GetValue();
			var v3 = getPaymentNarah();
			var v4 = clEditPaymentSaldo_orndpymnt.GetValue();
            if (v1 == null) v1 = 0;
            if (v2 == null) v2 = 0;
            if (v3 == null) v3 = 0;
			if (v4 == null) v4 = 0;
			//alert(v1); alert(v2); alert(v3);
            if (v1 + v2 + v4 != v3) {
                if (useValidationRules()) {
                    document.getElementById('valError').style.display = '';
                    document.getElementById('valError').innerHTML = 'Надходження + заборгованість + Сальдо на початок року не дорівнюють Нарахуванню';
                    return false;
                }
			}
			return true;
        }

		function CheckReturnAllOrendPayed() {
			//var v1 = edit_return_all_orend_payed.GetValue();
			//var v2 = edit_return_orend_payed.GetValue();
			//if (v1 == null) v1 = 0;
			//if (v2 == null) v2 = 0;
   //         if (v1 > v2) {
   //             if (useValidationRules()) {
   //                 document.getElementById('valError').style.display = '';
   //                 document.getElementById('valError').innerHTML = 'Сума повернення більше наявної суми переплати';
   //                 return false;
   //             }
			//}
			return true;
		}




        function OnRentalRateChanged(s, e) {
            var RentalRateId = ComboNotePaymentType1.GetValue();
            if (RentalRateId != null && RentalRateId != undefined ) {
                ComboRentalRate.SetValue(RentalRateId);

                EditNoteCostNarah.SetValue(ComboRentalRate.GetText());

            }
            }


     // ]]>

	</script>

	<script type="text/javascript" language="javascript">
        var imageIndex = 0;

        function DeleteImage(index) {
            var r = confirm("Ви дійсно хочете видалити фото?");
            if (r == true) {
                ContentCallback.PerformCallback('deleteimage:' + index);
                ASPxCallbackPanelImageGallery.PerformCallback('ASPxCallbackPanelImageGallery:');
            }
        }

        function EditImage(index) {
            editPopup.Show();
        }


        function Uploader_OnUploadStart() {
            btnUpload.SetEnabled(false);
        }


        function setUrlParam(prmName, val) {
            var res = '';
            var d = location.href.split("#")[0].split("?");
            var base = d[0];
            var query = d[1];
            if (query) {
                var params = query.split("&");
                for (var i = 0; i < params.length; i++) {
                    var keyval = params[i].split("=");
                    if (keyval[0] != prmName) {
                        res += params[i] + '&';
                    }
                }
            }
            res += prmName + '=' + val;
            window.location.href = base + '?' + res;
            return false;
        } 

        function Uploader_OnFilesUploadComplete(args) {
            UpdateUploadButton();
            ASPxCallbackPanelImageGallery.PerformCallback('refreshphoto:');
		}

        function UpdateUploadButton() {
            btnUpload.SetEnabled(uploader.GetText(0) != "");
        }
    </script>

    <script type="text/javascript">
        function OnInit(s, e) {
            AdjustSize();
        }
        function OnEndCallback(s, e) {
            AdjustSize();

			if (grid.IsEditing()) {
				var popup = s.GetEditFormTable();
				$(felm__tmp1.mainElement).hide();
				$(felm__tmp2.mainElement).hide();
                $(felm__tmp3.mainElement).hide();
				$(popup).find(".dxflCommandItemSys").css("margin-left", "0px");

				var err_object = $("#MainContent_CPMainPanel_CardPageControl_ASPxRoundPanel1_ASPxGridViewFreeSquare_DXEditingErrorItem");
                console.log("err_object", err_object);
				if (err_object.length > 0) {
					var err_text = err_object.text();
					if (err_text == null) err_text = "";
					if (err_text.indexOf("Об'єкт погоджено орендодавцем! Усі зміни ТІЛЬКИ з його дозволу за тел: 202-61-76, 202-61-77, 202-61-96 !") >= 0) {
						console.log("err_object.text", err_text);
						var save_image = $(popup).find(".dxflCommandItemSys").find("a").first();
						save_image.hide();
					}
				}


				console.log("felm__prop_srok_orands", felm__prop_srok_orands);
				felm__include_in_perelik.ValueChanged.AddHandler(OnEditFormTableItemChange);
				felm__prop_srok_orands.LostFocus.AddHandler(OnEditFormTableItemChange);

				felm__floor.tooltip = "Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)";
				$(felm__floor.GetInputElement()).attr('title', "Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)");
				$(popup).find("label").each(function (index) {
					if ($(this).text() == "Характеристика об’єкта оренди") {
						$(this).attr('title', "Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)");
					}
                });


                var orend_plat_borg = felm__orend_plat_borg.GetValue();
                if (orend_plat_borg == null) {
					var debtTotal = EditCollectionDebtTotal.GetValue();
					felm__orend_plat_borg.SetValue(debtTotal);
				}


				CustomizeEditFormTable();
			}

			if (editFreeSquareMode) {
				if (!grid.IsEditing()) {
					window.location = "Report1NFFreeSquare.aspx";
				}
			}
        }

		function OnEditFormTableItemChange() {
			CustomizeEditFormTable();
        }

		function CustomizeEditFormTable() {
			var include_in_perelik = felm__include_in_perelik.GetValue();
			var showrow1 = false;
            var showrow2 = false;

			showrow1 = true;
			//if (include_in_perelik == "1") {
			//	showrow1 = true;
			//} else if (include_in_perelik == "2") {
            //	showrow2 = true;
			//}

			var popup = grid.GetEditFormTable();
			var div = $(popup).find('.dxflFormLayout_DevEx');
			var dtable = $(div).children('table');
			var tbody = $(dtable).children('tbody');
			var rows = $(tbody).children('tr');
			var r1 = 9;
			var r2 = r1 + 1;
			if (showrow1) {
				$(rows.get(r1)).show();
			} else {
				$(rows.get(r1)).hide();
			}
			if (showrow2) {
				$(rows.get(r2)).show();
			} else {
				$(rows.get(r2)).hide();
			}


			var prop_srok_orands = felm__prop_srok_orands.GetValue();
			var prop = parseInt(prop_srok_orands, 10);
			var showrow1 = false;
			if (prop > 5) {
				showrow1 = true;
			}
			var r1 = 12;
			if (showrow1) {
				$(rows.get(r1)).show();
			} else {
				$(rows.get(r1)).hide();
            }
		}


        function OnControlsInitialized(s, e) {
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
            });
        }
        function AdjustSize() {
            var height = Math.max(0, document.documentElement.clientHeight) - 260;
            grid.SetHeight(height);
        }
        function ShowPhoto(s, e) {
            if (e.buttonID == 'btnPhoto') {
                $.cookie('RecordID', s.GetRowKey(e.visibleIndex));
                ASPxFileManagerPhotoFiles.Refresh();
                PopupObjectPhotos.Show();
            }
        }

		function ShowDogcontinuePhoto(s, e) {
			if (e.buttonID == 'btnPhoto') {
				$.cookie('RecordID', s.GetRowKey(e.visibleIndex));
				ASPxFileManagerPhotoFiles.Refresh();
				PopupObjectPhotos.Show();
			} else if (e.buttonID == 'btnPdfBuild') {
				grid.GetRowValues(e.visibleIndex, 'id', OnGridPdfBuildGetRowValues);
			} else if (e.buttonID == 'btnFreeCycle') {
				grid.GetRowValues(e.visibleIndex, 'id', OnFreeCycleGetRowValues);
			}
        }

		function OnGridPdfBuildGetRowValues(values) {
			var id = values;
			window.open(
				'BalansDogContinuePhotosPdf.aspx?id=' + id,
				'_blank',
			);
		}


	</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceBuilding" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 b.* FROM reports1nf_arenda ar INNER JOIN reports1nf_buildings b ON b.unique_id = ar.building_1nf_unique_id
        WHERE ar.id = @aid AND ar.report_id = @rep_id"
    OnSelecting="SqlDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentAgreement" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 ar.*,
        org_renter.full_name AS 'renter_name',
        org_giver.full_name AS 'giver_name',
        org_balans.full_name AS 'balans_name',
        CASE WHEN ar.agreement_state = 1 THEN 1 ELSE 0 END AS 'agr_active',
        CASE WHEN ar.agreement_state = 2 THEN 1 ELSE 0 END AS 'agr_toxic',
        CASE WHEN ar.agreement_state = 3 THEN 1 ELSE 0 END AS 'agr_continued_by_another'
        FROM reports1nf_arenda ar
        LEFT OUTER JOIN organizations org_renter ON org_renter.id = ar.org_renter_id and (org_renter.is_deleted is null or org_renter.is_deleted = 0)
        LEFT OUTER JOIN organizations org_giver ON org_giver.id = ar.org_giver_id and (org_giver.is_deleted is null or org_giver.is_deleted = 0)
        LEFT OUTER JOIN organizations org_balans ON org_balans.id = ar.org_balans_id and (org_balans.is_deleted is null or org_balans.is_deleted = 0)
        WHERE ar.id = @aid AND report_id = @rep_id"
    OnSelecting="SqlDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAgreementStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT CASE WHEN submit_date IS NULL OR modify_date IS NULL OR modify_date > submit_date THEN 0 ELSE 1 END AS 'record_status' FROM reports1nf_arenda WHERE id = @aid AND report_id = @rep_id"
    OnSelecting="SqlDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDecisions" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, arenda_id, doc_num, doc_date, doc_dodatok, doc_punkt, purpose_str, rent_square, pidstava
        FROM reports1nf_arenda_decisions WHERE report_id = @rep_id AND arenda_id = @aid"
    OnSelecting="SqlDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (not name is null) and (RTRIM(LTRIM(name)) <> '')">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBuildings" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, LTRIM(RTRIM(addr_nomer)) AS 'nomer' from buildings where 
        (is_deleted IS NULL OR is_deleted = 0) AND
        (master_building_id IS NULL) AND
        addr_street_id = @street_id AND
        (RTRIM(LTRIM(addr_nomer)) <> '') ORDER BY RTRIM(LTRIM(addr_nomer))"
    OnSelecting="SqlDataSourceDictBuildings_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="street_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceSubleases" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, arenda_id, payment_type_id, agreement_date, agreement_num, rent_start_date, rent_finish_date, rent_square, rent_payment_month, using_possible_id
        FROM reports1nf_arenda_subleases WHERE report_id = @rep_id AND arenda_id = @aid"
    OnSelecting="SqlDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceNotes" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, purpose_group_id, purpose_id, purpose_str, rent_square, note, rent_rate, cost_narah, cost_agreement,
        cost_expert_total, date_expert, payment_type_id, invent_no, note_status_id FROM reports1nf_arenda_notes WHERE (is_deleted IS NULL OR is_deleted = 0) AND report_id = @rep_id AND arenda_id = @aid"
    OnSelecting="SqlDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentPayment" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 * FROM reports1nf_arenda_payments WHERE arenda_id = @aid AND report_id = @rep_id"
    OnSelecting="SqlDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePaymentDocuments" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM reports1nf_payment_documents WHERE arenda_id = @aid AND report_id = @rep_id"
    OnSelecting="SqlDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePaymentType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_arenda_payment_type WHERE id IN (11, 8, 3, 7, 10) ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocKind" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_doc_kind ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePurpose" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_balans_purpose ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePurposeGroup" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_balans_purpose_group ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaNoteStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_arenda_note_status ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentPeriods" runat="server" EnableCaching="false"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rent_period">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaArchive" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT archive_id, org_renter_full_name, object_name, agreement_num, agreement_date, rent_start_date, rent_finish_date, rent_square, modified_by, modify_date
        FROM view_arch_arenda WHERE arenda_id = @arid AND (NOT modified_by IS NULL) AND (NOT modify_date IS NULL) ORDER BY modify_date, archive_id" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearchGiver" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
SELECT id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations org WHERE
    (is_deleted is null or is_deleted = 0) and
	@zkpo = '^' AND
	@fname = '^' AND
	(org.id IN (SELECT ar.org_giver_id FROM reports1nf_arenda ar WHERE ar.id = @aid AND ar.report_id = @rep_id) OR
	org.id IN (@balans_org, 27065, 8515,8400,1728,9826,15130,141824,137676,8566,308543,99405315))
UNION ALL	
SELECT id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations org WHERE
	@zkpo != '^' AND
	@fname != '^' AND
	org.id in (SELECT TOP 20 id FROM organizations WHERE zkpo_code LIKE @zkpo AND full_name LIKE @fname AND (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL)"
    OnSelecting="SqlDataSourceOrgSearchGiver_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="balans_org" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearchRenter" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations org
        WHERE
        	(@zkpo = '^' AND @fname = '^' AND (org.id IN (SELECT ar.org_renter_id FROM reports1nf_arenda ar WHERE ar.id = @aid AND ar.report_id = @rep_id))
	        OR
        	(@zkpo != '^' AND @fname != '^' AND org.id IN (SELECT TOP 20 id FROM organizations WHERE zkpo_code LIKE @zkpo AND full_name LIKE @fname AND (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL)))
        ORDER BY org.full_name"
    OnSelecting="SqlDataSourceOrgSearchRenter_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRenter" runat="server" ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT org.* FROM organizations org
                    LEFT JOIN reports1nf_arenda agr
                    ON agr.org_renter_id = org.id
                    WHERE agr.report_id = @rep_id AND agr.id = @aid"
    OnSelecting="SqlDataSourceRenter_Selecting">         
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>    

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictDistricts2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_1nf_org_ownership where len(name) > 0 order by name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgIndustry" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_industry ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOccupation" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_occupation ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_status ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictRentalRate" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, short_name = left(full_name, 150), rental_rate FROM dict_rental_rate ORDER BY short_name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceUsingPossible" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, left(full_name, 150) as name, rental_rate, 1 as ordrow FROM dict_rental_rate union select null, '<пусто>', null, 2 as ordrow ORDER BY ordrow, name">
</mini:ProfiledSqlDataSource>

<asp:ObjectDataSource ID="ObjectDataSourceBalansPhoto" runat="server" SelectMethod="SelectFromTempFolder" 
    TypeName="ExtDataEntry.Models.FileAttachment">
    <SelectParameters>
        <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
        <asp:Parameter DefaultValue="" Name="recordID" Type="Int32" />
        <asp:Parameter DefaultValue="" Name="tempGuid" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceTechStane" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_tech_stane">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePeriodUsed" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_period_used">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePowerInfo" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_power_info union select null, ''">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceInvestSolution" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_invest_solution union select null, ''">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceZgodaRenter" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_zgoda_renter ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeObjectType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_free_object_type ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceIncludeInPerelik" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT '1' id, '1' name, 1 as ordrow union SELECT '2' id, '2' name, 1 as ordrow union select null, '',  2 as ordrow ORDER BY ordrow, name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT [id]
      ,[arenda_id]
      ,[total_free_sqr]
      ,[free_sqr_korysna]
      ,[free_sqr_condition_id]
	  ,[period_used_id]
      ,[floor]
      ,[possible_using]
	  ,[using_possible_id]
      ,[water]
	  ,[heating]
      ,[power_info_id]
      ,[gas]
      ,[note]
      ,[modify_date]
      ,[modified_by]
      ,[report_id] 
      ,[is_solution] 
	  ,[invest_solution_id] 
      ,[initiator] 
      ,[zgoda_control_id] 
      ,[free_object_type_id] 
      ,[zgoda_renter_id] 
	  ,[is_included] 	
	  ,[komis_protocol] 	
      ,[include_in_perelik]
      ,[zal_balans_vartist]
      ,[perv_balans_vartist]  
      ,[punkt_metod_rozrahunok] 
      ,[prop_srok_orands] 
      ,[nomer_derzh_reestr_neruh] 
      ,[reenum_derzh_reestr_neruh] 
      ,[info_priznach_nouse] 
      ,[info_rahunok_postach] 
      ,[primitki] 
      ,[priznach_before] 
      ,[period_nouse] 
      ,[osoba_use_before]
      ,[has_perevazh_pravo]
      ,[polipshanya_vartist]
      ,[polipshanya_finish_date]
      ,[orend_plat_last_month]
      ,[orend_plat_borg]
      ,[stanom_na]
      ,[zalbalansvartist_date]
      ,[osoba_oznakoml]
    FROM [reports1nf_arenda_dogcontinue] WHERE [arenda_id] = @arenda_id and [report_id] = @report_id and ([id] = @free_square_id or @free_square_id = -1)" 
    DeleteCommand="EXEC [delete_reports1nf_arenda_dogcontinue] @id" 
    InsertCommand="INSERT INTO [reports1nf_arenda_dogcontinue]
    ([arenda_id]
      ,[total_free_sqr]
      ,[free_sqr_korysna]
      ,[free_sqr_condition_id]
	  ,[period_used_id]
      ,[floor]
      ,[possible_using]
	  ,[using_possible_id]
      ,[water]
      ,[heating]
      ,[power_info_id]
      ,[gas]
      ,[note]
      ,[modify_date]
      ,[modified_by]
      ,[report_id]
      ,[is_solution] 
	  ,[invest_solution_id]  
      ,[initiator] 
      ,[zgoda_control_id] 
      ,[free_object_type_id] 
      ,[zgoda_renter_id]
	  ,[is_included]
      ,[include_in_perelik]
      ,[zal_balans_vartist]
      ,[perv_balans_vartist]  
      ,[punkt_metod_rozrahunok] 
      ,[prop_srok_orands] 
      ,[nomer_derzh_reestr_neruh] 
      ,[reenum_derzh_reestr_neruh] 
      ,[info_priznach_nouse] 
      ,[info_rahunok_postach]   
      ,[primitki]   
      ,[priznach_before]   
      ,[period_nouse]
      ,[osoba_use_before]
      ,[has_perevazh_pravo]
      ,[polipshanya_vartist]
      ,[polipshanya_finish_date]
      ,[orend_plat_last_month]
      ,[orend_plat_borg]
      ,[stanom_na]
      ,[zalbalansvartist_date]
      ,[osoba_oznakoml]
    ) 
    VALUES
    (@arenda_id
      ,@total_free_sqr
      ,@free_sqr_korysna
      ,@free_sqr_condition_id
	  ,@period_used_id
      ,@floor
      ,@possible_using
	  ,@using_possible_id
      ,@water
      ,@heating
      ,@power_info_id
      ,@gas
      ,@note
      ,@modify_date
      ,@modified_by
      ,@report_id
      ,@is_solution
	  ,@invest_solution_id
      ,@initiator
      ,@zgoda_control_id
      ,@free_object_type_id
      ,@zgoda_renter_id
	  ,@is_included
      ,@include_in_perelik
      ,@zal_balans_vartist
      ,@perv_balans_vartist  
      ,@punkt_metod_rozrahunok 
      ,@prop_srok_orands 
      ,@nomer_derzh_reestr_neruh 
      ,@reenum_derzh_reestr_neruh 
      ,@info_priznach_nouse 
      ,@info_rahunok_postach       
      ,@primitki       
      ,@priznach_before       
      ,@period_nouse
      ,@osoba_use_before
      ,@has_perevazh_pravo
      ,@polipshanya_vartist
      ,@polipshanya_finish_date
      ,@orend_plat_last_month
      ,@orend_plat_borg
      ,@stanom_na
      ,@zalbalansvartist_date
      ,@osoba_oznakoml
    );
SELECT SCOPE_IDENTITY()" 
    UpdateCommand="UPDATE [reports1nf_arenda_dogcontinue]
SET
    [arenda_id] = @arenda_id,
    [total_free_sqr] = @total_free_sqr,
    [free_sqr_korysna] = @free_sqr_korysna,
    [free_sqr_condition_id] = @free_sqr_condition_id,
	[period_used_id] = @period_used_id, 
    [floor] = @floor,
    [possible_using] = @possible_using,
	[using_possible_id] = @using_possible_id,  
    [water] = @water,
    [heating] = @heating,
    [power_info_id] = @power_info_id,
    [gas] = @gas,
    [note] = @note,
    [modify_date] = @modify_date,
    [modified_by] = @modified_by,
    [report_id] = @report_id
      ,[is_solution] = @is_solution
	  ,[invest_solution_id] = @invest_solution_id
      ,[initiator] = @initiator
      ,[zgoda_control_id] = @zgoda_control_id
      ,[free_object_type_id] = @free_object_type_id
      ,[zgoda_renter_id] = @zgoda_renter_id 
	  ,[is_included] = @is_included 
      ,[include_in_perelik] = @include_in_perelik 
        ,[zal_balans_vartist]		  = @zal_balans_vartist
        ,[perv_balans_vartist]  	  = @perv_balans_vartist  
        ,[punkt_metod_rozrahunok] 	  = @punkt_metod_rozrahunok 
        ,[prop_srok_orands] 		  = @prop_srok_orands 
        ,[nomer_derzh_reestr_neruh] 	  = @nomer_derzh_reestr_neruh 
        ,[reenum_derzh_reestr_neruh] 	  = @reenum_derzh_reestr_neruh 
        ,[info_priznach_nouse] 		  = @info_priznach_nouse 
        ,[info_rahunok_postach]  	  = @info_rahunok_postach   
        ,[primitki]  	  = @primitki   
        ,[priznach_before]  	  = @priznach_before   
        ,[period_nouse]  	  = @period_nouse   
        ,[osoba_use_before]  	  = @osoba_use_before   
        ,[has_perevazh_pravo]  	  = @has_perevazh_pravo
        ,[polipshanya_vartist]  	  = @polipshanya_vartist
        ,[polipshanya_finish_date]  	  = @polipshanya_finish_date
        ,[orend_plat_last_month]  	  = @orend_plat_last_month
        ,[orend_plat_borg]  	  = @orend_plat_borg
        ,[stanom_na]  	  = @stanom_na
        ,[zalbalansvartist_date]  	  = @zalbalansvartist_date
        ,[osoba_oznakoml]  	  = @osoba_oznakoml
WHERE id = @id" 
        oninserting="SqlDataSourceFreeSquare_Inserting" 
        onupdating="SqlDataSourceFreeSquare_Updating" ProviderName="System.Data.SqlClient">
    <SelectParameters>
        <asp:Parameter Name="arenda_id" />
        <asp:Parameter Name="report_id" />
        <asp:Parameter Name="free_square_id" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="id" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="arenda_id" />
        <asp:Parameter Name="total_free_sqr" />
        <asp:Parameter Name="free_sqr_korysna" />
        <asp:Parameter Name="free_sqr_condition_id" />
		<asp:Parameter Name="period_used_id" />
        <asp:Parameter Name="floor" />
        <asp:Parameter Name="possible_using" />
		<asp:Parameter Name="using_possible_id" />
        <asp:Parameter Name="water" />
        <asp:Parameter Name="heating" />
        <asp:Parameter Name="power_info_id" />
        <asp:Parameter Name="gas" />
        <asp:Parameter Name="note" />
        <asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
        <asp:Parameter Name="report_id" />
        <asp:Parameter Name="is_solution" />
		<asp:Parameter Name="invest_solution_id" />
        <asp:Parameter Name="initiator" />
        <asp:Parameter Name="zgoda_control_id" />
        <asp:Parameter Name="free_object_type_id" />
        <asp:Parameter Name="zgoda_renter_id" />
		<asp:Parameter Name="is_included" />		
        <asp:Parameter Name="include_in_perelik" />
        <asp:Parameter Name="zal_balans_vartist" />
        <asp:Parameter Name="perv_balans_vartist" />
        <asp:Parameter Name="punkt_metod_rozrahunok" />
        <asp:Parameter Name="prop_srok_orands" />
        <asp:Parameter Name="nomer_derzh_reestr_neruh" />
        <asp:Parameter Name="reenum_derzh_reestr_neruh" />
        <asp:Parameter Name="info_priznach_nouse" />
        <asp:Parameter Name="info_rahunok_postach" />
        <asp:Parameter Name="primitki" />
        <asp:Parameter Name="priznach_before" />
        <asp:Parameter Name="period_nouse" />
        <asp:Parameter Name="osoba_use_before" />
        <asp:Parameter Name="has_perevazh_pravo" />
        <asp:Parameter Name="polipshanya_vartist" />
        <asp:Parameter Name="polipshanya_finish_date" />
        <asp:Parameter Name="orend_plat_last_month" />
        <asp:Parameter Name="orend_plat_borg" />
        <asp:Parameter Name="stanom_na" />
        <asp:Parameter Name="zalbalansvartist_date" />
        <asp:Parameter Name="osoba_oznakoml" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="arenda_id" />
        <asp:Parameter Name="total_free_sqr" />
        <asp:Parameter Name="free_sqr_korysna" />
        <asp:Parameter Name="free_sqr_condition_id" />
		<asp:Parameter Name="period_used_id" />
        <asp:Parameter Name="floor" />
        <asp:Parameter Name="possible_using" />
		<asp:Parameter Name="using_possible_id" />
        <asp:Parameter Name="water" />
        <asp:Parameter Name="heating" />
        <asp:Parameter Name="power_info_id" />
        <asp:Parameter Name="gas" />
        <asp:Parameter Name="note" />
        <asp:Parameter Name="modify_date" />
        <asp:Parameter Name="modified_by" />
        <asp:Parameter Name="report_id" />
        <asp:Parameter Name="is_solution" />
		<asp:Parameter Name="invest_solution_id" />
        <asp:Parameter Name="initiator" />
        <asp:Parameter Name="zgoda_control_id" />
        <asp:Parameter Name="free_object_type_id" />
        <asp:Parameter Name="zgoda_renter_id" />
		<asp:Parameter Name="is_included" />	
        <asp:Parameter Name="include_in_perelik" />	
        <asp:Parameter Name="zal_balans_vartist" />
        <asp:Parameter Name="perv_balans_vartist" />
        <asp:Parameter Name="punkt_metod_rozrahunok" />
        <asp:Parameter Name="prop_srok_orands" />
        <asp:Parameter Name="nomer_derzh_reestr_neruh" />
        <asp:Parameter Name="reenum_derzh_reestr_neruh" />
        <asp:Parameter Name="info_priznach_nouse" />
        <asp:Parameter Name="info_rahunok_postach" />
        <asp:Parameter Name="primitki" />
        <asp:Parameter Name="priznach_before" />
        <asp:Parameter Name="period_nouse" />
        <asp:Parameter Name="osoba_use_before" />
        <asp:Parameter Name="has_perevazh_pravo" />
        <asp:Parameter Name="polipshanya_vartist" />
        <asp:Parameter Name="polipshanya_finish_date" />
        <asp:Parameter Name="orend_plat_last_month" />
        <asp:Parameter Name="orend_plat_borg" />
        <asp:Parameter Name="stanom_na" />
        <asp:Parameter Name="zalbalansvartist_date" />
        <asp:Parameter Name="osoba_oznakoml" />
        <asp:Parameter Name="id" />
    </UpdateParameters>
</mini:ProfiledSqlDataSource>


<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Cabinet.aspx" Text="Стан"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgInfo.aspx" Text="Загальна Інформація"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansList.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" Text="Відчужені Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgArendaList.aspx" Text="Договори використання приміщень "></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgRentedList.aspx" Text="Договори Орендування"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="http://kmda.iisd.com.ua" Text="Класифікатор майна"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Картка договору передачі в оренду вільного приміщення" CssClass="pagetitle"/>
</p>

<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" runat="server" OnCallback="CPMainPanel_Callback">
    <ClientSideEvents EndCallback="afterCPMainPanelCallback" />
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent5" runat="server">

<asp:FormView runat="server" BorderStyle="None" ID="StatusForm" DataSourceID="SqlDataSourceAgreementStatus" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Необхідно надіслати внесені зміни до ДКВ" ClientVisible='<%# 0.Equals(Eval("record_status")) %>' ForeColor="Red" />
        <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Зміни надіслано до ДКВ" ClientVisible='<%# 1.Equals(Eval("record_status")) %>' />
    </ItemTemplate>
</asp:FormView>

<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" 
                runat="server" ActiveTabIndex="0">
    <TabPages>

        <dx:TabPage Text="Договір" Name="Tab1">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server">

                    <asp:FormView runat="server" BorderStyle="None" ID="AddressForm" DataSourceID="SqlDataSourceBuilding" EnableViewState="False">
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="PanelAddress" runat="server" HeaderText="Адреса будинку">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent4" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">

                                            <tr>
                                                <td width="120px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Район"></dx:ASPxLabel></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboRentAddrDistrict" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDistrict" ReadOnly="true" Enabled="false" Value='<%# Eval("addr_distr_new_id") %>'
                                                         Title="Адреса будинку - район" />
<%-- 
                                                    <dx:ASPxTextBox ID="TextRentAddrDistrict" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" 
                                                        DataSourceID="SqlDataSourceDistrict" Enabled="false" ReadOnly="true" Text='<%# Eval("addr_distr_new_id") %>' Width="270px"  Title="Адреса будинку - район" />
--%>
                                                </td>
                                                <td width="8px">&nbsp;</td>
                                                <td width="120px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Назва вулиці"></dx:ASPxLabel></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <%--<dx:ASPxTextBox ID="EditRentAddrStreet" runat="server" ReadOnly="true" Text='<%# Eval("addr_street_name") %>' Width="270px"  Title="Адреса будинку - Назва вулиці" />--%>
                                                    <dx:ASPxComboBox ID="ComboStreet" runat="server" ClientInstanceName="ComboStreet"
                                                        Value='<%# Bind("addr_street_id") %>'
                                                        DataSourceID="SqlDataSourceDictStreets" DropDownStyle="DropDownList" ValueType="System.Int32"
                                                        TextField="name" ValueField="id" Width="270px" IncrementalFilteringMode="StartsWith"
                                                        FilterMinLength="3" EnableCallbackMode="True" CallbackPageSize="50" EnableViewState="False"
                                                        EnableSynchronization="False">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { ComboBuilding.PerformCallback(ComboStreet.GetValue().toString()); }" />
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr><td colspan="7" height="4px"/></tr>
                                            <tr>
                                                <td width="120px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Номер будинку"></dx:ASPxLabel></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <%--<dx:ASPxTextBox ID="EditRentBuildingNum" runat="server" ReadOnly="true" Text='<%# Eval("addr_nomer") %>' Width="270px" Title="Адреса - номер будинку" />--%>
                                                    <dx:ASPxComboBox runat="server" ID="ComboBuilding" ClientInstanceName="ComboBuilding"
                                                        Value='<%# Bind("addr_nomer") %>'
                                                        DataSourceID="SqlDataSourceDictBuildings" DropDownStyle="DropDownList" TextField="nomer"
                                                        ValueField="id" Width="270px" IncrementalFilteringMode="StartsWith"
                                                        EnableSynchronization="False" OnCallback="ComboBuilding_Callback">
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td width="8px">&nbsp;</td>
                                                <td width="120px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Поштовий індекс"></dx:ASPxLabel></td>
                                                <td width="8px">&nbsp;</td>
                                                <td><dx:ASPxTextBox ID="EditRentZipCode" runat="server" ReadOnly="true" Text='<%# Eval("addr_zip_code") %>' Width="270px" Title="Адреса будинку - Поштовий індекс" /></td>
                                            </tr>
                                            <tr><td colspan="7" height="4px"/></tr>
                                            <tr>
                                                <td width="120px"><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Додаткова адреса"></dx:ASPxLabel></td>
                                                <td width="8px">&nbsp;</td>
                                                <td colspan="5"><dx:ASPxTextBox ID="EditRentMiscAddr" ReadOnly="true" runat="server" Text='<%# Eval("addr_misc") %>' Width="680px" Title="Адреса будинку - Додаткова адреса" /></td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                        </ItemTemplate>
                    </asp:FormView>

                    <p class="SpacingPara"/>

                    <asp:FormView runat="server" BorderStyle="None" ID="OrganizationsForm" DataSourceID="SqlDataSourceRentAgreement" EnableViewState="False">
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Орендар/Позичальник">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td> <dx:ASPxLabel ID="ASPxLabel58" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                                                <td> <dx:ASPxTextBox ID="EditRenterOrgZKPO" ClientInstanceName="EditRenterOrgZKPO" runat="server" Width="200px" Title="Код ЄДРПОУ Орендаря/позичальника"/> </td>
                                                <td> <dx:ASPxLabel ID="ASPxLabel59" runat="server" Text="Назва:" Width="50px" /> </td>
                                                <td> <dx:ASPxTextBox ID="EditRenterOrgName" ClientInstanceName="EditRenterOrgName" runat="server" Width="300px" Title="Назва Орендаря/позичальника"/> </td>
                                                <td align="right">
                                                    <dx:ASPxButton ID="BtnFindRenterOrg" ClientInstanceName="BtnFindRenterOrg" runat="server" AutoPostBack="False" Text="Знайти">
                                                        <ClientSideEvents Click="function (s, e) { ComboRenterOrg.PerformCallback(EditRenterOrgZKPO.GetText() + '|' + EditRenterOrgName.GetText()); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" valign="top">
                                                    <dx:ASPxComboBox ID="ComboRenterOrg" ClientInstanceName="ComboRenterOrg" runat="server" DropDownStyle="DropDownList"
                                                        Width="700px" DataSourceID="SqlDataSourceOrgSearchRenter" ValueField="id" ValueType="System.Int32"
                                                        TextField="search_name" EnableSynchronization="True" OnCallback="ComboRenterOrg_Callback"
                                                        Title="Орендар/Позичальник" Value='<%# Eval("org_renter_id") %>'>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td align="right">
                        
                                        <dx:ASPxCallbackPanel ID="CPPopUp" ClientInstanceName="CPPopUp" runat="server" OnCallback="CPPopUp_Callback">
                                        <ClientSideEvents EndCallback="function (s, e) { if (!LabelOrgEditError.clientVisible) { PopupEditRenterOrg.Hide(); }; if (NewOrgId.value != '') { PopupAddRenterOrg.Hide(); ComboRenterOrg.PerformCallback('create_org:'); } }" />
                        <PanelCollection>
                        <dx:panelcontent ID="Panelcontent5" runat="server">
                            <dx:ASPxPopupControl ID="PopupAddRenterOrg" runat="server" ClientInstanceName="PopupAddRenterOrg" Modal="true"
                                HeaderText="Створення Організації" PopupElementID="BtnCreateRenterOrg"  CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" >
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">     
                                                 
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" HeaderText="Основні відомості" Width="100%">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent9" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="NewOrgId" runat="server" ClientIDMode="static" Value="" /> 
                                                            <dx:ASPxLabel ID="ASPxLabel47" runat="server" Text="Повна Назва:" Width="85px" /> 
                                                        </td>
                                                        <td colspan="3"> 
                                                            <dx:ASPxTextBox ID="TextBoxFullNameOrg" ClientInstanceName="TextBoxFullNameOrg" runat="server" Width="100%" Title="" />                                                                 
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel48" runat="server" Text="Код ЄДРПОУ:" Width="85px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxZkpoCodeOrg" ClientInstanceName="TextBoxZkpoCodeOrg" runat="server" Width="90px" Title="" /> </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel49" runat="server" Text="Коротка Назва" Width="85px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxShortNameOrg" runat="server" Width="290px" ClientInstanceName="TextBoxShortNameOrg" Title="" /> </td>
                                                    </tr>
                                                </table>                                                                    
                                            </dx:PanelContent>
                                        </PanelCollection>                                                        
                                        </dx:ASPxRoundPanel>
                                        <p class="SpacingPara"/>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Юридична адреса" Width="100%">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent10" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Район" Width="95px" /> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxDistrictOrg" runat="server" ClientInstanceName="ComboBoxDistrictOrg" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="120px" Title="" 
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictDistricts2" /> </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Назва Вулиці" Width="84px" /> </td>
                                                        <td colspan="3"> 
                                                            <dx:ASPxTextBox ID="TextBoxStreetNameOrg" runat="server" ClientInstanceName="TextBoxStreetNameOrg"
                                                                Width="100%" Title="" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Номер Будинку" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxAddrNomerOrg" runat="server" Text="" Width="120px" ClientInstanceName="TextBoxAddrNomerOrg" Title="" /> </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabelCorp" runat="server" Text="Корпус" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxAddrKorpusFrom" runat="server" Text="" Width="80px" ClientInstanceName="TextBoxAddrKorpusFrom" Title="" /> </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabelZip" runat="server" Text="Пошт. Індекс" Width="85px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxAddrZipCodeOrg" runat="server" Width="80px" ClientInstanceName="TextBoxAddrZipCodeOrg" Title="" /> </td>
                                                    </tr>
                                                </table>                                                                    
                                            </dx:PanelContent>
                                        </PanelCollection>                                                        
                                        </dx:ASPxRoundPanel>
                                        <p class="SpacingPara"/>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel7" runat="server" HeaderText="Додаткові відомості" Width="100%">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent11" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Статус (фіз./юр. особа)" Width="160px"/> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxStatusOrg" runat="server" ClientInstanceName="ComboBoxStatusOrg" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title=""
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictOrgStatus" /> </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Форма Власності" Width="160px" /> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxFormVlasnOrg" runat="server" ClientInstanceName="ComboBoxFormVlasnOrg" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title="" 
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictOrgOwnership" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel50" runat="server" Text="Галузь" Width="160px" /> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxIndustryOrg" runat="server" ClientInstanceName="ComboBoxIndustryOrg" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title="" 
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictOrgIndustry" /> </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel51" runat="server" Text="Вид Діяльності" Width="160px" /> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxOccupationFrom" runat="server" ClientInstanceName="ComboBoxOccupationFrom" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title="" 
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictOrgOccupation" /> </td>
                                                    </tr>
                                                </table>                                                                    
                                            </dx:PanelContent>
                                        </PanelCollection>                                                        
                                        </dx:ASPxRoundPanel>
                                        <p class="SpacingPara"/>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel8" runat="server" HeaderText="Контактна інформація" Width="100%">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent12" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="ПІБ Директора" Width="95px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxDirectorFioOrg" runat="server" ClientInstanceName="TextBoxDirectorFioOrg" Width="250px" Title="" /> </td>
                                                        <td> &nbsp; </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Тел. Директора" Width="100px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxDirectorPhoneOrg" runat="server" ClientInstanceName="TextBoxDirectorPhoneOrg" Width="100px" Title="" /> </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Email Директора" Width="95px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxDirectorEmailOrg" runat="server" ClientInstanceName="TextBoxDirectorEmailOrg" Width="250px" Title="" /> </td>
                                                        <td colspan="3"> &nbsp; </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="ПІБ Бухгалтера" Width="95px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxBuhgalterFioOrg" runat="server" ClientInstanceName="TextBoxBuhgalterFioOrg" Width="250px" Title="" /> </td>
                                                        <td> &nbsp; </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Тел. Бухгалтера" Width="100px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxBuhgalterPhoneOrg" runat="server" ClientInstanceName="TextBoxBuhgalterPhoneOrg" Width="100px" Title="" /> </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Факс" Width="95px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxFax" runat="server" ClientInstanceName="TextBoxFax" Width="250px" Title="" /> </td>
                                                        <td colspan="3"> &nbsp; </td>
                                                    </tr>
                                                </table>                                                                    
                                            </dx:PanelContent>
                                        </PanelCollection>                                                        
                                        </dx:ASPxRoundPanel>
                                        <p class="SpacingPara"/>
                                        <dx:ASPxLabel ID="LabelOrgCreationError" ClientInstanceName="LabelOrgCreationError" runat="server" Text="" ClientVisible="false" ForeColor="Red" />
                                        <p class="SpacingPara"/>
                                        <dx:ASPxButton ID="ButtonDoAddOrgFrom" ClientInstanceName="ButtonDoAddOrgFrom" runat="server" AutoPostBack="False" Text="Зберегти">
                                            <ClientSideEvents Click="function (s, e) { CPPopUp.PerformCallback('create_org:'); }" />
                                        </dx:ASPxButton>       
                                    </dx:PopupControlContentControl>
                                </ContentCollection>

                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupEditRenterOrg" runat="server" ClientInstanceName="PopupEditRenterOrg" Modal="true"
                                HeaderText="Створення Організації" PopupElementID="BtnEditRenterOrg"  CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" >
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">     
                                        
                                        <asp:FormView runat="server" BorderStyle="None" ID="FormView1" DataSourceID="SqlDataSourceRenter" EnableViewState="False">
                                         <ItemTemplate>
                                                 
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" HeaderText="Основні відомості" Width="100%">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent9" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="OldOrgId" runat="server" ClientIDMode="static" Value='<%# Eval("id") %>' /> 
                                                            <dx:ASPxLabel ID="ASPxLabel47" runat="server" Text="Повна Назва:" Width="85px" /> 
                                                        </td>
                                                        <td colspan="3"> 
                                                            <dx:ASPxTextBox ID="TextBoxFullNameOrg" ClientInstanceName="TextBoxFullNameOrg" runat="server" Width="100%" Title=""
                                                                Value='<%# Eval("full_name") %>' />                                                                 
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel48" runat="server" Text="Код ЄДРПОУ:" Width="85px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxZkpoCodeOrg" ClientInstanceName="TextBoxZkpoCodeOrg" runat="server" Width="90px" Title="" Value='<%# Eval("zkpo_code") %>' /> </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel49" runat="server" Text="Коротка Назва" Width="85px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxShortNameOrg" runat="server" Width="290px" ClientInstanceName="TextBoxShortNameOrg" Title="" Value='<%# Eval("short_name") %>' /> </td>
                                                    </tr>
                                                </table>                                                                    
                                            </dx:PanelContent>
                                        </PanelCollection>                                                        
                                        </dx:ASPxRoundPanel>
                                        <p class="SpacingPara"/>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Юридична адреса" Width="100%">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent10" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Район" Width="95px" /> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxDistrictOrg" runat="server" ClientInstanceName="ComboBoxDistrictOrg" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="120px" Title="" 
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictDistricts2" Value='<%# Eval("addr_distr_new_id") %>' /> </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Назва Вулиці" Width="84px" /> </td>
                                                        <td colspan="3">
                                                            <dx:ASPxTextBox ID="TextBoxStreetNameOrg" runat="server" ClientInstanceName="TextBoxStreetNameOrg"
                                                                Width="100%" Title="" Value='<%# Eval("addr_street_name") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Номер Будинку" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxAddrNomerOrg" runat="server" Text="" Width="120px" ClientInstanceName="TextBoxAddrNomerOrg" Title="" Value='<%# Eval("addr_nomer") %>' /> </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabelCorp" runat="server" Text="Корпус" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxAddrKorpusFrom" runat="server" Text="" Width="80px" ClientInstanceName="TextBoxAddrKorpusFrom" Title="" Value='<%# Eval("addr_korpus") %>' /> </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabelZip" runat="server" Text="Пошт. Індекс" Width="85px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxAddrZipCodeOrg" runat="server" Width="80px" ClientInstanceName="TextBoxAddrZipCodeOrg" Title="" Value='<%# Eval("addr_zip_code") %>' /> </td>
                                                    </tr>
                                                </table>                                                                    
                                            </dx:PanelContent>
                                        </PanelCollection>                                                        
                                        </dx:ASPxRoundPanel>
                                        <p class="SpacingPara"/>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel7" runat="server" HeaderText="Додаткові відомості" Width="100%">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent11" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Статус (фіз./юр. особа)" Width="160px"/> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxStatusOrg" runat="server" ClientInstanceName="ComboBoxStatusOrg" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title=""
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictOrgStatus" Value='<%# Eval("status_id") %>' /> </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Форма Власності" Width="160px" /> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxFormVlasnOrg" runat="server" ClientInstanceName="ComboBoxFormVlasnOrg" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title="" 
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictOrgOwnership" Value='<%# Eval("form_ownership_id") %>' /></td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel50" runat="server" Text="Галузь" Width="160px" /> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxIndustryOrg" runat="server" ClientInstanceName="ComboBoxIndustryOrg" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title="" 
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictOrgIndustry" Value='<%# Eval("industry_id") %>' /> </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel51" runat="server" Text="Вид Діяльності" Width="160px" /> </td>
                                                        <td> <dx:ASPxComboBox ID="ComboBoxOccupationFrom" runat="server" ClientInstanceName="ComboBoxOccupationFrom" 
                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title="" 
                                                                IncrementalFilteringMode="StartsWith" 
                                                                DataSourceID="SqlDataSourceDictOrgOccupation" Value='<%# Eval("occupation_id") %>' /> </td>
                                                    </tr>
                                                </table>                                                                    
                                            </dx:PanelContent>
                                        </PanelCollection>                                                        
                                        </dx:ASPxRoundPanel>
                                        <p class="SpacingPara"/>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel8" runat="server" HeaderText="Контактна інформація" Width="100%">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent12" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="ПІБ Директора" Width="95px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxDirectorFioOrg" runat="server" ClientInstanceName="TextBoxDirectorFioOrg" Width="250px" Title="" Value='<%# Eval("director_fio") %>' /> </td>
                                                        <td> &nbsp; </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Тел. Директора" Width="100px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxDirectorPhoneOrg" runat="server" ClientInstanceName="TextBoxDirectorPhoneOrg" Width="100px" Title="" Value='<%# Eval("director_phone") %>' /> </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Email Директора" Width="95px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxDirectorEmailOrg" runat="server" ClientInstanceName="TextBoxDirectorEmailOrg" Width="250px" Title="" Value='<%# Eval("director_email") %>' /> </td>
                                                        <td colspan="3"> &nbsp; </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="ПІБ Бухгалтера" Width="95px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxBuhgalterFioOrg" runat="server" ClientInstanceName="TextBoxBuhgalterFioOrg" Width="250px" Title="" Value='<%# Eval("buhgalter_fio") %>' /> </td>
                                                        <td> &nbsp; </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Тел. Бухгалтера" Width="100px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxBuhgalterPhoneOrg" runat="server" ClientInstanceName="TextBoxBuhgalterPhoneOrg" Width="100px" Title="" Value='<%# Eval("buhgalter_phone") %>' /> </td>
                                                    </tr>
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Факс" Width="95px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="TextBoxFax" runat="server" ClientInstanceName="TextBoxFax" Width="250px" Title="" Value='<%# Eval("fax") %>' /> </td>
                                                        <td colspan="3"> &nbsp; </td>
                                                    </tr>
                                                </table>                                                                    
                                            </dx:PanelContent>
                                        </PanelCollection>                                                        
                                        </dx:ASPxRoundPanel>
                                        <p class="SpacingPara"/>
                                        <dx:ASPxLabel ID="LabelOrgEditError" ClientInstanceName="LabelOrgEditError" runat="server" Text="" ClientVisible="false" ForeColor="Red" />
                                        <p class="SpacingPara"/>
                                        <dx:ASPxButton ID="ButtonDoAddOrgFrom" ClientInstanceName="ButtonDoAddOrgFrom" runat="server" AutoPostBack="False" Text="Зберегти">
                                            <ClientSideEvents Click="function (s, e) { CPPopUp.PerformCallback('edit_org:'); }" />
                                        </dx:ASPxButton>       
                                        </ItemTemplate>
                                        </asp:FormView>  
                                    </dx:PopupControlContentControl>
                                </ContentCollection>

                            </dx:ASPxPopupControl>
                        </dx:panelcontent>
                            </PanelCollection>
                            </dx:ASPxCallbackPanel>  


                            <dx:ASPxButton ID="BtnCreateRenterOrg" ClientInstanceName="BtnCreateRenterOrg" runat="server" AutoPostBack="False" Text="Створити">
                            </dx:ASPxButton>    
                            <p class="SpacingPara"/>
                            <dx:ASPxButton ID="BtnEditRenterOrg" ClientInstanceName="BtnEditRenterOrg" runat="server" AutoPostBack="False" Text="Редагувати">
                            </dx:ASPxButton> 
                                                  
                                                </td>

                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Орендодавець/Позичкодавець">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent14" runat="server">
                                        
                                        <table border="0" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td> <dx:ASPxLabel ID="ASPxLabel56" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                                                <td> <dx:ASPxTextBox ID="EditGiverOrgZKPO" ClientInstanceName="EditGiverOrgZKPO" runat="server" Width="200px" Title="Код ЄДРПОУ Орендодавця/позичкодавця" /> </td>
                                                <td> <dx:ASPxLabel ID="ASPxLabel57" runat="server" Text="Назва:" Width="50px" /> </td>
                                                <td> <dx:ASPxTextBox ID="EditGiverOrgName" ClientInstanceName="EditGiverOrgName" runat="server" Width="300px" Title="Назва Орендодавця/позичкодавця" /> </td>
                                                <td align="right">
                                                    <dx:ASPxButton ID="BtnFindGiverOrg" ClientInstanceName="BtnFindGiverOrg" runat="server" AutoPostBack="False" Text="Знайти">
                                                        <ClientSideEvents Click="function (s, e) { ComboGiverOrg.PerformCallback(EditGiverOrgZKPO.GetText() + '|' + EditGiverOrgName.GetText()); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxComboBox ID="ComboGiverOrg" ClientInstanceName="ComboGiverOrg" runat="server"
                                                        Width="806px" DataSourceID="SqlDataSourceOrgSearchGiver" ValueField="id" ValueType="System.Int32"
                                                        TextField="search_name" EnableSynchronization="True" OnCallback="ComboGiverOrg_Callback"
                                                        Title="Орендодавець/позичкодавець" Value='<%# Eval("org_giver_id") %>'>
                                                        <ClientSideEvents SelectedIndexChanged="OnComboGiverOrgSelIndexChanged" />
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxLabel ID="LabelGiverComment" ClientInstanceName="LabelGiverComment" runat="server" Text="Будь ласка, прокоментуйте причину вибору іншого орендодавця:" Width="100%" ClientVisible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxTextBox ID="EditGiverComment" ClientInstanceName="EditGiverComment" runat="server" Width="100%" ClientVisible="false" Title="Коментар щодо вибору Орендодавця">
                                                        <ClientSideEvents Validation="OnValidateEditGiverComment" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
В договорах оренди укладених після 01.10.2011 згідно з Рішенням №34/6250 орендодавцем виступає: <br/>
<br/>
а) ДКВ (для майна, що перебуває в сфері управління міста),<br/>
б) РДА (для майна, що перебуває в сфері управління району),<br/>
в) Балансоутримувач, за умови, що площа на балансі не перевищує 200 кв.м.
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Балансоутримувач">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent15" runat="server">
                                        <dx:ASPxTextBox ID="EditBalansHolderName" ReadOnly="true" runat="server" Text='<%# Eval("balans_name") %>' Width="810px" Title="Балансоутримувач" />
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelAgreement" runat="server" HeaderText="Реквізити договору">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Номер договору"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditAgreementNum" runat="server" Text='<%# Eval("agreement_num") %>' Width="190px" Title="Номер договору оренди" MaxLength="18">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true"></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td> &nbsp; </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Дата укладання договору"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="EditAgreementDate" runat="server" Value='<%# Eval("agreement_date") %>' Width="190px" Title="Дата укладання договору">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true"></ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Дата початку використання приміщення"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="EditStartDate" runat="server" Value='<%# Eval("rent_start_date") %>' Width="190px" Title="Дата початку оренди">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true"></ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                                <td> &nbsp; </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Закінчення договору"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="EditFinishDate" runat="server" Value='<%# Eval("rent_finish_date") %>' Width="190px" Title="Дата закінчення оренди"></dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Вид оплати"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboPaymentType" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="190px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePaymentType" Value='<%# Eval("payment_type_id") %>'
                                                        Title="Вид оплати">
                                                          <ClientSideEvents 
                                                            SelectedIndexChanged ="function (s, e) { HideValidator(); }"
                                                           />
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" > <RequiredField IsRequired="false" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td> &nbsp; </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Фактична дата закінчення договору"></dx:ASPxLabel></td>
                                                <td><dx:ASPxDateEdit ID="EditActualFinishDate" ClientInstanceName="clEditActualFinishDate" runat="server" Value='<%# Eval("rent_actual_finish_date") %>' Width="190px" Title="Фактична дата закінчення оренди" >
                                                     <ClientSideEvents 
                                                            DateChanged ="function (s, e) { HideValidator(); }"
                                                        />
                                                    </dx:ASPxDateEdit></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Площа що використовується всього, кв.м"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox ID="EditRentSquare" runat="server" Enabled="false" Width="190px" Title="Загальна орендована площа"/></td>
                                                <td> &nbsp; </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Кількість приміщень, що використовуються"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox ID="EditNumApt" runat="server" Width="190px" Enabled="false" Title="Кількість приміщень, що орендуються" /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Оціночна вартість приміщень за договором, грн"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCostExpert" runat="server" NumberType="Float" Value='<%# Eval("cost_expert_total") %>' Width="190px" Title="Ринкова вартість приміщень"  MinValue ="0" MaxValue="999999999"/></td>
                                                <td> &nbsp; </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Дата, на яку проведена оцінка об'єкту"></dx:ASPxLabel></td>
                                                <td><dx:ASPxDateEdit ID="EditDateExpert" runat="server" Value='<%# Eval("date_expert") %>' Width="190px" Title="Дата незалежної оцінки об'єкту оренди" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Підстави для продовження використання"></dx:ASPxLabel>

                                                    <dx:ASPxMemo ID="MemoProlongationComment" runat="server" Text='<%# Eval("note") %>' Width="100%" Height="45px" Title="Пояснення для продовження терміну дії оренди" MaxLength="255" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxCheckBox ID="CheckSubarenda" runat="server" Text="Суборенда" Checked='<%# 1.Equals(Eval("is_subarenda")) %>' Title="Суборенда" ReadOnly="true" />
                                                </td>
                                                <td> &nbsp; </td>
                                                <td colspan="2">
                                                    <dx:ASPxRadioButton ID="RadioAgreementActive" runat="server" GroupName="AgreementState" Value='<%# 1.Equals(Eval("agr_active")) %>' Text="Договір діє"
                                                        Title="Договір діє" >
                                                        <ClientSideEvents 
                                                            CheckedChanged ="function (s, e) { HideValidator(); }"
                                                        />
                                                        </dx:ASPxRadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxCheckBox ID="CheckLoanAgreement" ClientInstanceName="clCheckLoanAgreement" runat="server"  Text="Договір позички" Checked='<%# 1.Equals(Eval("is_loan_agreement")) %>' Title="Договір позички" Font-Bold="true" />
                                                </td>
                                                <td> &nbsp; </td>
                                                <td colspan="2">
                                                    <dx:ASPxRadioButton ID="RadioAgreementToxic" ClientInstanceName="clRadioAgreementToxic" runat="server" GroupName="AgreementState" Value='<%# 1.Equals(Eval("agr_toxic")) %>'
                                                        Text="Договір закінчився" Title="Договір закінчився (заборгованності немає або не погашено і видаляти не можна)"> 
                                                        <ClientSideEvents 
                                                            CheckedChanged ="function (s, e) { HideValidator(); }"
                                                        />
                                                    </dx:ASPxRadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3"></td>
                                                <td colspan="2">
                                                    <dx:ASPxRadioButton ID="RadioAgreementContinuedByAnother" runat="server" GroupName="AgreementState" Value='<%# 1.Equals(Eval("agr_continued_by_another")) %>'
                                                        Text="Договір закінчився, оренда продовжена іншим договором" Title="Договір закінчився, оренда продовжена іншим договором" /> 
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                        </ItemTemplate>
                    </asp:FormView>

                    <p class="SpacingPara"/>

                    <asp:FormView runat="server" BorderStyle="None" ID="InsuranceForm" DataSourceID="SqlDataSourceRentAgreement" EnableViewState="False">
                        <ItemTemplate>
                            <dx:ASPxRoundPanel ID="InsurancePanel" runat="server" HeaderText="Відомості про страхування">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent7" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxCheckBox ID="is_insured" ClientInstanceName="is_insured" runat="server" Text="Страхування" Checked='<%# 1.Equals(Eval("is_insured")) %>' Title="Страхування">
                                                        <ClientSideEvents CheckedChanged="EnableInsuranceControls" />
                                                    </dx:ASPxCheckBox>
                                                </td>
                                                 <td><dx:ASPxLabel ID="lbl_insurance_sum" ClientInstanceName="lbl_insurance_sum" runat="server" Text="Вартість об'єкту страхування"></dx:ASPxLabel></td>
                                                <td align="right"">
                                                    <dx:ASPxSpinEdit ID="insurance_sum" ClientInstanceName="insurance_sum" runat="server" NumberType="Float" Value='<%# Eval("insurance_sum") %>' Width="150px" Title="Сума страхування">
                                                        <ClientSideEvents Validation="OnValidateInsuranceSum" />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="lbl_insurance_start" ClientInstanceName="lbl_insurance_start" runat="server" Text="Дата початку періоду страхування"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="insurance_start" ClientInstanceName="insurance_start" runat="server" Value='<%# Eval("insurance_start") %>' Width="150px" Title="Дата початку періоду страхування">    
                                                        <ClientSideEvents Validation="OnValidateInsuranceStart" />                                                    
                                                    </dx:ASPxDateEdit>
                                                </td>
                                                <td><dx:ASPxLabel ID="lbl_insurance_end" ClientInstanceName="lbl_insurance_end" runat="server" Text="Дата закінчення періоду страхування"></dx:ASPxLabel></td>
                                                <td align="right"">
                                                    <dx:ASPxDateEdit ID="insurance_end" ClientInstanceName="insurance_end" runat="server" Value='<%# Eval("insurance_end") %>' Width="150px" Title="Дата закінчення періоду страхування"> 
                                                        <ClientSideEvents Validation="OnValidateInsuranceEnd" />                                                                
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </ItemTemplate>
                    </asp:FormView>

                    <p class="SpacingPara"/>

                    <asp:FormView runat="server" BorderStyle="None" ID="FormViewState" DataSourceID="SqlDataSourceRentAgreement" EnableViewState="False">
                        <ItemTemplate>
                            <dx:ASPxRoundPanel ID="StatePanel" runat="server" HeaderText="Стан">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent7" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text='<%# EvaluateSignature(Eval("modified_by"), Eval("modify_date")) %>'></dx:ASPxLabel></td>

                                                <td align="right">
                                                    <dx:ASPxPopupControl ID="PopupArchiveStates" runat="server" 
                                                        HeaderText="Архівні стани" 
                                                        ClientInstanceName="PopupArchiveStates" 
                                                        PopupElementID="CardPageControl"
                                                        PopupAction="None"
                                                        PopupHorizontalAlign="Center"
                                                        PopupVerticalAlign="Middle"
                                                        PopupAnimationType="Slide">
                                                        <ContentCollection>
                                                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            
                                                                <dx:ASPxGridView ID="GridViewArendaArchiveStates" ClientInstanceName="GridViewArendaArchiveStates" runat="server"
                                                                    AutoGenerateColumns="False" DataSourceID="SqlDataSourceArendaArchive" KeyFieldName="archive_id" Width="780px">

                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn FieldName="archive_id" VisibleIndex="0" Caption="Картка Архівного Стану">
                                                                            <DataItemTemplate>
                                                                                <%# "<center><a href=\"javascript:ShowArendaArchiveCard(" + Eval("archive_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
                                                                            </DataItemTemplate>
                                                                            <Settings ShowInFilterControl="False"/>
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn FieldName="org_renter_full_name" VisibleIndex="1" Caption="Орендар" Width="150px"/>
                                                                        <dx:GridViewDataTextColumn FieldName="object_name" VisibleIndex="2" Caption="Використання Приміщення" Width="150px"/>
                                                                        <dx:GridViewDataTextColumn FieldName="agreement_num" VisibleIndex="3" Caption="Номер Договору"/>
                                                                        <dx:GridViewDataDateColumn FieldName="agreement_date" VisibleIndex="4" Caption="Дата Договору"/>
                                                                        <dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="5" Caption="Початок Оренди"/>
                                                                        <dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="6" Caption="Закінчення Оренди"/>
                                                                        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="7" Caption="Площа (кв.м.)"/>
                                                                        <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="8" Caption="Ким Внесені Зміни"></dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="9" Caption="Дата Внесення Змін"></dx:GridViewDataDateColumn>
                                                                    </Columns>

                                                                    <SettingsBehavior ColumnResizeMode="Control" EnableCustomizationWindow="False" />
                                                                    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                                                    <SettingsPager PageSize="10" />
                                                                    <Styles Header-Wrap="True" />
                                                                    <SettingsCookies CookiesID="GUKV.ArendaCard.ArchiveStates" Enabled="False" Version="A2_1" />

                                                                    <ClientSideEvents EndCallback="function (s,e) { GridViewArendaArchiveStates.SetHeight(500); }"/>
                                                                </dx:ASPxGridView>

                                                            </dx:PopupControlContentControl>
                                                        </ContentCollection>

                                                        <ClientSideEvents PopUp="function (s,e) { GridViewArendaArchiveStates.SetHeight(500); }"/>
                                                    </dx:ASPxPopupControl>

                                                    <dx:ASPxButton ID="ASPxButtonArchive" ClientInstanceName="ASPxButtonArchive" runat="server" Text="Архівні Стани" AutoPostBack="false"
                                                        ClientVisible='<%# IsHistoryButtonVisible() %>' >
                                                        <ClientSideEvents Click="function (s,e) { PopupArchiveStates.Show(); }" />
                                                    </dx:ASPxButton>
                                                </td>

                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Підстави на використання" Name="Tab2">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server">

                    <dx:ASPxCallbackPanel ID="CPDecisions" ClientInstanceName="CPDecisions" runat="server" OnCallback="CPDecisions_Callback">
                        <PanelCollection>
                            <dx:panelcontent ID="Panelcontent2" runat="server">

                                <dx:ASPxGridView ID="GridViewDecisions" ClientInstanceName="GridViewDecisions" runat="server" AutoGenerateColumns="False" 
                                    KeyFieldName="id" Width="810px"
                                    OnRowDeleting="GridViewDecisions_RowDeleting"
                                    OnRowUpdating="GridViewDecisions_RowUpdating" >

	                                <SettingsCommandButton>
		                                <EditButton>
			                                <Image Url="~/Styles/EditIcon.png" ToolTip="Редагувати Підставу" />
		                                </EditButton>
		                                <CancelButton>
			                                <Image Url="~/Styles/CancelIcon.png" />
		                                </CancelButton>
		                                <UpdateButton>
			                                <Image Url="~/Styles/SaveIcon.png" />
		                                </UpdateButton>
		                                <DeleteButton>
			                                <Image Url="~/Styles/DeleteIcon.png" ToolTip="Видалити Підставу" />
		                                </DeleteButton>
		                                <NewButton>
			                                <Image Url="~/Styles/AddIcon.png" />
		                                </NewButton>
		                                <ClearFilterButton Text="Очистити" RenderMode="Link" />
	                                </SettingsCommandButton>

                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" 
                                            ShowEditButton="true" ShowDeleteButton="True" >
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="pidstava" VisibleIndex="1" Caption="Назва Документу" Width="200px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="doc_num" VisibleIndex="2" Caption="Номер Документу"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn FieldName="doc_date" VisibleIndex="3" Caption="Дата Документу"></dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="4" Caption="Площа за Документом"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="purpose_str" VisibleIndex="5" Caption="Призначення за Документом" Width="200px"></dx:GridViewDataTextColumn>
                                    </Columns>

                                    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
                                    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" ShowHeaderFilterButton="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                    <SettingsPager Mode="ShowAllRecords" PageSize="10" />
                                    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
                                    <Styles Header-Wrap="True" >
                                        <Header Wrap="True"></Header>
                                    </Styles>
                                    <SettingsCookies CookiesID="GUKV.Reports1NF.ArendaDecisions" Enabled="False" Version="A2" />

                                    <Templates>
                                        <EditForm>
                                            <table border="0" cellspacing="0" cellpadding="2">
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Назва документу" /> </td>
                                                    <td colspan="5">
                                                        <dx:ASPxComboBox ID="ComboPidstavaDocKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                                            IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDocKind" Text='<%# Eval("pidstava") %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="Label1" runat="server" Text="Номер документу" /> </td>
                                                    <td> <dx:ASPxTextBox ID="EditPidstavaDocNum" runat="server" Width="120px" Text='<%# Eval("doc_num")%>' /> </td>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="від" /> </td>
                                                    <td> <dx:ASPxDateEdit ID="EditPidstavaDocDate" runat="server" Width="120px" Value='<%# Eval("doc_date")%>' /> </td>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Площа за Документом, кв.м." /> </td>
                                                    <td> <dx:ASPxTextBox ID="EditPidstavaDocSquare" runat="server" Width="120px" Text='<%# Eval("rent_square")%>' /> </td>
                                                </tr>
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Призначення за Документом" /> </td>
                                                    <td colspan="5">
                                                        <dx:ASPxTextBox ID="EditPidstavaDocPurpose" runat="server" Width="100%" Text='<%# Eval("purpose_str")%>' MaxLength="252" />
                                                    </td>
                                                </tr>
                                            </table>

                                            <br/>

                                            <table>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Зберегти" AutoPostBack="false">
                                                            <ClientSideEvents Click="function (s, e) { OnSaveDecisionClick(s, e); }" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Відмінити" AutoPostBack="false">
                                                            <ClientSideEvents Click="function (s, e) { OnCancelDecisionClick(s, e); }" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>    
                                        </EditForm>
                                    </Templates>
                                </dx:ASPxGridView>

                            </dx:panelcontent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>

                    <p class="SpacingPara"/>

                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="ButtonAddDecision" runat="server" Text="Додати Підставу" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { CPDecisions.PerformCallback('add:');  }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Об'єкти за договором" Name="Tab3">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

                    <dx:ASPxCallbackPanel ID="CPNotes" ClientInstanceName="CPNotes" runat="server" OnCallback="CPNotes_Callback">
                        <PanelCollection>
                            <dx:panelcontent ID="Panelcontent3" runat="server">

                                <dx:ASPxGridView ID="GridViewNotes" ClientInstanceName="GridViewNotes" runat="server" AutoGenerateColumns="False" 
                                    KeyFieldName="id" Width="1410px"
                                    OnRowDeleting="GridViewNotes_RowDeleting"
                                    OnRowUpdating="GridViewNotes_RowUpdating" >

	                                <SettingsCommandButton>
		                                <EditButton>
			                                <Image Url="~/Styles/EditIcon.png" ToolTip="Редагувати Об'єкт" />
		                                </EditButton>
		                                <CancelButton>
			                                <Image Url="~/Styles/CancelIcon.png" />
		                                </CancelButton>
		                                <UpdateButton>
			                                <Image Url="~/Styles/SaveIcon.png" />
		                                </UpdateButton>
		                                <DeleteButton>
			                                <Image Url="~/Styles/DeleteIcon.png" ToolTip="Видалити Об'єкт" />
		                                </DeleteButton>
		                                <NewButton>
			                                <Image Url="~/Styles/AddIcon.png" />
		                                </NewButton>
		                                <ClearFilterButton Text="Очистити" RenderMode="Link" />
	                                </SettingsCommandButton>

                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="60px"
                                            ShowEditButton="true" ShowDeleteButton="True" >
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="1" Caption="Площа приміщення, кв.м" Width="100px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="invent_no" VisibleIndex="2" Caption="Інвентарний номер" Width="100px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="note" VisibleIndex="3" Caption="Розташування приміщення (поверх)" Width="120px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataComboBoxColumn FieldName="purpose_group_id" VisibleIndex="4" Caption="Група призначення" Width="140px">
                                            <PropertiesComboBox DataSourceID="SqlDataSourcePurposeGroup" ValueField="id" TextField="name" ValueType="System.Int32" />
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataComboBoxColumn FieldName="purpose_id" VisibleIndex="5" Caption="Призначення за класифікацією" Width="140px">
                                            <PropertiesComboBox DataSourceID="SqlDataSourcePurpose" ValueField="id" TextField="name" ValueType="System.Int32" />
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataTextColumn FieldName="purpose_str" VisibleIndex="6" Caption="Використання згідно з договором" Width="140px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cost_expert_total" VisibleIndex="7" Caption="Ринкова вартість приміщення, грн." Width="100px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn FieldName="date_expert" VisibleIndex="8" Caption="Дата оцінки"></dx:GridViewDataDateColumn>
<%--                                        <dx:GridViewDataComboBoxColumn FieldName="payment_type_id" VisibleIndex="9" Caption="Вид оплати" Width="140px">
                                            <PropertiesComboBox DataSourceID="SqlDataSourcePaymentType" ValueField="id" TextField="name" ValueType="System.Int32" />
                                        </dx:GridViewDataComboBoxColumn>
--%>
                                        <dx:GridViewDataTextColumn FieldName="cost_narah" VisibleIndex="10" Caption="Ставка за використання, %" Width="85px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="rent_rate" VisibleIndex="11" Caption="Орендна плата за 1 кв.м, грн." Width="90px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="cost_agreement" VisibleIndex="12" Caption="Місячна орендна плата, грн." Width="85px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataComboBoxColumn FieldName="note_status_id" VisibleIndex="13" Caption="Стан використання приміщення" Width="140px">
                                            <PropertiesComboBox DataSourceID="SqlDataSourceArendaNoteStatus" ValueField="id" TextField="name" ValueType="System.Int32" />
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataComboBoxColumn FieldName="payment_type_id" VisibleIndex="14" Caption="Цільове використання майна" Width="140px">
                                            <PropertiesComboBox DataSourceID="SqlDataSourceDictRentalRate" ValueField="id" TextField="short_name" ValueType="System.Int32" />
                                        </dx:GridViewDataComboBoxColumn>
                                    </Columns>

                                    <TotalSummary>
                                        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />
                                        <dx:ASPxSummaryItem FieldName="cost_expert_total" SummaryType="Sum" DisplayFormat="{0}" />
                                        <dx:ASPxSummaryItem FieldName="rent_rate" SummaryType="Sum" DisplayFormat="{0}" />
                                        <dx:ASPxSummaryItem FieldName="cost_agreement" SummaryType="Sum" DisplayFormat="{0}" />
                                    </TotalSummary>

                                    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
                                    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" ShowHeaderFilterButton="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                    <SettingsPager Mode="ShowAllRecords" PageSize="10" />
                                    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
                                    <Styles Header-Wrap="True" >
                                        <Header Wrap="True"></Header>
                                    </Styles>
                                    <SettingsCookies CookiesID="GUKV.Reports1NF.ArendaNotes" Enabled="False" Version="A2" />

                                    <Templates>
                                        <EditForm>
                                            <table border="0" cellspacing="0" cellpadding="2">
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Площа приміщення, що використовується, кв.м" Width="150px" /> </td>
                                                    <td> <dx:ASPxTextBox ID="EditNoteSqr" ClientInstanceName="EditNoteSqr" runat="server" Width="200px" Text='<%# Eval("rent_square")%>' /> </td>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Розташування приміщення (поверх)" Width="150px" /> </td>
                                                    <td> <dx:ASPxTextBox ID="EditNoteFloor" runat="server" Width="200px" Text='<%# Eval("note")%>' MaxLength="252" /> </td>
                                                </tr>
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel45" runat="server" Text="Інвентарний номер" Width="150px" /> </td>
                                                    <td> <dx:ASPxTextBox ID="EditNoteInventNo" runat="server" Width="200px" Text='<%# Eval("invent_no")%>' MaxLength="128" /> </td>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel46" runat="server" Text="Cтан використання приміщення" Width="150px" /> </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="ComboNoteCurState" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="200px" 
                                                            IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceArendaNoteStatus" Value='<%# Eval("note_status_id") %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Група призначення" Width="150px" /> </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="ComboNotePurposeGroup" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="200px" 
                                                            IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePurposeGroup" Value='<%# Eval("purpose_group_id") %>' />
                                                    </td>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Призначення за класифікацією" Width="150px" /> </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="ComboNotePurpose" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="200px" 
                                                            IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePurpose" Value='<%# Eval("purpose_id") %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Використання згідно з договором" Width="150px" /> </td>
                                                    <td colspan="3">
                                                        <dx:ASPxTextBox ID="EditNotePurposeStr" runat="server" Width="100%" Text='<%# Eval("purpose_str")%>' MaxLength="252" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="Label1" runat="server" Text="Ринкова вартість приміщення, грн." Width="150px" /> </td>
                                                    <td> <dx:ASPxTextBox ID="EditNoteCostExpert" runat="server" Width="200px" Text='<%# Eval("cost_expert_total")%>' /> </td>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Дата оцінки" Width="150px" /> </td>
                                                    <td> <dx:ASPxDateEdit ID="EditNoteDateExpert" runat="server" Width="200px" Value='<%# Eval("date_expert")%>' /> </td>
                                                </tr>
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel52" runat="server" Text="Цільове використання майна" Width="150px" /> </td>
                                                    <td colspan="3">
                                                        <dx:ASPxComboBox ID="ComboNotePaymentType1" ClientInstanceName="ComboNotePaymentType1" runat="server" ValueType="System.Int32" TextField="short_name" ValueField="id" Width="600px" 
                                                            IncrementalFilteringMode="Contains" DataSourceID="SqlDataSourceDictRentalRate" Value='<%# Eval("payment_type_id") %>' >
                                                        <ClientSideEvents SelectedIndexChanged="OnRentalRateChanged" />
                                                        
                                                    </dx:ASPxComboBox>
                                                        <dx:ASPxComboBox ID="ComboRentalRate" ClientInstanceName="ComboRentalRate" runat="server" ValueType="System.Int32" TextField="rental_rate" ValueField="id" Width="60px"  ClientVisible = "false"
                                                            IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDictRentalRate" Value='<%# Eval("payment_type_id") %>' > 
                                                    </dx:ASPxComboBox>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Ставка за використання, %" Width="150px" /> </td>
                                                    <td> <dx:ASPxTextBox ID="EditNoteCostNarah" ClientInstanceName="EditNoteCostNarah" runat="server" Width="200px" Text='<%# Eval("cost_narah")%>' Title="Ставка за використання, %"  /> </td>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Орендна плата за 1 кв.м, грн." Width="150px" /> </td>
                                                    <td> <dx:ASPxTextBox ID="EditNoteRentRate" runat="server" Width="200px" Text='<%# Eval("rent_rate")%>' /> </td>
                                                </tr>
                                                <tr>
                                                    <td> <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Місячна орендна плата, грн." Width="150px" /> </td>
                                                    <td> <dx:ASPxTextBox ID="EditNoteCostAgreement" runat="server" Width="200px" Text='<%# Eval("cost_agreement")%>' /> </td>
<%--                                                    <td> <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Вид оплати" Width="150px" /> </td>
                                                    <td> 
                                                        <dx:ASPxComboBox ID="ComboNotePaymentType" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="200px" 
                                                            IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePaymentType" Value='<%# Eval("payment_type_id") %>' />
                                                    </td>
--%>
                                                </tr>
                                            </table>

                                            <br/>
                                            
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Зберегти" AutoPostBack="false">
                                                            <ClientSideEvents Click="function (s, e) { OnSaveNoteClick(s, e); }" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Відмінити" AutoPostBack="false">
                                                            <ClientSideEvents Click="function (s, e) { OnCancelNoteClick(s, e); }" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>     
                                        </EditForm>
                                    </Templates>

                                </dx:ASPxGridView>

                            </dx:panelcontent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>

                    <p class="SpacingPara"/>

                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="ButtonAddNote" runat="server" Text="Додати Об'єкт" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { CPNotes.PerformCallback('add:');  }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Плата за використання" Name="Tab4">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server">
                    <asp:FormView runat="server" BorderStyle="None" ID="PaymentForm" DataSourceID="SqlDataSourceRentPayment" EnableViewState="False">
                        <ItemTemplate>
                            <div style="margin-bottom: 10px;">
                                <div style="float: left; margin-right: 10px; line-height: 1.8em;">
                                    <dx:ASPxLabel ID="ReportingPeriodLabel" runat="server" Text="Звітний Період:"></dx:ASPxLabel>
                                </div>
                                <div style="float: left; margin-right: 10px; line-height: 1.8em;">
                                <dx:ASPxComboBox ID="ReportingPeriodCombo" runat="server" 
                                    DataSourceID="SqlDataSourceRentPeriods" TextField="name" ValueField="id" 
                                    ValueType="System.Int32" Value='<%# Eval("rent_period_id") %>'
                                    Title="період поточного року, за який обраховується та подається квартальний звіт: 3 місяці 2020, 6 місяців 2020, 9 місяців 2020, 12 місяців 2020" 
                                    ClientInstanceName="ReportingPeriodCombo">
                                    <ClientSideEvents 
                                        SelectedIndexChanged="function (s, e) { updateReportingPeriodComboStyles(); }"
                                    />
                                </dx:ASPxComboBox>
                                    </div>
                                <dx:ASPxLabel ID="NeededPeriodCombo"  runat="server"  ForeColor="White" />
                                
                            </div>
                            <dx:ASPxRoundPanel ID="PanelRentPayment" runat="server" HeaderText="Площа, надана в оренду">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent4" runat="server">                                        
                                        <table border="0" cellspacing="0" cellpadding="2" width="910px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Площа, надана в оренду, всього, кв.м." Width="650px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox ID="EditPaymentSqrTotal_orndpymnt" runat="server" Enabled="false" Width="150px" Title="Загальна площа, надана в оренду" /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="- у тому числі, на яку нараховується плата за використання у відсотках від вартості, кв.м."></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditPaymentSqrByPercent_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("sqr_payed_by_percent") %>' Width="150px"
                                                    Title="Площа, на яку нараховується плата за використання у відсотках від вартості"/></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="- у тому числі, на яку нараховується плата за використання в розмірі 1 грн., кв.м."></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditPaymentSqr1UAH_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("sqr_payed_by_1uah") %>' Width="150px"
                                                    Title="Площа, на яку нараховується плата за використання в розмірі 1 грн."/></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="- у тому числі, надана в погодинну оренду, чи відповідно до угод про співпрацю, кв.м."></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditPaymentSqrHourly_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("sqr_payed_hourly") %>' Width="150px"
                                                    Title="Площа, надана в погодинну оренду, чи відповідно до угод про співпрацю"/></td>
                                            </tr>   
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelRentPaymentDocuments" runat="server" HeaderText="Надходження орендної плати">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent9" runat="server">
                                        <dx:ASPxCallbackPanel ID="CPRentPayment" ClientInstanceName="CPRentPayment" runat="server" OnCallback="CPRentPayment_Callback">
                                            <PanelCollection>
                                            <dx:panelcontent ID="Panelcontent12" runat="server">
                                                <table border="0" cellspacing="0" cellpadding="2" width="910px">
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel65" runat="server" Text="Авансова орендна плата (нарахована), грн."></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="edit_avance_plat" ClientInstanceName="edit_avance_plat" runat="server" NumberType="Float" Value='<%# Eval("avance_plat") %>' Width="150px"
                                                            Title="двомісячна сума орендної плати у договорі, яка може бути використана ЛИШЕ для оплати використання останніх двох місяців договору">
                                                            <ClientSideEvents 
                                                                LostFocus="CalcCollectionDebtZvit" />                                                            
                                                            </dx:ASPxSpinEdit>
                                                            </td>
                                                    </tr>
                                                    <tr style="display:none" id="tr_alert_edit_avance_plat">
                                                        <td colspan="2" align="right" style="padding-right:20px"><dx:ASPxLabel ID="ASPxLabel67" ForeColor="Brown" runat="server" Text="Внесення авансової орендної плати одноразова операція, Ви повинні вирахувати її з поля «Надходження орендної плати за звітний період, всього, грн.»"></dx:ASPxLabel></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Нараховано орендної плати за звітний період, грн. (без ПДВ)" Width="650px"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentNarah_orndpymnt" ClientInstanceName="clEditPaymentNarah_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("payment_narah") %>' Width="150px"
                                                            Title="нарахована орендна плата за 3,6,9,12 місяців відповідно (індексація може враховуватися)">
                                                              <ClientSideEvents 
                                                            ValueChanged ="function (s, e) { HideValidator(); }"
                                                            LostFocus="CalcCollectionDebtZvit"
                                                            />
                                                            </dx:ASPxSpinEdit></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel66" runat="server" Text="- у тому числі, знято надмірно нарахованої за звітний період"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="edit_znyato_nadmirno_narah" ClientInstanceName="edit_znyato_nadmirno_narah" runat="server" NumberType="Float" Value='<%# Eval("znyato_nadmirno_narah") %>' Width="150px"
                                                            Title="сума що не підлягає оплаті, відповідно до акту про неможливість використання приміщення і вираховується з [Нараховано]">
                                                            <ClientSideEvents 
                                                                LostFocus="CalcCollectionDebtZvit" />                                                            
                                                            </dx:ASPxSpinEdit>
                                                            </td>
                                                    </tr>

                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel71" runat="server" Text="- у тому числі, використано авансової плати"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="edit_znyato_from_avance" ClientInstanceName="edit_znyato_from_avance" runat="server" NumberType="Float" Value='<%# Eval("znyato_from_avance") %>' Width="150px"
                                                            Title="якщо використовується, то до закінчення договору залишилося меньше 2-х місяців">
                                                            <ClientSideEvents 
                                                                LostFocus="CalcCollectionDebtZvit" />                                                            
                                                            </dx:ASPxSpinEdit>
                                                            </td>
                                                    </tr>

                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Сальдо (переплата) на початок року (незмінна впродовж року величина), грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentSaldo_orndpymnt" ClientInstanceName="clEditPaymentSaldo_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("last_year_saldo") %>' Width="150px"
                                                            Title="переплата орендарем коштів на початок звітного року,  використовується для покриття (повністю або частково) у разі виникненню заборгованості по нарахованій орендній платі за звітний період  (НЕ ВКЛЮЧАЄ АВАНСОВІ ПЛАТЕЖІ)">
                                                            <ClientSideEvents 
                                                                LostFocus="CalcCollectionDebtZvit" />                                                            
                                                            </dx:ASPxSpinEdit>
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel69" runat="server" Text="Сальдо авансової орендної плати на початок року (незмінна впродовж року величина), грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="Edit_avance_saldo" ClientInstanceName="avance_saldo" runat="server" NumberType="Float" Value='<%# Eval("avance_saldo") %>' Width="150px"
                                                            Title="отримана, до початку звітного періоду, сума авансової орендної плати орендної плати за даним договором">
                                                            <ClientSideEvents 
                                                                LostFocus="CalcCollectionDebtZvit" />                                                            
                                                            </dx:ASPxSpinEdit>
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Надходження орендної плати за звітний період, всього, грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentReceived_orndpymnt" ClientInstanceName="Received_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("payment_received") %>' Width="150px"
                                                            Title="загальна отримана протягом поточного звітного періоду сума коштів від орендаря (у т.ч. переплата протягом звітного періоду, погашення боргів, авансова плата)" MinValue ="0" MaxValue="999999999">
                                                            <ClientSideEvents 
                                                                LostFocus="CalcCollectionDebtZvit" />   
                                                            </dx:ASPxSpinEdit>
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <dx:ASPxButton ID="BtnAddPaymentDocument" ClientInstanceName="clientBtnAddPaymentDocument" runat="server" Text="Розрахувати" AutoPostBack="false" Width="150px">
                                                                <ClientSideEvents Click="function (s,e) { CPRentPayment.PerformCallback('calc:');  }" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="- у тому числі, з нарахованої за звітний період (без боргів та переплат)"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentNarZvit_orndpymnt"  ClientInstanceName="Zvit_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("payment_nar_zvit") %>' Width="150px"
                                                            Title="отримана протягом поточного звітного періоду сума коштів від орендаря призначена  лише на оплату нарахованої за звітний період орендної плати" MinValue ="0" MaxValue="999999999">
                                                                <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                            </dx:ASPxSpinEdit></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel68" runat="server" Text="- у тому числі, з нарахованої авансової орендної плати, грн."></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="Edit_avance_paymentnar"  ClientInstanceName="avance_paymentnar" runat="server" NumberType="Float" Value='<%# Eval("avance_paymentnar") %>' Width="150px"
                                                            Title="отримана протягом поточного звітного періоду сума коштів від орендаря призначена  лише на оплату нарахованої авансової орендної плати" MinValue ="0" MaxValue="999999999">
                                                                <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                            </dx:ASPxSpinEdit></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel55" runat="server" Text="- у тому числі, погашення заборгованості минулих періодів, грн."></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentOldDebtsPayed_orndpymnt" ClientInstanceName="clEditPaymentOldDebtsPayed_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("old_debts_payed") %>' Width="150px"
                                                            Title="частина коштів з надходження орендної плати за звітний період, всього, призначена на погашення заборгованості попередніх періодів">
                                                                <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                            </dx:ASPxSpinEdit></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel62" runat="server" Text="- у тому числі, переплата орендної плати за звітний період, грн."></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="edit_return_orend_payed" ClientInstanceName="edit_return_orend_payed" runat="server" NumberType="Float" Value='<%# Eval("return_orend_payed") %>' Width="150px" ReadOnly="true"
                                                            Title="залишок орендної плати після оплати нарахованої орендної плати, нарахованої авансової орендної плати,  погашення заборгованості попередніх періодів. (не включає &quot;Сальдо (переплата) на початок року&quot;)"/></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel72" runat="server" Text="Переплата орендної плати всього, грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="edit_total_pereplata" ClientInstanceName="edit_total_pereplata" runat="server" NumberType="Float"  Width="150px" ReadOnly="true"
                                                            Title="Переплата орендної плати всього, грн. (без ПДВ)"/></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel63" runat="server" Text="Повернення переплати орендної плати всього у звітному періоді, грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="edit_return_all_orend_payed" ClientInstanceName="edit_return_all_orend_payed" runat="server" NumberType="Float" Value='<%# Eval("return_all_orend_payed") %>' Width="150px" 
                                                            Title="повернення переплати орендної плати орендарю за його вимогою або по завершенні терміну договору">
                                                                <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                            </dx:ASPxSpinEdit>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel64" runat="server" Text="Розраховувати заборгованість з орендної плати"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxCheckBox ID="edit_use_calc_debt" ClientInstanceName="edit_use_calc_debt" runat="server" Text="" Checked='<%# 1.Equals(Eval("use_calc_debt")) %>' Title="Розраховувати заборгованість з орендної плати">
                                                                <ClientSideEvents CheckedChanged="CalcCollectionDebtZvit" />
                                                            </dx:ASPxCheckBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </dx:panelcontent>    
                                            </PanelCollection>                                                    
                                        </dx:ASPxCallbackPanel>
                                    </dx:PanelContent>
                                </PanelCollection>                                
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="AdditionalInfoPanel" runat="server" HeaderText="Знижка">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent11" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="2" width="910px">
                                            <tr style="visibility:collapse">
                                                <td colspan="7">
                                                    <dx:ASPxCheckBox ID="CheckRenterIsOut" runat="server" Text="Знижка" Checked='<%# 1.Equals(Eval("is_discount")) %>' Title="Знижка" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel runat="server" Text="Звільнено від сплати орендної плати на"></dx:ASPxLabel></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="%"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxSpinEdit ID="edit_zvilneno_percent" runat="server" NumberType="Float" Value='<%# Eval("zvilneno_percent") %>' Width="100px" Title="%"/></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="з"></dx:ASPxLabel></td>
												<td align="left"><dx:ASPxDateEdit ID="edit_zvilneno_date1" runat="server" NumberType="Float" Value='<%# Eval("zvilneno_date1") %>' Width="100px" Title="з"/></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="по"></dx:ASPxLabel></td>
												<td align="left"><dx:ASPxDateEdit ID="edit_zvilneno_date2" runat="server" NumberType="Float" Value='<%# Eval("zvilneno_date2") %>' Width="100px" Title="по"/></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel runat="server" Text="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332"></dx:ASPxLabel></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="%"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxSpinEdit ID="edit_zvilbykmp_percent" runat="server" NumberType="Float" Value='<%# Eval("zvilbykmp_percent") %>' Width="100px" Title="%"/></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="з"></dx:ASPxLabel></td>
												<td align="left"><dx:ASPxDateEdit ID="edit_zvilbykmp_date1" runat="server" NumberType="Float" Value='<%# Eval("zvilbykmp_date1") %>' Width="100px" Title="з"/></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="по"></dx:ASPxLabel></td>
												<td align="left"><dx:ASPxDateEdit ID="edit_zvilbykmp_date2" runat="server" NumberType="Float" Value='<%# Eval("zvilbykmp_date2") %>' Width="100px" Title="по"/></td>
                                            </tr>
                                            <tr>
                                                <td colspan="1"><dx:ASPxLabel runat="server" Text="Повідомлення орендаря до балансоутримувача про неможлівість використання"></dx:ASPxLabel></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="дата"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxDateEdit ID="edit_povidoleno1_date" runat="server" Value='<%# Eval("povidoleno1_date") %>' Width="100px" Title="дата"/></td>
                                                <td align="right"><dx:ASPxLabel runat="server" Text="№"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxTextBox ID="edit_povidoleno1_num" runat="server" Text='<%# Eval("povidoleno1_num") %>' Width="100px" Title="№" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="1"><dx:ASPxLabel runat="server" Text="Повідомлення орендаря до орендодавця про неможлівість використання"></dx:ASPxLabel></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="дата"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxDateEdit ID="edit_povidoleno2_date" runat="server" Value='<%# Eval("povidoleno2_date") %>' Width="100px" Title="дата"/></td>
                                                <td align="right"><dx:ASPxLabel runat="server" Text="№"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxTextBox ID="edit_povidoleno2_num" runat="server" Text='<%# Eval("povidoleno2_num") %>' Width="100px" Title="№" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="1"><dx:ASPxLabel runat="server" Text="Повідомлення орендаря до балансоутримувача про намір використовувати об'єкт"></dx:ASPxLabel></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="дата"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxDateEdit ID="edit_povidoleno3_date" runat="server" Value='<%# Eval("povidoleno3_date") %>' Width="100px" Title="дата"/></td>
                                                <td align="right"><dx:ASPxLabel runat="server" Text="№"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxTextBox ID="edit_povidoleno3_num" runat="server" Text='<%# Eval("povidoleno3_num") %>' Width="100px" Title="№" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="1"><dx:ASPxLabel runat="server" Text="Повідомлення орендаря до орендодавця про намір використовувати об'єкт"></dx:ASPxLabel></td>
												<td align="right"><dx:ASPxLabel runat="server" Text="дата"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxDateEdit ID="edit_povidoleno4_date" runat="server" Value='<%# Eval("povidoleno4_date") %>' Width="100px" Title="дата"/></td>
                                                <td align="right"><dx:ASPxLabel runat="server" Text="№"></dx:ASPxLabel></td>
                                                <td align="left"><dx:ASPxTextBox ID="edit_povidoleno4_num" runat="server" Text='<%# Eval("povidoleno4_num") %>' Width="100px" Title="№" /></td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>                                
                            </dx:ASPxRoundPanel>
                        </ItemTemplate>
                    </asp:FormView>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Претензійна робота" Name="Tab5">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server">
                    <asp:FormView runat="server" BorderStyle="None" ID="CollectionForm" DataSourceID="SqlDataSourceRentPayment" EnableViewState="False">
                        <ItemTemplate>
                            <dx:ASPxCheckBox ID="CheckNoDebt" ClientInstanceName="CheckNoDebt" runat="server" Text="Заборгованості немає" Checked='<%# 0.Equals(Eval("is_debt_exists")) %>'
                                Title="Заборгованості немає">
                                <ClientSideEvents CheckedChanged="EnableCollectionControls" />
                            </dx:ASPxCheckBox>
                            <p class="SpacingPara"/>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <dx:ASPxRoundPanel ID="PanelCollection" runat="server" HeaderText="Заборгованість по орендній платі">
                                            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent4" runat="server">
                                                    <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Заборгованість по орендній платі, грн. (без ПДВ):"></dx:ASPxLabel></td>
                                                        </tr>
                                                        <tr>
                                                            <td><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="- всього" Width="280px"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebtTotal" ClientInstanceName="EditCollectionDebtTotal" runat="server" NumberType="Float" Value='<%# Eval("debt_total") %>' Width="100px" ReadOnly = "true"
                                                                Title="є сумою всіх полів заборгованості і не підлягає редагуванню">
                                                                </dx:ASPxSpinEdit></td>
                                                            <td><div style="width:30px;"></div></td>
                                                            <td><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="- за звітний період" Width="280px"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebtZvit" ClientInstanceName="EditCollectionDebtZvit" runat="server" NumberType="Float" Value='<%# Eval("debt_zvit") %>' Width="100px" ReadOnly = "true"
                                                                Title="заборгованість оплати нарахованої орендної плати лише за звітний період"/></td>
                                                        </tr>
                                                        <tr>
                                                            <td><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="- поточна до 3-х місяців"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebt3Month" ClientInstanceName="EditCollectionDebt3Month" runat="server" NumberType="Float" Value='<%# Eval("debt_3_month") %>' Width="100px" ReadOnly = "true"
                                                                Title="редагуються балансоутримувачем, тільки через корекцію відповідних показників таблиці &quot;Заборгованість по кварталах&quot; і обраховується автоматично">
                                                                <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                            </dx:ASPxSpinEdit></td>
                                                            <td></td>
                                                            <td><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="- прострочена від 4 до 12 місяців"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebt12Month" ClientInstanceName="EditCollectionDebt12Month" runat="server" NumberType="Float" Value='<%# Eval("debt_12_month") %>' Width="100px" ReadOnly = "true"
                                                                Title="редагуються балансоутримувачем, тільки через корекцію відповідних показників таблиці &quot;Заборгованість по кварталах&quot; і обраховується автоматично">
                                                                <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                            </dx:ASPxSpinEdit></td>
                                                        </tr>
                                                        <tr>
                                                            <td><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="- прострочена від 1 до 3 років"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebt3Years" ClientInstanceName="EditCollectionDebt3Years" runat="server" NumberType="Float" Value='<%# Eval("debt_3_years") %>' Width="100px" ReadOnly = "true"
                                                                Title="редагуються балансоутримувачем, тільки через корекцію відповідних показників таблиці &quot;Заборгованість по кварталах&quot; і обраховується автоматично">
                                                                <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                            </dx:ASPxSpinEdit></td>
                                                            <td></td>                                                
                                                            <td><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="- безнадійна більше 3-х років"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebtOver3Years" ClientInstanceName="EditCollectionDebtOver3Years" runat="server" NumberType="Float" Value='<%# Eval("debt_over_3_years") %>' Width="100px" ReadOnly = "true"
                                                                Title="редагуються балансоутримувачем, тільки через корекцію відповідних показників таблиці &quot;Заборгованість по кварталах&quot; і обраховується автоматично">
                                                                <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                            </dx:ASPxSpinEdit></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4"><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="Заборгованість з орендної плати (із загальної заборгованості), розмір якої встановлено в межах витрат на утримання, грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebtVMezhahVitrat" ClientInstanceName="EditCollectionDebtVMezhahVitrat" runat="server" NumberType="Float" Value='<%# Eval("debt_v_mezhah_vitrat") %>' Width="100px" 
                                                                Title="Заборгованість з орендної плати, розмір якої встановлено в межах витрат на утримання">
                                                                <ClientSideEvents LostFocus="CalcDebt" />
                                                            </dx:ASPxSpinEdit></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4"><dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Списано заборгованості з орендної плати у звітному періоді, грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebtSpysano" ClientInstanceName="EditCollectionDebtSpysano" runat="server" NumberType="Float" Value='<%# Eval("debt_spysano") %>' Width="100px"
                                                                Title="Списано заборгованості з орендної плати у звітному періоді">
                                                                <ClientSideEvents LostFocus="CalcDebt" />
                                                            </dx:ASPxSpinEdit></td>
                                                        </tr>       
                                                        <tr>
                                                            <td colspan="4"><dx:ASPxLabel ID="ASPxLabel70" runat="server" Text="Заборгованість з нарахованої авансової орендної плати, грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="Edit_avance_debt" ClientInstanceName="Edit_avance_debt" runat="server" NumberType="Float" Value='<%# Eval("avance_debt") %>' Width="100px" ReadOnly="true"
                                                                Title="поточна різниця між нарахованою та отриманою авансовою орендною платою (включаючи Сальдо авансової орендної плати на початок року)">
                                                                <ClientSideEvents LostFocus="CalcDebt" />
                                                            </dx:ASPxSpinEdit></td>
                                                        </tr>                                                            
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                        <p class="SpacingPara"/>
                                        <dx:ASPxRoundPanel ID="PanelCollection2" runat="server" HeaderText="Претензійна робота за договором">
                                            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent13" runat="server">
                                                    <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                                        <tr>
                                                            <td><dx:ASPxLabel ID="ASPxLabel32" runat="server" Text="Кількість заходів (попереджень, приписів і т.п.), всього"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionNumZahodivTotal" ClientInstanceName="EditCollectionNumZahodivTotal" runat="server" NumberType="Integer" Value='<%# Eval("num_zahodiv_total") %>' Width="100px"
                                                                Title="Загальна кількість заходів (попереджень, приписів і т.п.)" AllowMouseWheel="false"/></td>
                                                            <td><div style="width:30px;"></div></td>
                                                            <td><dx:ASPxLabel ID="ASPxLabel33" runat="server" Text="Кількість заходів (попереджень, приписів і т.п.), за звітний період"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionNumZahodivZvit" ClientInstanceName="EditCollectionNumZahodivZvit" runat="server" NumberType="Integer" Value='<%# Eval("num_zahodiv_zvit") %>' Width="100px"
                                                                Title="Кількість заходів (попереджень, приписів і т.п.), за звітний період"/></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4"><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="В тому числі:"></dx:ASPxLabel></td>
                                                        </tr>
                                                        <tr>
                                                            <td><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="- кількість позовів до суду, всього"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionNumPozovTotal" ClientInstanceName="EditCollectionNumPozovTotal" runat="server" NumberType="Integer" Value='<%# Eval("num_pozov_total") %>' Width="100px"
                                                                Title="Загальна кількість позовів до суду"/></td>
                                                            <td></td>
                                                            <td><dx:ASPxLabel ID="ASPxLabel38" runat="server" Text="- кількість позовів до суду, за звітний період"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionNumPozovZvit" ClientInstanceName="EditCollectionNumPozovZvit" runat="server" NumberType="Integer" Value='<%# Eval("num_pozov_zvit") %>' Width="100px"
                                                                Title="Кількість позовів до суду за звітний період"/></td>
                                                        </tr>
                                                        <tr>
                                                            <td><dx:ASPxLabel ID="ASPxLabel39" runat="server" Text="- задоволено позовів, всього"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionPozovZadovTotal" ClientInstanceName="EditCollectionPozovZadovTotal" runat="server" NumberType="Integer" Value='<%# Eval("num_pozov_zadov_total") %>' Width="100px"
                                                                Title="Загальна кількість задоволених позовів"/></td>
                                                            <td></td>
                                                            <td><dx:ASPxLabel ID="ASPxLabel40" runat="server" Text="- задоволено позовів, за звітний період"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionPozovZadovZvit" ClientInstanceName="EditCollectionPozovZadovZvit" runat="server" NumberType="Integer" Value='<%# Eval("num_pozov_zadov_zvit") %>' Width="100px"
                                                                Title="Кількість задоволених позовів за звітний період"/></td>
                                                        </tr>
                                                        <tr>
                                                            <td><dx:ASPxLabel ID="ASPxLabel41" runat="server" Text="- відкрито виконавчих впроваджень, всього"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionPozovVikonTotal" ClientInstanceName="EditCollectionPozovVikonTotal" runat="server" NumberType="Integer" Value='<%# Eval("num_pozov_vikon_total") %>' Width="100px"
                                                                Title="Загальна кількість відкритих виконавчих впроваджень"/></td>
                                                            <td></td>
                                                            <td><dx:ASPxLabel ID="ASPxLabel42" runat="server" Text="- відкрито виконавчих впроваджень, за звітний період"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionPozovVikonZvit" ClientInstanceName="EditCollectionPozovVikonZvit" runat="server" NumberType="Integer" Value='<%# Eval("num_pozov_vikon_zvit") %>' Width="100px"
                                                                Title="Кількість відкритих виконавчих впроваджень за звітний період"/></td>
                                                        </tr>
                                                        <tr>
                                                            <td><dx:ASPxLabel ID="ASPxLabel43" runat="server" Text="- погашено заборгованості за результатами вжитих заходів за звітний період, грн. (без ПДВ), всього" Width="280px"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebtPayedTotal" ClientInstanceName="EditCollectionDebtPayedTotal" runat="server" NumberType="Float" Value='<%# Eval("debt_pogasheno_total") %>' Width="100px"
                                                                Title="Погашено заборгованості за результатами вжитих заходів, всього"/></td>
                                                            <td></td>
                                                            <td><dx:ASPxLabel ID="ASPxLabel44" runat="server" Text="- погашено заборгованості за результатами вжитих заходів за звітний період, грн. (без ПДВ), за звітний період" Width="280px"></dx:ASPxLabel></td>
                                                            <td><dx:ASPxSpinEdit ID="EditCollectionDebtPayedZvit" ClientInstanceName="EditCollectionDebtPayedZvit" runat="server" NumberType="Float" Value='<%# Eval("debt_pogasheno_zvit") %>' Width="100px"
                                                                Title="Погашено заборгованості за результатами вжитих заходів за звітний період"/></td>
                                                        </tr>
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td valign="top" title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                        <dx:ASPxRoundPanel ID="PanelCollectionDebitKvart" runat="server" HeaderText="Заборгованість по кварталах" CssClass="classPanelCollectionDebitKvart">
                                            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent18" runat="server">
                                                    <table border="0" cellspacing="0" cellpadding="2" width="200px">
                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_0" runat="server" Text="2020, 3кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_0" ClientInstanceName="edit_debtkvart_0" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_0") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_1" runat="server" Text="2020, 2кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_1" ClientInstanceName="edit_debtkvart_1" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_1") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                  
                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_2" runat="server" Text="2020, 1кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_2" ClientInstanceName="edit_debtkvart_2" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_2") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_3" runat="server" Text="2019, 4кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_3" ClientInstanceName="edit_debtkvart_3" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_3") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_4" runat="server" Text="2019, 3кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_4" ClientInstanceName="edit_debtkvart_4" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_4") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_5" runat="server" Text="2019, 2кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_5" ClientInstanceName="edit_debtkvart_5" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_5") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_6" runat="server" Text="2019, 1кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_6" ClientInstanceName="edit_debtkvart_6" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_6") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_7" runat="server" Text="2018, 4кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_7" ClientInstanceName="edit_debtkvart_7" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_7") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_8" runat="server" Text="2018, 3кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_8" ClientInstanceName="edit_debtkvart_8" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_8") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_9" runat="server" Text="2018, 2кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_9" ClientInstanceName="edit_debtkvart_9" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_9") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_10" runat="server" Text="2018, 1кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_10" ClientInstanceName="edit_debtkvart_10" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_10") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_11" runat="server" Text="2017, 4кв.:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_11" ClientInstanceName="edit_debtkvart_11" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_11") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>                                                        
                                                        <tr>
                                                            <td colspan="2"><dx:ASPxLabel ID="label_debtkvart_13" runat="server" Text="Безнадійна:"></dx:ASPxLabel></td>
                                                            <td>
                                                                <dx:ASPxSpinEdit ID="edit_debtkvart_13" ClientInstanceName="edit_debtkvart_13" runat="server" NumberType="Float" Value='<%# Eval("debtkvart_13") %>' Width="100px" Title="таблиця, ТІЛЬКИ у якій, формуються  поля заборгованостей за відповідні періоди. У разі погашення заборгованості за попередні квартали редагування повинно проводитися  балансоутримувачем ТІЛЬКИ у цій таблиці">
                                                                    <ClientSideEvents LostFocus="CalcCollectionDebtZvit" />
                                                               </dx:ASPxSpinEdit>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Платежі з оренди" Name="Tab6">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl6" runat="server">
                    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" ClientInstanceName="CPPaymentDocuments" runat="server" OnCallback="CPPaymentDocuments_Callback">
                        <PanelCollection>
                            <dx:panelcontent ID="Panelcontent8" runat="server">                                

                                <div>
                                    <table>
                                        <tr>
                                            <td colspan="5">
                                                <dx:ASPxGridView ID="GridViewPaymentDocuments" ClientInstanceName="GridViewPaymentDocuments" runat="server" KeyFieldName="id" Width="810px" OnRowDeleting="GridViewPaymentDocuments_RowDeleting"
                                                    OnRowUpdating="GridViewPaymentDocuments_RowUpdating" >

	                                                <SettingsCommandButton>
		                                                <EditButton>
			                                                <Image Url="~/Styles/EditIcon.png" ToolTip="Редагувати Платіж" />
		                                                </EditButton>
		                                                <CancelButton>
			                                                <Image Url="~/Styles/CancelIcon.png" />
		                                                </CancelButton>
		                                                <UpdateButton>
			                                                <Image Url="~/Styles/SaveIcon.png" />
		                                                </UpdateButton>
		                                                <DeleteButton>
			                                                <Image Url="~/Styles/DeleteIcon.png" ToolTip="Видалити Платіж" />
		                                                </DeleteButton>
		                                                <NewButton>
			                                                <Image Url="~/Styles/AddIcon.png" />
		                                                </NewButton>
		                                                <ClearFilterButton Text="Очистити" RenderMode="Link" />
	                                                </SettingsCommandButton>

                                                    <Columns>
                                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="60px"
                                                            ShowEditButton="True" ShowDeleteButton="true">
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataDateColumn FieldName="payment_date" VisibleIndex="1" Caption="Дата переказу" Width="100px"></dx:GridViewDataDateColumn>
                                                        <dx:GridViewDataTextColumn FieldName="payment_number" VisibleIndex="2" Caption="Номер квитанції" Width="100px"></dx:GridViewDataTextColumn>                                        
                                                        <dx:GridViewDataTextColumn FieldName="payment_sum" VisibleIndex="3" Caption="Сума" Width="150px"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="payment_purpose" VisibleIndex="4" Caption="Призначення платежу" Width="300px"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataComboBoxColumn FieldName="rent_period_id" ShowInCustomizationForm="True" VisibleIndex="6" Visible="True" Caption="Звітній Період" Width="100px">
                                                            <PropertiesComboBox DataSourceID="SqlDataSourceRentPeriods" ValueField="id" TextField="name" ValueType="System.Int32" />
                                                        </dx:GridViewDataComboBoxColumn>
                                                    </Columns>
                                                    <TotalSummary>
                                                        <dx:ASPxSummaryItem FieldName="payment_sum" SummaryType="Sum" DisplayFormat="{0}" />
                                                    </TotalSummary>

                                                    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
                                                    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" ShowHeaderFilterButton="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                                    <SettingsPager Mode="ShowAllRecords" PageSize="10" />
                                                    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
                                                    <Styles Header-Wrap="True" >
                                                        <Header Wrap="True"></Header>
                                                    </Styles>

                                                    <SettingsBehavior AllowFocusedRow="True" />
                                                    <Templates>
                                                        <EditForm>
                                                        <table border="0" cellspacing="0" cellpadding="2">
                                                            <tr>
                                                                <td> <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Дата переказу" Width="150px" /> </td>
                                                                <td> <dx:ASPxDateEdit ID="EditPaymentDate" runat="server" Width="200px" Value='<%# Eval("payment_date") %>' /> </td>
                                                                <td> <dx:ASPxLabel ID="ASPxLabel54" runat="server" Text="Звітний період" Width="150px" /> </td>
                                                                <td>
                                                                    <dx:ASPxComboBox ID="EditPaymentPeriod" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="200px" 
                                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceRentPeriods" Value='<%# Eval("rent_period_id") %>'>                                                            
                                                                    </dx:ASPxComboBox>
                                                                </td>                                                    
                                                            </tr>
                                                            <tr>
                                                                <td> <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Номер квитанції" Width="150px" /> </td>
                                                                <td> <dx:ASPxTextBox ID="EditPaymentNumber" runat="server" Width="200px" Text='<%# Eval("payment_number")%>' /> </td>
                                                                <td> <dx:ASPxLabel ID="ASPxLabel45" runat="server" Text="Сума" Width="150px" /> </td>
                                                                <td> <dx:ASPxSpinEdit ID="EditPaymentSum" runat="server" NumberType="Float" Value='<%# Eval("payment_sum") %>' Width="200px" /> </td>
                                                            </tr>      
                                                            <tr>
                                                                <td> <dx:ASPxLabel ID="ASPxLabel46" runat="server" Text="Призначення платежу" Width="150px" /> </td>
                                                                <td colspan="3">
                                                                    <dx:ASPxMemo ID="EditPaymentPurpose" runat="server" Width="560px" Text='<%# Eval("payment_purpose")%>' Height="50px" />
                                                                </td>                                                     
                                                            </tr>                                          
                                                        </table>
                                                        <br/>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Зберегти" AutoPostBack="false">
                                                                        <ClientSideEvents Click="function (s, e) { OnSaveClick(s, e); }" />
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Відмінити" AutoPostBack="false">
                                                                        <ClientSideEvents Click="function (s, e) { OnCancelClick(s, e); }" />
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>                                                         
                                                        </EditForm>
                                                    </Templates>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>


                            </dx:panelcontent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                    <p class="SpacingPara"/>

                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="BtnAddPaymentDocument" runat="server" Text="Додати платіж" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { CPPaymentDocuments.PerformCallback('add:');  }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

		<dx:TabPage Text="Суборенда" Name="Tab2">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl9" runat="server">

                    <dx:ASPxCallbackPanel ID="CPSubleases" ClientInstanceName="CPSubleases" runat="server" OnCallback="CPSubleases_Callback">
                        <PanelCollection>
                            <dx:panelcontent ID="Panelcontent17" runat="server">

                                <dx:ASPxGridView ID="GridViewSubleases" ClientInstanceName="GridViewSubleases" runat="server" AutoGenerateColumns="False" 
                                    KeyFieldName="id" Width="1150px"
                                    OnRowDeleting="GridViewSubleases_RowDeleting"
                                    OnRowUpdating="GridViewSubleases_RowUpdating" >

	                                <SettingsCommandButton>
		                                <EditButton>
			                                <Image Url="~/Styles/EditIcon.png" ToolTip="Редагувати Підставу" />
		                                </EditButton>
		                                <CancelButton>
			                                <Image Url="~/Styles/CancelIcon.png" />
		                                </CancelButton>
		                                <UpdateButton>
			                                <Image Url="~/Styles/SaveIcon.png" />
		                                </UpdateButton>
		                                <DeleteButton>
			                                <Image Url="~/Styles/DeleteIcon.png" ToolTip="Видалити Підставу" />
		                                </DeleteButton>
		                                <NewButton>
			                                <Image Url="~/Styles/AddIcon.png" />
		                                </NewButton>
		                                <ClearFilterButton Text="Очистити" RenderMode="Link" />
	                                </SettingsCommandButton>

                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image"
                                            ShowEditButton="true" ShowDeleteButton="true">
                                        </dx:GridViewCommandColumn>
										<dx:GridViewDataTextColumn FieldName="agreement_num" VisibleIndex="1" Caption="Номер договору суборендування" Width="150px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn FieldName="agreement_date" VisibleIndex="2" Caption="Дата укладання договору" Width="100px"></dx:GridViewDataDateColumn>
                                        <dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="3" Caption="Початок оренди згідно з договором"></dx:GridViewDataDateColumn>
										<dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="4" Caption="Закінчення оренди згідно з договором"></dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="5" Caption="Орендована площа, кв.м."></dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="rent_payment_month" VisibleIndex="6" Caption="Суборендна плата, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
										<dx:GridViewDataComboBoxColumn FieldName="using_possible_id" VisibleIndex="10" Width = "320px" Visible="True" Caption="Можливе використання вільного приміщення">
											<HeaderStyle Wrap="True" />
											<PropertiesComboBox DataSourceID="SqlDataSourceUsingPossible" ValueField="id" TextField="name" ValueType="System.Int32" />
										</dx:GridViewDataComboBoxColumn>
                                    </Columns>
                                    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
                                    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" ShowHeaderFilterButton="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                    <SettingsPager Mode="ShowAllRecords" PageSize="10" />
                                    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
                                    <Styles Header-Wrap="True" >
                                        <Header Wrap="True"></Header>
                                    </Styles>
                                    <SettingsCookies CookiesID="GUKV.Reports1NF.ArendaSubleases" Enabled="False" Version="A2" />

                                    <Templates>
                                        <EditForm>
                                            <table border="0" cellspacing="0" cellpadding="2">
												<tr>
													<td><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Номер договору суборендування"></dx:ASPxLabel></td>
													<td>
														<dx:ASPxTextBox ID="edit_agreement_num" runat="server" Text='<%# Eval("agreement_num") %>' Width="190px" MaxLength="18" Title="Номер договору орендування">
															<ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true"></ValidationSettings>
														</dx:ASPxTextBox>
													</td>
													<td> &nbsp; </td>
													<td><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Дата укладання договору"></dx:ASPxLabel></td>
													<td>
														<dx:ASPxDateEdit ID="edit_agreement_date" runat="server" Value='<%# Eval("agreement_date") %>' Width="100%" Title="Дата укладання договору">
															<ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true"></ValidationSettings>
														</dx:ASPxDateEdit>
													</td>
												</tr>
												<tr>
													<td><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Початок оренди згідно з договором"></dx:ASPxLabel></td>
													<td>
														<dx:ASPxDateEdit ID="edit_rent_start_date" runat="server" Value='<%# Eval("rent_start_date") %>' Width="190px" Title="Початок оренди згідно з договором">
															<ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true"></ValidationSettings>
														</dx:ASPxDateEdit>
													</td>
													<td> &nbsp; </td>
													<td><dx:ASPxLabel runat="server" Text="Закінчення оренди згідно з договором"></dx:ASPxLabel></td>
													<td><dx:ASPxDateEdit ID="edit_rent_finish_date" runat="server" Value='<%# Eval("rent_finish_date") %>' Width="100%"  Title="Закінчення оренди згідно з договором" /></td>
												</tr>
												<tr>
													<td><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Вид оплати"></dx:ASPxLabel></td>
													<td colspan="4">
														<dx:ASPxComboBox ID="edit_payment_type_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
															IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePaymentType" Value='<%# Eval("payment_type_id") %>'
															 Title="Вид оплати">
															<ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Необхідно вибрати вид оплати за договором" /> </ValidationSettings>
														</dx:ASPxComboBox>
													</td>
												</tr>
												<tr>
													<td><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Орендована площа, кв.м."></dx:ASPxLabel></td>
													<td><dx:ASPxSpinEdit ID="edit_rent_square" runat="server" NumberType="Float" Value='<%# Eval("rent_square") %>' Width="190px"  Title="Орендована площа" /></td>
													<td> &nbsp; </td>
													<td><dx:ASPxLabel ID="ASPxLabel60" runat="server" Text="Суборендна плата, грн. (без ПДВ)"></dx:ASPxLabel></td>
													<td><dx:ASPxSpinEdit ID="edit_rent_payment_month" runat="server" NumberType="Float" Value='<%# Eval("rent_payment_month") %>' Width="190px"  Title="Суборендна плата" /></td>
												</tr>
												<tr>
													<td><dx:ASPxLabel ID="ASPxLabel61" runat="server" Text="Можливе використання вільного приміщення"></dx:ASPxLabel></td>
													<td colspan="4">
														<dx:ASPxComboBox ID="edit_using_possible_id" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
															IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceUsingPossible" Value='<%# Eval("using_possible_id") %>'
															 Title="Можливе використання вільного приміщення">
														</dx:ASPxComboBox>
													</td>
												</tr>
                                            </table>

                                            <br/>

                                            <table>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Зберегти" AutoPostBack="false">
                                                            <ClientSideEvents Click="function (s, e) { OnSaveSubleaseClick(s, e); }" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Відмінити" AutoPostBack="false">
                                                            <ClientSideEvents Click="function (s, e) { OnCancelSubleaseClick(s, e); }" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>    
                                        </EditForm>
                                    </Templates>
                                </dx:ASPxGridView>

                            </dx:panelcontent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>

                    <p class="SpacingPara"/>

                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="ButtonAddSublease" runat="server" Text="Додати Суборенду" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { CPSubleases.PerformCallback('add:');  }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Продовження договору" Name="Tab4">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3a" runat="server">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Вільні приміщення">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent10" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                            <tr>
                                                <td>
                                                

    <dx:ASPxGridView ID="ASPxGridViewFreeSquare" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceFreeSquare" KeyFieldName="id" OnRowValidating="ASPxGridViewFreeSquare_RowValidating" OnStartRowEditing="ASPxGridViewFreeSquare_StartRowEditing"
            ClientInstanceName="grid" oninitnewrow="ASPxGridViewFreeSquare_InitNewRow" >
            <ClientSideEvents CustomButtonClick="ShowDogcontinuePhoto" Init="OnInit" EndCallback="OnEndCallback" />
        <Styles>  
            <EditForm CssClass="editForm999" ></EditForm>  
        </Styles>  

	    <SettingsCommandButton>
		    <EditButton>
			    <Image Url="~/Styles/EditIcon.png" />
		    </EditButton>
		    <CancelButton>
			    <Image Url="~/Styles/CancelIcon.png" />
		    </CancelButton>
		    <UpdateButton>
			    <Image Url="~/Styles/SaveIcon.png" />
		    </UpdateButton>
		    <DeleteButton>
			    <Image Url="~/Styles/DeleteIcon.png" />
		    </DeleteButton>
		    <NewButton>
			    <Image Url="~/Styles/AddIcon.png" />
		    </NewButton>
		    <ClearFilterButton Text="Очистити" RenderMode="Link" />
	    </SettingsCommandButton>

        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="False" 
                ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowClearFilterButton="true" ShowEditButton="true" ShowNewButton="true" >
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnPhoto" Text="Фото"> 
						<Image Url="~/Styles/PhotoIcon.png"> </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="btnPdfBuild" Text="Pdf"> 
						<Image Url="~/Styles/PdfReportIcon.png"> </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="btnFreeCycle" Text="Картка процесу передачі в оренду вільного приміщення" Visibility="Invisible"> 
						<Image Url="~/Styles/ReportDocument18.png"> </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>

                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="id" Caption="ID" VisibleIndex="1"  ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="arenda_id" Caption="arenda_id" VisibleIndex="2" ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="report_id" Caption="report_id" VisibleIndex="14" ReadOnly="true" Visible="false" >
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="total_free_sqr" Caption="Загальна площа об’єкта, кв.м." VisibleIndex="3" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="free_sqr_korysna" Caption="Корисна площа об’єкта, кв.м." VisibleIndex="4" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="free_sqr_condition_id" VisibleIndex="5" 
                Visible="True" Caption="Технічний стан об’єкта">
                <PropertiesComboBox DataSourceID="SqlDataSourceTechStane" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataComboBoxColumn FieldName="period_used_id" VisibleIndex="5" 
                Visible="True" Caption="Період використання">
                <PropertiesComboBox DataSourceID="SqlDataSourcePeriodUsed" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewBandColumn Caption="Комунальне забезпечення">
                <Columns>

                    <dx:GridViewDataCheckColumn FieldName="water" Caption="Водо-постача-ння" VisibleIndex="7">
                        <HeaderStyle Wrap="True" />
						<EditFormSettings Caption="Водопостачання" />
                    </dx:GridViewDataCheckColumn>

                    <dx:GridViewDataCheckColumn FieldName="heating" Caption="Тепло-постача-ння" VisibleIndex="8">
                        <HeaderStyle Wrap="True" />
						<EditFormSettings Caption="Теплопостачання" />
                    </dx:GridViewDataCheckColumn>

                    <dx:GridViewDataComboBoxColumn FieldName="power_info_id" Width="120px" VisibleIndex="9" 
                        Visible="True" Caption="Потужність електромережі">
                        <HeaderStyle Wrap="True" />
                        <PropertiesComboBox DataSourceID="SqlDataSourcePowerInfo" ValueField="id" TextField="name" ValueType="System.Int32" />
                    </dx:GridViewDataComboBoxColumn>


                    <dx:GridViewDataCheckColumn FieldName="gas" Caption="Газо-постача-ння" VisibleIndex="10">
						<EditFormSettings Caption="Газопостачання" />
                        <HeaderStyle Wrap="True" />
                    </dx:GridViewDataCheckColumn>

                </Columns>
            </dx:GridViewBandColumn>

            <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="12" ReadOnly="true">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataTextColumn FieldName="modified_by" Caption="Користувач" VisibleIndex="13" ReadOnly="true">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="floor" Caption="Характеристика об’єкта оренди" ToolTip="Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)" VisibleIndex="15">
                <HeaderStyle Wrap="True" />
                
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="possible_using" VisibleIndex="16" Width = "320px" Visible="True" Caption="Можливе використання вільного приміщення" >
				<HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceUsingPossible" DropDownStyle="DropDown" AllowMouseWheel="true" ValueField="name" TextField="name" ValueType="System.String" EnableSynchronization="False" />
            </dx:GridViewDataComboBoxColumn>


            <dx:GridViewDataComboBoxColumn FieldName="invest_solution_id" VisibleIndex="18" Width = "100px" Visible="True" Caption="Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації">
				<HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceInvestSolution" ValueField="id" TextField="name" ValueType="System.Int32" />
				<EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataTextColumn FieldName="initiator" Caption="Ініціатор оренди (текстова інформація, якщо балансоутримувач - пусто)" VisibleIndex="19" Width ="100px">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="zgoda_control_id" VisibleIndex="20" Width ="80px"
                Visible="True" Caption="Погодження органу управління балансоутримувача">
                <HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceZgodaRenter" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataComboBoxColumn FieldName="zgoda_renter_id" VisibleIndex="21" Width ="80px"
                Visible="True" Caption="Погодження органу охорони культурної спадщини">
                <HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceZgodaRenter" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataComboBoxColumn FieldName="free_object_type_id" VisibleIndex="22" Width ="100px"
                Visible="True" Caption="Тип об’єкта">
                <HeaderStyle Wrap="True" />
                <PropertiesComboBox DataSourceID="SqlDataSourceFreeObjectType" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>


            <dx:GridViewDataCheckColumn FieldName="is_included" Caption="Включено до переліку продовження договорів" VisibleIndex="30">
                <HeaderStyle Wrap="True" />
				<EditFormSettings Caption="Включено до переліку продовження договорів" />
            </dx:GridViewDataCheckColumn>

            <dx:GridViewDataTextColumn FieldName="komis_protocol" Caption="Погодження орендодавця" VisibleIndex="32" Width ="80px" >
                <HeaderStyle Wrap="True" />
				<EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="id" Caption="Реєстра-ційний №" VisibleIndex="40" Width ="50px"  >
				<CellStyle HorizontalAlign="Center" />
                <HeaderStyle Wrap="True" />
				<EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>

		    <dx:GridViewDataComboBoxColumn FieldName="include_in_perelik" VisibleIndex="50" Width = "50px" Visible="True" Caption="Включено до переліку №" ReadOnly="true">
			    <HeaderStyle Wrap="True" />
			    <PropertiesComboBox DataSourceID="SqlDataSourceIncludeInPerelik" ValueField="id" TextField="name" ValueType="System.String" />
                <EditFormSettings ColumnSpan="1" />
		    </dx:GridViewDataComboBoxColumn>

            <dx:GridViewDataTextColumn Name="tmp1" Caption="" VisibleIndex="55" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="zal_balans_vartist" Caption="Залишкова балансова вартість, грн." VisibleIndex="110" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="perv_balans_vartist" Caption="Первісна балансова вартість, грн." VisibleIndex="120" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="punkt_metod_rozrahunok" Caption="Посилання на пункт Методики розрахунку орендної плати, яким встановлена орендна ставка для запропонованого цільового призначення" VisibleIndex="130" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Name="tmp2" Caption="" VisibleIndex="135" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="prop_srok_orands" Caption="Пропонований строк оренди (у роках)" VisibleIndex="140" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Name="tmp3" Caption="" VisibleIndex="145" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="nomer_derzh_reestr_neruh" Caption="Номер запису про право власності у Реєстрація у Державному реєстрі речових прав на нерухоме майно" VisibleIndex="150" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="reenum_derzh_reestr_neruh" Caption="Реєстраційний номер об'єкту нерухомого майна у Реєстрація у Державному реєстрі речових прав на нерухоме майно" VisibleIndex="155" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="info_priznach_nouse" Caption="Інформація про цільове призначення об’єкта оренди у випадках неможливості використання об’єкта за будь-яким цільовим призначенням, якщо об’єкт розташований у приміщеннях, які мають відповідне соціально-економічне призначення" VisibleIndex="160" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="info_rahunok_postach" Caption="Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг, або інформація про порядок участі орендаря у компенсації балансоутримувачу витрат на оплату комунальних послуг, якщо об'єкт оренди не має окремих особових рахунків, відкритих для нього відповідними постачальниками комунальних послуг" VisibleIndex="165" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

<%--            <dx:GridViewDataTextColumn FieldName="priznach_before" Caption="Цільове призначення об’єкта, за яким об’єкт використовувався перед тим, як він став вакантним" VisibleIndex="205" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="orend_plat_borg" Caption="Заборгованість по орендній платі, грн. (без ПДВ)" VisibleIndex="205" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>


<%--            <dx:GridViewDataTextColumn FieldName="period_nouse" Caption="Період часу, протягом якого об’єкт не використовується" VisibleIndex="215" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataDateColumn FieldName="stanom_na" Caption="Станом на" VisibleIndex="215" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataDateColumn>


<%--            <dx:GridViewDataTextColumn FieldName="osoba_use_before" Caption="Інформацію про особу, яка використовувала об’єкт перед тим, як він став вакантним (якщо такою особою був балансоутримувач, проставляється позначка “об’єкт використовувався балансоутримувачем”)" VisibleIndex="225" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>--%>

            <dx:GridViewDataTextColumn FieldName="orend_plat_last_month" Caption="Місячна орендна плата за останній місяць(проіндексована)" VisibleIndex="225" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataCheckColumn FieldName="has_perevazh_pravo" Caption="Має переважне право на продовження" VisibleIndex="300">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataCheckColumn>

            <dx:GridViewDataTextColumn FieldName="polipshanya_vartist" Caption="Вартість здійснених чинним орендарем невід'ємних поліпшень" VisibleIndex="310" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataDateColumn FieldName="polipshanya_finish_date" Caption="Дата завершення здійснених чинним орендарем невід'ємних поліпшень" VisibleIndex="320" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataTextColumn FieldName="primitki" Caption="Примітки" VisibleIndex="330" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataDateColumn FieldName="zalbalansvartist_date" Caption="Дата формування залишкової вартості" VisibleIndex="340" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataTextColumn FieldName="osoba_oznakoml" Caption="Особа відповідальна за ознайомлення з об’єктом" VisibleIndex="350" Visible="false" >
                <HeaderStyle Wrap="True" />
                <EditFormSettings Visible="True" />
                <EditFormCaptionStyle Wrap="True"/>
            </dx:GridViewDataTextColumn>




        </Columns>
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsPager PageSize="10" />
        <SettingsEditing NewItemRowPosition="Bottom" />
        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" VerticalScrollableHeight="0" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard"/>
    </dx:ASPxGridView>

    <dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True" 
        ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
        HeaderText="Фотографії об'єкту з вільним приміщенням" Modal="True" 
        PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
        PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">

                <asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
                    DeleteMethod="Delete" InsertMethod="Insert" 
                    OnInserting="ObjectDataSourcePhotoFiles_Inserting" 
                    SelectMethod="Select" 
                    TypeName="ExtDataEntry.Models.FileAttachment">
                    <DeleteParameters>
                        <asp:Parameter DefaultValue="reports1nf_arenda_dogcontinue_photos" Name="scope" Type="String" />
                        <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                        <asp:Parameter Name="id" Type="String" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter DefaultValue="reports1nf_arenda_dogcontinue_photos" Name="scope" Type="String" />
                        <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                        <asp:Parameter Name="Name" Type="String" />
                        <asp:Parameter Name="Image" Type="Object" />
                    </InsertParameters>
                    <SelectParameters>
                        <asp:Parameter DefaultValue="reports1nf_arenda_dogcontinue_photos" Name="scope" Type="String" />
                        <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>

                <dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server" 
                    ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
                    <Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
                    <SettingsFileList>
                        <ThumbnailsViewSettings ThumbnailSize="180px" />
                    </SettingsFileList>
                    <SettingsEditing AllowDelete="True" />
                    <SettingsFolders Visible="False" />
                    <SettingsToolbar ShowDownloadButton="True" ShowPath="False" />
                    <SettingsUpload UseAdvancedUploadMode="True">
                        <AdvancedModeSettings EnableMultiSelect="True" />
                    </SettingsUpload>

                    <SettingsDataSource FileBinaryContentFieldName="Image" 
                        IsFolderFieldName="IsFolder" KeyFieldName="ID" 
                        LastWriteTimeFieldName="LastModified" NameFieldName="Name" 
                        ParentKeyFieldName="ParentID" />
                </dx:ASPxFileManager>

                <br />

                <dx:ASPxButton ID="ASPxButtonClose" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
                    <ClientSideEvents Click="function(s, e) { PopupObjectPhotos.Hide(); }" />
                </dx:ASPxButton>

            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>


                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
<%--                        </ItemTemplate>
                    </asp:FormView>--%>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

		<dx:TabPage Text="Фото / плани" Name="Tab6">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl7" runat="server">

                    <dx:ASPxRoundPanel ID="PanelPhoto" runat="server" HeaderText="Фото / плани" EnableViewState="true">
                        <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent16" runat="server">


                                <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                <tr>
                                <td>

                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanelImageGallery" EnableViewState="true"
                                    ClientInstanceName="ASPxCallbackPanelImageGallery" runat="server" 
                                    OnCallback="ASPxCallbackPanelImageGallery_Callback">

                                    <SettingsLoadingPanel Enabled="false"/>
                            
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
                                        
                                    <dx:ASPxImageGallery ID="imageGalleryDemo" runat="server" DataSourceID="ObjectDataSourceBalansPhoto"
                                        EnableViewState="false" 
                                        AlwaysShowPager="false" 
                                        PagerAlign="Center"
                                        ThumbnailHeight="190" ThumbnailWidth="190"
                                        SettingsFullscreenViewer-ShowCloseButton="true" 
                                        OnDataBound="imageGalleryDemo_DataBound" >

<%--    pgv                             <SettingsFolder ImageCacheFolder="~\Thumb\" ImageSourceFolder="~\ImgContent\tmp\" />      --%>         
                                      <SettingsFolder ImageCacheFolder="~\Thumb\"  /> 
                                        
                                        <ItemTextTemplate>

                                            <div class="item">
                                                <%--<div class="item_email" style="text-align:center"><%# Container.EvalDataItem("file_name")%></div><br />--%>
                                                <div class="item_email" style="text-align:center">
                                                <%--<a style="color:White;cursor:pointer" onclick="javascript:EditImage(<%# Container.ItemIndex %>);" title="Редагувати">Редагувати</a>&nbsp;&nbsp;--%>
                                                    <a style="color:White;cursor:pointer" onclick="javascript:DeleteImage(<%# Container.ItemIndex %>);" title="Видалити">Видалити</a>

                                                    <%--<a style="color:White;cursor:pointer" onclick="function(s, e) { imageGalleryDemo.PerformCallback('deleteimage:<%# Container.ItemIndex %>'); }" title="Видалити">Видалити</a>--%>
                                                    
                                                </div>
                                            </div>

                                        </ItemTextTemplate>
                                        <SettingsTableLayout RowsPerPage="2" ColumnCount="5" />

                                        <SettingsTableLayout ColumnCount="5" RowsPerPage="2"></SettingsTableLayout>

                                        <PagerSettings Position="TopAndBottom">
                                            <PageSizeItemSettings Visible="False" />
                                            <PageSizeItemSettings Visible="False"></PageSizeItemSettings>
                                        </PagerSettings>

                                    </dx:ASPxImageGallery> 

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                                </td>
                                </tr>
                                <tr>
                                <td>





<%--                                <asp:UpdatePanel ID="updPanel" EnableViewState="true" runat="server" ChildrenAsTriggers="true">
                                    <ContentTemplate>--%>

                                <dx:ASPxCallbackPanel ID="ContentCallback" runat="server" EnableViewState="true"
                                    ClientInstanceName="ContentCallback" OnCallback="ContentCallback_Callback">
                                    
                                    <PanelCollection>
                                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                            
<%--                                            <asp:HiddenField ID="TempFolderIDField" runat="server" 
                                                OnValueChanged="TempFolderIDField_ValueChanged" />      --%>

                                            <dx:ASPxUploadControl ID="uplImage" runat="server" ShowUploadButton="false" 
                                                FileUploadMode="OnPageLoad"
                                                ClientInstanceName="uploader" NullText="..." 
                                                OnFileUploadComplete="ASPxUploadPhotoControl_FileUploadComplete" 
                                                ShowProgressPanel="True" Size="35" UploadMode="Auto">
                                                <ValidationSettings AllowedFileExtensions=".jpg, .jpeg, .jpe, .gif, .png, .bmp, .pdf" 
                                                    MaxFileSize="20480000">

                                                </ValidationSettings>
                                                <ClientSideEvents FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" 
                                                    FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }" 
                                                    TextChanged="function(s, e) { UpdateUploadButton(); }" />
                                                <AdvancedModeSettings EnableMultiSelect="True" />
                                            </dx:ASPxUploadControl>

                                            <table>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Visible ="true"
                                                            ClientInstanceName="btnUpload" Text="Завантажити" onclick="btnUpload_Click">
                                                            <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                                        </dx:ASPxButton>
                                                   </td>
                                                   <td>
                                                        <dx:ASPxButton ID="btnBuildPdf" runat="server" OnClick="PdfImageBuild_Click"
                                                            ClientInstanceName="btnBuildPdf" Text="Друк" >
                                                            
                                                        </dx:ASPxButton>
                                                    </td>
                                                </tr>
                                             </table>

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>

<%--                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>

                                </td>
                                </tr>
                                </table>



                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

    </TabPages>
</dx:ASPxPageControl>

<asp:FormView ID="errorForm" runat="server" OnDataBound="errorForm_DataBound">
    <ItemTemplate>
        <div style="padding:10px;">
        <div style="color:Red;font-weight:bold">Наступні поля мають бути заповнені перед надсиланням до ДКВ:</div>
        <ul>
            <asp:Repeater ID="errorList" runat="server">
                <ItemTemplate>
                    <li style="color:Red"><%# Eval("error_message") %> </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
        </div>
    </ItemTemplate>
</asp:FormView>

<p class="SpacingPara"/>
            <table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSave" ClientInstanceName="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { if (ReportingPeriodCombovalidate()){ CPMainPanel.PerformCallback('save:'); }}"
                   LostFocus="HidePnl" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonSend" ClientInstanceName="ButtonSend" runat="server" Text="Надіслати" AutoPostBack="false"  CausesValidation="true">
                <ClientSideEvents  Click="function (s,e) { if (ReportingPeriodCombovalidate()){ CPMainPanel.PerformCallback('send:'); }  }" 
                    LostFocus="HidePnl"/>
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonClear" runat="server" Text="Відмінити зміни" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonComments" runat="server" Text="Коментарі" CausesValidation="false" AutoPostBack="false" />

            <dx:ASPxPopupControl ID="PopupComments" runat="server" 
                HeaderText="Коментарі" 
                ClientInstanceName="PopupComments" 
                PopupElementID="ButtonComments" >
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">

                        <dx:ASPxCallbackPanel ID="CPCommentViewerPanel" ClientInstanceName="CPCommentViewerPanel" runat="server" OnCallback="CPCommentViewerPanel_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent6" runat="server">

                                    <uc1:ReportCommentViewer ID="ReportCommentViewer1" runat="server"/>

                                </dx:panelcontent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents
                    PopUp="function (s,e) { CPCommentViewerPanel.PerformCallback('' + lastFocusedControlId + ';' + lastFocusedControlTitle); }"
                    CloseUp="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
            </dx:ASPxPopupControl>
        </td>
        <td style="width:200px"> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonCopyCard" runat="server" Text="Скопіювати картку договору" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { 
                    if (confirm('Скопіювати картку договору в новий договір ?')) 
                        window.location = 'OrgRentAgreement.aspx?rid=' + argReportID + '&copyid=' + argRentAgreementID;
                }" />
            </dx:ASPxButton>
        </td>
    </tr>
</table>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<p class="SpacingPara"/>

<dx:ASPxValidationSummary ID="ValidationSummary" ValidationGroup="MainGroup" runat="server" RenderMode="BulletedList" Width="800px" ClientInstanceName="ValidationSummary">
</dx:ASPxValidationSummary>

<div id ="valError" style="color:red; display:none;">По кожному договору зі списку ДПЗ, що має статус «Договір діє» та “Вид оплати”  - «ГРОШОВА ОПЛАТА», повинен бути встановлений діючий Звітний період </div>

    <table border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td> <dx:ASPxLabel ID="ASPxLabel53" runat="server" Text="..." Width="50px" /> </td>
       </td>
    </tr>
</table>

</asp:Content>


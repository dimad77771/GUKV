<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgRentAgreement.aspx.cs" Inherits="Reports1NF_OrgRentAgreement"
    MasterPageFile="~/NoMenu.master" Title="Картка Договору Оренди" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxImageGallery" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
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
    </style>

    <script type="text/javascript" language="javascript">

    // <![CDATA[
        var period;
        document.addEventListener("DOMContentLoaded", ready);
       

        function ready(event) {
            HidePnl();
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

            EditCollectionDebtTotal.SetEnabled(!isEnable);
            EditCollectionDebtZvit.SetEnabled(!isEnable);
            EditCollectionDebt3Month.SetEnabled(!isEnable);
            EditCollectionDebt12Month.SetEnabled(!isEnable);
            EditCollectionDebt3Years.SetEnabled(!isEnable);
            EditCollectionDebtOver3Years.SetEnabled(!isEnable);
            EditCollectionDebtVMezhahVitrat.SetEnabled(!isEnable);
            EditCollectionDebtSpysano.SetEnabled(!isEnable);
            EditCollectionNumZahodivTotal.SetEnabled(!isEnable);
            EditCollectionNumZahodivZvit.SetEnabled(!isEnable);
            EditCollectionNumPozovTotal.SetEnabled(!isEnable);
            EditCollectionNumPozovZvit.SetEnabled(!isEnable);
            EditCollectionPozovZadovTotal.SetEnabled(!isEnable);
            EditCollectionPozovZadovZvit.SetEnabled(!isEnable);
            EditCollectionPozovVikonTotal.SetEnabled(!isEnable);
            EditCollectionPozovVikonZvit.SetEnabled(!isEnable);
            EditCollectionDebtPayedTotal.SetEnabled(!isEnable);
            EditCollectionDebtPayedZvit.SetEnabled(!isEnable);
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

        function ReportingPeriodCombovalidate() {
            var cdate = new Date();
            if ((cdate.getMonth() == 0 || cdate.getMonth() == 3 || cdate.getMonth() == 6 || cdate.getMonth() == 9) && (cdate.getDate() >= 1 && cdate.getDate() <= 24)) 
            	{
            var error = false;
              var element = document.getElementById('<%=PaymentForm.ClientID %>' + '_ReportingPeriodCombo_I'); //ctl00_MainContent_CPMainPanel_CardPageControl_PaymentForm_ReportingPeriodCombo_I
            //var period = document.getElementById('<%=PaymentForm.ClientID %>' + '_NeededPeriodCombo'); 
            if (element.value != period)
                   error = true;

                var rbt1 = document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S');//ctl00_MainContent_CPMainPanel_CardPageControl_OrganizationsForm_PanelAgreement
            if ((rbt1.value == "C") && (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "грошова оплата" || document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "погодинно") && error == true) {
                    document.getElementById('valError').style.display = '';
                    document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» та “Вид оплати”-«ГРОШОВА ОПЛАТА», «ПОГОДИННО», повинен бути встановлений діючий Звітний період";
                    return false;
               	 }
               

                if (!ComboPaymentTypeValidate())
                    return false;

                if (!EditCollectionDebtTotalValidate())
                    return false;

                if (!CheckRadioAgreementActiveValidate())
                    return false;

//                if (!CheckRadioAgreementAndSquare())
//                    return false;

  
	}
           HideValidator();
            return true;
        }

        function HideValidator() {
            document.getElementById('valError').style.display = 'none';
        }
        
        function ComboPaymentTypeValidate() {
            //ctl00_MainContent_CPMainPanel_CardPageControl_OrganizationsForm_PanelAgreement_ComboPaymentType_I
            if ((document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "грошова оплата" || (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_ComboPaymentType_I').value.toLowerCase() == "погодинно") &&
                document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S').value == "C")) {

                var EditPaymentNarah = document.getElementById('<%=PaymentForm.ClientID %>' + '_PanelRentPaymentDocuments_CPRentPayment_EditPaymentNarah_orndpymnt_I').value; //ctl00_MainContent_CPMainPanel_CardPageControl_PaymentForm_PanelRentPaymentDocuments_CPRentPayment_EditPaymentNarah_orndpymnt
                var EditPaymentNarZvit = document.getElementById('<%=PaymentForm.ClientID %>' + '_PanelRentPaymentDocuments_CPRentPayment_EditPaymentNarZvit_orndpymnt_I').value;
//                if (EditPaymentNarah < EditPaymentNarZvit) {
                if (parseFloat(EditPaymentNarah.replace(",", ".")) <  parseFloat(EditPaymentNarZvit.replace(",", ".")) ) {

                    document.getElementById('valError').style.display = '';
                    document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» та “Вид оплати”-«ГРОШОВА ОПЛАТА», «ПОГОДИННО», значення поля «Нараховано орендної плати за звітний період, грн. (без ПДВ)» повинно бути більше, або дорівнювати значенню поля «у тому числі, з нарахованої за звітний період (без боргів та переплат)»";
                    return false;
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
                var EditPaymentNarah = document.getElementById('<%=PaymentForm.ClientID %>' + '_PanelRentPaymentDocuments_CPRentPayment_EditPaymentNarah_orndpymnt_I').value; //Нараховано орендної плати за звітний період, грн. (без ПДВ)

                if (EditCollectionDebtZvit == "") EditCollectionDebtZvit = "0";
                if (EditPaymentNarZvit == "") EditPaymentNarZvit = "0";
                if (EditPaymentNarah == "") EditPaymentNarah = "0";
                if (EditPaymentSaldo == "") EditPaymentSaldo = "0";
                

                if (EditPaymentNarah == "" && EditCollectionDebtZvit != "" && EditPaymentNarZvit != "" && EditPaymentSaldo != "")
                    EditPaymentNarah = "0";
                if (parseFloat(EditPaymentNarah.replace(",", ".")) > Math.round(parseFloat(parseFloat(EditCollectionDebtZvit.replace(",", ".")) + parseFloat(EditPaymentNarZvit.replace(",", ".")) + parseFloat(EditPaymentSaldo.replace(",", "."))) * 1000) / 1000) {
                    document.getElementById('valError').style.display = '';
//                    document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» та «Вид оплати»-«ГРОШОВА ОПЛАТА», значення поля «Нараховано орендної плати за звітний період, грн. (без ПДВ)» повинно дорівнювати сумі значень полів «у тому числі, з нарахованої за звітний період (без боргів та переплат)» та «Заборгованість по орендній платі, грн. - (без ПДВ): - за звітний період» та «Сальдо на початок року (незмінна впродовж року величина)».";
                    document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» та «Вид оплати»-«ГРОШОВА ОПЛАТА», «ПОГОДИННО», значення поля «Нараховано орендної плати за звітний період, грн. (без ПДВ)» не повинно перевищувати суму значень полів «у тому числі, з нарахованої за звітний період (без боргів та переплат)» та «Заборгованість по орендній платі, грн. - (без ПДВ): - за звітний період» та «Сальдо на початок року (незмінна впродовж року величина)».";
                    return false;
                }
            }
            return true;
        }

        function CheckRadioAgreementActiveValidate() {
            if (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S').value == "C" && 
                document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_EditActualFinishDate_I').value != "") {
                document.getElementById('valError').style.display = '';
                document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» значення поля «Фактична дата закінчення договору» повинно бути порожнім";
                    return false;
             }
            if ((document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementToxic_S').value == "C" || (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementContinuedByAnother_S').value == "C"))&&
               document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_EditActualFinishDate_I').value == "") {
                document.getElementById('valError').style.display = '';
               document.getElementById('valError').innerHTML = "По договору, що має статус «Договір закінчився» значення поля «Фактична дата закінчення договору» не повинно бути порожнім";
                 return false;
             }
            return true;
        }

        function CheckRadioAgreementAndSquare() {
            var square = document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_EditRentSquare_I').value;
            if (square == "") square = "0";

            if (document.getElementById('<%=OrganizationsForm.ClientID %>' + '_PanelAgreement_RadioAgreementActive_S').value == "C" &&
                parseFloat(square.replace(",", ".")) == 0) {
                 document.getElementById('valError').style.display = '';
                 document.getElementById('valError').innerHTML = "По договору, що має статус «Договір діє» значення поля «Площа що використовується всього, кв. м» не повинно дорівнювати 0 ";
                 return false;
            }
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

<asp:ObjectDataSource ID="ObjectDataSourceBalansPhoto" runat="server" SelectMethod="SelectFromTempFolder" 
    TypeName="ExtDataEntry.Models.FileAttachment">
    <SelectParameters>
        <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
        <asp:Parameter DefaultValue="" Name="recordID" Type="Int32" />
        <asp:Parameter DefaultValue="" Name="tempGuid" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>



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
    <asp:Label runat="server" ID="ASPxLabel19" Text="Картка договору надання об'єкту іншій організації" CssClass="pagetitle"/>
</p>

<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" runat="server" OnCallback="CPMainPanel_Callback">
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
                                                </td>
                                                <td width="8px">&nbsp;</td>
                                                <td width="120px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Назва вулиці"></dx:ASPxLabel></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditRentAddrStreet" runat="server" ReadOnly="true" Text='<%# Eval("addr_street_name") %>' Width="270px"  Title="Адреса будинку - Назва вулиці" />
                                                </td>
                                            </tr>
                                            <tr><td colspan="7" height="4px"/></tr>
                                            <tr>
                                                <td width="120px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Номер будинку"></dx:ASPxLabel></td>
                                                <td width="8px">&nbsp;</td>
                                                <td><dx:ASPxTextBox ID="EditRentBuildingNum" runat="server" ReadOnly="true" Text='<%# Eval("addr_nomer") %>' Width="270px" Title="Адреса - номер будинку" /></td>
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
                                                <td><dx:ASPxDateEdit ID="EditActualFinishDate" runat="server" Value='<%# Eval("rent_actual_finish_date") %>' Width="190px" Title="Фактична дата закінчення оренди" >
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
                                                <td><dx:ASPxSpinEdit ID="EditCostExpert" runat="server" NumberType="Float" Value='<%# Eval("cost_expert_total") %>' Width="190px" Title="Ринкова вартість приміщень" /></td>
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
                                                    <dx:ASPxCheckBox ID="CheckSubarenda" runat="server" Text="Суборенда" Checked='<%# 1.Equals(Eval("is_subarenda")) %>' Title="Суборенда" />
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
                                                    <dx:ASPxCheckBox ID="CheckLoanAgreement" runat="server"  Text="Договір позички" Checked='<%# 1.Equals(Eval("is_loan_agreement")) %>' Title="Договір позички" Font-Bold="true" />
                                                </td>
                                                <td> &nbsp; </td>
                                                <td colspan="2">
                                                    <dx:ASPxRadioButton ID="RadioAgreementToxic" runat="server" GroupName="AgreementState" Value='<%# 1.Equals(Eval("agr_toxic")) %>'
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

                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                                            <EditButton visible="True"> <Image ToolTip="Редагувати Підставу" Url="../Styles/EditIcon.png" /> </EditButton>
                                            <DeleteButton visible="True"> <Image ToolTip="Видалити Підставу" Url="../Styles/DeleteIcon.png" /> </DeleteButton>
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
                                    KeyFieldName="id" Width="810px"
                                    OnRowDeleting="GridViewNotes_RowDeleting"
                                    OnRowUpdating="GridViewNotes_RowUpdating" >

                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="60px">
                                            <EditButton visible="True"> <Image ToolTip="Редагувати Об'єкт" Url="../Styles/EditIcon.png" /> </EditButton>
                                            <DeleteButton visible="True"> <Image ToolTip="Видалити Об'єкт" Url="../Styles/DeleteIcon.png" /> </DeleteButton>
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
                                        <dx:GridViewDataTextColumn FieldName="cost_agreement" VisibleIndex="12" Caption="Плата за використання, грн." Width="85px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataComboBoxColumn FieldName="note_status_id" VisibleIndex="13" Caption="Стан використання приміщення" Width="140px">
                                            <PropertiesComboBox DataSourceID="SqlDataSourceArendaNoteStatus" ValueField="id" TextField="name" ValueType="System.Int32" />
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
                                                    <td> <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Плата за використання, грн." Width="150px" /> </td>
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
                                    Title="Звітний Період" ClientInstanceName="ReportingPeriodCombo">
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
                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
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
                                                <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Нараховано орендної плати за звітний період, грн. (без ПДВ)" Width="650px"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentNarah_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("payment_narah") %>' Width="150px"
                                                            Title="Нараховано орендної плати за звітний період">
                                                              <ClientSideEvents 
                                                            ValueChanged ="function (s, e) { HideValidator(); }"
                                                            />
                                                            </dx:ASPxSpinEdit></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Сальдо на початок року (незмінна впродовж року величина), грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentSaldo_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("last_year_saldo") %>' Width="150px"
                                                            Title="Переплачено орендної плати на початок звітного періоду"/></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Надходження орендної плати за звітний період, всього, грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentReceived_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("payment_received") %>' Width="150px"
                                                            Title="Надходження орендної плати за звітний період"/></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <dx:ASPxButton ID="BtnAddPaymentDocument" runat="server" Text="Розрахувати" AutoPostBack="false" Width="150px">
                                                                <ClientSideEvents Click="function (s,e) { CPRentPayment.PerformCallback('calc:');  }" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="- у тому числі, з нарахованої за звітний період (без боргів та переплат)"></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentNarZvit_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("payment_nar_zvit") %>' Width="150px"
                                                            Title="Надходження орендної плати з нарахованої за звітний період">
                                                             <ClientSideEvents 
                                                            ValueChanged ="function (s, e) { HideValidator(); }"
                                                            />
                                                            </dx:ASPxSpinEdit></td>
                                                    </tr>
                                                    <tr>
                                                        <td><dx:ASPxLabel ID="ASPxLabel55" runat="server" Text="Погашення заборгованості минулих періодів, грн."></dx:ASPxLabel></td>
                                                        <td><dx:ASPxSpinEdit ID="EditPaymentOldDebtsPayed_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("old_debts_payed") %>' Width="150px"
                                                            Title="Погашення заборгованості минулих періодів"/></td>
                                                    </tr>
                                                </table>
                                            </dx:panelcontent>    
                                            </PanelCollection>                                                    
                                        </dx:ASPxCallbackPanel>
                                    </dx:PanelContent>
                                </PanelCollection>                                
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="AdditionalInfoPanel" runat="server" HeaderText="Додаткові відомості">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent11" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxCheckBox ID="CheckRenterIsOut" runat="server" Text="Орендар виїхав" Checked='<%# 1.Equals(Eval("is_renter_out")) %>' Title="Орендар виїхав" />
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
                                                <td><dx:ASPxSpinEdit ID="EditCollectionDebtTotal" ClientInstanceName="EditCollectionDebtTotal" runat="server" NumberType="Float" Value='<%# Eval("debt_total") %>' Width="100px"
                                                    Title="Загальна заборгованість по орендній платі">
                                                     <ClientSideEvents 
                                                            ValueChanged ="function (s, e) { HideValidator(); }"
                                                     />
                                                    </dx:ASPxSpinEdit></td>
                                                <td><div style="width:30px;"></div></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="- за звітний період" Width="280px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCollectionDebtZvit" ClientInstanceName="EditCollectionDebtZvit" runat="server" NumberType="Float" Value='<%# Eval("debt_zvit") %>' Width="100px"
                                                    Title="Заборгованість по орендній платі за звітний період"/></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="- поточна до 3-х місяців"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCollectionDebt3Month" ClientInstanceName="EditCollectionDebt3Month" runat="server" NumberType="Float" Value='<%# Eval("debt_3_month") %>' Width="100px"
                                                    Title="Заборгованість по орендній платі поточна до 3-х місяців"/></td>
                                                <td></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="- прострочена від 3 до 12 місяців"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCollectionDebt12Month" ClientInstanceName="EditCollectionDebt12Month" runat="server" NumberType="Float" Value='<%# Eval("debt_12_month") %>' Width="100px"
                                                    Title="Заборгованість по орендній платі прострочена від 3 до 12 місяців"/></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="- прострочена від 1 до 3 років"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCollectionDebt3Years" ClientInstanceName="EditCollectionDebt3Years" runat="server" NumberType="Float" Value='<%# Eval("debt_3_years") %>' Width="100px"
                                                    Title="Заборгованість по орендній платі прострочена від 1 до 3 років"/></td>
                                                <td></td>                                                
                                                <td><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="- безнадійна більше 3-х років"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCollectionDebtOver3Years" ClientInstanceName="EditCollectionDebtOver3Years" runat="server" NumberType="Float" Value='<%# Eval("debt_over_3_years") %>' Width="100px"
                                                    Title="Заборгованість по орендній платі безнадійна більше 3-х років"/></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4"><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="Заборгованість з орендної плати (із загальної заборгованості), розмір якої встановлено в межах витрат на утримання, грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCollectionDebtVMezhahVitrat" ClientInstanceName="EditCollectionDebtVMezhahVitrat" runat="server" NumberType="Float" Value='<%# Eval("debt_v_mezhah_vitrat") %>' Width="100px"
                                                    Title="Заборгованість з орендної плати, розмір якої встановлено в межах витрат на утримання"/></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4"><dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Списано заборгованості з орендної плати у звітному періоді, грн. (без ПДВ)"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCollectionDebtSpysano" ClientInstanceName="EditCollectionDebtSpysano" runat="server" NumberType="Float" Value='<%# Eval("debt_spysano") %>' Width="100px"
                                                    Title="Списано заборгованості з орендної плати у звітному періоді"/></td>
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
                                                    <Columns>
                                                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="60px">
                                                            <EditButton visible="True"> <Image ToolTip="Редагувати Платіж" Url="../Styles/EditIcon.png" /> </EditButton>
                                                            <DeleteButton visible="True"> <Image ToolTip="Видалити Платіж" Url="../Styles/DeleteIcon.png" /> </DeleteButton>
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

                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanelImageGallery" ShowLoadingPanel="false" EnableViewState="true"
                                    ClientInstanceName="ASPxCallbackPanelImageGallery" runat="server" 
                                    OnCallback="ASPxCallbackPanelImageGallery_Callback">
                            
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

                                            <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Visible ="true"
                                                ClientInstanceName="btnUpload" Text="Завантажити" 
                                            onclick="btnUpload_Click">
                                                <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                            </dx:ASPxButton>

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
            <dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { if (ReportingPeriodCombovalidate()){ CPMainPanel.PerformCallback('save:'); }}"
                   LostFocus="HidePnl" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonSend" runat="server" Text="Надіслати" AutoPostBack="false"  CausesValidation="true">
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

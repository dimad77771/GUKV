<%@ page language="C#" autoeventwireup="true" inherits="Finance_FinanceView, App_Web_financeview.aspx.a413dc5" masterpagefile="~/NoHeader.master" title="Фінансовий Аналіз" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register assembly="DevExpress.XtraReports.v13.1.Web, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register src="../UserControls/FieldFixxer.ascx" tagname="FieldFixxer" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 185);
    }

    function GridViewFinanceInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewFinanceEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
    }

    function ShowFieldFixxerPopupControl(s, e) { PopupFieldFixxer.Show(); }

    /// Data comparisons //////////////////////////////////////////////////////////////////

    function OnButtonCompareClick(s, e) {

        var val1 = "" + ComboCompareField1.GetValue();
        var val2 = "" + ComboCompareField2.GetValue();

        if (val1 != null && val1 != undefined &&
            val2 != null && val2 != undefined) {

            var colName = "" + EditCompareColumnName.GetText();

            if (colName != "") {

                PopupCompare.Hide();

                var param = "compare:" + val1 + ":" + val2 + ":" + colName;

                if (CheckCompareByPercent.GetChecked()) {
                    param = param + "%";
                }

                PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam(param));
            }
            else {
                alert("Будь ласка, введіть назву колонки, що додається.");
            }
        }
        else {
            alert("Будь ласка, виберіть два показники для порівняння.");
        }
    }

    function GenerateCompareFieldName() {
        var name1 = "" + ComboCompareField1.GetText();
        var name2 = "" + ComboCompareField2.GetText();

        if (CheckCompareByPercent.GetChecked()) {
            EditCompareColumnName.SetText("Відносна динаміка " + name1 + " та " + name2);
        }
        else {
            EditCompareColumnName.SetText("Динаміка " + name1 + " та " + name2);
        }
    }

    /// Data Summing //////////////////////////////////////////////////////////////////////

    function OnButtonSumFieldsClick(s, e) {

        var val1 = "" + ComboSumField1.GetValue();
        var val2 = "" + ComboSumField2.GetValue();

        if (val1 != null && val1 != undefined &&
            val2 != null && val2 != undefined) {

            var colName = "" + EditSumColumnName.GetText();

            if (colName != "") {

                PopupSumFields.Hide();

                var tag = "sum:";

                if (RbSumFieldsDiff.GetChecked()) {
                    tag = "diff:";
                }

                var param = "" + tag + val1 + ":" + val2 + ":" + colName;

                PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam(param));
            }
            else {
                alert("Будь ласка, введіть назву колонки, що додається.");
            }
        }
        else {
            alert("Будь ласка, виберіть два показники для поєднання.");
        }
    }

    function GenerateSumFieldName() {
        var name1 = "" + ComboSumField1.GetText();
        var name2 = "" + ComboSumField2.GetText();

        if (RbSumFieldsDiff.GetChecked()) {
            EditSumColumnName.SetText("Різниця " + name1 + " та " + name2);
        }
        else {
            EditSumColumnName.SetText("Сума " + name1 + " та " + name2);
        }
    }

    /// Form View /////////////////////////////////////////////////////////////////////////

    function ShowFormCard() {

        var formId = ComboFormName.GetValue();

        if (formId != null && formId != undefined) {

            PopupOpenForm.Hide();

            var cardUrl = "FinForm.aspx?fid=" + formId.toString();

            window.open(cardUrl);

        } else {

            alert( "Будь ласка, виберіть період, тип звітності та назву форми." );
        }
    }

    function OnComboFormPeriodChanged() {

        var val1 = ComboFormPeriod.GetValue();
        var val2 = ComboFormReportType.GetValue();

        var val1text = "";
        var val2text = "";

        if (val1 != null && val1 != undefined) {
            val1text = "" + val1.toString();
        }

        if (val2 != null && val2 != undefined) {
            val2text = "" + val2.toString();
        }

        ComboFormName.PerformCallback(val1text + "|" + val2text);
    }

    function OnComboFormReportTypeChanged() {

        OnComboFormPeriodChanged();
    }

    /// Adding new 'Value' field //////////////////////////////////////////////////////////

    function OnComboValueColPeriodChanged() {

        var val1 = ComboValueColPeriod.GetValue();
        var val2 = ComboValueColReportType.GetValue();

        var val1text = "";
        var val2text = "";

        if (val1 != null && val1 != undefined) {
            val1text = "" + val1.toString();
        }

        if (val2 != null && val2 != undefined) {
            val2text = "" + val2.toString();
        }

        ComboValueColForm.PerformCallback(val1text + "|" + val2text);
    }

    function OnComboValueColReportTypeChanged() {

        OnComboValueColPeriodChanged();
    }

    function OnComboValueColFormEndCallback() {

        if (ComboValueColForm.GetItemCount() > 0) {

            ComboValueColForm.SetSelectedIndex(0);
        }

        OnComboValueColFormChanged();
    }

    function OnComboValueColFormChanged() {

        var val = ComboValueColForm.GetValue();

        if (val != null && val != undefined) {

            CPValueColRows.PerformCallback(val.toString());
        }
    }

    function CorrectColumnTitle(s) {

        var name = "" + s;

        var corrected = name.replace("[i] ", "");

        if (corrected.indexOf("грн") < 0 &&
            corrected.indexOf("Норматив ") < 0 &&
            corrected.indexOf("працюючих") < 0 &&
            corrected.indexOf("працівників ") < 0) {

            corrected = corrected + " (тис. грн.)";
        }

        return corrected;
    }

    function OnListBoxValueColRowsChanged() {

        var item = ListBoxValueColRows.GetSelectedItem();

        if (item != null && item != undefined) {

            EditValueColName.SetText(CorrectColumnTitle(item.text));
        }
    }

    function OnAddValueColumn(s, e) {

        var item = ListBoxValueColRows.GetSelectedItem();

        if (item != null && item != undefined) {

            var valForm = ComboValueColForm.GetValue();
            var valueRow = item.value;
            var valueCol = ComboValueColColumns.GetValue();

            if (valForm != null && valForm != undefined &&
                valueRow != null && valueRow != undefined &&
                valueCol != null && valueCol != undefined) {

                PopupAddValueColumn.Hide();

                var txtForm = "" + valForm;
                var txtRow = "" + valueRow;
                var txtCol = "" + valueCol;

                PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("addvalcolumn&#" + txtForm + "&#" + txtRow + "&#" + txtCol + "&#" + EditValueColName.GetText()));
            }
            else {
                alert("Будь ласка, виберіть період, тип звітності, назву форми, рядок та колонку у вибраній формі.");
            }
        }
        else {
            alert("Будь ласка, виберіть період, тип звітності, назву форми, рядок та колонку у вибраній формі.");
        }
    }

    /// Adding new 'Formula' field //////////////////////////////////////////////////////////

    function OnComboFormulaColPeriodChanged() {

        var val = ComboFormulaColPeriod.GetValue();

        if (val != null && val != undefined) {

            ComboFormula.PerformCallback("" + val);
        }
    }

    function OnComboFormulaChanged() {

        var val = ComboFormula.GetValue();

        if (val != null && val != undefined) {

            EditFormulaColName.SetText(CorrectColumnTitle(ComboFormula.GetText()));
        }
    }

    function OnComboFormulaEndCallback() {

        if (ComboFormula.GetItemCount() > 0) {

            ComboFormula.SetSelectedIndex(0);
        }

        OnComboFormulaChanged();
    }

    function OnAddFormulaColumn(s, e) {

        var val = ComboFormula.GetValue();

        if (val != null && val != undefined) {

            PopupAddFormulaColumn.Hide();

            PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("addformulacolumn&#" + val + "&#" + EditFormulaColName.GetText()));
        }
        else {
            alert("Будь ласка, виберіть період та формулу.");
        }
    }

    /// Removing all special columns //////////////////////////////////////////////////////////

    function OnButtonClearColumnsClick(s, e) {

        if (confirm("Усі колонки, що містять числові показники, будуть видалені. Продовжувати?")) {

            PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("clearcols:"));
        }
    }

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictPeriods" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, convert(VARCHAR(32), period_start, 104) AS 'name' from fin_report_periods WHERE (is_deleted = 0) AND (NOT period_start IS NULL) ORDER BY period_start">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictFinRepType" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name from dict_fin_report_type ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictForms" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, form_name + ' (Форма ' + form_number + ')' AS 'name' FROM fin_report_forms WHERE period_id = @per_id AND report_type_id = @rep_t ORDER BY form_number"
    OnSelecting="SqlDataSourceDictForms_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="per_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_t" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFormulae" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from fin_report_formulae where period_id = @per_id"
    OnSelecting="SqlDataSourceFormulae_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="per_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceValueColForms" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, form_name + ' (Форма ' + form_number + ')' AS 'name' FROM fin_report_forms WHERE period_id = @per_id AND report_type_id = @rep_t ORDER BY form_number"
    OnSelecting="SqlDataSourceValueColForms_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="per_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_t" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceValueColColumns" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT cl.col_index, cl.cell_text AS 'name'
                    FROM fin_report_cells cl INNER JOIN fin_report_forms frm ON frm.id = cl.form_id
                    WHERE cl.form_id = @fid AND cl.row_index = 1 AND cl.col_index > frm.num_fixed_cols ORDER BY cl.col_index"
    OnSelecting="SqlDataSourceValueColColumns_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="fid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceValueColRows" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT cl.row_index, cl.cell_text AS 'name'
                    FROM fin_report_cells cl INNER JOIN fin_report_forms frm ON frm.id = cl.form_id
                    WHERE cl.form_id = @fid AND cl.col_index = 1 AND cl.row_index > frm.num_fixed_rows ORDER BY cl.row_index"
    OnSelecting="SqlDataSourceValueColRows_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="fid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<center>

<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tr>
        <td colspan="2">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Фінансова звітність підприємств" CssClass="reporttitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table border="0" cellspacing="4" cellpadding="0">
                <tr>
                    <td>
                        <dx:ASPxPopupControl ID="PopupAddValueColumn" runat="server" 
                            HeaderText="Додати Значення" 
                            ClientInstanceName="PopupAddValueColumn" 
                            PopupElementID="ButtonOpenValueColumnPopup">
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                    
                                    <dx:ASPxLabel ID="LabelValueColPeriod" runat="server" Text="Період:"></dx:ASPxLabel>

                                    <dx:ASPxComboBox ID="ComboValueColPeriod" runat="server" ClientInstanceName="ComboValueColPeriod"
                                        DataSourceID="SqlDataSourceDictPeriods"
                                        DropDownStyle="DropDownList"
                                        ValueType="System.Int32"
                                        TextField="name"
                                        ValueField="id"
                                        Width="350px"
                                        IncrementalFilteringMode="None"
                                        EnableViewState="False"
                                        EnableSynchronization="False" >

                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnComboValueColPeriodChanged(); }" />
                                    </dx:ASPxComboBox>

                                    <br/>

                                    <dx:ASPxLabel ID="LabelValueColType" runat="server" Text="Тип Звітності:"></dx:ASPxLabel>

                                    <dx:ASPxComboBox runat="server" ID="ComboValueColReportType" ClientInstanceName="ComboValueColReportType" 
                                        DataSourceID="SqlDataSourceDictFinRepType"
                                        DropDownStyle="DropDownList"
                                        TextField="name"
                                        ValueField="id"
                                        ValueType="System.Int32"
                                        Width="350px"
                                        IncrementalFilteringMode="None"
                                        EnableViewState="False"
                                        EnableSynchronization="False"
                                        SelectedIndex="1" >

                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnComboValueColReportTypeChanged(); }" />
                                    </dx:ASPxComboBox>

                                    <br/>

                                    <dx:ASPxLabel ID="LabelValueColForm" runat="server" Text="Форма:"></dx:ASPxLabel>

                                    <dx:ASPxComboBox ID="ComboValueColForm" runat="server" ClientInstanceName="ComboValueColForm"
                                        DataSourceID="SqlDataSourceValueColForms"
                                        DropDownStyle="DropDownList"
                                        ValueType="System.Int32"
                                        TextField="name"
                                        ValueField="id"
                                        Width="350px"
                                        IncrementalFilteringMode="None"
                                        EnableCallbackMode="True"
                                        CallbackPageSize="50"
                                        EnableViewState="False"
                                        EnableSynchronization="False"
                                        OnCallback="ComboValueColForm_Callback" >

                                        <ClientSideEvents
                                            SelectedIndexChanged="function(s, e) { OnComboValueColFormChanged(); }"
                                            EndCallback="function(s, e) { OnComboValueColFormEndCallback(); }" />
                                    </dx:ASPxComboBox>

                                    <br/>

                                    <dx:ASPxCallbackPanel ID="CPValueColRows" runat="server" ClientInstanceName="CPValueColRows" OnCallback="CPValueColRows_Callback">
                                        <PanelCollection>
                                            <dx:panelcontent ID="Panelcontent0" runat="server">

                                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Рядок у Формі:"></dx:ASPxLabel>

                                                <dx:ASPxListBox ID="ListBoxValueColRows" ClientInstanceName="ListBoxValueColRows" runat="server"
                                                    DataSourceID="SqlDataSourceValueColRows" ValueField="row_index" TextField="name"
                                                    Width="350px" Height="200px" SelectionMode="Single" ValueType="System.Int32" >

                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnListBoxValueColRowsChanged(); }" />
                                                </dx:ASPxListBox>

                                                <br/>

                                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Колонка у Формі:"></dx:ASPxLabel>

                                                <dx:ASPxComboBox runat="server" ID="ComboValueColColumns" ClientInstanceName="ComboValueColColumns" 
                                                    DataSourceID="SqlDataSourceValueColColumns"
                                                    DropDownStyle="DropDownList"
                                                    TextField="name"
                                                    ValueField="col_index"
                                                    ValueType="System.Int32"
                                                    Width="350px"
                                                    IncrementalFilteringMode="None"
                                                    EnableViewState="False"
                                                    EnableSynchronization="False" >
                                                </dx:ASPxComboBox>

                                            </dx:panelcontent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>

                                    <br/>

                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Назва колонки, що додається:"></dx:ASPxLabel>

                                    <dx:ASPxTextBox ID="EditValueColName" ClientInstanceName="EditValueColName" runat="server" Width="350px"> </dx:ASPxTextBox>

                                    <br/>

                                    <center>
                                    <dx:ASPxButton ID="ButtonAddValueColumn" runat="server" AutoPostBack="False" Text="Додати в Таблицю" Width="148px">
                                        <ClientSideEvents Click="OnAddValueColumn" />
                                    </dx:ASPxButton>
                                    </center>

                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="ButtonOpenValueColumnPopup" runat="server" AutoPostBack="False" Text="Показники Звітності"></dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxPopupControl ID="PopupAddFormulaColumn" runat="server" 
                            HeaderText="Додати Значення з Формули" 
                            ClientInstanceName="PopupAddFormulaColumn" 
                            PopupElementID="ButtonOpenFormulaColumnPopup">
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                    
                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Період:"></dx:ASPxLabel>

                                    <dx:ASPxComboBox ID="ComboFormulaColPeriod" runat="server" ClientInstanceName="ComboFormulaColPeriod"
                                        DataSourceID="SqlDataSourceDictPeriods"
                                        DropDownStyle="DropDownList"
                                        ValueType="System.Int32"
                                        TextField="name"
                                        ValueField="id"
                                        Width="350px"
                                        IncrementalFilteringMode="None"
                                        EnableViewState="False"
                                        EnableSynchronization="False" >

                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnComboFormulaColPeriodChanged(); }" />
                                    </dx:ASPxComboBox>

                                    <br/>

                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Формула:"></dx:ASPxLabel>

                                    <dx:ASPxComboBox ID="ComboFormula" runat="server" ClientInstanceName="ComboFormula"
                                        DataSourceID="SqlDataSourceFormulae"
                                        DropDownStyle="DropDownList"
                                        ValueType="System.Int32"
                                        TextField="name"
                                        ValueField="id"
                                        Width="350px"
                                        IncrementalFilteringMode="None"
                                        EnableCallbackMode="True"
                                        CallbackPageSize="50"
                                        EnableViewState="False"
                                        EnableSynchronization="False"
                                        OnCallback="ComboFormula_Callback" >

                                        <ClientSideEvents
                                            SelectedIndexChanged="function(s, e) { OnComboFormulaChanged(); }"
                                            EndCallback="function(s, e) { OnComboFormulaEndCallback(); }" />
                                    </dx:ASPxComboBox>

                                    <br/>

                                    <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Назва колонки, що додається:"></dx:ASPxLabel>

                                    <dx:ASPxTextBox ID="EditFormulaColName" ClientInstanceName="EditFormulaColName" runat="server" Width="350px"> </dx:ASPxTextBox>

                                    <br/>

                                    <center>
                                    <dx:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" Text="Додати в Таблицю" Width="148px">
                                        <ClientSideEvents Click="OnAddFormulaColumn" />
                                    </dx:ASPxButton>
                                    </center>

                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="ButtonOpenFormulaColumnPopup" runat="server" AutoPostBack="False" Text="Формули Загальних Показників"></dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxPopupControl ID="PopupCompare" runat="server" 
                            ClientInstanceName="PopupCompare" 
                            HeaderText="Динаміка показників" 
                            PopupElementID="ButtonCompare">

                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">

                                    <dx:ASPxCallbackPanel ID="CallbackPanelCompare"
                                        ClientInstanceName="CallbackPanelCompare"
                                        runat="server" >
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">

                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Перший показник для порівняння:"></dx:ASPxLabel>
                                                <dx:ASPxComboBox ID="ComboCompareField1" runat="server" ValueType="System.Int32" Width="300px" ClientInstanceName="ComboCompareField1"
                                                    OnCallback="ComboCompareField_Callback" >
                                                    <ClientSideEvents
                                                        EndCallback="function (s, e) { GenerateCompareFieldName(); }"
                                                        SelectedIndexChanged="function (s, e) { GenerateCompareFieldName(); }" />
                                                </dx:ASPxComboBox>
                                                <br/>

                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Другий показник для порівняння:"></dx:ASPxLabel>
                                                <dx:ASPxComboBox ID="ComboCompareField2" runat="server" ValueType="System.Int32" Width="300px" ClientInstanceName="ComboCompareField2"
                                                    OnCallback="ComboCompareField_Callback" >
                                                    <ClientSideEvents
                                                        EndCallback="function (s, e) { GenerateCompareFieldName(); }"
                                                        SelectedIndexChanged="function (s, e) { GenerateCompareFieldName(); }" />
                                                </dx:ASPxComboBox>
                                                <br/>

                                                <dx:ASPxCheckBox ID="CheckCompareByPercent" ClientInstanceName="CheckCompareByPercent" runat="server" Checked='False' Text="Відносне Відхилення">
                                                    <ClientSideEvents CheckedChanged="function (s, e) { GenerateCompareFieldName(); }"/>
                                                </dx:ASPxCheckBox>
                                                <br/>

                                                <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Назва колонки, що додається:"></dx:ASPxLabel>
                                                <dx:ASPxTextBox ID="EditCompareColumnName" ClientInstanceName="EditCompareColumnName" runat="server" Width="300px"> </dx:ASPxTextBox>
                                                <br/>

                                                <center>
                                                <dx:ASPxButton ID="ButtonDoCompare" runat="server" AutoPostBack="False" Text="Порівняти" Width="148px">
                                                    <ClientSideEvents Click="OnButtonCompareClick" />
                                                </dx:ASPxButton>
                                                </center>

                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>

                                </dx:PopupControlContentControl>
                            </ContentCollection>

                            <ClientSideEvents PopUp="function (s, e) { ComboCompareField1.PerformCallback(''); ComboCompareField2.PerformCallback(''); }" />
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="ButtonCompare" ClientInstanceName="ButtonCompare" runat="server" AutoPostBack="False" Text="Динаміка Показників"></dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxPopupControl ID="PopupSumFields" runat="server" 
                            ClientInstanceName="PopupSumFields" 
                            HeaderText="Поєднання Колонок" 
                            PopupElementID="ButtonSumFieldsPopup">

                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">

                                    <dx:ASPxCallbackPanel ID="CallbackPanelSumFields" ClientInstanceName="CallbackPanelSumFields" runat="server" >
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">

                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Перший показник для поєднання:"></dx:ASPxLabel>
                                                <dx:ASPxComboBox ID="ComboSumField1" runat="server" ValueType="System.Int32" Width="300px" ClientInstanceName="ComboSumField1"
                                                    OnCallback="ComboSumField_Callback" >
                                                    <ClientSideEvents
                                                        EndCallback="function (s, e) { GenerateSumFieldName(); }"
                                                        SelectedIndexChanged="function (s, e) { GenerateSumFieldName(); }" />
                                                </dx:ASPxComboBox>
                                                <br/>

                                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Другий показник для поєднання:"></dx:ASPxLabel>
                                                <dx:ASPxComboBox ID="ComboSumField2" runat="server" ValueType="System.Int32" Width="300px" ClientInstanceName="ComboSumField2"
                                                    OnCallback="ComboSumField_Callback" >
                                                    <ClientSideEvents
                                                        EndCallback="function (s, e) { GenerateSumFieldName(); }"
                                                        SelectedIndexChanged="function (s, e) { GenerateSumFieldName(); }" />
                                                </dx:ASPxComboBox>
                                                <br/>

                                                <dx:ASPxRadioButton runat="server" GroupName="RbSumFieldsGroup1" ID="RbSumFieldsSum" ClientInstanceName="RbSumFieldsSum"
                                                    Checked="true" AutoPostBack="false" Text="Додати Значення" Width="300px">
                                                    <ClientSideEvents CheckedChanged="function (s, e) { GenerateSumFieldName(); }"/>
                                                </dx:ASPxRadioButton>
                                                <dx:ASPxRadioButton runat="server" GroupName="RbSumFieldsGroup1" ID="RbSumFieldsDiff" ClientInstanceName="RbSumFieldsDiff"
                                                    Checked="false" AutoPostBack="false" Text="Відняти Значення" Width="300px">
                                                    <ClientSideEvents CheckedChanged="function (s, e) { GenerateSumFieldName(); }"/>
                                                </dx:ASPxRadioButton>
                                                <br/>

                                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Назва колонки, що додається:"></dx:ASPxLabel>
                                                <dx:ASPxTextBox ID="EditSumColumnName" ClientInstanceName="EditSumColumnName" runat="server" Width="300px"> </dx:ASPxTextBox>
                                                <br/>

                                                <center>
                                                <dx:ASPxButton ID="ButtonSumFields" runat="server" AutoPostBack="False" Text="Поєднати" Width="148px">
                                                    <ClientSideEvents Click="OnButtonSumFieldsClick" />
                                                </dx:ASPxButton>
                                                </center>

                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>

                                </dx:PopupControlContentControl>
                            </ContentCollection>

                            <ClientSideEvents PopUp="function (s, e) { ComboSumField1.PerformCallback(''); ComboSumField2.PerformCallback(''); }" />
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="ButtonSumFieldsPopup" ClientInstanceName="ButtonSumFieldsPopup" runat="server" AutoPostBack="False" Text="Поєднання Колонок"></dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="ButtonRemoveCustomColumns" runat="server" AutoPostBack="False" Text="Очистити Таблицю">
                            <ClientSideEvents Click="OnButtonClearColumnsClick" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </td>
        <td align="right">
            <table border="0" cellspacing="4" cellpadding="0">
                <tr>
                    <td>
                        <dx:ASPxButton ID="ButtonShowFoldersPopup1" runat="server" AutoPostBack="False" Text="Зберегти звіт">
                            <ClientSideEvents Click="ShowFoldersPopupControl" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Додаткові Колонки">
                            <ClientSideEvents Click="ShowFieldChooserPopupControl" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки">
                            <ClientSideEvents Click="ShowFieldFixxerPopupControl" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                            HeaderText="Збереження у Файлі" 
                            ClientInstanceName="ASPxPopupControl_Finance_SaveAs" 
                            PopupElementID="ASPxButton_Finance_SaveAs">
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                    <dx:ASPxButton ID="ASPxButton2" runat="server" 
                                        Text="XLS - Microsoft Excel&reg;" 
                                        OnClick="ASPxButton_Finance_ExportXLS_Click" Width="180px">
                                    </dx:ASPxButton>
                                    <br />
                                    <dx:ASPxButton ID="ASPxButton4" runat="server" 
                                        Text="PDF - Adobe Acrobat&reg;" 
                                        OnClick="ASPxButton_Finance_ExportPDF_Click" Width="180px">
                                    </dx:ASPxButton>
                                    <br />
                                    <dx:ASPxButton ID="ASPxButton5" runat="server" 
                                        Text="CSV - значення, розділені комами" 
                                        OnClick="ASPxButton_Finance_ExportCSV_Click" Width="180px">
                                    </dx:ASPxButton>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="ASPxButton_Finance_SaveAs" runat="server" AutoPostBack="False" Text="Зберегти у Файлі" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterFinance" runat="server" 
    FileName="ФiнансовийЗвiт" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif"></Default>
        <AlternatingRowCell BackColor="#E0E0E0"></AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFinance" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM report_financial">
</mini:ProfiledSqlDataSource>

<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceFinance" 
    KeyFieldName="organization_id"
    Width="100%"
    EnableRowsCache="False"
    OnCustomCallback="GridViewFinance_CustomCallback"
    OnCustomFilterExpressionDisplayText="GridViewFinance_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewFinance_ProcessColumnAutoFilter"
    OnCustomUnboundColumnData="GridViewFinance_CustomUnboundColumnData"
    OnCustomColumnSort="GridViewFinance_CustomColumnSort" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} суб'єктів" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="0" Visible="True" Caption="Повна назва" Width="250px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="False" Caption="Коротка Назва" Width="250px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="True" Caption="Код ЄДРПОУ"></dx:GridViewDataTextColumn>
                           
        <dx:GridViewDataTextColumn FieldName="industry" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="3" Visible="False" Caption="Галузь (1НФ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="occupation" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="4" Visible="False" Caption="Вид Діяльності (1НФ)" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="True" Caption="Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="True" Caption="Вид Діяльності" Width="120px"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="form_gosp" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="False" Caption="Форма фінансування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_of_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="False" Caption="Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="gosp_struct" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="False" Caption="Госп. Структура"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="False" Caption="Орган управління"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_form" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Орг.-правова форма госп."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_organ" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="False" Caption="Орган госп. упр."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="status" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="False" Caption="Фіз. / Юр. Особа"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="addr_district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="True" Caption="Район" Width="150px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" ReadOnly="True" ShowInCustomizationForm="True" 
            
            VisibleIndex="15" Visible="True" Caption="Назва Вулиці" Width="200px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="16" Visible="True" Caption="Номер Будинку"><Settings SortMode="Custom" /></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="addr_korpus" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="Номер Будинку - Корпус"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_zip_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Поштовий Індекс"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_fio" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="False" Caption="ФІО Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_phone" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="False" Caption="Тел. Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_fio" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="ФІО Бухгалтера"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="buhgalter_phone" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="False" Caption="Тел. Бухгалтера"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="fax" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="False" Caption="Факс"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_auth" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="False" Caption="Реєстраційний Орган"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="False" Caption="Номер Запису про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="False" Caption="Дата Реєстрації"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="registration_svidot" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="27" Visible="False" Caption="Номер Свідоцтва про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="kved_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="28" Visible="False" Caption="КВЕД"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="koatuu" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="29" Visible="False" Caption="ОАТУУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Відділ ДКВ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="mayno" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="False" Caption="Правовий режим майна"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="is_liquidated" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="False" Caption="Ліквідовано"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="liquidation_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="False" Caption="Дата Ліквідації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_email" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="34" Visible="False" Caption="Ел. Адреса"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_posada" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="False" Caption="Контактна Особа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sfera_upr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="36" Visible="False" Caption="Сфера Управління"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="addr_address" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="37" Visible="False" Caption="Адреса Підприємства"></dx:GridViewDataTextColumn>
            
        <dx:GridViewDataTextColumn FieldName="is_under_closing" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="38" Visible="False" Caption="В стадії реорганізації/ліквідації"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager AlwaysShowPager="true" PageSize="25" Summary-Text="Сторінка {0} з {1} (Всього суб'єктів: {2})"> </SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="True" 
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True" 
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        VerticalScrollBarMode="Visible"
        VerticalScrollBarStyle="Virtual" />
    <SettingsCookies CookiesID="GUKV.Finance" Version="A21" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewFinanceInit" EndCallback="GridViewFinanceEndCallback" />
</dx:ASPxGridView>

</center>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <uc1:SaveReportCtrl ID="SaveReportCtrl1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="PopupFieldFixxer" runat="server" 
    HeaderText="Закріпити Колонки" 
    ClientInstanceName="PopupFieldFixxer" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
            <uc3:FieldFixxer ID="FieldFixxer1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns1.PerformCallback(); }" />
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="PopupFieldChooser" runat="server" 
    HeaderText="Додаткові Колонки" 
    ClientInstanceName="PopupFieldChooser" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>

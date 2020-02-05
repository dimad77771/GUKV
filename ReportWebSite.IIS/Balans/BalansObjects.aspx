<%@ page language="C#" autoeventwireup="true" inherits="Balans_BalansObjects, App_Web_balansobjects.aspx.c41eb26c" masterpagefile="~/NoHeader.master" title="Об'єкти на Балансі" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AddressPicker.ascx" tagname="AddressPicker" tagprefix="uc2" %>
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

    function ShowFieldFixxerPopupControl(s, e) { PopupFieldFixxer.Show(); }

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 185);
    }

    function GridViewBalansObjectsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewBalansObjectsEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function CheckBoxBalansObjectsComVlasn_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function CheckBoxBalansObjectsShowDeleted_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function ShowAddressPickerPopupControl(s, e) {

        PopupAddressPicker.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
    }

    // ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Balans/BalansObjects.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Balans/BalansArenda.aspx" Text="Договори Оренди по Об'єктах"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Balans/BalansOrgList.aspx" Text="Перелік Балансоутримувачів"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<%-- Content of the first tab BEGIN --%>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Об'єкти на балансі" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxBalansObjectsComVlasn" runat="server" Checked='False' Text="Лише Ком. Власність"
                Width="145px" ClientInstanceName="CheckBoxBalansObjectsComVlasn" >
                <ClientSideEvents CheckedChanged="CheckBoxBalansObjectsComVlasn_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxBalansObjectsShowDeleted" runat="server" Checked='False' Text="Видалені"
                Width="80px" ClientInstanceName="CheckBoxBalansObjectsShowDeleted" >
                <ClientSideEvents CheckedChanged="CheckBoxBalansObjectsShowDeleted_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonQuickSearchAddr1" runat="server" AutoPostBack="False" Text="" ImageSpacing="0px" AllowFocus="false"
                ToolTip="Щвидкий пошук за адресою">
                <Image Url="../Styles/HouseIcon.png" />
                <FocusRectPaddings Padding="1px" />
                <ClientSideEvents Click="ShowAddressPickerPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup1" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
		<td>
			<dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_BalansObjects_SaveAs" 
                PopupElementID="ASPxButton_BalansObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton3" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_BalansObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_BalansObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_BalansObjects_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_BalansObjects_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterBalansObjects" runat="server" 
    FileName="Баланс" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansObjects" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_balans_all WHERE
        ((@p_com_filter = 0) OR (@p_com_filter <> 0 AND (org_ownership_int IN (32,33,34) OR form_ownership_int IN (32,33,34)))) AND
        ((@p_show_deleted = 1) OR (@p_show_deleted = 0 AND (is_deleted IS NULL OR is_deleted = 0 OR is_not_accepted = 1))) AND
        ((@p_rda_district_id = 0) OR (org_sfera_upr_id = 4 AND org_district_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceBalansObjects_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_com_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_show_deleted" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceYesNo" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_yes_no" >
</mini:ProfiledSqlDataSource>

<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceBalansObjects" 
    KeyFieldName="balans_id"
    Width="100%"
    OnCustomCallback="GridViewBalansObjects_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewBalansObjects_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewBalansObjects_ProcessColumnAutoFilter"
    OnCustomSummaryCalculate="GridViewBalansObjects_CustomSummaryCalculate"
    OnCustomColumnSort="GridViewBalansObjects_CustomColumnSort" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="balans_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="True" Caption="Картка">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowBalansCard(" + Eval("balans_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="organization_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="1" Visible="False" Caption="ID Балансоутримувача">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("organization_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="True" Caption="Балансоутримувач - Повна Назва" Width="180px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("org_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="False" Caption="Балансоутримувач - Коротка Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("org_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="False" Caption="Балансоутримувач - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="False" Caption="Балансоутримувач - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="False" Caption="Балансоутримувач - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="True" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="True" Caption="Назва Вулиці" Width="120px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="True" Caption="Номер Будинку">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
            </DataItemTemplate>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="True" Caption="Загальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_pidval" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Підвальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_vlas_potreb" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="False" Caption="Площа Для Власних Потреб (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="False" Caption="Вільна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_in_rent" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="False" Caption="Площа Надана В Оренду (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_privatizov" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="False" Caption="Приватизована Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_not_for_rent" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="16" Visible="False" Caption="Площа Не Для Оренди (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_gurtoj" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="Площа Гуртожитку (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_non_habit" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Нежитлова Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_balans" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="True" Caption="Балансова Вартість (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert_1m" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="False" Caption="Оціночна Вартість За Кв.м. (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="Оціночна Вартість (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_zalishkova" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="False" Caption="Залишкова Вартість (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_rent_agr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="False" Caption="Кількість Договорів Оренди"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_privat_apt" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="False" Caption="Кількість Приватизованих Приміщень"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="approval_by" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="False" Caption="Дозвіл - Джерело"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="approval_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="False" Caption="Дозвіл - Номер"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="approval_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="27" Visible="False" Caption="Дозвіл - Дата"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="o26_code" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="28" Visible="False" Caption="О26"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bti_condition" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="29" Visible="False" Caption="Документація БТІ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_floors" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Кількість Поверхів"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_maintain_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="31" Visible="False" Caption="ID Утримуючої Організації">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_maintain_id") + ")\">" + Eval("org_maintain_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_maintainer_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="False" Caption="Утримуюча Організація - Повна Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_maintain_id") + ")\">" + Eval("org_maintainer_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_maintainer_short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="False" Caption="Утримуюча Організація - Коротка Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_maintain_id") + ")\">" + Eval("org_maintainer_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_maintainer_zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="34" Visible="False" Caption="Утримуюча Організація - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="False" Caption="Відділ ДКВ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="36" Visible="False" Caption="Форма Власності Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ownership_type" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="37" Visible="False" Caption="Право"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="38" Visible="True" Caption="Вид Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_type" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="39" Visible="True" Caption="Тип Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="condition" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="40" Visible="False" Caption="Стан Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="41" Visible="False" Caption="Група Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="42" Visible="False" Caption="Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="history" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="43" Visible="False" Caption="Історична Цінність"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="date_expert" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="44" Visible="False" Caption="Дата Затвердження Оцінки"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="floors" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="45" Visible="False" Caption="Поверхи"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_bti_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="46" Visible="False" Caption="Код БТІ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="memo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="47" Visible="False" Caption="Додаткова Інформація"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="note" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="48" Visible="False" Caption="Примітка"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="znos" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="49" Visible="False" Caption="Знос (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="znos_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="50" Visible="False" Caption="Знос станом на"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="org_vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="51" Visible="False" Caption="Балансоутримувач - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="52" Visible="False" Caption="Балансоутримувач - Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="input_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="53" Visible="False" Caption="Дата Актуальності"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="is_in_privat" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="54" Visible="False" Caption="Будинок В Програмі Приватизації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="balans_obj_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="55" Visible="False" Caption="Назва Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="56" Visible="False" Caption="Вільні Приміщення: Загальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_korysna" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="57" Visible="False" Caption="Вільні Приміщення: Корисна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_mzk" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="58" Visible="False" Caption="Вільні Приміщення: МЗК (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_floors" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="59" Visible="False" Caption="Місце Розташування Вільного Приміщення (поверх)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_purpose" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="60" Visible="False" Caption="Можливе Використання Вільного Приміщення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_on_balans" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="61" Visible="False" Caption="Об'єкт Перебуває На Балансі"></dx:GridViewDataTextColumn>
        <dx:GridViewDataComboBoxColumn FieldName="is_not_accepted" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="62" Visible="False" Caption="Об'єкт Знято З Балансу, Але Не Прийнято">
            <PropertiesComboBox DataSourceID="SqlDataSourceYesNo" ValueField="id" TextField="name" ValueType="System.Int32" />
        </dx:GridViewDataComboBoxColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_pidval" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_vlas_potreb" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_in_rent" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_privatizov" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_not_for_rent" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_gurtoj" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_non_habit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_balans" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert_1m" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_zalishkova" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="znos" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free_korysna" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free_mzk" SummaryType="Custom" DisplayFormat="{0}" />
    </TotalSummary>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
        <dx:ASPxSummaryItem FieldName="cost_balans" SummaryType="Sum" DisplayFormat="Балансова Вартість = {0} тис.грн." />
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="Загальна Площа = {0} кв.м." />
    </GroupSummary>

    <SettingsBehavior EnableCustomizationWindow="True"
        AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager AlwaysShowPager="true" PageSize="25"></SettingsPager>
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
    <SettingsCookies CookiesID="GUKV.BalansObjects" Version="A2_2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewBalansObjectsInit" EndCallback="GridViewBalansObjectsEndCallback" />
</dx:ASPxGridView>

<%-- Content of the first tab END --%>

</center>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" SupportsDisabledAttribute="True">
            <uc1:SaveReportCtrl ID="SaveReportCtrl2" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="PopupAddressPicker" runat="server" 
    HeaderText="Швидкий Пошук За Адресою" 
    ClientInstanceName="PopupAddressPicker" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
            <uc2:AddressPicker ID="AddressPicker2" runat="server"/>
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
        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>

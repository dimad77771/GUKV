<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RentAgreements.aspx.cs" Inherits="Arenda_RentAgreements" MasterPageFile="~/NoHeader.master" Title="Договори Оренди" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
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

    function GridViewArendaObjectsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewArendaObjectsEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function CheckBoxRentedObjectsComVlasn_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("bind:"));
    }

    function ShowAddressPickerPopupControl(s, e) {

        PopupAddressPicker.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
    }

    function CheckBoxRentedObjectsDPZ_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("bind:"));
    }

	function CheckBoxBigBorgShow_CheckedChanged(s, e) {

		PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("bind:"));
	}

	function CheckBoxBalansObjectsShowNeziznacheni_CheckedChanged(s, e) {

		PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("bind:"));
    }

	function OnContextMenuItemClick(s, e) {
        if (e.item.name === "Report_5") {
            //s.GetRowValues(e.elementIndex, "arenda_id;insurance_end", BuildReport5);
            //return;

            var id = s.GetRowKey(e.elementIndex)
			console.log('id', id)
			window.open(
				'\\Reports1NF\\Report5.aspx?id=' + id,
				//'RentAgreements.aspx',
				'_blank',
			)
		}
    }


    function BuildReport5(values) {
        console.log('values2', values)

   //     var id = values[0]
   //     setTimeout(() => {
			//window.open(
			//	'\\Reports1NF\\Report5.aspx?id=' + id,
			//	//'RentAgreements.aspx',
			//	'_blank',
			//)
   //     }, 100);
	}

    // ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
    <Items>
        <dx:MenuItem NavigateUrl="../Arenda/RentAgreements.aspx" Text="Договори Оренди"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Arenda/RentedObjects.aspx" Text="Орендовані Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Arenda/RentOrgList.aspx" Text="Перелік Орендарів"></dx:MenuItem>
		<dx:MenuItem NavigateUrl="../Arenda/RentSubleases.aspx" Text="Договори Суборенди"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Договори Оренди" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxBigBorgShow" runat="server" Checked='False' Text="Заборгованість понад 4 місяці" ForeColor="Red"
                Width="250px" ClientInstanceName="CheckBoxBigBorgShow" >
                <ClientSideEvents CheckedChanged="CheckBoxBigBorgShow_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>        
        <td>
            <dx:ASPxCheckBox ID="CheckBoxRentedObjectsDPZ" runat="server" Checked='True' Text="Дані ДПЗ"
                Width="100px" ClientInstanceName="CheckBoxRentedObjectsDPZ" >
                <ClientSideEvents CheckedChanged="CheckBoxRentedObjectsDPZ_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>        
        <td>
            <dx:ASPxCheckBox ID="CheckBoxBalansObjectsShowNeziznacheni" runat="server" Checked='False' Text="Недіючі" ToolTip="Показувати недіючі"
                Width="80px" ClientInstanceName="CheckBoxBalansObjectsShowNeziznacheni" >
                <ClientSideEvents CheckedChanged="CheckBoxBalansObjectsShowNeziznacheni_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxRentedObjectsComVlasn" runat="server" Checked='False' Text="Лише Ком. Власність" Visible="false"
                Width="155px" ClientInstanceName="CheckBoxRentedObjectsComVlasn" >
                <ClientSideEvents CheckedChanged="CheckBoxRentedObjectsComVlasn_CheckedChanged" />
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
	        <dx:ASPxButton ID="ASPxButtonEditColumnList" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px">
		        <ClientSideEvents Click="ShowFieldFixxerPopupControl" />
	        </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButtonEditColumnList2" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_ArendaObjects_SaveAs" 
                PopupElementID="ASPxButton_ArendaObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton2" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_ArendaObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_ArendaObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_ArendaObjects_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_ArendaObjects_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreecycleStepDict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select rtrim(ltrim(concat(namf,' ',nami,' ',namo))) fio, id from dict_orandodavec_user order by 1">
</mini:ProfiledSqlDataSource>


<dx:ASPxGridViewExporter ID="ASPxGridViewExporterArendaObjects" runat="server" 
    FileName="Оренда" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

    <%--EnableCaching="true"--%>
<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaObjects" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
A.*
,ar.orandodavec_user_id
,(select rtrim(ltrim(concat(Q2.namf,' ',Q2.nami,' ',Q2.namo))) from reports1nf Q1 join dict_orandodavec_user Q2 on Q2.id = Q1.orandodavec_user_id where Q1.organization_id = A.org_balans_id) as orandodavec_user_name2
,W.big_month_koef

        FROM reptab_RentAgreements A
        join arenda ar on ar.id = A.arenda_id
        left join (select * from bigborg_arenda(case when @p_bigborg_filter = 1 then @p_bigborg_email else '-' end)) W on W.arenda_id = ar.id

        WHERE 
    --isnull(ar.is_deleted, 0) = 0 and 
 	    ((@p_dpz_filter = 0) OR (@p_dpz_filter <> 0 AND ar.id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = ar.org_balans_id and ISNULL(b.is_deleted, 0) = 0 /*and b.agreement_state = 1*/ ) )) AND
        ((@p_com_filter = 0) OR (@p_com_filter <> 0 AND (A.balans_form_ownership_int IN (32,33,34) OR A.balans_org_ownership_int IN (32,33,34)))) AND
        ( (@p_bigborg_filter = 0) OR (@p_bigborg_filter = 1 AND W.arenda_id is not null) ) AND
        ((@p_show_neziznacheni = 1) OR (@p_show_neziznacheni = 0 AND (isnull(A.name, 'Невідомо') <> 'Невизначені'))) AND
        (   (@p_rda_district_id = 0) OR
            (A.org_balans_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND A.org_balans_district_id = @p_rda_district_id) OR
            (A.org_giver_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND A.org_giver_district_id = @p_rda_district_id) OR
            (A.org_renter_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND A.org_renter_district_id = @p_rda_district_id))

        AND 
        (
            isnull(@ref_balans_id,0) <= 0
                OR 
            A.arenda_id in (select distinct Q.arenda_id from view_arenda Q where Q.ref_balans_id = @ref_balans_id and isnull(Q.is_deleted,0)=0)
        )
        order by case when isnull(org_balans_zkpo,'') in('00000000','000000000','') then 2 else 1 end, org_balans_zkpo
    "

    OnSelecting="SqlDataSourceArendaObjects_Selecting"
    
UpdateCommand="UPDATE [arenda]
SET
    [orandodavec_user_id] = @orandodavec_user_id
WHERE id = @arenda_id"

    onupdating="SqlDataSourceFreeSquare_Updating"
    
    >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="1" Name="p_dpz_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_bigborg_filter" />
        <asp:Parameter DbType="String" DefaultValue="" Name="p_bigborg_email" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_com_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_show_neziznacheni" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="ref_balans_id" />
    </SelectParameters>



</mini:ProfiledSqlDataSource>

<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceArendaObjects" 
    KeyFieldName="arenda_id"
    Width="100%"
    OnRowUpdated="PrimaryGridView_RowUpdated"
    OnCustomCallback="GridViewArendaObjects_CustomCallback"
    OnCustomFilterExpressionDisplayText="GridViewArendaObjects_CustomFilterExpressionDisplayText"
    OnCustomSummaryCalculate="GridViewArendaObjects_CustomSummaryCalculate"
    OnProcessColumnAutoFilter = "GridViewArendaObjects_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewArendaObjects_CustomColumnSort"
    OnFillContextMenuItems="PrimaryGridView_FillContextMenuItems" >

    <SettingsContextMenu Enabled="true"/>

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
        <dx:GridViewCommandColumn VisibleIndex="0" Width="30px" ButtonType="Image" CellStyle-Wrap="True" FixedStyle="Left" 
            ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" >

            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataTextColumn FieldName="arenda_id" ReadOnly="True" ShowInCustomizationForm="False" Width="50px" 
            VisibleIndex="0" Visible="True" Caption="Картка">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowArendaCardEx(" + Eval("ex_reports1nf_arenda") + "," + Eval("arenda_id") + "," + Eval("arenda_report_id") + ")\"><img border='0' src='../Styles/" + ((int)Eval("is_dpz_object") == 1 ? "EditIcon_green.png" : "EditIcon.png") + "'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="1" Visible="False" Caption="ID Балансоутримувача">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_full_name" ReadOnly="True"
            VisibleIndex="2" Visible="False" Caption="Балансоутримувач - Повна Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_short_name" ReadOnly="True"
            VisibleIndex="3" Visible="False" 
            Caption="Балансоутримувач - Коротка Назва" Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="org_balans_zkpo" ReadOnly="True"
            VisibleIndex="4" Visible="False" Caption="Балансоутримувач - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_industry" ReadOnly="True"
            VisibleIndex="5" Visible="False" Caption="Балансоутримувач - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_occupation" ReadOnly="True"
            VisibleIndex="6" Visible="False" 
            Caption="Балансоутримувач - Вид Діяльності" Width="200"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="7" Visible="False" Caption="ID Орендаря">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_full_name" ReadOnly="True"
            VisibleIndex="8" Visible="False" Caption="Орендар - Повна Назва" Width="200">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_short_name" ReadOnly="True"
            VisibleIndex="9" Visible="True" Caption="Орендар - Коротка Назва" 
            Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_zkpo" ReadOnly="True"
            VisibleIndex="10" Visible="False" Caption="Орендар - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_industry" ReadOnly="True"
            VisibleIndex="11" Visible="False" Caption="Орендар - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_occupation" ReadOnly="True"
            VisibleIndex="12" Visible="False" Caption="Орендар - Вид Діяльності" Width="200"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_director_email" ReadOnly="True"
            VisibleIndex="12" Visible="False" Caption="Орендар - Ел. Адреса Керівника" Width="150"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="13" Visible="False" Caption="ID Орендодавця">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_full_name" ReadOnly="True"
            VisibleIndex="14" Visible="False" Caption="Орендодавець - Повна Назва" Width="200">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_short_name" ReadOnly="True"
            VisibleIndex="15" Visible="True" Caption="Орендодавець - Коротка Назва" 
            Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_zkpo" ReadOnly="True"
            VisibleIndex="16" Visible="False" Caption="Орендодавець - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_industry" ReadOnly="True"
            VisibleIndex="17" Visible="False" Caption="Орендодавець - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_occupation" ReadOnly="True"
            VisibleIndex="18" Visible="False" Caption="Орендодавець - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True"
            VisibleIndex="19" Visible="False" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True"
            VisibleIndex="20" Visible="True" Caption="Назва Вулиці" Width="140px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True"
            VisibleIndex="21" Visible="True" Caption="Номер Будинку">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
            </DataItemTemplate>

            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="agreement_date" ReadOnly="True"
            VisibleIndex="28" Visible="False" Caption="Дата укладання договору"></dx:GridViewDataDateColumn>
<%--         <dx:GridViewDataTextColumn FieldName="agreement_date_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="29" Visible="False" Caption="Дата Договору Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_date_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="30" Visible="False" Caption="Дата Договору Оренди - Квартал"></dx:GridViewDataTextColumn>     --%>
        <dx:GridViewDataTextColumn FieldName="agreement_num" ReadOnly="True"
            VisibleIndex="31" Visible="True" Caption="Номер Договору Оренди"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="count_dogovor_objects" ReadOnly="True"
            VisibleIndex="31" Visible="True" Caption="Кількість об’єктів за договором">
            <CellStyle HorizontalAlign="Center"></CellStyle>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="list_dogovor_objects" ReadOnly="True"
            VisibleIndex="31" Visible="False" Caption="ID об’єктів за договором">
            <CellStyle HorizontalAlign="Center"></CellStyle>
        </dx:GridViewDataTextColumn>


<%--        <dx:GridViewDataTextColumn FieldName="floor_number" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="32" Visible="False" Caption="Поверх"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_narah" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="True" Caption="Середня Ставка за використання (%)"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="priznachennya" VisibleIndex="33" Caption="Призначення за Документом" ShowInCustomizationForm="True" Visible="False" Width="150"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="cost_payed" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="34" Visible="False" Caption="Сплачена Вартість (грн.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="cost_debt" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="False" Caption="Борг (грн.)"></dx:GridViewDataTextColumn>       --%>
        <dx:GridViewDataTextColumn FieldName="n_cost_agreement" ReadOnly="True"
            VisibleIndex="36" Visible="True" Caption="Місячна орендна плата, грн."></dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="cost_agreement_max" ReadOnly="True"
            VisibleIndex="37" Visible="True" Caption="Максимальна Орендна Плата за об'єкт договору (грн.)"></dx:GridViewDataTextColumn>--%>
<%--        <dx:GridViewDataTextColumn FieldName="cost_narah_max" ReadOnly="True"
            VisibleIndex="38" Visible="True" Caption="Ставка за використання (%) об'єкту з макс.орен.платою"></dx:GridViewDataTextColumn>
          <dx:GridViewDataTextColumn FieldName="n_cost_expert_1m" ReadOnly="True" 
            VisibleIndex="37" Visible="True" Caption="Оціночна Вартість За Кв.м. (грн.)"></dx:GridViewDataTextColumn>   --%> 
        <dx:GridViewDataDateColumn FieldName="date_expert" ReadOnly="True"
            VisibleIndex="39" Visible="True" Caption="Дата, на яку проведена оцінка об'єкту"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert_total" ReadOnly="True"
            VisibleIndex="40" Visible="True" Caption="Оціночна вартість приміщень за договором, грн"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_cost_expert_total" ReadOnly="True"
            VisibleIndex="41" Visible="True" Caption="Ринкова вартість приміщень, грн"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="debt_timespan" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="42" Visible="False" Caption="Час Заборгованості"></dx:GridViewDataTextColumn>      
        <dx:GridViewDataTextColumn FieldName="pidstava_display" ReadOnly="True"
            VisibleIndex="43" Visible="False" Caption="Підстава"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataDateColumn FieldName="rent_start_date" ReadOnly="True" 
            VisibleIndex="44" Visible="False" Caption="Початок Оренди"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="rent_start_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="45" Visible="False" Caption="Початок Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_start_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="46" Visible="False" Caption="Початок Оренди - Квартал"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataDateColumn FieldName="rent_finish_date" ReadOnly="True"
            VisibleIndex="47" Visible="True" Caption="Закінчення Оренди"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="rent_finish_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="48" Visible="False" Caption="Закінчення Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_finish_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="49" Visible="False" Caption="Закінчення Оренди - Квартал"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataDateColumn FieldName="rent_actual_finish_date" ReadOnly="True"
            VisibleIndex="50" Visible="False" Caption="Фактичне Закінчення Оренди"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="actual_finish_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="51" Visible="False" Caption="Фактичне Закінчення Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="actual_finish_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="52" Visible="False" Caption="Фактичне Закінчення Оренди - Квартал"></dx:GridViewDataTextColumn>     --%>
<%--        <dx:GridViewDataTextColumn FieldName="rent_rate_percent" ReadOnly="True"
            VisibleIndex="52" Visible="False" Caption="Ставка %"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_rate_uah" ReadOnly="True"
            VisibleIndex="53" Visible="False" Caption="Ставка (грн.)"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataTextColumn FieldName="rent_square" ReadOnly="True" 
            VisibleIndex="54" Visible="False" Caption="Площа що орендується, кв.м"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="num_akt" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="55" Visible="False" Caption="Номер Акту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="date_akt" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="56" Visible="False" Caption="Дата Акту"></dx:GridViewDataDateColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="is_subarenda" ReadOnly="True"
            VisibleIndex="57" Visible="False" Caption="Суборенда"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_type" ReadOnly="True"
            VisibleIndex="58" Visible="False" Caption="Вид Розрахунків"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_active_s" ReadOnly="True"
            VisibleIndex="59" Visible="False" Caption="Стан договору"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_vedomstvo" ReadOnly="True"
            VisibleIndex="60" Visible="False" Caption="Балансоутримувач - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_vedomstvo" ReadOnly="True"
            VisibleIndex="61" Visible="False" Caption="Орендар - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_vedomstvo" ReadOnly="True" 
            VisibleIndex="62" Visible="False" Caption="Орендодавець - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
 <%--       <dx:GridViewDataTextColumn FieldName="balans_sqr_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="63" Visible="False" Caption="Балансоутримувач - Загальна Площа На Балансі (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="balans_num_rent_agr" ReadOnly="True"
            VisibleIndex="64" Visible="False" Caption="Балансоутримувач - Кількість Договорів Оренди"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="stanjuro" ReadOnly="True"
            VisibleIndex="64" Visible="True" Caption="Балансоутримувач - стан юр. особи"></dx:GridViewDataTextColumn> 

        <dx:GridViewDataTextColumn FieldName="balans_sqr_in_rent" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="65" Visible="False" Caption="Балансоутримувач - Загальна Площа Надана В Оренду (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_form_ownership" ReadOnly="True"
            VisibleIndex="67" Visible="False" Caption="Балансоутримувач - Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_gosp" ReadOnly="True" 
            VisibleIndex="68" Visible="False" Caption="Балансоутримувач - Форма фінансування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_organ" ReadOnly="True" 
            VisibleIndex="68" Visible="False" Caption="Балансоутримувач - Орган госп. упр."></dx:GridViewDataTextColumn>

<%--        <dx:GridViewDataTextColumn FieldName="agreement_num_int" ReadOnly="True"
            VisibleIndex="68" Visible="False" Caption="Номер Договору Оренди (число)"></dx:GridViewDataTextColumn>     
        <dx:GridViewDataTextColumn FieldName="is_in_privat" ReadOnly="True"
            VisibleIndex="69" Visible="False" Caption="Будинок В Програмі Приватизації"></dx:GridViewDataTextColumn>      --%>
<%--        <dx:GridViewDataTextColumn FieldName="sqr_free_total" ReadOnly="True"
            VisibleIndex="70" Visible="False" Caption="Вільні Приміщення: Загальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_korysna" ReadOnly="True"
            VisibleIndex="71" Visible="False" Caption="Вільні Приміщення: Корисна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_mzk" ReadOnly="True"
            VisibleIndex="72" Visible="False" Caption="Вільні Приміщення: МЗК (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_floors" ReadOnly="True"
            VisibleIndex="73" Visible="False" Caption="Місце Розташування Вільного Приміщення (поверх)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_purpose" ReadOnly="True"
            VisibleIndex="74" Visible="False" Caption="Можливе Використання Вільного Приміщення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_form_of_ownership" ReadOnly="True"
            VisibleIndex="75" Visible="False" Caption="Орендар - Форма Власності"></dx:GridViewDataTextColumn>     --%>

        <dx:GridViewDataTextColumn FieldName="org_balans_org_form" ReadOnly="True"
            VisibleIndex="76" Visible="False" Caption="Балансоутримувач - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_org_form" ReadOnly="True"
            VisibleIndex="77" Visible="False" Caption="Орендодавець - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_org_form" ReadOnly="True"
            VisibleIndex="78" Visible="False" Caption="Орендар - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contribution_rate" ReadOnly="True"
            VisibleIndex="79" Visible="False" Caption="Ставка відрахувань до бюджету (%)"></dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="modify_date" ReadOnly="True"
            VisibleIndex="80" Visible="False" Caption="Дата Актуальності"></dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn FieldName="has_reports1nf_photos" ReadOnly="True" 
            VisibleIndex="80" Visible="True" Caption="Наявність фото/плану" Width="40px">
            <DataItemTemplate>
               <%# Eval("has_reports1nf_photos").Equals(1) ?
                       "<center><img border='0' src='../Styles/photo.png'/></center>"
                       : ""
               %>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False" AllowAutoFilter="False" AllowHeaderFilter="False" />
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn FieldName="payment_narah" ReadOnly="True"
            VisibleIndex="81" Visible="True" Caption="Нараховано орендної плати за звітний період, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="last_year_saldo" ReadOnly="True"
            VisibleIndex="82" Visible="True" Caption="Сальдо (переплата) на початок року (незмінна впродовж року величина), грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="avance_plat" ReadOnly="True"
            VisibleIndex="82" Visible="True" Caption="Авансова орендна плата, грн."></dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="payment_received" ReadOnly="True"
            VisibleIndex="83" Visible="True" Caption="Надходження орендної плати за звітний період, всього, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_nar_zvit" ReadOnly="True"
            VisibleIndex="84" Visible="True" Caption="- у тому числі, з нарахованої за звітний період (без боргів та переплат)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_debts_payed" ReadOnly="True"
            VisibleIndex="85" Visible="True" Caption="Погашення заборгованості минулих періодів, грн"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="return_orend_payed" ReadOnly="True"
            VisibleIndex="86" Visible="True" Caption="Переплата орендної плати всього, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="return_all_orend_payed" ReadOnly="True"
            VisibleIndex="86" Visible="True" Caption="Повернення переплати орендної плати всього за звітний період, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataCheckColumn FieldName="use_calc_debt" ReadOnly="True"
            VisibleIndex="86" Visible="True" Caption="Розраховувати заборгованість з орендної плати"></dx:GridViewDataCheckColumn>


        <dx:GridViewDataTextColumn FieldName="debt_total" ReadOnly="True"
            VisibleIndex="86" Visible="True" Caption="Загальна заборгованість по орендній платі - всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_zvit" ReadOnly="True"
            VisibleIndex="87" Visible="True" Caption="Заборгованість по орендній платі за звітний період"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_month" ReadOnly="True"
            VisibleIndex="88" Visible="True" Caption="Заборгованість по орендній платі поточна до 3-х місяців"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_12_month" ReadOnly="True"
            VisibleIndex="89" Visible="True" Caption="Заборгованість по орендній платі прострочена від 4 до 12 місяців"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_years" ReadOnly="True"
            VisibleIndex="90" Visible="True" Caption="Заборгованість по орендній платі прострочена від 1 до 3 років"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_over_3_years" ReadOnly="True"
            VisibleIndex="91" Visible="True" Caption="Заборгованість по орендній платі безнадійна більше 3-х років"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_v_mezhah_vitrat" ReadOnly="True"
            VisibleIndex="92" Visible="True" Caption="Заборгованість з орендної плати (із загальної заборгованості), розмір якої встановлено в межах витрат на утримання, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_spysano" ReadOnly="True"
            VisibleIndex="93" Visible="True" Caption="Списано заборгованості з орендної плати у звітному періоді, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_total" ReadOnly="True"
            VisibleIndex="94" Visible="True" Caption="Кількість заходів (попереджень, приписів і т.п.), всього"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="num_pozov_total" ReadOnly="True"
            VisibleIndex="94" Visible="True" Caption="- кількість позовів до суду, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_total" ReadOnly="True"
            VisibleIndex="94" Visible="True" Caption="- задоволено позовів, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_total" ReadOnly="True"
            VisibleIndex="94" Visible="True" Caption="- відкрито виконавчих впроваджень, всього"></dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn FieldName="num_zahodiv_zvit" ReadOnly="True"
            VisibleIndex="95" Visible="True" Caption="Кількість заходів (попереджень, приписів і т.п.), за звітний період"></dx:GridViewDataTextColumn>



        <dx:GridViewDataTextColumn FieldName="insurance_sum" ReadOnly="True"
            VisibleIndex="96" Visible="True" Caption="Вартість об'єкту страхування, грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="insurance_start" ReadOnly="True"
            VisibleIndex="97" Visible="True" Caption="Дата початку періоду страхування"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="insurance_end" ReadOnly="True"
            VisibleIndex="98" Visible="True" Caption="Дата закінчення періоду страхування"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="sphera_dialnosti" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="99" Visible="True" Caption="Сфера діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_form_of_ownership" ReadOnly="True"  ShowInCustomizationForm="True"
            VisibleIndex="100" Visible="False" Caption="Орендар - Форма власності"></dx:GridViewDataTextColumn>


        <dx:GridViewDataCheckColumn FieldName="is_discount" ReadOnly="True"
            VisibleIndex="110" Visible="True" Caption="Знижка"></dx:GridViewDataCheckColumn>
        <dx:GridViewDataTextColumn FieldName="zvilneno_percent" ReadOnly="True"
            VisibleIndex="111" Visible="True" Caption="Звільнено від сплати орендної плати на (%)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="zvilneno_date1" ReadOnly="True"
            VisibleIndex="112" Visible="True" Caption="Звільнено від сплати орендної плати на (з)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="zvilneno_date2" ReadOnly="True"
            VisibleIndex="113" Visible="True" Caption="Звільнено від сплати орендної плати на (по)"></dx:GridViewDataDateColumn>

<%--        <dx:GridViewDataTextColumn FieldName="zvilbykmp_percent" ReadOnly="True"
            VisibleIndex="114" Visible="True" Caption="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332 (%)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="zvilbykmp_date1" ReadOnly="True"
            VisibleIndex="115" Visible="True" Caption="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332 (з)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="zvilbykmp_date2" ReadOnly="True"
            VisibleIndex="116" Visible="True" Caption="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332 (по)"></dx:GridViewDataDateColumn>--%>


<%--        <dx:GridViewDataDateColumn FieldName="povidoleno1_date" ReadOnly="True"
            VisibleIndex="117" Visible="True" Caption="Повідомлення орендаря до балансоутримувача про неможлівість використання (дата)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno1_num" ReadOnly="True"
            VisibleIndex="118" Visible="True" Caption="Повідомлення орендаря до балансоутримувача про неможлівість використання (№)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="povidoleno2_date" ReadOnly="True"
            VisibleIndex="119" Visible="True" Caption="Повідомлення орендаря до орендодавця про неможлівість використання (дата)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno2_num" ReadOnly="True"
            VisibleIndex="120" Visible="True" Caption="Повідомлення орендаря до орендодавця про неможлівість використання (№)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="povidoleno3_date" ReadOnly="True"
            VisibleIndex="121" Visible="True" Caption="Повідомлення орендаря до балансоутримувача про намір використовувати об'єкт (дата)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno3_num" ReadOnly="True"
            VisibleIndex="122" Visible="True" Caption="Повідомлення орендаря до балансоутримувача про намір використовувати об'єкт (№)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="povidoleno4_date" ReadOnly="True"
            VisibleIndex="123" Visible="True" Caption="Повідомлення орендаря до орендодавця про намір використовувати об'єкт (дата)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno4_num" ReadOnly="True"
            VisibleIndex="124" Visible="True" Caption="Повідомлення орендаря до орендодавця про намір використовувати об'єкт (№)"></dx:GridViewDataTextColumn>
--%>
        <dx:GridViewDataTextColumn FieldName="big_month_koef" ReadOnly="True" Width="80px"
            VisibleIndex="130" Visible="True" Caption="Поточна заборгованість, у місячних ОП"></dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="prozoro_number" Caption="Унікальний код обєкту у ЕТС Прозорро-продажі" VisibleIndex="130" Width="150px" Visible="False">
			<DataItemTemplate>
				<%# "<a target=\"_blank\" href=\"https://prozorro.sale/auction/" + Eval("prozoro_number") + "\">" + Eval("prozoro_number") + "</a>"%>
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn FieldName="arenda_id" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="130" Visible="False" Caption="ID договору"></dx:GridViewDataTextColumn>


        <dx:GridViewDataComboBoxColumn FieldName="orandodavec_user_id" Caption="Контроль орендодавця" Width="200px" VisibleIndex="130">
            <PropertiesComboBox 
				DataSourceID="SqlDataSourceFreecycleStepDict"
				DropDownStyle="DropDownList"
				DropDownWidth="500px"
                AllowNull="true"
				TextField="fio"  
				ValueField="id">
            </PropertiesComboBox>  
        </dx:GridViewDataComboBoxColumn>

        <%--<dx:GridViewDataTextColumn FieldName="orandodavec_user_name2" ReadOnly="True"
            VisibleIndex="131" Visible="True" Caption="Контроль орендодавця2"></dx:GridViewDataTextColumn>--%>




<%--        <dx:GridViewDataTextColumn FieldName="n_cost_narah" ReadOnly="True" VisibleIndex="78" Caption="(NEW) Орендна ставка (%)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_rent_rate" ReadOnly="True" VisibleIndex="79" Caption="(NEW) Орендна ставка (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_cost_expert_total" ReadOnly="True" VisibleIndex="81" Caption="(NEW) Експертна вартість (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_cost_agreement" ReadOnly="True" VisibleIndex="82" Caption="(NEW) Орендна Плата (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_rent_square" ReadOnly="True" VisibleIndex="83" Caption="(NEW) Площа за договором"></dx:GridViewDataTextColumn>--%>


    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="cost_agreement" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="balans_sqr_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="balans_num_rent_agr" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="balans_sqr_in_rent" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free_korysna" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free_mzk" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="count_dogovor_objects" SummaryType="Sum" DisplayFormat="{0}"/>


        <dx:ASPxSummaryItem FieldName="n_cost_agreement" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_agreement_max" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="n_cost_expert_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_narah" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="last_year_saldo" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_received" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_nar_zvit" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="return_orend_payed" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_spysano" SummaryType="Custom" DisplayFormat="{0}" />

        <dx:ASPxSummaryItem FieldName="debt_zvit" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_month" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_12_month" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_years" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_over_3_years" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_v_mezhah_vitrat" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_zvit" SummaryType="Sum" DisplayFormat="{0}" />

        <dx:ASPxSummaryItem FieldName="num_pozov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zadov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_vikon_total" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Custom" DisplayFormat="Загальна Орендована Площа = {0} кв.м." />
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
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.ArendaAgreements" Version="A2_34" Enabled="true" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewArendaObjectsInit" EndCallback="GridViewArendaObjectsEndCallback" ContextMenuItemClick="OnContextMenuItemClick" />
</dx:ASPxGridView>

</center>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
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
        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
            <uc2:AddressPicker ID="AddressPicker1" runat="server"/>
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


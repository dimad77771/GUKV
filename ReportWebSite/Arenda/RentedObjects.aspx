<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RentedObjects.aspx.cs" Inherits="Arenda_RentedObjects"
    MasterPageFile="~/NoHeader.master" Title="Орендовані Об'єкти" %>

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

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
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
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Об'єкти, що орендуються" CssClass="reporttitle"></asp:Label>
        </td>
          <td>
            <dx:ASPxCheckBox ID="CheckBoxRentedObjectsDPZ" runat="server" Checked='True' Text="Дані ДПЗ"
                Width="100px" ClientInstanceName="CheckBoxRentedObjectsDPZ" >
                <ClientSideEvents CheckedChanged="CheckBoxRentedObjectsDPZ_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>       
       <td>
            <dx:ASPxCheckBox ID="CheckBoxRentedObjectsComVlasn" runat="server" Checked='False' Text="Лише Ком. Власність"
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaObjects" runat="server" EnableCaching="false"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT m.*
    , substring(m.payment_type_obj, 1, 60)  as payment_type_obj_nam 
    , substring(m.factich_vikorist, 1, 60)  as 'factich_vikorist_type'
    , pryzn4doc = (select top 1 doc_display_name from view_arenda_link_2_decisions ld where ld.arenda_id = m.arenda_id order by ld.link_id) 
    , cast(CAST(CHECKSUM(NEWID()) & 0x7fffffff AS float) / CAST (0x7fffffff AS int) as varchar(1000)) as factich_vikorist_obj2
    --, r.rental_rate
    --, ar.cost_expert_total as cost_expert_total_agr
    , stan_prym = (select name FROM dbo.dict_arenda_note_status st where m.note_status_id = st.id)
    , isnull(ddd.name, 'Невідомо') as sphera_dialnosti

    FROM view_arenda m /*m_view_arenda m */
    left join 
	(
		select obp.org_id,occ.name from org_by_period obp
		join dict_rent_occupation occ on occ.id = obp.org_occupation_id
		where obp.period_id = (select top 1 id from dict_rent_period order by id desc)
	) DDD on DDD.org_id = m.org_balans_id
    --join arenda ar on ar.id = m.arenda_id 
    --left join arenda_notes n on n.arenda_id = ar.id and m.arenda_note_id = n.id 
    --left join dict_rental_rate r on n.payment_type_id = r.id
    WHERE isnull(m.is_deleted,0)=0 and 
 	    ((@p_dpz_filter = 0) OR (@p_dpz_filter <> 0 AND m.arenda_id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = m.org_balans_id and ISNULL(b.is_deleted, 0) = 0 /*and b.agreement_state = 1*/ ) )) AND 
        ((@p_com_filter = 0) OR (@p_com_filter <> 0 AND (balans_form_ownership_int IN (32,33,34) OR balans_org_ownership_int IN (32,33,34)))) AND
        (   (@p_rda_district_id = 0) OR
            (org_balans_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND org_balans_district_id = @p_rda_district_id) OR
            (org_giver_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND org_giver_district_id = @p_rda_district_id) OR
            (org_renter_form_ownership_id  in (select id from dict_org_ownership where is_rda = 1) AND org_renter_district_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceArendaObjects_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="1" Name="p_dpz_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_com_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceArendaObjects" 
    KeyFieldName="arenda_id"
    Width="100%"
    OnCustomCallback="GridViewArendaObjects_CustomCallback"
    OnCustomFilterExpressionDisplayText="GridViewArendaObjects_CustomFilterExpressionDisplayText"
    OnCustomSummaryCalculate="GridViewArendaObjects_CustomSummaryCalculate"
    OnProcessColumnAutoFilter = "GridViewArendaObjects_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewArendaObjects_CustomColumnSort" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="arenda_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="True" Caption="Картка">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowArendaCard(" + Eval("arenda_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="1" Visible="False" Caption="ID Балансоутримувача">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="False" Caption="Балансоутримувач - Повна Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="False" Caption="Балансоутримувач - Коротка Назва" Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_zkpo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="False" Caption="Балансоутримувач - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="False" Caption="Балансоутримувач - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="False" Caption="Балансоутримувач - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="7" Visible="False" Caption="ID Орендаря">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="False" Caption="Орендар - Повна Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="True" Caption="Орендар - Коротка Назва" Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_zkpo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="False" Caption="Орендар - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Орендар - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="False" Caption="Орендар - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="13" Visible="False" Caption="ID Орендодавця">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="False" Caption="Орендодавець - Повна Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="True" Caption="Орендодавець - Коротка Назва" Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_zkpo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="16" Visible="False" Caption="Орендодавець - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="Орендодавець - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Орендодавець - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="False" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="True" Caption="Назва Вулиці" Width="140px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="True" Caption="Номер Будинку">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
            </DataItemTemplate>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
<%--         <dx:GridViewDataTextColumn FieldName="purpose" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="True" Caption="Призначення за класифікацією"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataTextColumn FieldName="object_note" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="True" Caption="Розташування приміщення (поверх)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="True" Caption="Група Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="pryzn4doc" ReadOnly="True" ShowInCustomizationForm="True" 
            VisibleIndex="25" Visible="False" Caption="Призначення за документом"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_name" ReadOnly="True" ShowInCustomizationForm="True" 
            VisibleIndex="26" Visible="False" Caption="Використання згідно з договором: примітки"></dx:GridViewDataTextColumn>
<%--         <dx:GridViewDataTextColumn FieldName="is_privat" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="False" Caption="Приватизовано"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="27" Visible="False" Caption="Тип Договору Оренди"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataDateColumn FieldName="agreement_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="28" Visible="False" Caption="Дата Договору Оренди"></dx:GridViewDataDateColumn>
<%--         <dx:GridViewDataTextColumn FieldName="agreement_date_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="29" Visible="False" Caption="Дата Договору Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_date_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="30" Visible="False" Caption="Дата Договору Оренди - Квартал"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataTextColumn FieldName="agreement_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="True" Caption="Номер Договору Оренди"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_narah" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="False" Caption="Ставка за використання, %"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="invent_no_balans" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="True" Caption="Інвентарний номер об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="invent_no_agr_obj" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="34" Visible="True" Caption="Інвентарний номер об'єкту за договором"></dx:GridViewDataTextColumn>
        

<%--        
        <dx:GridViewDataTextColumn FieldName="floor_number" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="33" Visible="False" Caption="Поверх"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_payed" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="34" Visible="False" Caption="Надходження орендної плати за звітний період, всього, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_debt" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="35" Visible="False" Caption="Заборгованість по орендній платі, грн. (без ПДВ)"></dx:GridViewDataTextColumn>    --%>

        <dx:GridViewDataTextColumn FieldName="cost_agreement" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="36" Visible="True" Caption="Плата за використання (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert_total_agr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="37" Visible="True" Caption="Оціночна вартість приміщень за договором, грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="38" Visible="True" Caption="Ринкова вартість приміщення, грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="date_expert" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="39" Visible="True" Caption="Дата, на яку проведена оцінка об'єкту"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="debt_timespan" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="40" Visible="False" Caption="Час Заборгованості"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="pidstava_display" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="40" Visible="False" Caption="Підстава"></dx:GridViewDataTextColumn>     --%>
        <dx:GridViewDataDateColumn FieldName="rent_start_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="41" Visible="True" Caption="Початок Оренди"></dx:GridViewDataDateColumn>
        <%--<dx:GridViewDataTextColumn FieldName="rent_start_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="42" Visible="False" Caption="Початок Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_start_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="43" Visible="False" Caption="Початок Оренди - Квартал"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataDateColumn FieldName="rent_finish_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="44" Visible="True" Caption="Закінчення Оренди"></dx:GridViewDataDateColumn>
        <%--<dx:GridViewDataTextColumn FieldName="rent_finish_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="45" Visible="False" Caption="Закінчення Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_finish_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="46" Visible="False" Caption="Закінчення Оренди - Квартал"></dx:GridViewDataTextColumn>   --%>
        <dx:GridViewDataDateColumn FieldName="rent_actual_finish_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="47" Visible="False" Caption="Фактичне Закінчення Оренди"></dx:GridViewDataDateColumn>
        <%--<dx:GridViewDataTextColumn FieldName="actual_finish_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="48" Visible="False" Caption="Фактичне Закінчення Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="actual_finish_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="49" Visible="False" Caption="Фактичне Закінчення Оренди - Квартал"></dx:GridViewDataTextColumn>   
        <dx:GridViewDataTextColumn FieldName="rent_rate_percent" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="50" Visible="False" Caption="Ставка за договором, %"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="rent_rate_uah" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="51" Visible="False" Caption="Орендна плата за 1 кв.м, грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_square" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="52" Visible="False" Caption="Площа приміщення, кв.м"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="num_akt" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="53" Visible="False" Caption="Номер Акту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="date_akt" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="54" Visible="False" Caption="Дата Акту"></dx:GridViewDataDateColumn>          --%>
        <dx:GridViewDataTextColumn FieldName="is_subarenda" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="55" Visible="False" Caption="Суборенда"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_type" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="56" Visible="True" Caption="Вид оплати"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_active_s" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="57" Visible="True" Caption="Стан договору"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="58" Visible="False" Caption="Балансоутримувач - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="59" Visible="False" Caption="Орендар - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="60" Visible="False" Caption="Орендодавець - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="balans_sqr_total" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="61" Visible="False" Caption="Балансоутримувач - Загальна Площа На Балансі (кв.м.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="balans_num_rent_agr" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="62" Visible="False" Caption="Балансоутримувач - Кількість Договорів Оренди"></dx:GridViewDataTextColumn>   
        <dx:GridViewDataTextColumn FieldName="balans_sqr_in_rent" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="63" Visible="False" Caption="Балансоутримувач - Загальна Площа Надана В Оренду (кв.м.)"></dx:GridViewDataTextColumn>   --%>
        <dx:GridViewDataTextColumn FieldName="payment_type_obj_nam" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="64" Visible="True" Caption="Використання згідно з договором: цільове"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="factich_vikorist_type" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="65" Visible="True" Caption="Використання фактичне"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sphera_dialnosti" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="66" Visible="True" Caption="Сфера діяльності"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="balans_org_ownership" ReadOnly="True" ShowInCustomizationForm="True" 
            VisibleIndex="65" Visible="False" Caption="Балансоутримувач - Форма Власності"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataTextColumn FieldName="org_balans_form_ownership" ReadOnly="True" ShowInCustomizationForm="True" 
            VisibleIndex="67" Visible="False" Caption="Балансоутримувач - Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_gosp" ReadOnly="True" ShowInCustomizationForm="True" 
            VisibleIndex="67" Visible="False" Caption="Балансоутримувач - Форма фінансування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_num_int" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="68" Visible="False" Caption="Номер Договору Оренди (число)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_in_privat" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="69" Visible="False" Caption="Будинок в Програмі Приватизації"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="sqr_free_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="68" Visible="False" Caption="Вільні Приміщення: Загальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_korysna" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="69" Visible="False" Caption="Вільні Приміщення: Корисна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_mzk" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="70" Visible="False" Caption="Вільні Приміщення: МЗК (кв.м.)"></dx:GridViewDataTextColumn>    
        <dx:GridViewDataTextColumn FieldName="free_sqr_floors" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="71" Visible="False" Caption="Місце Розташування Вільного Приміщення (поверх)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_purpose" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="72" Visible="False" Caption="Можливе Використання Вільного Приміщення"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataTextColumn FieldName="org_renter_form_of_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="73" Visible="False" Caption="Орендар - Форма Власності"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="org_balans_org_form" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="74" Visible="False" Caption="Балансоутримувач - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_org_form" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="75" Visible="False" Caption="Орендодавець - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_org_form" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="76" Visible="False" Caption="Орендар - Організаційно-правова Форма"></dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="modify_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="77" Visible="False" Caption="Дата Актуальності"></dx:GridViewDataDateColumn>
         <dx:GridViewDataTextColumn FieldName="stan_prym" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="78" Visible="False" Caption="Поточний стан використання приміщення"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="arenda_id" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="101" Visible="False" Caption="ID об'єкту"></dx:GridViewDataTextColumn>
   </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="cost_agreement" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="balans_sqr_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="balans_sqr_in_rent" SummaryType="Custom" DisplayFormat="{0}" />
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
    <SettingsCookies CookiesID="GUKV.ArendaObjects" Version="A2_34" Enabled="true" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewArendaObjectsInit" EndCallback="GridViewArendaObjectsEndCallback" />
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

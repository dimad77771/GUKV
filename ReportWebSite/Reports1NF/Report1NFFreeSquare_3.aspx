<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFFreeSquare.aspx.cs" Inherits="Reports1NF_Report1NFFreeSquare"
    MasterPageFile="~/NoHeader.master" Title="Перелік вільних приміщень" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {

        FreeSquareGridView.SetHeight(window.innerHeight - 180);
    }

    function GridViewFreeSquareInit(s, e) {

        FreeSquareGridView.PerformCallback("init:");
    }

    function GridViewFreeSquareEndCallback(s, e) {

        AdjustGridSizes();
    }


    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
 row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr) as npp     
,fs.id
,org.short_name as org_name
,org.zkpo_code

,b.district
,b.street_full_name as street_name
,b.addr_nomer

,b.condition 
,b.object_type 
,b.object_kind
,b.sqr_total
,b.sqr_for_rent

,fs.total_free_sqr 
--,null as free_sql_usefull
--,null as mzk
,rfs.sqr_free_korysna as free_sql_usefull
,rfs.sqr_free_mzk as mzk

,fs.floor
,fs.water
,fs.heating
,fs.power
,fs.gas
,fs.possible_using
,fs.modify_date
, null as note
,rep.form_of_ownership
,rep.old_organ

FROM view_reports1nf rep
join reports1nf_balans bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_balans_free_square fs on fs.balans_id = bal.id
--left join (select * from dbo.reports1nf_balans_free_square where id = (select top 1 id from dbo.reports1nf_balans_free_square where balans_id = bal.id)) fs on fs.balans_id = bal.id
join reports1nf_org_info org on org.id = bal.organization_id

OUTER APPLY (SELECT TOP 1 * FROM rent_free_square rfs
		WHERE rfs.building_id = bal.building_id AND
		      rfs.organization_id = bal.organization_id order by rfs.rent_period_id DESC) rfs

LEFT JOIN (
			select obp.org_id
			, occ.name
			, occ.id
			, per.name as period 
			from org_by_period obp
			join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
			join dict_rent_occupation occ on occ.id = obp.org_occupation_id
				) DDD ON DDD.org_id = rep.organization_id

        WHERE (@p_rda_district_id = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @p_rda_district_id))

    order by org_name, street_name, addr_nomer, total_free_sqr   "
    OnSelecting="SqlDataSourceFreeSquare_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="period_year" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFAccounts.aspx" Text="Облікові Записи"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFNotifications.aspx" Text="Налаштування Повідомлень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>        
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFFreeSquare.aspx" Text="Перелік вільних приміщень"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік вільних приміщень" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_FreeSquare_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_FreeSquare_SaveAs" 
                PopupElementID="ASPxButton_FreeSquare_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_FreeSquare_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_FreeSquare_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_FreeSquare_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_FreeSquare_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewFreeSquareExporter" runat="server" 
    FileName="ВільніПлощі" GridViewID="FreeSquareGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

    <%--
<dx:ASPxGridView
    ID="PrimaryGridView"
    ClientInstanceName="PrimaryGridView"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceReports"
    KeyFieldName="report_id"
    OnCustomCallback="GridViewReports1NF_CustomCallback"
    OnCustomFilterExpressionDisplayText="GridViewReports1NF_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter="GridViewReports1NF_ProcessColumnAutoFilter" >
--%>
   <dx:ASPxGridView ID="FreeSquareGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceFreeSquare" KeyFieldName="id" Width="100%" 
        ClientInstanceName="FreeSquareGridView" 
        OnCustomCallback="GridViewFreeSquare_CustomCallback"
        OnCustomFilterExpressionDisplayText="GridViewFreeSquare_CustomFilterExpressionDisplayText"
        OnProcessColumnAutoFilter="GridViewFreeSquare_ProcessColumnAutoFilter" >

    <Columns>

<%--
        <dx:GridViewDataTextColumn FieldName="npp" Caption="Order Number" VisibleIndex="0"  Width="300px"/>
--%>


        <dx:GridViewDataTextColumn FieldName="org_name" Caption="Назва Організації" VisibleIndex="0"  Width="300px"/>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" Caption="Код ЄДРПОУ" VisibleIndex="1"/>
        <dx:GridViewDataTextColumn FieldName="district" Caption="Район" VisibleIndex="2" Width="120px"/>
        <dx:GridViewDataTextColumn FieldName="street_name" Caption="Назва Вулиці" VisibleIndex="3" Width="150px"/>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" Caption="Номер Будинку" VisibleIndex="4" Width="80px"/>


        <dx:GridViewBandColumn Caption="Вільні приміщення"  HeaderStyle-HorizontalAlign="Center" > 
           <Columns>
                <dx:GridViewDataTextColumn FieldName="floor" Caption="Місце розташування вільного приміщення (поверх)" VisibleIndex="5" Width="80px"/>
                <dx:GridViewDataTextColumn FieldName="sqr_for_rent" Caption="Загальна площа приміщень, що перебувають в орендному користуванні, кв.м." VisibleIndex="6" Width="80px"/>
                <dx:GridViewDataTextColumn FieldName="total_free_sqr" Caption="Загальна площа вільних приміщень, кв.м."  VisibleIndex="7" Width="80px"/>
                <dx:GridViewDataTextColumn FieldName="free_sql_usefull" Caption="Корисна площа вільних приміщень, кв.м." VisibleIndex="8" Width="80px"/>
<%--                <dx:GridViewDataTextColumn FieldName="mzk" Caption="МЗК, кв.м." VisibleIndex="9" Width="80px"/>     --%>
            </Columns>
         </dx:GridViewBandColumn>
 
        <dx:GridViewBandColumn Caption="Наявність комунікацій"  HeaderStyle-HorizontalAlign="Center" > 
                <Columns>
                    <dx:GridViewDataCheckColumn FieldName="water" Caption="Водопостачання" VisibleIndex="9" Width="50px"/>
                    <dx:GridViewDataCheckColumn FieldName="heating" Caption="Теплопостачання" VisibleIndex="10" Width="50px"/>
                    <dx:GridViewDataCheckColumn FieldName="power" Caption="Електропостачання" VisibleIndex="11" Width="50px"/>
                    <dx:GridViewDataCheckColumn FieldName="gas" Caption="Газопостачання" VisibleIndex="12" Width="50px"/>
                </Columns>
            </dx:GridViewBandColumn>

        <dx:GridViewBandColumn Caption="Додаткові"  HeaderStyle-HorizontalAlign="Center"> 
            <Columns>
                <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="13" Width="80px"/>
                <dx:GridViewDataTextColumn FieldName="condition" Caption="Технічний стан" VisibleIndex="14" Width="80px"/>
                <dx:GridViewDataTextColumn FieldName="possible_using" Caption="Можливе використання вільного приміщення" VisibleIndex="15" Width="150px"/>
            </Columns>
         </dx:GridViewBandColumn>
        <dx:GridViewDataTextColumn FieldName="form_of_ownership" Caption="Форма Власності" VisibleIndex="16" Width="150px"/>
        <dx:GridViewDataTextColumn FieldName="old_organ" Caption="Орган госп. упр." VisibleIndex="17" Width="150px"/>

    </Columns>

<%--        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />    --%>
<%--        <dx:ASPxSummaryItem FieldName="sqr_for_rent" SummaryType="Sum" DisplayFormat="{0}" />  --%>
<%--        <dx:ASPxSummaryItem FieldName="mzk" SummaryType="Sum" DisplayFormat="{0}" />      --%>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="total_free_sqr" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="free_sql_usefull" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
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
    <SettingsCookies CookiesID="GUKV.Reports1NF.FreeSquare" Version="A2_3" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewFreeSquareInit" EndCallback="GridViewFreeSquareEndCallback" />
</dx:ASPxGridView>


</asp:Content>

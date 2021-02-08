<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryObjectList.aspx.cs" Inherits="Reports1NF_InventoryObjectList"
    MasterPageFile="~/NoMenu.master" Title="Об'єкти на Балансі" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

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

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSize(); };

    function AdjustGridSize() {

        PrimaryGridView.SetHeight(window.innerHeight - 180);
    }

    function PrimaryGridViewEndCallback(s, e) {

        AdjustGridSize();

        CPStatus.PerformCallback('');
    }

    function ShowReportBalansObjCard(reportId, balansId) {

        var cardUrl = "../Reports1NF/OrgBalansObject.aspx?rid=" + reportId + "&bid=" + balansId;

        window.location = cardUrl;
    }

    function ShowSendingLog() {
        var diagnosticWindow = window.open("SendingBalanceObjectsDiagnostic.aspx?rid=<%= ReportID  %>", "", "width=600,height=400");
        diagnosticWindow.focus();
    }
    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT CASE WHEN COUNT(*) > 0 THEN 0 ELSE 1 END AS 'report_balans_status'
        FROM reports1nf_balans bal INNER JOIN reports1nf rep ON rep.id = bal.report_id
        WHERE bal.report_id = @rep_id AND ((bal.submit_date IS NULL OR bal.modify_date > bal.submit_date))">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceReportBalans" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
		(SELECT Q.name FROM dict_org_ownership Q where Q.id = org.form_ownership_id) as form_ownership_name
		,org.short_name as org_name
		,org.zkpo_code
		,(SELECT Q.name FROM dict_1nf_object_kind Q where Q.id = bal.object_kind_id) as object_kind_name
		,(SELECT Q.name FROM dict_1nf_tech_state Q where Q.id = bal.tech_condition_id) as tech_condition_name
		,bld.sqr_non_habit
		,bal.purpose_str
		,(select Q.full_name from organizations Q where Q.id = arenda.org_renter_id and (Q.is_deleted is null or Q.is_deleted = 0)) as renter_name
		,arenda.rent_start_date
		,arenda.rent_finish_date
		,arenda.agreement_num
		,(select sum(rent_square) from reports1nf_arenda_notes Q WHERE (Q.is_deleted IS NULL OR Q.is_deleted = 0) AND Q.report_id = bal.report_id AND Q.arenda_id = arenda.id) as rent_square
	
        ,bal.id AS 'balans_id'
        ,dict_districts2.name AS 'district'
        ,bld.addr_street_name
        ,(COALESCE(LTRIM(RTRIM(bld.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(bld.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(bld.addr_nomer3)), '')) AS 'addr_nomer'
        ,bal.sqr_total
        ,bal.sqr_vlas_potreb
        ,COALESCE(bal.purpose_str, dict_balans_purpose.name) AS 'purpose', bal.is_deleted, bal.modify_date, bal.submit_date, dict_own.name AS ownership_type
        ,CASE WHEN (((bal.is_valid = 0) OR (bal.is_valid IS NULL)) OR ((bal.submit_date IS NULL OR bal.modify_date > bal.submit_date))) THEN N'НІ' ELSE N'ТАК' END AS 'is_submitted'
        ,bal.modified_by
        ,bfs.total_free_sqr
        ,ag.num_rent_agr
        ,ag.total_rent_sqr
        ,inv_num = bal.reestr_no


        FROM reports1nf_balans bal
        INNER JOIN reports1nf rep ON rep.id = bal.report_id
        LEFT OUTER JOIN reports1nf_buildings bld ON bld.unique_id = bal.building_1nf_unique_id
        LEFT OUTER JOIN dict_balans_purpose ON dict_balans_purpose.id = bal.purpose_id
        LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = bld.addr_distr_new_id
        LEFT OUTER JOIN dict_org_ownership dict_own ON bal.form_ownership_id = dict_own.id
		LEFT OUTER JOIN reports1nf_org_info org on org.id = bal.organization_id
		outer apply (select ar.* from reports1nf_arenda ar left join reports1nf_arenda_notes an on ar.id = an.arenda_id and isnull(an.is_deleted,0) = 0 where bal.report_id = ar.report_id and bal.organization_id = ar.org_balans_id and bal.building_id = ar.building_id and isnull(ar.is_deleted,0)=0 and ar.agreement_state = 1) arenda 

        outer apply (select sum(case when fs.is_included = 1 then fs.total_free_sqr else 0 end) as total_free_sqr from reports1nf_balans_free_square fs where fs.balans_id = bal.id and fs.report_id = bal.report_id /*and fs.is_included = 1*/) bfs 
        outer apply (select COUNT(distinct ar.id) as num_rent_agr, sum(an.rent_square) as total_rent_sqr from reports1nf_arenda ar left join reports1nf_arenda_notes an on ar.id = an.arenda_id and isnull(an.is_deleted,0) = 0 where bal.report_id = ar.report_id and bal.organization_id = ar.org_balans_id and bal.building_id = ar.building_id and isnull(ar.is_deleted,0)=0 and ar.agreement_state = 1) ag 
        --left join reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = bal.report_id
        --LEFT JOIN reports1nf_arenda ar ON bal.report_id = ar.report_id and bal.organization_id = ar.org_balans_id and bal.building_id = ar.building_id and isnull(ar.is_deleted,0)=0 and ar.agreement_state = 1
        --LEFT JOIN reports1nf_arenda_notes an on ar.id = an.arenda_id and isnull(an.is_deleted,0) = 0 
        WHERE (bal.report_id = @rep_id or @rep_id = -1) and ISNULL(bal.is_deleted,0) = 0
		order by 2, addr_nomer 
	" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Cabinet.aspx" Text="Стан"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgInfo.aspx" Text="Загальна Інформація"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansList.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" Text="Відчужені Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgArendaList.aspx" Text="Договори використання приміщень "></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgRentedList.aspx" Text="Договори Орендування"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingRequestsList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Help.aspx" Text="?"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<p class="SpacingPara"/>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Перелік об'єктів, що знаходяться на балансі організації" CssClass="pagetitle"/>
</p>

<table border="0" cellspacing="0" cellpadding="2">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSendAll" runat="server" Text="Надіслати всі зміни в ДКВ" AutoPostBack="false" Visible="false"></dx:ASPxButton>

            <dx:ASPxTimer ID="ProgressTimer" runat="server" ClientInstanceName="ProgressTimer" Enabled="False" Interval="3000" Visible="false">
                <ClientSideEvents Tick="function(s, e) { CPProgress.PerformCallback('timer:'); }" />
            </dx:ASPxTimer>

            <dx:ASPxPopupControl ID="PopupSendAll" runat="server" ClientInstanceName="PopupSendAll"
                HeaderText="Надсилання до ДКВ усіх змінених об'єктів на балансі" PopupElementID="ButtonSendAll">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">

                        <dx:ASPxCallbackPanel ID="CPProgress" ClientInstanceName="CPProgress" runat="server" OnCallback="CPProgress_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent1" runat="server">

                                    <dx:ASPxLabel ID="LabelProgress" ClientInstanceName="LabelProgress" runat="server" Width="600px" />

                                    <dx:ASPxProgressBar ID="ProgressSendAll" ClientInstanceName="ProgressSendAll" runat="server" Width="600px" Height="24px" Minimum="0" Maximum="100" />

                                </dx:panelcontent>
                            </PanelCollection>

                            <ClientSideEvents EndCallback="function (s,e) {
                                        var id = '' + CPProgress.cpWorkItemId;
                                        if (id.length > 0)
                                        {
                                            ProgressTimer.SetEnabled(true);
                                        }
                                        else
                                        {
                                            ProgressTimer.SetEnabled(false);
                                            alert('Обробку завершено.');
                                            PopupSendAll.Hide();
                                            PrimaryGridView.PerformCallback('init:');
                                        }
                                    }" />
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents PopUp="function (s,e) { CPProgress.PerformCallback('init:'); ShowSendingLog();  }" />
            </dx:ASPxPopupControl>

        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_BalansObjects_SaveAs" 
                PopupElementID="ASPxButton_BalansObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton2" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_BalansObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_BalansObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxButton ID="ASPxButton_BalansObjects_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px" Visible="false">
            </dx:ASPxButton>
        </td>        
        <td>
            <dx:ASPxCallbackPanel ID="CPStatus" ClientInstanceName="CPStatus" runat="server" OnCallback="CPStatus_Callback" Visible="false">
                <PanelCollection>
                    <dx:panelcontent ID="Panelcontent6" runat="server">

                        <asp:FormView runat="server" BorderStyle="None" ID="BalansStatusForm" DataSourceID="SqlDataSourceBalansStatus" EnableViewState="False">
                            <ItemTemplate>

                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Необхідно надіслати внесені зміни до ДКВ" ClientVisible='<%# 0.Equals(Eval("report_balans_status")) %>' ForeColor="Red" />
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Усі зміни надіслано до ДКВ" ClientVisible='<%# 1.Equals(Eval("report_balans_status")) %>' />

                            </ItemTemplate>
                        </asp:FormView>

                    </dx:panelcontent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </td>
        <td width="100%" align ="right">
            <dx:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="160px">
                <ClientSideEvents Click="function (s,e) { PopupFieldChooser.Show(); }" />
            </dx:ASPxButton>
        </td>    
    </tr>
</table>


<dx:ASPxGridViewExporter ID="ASPxGridViewExporterBalansObjects" runat="server" 
    FileName="Balance" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView ID="PrimaryGridView" ClientInstanceName="PrimaryGridView" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceReportBalans" KeyFieldName="balans_id" Width="100%"
    OnCustomCallback="PrimaryGridView_CustomCallback"
    OnHtmlDataCellPrepared="PrimaryGridView_HtmlDataCellPrepared"
    OnCustomColumnSort="PrimaryGridView_CustomColumnSort" >

    <Columns>
<%--        <dx:GridViewDataTextColumn FieldName="balans_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="True" Caption="Картка Об'єкту" Width="50px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowReportBalansObjCard(" + Request.QueryString["rid"] + "," + Eval("balans_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False" AllowAutoFilter="False" AllowHeaderFilter="False" />
        </dx:GridViewDataTextColumn>--%>

		<dx:GridViewDataTextColumn FieldName="form_ownership_name" VisibleIndex="0" Caption="Сфера управління об'єкта (місто/район)"></dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="org_name" Caption="Балансоутримувач" VisibleIndex="0"  Width="300px" ReadOnly="true" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("org_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" Caption="Код ЄДРПОУ" VisibleIndex="0" Visible="false" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zkpo_code") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="1" Caption="Район" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" VisibleIndex="2" Caption="Вулиця" Width="150px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="3" Caption="Номер"><Settings SortMode="Custom" /></dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="purpose_str" VisibleIndex="3" Width="250" Caption="Назва Об'єкту"></dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="object_kind_name" VisibleIndex="3" Width="250" Caption="Короткий опис об'єкта (адміністративне, виробниче, навчальний заклад, заклад охорони здоров'я тощо)"></dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="tech_condition_name" VisibleIndex="3" Width="150" Caption="Загальний технічний стан (задовільний, потребує проведення ремонтних робіт, аварійний)"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="sqr_non_habit" VisibleIndex="3" Caption="Площа, що перебуває в комунальній власності територіальної громади міста Києва, кв. м"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="3" Caption="Загальна площа об'єкта, кв. м"></dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="sqr_vlas_potreb" VisibleIndex="3" Caption="Площа, що використвує-ться для власних потреб, кв. м" ShowInCustomizationForm="True"></dx:GridViewDataTextColumn>		
		<dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="3" Caption="Площа в орендному користуванні, кв. м"></dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="total_free_sqr" VisibleIndex="3" Caption="Площа, що тимчасово не використовується та може бути передана в орендне користвання, кв. м"></dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="renter_name" VisibleIndex="3" Caption="Назва суб'єкта господарювання, який є користувачем (орендарем) об'єкта" Width="270px"></dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="3" Caption="Початок оренди"></dx:GridViewDataDateColumn>
		<dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="3" Caption="Закінчення оренди"></dx:GridViewDataDateColumn>
		<dx:GridViewDataDateColumn FieldName="agreement_num" VisibleIndex="3" Caption="Номер Договору Оренди"></dx:GridViewDataDateColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="balans_id" SummaryType="Count" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="total_free_sqr" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_rent_agr" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="total_rent_sqr" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_vlas_potreb" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowFilterBar="Visible"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="20" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.Reports1NF.BalansList" Enabled="true" Version="A5" />

    <ClientSideEvents
        Init="function (s,e) { PrimaryGridView.PerformCallback('init:'); }"
        EndCallback="PrimaryGridViewEndCallback" />
</dx:ASPxGridView>

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

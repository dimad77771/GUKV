﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFFreeRejections.aspx.cs" Inherits="Reports1NF_Report1NFFreeRejections"
    MasterPageFile="~/FreeShowPublic.master" Title="Перелік вільних приміщень" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
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

	function ShowPhoto(s, e) {
		if (e.buttonID == 'btnPhoto') {
			//console.log("$.cookie", $.cookie);
			//console.log("GetRowKey", s.GetRowKey(e.visibleIndex));
			$.cookie('RecordID', s.GetRowKey(e.visibleIndex));
			ASPxFileManagerPhotoFiles.Refresh();
			PopupObjectPhotos.Show();
		} else if (e.buttonID == 'btnMapShow') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnMapShowGetRowValues);
		} else if (e.buttonID == 'btnPdfBuild') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnGridPdfBuildGetRowValues);
        }
	}

	function OnGridPdfBuildGetRowValues(values) {
		//console.log(values);
		var id = values;
		window.open(
			'BalansFreeSquarePhotosPdf.aspx?id=' + id,
			'_blank',
		);
	}

	function OnMapShowGetRowValues(values) {
		var id = values;
		window.open(
			'Report1NFFreeMap.aspx?fs_id=' + id,
			'_blank',
		);
	}


    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 

forg.zkpo_code as forg_zkpo,  
forg.full_name as forg_name,
freecycle_step.step_c1 as rejection_date,

--freecycle_rejection_dict.rejection_cod + ' - ' + freecycle_rejection_dict.rejection_name as rejection_fullname,
freecycle_rejection_dict.rejection_name as rejection_fullname,


	fs.komis_protocol,
	fs.geodata_map_url,
 row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr) as npp     
,fs.id
,org.short_name as org_name
,org.zkpo_code
,org.director_title as vidpov_osoba

,b.district
,b.street_full_name as street_name
,(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')) as addr_nomer

,b.condition 
,b.object_type 
,b.object_kind
,b.sqr_total
,b.sqr_for_rent

,fs.total_free_sqr 
--,null as free_sql_usefull
--,null as mzk
--,rfs.sqr_free_korysna as free_sql_usefull
--,rfs.sqr_free_mzk as mzk
,fs.free_sqr_korysna as free_sql_usefull


,fs.floor
,fs.water
,fs.heating
,fs.power
,fs.gas

,(select qq.name2 from view_dict_rental_rate qq where qq.id = fs.using_possible_id) as possible_using
--,fs.possible_using

,fs.modify_date
,fs.note
, solution = fs.is_solution
, fs.initiator
, zg2.name as zgoda_control
, zg.name as zgoda_renter

,st.kind
,rep.form_of_ownership
,rep.old_organ

,b.object_kind as vydbudynku
,history = case when isnull(b.history, 'НІ') = 'НІ' then '' else 'ТАК' end 
, isnull(ddd.name, 'Невизначені') as sf_upr
, @baseurl + '/Reports1NF/BalansFreeSquarePhotosPdf.aspx?id=' + cast(fs.id as varchar(100)) as pdfurl
, case when exists (select 1 from reports1nf_balans_free_square_photos qq where qq.free_square_id = fs.id) then 1 else 0 end as isexistsphoto

FROM view_reports1nf rep
join reports1nf_balans bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = rep.report_id
--left join (select * from dbo.reports1nf_balans_free_square where id = (select top 1 id from dbo.reports1nf_balans_free_square where balans_id = bal.id)) fs on fs.balans_id = bal.id
join reports1nf_org_info org on org.id = bal.organization_id
left join [dbo].[dict_streets] st on b.addr_street_id = st.id
left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id

--OUTER APPLY (SELECT TOP 1 * FROM rent_free_square rfs
--		WHERE rfs.building_id = bal.building_id AND
--		      rfs.organization_id = bal.organization_id order by rfs.rent_period_id DESC) rfs

LEFT JOIN (
			select obp.org_id
			, occ.name
			, occ.id
			, per.name as period 
			from org_by_period obp
			join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
			join dict_rent_occupation occ on occ.id = obp.org_occupation_id
				) DDD ON DDD.org_id = rep.organization_id

JOIN freecycle ON freecycle.free_square_id = fs.id AND freecycle.is_deleted = 0
JOIN freecycle_step ON freecycle_step.freecycle_id = freecycle.freecycle_id AND freecycle_step.is_deleted = 0 AND freecycle_step.step_c4 is not null
JOIN freecycle_orendar ON freecycle_orendar.freecycle_orendar_id = freecycle_step.freecycle_orendar_id AND freecycle_orendar.is_deleted = 0
JOIN organizations forg ON forg.id = freecycle_orendar.org_orendar_id 
JOIN freecycle_rejection_dict ON freecycle_rejection_dict.rejection_id = freecycle_step.step_c4

        WHERE (@p_rda_district_id = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @p_rda_district_id))
				--and (komis_protocol <> '' and komis_protocol not like '0%' and geodata_map_points <> '' and is_included = 1)
				and (@fs_id = -1 OR fs.id = @fs_id)
				and (@mode50 = 0 OR fs.freecycle_step_dict_id = 31)


    order by org_name, street_name, addr_nomer, total_free_sqr   "
    OnSelecting="SqlDataSourceFreeSquare_Selecting"

UpdateCommand="UPDATE [reports1nf_balans_free_square]
SET
    [komis_protocol] = @komis_protocol,
	[geodata_map_url] = @geodata_map_url
WHERE id = @id" 
	onupdating="SqlDataSourceFreeSquare_Updating"
	>
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="period_year" />
		<asp:Parameter DbType="String" DefaultValue="" Name="baseurl" />
		<asp:Parameter DbType="Int32" DefaultValue="-1" Name="fs_id" />
		<asp:Parameter DbType="Int32" DefaultValue="0" Name="mode50" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік відмов потенційним орендарям" CssClass="reporttitle"></asp:Label>
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
	   <ClientSideEvents CustomButtonClick="ShowPhoto" />
    <Columns>

        <dx:GridViewCommandColumn VisibleIndex="0" Width="50px" ButtonType="Image" CellStyle-Wrap="False" >
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="btnPhoto" Text="Фотографії об'єкту"> 
					<Image Url="~/Styles/PhotoIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnMapShow" Text="Показати на мапі"> 
					<Image Url="~/Styles/MapShowIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataTextColumn FieldName="forg_zkpo" Caption="ЕДРПОУ орендаря" VisibleIndex="0"  Width="80px" ReadOnly="true">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="forg_name" Caption="Назва орендаря" VisibleIndex="0"  Width="250px" ReadOnly="true">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rejection_fullname" Caption="Причина відмови" VisibleIndex="0"  Width="250px" ReadOnly="true">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="rejection_date" Caption="Дата відмови" VisibleIndex="0" Width="75px">
			<HeaderStyle Wrap="True" />
        </dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn FieldName="org_name" Caption="Балансоутримувач" VisibleIndex="0"  Width="250px" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("org_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" Caption="Код ЄДРПОУ" VisibleIndex="1" Width="100px" ReadOnly="true">
			<HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zkpo_code") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
		<dx:GridViewDataCheckColumn FieldName="isexistsphoto" Caption="Наяв- ність фото" VisibleIndex="1" Width="50px" ReadOnly="true"/>
        <dx:GridViewDataTextColumn FieldName="district" Caption="Район" VisibleIndex="2" Width="120px" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("district") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_name" Caption="Назва Вулиці" VisibleIndex="3" Width="150px" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("street_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" Caption="Номер Будинку" VisibleIndex="4" Width="80px" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("addr_nomer") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="id" Caption="Реєстра-ційний №" VisibleIndex="4" Width ="60px"  >
			<CellStyle HorizontalAlign="Center" />
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("id") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <%--<dx:GridViewDataTextColumn FieldName="geodata_map_url" Caption="URL на мапі" VisibleIndex="4" Width="100px">
        </dx:GridViewDataTextColumn>--%>


        <dx:GridViewBandColumn Caption="Вільні приміщення"  HeaderStyle-HorizontalAlign="Center" > 
           <Columns>
                <dx:GridViewDataTextColumn FieldName="floor" Caption="Місце розташування вільного приміщення (поверх)" VisibleIndex="5" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("floor") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
<%--                <dx:GridViewDataTextColumn FieldName="sqr_for_rent" Caption="Загальна площа приміщень, що перебувають в орендному користуванні, кв.м." VisibleIndex="6" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("sqr_for_rent") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn FieldName="total_free_sqr" Caption="Загальна площа об’єкта"  VisibleIndex="7" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("total_free_sqr") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="free_sql_usefull" Caption="Корисна площа об’єкта" VisibleIndex="8" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("free_sql_usefull") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
         </dx:GridViewBandColumn>
 
        <dx:GridViewBandColumn Caption="Наявність комунікацій"  HeaderStyle-HorizontalAlign="Center" > 
                <Columns>
                    <dx:GridViewDataCheckColumn FieldName="water" Caption="Водопостачання" VisibleIndex="9" Width="50px" ReadOnly="true">
						<%--<EditItemTemplate>
							<dx:ASPxLabel runat="server" Text='<%# Eval("water") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
						</EditItemTemplate>--%>
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataCheckColumn FieldName="heating" Caption="Теплопостачання" VisibleIndex="10" Width="50px" ReadOnly="true">
						<%--<EditItemTemplate>
							<dx:ASPxLabel runat="server" Text='<%# Eval("heating") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
						</EditItemTemplate>--%>
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataCheckColumn FieldName="power" Caption="Електропостачання" VisibleIndex="11" Width="50px" ReadOnly="true">
						<%--<EditItemTemplate>
							<dx:ASPxLabel runat="server" Text='<%# Eval("power") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
						</EditItemTemplate>--%>
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataCheckColumn FieldName="gas" Caption="Газопостачання" VisibleIndex="12" Width="50px" ReadOnly="true">
						<%--<EditItemTemplate>
							<dx:ASPxLabel runat="server" Text='<%# Eval("gas") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
						</EditItemTemplate>--%>
                    </dx:GridViewDataCheckColumn>
                </Columns>
            </dx:GridViewBandColumn>

        <dx:GridViewBandColumn Caption="Додаткові"  HeaderStyle-HorizontalAlign="Center"> 
            <Columns>
<%--                <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="13" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("modify_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataDateColumn>--%>
                <dx:GridViewDataTextColumn FieldName="condition" Caption="Технічний стан" VisibleIndex="14" Width="80px">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("condition") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="possible_using" Caption="Можливе використання вільного приміщення" VisibleIndex="15" Width="250px">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("possible_using") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
         </dx:GridViewBandColumn>
        <%--<dx:GridViewDataTextColumn FieldName="form_of_ownership" Caption="Форма Власності" VisibleIndex="16" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("form_of_ownership") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="old_organ" Caption="Орган госп. упр." VisibleIndex="17" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("old_organ") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataCheckColumn FieldName="solution" Caption="Наявність рішень про проведення інвестиційного конкурсу" VisibleIndex="18" Width="50px" ReadOnly="true">--%>
<%--			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("solution") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>--%>
        <%--</dx:GridViewDataCheckColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="initiator" Caption="Ініціатор оренди" VisibleIndex="19" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("initiator") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="zgoda_control" Caption="Погодження органу управління" VisibleIndex="20" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_control") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="zgoda_renter" Caption="Погодження орендодавця" VisibleIndex="21" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_renter") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="vydbudynku" Caption="Вид будинку" VisibleIndex="22" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("vydbudynku") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="history" Caption="Пам’ятка культурної спадщини" VisibleIndex="23" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("history") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="sf_upr" Caption="Сфера управління" VisibleIndex="24" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("sf_upr") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
		<dx:GridViewDataTextColumn FieldName="pdfurl" Caption="Pdf" VisibleIndex="25" Width="100px" Visible="false">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("pdfurl") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
		</dx:GridViewDataTextColumn>


        <%--<dx:GridViewDataTextColumn FieldName="komis_protocol" Caption="Протокол комісії" VisibleIndex="40" Width="100px">--%>
			<%--<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("komis_protocol") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>--%>
        <%--</dx:GridViewDataTextColumn>--%>


    </Columns>

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
        HorizontalScrollBarMode="Auto"
        ShowFooter="True"
        VerticalScrollBarMode="Auto"
        VerticalScrollBarStyle="Standard" />
    <%--<SettingsCookies CookiesID="GUKV.Reports1NF.FreeSquare" Version="A2_9" Enabled="True" />--%>
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewFreeSquareInit" EndCallback="GridViewFreeSquareEndCallback" />
</dx:ASPxGridView>

<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True" 
    ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
    HeaderText="Фотографії об'єкту з вільним приміщенням" Modal="True" 
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
    PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

            <asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
                SelectMethod="Select" 
                TypeName="ExtDataEntry.Models.FileAttachment">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
                    <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server" 
                ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
                <Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
                <SettingsFileList>
                    <ThumbnailsViewSettings ThumbnailSize="180px" />
                </SettingsFileList>
                    <SettingsEditing AllowDelete="false" AllowCreate="false" AllowDownload="true" AllowMove="false" AllowRename="false" />
                    <SettingsFolders Visible="False" />
                    <SettingsToolbar ShowDownloadButton="true" ShowPath="False" ShowFilterBox="false" />
                    <SettingsUpload UseAdvancedUploadMode="True" Enabled="false" >
                        <AdvancedModeSettings EnableMultiSelect="True" />
                    </SettingsUpload>
<%--                <SettingsEditing AllowDelete="false" AllowCreate="false" AllowDownload="false" AllowMove="false" AllowRename="false" />
                <SettingsFolders Visible="False" />
                <SettingsToolbar ShowDownloadButton="false" ShowPath="False" />
                <SettingsUpload UseAdvancedUploadMode="True" Enabled="false">
                    <AdvancedModeSettings EnableMultiSelect="True" />
                </SettingsUpload>--%>

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

</asp:Content>

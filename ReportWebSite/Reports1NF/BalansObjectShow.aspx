<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BalansObjectShow.aspx.cs" Inherits="Reports1NF_BalansObjectShow"
    MasterPageFile="~/BalansShowPublic.master" Title="Перелік об'єктів на балансі" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {
        //console.log("AdjustGridSizes", window.innerHeight - 180);
        //FreeSquareGridView.SetHeight(600);
        FreeSquareGridView.SetHeight(window.innerHeight - 180);
		//console.log("AdjustGridSizes2", FreeSquareGridView.GetHeight());
    }

	function GridViewBalansObjectsInit(s, e) {

		FreeSquareGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

	function GridViewBalansObjectsEndCallback(s, e) {

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

	window.onload = function () {
		jQuery(document).ready(function () {
			setTimeout(function () {
				AdjustGridSizes();
				$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI0_").hide();
				$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI5_IS").hide();
			})
		});
	};


    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansObjects" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT vb.* 
,b.sqr_total as sqr_total_bld
,b.sqr_pidval as sqr_pidval_bld
,b.sqr_mk as sqr_mk_bld
,b.sqr_dk as sqr_dk_bld
,b.sqr_rk as sqr_rk_bld
,b.sqr_other as sqr_other_bld
,b.sqr_rented as sqr_rented_bld
,b.sqr_zagal as sqr_zagal_bld
,b.sqr_for_rent as sqr_for_rent_bld
,b.sqr_habit as sqr_habit_bld
,b.sqr_non_habit as sqr_non_habit_bld
,b.sqr_loft as sqr_loft_bld 
,isnull(ddd.name, 'Невідомо') as sphera_dialnosti
,b.construct_year
,b.expl_enter_year
,case when exists (select 1 from reports1nf_balans q where q.id = vb.balans_id) then 1 else 0 end as ex_reports1nf_balans 
,(select top 1 q.report_id from reports1nf_balans q where q.id = vb.balans_id) as reports1nf_report_id
,case when vb.balans_id in (select b.id from dbo.reports1nf_balans b where b.organization_id = vb.organization_id and ISNULL(b.is_deleted, 0) = 0 ) then 1 else 0 end as is_dpz_object

    FROM view_balans_all vb
    LEFT JOIN reports1nf_balans bal on vb.balans_id = bal.id
    LEFT JOIN reports1nf_buildings b on bal.building_1nf_unique_id = b.unique_id
    LEFT OUTER JOIN (select obp.org_id,occ.name from org_by_period obp
         join dict_rent_occupation occ on occ.id = obp.org_occupation_id
          where obp.period_id = (select top 1 id from dict_rent_period order by id desc)) DDD ON DDD.org_id = vb.organization_id
    WHERE ( isnull(ddd.name,'') not in ('НЕВИЗНАЧЕНІ') ) and
 	 ((@p_dpz_filter = 0) OR (@p_dpz_filter <> 0 AND vb.balans_id in (select b.id from dbo.reports1nf_balans b where b.organization_id = vb.organization_id and ISNULL(b.is_deleted, 0) = 0 ) )) AND
        ((@p_com_filter = 0) OR (@p_com_filter <> 0 AND (vb.org_ownership_int IN (32,33,34) OR vb.form_ownership_int IN (32,33,34)))) AND
        ((@p_show_deleted = 1) OR (@p_show_deleted = 0 AND (vb.is_deleted IS NULL OR vb.is_deleted = 0 OR vb.is_not_accepted = 1))) AND
        ((@p_rda_district_id = 0) OR (vb.org_ownership_int in (select id from dict_org_ownership where is_rda = 1) AND vb.org_district_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceBalansObjects_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="1" Name="p_dpz_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_com_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_show_deleted" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceYesNo" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_yes_no" >
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік об'єктів на балансі" CssClass="reporttitle"></asp:Label>
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

            <dx:ASPxButton ID="ASPxButton_FreeSquare_SaveAs" runat="server" AutoPostBack="False" Visible="false"
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

<dx:ASPxGridView ID="FreeSquareGridView" runat="server" 
    ClientInstanceName="FreeSquareGridView"
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
            VisibleIndex="0" Visible="false" Caption="Картка" >
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowBalansCardEx(" + Eval("ex_reports1nf_balans") + "," + Eval("balans_id") + "," + Eval("reports1nf_report_id") + ")\"><img border='0' src='../Styles/" + ((int)Eval("is_dpz_object") == 1 ? "EditIcon_green.png" : "EditIcon.png") + "'/></a></center>"%>
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
<%--            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("org_full_name") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="False" Caption="Балансоутримувач - Коротка Назва">
<%--            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("org_short_name") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Балансоутримувач - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="is_dpz_object" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="true" Caption="DPZ"></dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="org_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="False" Caption="Балансоутримувач - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="False" Caption="Балансоутримувач - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="True" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="True" Caption="Назва Вулиці" Width="120px">
<%--            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="True" Caption="Номер Будинку">
<%--            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
            </DataItemTemplate>--%>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
<%-- --%>
             <dx:GridViewDataTextColumn FieldName="sqr_total_bld" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="False" Caption="Загальна Площа будинку (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_non_habit_bld" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Площа нежилих приміщень будинку (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_habit_bld" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="False" Caption="Площа житлового фонду будинку (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_pidval_bld" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="False" Caption="Площа підвалу будинку (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_loft_bld" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="False" Caption="Площа горища будинку (кв.м.)"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="sqr_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="True" Caption="Площа нежилих приміщень об'єкту (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_vlas_potreb" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="Площа об'єкту Для Власних Потреб (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_kor" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="Корисна площа об'єкту (кв.м.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="sqr_pidval" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="16" Visible="False" Caption="Площа підвалу об'єкту (кв.м.)"></dx:GridViewDataTextColumn>      
        <dx:GridViewDataTextColumn FieldName="sqr_free" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Загальна Площа вільного приміщення (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_in_rent" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="False" Caption="Площа  об'єкту, що надається в Оренду (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_privatizov" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="False" Caption="Приватизована Площа (кв.м.)"></dx:GridViewDataTextColumn>      
        <dx:GridViewDataTextColumn FieldName="sqr_gurtoj" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="False" Caption="Площа Гуртожитку (кв.м.)"></dx:GridViewDataTextColumn>      
        <dx:GridViewDataTextColumn FieldName="cost_expert_1m" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="False" Caption="Оціночна Вартість За Кв.м. (тис.грн.)"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="sqr_engineering" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="False" Caption="Площа техніко-інженерних потреб об'єкту (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_balans" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="true" Caption="Первісна (переоцінена) вартість, тис. грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="False" Caption="Ринкова вартість, тис. грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_zalishkova" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="27" Visible="True" Caption="Залишкова Вартість, тис.грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_rent_agr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="28" Visible="False" Caption="Кількість Договорів Оренди"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="obj_bti_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="29" Visible="False" Caption="Інвентаризаційний № справи"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="date_bti" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Дата виготовлення технічного паспорту"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="realestateobj" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="False" Caption="Реєстрація у Державному реєстрі (Об'єкт нерухомого майна)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="privacynote" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="True" Caption="Реєстрація у Державному реєстрі (Номер запису про право власності)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bti_condition" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="False" Caption="Реєстрація у Державному реєстрі (Реєстраційний номер об'єкту нерухомого майна)"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="invent_no_bti" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="34" Visible="False" Caption="Інвентарний номер об'єкту"></dx:GridViewDataTextColumn>

<%--        <dx:GridViewDataTextColumn FieldName="num_privat_apt" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="29" Visible="False" Caption="Кількість Приватизованих Приміщень"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="approval_by" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Дозвіл - Джерело"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="approval_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="False" Caption="Дозвіл - Номер"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="approval_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="False" Caption="Дозвіл - Дата"></dx:GridViewDataDateColumn>      
        <dx:GridViewDataTextColumn FieldName="o26_code" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="33" Visible="False" Caption="О26"></dx:GridViewDataTextColumn>      
        <dx:GridViewDataTextColumn FieldName="bti_condition" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="34" Visible="True" Caption="Інвентаризаційний № справи"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="num_floors" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="False" Caption="Кількість поверхів загальна"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="construct_year" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="36" Visible="False" Caption="Рік будівництва"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="expl_enter_year" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="37" Visible="False" Caption="Рік здачі в експлуатацію"></dx:GridViewDataTextColumn>

<%--        <dx:GridViewDataTextColumn FieldName="org_maintain_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="36" Visible="False" Caption="ID Утримуючої Організації">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_maintain_id") + ")\">" + Eval("org_maintain_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_maintainer_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="37" Visible="False" Caption="Утримуюча Організація - Повна Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_maintain_id") + ")\">" + Eval("org_maintainer_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_maintainer_short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="38" Visible="False" Caption="Утримуюча Організація - Коротка Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_maintain_id") + ")\">" + Eval("org_maintainer_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_maintainer_zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="39" Visible="False" Caption="Утримуюча Організація - Код ЄДРПОУ"></dx:GridViewDataTextColumn>      --%>

        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="40" Visible="False" Caption="Стан юр.особи"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="41" Visible="False" Caption="Форма Власності Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ownership_type" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="42" Visible="False" Caption="Право"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="43" Visible="True" Caption="Вид Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_type" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="44" Visible="True" Caption="Тип Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="condition" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="45" Visible="True" Caption="Стан Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="46" Visible="False" Caption="Група Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="47" Visible="True" Caption="Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="history" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="48" Visible="False" Caption="Історична Цінність"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="floors" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="50" Visible="False" Caption="Розташування приміщення (поверх)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="memo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="52" Visible="False" Caption="Додаткова Інформація"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="note" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="53" Visible="False" Caption="Примітка"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="znos" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="54" Visible="False" Caption="Знос (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="znos_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="55" Visible="False" Caption="Знос станом на"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="org_vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="56" Visible="False" Caption="Балансоутримувач - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="57" Visible="False" Caption="Балансоутримувач - Форма Власності"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataDateColumn FieldName="input_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="58" Visible="False" Caption="Дата Актуальності"></dx:GridViewDataDateColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="is_in_privat" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="59" Visible="False" Caption="Будинок В Програмі Приватизації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="balans_obj_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="60" Visible="True" Caption="Назва Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="modify_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="63" Visible="True" Caption="Дата Актуальності"></dx:GridViewDataTextColumn>

<%--        <dx:GridViewDataTextColumn FieldName="sqr_free_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="61" Visible="False" Caption="Вільні Приміщення: Загальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_korysna" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="62" Visible="False" Caption="Вільні Приміщення: Корисна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_floors" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="64" Visible="False" Caption="Місце Розташування Вільного Приміщення (поверх)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_purpose" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="65" Visible="False" Caption="Можливе Використання Вільного Приміщення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_on_balans" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="66" Visible="False" Caption="Об'єкт Перебуває На Балансі"></dx:GridViewDataTextColumn>
        <dx:GridViewDataComboBoxColumn FieldName="is_not_accepted" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="67" Visible="False" Caption="Об'єкт Знято З Балансу, Але Не Прийнято">      
            <PropertiesComboBox DataSourceID="SqlDataSourceYesNo" ValueField="id" TextField="name" ValueType="System.Int32" />      
        </dx:GridViewDataComboBoxColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="sphera_dialnosti" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="68" Visible="True" Caption="Сфера діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="gosp_struct" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="69" Visible="false" Caption="Госп. Структура"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_contacts" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="70" Visible="false" Caption="Контактні телефони" Width="160px"></dx:GridViewDataTextColumn>
<%-- --%>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total_bld" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_non_habit_bld" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_habit_bld" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_pidval_bld" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_loft_bld" SummaryType="Sum" DisplayFormat="{0}" />

        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_pidval" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_vlas_potreb" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_in_rent" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_privatizov" SummaryType="Sum" DisplayFormat="{0}" />
<%--        <dx:ASPxSummaryItem FieldName="sqr_not_for_rent" SummaryType="Sum" DisplayFormat="{0}" />    --%>
        <dx:ASPxSummaryItem FieldName="sqr_kor" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_gurtoj" SummaryType="Sum" DisplayFormat="{0}" />
<%--        <dx:ASPxSummaryItem FieldName="sqr_non_habit" SummaryType="Sum" DisplayFormat="{0}" />      --%>
        <dx:ASPxSummaryItem FieldName="sqr_engineering" SummaryType="Sum" DisplayFormat="{0}" />

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
        VerticalScrollBarMode="Auto"
        VerticalScrollBarStyle="Standard" />
    <%--<SettingsCookies CookiesID="GUKV.BalansObjects" Version="A2_5" Enabled="True" />--%>
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewBalansObjectsInit" EndCallback="GridViewBalansObjectsEndCallback" />
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
                    <SettingsToolbar ShowDownloadButton="true" ShowPath="true" ShowFilterBox="false" />
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

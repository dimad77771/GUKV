<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFPrivatisatShow.aspx.cs" Inherits="Reports1NF_Report1NFPrivatisatShow"
    MasterPageFile="~/PrivatisatShowPublic.master" Title="Перелік об'єктів приватизації" %>

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
			'Report1NFPrivatisatMap.aspx?fs_id=' + id,
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
    (select Q.name from dict_streets Q where Q.id = A.addr_street_id) as addr_street
      ,*
      ,(select Q.zkpo_code from reports1nf_org_info Q where Q.report_id = A.org_info_id) as zkpo_code
      ,(select Q.short_name from reports1nf_org_info Q where Q.report_id = A.org_info_id) as org_name
      ,(select Q.name from dict_1nf_districts2 Q where Q.id = A.addr_distr_new_id) as addr_distr
      ,@baseurl + '/Reports1NF/BalansFreeSquarePhotosPdf.aspx?id=' + cast(A.[id] as varchar(100)) as pdfurl
  FROM [privatisat] A
    order by 1, [addr_nomer]   "
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

<%--<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFAccounts.aspx" Text="Облікові Записи"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFNotifications.aspx" Text="Налаштування Повідомлень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>        
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFFreeShow.aspx" Text="Перелік вільних приміщень"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>--%>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік об'єктів приватизації" CssClass="reporttitle"></asp:Label>
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

   <dx:ASPxGridView ID="FreeSquareGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceFreeSquare" KeyFieldName="id" Width="100%" 
        ClientInstanceName="FreeSquareGridView" 
        OnDataBound="FreeSquareGridView_DataBound"
        OnCustomCallback="GridViewFreeSquare_CustomCallback"
        OnCustomFilterExpressionDisplayText="GridViewFreeSquare_CustomFilterExpressionDisplayText"
        OnProcessColumnAutoFilter="GridViewFreeSquare_ProcessColumnAutoFilter" >
	   <ClientSideEvents CustomButtonClick="ShowPhoto" />
    <Templates>
        <DetailRow>
            <div style="margin-left:5px">
                <table style="border:1px solid; margin-top:6px; border-collapse:collapse; width:1700px">
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px; width:200px">
                            Включено до переліку №
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px; width:200px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("include_in_perelik") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px; width:200px">
                            Залишкова балансова вартість, тис. грн.
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px; width:200px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("zal_balans_vartist") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Первісна балансова вартість, тис. грн.
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("perv_balans_vartist") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Тип об’єкта
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("free_object_type_name") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Пропонований строк оренди (у роках)
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("prop_srok_orands") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Пункт Методики розрахунку орендної плати (якщо об’єкт пропонується для включення до Переліку другого типу)
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("punkt_metod_rozrahunok") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("invest_solution") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Погодження органу управління балансоутримувача
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_control") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Район
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("district") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Назва Вулиці
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("street_name") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Номер Будинку
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("addr_nomer") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Загальна площа об’єкта, кв.м
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("total_free_sqr") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Корисна площа об’єкта, кв.м
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("free_sql_usefull") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Характеристика об’єкта оренди(будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі(надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("floor") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Технічний стан
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("condition") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Водопостачання
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("water") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Теплопостачання
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("heating") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Газопостачання
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("gas") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Електропостачання
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("power_text") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Пам’ятка культурної спадщини
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("history") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Погодження органу охорони культурної спадщини
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_renter") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Номер запису про право власності у Реєстрація у Державному реєстрі речових прав на нерухоме майно
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("nomer_derzh_reestr_neruh") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Реєстраційний номер об'єкту нерухомого майна у Реєстрація у Державному реєстрі речових прав на нерухоме майно
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("reenum_derzh_reestr_neruh") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Інформація про цільове призначення об’єкта оренди
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("info_priznach_nouse") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("info_rahunok_postach") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Цільове призначення об’єкта, за яким об’єкт використовувався перед тим, як він став вакантним
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("priznach_before") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Період часу, протягом якого об’єкт не використовується (у місяцях)
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("period_nouse") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Інформацію про особу, яка використовувала об’єкт перед тим, як він став вакантним
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("osoba_use_before") %>' Font-Bold="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </DetailRow>
    </Templates>
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


        <dx:GridViewDataSpinEditColumn FieldName="privat_year" Caption="Рік" Width="70px" CellStyle-HorizontalAlign="Center">
            <PropertiesSpinEdit NumberType="Integer">
                <SpinButtons Enabled="true" ></SpinButtons>
            </PropertiesSpinEdit>
        </dx:GridViewDataSpinEditColumn>

        <dx:GridViewDataTextColumn FieldName="kmr_number" Caption="Номер рішення КМР" Width="80px" CellStyle-HorizontalAlign="Center">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="kmr_date" Caption="Дата рішення КМР"  Width="80px" CellStyle-HorizontalAlign="Center">
        </dx:GridViewDataDateColumn>


        <dx:GridViewDataTextColumn FieldName="org_name" Caption="Балансоутримувач" Width="220px" ReadOnly="true" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("org_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


       <dx:GridViewDataTextColumn FieldName="zkpo_code" Caption="Код ЄДРПОУ"  Width="75px" CellStyle-HorizontalAlign="Center">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="organizator" Caption="Організатор продажу"  Width="200px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="obj_name" Caption="Назва об’єкта"  Width="200px">
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn FieldName="addr_distr" Caption="Район"  Width="120px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="addr_street" Caption="Назва вулиці"  Width="200px">
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn FieldName="addr_nomer" Caption="Номер будинку"  Width="75px">
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn FieldName="orendar" Caption="Орендар"  Width="200px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataSpinEditColumn FieldName="total_free_sqr" Caption="Площа об’єкта, кв.м" Width="75px">
            <PropertiesSpinEdit NumberType="Float" NumberFormat="Number">
                <SpinButtons Enabled="true" ></SpinButtons>
            </PropertiesSpinEdit>
        </dx:GridViewDataSpinEditColumn>

        <dx:GridViewDataTextColumn FieldName="sposib_privat" Caption="Спосіб приватизації"  Width="100px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="document_privat" Caption="Документи щодо приватизації об’єкта"  Width="300px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataSpinEditColumn FieldName="obj_price" Caption="Ціна продажу, грн." Width="95px">
            <PropertiesSpinEdit NumberType="Float" NumberFormat="Number">
                <SpinButtons Enabled="true" ></SpinButtons>
            </PropertiesSpinEdit>
        </dx:GridViewDataSpinEditColumn>

        <dx:GridViewDataTextColumn FieldName="buyer_name" Caption="Покупець (назва)"  Width="200px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="buyer_adr_street" Caption="Покупець (Назва вулиці)"  Width="200px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="buyer_adr_nomer" Caption="Покупець (Номер будинку, літери, корпус)"  Width="80px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="prozoro_number" Caption="Унікальний код обєкту у ЕТС Прозорро-продажі"  Width="150px">
            <DataItemTemplate>
                <%# "<a target=\"_blank\" href=\"https://prozorro.sale/ssp_object/" + Eval("prozoro_number") + "\">" + Eval("prozoro_number") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>




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
    HeaderText="Документи по об'єкту приватизації" Modal="True" 
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" RenderMode="Lightweight" 
    PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

            <asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
                SelectMethod="Select" 
                TypeName="ExtDataEntry.Models.FileAttachment">
                <SelectParameters>
                    <asp:Parameter DefaultValue="privatisat_documents" Name="scope" Type="String" />
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

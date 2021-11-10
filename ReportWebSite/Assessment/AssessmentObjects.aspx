<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssessmentObjects.aspx.cs" Inherits="Assessment_AssessmentObjects" MasterPageFile="~/NoHeader.master" Title="Оцінка Об'єктів"%>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AddressPicker.ascx" tagname="AddressPicker" tagprefix="uc2" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register src="../UserControls/FieldFixxer.ascx" tagname="FieldFixxer" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function ShowFieldFixxerPopupControl(s, e) { PopupFieldFixxer.Show(); }
    
    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 185);
    }

    function GridViewAssessmentObjectsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewAssessmentObjectsEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function ShowAddressPickerPopupControl(s, e) {

        PopupAddressPicker.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
	}

	function CustomButtonClick(s, e) {

		if (e.buttonID == 'btnEditAgreement') {
			var cardUrl = "../Cards/AssessmentCard.aspx?vid=" + PrimaryGridView.GetRowKey(e.visibleIndex);
			//alert(cardUrl);
			<%--var cardUrl = "../Reports1NF/OrgRentAgreement.aspx?rid=" + <%= Request.QueryString["rid"] %> + "&aid=" + PrimaryGridView.GetRowKey(e.visibleIndex);--%>
			window.location = cardUrl;
		}

		if (e.buttonID == 'btnDeleteAgreement') {

			if (confirm("Видалити Оцінку ?")) {
				PrimaryGridView.PerformCallback(JSON.stringify({ AgreementToDeleteID: PrimaryGridView.GetRowKey(e.visibleIndex) }));
			}
		}

		e.processOnServer = false;
	}

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentObjects" runat="server" EnableCaching="false"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_assessment">
</mini:ProfiledSqlDataSource>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Оцінка Об'єктів" CssClass="reporttitle"></asp:Label>
			<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
			<dx:ASPxButton ID="ButtonAddAgreement" runat="server" Text="Додати Нову Оцінку" AutoPostBack="false">
				<ClientSideEvents Click="function(s, e) {
					if (confirm('Додати нову оцінку ?')) {
						var cardUrl = '../Cards/AssessmentCard.aspx?isnew=1';
						window.location = cardUrl;
						e.processOnServer = false;
					}
				}" />
			</dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonQuickSearchAddr" runat="server" AutoPostBack="False" Text="" ImageSpacing="0px" AllowFocus="false" Visible="false"
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
			<dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px" Visible="false">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px" Visible="false">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_AllBuildings_SaveAs" 
                PopupElementID="ASPxButton_AssessmentObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxButton ID="ASPxButton3" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_AssessmentObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_AssessmentObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_AssessmentObjects_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_AssessmentObjects_SaveAs" runat="server" AutoPostBack="False" Visible="true" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewAssessmentObjectsExporter" runat="server" 
    FileName="Оцінка" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="PrimaryGridView"
    ClientInstanceName="PrimaryGridView"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceAssessmentObjects"
    KeyFieldName="id"
	OnHtmlRowPrepared="PrimaryGridView_HtmlRowPrepared"
	OnHtmlDataCellPrepared="PrimaryGridView_HtmlDataCellPrepared"
    OnCustomCallback="GridViewAssessmentObjects_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewAssessmentObjects_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewAssessmentObjects_ProcessColumnAutoFilter"
    OnCustomColumnDisplayText = "GridViewAssessmentObjects_CustomColumnDisplayText"
    OnCustomColumnSort="GridViewAssessmentObjects_CustomColumnSort" >

    <Columns>
		<dx:GridViewCommandColumn ButtonType="Image" Name="CmdColumn" Width="60px">
            <CustomButtons>                
                <dx:GridViewCommandColumnCustomButton ID="btnEditAgreement"><Image  Url="../Styles/EditIcon.png" ToolTip="Редагувати оцінку" /></dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnDeleteAgreement"><Image  Url="../Styles/DeleteIcon.png" ToolTip="Видалити оцінку" /></dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Вулиця" Width="180px">
            <%--<DataItemTemplate>
                <%# EvaluateObjectLink(Eval("building_id"), Eval("street_full_name"))%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Номер Будинку" Width="60px">
            <%--<DataItemTemplate>
                <%# EvaluateObjectLink(Eval("building_id"), Eval("addr_nomer"))%>
            </DataItemTemplate>--%>
            <%--<Settings SortMode="Custom" />--%>
        </dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="obj_square" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Площа" Width="70px">
		</dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="inputdoc_doc_num" ReadOnly="True" ShowInCustomizationForm="True" Caption="Вхідний номер" Width="90px" >
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("inputdoc_doc_num")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="inputdoc_doc_date" ReadOnly="True" ShowInCustomizationForm="True" Caption="Дата документа" Width="90px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("inputdoc_doc_date")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="inputdoc_control_date" ReadOnly="True" ShowInCustomizationForm="True" Caption="Контрольна дата" Width="90px">
			<DataItemTemplate>
                <%# EvaluateСontrolDate(Eval("inputdoc_control_date")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="inputdoc_rezenz_name" ReadOnly="True" ShowInCustomizationForm="True" Caption="Рецензент" Width="140px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("inputdoc_rezenz_name")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="inputdoc_korrespondent" ReadOnly="True" ShowInCustomizationForm="True" Caption="Кореспондент" Width="140px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("inputdoc_korrespondent")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>



        <%--<dx:GridViewDataTextColumn FieldName="renter_name" ReadOnly="True" ShowInCustomizationForm="False" Visible="True" Caption="Картка" Width="65px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowAssessmentCard(" + Eval("id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="renter_name" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Орендар" Width="160px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("org_renter_id"), Eval("renter_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="dict_expert_name" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="СОД" Width="180px">
            <%--<DataItemTemplate>
                <%# EvaluateObjectLink(Eval("building_id"), Eval("street_full_name"))%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="expert_obj_type" ReadOnly="True" ShowInCustomizationForm="True" Visible="true" Caption="Вид Об’єкта" Width="120px">
		</dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="stan_name" ReadOnly="True" ShowInCustomizationForm="True" Visible="true" Caption="Стан рецензії" Width="110px">
		</dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="outputdoc_doc_num" ReadOnly="True" ShowInCustomizationForm="True" Caption="Вихідний номер" Width="90px" >
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("outputdoc_doc_num")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="outputdoc_doc_date" ReadOnly="True" ShowInCustomizationForm="True" Caption="Дата документа" Width="90px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("outputdoc_doc_date")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="outputdoc_rezenz" ReadOnly="True" ShowInCustomizationForm="True" Caption="Категорія рецензії" Width="140px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("outputdoc_rezenz")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="outputdoc_rezenz_date" ReadOnly="True" ShowInCustomizationForm="True" Caption="Дата рецензування" Width="90px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("outputdoc_rezenz_date")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>



		<dx:GridViewDataTextColumn FieldName="balans_org_name" ReadOnly="True" ShowInCustomizationForm="True" Visible="true" Caption="Балансоутримувач" Width="220px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("org_balans_id"), Eval("balans_org_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>

        <%--<dx:GridViewDataDateColumn FieldName="valuation_date" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Дата рецензування" Width="80px">
        </dx:GridViewDataDateColumn>--%>

		<%--<dx:GridViewDataTextColumn FieldName="obj_square" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Площа Об'єкту" Width="80px">
		</dx:GridViewDataTextColumn>--%>


		<dx:GridViewDataTextColumn FieldName="details_obj_square" ReadOnly="True" ShowInCustomizationForm="True" Caption="Розбивка площі" Width="90px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("details_obj_square")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="details_floors" ReadOnly="True" ShowInCustomizationForm="True" Caption="Поверх" Width="110px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("details_floors")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="details_purpose" ReadOnly="True" ShowInCustomizationForm="True" Caption="НЕВ за звітом" Width="130px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("details_purpose")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="details_cost_1_usd" ReadOnly="True" ShowInCustomizationForm="True" Caption="Вартість 1 кв.м. без ПДВ, $" Width="90px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("details_cost_1_usd")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="cost_prim" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Вартість об’єкту без ПДВ (грн.)" Width="100px">
		</dx:GridViewDataTextColumn>

		<dx:GridViewDataDateColumn FieldName="final_date" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Дата затвердження" Width="80px">
		</dx:GridViewDataDateColumn>

		<dx:GridViewDataTextColumn FieldName="arch_num" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Архівний номер" Width="80px">
		</dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="note_text" ReadOnly="True" ShowInCustomizationForm="True" Caption="Примітка" Width="200px">
			<DataItemTemplate>
                <%# EvaluateMultiLineValue(Eval("note_text")) %>
            </DataItemTemplate>
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>


        
        <%--<dx:GridViewDataTextColumn FieldName="district" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Район" Width="120px"></dx:GridViewDataTextColumn>--%>
        
<%--		<dx:GridViewDataTextColumn FieldName="in_doc_dates" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" Caption="Дата Вхідного Документу"></dx:GridViewDataTextColumn>
        
        <dx:GridViewDataTextColumn FieldName="in_doc_korr" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" Caption="Кореспондент"></dx:GridViewDataTextColumn>--%>

<%--        <dx:GridViewDataTextColumn FieldName="out_doc_dates" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" Caption="Дата Вихідного Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="out_doc_numbers" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" Caption="Номер Вихідного Документу"></dx:GridViewDataTextColumn>--%>

        
        

<%--        <dx:GridViewDataTextColumn FieldName="cost_prim" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Вартість Об’єкта (грн.)"></dx:GridViewDataTextColumn>
    
        <dx:GridViewDataDateColumn FieldName="final_date" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Дата Затвердження Оцінки"></dx:GridViewDataDateColumn>
        
        <dx:GridViewDataTextColumn FieldName="valuation_kind" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" Caption="Вид Оцінки"></dx:GridViewDataTextColumn>--%>
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="obj_square" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_1_usd" SummaryType="Average" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_prim" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager PageSize="25"></SettingsPager>
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
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.Assessment.Objects" Version="A12" Enabled="true" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewAssessmentObjectsInit" EndCallback="GridViewAssessmentObjectsEndCallback" CustomButtonClick="CustomButtonClick" />
</dx:ASPxGridView>

</center>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
            <uc1:SaveReportCtrl ID="FolderBrowser" runat="server"/>
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
        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>

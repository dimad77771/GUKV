<%@ page language="C#" autoeventwireup="true" inherits="Assessment_AssessmentObjects, App_Web_assessmentobjects.aspx.26f71a44" masterpagefile="~/NoHeader.master" title="Оцінка Об'єктів" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
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

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentObjects" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_assessment">
</mini:ProfiledSqlDataSource>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Оцінка Об'єктів" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonQuickSearchAddr" runat="server" AutoPostBack="False" Text="" ImageSpacing="0px" AllowFocus="false"
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

            <dx:ASPxButton ID="ASPxButton_AssessmentObjects_SaveAs" runat="server" AutoPostBack="False" 
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
    KeyFieldName="valuation_id"
    OnCustomCallback="GridViewAssessmentObjects_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewAssessmentObjects_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewAssessmentObjects_ProcessColumnAutoFilter"
    OnCustomColumnDisplayText = "GridViewAssessmentObjects_CustomColumnDisplayText"
    OnCustomColumnSort="GridViewAssessmentObjects_CustomColumnSort" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="renter_name" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="0" Visible="True" Caption="Картка" Width="65px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowAssessmentCard(" + Eval("valuation_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="renter_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Visible="True" Caption="Орендар" Width="150px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("org_renter_id"), Eval("renter_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="balans_org_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Visible="False" Caption="Балансоутримувач" Width="150px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("org_balans_id"), Eval("balans_org_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" Visible="True" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Visible="True" Caption="Назва Вулиці">
            <DataItemTemplate>
                <%# EvaluateObjectLink(Eval("building_id"), Eval("street_full_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Visible="True" Caption="Номер Будинку">
            <DataItemTemplate>
                <%# EvaluateObjectLink(Eval("building_id"), Eval("addr_nomer"))%>
            </DataItemTemplate>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="in_doc_dates" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="6" Visible="False" Caption="Дата Вхідного Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="in_doc_numbers" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="7" Visible="False" Caption="Номер Вхідного Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="in_doc_korr" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="9" Visible="False" Caption="Кореспондент"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="out_doc_dates" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="11" Visible="False" Caption="Дата Вихідного Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="out_doc_numbers" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="12" Visible="False" Caption="Номер Вихідного Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="expert_obj_type" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="14" Visible="False" Caption="Вид Об’єкта"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_square" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="15" Visible="True" Caption="Площа" Width="70px"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="cost_prim" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="19" Visible="True" Caption="Вартість Об’єкта (грн.)"></dx:GridViewDataTextColumn>
    
        <dx:GridViewDataDateColumn FieldName="final_date" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="21" Visible="True" Caption="Дата Затвердження Оцінки"></dx:GridViewDataDateColumn>
        
        
        <dx:GridViewDataTextColumn FieldName="valuation_kind" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="25" Visible="False" Caption="Вид Оцінки"></dx:GridViewDataTextColumn>
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
        VerticalScrollBarStyle="Virtual" />
    <SettingsCookies CookiesID="GUKV.Assessment.Objects" Version="A2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewAssessmentObjectsInit" EndCallback="GridViewAssessmentObjectsEndCallback" />
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

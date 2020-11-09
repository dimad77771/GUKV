<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocRelations.aspx.cs" Inherits="Documents_DocRelations" MasterPageFile="~/NoHeader.master" Title="Документи" %>

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

    function GridViewDocumentsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewDocumentsEndCallback(s, e) {

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

        if (e.buttonID == 'btnDeleteAgreement') {

            if (confirm("Видалити Документ?")) {
                PrimaryGridView.PerformCallback(JSON.stringify({ AgreementToDeleteID: PrimaryGridView.GetRowKey(e.visibleIndex) }));
            }
        }

        e.processOnServer = false;
    }

    // ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Documents/DocRelations.aspx" Text="Документи"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Documents/DocObjects.aspx" Text="Об'єкти за Документами"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<%-- Content of the first tab BEGIN --%>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Документи" CssClass="reporttitle"></asp:Label>
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
                ClientInstanceName="ASPxPopupControl_Documents_SaveAs" 
                PopupElementID="ASPxButton_Documents_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton2" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Documents_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Documents_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Documents_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Documents_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterDocuments" runat="server" 
    FileName="Документи" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocuments" runat="server" EnableCaching="false"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM report_documents">
</mini:ProfiledSqlDataSource>

<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceDocuments" 
    KeyFieldName="parent_id;child_id"
    Width="100%"
    OnCustomCallback="GridViewDocuments_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewDocuments_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewDocuments_ProcessColumnAutoFilter" >

    <Columns>
        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Name="CmdColumn" Width="60px">
            <CustomButtons>                
                <dx:GridViewCommandColumnCustomButton ID="btnDeleteAgreement"><Image  Url="../Styles/DeleteIcon.png" ToolTip="Видалити" /></dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataTextColumn FieldName="parent_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="1" Visible="False" Caption="ID Документа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="False" Caption="Вид Документа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_general_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="False" Caption="Узагальнення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="parent_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Дата" Width="90px"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="parent_date_year" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="False" Caption="Дата - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_date_quarter" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="False" Caption="Дата - Квартал"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="True" Caption="Номер" Width="90px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("parent_id") + ")\">" + Eval("parent_num") + "</a>"%>
            </DataItemTemplate> 
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_topic" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="True" Caption="Назва" Width="230px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("parent_id") + ")\">" + Eval("parent_topic") + "</a>"%>
            </DataItemTemplate>
         </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_note" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="False" Caption="Примітка"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="parent_receive_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="False" Caption="Дата Отримання"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="parent_commission" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Комісія"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_source" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="False" Caption="Джерело"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_state" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="False" Caption="Стан"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_summa" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="False" Caption="Сума (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_summa_zalishkova" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="False" Caption="Залишкова Сума (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="16" Visible="False" Caption="ID Підп. Документа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="Вид Підп. Документа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_general_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Узагальнення Підп. Документа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="child_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="True" Caption="Дата Підп. Документа" Width="90px"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="child_date_year" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="False" Caption="Дата Підп. Документа - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_date_quarter" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="Дата Підп. Документа - Квартал"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="True" Caption="Номер Підп. Документа" Width="90px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("child_id") + ")\">" + Eval("child_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_topic" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="True" Caption="Назва Підп. Документа" Width="230px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("child_id") + ")\">" + Eval("child_topic") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_note" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="False" Caption="Примітка Підп. Документа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="child_receive_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="False" Caption="Дата Отримання Підп. Документа"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="child_commission" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="False" Caption="Комісія Підп. Документа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_source" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="27" Visible="False" Caption="Джерело Підп. Документа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_state" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="28" Visible="False" Caption="Стан Підп. Документа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_summa" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="29" Visible="False" Caption="Сума за Підп. Документом (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_summa_zalishkova" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Залишкова Сума за Підп. Документом (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="parent_is_rozp" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="False" Caption="Розпорядчий Документ"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="parent_summa" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="parent_summa_zalishkova" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="child_summa" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="child_summa_zalishkova" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
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
    <SettingsCookies CookiesID="GUKV.DocRelations" Version="A4" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents 
		Init="GridViewDocumentsInit" 
		EndCallback="GridViewDocumentsEndCallback"
		CustomButtonClick="CustomButtonClick" />
</dx:ASPxGridView>

<%-- Content of the first tab END --%>

</center>

<iframe id="docTextFrame" src="" style="visibility:hidden" width="0px" height="0px"></iframe>

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
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
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

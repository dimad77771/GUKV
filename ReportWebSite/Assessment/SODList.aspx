﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SODList.aspx.cs" Inherits="Assessment_SODList" MasterPageFile="~/NoHeader.master" Title="Каталог СОД" %>

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

    function GridViewAssessmentExpertsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewAssessmentExpertsEndCallback(s, e) {

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

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentExperts" runat="server" EnableCaching="false"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM dict_expert"
	InsertCommand="INSERT INTO [dict_expert](
					[id],
					[full_name],
					[short_name],	
					[fio_boss],	
					[tel_boss],
					[certificate_num],
					[certificate_date],
					[certificate_end_date],
					[case_num],
					[addr_full],
					[note])
				VALUES(
					isnull((select max(id) + 1 from dict_expert), 1),
					@full_name,
					@short_name,	
					@fio_boss,	
					@tel_boss,
					@certificate_num,
					@certificate_date,
					@certificate_end_date,
					@case_num,
					@addr_full,
					@note)"
	UpdateCommand="UPDATE [dict_expert] SET 
			[full_name] = @full_name,
			[short_name] = @short_name,
			--[modified_by] = @modified_by,
			--[modify_date] = @modify_date,
			[fio_boss] = @fio_boss,
			[tel_boss] = @tel_boss,
			--[addr_district_id] = @addr_district_id,
			--[addr_street_id] = @addr_street_id,
			--[addr_zip_code] = @addr_zip_code,
			--[addr_number] = @addr_number,
			[certificate_num] = @certificate_num,
			[certificate_date] = @certificate_date,
			[certificate_end_date] = @certificate_end_date,
			[case_num] = @case_num,
			[addr_full] = @addr_full,
			[note] = @note
			--[is_deleted] = @is_deleted,
			--[del_date] = @del_date
		WHERE id = @id"
	DeleteCommand="DELETE FROM [dict_expert] WHERE id = @id"
	>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
    <Items>
        <dx:MenuItem NavigateUrl="../Assessment/AssessmentObjects.aspx" Text="Оцінка Об'єктів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Assessment/SODList.aspx" Text="Каталог Суб'єктів Оціночної Діяльності"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle2" runat="server" Text="Каталог Суб'єктів Оціночної Діяльності" CssClass="reporttitle"></asp:Label>
        </td>
		<td>
			<dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px" Visible="false">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px" Visible="false">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_Experts_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Experts_SaveAs" 
                PopupElementID="ASPxButton_Experts_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton8" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                           OnClick="ASPxButton_Experts_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton9" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Experts_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton10" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Experts_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Experts_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewAssessmentExpertsExporter" runat="server" 
    FileName="СОД" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourceAssessmentExperts"
    KeyFieldName="id"
    OnCustomCallback="GridViewAssessmentExperts_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewAssessmentExperts_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewAssessmentExperts_ProcessColumnAutoFilter" >

	<SettingsCommandButton>
		<EditButton>
			<Image Url="~/Styles/EditIcon.png" ToolTip="Редагувати Підставу" />
		</EditButton>
		<CancelButton>
			<Image Url="~/Styles/CancelIcon.png" />
		</CancelButton>
		<UpdateButton>
			<Image Url="~/Styles/SaveIcon.png" />
		</UpdateButton>
		<DeleteButton>
			<Image Url="~/Styles/DeleteIcon.png" ToolTip="Видалити Підставу" />
		</DeleteButton>
		<NewButton>
			<Image Url="~/Styles/AddIcon.png" />
		</NewButton>
		<ClearFilterButton Text="Очистити" RenderMode="Link" />
	</SettingsCommandButton>

    <Columns>
		<dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowEditButton="true" ShowDeleteButton="True" ShowNewButton="true" />
        <dx:GridViewDataTextColumn FieldName="full_name" ShowInCustomizationForm="True" VisibleIndex="0" Visible="True" Caption="Повна Назва" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="short_name" ShowInCustomizationForm="True" VisibleIndex="1" Visible="true" Caption="Коротка Назва" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="fio_boss" ShowInCustomizationForm="True" VisibleIndex="2" Visible="True" Caption="Директор" Width="160px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="tel_boss" ShowInCustomizationForm="True" VisibleIndex="3" Visible="True" Caption="Телефон Директора"></dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="district" ShowInCustomizationForm="True" VisibleIndex="4" Visible="False" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ShowInCustomizationForm="True" VisibleIndex="5" Visible="True" Caption="Назва Вулиці" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ShowInCustomizationForm="True" VisibleIndex="6" Visible="True" Caption="Номер Будинку"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_zip_code" ShowInCustomizationForm="True" VisibleIndex="7" Visible="False" Caption="Поштовий Індекс"></dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="addr_full" ShowInCustomizationForm="True" VisibleIndex="8" Visible="true" Caption="Повна Адреса" Width="230px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="certificate_num" ShowInCustomizationForm="True" VisibleIndex="9" Visible="true" Caption="Номер Сертифікату"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="certificate_date" ShowInCustomizationForm="True" VisibleIndex="10" Visible="true" Caption="Дата Видачі Сертифікату"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="certificate_end_date" ShowInCustomizationForm="True" VisibleIndex="11" Visible="true" Caption="Дата Закінчення Дії Сертифікату"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="case_num" ShowInCustomizationForm="True" VisibleIndex="11" Visible="true" Caption="Номер Справи"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="note" ShowInCustomizationForm="True" VisibleIndex="12" Visible="true" Caption="Примітка" Width="160px"></dx:GridViewDataTextColumn>
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <SettingsBehavior EnableCustomizationWindow="True" ConfirmDelete="true" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager PageSize="25"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="false"
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.Assessment.Experts" Version="A4" Enabled="false" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewAssessmentExpertsInit" EndCallback="GridViewAssessmentExpertsEndCallback" />
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

<%@ page language="C#" autoeventwireup="true" inherits="Privatization_PrivatDocList, App_Web_privatdoclist.aspx.65b2b859" masterpagefile="~/NoHeader.master" title="Розпорядчі Документи з Приватизації" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
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

    function GridViewPrivatizationInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewPrivatizationEndCallback(s, e) {

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

<mini:ProfiledSqlDataSource ID="SqlDataSourceViewPrivatization" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_privatization_doc_links">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceViewPrivatDetail" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    OnSelecting="SqlDataSourceViewPrivatDetail_Selecting" 
    SelectCommand="SELECT * FROM view_priv_object_docs WHERE master_doc_id = @p_master_doc_id AND document_id = @p_slave_doc_id" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_master_doc_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_slave_doc_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Privatization/PrivatObjects.aspx" Text="Об'єкти в Програмі Приватизації"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Privatization/PrivatDocList.aspx" Text="Розпорядчі Документи з Приватизації"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle2" runat="server" Text="Загальна інформація по приватизації" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup2" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
		<td>
			<dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_Privatization_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Privatization_SaveAs" 
                PopupElementID="ASPxButton_Privatization_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton_Privatization_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Privatization_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Privatization_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Privatization_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Privatization_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Privatization_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Privatization_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewPrivatizationExporter" runat="server" 
    FileName="Приватизація" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourceViewPrivatization"
    KeyFieldName="link_id"
    OnCustomCallback="GridViewPrivatization_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewPrivatization_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewPrivatization_ProcessColumnAutoFilter" >

    <GroupSummary><dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" /></GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn Caption="Номер Головного Документу" FieldName="parent_num" ShowInCustomizationForm="True"
            VisibleIndex="0" Visible="True" Width="80px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("master_doc_id") + ")\">" + Eval("parent_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn Caption="Дата Головного Документу" FieldName="parent_date" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="True" Width="100px"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn Caption="Дата Головного Документу - Рік" FieldName="parent_date_year" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="False" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Дата Головного Документу - Квартал" FieldName="parent_date_quarter" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="False" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Назва Головного Документу" FieldName="parent_topic" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Width="260px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("master_doc_id") + ")\">" + Eval("parent_topic") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Вид Головного Документу" FieldName="parent_kind" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="False" Width="110px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Номер Підпорядкованого Документу" FieldName="child_num" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="True" Width="110px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("slave_doc_id") + ")\">" + Eval("child_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn Caption="Дата Підпорядкованого Документу" FieldName="child_date" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="True" Width="110px"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn Caption="Дата Підпорядкованого Документу - Рік" FieldName="child_date_year" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="False" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Дата Підпорядкованого Документу - Квартал" FieldName="child_date_quarter" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="False" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Назва Підпорядкованого Документу" FieldName="child_topic" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="True" Width="260px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("slave_doc_id") + ")\">" + Eval("child_topic") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Вид Підпорядкованого Документу" FieldName="child_kind" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Width="110px"></dx:GridViewDataTextColumn>
    </Columns>

        <Templates>
        <DetailRow>
            <dx:ASPxGridView ID="GridViewPrivatizationDetail" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSourceViewPrivatDetail" KeyFieldName="master_doc_id;document_id;privatization_id"
                onbeforeperformdataselect="GridViewPrivatizationDetail_BeforePerformDataSelect" 
                Width="100%">

                <Columns>
                    <dx:GridViewDataTextColumn FieldName="subord_code" Caption="Сфера Управління" VisibleIndex="0" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="district" Caption="Район" VisibleIndex="1" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="street_full_name" Caption="Назва Вулиці" VisibleIndex="2"  Width="120px">
                        <DataItemTemplate>
                            <%# "<a href=\"javascript:ShowObjectCardForPrivatization('" + Eval("balans_id") + "," + Eval("building_id") + "')\">" + Eval("street_full_name") + "</a>"%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="addr_nomer" Caption="Номер Будинку" VisibleIndex="3" Width="80px">
                        <DataItemTemplate>
                            <%# "<a href=\"javascript:ShowObjectCardForPrivatization('" + Eval("balans_id") + "," + Eval("building_id") + "')\">" + Eval("addr_nomer") + "</a>"%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="obj_name" Caption="Назва Об'єкту" VisibleIndex="4" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="org_name" Caption="Організація" VisibleIndex="5"  Width="180px">
                        <DataItemTemplate>
                            <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("org_name") + "</a>"%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="sqr_total" Caption="Загальна Площа (кв.м.)" VisibleIndex="6" Width="70px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="obj_group" Caption="Група Об'єкту" VisibleIndex="7" Width="70px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="privat_kind" Caption="Спосіб Приватизації" VisibleIndex="8" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="privat_state" Caption="Стан" VisibleIndex="9" Width="120px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="purpose_group" Caption="Група Призначення" VisibleIndex="10" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="object_kind" Caption="Вид Об'єкту" VisibleIndex="11" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="object_type" Caption="Тип Об'єкту" VisibleIndex="12" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="obj_floor" Caption="Поверх" VisibleIndex="13" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost" Caption="Вартість (грн.)" VisibleIndex="14" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_expert" Caption="Експертна Оцінка" VisibleIndex="15" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="expert_date" Caption="Дата Експертної Оцінки" VisibleIndex="16" ></dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="object_history" Caption="Історична Цінність" VisibleIndex="17" ></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="note" Caption="Примітка" VisibleIndex="18" ></dx:GridViewDataTextColumn>
                </Columns>

                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_expert" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>

                <SettingsPager PageSize="7"></SettingsPager>
                <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" 
                    EnableCustomizationWindow="True" />
                <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" />
                <Styles Header-Wrap="True" />
                <SettingsCookies CookiesID="GUKV.Privatization.PrivatDetail" Enabled="False" Version="A2" />
            </dx:ASPxGridView>
        </DetailRow>
    </Templates>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager AlwaysShowPager="true" PageSize="25"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <SettingsDetail ShowDetailRow="True" />
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
    <SettingsCookies CookiesID="GUKV.Privatization.Documents" Version="A2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewPrivatizationInit" EndCallback="GridViewPrivatizationEndCallback" />
</dx:ASPxGridView>

</center>

<iframe id="docTextFrame" src="" style="visibility:hidden" width="0px" height="0px"></iframe>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
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
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
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


<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RishProjectTableOrgToEditor.ascx.cs" Inherits="RishProjectTableOrgToEditor" %>

<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 20 id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations WHERE
        (is_deleted IS NULL OR is_deleted = 0) AND (master_org_id IS NULL) AND (zkpo_code LIKE @zkpo) AND (full_name LIKE @fname)"
    OnSelecting="SqlDataSourceOrgSearch_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictDistricts2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_status ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgFormGosp" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_form_gosp ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_1nf_org_ownership where len(name) > 0 order by name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgIndustry" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_industry ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOccupation" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_occupation ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgVedomstvo" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_vedomstvo ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_streets ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<dx:ASPxCallbackPanel ID="CPOrgToEditor" ClientInstanceName="CPOrgToEditor" runat="server" OnCallback="CPOrgToEditor_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent3" runat="server">

<table border="0" cellspacing="4" cellpadding="0" style="padding: 20px;">
    <tr>
        <td>
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Код ЄДРПОУ:" Width="85px" />
        </td>
        <td>
            <dx:ASPxTextBox ID="EditOrgToZKPO" ClientInstanceName="EditOrgToZKPO" runat="server" Width="100px" />
        </td>
        <td>
            <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Назва:" Width="40px" />
        </td>
        <td>
            <dx:ASPxTextBox ID="EditOrgToName" ClientInstanceName="EditOrgToName" runat="server"
                Width="150px" />
        </td>
        <td>
            <dx:ASPxButton ID="ButtonFindOrgTo" ClientInstanceName="ButtonFindOrgTo" runat="server"
                AutoPostBack="False" Text="Знайти">
                <ClientSideEvents Click="function (s, e) { 
    ComboOrgTo.PerformCallback(EditOrgToZKPO.GetText() + '|' + EditOrgToName.GetText()); 
}" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="PopupAddOrgTo" runat="server" ClientInstanceName="PopupAddOrgTo"
                HeaderText="Створення Організації, до якої передається Об'єкт" PopupElementID="ButtonAddOrgTo">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">

                        <table border="0" cellspacing="0" cellpadding="2">
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Повна Назва:" Width="85px" /> </td>
                                <td colspan="3"> <dx:ASPxTextBox ID="TextBoxFullNameTo" ClientInstanceName="TextBoxFullNameTo" runat="server" Width="100%" /> </td>
                            </tr>
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Код ЄДРПОУ:" Width="85px" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxZkpoCodeTo" ClientInstanceName="TextBoxZkpoCodeTo" runat="server" Width="90px" /> </td>
                                <td> <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Коротка Назва" Width="85px" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxShortNameTo" runat="server" Width="290px" ClientInstanceName="TextBoxShortNameTo" /> </td>
                            </tr>
                        </table>
    
                        <br />
                        <table border="0" cellspacing="0" cellpadding="2">
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Статус (фіз./юр. особа)" Width="160px"/> </td>
                                <td> <dx:ASPxComboBox ID="ComboBoxStatusTo" runat="server" ClientInstanceName="ComboBoxStatusTo" 
                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                        IncrementalFilteringMode="StartsWith" 
                                        DataSourceID="SqlDataSourceDictOrgStatus" /> </td>
                            </tr>
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Форма Фінансування" Width="160px" /> </td>
                                <td> <dx:ASPxComboBox ID="ComboBoxFormGospTo" runat="server" ClientInstanceName="ComboBoxFormGospTo" 
                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                        IncrementalFilteringMode="StartsWith" 
                                        DataSourceID="SqlDataSourceDictOrgFormGosp" /></td>
                            </tr>
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Форма Власності" Width="160px" /> </td>
                                <td> <dx:ASPxComboBox ID="ComboBoxFormVlasnTo" runat="server" ClientInstanceName="ComboBoxFormVlasnTo" 
                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                        IncrementalFilteringMode="StartsWith" 
                                        DataSourceID="SqlDataSourceDictOrgOwnership" /></td>
                            </tr>
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Галузь" Width="160px" /> </td>
                                <td> <dx:ASPxComboBox ID="ComboBoxIndustryTo" runat="server" ClientInstanceName="ComboBoxIndustryTo" 
                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                        IncrementalFilteringMode="StartsWith" 
                                        DataSourceID="SqlDataSourceDictOrgIndustry" /> </td>
                            </tr>
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Вид Діяльності" Width="160px" /> </td>
                                <td> <dx:ASPxComboBox ID="ComboBoxOccupationTo" runat="server" ClientInstanceName="ComboBoxOccupationTo" 
                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                        IncrementalFilteringMode="StartsWith" 
                                        DataSourceID="SqlDataSourceDictOrgOccupation" /> </td>
                            </tr>
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Орган Управління" Width="160px" /> </td>
                                <td> <dx:ASPxComboBox ID="ComboBoxVedomstvoTo" runat="server" ClientInstanceName="ComboBoxVedomstvoTo" 
                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                        IncrementalFilteringMode="StartsWith" 
                                        DataSourceID="SqlDataSourceDictOrgVedomstvo" /> </td>
                            </tr>
                        </table>
    
                        <br />
                        <table border="0" cellspacing="0" cellpadding="2">
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="ФІО Директора" Width="95px" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxDirectorFioTo" runat="server" ClientInstanceName="TextBoxDirectorFioTo" Width="250px" /> </td>
                                <td> &nbsp; </td>
                                <td> <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Тел. Директора" Width="100px" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxDirectorPhoneTo" runat="server" ClientInstanceName="TextBoxDirectorPhoneTo" Width="100px" /> </td>
                            </tr>
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="ФІО Бухгалтера" Width="95px" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxBuhgalterFioTo" runat="server" ClientInstanceName="TextBoxBuhgalterFioTo" Width="250px" /> </td>
                                <td> &nbsp; </td>
                                <td> <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Тел. Бухгалтера" Width="100px" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxBuhgalterPhoneTo" runat="server" ClientInstanceName="TextBoxBuhgalterPhoneTo" Width="100px" /> </td>
                            </tr>
                        </table>
    
                        <br />
                        <table border="0" cellspacing="0" cellpadding="2">
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="КВЕД" Width="95px" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxKvedCodeTo" runat="server" Width="465px" ClientInstanceName="TextBoxKvedCodeTo" /> </td>
                            </tr>
                        </table>
    
                        <br />
                        <table border="0" cellspacing="0" cellpadding="2">
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Район" Width="95px" /> </td>
                                <td> <dx:ASPxComboBox ID="ComboBoxDistrictTo" runat="server" ClientInstanceName="ComboBoxDistrictTo" 
                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="120px" 
                                        IncrementalFilteringMode="StartsWith" 
                                        DataSourceID="SqlDataSourceDictDistricts2" /> </td>
                                <td> <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Назва Вулиці" Width="84px" /> </td>
                                <td colspan="3"> <dx:ASPxComboBox ID="ComboBoxStreetNameTo" runat="server" ClientInstanceName="ComboBoxStreetNameTo" 
                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" IncrementalFilteringMode="StartsWith"
                                    EnableCallbackMode="True" CallbackPageSize="50" EnableViewState="False" 
                                        EnableSynchronization="False" FilterMinLength="2" 
                                        DataSourceID="SqlDataSourceDictStreets" /> </td>
                            </tr>
                            <tr>
                                <td> <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Номер Будинку" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxAddrNomerTo" runat="server" Text="" Width="120px" ClientInstanceName="TextBoxAddrNomerTo" /> </td>
                                <td> <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Корпус" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxAddrKorpusTo" runat="server" Text="" Width="80px" ClientInstanceName="TextBoxAddrKorpusTo" /> </td>
                                <td> <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Пошт. Індекс" Width="85px" /> </td>
                                <td> <dx:ASPxTextBox ID="TextBoxAddrZipCodeTo" runat="server" Width="80px" ClientInstanceName="TextBoxAddrZipCodeTo" /> </td>
                            </tr>
                        </table>

                        <br/>
                        <dx:ASPxLabel ID="LabelOrgToCreationError" ClientInstanceName="LabelOrgToCreationError" runat="server" Text="" ClientVisible="false" ForeColor="Red" />
                        <dx:ASPxButton ID="ButtonDoAddOrgTo" ClientInstanceName="ButtonDoAddOrgTo" runat="server" AutoPostBack="False" Text="Створити">
                            <ClientSideEvents Click="function (s, e) { CPOrgToEditor.PerformCallback('create:'); }" />
                        </dx:ASPxButton>

                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ButtonAddOrgTo" ClientInstanceName="ButtonAddOrgTo" runat="server" AutoPostBack="False" Text="Створити">
                <ClientSideEvents Click="function (s, e) { TextBoxZkpoCodeTo.SetText(EditOrgToZKPO.GetText()); TextBoxFullNameTo.SetText(EditOrgToName.GetText()); }" />
            </dx:ASPxButton>
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <dx:ASPxComboBox ID="ComboOrgTo" ClientInstanceName="ComboOrgTo" runat="server"
                Width="100%" DataSourceID="SqlDataSourceOrgSearch" ValueField="id" ValueType="System.Int32"
                TextField="search_name" EnableSynchronization="True" OnCallback="ComboOrgSearch_Callback">
            </dx:ASPxComboBox>
        </td>
    </tr>
</table>

        </dx:panelcontent>
    </PanelCollection>

    <ClientSideEvents EndCallback="function (s, e) { if (!LabelOrgToCreationError.GetVisible()) { PopupAddOrgTo.Hide(); }; }" />
</dx:ASPxCallbackPanel>

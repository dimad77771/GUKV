<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgInfo.aspx.cs" Inherits="Reports1NF_OrgInfo"
    MasterPageFile="~/NoHeader.master" Title="Інформація про Організацію" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
<%@ Register src="ReportCommentViewer.ascx" tagname="ReportCommentViewer" tagprefix="uc1" %>

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

<script type="text/javascript" language="javascript">

    // <![CDATA[

    var lastFocusedControlId = "";
    var lastFocusedControlTitle = "";

    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 info.*
        ,CASE WHEN info.submit_date IS NULL OR info.modify_date IS NULL OR info.modify_date > info.submit_date THEN 0 ELSE 1 END AS 'report_org_info_status'
        ,dbo.[get_kazna_total](
	        (select Q.zkpo_code from reports1nf_org_info Q where report_id = info.report_id), 
	        null, null 
         ) AS 'kazna_total' 
        FROM reports1nf_org_info info WHERE info.report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceIndustry" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_org_industry ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOccupation" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_org_occupation ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFormGosp" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_org_form_gosp ORDER BY id">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceGospStruct" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_org_gosp_struct ORDER BY name">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_org_ownership ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgForm" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_org_form ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceVedomstvo" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_org_vedomstvo ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOldOrgan" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_org_old_organ ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_districts2 WHERE id < 900 ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceStreet" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (name is not null) and (RTRIM(LTRIM(name)) <> '') order by name">
<%--    SelectCommand="select s.id, s.name as sname, r.name as rname, ISNULL(r.name + ' - ', '') + s.name as name from dict_streets s left join dict_regions r on r.id = s.region_id where (not s.name is null) and (RTRIM(LTRIM(s.name)) <> '') ORDER BY 4">   --%><%--    SelectCommand="select id, name from dict_streets where (not name is null) and (RTRIM(LTRIM(name)) <> '')">	--%>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOtdelDKV" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dbo.dict_otdel_gukv ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT [id]
        ,[report_id]
        ,[vlasn_name]
        ,[vlasn_inn]
        ,[vlasn_zkpo]
        ,[vlasn_kopfg]
        ,[vlasn_uradr]
        ,[is_zasnobn]
        ,[kol_akcia]
        ,[total_price]
        ,[part_price]
        ,[stat_found]
        ,[form_akcia]
        ,[nominal_vart]
        ,[stan_vlasn]
        ,[primitki]
    FROM [reports1nf_org_corprava] WHERE [report_id] = @report_id" 
    DeleteCommand="DELETE FROM [reports1nf_org_corprava] where [id] = @id" 
    InsertCommand="INSERT INTO [reports1nf_org_corprava]
        ([report_id]
        ,[vlasn_name]
        ,[vlasn_inn]
        ,[vlasn_zkpo]
        ,[vlasn_kopfg]
        ,[vlasn_uradr]
        ,[is_zasnobn]
        ,[kol_akcia]
        ,[total_price]
        ,[part_price]
        ,[stat_found]
        ,[form_akcia]
        ,[nominal_vart]
        ,[stan_vlasn]
        ,[primitki]
    ) 
    VALUES
        (@report_id
        ,@vlasn_name
        ,@vlasn_inn
        ,@vlasn_zkpo
        ,@vlasn_kopfg
        ,@vlasn_uradr
        ,@is_zasnobn
        ,@kol_akcia
        ,@total_price
        ,@part_price
        ,@stat_found
        ,@form_akcia
        ,@nominal_vart
        ,@stan_vlasn
        ,@primitki
    );
SELECT SCOPE_IDENTITY()" 
    UpdateCommand="UPDATE [reports1nf_org_corprava]
SET
    [report_id] = @report_id,
    [vlasn_name] = @vlasn_name,
    [vlasn_inn] = @vlasn_inn,
    [vlasn_zkpo] = @vlasn_zkpo,
    [vlasn_kopfg] = @vlasn_kopfg,
    [vlasn_uradr] = @vlasn_uradr,
    [is_zasnobn] = @is_zasnobn,
    [kol_akcia] = @kol_akcia,
    [total_price] = @total_price,
    [part_price] = @part_price,
    [stat_found] = @stat_found,
    [form_akcia] = @form_akcia,
    [nominal_vart] = @nominal_vart,
    [stan_vlasn] = @stan_vlasn,
    [primitki] = @primitki 
WHERE id = @id" 
        oninserting="SqlDataSourceFreeSquare_Inserting" 
        onupdating="SqlDataSourceFreeSquare_Updating" ProviderName="System.Data.SqlClient">
    <SelectParameters>
        <asp:Parameter Name="report_id" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="id" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="report_id" />
        <asp:Parameter Name="vlasn_name" />
        <asp:Parameter Name="vlasn_inn" />
        <asp:Parameter Name="vlasn_zkpo" />
        <asp:Parameter Name="vlasn_kopfg" />
        <asp:Parameter Name="vlasn_uradr" />
        <asp:Parameter Name="is_zasnobn" />
        <asp:Parameter Name="kol_akcia" />
        <asp:Parameter Name="total_price" />
        <asp:Parameter Name="part_price" />
        <asp:Parameter Name="stat_found" />
        <asp:Parameter Name="form_akcia" />
        <asp:Parameter Name="nominal_vart" />
        <asp:Parameter Name="stan_vlasn" />
        <asp:Parameter Name="primitki" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="report_id" />
        <asp:Parameter Name="vlasn_name" />
        <asp:Parameter Name="vlasn_inn" />
        <asp:Parameter Name="vlasn_zkpo" />
        <asp:Parameter Name="vlasn_kopfg" />
        <asp:Parameter Name="vlasn_uradr" />
        <asp:Parameter Name="is_zasnobn" />
        <asp:Parameter Name="kol_akcia" />
        <asp:Parameter Name="total_price" />
        <asp:Parameter Name="part_price" />
        <asp:Parameter Name="stat_found" />
        <asp:Parameter Name="form_akcia" />
        <asp:Parameter Name="nominal_vart" />
        <asp:Parameter Name="stan_vlasn" />
        <asp:Parameter Name="primitki" />
        <asp:Parameter Name="id" />
    </UpdateParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="true">
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
    <asp:Label runat="server" ID="ASPxLabel19" Text="Загальна інформація про Організацію, що подає Звіт" CssClass="pagetitle"/>
</p>

<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" runat="server" OnCallback="CPMainPanel_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent2" runat="server">

<asp:FormView runat="server" BorderStyle="None" ID="OrgDetails" DataSourceID="SqlDataSourceOrgProperties" EnableViewState="False">
    <ItemTemplate>

<dx:ASPxLabel ID="LabelNotSubmitted" runat="server" Text="Необхідно надіслати внесені зміни до ДКВ" ClientVisible='<%# 0.Equals(Eval("report_org_info_status")) %>' ForeColor="Red" />
<dx:ASPxLabel ID="LabelAllSubmitted" runat="server" Text="Зміни надіслано до ДКВ" ClientVisible='<%# 1.Equals(Eval("report_org_info_status")) %>' />
<dx:ASPxLabel ID="LabelPlaceholder" runat="server" Text="&nbsp;" ClientVisible="false" />

<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server">
    <TabPages>

        <dx:TabPage Text="Відомості про організацію" Name="Tab1">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server">

        <dx:ASPxRoundPanel ID="PanelBasics" runat="server" HeaderText="Базові відомості про Організацію, що подає Звіт">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server" >
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Код ЄДРПОУ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="EditZKPO" runat="server" Text='<%# Eval("zkpo_code") %>' Width="290px" ReadOnly="true" Title="Код ЄДРПОУ Організації" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Повна Назва"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="EditFullName" runat="server" Text='<%# Eval("full_name") %>' Width="700px" Title="Повна Назва Організації" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Скорочена Назва"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="EditShortName" runat="server" Text='<%# Eval("short_name") %>' Width="700px" Title="Скорочена Назва Організації" MaxLength="100" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelAddr" runat="server" HeaderText="Юридична адреса">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" >
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Район"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboAddrDistrict" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDistrict" Value='<%# Eval("addr_distr_new_id") %>'
                                     Title="Юридична адреса - Район">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Юридична адреса: необхідно вибрати район" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Назва Вулиці"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboAddrStreet" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceStreet" Value='<%# Eval("addr_street_id") %>'
                                    Title="Юридична адреса - Назва вулиці">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Юридична адреса: необхідно вибрати вулицю" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Номер Будинку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxTextBox ID="EditAddrNum" runat="server" Text='<%# Eval("addr_nomer") %>' Width="290px" Title="Юридична адреса - Номер будинку" MaxLength="30">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Юридична адреса: необхідно заповнити номер будинку" /> </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Поштовий Індекс"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditAddrZipCode" runat="server" Text='<%# Eval("addr_zip_code") %>' Width="290px" Title="Юридична адреса - Поштовий індекс" MaxLength="18" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Додаткова Адреса"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="EditAddrMisc" runat="server" Text='<%# Eval("addr_misc") %>' Width="100%" Title="Юридична адреса - Додаткова адреса" MaxLength="150" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelPhysAddr" runat="server" HeaderText="Поштова адреса">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent5" runat="server" >
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Район"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboPhysAddrDistrict" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDistrict" Value='<%# Eval("phys_addr_district_id") %>'
                                    Title="Поштова адреса - Район">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Фізична адреса: необхідно вибрати район" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Назва Вулиці"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboPhysAddrStreet" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceStreet" Value='<%# Eval("phys_addr_street_id") %>'
                                    Title="Поштова адреса - Назва вулиці">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Фізична адреса: необхідно вибрати вулицю" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Номер Будинку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxTextBox ID="EditPhysAddrNum" runat="server" Text='<%# Eval("phys_addr_nomer") %>' Width="290px" Title="Поштова адреса - Номер будинку" MaxLength="60">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Фізична адреса: необхідно заповнити номер будинку" /> </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Поштовий Індекс"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditPhysAddrZipCode" runat="server" Text='<%# Eval("phys_addr_zip_code") %>' Width="290px" Title="Поштова адреса - Поштовий індекс" MaxLength="24" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Додаткова Адреса"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="EditPhysAddrMisc" runat="server" Text='<%# Eval("phys_addr_misc") %>' Width="100%" Title="Поштова адреса - Додаткова адреса" MaxLength="150" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelContacts" runat="server" HeaderText="Контактна Інформація">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" >
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="ПІБ Керівника"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditDirectorName" runat="server" Text='<%# Eval("director_fio") %>' Width="290px" Title="ПІБ Керівника" MaxLength="70" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Ел. Адреса Керівника"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditDirectorEmail" runat="server" Text='<%# Eval("director_email") %>' Width="290px" Title="Електронна Адреса Керівника" MaxLength="100" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Тел. Керівника"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditDirectorPhone" runat="server" Text='<%# Eval("director_phone") %>' Width="290px" Title="Телефон Керівника" MaxLength="70" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="ПІБ Бухгалтера"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditBuhgalterName" runat="server" Text='<%# Eval("buhgalter_fio") %>' Width="290px" Title="ПІБ Бухгалтера" MaxLength="70" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px">&nbsp;</td>
                            <td width="8px">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Ел. Адреса Бухгалтера"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditBuhgalterEmail" runat="server" Text='<%# Eval("buhgalter_email") %>' Width="290px" Title="Електронна Адреса Бухгалтера" MaxLength="100" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Тел. Бухгалтера"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditBuhgalterPhone" runat="server" Text='<%# Eval("buhgalter_phone") %>' Width="290px" Title="Телефон Бухгалтера" MaxLength="23" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel38" runat="server" Text="Поточні рахунки у відділеннях банку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="EditBankRahunok" runat="server" Text='<%# Eval("bank_rahunok") %>' Width="100%" Title="Поточні рахунки у відділеннях банку" MaxLength="255" /></td>
                        </tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Відповідальна особа (звіт)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="EditDirectorTitle" runat="server" Text='<%# Eval("director_title") %>' Width="100%" Title="ПІБ та телефон відповідальної особи" MaxLength="500" /></td>
                        </tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel44" runat="server" Text="Відповідальна особа (prozoro)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="EditProzoroTitle" runat="server" Text='<%# Eval("prozoro_title") %>' Width="100%" Title="ПІБ та телефон відповідальної особи" MaxLength="500" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelInfo" runat="server" HeaderText="Додаткові відомості">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server" >
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Форма Власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboOwnership" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceOwnership" Value='<%# Eval("form_ownership_id") %>'
                                    Title="Форма Власності Організації">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Необхідно вибрати форму власності організації" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Форма фінансування"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboFormGosp" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceFormGosp" Value='<%# Eval("form_gosp_id") %>'
                                    Title="Форма Фінансування Організації">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Необхідно вибрати форму фінансування організації" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Галузь"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboIndustry" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceIndustry" Value='<%# Eval("industry_id") %>'
                                    Title="Галузь Організації">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Необхідно вибрати галузь" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Вид Діяльності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboOccupation" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceOccupation" Value='<%# Eval("occupation_id") %>'
                                    Title="Вид Діяльності Організації">
                                    <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Необхідно вибрати вид діяльності організації" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="КВЕД"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxTextBox ID="EditKVED" runat="server" Text='<%# Eval("kved_code") %>' Width="290px" Title="Код КВЕД Організації" MaxLength="7" />
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Орг.-правова форма госп."></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboOrgForm" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceOrgForm" Value='<%# Eval("form_id") %>'
                                    Title="Організаційно-правова форма господарювання" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Орган управління"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboVedomstvo" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceVedomstvo" Value='<%# Eval("vedomstvo_id") %>'
                                    Title="Орган Управління Організації" />
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Орган госп. упр."></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboOldOrgan" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceOldOrgan" Value='<%# Eval("old_organ_id") %>'
                                    Title="Орган Господарського Управління Організації" />
                            </td>
                         </tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Ставка відрахувань до бюджету (%)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="contribution_rate" runat="server" DropDownStyle="DropDown" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                     Value='<%# Eval("contribution_rate") %>' Title="Ставка відрахувань до бюджету">
                                        <Items>
                                            <dx:ListEditItem Value="" Text="" />
                                            <dx:ListEditItem Value="5" Text="5" />
                                            <dx:ListEditItem Value="25" Text="25" />
                                            <dx:ListEditItem Value="30" Text="30" />
                                            <dx:ListEditItem Value="50" Text="50" />
											<dx:ListEditItem Value="100" Text="100" />
                                        </Items>                                        
                                    </dx:ASPxComboBox>
                            </td>

                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Господарська структура"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboStructType" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceGospStruct" Value='<%# Eval("gosp_struct_id") %>'
                                    Title="Господарська структура" />
                            </td>

                            <td width="8px">&nbsp;</td>
                            <td width="100px"></td>
                            <td width="8px">&nbsp;</td>
                            <td>                            
                            </td>
                        </tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Стан юр.особи"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboOtdelType" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="290px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceOtdelDKV" Value='<%# Eval("otdel_gukv_id") %>'
                                    Title="Стан юр.особи" />
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Примітки балансоутримувача"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxTextBox ID="edit_prim_balanc" runat="server" Text='<%# Eval("prim_balanc") %>' Width="100%" Title="Примітки балансоутримувача" MaxLength="255" />
                           </td>
                        </tr>

                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel45" runat="server" Text="Корпоратині права"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxSpinEdit ID="EditCorporav_prava" runat="server" NumberType="Float" DecimalPlaces="2" Value='<%# Eval("corporav_prava") %>' Width="290px" 
                                        Title="Корпоратині права"/>                                
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px">&nbsp;</td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                &nbsp;
                           </td>
                        </tr>


                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Перерахування  до бюджету та планування" Name="Tab2">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

        <dx:ASPxRoundPanel ID="PanelPayments" runat="server" HeaderText="Перераховання до бюджету з надходжень від орендної плати">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent10" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="LabelBudgetNarah50UAH" runat="server" Text="Нарахована сума до бюджету 50% від загальної суми надходжень орендної плати за звітний період, грн. (без ПДВ)" Width="650px"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditBudgetNarah50_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("budget_narah_50_uah") %>' Width="150px"
                                Title="Нарахована сума до бюджету 50% від загальної суми надходжень орендної плати"/></td>
                        </tr>

                        <tr>
                            <td><dx:ASPxLabel ID="LabelKaznaNarah50UAH" runat="server" Text="Перераховано коштів до бюджету, у звітному періоді ″КАЗНАЧЕЙСТВО″, грн. (без ПДВ)" Width="650px" ForeColor="Blue"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditBudgetKazna50_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("kazna_total") %>' Width="150px" ReadOnly="true"
                                Title="Перераховано коштів до бюджету, у звітному періоді ″КАЗНАЧЕЙСТВО″"/></td>
                        </tr>

                        <%--<tr>
                            <td><dx:ASPxLabel ID="LabelZvitNarah50UAH" runat="server" Text="Перераховано до бюджету 50% за звітний період всього з 1 січня поточного року, без переплат, грн. (без ПДВ)"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditBudgetZvit50_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("budget_zvit_50_uah") %>' Width="150px"
                                Title="Перераховано до бюджету 50% за звітний період всього з 1 січня поточного року"/></td>
                        </tr>--%>
                        <tr>
                            <td><dx:ASPxLabel ID="LabelBudgetPrev50UAH" runat="server" Text="- в тому числі перераховано до бюджету 50% боргів у звітному періоді з 1 січня поточного року за попередні роки, грн. (без ПДВ)"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditBudgetPrev50_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("budget_prev_50_uah") %>' Width="150px"
                                Title="Перераховано до бюджету 50% боргів у звітному періоді з 1 січня поточного року за попередні роки"/></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="LabelBudgetDebt50UAH" runat="server" Text="Заборгованість зі сплати 50% до бюджету від оренди майна минулих років, грн. (без ПДВ)"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditBudgetDebtOld50_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("budget_debt_30_50_uah") %>' Width="150px"
                                Title="Заборгованість зі сплати 50% до бюджету від оренди майна минулих років"/></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dx:ASPxCheckBox ID="CheckIsSpecialOrganization" ClientInstanceName="CheckIsSpecialOrganization" runat="server" Text="Балансоутримувач Київенерго, Водоканал та інші"
                                    Checked='<%# 1.Equals(Eval("is_special_organization")) %>'
                                    Title="Балансоутримувач Київенерго, Водоканал та інші">
                                    <ClientSideEvents CheckedChanged="function (s,e) { EditPaymentSpecial.SetEnabled(CheckIsSpecialOrganization.GetChecked()); }" />
                                </dx:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Перераховано до бюджету за користування комунальним майном, грн. (без ПДВ) (тільки для 'Київенерго' та 'Водоканал')" Width="650px"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxSpinEdit ID="EditPaymentSpecial_orndpymnt" ClientInstanceName="EditPaymentSpecial" runat="server" NumberType="Float" Value='<%# Eval("payment_budget_special") %>' Width="150px"
                                    Title="Перераховано до бюджету за користування комунальним майном">
                                    <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Необхідно вказати, скільки перераховано до бюджету за користування комунальним майном" /> </ValidationSettings>
                                </dx:ASPxSpinEdit>
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>                                
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelUnknownPayments" runat="server" HeaderText="Інші платежі">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent7" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel32" runat="server" Text="Авансові платежі за участь у конкурсі, грн." Width="650px"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditKonkursPayments_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("konkurs_payments") %>' Width="150px"
                                Title="Авансові платежі за участь у конкурс"/></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel33" runat="server" Text="Сальдо платежів до бюджету (переплата на початок року), грн."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditUnknownPayments_orndpymnt" runat="server" NumberType="Float" Value='<%# Eval("unknown_payments") %>' Width="150px"
                                Title="Сальдо платежів до бюджету, грн."/></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Коментар щодо сальдо платежів до бюджету"></dx:ASPxLabel>
                                <dx:ASPxMemo ID="MemoUnknownPayments_orndpymnt" runat="server" Text='<%# Eval("unknown_payment_note") %>' Width="100%" Height="80px"
                                    Title="Коментар щодо сальдо платежів до бюджету" MaxLength="510" />
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>                                
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelPlanuvania" runat="server" HeaderText="Планування орендної плати">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent9" runat="server">
                    <table border="0" cellspacing="0" cellpadding="2" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel39" runat="server" Text="Прогнозовані надходження за місяць, грн." Width="650px"></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditPlanuvania_1" runat="server" NumberType="Float" Value='<%# Eval("planuvania_1") %>' Width="150px" ReadOnly="true"
                                Title="Прогнозовані надходження за місяць, грн."/></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel40" runat="server" Text="Прогнозовані надходження за рік, грн."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditPlanuvania_2" runat="server" NumberType="Float" Value='<%# Eval("planuvania_2") %>' Width="150px" ReadOnly="true"
                                Title="Прогнозовані надходження за рік, грн."/></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel41" runat="server" Text="Прогнозовані надходження  до бюджету за рік, грн."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditPlanuvania_3" runat="server" NumberType="Float" Value='<%# Eval("planuvania_3") %>' Width="150px" ReadOnly="true"
                                Title="Прогнозовані надходження  до бюджету за рік, грн."/></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel42" runat="server" Text="Планові надходження орендної плати за рік згідно фін. плану, грн."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditPlanuvania_4" runat="server" NumberType="Float" Value='<%# Eval("planuvania_4") %>' Width="150px"
                                Title="Планові надходження орендної плати за рік згідно фін. плану, грн."/></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel43" runat="server" Text="Планові надходження орендної плати до бюджету за рік згідно фін. плану, грн."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditPlanuvania_5" runat="server" NumberType="Float" Value='<%# Eval("planuvania_5") %>' Width="150px"
                                Title="Планові надходження орендної плати до бюджету за рік згідно фін. плану, грн."/></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>                                
        </dx:ASPxRoundPanel>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Відомості про корпоративні права" Name="Tab4" Visible="false">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3a" runat="server">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Відомості про корпоративні права">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent8" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                            <tr>
                                                <td>
                                                

    <dx:ASPxGridView ID="ASPxGridViewFreeSquare" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceFreeSquare" KeyFieldName="id" OnRowValidating="ASPxGridViewFreeSquare_RowValidating" OnStartRowEditing="ASPxGridViewFreeSquare_StartRowEditing"
            ClientInstanceName="grid" oninitnewrow="ASPxGridViewFreeSquare_InitNewRow" >
        <Styles>  
            <EditForm CssClass="editForm999" ></EditForm>  
        </Styles>  

	    <SettingsCommandButton>
		    <EditButton>
			    <Image Url="~/Styles/EditIcon.png" />
		    </EditButton>
		    <CancelButton>
			    <Image Url="~/Styles/CancelIcon.png" />
		    </CancelButton>
		    <UpdateButton>
			    <Image Url="~/Styles/SaveIcon.png" />
		    </UpdateButton>
		    <DeleteButton>
			    <Image Url="~/Styles/DeleteIcon.png" />
		    </DeleteButton>
		    <NewButton>
			    <Image Url="~/Styles/AddIcon.png" />
		    </NewButton>
		    <ClearFilterButton Text="Очистити" RenderMode="Link" />
	    </SettingsCommandButton>

        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="True" Width="70px" 
                ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowClearFilterButton="true" ShowEditButton="true" ShowNewButton="true" >
                <CustomButtons>
                </CustomButtons>
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="id" Caption="ID" VisibleIndex="1"  ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="report_id" Caption="report_id" VisibleIndex="2" ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="primitki" Caption="primitki" VisibleIndex="2" ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>


            <dx:GridViewDataTextColumn FieldName="vlasn_name" Caption="Назва/ФІО власника корпправа" VisibleIndex="3" Width="250" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="vlasn_zkpo" Caption="ЕДРПОУ/ІНН власника корпправа" VisibleIndex="3" Width="85" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="vlasn_kopfg" Caption="КОПФГ власника корпправа" VisibleIndex="3" Width="70" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="vlasn_uradr" Caption="Юр. Адреса власника корпправа" VisibleIndex="3" Width="250" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataCheckColumn FieldName="is_zasnobn" Caption="Є засновником" VisibleIndex="3" Width="40">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataCheckColumn>

            <dx:GridViewDataTextColumn FieldName="kol_akcia" Caption="Кількість акцій у власника, шт." VisibleIndex="3" Width="100" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="total_price" Caption="Частка власника, грн." VisibleIndex="3" Width="100" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="part_price" Caption="Частка власника, %" VisibleIndex="3" Width="80" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="stat_found" Caption="СК юр. особи, грн." VisibleIndex="3" Width="100" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="form_akcia" Caption="Форма існування акцій" VisibleIndex="3" Width="100" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="nominal_vart" Caption="Номінальна вартість акцій, грн." VisibleIndex="3" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="stan_vlasn" Caption="Стан реєстрації об'єкту корпоративної власності" VisibleIndex="3" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

        </Columns>
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsPager PageSize="10" />
        <SettingsEditing NewItemRowPosition="Bottom" />
        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" VerticalScrollableHeight="0" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard"/>
    </dx:ASPxGridView>

                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
<%--                        </ItemTemplate>
                    </asp:FormView>--%>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

    </TabPages>
</dx:ASPxPageControl>

    </ItemTemplate>
</asp:FormView>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<p class="SpacingPara"/>

<dx:ASPxValidationSummary ID="ValidationSummary" runat="server" RenderMode="BulletedList" Width="800px" ClientInstanceName="ValidationSummary">
</dx:ASPxValidationSummary>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('save:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonSend" runat="server" Text="Надіслати" AutoPostBack="false">
                <ClientSideEvents Click="function (s,e) { if (ASPxClientEdit.ValidateEditorsInContainer(null)) { CPMainPanel.PerformCallback('send:'); } }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonClear" runat="server" Text="Відмінити зміни" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonComments" runat="server" Text="Коментарі" CausesValidation="false" AutoPostBack="false" />

            <dx:ASPxPopupControl ID="PopupComments" runat="server" 
                HeaderText="Коментарі" 
                ClientInstanceName="PopupComments" 
                PopupElementID="ButtonComments" >
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">

                        <dx:ASPxCallbackPanel ID="CPCommentViewerPanel" ClientInstanceName="CPCommentViewerPanel" runat="server" OnCallback="CPCommentViewerPanel_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent6" runat="server">

                                    <uc1:ReportCommentViewer ID="ReportCommentViewer1" runat="server"/>

                                </dx:panelcontent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents
                    PopUp="function (s,e) { CPCommentViewerPanel.PerformCallback('' + lastFocusedControlId + ';' + lastFocusedControlTitle); }"
                    CloseUp="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
            </dx:ASPxPopupControl>
        </td>
    </tr>
</table>

</asp:Content>

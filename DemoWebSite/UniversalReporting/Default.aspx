<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="UniversalReporting_Default" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v12.1, Version=12.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v12.1, Version=12.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v12.1, Version=12.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v12.1.Export, Version=12.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <script type="text/javascript" language="javascript">

        // <![CDATA[

        function ButtonObjReportFieldChooserClick(s, e) {

            if (ASPxGridViewBuildings.IsCustomizationWindowVisible())
                ASPxGridViewBuildings.HideCustomizationWindow();
            else
                ASPxGridViewBuildings.ShowCustomizationWindow();
        }

        function ButtonBalansReportFieldChooserClick(s, e) {

            if (ASPxGridViewBalans.IsCustomizationWindowVisible())
                ASPxGridViewBalans.HideCustomizationWindow();
            else
                ASPxGridViewBalans.ShowCustomizationWindow();
        }

        function ButtonArendaReportFieldChooserClick(s, e) {

            if (ASPxGridViewArenda.IsCustomizationWindowVisible())
                ASPxGridViewArenda.HideCustomizationWindow();
            else
                ASPxGridViewArenda.ShowCustomizationWindow();
        }

        function ButtonZvitReportFieldChooserClick(s, e) {

            if (ASPxGridViewZvit.IsCustomizationWindowVisible())
                ASPxGridViewZvit.HideCustomizationWindow();
            else
                ASPxGridViewZvit.ShowCustomizationWindow();
        }

        // ]]>

    </script>

    <p>
        <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0">
            <TabPages>
                <dx:TabPage Text="Будинки">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">

<%-- Content of the first tab BEGIN --%>

                            <table border="0" cellspacing="4" cellpadding="0" width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <h1 style="padding: 0; margin: 0;">Загальна інформація по будинкам та об'єктам</h1>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
                                            Text="Додаткові Колонки" Width="148px">
                                            <ClientSideEvents Click="ButtonObjReportFieldChooserClick" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                    <td>
                                        <dx:ASPxPopupControl ID="ASPxPopupControl_Buildings_SaveAs" runat="server" 
                                            HeaderText="Збереження у Файлі" 
                                            ClientInstanceName="ASPxPopupControl_Buildings_SaveAs" 
                                            PopupElementID="ASPxButton_Buildings_SaveAs">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxButton ID="ASPxButton_Buildings_ExportXLS" runat="server" 
                                                        Text="XLS - Microsoft Excel&reg;" 
                                                        OnClick="ASPxButton_Buildings_ExportXLS_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton_Buildings_ExportPDF" runat="server" 
                                                        Text="PDF - Adobe Acrobat&reg;" 
                                                        OnClick="ASPxButton_Buildings_ExportPDF_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton_Buildings_ExportCSV" runat="server" 
                                                        Text="CSV - значення, розділені комами" 
                                                        OnClick="ASPxButton_Buildings_ExportCSV_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>

                                        <dx:ASPxButton ID="ASPxButton_Buildings_SaveAs" runat="server" AutoPostBack="False" 
                                            Text="Зберегти у Файлі" Width="148px">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>

                            <asp:SqlDataSource ID="SqlDataSourceBuildings" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                                
                                SelectCommand="SELECT [building_id], [street_full_name], [district], [addr_nomer],
                                               [addr_zip_code], [condition], [num_floors], [construct_year], [bti_code],
                                               [history], [object_type], [object_kind], [cost_balans], [sqr_total], [sqr_habit],
                                               [sqr_non_habit], [additional_info], [oatuu_code], [facade] FROM [view_buildings]">
                            </asp:SqlDataSource>

                            <dx:ASPxGridViewExporter ID="ASPxGridViewExporterBuildings" runat="server" 
                                FileName="Будівлі" GridViewID="ASPxGridViewBuildings" PaperKind="A4" 
                                BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
                                <Styles>
                                    <Default Font-Names="Calibri,Verdana,Sans Serif">
                                    </Default>
                                    <AlternatingRowCell BackColor="#E0E0E0">
                                    </AlternatingRowCell>
                                </Styles>
                            </dx:ASPxGridViewExporter>

                            <dx:ASPxGridView ID="ASPxGridViewBuildings" runat="server" 
                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceBuildings" 
                                KeyFieldName="building_id" Width="1200px" 
                                ClientInstanceName="ASPxGridViewBuildings">
                                <GroupSummary>
                                    <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                                </GroupSummary>

                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="building_id" ReadOnly="True" 
                                        ShowInCustomizationForm="True" VisibleIndex="0" Visible="False" 
                                        Caption="ID">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="street_full_name" 
                                        ShowInCustomizationForm="True" VisibleIndex="1" Caption="Вулиця">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="district" ShowInCustomizationForm="True" 
                                        VisibleIndex="2" Caption="Район">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" 
                                        ShowInCustomizationForm="True" VisibleIndex="3" Caption="Номер">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="addr_zip_code" 
                                        ShowInCustomizationForm="True" VisibleIndex="4" Caption="Пошт. Індекс">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="condition" ShowInCustomizationForm="True" 
                                        VisibleIndex="5" Caption="Стан">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="num_floors" 
                                        ShowInCustomizationForm="True" VisibleIndex="6" Caption="Кількість поверхів">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="construct_year" 
                                        ShowInCustomizationForm="True" VisibleIndex="7" Caption="Побудований">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="bti_code" ShowInCustomizationForm="True" 
                                        VisibleIndex="8" Caption="Код БТІ">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="history" ShowInCustomizationForm="True" 
                                        VisibleIndex="9" Caption="Історична Цінність">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="object_type" 
                                        ShowInCustomizationForm="True" VisibleIndex="10" Caption="Тип об'єкту">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="object_kind" 
                                        ShowInCustomizationForm="True" VisibleIndex="11" Caption="Вид об'єкту">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="cost_balans" 
                                        ShowInCustomizationForm="True" VisibleIndex="12" 
                                        Caption="Балансова Вартість">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="sqr_total" ShowInCustomizationForm="True" 
                                        VisibleIndex="13" Caption="Загальна Площа">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="sqr_habit" ShowInCustomizationForm="True" 
                                        VisibleIndex="14" Caption="Житлова Площа">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="sqr_non_habit" 
                                        ShowInCustomizationForm="True" VisibleIndex="15" Caption="Нежитлова Площа">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="additional_info" 
                                        ShowInCustomizationForm="True" VisibleIndex="16" 
                                        Caption="Додаткова Інформація">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="oatuu_code" 
                                        ShowInCustomizationForm="True" VisibleIndex="17" Caption="ОАТУУ">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="facade" ShowInCustomizationForm="True" 
                                        VisibleIndex="18" Caption="Фасадність">
                                    </dx:GridViewDataTextColumn>
                                </Columns>

                                <SettingsBehavior EnableCustomizationWindow="True" 
                                    AutoFilterRowInputDelay="500" ColumnResizeMode="Control" />
                                <SettingsPager PageSize="18">
                                </SettingsPager>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" 
                                    ShowFilterBar="Auto" ShowHeaderFilterButton="True" 
                                    ShowHorizontalScrollBar="True" />
                                <SettingsCookies CookiesID="GUKV.buildings" Version="1" Enabled="True" />
                                <SettingsDetail ShowDetailRow="True" />
                                <Styles Header-Wrap="True" />
                                <Templates>
                                    <DetailRow>
                                        <dx:ASPxGridView ID="ASPxGridViewObjects" runat="server" AutoGenerateColumns="False" 
                                            DataSourceID="SqlDataSourceBuildingDetail" 
                                            onbeforeperformdataselect="ASPxGridViewObjects_BeforePerformDataSelect" 
                                            Width="1140px">

                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="full_name" VisibleIndex="0" 
                                                    Caption="Повне Ім'я">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="short_name" VisibleIndex="1" 
                                                    Caption="Ім'я" Visible="False">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="zkpo_code" VisibleIndex="2" 
                                                    Caption="Код ЄДРПОУ">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="occupation" VisibleIndex="3" 
                                                    Caption="Вид Діяльності">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="status" VisibleIndex="4" 
                                                    Caption="Юр./Фіз.">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="form_gosp" VisibleIndex="5" 
                                                    Caption="Форма Господарювання">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="form_of_ownership" VisibleIndex="6" 
                                                    Caption="Форма Власності">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="gosp_struct" VisibleIndex="7" 
                                                    Caption="Госп. Структура">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="organ" VisibleIndex="8" Caption="Орган">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="industry" VisibleIndex="9" 
                                                    Caption="Галузь">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="priznak" VisibleIndex="10" 
                                                    Caption="Признак">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="vedomstvo" VisibleIndex="11" 
                                                    Caption="Відомча Належність">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="title" ReadOnly="True" VisibleIndex="12" 
                                                    Caption="Прикметник">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="org_form" VisibleIndex="13" 
                                                    Caption="Орг. Форма">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="gosp_struct_type" VisibleIndex="14" 
                                                    Caption="Вид Госп. Структури">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="share_type" VisibleIndex="15" 
                                                    Caption="Вид Акцій">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_city" VisibleIndex="16" 
                                                    Caption="Місто" Visible="False">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_district" VisibleIndex="17" 
                                                    Caption="Район" Visible="False">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_street_name" VisibleIndex="18" 
                                                    Caption="Юр. Адреса - Вулиця">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="19" 
                                                    Caption="Юр. Адреса - Номер">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_korpus" VisibleIndex="20" 
                                                    Caption="Юр. Адреса - Корпус">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_zip_code" VisibleIndex="21" 
                                                    Caption="Юр. Адреса - Індекс">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="director_fio" VisibleIndex="22" 
                                                    Caption="ПІБ Керівника">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="director_phone" VisibleIndex="23" 
                                                    Caption="Телефон Керівника">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="buhgalter_fio" VisibleIndex="24" 
                                                    Caption="ПІБ Головбуха">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="buhgalter_phone" VisibleIndex="25" 
                                                    Caption="Телефон Головбуха">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="num_buildings" VisibleIndex="26" 
                                                    Caption="Кількість Будинків на Балансі">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="fax" VisibleIndex="27" Caption="Факс">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="registration_auth" VisibleIndex="28" 
                                                    Caption="Реєстр. Орган">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="registration_num" VisibleIndex="29" 
                                                    Caption="Номер Реєстрації">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="registration_date" VisibleIndex="30" 
                                                    Caption="Дата Реєстрації">
                                                </dx:GridViewDataDateColumn>
                                                <dx:GridViewDataTextColumn FieldName="registration_svidot" VisibleIndex="31" 
                                                    Caption="Номер Свідоцтва">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_on_balance" VisibleIndex="32" 
                                                    Caption="Площа на Балансі">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_manufact" VisibleIndex="33" 
                                                    Caption="Площа Виробничого Призначення">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_non_manufact" VisibleIndex="34" 
                                                    Caption="Площа Невиробничого Призначення">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_free_for_rent" VisibleIndex="35" 
                                                    Caption="Вільна Площа">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="36" 
                                                    Caption="Загальна Площа">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_rented" VisibleIndex="37" 
                                                    Caption="Площа що Орендується">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_privat" VisibleIndex="38" 
                                                    Caption="Приватизована Площа">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_given_for_rent" VisibleIndex="39" 
                                                    Caption="Площа Надана в Оренду">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_znyata_z_balansu" VisibleIndex="40" 
                                                    Caption="Площа Знята з Балансу">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_prodaj" VisibleIndex="41" 
                                                    Caption="Площа Продажу">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_spisani_zneseni" VisibleIndex="42" 
                                                    Caption="Списані та Знесені Площі">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_peredana" VisibleIndex="43" 
                                                    Caption="Площі Передані Іншим Юр. Особам">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="num_objects" VisibleIndex="44" 
                                                    Caption="Кількість Об'єктів">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="kved_code" VisibleIndex="45" 
                                                    Caption="КВЕД">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="koatuu" VisibleIndex="46" Caption="АТУУ">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="otdel_gukv" VisibleIndex="47" 
                                                    Caption="Відділ ГУКВ" Visible="False">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="building_id" VisibleIndex="62" 
                                                    Caption="ID Будинка" Visible="False">
                                                </dx:GridViewDataTextColumn>
                                            </Columns>

                                            <SettingsBehavior AutoFilterRowInputDelay="500" ColumnResizeMode="Control" 
                                                EnableCustomizationWindow="True" />
                                            <Settings ShowHorizontalScrollBar="True" />
                                            <Styles Header-Wrap="True" />
                                            <SettingsCookies CookiesID="GUKV.Buildings.BalanceHolders" Enabled="True" 
                                                Version="1" />
                                        </dx:ASPxGridView>

                                        <asp:SqlDataSource ID="SqlDataSourceBuildingDetail" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                                            onselecting="SqlDataSourceBuildingDetail_Selecting" 
                                            SelectCommand="SELECT full_name, short_name, zkpo_code, occupation, status, form_gosp,
                                                           form_of_ownership, gosp_struct, organ, industry, priznak, vedomstvo,
                                                           title, org_form, gosp_struct_type, share_type, addr_city, addr_district,
                                                           addr_street_name, addr_nomer, addr_korpus, addr_zip_code, director_fio,
                                                           director_phone, buhgalter_fio, buhgalter_phone, num_buildings, fax,
                                                           registration_auth, registration_num, registration_date, registration_svidot,
                                                           sqr_on_balance, sqr_manufact, sqr_non_manufact, sqr_free_for_rent, sqr_total,
                                                           sqr_rented, sqr_privat, sqr_given_for_rent, sqr_znyata_z_balansu, sqr_prodaj,
                                                           sqr_spisani_zneseni, sqr_peredana, num_objects, kved_code, koatuu, otdel_gukv,
                                                           mayno, is_liquidated, liquidation_date, contact_email, contact_posada,
                                                           nadhodjennya, vibuttya, privat_status, cur_state, sfera_upr, plan_zone,
                                                           registr_org, nadhodjennya_date, vibuttya_date, building_id, building_street,
                                                           building_nomer, building_district, building_zip_code, building_condition,
                                                           building_bti_code, building_history, building_type, building_kind,
                                                           building_sqr_total, building_facade
                                                           FROM report_balans WHERE (building_id = @building_id)">
                                            <SelectParameters>
                                                <asp:Parameter DbType="Int32" DefaultValue="0" Name="building_id" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </DetailRow>
                                </Templates>
                            </dx:ASPxGridView>

<%-- Content of the first tab END --%>

                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Балансоутримувачі">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">

<%-- Content of the second tab BEGIN --%>

                            <table border="0" cellspacing="4" cellpadding="0" width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <h1 style="padding: 0; margin: 0;">Загальна інформація по балансоутримувачам</h1>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" 
                                            Text="Додаткові Колонки" Width="148px">
                                            <ClientSideEvents Click="ButtonBalansReportFieldChooserClick" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                    <td>
                                        <dx:ASPxPopupControl ID="ASPxPopupControl_Balans_SaveAs" runat="server" 
                                            HeaderText="Збереження у Файлі" 
                                            ClientInstanceName="ASPxPopupControl_Balans_SaveAs" 
                                            PopupElementID="ASPxButton_Balans_SaveAs">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxButton ID="ASPxButton_Balans_ExportXLS" runat="server" 
                                                        Text="XLS - Microsoft Excel&reg;" 
                                                        OnClick="ASPxButton_Balans_ExportXLS_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton_Balans_ExportPDF" runat="server" 
                                                        Text="PDF - Adobe Acrobat&reg;" 
                                                        OnClick="ASPxButton_Balans_ExportPDF_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton_Balans_ExportCSV" runat="server" 
                                                        Text="CSV - значення, розділені комами" 
                                                        OnClick="ASPxButton_Balans_ExportCSV_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>

                                        <dx:ASPxButton ID="ASPxButton_Balans_SaveAs" runat="server" AutoPostBack="False" 
                                            Text="Зберегти у Файлі" Width="148px">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>

                            <dx:ASPxGridViewExporter ID="ASPxGridViewExporterBalans" runat="server" 
                                FileName="Балансоутримувачi" GridViewID="ASPxGridViewBalans" PaperKind="A4" 
                                BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
                                <Styles>
                                    <Default Font-Names="Calibri,Verdana,Sans Serif">
                                    </Default>
                                    <AlternatingRowCell BackColor="#E0E0E0">
                                    </AlternatingRowCell>
                                </Styles>
                            </dx:ASPxGridViewExporter>

                            <asp:SqlDataSource ID="SqlDataSourceBalansOrganizations" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                                SelectCommand="SELECT organization_id, full_name, short_name, zkpo_code, occupation, status,
                                               form_gosp, form_of_ownership, industry, org_form, gosp_struct_type, addr_street_name,
                                               addr_nomer, director_fio, director_phone, kved_code, koatuu, otdel_gukv
                                               FROM view_organizations WHERE (organization_id IN (SELECT DISTINCT organization_id FROM balans))">
                            </asp:SqlDataSource>

                            <dx:ASPxGridView ID="ASPxGridViewBalans" runat="server" 
                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceBalansOrganizations" 
                                KeyFieldName="organization_id" Width="1200px" 
                                ClientInstanceName="ASPxGridViewBalans">
                                <GroupSummary>
                                    <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                                </GroupSummary>

                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="organization_id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0" Visible="False" Caption="ID"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="full_name" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Балансоутримувач"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="zkpo_code" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Код ЗКПО"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="addr_street_name" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Адреса - Вулиця"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="addr_nomer" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Адреса - Номер"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="status" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Фіз. / Юр. Особа"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="occupation" ShowInCustomizationForm="True" VisibleIndex="6" Caption="Вид Діяльності"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="form_gosp" ShowInCustomizationForm="True" VisibleIndex="7" Caption="Форма Господарювання"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="form_of_ownership" ShowInCustomizationForm="True" VisibleIndex="8" Caption="Форма Власності"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="industry" ShowInCustomizationForm="True" VisibleIndex="9" Caption="Галузь"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="org_form" ShowInCustomizationForm="True" VisibleIndex="10" Caption="Орг. Форма"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="gosp_struct_type" ShowInCustomizationForm="True" VisibleIndex="11" Caption="Госп. Структура"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="director_fio" ShowInCustomizationForm="True" VisibleIndex="12" Caption="Директор"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="director_phone" ShowInCustomizationForm="True" VisibleIndex="13" Caption="Телефон Директора"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="kved_code" ShowInCustomizationForm="True" VisibleIndex="14" Caption="КВЕД"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="koatuu" ShowInCustomizationForm="True" VisibleIndex="15" Caption="ОАТУУ"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="otdel_gukv" ShowInCustomizationForm="True" VisibleIndex="16" Caption="Відділ ГУКВ"></dx:GridViewDataTextColumn>
                                </Columns>

                                <SettingsBehavior EnableCustomizationWindow="True" 
                                    AutoFilterRowInputDelay="500" ColumnResizeMode="Control" />
                                <SettingsPager PageSize="18">
                                </SettingsPager>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" 
                                    ShowFilterBar="Auto" ShowHeaderFilterButton="True" 
                                    ShowHorizontalScrollBar="True" />
                                <SettingsCookies CookiesID="GUKV.balans" Version="1" Enabled="True" />
                                <SettingsDetail ShowDetailRow="True" />
                                <Styles Header-Wrap="True" />

                                <Templates>
                                    <DetailRow>
                                        <dx:ASPxGridView ID="ASPxGridViewBalansDetail" runat="server" AutoGenerateColumns="False" 
                                            DataSourceID="SqlDataSourceBalansDetail" 
                                            onbeforeperformdataselect="ASPxGridViewBalansDetail_BeforePerformDataSelect" 
                                            Width="1140px">

                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="district" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Адреса - Район"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="street_full_name" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Адреса - Вулиця"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_nomer" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Адреса - Номер"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_zip_code" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Адреса - Пошт. індекс"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_balans" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Площа на Балансі"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="cost_balans" ShowInCustomizationForm="True" VisibleIndex="6" Caption="Балансова Вартість"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="cost_extert_1m" ShowInCustomizationForm="True" VisibleIndex="7" Caption="Експертна Вартість (кв.м.)"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="cost_expert_total" ShowInCustomizationForm="True" VisibleIndex="8" Caption="Експертна Вартість (заг.)"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="condition" ShowInCustomizationForm="True" VisibleIndex="9" Caption="Стан Об'єкту"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="object_type" ShowInCustomizationForm="True" VisibleIndex="10" Caption="Тип Об'єкту"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="object_kind" ShowInCustomizationForm="True" VisibleIndex="11" Caption="Вид Об'єкту"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="bti_code" ShowInCustomizationForm="True" VisibleIndex="12" Caption="Код БТІ"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="history" ShowInCustomizationForm="True" VisibleIndex="13" Caption="Історична Цінність"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="purpose_group" ShowInCustomizationForm="True" VisibleIndex="14" Caption="Група Призначення"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="purpose" ShowInCustomizationForm="True" VisibleIndex="15" Caption="Призначення"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ownership_type" ShowInCustomizationForm="True" VisibleIndex="16" Caption="Володіння"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="otdel_gukv" ShowInCustomizationForm="True" VisibleIndex="17" Caption="Відділ ГУКВ"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="sqr_total" ShowInCustomizationForm="True" VisibleIndex="18" Caption="Загальна Площа Об'єкту"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="facade" ShowInCustomizationForm="True" VisibleIndex="19" Caption="Фасадність"></dx:GridViewDataTextColumn>
                                            </Columns>

                                            <SettingsBehavior AutoFilterRowInputDelay="500" ColumnResizeMode="Control" 
                                                EnableCustomizationWindow="True" />
                                            <Settings ShowHorizontalScrollBar="True" />
                                            <Styles Header-Wrap="True" />
                                            <SettingsCookies CookiesID="GUKV.Buildings.BalanceDetail" Enabled="True" Version="1" />
                                        </dx:ASPxGridView>

                                        <asp:SqlDataSource ID="SqlDataSourceBalansDetail" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                                            onselecting="SqlDataSourceBalansDetail_Selecting" 
                                            SelectCommand="SELECT * FROM view_org_balans WHERE (organization_id = @organization_id)">
                                            <SelectParameters>
                                                <asp:Parameter DbType="Int32" DefaultValue="0" Name="organization_id" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </DetailRow>
                                </Templates>
                            </dx:ASPxGridView>

<%-- Content of the second tab END --%>

                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Орендарі">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">

<%-- Content of the third tab BEGIN --%>
                            
                            <table border="0" cellspacing="4" cellpadding="0" width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <h1 style="padding: 0; margin: 0;">Загальна інформація по орендарям</h1>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" 
                                            Text="Додаткові Колонки" Width="148px">
                                            <ClientSideEvents Click="ButtonArendaReportFieldChooserClick" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                    <td>
                                        <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                                            HeaderText="Збереження у Файлі" 
                                            ClientInstanceName="ASPxPopupControl_Arenda_SaveAs" 
                                            PopupElementID="ASPxButton_Arenda_SaveAs">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxButton ID="ASPxButton_Arenda_ExportXLS" runat="server" 
                                                        Text="XLS - Microsoft Excel&reg;" 
                                                        OnClick="ASPxButton_Arenda_ExportXLS_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton_Arenda_ExportPDF" runat="server" 
                                                        Text="PDF - Adobe Acrobat&reg;" 
                                                        OnClick="ASPxButton_Arenda_ExportPDF_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton_Arenda_ExportCSV" runat="server" 
                                                        Text="CSV - значення, розділені комами" 
                                                        OnClick="ASPxButton_Arenda_ExportCSV_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>

                                        <dx:ASPxButton ID="ASPxButton_Arenda_SaveAs" runat="server" AutoPostBack="False" 
                                            Text="Зберегти у Файлі" Width="148px">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>

                            <asp:SqlDataSource ID="SqlDataSourceArendaOrganizations" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                                SelectCommand="SELECT organization_id, full_name, short_name, zkpo_code, occupation, status,
                                               form_gosp, form_of_ownership, industry, org_form, gosp_struct_type, addr_street_name,
                                               addr_nomer, director_fio, director_phone, kved_code, koatuu, otdel_gukv
                                               FROM view_organizations WHERE (organization_id IN (SELECT DISTINCT org_renter_id FROM arenda))">
                            </asp:SqlDataSource>

                            <dx:ASPxGridViewExporter ID="ASPxGridViewExporterArenda" runat="server" 
                                FileName="Орендарi" GridViewID="ASPxGridViewArenda" PaperKind="A4" 
                                BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
                                <Styles>
                                    <Default Font-Names="Calibri,Verdana,Sans Serif">
                                    </Default>
                                    <AlternatingRowCell BackColor="#E0E0E0">
                                    </AlternatingRowCell>
                                </Styles>
                            </dx:ASPxGridViewExporter>

                            <dx:ASPxGridView ID="ASPxGridViewArenda" runat="server" 
                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceArendaOrganizations" 
                                KeyFieldName="organization_id" Width="1200px" 
                                ClientInstanceName="ASPxGridViewArenda">
                                <GroupSummary>
                                    <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                                </GroupSummary>

                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="organization_id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0" Visible="False" Caption="ID"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="full_name" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Орендар"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="zkpo_code" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Код ЗКПО"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="addr_street_name" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Адреса - Вулиця"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="addr_nomer" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Адреса - Номер"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="status" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Фіз. / Юр. Особа"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="occupation" ShowInCustomizationForm="True" VisibleIndex="6" Caption="Вид Діяльності"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="form_gosp" ShowInCustomizationForm="True" VisibleIndex="7" Caption="Форма Господарювання"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="form_of_ownership" ShowInCustomizationForm="True" VisibleIndex="8" Caption="Форма Власності"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="industry" ShowInCustomizationForm="True" VisibleIndex="9" Caption="Галузь"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="org_form" ShowInCustomizationForm="True" VisibleIndex="10" Caption="Орг. Форма"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="gosp_struct_type" ShowInCustomizationForm="True" VisibleIndex="11" Caption="Госп. Структура"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="director_fio" ShowInCustomizationForm="True" VisibleIndex="12" Caption="Директор"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="director_phone" ShowInCustomizationForm="True" VisibleIndex="13" Caption="Телефон Директора"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="kved_code" ShowInCustomizationForm="True" VisibleIndex="14" Caption="КВЕД"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="koatuu" ShowInCustomizationForm="True" VisibleIndex="15" Caption="ОАТУУ"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="otdel_gukv" ShowInCustomizationForm="True" VisibleIndex="16" Caption="Відділ ГУКВ"></dx:GridViewDataTextColumn>
                                </Columns>

                                <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="500" ColumnResizeMode="Control" />
                                <SettingsPager PageSize="18">
                                </SettingsPager>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" 
                                    ShowFilterBar="Auto" ShowHeaderFilterButton="True" 
                                    ShowHorizontalScrollBar="True" />
                                <SettingsCookies CookiesID="GUKV.arenda" Version="1" Enabled="True" />
                                <SettingsDetail ShowDetailRow="True" />
                                <Styles Header-Wrap="True" />

                                <Templates>
                                    <DetailRow>
                                        <dx:ASPxGridView ID="ASPxGridViewArendaDetail" runat="server" AutoGenerateColumns="False" 
                                            DataSourceID="SqlDataSourceArendaDetail" KeyFieldName="arenda_id"
                                            onbeforeperformdataselect="ASPxGridViewArendaDetail_BeforePerformDataSelect" 
                                            Width="1140px">

                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="arenda_id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0" Visible="False" Caption="ID"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="org_giver_full_name" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Орендодавець"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="org_balans_full_name" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Балансоутримувач" Visible="False"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="rent_square" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Орендована Площа"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="district" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Адреса - Район"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="street_full_name" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Адреса - Вулиця"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_nomer" ShowInCustomizationForm="True" VisibleIndex="6" Caption="Адреса - Номер"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="addr_zip_code" ShowInCustomizationForm="True" VisibleIndex="7" Caption="Адреса - Пошт. індекс"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="object_type" ShowInCustomizationForm="True" VisibleIndex="8" Caption="Тип Об'єкту"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="object_kind" ShowInCustomizationForm="True" VisibleIndex="9" Caption="Вид Об'єкту"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="purpose_group" ShowInCustomizationForm="True" VisibleIndex="10" Caption="Група Призначенния"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="purpose" ShowInCustomizationForm="True" VisibleIndex="11" Caption="Призначення"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="is_privat" ShowInCustomizationForm="True" VisibleIndex="12" Caption="Приватизовано"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="agreement_kind" ShowInCustomizationForm="True" VisibleIndex="13" Caption="Вид Договору Оренди"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="agreement_date" ShowInCustomizationForm="True" VisibleIndex="14" Caption="Дата Договору Оренди"></dx:GridViewDataDateColumn>
                                                <dx:GridViewDataTextColumn FieldName="agreement_num" ShowInCustomizationForm="True" VisibleIndex="15" Caption="Номер Договору Оренди"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="floor_number" ShowInCustomizationForm="True" VisibleIndex="16" Caption="Поверх"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="cost_narah" ShowInCustomizationForm="True" VisibleIndex="17" Caption="Нараховано"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="cost_payed" ShowInCustomizationForm="True" VisibleIndex="18" Caption="Сплачено"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="cost_debt" ShowInCustomizationForm="True" VisibleIndex="19" Caption="Борг"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="cost_agreement" ShowInCustomizationForm="True" VisibleIndex="20" Caption="Вартість за Договором"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="pidstava" ShowInCustomizationForm="True" VisibleIndex="21" Caption="Підстава"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="pidstava_date" ShowInCustomizationForm="True" VisibleIndex="22" Caption="Підстава - Дата Документу"></dx:GridViewDataDateColumn>
                                                <dx:GridViewDataTextColumn FieldName="pidstava_num" ShowInCustomizationForm="True" VisibleIndex="23" Caption="Підстава - Номер Документу"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="rent_start_date" ShowInCustomizationForm="True" VisibleIndex="24" Caption="Початок Оренди"></dx:GridViewDataDateColumn>
                                                <dx:GridViewDataDateColumn FieldName="rent_finish_date" ShowInCustomizationForm="True" VisibleIndex="25" Caption="Закінчення Оренди"></dx:GridViewDataDateColumn>
                                                <dx:GridViewDataDateColumn FieldName="rent_actual_finish_date" ShowInCustomizationForm="True" VisibleIndex="26" Caption="Фактичне Закінчення Оренди"></dx:GridViewDataDateColumn>
                                                <dx:GridViewDataTextColumn FieldName="payment_type" ShowInCustomizationForm="True" VisibleIndex="27" Caption="Вид Розрахунків"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="is_subarenda" ShowInCustomizationForm="True" VisibleIndex="28" Caption="Субаренда"></dx:GridViewDataTextColumn>
                                            </Columns>

                                            <SettingsBehavior AutoFilterRowInputDelay="500" ColumnResizeMode="Control" 
                                                EnableCustomizationWindow="True" />
                                            <Settings ShowHorizontalScrollBar="True" />
                                            <Styles Header-Wrap="True" />
                                            <SettingsCookies CookiesID="GUKV.Buildings.ArendaDetail" Enabled="True" Version="1" />
                                        </dx:ASPxGridView>

                                        <asp:SqlDataSource ID="SqlDataSourceArendaDetail" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                                            onselecting="SqlDataSourceArendaDetail_Selecting" 
                                            SelectCommand="SELECT * FROM view_org_arenda WHERE (org_renter_id = @organization_id)">
                                            <SelectParameters>
                                                <asp:Parameter DbType="Int32" DefaultValue="0" Name="organization_id" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </DetailRow>
                                </Templates>
                            </dx:ASPxGridView>

<%-- Content of the third tab END --%>

                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Загальний Звіт">
                    <ContentCollection>
                        <dx:ContentControl runat="server" SupportsDisabledAttribute="True">

<%-- Content of the fourth tab BEGIN --%>

                            <table border="0" cellspacing="4" cellpadding="0" width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <h1 style="padding: 0; margin: 0;">Загальний Звіт по Будівлям, Балансоутримувачам та Орендарям</h1>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="False" 
                                            Text="Додаткові Колонки" Width="148px">
                                            <ClientSideEvents Click="ButtonZvitReportFieldChooserClick" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                    <td>
                                        <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                                            HeaderText="Збереження у Файлі" 
                                            ClientInstanceName="ASPxPopupControl_Zvit_SaveAs" 
                                            PopupElementID="ASPxButton_Zvit_SaveAs">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                                                    <dx:ASPxButton ID="ASPxButton_Zvit_ExportXLS" runat="server" 
                                                        Text="XLS - Microsoft Excel&reg;" 
                                                        OnClick="ASPxButton_Global_ExportXLS_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton_Zvit_ExportPDF" runat="server" 
                                                        Text="PDF - Adobe Acrobat&reg;" 
                                                        OnClick="ASPxButton_Global_ExportPDF_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton_Zvit_ExportCSV" runat="server" 
                                                        Text="CSV - значення, розділені комами" 
                                                        OnClick="ASPxButton_Global_ExportCSV_Click" Width="180px">
                                                    </dx:ASPxButton>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>

                                        <dx:ASPxButton ID="ASPxButton_Zvit_SaveAs" runat="server" AutoPostBack="False" 
                                            Text="Зберегти у Файлі" Width="148px">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>

                            <asp:SqlDataSource ID="SqlDataSourceZvit" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                                SelectCommand="SELECT building_id, district, street_full_name, addr_nomer, addr_zip_code,
                                               object_type, object_kind, balans_purpose_group, balans_purpose, condition,
                                               num_floors, construct_year, bti_code, history, sqr_total, sqr_habit, sqr_non_habit,
                                               additional_info, oatuu_code, facade, balans_org_full_name, balans_sqr_total,
                                               balans_sqr_pidval, balans_sqr_free, balans_sqr_privatizov, balans_sqr_non_habit,
                                               balans_cost, balans_cost_extert_1m, balans_cost_expert_total, balans_num_rent_agreements,
                                               balans_num_privat_apartments, balans_o26_code, balans_bti_condition, balans_num_floors,
                                               balans_ownership_type, balans_maintainer_full_name, arenda_renter_full_name,
                                               arenda_square, arenda_is_privat, arenda_agreement_kind, arenda_agreement_date,
                                               arenda_agreement_num, arenda_floor_number, arenda_cost_narah, arenda_cost_payed,
                                               arenda_debt, arenda_cost_agreement, arenda_pidstava, arenda_pidstava_date,
                                               arenda_pidstava_num, arenda_start_date, arenda_finish_date, arenda_actual_finish_date,
                                               arenda_rate, arenda_rate_uah, arenda_rishennya_code, arenda_payment_type,
                                               arenda_is_subarenda, balans_otdel_gukv FROM report_objects">
                            </asp:SqlDataSource>

                            <dx:ASPxGridViewExporter ID="ASPxGridViewExporterGlobal" runat="server" 
                                FileName="ЗагальнийЗвiт" GridViewID="ASPxGridViewZvit" PaperKind="A4" 
                                BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
                                <Styles>
                                    <Default Font-Names="Calibri,Verdana,Sans Serif">
                                    </Default>
                                    <AlternatingRowCell BackColor="#E0E0E0">
                                    </AlternatingRowCell>
                                </Styles>
                            </dx:ASPxGridViewExporter>

                            <dx:ASPxGridView ID="ASPxGridViewZvit" runat="server" 
                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceZvit" 
                                KeyFieldName="building_id" Width="1200px" 
                                ClientInstanceName="ASPxGridViewZvit">
                                <GroupSummary>
                                    <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                                </GroupSummary>

                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="building_id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0" Visible="False" Caption="ID"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="district" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Адреса - Район"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="street_full_name" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Адреса - Вулиця"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="addr_nomer" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Адреса - Номер"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="addr_zip_code" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Адреса - Пошт. Індекс"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="object_type" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Тип Об'єкту"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="object_kind" ShowInCustomizationForm="True" VisibleIndex="6" Caption="Вид Об'єкту"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_purpose_group" ShowInCustomizationForm="True" VisibleIndex="7" Caption="Група Призначення"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_purpose" ShowInCustomizationForm="True" VisibleIndex="8" Caption="Призначення"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="condition" ShowInCustomizationForm="True" VisibleIndex="9" Caption="Технічний Стан"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="num_floors" ShowInCustomizationForm="True" VisibleIndex="10" Caption="Кількість Поверхів"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="construct_year" ShowInCustomizationForm="True" VisibleIndex="11" Caption="Побудований"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="bti_code" ShowInCustomizationForm="True" VisibleIndex="12" Caption="Код БТІ"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="history" ShowInCustomizationForm="True" VisibleIndex="13" Caption="Історична Цінність"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="sqr_total" ShowInCustomizationForm="True" VisibleIndex="14" Caption="Загальна Площа"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="sqr_habit" ShowInCustomizationForm="True" VisibleIndex="15" Caption="Житлова Площа"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="sqr_non_habit" ShowInCustomizationForm="True" VisibleIndex="16" Caption="Нежитлова Площа"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="additional_info" ShowInCustomizationForm="True" VisibleIndex="17" Caption="Додаткова Інформація"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="oatuu_code" ShowInCustomizationForm="True" VisibleIndex="18" Caption="ОАТУУ"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="facade" ShowInCustomizationForm="True" VisibleIndex="19" Caption="Фасадність"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_org_full_name" ShowInCustomizationForm="True" VisibleIndex="20" Caption="Балансоутримувач"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_sqr_total" ShowInCustomizationForm="True" VisibleIndex="21" Caption="Площа на Балансі"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_sqr_pidval" ShowInCustomizationForm="True" VisibleIndex="22" Caption="Підвальна Площа (баланс)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_sqr_free" ShowInCustomizationForm="True" VisibleIndex="23" Caption="Вільна Площа (баланс)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_sqr_privatizov" ShowInCustomizationForm="True" VisibleIndex="24" Caption="Приватизована Площа (баланс)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_sqr_non_habit" ShowInCustomizationForm="True" VisibleIndex="25" Caption="Нежитлова Площа (баланс)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_cost" ShowInCustomizationForm="True" VisibleIndex="26" Caption="Балансова Вартість"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_cost_extert_1m" ShowInCustomizationForm="True" VisibleIndex="27" Caption="Експертна Балансова Вартість (кв.м.)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_cost_expert_total" ShowInCustomizationForm="True" VisibleIndex="28" Caption="Експертна Балансова Вартість (заг.)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_num_rent_agreements" ShowInCustomizationForm="True" VisibleIndex="29" Caption="Кількість Договорів Аренди"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_num_privat_apartments" ShowInCustomizationForm="True" VisibleIndex="30" Caption="Кількість Приватизованих Приміщень"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_o26_code" ShowInCustomizationForm="True" VisibleIndex="31" Caption="Код о26"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_bti_condition" ShowInCustomizationForm="True" VisibleIndex="32" Caption="Документи БТІ"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_num_floors" ShowInCustomizationForm="True" VisibleIndex="33" Caption="Кількість Поверхів (Баланс)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_ownership_type" ShowInCustomizationForm="True" VisibleIndex="34" Caption="Володіння (Баланс)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_maintainer_full_name" ShowInCustomizationForm="True" VisibleIndex="35" Caption="Експлуатуюча Організація"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_renter_full_name" ShowInCustomizationForm="True" VisibleIndex="36" Caption="Орендар"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_square" ShowInCustomizationForm="True" VisibleIndex="37" Caption="Орендована Площа"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_is_privat" ShowInCustomizationForm="True" VisibleIndex="38" Caption="Приватизовано (Оренда)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_agreement_kind" ShowInCustomizationForm="True" VisibleIndex="39" Caption="Тип Договору Оренди"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataDateColumn FieldName="arenda_agreement_date" ShowInCustomizationForm="True" VisibleIndex="40" Caption="Дата Договору Оренди"></dx:GridViewDataDateColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_agreement_num" ShowInCustomizationForm="True" VisibleIndex="41" Caption="Номер Договору Оренди"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_floor_number" ShowInCustomizationForm="True" VisibleIndex="42" Caption="Поверх (Оренда)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_cost_narah" ShowInCustomizationForm="True" VisibleIndex="43" Caption="Нарахована Орендна Плата"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_cost_payed" ShowInCustomizationForm="True" VisibleIndex="44" Caption="Сплачена Орендна Плата"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_debt" ShowInCustomizationForm="True" VisibleIndex="45" Caption="Заборгованість (Оренда)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_cost_agreement" ShowInCustomizationForm="True" VisibleIndex="46" Caption="Вартість Оренди за Договором"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_pidstava" ShowInCustomizationForm="True" VisibleIndex="47" Caption="Підстава Оренди"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataDateColumn FieldName="arenda_pidstava_date" ShowInCustomizationForm="True" VisibleIndex="48" Caption="Підстава Оренди - Дата Документу"></dx:GridViewDataDateColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_pidstava_num" ShowInCustomizationForm="True" VisibleIndex="49" Caption="Підстава Оренди - Номер Документу"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataDateColumn FieldName="arenda_start_date" ShowInCustomizationForm="True" VisibleIndex="50" Caption="Дата Початку Оренди"></dx:GridViewDataDateColumn>
                                    <dx:GridViewDataDateColumn FieldName="arenda_finish_date" ShowInCustomizationForm="True" VisibleIndex="51" Caption="Дата Завершення Оренди"></dx:GridViewDataDateColumn>
                                    <dx:GridViewDataDateColumn FieldName="arenda_actual_finish_date" ShowInCustomizationForm="True" VisibleIndex="52" Caption="Фактична Дата Завершення Оренди"></dx:GridViewDataDateColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_rate" ShowInCustomizationForm="True" VisibleIndex="53" Caption="Орендна Ставка"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_rate_uah" ShowInCustomizationForm="True" VisibleIndex="54" Caption="Орендна Ставка (грн.)"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_rishennya_code" ShowInCustomizationForm="True" VisibleIndex="55" Caption="Рішення з Оренди"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_payment_type" ShowInCustomizationForm="True" VisibleIndex="56" Caption="Вид Розрахунків"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="arenda_is_subarenda" ShowInCustomizationForm="True" VisibleIndex="57" Caption="Субаренда"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="balans_otdel_gukv" ShowInCustomizationForm="True" VisibleIndex="58" Caption="Відділ ГУКВ"></dx:GridViewDataTextColumn>
                                </Columns>

                                <SettingsBehavior EnableCustomizationWindow="True" 
                                    AutoFilterRowInputDelay="500" ColumnResizeMode="Control" />
                                <SettingsPager PageSize="18">
                                </SettingsPager>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" 
                                    ShowFilterBar="Auto" ShowHeaderFilterButton="True" 
                                    ShowHorizontalScrollBar="True" />
                                <SettingsCookies CookiesID="GUKV.GlobalZvit" Version="1" Enabled="True" />
                                
                                <Styles Header-Wrap="True" />
                            </dx:ASPxGridView>

<%-- Content of the fourth tab END --%>

                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
        </dx:ASPxPageControl>
    </p>
<p>
        &nbsp;</p>
</asp:Content>


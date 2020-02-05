<%@ Page Title="Перелік організацій, що мають подавати звіт 1НФ" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SubmitterList.aspx.cs" Inherits="SubmitterList_SubmitterList" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>

<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function ButtonFieldChooserClick(s, e) {

        PopupFieldChooser.Show();

        /*
        if (GridViewSubmitters.IsCustomizationWindowVisible())
            GridViewSubmitters.HideCustomizationWindow();
        else
            GridViewSubmitters.ShowCustomizationWindow();
        */
    }

    // ]]>

</script>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle" runat="server" Text="Перелік організацій, що мают подавати звіт 1НФ" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonMoreColumns" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ButtonFieldChooserClick" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl1" 
                PopupElementID="ButtonSaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxButton ID="ASPxButton3" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="Button_Submitters_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="Button_Submitters_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="Button_Submitters_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ButtonSaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<asp:SqlDataSource ID="SqlDataSourceSubmitters" runat="server" 
    ConnectionString="<%$ ConnectionStrings:Import1NFConnectionString %>" 
    SelectCommand="SELECT * FROM view_1nf_submitters">
</asp:SqlDataSource>

<dx:ASPxGridViewExporter ID="GridViewSubmittersExporter" runat="server" 
    FileName="ПерелікОрганізацій" GridViewID="GridViewSubmitters" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="GridViewSubmitters"
    ClientInstanceName="GridViewSubmitters"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceSubmitters"
    KeyFieldName="organization_id"
    OnCustomCallback="GridViewSubmitters_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewSubmitters_CustomFilterExpressionDisplayText" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="0" Visible="True" Caption="Повна назва" Width="250px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="False" Caption="Коротка Назва" Width="250px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="True" Caption="Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="True" Caption="Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Вид Діяльності" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_gosp" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="False" Caption="Форма фінансування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_of_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="False" Caption="Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="gosp_struct" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="False" Caption="Госп. Структура"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="False" Caption="Орган управління"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_form" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="False" Caption="Орг.-правова форма госп."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_organ" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="False" Caption="Орган госп. упр."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="status" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Фіз. / Юр. Особа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="True" Caption="Район" Width="150px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="True" Caption="Назва Вулиці" Width="200px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="True" Caption="Номер Будинку"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_korpus" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="False" Caption="Номер Будинку - Корпус"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_zip_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="16" Visible="False" Caption="Поштовий Індекс"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_fio" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="ФІО Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_phone" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Тел. Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_fio" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="False" Caption="ФІО Бухгалтера"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_phone" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="False" Caption="Тел. Бухгалтера"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="fax" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="Факс"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_auth" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="False" Caption="Реєстраційний Орган"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="False" Caption="Номер Запису про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="False" Caption="Дата Реєстрації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_svidot" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="False" Caption="Номер Свідоцтва про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="kved_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="False" Caption="КВЕД"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="27" Visible="False" Caption="Відділ ДКВ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="mayno" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="28" Visible="False" Caption="Правовий режим майна"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_liquidated" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="29" Visible="False" Caption="Ліквідовано"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="liquidation_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Дата Ліквідації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_email" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="False" Caption="Ел. Адреса"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_posada" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="False" Caption="Контактна Особа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sfera_upr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="False" Caption="Сфера Управління"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="report_exists" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="34" Visible="True" Caption="Звіт 1НФ Подано"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="report_accepted" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="True" Caption="Звіт 1НФ Прийнято"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_balans_objects" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="36" Visible="False" Caption="Кількість Об'єктів На Балансі"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="origin_db_str" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="37" Visible="False" Caption="Джерело Даних"></dx:GridViewDataTextColumn>
        
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="full_name" SummaryType="Count" DisplayFormat="{0} організацій" />
        <dx:ASPxSummaryItem FieldName="short_name" SummaryType="Count" DisplayFormat="{0} організацій" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2000" ColumnResizeMode="Control" />
    <SettingsPager Mode="ShowPager" PageSize="10"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="False"
        ShowFilterBar="Auto"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True" />
    <SettingsCookies CookiesID="GUKV.Import1NF.Submitters" Version="2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<dx:ASPxPopupControl ID="PopupFieldChooser" runat="server" 
    HeaderText="Додаткові Колонки" 
    ClientInstanceName="PopupFieldChooser" 
    PopupElementID="GridViewSubmitters"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
            <uc1:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>


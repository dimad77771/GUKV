<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RishProjectTableEditor.ascx.cs" Inherits="RishProjectTableEditor" EnableViewState="true" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<style type="text/css">
    .dxtcLite_Metropolis li.dxtc-rightIndent,
    .dxtcLite_MetropolisBlue li.dxtc-rightIndent
    {
    	width: 623px !important;
    }
    .dxtcLite_Metropolis .dxtc-strip,
    .dxtcLite_MetropolisBlue .dxtc-strip
    {
    	width: auto !important;
    }
</style>

<dx:ASPxCallbackPanel ID="CPTableProperties" ClientInstanceName="CPTableProperties"
    runat="server">
    <PanelCollection>
        <dx:PanelContent ID="Panelcontent4" runat="server">

            <div style="float: right;">
                <div class="data-field-label">
                    <dx:ASPxLabel ID="LabelObjectType" runat="server" Text="Тип об'єкту"></dx:ASPxLabel>
                </div>
                <div class="data-field-value">
                    <dx:ASPxComboBox ID="ComboObjectType" ClientInstanceName="ComboObjectType" 
                        runat="server" Width="260px"
                        ValueField="id" ValueType="System.Int32" TextField="name" 
                        style="margin-bottom: 0px">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	GridViewTable.PerformCallback(JSON.stringify({objectType: s.GetSelectedItem().value}));
}" />
                        <Items>
                            <dx:ListEditItem Text="Об’єкти нерухомості нежитлового фонду" Value="0" />
                            <dx:ListEditItem Text="Інші об’єкти комунальної власності" Value="1" />
                        </Items>
                    </dx:ASPxComboBox>
                </div>
            </div>

            <div class="data-field-label">
                <dx:ASPxLabel ID="LabelAppendixName" runat="server" Text="Назва додатку"></dx:ASPxLabel>
            </div>
            <div class="data-field-value">
                <dx:ASPxTextBox ID="EditAppendixName" ClientInstanceName="EditAppendixName" runat="server" Width="600px" />
            </div>

            <br />

            <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                RenderMode="Lightweight">
                <TabPages>
                    <dx:TabPage Text="Текст Додатку">
                        <ContentCollection>
                            <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                <br />
                                <div class="data-field-value">
                                    <dx:ASPxHtmlEditor ID="MemoPunktOutro" runat="server" 
                                        ClientInstanceName="MemoPunktOutro" Height="300px" Width="920px">
                                        <Settings AllowHtmlView="False" AllowPreview="False" />
                                    </dx:ASPxHtmlEditor>
                                </div>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Перелік Об'єктів">
                        <ContentCollection>
                            <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                <br />

                                <div class="data-field-label" style="display: inline;">
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Увага: Таблиця наповнюється шляхом завантаження зовнішнього документа" />
                                </div>

                                <br />

                                <dx:ASPxGridView ID="GridViewTable" runat="server" ClientInstanceName="GridViewTable" EnableViewState="true"
                                    AutoGenerateColumns="False" KeyFieldName="id" Width="920px" EnableRowsCache="False"
                                    OnCustomCallback="GridViewTable_CustomCallback" 
                                    OnHtmlDataCellPrepared="GridViewTable_HtmlDataCellPrepared">
                                    <Columns>
                                        <dx:GridViewDataTextColumn VisibleIndex="0" Visible="false" FieldName="id">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="0" Visible="true" FieldName="name" 
                                            Caption="Найменування об'єкта" Width="200px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="1" Visible="true" FieldName="address" 
                                            Caption="Адреса" Width="200px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="2" Visible="true" FieldName="addr_street_name" 
                                            Caption="Назва вулиці" Width="200px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="3" Visible="true" FieldName="addr_nomer" 
                                            Caption="Номер будинку" Width="80px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="4" Visible="true" FieldName="addr_misc" 
                                            Caption="Додаткова адреса" Width="80px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="5" Visible="true" FieldName="addr_distr" 
                                            Caption="Район" Width="80px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="6" Visible="true" FieldName="obj_kind" 
                                            Caption="Вид будинку" Width="80px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="7" Visible="true" FieldName="obj_type" 
                                            Caption="Тип будинку" Width="80px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="8" Visible="true" FieldName="year_built"
                                            Caption="Рік завершення будівництва">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="9" Visible="true" FieldName="sqr_total"
                                            Caption="Площа, кв.м">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="10" Visible="true" FieldName="location" 
                                            Caption="Місце розташування об’єкта" Width="200px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="11" Visible="true" FieldName="inv_number"
                                            Caption="Інвентарний номер" Width="130px">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn VisibleIndex="12" Visible="true" FieldName="commissioned_date"
                                            Caption="Дата введення в експлуатацію">
                                            <PropertiesTextEdit DisplayFormatString="d">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewBandColumn VisibleIndex="13" Caption="Вартість (грн.)">
                                            <Columns>
                                                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="initial_cost" Caption="Первісна">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn VisibleIndex="1" FieldName="remaining_cost" Caption="Залишкова">
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:GridViewBandColumn>
                                    </Columns>
                                    <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2500"
                                        ColumnResizeMode="Control" AllowDragDrop="false" ConfirmDelete="True" EnableRowHotTrack="True" />
                                    <SettingsPager PageSize="10">
                                    </SettingsPager>
                                    <SettingsPopup>
                                        <HeaderFilter Width="200" Height="300" />
                                    </SettingsPopup>
                                    <Settings ShowFilterRow="False" ShowFilterRowMenu="False" ShowGroupPanel="False"
                                        ShowFilterBar="Hidden" ShowHeaderFilterButton="False" HorizontalScrollBarMode="Visible"
                                        ShowFooter="False" VerticalScrollBarMode="Hidden" />
                                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                                    <SettingsCookies CookiesID="GUKV.RishProject.TableEditor" Version="A2_1" />
                                    <Styles Header-Wrap="True">
                                        <Header Wrap="True">
                                        </Header>
                                    </Styles>
                                </dx:ASPxGridView>
                                <br />
                                <div class="data-field-label" style="display: inline;">
                                    <dx:ASPxLabel ID="LabelFile" ClientInstanceName="LabelFile" runat="server" Text="Зовнішній документ:" />
                                </div>
                                <dx:ASPxHyperLink ID="LinkFileName" ClientInstanceName="LinkFileName" runat="server"
                                    Text="не завантажено" Target="_blank" NavigateUrl="" />
                                <br />
                                <dx:ASPxUploadControl ID="UploadFile" ClientInstanceName="UploadFile" runat="server" FileUploadMode="OnPageLoad"
                                    Width="920px" ShowProgressPanel="True" OnFileUploadComplete="UploadFile_FileUploadComplete"
                                    ShowClearFileSelectionButton="False">
                                    <ValidationSettings AllowedFileExtensions=".xlsx">
                                    </ValidationSettings>
                                    <ClientSideEvents FileUploadComplete="function(s, e) {
    GridViewTable.Refresh();
	var data = $.parseJSON(e.callbackData);
    if (data != null) {
        if (typeof data.ImportError == 'string') {
	        LinkFileName.SetText('не завантажено: ' + data.ImportError);
            LinkFileName.SetNavigateUrl('');
            $('#OrigFileName').val('');
            $('#TempFileName').val('');
        }
        else {
	        LinkFileName.SetText(data.OriginalFileName);
            LinkFileName.SetNavigateUrl(data.ViewDocumentUrl);
            $('#OrigFileName').val(data.OriginalFileName);
            $('#TempFileName').val(data.TempFileName);
        }
    } else {
	    LinkFileName.SetText('не завантажено');
        LinkFileName.SetNavigateUrl('');
        $('#OrigFileName').val('');
        $('#TempFileName').val('');
    }
}" TextChanged="function(s, e) { UploadFile.Upload(); }" />
                                </dx:ASPxUploadControl>
                                <asp:HiddenField ID="OrigFileName" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="TempFileName" ClientIDMode="Static" runat="server" />
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                </TabPages>
            </dx:ASPxPageControl>

            <br />

        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<dx:ASPxTreeListTemplateReplacement ID="Replacement1" runat="server" ReplacementType="UpdateButton" />
<dx:ASPxTreeListTemplateReplacement ID="Replacement2" runat="server" ReplacementType="CancelButton" />

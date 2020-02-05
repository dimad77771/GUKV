<%@ control language="C#" autoeventwireup="true" inherits="RishProjectMainDocEditor, App_Web_rishprojectmaindoceditor.ascx.23e82b75" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:SqlDataSource ID="SqlDataSourceProjectContact" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT dict_rish_project_org.name AS org_name, dict_rish_project_org_contact.id AS contact_id, dict_rish_project_org_contact.contact_name,
        dict_rish_project_org_contact.contact_title, dict_rish_project_org_contact.contact_phone
        FROM dict_rish_project_org INNER JOIN dict_rish_project_org_contact ON dict_rish_project_org.id = dict_rish_project_org_contact.project_org_id">
</asp:SqlDataSource>

<dx:ASPxCallbackPanel ID="CPMainDocProperties" ClientInstanceName="CPMainDocProperties" runat="server" OnCallback="CPMainDocProperties_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent2" runat="server">

            <div class="data-field-label">
            <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Тема документу"></dx:ASPxLabel>
            </div>
            <div class="data-field-value">
            <dx:ASPxTextBox ID="EditDocSubject" ClientInstanceName="EditDocSubject" runat="server" Width="860px" />
            </div>
            <br />

            <div class="data-field-label">
            <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Контактна особа" Width="160px"></dx:ASPxLabel>
            </div>
            <div class="data-field-value">
            <dx:ASPxComboBox ID="ComboRishContact" ClientInstanceName="ComboRishContact" 
                runat="server" ValueType="System.Int32" DataSourceID="SqlDataSourceProjectContact" TextField="contact_name" 
                ValueField="contact_id" TextFormatString="{1}" Width="860px">
                <Columns>
                    <dx:ListBoxColumn Caption="Організація" FieldName="org_name" />
                    <dx:ListBoxColumn FieldName="contact_id" Visible="False" />
                    <dx:ListBoxColumn Caption="Відповідальна Особа" FieldName="contact_name" />
                    <dx:ListBoxColumn Caption="Посада" FieldName="contact_title" />
                    <dx:ListBoxColumn Caption="Телефон" FieldName="contact_phone" />
                </Columns>
            </dx:ASPxComboBox>
            </div>
            <br />

            <div>
                <dx:ASPxRadioButton ID="RadioButtonDoc" runat="server" ClientInstanceName="RadioButtonDoc" 
                    Text="Створити документ використовуючи наступну інформацію:">
                    <ClientSideEvents CheckedChanged="function (s,e) { CPMainDocProperties.PerformCallback('enabledoc:'); }" />
                </dx:ASPxRadioButton>
                <br />

                <div class="data-field-label">
                <dx:ASPxLabel ID="LabelIntro" ClientInstanceName="LabelIntro" runat="server" Text="Початковий текст"></dx:ASPxLabel>
                </div>
                <div class="data-field-value">
                <dx:ASPxHtmlEditor ID="MemoDocIntro" ClientInstanceName="MemoDocIntro" runat="server" Width="860px" Height="160px">
                    <Settings AllowHtmlView="false" AllowPreview="False" />
                </dx:ASPxHtmlEditor>
                </div>
                <br />

                <div class="data-field-label">
                <dx:ASPxLabel ID="LabelOutro" ClientInstanceName="LabelOutro" runat="server" Text="Заключний текст"></dx:ASPxLabel>
                </div>
                <div class="data-field-value">
                <dx:ASPxHtmlEditor ID="MemoDocOutro" ClientInstanceName="MemoDocOutro" runat="server" Width="860px" Height="160px">
                    <Settings AllowHtmlView="false" AllowPreview="False" />
                </dx:ASPxHtmlEditor>
                </div>
            </div>
            <br />

            <div>
                <dx:ASPxRadioButton ID="RadioButtonFile" runat="server" ClientInstanceName="RadioButtonFile" 
                    Text="Підключити зовнішній документ:">
                    <ClientSideEvents CheckedChanged="function (s,e) { CPMainDocProperties.PerformCallback('enableattach:'); }" />
                </dx:ASPxRadioButton>
                <br />

                <div class="data-field-label" style="display: inline;">
                    <dx:ASPxLabel ID="LabelFile" ClientInstanceName="LabelFile" runat="server" Text="Зовнішній документ:" />
                </div>

                <dx:ASPxHyperLink ID="LinkFileName" ClientInstanceName="LinkFileName" runat="server" Text="не завантажено" Target="_blank" NavigateUrl="" />

                <br />

                <dx:ASPxUploadControl ID="UploadFile" ClientInstanceName="UploadFile" runat="server" Width="860px"
                    ShowProgressPanel="True" OnFileUploadComplete="UploadFile_FileUploadComplete" ShowClearFileSelectionButton="False" >
                    <ValidationSettings AllowedFileExtensions=".docx"></ValidationSettings>
                    <ClientSideEvents
FileUploadComplete="function(s, e) {
	var data = $.parseJSON(e.callbackData);
    if (data != null) {
	    LinkFileName.SetText(data.OriginalFileName);
        LinkFileName.SetNavigateUrl(data.ViewDocumentUrl);
        $('#OrigFileName').val(data.OriginalFileName);
        $('#TempFileName').val(data.TempFileName);
    } else {
	    LinkFileName.SetText('не завантажено');
        LinkFileName.SetNavigateUrl('');
        $('#OrigFileName').val('');
        $('#TempFileName').val('');
    }
}"

TextChanged="function(s, e) { UploadFile.Upload(); }"
                    />
                </dx:ASPxUploadControl>

                <asp:HiddenField ID="OrigFileName" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="TempFileName" ClientIDMode="Static" runat="server" />
            </div>
            <br />

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<dx:ASPxTreeListTemplateReplacement ID="Replacement1" runat="server" ReplacementType="UpdateButton" />
<dx:ASPxTreeListTemplateReplacement ID="Replacement2" runat="server" ReplacementType="CancelButton" />

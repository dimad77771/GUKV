<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RishProjectAppendixEditor.ascx.cs" Inherits="RishProjectAppendixEditor" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<dx:ASPxCallbackPanel ID="CPAppendixProperties" ClientInstanceName="CPAppendixProperties" runat="server" OnCallback="CPAppendixProperties_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent1" runat="server">

            <div>
                <dx:ASPxRadioButton ID="RadioButtonDoc" runat="server" ClientInstanceName="RadioButtonDoc" Text="Створити додаток використовуючи наступну інформацію:">
                    <ClientSideEvents CheckedChanged="function (s,e) { CPAppendixProperties.PerformCallback('enabledoc:'); }" />
                </dx:ASPxRadioButton>
                <br />

                <div class="data-field-label">
                    <dx:ASPxLabel ID="LabelAppendixName" runat="server" Text="Назва додатку"></dx:ASPxLabel>
                </div>
                <div class="data-field-value">
                    <dx:ASPxTextBox ID="EditAppendixName" ClientInstanceName="EditAppendixName" runat="server" Width="860px" />
                </div>
                <br />

                <div class="data-field-label">
                    <dx:ASPxLabel ID="LabelIntro" ClientInstanceName="LabelIntro" runat="server" Text="Початковий текст" Width="160px"></dx:ASPxLabel>
                </div>
                <div class="data-field-value">
                    <dx:ASPxHtmlEditor ID="MemoAppendixIntro" ClientInstanceName="MemoAppendixIntro" runat="server" Width="860px" Height="170px">
                        <Settings AllowHtmlView="false" AllowPreview="False" />
                    </dx:ASPxHtmlEditor>
                </div>
                <br />

                <div class="data-field-label">
                    <dx:ASPxLabel ID="LabelOutro" ClientInstanceName="LabelOutro" runat="server" Text="Заключний текст"></dx:ASPxLabel>
                </div>
                <div class="data-field-value">
                    <dx:ASPxHtmlEditor ID="MemoAppendixOutro" ClientInstanceName="MemoAppendixOutro" runat="server" Width="860px" Height="170px">
                        <Settings AllowHtmlView="false" AllowPreview="False" />
                    </dx:ASPxHtmlEditor>
                </div>
            </div>
            <br />

            <div>
                <dx:ASPxRadioButton ID="RadioButtonFile" runat="server" ClientInstanceName="RadioButtonFile" Text="Підключити зовнішній документ:">
                    <ClientSideEvents CheckedChanged="function (s,e) { CPAppendixProperties.PerformCallback('enableattach:'); }" />
                </dx:ASPxRadioButton>

                <br />

                <div class="data-field-label" style="display: inline;">
                    <dx:ASPxLabel ID="LabelFile" ClientInstanceName="LabelFile" runat="server" Text="Зовнішній документ:" />
                </div>

                <dx:ASPxHyperLink ID="LinkFileName" ClientInstanceName="LinkFileName" runat="server" Text="не завантажено" Target="_blank" NavigateUrl=""/>

                <br />

                <dx:ASPxUploadControl ID="UploadFile" ClientInstanceName="UploadFile" runat="server" Width="860px"
                    ShowProgressPanel="True" OnFileUploadComplete="UploadFile_FileUploadComplete" ShowClearFileSelectionButton="False">
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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocCard.aspx.cs" Inherits="Cards_DocCard"
    MasterPageFile="~/NoMenu.master" Title="Картка Документу" ValidateRequest="false" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxImageGallery" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" Width="100%">
    <TabPages>
        <dx:TabPage Text="Реквізити документу" Name="DocCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the first tab BEGIN --%>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocCardProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 id, kind, general_kind, doc_date, doc_num, topic, note, receive_date, commission, [source],
                   [state], summa, summa_zalishkova, is_text_exists FROM view_documents WHERE id = @doc_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="doc_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>


<asp:ObjectDataSource ID="ObjectDataSourceBalansPhoto" runat="server" SelectMethod="SelectFromTempFolder" 
    TypeName="ExtDataEntry.Models.FileAttachment">
    <SelectParameters>
        <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
        <asp:Parameter DefaultValue="" Name="recordID" Type="Int32" />
        <asp:Parameter DefaultValue="" Name="tempGuid" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>

	<script type="text/javascript" language="javascript">
        var imageIndex = 0;

        function DeleteImage(index) {
            var r = confirm("Ви дійсно хочете видалити фото?");
            if (r == true) {
                ContentCallback.PerformCallback('deleteimage:' + index);
                ASPxCallbackPanelImageGallery.PerformCallback('ASPxCallbackPanelImageGallery:');
            }
        }

        function EditImage(index) {
            editPopup.Show();
        }


        function Uploader_OnUploadStart() {
            btnUpload.SetEnabled(false);
        }


        function setUrlParam(prmName, val) {
            var res = '';
            var d = location.href.split("#")[0].split("?");
            var base = d[0];
            var query = d[1];
            if (query) {
                var params = query.split("&");
                for (var i = 0; i < params.length; i++) {
                    var keyval = params[i].split("=");
                    if (keyval[0] != prmName) {
                        res += params[i] + '&';
                    }
                }
            }
            res += prmName + '=' + val;
            window.location.href = base + '?' + res;
            return false;
        } 

        function Uploader_OnFilesUploadComplete(args) {
            UpdateUploadButton();
            ASPxCallbackPanelImageGallery.PerformCallback('refreshphoto:');
		}

        function UpdateUploadButton() {
            btnUpload.SetEnabled(uploader.GetText(0) != "");
        }
    </script>


<asp:FormView runat="server" BorderStyle="None" ID="DocDetails" DataSourceID="SqlDataSourceDocCardProperties" EnableViewState="False" Width="100%">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Реквізити документу">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Номер Документу"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("doc_num") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Дата Документу"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" ReadOnly="True" Value='<%# Eval("doc_date") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Назва Документу"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("topic") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Вид Документу"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Text='<%# Eval("kind") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Узагальнення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox22" runat="server" ReadOnly="True" Text='<%# Eval("general_kind") %>' Width="290px" /></td>
                        </tr>
                    </table>



                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Додаткові відомості">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Дата Отримання"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("receive_date") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Джерело Походження"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox5" runat="server" ReadOnly="True" Text='<%# Eval("source") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Статус"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("state") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Комісія, що розглядала документ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox7" runat="server" ReadOnly="True" Text='<%# Eval("commission") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Сума за документом"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox8" runat="server" ReadOnly="True" Text='<%# Eval("summa") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Залишкова сума"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" ReadOnly="True" Text='<%# Eval("summa_zalishkova") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Примітка"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox10" runat="server" ReadOnly="True" Text='<%# Eval("note") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td colspan="7"><dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" ReadOnly="True"
                                Checked='<%# (1.Equals(Eval("is_text_exists"))) ? true : false %>' Text="Текст Існує" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </ItemTemplate>
</asp:FormView>

<%-- Content of the first tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Підпорядковані документи" Name="DocCardChildDocs">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

<%-- Content of the second tab BEGIN --%>

<dx:ASPxGridView ID="GridViewDocCardChildDocs" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceDocCardChildDocs" KeyFieldName="link_id" Width="100%">

    <Columns>
        <dx:GridViewDataDateColumn FieldName="child_date" VisibleIndex="0" Caption="Дата Підпорядкованого Документу" Width="120px">
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="child_num" VisibleIndex="1" Caption="Номер Підпорядкованого Документу" Width="120px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("slave_doc_id") + ")\">" + Eval("child_num") + "</a>"%>
            </DataItemTemplate> 
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_topic" VisibleIndex="2" Caption="Назва Підпорядкованого Документу" Width="570px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("slave_doc_id") + ")\">" + Eval("child_topic") + "</a>"%>
            </DataItemTemplate> 
        </dx:GridViewDataTextColumn>
    </Columns>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" 
        EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.DocCard.ChildDocs" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocCardChildDocs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT link_id, slave_doc_id, child_num, child_date, child_topic, child_kind FROM view_doc_links WHERE master_doc_id = @m_doc_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="m_doc_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<%-- Content of the second tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Об'єкти за документом" Name="DocCardObjects">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the third tab BEGIN --%>

<dx:ASPxGridView ID="GridViewDocCardObjects" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceDocCardObjects" KeyFieldName="link_id" Width="100%">

    <Columns>
        <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="0" Caption="Назва Вулиці" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="1" Caption="Номер Будинку" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_name" VisibleIndex="2" Caption="Назва Об'єкту" Width="160px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" VisibleIndex="3" Caption="Група Призначення" Width="160px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="4" Caption="Призначення" Width="160px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_obj" VisibleIndex="5" Caption="Площа" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_balans" VisibleIndex="6" Caption="Балансова Вартість" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_zalishkova" VisibleIndex="7" Caption="Залишкова Вартість" Width="100px"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_obj" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_balans" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_zalishkova" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" 
        EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.DocCard.Objects" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocCardObjects" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT bd.link_id, bd.building_id, bd.document_id, b.district, b.street_full_name, b.addr_nomer, bd.purpose_group,
                   bd.purpose, bd.obj_name, bd.cost_balans, bd.cost_zalishkova, bd.sqr_obj FROM view_building_docs bd
                   INNER JOIN view_buildings b ON b.building_id = bd.building_id WHERE bd.document_id = @doc_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="doc_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<%-- Content of the third tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Текст" Name="DocCardText">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the fourth tab BEGIN --%>

<iframe id="docCardTextFrame" src="../Documents/DoTextEmbedded.aspx?docid=<%= Request.QueryString["docid"] %>"
    frameborder="0" width="100%" height="650px"></iframe>

<%-- Content of the fourth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

		<dx:TabPage Text="Фото / плани" Name="Tab6">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl7" runat="server">

                    <dx:ASPxRoundPanel ID="PanelPhoto" runat="server" HeaderText="Фото / плани" EnableViewState="true">
                        <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent16" runat="server">


                                <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                <tr>
                                <td>

                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanelImageGallery" ShowLoadingPanel="false" EnableViewState="true"
                                    ClientInstanceName="ASPxCallbackPanelImageGallery" runat="server" 
                                    OnCallback="ASPxCallbackPanelImageGallery_Callback">
                            
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
                                        
                                    <dx:ASPxImageGallery ID="imageGalleryDemo" runat="server" DataSourceID="ObjectDataSourceBalansPhoto"
                                        EnableViewState="false" 
                                        AlwaysShowPager="false" 
                                        PagerAlign="Center"
                                        ThumbnailHeight="190" ThumbnailWidth="190"
                                        SettingsFullscreenViewer-ShowCloseButton="true" 
                                        OnDataBound="imageGalleryDemo_DataBound" >

                                      <SettingsFolder ImageCacheFolder="~\Thumb\"  /> 
                                        
                                        <ItemTextTemplate>

                                            <div class="item">
                                                <div class="item_email" style="text-align:center">
                                                    <a style="color:White;cursor:pointer" onclick="javascript:DeleteImage(<%# Container.ItemIndex %>);" title="Видалити">Видалити</a>
                                                </div>
                                            </div>

                                        </ItemTextTemplate>
                                        <SettingsTableLayout RowsPerPage="2" ColumnCount="5" />

                                        <SettingsTableLayout ColumnCount="5" RowsPerPage="2"></SettingsTableLayout>

                                        <PagerSettings Position="TopAndBottom">
                                            <PageSizeItemSettings Visible="False" />
                                            <PageSizeItemSettings Visible="False"></PageSizeItemSettings>
                                        </PagerSettings>

                                    </dx:ASPxImageGallery> 

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                                </td>
                                </tr>
                                <tr>
                                <td>

                                <dx:ASPxCallbackPanel ID="ContentCallback" runat="server" EnableViewState="true"
                                    ClientInstanceName="ContentCallback" OnCallback="ContentCallback_Callback">
                                    
                                    <PanelCollection>
                                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                            <dx:ASPxUploadControl ID="uplImage" runat="server" ShowUploadButton="false" 
                                                FileUploadMode="OnPageLoad"
                                                ClientInstanceName="uploader" NullText="..." 
                                                OnFileUploadComplete="ASPxUploadPhotoControl_FileUploadComplete" 
                                                ShowProgressPanel="True" Size="35" UploadMode="Auto">
                                                <ValidationSettings AllowedFileExtensions=".jpg, .jpeg, .jpe, .gif, .png, .bmp, .pdf" 
                                                    MaxFileSize="20480000">

                                                </ValidationSettings>
                                                <ClientSideEvents FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" 
                                                    FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }" 
                                                    TextChanged="function(s, e) { UpdateUploadButton(); }" />
                                                <AdvancedModeSettings EnableMultiSelect="True" />
                                            </dx:ASPxUploadControl>

                                            <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Visible ="true"
                                                ClientInstanceName="btnUpload" Text="Завантажити" 
                                            onclick="btnUpload_Click">
                                                <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                            </dx:ASPxButton>

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                                </td>
                                </tr>
                                </table>



                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
    </TabPages>
</dx:ASPxPageControl>

<%--			<p class="SpacingPara"/>
			<table border="0" cellspacing="0" cellpadding="0">
				<tr>
					<td>
						<dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
							<ClientSideEvents Click="function (s,e) { if (ReportingPeriodCombovalidate()){ CPMainPanel.PerformCallback('save:'); }}"
							   LostFocus="HidePnl" />
						</dx:ASPxButton>
					</td>
					<td> &nbsp; </td>
				<td> &nbsp; </td>
					<td>
						<dx:ASPxButton ID="ButtonClear" runat="server" Text="Відмінити зміни" AutoPostBack="false" CausesValidation="false">
							<ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
						</dx:ASPxButton>
					</td>
				</tr>
			</table>--%>

</asp:Content>

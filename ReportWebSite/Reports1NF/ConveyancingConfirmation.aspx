<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConveyancingConfirmation.aspx.cs" Inherits="Reports1NF_ConveyancingConfirmation" MasterPageFile="~/NoHeader.master" Title="Зміна балансоутримувачів об'єктів - Новий запрос" %>

<%@ Register src="ObjectPicker.ascx" tagname="ObjectPicker" tagprefix="uctl1" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" />

<mini:ProfiledSqlDataSource ID="SqlDataSourceTransferRequest" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
    SELECT  TOP 1 req.*, org_to.full_name org_to_name, org_from.full_name org_from_name,
    CASE WHEN is_object_exists = 1 THEN bal.sqr_total ELSE ub.sqr_total END AS sqr_total,
    CASE WHEN is_object_exists = 1 THEN bui.addr_address ELSE bui_un.addr_address END AS full_addr,
    CASE WHEN is_object_exists = 1 THEN bal.purpose_str ELSE ub.purpose_str END AS purpose_str,
    CASE WHEN req.conveyancing_type = 1 THEN 'Передача об’єкту з балансу на баланс'
        WHEN req.conveyancing_type = 2 THEN 'Передача об’єкту з балансу на баланс'
        WHEN req.conveyancing_type = 3 THEN 'Списання об’єкту з балансу шляхом зносу'
        WHEN req.conveyancing_type = 4 THEN 'Списання об’єкту з балансу шляхом приватизації'
        WHEN req.conveyancing_type = 5 THEN 'Постановка  новозбудованого об’єкту на баланс'
        ELSE 'Невідомий'
    END AS type_of_conveyancing
    FROM transfer_requests req
    LEFT JOIN unverified_balans ub ON req.balans_id = ub.id
    LEFT JOIN organizations org_to ON req.org_to_id = org_to.id
    LEFT JOIN organizations org_from ON req.org_from_id = org_from.id
    LEFT JOIN balans bal ON req.balans_id = bal.id
    LEFT JOIN buildings bui ON bal.building_id = bui.id
    LEFT JOIN buildings bui_un ON ub.building_id = bui_un.id
    WHERE request_id = @req_id"
    OnSelecting="SqlDataSourceTransferRequest_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="req_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceProjectTypes" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_doc_kind d
        INNER JOIN rozp_doc_kinds r ON d.id = r.kind_id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRights" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_balans_ownership_type d INNER JOIN transfer_actual_ownership_types a ON d.id = a.ownership_type_id" />


<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBalansPurpose" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_balans_purpose WHERE len(name) > 0 ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<asp:ObjectDataSource ID="ObjectDataSourceBalansPhoto" runat="server" SelectMethod="SelectFromTransferRequestFolder" 
    TypeName="ExtDataEntry.Models.FileAttachment">
    <SelectParameters>
        <asp:Parameter DefaultValue="TransferRequest" Name="scope" Type="String" />
        <asp:Parameter DefaultValue="" Name="tempGuid" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>



<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Зміна балансоутримувачів об'єктів" CssClass="pagetitle"/>  
</p>

    <asp:FormView runat="server" BorderStyle="None" ID="ConveyancingForm" DataSourceID="SqlDataSourceTransferRequest" EnableViewState="False" OnItemCreated="ConveyancingForm_OnItemCreated">
    <ItemTemplate>
        <p><asp:Label runat="server" ID="ASPxLabel19" Text='<%# Eval("type_of_conveyancing") %>' CssClass="reporttitle"/></p>
        <dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" ActiveTabIndex="0">
            <TabPages>
                <dx:TabPage Text="Проект акту" Name="RishCardProperties">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl1" runat="server">
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Інформація про об'єкт">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">
                                    <asp:HiddenField ID="HdnObjectId" ClientIDMode="Static" Value="" runat="server"  />
                                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                        <tr>
                                            <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Адреса" Width="180px" /></td>
                                            <td width="8px">&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabelObjAddress" runat="server" Text='<%# Eval("full_addr") %>'></dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr><td colspan="3" height="14px"/></tr>
                                        <tr>
                                            <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Призначення" /></td>
                                            <td width="8px">&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabelObjPurpose" runat="server" Text='<%# Eval("purpose_str") %>'></dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr><td colspan="3" height="14px"/></tr>
                                        <tr>
                                            <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Загальна площа (м²)" /></td>
                                            <td width="8px">&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabelObjTotalArea" runat="server" Text='<%# Eval("sqr_total") %>'></dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr><td colspan="3" height="14px"/></tr>
                                        <tr>
                                            <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Площа, що передається (м²)" /></td>
                                            <td width="8px">&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" DisplayFormatString="N" DecimalPlaces="2" Text='<%# Eval("conveyancing_area") %>'></dx:ASPxLabel>
                                            </td>
                                        </tr>
                                    </table> 
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelRozp" runat="server" HeaderText="Розпорядчі документии">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                        <tr>
                                            <td valign="top" width="100px">
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Номер документу">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td width="8px">
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxTextBox ID="RozpDoc" runat="server" Value='<%# Eval("rish_num") %>' ReadOnly="true" Width="700px"></dx:ASPxTextBox>                                              
                                            </td>
                                        </tr>
                                        <tr><td colspan="3" height="4px"/></tr>                                        
                                        <tr>
                                            <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Назва документу"></dx:ASPxLabel></td>
                                            <td width="8px">&nbsp;</td>
                                            <td>
                                                <dx:ASPxMemo ID="TextBoxRishName" ClientInstanceName="TextBoxRishName" runat="server" Width="700px" Height="80px" Value='<%# Eval("rishrozp_name") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Назва документу має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxMemo>
                                            </td>
                                        </tr>
                                        <tr><td colspan="3" height="4px"/></tr>
                                        <tr>
                                            <td valign="top" width="100px">
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Дата прийняття">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td width="8px">
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxDateEdit ID="DateEditRishDate" ClientInstanceName="DateEditRishDate" runat="server" Value='<%# Eval("rishrozp_date") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Дата прийняття має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr><td colspan="3" height="4px"/></tr>
                                        <tr>
                                            <td valign="top" width="100px">
                                                <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Тип Документу">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td width="8px">
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxComboBox ID="ComboRishRozpType" runat="server" Value='<%# Eval("rish_doc_kind_id") %>' ReadOnly="true"
                                                    ClientInstanceName="ComboRishRozpType" DataSourceID="SqlDataSourceProjectTypes" 
                                                    TextField="name" ValueField="id" ValueType="System.Int32" Width="700px">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Тип Документу має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <%
                                            if ((int)((System.Data.DataRowView)ConveyancingForm.DataItem)["obj_right_id"] > 0)
                                            {
                                         %>
                                        <tr><td colspan="3" height="4px"/></tr>
                                        <tr>
                                            <td valign="top" width="100px">
                                                Право</td>
                                            <td width="8px">
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxComboBox ID="ASPxComboBoxRight" runat="server" Value='<%# Eval("obj_right_id") %>' ReadOnly="true"
                                                    DataSourceID="SqlDataSourceRights" TextField="name" ValueField="id" ValueType="System.Int32" Width="700px">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Право має бути заповнений" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <%
                                            }
                                         %>
                                    </table>                                    
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        
                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelAkt" runat="server" HeaderText="Акт">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                        <tr>
                                            <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel62" runat="server" Text="Номер акту" /></td>
                                            <td>
                                                <dx:ASPxTextBox ID="ActNumber" runat="server" Width="170px" Value='<%# Eval("new_akt_num") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Номер акту має бути заповнен" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>                                            
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel63" runat="server" Text="Дата акту">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="ActDate" runat="server" Value='<%# Eval("new_akt_date") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Дата акту має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr><td colspan="5" height="4px"/></tr>
                                        <tr>
                                            <td width="100px" valign="top">
                                                <dx:ASPxLabel ID="ASPxLabel64" runat="server" Text="Сума за актом" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="ActSum" runat="server" Width="170px" DisplayFormatString="C" DecimalPlaces="2" Value='<%# Eval("new_akt_summa") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Сума за актом має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel65" runat="server" Text="Залишкова сума" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="ActResidualSum" runat="server" Width="170px" DisplayFormatString="C" DecimalPlaces="2" Value='<%# Eval("new_akt_summa_zalishkova") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Залишкова сума за актом має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>                                        
                                    </table>    
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelOrgTo" runat="server" HeaderText="Кому передається" Visible='<%# (int)Eval("conveyancing_type") != 3 %>'>
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server">
                                <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel61" runat="server" Text="Організація, якій передається об'єкт:" /> 
                                        </td>
                                    </tr>
                                    <tr><td height="4px"/></tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Value='<%# Eval("org_to_name") %>' ReadOnly="true" Width="700px"></dx:ASPxTextBox>                                             
                                        </td>
                                    </tr>
                                </table>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelOrgFrom" runat="server" HeaderText="Від кого передається" Visible='<%# (int)Eval("conveyancing_type") != 5 %>'>
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent5" runat="server">
                                <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Організація, від якої передається:" /> 
                                        </td>
                                    </tr>
                                    <tr><td height="4px"/></tr>
                                    <tr> 
                                        <td>   
                                            <dx:ASPxTextBox ID="ASPxTextBox3" runat="server" Value='<%# Eval("org_from_name") %>' ReadOnly="true" Width="700px"></dx:ASPxTextBox>  
                                        </td>
                                    </tr>
                                </table>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>


                        <dx:ASPxPopupControl ID="pcWarn" runat="server" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcWarn"
                            HeaderText="Попередження" AllowDragging="True" PopupAnimationType="None" EnableViewState="False">
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                    <dx:ASPxPanel ID="Panel1" runat="server">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent51" runat="server">
                                                Необхідно додати коментар.
                                            </dx:PanelContent>                                        
                                        </PanelCollection>
                                    </dx:ASPxPanel>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Коментар">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent6" runat="server">
                                    <dx:ASPxMemo ID="Comment" ClientInstanceName="Comment" runat="server" Width="810px" Height="80px" Value='<%# Eval("comment") %>'>
                                    </dx:ASPxMemo>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>

				<dx:TabPage Text="Фото / плани" Name="TabPhotos" >
					<ContentCollection>
						<dx:ContentControl ID="ContentControl4" runat="server">

							<dx:ASPxRoundPanel ID="PanelPhoto" runat="server" HeaderText="Фото / плани" EnableViewState="true">
								<ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
								<PanelCollection>
									<dx:PanelContent ID="PanelContent7" runat="server">


										<table border="0" cellspacing="0" cellpadding="0" width="990px">
										<tr>
										<td>

										<dx:ASPxCallbackPanel ID="ASPxCallbackPanelImageGallery" EnableViewState="true"
											ClientInstanceName="ASPxCallbackPanelImageGallery" runat="server" 
											OnCallback="ASPxCallbackPanelImageGallery_Callback">

											<SettingsLoadingPanel Enabled="false"/>
                            
											<PanelCollection>
												<dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
                                        
											<dx:ASPxImageGallery ID="imageGalleryDemo" runat="server" DataSourceID="ObjectDataSourceBalansPhoto"
												EnableViewState="false" 
												AlwaysShowPager="false" 
												PagerAlign="Center"
												ThumbnailHeight="190" ThumbnailWidth="190"
												SettingsFullscreenViewer-ShowCloseButton="true" 
												OnDataBound="imageGalleryDemo_DataBound" >

		<%--    pgv                             <SettingsFolder ImageCacheFolder="~\Thumb\" ImageSourceFolder="~\ImgContent\tmp\" />      --%>         
											  <SettingsFolder ImageCacheFolder="~\Thumb\"  /> 
                                        
												
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





		<%--                                <asp:UpdatePanel ID="updPanel" EnableViewState="true" runat="server" ChildrenAsTriggers="true">
											<ContentTemplate>--%>

										<dx:ASPxCallbackPanel ID="ContentCallback" runat="server" EnableViewState="true" Visible="false"
											ClientInstanceName="ContentCallback" OnCallback="ContentCallback_Callback">
                                    
											<PanelCollection>
												<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                            
		<%--                                            <asp:HiddenField ID="TempFolderIDField" runat="server" 
														OnValueChanged="TempFolderIDField_ValueChanged" />      --%>

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

		<%--                                    </ContentTemplate>
										</asp:UpdatePanel>--%>

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
    </ItemTemplate>
    </asp:FormView>
 
<dx:ASPxCallbackPanel ID="CPApply" ClientInstanceName="CPApply" runat="server" OnCallback="CPApply_Callback"></dx:ASPxCallbackPanel>
<dx:ASPxPanel ID="ASPxPanelApplyButtons" runat="server" Width="600px" Visible="true">
    <PanelCollection>
        <dx:PanelContent>
            <div>
                <div style="margin: 5px 5px 5px 0; height: 25px;">
                    <div style="float: left; padding-right: 5px;">
                        <dx:ASPxButton ID="ASPxButton2" ClientInstanceName="ASPxButtonConfirm" runat="server" Text="Підтвердити" AutoPostBack="false" CausesValidation="false"> 
                            <ClientSideEvents Click="function(s, e) { s.SetEnabled(false); ASPxButtonReject.SetEnabled(false); CPApply.PerformCallback('approve:'); }" />            
                        </dx:ASPxButton>
                    </div>
                    <div style="float: left;  padding-right: 5px;">
                        <dx:ASPxButton ID="ASPxButton4" ClientInstanceName="ASPxButtonReject" runat="server" Text="Відхилити" AutoPostBack="false" CausesValidation="true">
                            <ClientSideEvents Click="function (s, e) { if(Comment.GetValue() == null || Comment.GetValue().trim() == '') pcWarn.Show(); else { s.SetEnabled(false); ASPxButtonConfirm.SetEnabled(false); CPApply.PerformCallback('discard:'+Comment.GetValue().trim()); } }" />
                        </dx:ASPxButton>
                    </div>  
                    <div style="float: left;">
                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Повернутися до списку запитів" AutoPostBack="false" CausesValidation="false"> 
                            <ClientSideEvents Click="function(s, e) { window.location.href  = 'ConveyancingList.aspx'; }" />
                        </dx:ASPxButton>
                    </div>       
                </div>
            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>
</asp:Content>

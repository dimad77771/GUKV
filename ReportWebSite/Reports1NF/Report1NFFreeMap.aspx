<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFFreeMap.aspx.cs" Inherits="Reports1NF_Report1NFFreeMap"
    MasterPageFile="~/FreeShowPublic.master" Title="Мапа вільних приміщень" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script src="https://maps.api.2gis.ru/2.0/loader.js?pkg=full"></script>
<script type="text/javascript" src="../Scripts/PageScript.js"></script>



<script type="text/javascript" language="javascript">
	// <![CDATA[

	var reportId = <%= ReportID %>;
	var allPoints = <%= Converter.JsonConvertSerializeObject(AllPoints) %>;
	//console.log("allPoints", allPoints);
	var selected_fs_id = <%= selected_fs_id %>;


	window.onresize = function () {
		AdjustGridSizes();
	};

	function AdjustGridSizes() {
		//console.log(window.innerHeight);
		//alert(window.innerHeight);
		$("#map").height(window.innerHeight - 180);
		$("#map").width(window.innerWidth - 20);
		//$("#map").height(600);
		//console.log($("#map"));
		//alert(reportId);
		//; height:600px
	}

	function formatNumber(arg) {
		if (arg != null) {
			return arg.toFixed(2);
		} else {
			return "";
		}
	}

	function formatBool(arg) {
		if (arg != null) {
			if (arg == true) {
				return "Так";
			} else {
				return "Ні";
			}
		} else {
			return "";
		}
	}

	function openLink(fs_id) {
		for (n = 0; n < allPoints.length; n++) {
			var apoint = allPoints[n];
			if (apoint.fs_id == fs_id) break;
		}
		var url = 'Report1NFFreeShow.aspx?fs_id=' + apoint.fs_id;
		window.open(url, '_blank');
	}

	function openDocument(fs_id) {
		for (n = 0; n < allPoints.length; n++) {
			var apoint = allPoints[n];
			if (apoint.fs_id == fs_id) break;
		}

		var adoctext = '';
		if (isNotEmpty(apoint.current_stage_docnum)) {
			adoctext += "№ " + apoint.current_stage_docnum;
		}
		if (isNotEmpty(apoint.current_stage_docdate)) {
			if (adoctext != '') {
				adoctext += ' від ';
			}
			adoctext += formatDate(apoint.current_stage_docdate);
		}

		//map.closePopupOnClick = false;
		console.log("map.closePopupOnClick", map.closePopupOnClick);
		$.cookie('RecordID', apoint.fs_id);
		ASPxFileManagerPhotoFiles.Refresh();
		//console.log("PopupObjectPhotos.ShowAtPos", PopupObjectPhotos.ShowAtPos);
		PopupObjectPhotos.SetHeaderText("Документ " + adoctext);
		PopupObjectPhotos.ShowAtPos(50, 100);
        //PopupObjectPhotos.Show();
	}

	function isNotEmpty(arg) {
		return (arg != null && arg != "");
	}

	function formatDate(date) {
		if (!isNotEmpty(date)) {
			return '';
		} else {
			console.log(date);
			var str = date.substring(8, 10) + "." + date.substring(5, 7) + "." + date.substring(0, 4);
			return str;
		}
	}

	var map;
	window.onload = function() {
		jQuery(document).ready(function () {
			setTimeout(function () {
				AdjustGridSizes();
				
				DG.then(function () {
					if (selected_fs_id != null) {
						for (n = 0; n < allPoints.length; n++) {
							if (allPoints[n].fs_id == selected_fs_id) {
								var selected_apoint = allPoints[n];
							}
						}
					}

					var center_point = [50.402866, 30.655109];
					var zoom = 10.5;
					if (selected_apoint != null) {
						center_point = [selected_apoint.point1, selected_apoint.point2];
						zoom = 18;
					}

					map = DG.map('map', {
						center: center_point,
						zoom: zoom,
						closePopupOnClick: false,
					});

					var myIcon_1 = DG.icon({
						iconUrl: '../Styles/DGCustomization__marker_1b.png',
						//iconRetinaUrl: '../Styles/DGCustomization__marker_2.png',
						iconSize: [22, 68],
						//iconAnchor: [22, 94],
						popupAnchor: [-1, -26],
						//shadowUrl: 'my-icon-shadow.png',
						//shadowRetinaUrl: 'my-icon-shadow@2x.png',
						//shadowSize: [68, 95],
						//shadowAnchor: [22, 94]
					});
					var myIcon_2 = DG.icon({
						iconUrl: '../Styles/DGCustomization__marker_2b.png',
						//iconRetinaUrl: '../Styles/DGCustomization__marker_2.png',
						iconSize: [22, 68],
						//iconAnchor: [22, 94],
						popupAnchor: [-1, -26],
						//shadowUrl: 'my-icon-shadow.png',
						//shadowRetinaUrl: 'my-icon-shadow@2x.png',
						//shadowSize: [68, 95],
						//shadowAnchor: [22, 94]
					});


					for (n = 0; n < allPoints.length; n++) {
						var apoint = allPoints[n];
						//console.log("apoint", apoint);

						var html_need_zgoda = "";
						if (apoint.need_zgoda == '+') {
							html_need_zgoda =
								'<tr>' +
								'<td colspan="2" style="text-align:left; border:1px solid; padding:5px"><span style="color:#FFD926; font-weight:bold">Потребує погодження</span></td>' +
								'</tr>';
						}


						//apoint.full_address = "ГІДРОПАРК/ДОЛОБЕЦЬКИЙ ОСТРІВ 50 -МАЙ ГІДРОПАРК/ДОЛОБЕЦЬКИЙ ОСТРІВ 50 -МАЙ ГІДРОПАРК/ДОЛОБЕЦЬКИЙ ОСТРІВ 50 -МАЙ END";
						var html =
							//'<a href="Report1NFFreeShow.aspx?fs_id=' + apoint.fs_id + '" target="_blank"><p style="text-align:center; font-size:large; color:white">'
							'<a onclick="openLink(' + apoint.fs_id + ')" style="cursor:pointer"><table style="width:100%"><tr><td><p style="text-align:center; font-size:large; color:white">'
							+ apoint.full_address + '</p></td>' +
							'<td style="text-align:right"><span style="margin-left:40px; vertical-align:super; text-align:center; font-size:small; color:white">Додаткова інформація</span><img src="../Styles/ExternalLinkIcon.png" height="18" width="18" style="margin-left:5px"/></td><td style="width:20px"></td></tr></table></a>' +
							'<table style="border:1px solid; width:580px; margin-top:6px">' +

							html_need_zgoda +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Орендодавець</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.orandodatel + '</td>' +
							'</tr>' +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Балансоутримувач</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.org_name + '</td>' +
							'</tr>' +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Відповідальна особа балансоутримувача</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.vidpov_osoba + '</td>' +
							'</tr>' +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Загальна площа вільних приміщень, кв.м.</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + formatNumber(apoint.total_free_sqr) + '</td>' +
							'</tr>' +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Технічний стан</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.condition + '</td>' +
							'</tr>' +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Можливе використання вільного приміщення</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.possible_using + '</td>' +
							'</tr>' +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Період використання</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.period_used_name + '</td>' +
							'</tr>' +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Наявність рішень про проведення інвестиційного конкурсу</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.invest_solution + '</td>' +
							'</tr>' +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Включено до переліку №</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.include_in_perelik + '</td>' +
							'</tr>' +

							'';

						//if (isNotEmpty(apoint.current_stage_name)) {
						if (true) {
							html +=
								'<tr>' +
								'<td style="text-align:left; border:1px solid; padding:5px">Стан процесу передачі</td>' +
								'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.current_stage_name + '</td>' +
								'</tr>';
							if (isNotEmpty(apoint.current_stage_docnum) || isNotEmpty(apoint.current_stage_docdate)) {
								var adoctext = '';
								if (isNotEmpty(apoint.current_stage_docnum)) {
									adoctext += "№ " + apoint.current_stage_docnum;
								}
								if (isNotEmpty(apoint.current_stage_docdate)) {
									if (adoctext != '') {
										adoctext += ' від ';
									}
									adoctext += formatDate(apoint.current_stage_docdate);
								}
								if (apoint.current_stage_has_documents == '+') {
									adoctext += '<a onclick="openDocument(' + apoint.fs_id + ')" style="cursor:pointer">'
										+ '<span style="margin-left:20px; vertical-align:super; text-align:center; font-size:small; color:white">документ</span><img src="../Styles/ExternalLinkIcon.png" height="18" width="18" style="margin-left:2px"/>'
										+ '</a>';
								}
								html +=
									'<tr>' +
									'<td style="text-align:left; border:1px solid; padding:5px">№ та дата документа</td>' +
									'<td style="text-align:left; border:1px solid; padding:5px">' + adoctext + '</td>' +
									'</tr>';
								//console.log("apoint", apoint);
								//console.log("apoint.current_stage_has_documents", apoint.current_stage_has_documents);
							}
							//if (apoint.current_stage_has_documents == '+') {
							//	html +=
							//		'<tr>' +
							//		'<td style="text-align:left; border:1px solid; padding:5px">Документ</td>' +
							//		'<td style="text-align:left; border:1px solid; padding:5px">' + apoint.current_stage_name + '</td>' +
							//		'</tr>';
							//}
						}

						html += '</table>';


						var popup = DG.popup({
							maxWidth: 650,
							minWidth: 650,
							});
						popup.setContent(html);


						DG.marker([apoint.point1, apoint.point2], {
							//title: "aaaa\njjj",
							icon: (apoint.need_zgoda == '+' ? myIcon_2 : myIcon_1),
						})
						.addTo(map)
						//.bindPopup('Вы кликнули по мне!')
						.bindPopup(popup);

						if (apoint == selected_apoint) {
							//console.log("popup", popup);
							//console.log("popup.setContent", popup.setContent);
							//console.log("popup.openOn", popup.openOn);
							popup.setLatLng([apoint.point1, apoint.point2]).openOn(map);
							//map.openPopup(popup);
						}
					}
				});
			}, 500);
		});
	}


	//console.log($("map"));

	// ]]>
</script>

<script type="text/javascript">
</script>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<style type="text/css">
    .dg-popup_hidden_true
    {
        display: inherit;
    }
	</style>


<div id="map" style="width:900px"/>


<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True" 
	ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
	HeaderText="Документ" Modal="True" 
	PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" RenderMode="Lightweight" 
	PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
	<ContentCollection>
		<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

			<asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
				SelectMethod="Select" 
				TypeName="ExtDataEntry.Models.FileAttachment">
				<SelectParameters>
					<asp:Parameter DefaultValue="free_square_current_stage_documents" Name="scope" Type="String" />
					<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
				</SelectParameters>
			</asp:ObjectDataSource>

			<dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server" 
				ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
				<Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
				<SettingsFileList>
					<ThumbnailsViewSettings ThumbnailSize="180px" />
				</SettingsFileList>
				<SettingsEditing AllowDelete="false" AllowCreate="false" AllowDownload="true" AllowMove="false" AllowRename="false" />
				<SettingsFolders Visible="False" />
				<SettingsToolbar ShowDownloadButton="true" ShowPath="False" ShowFilterBox="false" />
				<SettingsUpload UseAdvancedUploadMode="True" Enabled="false" >
					<AdvancedModeSettings EnableMultiSelect="True" />
				</SettingsUpload>

				<SettingsDataSource FileBinaryContentFieldName="Image" 
					IsFolderFieldName="IsFolder" KeyFieldName="ID" 
					LastWriteTimeFieldName="LastModified" NameFieldName="Name" 
					ParentKeyFieldName="ParentID" />
			</dx:ASPxFileManager>

			<br />

			<dx:ASPxButton ID="ASPxButtonClose" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
				<ClientSideEvents Click="function(s, e) { PopupObjectPhotos.Hide(); }" />
			</dx:ASPxButton>

		</dx:PopupControlContentControl>
	</ContentCollection>
</dx:ASPxPopupControl>


</asp:Content>

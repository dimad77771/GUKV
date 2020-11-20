<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFFreeMap_new3.aspx.cs" Inherits="Reports1NF_Report1NFFreeMap"
    MasterPageFile="~/FreeShowPublic2.master" Title="Мапа вільних приміщень" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<link rel="stylesheet" href="https://js.arcgis.com/3.34/dijit/themes/claro/claro.css"/>
<link rel="stylesheet" href="https://js.arcgis.com/3.34/dojox/widget/ColorPicker/ColorPicker.css"/>
<link rel="stylesheet" href="https://js.arcgis.com/3.34/esri/css/esri.css"/>

<style>
	html, body, #ui-esri-map {
		width:   100%;
		height:  100%;
		margin:  0;
		padding: 0;
	}

	#ui-esri-dijit-geocoder {
		top:      20px;
		left:     70px;
		position: absolute;
		z-index:  3;
	}

	.esriPopup .titlePane {
		text-shadow: none;
	}

	.esriPopup .titleButton.next {
		right: 40px;
	}

	.esriPopup .titleButton.prev {
		right: 53px;
	}

	.demographicInfoContent {
		padding-top: 10px;
	}

	.demographicInnerSpacing {
		display: inline-block;
		width:   8px;
	}

	.demographicNumericPadding {
		width:      90px;
		display:    inline-block;
		text-align: right;
	}
</style>



<script type="text/javascript" src="../Scripts/PageScript.js"></script>


<script type="text/javascript" language="javascript">
	// <![CDATA[

	var reportId = <%= ReportID %>;
	var allPoints = <%= Converter.JsonConvertSerializeObject(AllPoints) %>;
	//console.log("allPoints", allPoints);
	var selected_fs_id = <%= selected_fs_id %>;

	


	var formatNumber = function (value, key, data) {
		var searchText = "" + value;
		var formattedString = searchText.replace(/(\d)(?=(\d\d\d)+(?!\d))/gm, "$1,");
		return formattedString;
	};

	var getCounty = function (value, key, data) {
		if (value.toUpperCase() !== "LOUISIANA") {
			return "County";
		} else {
			return "Parish";
		}
	};




	window.onresize = function () {
		AdjustGridSizes();
	};

	function AdjustGridSizes() {
		console.log(window.innerHeight);
		//alert(window.innerHeight);
		$("#ui-esri-map").height(window.innerHeight - 180);
		$("#ui-esri-map").width(window.innerWidth - 20);
		//$("#map").height(600);
		console.log($("#ui-esri-map"));
		//alert(reportId);
		//; height:600px

		$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI0_").hide();
		$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI5_IS").hide();
	}

	function formatNumber2(arg) {
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
	window.onload = function () {
		jQuery(document).ready(function () {
			setTimeout(function () {
				AdjustGridSizes();




				return;
				
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
					var myIcon_3 = DG.icon({
						iconUrl: '../Styles/DGCustomization__marker_3b.png',
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
							'<table style="border:1px solid; width:680px; margin-top:6px">' +

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
							'<td style="text-align:left; border:1px solid; padding:5px">Загальна площа об’єкта</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' + formatNumber2(apoint.total_free_sqr) + '</td>' +
							'</tr>' +

							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Технічний стан об’єкта</td>' +
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

						html +=
							'<tr>' +
							'<td style="text-align:left; border:1px solid; padding:5px">Унікальний код обєкту у ЕТС Прозорро-продажі</td>' +
							'<td style="text-align:left; border:1px solid; padding:5px">' +
							"<a target=\"_blank\" href=\"https://prozorro.sale/auction/" + apoint.prozoro_number + "\">" + apoint.prozoro_number + "</a>" +
							'</td>' +
							'</tr>' +
							'';


						html += '</table>';


						var popup = DG.popup({
							maxWidth: 750,
							minWidth: 750,
							});
						popup.setContent(html);


						DG.marker([apoint.point1, apoint.point2], {
							//title: "aaaa\njjj",
							icon: (apoint.prozoro_number != '' ? myIcon_3 : apoint.need_zgoda == '+' ? myIcon_2 : myIcon_1),
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
			}, 0);
		});
	}


	//console.log($("map"));

	// ]]>
</script>

    <script type="text/javascript" src="https://js.arcgis.com/3.34/"></script>
    <script type="text/javascript">
		var map;

		require([
			"esri/map", "esri/geometry/Point",
			"esri/symbols/SimpleMarkerSymbol", "esri/graphic",
			"dojo/_base/array", "dojo/dom-style", "dojox/widget/ColorPicker",
			"dojo/domReady!"
		], function (
			Map, Point,
			SimpleMarkerSymbol, Graphic,
			arrayUtils, domStyle, ColorPicker
		) {
			map = new Map("map", {
				basemap: "oceans",
				center: [20, 44],
				zoom: 6,
				minZoom: 2
			});

			//map.on("load", mapLoaded);

			function mapLoaded() {
				var points = [[19.82, 41.33], [16.37, 48.21], [18.38, 43.85], [23.32, 42.7], [16, 45.8], [19.08, 47.5], [12.48, 41.9], [21.17, 42.67], [21.43, 42], [19.26, 42.44], [26.1, 44.43], [12.45, 43.93], [20.47, 44.82], [17.12, 48.15], [14.51, 46.06], [12.45, 41.9]];
				var iconPath = "M24.0,2.199C11.9595,2.199,2.199,11.9595,2.199,24.0c0.0,12.0405,9.7605,21.801,21.801,21.801c12.0405,0.0,21.801-9.7605,21.801-21.801C45.801,11.9595,36.0405,2.199,24.0,2.199zM31.0935,11.0625c1.401,0.0,2.532,2.2245,2.532,4.968S32.4915,21.0,31.0935,21.0c-1.398,0.0-2.532-2.2245-2.532-4.968S29.697,11.0625,31.0935,11.0625zM16.656,11.0625c1.398,0.0,2.532,2.2245,2.532,4.968S18.0555,21.0,16.656,21.0s-2.532-2.2245-2.532-4.968S15.258,11.0625,16.656,11.0625zM24.0315,39.0c-4.3095,0.0-8.3445-2.6355-11.8185-7.2165c3.5955,2.346,7.5315,3.654,11.661,3.654c4.3845,0.0,8.5515-1.47,12.3225-4.101C32.649,36.198,28.485,39.0,24.0315,39.0z";
				var initColor = "#ce641d";
				arrayUtils.forEach(points, function (point) {
					var graphic = new Graphic(new Point(point), createSymbol(iconPath, initColor));
					map.graphics.add(graphic);
				});

				var colorPicker = new ColorPicker({}, "picker1");
				colorPicker.setColor(initColor);
				domStyle.set(colorPicker, "left", "500px");
				colorPicker.on("change", function () {
					var colorCode = this.hexCode.value;
					map.graphics.graphics.forEach(function (graphic) {
						graphic.setSymbol(createSymbol(iconPath, colorCode));
					});
				});
			}

			function createSymbol(path, color) {
				var markerSymbol = new esri.symbol.SimpleMarkerSymbol();
				markerSymbol.setPath(path);
				markerSymbol.setColor(new dojo.Color(color));
				markerSymbol.setOutline(null);
				return markerSymbol;
			}
		});
	</script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<style type="text/css">
    .dg-popup_hidden_true
    {
        display: inherit;
    }
	</style>


<div id="ui-esri-map"></div>
<div id="ui-esri-dijit-geocoder"></div>



<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True" 
	ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
	HeaderText="Документ" Modal="True" 
	PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
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
				<SettingsToolbar ShowDownloadButton="true" ShowPath="true" ShowFilterBox="false" />
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

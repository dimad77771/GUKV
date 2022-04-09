<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFFreeMap__OPN.aspx.cs" Inherits="Reports1NF_Report1NFFreeMap"
    MasterPageFile="~/FreeShowPublic.master" Title="Мапа вільних приміщень" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script src="http://www.openlayers.org/api/OpenLayers.js"></script>
<%--<script type="text/javascript" src="http://maplib.khtml.org/khtml.maplib/khtml_all.js"> </script>--%>
<%--<link rel="stylesheet" href="https://cdn.jsdelivr.net/gh/openlayers/openlayers.github.io@master/en/v6.12.0/css/ol.css" type="text/css">
<script src="https://cdn.jsdelivr.net/gh/openlayers/openlayers.github.io@master/en/v6.12.0/build/ol.js"></script>--%>
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

		$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI0_").hide();
		$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI5_IS").hide();
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
		//console.log("map.closePopupOnClick", map.closePopupOnClick);
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
			//console.log(date);
			var str = date.substring(8, 10) + "." + date.substring(5, 7) + "." + date.substring(0, 4);
			return str;
		}
	}

	var map;
	window.onload = function() {
		jQuery(document).ready(function () {
			setTimeout(function () {
				AdjustGridSizes();



				/*
				const iconFeature = new ol.Feature({
					geometry: new ol.geom.Point([0, 0]),
					name: 'Null Island',
					population: 4000,
					rainfall: 500,
				});
				const iconStyle = new ol.style.Style({
					image: new ol.style.Icon({
						anchor: [0.5, 46],
						anchorXUnits: 'fraction',
						anchorYUnits: 'pixels',
						src: '../Styles/DGCustomization__marker_2b.png',
					}),
				});
				iconFeature.setStyle(iconStyle);

				const vectorSource = new ol.source.Vector({
					features: [iconFeature],
				});
				const vectorLayer = new ol.source.Vector({
					source: vectorSource,
				});

				const rasterLayer = new ol.source.Tile({
					source: new ol.source.TileJSON({
						url: 'https://a.tiles.mapbox.com/v3/aj.1x1-degrees.json?secure=1',
						crossOrigin: '',
					}),
				});

				const target = document.getElementById('map');
				const map = new ol.Map({
					layers: [rasterLayer, vectorLayer],
					target: target,
					view: new ol.View({
						center: [0, 0],
						zoom: 3,
					}),
				});

				return;
				*/

				
				//DG.then(function () {
					if (selected_fs_id != null) {
						for (n = 0; n < allPoints.length; n++) {
							if (allPoints[n].fs_id == selected_fs_id) {
								var selected_apoint = allPoints[n];
							}
						}
					}

					//var center_point = [50.402866, 30.655109];
					//var center_point = [30.655109, 50.402866];
					//var zoom = 10.5;
					//if (selected_apoint != null) {
					//	center_point = [selected_apoint.point1, selected_apoint.point2];
					//	zoom = 18;
					//}

					/*
					map = DG.map('map', {
						center: center_point,
						zoom: zoom,
						closePopupOnClick: false,
					});
					*/

					var AutoSizeFramedCloud = OpenLayers.Class(OpenLayers.Popup.FramedCloud, {
						'autoSize': true,
						'panMapIfOutOfView': true,
					});
					
					var map = new OpenLayers.Map("map", {
						scrollWheelZoom: false,
					});
					map.addLayer(new OpenLayers.Layer.OSM());

					var center_point = new OpenLayers.LonLat(30.655109,50.402866)
						.transform(
							new OpenLayers.Projection("EPSG:4326"), // transform from WGS 1984
							map.getProjectionObject() // to Spherical Mercator Projection
					);	
					var zoom = 10.5;
					map.setCenter(center_point, zoom);

					var markers = new OpenLayers.Layer.Markers("Markers");
					map.addLayer(markers);

					map.addControls([
						//new OpenLayers.Control.MousePosition(),
						//new OpenLayers.Control.ScaleLine(),
						//new OpenLayers.Control.LayerSwitcher(),
						//new OpenLayers.Control.Permalink({ anchor: true })
					]);

					console.log("OpenLayers", OpenLayers);
					


					/*
					var map = new ol.Map({
						target: 'map',
						layers: [
							new ol.layer.Tile({
								source: new ol.source.OSM()
							})
						],
						view: new ol.View({
							center: ol.proj.fromLonLat([30.655109, 50.402866]),
							zoom: 10.5
						})
					});
					var markers = new ol.layer.markers("Markers");
						map.addLayer(markers);
					return;
					*/

					/*
					map.on('popupopen', function (e) {
						console.log('popupopen', e.popup);
						var html = e.popup.getContent();
						var s1 = '<span id="spanwait" style="display:none">...</span>';
						//console.log("html", html);
						if (html.indexOf(s1) >= 0) {
							var re = new RegExp('onclick="openLink\\((\\d+)\\)"');
							var result = html.match(re);
							console.log("result", result);
							if (result != null) {
								var fsid = result[1];
								//fsid = 5097;
								$.get('BalansFreeSquarePhotosPdf.aspx?id=' + fsid + '&formap=1', function (data) {
									if (data != "") {
										//console.log("data", data);
										html = html.replaceAll(s1, data);
										e.popup.options.maxWidth = 1050;
										e.popup.setContent(html);
									}
								});
							}
						}
					});
					*/

					/****
					var myIcon_1 = DG.icon({
						iconUrl: '../Styles/DGCustomization__marker_1b.png',
						iconSize: [22, 68],
						popupAnchor: [-1, -26],
					});
					var myIcon_2 = DG.icon({
						iconUrl: '../Styles/DGCustomization__marker_2b.png',
						iconSize: [22, 68],
						popupAnchor: [-1, -26],
					});
					var myIcon_3 = DG.icon({
						iconUrl: '../Styles/DGCustomization__marker_3b.png',
						iconSize: [22, 68],
						popupAnchor: [-1, -26],
					});
					****/

					
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
							'<a onclick="openLink(' + apoint.fs_id + ')" style="cursor:pointer"><table style="width:100%"><tr><td><p style="text-align:center; font-size:large; color:blue">'
							+ apoint.full_address + '</p></td>' +
							'<td style="text-align:right"><span style="margin-left:40px; vertical-align:super; text-align:center; font-size:small; color:blue">Додаткова інформація</span><img src="../Styles/ExternalLinkIcon.png" height="18" width="18" style="margin-left:5px"/></td><td style="width:20px"></td></tr></table></a>' +
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
							'<td style="text-align:left; border:1px solid; padding:5px">' + formatNumber(apoint.total_free_sqr) + '</td>' +
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

						var imgdiv = '<span id="spanwait" style="display:none">...</span>';
						html = '<table><tr><td>' + html + '</td>' + '<td>' + imgdiv + '</td>' + '</tr></table>';

						/*
						var popup = DG.popup({
							maxWidth: 750,// + 300,
							minWidth: 750,
						});
						popup.setContent(html);
						*/

						/*
						DG.marker([apoint.point1, apoint.point2], {
							icon: (apoint.prozoro_number != '' ? myIcon_3 : apoint.need_zgoda == '+' ? myIcon_2 : myIcon_1),
						})
						.addTo(map)
						.bindPopup(popup);

						if (apoint == selected_apoint) {
							popup.setLatLng([apoint.point1, apoint.point2]).openOn(map);
						}
						*/

						//const iconStyle = new OpenLayers.Style({
						//	image: new Icon({
						//		anchor: [0.5, 46],
						//		anchorXUnits: 'fraction',
						//		anchorYUnits: 'pixels',
						//		src: '../Styles/DGCustomization__marker_2b.png',
						//	}),
						//});

						//let customIcon = {
						//	iconUrl: '../Styles/DGCustomization__marker_2b.png',
						//	iconSize: [40, 40]
						//};

						//let myIcon = new OpenLayers.Icon({
						//	url: '../Styles/DGCustomization__marker_2b.png',
						//	//iconSize: [40, 40],
						//});
						//myIcon.setSize([40, 40]);
						//console.log("myIcon", myIcon);

						//var lonLat = new OpenLayers.LonLat(apoint.point2, apoint.point1)
						//	.transform(
						//		new OpenLayers.Projection("EPSG:4326"), // transform from WGS 1984
						//		map.getProjectionObject() // to Spherical Mercator Projection
						//);
						//let iconOptions = {
						//	title: 'company name',
						//	draggable: true,
						//}
						//markers.addMarker(new OpenLayers.Marker(lonLat, myIcon ));

						//iconSize: [22, 68],
						//	popupAnchor: [-1, -26],

						var size = new OpenLayers.Size([22, 68]);
						var offset = new OpenLayers.Pixel(0, 0);
						var myIcon_1 = '../Styles/DGCustomization__marker_1b.png';
						var myIcon_2 = '../Styles/DGCustomization__marker_2b.png';
						var myIcon_3 = '../Styles/DGCustomization__marker_3b.png';
						var myIcon = (apoint.prozoro_number != '' ? myIcon_3 : apoint.need_zgoda == '+' ? myIcon_2 : myIcon_1);
						var icon = new OpenLayers.Icon(myIcon, size, offset);

						var lonLat = new OpenLayers.LonLat(apoint.point2, apoint.point1)
							.transform(
								new OpenLayers.Projection("EPSG:4326"), // transform from WGS 1984
								map.getProjectionObject() // to Spherical Mercator Projection
						);

						//markers.addMarker(new OpenLayers.Marker(lonLat, icon));

						popupClass = AutoSizeFramedCloud;
						//popupContentHTML = "<div>This popup can't be panned to fit in view, so its popup text should fit inside the map. If it doens't, instead of expaning outside, it will simply make the content scroll. Scroll scroll scroll your boat, gently down the stream! Chicken chicken says the popup text is really boring to write. Did you ever see a popup a popup a popup did you ever see a popup a popup right now. With this way and that way and this way and that way did you ever see a popup a popup right now. I wonder if this is long enough. it might be, but maybe i should throw in some other content. <ul><li>one</li><li>two</li><li>three</li><li>four</li></ul>(get your booty on the floor) </div>";
						popupContentHTML = html;
						addMarker(map, markers, lonLat, popupClass, popupContentHTML, true, true, icon);
					}
				//});
			}, 500);
		});
	}


	/**
	 * Function: addMarker
	 * Add a new marker to the markers layer given the following lonlat, 
	 *     popupClass, and popup contents HTML. Also allow specifying 
	 *     whether or not to give the popup a close box.
	 * 
	 * Parameters:
	 * ll - {<OpenLayers.LonLat>} Where to place the marker
	 * popupClass - {<OpenLayers.Class>} Which class of popup to bring up 
	 *     when the marker is clicked.
	 * popupContentHTML - {String} What to put in the popup
	 * closeBox - {Boolean} Should popup have a close box?
	 * overflow - {Boolean} Let the popup overflow scrollbars?
	 */
	function addMarker(map, markers, ll, popupClass, popupContentHTML, closeBox, overflow, icon) {

		var feature = new OpenLayers.Feature(markers, ll);
		feature.closeBox = closeBox;
		feature.popupClass = popupClass;
		feature.data.popupContentHTML = popupContentHTML;
		feature.data.overflow = (overflow) ? "auto" : "hidden";
		feature.data.icon = icon;

		var marker = feature.createMarker();
		
		var markerClick = function (evt) {
			if (this.popup == null) {
				this.popup = this.createPopup(this.closeBox);
				map.addPopup(this.popup);
				this.popup.show();
			} else {
				this.popup.toggle();
				this.popup.updateSize();
			}
			currentPopup = this.popup;
			OpenLayers.Event.stop(evt);
		};
		marker.events.register("mousedown", feature, markerClick);

		markers.addMarker(marker);
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


<div id="map" style="width:900px"></div>
<table style="margin:0px 5px 0px 5px; height:34px">
	<tr>
		<td>
			<img src="../Styles/Map__marker_2b.png" />
		</td>
		<td>
			<span style="font-size:Larger;padding-left:5px;padding-right:75px;text-align:left;">не всі погодження (органу управління або культурної спадщини) отримано</span>
		</td>
		<td>
			<img src="../Styles/Map__marker_1b.png" />
		</td>
		<td>
			<span style="font-size:Larger;padding-left:5px;padding-right:75px;text-align:left;">всі погодження отримано</span>
		</td>
		<td>
			<img src="../Styles/Map__marker_3b.png" />
		</td>
		<td>
			<span style="font-size:Larger;padding-left:5px;padding-right:5px;text-align:left;">виставлено на ПРОЗОРРО</span>
		</td>
	</tr>
</table>



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

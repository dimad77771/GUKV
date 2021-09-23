<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFBalansMap__MBK.aspx.cs" Inherits="Reports1NF_Report1NFFreeMap"
    MasterPageFile="~/Report1NFBalansMap2.master" Title="Мапа вільних приміщень" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no">
    <!-- 
  ArcGIS API for JavaScript, https://js.arcgis.com
  For more information about the graphics_svg_path sample, read the original sample description at developers.arcgis.com.
  https://developers.arcgis.com/javascript/3/jssamples/graphics_svg_path.html  
  -->
  <title>SimpleMarkerSymbol with SVG Path - Simplified</title>
    <link rel="stylesheet" href="https://js.arcgis.com/3.34/dijit/themes/claro/claro.css">
    <link rel="stylesheet" href="https://js.arcgis.com/3.34/dojox/widget/ColorPicker/ColorPicker.css">
    <link rel="stylesheet" href="https://js.arcgis.com/3.34/esri/css/esri.css">
    <style>
      html, body, #map {
        height:100%;
        width:100%;
        margin:0;
        padding:0;
      }
      .dojoxColorPicker {
        position: absolute;
        top: 15px;
        right: 15px;
        -moz-box-shadow: 0 0 7px #888;
        -webkit-box-shadow: 0 0 7px #888;
        box-shadow: 0 0 7px #888;
      }
    </style>
    <script src="https://js.arcgis.com/3.34/"></script>
    <script>
		var reportId = <%= ReportID %>;
		var allPoints = <%= Converter.JsonConvertSerializeObject(AllPoints) %>;
		//console.log("allPoints", allPoints);
		var selected_fs_id = <%= selected_fs_id %>;
		var all_graphs = []


		$(function () {
			$("#dialog").dialog({
				autoOpen: false,
				show: {
					effect: "blind",
					duration: 200
				},
				hide: {
					effect: "explode",
					duration: 200
				},
				modal: false,
				//height: 750,
				width: 730,
				close: onClosePopup,
				//position: {
				//	my: "left top",
				//	at: "left bottom",
				//	of: button,
				//}
			});
		});

		function onClosePopup() {
			console.log("last_open_symbol", last_open_symbol);
			var symbol = last_open_symbol;
			if (symbol != null) {
				var url = symbol.url;
				url = url.replace("d.png", "b.png");
				symbol.setUrl(url);
				map.graphics.refresh();
			}
		}

		var last_open_symbol = null;
		var map;
		require([
			//"esri/map", "esri/geometry/Point",
			//"esri/symbols/SimpleMarkerSymbol", "esri/graphic",
			//"esri/layers/ArcGISDynamicMapServiceLayer",
			//"dojo/_base/array", "dojo/dom-style", "dojox/widget/ColorPicker",

			//"dojo/dom-construct", "esri/Color", "esri/dijit/Geocoder", "esri/dijit/Popup", "esri/InfoTemplate",

			//"esri/symbols/SimpleFillSymbol",
			////"esri/core/Collection",

			//"dojo/domReady!"

			"esri/map", "esri/geometry/Point",
			"esri/symbols/SimpleMarkerSymbol",
			"esri/symbols/PictureMarkerSymbol",
			"esri/graphic",
			"dojo/_base/array", "dojo/dom-style", "dojox/widget/ColorPicker",
			"esri/layers/ArcGISDynamicMapServiceLayer",
			"esri/InfoTemplate",
			"dojo/dom",
			"esri/layers/FeatureLayer",
			"dojo/domReady!"
			

		], function (
			Map, Point,
			SimpleMarkerSymbol, PictureMarkerSymbol, Graphic,
			arrayUtils, domStyle, ColorPicker, ArcGISDynamicMapServiceLayer,
				InfoTemplate, dom, FeatureLayer, string
			)
			{

			console.log("ArcGISDynamicMapServiceLayer", ArcGISDynamicMapServiceLayer);

			//var infoTemplate = new InfoTemplate();
			//infoTemplate.setTitle("Population");
			//infoTemplate.setContent("2007 :D");

			//var infoWindow = new InfoWindow({
			//	domNode: domConstruct.create("div", null, dom.byId("mapDiv"))
			//});

			var selected_apoint = null;
			if (selected_fs_id != null) {
				for (n = 0; n < allPoints.length; n++) {
					if (allPoints[n].fs_id == selected_fs_id) {
						var selected_apoint = allPoints[n];
					}
				}
			}


			var center_point = [30.539254, 50.53902];
			var zoom = 120.0;
			if (selected_apoint != null) {
				center_point = [selected_apoint.point2, selected_apoint.point1];
				zoom = 1.0;
			}


			map = new Map("map", {
				//basemap: "topo",
				center: center_point, // long, lat
				//zoom: zoom,
				sliderStyle: "small",
				//infoTemplate: infoTemplate,
				//infoWindow: infoWindow,
			});
			//map = new Map({
			//	basemap: "topo",
			//});


			//map.on("click", function (evt) {
			//	if (evt.graphic != null && evt.graphic.symbol != null) {
			//		var apoint = evt.graphic.symbol.apoint;

			//		map.infoWindow.setTitle("Coordinates");
			//		//map.infoWindow.setContent("lat/lon : " + evt.mapPoint.y + ", " + evt.mapPoint.x);
			//		var html = GetPopupContent(apoint);
			//		map.infoWindow.setContent(html);
			//		map.infoWindow.resize(950, 750);
			//		map.infoWindow.show(evt.screenPoint, map.getInfoWindowAnchor(evt.screenPoint));
			//	}
			//});

			map.on("click", function (evt) {
				if (evt.graphic != null && evt.graphic.symbol != null) {
					var symbol = evt.graphic;
					openSymbol(evt.graphic);
				}
			});


			function openSymbol(graphic, mode) {
				var symbol = graphic.symbol;
				var apoint = symbol.apoint;
				var n = symbol.n;
				var point = [apoint.point2, apoint.point1];

				//var current_zoom = map.getZoom();
				//console.log("current_zoom", current_zoom);
				//map.centerAndZoom(point, 1 / current_zoom).then(function (value) {
				map.centerAt(point).then(function (value) {
					var image = graphic.getShape().rawNode;

					console.log("graphic", graphic);
					console.log("getShape", graphic.getShape().rawNode);

					var url = symbol.url;
					url = url.replace("b.png", "d.png");
					symbol.setUrl(url);
					last_open_symbol = symbol;
					map.graphics.refresh();

					//var graphic = all_graphs[n];
					//console.log("graphic", graphic);
					//console.log("map.graphics", map.graphics);
					//map.graphics.add(graphic);

					//map.graphics.redraw();


					$("#dialog").dialog("option", "position", {
						//my: "top+20",
						//at: "bottom",

						my: "right-50",
						at: "left",

						of: $(image),
					});

					$("#dialog").dialog("option", "title", apoint.full_address);

					var html = GetPopupContent(apoint);
					$("#dialog").html(html);
					//!!! $("#dialog").dialog("open");
				});
			}


			//var template = new InfoTemplate();
			//template.setTitle("1");
			//template.setContent("2");



			var oilAndGasLayer = new ArcGISDynamicMapServiceLayer("https://mkk.kga.gov.ua/arcgis/rest/services/RastrCash/Topo2000/MapServer", {
				"id": "1",
				"opacity": 1.0,
				//mode: FeatureLayer.MODE_ONDEMAND,
				//outFields: ["STATE_NAME", "SUB_REGION", "STATE_ABBR"],
				//infoTemplate: infoTemplate,
				//infoTemplate: template,
			});	
			map.addLayer(oilAndGasLayer);
			//var oilAndGasLayer = new ArcGISDynamicMapServiceLayer("https://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Petroleum/KGS_OilGasFields_Kansas/MapServer", {
			//	"id": "oilAndGasLayer",
			//	"opacity": 0.75
			//});
			//oilAndGasLayer.setInfoTemplates({
			//	0: { infoTemplate: template }
			//});



			//console.log("map.getZoom()", map.getZoom());
			//map.setLevel(10);
			//console.log("map.getZoom()", map.getZoom());

			//map.infoWindow.resize(200, 75);


			//var featureLayer = new FeatureLayer("https://sampleserver6.arcgisonline.com/arcgis/rest/services/Census/MapServer/3", {
			//	mode: FeatureLayer.MODE_ONDEMAND,
			//	infoTemplate: template,
			//	outFields: ["STATE_NAME", "SUB_REGION", "STATE_ABBR"]
			//});
			//map.addLayer(featureLayer);



			//console.log("map", map);
			//console.log("map", map.View);


			//var view = new MapView({
			//	container: "map",
			//	map: map,
			//	center: [30.539254, 50.43902],
			//	zoom: 14,
			//	sliderStyle: "small",
			//});


			map.on("load", mapLoaded);

			function mapLoaded() {
				map.centerAndZoom(center_point, zoom);

				//var points = [[30.539254, 50.43902], [19.82, 41.33], [16.37, 48.21], [18.38, 43.85], [23.32, 42.7], [16, 45.8], [19.08, 47.5], [12.48, 41.9], [21.17, 42.67], [21.43, 42], [19.26, 42.44], [26.1, 44.43], [12.45, 43.93], [20.47, 44.82], [17.12, 48.15], [14.51, 46.06], [12.45, 41.9]];
				//var iconPath = "M24.0,2.199C11.9595,2.199,2.199,11.9595,2.199,24.0c0.0,12.0405,9.7605,21.801,21.801,21.801c12.0405,0.0,21.801-9.7605,21.801-21.801C45.801,11.9595,36.0405,2.199,24.0,2.199zM31.0935,11.0625c1.401,0.0,2.532,2.2245,2.532,4.968S32.4915,21.0,31.0935,21.0c-1.398,0.0-2.532-2.2245-2.532-4.968S29.697,11.0625,31.0935,11.0625zM16.656,11.0625c1.398,0.0,2.532,2.2245,2.532,4.968S18.0555,21.0,16.656,21.0s-2.532-2.2245-2.532-4.968S15.258,11.0625,16.656,11.0625zM24.0315,39.0c-4.3095,0.0-8.3445-2.6355-11.8185-7.2165c3.5955,2.346,7.5315,3.654,11.661,3.654c4.3845,0.0,8.5515-1.47,12.3225-4.101C32.649,36.198,28.485,39.0,24.0315,39.0z";
				//var initColor = "#ce641d";
				//arrayUtils.forEach(points, function (point) {
				//	var graphic = new Graphic(new Point(point), createSymbol(iconPath, initColor));
				//	map.graphics.add(graphic);
				//});

				var selected_graphic;
				var myIcon_1 = '../Styles/Map__marker_1b.png';
				var myIcon_2 = '../Styles/Map__marker_2b.png';
				var myIcon_3 = '../Styles/Map__marker_3b.png';
				for (n = 0; n < allPoints.length; n++) {
					var apoint = allPoints[n];
					var point = [apoint.point2, apoint.point1];

					var icon = (apoint.prozoro_number != '' ? myIcon_3 : apoint.need_zgoda == '+' ? myIcon_2 : myIcon_1);
					var pictureMarkerSymbol = new esri.symbol.PictureMarkerSymbol(icon, 22, 34);
					pictureMarkerSymbol.apoint = apoint;
					pictureMarkerSymbol.n = n;
					var graphic = new Graphic(new Point(point), pictureMarkerSymbol);
					all_graphs[n] = graphic;
					map.graphics.add(graphic);
					if (selected_apoint == apoint) {
						selected_graphic = graphic;
					}
				}

				setTimeout(function () {
					if (selected_graphic != null) {
						openSymbol(selected_graphic, "start");
					}
				}, 100);
				
			}
		});


		window.onresize = function () {
			AdjustGridSizes();
		};

		function GetPopupContent(apoint) {
			var html_need_zgoda = "";
			if (apoint.need_zgoda == '+') {
				html_need_zgoda =
					'<tr>' +
					'<td colspan="2" style="text-align:left; border:1px solid; padding:5px"><span style="color:#FFD926; font-weight:bold">Потребує погодження</span></td>' +
					'</tr>';
			}


			var html =
				'<div>' +
				'<table style="border:0px; width:680px; margin-top:2px">' +
				'<tr>' +
				'<td>' +
					'<a onclick="openLink(' + apoint.fs_id + ')" style="cursor:pointer"><table style="width:100%"><tr><td><p style="text-align:center; font-size:large; color:white">' + 
					apoint.full_address + '</p></td>' +
					'<td style="text-align:right"><span style="margin-left:40px; vertical-align:super; text-align:center; font-size:small; color:white">Додаткова інформація</span><img src="../Styles/ExternalLinkIcon.png" height="18" width="18" style="margin-left:5px"/></td><td style="width:20px"></td></tr></table></a>' +
				'</td>' +
				'</tr>' +
				'</table>' +

				'<table style="border:1px solid; width:680px; margin-top:2px; color:white; border-collapse:collapse">' +
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


			html += '</table>'
			html += '</div>'

			return html;
		}

		function AdjustGridSizes() {
			console.log(window.innerHeight);
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

		$(document).ready(function () {
			setTimeout(function () {
				AdjustGridSizes();
			}, 500);
		});



	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server" class="claro">
    <div id="map"></div>

	<div id="dialog" title="Basic dialog" style="background-color:#696969">
		<p>This is an animated dialog which is useful for displaying information. The dialog window can be moved, resized and closed with the &apos;x&apos; icon.</p>
	</div>

	<div id="infowindow" style="display:none">
	</div>

  
</asp:Content>

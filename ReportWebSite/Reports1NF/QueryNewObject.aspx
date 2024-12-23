<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryNewObject.aspx.cs" Inherits="Reports1NF_Report1NFFreeMap"
    MasterPageFile="~/FreeShowPublic.master" Title="Мапа вільних приміщень" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
	
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/OpenLayers.js") %>"></script>
<%--<script type="text/javascript" src="https://www.openlayers.org/api/OpenLayers.js"></script>--%>



<script type="text/javascript" language="javascript">
	// <![CDATA[

	var reportId = <%= ReportID %>;
	var allPoints = <%= Converter.JsonConvertSerializeObject(AllPoints) %>;
	//console.log("allPoints", allPoints);
	var selected_fs_id = <%= selected_fs_id %>;
	var selected_lon = <%= selected_lon %>;
	var selected_lat = <%= selected_lat %>;
	var is_lonlat = (selected_lon != null && selected_lat != null)

	window.onresize = function () {
		AdjustGridSizes();
	};

	function AdjustGridSizes() {
		//console.log(window.innerHeight);
		//alert(window.innerHeight);
		$("#map").height(window.innerHeight - 100);
		$("#map").width(window.innerWidth - 20);
		//$("#map").height(600);
		//console.log($("#map"));
		//alert(reportId);
		//; height:600px

		$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI0_").hide();
		$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI5_IS").hide();
	}

	function OnCallbackComplete(e, d) {
		//console.log('OnCallbackComplete', e);
		if (d.result == "") {
			$('#error_text').hide()
			PopupObjectPhotos.Hide();
			alert("Заявку надіслано успішно");
		} else {
			$('#error_text').show()
			errorText.SetText(d.result)
		}
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

	function onMapClick(e) {
		console.log('e', e);
	}

	var map;
	window.onload = function() {
		jQuery(document).ready(function () {
			$("#panelTopFreeShowPublic").hide();
			$("#ASPChangeMapSystem").hide();
			$("#error_text").hide();

			setTimeout(function () {
				AdjustGridSizes();

				//Set up a click handler
				OpenLayers.Control.Click = OpenLayers.Class(OpenLayers.Control, {
					defaultHandlerOptions: {
						'single': true,
						'double': false,
						'pixelTolerance': 0,
						'stopSingle': false,
						'stopDouble': false
					},

					initialize: function (options) {
						this.handlerOptions = OpenLayers.Util.extend(
							{}, this.defaultHandlerOptions
						);
						OpenLayers.Control.prototype.initialize.apply(
							this, arguments
						);
						this.handler = new OpenLayers.Handler.Click(
							this, {
							'click': this.trigger
						}, this.handlerOptions
						);
					},

					trigger: function (e) {
						console.log('e',e)
						var lonlat = map.getLonLatFromViewPortPx(e.xy)

						lonlat.transform(
							new OpenLayers.Projection("EPSG:900913"),
							new OpenLayers.Projection("EPSG:4326")
						);

						//var url = "https://www.openstreetmap.org/query?lat=" + lonlat.lat + "&lon=" + lonlat.lon;
						var url = "http://localhost:6670/Reports1NF/QueryNewObject.aspx?lat=" + lonlat.lat + "&lon=" + lonlat.lon;
						console.log("url4", url);
						console.log("currentLogLat", currentLogLat);
						currentLogLat.Set("lat", lonlat.lat)
						currentLogLat.Set("lon", lonlat.lon)

						if (!is_lonlat) {
							PopupObjectPhotos.Show();
						}

						//var popup_point = new OpenLayers.LonLat(e.x, e.y);
						//var anchor = { 'size': new OpenLayers.Size(220, 330), 'offset': new OpenLayers.Pixel(10, 10) };
						////var anchor = { };
						//var contentString = "111";

						//popup = new OpenLayers.Popup.Anchored("fence",
						//	popup_point,
						//	new OpenLayers.Size(200, 200),
						//	contentString,
						//	anchor,
						//	false);

						//popup.autoSize = true;
						//map.addPopup(popup);
						//console.log('map.addPopup', map.addPopup)
					}

				});

				var options = {
					controls: [
						new OpenLayers.Control.Navigation({ zoomWheelEnabled: true }),	//defaultClick: onMapClick
						new OpenLayers.Control.PanZoomBar(),
						new OpenLayers.Control.Geolocate(),
						new OpenLayers.Control.SelectFeature(),
						
						

						//new OpenLayers.Control.Attribution()
					]
				};


				map = new OpenLayers.Map("map", options);
				map.addLayer(new OpenLayers.Layer.OSM());

				var click = new OpenLayers.Control.Click();
				map.addControl(click);
				click.activate();


				if (is_lonlat) {
					var lonLat3 = new OpenLayers.LonLat(selected_lon, selected_lat)
						.transform(
							new OpenLayers.Projection("EPSG:4326"),
							map.getProjectionObject()
						);
					var zoom = 18;
					map.setCenter(lonLat3, zoom);

					var markers = new OpenLayers.Layer.Markers("Markers");
					map.addLayer(markers);
					var size = new OpenLayers.Size(21, 25);
					var offset = new OpenLayers.Pixel(-(size.w / 2), -size.h);
					var icon = new OpenLayers.Icon('Scripts\\img\\marker.png', size, offset);
					markers.addMarker(new OpenLayers.Marker(lonLat3));

				} else {
					var lonLat = new OpenLayers.LonLat(30.5317, 50.4527)
						.transform(
							new OpenLayers.Projection("EPSG:4326"),
							map.getProjectionObject()
						);
					var zoom = 12;
					map.setCenter(lonLat, zoom);

					navigator.geolocation.getCurrentPosition((position) => {
						var lonLat2 = new OpenLayers.LonLat(position.coords.longitude, position.coords.latitude)
							.transform(
								new OpenLayers.Projection("EPSG:4326"),
								map.getProjectionObject()
							);
						var zoom2 = 18;
						map.setCenter(lonLat2, zoom2);
					});
				}


				
			}, 100);
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



<div id="map" style="width:900px">
<%--	<div>
		<form action="/">
			<table border="0" cellspacing="0" cellpadding="2">
				<tr>
					<td> <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Дата останньої актуалізації даних:" /> </td>
					<td> &nbsp; &nbsp; </td>
					<td> <dx:ASPxTextBox ID="EditReportDueDate"  ClientInstanceName="clEditReportDueDate" runat="server" ReadOnly="true" /> </td>
				</tr>
			</table>
		</form>
	</div>--%>
</div>

 


<table style="margin:0px 5px 0px 5px; height:34px; display:none">
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
	HeaderText="Заявка" Modal="True" 
	PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"  
	PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="900px" >
	<SettingsAdaptivity Mode="Always" MinWidth="90%" VerticalAlign="WindowCenter" />

	<ContentCollection>
		<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

					<dx:ASPxFormLayout runat="server" ID="formLayout">
                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="576" />
                        <Items>
                            <dx:LayoutGroup ColCount="1" GroupBoxDecoration="None" Paddings-Padding="0" Paddings-PaddingTop="5">
                                <Items>
                                    <dx:LayoutItem Caption="Email">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxTextBox runat="server" ID="tbEmail">
                                                    <ValidationSettings RequiredField-IsRequired="false" Display="Dynamic" />
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Примітки" Width="100%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxMemo runat="server" ID="mNotes" Rows="6" Text="">
                                                </dx:ASPxMemo>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                        </Items>
                    </dx:ASPxFormLayout>

					<dx:ASPxHiddenField ID="currentLogLat" ClientInstanceName="currentLogLat" runat="server" /> 


			<%--<table border="0" cellspacing="0" cellpadding="2" style="width:100%">
				<tr>
					<td> 
						<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Вкажіть ваш email:" /> 
					</td>
				</tr>
				<tr>
					<td> 
						<dx:ASPxTextBox ID="EditEmail" ClientInstanceName="clEditEmail" runat="server">

						</dx:ASPxTextBox>
					</td>
				</tr>
			</table>--%>

			<br />
			<div style="margin-bottom:5px" id="error_text">
				<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
				<dx:ASPxLabel ID="errorText" ClientInstanceName="errorText" runat="server" Text="" ForeColor="Red" /> 
			</div>
			<div>

				<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>

				<dx:ASPxButton ID="ASPxButtonSumbit" runat="server" Text="Надіслати заявку" AutoPostBack="false" HorizontalAlign="Center" >
					<ClientSideEvents Click="function(s, e) { cbSumbit.PerformCallback(); }" />
				</dx:ASPxButton>


				<dx:ASPxCallback ID="cbSumbit" ClientInstanceName="cbSumbit" runat="server" OnCallback="cbSumbit_Callback" >
					<ClientSideEvents CallbackComplete="OnCallbackComplete" BeginCallback="function(s, e) { $('#error_text').hide(); }"  />
				</dx:ASPxCallback>

				<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>

				<dx:ASPxButton ID="ASPxButtonClose" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
					<ClientSideEvents Click="function(s, e) { PopupObjectPhotos.Hide(); }" />
				</dx:ASPxButton>
			</div>

			

		</dx:PopupControlContentControl>
	</ContentCollection>
</dx:ASPxPopupControl>


</asp:Content>

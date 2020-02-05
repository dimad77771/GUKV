using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

public partial class GoogleMapsSnapIn : System.Web.UI.UserControl
{
    public const string DefaultLanguage = "en";
    public const int DefaultZoom = 16;
    public const string DefaultWidth = "400px";
    public const string DefaultHeight = "400px";
    public const string DefaultEmptyAddressMessage = "Choose a Street Address to Display Map";
    public const string DefaultGeocodeErrorMessage = "Geocode was not successful for the following reason";

    public string Address { get; set; }

    [DefaultValue(DefaultLanguage)]
    public string Language { get; set; }

    [DefaultValue(DefaultWidth)]
    public string Width { get; set; }

    [DefaultValue(DefaultHeight)]
    public string Height { get; set; }

    [DefaultValue(DefaultZoom)]
    public int Zoom { get; set; }

    [DefaultValue(DefaultEmptyAddressMessage)]
    public string EmptyAddressMessage { get; set; }

    [DefaultValue(DefaultGeocodeErrorMessage)]
    public string GeocodeErrorMessage { get; set; }

    public GoogleMapsSnapIn()
    {
        Width = DefaultWidth;
        Height = DefaultHeight;
        Language = DefaultLanguage;
        Zoom = DefaultZoom;
        EmptyAddressMessage = DefaultEmptyAddressMessage;
        GeocodeErrorMessage = DefaultGeocodeErrorMessage;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude("GoogleMapsAPI", "https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&language=" + Language);
        Page.ClientScript.RegisterClientScriptInclude("jQuery-1.4.1", Page.ResolveClientUrl("~/Scripts/jquery-1.4.1.js"));
        Page.ClientScript.RegisterClientScriptBlock(typeof(GoogleMapsSnapIn), "GoogleMapsSnapIn",

            @"<script type=""text/javascript"">
function CreateMarker(map, lat, lng, title, html) {
    var marker = new google.maps.Marker({
        map: map,
        position: new google.maps.LatLng(lat,lng),
        title: title,
    });
    if (html) {
        var infowindow = new google.maps.InfoWindow({
            content: html
        });
        google.maps.event.addListener(marker, 'click', function() {
               
            infowindow.open(map, marker);
        });
    }
}
function ShowMultipleLocations(center, items) {
    if (items.length == 0)
        return;
    var location = new google.maps.LatLng(center.lat,center.lng);
    var mapOptions = {
        zoom: 10,
        center: location,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    var map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
    var index;
    for (index = 0; index < items.length; index++) {
        CreateMarker(map, items[index].lat, items[index].lng, items[index].tooltip, items[index].html);
    }
    $('#map_address_empty').hide();
    $('#map_canvas').fadeIn('slow');
}
function ShowGeolocation(lat, lng, title, html) {
    var location = new google.maps.LatLng(lat,lng);
    var mapOptions = {
        zoom: " + Zoom + @",
        center: location,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    var map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
    CreateMarker(map, lat, lng, title, html);
    $('#map_address_empty').hide();
    $('#map_canvas').fadeIn('slow');
}
function ShowAddress(address, title, html) {
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode( { 'address': address }, function(results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var mapOptions = {
                zoom: " + Zoom + @",
                center: results[0].geometry.location,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            var map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
            CreateMarker(map, results[0].geometry.location.lat(), results[0].geometry.location.lng(),
                title, html);
            $('#map_address_empty').hide();
            $('#map_canvas').fadeIn('slow');
        } else {
            alert('" + GeocodeErrorMessage.Replace("'", "\\'") + @": ' + status);
        }
    });
}
</script>"
        );
    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);

        writer.Write(string.Format(
            @"<div id=""map_address_empty"" style=""width:{0};height:{1};text-align:center;vertical-align:middle;""><span style=""margin-top:50%;display:inline-block;"">{2}</span></div><div id=""map_canvas"" style=""width:{0};height:{1};display:none;""></div>", 
            Width, Height, EmptyAddressMessage));

        if (!string.IsNullOrWhiteSpace(Address))
        {
            writer.Write(
                @"<script type=""text/javascript"">

                $(document).ready(function() {
                    ShowAddress('" + Address + @"');
                });

                </script>"
            );
        }
    }
}
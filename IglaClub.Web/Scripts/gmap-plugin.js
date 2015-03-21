(function($) {
    $.fn.initializeMap = function(params) {
        var mapCanvasId = $(this).attr("id");
        var options = params;
        var latAndLng = utils.getLatLng(options.coordinates);
        var position = new google.maps.LatLng(latAndLng[0], latAndLng[1]);
        var markers = [];
        var initMap = function () {
            var mapOptions = {
                zoom: 15,
                center: position,
                mapTypeControlOptions: {
                    mapTypeIds: ['coordinate', google.maps.MapTypeId.ROADMAP],
                    style: google.maps.MapTypeControlStyle.DROPDOWN_MENU
                }
            };
            
            var map = new google.maps.Map(document.getElementById(mapCanvasId), mapOptions);
            //var defaultBounds = new google.maps.LatLngBounds(
            //    new google.maps.LatLng(-33.8902, 151.1759),
            //    new google.maps.LatLng(-33.8474, 151.2631));
            //map.fitBounds(defaultBounds);
            //map.setZoom(16);
            
            var marker = new google.maps.Marker({
                map: map,
                title: 'Club\'s location',
                position: position
            });

            markers.push(marker);
            
        };
        google.maps.event.addDomListener(window, 'load', initMap);
    };
    var utils = {
        setHiddenCoordinates: function (location) {
            $("#coords").val(location);
        },
        getLatLng : function(input) {
            var coords = [];
            var withoutParenthesis = input.substring(1, input.length - 1);
            var latAndLong = withoutParenthesis.split(',');
            if (latAndLong.length != 2) {
                return getDefaultCoordinates();
            }
            coords.push(latAndLong[0].trim());
            coords.push(latAndLong[1].trim());
            return coords;
        },
        getDefaultCoordinates : function() {
            return [-33.8902, 151.1759];
        },
        //markerImage : function() {
        //    var image = {
        //        url: place.icon,
        //        size: new google.maps.Size(71, 71),
        //        origin: new google.maps.Point(0, 0),
        //        anchor: new google.maps.Point(17, 34),
        //        scaledSize: new google.maps.Size(25, 25)
        //    };
        //    return image;
        //}
    };
})(jQuery);
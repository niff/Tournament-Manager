(function ($) {
    $.fn.initializePairsEdit = function (params) {
        var options = params;
        var initFormUrl = options.dataUrl;
        var tournamentId = options.tournamentId;

        $('.removePairLink').on("click", function () {
            var tid = $(this).data("tournament-id");
            var pid = $(this).data("pair-id");
            utils.removePair(tid, pid, initFormUrl);
        });
        $('#pairSearchSubmit').on("click", function() {
            var user1 = $('#user1').val();
            var user2 = $('#user2').val();
            if (user1 == '' || user2 == '') {
                alert("User is not chosen");
                return false;
            }
            if (user1 == user2) {
                alert("Choose two different users");
                return false;
            }
            $.ajax({
                url: "/Tournament/AddPair",
                type: "POST",
                data: { user1: user1, user2: user2, tournamentId: tournamentId },
                success: function(data) {
                    utils.refreshParticipants(initFormUrl);
                },
                error: function() {
                    alert("ajax failure");
                }
            });
            utils.clearHiddenUsers();
        });
    };
    var utils = {
        removePair: function (tId, pairId, initFormUrl) {
            $('.removePairLink').attr('href', '');
            $.ajax({
                url: "/Tournament/RemovePair",
                type: "POST",
                async: false,
                data: { tournamentId: tId, pairId: pairId },
                success: utils.refreshParticipants(initFormUrl)
            });
        },
        refreshParticipants: function (dataUrl) {
            $('#participantsDiv').load(dataUrl);
        },
        clearHiddenUsers: function () {
            $('.hidden.user').val('');
        }
    };
})(jQuery);
function toggleFullScreen() {
    if (!document.fullscreenElement &&    // alternative standard method
                !document.mozFullScreenElement && !document.webkitFullscreenElement) {  // current working methods
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    } else {
        if (document.cancelFullScreen) {
            document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
            document.webkitCancelFullScreen();
        }
    }
}

$(function () {
    var countdownIntervalSet = false;

    // Respond to navigation by URL hash
    window.onhashchange = doHashChange;

    // Redirect index page to load dashboard
    if (location.hash == "") {
        location.hash = "#/Home/Dashboard";
    } else {
        // All other pages just load the content over ajax
        doNavigation();
    }

    function doNavigation() {
        var url = location.hash.slice(1);
        //        if (!window.serverType || window.serverType != 'Development') {
        // Show 'please wait' spinner while loading. Disabled for local demo.
        $('.please-wait').show();
        //        }
        $('#container').load(url, null, finishNavigation);
    }

    $('a.administer-action').live('click', function (e) {
        if ($(this).attr('href') == "#") {
            e.preventDefault();
            $('.please-wait').show();
            var thisDose = { DoseID: $('#DoseID').val(), ScheduleID: $('#ScheduleID').val(), PrescriptionID: $('#PrescriptionID').val(), RecordedByUserID: $('#userID').val(), RecordedDateTime: new Date(), DoseUnits: $(this).attr("dose-result"), ResultCode: $(this).attr('result-id'), Side: $(this).attr('side') };
            $.ajax({
                type: "POST",
                url: "/Dose/Record",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(thisDose),
                success: function (data) {
                    // go to new location
                    location.hash = data.hash;
                }
            });
        }
    });

    function doHashChange(e) {
        // Check to see if it's navigation within the same page
        var oldURL = e.oldURL.split('?');
        var newURL = e.newURL.split('?');

        if (oldURL[0] === newURL[0]) {
            // Still on the same page, just show the right div panel
            if (newURL.length > 1) {
                $('.view').hide().filter('.' + newURL[1]).show();
            }
            else {
                $('.view').hide().filter('.main').show();
            }
        } else {
            doNavigation();
        }
    }

    function finishNavigation() {
        // Hide the ajax loading indicator
        $('.please-wait').hide();

        // If we're anywhere but the home page, show the Go Back button
        if (location.hash == "") {
            $('.go-back').hide();
        } else {
            $('.go-back').show();
        }

        // Update login info
        var userID = $('#userID');
        if (userID.attr('logged-in-as') != undefined) {
            $('#logged-in-as').text(userID.attr('logged-in-as'));
            $('#login-button').hide();
            $('#logout-button').show();
        } else {
            $('#logged-in-as').text('Not logged in.');
            $('#auto-logout').text('');
            $('#login-button').show();
            $('#logout-button').hide();
        }

        if (userID.attr('logged-in-as') != undefined) {
            //Create Countdown Function if User is logged-in
            function countdownFunction() {
                var timerEnded = false;
                if (countdownIntervalSet == true) {
                    clearInterval(countdownInterval);
                    countDownIntervalSet = false;
                }
                else {
                    if (typeof countdownInterval !== "undefined") {
                        clearInterval(countdownInterval);
                    }
                }
                function integerCheck() {
                    firstMinVal = parseInt(firstMinVal);
                    firstSecVal = parseInt(firstSecVal);
                    if (firstMinVal < 10) {
                        firstMinVal = '0' + firstMinVal;
                    }
                    if (firstSecVal < 10) {
                        firstSecVal = '0' + firstSecVal;
                    }
                }
                function beginTimer() {
                    if (firstSecVal == 0) {
                        firstSecVal = 60;
                        if (firstMinVal == 0) {
                            timerEnded = true;
                            clearInterval(countdownInterval);
                            countdownIntervalSet = false;
                            window.location.href = '#/Home/Login';
                        }
                        else {
                            firstMinVal = firstMinVal - 1;
                        }
                    }
                    if (timerEnded != true) {
                        firstSecVal = firstSecVal - 1;
                        integerCheck();
                        innerSpan.html(firstMinVal + ':' + firstSecVal);
                    }
                }
                logoutSpan = $('span#auto-logout'),
                firstMinVal = 1,
                firstSecVal = 59;
                integerCheck();
                logoutSpan.html('Auto logout in ');
                innerSpan = $('<span></span>').html(firstMinVal + ':' + firstSecVal).appendTo($('span#auto-logout'));
                countdownInterval = setInterval(function () {
                    beginTimer()
                },
                    1000);
                countdownIntervalSet = true;
            }
            countdownFunction();
        }

        // For demo purposes, make the header panel clickable to go into full-screen view
        $('.header-panel').click(toggleFullScreen);
    }

    $('.go-back').click(function () {
        window.history.back();
    });

    $('#tap-to-enter').click(toggleFullScreen);
    window.onresize = checkFullScreen;
    checkFullScreen();
});

function checkFullScreen() {
    if (!document.fullscreenElement &&
      !document.mozFullScreenElement && !document.webkitFullscreenElement) {
        $('#container').hide();
        $('#tap-to-enter').show();
    } else {
        $('#container').show();
        $('#tap-to-enter').hide();
    }
}
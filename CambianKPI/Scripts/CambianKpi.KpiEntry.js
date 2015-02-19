$(function () {
    //Added for IE8, to create a forEach function if it doesn't exist
    if (!Array.prototype.forEach) {

        Array.prototype.forEach = function (callback, thisArg) {

            var T, k;

            if (this == null) {
                throw new TypeError(' this is null or not defined');
            }

            // 1. Let O be the result of calling ToObject passing the |this| value as the argument.
            var O = Object(this);

            // 2. Let lenValue be the result of calling the Get internal method of O with the argument "length".
            // 3. Let len be ToUint32(lenValue).
            var len = O.length >>> 0;

            // 4. If IsCallable(callback) is false, throw a TypeError exception.
            // See: http://es5.github.com/#x9.11
            if (typeof callback !== "function") {
                throw new TypeError(callback + ' is not a function');
            }

            // 5. If thisArg was supplied, let T be thisArg; else let T be undefined.
            if (arguments.length > 1) {
                T = thisArg;
            }

            // 6. Let k be 0
            k = 0;

            // 7. Repeat, while k < len
            while (k < len) {

                var kValue;

                // a. Let Pk be ToString(k).
                //   This is implicit for LHS operands of the in operator
                // b. Let kPresent be the result of calling the HasProperty internal method of O with argument Pk.
                //   This step can be combined with c
                // c. If kPresent is true, then
                if (k in O) {

                    // i. Let kValue be the result of calling the Get internal method of O with argument Pk.
                    kValue = O[k];

                    // ii. Call the Call internal method of callback with T as the this value and
                    // argument list containing kValue, k, and O.
                    callback.call(T, kValue, k, O);
                }
                // d. Increase k by 1.
                k++;
            }
            // 8. return undefined
        };
    }

    //Added for IE8 to recognise IndexOf function
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (elt /*, from*/) {
            var len = this.length >>> 0;

            var from = Number(arguments[1]) || 0;
            from = (from < 0)
         ? Math.ceil(from)
         : Math.floor(from);
            if (from < 0)
                from += len;

            for (; from < len; from++) {
                if (from in this &&
          this[from] === elt)
                    return from;
            }
            return -1;
        };
    }

    //Check if form is editable at all to start with
    if (formEditable == false) {
        $('.editable').each(function () {
            $(this).removeClass("editable").attr("data-uneditable", "approved");
        });

        $('.kpi-score[data-uneditable="calc"], .kpi-score[data-uneditable="central"]').each(function () {
            $(this).attr('data-uneditable', 'approved');
        });
    };

    //add specific class to all cells with neg number
    $('.kpi-score').each(function () {
        var thisCell = $(this);
        var thisText = thisCell.html();
        if (thisText.indexOf("(") != -1) {
            thisCell.addClass("neg");
        }
    });

    //---------------------
    var thisCell = null,
        approvalColumnShowing = false,
        loggedInAs = "siteManager";

    $('.approved-label').each(function () {
        thisLabel = $(this);
        $('<input />').attr('type', 'checkbox').appendTo($(this));
    });

    $('.target-input, .editable').click(function () {
        if (!$(this).hasClass("posting") && !$(this).parents('li').hasClass('non-editable') && formEditable == true) {
            thisCell = $(this);
            thisCellType = thisCell.attr('class');
            thisInput = $('.lightbox-content input');
            if (thisCell.html() != "") {
                thisInput.val(thisCell.html());
                thisInput.val($('.lightbox-content input').val().replace('(', '-').replace(')', ''));
            }
            else {
                thisInput.val("");
            }
            $('#lightbox-overlay').fadeIn(100);
            $('.lightbox-content').fadeIn(100, function () {
                $('#resultNumber').focus();
            });
        }
    });

    function lightboxClose() {
        $('#lightbox-overlay').fadeOut(100);
        $('.lightbox-content').fadeOut(100);

        $('#incorrect-input-format-note').attr('class', '');

        relevantParag = thisCell.parents('li').find('.label-wrapper p');
        relevantLabel = thisCell.parents('li').find('.label-wrapper label');

        // i.e. if the entry is valid and not an empty entry
        if (thisCell.html() != "" && thisCellType == 'kpi-score') {

            if ($('input.show-detail').val() == "yes" && approvalColumnShowing == true) {
                if (relevantParag.css('display') == "none") {
                    relevantLabel.animate({ 'margin-top': 7 + 'px' }, function () {
                        relevantParag.fadeIn(60);
                    });
                }
            }
        }
        // i.e. if the entry is invalid and it is an empty entry
        else if (thisCell.html() == "" && thisCellType == 'kpi-score') {

            if (loggedInAs == "regionalDirector") {
                if (thisCell.siblings('.approved-label').hasClass('approved-label-grey')) {
                    thisCell.siblings('.approved-label').removeClass('approved-label-grey');
                }
                thisInput = thisCell.siblings('.approved-label').find('input');
                if (!thisInput.hasClass('approve-input-hidden') && !thisInput.parents('.approved-label').hasClass('approve-ticked')) {
                    thisInput.addClass('approve-input-hidden');
                    thisCell.siblings('.approved-label').removeClass('approve-input-grey');
                }
            }

            if ($('input.show-detail').val() == "yes" && approvalColumnShowing == true) {
                if (relevantParag.css('display') == "block") {
                    relevantParag.fadeOut(60, function () {
                        relevantLabel.animate({ 'margin-top': 15 + 'px' });
                    });
                }
            }
        }
    }

    cellsErroredArray = new Array();
    var ajaxPostCounter = 0;
    function doAjaxPost(relevantCell, relevantErroredArrayIndex) {
        ajaxPostCounter = ajaxPostCounter + 1;
        relevantCell.addClass("posting");
        relVal = relevantCell.html();
        if (relVal.indexOf('(') != -1) {
            relVal = relVal.replace('(', '-').replace(')', '');
        }

        $('<div></div>').addClass("spin-load-box").appendTo(relevantCell);

        objectToPost = { SiteID: $('#SiteID').val(), KpiMeasureID: relevantCell.attr('kpi-measure-id'), WeekEndingDate: relDate, Day: globalVariables.thisDay, Month: globalVariables.thisMonth, Year: globalVariables.thisYear, Value: relVal };

        $.ajax({
            type: "POST",
            url: "/KpiScore/Save",
            data: objectToPost,
            error: function (xhr, textStatus, errorThrown) {
                var errorDiv = $('#main-error-div');

                if (errorThrown && errorThrown != null) {
                    if (errorThrown == "Internal Server Error") {
                        errorDiv.html("Your changes may not have been saved correctly. Please refresh the page to check what Data has been saved.");
                    }
                }
                else {
                    errorDiv.html("Unable to connect to Server. Your changes have not been saved.");
                }
                if (!errorDiv.hasClass("visible")) {
                    errorDiv.addClass("visible").fadeIn();
                }

                //only if you're dealing with a cell that has not previously errored, will it
                // go into the errored array (which stores all cells that have errored to resubmit them later)
                if (relevantErroredArrayIndex == null) {
                    cellsErroredArray.push(relevantCell);
                }
                relevantCell.find(".spin-load-box").remove();
                $('<div></div>').addClass("error-box").html("!").insertAfter(relevantCell);
                relevantCell.removeClass("posting");
            },
            success: function (data) {

                relevantCell.find(".spin-load-box").remove();

                pastErrorBox = relevantCell.next('div');
                if (pastErrorBox && pastErrorBox.hasClass('error-box')) {
                    pastErrorBox.remove();
                }

                relevantCell.removeClass("posting");

                if ($('#main-error-div').hasClass("visible")) {
                    $(this).removeClass("visible").fadeOut();
                }

                if (relevantErroredArrayIndex != null) {
                    cellsErroredArray.splice(cellsErroredArray[relevantErroredArrayIndex]);
                }
                else {
                    for (a = 0; a < cellsErroredArray.length; a++) {
                        doAjaxPost(cellsErroredArray[a], a)
                    }
                }
                //Go through entries that failed in the past and try and resubmit

                objAlreadyEnteredArray = []

                //$('.kpi-score.validation-fail').removeClass("validation-fail");

                data["Scores"].forEach(function (obj) {
                    if (objAlreadyEnteredArray.indexOf(obj[0].KpiMeasureID) == -1) {
                        relevantKPIID = parseInt(obj[0].KpiMeasureID);
                        relevantLabel = $('.list-content span.kpi-score[kpi-measure-id=' + relevantKPIID + ']');

                        if (relevantLabel.length > 0) {
                            relevantLabel.html(obj[0].Value);

                            if (obj[0].Value < 0) {
                                relevantLabel.html(relevantLabel.html().replace('-', '('));
                                relevantLabel.html(relevantLabel.html() + ')');
                                relevantLabel.addClass("neg");
                            }
                            else {
                                if (relevantLabel.hasClass("neg")) {
                                    relevantLabel.removeClass("neg");
                                }
                            }

                            //this means the score has a target, but it's failed target and needs comment
                            if (obj[1] == "false") {
                                relevantLabel.siblings('.kpi-result').find('p').attr("class", "target-failed");

                                relevantRow = $('.list-content li span.kpi-score[kpi-measure-id=' + relevantKPIID + ']').closest('li');
                                existingCommentButton = relevantRow.find('span.score-comment-button');

                                if (existingCommentButton.length == 0) {
                                    newCommentButton = $('<span></span>').addClass("score-comment-button").appendTo(relevantRow);

                                    if (obj[1]["Comment"] == null) {
                                        newCommentButton.addClass("empty").html('!');
                                    }
                                    else {
                                        newCommentButton.html('...');
                                    }

                                    newCommentButton.on("click", function () {
                                        newCommentButton = $(this);
                                        //add Paragraph (which is hidden) to this row to save their comment inside if they want to re-open
                                        var liWrapper = newCommentButton.closest('li');
                                        $('<p></p>').addClass("score-comment").addClass("current-comment").appendTo(liWrapper);

                                        //before opening lightbox add unique text etc
                                        var KPIName = newCommentButton.siblings('.label-wrapper').find('label').html();
                                        $('#comment-popup').find('h5').html("KPI: " + KPIName);
                                        $('#comment-popup .note-for-scorecomment').css('display', 'block');

                                        //assign new class to button in lightbox so the JS knows what to do when enter is pressed
                                        $('#comment-popup').find('button.enter').addClass("comment-for-score");
                                        $('#comment-popup').find('textarea').html("");

                                        $('#lightbox-overlay').fadeIn(100);
                                        commentToShow = $(this).siblings('p.score-comment').html();
                                        $('#comment-popup').find('textarea').val(commentToShow).attr("disabled", "disabled");

                                        if (newCommentButton.hasClass("empty")) {
                                            showCommentsBox("editable");
                                        }
                                        else {
                                            showCommentsBox("non-editable");
                                        }
                                    });
                                }
                                else {
                                    existingCommentButton.show();
                                }
                            }
                            else if (obj[1] == "true") {
                                //this means a target was applicable and the score reached it
                                relevantLabel.siblings('.kpi-result').find('p').attr("class", "target-reached");

                                relevantRow = $('.list-content li span.kpi-score[kpi-measure-id=' + relevantKPIID + ']').closest('li');
                                existingCommentButton = relevantRow.find('span.score-comment-button');

                                if (existingCommentButton) {
                                    existingCommentButton.hide();
                                }
                            }

                            objAlreadyEnteredArray.push(obj[0].KpiMeasureID);

                        }
                    }
                });

                if (data["CellsWhichPassedValidation"].length > 0) {
                    for (a = 0; a < data["CellsWhichPassedValidation"].length; a++) {
                        for (b = 0; b < data["CellsWhichPassedValidation"][a].length; b++) {
                            thisCell = $('.kpi-score[kpi-measure-id="' + data["CellsWhichPassedValidation"][a][b][0] + '"]');

                            if (thisCell.hasClass("validation-fail" + data["CellsWhichPassedValidation"][a][b][1])) {
                                thisCell.removeClass("validation-fail" + data["CellsWhichPassedValidation"][a][b][1]);
                            }
                        }
                    };
                }

                if (data["CellsWhichFailedValidation"].length > 0) {
                    for (a = 0; a < data["CellsWhichFailedValidation"].length; a++) {
                        for (b = 0; b < data["CellsWhichFailedValidation"][a].length; b++) {
                            thisCell = $('.kpi-score[kpi-measure-id="' + data["CellsWhichFailedValidation"][a][b][0] + '"]');

                            if (!thisCell.hasClass("validation-fail" + data["CellsWhichFailedValidation"][a][b][1])) {
                                thisCell.addClass("validation-fail" + data["CellsWhichFailedValidation"][a][b][1]);
                            }
                        }
                    };
                }

                //Check to see if the allScoresFilled value is true
                if ("areAllScoresFilled" in data) {
                    if (data["areAllScoresFilled"]) { // == true && data["allScoresValid"] == true && data["missingCommentsArray"].length == 0 && data["CellsWhichFailedValidation"].length == 0) {
                        $('input.approvals').removeAttr("disabled");
                        if (data["allScoresValid"]) {
                            $('#form-submission-note').html('This form is now ready to Submit.');
                        } else {
                            $('#form-submission-note').html('This form is now ready to Submit, however be aware that some values have failed validation.');
                        }
                    }
                    else {
                        $("input.approvals").attr("disabled", "disabled");
                        $('#form-submission-note').html('This Form can not be submitted yet as not all the Scores/Comments are filled/valid.');
                        if (data["missingCommentsArray"].length > 0) {
                            //Go through all scores that need comment and put coment needed icon
                            for (a = 0; a < data["missingCommentsArray"].length; a++) {
                                relevantKPIID = data["missingCommentsArray"][a];
                                relevantRow = $('.list-content li span.kpi-score[kpi-measure-id=' + relevantKPIID + ']').closest('li');

                                existingCommentButton = relevantRow.find('span.score-comment-button');

                                if (existingCommentButton.length == 0) {
                                    newCommentButton = $('<span></span>').addClass("score-comment-button").addClass("empty").html('!').appendTo(relevantRow);

                                    newCommentButton.on("click", function () {
                                        newCommentButton = $(this);
                                        //add Paragraph (which is hidden) to this row to save their comment inside if they want to re-open
                                        var liWrapper = newCommentButton.closest('li');
                                        $('<p></p>').addClass("score-comment").addClass("current-comment").appendTo(liWrapper);

                                        //before opening lightbox add unique text etc
                                        var KPIName = newCommentButton.siblings('.label-wrapper').find('label').html();
                                        $('#comment-popup').find('h5').html("KPI: " + KPIName);
                                        $('#comment-popup .note-for-scorecomment').css('display', 'block');

                                        //assign new class to button in lightbox so the JS knows what to do when enter is pressed
                                        $('#comment-popup').find('button.enter').addClass("comment-for-score");
                                        $('#comment-popup').find('textarea').html("");

                                        $('#lightbox-overlay').fadeIn(100);
                                        commentToShow = $(this).siblings('p.score-comment').html();
                                        $('#comment-popup').find('textarea').val(commentToShow).attr("disabled", "disabled");

                                        if (newCommentButton.hasClass("empty")) {
                                            showCommentsBox("editable");
                                        }
                                        else {
                                            showCommentsBox("non-editable");
                                        }
                                    });
                                }
                            }
                        }
                    }
                }

                if (data["isCommentRequired"] == false) {
                    if (!relevantCell.siblings('.kpi-target').hasClass('void')) {
                        // just change the result cell visually, if it exists at all for this row
                        resultParag = relevantCell.siblings('.kpi-result').find('p');

                        if (resultParag.length > 0) {
                            resultParag.attr('class', 'target-reached');
                            commentButton = relevantCell.siblings('.score-comment-button');
                            if (commentButton.length > 0) {
                                commentButton.hide();
                            }
                        }
                    }
                }

            }
        })
    }

    //if they press Enter when inside the Lightbox submit, it hsould submit it
    $(document).keydown(function (e) {
        keyCode = e.keyCode;
        if (keyCode == 13 && $('.lightbox-content').css('display') == "block") {
            e.preventDefault();
            var reg = new RegExp(/^[\-]{0,1}[\d]+[\.]{0,1}[\d]{0,1}$/);
            if (reg.test($('.lightbox-content input').val()) == true) {
                thisCell.html($('.lightbox-content input').val());
                if ($('.lightbox-content input').val() < 0) {
                    thisCell.html(thisCell.html().replace('-', '('));
                    thisCell.html(thisCell.html() + ')');
                    thisCell.addClass("neg");
                }
                else {
                    if (thisCell.hasClass("neg")) {
                        thisCell.removeClass("neg");
                    }
                }
                doAjaxPost(thisCell, null);
                lightboxClose();
                if ($('#incorrect-input-format-note').hasClass("showing")) {
                    $('#incorrect-input-format-note').removeClass("showing");
                }
            }
            else {
                $('#incorrect-input-format-note').addClass("showing");
            }
        }
    });

    $('.lightbox-content button.enter').click(function () {
        var reg = new RegExp(/^[\-]{0,1}[\d]+[\.]{0,1}[\d]{0,1}$/);
        if (reg.test($('.lightbox-content input').val()) == true) {
            thisCell.html($('.lightbox-content input').val());
            if ($('.lightbox-content input').val() < 0) {
                thisCell.html(thisCell.html().replace('-', '('));
                thisCell.html(thisCell.html() + ')');
                thisCell.addClass("neg");
            }
            else {
                if (thisCell.hasClass("neg")) {
                    thisCell.removeClass("neg");
                }
            }
            doAjaxPost(thisCell, null);
            lightboxClose();
            if ($('#incorrect-input-format-note').hasClass("showing")) {
                $('#incorrect-input-format-note').removeClass("showing");
            }
        }
        else {
            $('#incorrect-input-format-note').addClass("showing");
        }
    });

    $('.lightbox-content button.close').click(function () {
        lightboxClose();
    });

    $('.approved-label').find('input').click(function (e) {
        relevantActualBox = $(this).parents('.approved-label').siblings('.kpi-score');
        if (loggedInAs == "regionalDirector" && relevantActualBox.html() != "") {
            $(this).parents('.approved-label').addClass("approve-ticked").removeClass("approve-label-grey");
            $(this).addClass('approve-input-hidden');
        }
        else {
            e.preventDefault()
        }
    });

    $('input.show-detail').click(function () {
        if ($(this).val() == "yes" && approvalColumnShowing == false) {
            approvalColumnShowing = true;

            $('ul.list-content li').each(function () {
                thisRow = $(this);
                thisActualBox = thisRow.find('.kpi-score');
                thisRow.find('div.label-wrapper label').animate({ 'margin-top': 7 + 'px' }, function () {
                    $(this).siblings('div.label-wrapper p').fadeIn();
                });
            });
        }
        else if ($(this).val() == "no" && approvalColumnShowing == true) {
            approvalColumnShowing = false;

            $('ul.list-content li').each(function () {
                thisRow = $(this);
                thisActualBox = thisRow.find('.kpi-score');
                thisRow.find('div.label-wrapper p').fadeOut(function () {
                    $(this).siblings('div.label-wrapper label').animate({ 'margin-top': 15 + 'px' });
                });
            });
        }
    });

    //Handle the hover note for non-editable boxes

    //this object should have descriptions for the various different kinds of uneditable box
    //the key will reflect the 'data-uneditable' attribute on that particular cell
    uneditableReasonsObject = {
        central: "This Cell cannot be edited as it can only be Changed by Head Office",
        calc: "This Cell cannot be edited as it is a calculation of other cells",
        approved: "This Cell cannot be edited as the Form has already been approved"
    };

    //Need to calculate the note divs original offset, and deduct this from the offset().top of the
    //Box which is hovered on

    var noteDiv = $('#uneditable-note'),
                origOffset = $('#main-inner').offset().top + 10;

    //Apply the Tipsy plug-in to all the relevant cells
    $('.kpi-score').each(function () {
        if (!$(this).hasClass("editable")) {
            uneditableReason = uneditableReasonsObject[$(this).attr('data-uneditable')];
            $(this).attr("title", uneditableReason);
        }
    });

    //------------------

    // Deal with all showing comments:

    //create lightbox function for showing comments and comments input

    commentBox = $('#comment-popup');
    var globalVariable = {};
    globalVariable.relevantClass = "nbone";

    function showCommentsBox(showType) {
        commentBox.fadeIn(100, function () {
            $('#comment-popup textarea').focus();
        });

        if (showType != "editable") {
            commentBox.find('button.enter').css('display', 'none');
        }
        else {
            commentBox.find('button.enter').css('display', 'block');
            commentBox.find('textarea').removeAttr("disabled");
        }
    }

    function popupClose(closeType, relevantdd) {
        commentBox.fadeOut(100);
        if (closeType == "close") {
            $('#lightbox-overlay').fadeOut(100);
        }
        else {
            $('#lightbox-overlay').fadeOut(100, function () {
                relevantdd.find('.comment').fadeIn(200);
            });
        }
        commentBox.find('textarea').val("");
        commentBox.find('.note-for-scorecomment').css("display", "none");
    }

    $('.comment').each(function () {
        thisComment = $(this);
        if (thisComment.css('display') == "block") {
            if (thisComment.find('p.comment-hide').html() != thisComment.find('p.comment-show').html()) {
                thisComment.find('button').css('display', 'block');
            }
        }
    });

    $('.show-comment').on("click", function (e) {
        e.preventDefault();
        $('#lightbox-overlay').fadeIn(100);
        commentToShow = $(this).siblings('p.comment-hide').html();
        commentBox.find('textarea').val(commentToShow).attr("disabled", "disabled");
        showCommentsBox("non-editable");
    });

    commentBox.find('.close').on("click", function () {
        $('.current-comment').attr('class', 'score-comment');
        popupClose('close', null);
    });

    commentBox.find('.enter').on("click", function () {
        comment = commentBox.find('textarea').val();
        if (comment != "") {
            var button = commentBox.find('button.enter');
            if (button.hasClass("comment-for-score")) {
                $('p.score-comment').each(function () {
                    if ($(this).hasClass('current-comment')) {
                        thisScoreComment = $(this);
                    }
                });

                // save 'comment' to db for this score with AJAX call
                commentObject = { SiteID: $('#SiteID').val(), KpiMeasureID: thisScoreComment.closest('li').find('.kpi-score').attr('kpi-measure-id'), Day: globalVariables.thisDay, Month: globalVariables.thisMonth, Year: globalVariables.thisYear, Comment: comment };
                $.ajax({
                    type: "POST",
                    url: "/KpiScore/SaveScoreComment",
                    data: commentObject,
                    error: function (xhr, textStatus, errorThrown) {
                        var errorDiv = $('#main-error-div');

                        if (errorThrown == "Internal Server Error") {
                            errorDiv.html("Your data may not have been saved correctly. Please refresh the page to check what Data has been saved.");
                        }
                    },
                    success: function (data) {
                        // save html of comment into the new hidden p you created for this row
                        thisScoreComment.html(comment);
                        thisScoreComment.attr('class', 'score-comment');

                        existingCommentButton = thisScoreComment.closest('li').find('span.score-comment-button');

                        if (existingCommentButton.length == 0) {
                            //create span for this row to act as button to see comment
                            var newSpan = $('<span></span>').addClass('score-comment-button').html('...').appendTo(thisScoreComment.closest('li'));

                            newSpan.on("click", function () {
                                $('#lightbox-overlay').fadeIn(100);
                                commentToShow = $(this).siblings('p.score-comment').html();
                                $('#comment-popup').find('textarea').val(commentToShow).attr("disabled", "disabled");
                                showCommentsBox("non-editable");
                            });
                            newSpan.on("mouseover", function () {
                                newTopVal = $(this).offset().top - origOffset;
                                if ($(this).hasClass("empty")) {
                                    $('#red-comment-popup').css({ 'top': newTopVal + 'px' }).show();
                                }
                                else {
                                    $('#blue-comment-popup').css({ 'top': newTopVal + 'px' }).show();
                                }
                            });
                            newSpan.on("mouseout", function () {
                                if ($(this).hasClass("empty")) {
                                    $('#red-comment-popup').hide();
                                }
                                else {
                                    $('#blue-comment-popup').hide();
                                }
                            });
                        }
                        else {
                            if (existingCommentButton.hasClass("empty")) {
                                existingCommentButton.attr('class', 'score-comment-button').html('...');
                            }
                        }

                        if (data["areAllScoresFilled"] == true
                        && data["allScoresValid"] == true
                        && data["missingCommentsArray"].length == 0
                        && data["CellsWhichFailedValidation"].length == 0) {
                            $('.to-enable').removeAttr("disabled");
                            $('#form-submission-note').html('This form is now ready to Submit.');
                        }
                        else if (data["missingCommentsArray"].length > 0) {
                            //Go through all scores that need comment and put coment needed icon
                            for (a = 0; a < data["missingCommentsArray"].length; a++) {
                                relevantKPIID = data["missingCommentsArray"][a];
                                relevantRow = $('.list-content li span.kpi-score[kpi-measure-id=' + relevantKPIID + ']').closest('li');

                                existingCommentButton = relevantRow.find('span.score-comment-button');

                                if (!existingCommentButton) {

                                    newCommentButton = $('<span></span>').addClass("score-comment-button").addClass("empty").html('!').appendTo(relevantRow);

                                    newCommentButton.on("click", function () {
                                        newCommentButton = $(this);
                                        //add Paragraph (which is hidden) to this row to save their comment inside if they want to re-open
                                        var liWrapper = newCommentButton.closest('li');
                                        $('<p></p>').addClass("score-comment").addClass("current-comment").appendTo(liWrapper);

                                        //before opening lightbox add unique text etc
                                        var KPIName = newCommentButton.siblings('.label-wrapper').find('label').html();
                                        $('#comment-popup').find('h5').html("KPI: " + KPIName);
                                        $('#comment-popup .note-for-scorecomment').css('display', 'block');

                                        //assign new class to button in lightbox so the JS knows what to do when enter is pressed
                                        $('#comment-popup').find('button.enter').addClass("comment-for-score");
                                        $('#comment-popup').find('textarea').html("");

                                        $('#lightbox-overlay').fadeIn(100);
                                        commentToShow = $(this).siblings('p.score-comment').html();
                                        $('#comment-popup').find('textarea').val(commentToShow).attr("disabled", "disabled");
                                        showCommentsBox("editable");
                                    });

                                    newCommentButton.on("mouseover", function () {
                                        newTopVal = $(this).offset().top - origOffset;
                                        if ($(this).hasClass("empty")) {
                                            $('#red-comment-popup').css({ 'top': newTopVal + 'px' }).show();
                                        }
                                        else {
                                            $('#blue-comment-popup').css({ 'top': newTopVal + 'px' }).show();
                                        }
                                    });
                                    newCommentButton.on("mouseout", function () {
                                        if ($(this).hasClass("empty")) {
                                            $('#red-comment-popup').hide();
                                        }
                                        else {
                                            $('#blue-comment-popup').hide();
                                        }
                                    });
                                }
                            }
                        }
                    }
                })

                //close pop-up
                popupClose('close', null);
                button.attr('class', 'enter');
                commentBox.find('.note-for-scorecomment').css('display', 'none');
            }
            else {

                // save 'comment' to db for this score with AJAX call
                commentObject = { KpiCollectionID: 0, SiteID: $('#SiteID').val(), Day: globalVariables.thisDay, Month: globalVariables.thisMonth, Year: globalVariables.thisYear, SignOffLevel: globalVariable.SignLevel, Comment: comment };
                $.ajax({
                    type: "POST",
                    url: "/KpiScore/SaveSiteSignOffComment",
                    data: commentObject,
                    error: function (xhr, textStatus, errorThrown) {

                    },
                    success: function (data) {
                        $('.main-approvals-container').each(function () {
                            thisdd = $(this).find('dd');
                            if (thisdd.hasClass(globalVariable.relevantClass)) {
                                thisdd.find('.comment p.comment-hide').html(comment);

                                if (comment.length > 73) {
                                    shortComment = comment.substr(0, 73);
                                    thisdd.find('.comment p.comment-show').html(shortComment + '...');
                                    thisdd.find('.show-comment').css('display', 'block');
                                }
                                else {
                                    thisdd.find('.comment p.comment-show').html(comment);
                                }
                                thisdd.find('.comment p.comment-show').css('display', 'block');
                                thisdd.find('.comment').css('display', 'block');
                                popupClose('enter', thisdd);

                                //work on this to bring object back from server, and make comment appear in comments box 
                                if (globalVariables.relevantClass == "rm-approved") {
                                    existingComment = $('.form-comments').find('p.' + globalVariable.relevantClass + '');
                                }
                                else {
                                    existingComment = $('.form-comments').find('p.' + globalVariable.relevantClass + '');
                                }
                                if (existingComment) {

                                }
                            }
                        });
                    }
                });
            }
        }
        else {
            popupClose('enter', null);
        }
    });

    //--------------------
    //Handle submissions, either when they press the tickBox or the Reject button

    $('input.approvals, button.approvals, button.reject').on("click", function (e) {
        e.preventDefault();

        //storing the element that was clicked so we can use later to decide what to hide/show
        var clickedElem = $(this);

        if (!clickedElem.hasClass("reject")) {
            //Find out what level of approval based on which class the container of clicked element has
            if (clickedElem.parents('dd').hasClass("submission")) {
                globalVariable.SignLevel = 0;
                globalVariable.relevantClass = "submission";
                commentBox.find('h5').html("COMMENT: Manager");
            }
            else if (clickedElem.closest('dd').hasClass("rm-approved")) {
                $('button.reject').hide();
                globalVariable.SignLevel = 3;
                globalVariable.relevantClass = "rm-approved";
                commentBox.find('h5').html("COMMENT: Regional Manager");
            }
            else if (clickedElem.parents('dd').hasClass("ho-approved")) {
                globalVariable.SignLevel = 2;
                globalVariable.relevantClass = "ho-approved";
                commentBox.find('h5').html("COMMENT: Ops Director");
            }
        }
        else {
            $('button.reject').hide();
            globalVariable.SignLevel = 3;
            globalVariable.relevantClass = "rm-approved";
            commentBox.find('h5').html("COMMENT: Regional Manager");
        }

        //First deal with the comments

        /*
        if (thisCell.html() != "") {
        $('.lightbox-content input').val(thisCell.html());
        }
        else {
        $('.lightbox-content input').val("");
        }
        */

        $('#lightbox-overlay').fadeIn(100);

        // ------------------------

        // Now we deal with the AJAX Function and the Success callback in line with that etc

        if (!clickedElem.hasClass("approvals")) {
            //i.e if it's not the checkbox they clicked, but the input instead - that means they rejected it
            var SignStatus = 1;
        }
        else {
            var SignStatus = 2;
        }

        //Find out date being signed off
        var weekEnding = $('#WeekEndingDate').val(),
                    yearVal = weekEnding.substring(0, 4),
                    monthVal = weekEnding.substring(5, 7),
                    dayVal = weekEnding.substring(8, 10);

        //pick up values to post in Ajax

        var objectToPost =
                {
                    SiteID: $('input#SiteID').val(),
                    Year: yearVal,
                    Month: monthVal,
                    Day: dayVal,
                    WeekEndingDate: $('#main-site-selection select').val(),
                    SignoffLevel: globalVariable.SignLevel,
                    SignoffStatus: SignStatus
                }

        $.ajax({
            type: "POST",
            url: "/KpiScore/Approve",
            data: objectToPost,
            dataType: "json",
            error: function (xhr, textStatus, errorThrown) {

                /*

                Error function not yet designed and planned

                */
            },
            success: function (data) {

                if (typeof globalVariable.SignLevel == "undefined") {
                    globalVariable.SignLevel = 0;
                }

                // Depending on what kind of approval we must do various visual things here

                if (globalVariable.SignLevel == 0) {
                    //Actions to take if sign off was at Site level
                    $('.submission').find('p.main-signoff').css('color', '#00cc00').html(data[0].Blame);
                    $('dd.submission input').hide();
                    $('dd.submission .icon-img').show().attr('src', '/Content/tick.jpg');
                }
                else if (globalVariable.SignLevel == 3) {
                    //Actions to take if sign off was at Sub-Region level
                    if (SignStatus == 1) {
                        // If it was rejected
                        $('.rm-approved').find('button').hide();
                        $('.rm-approved').addClass("inactive");
                        $('.rm-approved').find('p.main-signoff').addClass("rejected").css({ 'display': 'block', 'color': '#dd0000' }).html(data[0].Blame);
                        $('.submission').find('p.main-signoff').css('color', '#dd0000').html('Awaiting Resubmission');
                    }
                    else if (SignStatus == 2) {
                        // If it was approved
                        $('.rm-approved').find('p.main-signoff').css({ 'display': 'block', 'color': '#00cc00' }).html(data[0].Blame);
                        $('dd.rm-approved button').hide();
                        $('dd.rm-approved .icon-img').show().attr('src', '/Content/tick.jpg');

                        $('.kpi-score.editable').each(function () {
                            $(this).removeClass("editable").attr('data-uneditable', 'approved').attr('title', 'This Cell cannot be edited as the Form has already been approved.');
                        });
                        $('.kpi-score[data-uneditable="approved"]').tipsy();
                    }
                }
                /*
                else if (globalVariable.SignLevel == 2) {
                //Actions to take if sign off was at Region level
                if (SignStatus == 1) {
                // If it was rejected
                $('.rm-approved').addClass("inactive");
                $('.rm-approved input').removeClass("submitted");
                $('.submission').find('p.main-signoff').css({ 'display': 'block', 'color': '#dd0000' }).html('Awaiting Resubmission');
                $('.ho-approved').find('p.main-signoff').css({ 'display': 'block', 'color': '#dd0000' }).html(data[0].Blame);
                }
                else if (SignStatus == 2) {
                // If it was approved
                $('.ho-approved').find('p.main-signoff').css({ 'display': 'block', 'color': '#00cc00' }).html(data[0].Blame);
                $('dd.ho-approved input').hide();
                $('dd.ho-approved .icon-img').show().attr('src', '/Content/tick.jpg');
                }
                }
                */
            }

        })

        setTimeout(function () { showCommentsBox("editable") }, 500);
    });


    $('.score-comment-button').on("click", function () {
        thisCommentButton = $(this);
        var KPIName = $(this).siblings('.label-wrapper').find('label').html();
        $('#comment-popup').find('h5').html("KPI: " + KPIName);

        $('#lightbox-overlay').fadeIn(100);
        commentToShow = $(this).siblings('p.score-comment').html();
        if (thisCommentButton.hasClass("empty")) {
            $('#comment-popup').find('button.enter').addClass("comment-for-score");
            $(this).siblings('p.score-comment').addClass("current-comment");
            showCommentsBox("editable");
        }
        else {
            $('#comment-popup').find('textarea').val(commentToShow).attr("disabled", "disabled");
            showCommentsBox("non-editable");
        }
    });

    $('.score-comment-button').on("mouseover", function () {
        newTopVal = $(this).offset().top - origOffset;
        if ($(this).hasClass("empty")) {
            $('#red-comment-popup').css({ 'top': newTopVal + 'px' }).show();
        }
        else {
            $('#blue-comment-popup').css({ 'top': newTopVal + 'px' }).show();
        }
    });
    $('.score-comment-button').on("mouseout", function () {
        if ($(this).hasClass("empty")) {
            $('#red-comment-popup').hide();
        }
        else {
            $('#blue-comment-popup').hide();
        }
    });

});
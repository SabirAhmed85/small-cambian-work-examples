(function ($) {

    $.fakeajax = function (config) {
        $.realajax = $.ajax;

        //Attach this new method to jQuery
        $.ajax = function (object) {

            //code to be inserted here
            fakeReturnObject = config.urls[object.url];
            

            //Delay is just included to allow users to see visuals of 'loading-screen' or 'loading-objects'
            setTimeout(function () {
                //perform the success/error functions depending on if the coder has entered a valid url
                //that exists in our object
                if (typeof fakeReturnObject !== "undefined") {
                    object.success(fakeReturnObject);
                }
                else {
                    $.realajax(object);
                }
            }, 1000);

        };
    };

    //pass jQuery to the function, 
    //So that we will able to use any valid Javascript variable name

})(jQuery);
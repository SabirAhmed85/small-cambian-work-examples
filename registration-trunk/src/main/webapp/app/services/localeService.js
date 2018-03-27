'use strict';

/**
 * @ngdoc function
 * @name app.factory:localeService
 * @description
 * # localeService
 * Service for setting/getting current locale
 */
angular.module('app')
  .service('localeService', ['$translate', 'LOCALES', '$rootScope', '$filter', 'tmhDynamicLocale', '$window', function ($translate, LOCALES, $rootScope, $filter, tmhDynamicLocale, $window) {
    // VARS
    var that = this;
    var locales = LOCALES.locales;

    // locales and locales display names
    if (!locales || locales.length === 0) {
      console.error('There are no _LOCALES provided');
    }

    var currentLocale = {"key": $translate.proposedLanguage()};// because of async loading

    // METHODS
    var checkLocaleKeyIsValid = function (key) {
      var found = $filter('filter')(locales, {key : key});
      return !!found && found.length > 0;
    };

    /**
     * Stop application loading animation when translations are loaded
     */
    var $html = angular.element('html');
    var LOADING_CLASS = 'app-loading';

    function startLoadingAnimation() {
      $html.addClass(LOADING_CLASS);
    }

    function stopLoadingAnimation() {
      $html.removeClass(LOADING_CLASS);
    }

    // EVENTS
    $rootScope.$on('$translateChangeSuccess', function (event, data) {
      document.documentElement.setAttribute('lang', data.language);// sets "lang" attribute to html

      tmhDynamicLocale.set(data.language.toLowerCase().replace(/_/g, '-'));// load Angular locale
    });

    $rootScope.$on('$localeChangeSuccess', function () {
      stopLoadingAnimation();
    });

    $translate.onReady(function(){
      //TODO don't change if users has changed locale on login
      var key = $translate.use();
      var found = $filter('filter')(locales, {key : key});

      currentLocale.key = key;

      if (!!found && found.length >= 1){
        currentLocale.value = found[0].value;
        moment.locale(key);
        tmhDynamicLocale.set(key);
      }
    });

    function setLocale(locale){
      if (!checkLocaleKeyIsValid(locale)) {
        console.error('Locale name "' + locale + '" is invalid');
        return;
      }
      startLoadingAnimation();
      var found = $filter('filter')(locales, {key : locale});

      currentLocale.key = locale;
      if (!!found && found.length >= 1){
        currentLocale.value = found[0].value;
      }

      $translate.use(locale);
      moment.locale(locale);
    }

    return {
      setLocale: setLocale,
      getLocale: function () {
        return currentLocale;
      },
      getLocales: function () {
        return locales;
      }      
    };
  }]);

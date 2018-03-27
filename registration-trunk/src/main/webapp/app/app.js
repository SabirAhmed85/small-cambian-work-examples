'use strict';

var app = angular.module('app', [
  'ngRoute'
  , 'ui.bootstrap'
  , 'pascalprecht.translate'
  , 'ngCookies'
  , 'ui.select'
  , 'tmh.dynamicLocale'
  , 'tmh.dynamicLocalePreload'
]);
app.constant('LOCALES', {
    'locales': [
      { 
        'key': 'en', 
        'value': 'English',
        'form-validation-rules': {
          'postcode-regexp': "[a-zA-Z]{1,2}\\d{1,2}[a-zA-Z]{0,1}\\s*\\d[a-zA-Z]{2}$",
          'phone-min-length': 8
        }
      }, 
      { 
        'key': 'de',
        'value': 'Deutsch',
        'form-validation-rules': {
          'postcode-regexp': "(?!01000|99999)(0[1-9]\\d{3}|[1-9]\\d{4})$",
          'phone-min-length': 10
        }
      },
      { 
        'key': 'es',
        'value': 'Español',
        'form-validation-rules': {
          'postcode-regexp': "((0[1-9]|5[0-2])|[1-4][0-9])[0-9]{3}$"
        }
      },
      { 
        'key': 'fr',
        'value': 'Français',
        'form-validation-rules': {
          'postcode-regexp': "[0-9]{5}$"
        }
      },
      { 
        'key': 'it',
        'value': 'Italiano',
        'form-validation-rules': {
          'postcode-regexp': "[0-9]{5}$"
        }
      },
      { 
        'key': 'ja',
        'value': '日本語',
        'form-validation-rules': {
          'postcode-regexp': "\\d{3}-\\d{4}$"
        }
      }
    ],
    'preferredLocale': 'en'
});
app.config(['$routeProvider', '$httpProvider', function ($routeProvider, $httpProvider) {
  $routeProvider
    .when('/:guid', {
      controller: 'RegistrationController',
      templateUrl: 'app/components/registration/registration.view.html'
    })
    .otherwise({
      redirectTo: "/",
      controller: 'RegistrationController',
      templateUrl: 'app/components/registration/registration.view.html'
    });
}]);
// Angular Translate
app.config(["$translateProvider", "LOCALES", function ($translateProvider, LOCALES) {
    $translateProvider.useMissingTranslationHandlerLog();// warns about missing translates

    $translateProvider.useStaticFilesLoader({
      prefix: 'assets/locale/',
      suffix: '.json'
    });

    $translateProvider.preferredLanguage(LOCALES.preferredLocale);
    $translateProvider.useLocalStorage();
    $translateProvider.useSanitizeValueStrategy('sce');
}]);

app.run(['$rootScope', function ($rootScope) {
  $rootScope.$on('$routeChangeStart', function (event, next, prev) {
    $rootScope.loadingView = true;
  });

  $rootScope.$on('$routeChangeSuccess', function (e, curr, prev) {
    // Hide loading message
    $rootScope.loadingView = false;
    $('#mainContainer').animate({
      scrollTop: 0
    }, 0);
  });

}]);

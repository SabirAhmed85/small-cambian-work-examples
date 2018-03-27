'use strict';

/**
 * @ngdoc function
 * @name translateApp.directive:LanguageToolbarDirective
 * @description
 * # LanguageToolbarDirective
 * Directive to append language select and set its view and behavior
 */
angular.module('app')
  .directive('translateLanguageToolbar', ['$rootScope', '$compile', 'localeService', 'ApiService', function($rootScope, $compile, localeService, ApiService) {
    var directive = {};

    directive.restrict = 'A';
    //directive.replace = true;
    directive.templateUrl = 'app/shared/views/language-toolbar.view.html';

    directive.scope = {
      translateEvent: "="
    };

    var linker = function (scope, element, attrs) {
      scope.currentLocaleDisplayName = localeService.getLocale();
      scope.locales = localeService.getLocales();
      scope.visible = scope.localesDisplayNames && scope.localesDisplayNames.length > 1;

      scope.changeLanguage = function (key) {
        if (!!scope.translateEvent){
          scope.translateEvent.success = false;
          scope.translateEvent.error = false;
        }

        localeService.setLocale(key);
      };

      element.html(directive.template).show();

      //$compile(element.contents())(scope);
    }

    directive.link = linker;
    
    return directive;
  }]);

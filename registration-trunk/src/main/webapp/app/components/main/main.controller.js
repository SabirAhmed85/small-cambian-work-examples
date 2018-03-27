'use strict';

angular.module('app')
  .controller('MainController', ['$scope','$rootScope',
    function($scope, $rootScope) {
      var self = this;
      self.logo = "assets/images/colt-logo-light@2x.png";
      self.loading = true;
      self.loadingType = 'loading';

      // subscribe to login and logout events and set variables accordingly
      function subscribe() {
        $rootScope.$on('loaded', function() {
          self.loading = false;
        });
      }

      subscribe();
    }
  ]);

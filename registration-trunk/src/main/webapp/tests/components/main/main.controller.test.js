'use strict';

describe('Controller: MainController', function () {
  describe('Controller: MainController all working', function () {
    var MainCtrl, scope, httpBackend, rootScope;

    beforeEach(function () {
      module('app')
    });

    beforeEach(inject(function ($controller, $rootScope, _$httpBackend_) {
      scope = $rootScope.$new();
      rootScope = $rootScope;
      httpBackend = _$httpBackend_;

      MainCtrl = $controller('MainController', {
        $scope: scope
      });

    }));

    it('should expose all relevant variables', function () {
      httpBackend.whenGET(/assets\/locale\/.*/).respond({});
      expect(MainCtrl.logo).toBe("assets/images/colt-logo-light@2x.png");
      expect(MainCtrl.loading).toBe(true);
      expect(MainCtrl.loadingType).toBe("loading");
    });

    it('should set scope.loading to false when loaded', function () {
      MainCtrl.loading = true;
      httpBackend.whenGET(/assets\/locale\/.*/).respond({});
      rootScope.$broadcast("loaded");
      scope.$digest();
      expect(MainCtrl.loading).toBe(false);
    });
  });
});

'use strict';

describe('Controller: RegistrationController', function () {
  describe('Controller: RegistrationController all working', function () {
    var RegCtrl, scope, TranslationProv, httpBackend;

    beforeEach(function () {
      module('app')
    });

    beforeEach(inject(function ($controller, $rootScope, _$httpBackend_) {
      scope = $rootScope.$new();
      httpBackend = _$httpBackend_;
      httpBackend.whenGET('/api/registration/aa7a77a7a').respond({});
      httpBackend.whenGET('/api/supported_currencies').respond({});
      httpBackend.whenGET(/assets\/locale\/.*/).respond({});

      TranslationProv = {
        useStaticFilesLoader: function () {
          return {
            prefix: 'assets/locale/',
            suffix: '.json'
          }
        },
        preferredLanguage: function (lang) {
          return lang;
        },
        useLocalStorage: function () {
          return 1;
        }
      };

      RegCtrl = $controller('RegistrationController', {
        $scope: scope,
        translationProvider: TranslationProv,
        $routeParams: {guid: 'aa7a77a7a'}
      });

    }));

    it('should have received correct object from TranslationProvider useStaticFilesLoader method by default', function () {
      expect(TranslationProv.useStaticFilesLoader().prefix).toBe('assets/locale/');
      expect(TranslationProv.useStaticFilesLoader().suffix).toBe('.json');
    });

    it('should have called TranslationProvider useLocalStorage method by default', function () {
      expect(TranslationProv.useLocalStorage()).toBe(1);
    });

    it('should have called TranslationProvider preferredLanguage method when preferred language selected', function () {
      expect(TranslationProv.preferredLanguage('en')).toBe('en');
    });

    it('should return true if OCN Number is entered', function () {
      scope.OCN = '';
      scope.$digest();
      scope.OCN = '8S7NH';
      scope.$digest();
      expect(scope.OCNExists).toBe(true);
    });

    it('should recognize when the Contact Details Fields are not complete', function () {
      scope.ContactName = 'Sarah Smith';
      scope.ContactPhoneNumber = '02087667687';

      scope.$digest();

      expect(scope.ContactDetailsComplete).toBe(false);
    });

    it('should recognize when the Contact Details Fields are complete', function () {
      scope.ContactName = 'Sarah Smith';
      scope.ContactPhoneNumber = '02087667687';
      scope.ContactEmail = 'sarah@email.com';

      scope.$digest();

      expect(scope.ContactDetailsComplete).toBe(true);
    });

    it('should recognize when the Company Details Fields are not complete', function () {
      scope.CompanyName = 'ASDA';
      scope.PremisesNumber = '24';
      scope.CompanyStreetName = 'Commercial Street';

      scope.$digest();

      expect(scope.CompanyDetailsComplete).toBe(false);
    });

    it('should recognize when the Company Details Fields are complete', function () {
      scope.OCNExists = false;
      scope.CompanyName = 'ASDA';
      scope.selectedVals = {};
      scope.selectedVals.CompanyCountry = {country: 'Germany'};
      scope.selectedVals.CompanyLanguage = 'German';
      scope.PremisesNumber = '24';
      scope.CompanyStreetName = 'Commercial Street';
      scope.CompanyPostCode = 'B21 6DJ';
      scope.CompanyProvince = 'Birmingham';
      scope.VATNumber = '87HUHSISK';

      scope.$digest();

      expect(scope.CompanyDetailsComplete).toBe(true);
    });

    it('should recognize when the Company Details Fields are complete and OCN Number is entered', function () {
      scope.OCNExists = true;
      scope.selectedVals = {};
      scope.selectedVals.CompanyCountry = 'Germany';
      scope.selectedVals.CompanyLanguage = 'German';

      scope.$digest();

      expect(scope.CompanyDetailsComplete).toBe(true);
    });

    it('should recognize when the Billing Details Fields are not complete', function () {
      scope.billingAddressFields = {
        BillingPremiseNumber: {required: true, value: '20'},
        BillingStreetName: {required: true, value: 'something'},
        BillingCity: {required: true, value: ''},
        BillingPostCode: {required: true, value: ''}
      };

      scope.$digest();

      expect(scope.NewBillingDetailsComplete).toBe(false);
    });

    it('should recognize when the Billing Details Fields are complete', function () {
      scope.billingAddressFields = {
        BillingPremiseNumber: {value: '20'},
        BillingStreetName: {value: 'Lovely Street'},
        BillingPostCode: {value: 'something'},
        BillingCity: {value: 'something'},
        BillingProvince: {value: 'something'},
        BillingCountry: {value: 'something'},
        BankName: {value: 'something'},
        AccountName: {value: 'something'},
        PaymentMethod: {value: 'something'},
        BranchName: {value: 'something'},
        SortCode: {value: 'something'},
        IBAN: {value: 'something'},
        BIC: {value: 'something'},
      };

      scope.$digest();

      expect(scope.NewBillingDetailsComplete).toBe(true);
    });

    it('should recognise when to toggle the NewBillingDetails bool', function () {
      scope.billingAddressFields = {
        BillingPremiseNumber: {value: ''},
        BillingStreetName: {value: ''},
        BillingCity: {value: ''},
        BillingPostCode: {value: ''},
        BillingProvince: {value: ''},
        BillingCountry: {value: ''}
      };

      scope.selectedVals = {
        CompanyCountry: {
          label: 'England_label',
          country: 'England_country'
        }
      };
      
      scope.PremisesNumber = '55';
      scope.CompanyStreetName = 'Laandaaaan Street';
      scope.CompanyCity = 'Laandaaaan';
      scope.page = {};
      scope.page.CopyCompanyAddress = false;
      scope.$digest();

      scope.page.CopyCompanyAddress = true;
      
      scope.$digest();

      expect(scope.billingAddressFields['BillingPremiseNumber'].value).toBe('55');
      expect(scope.billingAddressFields['BillingStreetName'].value).toBe('Laandaaaan Street');
      expect(scope.billingAddressFields['BillingCity'].value).toBe('Laandaaaan');
    });

    it('should recognise when to toggle the NewBillingDetails bool', function () {
      scope.NewBillingDetails = false;
      scope.toggleNewBillingBool(true);

      scope.$digest();

      expect(scope.NewBillingDetails).toBe(true);

      scope.toggleNewBillingBool(false);
      
      scope.$digest();

      expect(scope.NewBillingDetails).toBe(false);
    });

    it('should add a user to the users object if object count is below 5', function () {
      scope.users = [{'number':1},{'number':2},{'number':3}];
      scope.addUser();
      expect(scope.users.length).toBe(4);
    });

    it('should remove a user from the users object', function () {
      scope.users = [{'number':1},{'number':2},{'number':3}];
      scope.removeUser({'number': 2});
      expect(scope.users.length).toBe(2);
    });

    it('should remove the appropriate user from the users object by matching the users "number" property', function () {
      scope.users = [{'number':1, 'name': 'Paul'},{'number':2, 'name': 'John'},{'number':3, 'name': 'Sarah'}];
      scope.removeUser({'number': 2, 'name': 'John'});
      expect(scope.users[0].name).toBe('Paul');
      expect(scope.users[1].name).toBe('Sarah');
    });

    it('should decrement the "number" property for subsequent users in the users object, when a user is removed from the users object', function () {
      scope.users = [{'number':1},{'number':2},{'number':3},{'number':4}];
      scope.removeUser({'number': 2});
      expect(scope.users[1].number).toBe(2);
      expect(scope.users[2].number).toBe(3);
    });

    it('should change the FormSubmissionAttempted bool when form processing is attempted', function () {
      var form = {};
      scope.users = [];

      scope.processForm(form);
      expect(scope.FormSubmissionAttempted).toBe(true);
    });
  });

  describe('Controller: RegistrationController error', function () {
    beforeEach(module('app'));
    var RegCtrl, scope, httpBackend;

    beforeEach(inject(function ($controller, $rootScope, _$httpBackend_) {
      scope = $rootScope.$new();
      httpBackend = _$httpBackend_;
      httpBackend.whenGET('/api/registration/aa7a77a7a').respond({});
      httpBackend.whenGET(/assets\/locale\/.*/).respond({});

      RegCtrl = $controller('RegistrationController', {
        $scope: scope
      });

    }));

    it('should return false if OCN Number is not entered', function () {
      httpBackend.whenGET('app/components/shared/modals/registration-submission-received/registration-submission-received-modal-dialog.view.html').respond({});
      scope.OCN = null;
      scope.$digest();
      scope.OCN = '';
      scope.OCNExists = null;
      scope.$digest();
      expect(scope.OCNExists).toBe(false);
    });

    it('should not add any users to the users object if object count is already 5', function () {
      scope.users = [{'number':1},{'number':2},{'number':3},{'number':4},{'number':5}];
      scope.addUser();
      expect(scope.users.length).toBe(5);
    });

  });
});

'use strict';

describe('Controller: SystemUserController', function () {
  describe('Controller: SystemUserController all working', function () {
    beforeEach(module('app'));
    var SUCtrl, scope, httpBackend;

    beforeEach(inject(function ($controller, $rootScope, _$httpBackend_) {
      scope = $rootScope.$new();
      httpBackend = _$httpBackend_;
      httpBackend.whenGET('/api/registration/aa7a77a7a').respond({});
      httpBackend.whenGET(/assets\/locale\/.*/).respond({});

      SUCtrl = $controller('SystemUserController', {
        $scope: scope
      });

    }));

    it('should exist', function () {
      expect(SUCtrl).toBeDefined();
    });

    it('should recognise when user details are not conmplete', function () {
      scope.users = [
          {
              number: 1,
              detailsComplete: null
          }
      ];
      scope.user = {
          number: 1,
          UserInfoFullNameAdmin: 'Sarah Jane',
          UserInfoTelephone: '02089959965',
          UserInfoEmail: 'sarah@hotmail.com'
      };
      scope.$digest();

      expect(scope.users[0].detailsComplete).toBe(false);
    });

    it('should recognise when user is not an existing colt user and user details are complete', function () {
      scope.users = [
          {
              ExistingColtUser: false,
              number: 1,
              UserInfoFullNameAdmin: 'Sarah Jane',
              UserInfoTelephone: '02089959965',
              UserInfoEmail: 'sarah@hotmail.com',
              selectedRole: {value: 'Full'},
              UserInfoTitle: {value: 'Mr'},
              UserInfoColtOnlineUserIDAdmin: 'OI090387',
              UserColtOnlinePortalDisplayLanguage: {value: 'English'}
          }
      ];

      scope.user = {
            ExistingColtUser: false,
            number: 1,
            UserInfoFullNameAdmin: 'Sarah Jane',
            UserInfoTelephone: '02089959965',
            UserInfoEmail: 'sarah@hotmail.com',
            selectedRole: {value: 'Full'},
            UserInfoTitle: {value: 'Mr'},
            UserInfoColtOnlineUserIDAdmin: 'OI090387',
            UserColtOnlinePortalDisplayLanguage: {value: 'English'}
      };
      
      scope.$digest();

      expect(scope.users[0].detailsComplete).toBe(true);
    });

    it('should recognise when user is an existing colt user and user details are complete', function () {
      scope.users = [
          {
              ExistingColtUser: true,
              number: 1,
              UserInfoFullNameAdmin: 'Sarah Jane',
              UserInfoTelephone: '02089959965',
              UserInfoEmail: 'sarah@hotmail.com',
              selectedRole: {value: 'Full'},
              UserInfoTitle: {value: 'Mr'},
              UserInfoColtOnlineUserIDAdmin: 'OI090387',
              UserColtOnlinePortalDisplayLanguage: {value: 'English'}
          }
      ];

      scope.user = {
            ExistingColtUser: false,
            number: 1,
            UserInfoFullNameAdmin: 'Sarah Jane',
            UserInfoTelephone: '02089959965',
            UserInfoEmail: 'sarah@hotmail.com',
            selectedRole: {value: 'Full'},
            UserInfoTitle: {value: 'Mr'},
            UserInfoColtOnlineUserIDAdmin: 'OI090387',
            UserColtOnlinePortalDisplayLanguage: {value: 'English'}
      };
      
      scope.$digest();

      expect(scope.users[0].detailsComplete).toBe(true);
    });

  });
});

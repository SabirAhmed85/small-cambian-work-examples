'use strict';

angular.module('app')
  .controller('RegistrationController', ['$scope', '$rootScope', 'LOCALES', '$translate', 'ApiService', '$uibModal', '$window', '$location', '$routeParams', '$timeout',
    function($scope, $rootScope, LOCALES, $translate, ApiService, $uibModal, $window, $location, $routeParams, $timeout) {

      var self = this;
      self._apiService = ApiService;

      function getPostCodeRegExp (country) {
        for (var a = 0; a < $scope.countryOptions.length; a++) {
          if ($scope.countryOptions[a]['country'] == country) {   
            return $scope.countryOptions[a]['form-validation-rules']['postcode-regexp'];
          }
        }
      }

      function setUpFormFields(data) {

        //Check if any billing details filled
        function detailsEntered (typeToCheck, data) {
          var invoiceDet, billingDet, companyDet = false;

          if (data.vat_number ||
            data.address_premise_no ||
            data.address_street ||
            data.address_postcode ||
            data.address_city ||
            data.address_county) {
            companyDet = true;
          } else {
            companyDet = false;
          }

          if (data.billing_account_name 
            || data.billing_address_city 
            || data.billing_address_country 
            || data.billing_address_county 
            || data.billing_address_postcode
            || data.billing_address_premise_no
            || data.billing_address_street
            || data.billing_bank_name
            || data.billing_bic
            || data.billing_branch_name
            || data.billing_iban
            || data.billing_payment_method) {
            billingDet = true;
          } else {
            billingDet = false;
          }

          if (data.invoice_title 
            || data.invoice_contact_name 
            || data.invoice_po_number 
            || data.invoice_email_address 
            || data.invoice_customer_ref 
            || data.invoice_tel 
            || data.invoice_language 
            || data.invoice_currency) {
            invoiceDet = true;
          } else {
            invoiceDet = false;
          }

          if (typeToCheck == 'billing' && (billingDet || invoiceDet)) {
            return true;
          }
          else if (typeToCheck == 'billing' && !billingDet && !invoiceDet) {
            return false;
          }
          else if (typeToCheck == 'invoice' && invoiceDet) {
            return true;
          }
          else if (typeToCheck == 'invoice' && !invoiceDet) {
            return false;
          }
          else if (typeToCheck == 'company' && companyDet) {
            return true;
          }
          else if (typeToCheck == 'company' && !companyDet) {
            return false;
          }
        }

        //Build Users Array
        function setUpUsersArr () {
          if (data.col_users.length > 0) {
            $scope.users = data.col_users.map(function (x, i) {
              var selectedRoleTransLabel, selectedTitleTransLabel;
              if (x.requested_role) {
                for (var a = 0; a < $scope.userRoles.length; a++) {
                  if ($scope.userRoles[a].value == x.requested_role) {
                    selectedRoleTransLabel = $scope.userRoles[a].label;
                  }
                }
              }
              if (x.title) {
                for (var a = 0; a < $scope.titles.length; a++) {
                  if ($scope.titles[a].value == x.title) {
                    selectedTitleTransLabel = $scope.titles[a].label;
                  }
                }
              }
              
              var forename = (typeof(x.forename) !== 'undefined') ? x.forename: '';
              var surname = (typeof(x.surname) !== 'undefined') ? x.surname: '';
              
              return ({
                "number": i + 1,
                "id": x.id || '',
                "UserInfoNewColId": x.new_col_id,
                "selectedRole": x.requested_role || '',
                "translatedSelectedRoleLabel": (x.requested_role) ? $translate.instant(selectedRoleTransLabel).toString(): '',
                "UserInfoTitle": (x.title) ? {value: x.title}: {value: ''},
                "translatedUserInfoTitleLabel": (x.title) ? $translate.instant(selectedTitleTransLabel).toString(): '',
                "UserInfoFullNameAdmin": (typeof(x.forename) !== 'undefined' || typeof(x.surname) !== 'undefined') ? forename + ' ' + surname : '',
                "UserInfoTelephone": x.telephone || '',
                "UserColtOnlinePortalDisplayLanguage": {'value': x.language, 'label': x.language},
                "UserInfoColtOnlineUserIDAdmin": x.existing_col_id || '',
                "UserInfoEmail": x.email || '',
                'ExistingColtUser': (x.existing_col_id) ? true: false 
              });
            });
            $scope.latestUserNumber = $scope.users.length + 1;
          } else {
            $scope.users = [
              {
                'number': 1,
                'UserInfoColtOnlineUserIDAdmin': '',
                'selectedRole': 'Full',
                'UserInfoFullNameAdmin': '',
                'UserInfoTitle': {'value':''},
                'UserInfoEmail': '',
                'UserInfoTelephone': '',
                'ExistingColtUser': true,
                'UserColtOnlinePortalDisplayLanguage': {'value':''},
                'detailsComplete': false
              }
            ];
            $scope.latestUserNumber = 1;
          }
        }

        if (data != '') {
          setUpUsersArr();
          $scope.Status = data.status,
          $scope.SalesChannel = data.sales_channel,
          $scope.OCN = data.ocn || '',
          $scope.ContactName = data.primary_contact_name || '',
          $scope.ContactPhoneNumber = data.primary_contact_tel || '',
          $scope.ContactEmail = data.primary_contact_email || '',
          $scope.CompanyName = data.company_name || '',
          $scope.VATNumber = data.vat_number || '',
          $scope.PremisesNumber = data.address_premise_no || '',
          $scope.CompanyStreetName = data.address_street || '',
          $scope.CompanyPostCode = data.address_postcode || '',
          $scope.CompanyCity = data.address_city || '',
          $scope.CompanyProvince = data.address_county || '',
          $scope.RefBCN = data.reference_bcn || '',
          $scope.OCNNumberExists = (data.ocn) ? true: false,
          $scope.OCNExists = (data.ocn) ? true: detailsEntered('company', data) ? false: null,
          $scope.NewBillingDetails = (data.reference_bcn) ? false: detailsEntered('billing', data) ? true: false,
          $scope.page.CopyCompanyAddress = (data.billing_address_premise_no || data.billing_address_street || data.billing_address_postcode || data.billing_address_city || data.billing_address_county || data.billing_address_country) ? false : true;
          $scope.page.EnterInvoiceDetails = (detailsEntered('invoice', data)) ? true: false;
        }
        else {
          $scope.OCNExists = null,
          $scope.ContactDetailsComplete = false,
          $scope.CompanyDetailsComplete = false,
          $scope.NewBillingDetails = false,
          $scope.NewBillingDetailsComplete = false,
          $scope.LegalNoticeAccepted = false,
          $scope.FormSubmissionAttempted = false,
          $scope.latestUserNumber = 1,
          $scope.page.CopyCompanyAddress = true,
          $scope.page.EnterInvoiceDetails = false,
          $scope.RefBCN ='';

          $scope.users = [
            {
              'number': 1,
              'UserInfoColtOnlineUserIDAdmin': '',
              'selectedRole': 'Full',
              'UserInfoFullNameAdmin': '',
              'UserInfoTitle': {'value':''},
              'UserInfoEmail': '',
              'UserInfoTelephone': '',
              'ExistingColtUser': true,
              'UserColtOnlinePortalDisplayLanguage': {'value':''},
              'detailsComplete': false
            }
          ];
          $scope.latestUserNumber = 1;
        }

        var invLangLabel = function () {
          if (data.invoice_language) {
            for (var a = 0; a < $scope.languages.length; a++) {
              if ($scope.languages[a]["value"] == data.invoice_language) {
                return $scope.languages[a]["label"];
              }
            }
          }
          else {
            return '';
          }
        }
  
        $scope.selectedVals = {
          CompanyCountry: {country: data.address_country || '', label: data.address_country || ''},
          CompanyLanguage: {
            label: data.language || '',
            value: (data.language) ?
              $scope.languages.filter(function (x) {if (x.value == data.language) {return x.value}})[0]["value"] :
              ''
          },
          PaymentMethod: {
            value: data.billing_payment_method || ''
          },
          OCNExists: {
            value: (data.ocn) ? true: detailsEntered('company', data) ? false: null
          }
        };
        
        $scope.billingAddressFields = {
          'BillingPremiseNumber': {
            'required': true,
            'value': data.billing_address_premise_no || ''
          },
          'BillingStreetName': {
            'required': true,
            'value': data.billing_address_street || ''
          },
          'BillingPostCode': {
            'required': true,
            'value': data.billing_address_postcode || ''
          },
          'BillingCity': {
            'required': true,
            'value': data.billing_address_city || ''
          },
          'BillingProvince': {
            'required': true,
            'value': data.billing_address_county || ''
          },
          'BankName': {
            'required': true,
            'value': data.billing_bank_name || ''
          },
          'AccountName': {
            'required': true,
            'value': data.billing_account_name || ''
          },
          'BranchName': {
            'required': true,
            'value': data.billing_branch_name || ''
          },
          'SortCode': {
            'required': true,
            'value': data.billing_sort_code || ''
          },
          'IBAN': {
            'required': true,
            'value': data.billing_iban || ''
          },
          'BIC': {
            'required': true,
            'value': data.billing_bic || ''
          },
          'BillingCountry': {
            'required': true,
            'value': $scope.countryLabels[data.billing_address_country] || '',
            'label': $scope.countryLabels[data.billing_address_country] || '',
            'country': data.billing_address_country || ''
          }
        };

        $scope.invoiceDetailsFields = {
          'InvoiceTitle': {
            'value': data.invoice_title || ''
          },
          'InvoiceName': {
            'value': data.invoice_contact_name || ''
          },
          'PO': {
            'value': data.invoice_po_number || ''
          },
          'InvoiceEmailAddress': {
            'value': data.invoice_email_address || ''
          },
          'CustomerReference': {
            'value': data.invoice_customer_ref || ''
          },
          'InvoiceTelephoneNumber': {
            'value': data.invoice_tel || ''
          },
          'Currency': {
            'value': data.invoice_currency || '', 'label': data.invoice_currency || ''
          },
          'InvoiceLanguage': {
            'value': data.invoice_language || '', 'label': invLangLabel()
          }
        };

        $timeout(function () {
          $scope.loading = false;
          $rootScope.$broadcast('loaded');
        }, 1500);
      }

      function resetFormData () {
        $scope.OCN = '',
        $scope.ContactName = '',
        $scope.ContactPhoneNumber = '',
        $scope.ContactEmail = '',
        $scope.CompanyName = '',
        $scope.VATNumber = '',
        $scope.PremisesNumber = '',
        $scope.CompanyStreetName = '',
        $scope.CompanyPostCode = '',
        $scope.CompanyCity = '',
        $scope.CompanyProvince = '',
        $scope.CompanyCountry = null,
        $scope.CompanyLanguage = null;

        setUpFormFields('');
      }

      $scope.showFormModal = function (status) {
        var head, title, message, message2, subHead;
        var showClose = true;

        if (status == 'success') {
          title = $translate.instant("registration.popup-labels.success-header").toString();
          message = $translate.instant("registration.popup-labels.success-message").toString();
        }
        else if (status == 'save-success') {
          title = $translate.instant("registration.popup-labels.save-success-header").toString();
          message = $translate.instant("registration.popup-labels.save-success-message").toString();
        }
        else if (status == 'privacy-link') {
          head = $translate.instant("registration.popup-labels.legal-notice-heading").toString();
          title = $translate.instant("registration.form-labels.legal-notice-header").toString();
          message = $translate.instant("registration.popup-labels.legal-notice-message").toString();
          subHead = $translate.instant("registration.popup-labels.legal-notice-message-subheading").toString();
          message2 = $translate.instant("registration.popup-labels.legal-notice-message2").toString();
        }
        else if (status == 'url-error') {
          title = $translate.instant("registration.popup-labels.url-error-header").toString();
          message = $translate.instant("registration.popup-labels.url-error-message").toString();
          showClose = false;
        }
        else if (status == 'error') {
          title = $translate.instant("registration.popup-labels.error-header").toString();
          message = $translate.instant("registration.popup-labels.error-message").toString();
        }
        else if (status == 'submit-error') {
          title = $translate.instant("registration.popup-labels.submit-error-header").toString();
          message = $translate.instant("registration.popup-labels.error-message").toString();
        }

        var modalInstance = $uibModal.open({
          backdrop: 'static',
          animation: true,
          component: 'registrationSubmissionReceivedModalComponent',
          resolve: {
            messages: function () {return {Title: title, Head: head, Message: message, Message2: message2, SubHead: subHead, ShowClose: showClose}}
          }
        });
      }

      $scope.$watch('[ContactName, ContactPhoneNumber, ContactEmail]', function (newVal, oldVal, scope) {
        var name = newVal[0];
        var phone = newVal[1];
        var email = newVal[2];
        if (name != '' && typeof(name) !== 'undefined' && phone != '' && typeof(phone) !== 'undefined' && email != '' && typeof(email) !== 'undefined') {
          $scope.ContactDetailsComplete = true;
        } else {
          $scope.ContactDetailsComplete = false;
        }
      });

      $scope.$watch('[CompanyName, selectedVals.CompanyCountry, selectedVals.CompanyLanguage, PremisesNumber, CompanyStreetName, CompanyPostCode, CompanyProvince, VATNumber]', function (newVal, oldVal, scope) {
        var CompanyName = newVal[0];
        var CompanyCountry = newVal[1];
        var CompanyLanguage = newVal[2];
        var PremisesNumber = newVal[3];
        var CompanyStreetName = newVal[4];
        var CompanyPostCode = newVal[5];
        var CompanyProvince = newVal[6];
        var VATNumber = newVal[7];

        if ($scope.OCNExists == false
        && CompanyName != '' && typeof(CompanyName) !== 'undefined' 
        && CompanyCountry != '' && typeof(CompanyCountry) !== 'undefined' 
        && CompanyLanguage != '' && typeof(CompanyLanguage) !== 'undefined'
        && PremisesNumber != '' && typeof(PremisesNumber) !== 'undefined'
        && CompanyStreetName != '' && typeof(CompanyStreetName) !== 'undefined'
        && CompanyPostCode != '' && typeof(CompanyPostCode) !== 'undefined'
        && CompanyProvince != '' && typeof(CompanyProvince) !== 'undefined'
        && VATNumber != '' && typeof(VATNumber) !== 'undefined') {
          scope.CompanyDetailsComplete = true;
        }
        else if ($scope.OCNExists == true) {
            scope.CompanyDetailsComplete = true; 
          } else {
          scope.CompanyDetailsComplete = false;
        }
      });

      $scope.$watch('billingAddressFields', function (newValue, oldValue, scope) {
        var NewBillingDetailsComplete = true;
        for (var prop in $scope.billingAddressFields) {
          if (prop != 'selectedVals') {
            var obj = $scope.billingAddressFields[prop];
            
            if (obj.required == true && (obj.value == '' || typeof(obj.value) === 'undefined')) {
              NewBillingDetailsComplete = false;
            }
          }
        }
        $scope.NewBillingDetailsComplete = NewBillingDetailsComplete;
      }, true);

      $scope.$watch('selectedVals.CompanyCountry', function (newVal, oldVal, scope) {
        if (newVal === oldVal || newVal == '') {
          return;
        }

        scope.companyPostCodeRegExp = getPostCodeRegExp(newVal.label);
      });

      $scope.$watch('billingAddressFields.BillingCountry', function (newVal, oldVal, scope) {
        if (newVal === oldVal || newVal == '') {
          return;
        }
        
        scope.billingPostCodeRegExp = getPostCodeRegExp(newVal.label);
      });

      $scope.$watch('OCN', function (newVal, oldVal, scope) {
        if (newVal === oldVal) {
          return false;
        }
        
        $scope.OCNExists = (typeof(newVal) !== 'undefined' && newVal.length > 0) ? true: false;
      });

      $scope.$watch('page.CopyCompanyAddress', function (newVal, oldVal, scope) {
        if (newVal === oldVal) {
          return false;
        }
        if (newVal == true) {
          $scope.billingAddressFields['BillingPremiseNumber'].value = (typeof($scope.PremisesNumber) === 'undefined') ? '': $scope.PremisesNumber;
          $scope.billingAddressFields['BillingStreetName'].value = (typeof($scope.CompanyStreetName) === 'undefined') ? '': $scope.CompanyStreetName;
          $scope.billingAddressFields['BillingPostCode'].value = (typeof($scope.CompanyPostCode) === 'undefined') ? '': $scope.CompanyPostCode;
          $scope.billingAddressFields['BillingCity'].value = (typeof($scope.CompanyCity) === 'undefined') ? '': $scope.CompanyCity;
          $scope.billingAddressFields['BillingProvince'].value = (typeof($scope.CompanyProvince) === 'undefined') ? '': $scope.CompanyProvince;
          $scope.billingAddressFields['BillingCountry'].value = ($scope.selectedVals.CompanyCountry === null) ? '': $scope.selectedVals.CompanyCountry.country;
          $scope.billingAddressFields['BillingCountry'].country = ($scope.selectedVals.CompanyCountry === null) ? '': $scope.selectedVals.CompanyCountry.country;
        }
      });

      $scope.setOCNExists = function () {
        $scope.OCNExists = ($scope.OCNExists == null) ? false : !$scope.OCNExists;
      }

      $scope.selectPaymentMethod = function (method) {
        if ($scope.selectedVals.PaymentMethod.value == method) {return false};
        $scope.selectedVals.PaymentMethod.value = ($scope.selectedVals.PaymentMethod.value == method) ? null: method;
      }

      $scope.toggleNewBillingBool = function (bool) {
        if ($scope.NewBillingDetails == bool) {return false};
        $scope.NewBillingDetails = ($scope.NewBillingDetails == bool) ? null: bool;
      }

      $scope.toggleCompanyAddressBool = function (bool) {
        if ($scope.page.CopyCompanyAddress == bool) {return false};
        $scope.page.CopyCompanyAddress = ($scope.page.CopyCompanyAddress == bool) ? null: bool;
      }

      $scope.selectUserType = function (user, bool) {
        if (user.ExistingColtUser == bool) {return false};
        user.ExistingColtUser = (user.ExistingColtUser == bool) ? null: bool;
      }
/*
      $scope.selectPaymentMethod = function () {
        var thisVal = $scope.selectedVals.PaymentMethod.value;
        for (var a = 0; a < $scope.paymentMethods.length; a++) {
          if ($scope.paymentMethods[a].value == thisVal) {
            $scope.translatedSelectedPaymentMethodLabel = $translate.instant($scope.paymentMethods[a].label).toString();
          }
        }
      }
*/
      $scope.addUser = function () {
        if ($scope.users.length < 5) {
          var userNum = $scope.users.length + 1;
          $scope.users.push({
            'number': userNum,
            'UserInfoColtOnlineUserIDAdmin': '',
            'UserInfoFullNameAdmin': '',
            'UserInfoTitle': {'value':''},
            'UserInfoEmail': '',
            'UserInfoTelephone': '',
            'UserColtOnlinePortalDisplayLanguage': {'value':''},
            'SelectedRole': '',
            'ExistingColtUser': true,
            'detailsComplete': false
          });
          $scope.latestUserNumber += 1; 
        }
      }

      $scope.removeUser = function (user) {
        var indToRemove = null;
        
        for (var a = 0; a < $scope.users.length; a++) {
          if ($scope.users[a].number == user.number) {
            indToRemove = a;
          }
        }
        
        $scope.users.splice(indToRemove, 1);

        for (var a = 0; a < $scope.users.length; a++) {
          $scope.users[a].number = a + 1;
        }

        $scope.latestUserNumber -= 1; 
      }

      $scope.selectUserRole = function (user) {
        var thisRole = user.selectedRole.value;
        for (var a = 0; a < $scope.userRoles.length; a++) {
          if ($scope.userRoles[a].value == thisRole) {
            user.translatedSelectedRoleLabel = $translate.instant($scope.userRoles[a].label).toString();
          }
        }
      }

      $scope.selectUserTitle = function (user) {
        var thisTitle = user.UserInfoTitle.value;
        for (var a = 0; a < $scope.titles.length; a++) {
          if ($scope.titles[a].value == thisTitle) {
            user.translatedUserInfoTitleLabel = $translate.instant($scope.titles[a].label).toString();
          }
        }
      }

      $scope.processForm = function (form, state) {
        $scope.FormSubmissionAttempted = (state == 'save') ? false: true;

        var systemUsersArr = ($scope.users.length == 0) ? "": $scope.users.map(function (x) {
          if (x.ExistingColtUser == true) {
            return ({
              "existing_col_id": x.UserInfoColtOnlineUserIDAdmin,
              "email": x.UserInfoEmail,
              "requested_role": (x.number == 1) ? 'Full' : x.selectedRole,
              "title": "",
              "forename": "",
              "surname": "",
              "telephone": "",
              "language": ""
            })
          } 
          else if (x.ExistingColtUser == false) {
            return ({
              "id": '',
              "new_col_id": true,
              "requested_role": (x.number == 1) ? 'Full' : x.selectedRole,
              "title": x.UserInfoTitle.value,
              "forename": x.UserInfoFullNameAdmin.split(" ")[0],
              "surname": (x.UserInfoFullNameAdmin.split(" ").length > 1) ? x.UserInfoFullNameAdmin.split(" ")[x.UserInfoFullNameAdmin.split(" ").length - 1]: '',
              "email": x.UserInfoEmail,
              "telephone": x.UserInfoTelephone,
              "language": x.UserColtOnlinePortalDisplayLanguage.value,
              "existing_col_id": ""
            })
          }
        });
        
        var data = {
          "colt_creator": "DUMMY VAL",
          "status": $scope.Status,
          "sales_channel": $scope.SalesChannel,
          "ocn": ($scope.OCNExists == true) ? $scope.OCN || '': '',
          "primary_contact_name": $scope.ContactName || '',
          "primary_contact_tel": $scope.ContactPhoneNumber || '',
          "primary_contact_email": $scope.ContactEmail || '',
          "company_name": $scope.CompanyName || '',
          "vat_number": ($scope.OCNExists == false) ? $scope.VATNumber || '': '',
          "address_premise_no": ($scope.OCNExists == false) ? $scope.PremisesNumber || '': '',
          "address_street": ($scope.OCNExists == false) ? $scope.CompanyStreetName || '': '',
          "address_postcode": ($scope.OCNExists == false) ? $scope.CompanyPostCode || '': '',
          "address_city": ($scope.OCNExists == false) ? $scope.CompanyCity || '': '',
          "address_county": ($scope.OCNExists == false) ? $scope.CompanyProvince || '': '',
          "address_country": ($scope.OCNExists == false) ? $scope.selectedVals.CompanyCountry.country || '': '',
          "language": ($scope.OCNExists == false) ? $scope.selectedVals.CompanyLanguage.value || '': '',
          "reference_bcn": ($scope.NewBillingDetails == false) ? $scope.RefBCN || '': '',
          "invoice_title": ($scope.NewBillingDetails == true && $scope.page.EnterInvoiceDetails == true) ? $scope.invoiceDetailsFields.InvoiceTitle.value || '': '',
          "invoice_contact_name": ($scope.NewBillingDetails == true && $scope.page.EnterInvoiceDetails == true) ? $scope.invoiceDetailsFields.InvoiceName.value || '': '',
          "invoice_po_number": ($scope.NewBillingDetails == true && $scope.page.EnterInvoiceDetails == true) ? $scope.invoiceDetailsFields.PO.value || '': '',
          "invoice_email_address": ($scope.NewBillingDetails == true && $scope.page.EnterInvoiceDetails == true) ? $scope.invoiceDetailsFields.InvoiceEmailAddress.value || '': '',
          "invoice_customer_ref": ($scope.NewBillingDetails == true && $scope.page.EnterInvoiceDetails == true) ? $scope.invoiceDetailsFields.CustomerReference.value || '': '',
          "invoice_tel": ($scope.NewBillingDetails == true && $scope.page.EnterInvoiceDetails == true) ? $scope.invoiceDetailsFields.InvoiceTelephoneNumber.value || '': '',
          "invoice_currency": ($scope.NewBillingDetails == true && $scope.page.EnterInvoiceDetails == true) ? $scope.invoiceDetailsFields.Currency.value || '': '',
          "invoice_language": ($scope.NewBillingDetails == true && $scope.page.EnterInvoiceDetails == true) ? $scope.invoiceDetailsFields.InvoiceLanguage.value || '': '',
          "billing_address_premise_no": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.BillingPremiseNumber.value || '': '',
          "billing_address_street": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.BillingStreetName.value || '': '',
          "billing_address_postcode": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.BillingPostCode.value || '': '',
          "billing_address_city": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.BillingCity.value || '': '',
          "billing_address_county": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.BillingProvince.value || '': '',
          "billing_address_country": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.BillingCountry.country || '': '',
          "billing_bank_name": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.BankName.value || '': '',
          "billing_account_name": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.AccountName.value || '': '',
          "billing_payment_method": ($scope.NewBillingDetails == true) ? $scope.selectedVals.PaymentMethod.value || '': '',
          "billing_branch_name": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.BranchName.value || '': '',
          "billing_sort_code": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.SortCode.value || '': '',
          "billing_iban": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.IBAN.value || '': '',
          "billing_bic": ($scope.NewBillingDetails == true) ? $scope.billingAddressFields.BIC.value || '': '',
          "last_updated": "",
          "col_users": systemUsersArr
        }
        
        ApiService.createRegistration($scope.guid, state, data).then(function (d) {
          if (state == 'submit') {
            resetFormData();
            $scope.showFormModal('success');
          } else {
            $scope.showFormModal('save-success');
          }
        }, function (err) {
          if (state == 'submit') {
            $scope.showFormModal('submit-error');
          }
          else {
            $scope.showFormModal('error');
          }
          $window.scrollTo(0, 0);
        });
      }

      function init () {
        //Must include UNICODE for Japanese chars
        var letterOnlyRegExpPart = "A-Za-z\\u00AA\\u00B5\\u00BA\\u00C0-\\u00D6\\u00D8-\\u00F6\\u00F8-\\u02C1\\u02C6-\\u02D1\\u02E0-\\u02E4\\u02EC\\u02EE\\u0370-\\u0374\\u0376\\u0377\\u037A-\\u037D\\u037F\\u0386\\u0388-\\u038A\\u038C\\u038E-\\u03A1\\u03A3-\\u03F5\\u03F7-\\u0481\\u048A-\\u052F\\u0531-\\u0556\\u0559\\u0561-\\u0587\\u05D0-\\u05EA\\u05F0-\\u05F2\\u0620-\\u064A\\u066E\\u066F\\u0671-\\u06D3\\u06D5\\u06E5\\u06E6\\u06EE\\u06EF\\u06FA-\\u06FC\\u06FF\\u0710\\u0712-\\u072F\\u074D-\\u07A5\\u07B1\\u07CA-\\u07EA\\u07F4\\u07F5\\u07FA\\u0800-\\u0815\\u081A\\u0824\\u0828\\u0840-\\u0858\\u08A0-\\u08B4\\u0904-\\u0939\\u093D\\u0950\\u0958-\\u0961\\u0971-\\u0980\\u0985-\\u098C\\u098F\\u0990\\u0993-\\u09A8\\u09AA-\\u09B0\\u09B2\\u09B6-\\u09B9\\u09BD\\u09CE\\u09DC\\u09DD\\u09DF-\\u09E1\\u09F0\\u09F1\\u0A05-\\u0A0A\\u0A0F\\u0A10\\u0A13-\\u0A28\\u0A2A-\\u0A30\\u0A32\\u0A33\\u0A35\\u0A36\\u0A38\\u0A39\\u0A59-\\u0A5C\\u0A5E\\u0A72-\\u0A74\\u0A85-\\u0A8D\\u0A8F-\\u0A91\\u0A93-\\u0AA8\\u0AAA-\\u0AB0\\u0AB2\\u0AB3\\u0AB5-\\u0AB9\\u0ABD\\u0AD0\\u0AE0\\u0AE1\\u0AF9\\u0B05-\\u0B0C\\u0B0F\\u0B10\\u0B13-\\u0B28\\u0B2A-\\u0B30\\u0B32\\u0B33\\u0B35-\\u0B39\\u0B3D\\u0B5C\\u0B5D\\u0B5F-\\u0B61\\u0B71\\u0B83\\u0B85-\\u0B8A\\u0B8E-\\u0B90\\u0B92-\\u0B95\\u0B99\\u0B9A\\u0B9C\\u0B9E\\u0B9F\\u0BA3\\u0BA4\\u0BA8-\\u0BAA\\u0BAE-\\u0BB9\\u0BD0\\u0C05-\\u0C0C\\u0C0E-\\u0C10\\u0C12-\\u0C28\\u0C2A-\\u0C39\\u0C3D\\u0C58-\\u0C5A\\u0C60\\u0C61\\u0C85-\\u0C8C\\u0C8E-\\u0C90\\u0C92-\\u0CA8\\u0CAA-\\u0CB3\\u0CB5-\\u0CB9\\u0CBD\\u0CDE\\u0CE0\\u0CE1\\u0CF1\\u0CF2\\u0D05-\\u0D0C\\u0D0E-\\u0D10\\u0D12-\\u0D3A\\u0D3D\\u0D4E\\u0D5F-\\u0D61\\u0D7A-\\u0D7F\\u0D85-\\u0D96\\u0D9A-\\u0DB1\\u0DB3-\\u0DBB\\u0DBD\\u0DC0-\\u0DC6\\u0E01-\\u0E30\\u0E32\\u0E33\\u0E40-\\u0E46\\u0E81\\u0E82\\u0E84\\u0E87\\u0E88\\u0E8A\\u0E8D\\u0E94-\\u0E97\\u0E99-\\u0E9F\\u0EA1-\\u0EA3\\u0EA5\\u0EA7\\u0EAA\\u0EAB\\u0EAD-\\u0EB0\\u0EB2\\u0EB3\\u0EBD\\u0EC0-\\u0EC4\\u0EC6\\u0EDC-\\u0EDF\\u0F00\\u0F40-\\u0F47\\u0F49-\\u0F6C\\u0F88-\\u0F8C\\u1000-\\u102A\\u103F\\u1050-\\u1055\\u105A-\\u105D\\u1061\\u1065\\u1066\\u106E-\\u1070\\u1075-\\u1081\\u108E\\u10A0-\\u10C5\\u10C7\\u10CD\\u10D0-\\u10FA\\u10FC-\\u1248\\u124A-\\u124D\\u1250-\\u1256\\u1258\\u125A-\\u125D\\u1260-\\u1288\\u128A-\\u128D\\u1290-\\u12B0\\u12B2-\\u12B5\\u12B8-\\u12BE\\u12C0\\u12C2-\\u12C5\\u12C8-\\u12D6\\u12D8-\\u1310\\u1312-\\u1315\\u1318-\\u135A\\u1380-\\u138F\\u13A0-\\u13F5\\u13F8-\\u13FD\\u1401-\\u166C\\u166F-\\u167F\\u1681-\\u169A\\u16A0-\\u16EA\\u16F1-\\u16F8\\u1700-\\u170C\\u170E-\\u1711\\u1720-\\u1731\\u1740-\\u1751\\u1760-\\u176C\\u176E-\\u1770\\u1780-\\u17B3\\u17D7\\u17DC\\u1820-\\u1877\\u1880-\\u18A8\\u18AA\\u18B0-\\u18F5\\u1900-\\u191E\\u1950-\\u196D\\u1970-\\u1974\\u1980-\\u19AB\\u19B0-\\u19C9\\u1A00-\\u1A16\\u1A20-\\u1A54\\u1AA7\\u1B05-\\u1B33\\u1B45-\\u1B4B\\u1B83-\\u1BA0\\u1BAE\\u1BAF\\u1BBA-\\u1BE5\\u1C00-\\u1C23\\u1C4D-\\u1C4F\\u1C5A-\\u1C7D\\u1CE9-\\u1CEC\\u1CEE-\\u1CF1\\u1CF5\\u1CF6\\u1D00-\\u1DBF\\u1E00-\\u1F15\\u1F18-\\u1F1D\\u1F20-\\u1F45\\u1F48-\\u1F4D\\u1F50-\\u1F57\\u1F59\\u1F5B\\u1F5D\\u1F5F-\\u1F7D\\u1F80-\\u1FB4\\u1FB6-\\u1FBC\\u1FBE\\u1FC2-\\u1FC4\\u1FC6-\\u1FCC\\u1FD0-\\u1FD3\\u1FD6-\\u1FDB\\u1FE0-\\u1FEC\\u1FF2-\\u1FF4\\u1FF6-\\u1FFC\\u2071\\u207F\\u2090-\\u209C\\u2102\\u2107\\u210A-\\u2113\\u2115\\u2119-\\u211D\\u2124\\u2126\\u2128\\u212A-\\u212D\\u212F-\\u2139\\u213C-\\u213F\\u2145-\\u2149\\u214E\\u2183\\u2184\\u2C00-\\u2C2E\\u2C30-\\u2C5E\\u2C60-\\u2CE4\\u2CEB-\\u2CEE\\u2CF2\\u2CF3\\u2D00-\\u2D25\\u2D27\\u2D2D\\u2D30-\\u2D67\\u2D6F\\u2D80-\\u2D96\\u2DA0-\\u2DA6\\u2DA8-\\u2DAE\\u2DB0-\\u2DB6\\u2DB8-\\u2DBE\\u2DC0-\\u2DC6\\u2DC8-\\u2DCE\\u2DD0-\\u2DD6\\u2DD8-\\u2DDE\\u2E2F\\u3005\\u3006\\u3031-\\u3035\\u303B\\u303C\\u3041-\\u3096\\u309D-\\u309F\\u30A1-\\u30FA\\u30FC-\\u30FF\\u3105-\\u312D\\u3131-\\u318E\\u31A0-\\u31BA\\u31F0-\\u31FF\\u3400-\\u4DB5\\u4E00-\\u9FD5\\uA000-\\uA48C\\uA4D0-\\uA4FD\\uA500-\\uA60C\\uA610-\\uA61F\\uA62A\\uA62B\\uA640-\\uA66E\\uA67F-\\uA69D\\uA6A0-\\uA6E5\\uA717-\\uA71F\\uA722-\\uA788\\uA78B-\\uA7AD\\uA7B0-\\uA7B7\\uA7F7-\\uA801\\uA803-\\uA805\\uA807-\\uA80A\\uA80C-\\uA822\\uA840-\\uA873\\uA882-\\uA8B3\\uA8F2-\\uA8F7\\uA8FB\\uA8FD\\uA90A-\\uA925\\uA930-\\uA946\\uA960-\\uA97C\\uA984-\\uA9B2\\uA9CF\\uA9E0-\\uA9E4\\uA9E6-\\uA9EF\\uA9FA-\\uA9FE\\uAA00-\\uAA28\\uAA40-\\uAA42\\uAA44-\\uAA4B\\uAA60-\\uAA76\\uAA7A\\uAA7E-\\uAAAF\\uAAB1\\uAAB5\\uAAB6\\uAAB9-\\uAABD\\uAAC0\\uAAC2\\uAADB-\\uAADD\\uAAE0-\\uAAEA\\uAAF2-\\uAAF4\\uAB01-\\uAB06\\uAB09-\\uAB0E\\uAB11-\\uAB16\\uAB20-\\uAB26\\uAB28-\\uAB2E\\uAB30-\\uAB5A\\uAB5C-\\uAB65\\uAB70-\\uABE2\\uAC00-\\uD7A3\\uD7B0-\\uD7C6\\uD7CB-\\uD7FB\\uF900-\\uFA6D\\uFA70-\\uFAD9\\uFB00-\\uFB06\\uFB13-\\uFB17\\uFB1D\\uFB1F-\\uFB28\\uFB2A-\\uFB36\\uFB38-\\uFB3C\\uFB3E\\uFB40\\uFB41\\uFB43\\uFB44\\uFB46-\\uFBB1\\uFBD3-\\uFD3D\\uFD50-\\uFD8F\\uFD92-\\uFDC7\\uFDF0-\\uFDFB\\uFE70-\\uFE74\\uFE76-\\uFEFC\\uFF21-\\uFF3A\\uFF41-\\uFF5A\\uFF66-\\uFFBE\\uFFC2-\\uFFC7\\uFFCA-\\uFFCF\\uFFD2-\\uFFD7\\uFFDA-\\uFFDC",
            wordOnlyRegExpPart = "\\w\\u00AA\\u00B5\\u00BA\\u00C0-\\u00D6\\u00D8-\\u00F6\\u00F8-\\u02C1\\u02C6-\\u02D1\\u02E0-\\u02E4\\u02EC\\u02EE\\u0370-\\u0374\\u0376\\u0377\\u037A-\\u037D\\u037F\\u0386\\u0388-\\u038A\\u038C\\u038E-\\u03A1\\u03A3-\\u03F5\\u03F7-\\u0481\\u048A-\\u052F\\u0531-\\u0556\\u0559\\u0561-\\u0587\\u05D0-\\u05EA\\u05F0-\\u05F2\\u0620-\\u064A\\u066E\\u066F\\u0671-\\u06D3\\u06D5\\u06E5\\u06E6\\u06EE\\u06EF\\u06FA-\\u06FC\\u06FF\\u0710\\u0712-\\u072F\\u074D-\\u07A5\\u07B1\\u07CA-\\u07EA\\u07F4\\u07F5\\u07FA\\u0800-\\u0815\\u081A\\u0824\\u0828\\u0840-\\u0858\\u08A0-\\u08B4\\u0904-\\u0939\\u093D\\u0950\\u0958-\\u0961\\u0971-\\u0980\\u0985-\\u098C\\u098F\\u0990\\u0993-\\u09A8\\u09AA-\\u09B0\\u09B2\\u09B6-\\u09B9\\u09BD\\u09CE\\u09DC\\u09DD\\u09DF-\\u09E1\\u09F0\\u09F1\\u0A05-\\u0A0A\\u0A0F\\u0A10\\u0A13-\\u0A28\\u0A2A-\\u0A30\\u0A32\\u0A33\\u0A35\\u0A36\\u0A38\\u0A39\\u0A59-\\u0A5C\\u0A5E\\u0A72-\\u0A74\\u0A85-\\u0A8D\\u0A8F-\\u0A91\\u0A93-\\u0AA8\\u0AAA-\\u0AB0\\u0AB2\\u0AB3\\u0AB5-\\u0AB9\\u0ABD\\u0AD0\\u0AE0\\u0AE1\\u0AF9\\u0B05-\\u0B0C\\u0B0F\\u0B10\\u0B13-\\u0B28\\u0B2A-\\u0B30\\u0B32\\u0B33\\u0B35-\\u0B39\\u0B3D\\u0B5C\\u0B5D\\u0B5F-\\u0B61\\u0B71\\u0B83\\u0B85-\\u0B8A\\u0B8E-\\u0B90\\u0B92-\\u0B95\\u0B99\\u0B9A\\u0B9C\\u0B9E\\u0B9F\\u0BA3\\u0BA4\\u0BA8-\\u0BAA\\u0BAE-\\u0BB9\\u0BD0\\u0C05-\\u0C0C\\u0C0E-\\u0C10\\u0C12-\\u0C28\\u0C2A-\\u0C39\\u0C3D\\u0C58-\\u0C5A\\u0C60\\u0C61\\u0C85-\\u0C8C\\u0C8E-\\u0C90\\u0C92-\\u0CA8\\u0CAA-\\u0CB3\\u0CB5-\\u0CB9\\u0CBD\\u0CDE\\u0CE0\\u0CE1\\u0CF1\\u0CF2\\u0D05-\\u0D0C\\u0D0E-\\u0D10\\u0D12-\\u0D3A\\u0D3D\\u0D4E\\u0D5F-\\u0D61\\u0D7A-\\u0D7F\\u0D85-\\u0D96\\u0D9A-\\u0DB1\\u0DB3-\\u0DBB\\u0DBD\\u0DC0-\\u0DC6\\u0E01-\\u0E30\\u0E32\\u0E33\\u0E40-\\u0E46\\u0E81\\u0E82\\u0E84\\u0E87\\u0E88\\u0E8A\\u0E8D\\u0E94-\\u0E97\\u0E99-\\u0E9F\\u0EA1-\\u0EA3\\u0EA5\\u0EA7\\u0EAA\\u0EAB\\u0EAD-\\u0EB0\\u0EB2\\u0EB3\\u0EBD\\u0EC0-\\u0EC4\\u0EC6\\u0EDC-\\u0EDF\\u0F00\\u0F40-\\u0F47\\u0F49-\\u0F6C\\u0F88-\\u0F8C\\u1000-\\u102A\\u103F\\u1050-\\u1055\\u105A-\\u105D\\u1061\\u1065\\u1066\\u106E-\\u1070\\u1075-\\u1081\\u108E\\u10A0-\\u10C5\\u10C7\\u10CD\\u10D0-\\u10FA\\u10FC-\\u1248\\u124A-\\u124D\\u1250-\\u1256\\u1258\\u125A-\\u125D\\u1260-\\u1288\\u128A-\\u128D\\u1290-\\u12B0\\u12B2-\\u12B5\\u12B8-\\u12BE\\u12C0\\u12C2-\\u12C5\\u12C8-\\u12D6\\u12D8-\\u1310\\u1312-\\u1315\\u1318-\\u135A\\u1380-\\u138F\\u13A0-\\u13F5\\u13F8-\\u13FD\\u1401-\\u166C\\u166F-\\u167F\\u1681-\\u169A\\u16A0-\\u16EA\\u16F1-\\u16F8\\u1700-\\u170C\\u170E-\\u1711\\u1720-\\u1731\\u1740-\\u1751\\u1760-\\u176C\\u176E-\\u1770\\u1780-\\u17B3\\u17D7\\u17DC\\u1820-\\u1877\\u1880-\\u18A8\\u18AA\\u18B0-\\u18F5\\u1900-\\u191E\\u1950-\\u196D\\u1970-\\u1974\\u1980-\\u19AB\\u19B0-\\u19C9\\u1A00-\\u1A16\\u1A20-\\u1A54\\u1AA7\\u1B05-\\u1B33\\u1B45-\\u1B4B\\u1B83-\\u1BA0\\u1BAE\\u1BAF\\u1BBA-\\u1BE5\\u1C00-\\u1C23\\u1C4D-\\u1C4F\\u1C5A-\\u1C7D\\u1CE9-\\u1CEC\\u1CEE-\\u1CF1\\u1CF5\\u1CF6\\u1D00-\\u1DBF\\u1E00-\\u1F15\\u1F18-\\u1F1D\\u1F20-\\u1F45\\u1F48-\\u1F4D\\u1F50-\\u1F57\\u1F59\\u1F5B\\u1F5D\\u1F5F-\\u1F7D\\u1F80-\\u1FB4\\u1FB6-\\u1FBC\\u1FBE\\u1FC2-\\u1FC4\\u1FC6-\\u1FCC\\u1FD0-\\u1FD3\\u1FD6-\\u1FDB\\u1FE0-\\u1FEC\\u1FF2-\\u1FF4\\u1FF6-\\u1FFC\\u2071\\u207F\\u2090-\\u209C\\u2102\\u2107\\u210A-\\u2113\\u2115\\u2119-\\u211D\\u2124\\u2126\\u2128\\u212A-\\u212D\\u212F-\\u2139\\u213C-\\u213F\\u2145-\\u2149\\u214E\\u2183\\u2184\\u2C00-\\u2C2E\\u2C30-\\u2C5E\\u2C60-\\u2CE4\\u2CEB-\\u2CEE\\u2CF2\\u2CF3\\u2D00-\\u2D25\\u2D27\\u2D2D\\u2D30-\\u2D67\\u2D6F\\u2D80-\\u2D96\\u2DA0-\\u2DA6\\u2DA8-\\u2DAE\\u2DB0-\\u2DB6\\u2DB8-\\u2DBE\\u2DC0-\\u2DC6\\u2DC8-\\u2DCE\\u2DD0-\\u2DD6\\u2DD8-\\u2DDE\\u2E2F\\u3005\\u3006\\u3031-\\u3035\\u303B\\u303C\\u3041-\\u3096\\u309D-\\u309F\\u30A1-\\u30FA\\u30FC-\\u30FF\\u3105-\\u312D\\u3131-\\u318E\\u31A0-\\u31BA\\u31F0-\\u31FF\\u3400-\\u4DB5\\u4E00-\\u9FD5\\uA000-\\uA48C\\uA4D0-\\uA4FD\\uA500-\\uA60C\\uA610-\\uA61F\\uA62A\\uA62B\\uA640-\\uA66E\\uA67F-\\uA69D\\uA6A0-\\uA6E5\\uA717-\\uA71F\\uA722-\\uA788\\uA78B-\\uA7AD\\uA7B0-\\uA7B7\\uA7F7-\\uA801\\uA803-\\uA805\\uA807-\\uA80A\\uA80C-\\uA822\\uA840-\\uA873\\uA882-\\uA8B3\\uA8F2-\\uA8F7\\uA8FB\\uA8FD\\uA90A-\\uA925\\uA930-\\uA946\\uA960-\\uA97C\\uA984-\\uA9B2\\uA9CF\\uA9E0-\\uA9E4\\uA9E6-\\uA9EF\\uA9FA-\\uA9FE\\uAA00-\\uAA28\\uAA40-\\uAA42\\uAA44-\\uAA4B\\uAA60-\\uAA76\\uAA7A\\uAA7E-\\uAAAF\\uAAB1\\uAAB5\\uAAB6\\uAAB9-\\uAABD\\uAAC0\\uAAC2\\uAADB-\\uAADD\\uAAE0-\\uAAEA\\uAAF2-\\uAAF4\\uAB01-\\uAB06\\uAB09-\\uAB0E\\uAB11-\\uAB16\\uAB20-\\uAB26\\uAB28-\\uAB2E\\uAB30-\\uAB5A\\uAB5C-\\uAB65\\uAB70-\\uABE2\\uAC00-\\uD7A3\\uD7B0-\\uD7C6\\uD7CB-\\uD7FB\\uF900-\\uFA6D\\uFA70-\\uFAD9\\uFB00-\\uFB06\\uFB13-\\uFB17\\uFB1D\\uFB1F-\\uFB28\\uFB2A-\\uFB36\\uFB38-\\uFB3C\\uFB3E\\uFB40\\uFB41\\uFB43\\uFB44\\uFB46-\\uFBB1\\uFBD3-\\uFD3D\\uFD50-\\uFD8F\\uFD92-\\uFDC7\\uFDF0-\\uFDFB\\uFE70-\\uFE74\\uFE76-\\uFEFC\\uFF21-\\uFF3A\\uFF41-\\uFF5A\\uFF66-\\uFFBE\\uFFC2-\\uFFC7\\uFFCA-\\uFFCF\\uFFD2-\\uFFD7\\uFFDA-\\uFFDC";

        $scope.loading = true;
        $scope.logo = "assets/images/colt-logo-light@2x.png";
        $scope.page = {};

        $scope.titles = [
          {
            'label': "registration.form-labels.title-sir",
            'value': "Sir"
          },
          {
            'label': "registration.form-labels.title-mr",
            'value': "Mr"
          },
          {
            'label': "registration.form-labels.title-mrs",
            'value': "Mrs"
          },
          {
            'label': "registration.form-labels.title-miss",
            'value': "Miss"
          },
          {
            'label': "registration.form-labels.title-ms",
            'value': "Ms"
          }
        ];
        $scope.userRoles = [
          {
            'label': "registration.form-labels.full",
            'description': "registration.form-labels.full-role-description",
            'value': 'Full',
            'selected': true
          },
          {
            'label': "registration.form-labels.primary",
            'description': "registration.form-labels.primary-role-description",
            'value': 'Primary'
          },
          {
            'label': "registration.form-labels.flex",
            'description': "registration.form-labels.flex-role-description",
            'value': 'Flex'
          },
          {
            'label': "registration.form-labels.read-only",
            'description': "registration.form-labels.read-only-role-description",
            'value': 'Read Only'
          }
        ];
        $scope.languages = [
          {
            'label': "English",
            'value': "English",
            'country': "England",
            'selected': true
          },
          {
            'label': "Deutsch",
            'value': "German",
            'country': "Germany"
          },
          {
            'label': "Español",
            'value': "Spanish",
            'country': "Spain"
          },
          {
            'label': "Français",
            'value': "French",
            'country': "France"
          },
          {
            'label': "Italiano",
            'value': "Italian",
            'country': "Italy"
          },
          {
            'label': "日本語",
            'value': "Japanese",
            'country': "Japan"
          }
        ];
        $scope.countryLabels = {
          'England': 'English',
          'Germany': 'Deutsch',
          'Spain': 'Español',
          'France': 'Français',
          'Italy': 'Italiano',
          'Austria': 'Austria',
          'Belgium': 'Belgium',
          'Denmark': 'Denmark',
          'Finland': 'Finland',
          'Hong Kong': 'Hong-Kong',
          'Ireland': 'Ireland',
          'Netherlands': 'Netherlands',
          'Norway': 'Norway',
          'Portugal': 'Portugal',
          'Singapore': 'Singapore',
          'Sweden': 'Sweden',
          'Switzerland': 'Switzerland',
          'United Kingdom': 'United Kingdom'
        };
        $scope.paymentMethods = [
          {
            'label': 'Bank Tansfer',
            'value': 'Bank Transfer'
          },
          {
            'label': 'Direct Debit',
            'value': 'Direct Debit'
          }
        ];
        $scope.countryOptions = [
          { 
            'country':'Austria',
            'value':'Austria',
            'label':'Austria',
            'form-validation-rules': {
              'postcode-regexp': "[0-9]{4}$",
              'phone-min-length': 8
            }
          },
          { 
            'country':'Belgium',
            'value':'Belgium',
            'label':'Belgium',
            'form-validation-rules': {
              'postcode-regexp': "[0-9]{4}$",
              'phone-min-length': 8
            }
          },
          { 
            'country':'Denmark',
            'value':'Denmark',
            'label':'Denmark',
            'form-validation-rules': {
              'postcode-regexp': "[0-9]{3,4}$",
              'phone-min-length': 8
            }
          },
          { 
            'country':'Finland',
            'value':'Finland',
            'label':'Finland',
            'form-validation-rules': {
              'postcode-regexp': "\\d{5}$",
              'phone-min-length': 8
            }
          },
          { 
            'country':'France',
            'value':'France',
            'label':'France',
            'form-validation-rules': {
              'postcode-regexp': "[0-9]{5}$"
            }
          },
          { 
            'country':'Germany',
            'value':'Germany',
            'label':'Germany',
            'form-validation-rules': {
              'postcode-regexp': "(?!01000|99999)(0[1-9]\\d{3}|[1-9]\\d{4})$",
              'phone-min-length': 10
            }
          },
          { 
            'country':'Hong-Kong',
            'value':'Hong-Kong',
            'label':'Hong-Kong',
            'form-validation-rules': {
              'postcode-regexp': "\\d{6}$",
              'phone-min-length': 10
            }
          },
          { 
            'country':'Ireland',
            'value':'Ireland',
            'label':'Ireland',
            'form-validation-rules': {
              'postcode-regexp': "[AC-FHKNPRTV-Y]{1}[0-9]{1}[0-9W]{1}[ \-]?[0-9AC-FHKNPRTV-Y]{4}$",
              'phone-min-length': 10
            }
          },
          { 
            'country':'Italy',
            'value':'Italy',
            'label':'Italy',
            'form-validation-rules': {
              'postcode-regexp': "[0-9]{5}$"
            }
          },
          { 
            'country':'Japan',
            'value':'Japan',
            'label':'Japan',
            'form-validation-rules': {
              'postcode-regexp': "\\d{3}-\\d{4}$"
            }
          },
          { 
            'country':'Netherlands',
            'value':'Netherlands',
            'label':'Netherlands',
            'form-validation-rules': {
              'postcode-regexp': "[1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2}$"
            }
          },
          { 
            'country':'Norway',
            'value':'Norway',
            'label':'Norway',
            'form-validation-rules': {
              'postcode-regexp': "\\d{4}$"
            }
          },
          { 
            'country':'Portugal',
            'value':'Portugal',
            'label':'Portugal',
            'form-validation-rules': {
              'postcode-regexp': "\\d{4}([- ]\\d{3})?$"
            }
          },
          { 
            'country':'Singapore',
            'value':'Singapore',
            'label':'Singapore',
            'form-validation-rules': {
              'postcode-regexp': "\\d{2}(\\d{2})?(\\d{2})?$"
            }
          },
          { 
            'country':'Spain',
            'value':'Spain',
            'label':'Spain',
            'form-validation-rules': {
              'postcode-regexp': "((0[1-9]|5[0-2])|[1-4][0-9])[0-9]{3}$"
            }
          },
          { 
            'country':'Sweden',
            'value':'Sweden',
            'label':'Sweden',
            'form-validation-rules': {
              'postcode-regexp': "\\d{5}\\s\\(\\d{3}\\s\\d{2}\\)$"
            }
          },
          { 
            'country':'Switzerland',
            'value':'Switzerland',
            'label':'Switzerland',
            'form-validation-rules': {
              'postcode-regexp': "\\d{4}$"
            }
          },
          { 
            'country':'United Kingdom',
            'value':'United Kingdom',
            'label':'United Kingdom',
            'form-validation-rules': {
              'postcode-regexp': "[a-zA-Z]{1,2}\\d{1,2}[a-zA-Z]{0,1}\\s*\\d[a-zA-Z]{2}$",
              'phone-min-length': 8
            }
          }
        ];

        $scope.postCodeRegExp = getPostCodeRegExp(LOCALES.preferredLocale);
        $scope.companyPostCodeRegExp = getPostCodeRegExp(LOCALES.preferredLocale);
        $scope.billingPostCodeRegExp = getPostCodeRegExp(LOCALES.preferredLocale);
        $scope.latestUserNumber = 1;
        $scope.currentFormSection = 1;
        
        $scope.emailRegExp = "^[" + wordOnlyRegExpPart + "]+([\\.-]?";
        $scope.emailRegExp += "[" + wordOnlyRegExpPart + "]+)";
        $scope.emailRegExp += "*@[" + wordOnlyRegExpPart + "]+([\\.-]?";
        $scope.emailRegExp += "[" + wordOnlyRegExpPart + "]+)*(\\.";
        $scope.emailRegExp += "[" + wordOnlyRegExpPart + "]{2,3})+$";
        $scope.letterAndSpaceOnlyRegExp = "^[\\s" + letterOnlyRegExpPart + "]+$";
        $scope.fullNameRegExp = "^[" + letterOnlyRegExpPart + "]";
        $scope.fullNameRegExp += "([-']?[" + letterOnlyRegExpPart + "]+)";
        $scope.fullNameRegExp += "*( [" + letterOnlyRegExpPart + "]";
        $scope.fullNameRegExp += "([-']?[" + letterOnlyRegExpPart + "]+)*)+$";
        $scope.phoneNumRegExp = "(?=.*\\d)[\\d ]+$";
        $scope.refBCNRegExp = /^[0-9]{6}$/;
        $scope.ocnRegExp = /^[a-zA-Z0-9]+$/;
        $scope.guid = $routeParams.guid;
        $scope.OCN = "";

        if (!$scope.guid || $scope.guid == '') {
          $scope.showFormModal('url-error');
        } else {
          ApiService.getSupportedCurrencies().then(function (sdata) {
            $scope.currencies = sdata.map(function (x) {
              return ({label: x.currency_code, value: x.currency_code});
            });
          }, function (edata) {
            console.log(edata);
          });

          ApiService.getGuid($scope.guid).then(function (data) {
            setUpFormFields(data);
          }, function (err) {
            $scope.showFormModal('url-error');
          });
        }
      }
      
      init();
  }

  

]);

angular.module('app')
  .controller('SystemUserController', ['$scope', '$rootScope',
    function($scope, $rootScope) {
    
    $scope.$watch('[user.number, user.selectedRole, user.UserInfoTitle, user.UserInfoFullNameAdmin, user.UserInfoTelephone, user.UserInfoEmail, user.UserInfoColtOnlineUserIDAdmin, user.UserColtOnlinePortalDisplayLanguage]', function (newVal, oldVal, scope) {
      var user = newVal;

      for (var a = 0; a < $scope.users.length; a++) {
        if ($scope.users[a]['number'] == newVal[0]) {
          
          if ($scope.users[a].ExistingColtUser == false &&
            $scope.users[a].selectedRole != null &&
            $scope.users[a].UserInfoTitle.value != '' &&
            $scope.users[a].UserInfoFullNameAdmin != '' && typeof($scope.users[a].UserInfoFullNameAdmin) !== 'undefined' &&
            $scope.users[a].UserInfoTelephone != '' && typeof($scope.users[a].UserInfoTelephone) !== 'undefined' &&
            $scope.users[a].UserColtOnlinePortalDisplayLanguage.value != '') {
              $scope.users[a].detailsComplete = true;
          } else if ($scope.users[a].ExistingColtUser == true &&
            $scope.users[a].UserInfoColtOnlineUserIDAdmin != '' &&
            $scope.users[a].UserInfoEmail != '' &&
            typeof($scope.users[a].UserInfoEmail) !== 'undefined') {
            $scope.users[a].detailsComplete = true;  
          }else {
            $scope.users[a].detailsComplete = false;
          }
        }
      }
      
    });

  }
]);

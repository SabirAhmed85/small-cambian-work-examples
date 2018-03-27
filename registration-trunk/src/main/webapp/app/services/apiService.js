'use strict';

angular.module('app')
.service('ApiService', ['$http', '$q',
  function($http, $q) {

        this.getGuid = function (guid) {
          var deferred = $q.defer();
          $http.get('/api/registration/'+guid).then(function (res) {
            if (res.status === 200 || res.status === 201) {
              deferred.resolve(res.data);
            } else {
              deferred.reject(res);
            }
          }, function (resErr) {
            deferred.reject(resErr);
          });
          
          return deferred.promise;
        }

        this.getSupportedCurrencies = function () {
          var deferred = $q.defer();
          
          /* http://amsnov02:8080/registration-api/registration */
          $http.get('/api/supported_currencies').then(function (res) {
            if (res.status === 200 || res.status === 201) {
              deferred.resolve(res.data);
            } else {
              deferred.reject(res);
            }
          }, function (resErr) {
            deferred.reject(resErr);
          });
      
          return deferred.promise;
        }

        this.createRegistration = function (guid, action, registrationData) {
            var deferred = $q.defer();
            
            /* http://amsnov02:8080/registration-api/registration */
            $http.put('/api/registration/' + guid + '?action=' + action, registrationData).then(function (res) {
              if (res.status === 200 || res.status === 201) {
                deferred.resolve(res.data);
              } else {
                deferred.reject(res);
              }
            }, function (resErr) {
              deferred.reject(resErr);
            });
        
            return deferred.promise;
        }
    }
  
]);

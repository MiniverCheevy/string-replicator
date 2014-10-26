//***************************************************************
//This code just called you a tool
//What I meant to say is that this code was generated by a tool
//so don't mess with it unless you're debugging
//subject to change without notice, might regenerate while you're reading, etc
//***************************************************************

(function() {
    angular
        .module('app')
        .factory('sessionFactory', ['$http', function($http) {

            var urlBase = '/api/session';
            var sessionFactory = {};


            sessionFactory.delete = function(request) {
                var operation = $http({ method: 'DELETE', url: urlBase, params: request });
                return operation.then(function(data, status, headers, config) {
                    return data.data;
                }, function(error) {
                    console.log(error);
                    return { isOk: false, message: error };
                });

            };
		

            return sessionFactory;
        }]);
}())
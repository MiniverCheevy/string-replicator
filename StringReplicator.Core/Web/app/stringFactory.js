//***************************************************************
//This code just called you a tool
//What I meant to say is that this code was generated by a tool
//so don't mess with it unless you're debugging
//subject to change without notice, might regenerate while you're reading, etc
//***************************************************************

(function() {
    angular
        .module('app')
        .factory('stringFactory', ['$http', function($http) {

            var urlBase = '/api/string';
            var stringFactory = {};


            stringFactory.get = function(request) {
                var operation = $http({ method: 'GET', url: urlBase, params: request });
                return operation.then(function(data, status, headers, config) {
                    return data.data;
                }, function(error) {
                    console.log(error);
                    return { isOk: false, message: error };
                });

            };
		

            return stringFactory;
        }]);
}())
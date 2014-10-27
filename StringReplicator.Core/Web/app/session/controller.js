(function () {
    "use strict";    
    angular.module('app')
        .controller("session", ['$scope','sessionFactory',
    function ($scope,sessionFactory) {
        //TODO: $window does not work
                    window.onbeforeunload = 
                      function () {
                          sessionFactory.delete({}); };
                  }
       ]);
}())




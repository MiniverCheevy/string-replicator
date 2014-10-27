(function() {
    "use strict";
    angular.module('app')
        .controller("string", ['$scope', '$window', 'stringFactory', 'dataFactory',
            function ($scope, $window, stringFactory, dataFactory) {
                $scope.detail = {};

                $window.onbeforeunload =
                   function () {
                       debugger;
                       stringFactory.post($scope.detail);
                   };

                dataFactory.get({}).then($scope.loadLastRequest);

                $scope.loadLastRequest = function (response) {
                    if (response.isOk) {
                        $scope.detail.formatString = response.formatString;
                        $scope.detail.dataString = response.dataString;
                    } else {
                        $scope.error = response.message;
                    }
                };
                

                $scope.format = function() {
                    stringFactory.get($scope.detail).then($scope.replaceOutput);
                };

                $scope.replaceOutput = function(response) {                    
                    if (response.isOk) {
                        $scope.detail.output = response.text;
                    } else {
                        $scope.error = response.message;
                    }
                };

                $scope.clearMessages = function() {
                    $scope.error = '';
                    $scope.info = '';
                };
            }
        ]);
}())
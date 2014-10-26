(function() {
    "use strict";
    angular.module('app')
        .controller("string", ['$scope', 'stringFactory',
            function($scope, stringFactory) {

                $scope.detail = {};
                $scope.detail.dataString = 'a,b,c';
                $scope.detail.formatString = '{0}--{1}';
                $scope.format = function() {
                    debugger;
                    stringFactory.get($scope.detail).then($scope.replaceOutput);
                };

                $scope.replaceOutput = function(response) {
                    debugger;
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
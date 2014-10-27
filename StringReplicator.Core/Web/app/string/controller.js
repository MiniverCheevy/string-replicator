(function() {
    "use strict";
    angular.module('app')
        .controller("string", ['$scope', 'stringFactory', 'dataFactory',
            function($scope, stringFactory, dataFactory) {
                $scope.detail = {};

                $scope.loadLastRequest = function(response) {
                    if (response.isOk) {
                        $scope.detail.formatString = response.data.formatString;
                        $scope.detail.dataString = response.data.dataString;
                    } else {
                        $scope.error = response.message;
                    }
                };

                dataFactory.get({}).then($scope.loadLastRequest);

                $scope.format = function() {
                    stringFactory.post($scope.detail).then($scope.replaceOutput);
                };

                $scope.replaceOutput = function(response) {
                    if (response.isOk) {
                        $scope.detail.output = response.text;
                    } else {
                        $scope.error = response.message;
                        $scope.failure(response.details, $scope.form);
                    }
                };

                $scope.clearMessages = function() {
                    $scope.error = '';
                    $scope.info = '';
                };

                $scope.sortAndDisting = function () {

                };

                $scope.failure = function(details, form) {
                    $scope.detail.errors = {};
                    angular.forEach(details, function(e) {
                        var field = e.key.charAt(0).toLowerCase() + e.key.slice(1);
                        form[field].$setValidity("server", false);
                        $scope.detail.errors[field] = e.value;
                    });
                };
            }
        ]);
}())
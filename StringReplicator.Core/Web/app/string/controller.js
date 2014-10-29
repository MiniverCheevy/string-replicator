(function () {
    "use strict";
    angular.module('app')
        .controller("string", ['$scope', 'stringFactory', 'historyFactory', 'cleanFactory', 'voodooUtility',
            function ($scope, stringFactory, historyFactory, cleanFactory, util) {
                
                $scope.detail = {};
                $scope.detail.database = {};
                $scope.isTested = false;
                
                self = this;
                
                this.loadLastRequest = function (response) {
                    if (util.isOk($scope,response))
                        $scope.detail = response.data;
                };
                
                historyFactory.get({}).then(self.loadLastRequest);

                $scope.format = function () {
                    stringFactory.post($scope.detail).then(self.replaceOutput);
                };

                this.replaceOutput = function (response) {
                    if (util.isOk($scope,response,$scope.form))
                        $scope.detail.output = response.text;
                };
                $scope.clean = function () {
                    var request = {};
                    request.text =  $scope.detail.dataString;
                    cleanFactory.post(request).then(self.cleanDone);
                };
                this.cleanDone = function (response) {
                    if (util.isOk($scope,response))
                        $scope.detail.dataString = response.text;                    
                };                
            }
        ]);
}())
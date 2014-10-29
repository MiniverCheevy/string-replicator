(function() {
    angular.module('voodoo.ui.database', ['ui.bootstrap'])
        .controller('voodooDatabaseController', ['$scope','$modalInstance','testFactory',
            'voodooUtility','databaseFactory',
            function ($scope, $modalInstance, testFactory,util, databaseFactory)
            {

                //http://stackoverflow.com/questions/19312936/angularjs-modal-dialog-form-object-is-undefined-in-controller
                $scope.form = {};
                $scope.form.details = {};

                $scope.modalInstance = $modalInstance;
                $scope.detail = $scope.$parent.database;
                $scope.detail.dataBaseType = 'none';
                    
                self = this;
                
                    $scope.ok = function () {
                        $scope.modalInstance.close($scope.detail);
                    };

                    $scope.cancel = function () {
                        $scope.modalInstance.dismiss('cancel');
                    };
                    $scope.test = function () {
                        $scope.isTested=false;
                        testFactory.get($scope.detail).then(self.testDone);
                    };
                    this.testDone=function(response)
                    {
                        if (util.isOk($scope, response, $scope.form.details))
                            $scope.isTested=true;
                    };
                    $scope.query = function () {
                        $scope.isTested=false;
                        databaseFactory.get($scope.detail).then(self.queryDone);
                    };
                    this.queryDone=function(response)
                    {
                        if (util.isOk($scope, response, $scope.form.details))
                        {
                            $scope.isTested=true;
                            $scope.detail.result=response.text;
                            $scope.info = 'Done. Click ok to update your csv data.';
                        }
                    };                    
             }])
        .directive('vDatabase', ['$modal',
            function( $modal) {               
                return {
                    restrict: 'A',
                    scope: {
                        database: "=",
                        dataString: "="

                    },           
                    link: function(scope, element, attrs) {
                     
                        element.bind('click', function() {
                            var modalInstance = $modal.open({
                                templateUrl: 'app/voodoo/voodooDatabase.tmpl.html',
                                controller: 'voodooDatabaseController',
                                scope: scope,    
                                size: 'lg'
                            });
                            modalInstance.result.then(function(database) {
                                scope.database = database;
                                scope.dataString = database.result;                                
                            }, function() {
                                //Modal dismissed
                            });

                        });
                    }
                };
            }]);
}())
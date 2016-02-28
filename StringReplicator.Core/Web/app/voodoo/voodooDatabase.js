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
                $scope.isComplete=false;

                self = this;
                
                    $scope.ok = function () {
                        $scope.modalInstance.close($scope.detail);
                    };

                    $scope.cancel = function () {
                        $scope.modalInstance.dismiss('cancel');
                    };
                    $scope.invoke = function () {                        
                        databaseFactory.put({}).then($scope.invokeDone);
                    };
                    $scope.invokeDone=function(response)
                    {
                        if (util.isOk($scope, response, $scope.form.details))
                            $scope.database.udlFile = response.text;
                    };
                    $scope.test = function () {
                        $scope.isTested=false;
                        testFactory.get($scope.detail).then($scope.testDone);
                    };
                    $scope.testDone=function(response)
                    {
                        if (util.isOk($scope, response, $scope.form.details))
                            $scope.isTested=true;
                    };
                    $scope.query = function () {
                        
                        $scope.isComplete=false;
                        databaseFactory.get($scope.detail).then($scope.queryDone);
                    };
                    $scope.queryDone=function(response)
                    {
                        if (util.isOk($scope, response, $scope.form.details))
                        {
                            $scope.isComplete=true;
                            $scope.detail.result=response.text;
                            $scope.info = 'Done. '+ response.numberOfRowsEffected+' row(s) returned. Click ok to update your csv data.';
                        }
                    };                    
             }])
        .directive('vDatabase', ['$modal',
            function( $modal) {               
                return {
                    restrict: 'A',
                    scope: {
                        database: "=",
                        data: "="

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
                                scope.data = database.result;                                
                            }, function() {
                                //Modal dismissed
                            });

                        });
                    }
                };
            }]);
}())
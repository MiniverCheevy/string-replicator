(function() {
    angular.module('voodoo.ui.utility', [])
        .service('voodooUtility', ['$q', function($q) {
            this.isNullOrEmpty = function(value) {
                var isNullOrEmpty = true;
                if (value) {
                    if (typeof(value) == 'string') {
                        if (value.length > 0)
                            isNullOrEmpty = false;
                    }
                }
                return isNullOrEmpty;
            };
	    this.isOk = function($scope, response, form) {
                $scope.error = '';
                $scope.info = '';
                if (!response.isOk) {
                    $scope.error = response.message;
                    if (form) {
                        $scope.detail.errors = {};
                        angular.forEach(response.details, function(e) {
                            var field = e.key.charAt(0).toLowerCase() + e.key.slice(1);
                            form[field].$setValidity("server", false);
                            $scope.detail.errors[field] = e.value;
                        });
                    }
                }
                return response.isOk;
            };
            this.getFormHelper = function ($scope, form) {
                debugger;
                var helper = {};
                helper.form = form;
                helper.scope = $scope;
                helper.isOk = function(response) {
                    helper.scope.error = '';
                    helper.scope.info = '';
                    var deferred = $q.defer();
                    
                        setTimeout(function() {
                            if (response.isOk) {
                                deferred.resolve(response);
                            } else {
                                helper.scope.error = response.message;
                                if (form) {
                                    helper.scope.detail.errors = {};
                                    angular.forEach(response.details, function(e) {
                                        var field = e.key.charAt(0).toLowerCase() + e.key.slice(1);
                                        helper.form[field].$setValidity("server", false);
                                        helper.scope.detail.errors[field] = e.value;
                                    });                                    
                                    deferred.reject();
                                }
                            }
                        }, 1);

                    return deferred.promise;
                };
                return helper;
            };
        }]);
}())
        
                //this.isOk = function($scope, response, form) {
                //    $scope.error = '';
                //    $scope.info = '';
                //    if (!response.isOk) {
                //        $scope.error = response.message;
                //        if (form) {
                //            $scope.detail.errors = {};
                //            angular.forEach(response.details, function(e) {
                //                var field = e.key.charAt(0).toLowerCase() + e.key.slice(1);
                //                form[field].$setValidity("server", false);
                //                $scope.detail.errors[field] = e.value;
                //            });
                //        }
                //    }
                //    return response.isOk;
                //};

            //}
            
            //]);
        //}())
(function() {
    angular.module('voodoo.ui.utility', [])
        .service('voodooUtility', [ function() {
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

        }]);
}())
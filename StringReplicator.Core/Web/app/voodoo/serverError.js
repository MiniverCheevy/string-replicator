(function() {
    angular.module('serverError', [])
        .directive("serverError", [function() {
            //http://icanmakethiswork.blogspot.com/2014/08/angularjs-meet-aspnet-server-validation.html
            // Usage:
            // <input class="col-xs-12 col-sm-9" 
            //        name="sage.name" ng-model="vm.sage.name" server-error="vm.errors" />
            return {
                restrict: "A",
                require: "ngModel",
                scope: {
                    name: "@",
                    serverError: "="
                },

                link: function(scope, element, attrs, ngModelController) {

                    // Bootstrap alert template for error
                    //var template = '<div class="alert alert-danger" role="alert">' +
                    //    '<i class="glyphicon glyphicon-warning-sign"></i> ' +
                    //    '%error%</div>';
                    //    var template = '<span class="label label-danger"><i class="glyphicon glyphicon-warning-sign"></i>&nbsp;' +
                    //'%error%</span>';
                    var template = '<span class="text-danger"><i class="glyphicon glyphicon-warning-sign"></i>&nbsp;' +
                        '%error%</span>';

                    // Create an element to hold the validation message
                    var decorator = angular.element(' <div class="col-lg-4"></div>');
                    //element.after(decorator);
                    element.parent().after(decorator);

                    // Watch ngModelController.$error.server & show/hide validation accordingly
                    scope.$watch(function() {
                        try {
                            return ngModelController.$error.server;
                        } catch(e) {
                            return null;
                        }
                    }, showHideValidation);

                    function showHideValidation(serverError) {
                        // Display an error if serverError is true otherwise clear the element
                        var errorHtml = "";
                        if (serverError) {
                            // Aliasing serverError and name to make it more obvious what their purpose is
                            var errorDictionary = scope.serverError;
                            var errorKey = scope.name;
                            errorHtml = template.replace(/%error%/, errorDictionary[errorKey] || "Unknown error occurred...");
                        }
                        decorator.html(errorHtml);
                    }

                    // wipe the server error message upon keyup or change events so can revalidate with server
                    element.on("keyup change", function(event) {
                        scope.$apply(function() {
                            ngModelController.$setValidity("server", true);
                        });
                    });
                }
            };

        }]);
}())
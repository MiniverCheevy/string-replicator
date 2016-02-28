(function() {

    //derived from http://icanmakethiswork.blogspot.com/2014/08/angularjs-meet-aspnet-server-validation.html
    // Usage:
    // <input class="col-xs-12 col-sm-9" 
    //        name="sage.name" ng-model="vm.sage.name" server-error="vm.errors" />
    angular.module('voodoo.ui.serverError', [])
        .controller("voodooServerErrorController", ['$scope', function($scope) {
            self = this;
            self.scope = $scope;

            this.showHideValidation = function(serverError) {
                var errorHtml = "";
                if (serverError) {
                    var errorDictionary = scope.serverError;
                    var errorKey = scope.name;
                    errorHtml = scope.template.replace(/%error%/, errorDictionary[errorKey] || "Unknown error occurred...");
                }
                scope.controlScope.decorator.html(errorHtml);
            };
            this.init = function(controlScope) {
                scope.controlScope = controlScope;

                controlScope.$watch(function() {
                    try {
                        return controlScope.$error.server;
                    } catch(e) {
                        return null;
                    }
                }, this.showHideValidation);

                controlScope.element.on("keyup change", function(event) {
                    controlScope.$apply(function() {
                        controlScope.$setValidity("server", true);
                    });
                });
            };
        }])
        .directive("serverError", [function() {
            return {
                restrict: "A",
                controller: 'voodooServerErrorController',
                scope: {
                    name: "@",
                    serverError: "=",
                },
                link: function(scope, element, attrs, voodooServerErrorController) {
                    var template = '<span class="text-danger"><i class="glyphicon glyphicon-warning-sign"></i>&nbsp;' +
                        '%error%</span>';
                    var decorator = angular.element(' <div class="col-lg-4"></div>');
                    element.parent().after(decorator);
                    voodooServerErrorController.init(scope);
                }
            };
        }])
        .directive("serverErrorBelow", [function() {

            return {
                restrict: "A",
                controller: 'voodooServerErrorController',
                require: ngModel,
                scope: {
                    name: "@",
                    serverError: "=",
                },
                link: function(scope, element, attrs, voodooServerErrorController) {
                    var template = '<div class="alert alert-danger" role="alert">' +
                        '<i class="glyphicon glyphicon-warning-sign"></i> ' +
                        '%error%</div>';
                    var decorator = angular.element('<div></div>');
                    element.after(decorator);
                    voodooServerErrorController.init(element, decorator, scope);
                }
            };

        }]);
}())
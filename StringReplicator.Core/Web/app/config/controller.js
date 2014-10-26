(function() {
    "use strict";
    angular.module('app')
        .controller("config", ['$scope', 'sessionFactory',
            function($scope, sessionFactory) {
                debugger;
                window.onbeforeunload =
                    function() {
                        debugger;
                        sessionFactory.delete({});
                    };
            }
        ]);
}())
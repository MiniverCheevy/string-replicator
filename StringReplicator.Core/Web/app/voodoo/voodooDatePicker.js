(function () {
    angular.module('voodoo.ui.datePicker', [])
        .controller('voodooDatePickerController',
        ['$scope', function ($scope) {
            $scope.open = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();

                $scope.opened = true;
            };

        }])
    .directive('vDatePicker', function () {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                'value': '=',
                id:'@',
                placeholder:'@'
                
            },
            controller: 'voodooDatePickerController',
            templateUrl: '/app/voodoo/voodooDatePicker.tmpl.html',

            link: function (scope, element, attrs, voodooSorterController) {

                
            }
        };

    });
}())
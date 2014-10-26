(function () {
angular.module('voodoo.ui.sorter', [])
    .controller('voodooSorterController', ['$scope', function($scope) {
        self.scope = $scope;
        $scope.currentSortKey = '';
      

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

        this.init = function (element, callback, text, sort) {
            self.$element = element;
            $scope.callback = callback;
            $scope.sortMember = sort;
            $scope.text = text;
        };
        this.stateChanged = function(gridState) {
            if (gridState != null) {
                $scope.gridState = gridState;
                this.setup();
            }
        };
        this.setup = function() {
            $scope.currentSort = $scope.gridState.sortDirection + $scope.gridState.sortMember;
        };
        $scope.isCurrentSortAsc = function(member) {
            return $scope.gridState != null && $scope.gridState.sortMember.toUpperCase() == member.toUpperCase()
                && $scope.gridState.sortDirection.toUpperCase() == "ASC";
        };
        $scope.isCurrentSortDesc = function(member) {
            return $scope.gridState != null && $scope.gridState.sortMember.toUpperCase() == member.toUpperCase()
                && $scope.gridState.sortDirection.toUpperCase() == "DESC";
        };
        $scope.sort = function(member) {
            if (member.toUpperCase() == $scope.gridState.sortMember.toUpperCase())
                $scope.gridState.sortDirection = $scope.gridState.sortDirection === "ASC" ? "DESC" : "ASC";
            else
                $scope.gridState.sort = "ASC";
            $scope.gridState.sortMember = member;
            $scope.callback(scope, {});
        };


    }])
    .directive('vSorter', function() {
        return {
            restrict: 'EA',
            replace: true,
            scope: {
                gridState: '=gridState',
                text: "@",
                member: "@",
                onsort: '&',
            },
            controller: 'voodooSorterController',
            templateUrl: '/app/voodoo/voodooSorter.tmpl.html',

            link: function(scope, element, attrs, voodooSorterController) {

                scope.$watch('gridState', function() {
                    if (scope.gridState != null) {
                        voodooSorterController.stateChanged(scope.gridState);
                        voodooSorterController.init(element, scope.onsort, attrs.text, attrs.sort);
                    }
                });
            }
        };

    });
}())
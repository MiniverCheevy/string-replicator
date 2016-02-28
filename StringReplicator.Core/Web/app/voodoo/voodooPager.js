(function() {

    angular.module('voodoo.ui.pager', [])
        .controller('voodooPagerController', ['$scope', function($scope) {
            self.scope = $scope;
            $scope.pageBlock = [];
            $scope.isLastBlock = false;
            $scope.isFirstBlock = false;
            $scope.isFirstPage = false;
            $scope.isLastPage = false;
            $scope.hasRecords = false;
            $scope.totalPages = 0;
            $scope.blockSize = 10;

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

            this.init = function(element, callback) {
                self.$element = element;
                $scope.callback = callback;
            };
            this.stateChanged = function(gridState) {
                if (gridState != null) {
                    $scope.gridState = gridState;
                    this.setup();
                }
            };

            this.resetPagingIfNeeded = function() {
                if ($scope.gridState != null && $scope.gridState.resetPaging)
                    $scope.gridState.pageNumber = 1;
            };
            this.setup = function() {

                $scope.hasRecords = $scope.gridState.totalRecords != 0;

                $scope.totalPages = Math.ceil($scope.gridState.totalRecords / $scope.gridState.pageSize);
                var blocks = [];
                var blockNumber = Math.ceil($scope.gridState.pageNumber / $scope.blockSize) - 1;
                var blockStart = blockNumber * $scope.blockSize;
                var min = blockStart + 1;
                var max = blockStart + $scope.blockSize;

                for (var i = 1; i < $scope.blockSize + 1; i++) {
                    if (blockStart + i < $scope.totalPages + 1)
                        blocks.push({ page: blockStart + i, isActive: blockStart + i == $scope.gridState.pageNumber });
                }
                $scope.pageBlock = blocks;

                var pageNumber = $scope.gridState.pageNumber;
                $scope.prevBlockPage = min - 1;
                $scope.nextBlockPage = max + 1;

                $scope.isLastBlock = $scope.pageBlock.length > 0 && max >= $scope.totalPages;
                $scope.isFirstBlock = min == 1;
                $scope.isFirstPage = pageNumber == 1;
                $scope.isLastPage = $scope.totalPages == pageNumber;
                var startRow = ((pageNumber - 1) * $scope.gridState.pageSize) + 1;
                var endRow = parseInt(startRow) + parseInt($scope.gridState.pageSize) -1
                if ($scope.isLastPage)
                    endRow = $scope.gridState.totalRecords;

                $scope.recordsVerbiage = 'Showing ' + startRow + ' to ' + endRow + ' of ' + $scope.gridState.totalRecords;


            };

            $scope.page = function(number) {
                $scope.gridState.pageNumber = number;
                $scope.callback(scope, {});
            };
            $scope.firstPage = function() {
                return $scope.page(1);
            };
            $scope.prevBlock = function() {
                return $scope.page($scope.prevBlockPage);
            };
            $scope.prevPage = function() {
                return $scope.page($scope.gridState.pageNumber - 1);
            };
            $scope.nextPage = function() {
                return $scope.page($scope.gridState.pageNumber + 1);
            };
            $scope.nextBlock = function() {
                return $scope.page($scope.nextBlockPage);
            };
            $scope.lastPage = function() {
                return $scope.page($scope.totalPages);
            };

        }])
        .directive('vPager', function() {
            return {
                restrict: 'EA',
                replace: true,
                controller: 'voodooPagerController',
                templateUrl: '/app/voodoo/voodooPager.tmpl.html',
                scope: {
                    gridState: '=gridState',
                    onchange: '&',
                },

                link: function(scope, element, attrs, voodooPagerController) {
                    scope.$watch('gridState', function() {
                        if (scope.gridState != null) {
                            voodooPagerController.stateChanged(scope.gridState);
                            voodooPagerController.init(element, scope.onchange);
                        }
                    });

                }
            };
        });
}())
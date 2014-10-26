(function () {
    "use strict";
    angular.module('app')
        .controller("apps", ['$scope', '$parse', 'appsFactory',
            function ($scope, $parse, appsFactory) {
                $scope.mode = 'grid';

                $scope.gridUrl = '/app/apps/grid.html';
                $scope.editUrl = '/app/apps/edit.html';
                $scope.addUrl = '/app/apps/add.html';
                $scope.fieldsUrl = 'app/apps/fields.html';

                $scope.load = function (response) {
                    if (response.isOk) {
                        $scope.state = response.state;
                        $scope.data = response.data;
                    } else {
                        $scope.error = response.message;
                    }
                };

                appsFactory.get({}).then($scope.load);

                $scope.clearMessages = function () {
                    $scope.error = '';
                    $scope.info = '';
                };

                $scope.refresh = function () {
                    $scope.clearMessages();
                    $scope.mode = 'grid';
                    $scope.detail = null;
                    appsFactory.get($scope.state).then($scope.load);
                };

                $scope.edit = function (key) {
                    $scope.clearMessages();
                    appsFactory.get({ key: key }).then($scope.loadEditItem);
                };
                $scope.add = function () {
                    $scope.clearMessages();
                    $scope.detail = {};
                    $scope.mode = 'add';
                };

                $scope.loadEditItem = function (response) {
                    $scope.clearMessages();
                    if (response.isOk) {
                        $scope.detail = response.data[0];
                        $scope.detail.errors = {};
                        $scope.mode = 'edit';
                    } else {
                        $scope.error = response.message;
                    }
                };

                $scope.editSave = function () {                    
                    appsFactory.post($scope.detail).then($scope.editDone);
                };
                $scope.addSave = function () {
                    appsFactory.put($scope.detail).then($scope.addDone);
                };
                $scope.delete = function () {
                    appsFactory.delete($scope.detail).then($scope.deleteDone);
                };
                $scope.addDone = function (response) {
                    $scope.done(response, $scope.addForm,'App added');
                };
                $scope.editDone = function (response) {
                    $scope.done(response, $scope.editForm, 'App updated');
                };
                $scope.deleteDone = function (response) {
                    $scope.done(response, $scope.editForm,'App deleted');
                };
                $scope.done = function (response, form, message) {
                    $scope.clearMessages();
                    if (!response.isOk) {
                        $scope.error = response.message;
                        $scope.failure(response.details, form);                    
                } else {
                        $scope.refresh();
                        $scope.mode = 'grid';
                        $scope.info = message;
                    }
                };
                
                $scope.failure = function (details, form) {
                    $scope.detail.errors = {};
                    angular.forEach(details, function (e) {
                        var field = e.key.charAt(0).toLowerCase() + e.key.slice(1);
                        form[field].$setValidity("server", false);
                        $scope.detail.errors[field] = e.value;
                    });
                };
            }]);
}())




var myApp = angular.module('myApp', []);
    myApp.controller('myController', ['$scope', '$http', function ($scope, $http) {
        $scope.timezones = [
            { id: 'UTC', name: 'Coordinated Universal Time (UTC)' },
            { id: 'PST', name: 'Pacific Standard Time (PST)' },
            { id: 'MST', name: 'Mountain Standard Time (MST)' },
            { id: 'CST', name: 'Central Standard Time (CST)' },
            { id: 'EST', name: 'Eastern Standard Time (EST)' }
        ];
        $scope.selectedTimezone = $scope.timezones[0]; 
        $scope.saveDateTime = function () {
            var model = {
                value: new Date()
            };
            
            model.value.setUTCMinutes(model.value.getUTCMinutes() - model.value.getTimezoneOffset());
            
            $http.post('http://localhost:5008/datetime', model).then(function (response) {
                console.log('Datetime value saved successfully:', response.data);
            }, function (error) {
                console.error('Error saving datetime value:', error);
            });
        };
    }]);

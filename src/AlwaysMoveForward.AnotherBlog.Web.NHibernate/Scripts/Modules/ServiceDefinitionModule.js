var theApp = angular.module('theApp', ['ngResource']);

theApp.filter('encodeURIComponent', function () {
    return window.encodeURIComponent;
});


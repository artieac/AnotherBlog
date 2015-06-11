angular.module('factoryModule', []).

// Teach the injector how to build a 'greeter'
// Notice that greeter itself is dependent on '$window'
  factory('mostViewedPostService', function () {
      // This is a factory function, and is responsible for 
      // creating the 'greet' service.
      return {
         return MostViewedPosts
      };
  });
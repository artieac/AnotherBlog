theApp.controller('ListController', function ($scope, $resource, $http) {
    $scope.getAllBlogs = function () {
        var allBlogsRequest = $resource('/api/Lists/Blogs/All');
        $scope.allBlogs = allBlogsRequest.get();
    }

    $scope.getMostViewedPosts = function (blogSubFolder) {
        if (blogSubFolder != null && blogSubFolder != '') {
            var mostViewedPostsRequestForBlog = $resource('/api/Blog/:blogSubFolder/Lists/MostViewed');
            $scope.mostViewedPosts = mostViewedPostsRequestForBlog.get({ blogSubFolder: blogSubFolder });
        }
        else {
            var mostViewedPostsRequest = $resource('/api/Lists/BlogPosts/MostViewed');
            $scope.mostViewedPosts = mostViewedPostsRequest.get();
        }
    }

    $scope.getBlogLists = function (blogSubFolder) {
        var getBlogListsRequest = $resource('/api/Blog/:blogSubFolder/Lists');
        $scope.blogLists = getBlogListsRequest.query({ blogSubFolder: blogSubFolder });
    }
});
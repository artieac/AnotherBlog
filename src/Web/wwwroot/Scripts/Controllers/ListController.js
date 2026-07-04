theApp.controller('ListController', function ($scope, $resource, $http) {
    $scope.getAllBlogs = function () {
        var allBlogsRequest = $resource('/api/Lists/Blogs/All');
        $scope.allBlogs = allBlogsRequest.get();
    }

    $scope.getMostViewedPosts = function (blogSubFolder) {
        if (blogSubFolder != null && blogSubFolder != '') {
            var mostViewedPostsRequestForBlog = $resource('/api/Lists/Blog/:blogSubFolder/MostViewed');
            $scope.mostViewedPosts = mostViewedPostsRequestForBlog.get({ blogSubFolder: blogSubFolder });
        }
        else {
            var mostViewedPostsRequest = $resource('/api/Lists/BlogPosts/MostViewed');
            $scope.mostViewedPosts = mostViewedPostsRequest.get();
        }
    }

    $scope.getRecentlyMostViewedPosts = function (blogSubFolder) {
        if (blogSubFolder != null && blogSubFolder != '') {
            var recentMostViewedPostsRequestForBlog = $resource('/api/Lists/Blog/:blogSubFolder/RecentMostViewed');
            $scope.mostViewedPosts = recentMostViewedPostsRequestForBlog.get({ blogSubFolder: blogSubFolder });
        }
        else {
            var recentMostViewedPostsRequest = $resource('/api/Lists/BlogPosts/RecentMostViewed');
            $scope.mostViewedPosts = recentMostViewedPostsRequest.get();
        }
    }

    $scope.getBlogLists = function (blogSubFolder) {
        var getBlogListsRequest = $resource('/api/Lists/Blog/:blogSubFolder');
        $scope.blogLists = getBlogListsRequest.query({ blogSubFolder: blogSubFolder });
    }
});
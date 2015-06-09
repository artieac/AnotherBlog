function ListController($scope, $resource) {
    $scope.getAllPolls = function () {
        var allBlogsRequest = $resource('/API/blogAPI/getall');
        $scope.allBlogs = allBlogsRequest.get();
    }

    $scope.getMostViewedPosts = function (blogSubFolder) {
        if (blogSubFolder != null && blogSubFolder != '') {
            var mostViewedPostsRequestForBlog = $resource('/:blogSubFolder/API/displaylistapi/getmostviewed');
            $scope.mostViewedPosts = mostViewedPostsRequestForBlog.get({ blogSubFolder: blogSubFolder });
        }
        else {
            var mostViewedPostsRequest = $resource('/API/displaylistapi/getmostviewed');
            $scope.mostViewedPosts = mostViewedPostsRequest.get();
        }
    }

    $scope.getBlogLists = function (blogSubFolder) {
        var getBlogListsRequest = $resource('/:blogSubFolder/API/displaylistapi/getbloglists');
        $scope.blogLists = getBlogListsRequest.query({ blogSubFolder: blogSubFolder });
    }
}
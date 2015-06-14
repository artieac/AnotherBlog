function ManageBlogController($scope, $resource, $http) {
    $scope.getPosts = function () {
        var blogSubFolder = jQuery("#targetBlog").val();
        var getPostsRequest = $resource('/admin/manageblog/getposts?blogSubFolder=' + blogSubFolder);
        $scope.blogPosts = getPostsRequest.query();
    }
}

function ManageBlogController($scope, $resource, $http) {
    $scope.getComments = function () {
        var blogSubFolder = jQuery("#targetBlog").val();
        var status = jQuery("#commentFilter").val();
        var getCommentsRequest = $resource('/api/Blog/' + blogSubFolder + '/Comments/' + status);
        $scope.blogComments = getCommentsRequest.query();
    }

    $scope.getPosts = function () {
        var blogSubFolder = jQuery("#targetBlog").val();
        var getPostsRequest = $resource('/admin/manageblog/getposts?blogSubFolder=' + blogSubFolder);
        $scope.blogPosts = getPostsRequest.query();
    }
}

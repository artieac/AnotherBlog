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

    $scope.approveComment = function (blogSubFolder, blogPostId, commentId) {
        $http.put('/api/Blog/' + blogSubFolder + "/BlogPost/" + blogPostId + "/Comment/" + commentId + "/Approved")
            .success(function (data) {
                $scope.getComments();
            });
    }

    $scope.deleteComment = function (blogSubFolder, blogPostId, commentId) {
        $http.delete('/api/Blog/' + blogSubFolder + "/BlogPost/" + blogPostId + "/Comment/" + commentId)
            .success(function (data) {
                $scope.getComments();
            });
    }
}

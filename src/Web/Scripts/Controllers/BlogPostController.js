function BlogPostController($scope, $resource, $http) {
    $scope.getBlogPost = function (blogSubFolder, blogPostId) {
        var getBlogPostRequest = $resource('/api/Blog/:blogSubFolder/BlogPost/:blogPostId');
        $scope.blogPost = getBlogPost.get({ blogSubFolder: blogSubFolder, blogPostId: blogPostId });
    }

    $scope.saveBlogPost = function (blogSubFolder) {
        var blogPostId = jQuery("#blogPostId").val();

        $http.put('/api/Blog/' + blogSubFolder + '/BlogPost/' + blogPostId, $scope.blogPostForm)
            .success(function (data) {
                jQuery("#blogPostId").val(data.Id);
            });
    }
}
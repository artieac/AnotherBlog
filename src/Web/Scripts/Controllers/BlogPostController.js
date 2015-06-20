theApp.controller('BlogPostController', function ($scope, $resource, $http, $interval) {

    $interval(
        function () {
            $scope.saveBlogPost();
        }
        , 600000
    );

    $scope.getBlogPost = function (blogSubFolder, blogPostId) {
        var getBlogPostRequest = $resource('/api/Blog/:blogSubFolder/BlogPost/:blogPostId');
        $scope.blogPost = getBlogPost.get({ blogSubFolder: blogSubFolder, blogPostId: blogPostId });
    }

    $scope.saveBlogPost = function () {
        var saveBlogPostForm = jQuery("#saveBlogPostForm");

        if (saveBlogPostForm != null) {
            var blogPostId = jQuery("#blogPostId").val();
            var blogSubFolder = jQuery("#blogSubFolder").val();

            $scope.blogPostInput = {};
            $scope.blogPostInput.IsPublished = jQuery("#isPublished").attr('checked');
            $scope.blogPostInput.Title = jQuery("#title").val();
            $scope.blogPostInput.Text = jQuery("#inputText").val();
            $scope.blogPostInput.Tags = jQuery("#tagInput").val();

            $http.put('/api/Blog/' + blogSubFolder + '/BlogPost/' + blogPostId, $scope.blogPostInput)
                .success(function (data) {
                    jQuery("#blogPostId").val(data.Id);
                });
        }
    }
});
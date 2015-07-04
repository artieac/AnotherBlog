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
            var blogPostId = parseInt(jQuery("#blogPostId").val());
            var blogSubFolder = jQuery("#blogSubFolder").val();

            $scope.blogPostInput = {};
            $scope.blogPostInput.IsPublished = jQuery("#isPublished").is(':checked');
            $scope.blogPostInput.Title = jQuery("#title").val();
            $scope.blogPostInput.Text = jQuery("#inputText").val();
            $scope.blogPostInput.Tags = jQuery("#tagInput").val();

            if (blogPostId < 0) {
                $scope.addBlogPost(blogSubFolder, $scope.blogPostInput);
            }
            else {
                $scope.updateBlogPost(blogSubFolder, blogPostId, $scope.blogPostInput);
            }
        }
    }

    $scope.addBlogPost = function (blogSubFolder, blogPostInput) {
        var addBlogPostRequest = $resource('/api/Blog/:blogSubFolder/BlogPost',
            { blogSubFolder: blogSubFolder });

        addBlogPostRequest.save(blogPostInput, function (data) {
            jQuery("#blogPostId").val(data.Id);
        });
    }

    $scope.updateBlogPost = function (blogSubFolder, blogPostId, blogPostInput) {
        var updateBlogPostRequest = $resource('/api/Blog/:blogSubFolder/BlogPost/:blogPostId',
            { blogSubFolder: blogSubFolder, blogPostId: blogPostId }, {
                update: { method: 'PUT' }
            }
        );

        updateBlogPostRequest.update(blogPostInput, function (data) {
            jQuery("#blogPostId").val(data.Id);
        });            
    }
});
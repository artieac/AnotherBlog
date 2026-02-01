theApp.controller('BlogController', function ($scope, $resource, $http) {
    $scope.getAllBlogs = function () {
        var allBlogsRequest = $resource('/api/Blogs');
        $scope.allBlogs = allBlogsRequest.query();
    }

    $scope.getBlogTags = function (blogSubFolder) {
        var getTagsRequest = $resource('/api/Blog/:blogSubFolder/Tags');
        $scope.blogTags = getTagsRequest.query({ blogSubFolder: blogSubFolder });
    }

    $scope.getArchiveDates = function (blogSubFolder) {
        var getArchiveDatesRequest = $resource('/api/Lists/Blog/:blogSubFolder/ArchiveDates');
        $scope.archiveDates = getArchiveDatesRequest.get({ blogSubFolder: blogSubFolder });
    }

    $scope.getComments = function (blogSubFolder, blogPostId) {
        var getCommentsRequest = $resource('/api/Blog/:blogSubFolder/BlogPost/:blogPostId/Comments');
        $scope.blogPostComments = getCommentsRequest.query({ blogSubFolder: blogSubFolder, blogPostId: blogPostId });
    }

    $scope.submitComment = function (blogSubFolder, blogPostId) {
        $http.post('/api/Blog/' + blogSubFolder + '/BlogPost/' + blogPostId + '/Comment', $scope.newComment)
            .success(function (data) {
                $scope.blogPostComments = data;
            });
    }

    $scope.saveBlog = function () {
        var saveBlogForm = jQuery("#saveBlogForm");

        if (saveBlogForm != null) {
            var blogId = jQuery("#blogId").val();

            $scope.blogInput = {};
            $scope.blogInput.Name = jQuery("#blogName").val();
            $scope.blogInput.Theme = jQuery("#blogTheme").val();
            $scope.blogInput.Welcome = jQuery("#blogWelcome").val();
            $scope.blogInput.About = jQuery("#blogAbout").val();
            $scope.blogInput.SubFolder = jQuery("#blogSubFolder").val();
            $scope.blogInput.Description = jQuery("#blogDescription").val();

            $http.put('/api/Blog/' + blogId, $scope.blogInput)
                .success(function (data) {
                    jQuery("#blogId").val(data.Id);
                });
        }
    }
});
function BlogController($scope, $resource, $http) {
    $scope.getBlogTags = function (blogSubFolder) {
        var getTagsRequest = $resource('/:blogSubFolder/API/blogapi/gettags');
        $scope.blogTags = getTagsRequest.query({ blogSubFolder: blogSubFolder });
    }

    $scope.getArchiveDates = function (blogSubFolder) {
        var getArchiveDatesRequest = $resource('/:blogSubFolder/API/blogAPI/getarchivedates');
        $scope.archiveDates = getArchiveDatesRequest.get({ blogSubFolder: blogSubFolder });
    }

    $scope.getComments = function (blogSubFolder, blogPostId) {
        var getCommentsRequest = $resource('/:blogSubFolder/API/blogAPI/getcomments/:blogPostId');
        $scope.blogPostComments = getCommentsRequest.query({ blogSubFolder: blogSubFolder, blogPostId: blogPostId });
    }

    $scope.submitComment = function (blogSubFolder, entryId) {
        $scope.newComment.entryId = entryId;
        $http.put('/' + blogSubFolder + '/blog/savecomment', $scope.newComment)
            .success(function (data) {
                $scope.blogPostComments = data;
            });
        }
}
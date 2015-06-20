theApp.controller('BlogController', function ($scope, $resource, $http) {

    $scope.getBlogTags = function (blogSubFolder) {
        var getTagsRequest = $resource('/api/Blog/:blogSubFolder/Tags');
        $scope.blogTags = getTagsRequest.query({ blogSubFolder: blogSubFolder });
    }

    $scope.getArchiveDates = function (blogSubFolder) {
        var getArchiveDatesRequest = $resource('/api/Blog/:blogSubFolder/Lists/ArchiveDates');
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
});
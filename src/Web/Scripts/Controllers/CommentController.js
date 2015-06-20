theApp.controller('CommentController', function ($scope, $resource, $http) {
    $scope.areAllSelected = false;

    $scope.getComments = function () {
        var blogSubFolder = jQuery("#targetBlog").val();
        var status = jQuery("#commentFilter").val();
        var getCommentsRequest = $resource('/api/Blog/' + blogSubFolder + '/Comments/' + status);
        $scope.selectedComments = {};
        $scope.areAllSelected = false;
        $scope.blogComments = getCommentsRequest.query();
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

    $scope.onSelectAll = function () {
        $scope.areAllSelected = !$scope.areAllSelected;

        if ($scope.areAllSelected === true) {
            if (typeof $scope.blogComments !== 'undefined' && $scope.blogComments !== null) {
                for (var i = 0; i < $scope.blogComments.length; i++) {
                    $scope.onSelectComment($scope.blogComments[i].Id, $scope.blogComments[i].BlogPostId);
                }
            }
        }
        else {
            $scope.selectedComments = {};
        }
    }

    $scope.isCommentSelected = function (commentId, blogPostId) {
        var retVal = false;

        if ($scope.selectedComments !== 'undefined' && $scope.selectedComments !== null) {
            if ($scope.selectedComments[commentId] !== 'undefined' && $scope.selectedComments[commentId] !== null) {
                if ($scope.selectedComments[commentId] === blogPostId) {
                    retVal = true;
                }
            }
        }

        return retVal;
    }

    $scope.onSelectComment = function (commentId, blogPostId) {
        if (typeof $scope.selectedComments === 'undefined' || $scope.selectedComments === null) {
            $scope.selectedComments = {};
        }

        $scope.selectedComments[commentId] = blogPostId;
    }

    $scope.approveComments = function (blogSubFolder) {
        $http.put('/api/Blog/' + blogSubFolder + "/Comments/Approved", $scope.selectedComments)
            .success(function (data) {
                $scope.getComments();
            });
    }

    $scope.deleteComments = function (blogSubFolder) {
        $http.put('/api/Blog/' + blogSubFolder + "/Comments/Deleted", $scope.selectedComments)
            .success(function (data) {
                $scope.getComments();
            });
    }
});
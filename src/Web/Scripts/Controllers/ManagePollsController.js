function ManagePollsController($scope, $resource, $http) {
    $scope.pollElements = { selectedPoll: 0 };
    $scope.pollOptionElements = { selectedPollOption: 0 };

    $scope.getAllPolls = function () {
        var getPollQuestionsRequest = $resource('/admin/managepolls/getall');
        $scope.pollItems = getPollQuestionsRequest.query();
    }

    $scope.getPollQuestion = function (pollQuestionId) {
        jQuery.each($scope.pollItems, function (i, val) {
            if (val.Id == pollQuestionId) {
                $scope.selectedPoll = val;
            }
        });
    }

    $scope.deletePoll = function (pollQuestionId) {
        $http.put('/Admin/managepolls/deletepoll?pollQuestionId=' + pollQuestionId)
           .success(function (data) {
           });
    }

    $scope.addPollQuestion = function () {
        $http.put('/Admin/ManagePolls/AddPoll', $scope.newPoll)
           .success(function (data) {
               $scope.pollItems = data;
           });
    }

    $scope.putOption = function (pollQuestionId) {
        $scope.newPollOption.pollQuestionId = pollQuestionId;
        $http.put('/Admin/ManagePolls/putOption', $scope.newPollOption)
           .success(function (data) {
               $scope.selectedPoll = data;
           });
    }

    $scope.deletePollOption = function (pollQuestionId, optionId) {
        $http.delete('/Admin/ManagePolls/deleteOption/' + pollQuestionId + "?optionId=" + optionId)
           .success(function (data) {
               $scope.selectedPoll = data;
           });
    }
}
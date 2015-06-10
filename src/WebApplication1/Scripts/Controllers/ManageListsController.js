function ManageListsController($scope, $resource, $http) {
    $scope.blogListElements = {selectedList: 0};
    $scope.blogListItemElements = { selectedListItem: 0 };

    $scope.getAll = function (blogSubFolder) {
        var getBlogListsRequest = $resource('/admin/managelists/getall?blogSubFolder=' + blogSubFolder);
        $scope.blogLists = getBlogListsRequest.query();
    }

    $scope.getListItems = function (listId) {
        jQuery.each($scope.blogLists, function (i, val) {
            if (val.Id == listId)
            {
                $scope.currentList = val;
            }
        });
    }

    $scope.deleteList = function (listId, blogSubFolder) {
         $http.put('/Admin/ManageLists/Delete?listId=' + listId + '&blogSubFolder=' + blogSubFolder, $scope.newComment)
            .success(function (data) {
            });
     }

    $scope.addList = function (blogSubFolder) {
         if ($scope.newList.showOrdered == null) {
             $scope.newList.showOrdered = false;
         }

         $http.put('/Admin/ManageLists/Add/' + blogSubFolder, $scope.newList)
            .success(function (data) {
                $scope.blogLists = data;
            });
    }

    $scope.putItem = function (blogSubFolder, listId) {
        $scope.newListItem.listItemId = 0;
        $scope.newListItem.listId = listId;

        $http.put('/Admin/ManageLists/putItem/' + blogSubFolder, $scope.newListItem)
           .success(function (data) {
               $scope.currentList = data;
           });
    }

    $scope.deleteListItem = function (blogSubFolder, listId, listItemId) {
        $http.put('/Admin/ManageLists/DeleteListItem/' + blogSubFolder + '?listId=' + listId + '&listItemId=' + listItemId)
           .success(function (data) {
               $scope.currentList = data;
           });
    }
}
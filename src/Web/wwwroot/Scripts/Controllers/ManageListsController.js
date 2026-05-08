theApp.controller('ManageListsController', function ($scope, $resource, $http) {
    $scope.blogListElements = { selectedList: 0 };
    $scope.blogListItemElements = { selectedListItem: 0 };

    $scope.getAll = function (blogSubFolder) {
        var getBlogListsRequest = $resource('/api/Blog/:blogSubFolder/Lists', { blogSubFolder: blogSubFolder });
        $scope.blogLists = getBlogListsRequest.query();
    }

    $scope.getListItems = function (listId) {
        jQuery.each($scope.blogLists, function (i, val) {
            if (val.Id == listId) {
                $scope.currentList = val;
            }
        });
    }

    $scope.getListItem = function (list, listItemId) {
        jQuery.each(list.Items, function (i, val) {
            if (val.Id == listItemId) {
                $scope.currentListItem = val;
            }
        });

        return $scope.currentListItem;
    }

    $scope.deleteList = function (listId, blogSubFolder) {
        var deleteListRequest = $resource('/api/Blog/:blogSubFolder/List/:listId', { blogSubFolder: blogSubFolder, listId: listId });
        deleteListRequest.delete(function (data) {
            $scope.blogLists.splice($.inArray(data, $scope.blogLists), 1);
        });
    }

    $scope.addList = function (blogSubFolder) {
        if ($scope.newList.showOrdered == null) {
            $scope.newList.showOrdered = false;
        }

        var addListRequest = $resource('/api/Blog/:blogSubFolder/List',{blogSubFolder: blogSubFolder});
        addListRequest.save($scope.newList, function (data) {
               $scope.blogLists[$scope.blogLists.length] = data;
           });
    }

    $scope.updateList = function (blogSubFolder, listId) {

        $scope.updateList = {};
        $scope.updateList.Name = $scope.currentList.Name;
        $scope.updateList.ShowOrdered = $scope.currentList.ShowOrdered;

        var addListRequest = $resource('/api/Blog/:blogSubFolder/List/:listId',
            { blogSubFolder: blogSubFolder, listId: listId }, {
                update: {method:'PUT'}
            }
        );
        
        addListRequest.update($scope.updateList, function (data) {
            $scope.currentList = data;
        });
    }

    $scope.addListItem = function (blogSubFolder, listId) {        
        var addListItemRequest = $resource('/api/Blog/:blogSubFolder/List/:listId/Item', { blogSubFolder: blogSubFolder, listId: listId});
        addListItemRequest.save($scope.newListItem, function (data) {
            $scope.currentList = data;
        });
    }

    $scope.updateListItem = function (blogSubFolder, listId, itemId) {

        var listItem = $scope.getListItem($scope.currentList, itemId);

        $scope.updateItem = {};
        $scope.updateItem.Name = listItem.Name;
        $scope.updateItem.RelatedLink = listItem.RelatedLink;
        $scope.updateItem.DisplayOrder = listItem.DisplayOrder;

        var addListItemRequest = $resource('/api/Blog/:blogSubFolder/List/:listId/Item/:itemId',
            { blogSubFolder: blogSubFolder, listId: listId, itemId: itemId }, {
                update: { method: 'PUT' }
            }
        );

        addListItemRequest.update($scope.updateItem, function (data) {
            $scope.currentList = data;
        });
    }

    $scope.deleteListItem = function (blogSubFolder, listId, listItemId) {
        var deleteListItemRequest = $resource('/api/Blog/:blogSubFolder/List/:listId/Item/:itemId', { blogSubFolder: blogSubFolder, listId: listId, itemId: listItemId });
        deleteListItemRequest.delete(function (data) {
            $scope.currentList = data;
        });
    }
});
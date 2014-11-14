function urlFor(actionName, controllerName)
{
    var actionNameValid = /^[A-Za-z0-9_]+$/.test(actionName);
    var controllerNameValid = /^[A-Za-z0-9_]+$/.test(controllerName);
    if (!actionNameValid || !controllerNameValid)
        throw new Error("Invalid action or controller name.");

    return "/" + controllerName + "/" + actionName;
}

var wishlistApp = angular.module("wishlistApp", []);

wishlistApp.controller('listUsersController', function ($scope, $http)
{
    $scope.users = [];

    $http.post(urlFor("GetUserInfos", "Home"), {})
        .success(function (data, status, headers, config)
        {
            $scope.users = data.Users;
        });

    $scope.viewUser = function (userId)
    {
        window.location.href = urlFor("ViewUser", "Wishlist") + "?ID=" + userId;
    }
});

wishlistApp.controller("wishlistsController", function ($scope, $http)
{
    $scope.Wishlists = [];

    $http.post(urlFor("GetWishlists", "Wishlist"), {})
        .success(function (data, status, headers, config)
        {
            $scope.Wishlists = data.Wishlists;

            $scope.addWishlist = function ()
            {
                $http.post(urlFor("NewWishlist", "Wishlist"), {})
                    .success(function (data, status, headers, config)
                    {
                        $scope.Wishlists.push(data);
                    });
            };

            $scope.changeWishlist = function (wl)
            {
                $http.post(urlFor("ChangeWishlist", "Wishlist"), wl.Info)
                    .success(function (data, status, headers, config)
                    {
                        if (data.Success)
                            ;
                        else
                            console.warn(data.Exception);
                    });
            }

            $scope.toggleWishlistVisibility = function (wl)
            {
                wl.Info.IsPublic = !wl.Info.IsPublic;
                $http.post(urlFor("ChangeWishlist", "Wishlist"), wl.Info)
                    .success(function (data, status, headers, config)
                    {
                        if (data.Success)
                            ;
                        else
                            console.warn(data.Exception);
                    });
            }

            $scope.removeWishlist = function (wl)
            {
                $http.post(urlFor("RemoveWishlist", "Wishlist"), { ID: wl.Info.ID })
                    .success(function (data, status, headers, config)
                    {
                        if (data.Success)
                        {
                            var index = $scope.Wishlists.indexOf(wl);
                            if (index >= 0)
                                $scope.Wishlists.splice(index, 1);
                        }
                        else
                            console.warn(data.Exception);
                    });
            }

            $scope.addWishlistItem = function (wl)
            {
                $http.post(urlFor("NewWishlistItem", "Wishlist"), { ID: wl.Info.ID })
                    .success(function (data, status, headers, config)
                    {
                        if (data.Success)
                            wl.WishlistItems.push(data.Result);
                        else
                            console.warn(data.Exception);
                    });
            };

            $scope.changeWishlistItem = function (wli)
            {
                $http.post(urlFor("ChangeWishlistItem", "Wishlist"), wli)
                    .success(function (data, status, headers, config)
                    {
                        if (data.Success)
                            ;
                        else
                            console.warn(data.Exception);
                    });
            };

            $scope.removeWishlistItem = function (wli, wl)
            {
                $http.post(urlFor("RemoveWishlistItem", "Wishlist"), { ID: wli.ID })
                    .success(function (data, status, headers, config)
                    {
                        if (data.Success)
                        {
                            var index = wl.WishlistItems.indexOf(wli);
                            if (index >= 0)
                                wl.WishlistItems.splice(index, 1);
                        }
                        else
                            console.warn(data.Exception);
                    });
            };
        });
});



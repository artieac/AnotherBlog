var ManageUsers = new function () {
    this.EditUserInitializeUserBlogs = function () {
        var userBlogOptions = { target: '#userBlogsContainer' };
        jQuery('#viewUserBlogs').ajaxSubmit(userBlogOptions);
    };

    this.EditUserSetupUserBlogAjax = function () {
        var commentOptions = { target: '#userBlogsContainer' };
        jQuery('#userAddBlogForm').ajaxForm(commentOptions);
    };
}
var AdminCommon = new function () {
    this.HandleBlogSelectionChange = function () {
        var changeBlogForm = jQuery("#changeBlogForm");

        if (changeBlogForm != null) {
            var blogSubFolder = jQuery("#targetBlog");
            changeBlogForm.attr('action', changeBlogForm.attr('action') + blogSubFolder.val());
            changeBlogForm.submit();
        }
    }
}
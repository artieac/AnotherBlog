var ManageBlogPosts = new function () {
    this.ToggleSaveState = function (isDirty) {
        var saveStateImage = $("#blogPostSaveStateImage");

        if (isDirty === true) {
            $("#blogPostSaveStateImage").attr('src', '/Content/Images/action_delete.png');
            $("#saveButton").removeAttr("disabled");
        }
        else {
            $("#blogPostSaveStateImage").attr('src', '/Content/Images/action_check.png');
            $("#saveButton").attr("disabled", "disabled");
        }
    };

    this.CollectBlogPostData = function(){
        var retVal = jQuery("#autoSaveUrl").val();
        retVal = retVal + "/" + jQuery("#blogPostId").val();
            
        var blogEntryAjaxForm = jQuery("#blogEntryAjaxForm");
        
        if (blogEntryAjaxForm != null) {
            blogEntryAjaxForm.attr('action', autoSaveUrl);
            jQuery("#ajaxIsPublished").val(jQuery("#isPublished").attr('checked'));
            jQuery("#ajaxTitle").val(jQuery("#title").val());
            jQuery("#ajaxEntryText").val(jQuery("#inputText").val());
            jQuery("#ajaxTagInput").val(jQuery("#tagInput").val());
        }

        return retVal;
    };


    this.ExecuteBlogEntryAutoSave = function () {
        var autoSaveUrl = ManageBlogPosts.CollectBlogPostData();

        var blogEntryAjaxForm = jQuery("#blogEntryAjaxForm");
        blogEntryAjaxForm.attr('action', autoSaveUrl);

        var ajaxOptions = { success: ManageBlogPosts.ProcessAutoSaveReturn, dataType: 'json' };
        blogEntryAjaxForm.ajaxSubmit(ajaxOptions);
    };

    this.ProcessAutoSaveReturn = function (data) {
        jQuery("#blogPostId").val(data.Id);
        ManageBlogPosts.SetupAutoSaveTimer();
        ManageBlogPosts.ToggleSaveState(false);

    };

    this.SetupAutoSaveTimer = function(){
        setTimeout("ManageBlogPosts.ExecuteBlogEntryAutoSave()", 30000);
    };

    this.SubmitFileUpload = function () {
        var uploadOptions = { target: '#fileUploadSection' };
        jQuery('#fileUploadForm').ajaxSubmit(uploadOptions);
    };
}
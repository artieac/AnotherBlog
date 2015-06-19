var ManageBlogPosts = new function () {
    this.ExecuteBlogEntryAutoSave = function () {
        var blogEntryAjaxForm = jQuery("#blogEntryAjaxForm");
        
        if (blogEntryAjaxForm != null) {
            var autoSaveUrl = jQuery("#autoSaveUrl").val();
            autoSaveUrl = autoSaveUrl + "/" + jQuery("#entryId").val();
            blogEntryAjaxForm.attr('action', autoSaveUrl);

            jQuery("#ajaxIsPublished").val(jQuery("#isPublished").attr('checked'));
            jQuery("#ajaxTitle").val(jQuery("#title").val());
            jQuery("#ajaxEntryText").val(jQuery("#entryText").val());
            jQuery("#ajaxTagInput").val(jQuery("#tagInput").val());

            var ajaxOptions = { success: ManageBlogPosts.ProcessAutoSaveReturn, dataType: 'json' };
            blogEntryAjaxForm.ajaxSubmit(ajaxOptions);
        }
    };

    this.ProcessAutoSaveReturn = function (data) {
        jQuery("#entryId").val(data.Id);
        ManageBlogPosts.SetupAutoSaveTimer();
    };

    this.SetupAutoSaveTimer = function(){
        setTimeout("ManageBlogPosts.ExecuteBlogEntryAutoSave()", 30000);
    };

    this.SubmitFileUpload = function () {
        var uploadOptions = { target: '#fileUploadSection' };
        jQuery('#fileUploadForm').ajaxSubmit(uploadOptions);
    };
}
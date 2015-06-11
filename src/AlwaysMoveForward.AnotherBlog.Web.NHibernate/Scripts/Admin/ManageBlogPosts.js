var ManageBlogPosts = new function () {
    this.ExecuteBlogEntryAutoSave = function () {
        var blogEntryAjaxForm = jQuery("#blogEntryAjaxForm");
        
        if (blogEntryAjaxForm != null) {
            jQuery("#ajaxIsPublished").val(jq("#isPublished").attr('checked'));
            jQuery("#ajaxEntryId").val(jq("#entryId").val());
            jQuery("#ajaxTitle").val(jq("#title").val());
            jQuery("#ajaxEntryText").val(jq("#entryText").val());
            jQuery("#ajaxTagInput").val(jq("#tagInput").val());

            var ajaxOptions = { success: ProcessAutoSaveReturn, dataType: 'json' };
            blogEntryAjaxForm.ajaxSubmit(ajaxOptions);
        }
    };

    this.ProcessAutoSaveReturn = function (data) {
        jQuery("#entryId").val(data.EntryId);
        setTimeout("ExecuteBlogEntryAutoSave()", 300000);
    };

    this.SubmitFileUpload = function () {
        var uploadOptions = { target: '#fileUploadSection' };
        jQuery('#fileUploadForm').ajaxSubmit(uploadOptions);
    };
}
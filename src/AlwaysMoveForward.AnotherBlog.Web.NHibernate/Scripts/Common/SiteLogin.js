var SiteLogin = new function () {
    this.ShowForgotPassword = function () {
        jQuery('#loginFormsDiv').hide();
        jQuery('#forgotPasswordFormDiv').css("display", "block");
    };

    this.SubmitForgotPassword = function () {
        var loginOptions = { target: '#loginSection' };
        jQuery('#forgotPasswordForm').ajaxSubmit(loginOptions);
    };

    this.SubmitLogin = function () {
        var loginOptions = { success: SiteLogin.ProcessPostLogin };
        jQuery('#loginForm').submit(loginOptions);
    };

    this.SubmitLogout = function () {
        var logoutOptions = { success: SiteLogin.ProcessPostLogout };
        jQuery('#logoutForm').ajaxSubmit(logoutOptions);
    };

    this.ProcessPostLogin = function (responseText, statusText) {
        if(responseText.IsAuthorized==true)
        {
            jQuery("#loginErrorMessage").hide();
            location.reload(true);
        }
        else 
        {
            jQuery("#loginErrorMessage").show();
        }
    };

    this.ProcessPostLogout = function (responseText, statusText) {
        location.reload(true);
    };
}
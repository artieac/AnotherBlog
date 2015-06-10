var AMFOAuth = new function () {
    this.loginForm = '';
    this.isLoggedIn = false;

    this.Initialize = function (htmlLoginForm, loggedIn) {
        this.loginForm = htmlLoginForm;
        this.isLoggedIn = loggedIn;
    };

    this.displayLoginControl = function(){
        if(this.isLoggedIn == false)
        {
            $("<button type='button' class='btn btn-sm btn-info' id='submitLoginButton' onclick='AMFOAuth.SubmitLoginForm();'>log in</button>").append("#" + this.loginForm);
        }
        else
        {
            $("<input type='hidden' id='loginAction' name='loginAction' value='logout' />").append("#" + this.loginForm);
            $("<button type='button' class='btn btn-sm btn-info' id='submitLogoutButton' onclick='AMFOAuth.SubmitLoginForm();'>log out</button>").append("#" + this.loginForm);
        }
    }

    this.SubmitLoginForm = function () {
        var loginOptions = { success: SiteLogin.ProcessPostLogin };
        jQuery("#" + this.loginForm).submit(loginOptions);
    };

    this.ProcessPostLogin = function (responseText, statusText) {
        if (responseText.ProcessedLogin == true) {
            if (responseText.IsAuthorized == true) {
                jQuery("#loginErrorMessage").hide();
                location.reload(true);
            }
            else {
                jQuery("#loginErrorMessage").show();
            }
        }
        else {
            location.reload(true);
        }
    };
}
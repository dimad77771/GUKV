DXTheme = {};
DXTheme.CurrentThemeCookieKey = "DXCurrentTheme";
DXTheme.ShowThemeSelector = function () {
    ThemeSelectorPopup.Show();
}
DXTheme.GetThemeSelectorButton = function () {
    return document.getElementById("ThemeSelectorButton");
};
DXTheme.ThemeSelectorPopupPopUp = function () {
    DXTheme.GetThemeSelectorButton().className += " SelectedButton";
}
DXTheme.ThemeSelectorPopupCloseUp = function () {
    var b = DXTheme.GetThemeSelectorButton();
    b.className = b.className.replace("SelectedButton", "");
}
DXTheme.SetCurrentTheme = function (theme) {
    ThemeSelectorPopup.Hide();
    ASPxClientUtils.SetCookie(DXTheme.CurrentThemeCookieKey, theme);
    if (document.forms[0] && (!document.forms[0].onsubmit || document.forms[0].onsubmit.toString().indexOf("Sys.Mvc.AsyncForm") == -1)) {
        // for export purposes
        var eventTarget = document.getElementById("__EVENTTARGET");
        if (eventTarget)
            eventTarget.value = "";
        var eventArgument = document.getElementById("__EVENTARGUMENT");
        if (eventArgument)
            eventArgument.value = "";

        document.forms[0].submit();
    } else {
        window.location.reload();
    }
}

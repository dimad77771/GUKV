
function StringTrim(s) {

    var str = "" + s;

    while (str.length > 0 && str.charAt(0) == " ") {
        str = str.substring(1);
    }

    while (str.length > 0 && str.charAt(str.length - 1) == " ") {
        str = str.substring(0, str.length - 1);
    }

    return str;
}

function ShowReportErrorList(reportId) {

    var pageUrl = "../Reports1NF/ReportErrors.aspx?rid=" + reportId;

    window.open(pageUrl);
}

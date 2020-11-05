
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

function ShowObjectCard(balansId, buildingId) {

    var cardUrl = "";
    var balansIdStr = "" + balansId;

    if (balansIdStr == "") {
        cardUrl = "../Cards/ObjCard.aspx?bid=" + buildingId;
    }
    else {
        cardUrl = "../Cards/ObjCard.aspx?balid=" + balansId + "&bid=" + buildingId;
    }

    window.open(cardUrl);
}

function ShowObjectCardSimple(buildingId) {

    var cardUrl = "../Cards/ObjCard.aspx?bid=" + buildingId;

    window.open(cardUrl);
}

function ShowObjectCardForPrivatization(encodedIDs) {

    var str = "" + encodedIDs;
    var firstChar = "" + str.charAt(0);

    if (firstChar == ",") {
        // Balans ID is missing - remove the comma, and use Building ID only
        cardUrl = "../Cards/ObjCard.aspx?bid=" + str.substr(1, str.length - 1);
    }
    else {
        // Split the string into Balans ID and Building ID
        var parts = str.split(",");

        cardUrl = "../Cards/ObjCard.aspx?balid=" + parts[0] + "&bid=" + parts[1];
    }

    window.open(cardUrl);
}

function ShowOrganizationCard(orgId) {

    var cardUrl = "../Cards/OrgCard.aspx?orgid=" + orgId;

    window.open(cardUrl);
}

function ShowOrgInfo(orgId) {

    var cardUrl = "../Reports1NF/OrgInfo.aspx?rid=" + orgId;

    window.open(cardUrl);
}


function ShowDocumentCard(docId) {

    var cardUrl = "../Cards/DocCard.aspx?docid=" + docId;

    window.open(cardUrl);
}

function ShowAssessmentCard(orgId) {

    var cardUrl = "../Cards/AssessmentCard.aspx?vid=" + orgId;

    window.open(cardUrl);
}

function ShowBalansCard(balansId) {

    var cardUrl = "../Cards/BalansCardArchive.aspx?balid=" + balansId;

    window.open(cardUrl);
}

function ShowBalansCardEx(ex_reports1nf_balans, balansId, reportId) {

    if (ex_reports1nf_balans === 1) {

        var cardUrl = "../Reports1NF/OrgBalansObject.aspx?rid=" + reportId + "&bid=" + balansId;

    } else {

        var cardUrl = "../Cards/BalansCardArchive.aspx?balid=" + balansId;

    }

    window.open(cardUrl);
}

function ShowArendaCard(arendaId) {

    var cardUrl = "../Cards/ArendaCardArchive.aspx?arid=" + arendaId;

    window.open(cardUrl);
}

function ShowArendaCardEx(ex_reports1nf_arenda, arendaId, reportId) {

    if (ex_reports1nf_arenda === 1) {

        var cardUrl = "../Reports1NF/OrgRentAgreement.aspx?rid=" + reportId + "&aid=" + arendaId;

    } else {

        var cardUrl = "../Cards/ArendaCardArchive.aspx?arid=" + arendaId;
    }

	window.open(cardUrl);
}

function ShowSubleaseCard(reportId, arendaId) {

	var cardUrl = "../Reports1NF/OrgRentAgreement.aspx?rid=" + reportId + "&aid=" + arendaId;

	window.open(cardUrl);
}

function OpenDocText(docId) {

    var docUrl = "../Documents/DoTextDownload.aspx?docid=" + docId;

    var frm = document.getElementById('docTextFrame');

    frm.src = docUrl;
}

function ShowGeneralReport(gridToken) {

    var reportUrl = "../Cards/ReportViewer.aspx?grid=" + gridToken;

    window.open(reportUrl);
}


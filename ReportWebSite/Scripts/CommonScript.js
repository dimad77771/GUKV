
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

	window.location = cardUrl;
    //window.open(cardUrl);
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

function myEscapeHtml(s) {
    return s.replace(/[&<>"']/g, ch => ({
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#39;'
    }[ch]));
}

function myTextToHtml(txt) {
    const safe = myEscapeHtml(txt).replace(/\r\n|\r|\n/g, '<br/>');
    return `<span style="font-family:'Times New Roman', Times, serif; font-size:18.5px; line-height:1.8;">${safe}</span>`;
}

function myCopyToClipboard(txt) {
    //console.log("navigator.clipboard2", navigator.clipboard)
    //console.log("window.ClipboardItem", window.ClipboardItem)
    if (navigator.clipboard && window.ClipboardItem) {
        const html = myTextToHtml(txt);
        const data = {
            'text/html': new Blob([html], { type: 'text/html' }),
            'text/plain': new Blob([txt], { type: 'text/plain' })
        };
        navigator.clipboard.write([new ClipboardItem(data)]);
        //console.log("GOGGGGGGGG")
        return;
    }

    $("#inpit-for-copy-clipboard").val(txt);
    $("#inpit-for-copy-clipboard").select();
    document.execCommand("copy");

    return;

    navigator.clipboard.writeText(txt).then(function () {
        alert("Опис скопійовано в буфер обміну");
    }, function () {
        alert("Не можу записати буфер обміну");
    });
}

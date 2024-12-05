function GetRFCAppovalHistory(RFCNo, DialogID) {
    $.ajax({
        url: '../Transaction/GetRFCAppovalHistory?RFCNo=' + RFCNo,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            //$.growlUI(null, response);
            if (result.CodeValue == "0") {
                var record = result.DataValue;
                var userrecord = record.split("#-#");

                var RequestData = "<form class='form-horizontal' id='frm_apprequest1' name='frm_apprequest1' method='post' action='' novalidate='novalidate'><div class='widget-body'><div class='widget-main'>";
                RequestData += "<table width='90%' border='1' ><tr><td>Sr. No</td><td>Name</td><td>Role</td><td>Remarks</td><td>Status</td><td>Action Taken on</td></tr>";

                for (var i = 0; i < userrecord.length - 1; ++i) {
                    var RecordValue = userrecord[i].split("#");
                    RequestData += "<tr><td>" + RecordValue[0] + "</td><td>" + RecordValue[1] + "</td><td>" + RecordValue[2] + "</td><td>" + RecordValue[3] + "</td><td>" + RecordValue[4] + "</td><td>" + RecordValue[5] + "</td></tr>";
                }

                RequestData += "</table>";
                RequestData += "</div></div></form>";
                $("#" + DialogID).html(RequestData);
            }
            else {
                ShowError(result.DataValue);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert(xhr.status);
        }
    });

    var DialogHeight = $(window).height() * 0.85;
    $("#" + DialogID).removeClass('hide').dialog({
        resizable: false,
        width: '80%',
        height: DialogHeight,
        modal: true,
        closeOnEscape: false,
        position: { my: 'top', at: 'top+50' },
        title: "Request Approval History Detail",
        title_html: true,
        buttons: [
                    {
                        html: "<i class='ace-icon fa fa-times bigger-110 dark'></i><span style='color: #121212'>&nbsp; Close</span>",
                        "class": "btn btn-minier",
                        "style": "padding: 8px;",
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
        ]
    });
}
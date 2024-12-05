var availableTags = new Array();

function ShowSuccess(message) {    
    $.gritter.add({
        title: 'Success',
        text: message,
        class_name: 'gritter-success gritter-light'
    });
}

function ShowError(message) {    
    $.gritter.add({
        title: 'Error Message',
        text: message,
        class_name: 'gritter-error gritter-light'
    });
}

function clearValidation(formElement) {
    //Internal $.validator is exposed through $(form).validate()
    var validator = $(formElement).validate();
    //Iterate through named elements inside of the form, and mark them as error free
    $('[name]', formElement).each(function () {
        validator.successList.push(this); //mark as error free
        validator.showErrors(); //remove error messages if present
    });
    //validator.resetForm(); //remove error class on name elements and clear history
    //validator.reset(); //remove all error and success data
}

function InputFieldLimiter() {
    $('input.limited').inputlimiter({
        remText: '%n character%s remaining...',
        limitText: 'max allowed : %n.'
    });
}

function TextAreaFieldLimiter() {
    $('textarea.limited').inputlimiter({
        remText: '%n character%s remaining...',
        limitText: 'max allowed : %n.'
    });
}

$(document).ready(function () {
    $(window).bind('resize', function () {
        var width = $('#jqGridContainer').width();
        $('#jqTable').setGridWidth(width);
    });

    $("[data-rel=tooltip]").tooltip();

    $('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true
    })
    //show datepicker when clicking on the icon
	.next().on(ace.click_event, function () {
	    $(this).prev().focus();
	});

	//$(".date-picker").datepicker("setDate", new Date());

    $(window).bind('resize', function () {
        var width = $('#jqGridContainer').width();
        $('#jqTable').setGridWidth(width);
    });

    $('.modal.aside').ace_aside();

    $('#aside-inside-modal').addClass('aside').ace_aside({ container: '#my-modal > .modal-dialog' });

    //$('#top-menu').modal('show')

    $(document).one('ajaxloadstart.page', function (e) {
        //in ajax mode, remove before leaving page
        $('.modal.aside').remove();
        $(window).off('.aside')
    });

    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
        _title: function (title) {
            var $title = this.options.title || '&nbsp;'
            if (("title_html" in this.options) && this.options.title_html == true)
                title.html($title);
            else title.text($title);
        }
    }));
});

function updatePagerIcons(table) {
    var replacement = {
        'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
        'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
        'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
        'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'
    };
    $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

        if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
    })
}

function enableTooltips(table) {
    $('.ui-jqgrid-pager .ui-pg-button').attr("data-placement", "bottom").tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').attr("data-placement", "bottom").tooltip({ container: 'body' });
    $(table).find('.ui-pg-button').attr("data-placement", "button").tooltip({ container: 'body' });
}


function AutoCompleteEmpDetails() {
    $.ajax({
        url: '../Home/GetEmpDetails',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            //$.growlUI(null, response);
            if (result.MessageCode == "0") {
                var record = result.message;
                var userrecord = record.split("#-#");

                var DrpObj = document.getElementById("drp_prodname");
                for (var i = 0; i < DrpObj.length; i = i) {
                    DrpObj.remove(i);
                }
                var opt = document.createElement("option");
                DrpObj.options.add(opt);                    
                for (var i = 0; i < userrecord.length - 1; ++i) {
                    var TestOption = userrecord[i].split("#");
                    var opt = document.createElement("option");
                    // Add an Option object to Drop Down/List Box
                    DrpObj.options.add(opt);
                    // Assign text and value to Option object
                    opt.text = TestOption[1];
                    opt.value = TestOption[0];                        
                }
                $("#drp_prodname").trigger("chosen:updated"); 
            }
            else {
                ShowError(result.message);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert(xhr.status);
        }
    });
}

function GetDropDownListValues(RequestURL, Type, KeyWord, ControlName, FormName) {    
    $.ajax({
        url: RequestURL + '?Type=' + Type + '&KeyWord=' + KeyWord,
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.CodeValue == "0") {
                var record = result.DataValue;
                var userrecord = record.split("#-#");

                var DrpObj = document.getElementById(ControlName);
                for (var i = 0; i < DrpObj.length; i = i) {
                    DrpObj.remove(i);
                }
                var opt = document.createElement("option");
                DrpObj.options.add(opt);
                for (var i = 0; i < userrecord.length - 1; ++i) {
                    var TestOption = userrecord[i].split("#");
                    var opt = document.createElement("option");
                    // Add an Option object to Drop Down/List Box
                    DrpObj.options.add(opt);
                    // Assign text and value to Option object
                    opt.text = TestOption[1];
                    opt.value = TestOption[0];
                }
                $("#" + ControlName).trigger("chosen:updated");
            }
            else {
                //ShowError(result.DataValue);
            }

            //clearValidation($("#" + FormName));

        },
        error: function (xhr, textStatus, errorThrown) {
            alert(xhr.status);
        }
    });
}


function GetGridDropDownValues(RequestURL, Parameters) {
    var ReturnValue = "";
    $.ajax({
        url: RequestURL + Parameters,
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {

            if (result.CodeValue == "0") {
                var record = result.DataValue;
                var userrecord = record.split("#-#");

                for (var i = 0; i < userrecord.length - 1; ++i) {
                    var TestOption = userrecord[i].split("#");
                    if (i != 0)
                        ReturnValue += ";";
                    else
                        ReturnValue += ":---Select---;";
                    ReturnValue += TestOption[0] + ":" + TestOption[1];
                }

            }
            else {                
                //ShowError(result.DataValue);
                return "";
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert(xhr.status);
            return "";
        }
    });
    return ReturnValue;
}
function GetGridDropDownActualValue(RequestURL, Parameters) {
    var ReturnValue = "";
    $.ajax({
        url: RequestURL + Parameters,
        type: 'POST',
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {

            if (result.CodeValue == "0") {
                ReturnValue = result.DataValue;
                return ReturnValue;
            }
            else {
                //ShowError(result.DataValue);
                return "";
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            alert(xhr.status);
            return "";
        }
    });
    return ReturnValue;
}
function SetJqGridDropDown(jqTable, jqColName, URL, Parameters) {
    var cm = $("#" + jqTable).jqGrid('getColProp', jqColName);
    cm.edittype = 'select';
    cm.formatter = "select";
    cm.editable = true;

    var url = URL;
    var ReturnValue = "";
    //Retrieve Master data from Database.
    ReturnValue = GetGridDropDownValues(url, Parameters);

    if (jqColName == "Consultant") {
        ReturnValue = ReturnValue.replace(":---Select---;", "");        
    }

    //Assign values to editoptions of jqGrid functions
    cm.editoptions = { value: ReturnValue };   
}

function validateForm(formid, objrules, objmessages) {
    return $("#" + formid).validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: false,
        ignore: "",
        rules: objrules,
        messages:objmessages,
        highlight: function (e) {
            $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
        },

        success: function (e) {
            $(e).closest('.form-group').removeClass('has-error'); //.addClass('has-info');
            $(e).remove();
        },

        errorPlacement: function (error, element) {
            if (element.is('input[type=checkbox]') || element.is('input[type=radio]')) {
                var controls = element.closest('div[class*="col-"]');
                if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
                else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
            }
            else if (element.is('.select2')) {
                error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
            }
            else if (element.is('.chosen-select')) {
                error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
            }
            else error.insertAfter(element.parent());
        },

        submitHandler: function (form) {
        },
        invalidHandler: function (form) {
        }
    });
}
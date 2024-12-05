jQuery.GridList = {
    Grid:
    function (jqTable, jqTablePager, objcolNames, objcolModel, objurl, objcaption, objsortname, objsortorder, jsfunonSelectRow) {
        $(jqTable).jqGrid({
            // Ajax related configurations
            url: objurl,
            datatype: "json",
            mtype: "POST",
            // Specify the column names
            colNames: objcolNames,
            colModel: objcolModel,
            //For Column Not resizable resizable:false
            resizable: false,
            // Grid total width and height
            // width: 800,
            autowidth: true,
            height: 400,
            // Paging
            toppager: false,
            pager: $(jqTablePager),
            rowNum: 50,
            rowList: [50, 100, 150],
            emptyrecords: "No records to display",
            shrinkToFit: false, // For horizontal-scrollbar remove width in colModel and set shrinkToFit:false,forceFit:true,
            forceFit: true,
            viewrecords: false, // Specify if "total number of records" is displayed
            onSelectRow: function (row) {
                if (jsfunonSelectRow != null) {
                    if (typeof (window[jsfunonSelectRow]) === "function") {
                        window[jsfunonSelectRow](row);
                    }
                }
            },
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    jQuery.GridList.updateActionIcons(table);
                    jQuery.GridList.updatePagerIcons(table);
                    jQuery.GridList.enableTooltips(table);
                }, 0);
            },
            // Grid caption
            caption: objcaption,
            // Default sorting
            sortname: objsortname,
            sortorder: objsortorder,
            // For Set Footer
            footerrow: false,
            userDataOnFooter: false
        }).navGrid(jqTablePager,
            {
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey',
            },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            {
                sopt: ['cn']
                //sopt: ['eq', 'ne', 'cn', 'nc', 'bw', 'ew'] // Search options. Some options can be set on column level
            }

        )            //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });;
    },
    updatePagerIcons: function (table) {
        var replacement =
            {
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
    },
    updateActionIcons: function (table) {
        /**
        var replacement =
        {
        'ui-ace-icon fa fa-pencil' : 'ace-icon fa fa-pencil blue',
        'ui-ace-icon fa fa-trash-o' : 'ace-icon fa fa-trash-o red',
        'ui-icon-disk' : 'ace-icon fa fa-check green',
        'ui-icon-cancel' : 'ace-icon fa fa-times red'
        };
        $(table).find('.ui-pg-div span.ui-icon').each(function(){
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));
        if($class in replacement) icon.attr('class', 'ui-icon '+replacement[$class]);
        })
        */
    },
    SetCheckBox: function (cellvalue, options, rowObject) {
        var control = "";
        control = "<input type='checkbox' onclick=\"SetAssignto('" + rowObject[0] + "','" + rowObject[1] + "');\"></span>";
        return control;
    },

    enableTooltips: function (table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    },

    popuppartialview: function (objurl, objdata, objtitle, objdiologdiv, objbuttons) {

        $.ajax({
            type: "POST",
            url: objurl,
            data: objdata,
            aysnc: false,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#' + objdiologdiv).html(response);
                //$('#editmaterial').dialog('open');
                $('#' + objdiologdiv).removeClass('hide').dialog({
                    resizable: false,
                    width: '75%',
                    height: 600,
                    modal: true,
                    autoOpen: true,
                    closeOnEscape: true,
                    position: { my: 'top', at: 'top+50' },
                    title_html: true,
                    title: objtitle,
                    buttons: objbuttons
                });
                $('#' + objdiologdiv).load();
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
};
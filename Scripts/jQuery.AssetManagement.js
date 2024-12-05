jQuery.MobileReimbursement = {
    AssetRequest:
               function (objurl) {
                   $("#jqTable2").jqGrid({
                       // Ajax related configurations
                       url: objurl,
                       datatype: "json",
                       //data: objdata,
                       mtype: "POST",
                       // Specify the column names
                       colNames: ['Action','Request No', 'Request For', 'Item', 'HOD', 'Finance Controller'],
                       colModel: [
                           { name: 'Action', index: 'Action', width: 20 },
                           { name: 'Reqno', index: 'Reqno', width: 20 },
                           { name: 'Reqfor', index: 'Reqfor', width: 20 },
                           { name: 'Item', index: 'Item', width: 30 },
                           { name: 'HOD', index: 'HOD', width: 30 },
                           { name: 'FC', index: 'FC', width: 65 }
                       ],

                       // Grid total width and height
                       //width: 700,
                       autowidth: true,
                       //shrinkToFit:false,
                       //styleUI : 'Bootstrap', 
                       height: 150,
                       // Paging
                       toppager: false,
                       pager: $("#jqTablePager2"),
                       //rowNum: 25,
                       //rowList: [25, 50, 100],
                       page: 1,
                       scroll: 1,
                       emptyrecords: "No records to display",
                       viewrecords: true, // Specify if "total number of records" is displayed                      
                       multiselect: true,
                       // Default sorting
                       //onSelectRow: function (rowid, status, e) {                           
                         //  var r = jQuery('#jqTable2').jqGrid('getRowData', rowid);                          
                       //},
                       loadComplete: function () {
                           var table = this;
                           setTimeout(function () {
                               jQuery.AddressList.updateActionIcons(table);
                               jQuery.AddressList.updatePagerIcons(table);
                               jQuery.AddressList.enableTooltips(table);
                           }, 0);

                       },
                       // beforeRequest: function () {
                       /*Get Consultant Dropdown data*/

                       /*var URL1 = "../Transaction/GetDropDownlist";
                       var Parameters = '?Type=MASTER&KeyWord=DOCUMENT';
                       SetJqGridDropDown("jqTable2", "DocumentType", URL1 , Parameters);*/
                       // },
                       //sortname: "SrNo",
                       //sortorder: "desc",
                       // Grid caption 
                       caption: "Asset Request Details",
                       // For Set Footer
                       footerrow: false,
                       userDataOnFooter: true
                   }).navGrid("#jqTablePager2",
                     {
                         edit: false,
                         editicon: 'ace-icon fa fa-pencil blue',
                         add: false,
                         addicon: 'ace-icon fa fa-plus-circle purple',
                         del: false,
                         delicon: 'ace-icon fa fa-trash-o red',
                         search: false,
                         searchicon: 'ace-icon fa fa-search orange',
                         refresh: false,
                         refreshicon: 'ace-icon fa fa-refresh green',
                         view: false,
                         viewicon: 'ace-icon fa fa-search-plus grey'

                     },
                   {
                       //edit record form
                   },
                   {
                       //new record form
                   },
                   {
                       //delete record form
                   },
                   {
                       //search form
                       recreateForm: true,
                       afterShowSearch: function (e) {
                           var form = $(e[0]);
                           form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                           style_search_form(form);
                       },
                       afterRedraw: function () {
                           style_search_filters($(this));
                       }
                    ,
                       multipleSearch: true,
                       /**
                       multipleGroup:true,
                       showQuery: true
                       */
                   },
                {
                    //view record form
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                    }
                },
                   { sopt: ["cn"] } // Search options. Some options can be set on column level
                 ).navButtonAdd('#jqTablePager2', {
                     caption: "",
                     title: "Print",
                     id: "btn_report",
                     buttonicon: "ui-icon ace-icon fa fa-print green",
                     position: "last"
                 }).jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
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
    enableTooltips: function (table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    },
    createactioncontrol: function (cellvalue, options, rowObject) {
        // return "<div title=\"Edit\" style=\"float:left;cursor:pointer;\" class=\"ui-pg-div ui-inline-edit\" onclick=\"scriptcreategatepass.MatirialAddEdit('" + rowObject[1] + "','" + rowObject[2] + "'); \" id=\"jEditButton_1\"><span class=\"ace-icon fa fa-pencil blue bigger-125\"></span></div><div title=\"Delete\" style=\"float:left; cursor:pointer;\" class=\"ui-pg-div ui-inline-edit\" id=\"jEditButton_1\" onclick=\"scriptcreategatepass.MatirialAddEdit('" + rowObject[2] + "','" + rowObject[2] + "'); \" title=\"Delete\">&nbsp;&nbsp;<span class=\"ace-icon fa fa-trash-o bigger-120 red\"></span></div>";        
        return "<div title=\"Edit\" style=\"float:left;cursor:pointer;\" class=\"ui-pg-div ui-inline-edit\" onclick=\"MatirialAddEdit('" + rowObject[1] + "'); \" id=\"jEditButton_1\"><span class=\"ace-icon fa fa-pencil blue bigger-125\"></span></div>";
    },
    //enable datepicker
    pickDate: function (cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=text]')
                .datepicker({ format: 'dd/mm/yyyy', autoclose: true });
        }, 0);
    },
    style_edit_form: function (form) {
        //enable datepicker on "sdate" field and switches for "stock" field
        var arr = ["ExpectedStartDate", "ExpectedCompletionDate", "ActualStartDate", "ActualCompletionDate"];
        jQuery.each(arr, function (i, val) {
            form.find('input[name=' + val + ']').datepicker({ format: 'dd/mm/yyyy', autoclose: true })
            form.find('input[name=' + val + ']').parent().addClass("input-group controls");
            form.find('input[name=' + val + ']').before('<span class="input-group-addon"><i class="fa fa-calendar bigger-110"></i></span>');
        });


        //form.find('input[name=stock]').addClass('ace ace-switch ace-switch-5').after('<span class="lbl"></span>');
        //don't wrap inside a label element, the checkbox value won't be submitted (POST'ed)
        //.addClass('ace ace-switch ace-switch-5').wrap('<label class="inline" />').after('<span class="lbl"></span>');


        //update buttons classes
        var buttons = form.next().find('.EditButton .fm-button');
        buttons.addClass('btn btn-sm').find('[class*="-icon"]').hide();//ui-icon, s-icon
        buttons.eq(0).addClass('btn-primary').prepend('<i class="ace-icon fa fa-check"></i>');
        buttons.eq(1).prepend('<i class="ace-icon fa fa-times"></i>')

        buttons = form.next().find('.navButton a');
        buttons.find('.ui-icon').hide();
        buttons.eq(0).append('<i class="ace-icon fa fa-chevron-left"></i>');
        buttons.eq(1).append('<i class="ace-icon fa fa-chevron-right"></i>');
    },
    /*
      objurl, 
      objdata, 
      objtitle, 
      objdiologdiv , 
      objbuttons : Buttons if required 
    */
    popuppartialview: function (objurl, objdata, objtitle, objdiologdiv, objbuttons) {

        $.ajax({
            type: "POST",
            url: objurl,
            data: objdata,
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
                    //buttons: [{
                    //    html: "<i class='aace-icon fa fa-floppy-o icon-on-right'></i><span style='color: #121212' accesskey='c'>&nbsp; Save</span>",
                    //    "class": "btn btn-success btn-next",
                    //    "style": "padding: 8px;",
                    //    "form": "frm_GateEditMaterial",
                    //    click: function () {
                    //        var mySubDiv = document.querySelector("#editmaterial #btn_save");
                    //        mySubDiv.click();+
                    //    }

                    //},
                    //    {
                    //        html: "<i class='ace-icon fa fa-times bigger-110 dark'></i><span style='color: #121212' accesskey='c'>&nbsp; Close</span>",
                    //        "id": "btn_gvsearchgpno",
                    //        "name": "btn_gvsearchgpno",
                    //        "class": "btn",
                    //        "style": "padding: 8px;",
                    //        click: function () {
                    //            $(this).dialog("close");
                    //        }
                    //    }
                    //]
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

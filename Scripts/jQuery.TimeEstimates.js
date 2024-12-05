jQuery.TimeEstimates = {
    Grid:
               function (objurl, RFCNo) {                  

                   $("#jqTable").jqGrid({
                       // Ajax related configurations
                       url: objurl,
                       datatype: "json",
                       // postData: { "RFCNo": RFCNo },
                       mtype: "POST",
                       cache: false,
                       // Specify the column names
                       colNames: ['SrNo', 'Lifecycle Stage', 'Expected Start Date', 'Expected Completion Date', 'Estimate Time (In Hours)', 'Actual Start Date', 'Actual Completion Date', 'Actual Time (In Hours)', 'Consultant'],
                       colModel: [
                           { name: 'SrNo', index: 'SrNo', width: 30, editable: true, hidden: true },
                           { name: 'LifecycleStage', index: 'LifecycleStage', width: 30, editable: true },
                           { name: 'ExpectedStartDate', index: 'ExpectedStartDate', width: 30, editable: true, sorttype: "date", unformat: jQuery.TimeEstimates.pickDate },
                           { name: 'ExpectedCompletionDate', index: 'ExpectedCompletionDate', width: 30, editable: true, sorttype: 'date', unformat: jQuery.TimeEstimates.pickDate },
                           { name: 'EstimateTime', index: 'EstimateTime', width: 30, editable: true },
                           { name: 'ActualStartDate', index: 'ActualStartDate', width: 30, editable: true, sorttype: "date", unformat: jQuery.TimeEstimates.pickDate },
                           { name: 'ActualCompletionDate', index: 'ActualCompletionDate', width: 30, editable: true, sorttype: "date", unformat: jQuery.TimeEstimates.pickDate },
                           { name: 'ActualTime', index: 'ActualTime', width: 30, editable: true },
                           {
                               name: 'Consultant', index: 'Consultant', width: 30, editable: true, formatter: "select", edittype: "select", editoptions: { value: { '1': '---Select----' } }
                           }
                       ],

                       // Grid total width and height
                       //width: 700,
                       autowidth: true,
                       //shrinkToFit:false,
                       //styleUI : 'Bootstrap', 
                       height: 150,
                       // Paging
                       toppager: false,
                       pager: $("#jqTablePager"),
                       rowNum: 25,
                       rowList: [25, 50, 100],
                       emptyrecords: "No records to display",
                       viewrecords: true, // Specify if "total number of records" is displayed                      
                       // Default sorting
                       onSelectRow: function (rowid, status, e) {
                           // var rowData = jQuery('#jqTable').jqGrid('getRowData', rowid);                        

                       },
                       editurl: "../Transaction/SaveTimeEstimates?RFCNo=" + RFCNo,
                       loadComplete: function () {                           
                           var table = this;
                           setTimeout(function () {                              
                               jQuery.TimeEstimates.updateActionIcons(table);
                               jQuery.TimeEstimates.updatePagerIcons(table);
                               jQuery.TimeEstimates.enableTooltips(table);
                           }, 0);
                       },
                       gridComplete: function () {
                       },
                       beforeRequest: function()
                       {
                           /*Get Consultant Dropdown data*/
                           
                           var ServiceLineCode = $("#drp_ServiceLine").val();
                           var ServiceLineModule = $("#drp_ServiceModule").val();

                           var URL1 = "../Transaction/GetEngineerList";
                           var Parameters = '?ServiceLine=' + ServiceLineCode + '&ServiceLineModule=' + ServiceLineModule;

                           var ServiceLineCode = $("#drp_ServiceLine").val();
                           var ServiceLineModule = $("#drp_ServiceModule").val();

                           if (ServiceLineCode != "" & ServiceLineModule != null) {
                               SetJqGridDropDown("jqTable", "Consultant", URL1, Parameters);
                           }
                           
                           //SetJqGridDropDown("jqTable", "LifecycleStage", URL1, 'MASTER', 'CHANGE_TYPE');
                       },                       
                       sortname: "LifecycleStage",
                       sortorder: "desc",
                       // Grid caption 
                       caption: "",
                       // For Set Footer
                       footerrow: false,
                       userDataOnFooter: true
                   }).navGrid("#jqTablePager",
                     {
                         edit: true,
                         editicon: 'ace-icon fa fa-pencil blue',
                         add: true,
                         addicon: 'ace-icon fa fa-plus-circle purple',
                         del: false,
                         delicon: 'ace-icon fa fa-trash-o red',
                         search: false,
                         searchicon: 'ace-icon fa fa-search orange',
                         refresh: true,
                         refreshicon: 'ace-icon fa fa-refresh green',
                         view: false,
                         viewicon: 'ace-icon fa fa-search-plus grey'

                     },
                   {
                       //edit record form
                       closeAfterEdit: true,
                       viewPagerButtons: false,
                       //width: 700,
                       recreateForm: true,
                       /*
                       beforeShowForm: function ($form) {
                           $form.closest(".ui-jqdialog").position({
                               of: window,
                               my: "center center",
                               at: "center center"
                           });
                       }
                       */
                       beforeShowForm: function (e) {
                           var form = $(e[0]);
                           var position = $("#TimeEstimates").offset();
                           
                           form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />');
                           $('.ui-jqdialog').css(position);
                           jQuery.TimeEstimates.style_edit_form(form);
                       }                       
                   },
                   {
                       //new record form
                       width: 700,
                       closeAfterAdd: true,
                       recreateForm: true,
                       viewPagerButtons: false,                       
                       beforeShowForm: function (e) {
                           var form = $(e[0]);
                           var position = $("#TimeEstimates").offset();                           
                           form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar')
                           .wrapInner('<div class="widget-header" />');
                           $('.ui-jqdialog').css(position);
                           jQuery.TimeEstimates.style_edit_form(form);
                           
                       }
                   },
                   {
                       //delete record form
                       recreateForm: true,
                       beforeShowForm: function (e) {
                           var form = $(e[0]);
                           if (form.data('styled')) return false;

                           form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
                           style_delete_form(form);

                           form.data('styled', true);
                       },
                       onClick: function (e) {
                           //alert(1);
                       }
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
                 );
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
        return "<i class=\"ace-icon fa fa-pencil blue bigger-125\" onclick=\"jQuery.VeriEmpCCBranded.Edit('" + options.rowId + "');\" aria-hidden=\"true\" style=\"cursor:pointer\" title=\"Edit\"></i>&nbsp;&nbsp;<i class=\"ace-icon fa fa-floppy-o blue bigger-125\" onclick=\"jQuery.VeriEmpCCBranded.Save('" + options.rowId + "');\" aria-hidden=\"true\" style=\"cursor:pointer\" title=\"Save\"></i>&nbsp;&nbsp;<i class=\"ace-icon fa fa-times red bigger-125\" onclick=\"jQuery.VeriEmpCCBranded.Cancel('" + options.rowId + "');\" aria-hidden=\"true\" style=\"cursor:pointer\" title=\"Cancel\"></i>";
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
        }
//        ,
//    SetJqGridDropDown: function (jqTable, jqColName, URL, Parameters) {
//    var cm = $("#" + jqTable).jqGrid('getColProp', jqColName);
//    cm.edittype = 'select';
//    cm.formatter = "select";
//    cm.editable = true;

//    var url = URL;
//    var ReturnValue = "";
//    //Retrieve Master data from Database.
//    ReturnValue = GetGridDropDownValues(url, Parameters);

//    if (jqColName == "Consultant") {
//        ReturnValue = ReturnValue.replace(":---Select---;", "");        
//    }

//    //Assign values to editoptions of jqGrid functions
//            //cm.editoptions = { value: ReturnValue };   
    
//    ReturnValue = GetGridDropDownActualValue(url, Parameters);
//    var userrecord = ReturnValue.split("#-#");

//    var DrpObj = document.getElementById(jqColName);

//    var opt = document.createElement("option");    
//    alert(userrecord.length);
//    for (var i = 0; i < userrecord.length - 1; ++i) {
//        var TestOption = userrecord[i].split("#");
//        var opt = document.createElement("option");
//        // Add an Option object to Drop Down/List Box
//        DrpObj.options.add(opt);
//        // Assign text and value to Option object
//        opt.text = TestOption[1];
//        opt.value = TestOption[0];
//    }
//}

};

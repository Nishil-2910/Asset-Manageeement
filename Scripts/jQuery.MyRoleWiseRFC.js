jQuery.MyRoleWiseRFC = {
    Grid:
               function (objurl) {
                   $("#jqTable1").jqGrid({
                       // Ajax related configurations
                       url: objurl,
                       datatype: "json",                       
                       mtype: "POST",
                       // Specify the column names
                       colNames: ['IMS No', 'Requester', 'RFC Date', 'Service Line', 'Service Line Module', 'Short Change Desc', 'Priority', 'Support', 'Work Type', 'Change Type', 'Change Category', 'PendingWith', 'Role Name'],
                       colModel: [
                           { name: 'IMSNo', index: 'IMSNo', width: 100 },
                           { name: 'Requester', index: 'Requester', width: 200 },
                           { name: 'RFCDate', index: 'RFCDate', width: 90 },
                           { name: 'ServiceLine', index: 'ServiceLine', width: 120 },
                           { name: 'ServiceLineModule', index: 'ServiceLineModule', width: 120},
                           { name: 'ShortChangeDesc', index: 'ShortChangeDesc', width: 150},
                           { name: 'Priority', index: 'Priority', width: 80},
                           { name: 'Support', index: 'Support', width: 100},
                           { name: 'WorkType', index: 'WorkType', width: 100 },
                           { name: 'ChangeType', index: 'ChangeType', width: 150},
                           { name: 'ChangeCategory', index: 'ChangeCategory', width: 150 },
                           { name: 'PendingWith', index: 'PendingWith', width: 150 },
                           { name: 'RoleName', index: 'RoleName', width: 100 }
                       ],


                       // Grid total width and height
                       //width: 900,
                       autowidth: true,
                       shrinkToFit: true,                       
                       //styleUI : 'Bootstrap', 
                       height: 450,
                       // Paging
                       toppager: false,
                       pager: $("#jqTablePager1"),
                       rowNum: 25,
                       rowList: [25, 50, 100],
                       emptyrecords: "No records to display",
                       shrinkToFit: false,
                       viewrecords: true, // Specify if "total number of records" is displayed                      
                       // Default sorting
                       onSelectRow: function (rowid, status, e) {
                           // var rowData = jQuery('#jqTable').jqGrid('getRowData', rowid);                        

                       },                       
                       loadComplete: function () {
                           var table = this;
                           setTimeout(function () {                              
                               jQuery.MyRoleWiseRFC.updateActionIcons(table);
                               jQuery.MyRoleWiseRFC.updatePagerIcons(table);
                               jQuery.MyRoleWiseRFC.enableTooltips(table);
                           }, 0);

                       },
                       sortname: "RFCNo",
                       sortorder: "desc",
                       // Grid caption 
                       caption: "",
                       // For Set Footer
                       footerrow: false,
                       userDataOnFooter: true
                   }).navGrid("#jqTablePager1",
                     {
                         edit: false,
                         editicon: 'ace-icon fa fa-pencil blue',
                         add: false,
                         addicon: 'ace-icon fa fa-plus-circle purple',
                         del: false,
                         delicon: 'ace-icon fa fa-trash-o red',
                         search: true,
                         searchicon: 'ace-icon fa fa-search orange',
                         refresh: true,
                         refreshicon: 'ace-icon fa fa-refresh green',
                         view: false,
                         viewicon: 'ace-icon fa fa-search-plus grey'

                     },
                     {},
                     {},
                     {},
                    {
                        //search form
                        recreateForm: true,
                        afterShowSearch: function (e) {                            
                            var form = $(e[0]);
                            var position = $("#RoleWiseRFCRequests").offset();
                            form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar')
                            .wrapInner('<div class="widget-header" />');
                            $('#searchmodfbox_jqTable1').css(position);                            
                        },     
                        multipleSearch: false,
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
    }

};

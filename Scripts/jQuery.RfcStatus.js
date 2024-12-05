jQuery.RfcStatus = {
    
    Grid:
			function (grid_selector, pager_selector,objurl) {
			    $(grid_selector).jqGrid({
			        // Ajax related configurations
			        url: objurl,
			        datatype: "json",
			        mtype: "POST",
			        // Specify the column names
			        colNames: ['IMS No', 'RFC No', 'Consultant', 'Initiator', 'HOD', 'SLH', 'CCB', 'Consultant (Dev)', 'Admin'],
			        colModel: [
                        { name: 'IMSno', index: 'IMSno', width: 30, search: true, searchoptions: { sopt: ['cn'] } },
                        { name: 'RFCno', index: 'RFCno', width: 15, search: true, searchoptions: { sopt: ['eq', 'ne'] }, formatter: jQuery.RfcStatus.status_button_maker },
                        { name: 'Consultant', index: 'Consultant', width: 30, search: true, searchoptions: { sopt: ['cn'] } },
                        { name: 'Initiator', index: 'Initiator', width: 30, search: true, searchoptions: { sopt: ['cn'] } },
                        { name: 'HOD', index: 'HOD', width: 30, search: true, searchoptions: { sopt: ['cn'] } },
                        { name: 'SLH', index: 'SLH', width: 30,search: true, searchoptions: { sopt: ['cn'] } },
                        { name: 'CCB', index: 'CCB', width: 30 ,search: true, searchoptions: { sopt: ['cn'] } },
                        { name: 'Consultantdev', index: 'Consultantdev', width: 30 ,search: true, searchoptions: { sopt: ['cn'] } },
                        { name: 'Admin', index: 'Admin', width: 30 ,search: true, searchoptions: { sopt: ['cn'] } }
			        ],
			        // Grid total width and height
			        //width: 700,
			        autowidth: true,
			        //shrinkToFit:false,
			        //  styleUI : "Bootstrap", 
			        height: 400,
			        // Paging
			        toppager: false,
			        pager: $(pager_selector),
			        rowNum: 25,
			        rowList: [100, 200, 300],
			        emptyrecords: "No records to display",
			        viewrecords: true, // Specify if "total number of records" is displayed                      
			        // Default sorting

			        onSelectRow: function (rowid, status, e) {
                        

			        },
			        loadComplete: function () {
			            var top_rowid = $(grid_selector + ' tr:nth-child(2)').attr('id');
			            $("#jqTable3").setSelection(top_rowid, true);
			            var table = this;
			            setTimeout(function () {
			                jQuery.RfcStatus.updateActionIcons(table);
			                jQuery.RfcStatus.updatePagerIcons(table);
			                jQuery.RfcStatus.enableTooltips(table);
			            }, 0);
			           
			        },                   
			        sortname: "RFCno",
			        sortorder: "desc",
			        // Grid caption 
			        caption: "",
			        // For Set Footer
			        footerrow: false,
			        userDataOnFooter: true
			    }).navGrid(pager_selector,
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
                    
                    //search form
                    //sopt: ["cn"],
                    recreateForm: true,
                    afterShowSearch: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                        jQuery.RfcStatus.style_search_form(form);
                    },
                    afterRedraw: function () {
                        jQuery.RfcStatus.style_search_filters($(this));
                    }
                    ,
                    multipleSearch: true,
                    /**
                    multipleGroup:true,
                    showQuery: true
                    */
                    
                } // Search options. Some options can be set on column level
         ).navButtonAdd(pager_selector, {
             caption: "",
             title: "Export to Excel",
             id: "btn_report",
             buttonicon: "ace-icon fa fa-file-excel-o  bigger-140",
             position: "last",
             onClickButton: function (e) {
                 var record = $(grid_selector).jqGrid('getGridParam', 'records');
                 if (record > 0) {
                     ExporttoExcel();
                 }
             }
         });
			},
    cellattr: function (rowId, tv, rawObject, cm, rdata) {
        return 'style="white-space: normal;'
    },   
    //resize to fit page size
    FitToPageSize:
    function (grid_selector) {
        var parent_column = $(grid_selector).closest('[class*="col-"]');
        $(window).on('resize.jqGrid', function () {
            $(grid_selector).jqGrid('setGridWidth', parent_column.width());
        })

        //resize on sidebar collapse/expand
        $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
            if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
                //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
                setTimeout(function () {
                    $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                }, 20);
            }
        })
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
    style_search_filters: function (form) {
					form.find('.delete-rule').val('X');
                    form.find('.add-rule').addClass('btn btn-xs btn-primary');
                    form.find('.add-group').addClass('btn btn-xs btn-success');
                    form.find('.delete-group').addClass('btn btn-xs btn-danger');
    },
    style_search_form: function (form) {
					    var dialog = form.closest('.ui-jqdialog');
                        var buttons = dialog.find('.EditTable')
                        buttons.find('.EditButton a[id*="_reset"]').addClass('btn btn-sm btn-info').find('.ui-icon').attr('class', 'ace-icon fa fa-retweet');
                        buttons.find('.EditButton a[id*="_query"]').addClass('btn btn-sm btn-inverse').find('.ui-icon').attr('class', 'ace-icon fa fa-comment-o');
                        buttons.find('.EditButton a[id*="_search"]').addClass('btn btn-sm btn-purple').find('.ui-icon').attr('class', 'ace-icon fa fa-search');
    },
    enableTooltips: function (table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    },
    status_button_maker: function (cellvalue, options, rowObject) {
        return "<a style=\"cursor: pointer;\" title=\"View RFC\" onclick=\"RequiredDocumentList('" + rowObject[1] + "');\">"+ rowObject[1] +"</a>";
        //return "<i class=\"ace-icon fa fa-share-alt blue bigger-125\" onclick=\"RequiredDocumentList('" + rowObject[1] + "');\" aria-hidden=\"true\" style=\"cursor:pointer\" title=\"Call Asign\"></i>&nbsp;&nbsp;<i class=\"ace-icon fa fa-times orange bigger-125\" onclick=\"Completionaction('" + rowObject[0] + "');\" aria-hidden=\"true\" style=\"cursor:pointer\" title=\"Call Completion\"></i>"
    }
};
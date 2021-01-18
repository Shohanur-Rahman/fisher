
function CreateDynamicGuidFromJs() {
    // Inser befor PDF name
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}


function DownloadExcelSheetForKendoGridData(gridName, message) {
    try {

        var gview = $(gridName).data("kendoGrid");

        if (typeof gview === 'undefined') {
            ShowAjaxNotificationMessage("দুঃখিত", message, "warning");
            return false;
        }

        var gridElements = gview.dataSource;
        gridElements.fetch(function () {
            var total = gridElements.total();
            if (total > 0) {
                gview.saveAsExcel();
            } else {
                ShowAjaxNotificationMessage("দুঃখিত", message, "warning");
                return false;
            }
        });
    } catch (e) {
        ShowAjaxNotificationMessage("দুঃখিত", e, "warning");
    }

}


// function call from every grid where you need to generate excel document;
function GenerateGridInExcelDocumentEventHandler(e, gridName) {
    var rowCount = 0;
    var myWorkNameRow = 0;
    var myWorkStatusRow = 0;
    var sheet = e.workbook.sheets[0];
    var fullName = "";
    var status = "Closed";
    try {
        //gridRowIndex is the kendo grid row number, 0 index for header, if you want to change excell header style need to change gridRowIndex[0]
        for (var gridRowIndex = 0; gridRowIndex < sheet.rows.length; gridRowIndex++) {

            // This if condition for each row inner data
            if (gridRowIndex > 0) {
                //Source https:// docs.telerik.com/kendo-ui/controls/data-management/grid/how-to/excel/column-template-export
                if (gridName == "#divconcatenateFirstNameAndLastName") {
                    var gView = $(gridName).data("kendoGrid");
                    fullName = gView.dataSource._data[rowCount].SERVICE_USER_FIRST_NAME + " " + gView.dataSource._data[rowCount].SERVICE_USER_LAST_NAME;
                 
                    rowCount = rowCount + 1;
                }

                //cellIndex is the kendo grid column index in each row, 
                for (var ci = 0; ci < sheet.rows[gridRowIndex].cells.length; ci++) {
                    //sheet.rows[i].cells[ci].hAlign = "right"; This line for text align

                    //type detected header,data,footer
                    var rowType = sheet.rows[gridRowIndex].type;
                    // if row has data type column
                    if (rowType == "data") {
                        //Get each row column value
                        var rowValue = sheet.rows[gridRowIndex].cells[ci].value;
                        //if column value not null and not empty
                        if (rowValue != null && rowValue != "") {
                            //check row value is date
                            if (rowValue.length > 5) {
                                var matchDate = rowValue.substring(0, 6);
                                if (matchDate == "/Date(") {
                                    //if date convert server string(/Date(1487016420000+0600)/) to date format
                                    var generateDate = kendo.toString(kendo.parseDate(rowValue, 'yyyy-MM-dd'), 'dd/MM/yyyy');
                                    // Replace excell cell value dd/MM/yyyy format
                                    sheet.rows[gridRowIndex].cells[ci].value = generateDate;
                                  
                                }
                            }
                        }

                    }

                }// end row cell loop
            }// end if grid index
        }

    } catch (e) {
        ShowErrorMessageFromGridOrAjaxEvent(e);
    }
}




function DownloadPDFForKendoGridData(gridname,fileName, message) {
    try {

        $("body").css({ "cursor": "wait" });
        $(".btn").prop("disabled", true);
        var gview = $(gridname).data("kendoGrid");

        if (typeof gview === 'undefined' || gview == null) {
            ShowAjaxNotificationMessage("দুঃখিত", message, "warning");
            $("body").css({ "cursor": "default" });
            $(".btn").prop("disabled", false);
            return false;
        }

        var gridElements = gview.dataSource;
        gridElements.fetch(function () {
            var total = gridElements.total();
            if (total > 0) {
                $("body").css({ "cursor": "wait" });
                var progress = $.Deferred();

                gview._drawPDF(progress)
                    .then(function (root) {
                        return kendo.drawing.exportPDF(root, {
                            multiPage: true,
                            allPages: false
                        });
                    })
                    .done(function (dataURI) {
                        kendo.saveAs({
                            dataURI: dataURI,
                            fileName: fileName + "_" + CreateDynamicGuidFromJs() + ".pdf"
                        });
                        progress.resolve();
                        $("body").css({ "cursor": "default" });
                        $(".btn").prop("disabled", false);
                    });
            } else {
                ShowAjaxNotificationMessage("দুঃখিত", message, "warning");
                $("body").css({ "cursor": "default" });
                $(".btn").prop("disabled", false);
                return false;
            }
        });
    } catch (e) {
        
    }
}
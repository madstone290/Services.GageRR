var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var RangeMethod = function () {
    function GetInputId(appraiser, part) {
        return "input-".concat(appraiser, "-").concat(part);
    }
    function GetInputAvgId(appraiser) {
        return "input-avg-".concat(appraiser);
    }
    function GetRangeId(part) {
        return "range-".concat(part);
    }
    function GetRangeAvgId() {
        return "range-avg";
    }
    function findInput(appraiser, part) {
        var id = GetInputId(appraiser, part);
        return document.getElementById(id);
    }
    function findInputById(id) {
        return document.getElementById(id);
    }
    function setText(appraiser, part, value) {
        var input = findInput(appraiser, part);
        if (input && value)
            input.value = parseFloat(value).toString();
    }
    function handlePaste(e) {
        e.preventDefault();
        var startAppraiser = parseInt(e.target.attributes.appraiser.value);
        var startPart = parseInt(e.target.attributes.part.value);
        var data = e.clipboardData.getData('text');
        data = data.trim("\r\n");
        var values = data.split('\r\n');
        values = values.map(function (x) { return x.split('\t'); });
        var inputAppraiserCount = values.length;
        var inputPartCount = values.reduce(function (accumulator, currentValue) { return Math.max(accumulator, currentValue.length); }, 0);
        for (var appraiser = 0; appraiser < inputAppraiserCount; appraiser++) {
            for (var part = 0; part < inputPartCount; part++) {
                setText(startAppraiser + appraiser, startPart + part, values[appraiser][part]);
            }
        }
    }
    function calcuate() {
        return __awaiter(this, void 0, void 0, function () {
            var basicInfo, appraiserCount, partCount, input, appraiser, part, inputelement, response, json, appraiser, part;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        basicInfo = window.basicInfo;
                        appraiserCount = basicInfo.appraiserCount;
                        partCount = basicInfo.partCount;
                        input = {};
                        input.AppraiserCount = basicInfo.appraiserCount;
                        input.PartCount = basicInfo.partCount;
                        input.SpecLower = parseFloat(findInputById("sl").value);
                        input.SpecUpper = parseFloat(findInputById("su").value);
                        //input.Unit = "MM";
                        input.Unit = 0; // 0 for MM, 1 for INCH
                        input.Records = [];
                        for (appraiser = 1; appraiser <= appraiserCount; appraiser++) {
                            for (part = 1; part <= partCount; part++) {
                                inputelement = findInput(appraiser, part);
                                input.Records.push({
                                    Appraiser: appraiser,
                                    Part: part,
                                    Value: inputelement.value ? parseFloat(inputelement.value) : 0
                                });
                            }
                        }
                        return [4 /*yield*/, fetch("/api/GageRR/Range", {
                                method: "POST",
                                headers: {
                                    "Content-Type": "application/json",
                                },
                                body: JSON.stringify(input),
                            })];
                    case 1:
                        response = _a.sent();
                        return [4 /*yield*/, response.json()];
                    case 2:
                        json = _a.sent();
                        findInputById("output-r_").value = json.R_;
                        findInputById("output-t").value = (input.SpecUpper - input.SpecLower).toFixed(2);
                        findInputById("output-grr").value = json.GRR;
                        findInputById("output-grr_t").value = json.GRR_T;
                        for (appraiser = 1; appraiser <= appraiserCount; appraiser++) {
                            findInputById(GetInputAvgId(appraiser)).value = json.AppraiserAvg[appraiser];
                        }
                        for (part = 1; part <= partCount; part++) {
                            findInputById(GetRangeId(part)).value = json.PartRange[part];
                        }
                        findInputById(GetRangeAvgId()).value = json.R_;
                        return [2 /*return*/];
                }
            });
        });
    }
    function downloadExcelDynamic() {
        var Workbook = window.ExcelJS.Workbook;
        var workbook = new Workbook();
        workbook.addWorksheet("Sheet1");
        var ws = workbook.getWorksheet("Sheet1");
        ws.getCell('A1').value = 'John Doe';
        ws.getCell('B1').value = 'gardener';
        ws.getCell('C1').value = new Date().toLocaleString();
        var r3 = ws.getRow(3);
        r3.values = [1, 2, 3, 4, 5, 6];
        var fileName = 'simple.xlsx';
        var saveAs = window.saveAs;
        workbook.xlsx
            .writeBuffer()
            .then(function (buffer) { return saveAs(new Blob([buffer]), "".concat(fileName, "_").concat(Date.now(), ".xlsx")); })
            .then(function () {
            console.log('file created');
        })
            .catch(function (err) {
            console.log(err.message);
        });
    }
    function downloadExcel() {
        return __awaiter(this, void 0, void 0, function () {
            var response, blob, fileName, saveAs;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, fetch("/files/GageRange.xlsx")];
                    case 1:
                        response = _a.sent();
                        return [4 /*yield*/, response.blob()];
                    case 2:
                        blob = _a.sent();
                        fileName = 'GageRange.xlsx';
                        saveAs = window.saveAs;
                        saveAs(blob, fileName);
                        return [2 /*return*/];
                }
            });
        });
    }
    function uploadExcel(e) {
        if (!e.target.files)
            return;
        var ExcelJS = window.ExcelJS;
        var file = e.target.files[0];
        var wb = new ExcelJS.Workbook();
        var reader = new FileReader();
        var data = new Map();
        reader.readAsArrayBuffer(file);
        reader.onload = function () {
            var buffer = reader.result;
            wb.xlsx.load(buffer).then(function (workbook) {
                workbook.eachSheet(function (sheet, _) {
                    sheet.eachRow(function (row, rowNumber) {
                        row.eachCell(function (cell, colNumber) {
                            if (cell.formula)
                                console.log(colNumber, cell.formulaType, cell.value, cell.result);
                            else
                                console.log(colNumber, cell.value, cell.text);
                            var dataRowNumber = rowNumber - 1;
                            var dataColNumber = colNumber - 1;
                            if (dataRowNumber < 1 || dataColNumber < 1)
                                return;
                            var appraiser = dataRowNumber;
                            var part = dataColNumber;
                            // 데이터 입력
                            if (!data.has(appraiser))
                                data.set(appraiser, new Map());
                            data.get(appraiser).set(part, cell.value);
                        });
                    });
                    console.log("data", data);
                    // 데이터 붙여넣기
                    for (var appraiser = 1; appraiser <= 2; appraiser++) {
                        for (var part = 1; part <= 5; part++) {
                            setText(appraiser, part, data.get(appraiser).get(part));
                        }
                    }
                });
            });
        };
    }
    return {
        uploadExcel: uploadExcel,
        downloadExcel: downloadExcel,
        calcuate: calcuate,
        handlePaste: handlePaste,
    };
}();
window.Page = RangeMethod;
//# sourceMappingURL=RangeMethod.js.map
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
var AverageRangeMethod = function () {
    function downloadExcel() {
        return __awaiter(this, void 0, void 0, function () {
            var response, blob, fileName, saveAs;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, fetch("/files/GageAverageRange.xlsx")];
                    case 1:
                        response = _a.sent();
                        return [4 /*yield*/, response.blob()];
                    case 2:
                        blob = _a.sent();
                        fileName = 'GageAverageRange.xlsx';
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
                            var dataColNumber = colNumber - 2;
                            if (dataRowNumber < 1 || dataColNumber < 1)
                                return;
                            var appraiser = Math.floor((dataRowNumber - 1) / 3) + 1;
                            var trial = (dataRowNumber - 1) % 3 + 1;
                            var part = dataColNumber;
                            // 데이터 입력
                            if (!data.has(appraiser)) {
                                data.set(appraiser, new Map());
                            }
                            if (!data.get(appraiser).has(trial)) {
                                data.get(appraiser).set(trial, new Map());
                            }
                            data.get(appraiser).get(trial).set(part, cell.value);
                        });
                    });
                    console.log("data", data);
                    // 데이터 붙여넣기
                    for (var appraiser = 1; appraiser <= 3; appraiser++) {
                        for (var trial = 1; trial <= 3; trial++) {
                            for (var part = 1; part <= 10; part++) {
                                setText(appraiser, trial, part, data.get(appraiser).get(trial).get(part));
                            }
                        }
                    }
                });
            });
        };
    }
    function GetInputId(appraiser, trial, part) {
        return "input-".concat(appraiser, "-").concat(trial, "-").concat(part);
    }
    function GetAppraiserPartAvgId(appraiser, part) {
        return "app-part-avg-".concat(appraiser, "-").concat(part);
    }
    function GetAppraiserPartRangeId(appraiser, part) {
        return "app-part-range-".concat(appraiser, "-").concat(part);
    }
    function GetAppraiserTrialAvgId(appraiser, trial) {
        return "app-trial-avg-".concat(appraiser, "-").concat(trial);
    }
    function GetAppraiserPartAvgAvgId(appraiser) {
        return "app-part-avg-avg-".concat(appraiser);
    }
    function GetAppraiserPartRangeAvgId(appraiser) {
        return "app-part-range-avg-".concat(appraiser);
    }
    function GetPartAvgId(part) {
        return "part-avg-".concat(part);
    }
    function findInput(appraiser, trial, part) {
        var id = GetInputId(appraiser, trial, part);
        return document.getElementById(id);
    }
    function findInputById(id) {
        return document.getElementById(id);
    }
    function setText(appraiser, trial, part, value) {
        var input = findInput(appraiser, trial, part);
        if (input && value)
            input.value = parseFloat(value).toString();
    }
    function handlePaste(e) {
        e.preventDefault();
        var startAppraiser = e.target.attributes.appraiser.value;
        var startTrial = parseInt(e.target.attributes.trial.value);
        var startPart = parseInt(e.target.attributes.part.value);
        var data = e.clipboardData.getData('text');
        data = data.trim("\r\n");
        var values = data.split('\r\n');
        values = values.map(function (x) { return x.split('\t'); });
        var totalTrials = values.length;
        var totalParts = values.reduce(function (accumulator, currentValue) { return Math.max(accumulator, currentValue.length); }, 0);
        console.log(startAppraiser, startTrial, startPart, values, totalTrials, totalParts);
        for (var trial = 0; trial < totalTrials; trial++) {
            for (var part = 0; part < totalParts; part++) {
                setText(startAppraiser, startTrial + trial, startPart + part, values[trial][part]);
            }
        }
    }
    function calcuate() {
        return __awaiter(this, void 0, void 0, function () {
            var totalAppraiser, totalTrial, totalPart, input, appraiser, trial, part, inputelement, response, json, appraiser, part, avgEdit, rangeEdit, trial, trialEdit, avgAvgEdit, rangeAvgEdit, part, partAvgEdit, partAvgAvgEdit;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        totalAppraiser = window.RazorPage.AppraiserCount;
                        totalTrial = window.RazorPage.TrialCount;
                        totalPart = window.RazorPage.PartCount;
                        input = {};
                        input.AppraiserCount = totalAppraiser;
                        input.TrialCount = totalTrial;
                        input.PartCount = totalPart;
                        input.SpecLower = parseFloat(findInputById("sl").value);
                        input.SpecUpper = parseFloat(findInputById("su").value);
                        //input.Unit = "MM";
                        input.Unit = 0; // 0 for MM, 1 for INCH
                        input.Records = [];
                        for (appraiser = 1; appraiser <= totalAppraiser; appraiser++) {
                            for (trial = 1; trial <= totalTrial; trial++) {
                                for (part = 1; part <= totalPart; part++) {
                                    inputelement = findInput(appraiser, trial, part);
                                    input.Records.push({
                                        Appraiser: appraiser,
                                        Trial: trial,
                                        Part: part,
                                        Value: inputelement.value ? parseFloat(inputelement.value) : 0
                                    });
                                }
                            }
                        }
                        return [4 /*yield*/, fetch("/api/GageRR/AverageRange", {
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
                        findInputById("output-rp").value = json.Rp;
                        findInputById("output-r__").value = json.R__;
                        findInputById("output-x_diff").value = json.X_Diff;
                        findInputById("output-ev_sd").value = json.EV_SD;
                        findInputById("output-av_sd").value = json.AV_SD;
                        findInputById("grr_sd").value = json.GRR_SD;
                        findInputById("output-pv_sd").value = json.PV_SD;
                        findInputById("output-tv_sd").value = json.TV_SD;
                        findInputById("output-ndc").value = json.NDC;
                        findInputById("output-ev_sv").value = json.EV_SV;
                        findInputById("output-av_sv").value = json.AV_SV;
                        findInputById("output-grr_sv").value = json.GRR_SV;
                        findInputById("output-pv_sv").value = json.PV_SV;
                        findInputById("output-ev_t").value = json.EV_T;
                        findInputById("output-av_t").value = json.AV_T;
                        findInputById("output-grr_t").value = json.GRR_T;
                        findInputById("output-pv_t").value = json.PV_T;
                        for (appraiser = 1; appraiser <= totalAppraiser; appraiser++) {
                            for (part = 1; part <= totalPart; part++) {
                                avgEdit = findInputById(GetAppraiserPartAvgId(appraiser, part));
                                avgEdit.value = json.AppraiserPartAvg[appraiser][part];
                                rangeEdit = findInputById(GetAppraiserPartRangeId(appraiser, part));
                                rangeEdit.value = json.AppraiserPartRange[appraiser][part];
                            }
                            for (trial = 1; trial <= totalTrial; trial++) {
                                trialEdit = findInputById(GetAppraiserTrialAvgId(appraiser, trial));
                                trialEdit.value = json.AppraiserTrialAvg[appraiser][trial];
                            }
                            avgAvgEdit = findInputById(GetAppraiserPartAvgAvgId(appraiser));
                            avgAvgEdit.value = json.AppraiserAvg[appraiser];
                            rangeAvgEdit = findInputById(GetAppraiserPartRangeAvgId(appraiser));
                            rangeAvgEdit.value = json.AppraiserPartRangeAvg[appraiser];
                        }
                        for (part = 1; part <= totalPart; part++) {
                            partAvgEdit = findInputById(GetPartAvgId(part));
                            partAvgEdit.value = json.PartAvg[part];
                        }
                        partAvgAvgEdit = findInputById('resultAvg');
                        partAvgAvgEdit.value = json.PartAvgAvg;
                        return [2 /*return*/];
                }
            });
        });
    }
    return {
        downloadExcel: downloadExcel,
        uploadExcel: uploadExcel,
        handlePaste: handlePaste,
        calcuate: calcuate
    };
}();
window.Page = AverageRangeMethod;
//# sourceMappingURL=AverageRangeMethod.js.map
const AnovaMethod = function () {
    async function downloadExcel() {
        const response = await fetch("/files/AnovaInput.xlsx");
        const blob = await response.blob();
        const fileName = 'AnovaInput.xlsx';

        const saveAs = (window as any).saveAs;
        saveAs(blob, fileName);
    }


    function uploadExcel(e) {
        if (!e.target.files)
            return;

        const ExcelJS = (window as any).ExcelJS;

        const file = e.target.files[0];
        const wb = new ExcelJS.Workbook();
        const reader = new FileReader()

        const data = new Map<number, Map<number, Map<number, number>>>();

        reader.readAsArrayBuffer(file)
        reader.onload = () => {
            const buffer = reader.result as ArrayBuffer;
            wb.xlsx.load(buffer).then(workbook => {
                workbook.eachSheet((sheet, _) => {
                    sheet.eachRow((row, rowNumber) => {
                        row.eachCell((cell, colNumber) => {
                            if (cell.formula)
                                console.log(colNumber, cell.formulaType, cell.value, cell.result);
                            else
                                console.log(colNumber, cell.value, cell.text);

                            const dataRowNumber = rowNumber - 1;
                            const dataColNumber = colNumber - 2;
                            if (dataRowNumber < 1 || dataColNumber < 1)
                                return;

                            const appraiser = Math.floor((dataRowNumber - 1) / 3) + 1;
                            const trial = (dataRowNumber - 1) % 3 + 1;
                            const part = dataColNumber;
                            // 데이터 입력
                            if (!data.has(appraiser)) {
                                data.set(appraiser, new Map<number, Map<number, number>>());
                            }
                            if (!data.get(appraiser).has(trial)) {
                                data.get(appraiser).set(trial, new Map<number, number>());
                            }
                            data.get(appraiser).get(trial).set(part, cell.value);


                        });
                    })
                    console.log("data", data);
                    // 데이터 붙여넣기
                    for (let appraiser = 1; appraiser <= 3; appraiser++) {
                        for (let trial = 1; trial <= 3; trial++) {
                            for (let part = 1; part <= 10; part++) {
                                setText(appraiser, trial, part, data.get(appraiser).get(trial).get(part));
                            }
                        }

                    }
                })
            })
        };

    }

    function GetInputId(appraiser, trial, part) {
        return `input-${appraiser}-${trial}-${part}`;
    }

    function GetAppraiserPartAvgId(appraiser, part) {
        return `app-part-avg-${appraiser}-${part}`;
    }

    function GetAppraiserPartRangeId(appraiser, part) {
        return `app-part-range-${appraiser}-${part}`;
    }

    function GetAppraiserTrialAvgId(appraiser, trial) {
        return `app-trial-avg-${appraiser}-${trial}`;
    }

    function GetAppraiserPartAvgAvgId(appraiser) {
        return `app-part-avg-avg-${appraiser}`;
    }

    function GetAppraiserPartRangeAvgId(appraiser) {
        return `app-part-range-avg-${appraiser}`;
    }

    function GetPartAvgId(part) {
        return `part-avg-${part}`;
    }

    function findInput(appraiser, trial, part) {
        const id = GetInputId(appraiser, trial, part);
        return document.getElementById(id) as HTMLInputElement;
    }

    function findInputById(id: string){
        return document.getElementById(id) as HTMLInputElement;
    }

    function setText(appraiser, trial, part, value) {
        const input = findInput(appraiser, trial, part);
        if (input && value)
            input.value = parseFloat(value).toString();
    }

    function handlePaste(e) {
        e.preventDefault();

        const startAppraiser = e.target.attributes.appraiser.value;
        const startTrial = parseInt(e.target.attributes.trial.value);
        const startPart = parseInt(e.target.attributes.part.value);

        let data = e.clipboardData.getData('text');
        data = data.trim("\r\n")

        let values = data.split('\r\n');
        values = values.map(x => x.split('\t'));

        const totalTrials = values.length;
        const totalParts = values.reduce((accumulator, currentValue) => Math.max(accumulator, currentValue.length), 0);

        console.log(startAppraiser, startTrial, startPart, values, totalTrials, totalParts);
        for (let trial = 0; trial < totalTrials; trial++) {
            for (let part = 0; part < totalParts; part++) {
                setText(startAppraiser, startTrial + trial, startPart + part, values[trial][part]);
            }
        }
    }

    async function calcuate() {
        const totalAppraiser = (window as any).RazorPage.AppraiserCount;
        const totalTrial = (window as any).RazorPage.TrialCount;
        const totalPart = (window as any).RazorPage.PartCount;

        const input: any = {};

        input.AppraiserCount = totalAppraiser;
        input.TrialCount = totalTrial;
        input.PartCount = totalPart;
        input.Records = [];

        for (let appraiser = 1; appraiser <= totalAppraiser; appraiser++) {
            for (let trial = 1; trial <= totalTrial; trial++) {
                for (let part = 1; part <= totalPart; part++) {
                    const inputelement = findInput(appraiser, trial, part);
                    input.Records.push({
                        Appraiser: appraiser,
                        Trial: trial,
                        Part: part,
                        Value: inputelement.value ? parseFloat(inputelement.value) : 0
                    });
                }
            }
        }

        const response = await fetch("/api/GageRR/Anova", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(input),
        });

        const json = await response.json();

        findInputById("output-df_o").value = json.DF_Operator;
        findInputById("output-df_p").value = json.DF_Part;
        findInputById("output-df_op").value = json.DF_Operator_Part;
        findInputById("output-df_r").value = json.DF_Repeatability;
        findInputById("output-df_total").value = json.DF_Total;

        findInputById("output-ss_o").value = json.SS_Operator;
        findInputById("output-ss_p").value = json.SS_Part;
        findInputById("output-ss_op").value = json.SS_Operator_Part;
        findInputById("output-ss_r").value = json.SS_Repeatability;
        findInputById("output-ss_total").value = json.SS_Total;

        findInputById("output-ms_o").value = json.MS_Operator;
        findInputById("output-ms_p").value = json.MS_Part;
        findInputById("output-ms_op").value = json.MS_Operator_Part;
        findInputById("output-ms_r").value = json.MS_Repeatability;

        findInputById("output-f_o").value = json.F_Operator;
        findInputById("output-f_p").value = json.F_Part;
        findInputById("output-f_op").value = json.F_Operator_Part;

        findInputById("output-p_o").value = json.P_Operator;
        findInputById("output-p_p").value = json.P_Part;
        findInputById("output-p_op").value = json.P_Operator_Part;


    }

    return {
        downloadExcel,
        uploadExcel,
        handlePaste,
        calcuate
    }
}();

(window as any).Page = AnovaMethod;

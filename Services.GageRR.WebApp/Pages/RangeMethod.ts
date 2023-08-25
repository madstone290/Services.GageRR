const RangeMethod = function () {
    function GetInputId(appraiser, part) {
        return `input-${appraiser}-${part}`;
    }

    function GetInputAvgId(appraiser) {
        return `input-avg-${appraiser}`;
    }

    function GetRangeId(part) {
        return `range-${part}`;
    }

    function GetRangeAvgId() {
        return "range-avg";
    }


    function findInput(appraiser, part) {
        const id = GetInputId(appraiser, part);
        return document.getElementById(id) as HTMLInputElement;
    }

    function findInputById(id: string) {
        return document.getElementById(id) as HTMLInputElement;
    }
    function setText(appraiser, part, value) {
        const input = findInput(appraiser, part);
        if (input && value)
            input.value = parseFloat(value).toString();
    }

    function handlePaste(e) {
        e.preventDefault();

        const startAppraiser = parseInt(e.target.attributes.appraiser.value);
        const startPart = parseInt(e.target.attributes.part.value);

        let data = e.clipboardData.getData('text');
        data = data.trim("\r\n")

        let values = data.split('\r\n');
        values = values.map(x => x.split('\t'));

        const inputAppraiserCount = values.length;
        const inputPartCount = values.reduce((accumulator, currentValue) => Math.max(accumulator, currentValue.length), 0);

        for (let appraiser = 0; appraiser < inputAppraiserCount; appraiser++) {
            for (let part = 0; part < inputPartCount; part++) {
                setText(startAppraiser + appraiser, startPart + part, values[appraiser][part]);
            }
        }
    }

    async function calcuate() {
        // from <script> tag in RangeMethod.cshtml
        const basicInfo = (<any>window).basicInfo;

        const appraiserCount = basicInfo.appraiserCount;
        const partCount = basicInfo.partCount;

        const input: any = {};
        input.AppraiserCount = basicInfo.appraiserCount;
        input.PartCount = basicInfo.partCount;
        input.SpecLower = parseFloat(findInputById("sl").value);
        input.SpecUpper = parseFloat(findInputById("su").value);
        //input.Unit = "MM";
        input.Unit = 0; // 0 for MM, 1 for INCH
        input.Records = [];

        for (let appraiser = 1; appraiser <= appraiserCount; appraiser++) {
            for (let part = 1; part <= partCount; part++) {
                const inputelement = findInput(appraiser, part);
                input.Records.push({
                    Appraiser: appraiser,
                    Part: part,
                    Value: inputelement.value ? parseFloat(inputelement.value) : 0
                });
            }
        }

        const response = await fetch("/api/GageRR/Range", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(input),
        });

        const json = await response.json();


        findInputById("output-r_").value = json.R_;
        findInputById("output-t").value = (input.SpecUpper - input.SpecLower).toFixed(2);
        findInputById("output-grr").value = json.GRR;
        findInputById("output-grr_t").value = json.GRR_T;

        for (let appraiser = 1; appraiser <= appraiserCount; appraiser++) {
            findInputById(GetInputAvgId(appraiser)).value = json.AppraiserAvg[appraiser];
        }

        for (let part = 1; part <= partCount; part++) {
            findInputById(GetRangeId(part)).value = json.PartRange[part];
        }

        findInputById(GetRangeAvgId()).value = json.R_;

    }


    function downloadExcelDynamic() {
        const Workbook = (window as any).ExcelJS.Workbook;
        const workbook = new Workbook();
        workbook.addWorksheet("Sheet1");

        const ws = workbook.getWorksheet("Sheet1");

        ws.getCell('A1').value = 'John Doe';
        ws.getCell('B1').value = 'gardener';
        ws.getCell('C1').value = new Date().toLocaleString();


        const r3 = ws.getRow(3);
        r3.values = [1, 2, 3, 4, 5, 6];

        const fileName = 'simple.xlsx';

        const saveAs = (window as any).saveAs;
        workbook.xlsx
            .writeBuffer()
            .then(buffer => saveAs(new Blob([buffer]), `${fileName}_${Date.now()}.xlsx`))
            .then(() => {
                console.log('file created');
            })
            .catch(err => {
                console.log(err.message);
            });
    }

    async function downloadExcel() {
        const response = await fetch("/files/RangeInput.xlsx");
        const blob = await response.blob();
        const fileName = 'RangeInput.xlsx';

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

        const data = new Map<number, Map<number, number>>();

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
                            const dataColNumber = colNumber - 1;
                            if (dataRowNumber < 1 || dataColNumber < 1)
                                return;

                            const appraiser = dataRowNumber;
                            const part = dataColNumber;

                            // 데이터 입력
                            if (!data.has(appraiser))
                                data.set(appraiser, new Map<number, number>());
                            data.get(appraiser).set(part, cell.value);

                        });
                    })
                    console.log("data", data);
                    // 데이터 붙여넣기
                    for (let appraiser = 1; appraiser <= 2; appraiser++) {
                        for (let part = 1; part <= 5; part++) {
                            setText(appraiser, part, data.get(appraiser).get(part));
                        }
                    }
                })
            })
        };

    }
    return {
        uploadExcel,
        downloadExcel,
        calcuate,
        handlePaste,
    }
}();


(window as any).Page = RangeMethod;
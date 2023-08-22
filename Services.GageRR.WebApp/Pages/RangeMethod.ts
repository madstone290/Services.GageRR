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

    const input :any = {};
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

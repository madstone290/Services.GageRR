const AverageRangeMethod = function () {
    async function downloadExcel() {
        const response = await fetch("/files/GageAverageRange.xlsx");
        const blob = await response.blob();
        const fileName = 'GageAverageRange.xlsx';

        const saveAs = (window as any).saveAs;
        saveAs(blob, fileName);
    }
    return {
        downloadExcel
    }
}();

(window as any).Page = AverageRangeMethod;

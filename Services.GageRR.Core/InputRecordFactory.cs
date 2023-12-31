﻿using Services.GageRR.Core.Data;

namespace Services.GageRR.Core
{
    /// <summary>
    /// 여러 형태의 입력을 받아서 Input.Record 리스트로 변환한다.
    /// </summary>
    public class InputRecordFactory
    {
        /// <summary>
        /// 평가자, 시행, 파트순의 데이터.
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        public static List<AverageRangeInput.Record> BuildAverageRangeRecord(List<List<List<double>>> listData)
        {
            return listData.SelectMany((trials, appraiserIdx) =>
            {
                return trials.SelectMany((parts, trialIdx) =>
                {
                    return parts.Select((value, partIdx) =>
                    {
                        return new AverageRangeInput.Record()
                        {
                            Appraiser = appraiserIdx + 1,
                            Trial = trialIdx + 1,
                            Part = partIdx + 1,
                            Value = (decimal)value
                        };
                    });
                });
            })
            .ToList();
        }

        public static List<AnovaInput.Record> BuildAnovaRecord(List<List<List<double>>> listData)
        {
            return listData.SelectMany((trials, appraiserIdx) =>
            {
                return trials.SelectMany((parts, trialIdx) =>
                {
                    return parts.Select((value, partIdx) =>
                    {
                        return new AnovaInput.Record()
                        {
                            Appraiser = appraiserIdx + 1,
                            Trial = trialIdx + 1,
                            Part = partIdx + 1,
                            Value = (decimal)value
                        };
                    });
                });
            })
            .ToList();
        }

    }
}

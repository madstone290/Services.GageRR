namespace Services.GageRR.Core
{
    /// <summary>
    /// Gage R&R 입력값을 검증한다.
    /// </summary>
    public class InputValidator
    {
        public void Validate(Input input)
        {
            if (input == null)
                throw new GageException("입력값이 없습니다.");

            if(input.AppraiserCount < 2 || input.AppraiserCount > 3)
                throw new GageException("평가자수는 2명 또는 3명이어야 합니다.");
            if(input.TrialCount < 2 || input.TrialCount > 3)
                throw new GageException("시행횟수는 2회 또는 3회이어야 합니다.");
            if(input.PartCount < 2 || input.PartCount > 10)
                throw new GageException("파트수는 2개 이상 10개 이하여야 합니다.");
            if(input.SpecUpper < input.SpecLower)
                throw new GageException("규격상한은 규격하한보다 커야 합니다.");

            if (input.Records.Count != input.AppraiserCount * input.TrialCount * input.PartCount)
                throw new GageException("측정값의 개수가 잘못되었습니다.");
            if(input.Records.GroupBy(x => x.Appraiser).Any(x => x.Count() != input.TrialCount * input.PartCount))
                throw new GageException("평가자별 측정값의 개수가 잘못되었습니다.");
            if(input.Records.GroupBy(x => x.Trial).Any(x => x.Count() != input.AppraiserCount * input.PartCount))
                throw new GageException("시행별 측정값의 개수가 잘못되었습니다.");
            if (input.Records.GroupBy(x => x.Part).Any(x => x.Count() != input.AppraiserCount * input.TrialCount))
                throw new GageException("파트별 측정값의 개수가 잘못되었습니다.");

        }
    }
}


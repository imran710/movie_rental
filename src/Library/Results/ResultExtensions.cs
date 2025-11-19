namespace Library.Results;

public static class ResultExtensions
{
    public static Result<(T1, T2)> Merge<T1, T2>(this Result<T1> result1, Result<T2> result2)
    {
        var errors = new ValidationErrors();

        if (result1.IsError && result1.Error.Type == ErrorType.Validation)
        {
            if (result1.Error.ValidationErrors.Count > 0)
            {
                errors = result1.Error.ValidationErrors;
            }
        }

        if (result2.IsError && result1.Error.Type == ErrorType.Validation)
        {
            if (result2.Error.ValidationErrors.Count > 0)
            {
                errors = errors.Merge(result2.Error.ValidationErrors);
            }
        }

        if (errors.HasErrors)
        {
            return Error.Validation(errors);
        }

        if (result1.IsError) return result1.Error;
        if (result2.IsError) return result2.Error;

        return Result.Success((result1.Value, result2.Value));
    }

    public static Result<(T1, T2, T3)> Merge<T1, T2, T3>(
        this Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3)
    {
        var twoResultMerge = result1.Merge(result2);
        if (twoResultMerge.IsError)
        {
            return twoResultMerge.Error;
        }

        var errors = new ValidationErrors();
        if (result3.IsError && result3.Error.Type == ErrorType.Validation)
        {
            if (result3.Error.ValidationErrors.Count > 0)
            {
                errors = result3.Error.ValidationErrors;
            }
        }
        if (errors.HasErrors)
        {
            return Error.Validation(errors);
        }
        if (result3.IsError) return result3.Error;

        return Result.Success((twoResultMerge.Value.Item1, twoResultMerge.Value.Item2, result3.Value));
    }

    public static Result<(T1, T2, T3, T4)> Merge<T1, T2, T3, T4>(
        this Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4)
    {
        var threeResultMerge = result1.Merge(result2, result3);
        if (threeResultMerge.IsError)
        {
            return threeResultMerge.Error;
        }

        var errors = new ValidationErrors();
        if (result4.IsError && result4.Error.Type == ErrorType.Validation)
        {
            if (result4.Error.ValidationErrors.Count > 0)
            {
                errors = result4.Error.ValidationErrors;
            }
        }
        if (errors.HasErrors)
        {
            return Error.Validation(errors);
        }
        if (result4.IsError) return result4.Error;

        return Result.Success((
            threeResultMerge.Value.Item1,
            threeResultMerge.Value.Item2,
            threeResultMerge.Value.Item3,
            result4.Value
        ));
    }

    public static Result<(T1, T2, T3, T4, T5)> Merge<T1, T2, T3, T4, T5>(
        this Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5)
    {
        var fourResultMerge = result1.Merge(result2, result3, result4);
        if (fourResultMerge.IsError)
        {
            return fourResultMerge.Error;
        }

        var errors = new ValidationErrors();
        if (result5.IsError && result5.Error.Type == ErrorType.Validation)
        {
            if (result5.Error.ValidationErrors.Count > 0)
            {
                errors = result5.Error.ValidationErrors;
            }
        }
        if (errors.HasErrors)
        {
            return Error.Validation(errors);
        }
        if (result5.IsError) return result5.Error;

        return Result.Success((
            fourResultMerge.Value.Item1,
            fourResultMerge.Value.Item2,
            fourResultMerge.Value.Item3,
            fourResultMerge.Value.Item4,
            result5.Value
        ));
    }

    public static Result<(T1, T2, T3, T4, T5, T6)> Merge<T1, T2, T3, T4, T5, T6>(
        this Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6)
    {
        var fiveResultMerge = result1.Merge(result2, result3, result4, result5);
        if (fiveResultMerge.IsError)
        {
            return fiveResultMerge.Error;
        }

        var errors = new ValidationErrors();
        if (result6.IsError && result6.Error.Type == ErrorType.Validation)
        {
            if (result6.Error.ValidationErrors.Count > 0)
            {
                errors = result6.Error.ValidationErrors;
            }
        }
        if (errors.HasErrors)
        {
            return Error.Validation(errors);
        }
        if (result6.IsError) return result6.Error;

        return Result.Success((
            fiveResultMerge.Value.Item1,
            fiveResultMerge.Value.Item2,
            fiveResultMerge.Value.Item3,
            fiveResultMerge.Value.Item4,
            fiveResultMerge.Value.Item5,
            result6.Value
        ));
    }

    public static Result<(T1, T2, T3, T4, T5, T6, T7)> Merge<T1, T2, T3, T4, T5, T6, T7>(
        this Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Result<T6> result6,
        Result<T7> result7)
    {
        var sixResultMerge = result1.Merge(result2, result3, result4, result5, result6);
        if (sixResultMerge.IsError)
        {
            return sixResultMerge.Error;
        }

        var errors = new ValidationErrors();
        if (result7.IsError && result7.Error.Type == ErrorType.Validation)
        {
            if (result7.Error.ValidationErrors.Count > 0)
            {
                errors = result7.Error.ValidationErrors;
            }
        }
        if (errors.HasErrors)
        {
            return Error.Validation(errors);
        }
        if (result7.IsError) return result7.Error;

        return Result.Success((
            sixResultMerge.Value.Item1,
            sixResultMerge.Value.Item2,
            sixResultMerge.Value.Item3,
            sixResultMerge.Value.Item4,
            sixResultMerge.Value.Item5,
            sixResultMerge.Value.Item6,
            result7.Value
        ));
    }
}

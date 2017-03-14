namespace Formula.Search
{
    using System;

    /// <summary>
    /// 查询方式
    /// </summary>
    public enum Mode
    {
        Equal,
        NotEqual,
        In,
        NotIn,
        Like,
        NotLike,
        MoreThan,
        MoreThanEqual,
        LessThan,
        LessThanEqual,
        StartWith,
        EndWith,
        NotStartWith,
        NotEndWith,
    }

    public enum SpecialValue
    {
        IsNull,
        IsNotNull,
        IsNullOrEmpty,
        IsNotNullorEmpty,
    }
}
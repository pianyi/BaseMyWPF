﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Sample.Core.Attributes.Validation
{
    /// <summary>
    /// 指定ドライブが存在するかどうかをチェックする属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExistsDriveAttribute : ValidationAttribute
    {
    }
}

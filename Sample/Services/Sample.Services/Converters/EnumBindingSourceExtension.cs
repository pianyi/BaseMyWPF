using Sample.Core.Attributes;
using Sample.Core.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Markup;
using static Sample.Core.Attributes.Model.CustomAttributes;

namespace Sample.Services.Converters
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;
        public Type EnumType
        {
            get { return this._enumType; }
            set
            {
                if (value != this._enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    this._enumType = value;
                }
            }
        }

        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type enumType)
        {
            this.EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == this._enumType)
                throw new InvalidOperationException("The EnumType must be specified.");

            Type actualEnumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            //追加して良いかチェックする
            List<Enum> resultList = new();
            foreach (Enum value in enumValues)
            {
                if (value.GetAttribute<UseAttribute>() != null)
                {
                    if (value.CanUse())
                    {
                        resultList.Add(value);
                    }
                }
                else
                {
                    resultList.Add(value);
                }
            }
            Enum[] resultArray = resultList.ToArray();

            if (actualEnumType == this._enumType)
                return resultArray;

            Array tempArray = Array.CreateInstance(actualEnumType, resultArray.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}

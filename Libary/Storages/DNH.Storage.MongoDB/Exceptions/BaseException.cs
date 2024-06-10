using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace DNH.Storage.MongoDB.Exceptions
{
    [Serializable]
    public class BaseException : Exception
    {
        private string code = "Unknown";

        public virtual string Code
        {
            get => code;
            set => code = value;
        }

        public BaseException()
        {
        }

        public BaseException(string message)
            : base(message)
        {
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Code = info.GetString(nameof(Code));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Code), Code);
        }

        protected void SetValue<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            Data[propertyName] = value;
        }

        protected T GetValue<T>([CallerMemberName] string propertyName = null, T defaultValue = default(T))
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (Data.Contains(propertyName))
            {
                return (T)Data[propertyName];
            }

            return defaultValue;
        }
    }
}

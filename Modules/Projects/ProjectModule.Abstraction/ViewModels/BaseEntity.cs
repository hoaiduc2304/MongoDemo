using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectModule.Abstraction.ViewModels
{
    public abstract class BaseEntity
    {
    }
    public abstract class BaseEntityCollection<T> : List<T> where T : BaseEntity, new()
    {


    }
}

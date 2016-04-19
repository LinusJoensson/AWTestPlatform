using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;

namespace TestPlatform.ViewModels
{
    public interface IGridableVM
    {
        int Id { get; set; }
        string Name { get; set; }
        GridItemType ItemType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;

namespace TestPlatform.ViewModels
{
    public interface IGridableVM
    {
        GridItemType ItemType { get; set; }
        List<GridItemDetailVM> Items { get; set; }
        string[] SelectedItems { get; set; }
    }
}

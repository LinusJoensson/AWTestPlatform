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
        string[] SelectedItems { get; set; }
        List<GridItemDetailVM> GridItemDetails { get; set; }
    }
}

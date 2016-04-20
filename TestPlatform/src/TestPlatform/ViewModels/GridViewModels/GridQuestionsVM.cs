using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;

namespace TestPlatform.ViewModels.GridViewModels
{
    public class GridQuestionsVM : IGridableVM
    {
        public GridItemType ItemType { get; set; }
        public string[] SelectedItems { get; set; }
        public List<GridItemDetailVM> GridItemDetails { get; set; }
    }
}

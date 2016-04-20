using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;

namespace TestPlatform.ViewModels.GridViewModels
{
    public class GridQuestionsVM : IGridableVM
    {
        public int Id { get; set; }
        public GridItemType ItemType { get; set; }
        public List<GridItemDetailVM> Items { get; set; }
        public string[] SelectedItems { get; set; }
    }
}

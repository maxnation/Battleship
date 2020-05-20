using System.Collections.Generic;

namespace Battleship.Models
{
    public class FieldViewModel
    {
        public List<CellViewModel> Cells { get; set; }

        public FieldViewModel()
        {
            Cells = new List<CellViewModel>(100);
        }
    }
}
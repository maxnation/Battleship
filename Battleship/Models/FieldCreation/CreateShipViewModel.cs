using System.Collections.Generic;

namespace Battleship.Models.FieldCreation
{
    public class CreateShipViewModel
    {
        public int Size { get; set; }

        public List<CreateShipCellViewModel> Cells { get; set; }

        public CreateShipViewModel()
        {
            Cells = new List<CreateShipCellViewModel>(4);
        }
    }
}

﻿namespace Battleship.Models
{
    public class StepViewModel
    {
        public int PlayerId { get; set; }

        public int RivalId { get; set; }

        public int LineNo { get; set; }

        public int ColumnNo { get; set; }

        public bool IsHit { get; set; }
    }
}
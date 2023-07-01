using System.Collections.Generic;

namespace GModPrePubWPF.Classes
{
    public class CheckedListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public List<CheckedListItem> AvailablePresentationObjects;
    }
}

using System;
using System.Globalization;
using Inventory.Core;
using Inventory.Desktop.ViewModel;

namespace Inventory.Desktop.Model
{
    public class RecordViewModel : ViewModelBase
    {
        private int id;
        private string name;
        private DateTime createdAt;

        public int ID
        {
            get => id;
            set => SetProperty(ref id,value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public DateTime CreatedAt
        {
            get => createdAt;
            set => SetProperty(ref createdAt, value);
        }

        public string CreatedDateTime
        {
            set => CreatedAt = DateTime.Parse(value);
        }

        public string CreatedAtString => CreatedAt.ToString("yyyy/M/dd HH:mm", CultureInfo.InvariantCulture);

        public static implicit operator RecordModel(RecordViewModel model)
        {
            return new RecordModel()
            {
                ID = model.ID,
                CreatedAt = model.CreatedAt,
                Name = model.Name
            };
        }

        public static implicit operator RecordViewModel(RecordModel model)
        {
            return new RecordViewModel()
            {
                ID = model.ID,
                CreatedAt = model.CreatedAt,
                Name = model.Name
            };
        }
    }
}
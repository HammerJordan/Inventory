using System;
using System.Globalization;
using InventoryManagement.Core.Models;
using InventoryManagement.Desktop.ViewModel;

namespace InventoryManagement.Desktop.Model
{
    public class RecordBindableModel : ViewModelBase
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

        public static implicit operator RecordModel(RecordBindableModel model)
        {
            return new RecordModel()
            {
                ID = model.ID,
                CreatedAt = model.CreatedAt,
                Name = model.Name
            };
        }

        public static implicit operator RecordBindableModel(RecordModel model)
        {
            return new RecordBindableModel()
            {
                ID = model.ID,
                CreatedAt = model.CreatedAt,
                Name = model.Name
            };
        }
    }
}
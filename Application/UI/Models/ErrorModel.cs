using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Application.UI.Models
{
    public class ErrorModel : INotifyDataErrorInfo
    {

        private readonly Dictionary<string, List<string>> errors = new();

        public bool HasErrors => errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ClearErrors([CallerMemberName] string propertyName = null)
        {
            if (errors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName is null)
            {
                return errors.First().Value;
            }
            return errors.GetValueOrDefault(propertyName);
        }


        public void AddError(string message, [CallerMemberName] string propertyName = null)
        {
            if (!errors.ContainsKey(propertyName))
            {
                errors.Add(propertyName, new List<string>());
            }

            errors[propertyName].Add(message);
            OnErrorsChanged(propertyName);
        }
    }
}

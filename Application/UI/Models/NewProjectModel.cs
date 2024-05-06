using Application.UI.Models;
using NativeAudioGen.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeAudioGen.Models
{
    public class NewProjectModel : Base, INotifyDataErrorInfo
    {
        private ErrorModel errorModel;

        public bool HasErrors => errorModel.HasErrors;

        public bool CanCreate => !HasErrors;

        private string resourceName = string.Empty;
        private string resourcePath;

        public string ResourceName
        {
            get => resourceName;
            set
            {
                resourceName = value;

                errorModel.ClearErrors();

                var result = new ResourceValidator().Validate(resourceName, null);

                if (!result.IsValid)
                {
                    errorModel.AddError(result.ErrorContent.ToString());
                }

                OnPropertyChanged();
            }
        }

        public string ResourcePath
        {
            get => resourcePath;
            set
            {
                resourcePath = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return errorModel.GetErrors(propertyName);
        }
    }
}

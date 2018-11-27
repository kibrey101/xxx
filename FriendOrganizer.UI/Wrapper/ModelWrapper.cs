using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.UI.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase
    {
        public ModelWrapper(T model) => Model = model;
        public T Model { get; }
        protected virtual TValue GetValue<TValue>([CallerMemberName] string propertyName = null) => 
            (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);

        protected virtual void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged();
            ValidatePropertyInternal(propertyName, value);
        }

        private void ValidatePropertyInternal(string propertyName, object value)
        {
            ClearErrors(propertyName);
            ValidateDataAnnotations(propertyName, value);
            ValidateCustomErrors(propertyName);
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);

            if (errors == null) return;

            foreach (var error in errors)        
                AddError(propertyName, error);
            
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName) => null;

        private void ValidateDataAnnotations(string propertyName, object value)
        {
            var context = new ValidationContext(Model) { MemberName = propertyName };
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, context, validationResults);

            foreach (var result in validationResults)
                AddError(propertyName, result.ErrorMessage);
        }
    }
    
}

using FriendOrganizer.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendWrapper : ModelWrapper<Friend>
    {
        public FriendWrapper(Friend model) : base(model)
        {
        }

        public int Id {
            get => GetValue<int>();
            private set => SetValue(value);
        }
        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int? ProgrammingLanguageId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
        protected override IEnumerable<string> ValidateProperty([CallerMemberName] string propertyName = null)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "rob", StringComparison.OrdinalIgnoreCase))
                        yield return "Robots are not valid friends";
                    break;
            }
        }
    }
}

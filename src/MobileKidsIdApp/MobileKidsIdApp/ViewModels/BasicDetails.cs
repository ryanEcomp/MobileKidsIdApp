﻿using Csla.Xaml;
using MobileKidsIdApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MobileKidsIdApp.Models;

namespace MobileKidsIdApp.ViewModels
{
    public class BasicDetails : ViewModelBase<Models.ChildDetails>
    {
        private ContactInfo _contact;

        public ICommand ChangeContactCommand { get; private set; }

        public BasicDetails(Models.ChildDetails details)
        {
            ChangeContactCommand = new Command(async () =>
            {
                ContactInfo contact = await DependencyService.Get<IContactPicker>().GetSelectedContactInfo();
                if (contact == null)
                {
                    //Do nothing, user must have cancelled.
                }
                else
                {
                    _contact = contact;
                    Model.ContactId = contact.Id;

                    //Only overwrite name fields if they were blank.
                    if (string.IsNullOrEmpty(Model.FamilyName))
                        Model.FamilyName = contact.FamilyName;
                    if (string.IsNullOrEmpty(Model.AdditionalName))
                        Model.AdditionalName = contact.AdditionalName;
                    if (string.IsNullOrEmpty(Model.GivenName))
                        Model.GivenName = contact.GivenName;
                    
                    OnPropertyChanged(nameof(Contact));
                }
            });

            Model = details;
        }
        
        public async Task SaveDataAsync()
        {
            await App.CurrentFamily.SaveFamilyAsync();
        }

        protected override async Task<ChildDetails> DoInitAsync()
        {
            _contact = await DependencyService.Get<IContactPicker>().GetContactInfoForId(Model.ContactId);
            return Model;
        }

        public ContactInfo Contact { get { return _contact; } }
    }
}

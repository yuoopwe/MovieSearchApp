using FunctionZero.MvvmZero;
using MovieSearchApp.Models.UserAccount;
using MovieSearchApp.Services.Alert_Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class JournalPageVm : MvvmZeroBaseVm
    {
        private IAlertService _alertService;
        private IPageServiceZero _pageService;
        private MyFlyoutPageFlyoutVm _flyoutVm;
        private List<JournalDetailsModel> _journalDetailsList;

        public List<JournalDetailsModel> JournalDetailsList
        {
            get => _journalDetailsList;
            set => SetProperty(ref _journalDetailsList, value);
        }


        public JournalPageVm(IPageServiceZero pageService, IAlertService alertService, MyFlyoutPageFlyoutVm flyoutVm)
        {

            _alertService = alertService;
            _pageService = pageService;
            _flyoutVm = flyoutVm;

        }

        public void Init(List<JournalDetailsModel> JournalList)
        {
            JournalDetailsList = JournalList;


        }


    }
}

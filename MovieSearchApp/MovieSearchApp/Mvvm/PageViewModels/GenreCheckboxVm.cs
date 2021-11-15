using FunctionZero.CommandZero;
using FunctionZero.MvvmZero;
using MovieSearchApp.Models.PopularPage;
using MovieSearchApp.Mvvm.Pages.PopularPage;
using MovieSearchApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieSearchApp.Mvvm.PageViewModels
{
    class GenreCheckboxVm : MvvmZeroBaseVm
    {
        private IPageServiceZero _pageService;
        public ICommand DoneButtonCommand { get; }
        private IList<CheckboxModel> _CheckboxList;

        public IList<CheckboxModel> CheckboxList
        {
            get => _CheckboxList;
            set => SetProperty(ref _CheckboxList, value);
        }
        public GenreCheckboxVm(IPageServiceZero pageService)
        {
            _pageService = pageService;
            DoneButtonCommand = new CommandBuilder().SetExecuteAsync(DoneButtonExecute).Build();



        }

        public async Task DoneButtonExecute()
        {
            await _pageService.PushPageAsync<PopularPage, PopularPageVm>((vm) => vm.Apply(CheckboxList));
        }

        public void init(IList<CheckboxModel> checkboxList)
        {

            CheckboxList = checkboxList;

        }
    }
}

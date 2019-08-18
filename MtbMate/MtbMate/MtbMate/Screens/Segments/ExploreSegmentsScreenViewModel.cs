﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;

namespace MtbMate.Screens.Settings
{
    public class ExploreSegmentsScreenViewModel : ViewModelBase
    {
        public ExploreSegmentsScreenViewModel(MainContext context) : base(context)
        {
        }

        public ObservableCollection<SegmentModel> Segments => Context.Model.Segments;

        public override string Title => "Segments";

        public async Task AddSegment(INavigation nav)
        {
            await Context.UI.GoToCreateSegment(nav);
        }

        public async Task GoToSegment(INavigation nav, SegmentModel segment)
        {
            await Context.UI.GoToSegment(nav, segment);
        }
    }
}
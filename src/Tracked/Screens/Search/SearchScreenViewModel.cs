using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Screens.Search {
    public class SearchScreenViewModel : TabbedViewModelBase {
        private string searchText;
        private ObservableCollection<UserSearchDto> results;

        public SearchScreenViewModel(MainContext context) : base(context) {
            results = new ObservableCollection<UserSearchDto>();
        }

        public override string Title => "Search";

        protected override TabItemType SelectedTab => TabItemType.Search;

        public string SearchText {
            get { return searchText; }
            set {
                if (searchText != value) {
                    searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    OnPropertyChanged(nameof(ShouldSearch));
                }
            }
        }

        public ObservableCollection<UserSearchDto> Results {
            get { return results; }
            set {
                if (results != value) {
                    results = value;
                    OnPropertyChanged(nameof(Results));
                    OnPropertyChanged(nameof(HasResults));
                    OnPropertyChanged(nameof(EmptyText));
                }
            }
        }

        public bool HasResults => Results.Any();

        public bool ShouldSearch => !string.IsNullOrWhiteSpace(SearchText);

        public string EmptyText => ShouldSearch ? "No results found" : "Type in the name of the user you want to find";

        public ICommand RefreshCommand {
            get { return new Command(async () => await SearchAsync()); }
        }

        public async Task SearchAsync() {
            IsRefreshing = true;

            if (!ShouldSearch) {
                Results = new ObservableCollection<UserSearchDto>();
                IsRefreshing = false;
                return;
            }

            var results = await Context.Services.SearchUsers(SearchText.Trim());

            Results = results.ToObservable();

            IsRefreshing = false;
        }

        public async Task GoToUser(int userId) {
            await Context.UI.GoToProfileScreenAsync(userId);
        }
    }
}

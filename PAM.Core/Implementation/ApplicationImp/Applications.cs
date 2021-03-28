using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using LanguageExt;
using PAM.Core.Abstract;
using RZ.Foundation.Extensions;

namespace PAM.Core.Implementation.ApplicationImp
{
    public sealed class Applications : ObservableCollection<Application>
    {
        readonly ICollectionView filteredItems;

        public Applications()
        {
            filteredItems = CollectionViewSource.GetDefaultView(this);
        }

        public bool Contains(string application) => this[application].IsSome;
        public bool Contains(string application, string path) => this.FirstOrDefault(app => app.Name == application && app.Path == path) != null;

        public Option<IApplication> this[string applicationName] => this.TryFirst(app => app.Name == applicationName).Map(x => (IApplication)x);

        public TimeSpan TotalTime =>
            this.Select(app => app.TotalUsageTime)
                .Aggregate(TimeSpan.Zero, (subtotal, t) => subtotal.Add(t));

        public void UIRefresh()
        {
            TimeConverter.AppsTotalTime = TotalTime;

            foreach (var application in this)
            {
                application.Refresh();
            }
        }


        #region Different views

        public ICollectionView FilteredItems()
        {
            return filteredItems;
        }

        public void ShowAll()
        {
            filteredItems.SortDescriptions.Clear();
            filteredItems.Filter -= Filter;
        }

        public bool Filter(object app)
        {
            //var application = app as MyApplication;
            //return application.Duration.TotalMinutes > 50;
            return true;
        }

        public void SortAsc()
        {
            filteredItems.SortDescriptions.Clear();
            filteredItems.SortDescriptions.Add(new SortDescription("Duration", ListSortDirection.Ascending));
        }

        public void SortDesc()
        {
            filteredItems.SortDescriptions.Clear();
            filteredItems.SortDescriptions.Add(new SortDescription("Duration", ListSortDirection.Descending));
        }


        #endregion

    }
}
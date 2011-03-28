using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using PAM.Core.Abstract;

namespace PAM.Core.Implementation.ApplicationImp
{
    public class Applications : ObservableCollection<Application>
    {

        private ICollectionView _filteredItems;

        public Applications()
        {
            _filteredItems = CollectionViewSource.GetDefaultView(this);
        }

        public bool Contains(string application)
        {
            return (from app in this
                    where app.Name == application
                    select app).FirstOrDefault() != null;
        }

        public bool Contains(string application, string path)
        {
            return (from app in this
                    where app.Name == application && app.Path == path
                    select app).FirstOrDefault() != null;
        }

        public IApplication this[string applicationName]
        {
            get
            {
                return (from app in this
                        where app.Name == applicationName
                        select app).FirstOrDefault();


            }

        }

        public TimeSpan TotalTime
        {
            get
            {
                return (from app in this
                        select app.TotalUsageTime).Aggregate(TimeSpan.Zero, (subtotal,
                                                                             t) => subtotal.Add(t));
            }
        }

        public void Refresh()
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
            return _filteredItems;
        }

        public void ShowAll()
        {
            _filteredItems.SortDescriptions.Clear();
            _filteredItems.Filter -= Filter;
        }


        public bool Filter(object app)
        {
            //var application = app as MyApplication;
            //return application.Duration.TotalMinutes > 50;
            return true;
        }

        public void SortAsc()
        {
            _filteredItems.SortDescriptions.Clear();
            _filteredItems.SortDescriptions.Add(new SortDescription("Duration", ListSortDirection.Ascending));
        }

        public void SortDesc()
        {
            _filteredItems.SortDescriptions.Clear();
            _filteredItems.SortDescriptions.Add(new SortDescription("Duration", ListSortDirection.Descending));
        }


        #endregion

    }
}